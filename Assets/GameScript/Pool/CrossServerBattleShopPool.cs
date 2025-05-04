using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 跨服战商店
/// </summary>
public class CrossServerBattleShopPool : BasePool
{
    private bool m_IsShopInit = false;

    private List<BasePoolDT<long>> m_CurLvShopList;

    public List<BasePoolDT<long>> ShopList
    {
        get
        {
            return m_CurLvShopList;
        }
    }

    public CrossServerBattleShopPool() : base("CrossServerBattleShopPoolDT")
    {
        
    }

    protected override void f_Init()
    {
        m_CurLvShopList = new List<BasePoolDT<long>>();
        List<NBaseSCDT> tInitList = glo_Main.GetInstance().m_SC_Pool.m_CrossServerBattleShopSC.f_GetAll();
        for (int i = 0; i < tInitList.Count; i++)
        {
            CrossServerBattleShopDT tInitNode = (CrossServerBattleShopDT)tInitList[i];
            f_Save(new CrossServerBattleShopPoolDT(tInitNode));
        }
        f_SortData1();
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.PlayerLvUpdate, f_UpdateDataByLv);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, f_UpdateDataByNextDay); 
    }

    private void f_UpdateDataByLv(object value)
    {
        int lv = (int)value;
        m_CurLvShopList.Clear();
        List<BasePoolDT<long>> tSourceList = f_GetAll();
        for (int i = 0; i < tSourceList.Count; i++)
        {
            CrossServerBattleShopPoolDT tNode = (CrossServerBattleShopPoolDT)tSourceList[i];
            if (tNode.Template.iShowLv <= lv)
            {
                m_CurLvShopList.Add(tNode);
            }
        }
    }

    private void f_UpdateDataByNextDay(object value)
    {
        List<BasePoolDT<long>> tResetList = f_GetAll();
        for (int i = 0; i < tResetList.Count; i++)
        {
            CrossServerBattleShopPoolDT tNode = (CrossServerBattleShopPoolDT)tResetList[i];
            tNode.f_Reset();
        }
    }

    protected override void RegSocketMessage()
    {
        CMsg_SC_GoodInfoNode tSC_GoodNode = new CMsg_SC_GoodInfoNode();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_ShopCrossServInfo, tSC_GoodNode, f_SC_ShopInfoUpdateHandle);
    }

    public void f_SC_ShopInfoUpdateHandle(int value1, int value2, int num, System.Collections.ArrayList arrayList)
    {
        for (int i = 0; i < arrayList.Count; i++)
        {
            CMsg_SC_GoodInfoNode serverData = (CMsg_SC_GoodInfoNode)arrayList[i];
            CrossServerBattleShopPoolDT shopItem = (CrossServerBattleShopPoolDT)f_GetForId(serverData.goodsId);
            if (shopItem != null)
            {
                shopItem.f_UpdateInfo(serverData.times);
            }
            else
            {
MessageBox.ASSERT(string.Format("The server sent item data does not exist,NodeType:{0}; GoodId:{1}", value1, serverData.goodsId));
            }
        }
    }

    public void f_ShopInit(SocketCallbackDT socketCallbackDt)
    {
        if (m_IsShopInit)
        {
            socketCallbackDt.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        m_IsShopInit = true;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopCrossServInfo, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopCrossServInfo, bBuf);
    }

    public void f_Buy(int shopItemId,int buyNum, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopCrossServBuy, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(shopItemId);
        tCreateSocketBuf.f_Add(buyNum);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopCrossServBuy, bBuf);
    }
    

    #region 无用的

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
    }

    #endregion
}
