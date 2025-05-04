using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
/// <summary>
/// 月卡界面
/// </summary>
public class ActMonthCardCtl : UIFramwork
{
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调
    private UIFramwork actPage;
    private EM_MonthCardType EM_curMonthCardType;
    private string strTexBgRoot = "UI/TextureRemove/Activity/Tex_MonthCardBg";
    private string strTexMonthCard25Root = "UI/TextureRemove/Activity/Tex_Card25Bg";
    private string TexSyceeIcon25Root = "UI/TextureRemove/Activity/Icon_Card25Sycee";
    private string strTexSyceeIcon50Root = "UI/TextureRemove/Activity/Tex_Card50Bg";
    private string TexSyceeIcon50Root = "UI/TextureRemove/Activity/Icon_Card50Sycee";
    private string TexMonthBg = "UI/TextureRemove/Activity/hd_yk_pic_d";
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView(UIFramwork activityPage)
    {
        gameObject.SetActive(true);
        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;
        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;

        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_QueryMonthCard(QueryCallback);
        UpdateContent();
        f_RegClickEvent("BtnBuyMonthCard25", OnBtnBuyMonthCard25Click);
        f_RegClickEvent("BtnGetMonthCard25", OnBtnGetMonthCard25Click);
        f_RegClickEvent("BtnBuyMonthCard50", OnBtnBuyMonthCard50Click);
        f_RegClickEvent("BtnGetMonthCard50", OnBtnGetMonthCard50Click);

        //f_GetObject("LabelTitleCard1").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1357), (glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(1) as PccaccyDT).iPccaccyNum);
        //f_GetObject("LabelTitleCard2").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1357), (glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(1) as PccaccyDT).iPccaccyNum);
        GameNameParamDT gameParam = glo_Main.GetInstance().m_SC_Pool.m_GameNameParamSC.f_GetSC((int)EM_GameNameParamType.MothCard25) as GameNameParamDT;
        ResourceCommonDT commonDT = new ResourceCommonDT();
        PccaccyDT PayDT = glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC((int)EM_GameNameParamType.MothCard25) as PccaccyDT;
        string[] awardStr = gameParam.szParam1.Split(';');
        commonDT.f_UpdateInfo(byte.Parse(awardStr[0]), int.Parse(awardStr[1]), int.Parse(awardStr[2]));
        f_GetObject("LabelAward1").GetComponent<UILabel>().text = commonDT.mResourceNum + commonDT.mName;
        f_GetObject("25PayGive").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(2248), CommonTools.f_GetTransLanguage(2294));


        //f_GetObject("LabelTitleCard3").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1358), (glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(2) as PccaccyDT).iPccaccyNum);
        GameNameParamDT gameParam2 = glo_Main.GetInstance().m_SC_Pool.m_GameNameParamSC.f_GetSC((int)EM_GameNameParamType.MothCard50) as GameNameParamDT;
        ResourceCommonDT commonDT2 = new ResourceCommonDT();
        PayDT= glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC((int)EM_GameNameParamType.MothCard50) as PccaccyDT;
        string[] awardStr2 = gameParam2.szParam1.Split(';');
        commonDT2.f_UpdateInfo(byte.Parse(awardStr2[0]), int.Parse(awardStr2[1]), int.Parse(awardStr2[2]));
        f_GetObject("LabelAward2").GetComponent<UILabel>().text = commonDT2.mResourceNum + commonDT2.mName;
        GameParamDT MonthCardDT = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameNameParamType.MonthCard) as GameParamDT;
        GameParamDT PerpetualDT = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameNameParamType.PerpetualCard) as GameParamDT;
        f_GetObject("LabelAddPro1").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(2238) + string.Format("+{0}%", MonthCardDT.iParam1 / 100);
        f_GetObject("LabelPro2").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(2238) + string.Format("+{0}%", PerpetualDT.iParam1 / 100);
        f_GetObject("LabelExp").GetComponent<UILabel>().text = string.Format("[27FA34FF]{0}%", PerpetualDT.iParam2 / 100);
        this.actPage = activityPage;
        f_GetObject("50PayGive").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(2248), CommonTools.f_GetTransLanguage(2295));
        f_LoadTexture();
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;

            UITexture TexMonthCard25 = f_GetObject("TexMonthCard25Texture").GetComponent<UITexture>();
            Texture2D tTexMonthCard25 = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexMonthCard25Root);
            TexMonthCard25.mainTexture = tTexMonthCard25;
            UITexture TexSyceeIcon25 = f_GetObject("TexSyceeIcon25").GetComponent<UITexture>();
            Texture2D tTexSyceeIcon25 = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(TexSyceeIcon25Root);
            TexSyceeIcon25.mainTexture = tTexSyceeIcon25;

            UITexture TexMonthCard50 = f_GetObject("TexMonthCard50Texture").GetComponent<UITexture>();
            Texture2D tTexMonthCard50 = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexSyceeIcon50Root);
            TexMonthCard50.mainTexture = tTexMonthCard50;
            UITexture TexSyceeIcon50 = f_GetObject("TexSyceeIcon50").GetComponent<UITexture>();
            Texture2D tTexSyceeIcon50 = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(TexSyceeIcon50Root);
            TexSyceeIcon50.mainTexture = tTexSyceeIcon50;

            UITexture TexMonthCard25Bg = f_GetObject("TexMonthCard25BgTexture").GetComponent<UITexture>();
            UITexture TexMonthCard50Bg = f_GetObject("TexMonthCard50BgTexture").GetComponent<UITexture>();
            Texture2D tTexMonthCard25Bg = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(TexMonthBg);
            TexMonthCard25Bg.mainTexture = tTexMonthCard25Bg;
            TexMonthCard50Bg.mainTexture = tTexMonthCard25Bg;
        }
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        f_GetObject("BtnBuyMonthCard25").SetActive(!Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy25);
        f_GetObject("BtnGetMonthCard25").SetActive(Data_Pool.m_ActivityCommonData.m_MonthCardIsCanGet25 && Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy25);
        f_GetObject("HasGetMonthCard25").SetActive(!Data_Pool.m_ActivityCommonData.m_MonthCardIsCanGet25 && Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy25);
        f_GetObject("BtnBuyMonthCard50").SetActive(!Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy50);
        f_GetObject("BtnGetMonthCard50").SetActive(Data_Pool.m_ActivityCommonData.m_MonthCardIsCanGet50 && Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy50);
        f_GetObject("HasGetMonthCard50").SetActive(!Data_Pool.m_ActivityCommonData.m_MonthCardIsCanGet50 && Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy50);

        if (Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy25)
            f_GetObject("TexMonthCard25").transform.Find("LabelTime").GetComponent<UILabel>().text =
                String.Format(CommonTools.f_GetTransLanguage(1359), Data_Pool.m_ActivityCommonData.remainDay25);
        else
            f_GetObject("TexMonthCard25").transform.Find("LabelTime").GetComponent<UILabel>().text = "";

        if (Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy50)
            f_GetObject("TexMonthCard50").transform.Find("LabelTime").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1360);
        //String.Format("[FFFFFF]有效期还剩[FF0059DF]{0}[FFFFFFFF]天", Data_Pool.m_ActivityCommonData.remainDay50);
        else
            f_GetObject("TexMonthCard50").transform.Find("LabelTime").GetComponent<UILabel>().text = "";
    }
    /// <summary>
    /// 充值页面返回得更新UI
    /// </summary>
    /// <param name="e"></param>
    public void f_ViewResume(object e)
    {
        if (gameObject.activeInHierarchy)
        {
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_ActivityCommonData.f_QueryMonthCard(QueryCallback);
        }
    }
    /// <summary>
    /// 点击购买月卡25(前往购买)
    /// </summary>
    private void OnBtnBuyMonthCard25Click(GameObject go, object obj1, object ob2)
    {
        //Data_Pool.m_ActivityCommonData.f_BuyMonthCard(EM_MonthCardType.Card25, BuyCallback);
        ccUIHoldPool.GetInstance().f_Hold(actPage);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.Recharge);
    }
    /// <summary>
    /// 领取月卡25奖励
    /// </summary>
    private void OnBtnGetMonthCard25Click(GameObject go, object obj1, object ob2)
    {
        EM_curMonthCardType = EM_MonthCardType.Card25;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_GetMonthCardSycee(EM_MonthCardType.Card25, RequestGetCallback);
    }

    /// <summary>
    /// 点击购买月卡50
    /// </summary>
    private void OnBtnBuyMonthCard50Click(GameObject go, object obj1, object ob2)
    {
        //Data_Pool.m_ActivityCommonData.f_BuyMonthCard(EM_MonthCardType.Card50, BuyCallback);
        ccUIHoldPool.GetInstance().f_Hold(actPage);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.Recharge);
    }
    /// <summary>
    /// 领取月卡50奖励
    /// </summary>
    private void OnBtnGetMonthCard50Click(GameObject go, object obj1, object ob2)
    {
        EM_curMonthCardType = EM_MonthCardType.Card50;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_GetMonthCardSycee(EM_MonthCardType.Card50, RequestGetCallback);
    }
    #region 服务器回调
    /// <summary>
    /// 领取成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        switch (EM_curMonthCardType)
        {
            case EM_MonthCardType.Card25:
                Data_Pool.m_ActivityCommonData.m_MonthCardIsCanGet25 = false;
                AwardPoolDT item1 = new AwardPoolDT();
                item1.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee, 200);
                awardList.Add(item1);
                f_GetObject("TexMonthCard25").transform.Find("LabelTime").GetComponent<UILabel>().text =
                String.Format(CommonTools.f_GetTransLanguage(1359), Data_Pool.m_ActivityCommonData.remainDay25);
                break;
            case EM_MonthCardType.Card50:
                Data_Pool.m_ActivityCommonData.m_MonthCardIsCanGet50 = false;
                AwardPoolDT item2 = new AwardPoolDT();
                item2.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee, 500);
                awardList.Add(item2);
                f_GetObject("TexMonthCard50").transform.Find("LabelTime").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1360);
                //String.Format("[FFFFFF]有效期还剩[FF0059DF]{0}[FFFFFFFF]天", Data_Pool.m_ActivityCommonData.remainDay50);
                break;
        }
        Data_Pool.m_ActivityCommonData.f_QueryMonthCard(QueryCallback);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        //更新UI显示
        UpdateContent();
    }
    private void OnGetFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1361) + CommonTools.f_GetTransLanguage((int)obj));
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1362) + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion
}

