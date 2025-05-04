using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
public class RechargePool : BasePool
{
    //充值时间戳记录
    private List<CMsg_SC_PccaccyRecordInfoNode> payRecordList = null;
    public Dictionary<string, int>[] VipSc;
    public RechargePool()
        : base("RechargePoolDT", true)
    {
        int tLen = glo_Main.GetInstance().m_SC_Pool.m_VipPrivilegeSC.f_GetAll()[glo_Main.GetInstance().m_SC_Pool.m_VipPrivilegeSC.f_GetAll().Count - 1].iId;
        VipSc = new Dictionary<string, int>[tLen];
        AddData();
    }

    protected override void f_Init()
    {
        payRecordList = new List<CMsg_SC_PccaccyRecordInfoNode>();
        unsettledPayList = new List<CMsg_SC_UnsettledPccaccyNode>();
        unsettledPayProcessing = false;
        List<NBaseSCDT> tInitList = glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetAll();
        for (int i = 0; i < tInitList.Count; i++)
        {
            PccaccyDT tNode = (PccaccyDT)tInitList[i];
            RechargePoolDT tPoolDataDT = new RechargePoolDT(tNode.iId);
            f_Save(tPoolDataDT);
        }
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
    }

    protected override void RegSocketMessage()
    {
        //SC_RechargeHistory tSC_RechargInit=new SC_RechargeHistory();
        //GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_RechargeHistory, tSC_RechargInit, Callback_SocketData_Update);
        SC_RechargeHistory tServerData = new SC_RechargeHistory();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_RechargeHistory, tServerData, Callback_times);

        CMsg_SC_UnsettledPccaccyNode tUnsettledPayNode = new CMsg_SC_UnsettledPccaccyNode();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_UnsettledPay, tUnsettledPayNode, f_OnUnsettledPay);

        CMsg_SC_PccaccyRecordInfoNode tPayRecordInfoNode = new CMsg_SC_PccaccyRecordInfoNode();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_RechargeRecord, tPayRecordInfoNode, f_OnRechargeRecord);

    }
    //void Caallback_times(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    //{
    //    foreach (SockBaseDT item in aData)
    //    {
    //        Callback_times(item);
    //    }
    //}

    private void f_OnRechargeRecord(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    {
        //MessageBox.ASSERT("TsuLog RechargePool f_OnRechargeRecord - "+ aData.Count);
        for (int i = 0; i < aData.Count; i++)
        {
            CMsg_SC_PccaccyRecordInfoNode tServerData = (CMsg_SC_PccaccyRecordInfoNode)aData[i];
            payRecordList.Add(tServerData);
        }
    }


    private bool unsettledPayProcessing = false;
    private List<CMsg_SC_UnsettledPccaccyNode> unsettledPayList = null;
    public List<CMsg_SC_UnsettledPccaccyNode> UnsettledPayList
    {
        get
        {
            return unsettledPayList;
        }
    }

    private void f_OnUnsettledPay(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    {
        unsettledPayList.Clear();
        for (int i = 0; i < aData.Count; i++)
        {
            CMsg_SC_UnsettledPccaccyNode tServerData = (CMsg_SC_UnsettledPccaccyNode)aData[i];
            unsettledPayList.Add(tServerData);
        }

        if (!unsettledPayProcessing)
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UnsettledPay_Tip);
    }

    public void f_StartProcessUnsettledPay()
    {
        if (unsettledPayList.Count > 0)
        {
            unsettledPayProcessing = true;
            SDKPccaccyResult tResult = new SDKPccaccyResult(EM_PccaccyResult.UnsettledPay, Data_Pool.m_RechargePool.UnsettledPayList[0].payId, Data_Pool.m_RechargePool.UnsettledPayList[0].szSerial);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_PAY_RESULT, tResult);
        }
    }

    public void f_ProcessUnsettledPay(eMsgOperateResult result, EM_PccaccyResult payResult, string orderId, int payTemplateId)
    {
        if (payResult != EM_PccaccyResult.UnsettledPay)
            return;
        if (result != eMsgOperateResult.OR_Succeed)
        {
            //服务器发送不存在的流水号
			MessageBox.ASSERT("The deposit (unprocessed) code received from the server does not exist");
        }
        PccaccyDT tPayDt = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(payTemplateId);
        string tPayName = string.Format(CommonTools.f_GetTransLanguage(2265), tPayDt.iPccaccyNum);
		PopupMenuParams tParam = new PopupMenuParams("Quá trình nạp tiền", string.Format("Quá trình thành công {0}", tPayName), "Xác nhận", f_ProcessNextUnsettledPay, "", null, orderId);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_ProcessNextUnsettledPay(object value)
    {
        string orderId = (string)value;
        int removeIdx = -99;
        for (int i = 0; i < unsettledPayList.Count; i++)
        {
            if (unsettledPayList[i].szSerial.Equals(orderId))
            {
                removeIdx = i;
                break;
            }
        }
        if (removeIdx >= 0)
            unsettledPayList.RemoveAt(removeIdx);
        if (unsettledPayList.Count == 0)
            unsettledPayProcessing = false;
        else
            f_StartProcessUnsettledPay();
    }


    void Callback_times(object Obj)
    {
        //MessageBox.ASSERT("TsuLog RechargePool Callback_times");
        SC_RechargeHistory tServerData = (SC_RechargeHistory)Obj;
        for (int i = 0; i < tServerData.nodes.Length; i++)
        {
            if ((RechargePoolDT)f_GetForId(tServerData.nodes[i].id) == null)
            {
                RechargePoolDT tPoolDataDT = new RechargePoolDT(tServerData.nodes[i].id);
                tPoolDataDT.times = tServerData.nodes[i].times;
                if (tPoolDataDT.iId == 1 && tPoolDataDT.times > 0)//月卡25
                {
                    Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy25 = true;
                }
                if (tPoolDataDT.iId == 2 && tPoolDataDT.times > 0)//月卡50
                {
                    Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy50 = true;
                }
                f_Save(tPoolDataDT);
            }
            else
            {
                RechargePoolDT tPoolDataDT = (RechargePoolDT)f_GetForId(tServerData.nodes[i].id);
                tPoolDataDT.times = tServerData.nodes[i].times;
                if (tPoolDataDT.iId == 1 && tPoolDataDT.times > 0)//月卡25
                {
                    Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy25 = true;
                }
                if (tPoolDataDT.iId == 2 && tPoolDataDT.times > 0)//月卡50
                {
                    Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy50 = true;
                }
            }
        }
    }

    Dictionary<string, int> f_GetField<T>(T t)
    {
        Dictionary<string, int> TmpStr = new Dictionary<string, int>();
        if (t == null)
        {
            return TmpStr;
        }
        System.Reflection.FieldInfo[] properties = t.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

        if (properties.Length <= 0)
        {
            return TmpStr;
        }
        foreach (System.Reflection.FieldInfo item in properties)
        {
            string name = item.Name;
            object value = item.GetValue(t);
            if (item.FieldType.IsValueType || item.FieldType.Name.StartsWith("int"))
            {
                TmpStr.Add(name, (int)value);
            }
        }
        return TmpStr;
    }

    void AddData()
    {
        for (int i = 0; i < VipSc.Length; i++)
        {
            VipSc[i] = f_GetField(glo_Main.GetInstance().m_SC_Pool.m_VipPrivilegeSC.f_GetSC(i + 1));
        }
    }

    /// <summary>
    /// 白名单充值
    /// </summary>
    /// <param name="id">payId</param>
    /// <param name="tSocketCallbackDT">操作结果返回</param>
    public void RechargeWhitelist(int id, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_Recharge, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_Recharge, bBuf);
    }

    /// <summary>
    /// 流水号充值
    /// </summary>
    /// <param name="orderId">流水号</param>
    /// <param name="tSocketCallbackDT">操作结果返回</param>
    public void RechargeSDK(string orderId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PaySDK, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(orderId, SDKPccaccyResult.ORDERID_MAX_LEN);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_PaySDK, bBuf);
    }


    /// //////////////////////////外部接口//////////////////////////////////////////////
    private const string KeyStr = "iLv";
    /// <summary>
    /// 根据VIP等级获取相对参数
    /// </summary>
    /// <param name="vipPrivilegeType"></param>
    /// <param name="vipLv"></param>
    /// <returns></returns>
    public int f_GetVipPriValue(EM_VipPrivilege vipPrivilegeType, int vipLv)
    {
        string tKey = string.Format("{0}{1}", KeyStr, vipLv);
        int tIdx = (int)vipPrivilegeType - 1;
        if (VipSc[tIdx].ContainsKey(tKey))
        {
            return VipSc[tIdx][tKey];
        }
        else
        {
            //MessageBox.ASSERT("获取VIP特权参数出错 VipLv：" + vipLv);
            return 0;
        }
    }

    /// <summary>
    /// 获取单前VIP等级的参数
    /// </summary>
    /// <param name="vipPrivilegeType"></param>
    /// <returns></returns>
    public int f_GetCurLvVipPriValue(EM_VipPrivilege vipPrivilegeType)
    {
        int tVipLv = UITool.f_GetNowVipLv();
        string tKey = string.Format("{0}{1}", KeyStr, tVipLv);
        int tIdx = (int)vipPrivilegeType - 1;
        if (VipSc[tIdx].ContainsKey(tKey))
        {
            return VipSc[tIdx][tKey];
        }
        else
        {
            //MessageBox.ASSERT("获取VIP特权参数出错 VipLv：" + tVipLv);
            return 0;
        }
    }

    /// <summary>
    /// 获取开启扫荡的VIP等级限制
    /// </summary>
    /// <returns></returns>
    public int f_SweepVipLvLimit()
    {
        for (int i = 0; i <= GameParamConst.VipLvMaxNuM; i++)
        {
            int tValue = f_GetVipPriValue(EM_VipPrivilege.eVip_OpenSweep, i);
            if (tValue > 0)
                return i;
        }
        return 0;
    }

    /// <summary>
    /// 获取某个类型的开启条件
    /// </summary>
    /// <param name="tmpVipPrivilege"></param>
    /// <returns></returns>
    public int f_GetVipLvLimit(EM_VipPrivilege tmpVipPrivilege)
    {

        for (int i = 0; i <= GameParamConst.VipLvMaxNuM; i++)
        {
            int tValue = f_GetVipPriValue(tmpVipPrivilege, i);
            if (tValue > 0)
                return i;
        }
        return 0;
    }

    public int f_GetDayRechageMoneyMax(DateTime DayNum)
    {
        int RechangeNum = 0;
        DateTime RechangeTime;
        for (int i = 0; i < payRecordList.Count; i++)
        {
            RechangeTime = ccMath.time_t2DateTime(payRecordList[i].time);
            if (RechangeTime.Day == DayNum.Day && RechangeTime.Month == DayNum.Month)
            {
                RechangeNum += (glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC((int)(payRecordList[i].payId)) as PccaccyDT).iPccaccyNum;
            }
        }
        return RechangeNum;
    }
    public int f_GetAllRechageMoney()
    {
        int RechangeNum = 0;
        for (int i = 0; i < payRecordList.Count; i++)
        {
            if(glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC((int)(payRecordList[i].payId)) !=null)
                RechangeNum += (glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC((int)(payRecordList[i].payId)) as PccaccyDT).iPccaccyNum;
        }
        return RechangeNum;
    }
    public int f_GetAllRechageMoney(long timeStart, long timeEnd)
    {
        int RechangeNum = 0;
        for (int i = 0; i < payRecordList.Count; i++)
        {
            //TsuComment
            //if (payRecordList[i].time >= timeStart && payRecordList[i].time < timeEnd)
            //    RechangeNum += (glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC((int)(payRecordList[i].payId)) as PccaccyDT).iPccaccyNum;
            //TsuCode
            if (payRecordList[i].time >= timeStart && payRecordList[i].time < timeEnd)
            {
                PccaccyDT pay = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(payRecordList[i].payId);
                float num = pay.iPccaccyNum * pay.iRate;
                int payInt = (int)num;
                RechangeNum += payInt;
            }
            //
        }
        return RechangeNum;
    }
    public int f_GetAllRechageMoneyToday()
    {
        int RechangeNum = 0;
        for (int i = 0; i < payRecordList.Count; i++)
        {
            if (CommonTools.f_CheckSameDay(GameSocket.GetInstance().f_GetServerTime(), payRecordList[i].time))
            {
                if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_Pay)
                {
                    PccaccyDT pay = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(payRecordList[i].payId);
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    int payInt = pay.iPayCount;
                    RechangeNum += payInt;
                }
                else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_LevelGift)
                {
                    LevelGiftDT pay = (LevelGiftDT)glo_Main.GetInstance().m_SC_Pool.m_LevelGiftSC.f_GetSC(payRecordList[i].payId);
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    int payInt = pay.iPayCount;
                    RechangeNum += payInt;
                }
                else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_TripleMoney)
                {
                    TripleMoneyDT pay = (TripleMoneyDT)glo_Main.GetInstance().m_SC_Pool.m_TripleMoneySC.f_GetSC(payRecordList[i].payId);
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    int payInt = pay.iPayCount;
                    RechangeNum += payInt;
                }
                else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_DealsEveryDay)
                {
                    NewYearDealsEveryDayDT pay = (NewYearDealsEveryDayDT)glo_Main.GetInstance().m_SC_Pool.m_NewYearDealsEveryDaySC.f_GetSC(payRecordList[i].payId);
                    int payInt = 0;
                    GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(119);

                    if (pay != null)
                    {
                        payInt = pay.iCondition;
                    }
                    else
                    {
                        payInt = param.iParam1;

                    }
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    RechangeNum += payInt;
                }
                else
                {
                    GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(116);
                    int payInt = param.iParam1*5;
                    RechangeNum += payInt;
                }
            }
        }
        return RechangeNum;
    }
    /// <summary>
    /// 检测玩家在某个时间内单笔充值是否有某个值
    /// </summary>
    /// <param name="timeStart"></param>
    /// <param name="timeEnd"></param>
    /// <returns></returns>
    public bool f_GetIsSingleRecharge(int RechargeNum, long timeStart, long timeEnd)
    {
        for (int i = 0; i < payRecordList.Count; i++)
        {
            CMsg_SC_PccaccyRecordInfoNode node = payRecordList[i];
            int payNum = (glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(node.payId) as PccaccyDT).iPccaccyNum;
            //TsuCode
            PccaccyDT pay = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(node.payId);
            float num = payNum * pay.iRate;
            int payInt = (int)num;
            //int payInt = pay.iPayCount;
            //TsuCode
            if (node.time >= timeStart && node.time < timeEnd && payInt == RechargeNum)
            {
                return true;
            }
            ///
              //if (node.time >= timeStart && node.time < timeEnd && payNum == RechargeNum)
            //{
            //    return true;
            //} //tsuComment
        }
        return false;
    }
    public bool f_GetMonthCard(int MonthCardid)
    {
        bool bIsMonthCard = false;
        for (int i = 0; i < payRecordList.Count; i++)
        {
            if (payRecordList[i].payId == MonthCardid && payRecordList[i].ePayFromConfig== (int)ePayFromConfig.ePay_Pay)
                bIsMonthCard = true;
        }
        return bIsMonthCard;
    }

    public int f_GetPayRecordCount()
    {
        if (payRecordList == null)
            return 0;
        return payRecordList.Count;
    }
    #region TsuCode - Func
    public int f_GetDayRechageMoneyMax_TsuFunc(DateTime DayNum)
    {
        int RechangeNum = 0;
        DateTime RechangeTime;
        for (int i = 0; i < payRecordList.Count; i++)
        {
            RechangeTime = ccMath.time_t2DateTime(payRecordList[i].time);
            if (RechangeTime.Day == DayNum.Day && RechangeTime.Month == DayNum.Month)
            {
                if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_Pay)
                {
                    PccaccyDT pay = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(payRecordList[i].payId);
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    int payInt = pay.iPayCount;
                    RechangeNum += payInt;
                }
                else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_LevelGift)
                {
                    LevelGiftDT pay = (LevelGiftDT)glo_Main.GetInstance().m_SC_Pool.m_LevelGiftSC.f_GetSC(payRecordList[i].payId);
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    int payInt = pay.iPayCount;
                    RechangeNum += payInt;
                }
                else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_TripleMoney)
                {
                    TripleMoneyDT pay = (TripleMoneyDT)glo_Main.GetInstance().m_SC_Pool.m_TripleMoneySC.f_GetSC(payRecordList[i].payId);
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    int payInt = pay.iPayCount;
                    RechangeNum += payInt;
                }
                else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_DealsEveryDay)
                {
                    NewYearDealsEveryDayDT pay = (NewYearDealsEveryDayDT)glo_Main.GetInstance().m_SC_Pool.m_NewYearDealsEveryDaySC.f_GetSC(payRecordList[i].payId);
                    int payInt = 0;
                    GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(119);

                    if (pay != null)
                    {
                        payInt = pay.iCondition;
                    }
                    else
                    {
                        payInt = param.iParam1;

                    }
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    RechangeNum += payInt;
                }
                else
                {
                    GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(116);
                    int payInt = param.iParam1 * 5;
                    RechangeNum += payInt;
                }
            }
        }
        
        return RechangeNum;
    }
    public int f_GetAllRechageMoney_TsuFunc()
    {
        int RechangeNum = 0;
        for (int i = 0; i < payRecordList.Count; i++)
        {
            //if (payRecordList[i].payId == 1 || payRecordList[i].payId == 2) continue;
            //RechangeNum += (glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC((int)(payRecordList[i].payId)) as PccaccyDT).iPayCount;
            if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_Pay)
            {
                PccaccyDT pay = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(payRecordList[i].payId);
                //float num = pay.iPccaccyNum * pay.iRate;
                //int payInt = (int)num;
                //if (pay.iId == 1 || pay.iId == 2) continue;
                int payInt = pay.iPayCount;
                RechangeNum += payInt;
            }
            else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_LevelGift)
            {
                LevelGiftDT pay = (LevelGiftDT)glo_Main.GetInstance().m_SC_Pool.m_LevelGiftSC.f_GetSC(payRecordList[i].payId);
                //float num = pay.iPccaccyNum * pay.iRate;
                //int payInt = (int)num;
                //if (pay.iId == 1 || pay.iId == 2) continue;
                int payInt = pay.iPayCount;
                RechangeNum += payInt;
            }
            else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_TripleMoney)
            {
                TripleMoneyDT pay = (TripleMoneyDT)glo_Main.GetInstance().m_SC_Pool.m_TripleMoneySC.f_GetSC(payRecordList[i].payId);
                //float num = pay.iPccaccyNum * pay.iRate;
                //int payInt = (int)num;
                //if (pay.iId == 1 || pay.iId == 2) continue;
                int payInt = pay.iPayCount;
                RechangeNum += payInt;
            }
            else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_DealsEveryDay)
            {
                NewYearDealsEveryDayDT pay = (NewYearDealsEveryDayDT)glo_Main.GetInstance().m_SC_Pool.m_NewYearDealsEveryDaySC.f_GetSC(payRecordList[i].payId);
                int payInt = 0;
                GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(119);

                if (pay != null)
                {
                    payInt = pay.iCondition;
                }
                else
                {
                    payInt = param.iParam1;

                }
                //float num = pay.iPccaccyNum * pay.iRate;
                //int payInt = (int)num;
                //if (pay.iId == 1 || pay.iId == 2) continue;
                RechangeNum += payInt;
            }
            else
            {
                GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(116);
                int payInt = param.iParam1 * 5;
                RechangeNum += payInt;
            }
        }
        return RechangeNum;
    }
    public bool f_GetIsSingleRecharge_TsuFunc(int RechargeNum, long timeStart, long timeEnd)
    {
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(94);
        int limitDay = param.iParam1;
        //Kiem tra xem limitday co nam giua startTime va endTime cua Su kien moi k

        int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        int openServerDay = ccMath.f_GetTotalDaysByTime(GameSocket.GetInstance().f_GetServerTime()) - ccMath.f_GetTotalDaysByTime(openServerTime) + 1;
        long zeroTimeLimitDay = openServerTime + (limitDay * 86400) - 86400;
        if (openServerDay >= limitDay && zeroTimeLimitDay > timeStart && zeroTimeLimitDay < timeEnd)
        {
            MessageBox.ASSERT("In Checking.................................");
            timeStart = zeroTimeLimitDay;
        }
        //-----------------
        for (int i = 0; i < payRecordList.Count; i++)
        {
            CMsg_SC_PccaccyRecordInfoNode node = payRecordList[i];
            if((ePayFromConfig)node.ePayFromConfig == ePayFromConfig.ePay_Pay)
            {
                //int payNum = (glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(node.payId) as PccaccyDT).iPccaccyNum;
                //TsuCode
                PccaccyDT pay = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(node.payId);
                //float num = payNum * pay.iRate;
                //int payInt = (int)num;
                int payInt = pay.iPayCount;
                //if (pay.iId == 1 || pay.iId == 2) continue;
                //TsuCode
                if (node.time >= timeStart && node.time < timeEnd && payInt == RechargeNum)
                {
                    return true;
                }
            }
            else if ((ePayFromConfig)node.ePayFromConfig == ePayFromConfig.ePay_LevelGift)
            {
                //int payNum = (glo_Main.GetInstance().m_SC_Pool.m_LevelGiftSC.f_GetSC(node.payId) as LevelGiftDT).iPayCount;
                //TsuCode
                LevelGiftDT pay = (LevelGiftDT)glo_Main.GetInstance().m_SC_Pool.m_LevelGiftSC.f_GetSC(node.payId);
                //float num = payNum * pay.iRate;
                //int payInt = (int)num;
                int payInt = pay.iPayCount;
                //if (pay.iId == 1 || pay.iId == 2) continue;
                //TsuCode
                if (node.time >= timeStart && node.time < timeEnd && payInt == RechargeNum)
                {
                    return true;
                }
            }
            else if ((ePayFromConfig)node.ePayFromConfig == ePayFromConfig.ePay_TripleMoney)
            {
                TripleMoneyDT pay = (TripleMoneyDT)glo_Main.GetInstance().m_SC_Pool.m_TripleMoneySC.f_GetSC(node.payId);
                //float num = payNum * pay.iRate;
                //int payInt = (int)num;
                int payInt = pay.iPayCount;
                //if (pay.iId == 1 || pay.iId == 2) continue;
                //TsuCode
                if (node.time >= timeStart && node.time < timeEnd && payInt == RechargeNum)
                {
                    return true;
                }
            }
            else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_DealsEveryDay)
            {
                NewYearDealsEveryDayDT pay = (NewYearDealsEveryDayDT)glo_Main.GetInstance().m_SC_Pool.m_NewYearDealsEveryDaySC.f_GetSC(payRecordList[i].payId);
                int payInt = 0;
                GameParamDT param3 = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(119);

                if (pay != null)
                {
                    payInt = pay.iCondition;
                }
                else
                {
                    payInt = param3.iParam1;

                }
                if (node.time >= timeStart && node.time < timeEnd && payInt == RechargeNum)
                {
                    return true;
                }
            }
            else
            {
                GameParamDT param2 = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(116);
                int payInt = param2.iParam1 * 5;
                if (node.time >= timeStart && node.time < timeEnd && payInt == RechargeNum)
                {
                    return true;
                }
            }


            ///
              //if (node.time >= timeStart && node.time < timeEnd && payNum == RechargeNum)
            //{
            //    return true;
            //} //tsuComment
        }
        return false;
    }
    public int f_GetAllRechageMoney_TsuFunc(long timeStart, long timeEnd)
    {
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(94);
        int limitDay = param.iParam1;
        //Kiem tra xem limitday co nam giua startTime va endTime cua Su kien moi k

        int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        int openServerDay = ccMath.f_GetTotalDaysByTime(GameSocket.GetInstance().f_GetServerTime()) - ccMath.f_GetTotalDaysByTime(openServerTime) + 1;
        long zeroTimeLimitDay = openServerTime + (limitDay * 86400) - 86400;
        if (openServerDay >= limitDay && zeroTimeLimitDay > timeStart && zeroTimeLimitDay < timeEnd)
        {
            MessageBox.ASSERT("In Checking.................................");
            timeStart = zeroTimeLimitDay;
        }
        //-----------------


        int RechangeNum = 0;
        for (int i = 0; i < payRecordList.Count; i++)
        {
            //TsuComment
            //if (payRecordList[i].time >= timeStart && payRecordList[i].time < timeEnd)
            //    RechangeNum += (glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC((int)(payRecordList[i].payId)) as PccaccyDT).iPccaccyNum;
            //TsuCode
            if (payRecordList[i].time >= timeStart && payRecordList[i].time < timeEnd)
            {

                if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_Pay)
                {
                    PccaccyDT pay = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(payRecordList[i].payId);
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    int payInt = pay.iPayCount;
                    RechangeNum += payInt;
                }
                else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_LevelGift)
                {
                    LevelGiftDT pay = (LevelGiftDT)glo_Main.GetInstance().m_SC_Pool.m_LevelGiftSC.f_GetSC(payRecordList[i].payId);
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    int payInt = pay.iPayCount;
                    RechangeNum += payInt;
                }
                else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_TripleMoney)
                {
                    TripleMoneyDT pay = (TripleMoneyDT)glo_Main.GetInstance().m_SC_Pool.m_TripleMoneySC.f_GetSC(payRecordList[i].payId);
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    int payInt = pay.iPayCount;
                    RechangeNum += payInt;
                }
                else if ((ePayFromConfig)payRecordList[i].ePayFromConfig == ePayFromConfig.ePay_DealsEveryDay)
                {
                    NewYearDealsEveryDayDT pay = (NewYearDealsEveryDayDT)glo_Main.GetInstance().m_SC_Pool.m_NewYearDealsEveryDaySC.f_GetSC(payRecordList[i].payId);
                    int payInt = 0;
                    GameParamDT param3 = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(119);

                    if (pay != null)
                    {
                        payInt = pay.iCondition;
                    }
                    else
                    {
                        payInt = param3.iParam1;

                    }
                    //float num = pay.iPccaccyNum * pay.iRate;
                    //int payInt = (int)num;
                    //if (pay.iId == 1 || pay.iId == 2) continue;
                    RechangeNum += payInt;
                }
                else
                {
                    GameParamDT param2 = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(116);
                    int payInt = param2.iParam1 * 5;
                    RechangeNum += payInt;
                }
            }
            //
        }
        return RechangeNum;
    }
    #endregion TsuCode - Func


    #region TsuCode - kim phieu
    public void RechargeCoin(int id, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RechargeCoin, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RechargeCoin, bBuf);
    }

    public void RechargeTripleMoney(int iIdEventTime, int iIdVoteApp, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RechargeTripleMoney, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iIdEventTime);
        tCreateSocketBuf.f_Add(iIdVoteApp);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RechargeTripleMoney, bBuf);
    }

    public void RechargeLevelGift(int iIdEventTime, int iIdVoteApp, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RechargeLevelGift, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iIdEventTime);
        tCreateSocketBuf.f_Add(iIdVoteApp);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RechargeLevelGift, bBuf);
    }

    public void BuyCoinSDK(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_BuyCoinSDK, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        //tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_BuyCoinSDK, bBuf);
    }

    public void RechargeBattlePass(int iIdEventTime,int idinfo, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RechargeBattlePass, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iIdEventTime);
        tCreateSocketBuf.f_Add(idinfo);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RechargeBattlePass, bBuf);
    }

    public void RechargeScoreBattlePass(int iIdEventTime, int count, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RechargeScoreBattlePass, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iIdEventTime);
        tCreateSocketBuf.f_Add(count);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RechargeScoreBattlePass, bBuf);
    }
    #endregion TsuCode - kim phieu
}
