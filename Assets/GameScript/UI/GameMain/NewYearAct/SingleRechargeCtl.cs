using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// 单笔充值
/// </summary>
public class SingleRechargeCtl : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//请求领取回调
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();
    SingleRechargePoolDT curSelectPoolDT;
    private bool isInitView = false;
    private ccUIBase actPage;
    
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView(ccUIBase actPage,int timeEnd)
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
        

        Data_Pool.m_SingleRechargePool.f_QueryInfo(QueryCallback);

        UpdateContent();

        GameObject ModelPoint = f_GetObject("ModelPoint");
        if (ModelPoint.transform.Find("Model") == null)
        {
            UITool.f_GetStatelObject(1200, ModelPoint.transform, Vector3.zero, Vector3.zero, 18, "Model", 150);
        }
        //DateTime EndTime = ccMath.time_t2DateTime(CommonTools.f_GetActEndTimeForOpenSeverTime(timeEnd));
        DateTime EndTime = ccMath.time_t2DateTime(CommonTools.f_GetActEndTimeForOpenSeverTime_Tsu(timeEnd)); //TsuCode
        //string str = timeEnd.ToString();
        //int year = int.Parse(str.Substring(0, 4));//20181224
        //int month = int.Parse(str.Substring(4, 2));
        //int day = int.Parse(str.Substring(6, 2));
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
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        if (!isInitView)
        {
            isInitView = true;
            listContent = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_SingleRechargePool.f_GetAll());
            for (int i = listContent.Count - 1; i >= 0; i--)
            {
                SingleRechargePoolDT poolDT = listContent[i] as SingleRechargePoolDT;
                //if(!CommonTools.f_CheckActIsOpenForOpenSeverTime(poolDT.m_NewYearSingleRechargeAwardDT.iTimeBeg, poolDT.m_NewYearSingleRechargeAwardDT.iTimeEnd)) //(!CommonTools.f_CheckTime(poolDT.m_NewYearSingleRechargeAwardDT.iTimeBeg.ToString(), poolDT.m_NewYearSingleRechargeAwardDT.iTimeEnd.ToString()))
                if (!CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(poolDT.m_NewYearSingleRechargeAwardDT.iTimeBeg, poolDT.m_NewYearSingleRechargeAwardDT.iTimeEnd)) //TsuCode
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
        SingleRechargeItem singleRechargeItem = item.GetComponent<SingleRechargeItem>();

        SingleRechargePoolDT poolDT = data as SingleRechargePoolDT;
        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(poolDT.m_NewYearSingleRechargeAwardDT.szAward);

        singleRechargeItem.f_SetData(poolDT, listCommonDT);

        EM_BoxGetState state = EM_BoxGetState.Lock;
        if (poolDT.mGetTime > 0)
            state = EM_BoxGetState.AlreadyGet;
        else
        {
            //TsuComment
            //bool isCanGet = Data_Pool.m_RechargePool.f_GetIsSingleRecharge(poolDT.m_NewYearSingleRechargeAwardDT.iCondition,
            //   CommonTools.f_GetActStarTimeForOpenSeverTime(poolDT.m_NewYearSingleRechargeAwardDT.iTimeBeg), CommonTools.f_GetActEndTimeForOpenSeverTime(poolDT.m_NewYearSingleRechargeAwardDT.iTimeEnd));//ccMath.f_Data2Int(poolDT.m_NewYearSingleRechargeAwardDT.iTimeBeg), ccMath.f_Data2Int(poolDT.m_NewYearSingleRechargeAwardDT.iTimeEnd));
            //TsuCode
            //int startTime = CommonTools.f_GetActStarTimeForOpenSeverTime(poolDT.m_NewYearSingleRechargeAwardDT.iTimeBeg);
            //int endTime = CommonTools.f_GetActEndTimeForOpenSeverTime(poolDT.m_NewYearSingleRechargeAwardDT.iTimeEnd);
            int startTime = CommonTools.f_GetActStarTimeForOpenSeverTime_Tsu(poolDT.m_NewYearSingleRechargeAwardDT.iTimeBeg);
            int endTime = CommonTools.f_GetActEndTimeForOpenSeverTime_Tsu(poolDT.m_NewYearSingleRechargeAwardDT.iTimeEnd);
            //bool isCanGet = Data_Pool.m_RechargePool.f_GetIsSingleRecharge(poolDT.m_NewYearSingleRechargeAwardDT.iCondition,startTime,endTime);
            bool isCanGet = Data_Pool.m_RechargePool.f_GetIsSingleRecharge_TsuFunc(poolDT.m_NewYearSingleRechargeAwardDT.iCondition, startTime, endTime); //Tsucode
            //
            if (isCanGet)
                state = EM_BoxGetState.CanGet;
        }
        singleRechargeItem.mBtnGet.SetActive(state == EM_BoxGetState.CanGet);
        singleRechargeItem.mBtnHasGet.SetActive(state == EM_BoxGetState.AlreadyGet);
        singleRechargeItem.mBtnGoRecharge.SetActive(state == EM_BoxGetState.Lock);
        f_RegClickEvent(singleRechargeItem.mBtnGoRecharge, OnGotoClick);
        f_RegClickEvent(singleRechargeItem.mBtnGet, OnBtnGetClick, poolDT);
    }
    /// <summary>
    /// 点击前往
    /// </summary>
    private void OnGotoClick(GameObject go, object obj1, object obj2)
    {
        UITool.f_GotoPage(this.actPage, UINameConst.ShowVip, (int)ShowVip.EM_PageIndex.Recharge);
    }
    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnBtnGetClick(GameObject go, object obj1, object obj2)
    {
        SingleRechargePoolDT poolDT = obj1 as SingleRechargePoolDT;

        //if (!CommonTools.f_CheckActIsOpenForOpenSeverTime(poolDT.m_NewYearSingleRechargeAwardDT.iTimeBeg, poolDT.m_NewYearSingleRechargeAwardDT.iTimeEnd))//(!CommonTools.f_CheckTime(poolDT.m_NewYearSingleRechargeAwardDT.iTimeBeg.ToString(), poolDT.m_NewYearSingleRechargeAwardDT.iTimeEnd.ToString()))
        if (!CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(poolDT.m_NewYearSingleRechargeAwardDT.iTimeBeg, poolDT.m_NewYearSingleRechargeAwardDT.iTimeEnd))//TsuCode
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1474));
            return;
        }
        curSelectPoolDT = poolDT;
        Data_Pool.m_SingleRechargePool.f_GetAward((int)poolDT.iId, RequestGetCallback);
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1477) + CommonTools.f_GetTransLanguage((int)obj));
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
            new object[] { CommonTools.f_GetListAwardPoolDT(curSelectPoolDT.m_NewYearSingleRechargeAwardDT.szAward) });
    }
    private void OnGetFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1480) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
}
