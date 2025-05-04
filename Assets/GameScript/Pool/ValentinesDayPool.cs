using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

public class ValentinesDayPool : BasePool
{

    public ValentinesDayPoolDT m_ValentinesDayPoolDT = new ValentinesDayPoolDT();
    public List<AwardPoolDT> m_ValentinesDayAwardList = new List<AwardPoolDT>();
    public ValentinesDayPool() : base("ValentinesDayPoolDT", true)
    {
    }

    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
    }

    protected override void RegSocketMessage()
    {
        SC_ValentineSendRoseInit tSC_ValentineSendRoseInit = new SC_ValentineSendRoseInit();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ValentineSendRoseInit, tSC_ValentineSendRoseInit, _AddMessage);

        SC_CenterAwardNode tSC_CenterAwardNode = new SC_CenterAwardNode();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_ValentineSendRoseRet, tSC_CenterAwardNode, _AddAward);
    }

    private void _AddMessage(object obj)
    {
        SC_ValentineSendRoseInit tSC_ValentineSendRoseInit = (SC_ValentineSendRoseInit)obj;
        m_ValentinesDayPoolDT.m_iGrade = tSC_ValentineSendRoseInit.uScore;
        m_ValentinesDayPoolDT.m_iBoxFlag = tSC_ValentineSendRoseInit.uBoxFlag;
        m_ValentinesDayPoolDT.m_uNum = tSC_ValentineSendRoseInit.uNum;
    }

    private void _AddAward(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    {
        foreach (SC_CenterAwardNode item in aData)
        {
            AwardPoolDT tSC_CenterAwardNode = new AwardPoolDT();
            tSC_CenterAwardNode.f_UpdateByInfo(item.uType, item.uId, item.uNum);
            m_ValentinesDayAwardList.Add(tSC_CenterAwardNode);
        }
    }


    #region 外部接口
    public void f_ValentinesInfo(SocketCallbackDT SocketCallback)
    {
        if (SocketCallback != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ValentineSendRoseInit, SocketCallback.m_ccCallbackSuc, SocketCallback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ValentineSendRoseInit, bBuf);
    }

    public void f_ValentinesGetBox(byte BoxIdx, SocketCallbackDT SocketCallback)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ValentineSendRoseBoxAward, SocketCallback.m_ccCallbackSuc, SocketCallback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(BoxIdx);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ValentineSendRoseBoxAward, bBuf);
    }

    public void f_ValentinesSendRose(byte idx2Whom, int num, SocketCallbackDT SocketCallback)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ValentineSendRose, SocketCallback.m_ccCallbackSuc, SocketCallback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(idx2Whom);
        tCreateSocketBuf.f_Add(num);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ValentineSendRose, bBuf);
    }


    #endregion
}

