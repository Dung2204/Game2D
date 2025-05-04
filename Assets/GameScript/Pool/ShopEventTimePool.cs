using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 商店抽牌pool
/// </summary>
public class ShopEventTimePool : BasePool
{
    public ShopEventTimePool() : base("ShopEventTimePoolDT", true)
    {

    }
    
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
    }
    
    protected override void RegSocketMessage()
    {
        ShopEventTimeInfo tShopEventTimeInfo = new ShopEventTimeInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ShopEventTimeInfo, tShopEventTimeInfo, _Callback_SocketData_Update);
        CMsg_SC_BuyShopEventTime tSC_BuyShopEventTime = new CMsg_SC_BuyShopEventTime();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_BuyShopEventTime, tSC_BuyShopEventTime, _Callback_SC_BuyShopEventTime);
    }
    private void _Callback_SocketData_Update(object value)
    {
        ShopEventTimeInfo tServerInfo = (ShopEventTimeInfo)value;
        int key = tServerInfo.ieventtime;// InitKey(EventTimeInfo.ieventtime, EventTimeInfo.ievent);

        ShopEventTimePoolDT tPoolDataDT = (ShopEventTimePoolDT)f_GetForId(key);
        if (tPoolDataDT == null)
        {
            tPoolDataDT = new ShopEventTimePoolDT();
            tPoolDataDT.iId = key;
            tPoolDataDT.IEventTimeId = tServerInfo.ieventtime;
            tPoolDataDT.idata1 = tServerInfo.idata1;
            tPoolDataDT.idata2 = tServerInfo.idata2;
            tPoolDataDT.idata3 = tServerInfo.idata3;
            tPoolDataDT.idata4 = tServerInfo.idata4;
            tPoolDataDT.m_AwardPoolDT = new Dictionary<int, ShopEventTimeAwardPoolDT>();
            ShopEventAwardInfo info = tServerInfo.info;
            int num = tServerInfo.info.num;
            for (int i = 0; i < num; i++)
            {
                ShopEventAward award = (ShopEventAward)info.awards[i];
                ShopEventTimeAwardPoolDT tempData;// = new ShopEventTimeAwardPoolDT();
                if (tPoolDataDT.m_AwardPoolDT.TryGetValue(award.awardid, out tempData))
                {
                    tempData.IAwardId = award.awardid;
                    tempData.idata1 = award.idata1;
                    tempData.idata2 = award.idata2;
                    tempData.idata3 = award.idata3;
                    tempData.idata4 = award.idata4;
                    if (tempData.idata1 >= tempData.m_AwardDT.iLimit)
                    {
                        tempData.zzIndex = 999999;
                    }
                    else
                    {
                        tempData.zzIndex = 0;
                    }
                    tPoolDataDT.m_AwardPoolDT[award.awardid] = tempData;
                }
                else
                {
                    tempData = new ShopEventTimeAwardPoolDT();
                    tempData.IAwardId = award.awardid;
                    tempData.idata1 = award.idata1;
                    tempData.idata2 = award.idata2;
                    tempData.idata3 = award.idata3;
                    tempData.idata4 = award.idata4;
                    if (tempData.idata1 >= tempData.m_AwardDT.iLimit)
                    {
                        tempData.zzIndex = 999999;
                    }
                    else
                    {
                        tempData.zzIndex = 0;
                    }
                    tPoolDataDT.m_AwardPoolDT.Add(award.awardid, tempData);
                }
                
                
            }
            f_Save(tPoolDataDT);
        }
        else
        {
            tPoolDataDT.idata1 = tServerInfo.idata1;
            tPoolDataDT.idata2 = tServerInfo.idata2;
            tPoolDataDT.idata3 = tServerInfo.idata3;
            tPoolDataDT.idata4 = tServerInfo.idata4;
            tPoolDataDT.m_AwardPoolDT = new Dictionary<int, ShopEventTimeAwardPoolDT>();
            ShopEventAwardInfo info = tServerInfo.info;
            int num = tServerInfo.info.num;
            for (int i = 0; i < num; i++)
            {
                ShopEventAward award = (ShopEventAward)info.awards[i];
                ShopEventTimeAwardPoolDT tempData;// = new ShopEventTimeAwardPoolDT();
                if (tPoolDataDT.m_AwardPoolDT.TryGetValue(award.awardid, out tempData))
                {
                    tempData.IAwardId = award.awardid;
                    tempData.idata1 = award.idata1;
                    tempData.idata2 = award.idata2;
                    tempData.idata3 = award.idata3;
                    tempData.idata4 = award.idata4;
                    if (tempData.idata1 >= tempData.m_AwardDT.iLimit)
                    {
                        tempData.zzIndex = 999999;
                    }
                    else
                    {
                        tempData.zzIndex = 0;
                    }
                    tPoolDataDT.m_AwardPoolDT[award.awardid] = tempData;
                }
                else
                {
                    tempData = new ShopEventTimeAwardPoolDT();
                    tempData.IAwardId = award.awardid;
                    tempData.idata1 = award.idata1;
                    tempData.idata2 = award.idata2;
                    tempData.idata3 = award.idata3;
                    tempData.idata4 = award.idata4;
                    if (tempData.idata1 >= tempData.m_AwardDT.iLimit)
                    {
                        tempData.zzIndex = 999999;
                    }
                    else
                    {
                        tempData.zzIndex = 0;
                    }
                    tPoolDataDT.m_AwardPoolDT.Add(award.awardid, tempData);
                }
            }
        }

    }
    private void _Callback_SC_BuyShopEventTime(object value)
    {
        //CMsg_SC_BuyShopEventTime
        CMsg_SC_BuyShopEventTime tServerInfo = (CMsg_SC_BuyShopEventTime)value;
        ShopEventTimePoolDT dt = f_GetForId(tServerInfo.eventId) as ShopEventTimePoolDT;
        if (dt != null)
        {
            for (int i = 0; i < dt.m_AwardPoolDT.Count; i++)
            {
                ShopEventTimeAwardPoolDT Value = dt.m_AwardPoolDT.ElementAt(i).Value;
                if (Value.IAwardId == tServerInfo.awardId)
                {
                    Value.idata1 = tServerInfo.count;
                    if (Value.idata1 >= Value.m_AwardDT.iLimit)
                    {
                        Value.zzIndex = 999999;
                    }
                    else
                    {
                        Value.zzIndex = 0;
                    }
                    break;
                }
            }
        }
        // show quà
        List<AwardPoolDT> tAwardPoolList = new List<AwardPoolDT>();
        for(int i =0;i< tServerInfo.num; i++)
        {
            SC_Award item = tServerInfo.awards[i];
            AwardPoolDT taward = new AwardPoolDT();
            taward.f_UpdateByInfo(item.resourceType, item.resourceId, item.resourceNum);
            tAwardPoolList.Add(taward);
        }
        //foreach (SC_Award item in aData)
        //{
        //    AwardPoolDT taward = new AwardPoolDT();
        //    taward.f_UpdateByInfo(item.resourceType, item.resourceId, item.resourceNum);
        //    tAwardPoolList.Add(taward);
        //}
        if (tAwardPoolList.Count > 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tAwardPoolList });
            tAwardPoolList.Clear();
        }
    }
    public void f_Buy(int eventId, int awardid, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_BuyShopEventTime, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((int)eventId);
        tCreateSocketBuf.f_Add((int)awardid);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_BuyShopEventTime, bBuf);
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        //f_Socket_UpdateData(Obj);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
   
    public bool IsOpen()
    {
        List<BasePoolDT<long>> allShop = f_GetAll();
        for (int i = 0; i < allShop.Count; i++)
        {
            ShopEventTimePoolDT dt = allShop[i] as ShopEventTimePoolDT;
            if(dt.IEventTimeId > 0 && GameSocket.GetInstance().f_GetServerTime() >= dt.idata1 && GameSocket.GetInstance().f_GetServerTime() < dt.idata2)
            {
                return true;
            }
            
        }
        return false;
    }

    public bool IsOpenById(int id)
    {
        ShopEventTimePoolDT dt = f_GetForId(id) as ShopEventTimePoolDT;
        if (dt != null)
        {
            if (dt.IEventTimeId > 0 && GameSocket.GetInstance().f_GetServerTime() >= dt.idata1 && GameSocket.GetInstance().f_GetServerTime() < dt.idata2)
            {
                return true;
            }
        }
        
        return false;
    }

    public List<BasePoolDT<long>> GetList(int id)
    {
        List<BasePoolDT<long>> m_List = new List<BasePoolDT<long>>();
        ShopEventTimePoolDT dt = f_GetForId(id) as ShopEventTimePoolDT;
        if (dt != null)
        {
            for (int i = 0; i < dt.m_AwardPoolDT.Count; i++)
            {
                ShopEventTimeAwardPoolDT Value = dt.m_AwardPoolDT.ElementAt(i).Value;
                if(Value.idata1 >= Value.m_AwardDT.iLimit)
                {
                    Value.zzIndex = 999999;
                }
                m_List.Add(Value);
            }
        }
        m_List.Sort(delegate (BasePoolDT<long> value1, BasePoolDT<long> value2)
        {
            ShopEventTimeAwardPoolDT item1 = (ShopEventTimeAwardPoolDT)value1;
            ShopEventTimeAwardPoolDT item2 = (ShopEventTimeAwardPoolDT)value2;
            return item1.zzIndex - item2.zzIndex;
        }
        );
        return m_List;
    }
}
