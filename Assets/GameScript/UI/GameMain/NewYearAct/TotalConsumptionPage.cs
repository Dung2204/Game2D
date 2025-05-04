using UnityEngine;
using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TotalConsumptionPage : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//请求领取回调
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();
    private bool isInitView = false;
    private ccUIBase actPage;
    private UILabel TotalNum;


    private long _StarTime = 0;
    private long _OverTime = 0;

    private TotalConsumptionPoolDT curSelectPoolDT;
    private int _HiostryConst = 0;
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView(ccUIBase actPage, int timeEnd)
    {
        gameObject.SetActive(true);
        this.actPage = actPage;
        isInitView = false;

        f_LoadTexture();
        if (timeEnd == 0)
        {
            f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1472);
            return;
        }

        _InfoData();
        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        TotalNum = f_GetObject("TotalNum").GetComponent<UILabel>();
        Data_Pool.m_TotalConsumptionPool.f_TotalConsumpInfo(QueryCallback);
        UpdateContent();

        GameObject ModelPoint = f_GetObject("ModelPoint");
        if (ModelPoint.transform.Find("Model") == null)
        {//1309//1204
            UITool.f_GetStatelObject(1417, ModelPoint.transform, Vector3.zero, Vector3.zero, 18, "Model", 230);
        }

        //string str = timeEnd.ToString();
        //int year = int.Parse(str.Substring(0, 4));//20181224
        //int month = int.Parse(str.Substring(4, 2));
        //int day = int.Parse(str.Substring(6, 2));
        DateTime EndTime = ccMath.time_t2DateTime(timeEnd);
        f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1473), EndTime.Day, EndTime.Month, EndTime.Year);
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

    private void _InfoData()
    {
        NewYearSyceeConsumeDT tNewYearSyceeConsumeDT;
        int _NowTime = GameSocket.GetInstance().f_GetServerTime();
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_NewYearSyceeConsumeSC.f_GetAll().Count; i++)
        {
            tNewYearSyceeConsumeDT = glo_Main.GetInstance().m_SC_Pool.m_NewYearSyceeConsumeSC.f_GetAll()[i] as NewYearSyceeConsumeDT;

            //DateTime dataTimeStart = CommonTools.f_GetDateTimeByTimeStr(tNewYearSyceeConsumeDT.iTimeBegin.ToString());
            //DateTime dataTimeEnd = CommonTools.f_GetDateTimeByTimeStr(tNewYearSyceeConsumeDT.iTimeEnd.ToString());
            //if (CommonTools.f_CheckActIsOpenForOpenSeverTime(tNewYearSyceeConsumeDT.iTimeBegin, tNewYearSyceeConsumeDT.iTimeEnd))//(CommonTools.f_CheckTime(dataTimeStart, dataTimeEnd))
            if (CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(tNewYearSyceeConsumeDT.iTimeBegin, tNewYearSyceeConsumeDT.iTimeEnd)) //TsuCode
            {
                //_StarTime = CommonTools.f_GetActStarTimeForOpenSeverTime(tNewYearSyceeConsumeDT.iTimeBegin); //ccMath.DateTime2time_t(dataTimeStart);
                //_OverTime = CommonTools.f_GetActEndTimeForOpenSeverTime(tNewYearSyceeConsumeDT.iTimeEnd); //ccMath.DateTime2time_t(dataTimeEnd);
                //TsuCode
                _StarTime = CommonTools.f_GetActStarTimeForOpenSeverTime_Tsu(tNewYearSyceeConsumeDT.iTimeBegin); //ccMath.DateTime2time_t(dataTimeStart);
                _OverTime = CommonTools.f_GetActEndTimeForOpenSeverTime_Tsu(tNewYearSyceeConsumeDT.iTimeEnd); //ccMath.DateTime2time_t(dataTimeEnd);
                //
                break;
            }
        }
        _HiostryConst = Data_Pool.m_HistoryConstPool.f_GetAppointTimeRanges(_StarTime, _OverTime);
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        if (!isInitView)
        {
            isInitView = true;
            TotalNum.text = string.Format(CommonTools.f_GetTransLanguage(1491), _HiostryConst);
            listContent = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_TotalConsumptionPool.f_GetAll());
            //去除未在时间段内的
            for (int i = listContent.Count - 1; i >= 0; i--)
            {
                TotalConsumptionPoolDT poolDt = listContent[i] as TotalConsumptionPoolDT;
                //if (!CommonTools.f_CheckActIsOpenForOpenSeverTime(poolDt.m_NewYearSyceeConsume.iTimeBegin, poolDt.m_NewYearSyceeConsume.iTimeEnd))//(!CommonTools.f_CheckTime(poolDt.m_NewYearSyceeConsume.iTimeBegin.ToString(), poolDt.m_NewYearSyceeConsume.iTimeEnd.ToString()))
                if (!CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(poolDt.m_NewYearSyceeConsume.iTimeBegin, poolDt.m_NewYearSyceeConsume.iTimeEnd)) //TsuCode
                {
                    listContent.Remove(poolDt);
                }
            }
            if (_contentWrapComponet == null)
            {
                _contentWrapComponet = new UIWrapComponent(210, 1, 1400, 7, f_GetObject("GridContentParent"), f_GetObject("Item"), listContent, OnContentItemUpdate, null);
            }
            _contentWrapComponet.f_ResetView();
        }
        _contentWrapComponet.f_ViewGotoRealIdx(goToIndex, 2);
        _contentWrapComponet.f_UpdateView();
        UITool.f_OpenOrCloseWaitTip(false);

    }
    private int goToIndex = 0;
    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        TotalConsumptionPoolDT poolDT = data as TotalConsumptionPoolDT;
        SingleRechargeItem tItem = item.GetComponent<SingleRechargeItem>();
        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(poolDT.m_NewYearSyceeConsume.szAward);

        EM_BoxGetState state = EM_BoxGetState.Lock;

       // int _count = Data_Pool.m_HistoryConstPool.f_GetAppointTimeRanges(CommonTools.f_GetActStarTimeForOpenSeverTime(poolDT.m_NewYearSyceeConsume.iTimeBegin),
       //CommonTools.f_GetActEndTimeForOpenSeverTime(poolDT.m_NewYearSyceeConsume.iTimeEnd));
        //(ccMath.f_Data2Int(poolDT.m_NewYearSyceeConsume.iTimeBegin),ccMath.f_Data2Int(poolDT.m_NewYearSyceeConsume.iTimeEnd));
        int _count = Data_Pool.m_HistoryConstPool.f_GetAppointTimeRanges(CommonTools.f_GetActStarTimeForOpenSeverTime_Tsu(poolDT.m_NewYearSyceeConsume.iTimeBegin), //TsuCode 
            CommonTools.f_GetActEndTimeForOpenSeverTime_Tsu(poolDT.m_NewYearSyceeConsume.iTimeEnd)); //TsuCode
        bool bCanGet = _count >= poolDT.m_NewYearSyceeConsume.iCondition;
        if (poolDT.m_bIsGetAward)
        {
            state = EM_BoxGetState.AlreadyGet;
            goToIndex = listContent.IndexOf(poolDT);
        }
        else
        {
            if (bCanGet)
            {
                goToIndex = listContent.IndexOf(poolDT);
                state = EM_BoxGetState.CanGet;
            }
            else
                state = EM_BoxGetState.Lock;
        }

        tItem.f_SetData(poolDT, listCommonDT);

        tItem.mBtnGet.SetActive(state == EM_BoxGetState.CanGet);
        tItem.mBtnHasGet.SetActive(state == EM_BoxGetState.AlreadyGet);
        tItem.mBtnGoRecharge.SetActive(state == EM_BoxGetState.Lock);

        f_RegClickEvent(tItem.mBtnGet, OnBtnGetClick, poolDT);
    }
    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnBtnGetClick(GameObject go, object obj1, object obj2)
    {
        TotalConsumptionPoolDT poolDT = obj1 as TotalConsumptionPoolDT;

        //if(!CommonTools.f_CheckActIsOpenForOpenSeverTime(poolDT.m_NewYearSyceeConsume.iTimeBegin, poolDT.m_NewYearSyceeConsume.iTimeEnd)) //(!CommonTools.f_CheckTime(poolDT.m_NewYearSyceeConsume.iTimeBegin.ToString(), poolDT.m_NewYearSyceeConsume.iTimeEnd.ToString()))
        if (!CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(poolDT.m_NewYearSyceeConsume.iTimeBegin, poolDT.m_NewYearSyceeConsume.iTimeEnd)) //TsuCode
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1474));
            return;
        }
        curSelectPoolDT = poolDT;
        Data_Pool.m_TotalConsumptionPool.f_GetAward((int)poolDT.iId, RequestGetCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    #region 回调
    /// <summary>
    /// 查询成功回调
    /// </summary>
    private void OnQuerySucCallback(object obj)
    {
        _InfoData();
        UpdateContent();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnQueryFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1477) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        _InfoData();
        //更新UI显示
        _contentWrapComponet.f_UpdateView();
        List<AwardPoolDT> tGoods = CommonTools.f_GetListAwardPoolDT(curSelectPoolDT.m_NewYearSyceeConsume.szAward);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tGoods });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnGetFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1478) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
}
