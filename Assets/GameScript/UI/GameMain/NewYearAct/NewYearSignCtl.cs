using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
/// <summary>
/// 新年豪华签到
/// </summary>
public class NewYearSignCtl : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT RequestInfoCallback = new SocketCallbackDT();//请求信息回调
    private SocketCallbackDT RequestUserSignCallback = new SocketCallbackDT();//签到回调
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();

    private List<NewYearSignDT> listSignDT = new List<NewYearSignDT>();
    private NewYearSignDT currentSignedDT;
    private UIFramwork actPage;
    private string strTexAdsRoot = "UI/TextureRemove/Activity/Tex_ActAd2";
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView(UIFramwork actPage)
    {
        gameObject.SetActive(true);
        this.actPage = actPage;

        listSignDT.Clear();
        List<NBaseSCDT> listSignSC = glo_Main.GetInstance().m_SC_Pool.m_NewYearSignSC.f_GetAll();
        for (int i = 0; i < listSignSC.Count; i++)
        {
            NewYearSignDT signedDT = listSignSC[i] as NewYearSignDT;
            listSignDT.Add(signedDT);
        }

        RequestInfoCallback.m_ccCallbackSuc = OnInfoSucCallback;
        RequestInfoCallback.m_ccCallbackFail = OnInfoFailCallback;
        RequestUserSignCallback.m_ccCallbackSuc = OnSignSucCallback;
        RequestUserSignCallback.m_ccCallbackFail = OnSignFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_NewYearSignPool.f_RequestSignInfo(RequestInfoCallback);
        f_LoadTexture();

        GameParamDT paramDT = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearSign) as GameParamDT);
        bool isOpen = CommonTools.f_CheckTime(paramDT.iParam1.ToString(), paramDT.iParam2.ToString());
        if (isOpen)
        {
            DateTime startTime = CommonTools.f_GetDateTimeByTimeStr(paramDT.iParam1.ToString());
            int timeNow = GameSocket.GetInstance().f_GetServerTime();
            long timeStart = ccMath.DateTime2time_t(startTime);
            int freshDay = paramDT.iParam3;
            while(timeNow > timeStart)
            {
                timeStart += freshDay * 24 * 60 * 60;
            }
            DateTime timeEndTime = ccMath.time_t2DateTime(timeStart);

            f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1473), timeEndTime.Day, timeEndTime.Month, timeEndTime.Year);
        }
        else
        {
            f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1472);
        }
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载广告图
        UITexture TexAds = f_GetObject("TexAds").GetComponent<UITexture>();
       // TexAds.transform.position = new Vector3(transform.position.x, TexAds.transform.position.y, TexAds.transform.position.z);
        if (TexAds.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexAdsRoot);
            TexAds.mainTexture = tTexture2D;
        }
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        Data_Pool.m_NewYearSignPool.isSeeGrandSignPage = true;

        listContent.Clear();
        for (int i = 1; i <= listSignDT.Count; i++)
        {
            BasePoolDT<long> item = new BasePoolDT<long>();
            item.iId = i;
            listContent.Add(item);
        }
        if (_contentWrapComponet == null)
        {
            _contentWrapComponet = new UIWrapComponent(240, 1, 1400, 8, f_GetObject("GridContentParent"), f_GetObject("ContentGrandSignItem"), listContent, OnContentItemUpdate, null);
        }
        _contentWrapComponet.f_ResetView();
        int signedCount = Data_Pool.m_NewYearSignPool.signedCount;
        signedCount = signedCount < 0 ? 0 : signedCount;
        signedCount = signedCount >= listContent.Count ? listContent.Count - 1 : signedCount;
        _contentWrapComponet.f_ViewGotoRealIdx(signedCount, 1);
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        ActUserGrandSignItemCtl actUserGrandSignItemCtl = item.GetComponent<ActUserGrandSignItemCtl>();
        NewYearSignDT signedDTGrand = listSignDT[(int)data.iId - 1];
        actUserGrandSignItemCtl.f_SetData(signedDTGrand.iRechargeNum, (int)data.iId, signedDTGrand.iGrandAwardType1, signedDTGrand.iGrandAwardID1, signedDTGrand.iGrandAwardNum1,
            signedDTGrand.iGrandAwardType2, signedDTGrand.iGrandAwardID2, signedDTGrand.iGrandAwardNum2, signedDTGrand.iGrandAwardType3, signedDTGrand.iGrandAwardID3,
            signedDTGrand.iGrandAwardNum3, signedDTGrand.iGrandAwardType4, signedDTGrand.iGrandAwardID4, signedDTGrand.iGrandAwardNum4);
        f_RegClickEvent(actUserGrandSignItemCtl.SprAward1.gameObject, OnAwardIconClick, signedDTGrand, 1);
        f_RegClickEvent(actUserGrandSignItemCtl.SprAward2.gameObject, OnAwardIconClick, signedDTGrand, 2);
        f_RegClickEvent(actUserGrandSignItemCtl.SprAward3.gameObject, OnAwardIconClick, signedDTGrand, 3);
        f_RegClickEvent(actUserGrandSignItemCtl.SprAward4.gameObject, OnAwardIconClick, signedDTGrand, 4);

        int grandSignedCount = Data_Pool.m_NewYearSignPool.signedCount;
        bool isSignedToday = CommonTools.f_CheckSameDay(GameSocket.GetInstance().f_GetServerTime(), Data_Pool.m_NewYearSignPool.lastSignTime);
        item.Find("BtnReCharge").gameObject.SetActive(false);
        item.Find("BtnHasGet").gameObject.SetActive(false);
        item.Find("BtnWaitGet").gameObject.SetActive(false);
        if (data.iId <= grandSignedCount)//显示已领取
        {
            item.Find("BtnHasGet").gameObject.SetActive(true);
            actUserGrandSignItemCtl.BtnRecharge.gameObject.SetActive(false);
            actUserGrandSignItemCtl.BtnGet.gameObject.SetActive(false);
        }
        else if (data.iId > grandSignedCount + 1)//显示领取灰显
        {
            item.Find("BtnWaitGet").gameObject.SetActive(true);
        }
        else//如果今日没领取显示领取，并可以点击
        {
            if (isSignedToday)
            {
                item.Find("BtnWaitGet").gameObject.SetActive(true);
            }
            else
            {
                if (Data_Pool.m_RechargePool.f_GetAllRechageMoneyToday() >= signedDTGrand.iRechargeNum)
                {
                    actUserGrandSignItemCtl.BtnRecharge.gameObject.SetActive(false);
                    actUserGrandSignItemCtl.BtnGet.gameObject.SetActive(true);
                    f_RegClickEvent(actUserGrandSignItemCtl.BtnGet.gameObject, OnUGrandSignClick, data);
                }
                else
                {
                    actUserGrandSignItemCtl.BtnRecharge.gameObject.SetActive(true);
                    actUserGrandSignItemCtl.BtnGet.gameObject.SetActive(false);
                    f_RegClickEvent(actUserGrandSignItemCtl.BtnRecharge.gameObject, OnUserRechargeClick);
                }
            }
        }
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        NewYearSignDT signedDT = obj1 as NewYearSignDT;
        int type = 0;
        int id = 0;
        int num = 0;
        switch ((int)obj2)
        {
            case 1:
                type = signedDT.iGrandAwardType1;
                id = signedDT.iGrandAwardID1;
                num = signedDT.iGrandAwardNum1;
                break;
            case 2:
                type = signedDT.iGrandAwardType2;
                id = signedDT.iGrandAwardID2;
                num = signedDT.iGrandAwardNum2;
                break;
            case 3:
                type = signedDT.iGrandAwardType3;
                id = signedDT.iGrandAwardID3;
                num = signedDT.iGrandAwardNum3;
                break;
            case 4:
                type = signedDT.iGrandAwardType4;
                id = signedDT.iGrandAwardID4;
                num = signedDT.iGrandAwardNum4;
                break;
        }
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)type, id, num);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }

    /// <summary>
    /// 点击签到领取按钮事件
    /// </summary>
    private void OnUGrandSignClick(GameObject go, object obj1, object obj2)
    {
        GameParamDT gameParamDT = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearSign) as GameParamDT);
        if (!CommonTools.f_CheckTime(gameParamDT.iParam1.ToString(), gameParamDT.iParam2.ToString()))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1474));
            return;
        }
        BasePoolDT<long> data = obj1 as BasePoolDT<long>;
Debug.Log("Click register to load" + data.iId);
        currentSignedDT = listSignDT[(int)data.iId - 1];
        Data_Pool.m_NewYearSignPool.f_GetSign(RequestUserSignCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    /// <summary>
    /// 点击充值
    /// </summary>
    private void OnUserRechargeClick(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(actPage);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.Recharge);
    }
    #region 每日签到回调
    /// <summary>
    /// 信息回调
    /// </summary>
    private void OnInfoSucCallback(object obj)
    {
        UpdateContent();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnInfoFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1494) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 签到成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnSignSucCallback(object obj)
    {
        //更新UI显示
        _contentWrapComponet.f_UpdateView();

        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)currentSignedDT.iGrandAwardType1, currentSignedDT.iGrandAwardID1, currentSignedDT.iGrandAwardNum1);
        awardList.Add(item1);
        AwardPoolDT item2 = new AwardPoolDT();
        item2.f_UpdateByInfo((byte)currentSignedDT.iGrandAwardType2, currentSignedDT.iGrandAwardID2, currentSignedDT.iGrandAwardNum2);
        awardList.Add(item2);
        AwardPoolDT item3 = new AwardPoolDT();
        item3.f_UpdateByInfo((byte)currentSignedDT.iGrandAwardType3, currentSignedDT.iGrandAwardID3, currentSignedDT.iGrandAwardNum3);
        awardList.Add(item3);

        if (currentSignedDT.iGrandAwardType4 > 0)
        {
            AwardPoolDT item4 = new AwardPoolDT();
            item4.f_UpdateByInfo((byte)currentSignedDT.iGrandAwardType4, currentSignedDT.iGrandAwardID4, currentSignedDT.iGrandAwardNum4);
            awardList.Add(item4);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnSignFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1495) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
}

