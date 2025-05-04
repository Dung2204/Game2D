using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using UnityEngine;
class Award : UIFramwork
{
    List<BasePoolDT<long>> _AwardCenter;
    private UIWrapComponent _AwardParent;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Data_Pool.m_RebelArmyPool.f_CrusadeRebelList(Initialize);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void Initialize(object boj)
    {
        AwardDetails = f_GetObject("AwardDetails").transform;
        AwardParent = f_GetObject("AwardParent").transform;
        AwardParentWrap.f_ResetView();
        _AwardParent.f_OnClickIndex(0);
        f_Message();
    }

    #region
    Transform AwardDetails;
    Transform AwardParent;
    public UIWrapComponent AwardParentWrap
    {
        get
        {
            if (_AwardParent == null)
            {
                _AwardCenter = Data_Pool.m_AwardPool.f_GetAll();
                _AwardParent = new UIWrapComponent(155, 1, 800, _AwardCenter.Count, AwardParent.gameObject, AwardDetails.gameObject, _AwardCenter, UpdateAwardItem, f_OnRankItemClick);
            }
            return _AwardParent;
        }
    }
    #endregion

    void UpdateAwardItem(Transform item, BasePoolDT<long> dt)
    {
        AwardPoolDT tAward = dt as AwardPoolDT;
        UILabel tName = item.Find("Name").GetComponent<UILabel>();
        tName.text = tAward.m_AwardCenterDT.szTitle;
        //f_RegClickEvent(item.gameObject, f_OnRankItemClick, dt);
        //f_OnRankItemClick(item.gameObject, dt, null);
    }

    string _GetImporent(int imporent)
    {
        string tImportant = string.Empty;
        switch ((EM_Important)imporent)
        {
            case EM_Important.Green:
tImportant = "Normal";
                break;
            case EM_Important.Blue:
tImportant = "Brave";
                break;
            case EM_Important.Purple:
tImportant = "Unparalleled";
                break;
            default:
                break;
        }
        return tImportant;
    }
    private GameObject mlastSelectedItem;
    private void f_OnRankItemClick(Transform item, BasePoolDT<long> obj1)
    {
        AwardPoolDT tAward = obj1 as AwardPoolDT;
        GameObject objSelectedFlag = item.transform.Find("Sprite_Select").gameObject;
        //if (mlastSelectedItem == objSelectedFlag)
        //{
        //    return;
        //}

        UILabel tName = objSelectedFlag.transform.Find("Name").GetComponent<UILabel>();
        tName.text = tAward.m_AwardCenterDT.szTitle;

        if (null != mlastSelectedItem) mlastSelectedItem.SetActive(false);
        mlastSelectedItem = objSelectedFlag;
        mlastSelectedItem.SetActive(true);
        f_UpdateLeftDisplayRankInfo(tAward);
    }

    private void f_UpdateLeftDisplayRankInfo(AwardPoolDT dt)
    {
        AwardPoolDT tAward = dt as AwardPoolDT;
        Transform item = f_GetObject("Info").transform;
        Transform tAwardGoodsParent = item.Find("AwardGoods");
        UILabel tDesc = item.Find("Desc").GetComponent<UILabel>();
        switch ((EM_AwardCenter)tAward.m_CenterAwardType)
        {
            case EM_AwardCenter.RebelArmyRankExploit:
            case EM_AwardCenter.RebelarmyRankDPS:
            case EM_AwardCenter.ArenaAward:
            case EM_AwardCenter.PayBeta:
            case EM_AwardCenter.eAward_FirstRechargePresent:
                string Desc = tAward.m_AwardCenterDT.szDesc.Replace(GameParamConst.ReplaceFlag, tAward.m_Param[0].ToString());
                Desc = Desc.Replace(GameParamConst.ReplaceFlag2, tAward.m_Param[1].ToString());
                tDesc.text = Desc;
                //= tAward.m_AwardCenterDT.szDesc.Replace(GameParamConst.ReplaceFlag, tAward.m_Param[0].ToString(), tAward.m_Param[1].ToString());

                break;
            case EM_AwardCenter.KillRebelArmyAward:
                RebelArmyDeployDT trebelarmyDT = glo_Main.GetInstance().m_SC_Pool.m_RebelArmyDeploySC.f_GetSC(tAward.m_Param[0]) as RebelArmyDeployDT;
                int[] CardId = ccMath.f_String2ArrayInt(trebelarmyDT.szMonsterId, ";");
                CardDT tcard = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(CardId[4]) as CardDT;
                string tImportant = string.Empty;
                tImportant = _GetImporent(tAward.m_Param[1]);
                tImportant += tcard.szName;
                tImportant = UITool.f_GetImporentForName(tAward.m_Param[1], tImportant);
                tDesc.text = tAward.m_AwardCenterDT.szDesc.Replace(GameParamConst.ReplaceFlag, tImportant);
                break;
            case EM_AwardCenter.FindRebelArmyAward:
                //int[] CardId2 = ccMath.f_String2ArrayInt(Data_Pool.m_RebelArmyPool.M_UserFindRebelarmy.m_RebelArmyDeploy.szMonsterId, ";");
                //CardDT tcard2 = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(CardId2[4]) as CardDT;
                //string name = string.Empty;
                //name = _GetImporent(Data_Pool.m_RebelArmyPool.M_UserFindRebelarmy.m_Color);
                //name += tcard2.szName;
                //name = UITool.f_GetImporentForName(Data_Pool.m_RebelArmyPool.M_UserFindRebelarmy.m_Color, name);
                //tDesc.text = tAward.m_AwardCenterDT.szDesc.Replace(GameParamConst.ReplaceFlag, name);

                if (CommonTools.f_TwoInt2Long((uint)tAward.m_Param[0], (uint)tAward.m_Param[1]) != Data_Pool.m_UserData.m_iUserId)
                    tDesc.text = tAward.m_AwardCenterDT.szDesc.Replace(GameParamConst.ReplaceFlag,
                        (Data_Pool.m_GeneralPlayerPool.f_GetForId(CommonTools.f_TwoInt2Long((uint)tAward.m_Param[0], (uint)tAward.m_Param[1])) as BasePlayerPoolDT).m_szName);
                else
                    tDesc.text = tAward.m_AwardCenterDT.szDesc.Replace(GameParamConst.ReplaceFlag, Data_Pool.m_UserData.m_szRoleName);
                break;

            case EM_AwardCenter.eAward_ValentineRose:
                string tRoleName = string.Empty;
                if (tAward.m_Param[0] == 0)
tRoleName = "Football";
                else if (tAward.m_Param[0] == 1)
tRoleName = "Hoang Nguyet Anh";
                else if (tAward.m_Param[0] == 2)
tRoleName = "Tieu Kieu";
                else if (tAward.m_Param[0] == 3)
tRoleName = "Dream Boat";
                tDesc.text = tAward.m_AwardCenterDT.szDesc.Replace(GameParamConst.ReplaceFlag, tRoleName);
                tDesc.text = tDesc.text.Replace(GameParamConst.ReplaceFlag2, tAward.m_Param[1].ToString());
                break;
            //case EM_AwardCenter.CrossTournament:
                
            //    break;
            //case EM_AwardCenter.CrossTournamentFightMail:

            //    break;
            //case EM_AwardCenter.CrossTournamentBonusTop1Point:

            //    break;
            //case EM_AwardCenter.CrossTournamentBonusTop1:

            //    break;
            default:
                tDesc.text = tAward.m_AwardCenterDT.szDesc.Replace(GameParamConst.ReplaceFlag, tAward.m_Param[0].ToString());
                if (tAward.m_Param[1] != 0)
                    tDesc.text = tDesc.text.Replace(GameParamConst.ReplaceFlag2, tAward.m_Param[1].ToString());
                break;

        }
        for (int i = 0; i < tAward.m_Goods.Length; i++)
        {
            ResourceCommonDT ttResource = new ResourceCommonDT();
            GameObject tmpGoods;
            if (i > tAwardGoodsParent.childCount - 1)
                tmpGoods = NGUITools.AddChild(tAwardGoodsParent.gameObject, item.Find("AwardGoods/AwardItem").gameObject);
            else
                tmpGoods = tAwardGoodsParent.GetChild(i).gameObject;
            if (tAward.m_Goods[i].uType == 0)
            {
                tmpGoods.SetActive(false);
                continue;
            }
            tmpGoods.SetActive(true);
            ttResource.f_UpdateInfo(tAward.m_Goods[i].uType, tAward.m_Goods[i].uId, tAward.m_Goods[i].uNum);

            tmpGoods.SetActive(true);
            tmpGoods.GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(ttResource.mImportant);
            tmpGoods.transform.Find("Num").GetComponent<UILabel>().text = ttResource.mResourceNum.ToString();
            tmpGoods.transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(ttResource.mIcon);
            f_RegClickEvent(tmpGoods, UI_ShowAward, ttResource);
        }
        AwardId = tAward;
        f_RegClickEvent(item.Find("Get").gameObject, Award_Get, tAward);
        tAwardGoodsParent.GetComponent<UIGrid>().Reposition();

    }



    /// <summary>
    /// 绑定事件
    /// </summary>
    protected void f_Message()
    {
        f_RegClickEvent("Close", UI_CloseThis);
        f_RegClickEvent("MaskClose", UI_CloseThis);
        f_RegClickEvent("OneKeyGetReward", Award_OneKeyGet);
    }

    void UI_CloseThis(GameObject go, object obj1, object obj2)
    {
        //ccUIHoldPool.GetInstance().f_UnHold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.Award, UIMessageDef.UI_CLOSE);
    }

    /// <summary>
    /// 一键领取
    /// </summary>
    void Award_OneKeyGet(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tback = new SocketCallbackDT();
        tback.m_ccCallbackFail = GetFaill;
        Data_Pool.m_AwardPool.f_OneKeyRewardGet(tback);
    }

    /// <summary>
    /// 点击领奖
    /// </summary>
    void Award_Get(GameObject go, object obj1, object obj2)
    {
       // AwardPoolDT tAward = (AwardPoolDT)obj1;
        UITool.f_OpenOrCloseWaitTip(true);
        //Data_Pool.m_AwardPool.m_GetLoginAward.Clear();
        SocketCallbackDT tback = new SocketCallbackDT();
        tback.m_ccCallbackSuc = GetSuc;
        tback.m_ccCallbackFail = GetFaill;
        
        Data_Pool.m_AwardPool.f_Get(AwardId.iId, tback);
    }
    private AwardPoolDT AwardId;

    void GetFaill(object obj)
    {
UITool.f_OpenOrCloseWaitTip(false); UITool.Ui_Trip("Nhận thất bại");
    }
    void GetSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if (AwardId == null)
        {
MessageBox.ASSERT("PoolDT returns Null");
        }
        //UITool.Ui_Trip("领奖成功");
        AwardParentWrap.f_ResetView();
        AwardParentWrap.f_UpdateView();

        Data_Pool.m_AwardPool._RunDelg();

        List<AwardPoolDT> awardList = new List<AwardPoolDT>(); //Data_Pool.m_AwardPool.m_GetLoginAward;

        for (int i = 0; i < AwardId.m_Goods.Length; i++)
        {
            AwardPoolDT poolDT = new AwardPoolDT();
            if (AwardId.m_Goods[i].uType == 0)
            {
                continue;
            }
            poolDT.f_UpdateByInfo(AwardId.m_Goods[i].uType, AwardId.m_Goods[i].uId, AwardId.m_Goods[i].uNum);
            awardList.Add(poolDT);
        }
        if (awardList.Count >= 1)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        }
        if (_AwardCenter.Count <= 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.Award, UIMessageDef.UI_CLOSE);
        }

        AwardParentWrap.f_OnClickIndex(0);
    }

    /// <summary>
    /// 点击icon显示详细信息
    /// </summary>
    void UI_ShowAward(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, obj1);
    }
}
