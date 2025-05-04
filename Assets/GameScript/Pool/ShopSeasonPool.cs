using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 商店抽牌pool
/// </summary>
public class ShopSeasonPool : BasePool
{
    private long iEndTime = 0;
    public long m_iEndTime
    {
        get{
            return iEndTime;
        }
    }
    public ShopSeasonPool() : base("ShopSeasonAwardPoolDT", true)
    {

    }
    
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        InitShop();
    }

    private void InitShop()
    {
        List<NBaseSCDT> allShop = glo_Main.GetInstance().m_SC_Pool.m_ShopSeasonAwardSC.f_GetAll();
        for (int i = 0; i < allShop.Count; i++)
        {
            ShopSeasonAwardDT dt = allShop[i] as ShopSeasonAwardDT;
            if (dt != null)
            {
                ShopSeasonAwardPoolDT poolDt = new ShopSeasonAwardPoolDT();
                poolDt.iId = dt.iId;
                poolDt.IAwardId = dt.iId;
                poolDt.idata1 = 0;
                poolDt.idata2 = 0;
                poolDt.idata3 = 0;
                poolDt.idata4 = 0;
                poolDt.zzIndex = 0;
               
                f_Save(poolDt);
            }
        }
    }
    protected override void RegSocketMessage()
    {
        CMsg_SC_ShopSeasonInfo tShopInfo = new CMsg_SC_ShopSeasonInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ShopSeasonInfo, tShopInfo, Callback_Info_Update);
        SC_Award tSC_BuyShop = new SC_Award();
        GameSocket.GetInstance().f_RegMessage_Int2((int)SocketCommand.SC_BuyShopSeason, tSC_BuyShop, Callback_SC_BuyShop);
       
    }

    protected void Callback_Info_Update(object Obj)
    {
        CMsg_SC_ShopSeasonInfo tServerInfo = (CMsg_SC_ShopSeasonInfo)Obj;

        this.iEndTime = tServerInfo.endTime;
        //this.iEndWeek = tServerInfo.endWeek;
        //this.iEndMonth = tServerInfo.endMonth;
        for(int i=0;i< tServerInfo.num; i++)
        {
            ShopEventAward tData = tServerInfo.awards[i];
            f_Socket_UpdateData(tData);
        }
    }
    private void Callback_SC_BuyShop(int data1, int idata2, int iNum, ArrayList aData)
    {
        ShopSeasonAwardPoolDT tPoolDataDT = (ShopSeasonAwardPoolDT)f_GetForId(data1);
        if (tPoolDataDT == null)
        {
            tPoolDataDT = new ShopSeasonAwardPoolDT();
            tPoolDataDT.IAwardId = data1;
            tPoolDataDT.idata1 = idata2;
            tPoolDataDT.idata2 = 0;
            tPoolDataDT.idata3 = 0;
            tPoolDataDT.idata4 = 0;
            f_Save(tPoolDataDT);
        }
        else
        {
            tPoolDataDT.idata1 = idata2;
        }
        List<AwardPoolDT> tAwardPoolList = new List<AwardPoolDT>();
        foreach (SC_Award item in aData)
        {
            AwardPoolDT taward = new AwardPoolDT();
            taward.f_UpdateByInfo(item.resourceType, item.resourceId, item.resourceNum);
            tAwardPoolList.Add(taward);
        }
        if (tAwardPoolList.Count > 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tAwardPoolList });
            tAwardPoolList.Clear();
        }
    }
    
    public void f_Buy(int awardid, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_BuyShopSeason, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((int)awardid);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_BuyShopSeason, bBuf);
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        f_Socket_UpdateData(Obj);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        ShopEventAward tServerInfo = (ShopEventAward)Obj;
        ShopSeasonAwardPoolDT tPoolDataDT = (ShopSeasonAwardPoolDT)f_GetForId(tServerInfo.awardid);
        if (tPoolDataDT == null)
        {
            tPoolDataDT = new ShopSeasonAwardPoolDT();
            tPoolDataDT.iId = tServerInfo.awardid;
            tPoolDataDT.IAwardId = tServerInfo.awardid;
            tPoolDataDT.idata1 = tServerInfo.idata1;
            tPoolDataDT.idata2 = tServerInfo.idata2;
            tPoolDataDT.idata3 = tServerInfo.idata3;
            tPoolDataDT.idata4 = tServerInfo.idata4;
            if (tPoolDataDT.idata1 >= tPoolDataDT.m_PoolDT.iLimit)
            {
                tPoolDataDT.zzIndex = 999999;
            }
            else
            {
                tPoolDataDT.zzIndex = 0;
            }
            f_Save(tPoolDataDT);
        }
        else
        {
            tPoolDataDT.idata1 = tServerInfo.idata1;
            tPoolDataDT.idata2 = tServerInfo.idata2;
            tPoolDataDT.idata3 = tServerInfo.idata3;
            tPoolDataDT.idata4 = tServerInfo.idata4;
            if (tPoolDataDT.idata1 >= tPoolDataDT.m_PoolDT.iLimit)
            {
                tPoolDataDT.zzIndex = 999999;
            }
            else
            {
                tPoolDataDT.zzIndex = 0;
            }
        }
    }
   
    public List<BasePoolDT<long>> GetList()
    {
        List<BasePoolDT<long>> m_Result = new List<BasePoolDT<long>>();
        List<BasePoolDT<long>> m_List = f_GetAll();
        for (int i = 0; i < m_List.Count; i++)
        {
            ShopSeasonAwardPoolDT item = (ShopSeasonAwardPoolDT)m_List[i];
            if (item.idata1 >= item.m_PoolDT.iLimit)
            {
                item.zzIndex = 999999;
            }
            else
            {
                item.zzIndex = 0;
            }
            m_Result.Add(item);
            //if (item.m_PoolDT.iVip <= UITool.f_GetNowVipLv())
            //{
            //    m_Result.Add(item);
            //}
        }

        m_Result.Sort(delegate (BasePoolDT<long> value1, BasePoolDT<long> value2)
        {
            ShopSeasonAwardPoolDT item1 = (ShopSeasonAwardPoolDT)value1;
            ShopSeasonAwardPoolDT item2 = (ShopSeasonAwardPoolDT)value2;
            return item1.zzIndex - item2.zzIndex;
        }
        );
        return m_Result;
    }

}
