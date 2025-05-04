using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
/// <summary>
/// 活动开服基金
/// </summary>
public class ActOpenServFundCtl : UIFramwork
{
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调
    private SocketCallbackDT BuyCallback = new SocketCallbackDT();//购买回调
    private List<BasePoolDT<long>> listOpenServFundPoolDT = new List<BasePoolDT<long>>();
    OpenServFundDT currentSelectDT;//当前选中的dt
    private string strTexBgRoot = "UI/TextureRemove/Activity/Tex_OpenServFundBg";
    private string strTexAdsRoot = "UI/TextureRemove/Activity/TexOpenServFund";

    private int iVip;
    private int iLevel;
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView()
    {
        gameObject.SetActive(true);
        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;
        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        BuyCallback.m_ccCallbackSuc = OnBuySucCallback;
        BuyCallback.m_ccCallbackFail = OnBuyFailCallback;

        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_OpenServFundPool.f_QueryBuyInfo(QueryCallback);
        Data_Pool.m_OpenServFundPool.f_QueryOpenServFundInfo(QueryCallback);
        InitUI();
        f_LoadTexture();
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        //f_GetObject("TexAds").transform.position = new Vector3(transform.position.x, f_GetObject("TexAds").transform.position.y, f_GetObject("TexAds").transform.position.z);
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;

            UITexture TexTexAds = f_GetObject("TexAds").GetComponent<UITexture>();
            Texture2D tTexAds = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexAdsRoot);
            TexTexAds.mainTexture = tTexAds;
        }
    }
    /// <summary>
    /// 初始化UI
    /// </summary>
    private void InitUI()
    {
        int vipLevel = UITool.f_GetNowVipLv();
        //去掉双倍显示
        f_GetObject("SprRightDirVip5").SetActive(false);//(vipLevel >= 5 && vipLevel < 8);
        f_GetObject("SprRightDirVip8").SetActive(false); //(vipLevel >= 8);
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        listOpenServFundPoolDT = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_OpenServFundPool.f_GetAll());
        for (int i = listOpenServFundPoolDT.Count - 1; i >= 0; i--)
        {
            OpenServFundPoolDT openServFundPoolDT = listOpenServFundPoolDT[i] as OpenServFundPoolDT;
            if (openServFundPoolDT.eOpenServFundType != EM_OpenServFundType.OpenServFund)
            {
                listOpenServFundPoolDT.RemoveAt(i);
            }
        }
        //排序
        for (int i = 0; i < listOpenServFundPoolDT.Count; i++)
        {
            OpenServFundPoolDT openServFundPoolDTI = listOpenServFundPoolDT[i] as OpenServFundPoolDT;
            for (int j = i + 1; j < listOpenServFundPoolDT.Count; j++)
            {
                OpenServFundPoolDT openServFundPoolDTJ = listOpenServFundPoolDT[j] as OpenServFundPoolDT;
                if (openServFundPoolDTJ.m_OpenServFundDT.iCondiction < openServFundPoolDTI.m_OpenServFundDT.iCondiction)
                {
                    OpenServFundPoolDT temp = openServFundPoolDTI;
                    listOpenServFundPoolDT[i] = openServFundPoolDTJ;
                    listOpenServFundPoolDT[j] = temp;
                }
            }
        }
        GameObject BtnRoot = f_GetObject("BtnRoot");
        GameObject LevelRoot = f_GetObject("LevelRoot");
        GameObject SyceeRoot = f_GetObject("SyceeRoot");
        //BtnRoot.SetActive(true);
        //LevelRoot.SetActive(true);
        //SyceeRoot.SetActive(true);
        iVip = UITool.f_GetNowVipLv();
        iLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        //展示用(写死) v5 1.5倍 v8  2.0倍
        //float times = 1f;
        //if (iVip >= 5)
        //    times = 1.5f;
        //if (iVip >= 8)
        //    times = 2.0f;
        GridUtil.f_SetGridView<BasePoolDT<long>>(f_GetObject("ItemParent"), f_GetObject("Item"), listOpenServFundPoolDT, UpdateItem);

        //for (int i = 0; i < 6; i++)
        //{
        //    OpenServFundPoolDT openServFundPoolDT = listOpenServFundPoolDT[i] as OpenServFundPoolDT;
        //    LevelRoot.transform.Find("Level" + i).GetComponent<UILabel>().text = openServFundPoolDT.m_OpenServFundDT.iCondiction.ToString();
        //    SyceeRoot.transform.Find("LabelSycee" + i).GetComponent<UILabel>().text = openServFundPoolDT.m_OpenServFundDT.iGiftCount.ToString() + (times > 1 ? "*" + times.ToString() : "");
        //    UISprite btnSprite = BtnRoot.transform.Find("BtnBuy" + i).GetComponent<UISprite>();
        //    UILabel btnTitle = BtnRoot.transform.Find("BtnBuy" + i).GetComponentInChildren<UILabel>();
        //    btnTitle.text = CommonTools.f_GetTransLanguage(1344);
        //    UITool.f_SetSpriteGray(btnSprite, true);
        //    f_RegClickEvent(btnSprite.gameObject, null);
        //    if (Data_Pool.m_OpenServFundPool.m_HasBuyOpenServFund)
        //    {
        //        if (openServFundPoolDT.m_buyTimes > 0)
        //        {
        //            btnTitle.text = CommonTools.f_GetTransLanguage(1345);
        //        }
        //        else if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= openServFundPoolDT.m_OpenServFundDT.iCondiction)//显示领取
        //        {
        //            UITool.f_SetSpriteGray(btnSprite, false);
        //            f_RegClickEvent(btnSprite.gameObject, OnGetClick, openServFundPoolDT);
        //        }
        //    }
        //}
        UpdateUIView();
    }

    private void UpdateItem(GameObject go, BasePoolDT<long> tOpenServFundPoolDT)
    {
        OpenServFundPoolDT DT = tOpenServFundPoolDT as OpenServFundPoolDT;
        Transform tran = go.transform;
        UISprite BG = tran.Find("Bg").GetComponent<UISprite>();
        UISprite Case = tran.Find("Case").GetComponent<UISprite>();
        UI2DSprite Icon = tran.Find("Icon").GetComponent<UI2DSprite>();
        UISprite Bei = tran.Find("Bei").GetComponent<UISprite>();
        GameObject Get = tran.Find("Get").gameObject;
        GameObject Geted = tran.Find("Geted").gameObject;
        UILabel Num = tran.Find("Num").GetComponent<UILabel>();
        UILabel Desc = tran.Find("Desc").GetComponent<UILabel>();

        bool isGray = true;

        AwardPoolDT tAward = new AwardPoolDT();

        tAward.f_UpdateByInfo((byte)DT.m_OpenServFundDT.iGiftTabID, DT.m_OpenServFundDT.iGiftID, DT.m_OpenServFundDT.iGiftCount);

        Icon.sprite2D = UITool.f_GetIconSprite(tAward.mTemplate.mIcon);
        Case.spriteName = UITool.f_GetImporentCase(tAward.mTemplate.mImportant);
        Desc.text = DT.m_OpenServFundDT.szActContext;
        Num.text = tAward.mTemplate.mResourceNum.ToString();
        if (iVip < 5) Bei.spriteName = "";
        else if (iVip >= 5 && iVip < 8) Bei.spriteName = "Border_bei1";
        else Bei.spriteName = "Border_bei2";
        if (Data_Pool.m_OpenServFundPool.m_HasBuyOpenServFund)
        {
            Get.SetActive(DT.m_buyTimes<=0&& iLevel>= DT.m_OpenServFundDT.iCondiction);
            Geted.SetActive(DT.m_buyTimes>0 && iLevel >= DT.m_OpenServFundDT.iCondiction);
            isGray = DT.m_buyTimes > 0;
        }
        else
        {
            Get.SetActive(false);
            Geted.SetActive(false);
        }
        f_RegClickEvent(Get, OnGetClick, DT);
        UITool.f_Set2DSpriteGray(Icon, isGray);
        UITool.f_SetSpriteGray(BG,isGray);
        UITool.f_SetSpriteGray(Case, isGray);
        UITool.f_SetSpriteGray(Bei, isGray);
        //去掉双倍显示
        Bei.gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新广告条数据
    /// </summary>
    private void UpdateUIView()
    {
        listOpenServFundPoolDT = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_OpenServFundPool.f_GetAll());
        int buyCount = 0;
        for (int i = listOpenServFundPoolDT.Count - 1; i >= 0; i--)
        {
            OpenServFundPoolDT openServFundPoolDT = listOpenServFundPoolDT[i] as OpenServFundPoolDT;
            if (openServFundPoolDT.eOpenServFundType != EM_OpenServFundType.OpenServFund)
            {
                listOpenServFundPoolDT.RemoveAt(i);
            }
            else
            {
                if (openServFundPoolDT.m_buyTimes > 0)
                    buyCount += openServFundPoolDT.m_OpenServFundDT.iGiftCount;
            }
        }
        f_GetObject("LabelVip").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1608), UITool.f_GetNowVipLv());
        f_GetObject("LabelBuyCount").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1346), Data_Pool.m_OpenServFundPool.m_buyFundCount);
        if (Data_Pool.m_OpenServFundPool.m_HasBuyOpenServFund)
        {
            f_GetObject("BtnBuy").SetActive(false);
            f_GetObject("BtnHasBuy").SetActive(true);
            f_GetObject("LabelHasGetCount").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1347), buyCount);
            f_GetObject("LabelWaitGetCount").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1348), 10000 - buyCount);
            f_GetObject("LabelHasGetCount").SetActive(true);
            f_GetObject("LabelWaitGetCount").SetActive(true);
            f_GetObject("LabelHintVipLimit").SetActive(false);
        }
        else
        {
            f_GetObject("BtnBuy").SetActive(true);
            f_GetObject("BtnHasBuy").SetActive(false);
            f_RegClickEvent(f_GetObject("BtnBuy"), OnBuyClick);
            f_GetObject("LabelHasGetCount").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1347), 0);
            f_GetObject("LabelWaitGetCount").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1348), 0);
            f_GetObject("LabelHasGetCount").SetActive(true);
            f_GetObject("LabelWaitGetCount").SetActive(true);
            f_GetObject("LabelHintVipLimit").SetActive(true);
        }
    }
    /// <summary>
    /// 点击购买基金
    /// </summary>
    private void OnBuyClick(GameObject go, object obj1, object ob2)
    {
        //Data_Pool.m_OpenServFundPool.m_HasBuyOpenServFund = true;

        if (UITool.f_GetNowVipLv() < 5)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1349));
            return;
        }
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < 1000)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1350));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_OpenServFundPool.f_BuyServFund(BuyCallback);
    }
    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnGetClick(GameObject go, object obj1, object obj2)
    {
        OpenServFundPoolDT openServFundPoolDT = obj1 as OpenServFundPoolDT;
        currentSelectDT = openServFundPoolDT.m_OpenServFundDT;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_OpenServFundPool.f_Get(EM_OpenServFundType.OpenServFund, (int)openServFundPoolDT.iId, RequestGetCallback);
    }
    #region 服务器回调
    /// <summary>
    /// 购买基金成功
    /// </summary>
    private void OnBuySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UpdateContent();
    }
    /// <summary>
    /// 购买基金失败
    /// </summary>
    private void OnBuyFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1351) + CommonTools.f_GetTransLanguage((int)obj));
    }
    /// <summary>
    /// 领取成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //更新UI显示
        UpdateContent();

        int vipLevel = UITool.f_GetNowVipLv();
        //展示用(写死) v5 1.5倍 v8  2.0倍
        float times = 1f;
        //if (vipLevel >= 5)
        //    times = 1.5f;
        //if (vipLevel >= 8)
        //    times = 2.0f;

        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)currentSelectDT.iGiftTabID, currentSelectDT.iGiftID, (int)(currentSelectDT.iGiftCount * times));
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        Data_Pool.m_OpenServFundPool.QueryOpenServFundInfoSucRedPoint(null);
    }
    private void OnGetFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1352) + CommonTools.f_GetTransLanguage((int)obj));
    }
    /// <summary>
    /// 查询成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuerySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //更新UI显示
        UpdateContent();
    }
    private void OnQueryFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1353) + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion
}

