using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// 首充
/// </summary>
public class FirstRechargeCtl : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//请求领取回调
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();
    FirstRechargePoolDT curSelectPoolDT;
    private bool isInitView = false;
    private ccUIBase actPage;
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView(ccUIBase actPage)
    {
        gameObject.SetActive(true);
        isInitView = false;
        this.actPage = actPage;

        f_LoadTexture();

        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);

        Data_Pool.m_FirstRechargePool.f_QueryInfo(QueryCallback);
        
        f_GetObject("TotalNum").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1380) + Data_Pool.m_RechargePool.f_GetAllRechageMoney();
    }
    private string strTexBgRoot = "UI/TextureRemove/Activity/Tex_FirstRechargeBg";
    private string strTexBg2Root = "UI/TextureRemove/Activity/Tex_FRBGTop";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
        TexBg.mainTexture = tTexture2D;
        UITexture TexBg2 = f_GetObject("TexBg2").GetComponent<UITexture>();
        Texture2D tTexture2D2 = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBg2Root);
        TexBg2.mainTexture = tTexture2D2;
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        if (!isInitView)
        {
            isInitView = true;
            if (_contentWrapComponet == null)
            {
                _contentWrapComponet = new UIWrapComponent(270, 1, 1400, 7, f_GetObject("GridContentParent"), f_GetObject("Item"),
                    Data_Pool.m_FirstRechargePool.f_GetAll(), OnContentItemUpdate, null);
            }
            _contentWrapComponet.f_ResetView();
        }
        _contentWrapComponet.f_UpdateView();
        UITool.f_OpenOrCloseWaitTip(false);

    }
    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        FirstRechargeItem firstRechargeItem = item.GetComponent<FirstRechargeItem>();

        FirstRechargePoolDT poolDT = data as FirstRechargePoolDT;
        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(poolDT.m_FirstRechargeDT.szAward);
        int rechargeAllCount = Data_Pool.m_RechargePool.f_GetAllRechageMoney();
        firstRechargeItem.f_SetData(rechargeAllCount, poolDT, listCommonDT);

        EM_BoxGetState state = EM_BoxGetState.Lock;
        if (poolDT.mGetTimes > 0)
            state = EM_BoxGetState.AlreadyGet;
        else
        {
            if (rechargeAllCount >= poolDT.m_FirstRechargeDT.iCondition)
            {
                state = EM_BoxGetState.CanGet;
            }
        }
        firstRechargeItem.mBtnGet.SetActive(state == EM_BoxGetState.CanGet);
        firstRechargeItem.mBtnHasGet.SetActive(state == EM_BoxGetState.AlreadyGet);
        firstRechargeItem.mBtnGoRecharge.SetActive(state == EM_BoxGetState.Lock);

        f_RegClickEvent(firstRechargeItem.mBtnGoRecharge, OnGotoClick);
        f_RegClickEvent(firstRechargeItem.mBtnGet, OnBtnGetClick, poolDT, state);
    }
    /// <summary>
    /// 点击前往
    /// </summary>
    private void OnGotoClick(GameObject go, object obj1, object obj2)
    {
        UITool.f_GotoPage(this.actPage, UINameConst.ShowVip, (int)ShowVip.EM_PageIndex.Recharge);
    }
    /// <summary>
    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnBtnGetClick(GameObject go, object obj1, object obj2)
    {
        if ((EM_BoxGetState)obj2 != EM_BoxGetState.CanGet)
        {
            return;
        }
        FirstRechargePoolDT poolDT = obj1 as FirstRechargePoolDT;
        curSelectPoolDT = poolDT;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_FirstRechargePool.f_GetAward((int)poolDT.iId, RequestGetCallback);
    }
    #region 回调
    /// <summary>
    /// 查询成功回调
    /// </summary>
    private void OnQuerySucCallback(object obj)
    {
        UpdateContent();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnQueryFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1381) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        Data_Pool.m_FirstRechargePool.f_CheckRedPoint();
        UITool.f_OpenOrCloseWaitTip(false);
        //更新UI显示
        _contentWrapComponet.f_UpdateView();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { CommonTools.f_GetListAwardPoolDT(curSelectPoolDT.m_FirstRechargeDT.szAward) });
    }
    private void OnGetFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1382) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
}
