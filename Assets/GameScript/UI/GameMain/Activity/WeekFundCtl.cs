using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 周基金
/// </summary>
public class WeekFundCtl : UIFramwork
{
    private string strTextBg = "UI/TextureRemove/Activity/Tex_WeekFundBg";
    private ccUIBase actPage;
    private string currentAward;
    private SocketCallbackDT QueryWeekFundCallback = new SocketCallbackDT();//请求周基金信息回调
    private SocketCallbackDT RequestWeekFundGetCallback = new SocketCallbackDT();//请求领取周基金
    private const int ShowModeId = 11001;//模型id
    private GameObject role;
    private GameObject btnRecharge;
    private UILabel ActEndTime;//活动结束倒计时
    private UILabel ActLastTimeName;//活动结束倒计时

    private UILabel GetEndTime;
    private UILabel GetEndTimeName;
    private UILabel ActRecharge;//活动充值金额
    private long iTime;
    private long iGetEndTime;
    private DateTime tDate;
    private int Time_ShowTime;
    /// <summary>
    /// 隐藏周基金
    /// </summary>
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_ShowTime);
    }

    /// <summary>
    /// 显示周基金
    /// </summary>
    public void f_ShowView(ccUIBase actPage)
    {
        gameObject.SetActive(true);
        ActEndTime = f_GetObject("UILableEndTime").GetComponent<UILabel>();
        ActRecharge = f_GetObject("RechargeText").GetComponent<UILabel>();

        GetEndTime = f_GetObject("UILableGetEndTime").GetComponent<UILabel>();
        GetEndTimeName = f_GetObject("UILableGetEndTimeName").GetComponent<UILabel>();

        btnRecharge = f_GetObject("BtnReCharge");
        ActLastTimeName = f_GetObject("UILableEndTimeName").GetComponent<UILabel>();
        this.actPage = actPage;
        //查询 成功 / 失败
        QueryWeekFundCallback.m_ccCallbackSuc = OnWeekFundInfoSucCallback;
        QueryWeekFundCallback.m_ccCallbackFail = OnWeekFundInfoFailCallback;
        //领取 成功  / 失败
        RequestWeekFundGetCallback.m_ccCallbackSuc = OnGetWeekFundSucCallback;
        RequestWeekFundGetCallback.m_ccCallbackFail = OnGetWeekFundFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_WeekFundPool.f_QueryWeekFundInfo(QueryWeekFundCallback);
        UITool.f_CreateRoleByModeId(ShowModeId, ref role, f_GetObject("ModelParent").transform, 1);

        Time_ShowTime = ccTimeEvent.GetInstance().f_RegEvent(1f, true, null, _UpdateTimeData);
        f_LoadTextTure();
        f_RegClickEvent("BtnReCharge", OnGotoRechargeClick);
    }

    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTextTure()
    {
        UITexture textBg = f_GetObject("TextNumBg").GetComponent<UITexture>();
        if (textBg.mainTexture == null)
        {
            Texture2D texTure2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTextBg);
            textBg.mainTexture = texTure2D;
        }
    }


    /// <summary>
    /// 更新内容
    /// </summary>
    private void f_UpdateContent()
    {
        Data_Pool.m_WeekFundPool.f_CheckWeekFundRedPoint();
        Time_ShowTime = ccTimeEvent.GetInstance().f_RegEvent(1f, true, null, _UpdateTimeData);
        f_SetRechargeAwardData();
        f_SetWeedFundAwardData();
        //是否购买周基金
        if (!Data_Pool.m_WeekFundPool.m_IsBought)
        {
            btnRecharge.SetActive(true);
        }
        else
        {
            btnRecharge.SetActive(false);
        }
    }

    /// <summary>
    /// 设置充值奖励预览信息
    /// </summary>
    private void f_SetRechargeAwardData()
    {
        WeekFundPoolDT dataDT = new WeekFundPoolDT();
        WeekFundPoolDT poolDT = Data_Pool.m_WeekFundPool.f_GetCurWeekFundPoolDt();
        ActRecharge.text = poolDT.WeekFundData.iRechargeNum.ToString();
        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(poolDT.WeekFundData.szNote);// new List<ResourceCommonDT>();
        //listCommonDT.Add(CommonTools.f_GetListCommonDT(poolDT.WeekFundData.szAward1)[1]);
        //listCommonDT.Add(CommonTools.f_GetListCommonDT(poolDT.WeekFundData.szAward2)[1]);
        //listCommonDT.Add(CommonTools.f_GetListCommonDT(poolDT.WeekFundData.szAward3)[1]);
        //listCommonDT.Add(CommonTools.f_GetListCommonDT(poolDT.WeekFundData.szAward4)[1]);
        //listCommonDT.Add(CommonTools.f_GetListCommonDT(poolDT.WeekFundData.szAward5)[1]);
        //listCommonDT.Add(CommonTools.f_GetListCommonDT(poolDT.WeekFundData.szAward6)[1]);
        //listCommonDT.Add(CommonTools.f_GetListCommonDT(poolDT.WeekFundData.szAward7)[1]);
        GridUtil.f_SetGridView<ResourceCommonDT>(f_GetObject("AwardParent"), f_GetObject("AwardItem"), listCommonDT, OnAwardItemUpdate);
        f_GetObject("AwardParent").transform.GetComponent<UIGrid>().Reposition();
        f_GetObject("AwardParent").transform.GetComponentInParent<UIScrollView>().ResetPosition();
    }

    /// <summary>
    /// 设置周基金奖励信息
    /// </summary>
    private void f_SetWeedFundAwardData()
    {
        WeekFundPoolDT poolDT = Data_Pool.m_WeekFundPool.f_GetCurWeekFundPoolDt();
        List<WeekFundAwardNode> listFundAwardData = new List<WeekFundAwardNode>();
        for (int i = 0; i < poolDT.m_AwardNodeList.Count; i++)
        {
            listFundAwardData.Add(poolDT.m_AwardNodeList[i]);
        }
        GridUtil.f_SetGridView<WeekFundAwardNode>(f_GetObject("ItemGrid"), f_GetObject("Item"), listFundAwardData, OnWeekFundItemUpdate);
    }

    /// <summary>
    /// 充值奖励item更新
    /// </summary>
    /// <param name="item"></param>
    /// <param name="data"></param>
    private void OnAwardItemUpdate(GameObject item, ResourceCommonDT data)
    {
        string sailResName = data.mName;
        item.transform.Find("LabelCount").GetComponent<UILabel>().text = data.mResourceNum.ToString();
        item.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(data.mImportant, ref sailResName);
        item.transform.GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(data.mIcon);
        f_RegClickEvent(item, UITool.f_OnItemIconClick, data);
    }
    /// <summary>
    /// 七天奖励
    /// </summary>
    /// <param name="item"></param>
    /// <param name="data"></param>
    private void OnWeekFundItemUpdate(GameObject item, WeekFundAwardNode data)
    {
        //设置天数
        item.transform.Find("UILabelDay").GetComponent<UILabel>().text = data.m_DayName;
        //设置状态
        f_SetState(item, data.m_Day);
        //设置奖励
        GridUtil.f_SetGridView<ResourceCommonDT>(item.transform.Find("Grid").gameObject, f_GetObject("WeekAwardItem"), data.m_ResourceCommonDTList, OnAwardItemUpdate);

        //领取
        f_RegClickEvent(item.transform.Find("BtnReadyReceive").gameObject, OnWeekFundGetClick, data);
    }

    //设置领取状态
    private void f_SetState(GameObject item, byte curDay)
    {

        EM_WeekFundGetState state = f_GetAwardStateByCurDay(curDay);
        GameObject btnGet = item.transform.Find("BtnReadyReceive").gameObject;//领取
        GameObject btnCompleteReceive = item.transform.Find("BtnCompleteReceive").gameObject;//已领取
        GameObject btnEndReceive = item.transform.Find("BtnEndReceive").gameObject;//已过期
        if ((EM_WeekFundGetState)state == EM_WeekFundGetState.NoGet)
        {
            btnGet.SetActive(false);
            btnCompleteReceive.SetActive(false);
            btnEndReceive.SetActive(false);
        }
        else if ((EM_WeekFundGetState)state == EM_WeekFundGetState.CanGet)
        {
            btnGet.SetActive(true);
            btnCompleteReceive.SetActive(false);
            btnEndReceive.SetActive(false);
        }
        else if ((EM_WeekFundGetState)state == EM_WeekFundGetState.HasGet)
        {
            btnGet.SetActive(false);
            btnCompleteReceive.SetActive(true);
            btnEndReceive.SetActive(false);
        }
        else
        {
            btnGet.SetActive(false);
            btnCompleteReceive.SetActive(false);
            btnEndReceive.SetActive(true);
        }
    }


    /// <summary>
    /// 时间
    /// </summary>
    /// <param name="boj"></param>
    private void _UpdateTimeData(object boj)
    {
        WeekFundPoolDT poolDT = Data_Pool.m_WeekFundPool.f_GetCurWeekFundPoolDt();
        int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        int nowTime = GameSocket.GetInstance().f_GetServerTime();
        int openServerDay = ccMath.f_GetTotalDaysByTime(nowTime) - ccMath.f_GetTotalDaysByTime(openServerTime) + 1;
        long delayTime = 0;
        long GetDelayTime = 0;
        if (poolDT.WeekFundData.iId == 1)
        {
            GetDelayTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + 10 * 86400;    //(10 - openServerDay) * 24 * 3600;
            GetEndTimeName.text = CommonTools.f_GetTransLanguage(1385);

            ActLastTimeName.text = CommonTools.f_GetTransLanguage(1386);
            delayTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + 7 * 86400;//(7 - openServerDay) * 24 * 3600;
                                                                                    //if (Data_Pool.m_WeekFundPool.m_IsBought)
                                                                                    //{
                                                                                    //    delayTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + 10 * 86400;    //(10 - openServerDay) * 24 * 3600;
                                                                                    //    ActLastTimeName.text = CommonTools.f_GetTransLanguage(1385);
                                                                                    //}
                                                                                    //else
                                                                                    //{
                                                                                    //    ActLastTimeName.text = CommonTools.f_GetTransLanguage(1386);
                                                                                    //    delayTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + 7 * 86400;//(7 - openServerDay) * 24 * 3600;
                                                                                    //}

        }
        else
        {
            long timeEnd = ccMath.DateTime2time_t(GetTimeByTimeStr(poolDT.WeekFundData.iActivityEnd.ToString()));
            GetDelayTime = ccMath.DateTime2time_t(GetTimeByTimeStr(poolDT.WeekFundData.iAwardEnd.ToString()));
            ActLastTimeName.text = CommonTools.f_GetTransLanguage(1387);

            delayTime = timeEnd;
            ActLastTimeName.text = CommonTools.f_GetTransLanguage(1388);
            //if (Data_Pool.m_WeekFundPool.m_IsBought)
            //{
            //    delayTime = ccMath.DateTime2time_t(GetTimeByTimeStr(poolDT.WeekFundData.iAwardEnd.ToString())); 
            //    ActLastTimeName.text = CommonTools.f_GetTransLanguage(1387);
            //}
            //else
            //{
            //    delayTime = timeEnd; 
            //    ActLastTimeName.text = CommonTools.f_GetTransLanguage(1388);
            //}
        }
        iTime = delayTime - GameSocket.GetInstance().f_GetServerTime();
        iGetEndTime = GetDelayTime - GameSocket.GetInstance().f_GetServerTime();
        if (iTime <= 0)
        {
            //活动结束
            ActEndTime.text = CommonTools.f_GetTransLanguage(1389);
        }
        else
        {
            tDate = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime);
            if (tDate.Day != 1)
                ActEndTime.text = string.Format(CommonTools.f_GetTransLanguage(1390), tDate.Day - 1, tDate.Hour, tDate.Minute) + tDate.Second + CommonTools.f_GetTransLanguage(1392);
            else
                ActEndTime.text = string.Format(CommonTools.f_GetTransLanguage(1391), tDate.Hour, tDate.Minute) + tDate.Second + CommonTools.f_GetTransLanguage(1392);
        }

        if (iGetEndTime > 0)
        {
            tDate = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iGetEndTime);
            if (tDate.Day != 1)
                GetEndTime.text = string.Format(CommonTools.f_GetTransLanguage(1390), tDate.Day - 1, tDate.Hour, tDate.Minute) + tDate.Second + CommonTools.f_GetTransLanguage(1392);
            else
                GetEndTime.text = string.Format(CommonTools.f_GetTransLanguage(1391), tDate.Hour, tDate.Minute) + tDate.Second + CommonTools.f_GetTransLanguage(1392);
        }
        else {
            GetEndTime.text = CommonTools.f_GetTransLanguage(1389);
            return;
        }
        DateTime tDate3 = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        if ((GameSocket.GetInstance().mNextDayTime - tDate3).TotalSeconds < 0)
        {
            _Next2Day();
        }
    }

    /// <summary>
    /// 跨天时间
    /// </summary>
    private void _Next2Day()
    {
        SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
        QueryCallback.m_ccCallbackSuc = OnWeekFundSucCallback;
        Data_Pool.m_WeekFundPool.f_QueryWeekFundInfo(QueryCallback);
    }

    private void OnWeekFundSucCallback(object obj)
    {
        Data_Pool.m_WeekFundPool.f_CheckWeekFundRedPoint();
        f_SetRechargeAwardData();
        f_SetWeedFundAwardData();
    }

    private DateTime GetTimeByTimeStr(string StrTime)
    {
        int bYear = int.Parse(StrTime.Substring(0, 4));
        int bMonth = int.Parse(StrTime.Substring(4, 2));
        int bDay = int.Parse(StrTime.Substring(6, 2));
        DateTime time = new DateTime(bYear, bMonth, bDay, 0,
            0, 0);
        return time;
    }

    /// <summary>
    /// 请求服务器信息成功回调
    /// </summary>
    private void OnWeekFundInfoSucCallback(object obj)
    {
        //显示信息
        f_UpdateContent();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 信息失败回调
    /// </summary>
    private void OnWeekFundInfoFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1393) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }


    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnWeekFundGetClick(GameObject go, object obj1, object obj2)
    {
        WeekFundAwardNode data = obj1 as WeekFundAwardNode;
        if (null == data)
        {
MessageBox.ASSERT("No reward data！");
            return;
        }
        currentAward = data.m_SZAward;
        Data_Pool.m_WeekFundPool.f_GetWeekFundAward(data.m_Day, RequestWeekFundGetCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }

    /// <summary>
    /// 领取成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetWeekFundSucCallback(object obj)
    {
        //刷新数据
        f_SetWeedFundAwardData();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { CommonTools.f_GetListAwardPoolDT(currentAward) });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 领取失败
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetWeekFundFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1394) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }


    /// <summary>
    /// 从充值页面返回
    /// </summary>
    /// <param name="e"></param>
    public void f_ViewResume(object e)
    {
        Data_Pool.m_WeekFundPool.f_QueryWeekFundInfo(QueryWeekFundCallback);
    }

    /// <summary>
    /// 前往充值
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void OnGotoRechargeClick(GameObject go, object obj1, object obj2)
    {
        UITool.f_GotoPage(this.actPage, UINameConst.ShowVip, (int)ShowVip.EM_PageIndex.Recharge);
    }

    private EM_WeekFundGetState f_GetAwardStateByCurDay(byte curDay)
    {
        int curActivityDay = Data_Pool.m_WeekFundPool.f_GetCurDay();
        if (curActivityDay < 0)
        {
            return EM_WeekFundGetState.OverGet;
        }

        if (!Data_Pool.m_WeekFundPool.m_IsBought || (curActivityDay + 1) < curDay)
        {
            return EM_WeekFundGetState.NoGet;
        }

        if (BitTool.BitTest(Data_Pool.m_WeekFundPool.m_AwardFlag, curDay))
        {
            return EM_WeekFundGetState.HasGet;
        }

        return EM_WeekFundGetState.CanGet;
    }

    /// <summary>
    /// 领取状态枚举
    /// </summary>
    private enum EM_WeekFundGetState
    {
        NoGet = 0,//不可领取
        CanGet = 1,//可领取
        HasGet = 2,//已领取
        OverGet = 3,//已过期
    }
}
