using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
using Spine;
using System.Linq;
using Spine.Unity;

public class RebelArmy : UIFramwork
{
    private float OneBillionth  = 0.00000001f; //亿分之一，用以计算叛军血量（以前百分比，如4000万的血，百分之一就是40万，，会出现如果小于40万的伤害,百分比就不会变化的bug!!!）
    private int TenThousand = 10000;
    private const int CrusadeTokenId = 202;

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);

        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.Arena);
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_CrusadeToken);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Fame);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        //断线重连如果切去其他界面，这时候不处理，服务器消息返回再打开界面就会和其他界面重叠
        if (!UITool.f_IsCanOpenChallengePage(UINameConst.RebelArmy))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelArmy, UIMessageDef.UI_CLOSE);
            return;
        }

        InitUI();
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_RebelArmyPool.f_CrusadeRebelInit(Init);
        Data_Pool.m_RebelArmyPool.f_CrusadeRebelList(delegate (object obj)
        {
            //如果e不等于null,则表示从战斗结算中回来，，此时则根据需要弹出二级挑战界面及分享界面
            if (null != e)
            {
                f_ShowChallengePanelAfterFight();
            }

            //设置叛军主界面
            UpdateRebel();
        });
        //f_LoadTexture(true);
    }

    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RebelArmyAward, f_GetObject("Btn_Exploit"), ReddotCallback_Show_Award, true);
        UpdateReddotUI();
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RebelArmyAward);
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.RebelArmyAward, f_GetObject("Btn_Exploit"));
    }
    private void ReddotCallback_Show_Award(object obj)
    {
        int iNum = (int)obj;
        GameObject BtnFragment = f_GetObject("Btn_Exploit");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(73, 77, 0), 74);
    }


    #endregion


    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture(bool light)
    {
        //加载背景图
        UITexture TextureBg = f_GetObject("TextureBg").GetComponent<UITexture>();
        if (light)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRootL);
            TextureBg.mainTexture = tTexture2D;
        }
        else
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRootD);
            TextureBg.mainTexture = tTexture2D;
        }
    }

    Transform RebelArmyPos;   //主界面显示模型的位置
    RebelArmyPoolDT tRebelArmy;//当前详情界面的叛军
    UILabel Particulars_Time;
    EM_RankType m_RankType;   //排行榜类型
    private string strTexBgRootL = "UI/TextureRemove/RebelArmy/RebelBgL";
    private string strTexBgRootD = "UI/TextureRemove/RebelArmy/RebelBgD";

    void InitUI()
    {
        RebelArmyPos = f_GetObject("RebelArmyPos").transform;
        Particulars_Time = f_GetObject("Particulars_Time").GetComponent<UILabel>();
    }

    void Init(object obj)
    {
        int RankExploit = Data_Pool.m_RebelArmyPool.tRebelInit.rankExploit;
        int RankDPS = Data_Pool.m_RebelArmyPool.tRebelInit.rankDmg;
        f_GetObject("ExploitLabel").GetComponent<UILabel>().text = string.Format("{0}({1})", Data_Pool.m_RebelArmyPool.tRebelInit.uExploit, RankExploit == 0 ? CommonTools.f_GetTransLanguage(954) : CommonTools.f_GetTransLanguage(955) + RankExploit);
        if (Data_Pool.m_RebelArmyPool.tRebelInit.maxDmg >= TenThousand)
        {
            f_GetObject("DPSLabel").GetComponent<UILabel>().text = string.Format("{0}{1}({2})", Data_Pool.m_RebelArmyPool.tRebelInit.maxDmg / TenThousand, CommonTools.f_GetTransLanguage(286),
                RankDPS == 0 ? CommonTools.f_GetTransLanguage(954) : CommonTools.f_GetTransLanguage(955) + RankDPS);
        }
        else
        {
            f_GetObject("DPSLabel").GetComponent<UILabel>().text = string.Format("{0}({1})", Data_Pool.m_RebelArmyPool.tRebelInit.maxDmg, RankDPS == 0 ? CommonTools.f_GetTransLanguage(954) : CommonTools.f_GetTransLanguage(955) + RankDPS);
        }
        
        f_GetObject("AbilityLabel").GetComponent<UILabel>().text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_BattleFeat).ToString();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Btn_Close", UI_CloseThis);
        f_RegClickEvent("Btn_ClosePar", UI_CloseParticulars);
        f_RegClickEvent("Btn_ClosePar2", UI_CloseParticulars);
        f_RegClickEvent("Btn_Ranking", UI_OpenRank);
        f_RegClickEvent("Btn_Exploit", UI_OpenExpliotAward);
        f_RegClickEvent("Btn_Invite", UI_OpenRecommendFriendPage);
        f_RegClickEvent("CresadeAdd", UI_OpenUseGoods);
        f_RegClickEvent("Btn_Embbatle", UI_OpenClothArray);
        f_RegClickEvent("Rebel_Help", UI_OpenHelp);
        f_RegClickEvent("CloseHelpPage", UI_CloseUI, "HelpPage");
        f_RegClickEvent("CloseHelpPage2", UI_CloseUI, "HelpPage");
        f_RegClickEvent("Btn_ExploitShop", UI_OpenShop);
        f_RegClickEvent("CloseRebelAward", UI_CloseUI, "RebelAward");
        f_RegClickEvent("CloseRebelAward2", UI_CloseUI, "RebelAward");
        f_RegClickEvent("CloseShare", UI_CancelShare);
        f_RegClickEvent("ConfirmShare", UI_ConfirmShare);
        f_RegClickEvent("CloseShare2", UI_CancelShare);
    }

    void UI_OpenShop(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.BattleFeatShop);
    }
    
    /// <summary>
    /// 打开帮助界面
    /// </summary>
    void UI_OpenHelp(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage,UIMessageDef.UI_OPEN,7);
        //f_GetObject("HelpPage").SetActive(true);
    }

    void UI_CloseThis(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelArmy, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.MainMenu, UIMessageDef.UI_OPEN);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    protected override void UI_UNHOLD(object object_0)
    {
        base.UI_UNHOLD(object_0);
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_CrusadeToken);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Fame);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }

    void UI_OpenExpliotAward(GameObject go, object obj1, object obj2)
    {
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(956));
        UpdateRebelAward();
    }

    void UI_CloseUI(GameObject go, object obj1, object obj2)
    {
        string str = (string)obj1;
        if (str == UINameConst.RebelArmy)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelArmy, UIMessageDef.UI_CLOSE);
        }
        f_GetObject(str).SetActive(false);
    }

    void UI_OpenClothArray(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

    void UI_OpenRecommendFriendPage(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RecommendFriendPage, UIMessageDef.UI_OPEN);
    }

    #region  功勋奖励
    UIWrapComponent _AwardWrap;
    List<NBaseSCDT> _AwardList;
    List<NBaseSCDT> _tAward;
    GameObject _AwardItem;
    GameObject _AwardItemParent;
    void UpdateRebelAward()
    {
        Data_Pool.m_RebelArmyPool.f_CrusadeRebelInit(ShowAward);
    }
    void ShowAward(object obj)
    {
        f_GetObject("RebelAward").SetActive(true);
        _tAward = new List<NBaseSCDT>();
        _AwardList = glo_Main.GetInstance().m_SC_Pool.m_ExploitAwardSC.f_GetAll();
        ExploitLvDT tlv = null;
        List<NBaseSCDT> tLvList = glo_Main.GetInstance().m_SC_Pool.m_ExploitLvSC.f_GetAll();
        for (int i = 0; i < tLvList.Count; i++)
        {
            if ((tLvList[i] as ExploitLvDT).iLv >= Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level))
            {
                tlv = tLvList[i] as ExploitLvDT;
                break;
            }
        }
        for (int i = 0; i < _AwardList.Count; i++)
        {
            if ((_AwardList[i] as ExploitAwardDT).iLvId == tlv.iId)
                _tAward.Add(_AwardList[i]);
        }
        _tAward.Sort(_AwardSort);
        _AwardItem = f_GetObject("AwardItem");
        _AwardItemParent = f_GetObject("AwaedItemParent");
        if (_AwardWrap == null)
        {
            _AwardWrap = new UIWrapComponent(200, 1, 770, 6, _AwardItemParent, _AwardItem, _tAward, UpdateAwardInfo, null);
        }
        else
        {
            _AwardWrap.f_UpdateList(_tAward);
            _AwardWrap.f_UpdateView();
        }
        f_RegClickEvent("OneKeyGet", RebelArmyAward, 0);
    }

    int _AwardSort(NBaseSCDT Dt1, NBaseSCDT Dt2)
    {
        ExploitAwardDT Exploit1 = Dt1 as ExploitAwardDT;
        ExploitAwardDT Exploit2 = Dt2 as ExploitAwardDT;
        if (Exploit1.iState == 1 && Exploit2.iState != 1)
        {
            return -1;
        }
        else if (Exploit1.iState != 1 && Exploit2.iState == 1)
        {
            return 1;
        }
        else
        {
            if (Exploit1.iState == 2 && Exploit2.iState != 2)
                return 1;
            else if (Exploit1.iState != 2 && Exploit2.iState == 2)
                return -1;
            else
            {
                if (Exploit1.iId > Exploit2.iId)
                    return 1;
                else if (Exploit2.iId > Exploit1.iId)
                    return -1;
                else
                    return 0;
            }
        }
    }


    bool f_GetIsOpenAward(ExploitAwardDT t)
    {
        int index = _tAward.IndexOf(t);
        int flagN;
        int flagM;
        if (index != 0)
        {
            flagN = index / 10;
            flagM = index - flagN * 10;
            return Data_Pool.m_RebelArmyPool.m_AwardFlag[flagN][flagM] == '0' &&
                Data_Pool.m_RebelArmyPool.tRebelInit.uExploit >= t.iRequirement;

        }

        return false;
    }
    /// <summary>
    /// 更新奖励初始化
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="dt"></param>
    void UpdateAwardInfo(Transform tf, NBaseSCDT dt)
    {
        int index = _tAward.IndexOf(dt);
        int flagN = index / 10;    //N段
        int flagM = index - flagN * 10;    //M档
        //string flag = Data_Pool.m_RebelArmyPool.m_AwardFlag[flagN];
        //for (int i = flag.Length; i < 10; i++)
        //    flag = '0' + flag;
        ExploitAwardDT Exploit = dt as ExploitAwardDT;
        UILabel tName = tf.Find("Item_Name").GetComponent<UILabel>();
        UILabel tNeedExploit = tf.Find("RankPro").GetComponent<UILabel>();
        UILabel tPlan = tf.Find("CapacityNum").GetComponent<UILabel>();
        UI2DSprite tIcon = tf.Find("HeadCase/Head").GetComponent<UI2DSprite>();
        UILabel tNum = tf.Find("HeadCase/Num").GetComponent<UILabel>();
        GameObject Geted = tf.Find("geted").gameObject;
        Geted.SetActive(false);
        GameObject Get = tf.Find("Get").gameObject;
        Get.SetActive(false);
        tNum.text = Exploit.iNum.ToString();
        tName.text = UITool.f_GetName((EM_ResourceType)Exploit.iAwardType, Exploit.iAward);
        tNeedExploit.text = Exploit.iRequirement.ToString();//string.Format(CommonTools.f_GetTransLanguage(957), );

        tPlan.text = string.Format(Exploit.iRequirement > Data_Pool.m_RebelArmyPool.tRebelInit.uExploit ? CommonTools.f_GetTransLanguage(958)
            : CommonTools.f_GetTransLanguage(959), Data_Pool.m_RebelArmyPool.tRebelInit.uExploit, Exploit.iRequirement);
        UITool.f_SetIconSprite(tIcon, (EM_ResourceType)Exploit.iAwardType, Exploit.iAward);
        if (Exploit.iState == 2)
            Geted.SetActive(true);
        else if (Exploit.iState == 1)
        {
            Get.SetActive(true);
            f_RegClickEvent(Get.transform.GetChild(0).gameObject, RebelArmyAward, Exploit.iIndex);
        }
    }

    void RebelArmyAward(GameObject go, object obj1, object obj2)
    {
        GetAward((int)obj1);
    }
    List<ExploitAwardDT> awardList = new List<ExploitAwardDT>();
    void GetAward(int tmp)
    {
        if (tmp == 0)
        {
            bool isOneGet = false;
            for (int i = 0; i < _tAward.Count; i++)
            {
                ExploitAwardDT t = _tAward[i] as ExploitAwardDT;
                if (t.iState==1) {
                    isOneGet = true;
                    awardList.Add(t);
                }
            }

            if (!isOneGet)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(960));
                return;
            }
        }
        AwardType = (byte)tmp;
        tAwardFlag = Data_Pool.m_RebelArmyPool.tRebelInit.uAwardFlag;
        SocketCallbackDT tCallBack = new SocketCallbackDT();
        tCallBack.m_ccCallbackSuc = AwardSuc;
        tCallBack.m_ccCallbackFail = AwardFail;
        Data_Pool.m_RebelArmyPool.f_CrusadeRebelExploitAward((byte)tmp, tCallBack);
    }
    byte AwardType = new byte();
    int[] tAwardFlag;
    void AwardSuc(object obj)
    {
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(961));
        _AwardWrap.f_UpdateView();
        if (AwardType == 0)
        {
            //f_GetObject("Award").SetActive(true);
            OneGet();
        }
        Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.RebelArmyAward);
    }

    void AwardFail(object obj)
    {
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(962));
    }

    void OneGet()
    {
        //for (int i = 0; i < f_GetObject("GoodsParent").transform.childCount; i++)
        //    Destroy(f_GetObject("GoodsParent").transform.GetChild(i).gameObject);
        ArrayList tArrList = new ArrayList();  //放的是奖励类型
        Dictionary<int, int> tDictionary = new Dictionary<int, int>(); // Key:物品ID   ,   Value:物品数量
        for (int i = 0; i < awardList.Count; i++)
        {
            if (tDictionary.ContainsKey(awardList[i].iAward))
                tDictionary[awardList[i].iAward] += awardList[i].iNum;
            else
            {
                tDictionary.Add(awardList[i].iAward, awardList[i].iNum);
                tArrList.Add(awardList[i].iAwardType);
            }
        }
        object[] arrint = tArrList.ToArray();
        List<AwardPoolDT> Award = new List<AwardPoolDT>();
        for (int i = 0; i < arrint.Length; i++)
        {
            //Transform go = NGUITools.AddChild(f_GetObject("GoodsParent"), f_GetObject("GoodsItem")).transform;
           // go.gameObject.SetActive(true);
            AwardPoolDT tDT = new AwardPoolDT();
            //ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
            tDT.f_UpdateByInfo((byte)(EM_ResourceType)((int)arrint[i]), tDictionary.ElementAt(i).Key, tDictionary.ElementAt(i).Value);
            Award.Add(tDT);
            //go.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(resourceCommonDT.mIcon);
            //go.Find("Num").GetComponent<UILabel>().text = tDictionary.ElementAt(i).Value.ToString();
            //string Name = resourceCommonDT.mName;
            //go.Find("Icon_Out").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(resourceCommonDT.mImportant, ref Name);
            //go.Find("Name").GetComponent<UILabel>().text = Name;

        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage,UIMessageDef.UI_OPEN,new object[] { Award });

        //f_GetObject("GoodsParent").GetComponent<UIGrid>().repositionNow = true;


        //f_RegClickEvent("Btn_SucAward", (GameObject gp, object obj1, object obj2) => { f_GetObject("Award").SetActive(false); awardList.Clear(); });
        //f_RegClickEvent("Btn_SucAward2", (GameObject gp, object obj1, object obj2) => { f_GetObject("Award").SetActive(false); awardList.Clear(); });
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RebelArmyAward);
    }
    #endregion

    void f_ShowChallengePanelAfterFight()
    {
        //如果胜利则表示叛军已打死，就不用弹二级界面
        var ebelArmyFinish = Data_Pool.m_RebelArmyPool.mRebelArmyFinish;
        if (ebelArmyFinish.isWin == 1)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RebelArmy);
            return;
        }

        //打开叛军二级挑战界面
        long lastArmyId = LocalDataManager.f_GetLocalData<long>(LocalDataType.Long_LastRebelArmyId);
        if (lastArmyId <= 0)
        {
            return;
        }
        UpdateParticylars(lastArmyId);

        //分享叛军
        if (ebelArmyFinish.isCanShare == 1)
        {
            var objShare = f_GetObject("Share");
            if (null == objShare)
            {
MessageBox.DEBUG("objShare does not exist！");
                return;
            }
            objShare.SetActive(true);
        }
    }

    /// <summary>
    /// 更新主界面显示人物
    /// </summary>
    void UpdateRebel()
    {
        //设置叛军
        UITool.f_OpenOrCloseWaitTip(false);
        List<BasePoolDT<long>> tPoolDT = Data_Pool.m_RebelArmyPool.f_GetAll();

        //正常我测了我的都在第一位，但是测试测出来不在第一位，为了避免新手卡死，强制在这里处理成把我的放第一位
        int myIndex = -1;
        for (int i = 0; i < tPoolDT.Count; i++)
        {
            if (tPoolDT[i].iId == Data_Pool.m_UserData.m_iUserId)
            {
                myIndex = i;
                break;
            }
        }
        if (myIndex > 0)
        {
            //我的不在第一个，交换位置
            BasePoolDT<long> temp = tPoolDT[0];
            tPoolDT[0] = tPoolDT[myIndex];
            tPoolDT[myIndex] = temp;
        }
        
        var isHadArmy = false;
        for (int i = 0; i < 3; i++)
        {
            Transform transArmyParent = RebelArmyPos.Find("Army" + (i + 1));
            if (null == transArmyParent)
            {
MessageBox.DEBUG("transArmyParent null，order number：" + i);
                continue;
            }
            transArmyParent.gameObject.SetActive(false);
            if (i >= tPoolDT.Count)
                continue; ;
            if (null != transArmyParent.Find("Model"))
            {
                UITool.f_DestoryStatelObject(transArmyParent.Find("Model").gameObject);
            }

            //逃走不显示
            var rebelArmyPoolDT = tPoolDT[i] as RebelArmyPoolDT;
            if (null == rebelArmyPoolDT)
            {
                continue;
            }
            if (!isAway(rebelArmyPoolDT))
                continue;

            CreateRebelModel(rebelArmyPoolDT, transArmyParent, 3 - i);
            transArmyParent.Find("Model").gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
            transArmyParent.gameObject.SetActive(true);
            isHadArmy = true;
        }

        //设置没有叛军的信息
        Transform transNoArmy = RebelArmyPos.Find("NoArmy");
        if (!isHadArmy)
        {
            if (transNoArmy.Find("Model") == null)
            {
                UITool.f_GetStatelObject(Data_Pool.m_CardPool.mRolePoolDt, transNoArmy, new Vector3(0, -180, 0), Vector3.zero, 6, "Model", 80 );
            }

            transNoArmy.GetChild(0).GetComponent<UILabel>().text = "";
            transNoArmy.gameObject.SetActive(true);
            f_RegClickEvent("Rebel_GoToWay", Btn_GoToWay);
            f_LoadTexture(true);
        }
        else
        {
            transNoArmy.gameObject.SetActive(false);
            f_LoadTexture(false);
        }
    }

    /// <summary>
    /// 更新单个模型
    /// </summary>
    /// <param name="RebelPoolDT"></param>
    /// <param name="tTran"></param>
    void CreateRebelModel(RebelArmyPoolDT RebelPoolDT, Transform tTran, int i = 1 )
    {
        int[] CardId = ccMath.f_String2ArrayInt(RebelPoolDT.m_RebelArmyDeploy.szMonsterId, ";");
        CardDT tcard = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(CardId[4]) as CardDT;

        string tImportant = string.Empty;
        switch ((EM_Important)RebelPoolDT.m_Color)
        {
            case EM_Important.Green:
                tImportant = CommonTools.f_GetTransLanguage(963);
                break;
            case EM_Important.Blue:
                tImportant = CommonTools.f_GetTransLanguage(964);
                break;
            case EM_Important.Purple:
                tImportant = CommonTools.f_GetTransLanguage(965);
                break;
            default:
                break;
        }
        tImportant += tcard.szName;

        tTran.Find("Panel/Name").GetComponent<UILabel>().text = UITool.f_GetImporentForName(RebelPoolDT.m_Color, tImportant);
        var hpSlider = tTran.Find("Panel/HP").GetComponent<UISlider>();

        if (RebelPoolDT.iId == Data_Pool.m_UserData.m_iUserId)  //如果是自己就显示自己的ID
            tTran.Find("Panel/Find").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(966), Data_Pool.m_UserData.m_szRoleName);
        else   //  去玩家Pool中需找玩家的名字
            tTran.Find("Panel/Find").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(966), (Data_Pool.m_GeneralPlayerPool.f_GetForId(RebelPoolDT.iId) as BasePlayerPoolDT).m_szName);

        GameObject Stare = UITool.f_GetStatelObject(tcard.iId, tTran, Vector3.zero, Vector3.zero, i, "Model", 55);
        bool isKill = (RebelPoolDT.hpPercent * OneBillionth) <= 0;
        tTran.Find("Panel/Kill").gameObject.SetActive(isKill);
        if (isKill)  // 如果血量低于0  , 就显示已经击杀
        {
            f_UnRegClickEvent(tTran.gameObject);
            hpSlider.gameObject.SetActive(false);

            //死亡，停止播放动画
            SkeletonAnimation SkeAni = Stare.GetComponent<SkeletonAnimation>();
            if (null == SkeAni)
            {
                Debug.LogError("SkeletonAnimation null，Model id:" + tcard.iId);
                return;
            }
            SkeAni.state.SetEmptyAnimation(0, 0);

            //设置层级
            var meshRenderer = Stare.GetComponent<MeshRenderer>();
            if (null == meshRenderer)
            {
                return;
            }
            meshRenderer.sortingOrder = 1;
            meshRenderer.sortingLayerName = "Default";

            //模型变灰
            SetModelColor.SetColor(Stare, SetModelColor.gray);          
        }
        else
        {
            SetModelColor.SetColor(Stare, SetModelColor.normal);
            hpSlider.value = (float)(RebelPoolDT.hpPercent * OneBillionth);
            hpSlider.gameObject.SetActive(true);
            f_RegClickEvent(tTran.gameObject, OnClick, RebelPoolDT.iId);
        }
    }

    /// <summary>
    /// 点击显示详情界面
    /// </summary>
    void OnClick(GameObject go, object obj1, object obj2)
    {
        UpdateParticylars((long)obj1);
    }
    /// <summary>
    /// 打开排行榜界面
    /// </summary>
    void UI_OpenRank(GameObject go, object obj1, object obj2)
    {
        UI_UpdateRank();
    }

    void Btn_GoToWay(GameObject go, object obj1, object obj2)
    {
        UITool.f_GotoPage(this, UINameConst.DungeonChapterPageNew, (int)EM_Fight_Enum.eFight_DungeonMain);
    }
    /// <summary>
    /// 关闭详情界面
    /// </summary>
    void UI_CloseParticulars(GameObject go, object obj1, object obj2)
    {
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(968));
        f_GetObject("ParticularsPanel").SetActive(false);
        if (ArmyPos != null)
            UITool.f_DestoryStatelObject(ArmyPos);
        CancelInvoke("f_UpdateTime");
    }

    GameObject ArmyPos;   //详细界面的模型
                          /// <summary>
                          /// 刷新详情界面
                          /// </summary>
    void UpdateParticylars(long i)
    {
        CancelInvoke("f_UpdateTime");
        RebelArmyPoolDT tDateDT = Data_Pool.m_RebelArmyPool.f_GetForId(i) as RebelArmyPoolDT;
        if (tDateDT == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(969));
            return;
        }
        f_GetObject("ParticularsPanel").SetActive(true);
        LocalDataManager.f_SetLocalData<long>(LocalDataType.Long_LastRebelArmyId, i);
        tRebelArmy = tDateDT;
        int[] CardId = ccMath.f_String2ArrayInt((tDateDT.m_RebelArmyDeploy.szMonsterId), ";");
        CardDT tcard = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(CardId[4]) as CardDT;
        string name = tcard.szName;
        UITool.f_GetImporentColorName(tDateDT.m_Color, ref name);
        f_GetObject("ArmyName").GetComponent<UILabel>().text = name;


        if (f_GetObject("ArmyPos").transform.childCount >= 1)
            UITool.f_DestoryStatelObject(f_GetObject("ArmyPos").transform.GetChild(0).gameObject);
        ArmyPos = UITool.f_GetStatelObject(tcard);
        ArmyPos.transform.parent = f_GetObject("ArmyPos").transform;
        ArmyPos.transform.localScale = tcard.iId == 1800 ? Vector3.one * 50 : Vector3.one * 70;
        ArmyPos.transform.localPosition = new Vector3(0, 0, 0);
        ArmyPos.transform.localEulerAngles = new Vector3(0, 180, 0);
        ArmyPos.layer = LayerMask.NameToLayer(GameParamConst.UILayerName);
        ArmyPos.GetComponent<MeshRenderer>().sortingOrder = 7;
        //ArmyPos.GetComponent<SkeletonAnimation>().AnimationName = "Stand";
        //ArmyPos.GetComponent<SkeletonAnimation>().loop = true;

        SkeletonAnimation SkeAni = ArmyPos.GetComponent<SkeletonAnimation>();
        SkeAni.state.SetAnimation(0, "Stand", true);

        f_GetObject("Particulars_Lv").GetComponent<UILabel>().text = string.Format("{0}", tDateDT.m_RevelLv);
        long tHp = (glo_Main.GetInstance().m_SC_Pool.m_RebelArmySC.f_GetSC(1 * 100000 + tDateDT.m_Color * 10000 + tDateDT.m_RevelLv) as RebelArmyDT).lHp * 6;
        float per = tDateDT.hpPercent * OneBillionth;
        int remainHp = (int)(tHp * per);
        if (remainHp >= TenThousand)
        {
            f_GetObject("Particulars_Hp").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(970), (int)(remainHp * 0.0001), (int)(tHp * 0.0001));
        }
        else
        {
            f_GetObject("Particulars_Hp").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(2219), remainHp, (int)(tHp * 0.0001));
        }
        f_GetObject("HpSlider").GetComponent<UISlider>().value = per;
        int tCresadeNum = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_CrusadeToken);
        if (tCresadeNum <= 0)
            f_GetObject("CresadeNum").GetComponent<UILabel>().text = string.Format("[ff0000]{0}/{1}[-]", tCresadeNum, 10);
        else
            f_GetObject("CresadeNum").GetComponent<UILabel>().text = string.Format("{0}/{1}", tCresadeNum, 10);
        InvokeRepeating("f_UpdateTime", 0, 1f);
        DateTime tTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        //new DateTime(GameSocket.GetInstance().f_GetServerTime());ccMath.f_ti
        //ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        MessageBox.DEBUG(tTime + "");
        if (tTime.Hour >= 12 && tTime.Hour < 14)
            f_GetObject("AllOutNum").GetComponent<UILabel>().text = "1";
        else
            f_GetObject("AllOutNum").GetComponent<UILabel>().text = "2";
        f_RegClickEvent("Btn_Common", CrusadeRebel, tDateDT, 0);
        f_RegClickEvent("Btn_AllOut", CrusadeRebel, tDateDT, 1);
        if (!isAway(tDateDT))
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(971));
    }

    //确定分享
    void UI_ConfirmShare(GameObject obj, object obj1, object obj2)
    {
        var tCallBack = new SocketCallbackDT();
        tCallBack.m_ccCallbackSuc = ShareSuccess;
        Data_Pool.m_RebelArmyPool.f_CrusadeRebelShare(tCallBack);

        var objShare = f_GetObject("Share");
        if (null == objShare)
        {
MessageBox.DEBUG("objShare does not exist！");
            return;
        }
        objShare.SetActive(false);
    }

    //取消分享
    void UI_CancelShare(GameObject obj, object obj1, object obj2)
    {
        var objShare = f_GetObject("Share");
        if (null == objShare)
        {
MessageBox.DEBUG("objShare does not exist！");
            return;
        }
        objShare.SetActive(false);
    }

    //分享成功
    private void ShareSuccess(object obj)
    {
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2153));
    }

    #region  排行榜界面

    void UI_UpdateRank(GameObject go, object obj1, object obj2)
    {
        EM_RankType tType = (EM_RankType)obj1;
        m_RankType = tType;
        switch (tType)
        {
            case EM_RankType.Exploit:
                UpdateExploitRank(EM_RankType.Exploit);
                break;
            case EM_RankType.DPS:
                UpdateExploitRank(EM_RankType.DPS);
                break;
            case EM_RankType.Award:
                AwardPreview();
                break;
        }
    }
    /// <summary>
    /// 更新排行榜界面
    /// </summary>
    void UI_UpdateRank()
    {
        f_GetObject("Rank").SetActive(true);
        m_RankType = EM_RankType.Exploit;
        f_GetObject("ExploitRank_Btn").GetComponent<UIToggle>().value = true;
        UpdateExploitRank(EM_RankType.Exploit);
        f_RegClickEvent("CloseRank", UI_CloseUI, "Rank");
        f_RegClickEvent("CloseRank2", UI_CloseUI, "Rank");
        f_RegClickEvent("ExploitRank_Btn", UI_UpdateRank, EM_RankType.Exploit);
        f_RegClickEvent("DPSRank_Btn", UI_UpdateRank, EM_RankType.DPS);
        f_RegClickEvent("RankAwad_Btn", UI_UpdateRank, EM_RankType.Award);
    }
    /// <summary>
    /// 更新排名显示
    /// </summary>
    void UpdateLeg(string Rank, int AnimRank, int BattleFeat, int AnimBattleFeat)
    {
        UILabel tMyRank = f_GetObject("MyRank").GetComponent<UILabel>();
        UILabel tAimRank = f_GetObject("AimRank").GetComponent<UILabel>();
        UILabel tRankAwardNum = f_GetObject("RankAwardNum").GetComponent<UILabel>();
        UILabel tRankAwardNum2 = f_GetObject("RankAwardNum2").GetComponent<UILabel>();

        tMyRank.text = string.Format(CommonTools.f_GetTransLanguage(972), Rank);
        if (AnimRank != 0)
        {
            f_GetObject("AimRank").SetActive(true);
            f_GetObject("RankAwardNum2").SetActive(true);
            f_GetObject("RankAward2").SetActive(true);
            tAimRank.text = string.Format(CommonTools.f_GetTransLanguage(973), AnimRank);
            tRankAwardNum2.text = AnimBattleFeat.ToString();
        }
        else
        {
            f_GetObject("RankAward2").SetActive(false);
            f_GetObject("RankAwardNum2").SetActive(false);
            f_GetObject("AimRank").SetActive(false);
        }
        tRankAwardNum.text = BattleFeat.ToString();
    }
    /// <summary>
    /// 刷新功勋排行榜
    /// </summary>
    void UpdateExploitRank(EM_RankType tType)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_RebelArmyPool.f_CrusadeRebelInit();
        switch (tType)
        {
            case EM_RankType.Exploit:
                Data_Pool.m_RebelArmyPool.f_CrusadeRebelExploitRank(external);
                break;
            case EM_RankType.DPS:
                Data_Pool.m_RebelArmyPool.f_CrusadeRebelDPSRank(external);
                break;
        }
    }
    void GetAward(int Rank, ref RebelArmyRankAwardDT tAwardDT, ref RebelArmyRankAwardDT NextAwaredDT)
    {
        List<NBaseSCDT> tAward = glo_Main.GetInstance().m_SC_Pool.m_RebelArmyRankAwardSC.f_GetAll();

        if (Rank == 0)  //判断玩家排名是否在200名外
        {
            tAwardDT = new RebelArmyRankAwardDT();
            tAwardDT.iExploitNum = 0;
            tAwardDT.iDmgNum = 0;
            NextAwaredDT = tAward[tAward.Count - 1] as RebelArmyRankAwardDT;
        }
        else
        {
            for (int index = 0; index < tAward.Count; index++)
            {
                if (Rank <= (tAward[index] as RebelArmyRankAwardDT).iRankUp)
                {
                    if (index != 0)//第一名就显示MAX
                        NextAwaredDT = tAward[index - 1] as RebelArmyRankAwardDT;
                    else
                    {
                        NextAwaredDT = new RebelArmyRankAwardDT();
                        NextAwaredDT.iExploitNum = 0;
                        NextAwaredDT.iDmgNum = 0;
                    }
                    tAwardDT = tAward[index] as RebelArmyRankAwardDT;
                    break;
                }
            }
        }
    }
    void external(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
       
        UpdateRank(((SC_CrusadeRebelDmgRank)obj).Info);
        string tRankExploit = string.Empty;
        string tAnimExplpit = string.Empty;
        RebelArmyRankAwardDT tAwardDT = null;
        RebelArmyRankAwardDT NextAwaredDT = null;
        switch (m_RankType)
        {
            case EM_RankType.Exploit:
                if (Data_Pool.m_RebelArmyPool.tRebelInit.rankExploit == 0)
                    tRankExploit = "200+";
                else
                    tRankExploit = Data_Pool.m_RebelArmyPool.tRebelInit.rankExploit.ToString();
                GetAward(Data_Pool.m_RebelArmyPool.tRebelInit.rankExploit, ref tAwardDT, ref NextAwaredDT);
                break;
            case EM_RankType.DPS:
                if (Data_Pool.m_RebelArmyPool.tRebelInit.rankDmg == 0)
                    tRankExploit = "200+";
                else
                    tRankExploit = Data_Pool.m_RebelArmyPool.tRebelInit.rankDmg.ToString();
                GetAward(Data_Pool.m_RebelArmyPool.tRebelInit.rankDmg, ref tAwardDT, ref NextAwaredDT);
                break;
        }
        int AnimRank = 0;
        if (NextAwaredDT.iExploitNum != 0)
            AnimRank = NextAwaredDT.iRankUp;
        else
            AnimRank = 0;
        if (m_RankType == EM_RankType.Exploit)
        {
            UpdateLeg(tRankExploit, AnimRank, tAwardDT.iExploitNum, NextAwaredDT.iExploitNum);
        }
        else if (m_RankType == EM_RankType.DPS)
        {
            UpdateLeg(tRankExploit, AnimRank, tAwardDT.iDmgNum, NextAwaredDT.iDmgNum);
        }
    }
    void UpdateRank(CrusadeRebelRank[] Rank)
    {
        Transform tItem = f_GetObject("PlayerParent").transform;
        for (int i = 0; i < Rank.Length; i++)
        {
            if (Rank[i].userId == 0)
                tItem.GetChild(i).gameObject.SetActive(false);
            else
            {
                switch (m_RankType)
                {
                    case EM_RankType.Exploit:
                        UpdateItem(tItem.GetChild(i), Rank[i], true);
                        break;
                    case EM_RankType.DPS:
                        UpdateItem(tItem.GetChild(i), Rank[i], false);
                        break;
                }
                tItem.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    void AwardPreview()
    {
        UILabel tBody1 = f_GetObject("RankAward").transform.Find("Body1").GetComponent<UILabel>();
        UILabel tBody2 = f_GetObject("RankAward").transform.Find("Body2").GetComponent<UILabel>();
        UILabel tBody3 = f_GetObject("RankAward").transform.Find("Body3").GetComponent<UILabel>();
        List<NBaseSCDT> tList = glo_Main.GetInstance().m_SC_Pool.m_RebelArmyRankAwardSC.f_GetAll();
        tBody1.text = string.Empty;
        tBody2.text = string.Empty;
        tBody3.text = string.Empty;
        for (int i = 0; i < tList.Count; i++)
        {
            if ((tList[i] as RebelArmyRankAwardDT).iRankDown == (tList[i] as RebelArmyRankAwardDT).iRankUp)
                tBody1.text += string.Format("{0}", (tList[i] as RebelArmyRankAwardDT).iRankUp);
            else
                tBody1.text += string.Format("{0}-{1}", (tList[i] as RebelArmyRankAwardDT).iRankDown, (tList[i] as RebelArmyRankAwardDT).iRankUp);
            tBody2.text += string.Format(CommonTools.f_GetTransLanguage(974), (tList[i] as RebelArmyRankAwardDT).iExploitNum);
            tBody3.text += string.Format(CommonTools.f_GetTransLanguage(974), (tList[i] as RebelArmyRankAwardDT).iDmgNum);
            if (i + 1 != tList.Count)
            {
                tBody1.text += "\n";
                tBody2.text += "\n";
                tBody3.text += "\n";
            }
        }
    }
    /// <summary>
    /// 更新排行榜界面角色详细信息
    /// </summary>
    void UpdateItem(Transform item, CrusadeRebelRank rank, bool rankType)
    {
        UI2DSprite tHead = item.Find("HeadCase/Head").GetComponent<UI2DSprite>();
        UISprite Item_Frame = item.Find("HeadCase/Outer").GetComponent<UISprite>();
        BasePlayerPoolDT tplay;
        UILabel tRankProNum = item.Find("RankPro/RankProNum").GetComponent<UILabel>();
        UILabel RankProName = item.Find("RankPro").GetComponent<UILabel>();
        UILabel CapacityNum = item.Find("CapacityNum").GetComponent<UILabel>();
        UILabel Item_Lv = item.Find("Item_Lv").GetComponent<UILabel>();
        UILabel Item_Name = item.Find("Item_Name").GetComponent<UILabel>();

        tRankProNum.text = rank.uDmg.ToString();
        if (rank.uDmg >= TenThousand)
        {
            tRankProNum.text = (rank.uDmg/TenThousand) + CommonTools.f_GetTransLanguage(286);
        }
        if (rankType)
            RankProName.text = CommonTools.f_GetTransLanguage(975);
        else
        {
            RankProName.text = CommonTools.f_GetTransLanguage(976);
            //tRankProNum.text += CommonTools.f_GetTransLanguage(977);
        }
        Debug.LogError("============" + rank.userId);
        if (Data_Pool.m_UserData.m_iUserId != rank.userId)
        {
            tplay = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(rank.userId);
            
            if (tplay != null)
            {
                string tName = tplay.m_szName;
                int iFrame = tplay.m_iFrameId;
                Item_Frame.spriteName = UITool.f_GetImporentColorName(iFrame, ref tName);
                Item_Name.text = tName;
                tHead.sprite2D = UITool.f_GetIconSpriteByCardId(tplay.m_CardId);
                if (tplay.m_iBattlePower < TenThousand)
                    CapacityNum.text = CommonTools.f_GetTransLanguage(978) + tplay.m_iBattlePower.ToString();
                else
                    CapacityNum.text = CommonTools.f_GetTransLanguage(978) + (tplay.m_iBattlePower / TenThousand).ToString() + CommonTools.f_GetTransLanguage(977);
                Item_Lv.text = string.Format(CommonTools.f_GetTransLanguage(979), tplay.m_iLv);
            }
        }
        else
        {
            TeamPoolDT teamPoolDT = Data_Pool.m_TeamPool.f_GetForId((int)EM_FormationPos.eFormationPos_Main) as TeamPoolDT;
            string tName = Data_Pool.m_UserData.m_szRoleName;
            int iFrame = teamPoolDT.m_CardPoolDT.m_CardDT.iImportant;
            Item_Frame.spriteName = UITool.f_GetImporentColorName(iFrame, ref tName);
            Item_Name.text = tName;
            tHead.sprite2D = UITool.f_GetIconSpriteByCardId(teamPoolDT.m_CardPoolDT);
            if (Data_Pool.m_TeamPool.f_GetTotalBattlePower() < TenThousand)
                CapacityNum.text = CommonTools.f_GetTransLanguage(978) + Data_Pool.m_TeamPool.f_GetTotalBattlePower();
            if (Data_Pool.m_TeamPool.f_GetTotalBattlePower() > TenThousand)
                CapacityNum.text = CommonTools.f_GetTransLanguage(978) + (Data_Pool.m_TeamPool.f_GetTotalBattlePower() / TenThousand + CommonTools.f_GetTransLanguage(977));
            Item_Lv.text = string.Format(CommonTools.f_GetTransLanguage(979), Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level));// Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level).ToString() + CommonTools.f_GetTransLanguage(979);
        }
    }
    #endregion



    /// <summary>
    /// 更新时间
    /// </summary>
    void f_UpdateTime()
    {
        if (isAway())
        {
            int tTime = tRebelArmy.m_EndTime - GameSocket.GetInstance().f_GetServerTime();
            string sTime = ccMath.f_Time_Int2String(tTime);
            MessageBox.DEBUG(sTime);
            Particulars_Time.text = sTime;
        }
        else
        {
            Data_Pool.m_RebelArmyPool.f_Delete(tRebelArmy.iId);
            Particulars_Time.text = CommonTools.f_GetTransLanguage(980);
        }
    }
    /// <summary>
    /// 判断叛军是否已经逃跑
    /// </summary>
    bool isAway(RebelArmyPoolDT tDateDT = null)
    {
        if (tDateDT == null)
            tDateDT = tRebelArmy;
        MessageBox.DEBUG(tDateDT.iData0 + "1111 " + tDateDT.iData1 + "   " + tDateDT.iData2 + "   " + tDateDT.iData3 + "   " + tDateDT.iData4 + "   ");
        if (tDateDT.m_EndTime > GameSocket.GetInstance().f_GetServerTime())
        {
            MessageBox.DEBUG(GameSocket.GetInstance().f_GetServerTime().ToString());
            return true;
        }
        return false;
    }
    void CrusadeRebel(GameObject go, object obj1, object obj2)
    {
        RebelArmyPoolDT tDateDT = obj1 as RebelArmyPoolDT;
        if (!isAway(tDateDT))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(981));
            return;
        }
        int t = (int)obj2;
        byte tb = (byte)t;
        if (tb == 0)
        {
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(982));
            if (!Trip(1))
                return;
        }
        else
        {
            DateTime tTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
            if (tTime.Hour >= 12 && tTime.Hour < 14)
            {
                if (!Trip(1))
                    return;
            }
            else
            {
                if (!Trip(2))
                    return;
            }
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(983));
        }
        if (tDateDT.hpPercent == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(984));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tBack = new SocketCallbackDT();
        tBack.m_ccCallbackSuc = OpenCombat;
        tBack.m_ccCallbackFail = CallBack_Fail;
        Data_Pool.m_RebelArmyPool.CR_CrusadeRebel(tDateDT.iId, tb, tBack);
    }

    #region 使用征讨令
    int UseNum;
    UILabel UseNumLabel;
    void UI_OpenUseGoods(GameObject go, object obj1, object obj2)
    {
        UI_OpenUse();
    }
    void UI_OpenUse()
    {        
        BaseGoodsPoolDT tGoods = Data_Pool.m_BaseGoodsPool.f_GetForData5(UITool.f_GetGoodsForEffect(EM_GoodsEffect.GetKill)[0]) as BaseGoodsPoolDT;
        Transform tTran = f_GetObject("Use").transform.Find("Batch");
        UILabel tName = tTran.Find("Name").GetComponent<UILabel>();
        UseNumLabel = tTran.Find("NumBg/Num").GetComponent<UILabel>();
        UILabel tNum = tTran.Find("own/Num").GetComponent<UILabel>();
        UI2DSprite tIcon = tTran.Find("Icon").GetComponent<UI2DSprite>();
        UISprite IconBorder = tTran.Find("IconBg").GetComponent<UISprite>();
        GameObject Add = tTran.Find("Add").gameObject;
        GameObject Add10 = tTran.Find("Add10").gameObject;
        GameObject Minus = tTran.Find("Minus").gameObject;
        GameObject Minus10 = tTran.Find("Minus10").gameObject;
        GameObject tClose = tTran.Find("Close").gameObject;
        GameObject tClose2 = tTran.Find("Close2").gameObject;
        GameObject tSuc = tTran.Find("Suc").gameObject;
        UILabel labelTips = tTran.Find("Label_Tip").GetComponent <UILabel>();

        ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
        resourceCommonDT.f_UpdateInfo((byte)EM_ResourceType.Good, 202, 0);
        string Name = resourceCommonDT.mName;
        string BorderName = UITool.f_GetImporentColorName(resourceCommonDT.mImportant, ref Name);
        IconBorder.spriteName = BorderName;
        tName.text = Name;
        tIcon.sprite2D = UITool.f_GetIconSprite(tGoods.m_BaseGoodsDT.iIcon);
        int totalNum = UITool.f_GetGoodsNum(UITool.f_GetGoodsForEffect(EM_GoodsEffect.GetKill)[0]);
        tNum.text = totalNum.ToString();
        labelTips.text = string.Format(CommonTools.f_GetTransLanguage(778), totalNum);
        UseNum = totalNum;
        UseNumLabel.text = UseNum.ToString();

        f_RegClickEvent(Add, UseAddorMinus, 1);
        f_RegClickEvent(Add10, UseAddorMinus, 10);
        f_RegClickEvent(Minus, UseAddorMinus, -1);
        f_RegClickEvent(Minus10, UseAddorMinus, -10);
        f_RegClickEvent(tSuc, UseKill);
        f_RegClickEvent(tClose, UI_CloseUI, "Use");
        f_RegClickEvent(tClose2, UI_CloseUI, "Use");
        f_GetObject("Use").SetActive(true);

    }


    void UseAddorMinus(GameObject go, object obj1, object obj2)
    {
        int totalNum = UITool.f_GetGoodsNum(UITool.f_GetGoodsForEffect(EM_GoodsEffect.GetKill)[0]);
        int N = (int)obj1;
        UseNum += N;
        int maxNum = totalNum > 999 ? 999 : totalNum;
        if (UseNum < 0)
            UseNum = totalNum > 0 ? 1 : 0;
        else if (UseNum > maxNum)
            UseNum = maxNum;
        UseNumLabel.text = UseNum.ToString();
    }

    void UseKill(GameObject go, object obj1, object obj2)
    {
        if (UseNum == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(986));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tBack = new SocketCallbackDT();
        tBack.m_ccCallbackSuc = UseSuc;
        tBack.m_ccCallbackFail = UseFail;
        int[] killBackpackIds = UITool.f_GetGoodsForEffect(EM_GoodsEffect.GetKill);
        List<BasePoolDT<long>> killGoodsList = Data_Pool.m_BaseGoodsPool.f_GetAllForData5(killBackpackIds[0]);
        for (int i = 0; i < killGoodsList.Count; i++)
        {
            //使用物品是以每个格子为单位的，，，所以如果征讨令在多个格子，，就要发送多个使用消息
            BaseGoodsPoolDT tGoods = killGoodsList[i] as BaseGoodsPoolDT;
            if (null == tGoods) continue;
            int useNum = tGoods.m_iNum >= UseNum ? UseNum : tGoods.m_iNum;
            Data_Pool.m_BaseGoodsPool.f_Use(tGoods.iId, useNum, 0, tBack);
            UseNum -= useNum;
            if (UseNum <= 0)
            {
                break;
            }
        }
    }

    void UseSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UI_CloseUI(gameObject, "Use", null);
        ccTimeEvent.GetInstance().f_RegEvent(0.8f, false, null, (object obj1) =>
           {
               //可能回调回来征讨令还没有刷新，，，所以延迟刷新
               f_GetObject("CresadeNum").GetComponent<UILabel>().text = string.Format("{0}/{1}", Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_CrusadeToken), 10);
           });
    }
    void UseFail(object obj) {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(987));
    }

    #endregion

    bool Trip(int Num)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_CrusadeToken) < Num)
        {
            if (UITool.f_GetGoodsNum(UITool.f_GetGoodsForEffect(EM_GoodsEffect.GetKill)[0]) > 0)
            {
                UI_OpenUse();
                return false;
            }
            else
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(988));
                GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, CrusadeTokenId, this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                return false;
            }
            
        }
        return true;
    }

    void OpenCombat(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(989) + obj.ToString());
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
    }

    void CallBack_Fail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(990) + obj.ToString());

        //处理失败结果
        int result = (int)obj;
        if (result == (int)eMsgOperateResult.eOR_ResLimit)
        {
            //征讨令不够
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(988));
            return;
        }
        else if (result == (int)eMsgOperateResult.eOR_CrusadeRebelDisscoverNotFound || result == (int)eMsgOperateResult.eOR_CrusadeRebelNotShare)
        {
            //叛军发现者未找到
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2154) + result);
        }
        else if (result == (int)eMsgOperateResult.eOR_CrusadeRebelHasRun)
        {
            //叛军已逃跑
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(981));
        }
        else if (result == (int)eMsgOperateResult.eOR_CrusadeRebelHasKilled)
        {
            //叛军已被击杀
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(984));
        }

        //关闭挑战界面并刷新主界面
        UI_CloseParticulars(null, null, null);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelArmy, UIMessageDef.UI_OPEN);
    }

    enum EM_RankType
    {
        Exploit,   //功勋
        DPS,    //伤害
        Award,    //奖励
    }

}
