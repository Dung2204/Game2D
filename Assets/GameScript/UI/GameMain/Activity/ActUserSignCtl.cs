using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 用户每日签到
/// </summary>
public class ActUserSignCtl : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT RequestUserSignCallback = new SocketCallbackDT();//签到回调
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();
    private List<SignedDT> listSignDT = new List<SignedDT>();
    SignedDT currentSignedDT;//当前点击签到的signDT
    private string strTexBgRoot = "UI/TextureRemove/Activity/Tex_UserSignBg";
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
        RequestUserSignCallback.m_ccCallbackSuc = OnUserSignSucCallback;
        RequestUserSignCallback.m_ccCallbackFail = OnUserSignFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        if (!Data_Pool.m_SignPool.f_RequestIsSignToday(OnRequestSignInfoCallback))
        {
            UpdateContent();
        }
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
        }
    }
    private void UpdateContent()
    {
        listSignDT = Data_Pool.m_SignPool.f_GetSignData(GetSignPoolDT(SignType.DaySign).signSubType);
        listContent.Clear();
        for (int i = 1; i <= listSignDT.Count; i++)//listSignDT.Count
        {
            BasePoolDT<long> item = new BasePoolDT<long>();
            item.iId = i;
            listContent.Add(item);
        }
        f_RegClickEvent("BtnSign", OnUserSignEmpty);//清空事件
        if (_contentWrapComponet == null)
        {
            _contentWrapComponet = new UIWrapComponent(180, 1, 832, listContent.Count, f_GetObject("GridContentParent"), f_GetObject("ContentSignItem"), listContent, OnContentItemUpdate, null);
        }
        _contentWrapComponet.f_ResetView();
        _contentWrapComponet.f_UpdateList(listContent);
        int signedCount = GetSignPoolDT(SignType.DaySign).signedCount;
        signedCount = signedCount < 0 ? 0 : signedCount;
        signedCount = signedCount >= listContent.Count ? listContent.Count - 1 : signedCount;
        int count =
            Mathf.CeilToInt(f_GetObject("GridContentParent").transform.parent.GetComponent<UIPanel>().GetViewSize().x / 188 + 0.5f);
        _contentWrapComponet.f_ViewGotoRealIdx(signedCount, count);
        UITool.f_OpenOrCloseWaitTip(false);
        _contentWrapComponet.f_UpdateView();
    }
    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        ActUserSignItemCtl actUserSignItemCtl = item.GetComponent<ActUserSignItemCtl>();
        SignedDT signedDT = listSignDT[(int)data.iId - 1];

        string vipDoubleStr = string.Format(CommonTools.f_GetTransLanguage(1293), signedDT.iVipRange);
        vipDoubleStr = signedDT.iVipRange == 0 ? "" : vipDoubleStr;

        int signedCount = GetSignPoolDT(SignType.DaySign).signedCount;
        bool isGetToday = GetSignPoolDT(SignType.DaySign).isSignToday;
        EM_AwardGetState awardGetState = EM_AwardGetState.Lock;

        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)signedDT.iAwardType, signedDT.iAwardID, signedDT.iAwardNum);

        if (data.iId <= signedCount)//显示已领取
        {
            awardGetState = EM_AwardGetState.AlreadyGet;
        }
        else if (data.iId == signedCount + 1 && !isGetToday)//可领取
        {
            awardGetState = EM_AwardGetState.CanGet;
            f_RegClickEvent(actUserSignItemCtl.BtnGet.gameObject, OnUserSignClick, data, actUserSignItemCtl);
            f_RegClickEvent("BtnSign", OnUserSignClick, data, actUserSignItemCtl);
        }
        if (awardGetState != EM_AwardGetState.CanGet)
        {
            f_RegClickEvent(actUserSignItemCtl.BtnGet.gameObject, OnAwardIconClick, signedDT, awardGetState);
        }
        
        actUserSignItemCtl.SetData((int)data.iId, signedDT.iAwardType, signedDT.iAwardID, signedDT.iAwardNum, 
            vipDoubleStr, awardGetState, data.iId == signedCount + 1 && !isGetToday);
       
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        f_RegClickEvent("BtnSign", OnUserSignEmpty);//清空事件
        SignedDT signedDT = obj1 as SignedDT;
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)signedDT.iAwardType, signedDT.iAwardID, signedDT.iAwardNum);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    private SignPoolDT GetSignPoolDT(SignType signType)
    {
        return Data_Pool.m_SignPool.f_GetForId((int)signType) as SignPoolDT;
    }
    private void OnUserSignEmpty(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1294));
    }
    /// <summary>
    /// 点击领取/签到按钮事件
    /// </summary>
    private void OnUserSignClick(GameObject go, object obj1, object obj2)
    {
        f_RegClickEvent("BtnSign", OnUserSignEmpty);//清空事件
        BasePoolDT<long> data = obj1 as BasePoolDT<long>;
        ActUserSignItemCtl actUserSignItemCtl = obj2 as ActUserSignItemCtl;
        Debug.Log(CommonTools.f_GetTransLanguage(1295) + data.iId);
        currentSignedDT = listSignDT[(int)data.iId - 1];
        Data_Pool.m_SignPool.f_GetShopRandInfo(SignType.DaySign, RequestUserSignCallback);
        actUserSignItemCtl.playTween();
        UITool.f_OpenOrCloseWaitTip(true);
    }
    #region 每日签到回调
    /// <summary>
    /// 每日签到成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnUserSignSucCallback(object obj)
    {
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "签到成功，奖励已发放！");
        //更新UI显示
        _contentWrapComponet.f_UpdateView();
        int playerVip = UITool.f_GetNowVipLv();
        bool isDouble = (currentSignedDT.iVipRange > 0 && playerVip >= currentSignedDT.iVipRange) ? true : false;
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)currentSignedDT.iAwardType, currentSignedDT.iAwardID, currentSignedDT.iAwardNum * (isDouble ? 2 : 1));
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnUserSignFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1296) + CommonTools.f_GetTransLanguage((int)obj));
        Debug.Log(CommonTools.f_GetTransLanguage(1297) + teMsgOperateResult.ToString());
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion

    private void OnRequestSignInfoCallback(object obj)
    {
        Debug.Log(CommonTools.f_GetTransLanguage(1298));
        UpdateContent();
    }
}
