using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;

public class MutiRechargeCtl : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//请求领取回调
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();
    MutiRechargePoolDT curSelectPoolDT;
    private bool isInitView = false;
    private ccUIBase actPage;
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView(ccUIBase actPage,int timeStart, int timeEnd)
    {
        gameObject.SetActive(true);
        isInitView = false;
        this.actPage = actPage;

        f_LoadTexture();
        if (timeEnd == 0)
        {
            f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1472);
            return;
        }

        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        
        Data_Pool.m_MutiRechargePool.f_QueryInfo(QueryCallback);

        UpdateContent();

        GameObject ModelPoint = f_GetObject("ModelPoint");
        if (ModelPoint.transform.Find("Model") == null)
        {//1309//1204
            UITool.f_GetStatelObject(1318, ModelPoint.transform, Vector3.zero, Vector3.zero, 18, "Model", 200);
        }

        //string str = timeEnd.ToString();
        //int year = int.Parse(str.Substring(0, 4));//20181224
        //int month = int.Parse(str.Substring(4, 2));
        //int day = int.Parse(str.Substring(6, 2));
        //long timeStart1 = timeStart;//(CommonTools.f_GetDateTimeByTimeStr(timeStart.ToString()));
        //long timeEnd1 = timeEnd;//(CommonTools.f_GetDateTimeByTimeStr(timeEnd.ToString()));
        DateTime EndTime = ccMath.time_t2DateTime(timeEnd);

        f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1473), EndTime.Day, EndTime.Month, EndTime.Year);
        //TsuComment //f_GetObject("TotalNum").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1380) + Data_Pool.m_RechargePool.f_GetAllRechageMoney(timeStart, timeEnd);
        f_GetObject("TotalNum").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1380) + Data_Pool.m_RechargePool.f_GetAllRechageMoney_TsuFunc(timeStart, timeEnd); //TsuCode
    }
    private string strTexBgRoot = "UI/TextureRemove/NewYearActivity/TexTotalWastBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        //if (TexBg.mainTexture == null)
        //{
        //    Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
        //    TexBg.mainTexture = tTexture2D;
        //}
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        //List<BasePoolDT<long>> listPoolDT = Data_Pool.m_RechargePool.f_get
        if (!isInitView)
        {
            isInitView = true;
            listContent = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_MutiRechargePool.f_GetAll());
            for (int i = listContent.Count - 1; i >= 0; i--)
            {
                MutiRechargePoolDT poolDT = listContent[i] as MutiRechargePoolDT;
                //if (!CommonTools.f_CheckActIsOpenForOpenSeverTime(poolDT.m_NewYearMultiRechargeAwardDT.iTimeBeg, poolDT.m_NewYearMultiRechargeAwardDT.iTimeEnd)) //(!CommonTools.f_CheckTime(poolDT.m_NewYearMultiRechargeAwardDT.iTimeBeg.ToString(), poolDT.m_NewYearMultiRechargeAwardDT.iTimeEnd.ToString()))
                if (!CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(poolDT.m_NewYearMultiRechargeAwardDT.iTimeBeg, poolDT.m_NewYearMultiRechargeAwardDT.iTimeEnd)) //TsuCode
                {
                    listContent.Remove(poolDT);
                }
            }
            if (_contentWrapComponet == null)
            {
                _contentWrapComponet = new UIWrapComponent(210, 1, 1400, 7, f_GetObject("GridContentParent"), f_GetObject("Item"), listContent, OnContentItemUpdate, null);
            }
            _contentWrapComponet.f_ResetView();
        }
        _contentWrapComponet.f_UpdateView();
        UITool.f_OpenOrCloseWaitTip(false);

    }
    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        MutiRechargeItem mutiRechargeItem = item.GetComponent<MutiRechargeItem>();

        MutiRechargePoolDT poolDT = data as MutiRechargePoolDT;
        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(poolDT.m_NewYearMultiRechargeAwardDT.szAward);
        //long timeStart = CommonTools.f_GetActStarTimeForOpenSeverTime(poolDT.m_NewYearMultiRechargeAwardDT.iTimeBeg); //ccMath.f_Data2Int(poolDT.m_NewYearMultiRechargeAwardDT.iTimeBeg);
        //long timeEnd = CommonTools.f_GetActEndTimeForOpenSeverTime(poolDT.m_NewYearMultiRechargeAwardDT.iTimeEnd); //ccMath.f_Data2Int(poolDT.m_NewYearMultiRechargeAwardDT.iTimeEnd);
        //TsuCode
        long timeStart = CommonTools.f_GetActStarTimeForOpenSeverTime_Tsu(poolDT.m_NewYearMultiRechargeAwardDT.iTimeBeg); 
        long timeEnd = CommonTools.f_GetActEndTimeForOpenSeverTime_Tsu(poolDT.m_NewYearMultiRechargeAwardDT.iTimeEnd);
        //
        //TsuComment //int rechargeAllCount = Data_Pool.m_RechargePool.f_GetAllRechageMoney(timeStart, timeEnd);
        //TsuCode---
        int rechargeAllCount = Data_Pool.m_RechargePool.f_GetAllRechageMoney_TsuFunc(timeStart, timeEnd);
        ///------------
        mutiRechargeItem.f_SetData(rechargeAllCount, poolDT, listCommonDT);

        EM_BoxGetState state = EM_BoxGetState.Lock;
        if (poolDT.mGetTime > 0)
            state = EM_BoxGetState.AlreadyGet;
        else
        {
            if (rechargeAllCount >= poolDT.m_NewYearMultiRechargeAwardDT.iCondition)
            {
                state = EM_BoxGetState.CanGet;
            }
        }
        mutiRechargeItem.mBtnGet.SetActive(state == EM_BoxGetState.CanGet);
        mutiRechargeItem.mBtnHasGet.SetActive(state == EM_BoxGetState.AlreadyGet);
        mutiRechargeItem.mBtnGoRecharge.SetActive(state == EM_BoxGetState.Lock);

        f_RegClickEvent(mutiRechargeItem.mBtnGoRecharge, OnGotoClick);
        f_RegClickEvent(mutiRechargeItem.mBtnGet, OnBtnGetClick, poolDT);
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
        MutiRechargePoolDT poolDT = obj1 as MutiRechargePoolDT;
        //if (!CommonTools.f_CheckActIsOpenForOpenSeverTime(poolDT.m_NewYearMultiRechargeAwardDT.iTimeBeg, poolDT.m_NewYearMultiRechargeAwardDT.iTimeEnd))//(!CommonTools.f_CheckTime(poolDT.m_NewYearMultiRechargeAwardDT.iTimeBeg.ToString(), poolDT.m_NewYearMultiRechargeAwardDT.iTimeEnd.ToString()))
        if (!CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(poolDT.m_NewYearMultiRechargeAwardDT.iTimeBeg, poolDT.m_NewYearMultiRechargeAwardDT.iTimeEnd))//(!CommonTools.f_CheckTime(poolDT.m_NewYearMultiRechargeAwardDT.iTimeBeg.ToString(), poolDT.m_NewYearMultiRechargeAwardDT.iTimeEnd.ToString()))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1474));
            return;
        }
        curSelectPoolDT = poolDT;
        Data_Pool.m_MutiRechargePool.f_GetAward((int)poolDT.iId, RequestGetCallback);
        UITool.f_OpenOrCloseWaitTip(true);
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1479) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //更新UI显示
        _contentWrapComponet.f_UpdateView();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { CommonTools.f_GetListAwardPoolDT(curSelectPoolDT.m_NewYearMultiRechargeAwardDT.szAward) });
    }
    private void OnGetFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(14780) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
}
