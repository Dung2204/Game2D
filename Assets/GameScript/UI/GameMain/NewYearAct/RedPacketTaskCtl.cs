using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
/// <summary>
/// 红包任务活动
/// </summary>
public class RedPacketTaskCtl : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT QueryExchargeCallback = new SocketCallbackDT();//查询信息回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//请求兑换回调
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();
    RedPacketTaskPoolDT curSelectPoolDT;
    ccUIBase actPage;
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

        if (timeEnd == 0)
        {
            f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1472);
            return;
        }

        QueryExchargeCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryExchargeCallback.m_ccCallbackFail = OnQueryFailCallback;
        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_RedPacketTaskPool.f_QueryInfo(QueryExchargeCallback);

        string str = timeEnd.ToString();
        //int year = int.Parse(str.Substring(0, 4));//20181224
        //int month = int.Parse(str.Substring(4, 2));
        //int day = int.Parse(str.Substring(6, 2));

        //TsuCode
        int year = CommonTools.getYear(timeEnd);
        int month = CommonTools.getMonth(timeEnd);
        int day = CommonTools.getDay(timeEnd);

        //

        f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1473), day, month, year);

        //f_GetObject("SprTitleBg").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(303);
        //f_GetObject("SprTitleBg").GetComponent<UI2DSprite>().MakePixelPerfect();
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        List<BasePoolDT<long>> listTemp = Data_Pool.m_RedPacketTaskPool.f_GetAll();
        listContent.Clear();
        for (int i = 0; i < listTemp.Count; i++)
        {
            RedPacketTaskPoolDT poolDT = listTemp[i] as RedPacketTaskPoolDT;
            if (poolDT.mRedPacketTaskDT.iTaskType == Data_Pool.m_RedPacketTaskPool.mRecruitGiftTaskID)
                continue;
            //if(!CommonTools.f_CheckTime(poolDT.mRedPacketTaskDT.iTimeBegin.ToString(), poolDT.mRedPacketTaskDT.iTimeEnd.ToString()))
            //    continue;

            //TsuCode
            if (!CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(poolDT.mRedPacketTaskDT.iTimeBegin, poolDT.mRedPacketTaskDT.iTimeEnd))
                continue;
            //
            if (!isContailTaskType(listContent, poolDT))
            {
                listContent.Add(poolDT);
            }
        }
        if (_contentWrapComponet == null)
        {
            _contentWrapComponet = new UIWrapComponent(222, 2, 650, 8, f_GetObject("GridContentParent"), f_GetObject("Item"), listContent, OnContentItemUpdate, null);
        }
        _contentWrapComponet.f_ResetView();
        _contentWrapComponet.f_UpdateList(listContent);
        _contentWrapComponet.f_UpdateView();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 检测是否有同类型任务的poolDT
    /// </summary>
    /// <param name="listContent">poolDT列表</param>
    /// <param name="temppoolDT">待加入的poolDT</param>
    /// <returns></returns>
    private bool isContailTaskType(List<BasePoolDT<long>> listContent, RedPacketTaskPoolDT temppoolDT)
    {
        for (int i = 0; i < listContent.Count; i++)
        {
            RedPacketTaskPoolDT poolDT = listContent[i] as RedPacketTaskPoolDT;
            if (poolDT.mRedPacketTaskDT.iTaskType == temppoolDT.mRedPacketTaskDT.iTaskType)
            {
                if (poolDT.mHasGetCount > 0)
                {
                    listContent.Remove(poolDT);
                    return false;
                }
                else
                    return true;
            }
        }
        return false;
    }
    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        RedPacketTaskItem redPacketExchangeItem = item.GetComponent<RedPacketTaskItem>();
        RedPacketTaskPoolDT poolDT = data as RedPacketTaskPoolDT;

        redPacketExchangeItem.f_SetData(poolDT);
        //显示可兑换/不可兑换，注册按钮事件
        f_RegClickEvent(redPacketExchangeItem.mBtnGet, OnBtnGetClick, poolDT);
        f_RegClickEvent(redPacketExchangeItem.mBtnGoto, OnBtnGotoClick, poolDT.mRedPacketTaskDT.szUIName, poolDT.mRedPacketTaskDT.iUIParam);
    }
    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnBtnGetClick(GameObject go, object obj1, object obj2)
    {
        RedPacketTaskPoolDT poolDT = obj1 as RedPacketTaskPoolDT;
        //if (!CommonTools.f_CheckTime(poolDT.mRedPacketTaskDT.iTimeBegin.ToString(), poolDT.mRedPacketTaskDT.iTimeEnd.ToString()))
        if (!CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(poolDT.mRedPacketTaskDT.iTimeBegin, poolDT.mRedPacketTaskDT.iTimeEnd)) //TsuCode
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1474));
            return;
        }

        curSelectPoolDT = poolDT;
        Data_Pool.m_RedPacketTaskPool.f_GetAward(poolDT.mRedPacketTaskDT.iId, RequestGetCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    /// <summary>
    /// 点击前往按钮事件
    /// </summary>
    private void OnBtnGotoClick(GameObject go, object obj1, object obj2)
    {
        string uiName = (string)obj1;
        int param = (int)obj2;
        if (uiName != null && uiName != "")
        {
            UITool.f_GotoPage(actPage, uiName, param);
        }
    }
    #region 回调
    /// <summary>
    /// 查询信息回调
    /// </summary>
    /// <param name="obj"></param>
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
    /// 领取成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        //更新UI显示
        UpdateContent();

        List<AwardPoolDT> listAwardPoolDT = new List<AwardPoolDT>();
        if (curSelectPoolDT.mRedPacketTaskDT.szAward1.Contains(";"))
            listAwardPoolDT.Add(CommonTools.f_GetAwardPoolDTByResourceStr(curSelectPoolDT.mRedPacketTaskDT.szAward1));
        if (curSelectPoolDT.mRedPacketTaskDT.szAward2.Contains(";"))
            listAwardPoolDT.Add(CommonTools.f_GetAwardPoolDTByResourceStr(curSelectPoolDT.mRedPacketTaskDT.szAward2));
        if (curSelectPoolDT.mRedPacketTaskDT.szAward3.Contains(";"))
            listAwardPoolDT.Add(CommonTools.f_GetAwardPoolDTByResourceStr(curSelectPoolDT.mRedPacketTaskDT.szAward3));
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { listAwardPoolDT });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnGetFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1480) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
}

