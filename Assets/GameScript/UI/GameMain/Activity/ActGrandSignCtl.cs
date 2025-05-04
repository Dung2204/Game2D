using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 豪华签到
/// </summary>
public class ActGrandSignCtl : UIFramwork {
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT RequestUserSignCallback = new SocketCallbackDT();//签到回调
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();
    private List<SignedDT> listSignDT = new List<SignedDT>();
    private SignedDT currentSignedDT;
    private UIFramwork actPage;
    private string strTexAdsRoot = "UI/TextureRemove/Activity/TexOpenServFund";
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView(UIFramwork actPage)
    {
        f_LoadTexture();
        gameObject.SetActive(true);
        this.actPage = actPage;
        RequestUserSignCallback.m_ccCallbackSuc = OnSignSucCallback;
        RequestUserSignCallback.m_ccCallbackFail = OnSignFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        if (!Data_Pool.m_SignPool.f_RequestIsSignToday(OnRequestSignInfoCallback))
            UpdateContent();
        
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载广告图
        UITexture TexAds = f_GetObject("TexAds").GetComponent<UITexture>();
        //TexAds.transform.position =  new Vector3(transform.position.x, TexAds.transform.position.y, TexAds.transform.position.z);
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
        Data_Pool.m_SignPool.isSeeGrandSignPage = true;
        Data_Pool.m_SignPool.CheckRedPoint();

        string freshTime = Data_Pool.m_SignPool.GrandSignResetTimeStamp.ToString();
        int year = int.Parse(freshTime.Substring(0, 4));//20181224
        int month = int.Parse(freshTime.Substring(4, 2));
        int day = int.Parse(freshTime.Substring(6, 2));

        f_GetObject("UpdateTime").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1299), year, month, day);

        listSignDT = Data_Pool.m_SignPool.f_GetSignData(GetSignPoolDT(SignType.GrandSign).signSubType);
        listContent.Clear();
        for (int i = 1; i <= listSignDT.Count; i++)
        {
            BasePoolDT<long> item = new BasePoolDT<long>();
            item.iId = i;
            listContent.Add(item);
        }
        if (_contentWrapComponet == null)
        {
            _contentWrapComponet = new UIWrapComponent(230, 1, 1400, 8, f_GetObject("GridContentParent"), f_GetObject("ContentGrandSignItem"), listContent, OnContentItemUpdate, null);
        }
        _contentWrapComponet.f_ResetView();
        _contentWrapComponet.f_UpdateList(listContent);
        _contentWrapComponet.f_UpdateView();
        int signedCount = GetSignPoolDT(SignType.GrandSign).signedCount - 1;
        signedCount = signedCount < 0 ? 0 : signedCount;
        signedCount = signedCount >= listContent.Count ? listContent.Count - 1 : signedCount;
        _contentWrapComponet.f_ViewGotoRealIdx(signedCount, 1);
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        ActUserGrandSignItemCtl actUserGrandSignItemCtl = item.GetComponent<ActUserGrandSignItemCtl>();
        SignedDT signedDTGrand = listSignDT[(int)data.iId - 1];
        actUserGrandSignItemCtl.f_SetData(6, (int)data.iId, signedDTGrand.iGrandAwardType1,signedDTGrand.iGrandAwardID1, signedDTGrand.iGrandAwardNum1,
            signedDTGrand.iGrandAwardType2,signedDTGrand.iGrandAwardID2,signedDTGrand.iGrandAwardNum2, signedDTGrand.iGrandAwardType3, signedDTGrand.iGrandAwardID3,
            signedDTGrand.iGrandAwardNum3, signedDTGrand.iGrandAwardType4, signedDTGrand.iGrandAwardID4, signedDTGrand.iGrandAwardNum4);
        f_RegClickEvent(actUserGrandSignItemCtl.SprAward1.gameObject, OnAwardIconClick, signedDTGrand, 1);
        f_RegClickEvent(actUserGrandSignItemCtl.SprAward2.gameObject, OnAwardIconClick, signedDTGrand, 2);
        f_RegClickEvent(actUserGrandSignItemCtl.SprAward3.gameObject, OnAwardIconClick, signedDTGrand, 3);
        f_RegClickEvent(actUserGrandSignItemCtl.SprAward4.gameObject, OnAwardIconClick, signedDTGrand, 4);
        int grandSignedCount = GetSignPoolDT(SignType.GrandSign).signedCount;
        bool isSignedToday = GetSignPoolDT(SignType.GrandSign).isSignToday;
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
                if (Data_Pool.m_RechargePool.f_GetAllRechageMoneyToday() >= 6)
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
    private SignPoolDT GetSignPoolDT(SignType signType)
    {
        return Data_Pool.m_SignPool.f_GetForId((int)signType) as SignPoolDT;
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        SignedDT signedDT = obj1 as SignedDT;
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
    /// 点击豪华签到领取按钮事件
    /// </summary>
    private void OnUGrandSignClick(GameObject go, object obj1, object obj2)
    {
        BasePoolDT<long> data = obj1 as BasePoolDT<long>;
        Debug.Log(CommonTools.f_GetTransLanguage(1300) + data.iId);
        currentSignedDT = listSignDT[(int)data.iId - 1];
        Data_Pool.m_SignPool.f_GetShopRandInfo(SignType.GrandSign, RequestUserSignCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    /// <summary>
    /// 从充值页面返回
    /// </summary>
    /// <param name="e"></param>
    public void f_ViewResume(object e)
    {
        if (!Data_Pool.m_SignPool.f_RequestIsSignToday(OnRequestSignInfoCallback))
            UpdateContent();
    }
    /// <summary>
    /// 点击充值
    /// </summary>
    private void OnUserRechargeClick(GameObject go,object obj1,object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(actPage);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.Recharge);
    }
    #region 每日签到回调
    /// <summary>
    /// 签到成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnSignSucCallback(object obj)
    {
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "签到成功，奖励已发放！");
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
        AwardPoolDT item4 = new AwardPoolDT();
        item4.f_UpdateByInfo((byte)currentSignedDT.iGrandAwardType4, currentSignedDT.iGrandAwardID4, currentSignedDT.iGrandAwardNum4);
        awardList.Add(item4);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnSignFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1301) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion

    private void OnRequestSignInfoCallback(object obj)
    {
        Debug.Log(CommonTools.f_GetTransLanguage(1302));
        UpdateContent();
    }
}

