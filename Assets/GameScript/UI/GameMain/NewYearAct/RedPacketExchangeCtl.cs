using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
/// <summary>
/// 红包兑换活动
/// </summary>
public class RedPacketExchangeCtl : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
    private SocketCallbackDT RequestExchargeCallback = new SocketCallbackDT();//请求兑换回调
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();
    RedPacketExchangePoolDT curSelectPoolDT;
    private bool isInitView = false;
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView(int timeEnd)
    {
        gameObject.SetActive(true);
        isInitView = false;
        
        if (timeEnd == 0)
        {
            f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1472);
            return;
        }

        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        RequestExchargeCallback.m_ccCallbackSuc = OnExchargeSucCallback;
        RequestExchargeCallback.m_ccCallbackFail = OnExchargeFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_RedPacketExchangePool.f_QueryInfo(QueryCallback);

        InitUI();
        string str = timeEnd.ToString();
        int year = int.Parse(str.Substring(0, 4));//20181224
        int month = int.Parse(str.Substring(4, 2));
        int day= int.Parse(str.Substring(6, 2));
        f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1473), day, month, year);


        //f_GetObject("SprTitleBg").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(304);
        //f_GetObject("SprTitleBg").GetComponent<UI2DSprite>().MakePixelPerfect();
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    private void InitUI()
    {
        f_GetObject("LabelCount1").GetComponent<UILabel>().text = UITool.f_GetGoodNum(EM_ResourceType.Good, 361).ToString();
        f_GetObject("LabelCount2").GetComponent<UILabel>().text = UITool.f_GetGoodNum(EM_ResourceType.Good, 362).ToString();
        f_GetObject("LabelCount3").GetComponent<UILabel>().text = UITool.f_GetGoodNum(EM_ResourceType.Good, 363).ToString();
        f_GetObject("LabelCount4").GetComponent<UILabel>().text = UITool.f_GetGoodNum(EM_ResourceType.Good, 364).ToString();
        f_GetObject("LabelCount5").GetComponent<UILabel>().text = UITool.f_GetGoodNum(EM_ResourceType.Good, 365).ToString();
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        listContent = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_RedPacketExchangePool.f_GetAll());
        for(int i = listContent.Count - 1; i >= 0; i--)
        {
            RedPacketExchangePoolDT poolDT = listContent[i] as RedPacketExchangePoolDT;
            if (!CommonTools.f_CheckTime(poolDT.mRedPacketExChangeDT.iTimeBegin.ToString(), poolDT.mRedPacketExChangeDT.iTimeEnd.ToString()))
            {
                listContent.Remove(poolDT);
            }
        }
        if (!isInitView)
        {
            isInitView = true;
            if (_contentWrapComponet == null)
            {
                _contentWrapComponet = new UIWrapComponent(250, 1, 1400, 8, f_GetObject("GridContentParent"), f_GetObject("Item"), listContent, OnContentItemUpdate, null);
            }
            _contentWrapComponet.f_ResetView();
        }
        _contentWrapComponet.f_UpdateList(listContent);
        _contentWrapComponet.f_UpdateView();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        RedPacketExchangeItem redPacketExchangeItem = item.GetComponent<RedPacketExchangeItem>();
        RedPacketExchangePoolDT poolDT = data as RedPacketExchangePoolDT;
        ResourceCommonDT sailCommonDT = new ResourceCommonDT();
        sailCommonDT.f_UpdateInfo((byte)poolDT.mRedPacketExChangeDT.iDstResType, poolDT.mRedPacketExChangeDT.iDstResId, poolDT.mRedPacketExChangeDT.iDstResCount);

        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(poolDT.mRedPacketExChangeDT.szConsumeRes);

        redPacketExchangeItem.f_SetData(sailCommonDT, poolDT.mHasExchangeCount, poolDT.mRedPacketExChangeDT.iExchangeTimes, listCommonDT);
        //显示可兑换/不可兑换，注册按钮事件
        f_RegClickEvent(redPacketExchangeItem.mExchargeIcon.gameObject, UITool.f_OnItemIconClick, sailCommonDT);
        f_RegClickEvent(redPacketExchangeItem.mBtnExcharge, OnBtnExchargeClick, poolDT, listCommonDT);
    }
    /// <summary>
    /// 点击兑换按钮事件
    /// </summary>
    private void OnBtnExchargeClick(GameObject go, object obj1, object obj2)
    {
        RedPacketExchangePoolDT poolDT = obj1 as RedPacketExchangePoolDT;
        if (!CommonTools.f_CheckTime(poolDT.mRedPacketExChangeDT.iTimeBegin.ToString(), poolDT.mRedPacketExChangeDT.iTimeEnd.ToString()))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1474));
            return;
        }
        if (poolDT.mHasExchangeCount >= poolDT.mRedPacketExChangeDT.iExchangeTimes)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1475));
            return;
        }
        List<ResourceCommonDT> listCommonDT = (obj2) as List<ResourceCommonDT>;
        for (int i = 0; i < listCommonDT.Count; i++)
        {
            ResourceCommonDT commonDT = listCommonDT[i];
            int hasNum = UITool.f_GetGoodNum((EM_ResourceType)commonDT.mResourceType, commonDT.mResourceId);
            if (hasNum < commonDT.mResourceNum)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1476));
                return;
            }
        }
        curSelectPoolDT = poolDT;
        Data_Pool.m_RedPacketExchangePool.f_Exchange((int)poolDT.iId, RequestExchargeCallback);
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
    /// 兑换成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnExchargeSucCallback(object obj)
    {
        //更新UI显示
        _contentWrapComponet.f_UpdateView();

        InitUI();
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)curSelectPoolDT.mRedPacketExChangeDT.iDstResType, curSelectPoolDT.mRedPacketExChangeDT.iDstResId, curSelectPoolDT.mRedPacketExChangeDT.iDstResCount);
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnExchargeFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1478) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
}

