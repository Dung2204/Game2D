using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 广播消息
/// </summary>
public class BroadcastPool : BasePool
{
    private long index = 0;
    public BroadcastPool() : base("BroadcastPoolDT", true)
    {

    }
    protected override void f_Init()
    {

    }

    protected override void RegSocketMessage()
    {
        RC_OrangeCard rcOrangeCard = new RC_OrangeCard();
        ChatSocket.GetInstance().f_RegMessage((int)ChatSocketCommand.RC_OrangeCard, rcOrangeCard, OnRCOrangeCardCallback);
    }
    private void OnRCOrangeCardCallback(object data)
    {
        RC_OrangeCard rcOrangeCard = (RC_OrangeCard)data;
        BroadcastPoolDT poolDT = new BroadcastPoolDT();
        while (f_GetForId(index) != null)
        {
            index++;
        }
        poolDT.iId = index;
        //poolDT.userid = rcOrangeCard.userId;
        poolDT.cardTempId = rcOrangeCard.cardTempId;
        poolDT.opType = rcOrangeCard.opType;
        poolDT.szName = rcOrangeCard.szName;
        if (f_GetAll().Count>=10) {
            f_GetAll().RemoveAt(0);
        }
        f_Save(poolDT);
        if(StaticValue.m_curScene == EM_Scene.GameMain)
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ServerBroadcast);
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #region 外部接口

    #endregion
}
