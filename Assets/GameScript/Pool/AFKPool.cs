using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;

public class AFKPool : BasePool
{
    public long afkTime = 0;
    public int isFirst = 0;
    public List<AwardPoolDT> awardList = new List<AwardPoolDT>();
    public AFKPool() : base("TurntablePoolDT", true)
    {

    }

    protected override void f_Init()
    {
        //List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_AFKAwardSC.f_GetAll();
        //for (int i = 0; i < listDT.Count; i++)
        //{
        //    AFKAwardDT dt = listDT[i] as AFKAwardDT;
        //    AFKAwardPoolDT poolDT = new AFKAwardPoolDT();
        //    poolDT.iId = dt.iId;
        //    poolDT.m_AFKAwardData = dt;
        //    f_Save(poolDT);
        //}
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }



    protected override void RegSocketMessage()
    {

        SC_AFKGetAward sC_AFKGetAward = new SC_AFKGetAward();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_AFKGetAward, sC_AFKGetAward, CallbacksAFKGetAward);


        SC_AFKInfo scTurntableBoxInfo = new SC_AFKInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_AFKInfo, scTurntableBoxInfo, CallbackscAFKInfo);

    }


    private void CallbackscAFKInfo(object data)
    {
        SC_AFKInfo info = (SC_AFKInfo)data;
        afkTime = info.afkTime;
        isFirst = info.isFirst;
    }

    private void CallbacksAFKGetAward(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    {
        MessageBox.ASSERT("TsuLog check AFK get Award CallbacksAFKGetAward ");
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        if (awardList != null) awardList.Clear();
        for (int i = 0; i < aData.Count; i++)
        {
            SC_AFKGetAward node = (SC_AFKGetAward)aData[i];
            AwardPoolDT poolDT = new AwardPoolDT();
            poolDT.f_UpdateByInfo((byte)node.awardType, node.awardId, node.awardNum);
            awardList.Add(poolDT);
        }
        if (awardList.Count >= 1)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        }
    }
    public void f_QueryAFKTimeInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_AFKTimeInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_AFKTimeInfo, bBuf);
    }
    public void f_AFKGetAward(SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_AFKGetAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_AFKGetAward, bBuf);
    }

}