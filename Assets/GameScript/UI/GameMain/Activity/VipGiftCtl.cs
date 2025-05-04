using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
/// <summary>
/// vip礼包活动
/// </summary>
public class VipGiftCtl : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT InfoCallback = new SocketCallbackDT();//信息回调
    private SocketCallbackDT RequestUserGetCallback = new SocketCallbackDT();//领取回调
    private VipGiftPoolDT currentPoolDT;
    private string strTexAdsRoot = "UI/TextureRemove/Activity/TexOpenServFund";
    private UIFramwork actPage;
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
        InfoCallback.m_ccCallbackSuc = OnInfoSucCallback;
        InfoCallback.m_ccCallbackFail = OnInfoFailCallback;
        RequestUserGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestUserGetCallback.m_ccCallbackFail = OnGetFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_VipGiftPool.f_RequestInfo(InfoCallback);

        int vipExp = (int)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip);
        int tVipLv = 0;
        int tNeedExp = 0;
        UITool.f_GetVipLvAndNeedExp(vipExp, ref tVipLv, ref tNeedExp);
        f_GetObject("VipLevel").GetComponent<UILabel>().text = tVipLv.ToString();
        f_GetObject("SliderVipExp").GetComponent<UISlider>().value = (float)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip) / (float)tNeedExp;

        f_LoadTexture();
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载广告图
        UITexture TexAds = f_GetObject("TexAds").GetComponent<UITexture>();
        if (TexAds.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexAdsRoot);
            TexAds.mainTexture = tTexture2D;
        }
        //TexAds.transform.position = new Vector3(TexAds.transform.position.x, TexAds.transform.position.y, TexAds.transform.position.z);
    }
    /// <summary>
    /// 从充值页面返回
    /// </summary>
    /// <param name="e"></param>
    public void f_ViewResume(object e)
    {
        Data_Pool.m_VipGiftPool.f_RequestInfo(InfoCallback);
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        Data_Pool.m_VipGiftPool.CheckRedPoint();

        int vipExp = (int)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip);
        int tVipLv = 0;
        int tNeedExp = 0;
        UITool.f_GetVipLvAndNeedExp(vipExp, ref tVipLv, ref tNeedExp);

        List<BasePoolDT<long>> listData = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_VipGiftPool.f_GetAll());
        for (int i = listData.Count - 1; i >= 0; i--)
        {
            VipGiftPoolDT poolDT = listData[i] as VipGiftPoolDT;
            //if ((tVipLv + 1) < poolDT.mVipGiftDT.iId)
            //{
            //    listData.RemoveAt(i);
            //}
            if (tVipLv > poolDT.mVipGiftDT.iId || (tVipLv + 1) < poolDT.mVipGiftDT.iId)
            {
                listData.RemoveAt(i);
            }
        }
        if (_contentWrapComponet == null)
        {
            _contentWrapComponet = new UIWrapComponent(230, 1, 1400, 8, f_GetObject("GridContentParent"), f_GetObject("ContentItem"),
                listData, OnContentItemUpdate, null);
        }
        _contentWrapComponet.f_ResetView();
        _contentWrapComponet.f_UpdateList(listData);
        _contentWrapComponet.f_UpdateView();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        VipGiftItem vipGiftItem = item.GetComponent<VipGiftItem>();

        VipGiftPoolDT poolDT = data as VipGiftPoolDT;

        int vipExp = (int)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip);
        int tVipLv = 0;
        int tNeedExp = 0;
        UITool.f_GetVipLvAndNeedExp(vipExp, ref tVipLv, ref tNeedExp);

        vipGiftItem.InitData(poolDT);

        //vipGiftItem.mBtnGet.SetActive(false);
        //vipGiftItem.mBtnHasGet.SetActive(false);
        //vipGiftItem.mBtnGetGay.SetActive(tVipLv > poolDT.mVipGiftDT.iId);
        vipGiftItem.mBtnRecharge.SetActive(tVipLv < poolDT.mVipGiftDT.iId);
        vipGiftItem.mBtnGet.SetActive(!poolDT.mToDayGet&& tVipLv >= poolDT.mVipGiftDT.iId);
        vipGiftItem.mBtnHasGet.SetActive(poolDT.mToDayGet);
        //if (tVipLv == poolDT.mVipGiftDT.iId)
        //{

        //}

        f_RegClickEvent(vipGiftItem.mBtnGet, OnGetClick, poolDT);
        f_RegClickEvent(vipGiftItem.mBtnRecharge, OnUserRechargeClick);
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT commonData = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }

    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnGetClick(GameObject go, object obj1, object obj2)
    {
        currentPoolDT = obj1 as VipGiftPoolDT;
        Data_Pool.m_VipGiftPool.f_GetAward((byte)currentPoolDT.iId, RequestUserGetCallback);
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
    #region 服务器回调
    /// <summary>
    /// 信息成功回调
    /// </summary>
    private void OnInfoSucCallback(object obj)
    {
        UpdateContent();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 信息失败回调
    /// </summary>
    private void OnInfoFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1378) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 领取成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        //更新UI显示
        _contentWrapComponet.f_UpdateView();

        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { CommonTools.f_GetListAwardPoolDT(currentPoolDT.mVipGiftDT.szAward) });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnGetFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1379) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
}