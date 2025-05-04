﻿using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
//TsuCode
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Text;
using System.IO;
//


public enum EM_SocketStatic
{
    OffLine = 0,
    Connecting = 1,
    ConnectSuc = 2,
    Logining = 3,
    OnLine = 4,
    LoginEro = 5,
}


public class BaseSocket
{
    public string m_strTTTT = "";
    private int m_NewServerTime;

    class sCommandReturn
    {
        public SocketCallbackDT m_SocketCallbackDT = new SocketCallbackDT();
        public bool m_bAlive;
    }
    class CatchBuf
    {
        public CatchBuf()
        {
            m_fSleepTime = 0;
            bytes = null;
        }
        public float m_fSleepTime;
        public byte[] bytes;
    }
#if UNITY_WEBPLAYER
     private ccSoket m_Socket = new ccSoket(true);
#else
    private ccSoket m_Socket = new ccSoket();
#endif

    public System.DateTime m_dtSocketTimeout;
    private bool _bLoginSuc = false;
    private bool m_bSendConnectEro = false;
    private EM_SocketStatic _EM_SocketStatic = EM_SocketStatic.OffLine;
    protected ccMachineManager _SocketMachineManger = null;
    private Socket_Loop _Socket_Loop = null;
    
    private int m_iId = 0;

    private SocketState m_SocketState = new SocketState();    
    private ccSocketMessagePool m_GMSocketMessagePool = new ccSocketMessagePool();
    private ccMessagePoolV2 m_MessagePool = new ccMessagePoolV2();

    private ccCallback m_LoginCallbackFunc = null;
    
    private float m_fLastTime;

    public BaseSocket()
    {
        InitMessage();
        InitMachine();
    }

    protected virtual void InitMachine()
    {
       
    }

    public ccSoket f_GetSocket()
    {
        return m_Socket;
    }

    ///// <summary>
    ///// 连接服务器
    ///// </summary>
    //public void f_ConnectServer(ccCallback callback)
    //{
    //    Socket_Connect tSocket_Connect = (Socket_Connect)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Connect);
    //    tSocket_Connect.f_SetCallback(callback);
    //    Socket_Loop tSocket_Loop = (Socket_Loop)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Loop);
    //    _SocketMachineManger.f_ChangeState(tSocket_Connect, tSocket_Loop);
    //}
    public bool Connect(string strSvrIP, int iPort, aaaa222235 func, bool bCloseThread, ccCallback tccCallback = null, int iTime = 10000)
    {
        return m_Socket.Connect(strSvrIP, iPort, func, bCloseThread, tccCallback, iTime);
    }

    public virtual void f_Close()
    {
        _EM_SocketStatic = EM_SocketStatic.OffLine;
        //MessageBox.DEBUG("关闭连接...");
        Debug.LogError("================="+ m_strTTTT+ "   Đóng kết nối=================");
        m_Socket.Close();
    }

    public bool f_TestSocket()
    {
        if (m_Socket.f_TestSocket() || Application.internetReachability != NetworkReachability.NotReachable)
        {
            return true;
        }
        Debug.LogError("m_Socket.f_TestSocket(): " + m_Socket.f_TestSocket());
        Debug.LogError("Application.internetReachability != NetworkReachability.NotReachable: " + Application.internetReachability);
        return false;
    }

    public void f_SetSocketStatic(EM_SocketStatic tEM_SocketStatic)
    {
        //Debug.LogError("f_SetSocketStatic " + _EM_SocketStatic.ToString() + ">>>" + tEM_SocketStatic.ToString() + "    " + m_strTTTT);
        _EM_SocketStatic = tEM_SocketStatic;
    }

    public EM_SocketStatic f_GetSocketStatic()
    {
        //Debug.LogError("f_GetSocketStatic " + _EM_SocketStatic.ToString()+"    "+m_strTTTT);
        return _EM_SocketStatic;
    }

    #region 内部消息处理

    public virtual void Destroy()
    {
        f_Close();
        m_aTTTTT.Clear();
        m_aRRRRR.Clear();
        m_aTTTTT = null;
        m_aRRRRR = null;
        m_aOutCatchBuf.Clear();
        m_aOutCatchBuf = null;
        
    }

    public void f_AddListener_EndString(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_EndString handler)
    {
        m_GMSocketMessagePool.f_AddListenerEndString(tSocketCommand.ToString(), tSockBaseDT, handler, null);
    }

    public void f_RegMessage(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback ccCallbackSuc, int teMsgOperateType = (int)eMsgOperateType.OT_NULL, ccCallback ccCallbackFail = null, bool bAlive = true)
    {
        f_AddListener(iSocketCommand, tSockBaseDT, ccCallbackSuc, 0, bAlive);
        if (teMsgOperateType != (int)eMsgOperateType.OT_NULL)
        {
            if (ccCallbackFail == null)
            {
MessageBox.ASSERT("The specified command returned null result " + teMsgOperateType.ToString());
            }
            f_RegCommandReturn(teMsgOperateType, null, ccCallbackFail);
        }
    }

    public void f_RegMessage_Int0_EndString(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback_EndString ccCallbackSuc)
    {
        f_AddListener_EndString(iSocketCommand, tSockBaseDT, ccCallbackSuc);
    }

    public void f_AddListener_Buf_Int2_V2(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Buf_Int2_V2 handler)
    {
        m_GMSocketMessagePool.f_AddListener_Buf_Int2_V2(iSocketCommand.ToString(), tSockBaseDT, handler, null);
    }


    public void f_RegMessage_Int0(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 ccCallbackSuc,
       int tEroeMsgOperateType = (int)eMsgOperateType.OT_NULL, ccCallback ccCallbackFail = null)
    {
        f_AddListener_Int0_V2(iSocketCommand, tSockBaseDT, ccCallbackSuc, null);
        if (tEroeMsgOperateType != (int)eMsgOperateType.OT_NULL)
        {
            if (ccCallbackFail == null)
            {
MessageBox.ASSERT("The specified command returned null result " + tEroeMsgOperateType.ToString());
            }
            f_RegCommandReturn(tEroeMsgOperateType, null, ccCallbackFail);
        }
    }

    /// <summary>
    /// 前2位为独立的int,第3位开始为数量
    /// </summary>
    public void f_RegMessage_Int2(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 ccCallbackSuc,
       int tEroeMsgOperateType = (int)eMsgOperateType.OT_NULL, ccCallback ccCallbackFail = null)
    {
        //TsuCode
        if (iSocketCommand == 36112)
            MessageBox.ASSERT("TsuLog : " + iSocketCommand + ":" + tSockBaseDT +" : " + ccCallbackSuc);
        //
        f_AddListener_Int2_V2(iSocketCommand, tSockBaseDT, ccCallbackSuc, null);
        if (iSocketCommand == 36112) MessageBox.ASSERT("TsuLog + " + iSocketCommand + " Suc");
        if (tEroeMsgOperateType != (int)eMsgOperateType.OT_NULL)
        {
            //TsuCode
            if (iSocketCommand == 36112) MessageBox.ASSERT("TsuLog : Fail cmnr");
            ///
            if (ccCallbackFail == null)
            {
MessageBox.ASSERT("The specified command returned null result " + tEroeMsgOperateType.ToString());
            }
            f_RegCommandReturn(tEroeMsgOperateType, null, ccCallbackFail);
        }
    }

    /// <summary>
    /// 前3位为独立的int,第4位开始为数量
    /// </summary>
    public void f_RegMessage_Int3(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int3_V2 ccCallbackSuc,
       int tEroeMsgOperateType = (int)eMsgOperateType.OT_NULL, ccCallback ccCallbackFail = null)
    {
        f_AddListener_Int3_V2(iSocketCommand, tSockBaseDT, ccCallbackSuc, null);
        if (tEroeMsgOperateType != (int)eMsgOperateType.OT_NULL)
        {
            if (ccCallbackFail == null)
            {
MessageBox.ASSERT("The specified command returned null result " + tEroeMsgOperateType.ToString());
            }
            f_RegCommandReturn(tEroeMsgOperateType, null, ccCallbackFail);
        }
    }

    /// <summary>
    /// 前1位为独立的int,第2位开始为数量
    /// </summary>
    public void f_RegMessage_Int1(int iSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 ccCallbackSuc,
       int tEroeMsgOperateType = (int)eMsgOperateType.OT_NULL, ccCallback ccCallbackFail = null)
    {
        f_AddListener_Int1_V2(iSocketCommand, tSockBaseDT, ccCallbackSuc, null);
        if (tEroeMsgOperateType != (int)eMsgOperateType.OT_NULL)
        {
            if (ccCallbackFail == null)
            {
MessageBox.ASSERT("The specified command returned null result " + tEroeMsgOperateType.ToString());
            }
            f_RegCommandReturn(tEroeMsgOperateType, null, ccCallbackFail);
        }
    }

    /// <summary>
    /// 定义游戏消息，并按定义的消息对相应的数据进行拆分和转发
    /// </summary>
    /// <param name="tSocketCommand"></param>
    /// <param name="tSockBaseDT"></param>
    /// <param name="handler"></param>
    /// <param name="pParent"></param>
    /// <param name="iHaveNum"></param>
    public void f_AddListener(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback handler, int iHaveNum = 0, bool bAlive = true)
    {
        m_GMSocketMessagePool.f_AddListener(tSocketCommand.ToString(), tSockBaseDT, handler, null, iHaveNum, bAlive);
    }

    
    public void f_RemoveListener(int tSocketCommand)
    {
        m_GMSocketMessagePool.f_RemoveListener(tSocketCommand.ToString());
    }
    
    /// <summary>
    /// 前0位为独立的int,第1位开始为数量
    /// </summary>
    private void f_AddListener_Int0_V2(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 handler, UnityEngine.Object pParent)
    {
        m_GMSocketMessagePool.f_AddListener_Int0_V2(tSocketCommand.ToString(), tSockBaseDT, handler, pParent);
    }

    /// <summary>
    /// 前1位为独立的int,第2位开始为数量
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="tSockBaseDT"></param>
    /// <param name="handler"></param>
    /// <param name="pParent"></param>
    public void f_AddListener_Int1_V2(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 handler, UnityEngine.Object pParent)
    {
        m_GMSocketMessagePool.f_AddListener_Int1_V2(tSocketCommand.ToString(), tSockBaseDT, handler, pParent);
    }

    /// <summary>
    /// 前2位为独立的int,第3位开始为数量
    /// </summary>
    private void f_AddListener_Int2_V2(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int2_V2 handler, UnityEngine.Object pParent)
    {
        m_GMSocketMessagePool.f_AddListener_Int2_V2(tSocketCommand.ToString(), tSockBaseDT, handler, pParent);
    }

    /// <summary>
    /// 前三位为独立的int,第4位开始为数量
    /// </summary>
    private void f_AddListener_Int3_V2(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Int3_V2 handler, UnityEngine.Object pParent)
    {
        m_GMSocketMessagePool.f_AddListener_Int3_V2(tSocketCommand.ToString(), tSockBaseDT, handler, pParent);
    }

    private void f_AddListener_Buf_V2(int tSocketCommand, SockBaseDT tSockBaseDT, ccCallback_Buf_V2 handler, UnityEngine.Object pParent)
    {
        m_GMSocketMessagePool.f_AddListener_Buf_V2(tSocketCommand.ToString(), tSockBaseDT, handler, pParent);
    }

    public bool f_CheckHaveBuf()
    {
        return m_aOutCatchBuf.Count > 0 ? true : false;
    }

    Queue m_aOutCatchBuf = new Queue();
    Queue m_aTTTTT = new Queue();
    Queue m_aRRRRR = new Queue();
    public void f_Router(int iResult, aaa12334 stHead, byte[] bytes, aaaa343 NetMsgHead, ushort wTemp)
    {
        if (iResult < 0)
        {
Debug.Log(m_strTTTT + " Error data " + iResult);
            return;
        }
        byte[] TmpBytes = new byte[stHead.iBodyLen];
        Array.Copy(bytes, TmpBytes, stHead.iBodyLen);
string ppSQL = m_strTTTT + " " + wTemp + " Return result (" + iResult + ") - Type:" + stHead.iPackType + " Length:" + stHead.iBodyLen + "-" + stHead.iRecvUserID + " -" + stHead.iTemp + "-" + stHead.iTemp1 + "-" + stHead.iTemp2;
        //MessageBox.DEBUG(ppSQL);
        //int iDoLen = DoRouter(stHead, bytes, 0);

        MessageBox.DEBUG(ppSQL);

        m_aTTTTT.Enqueue(stHead);
        m_aRRRRR.Enqueue(TmpBytes);
    }

    private void RouterUpdate()
    {
        while (m_aTTTTT.Count > 0)
        {
            aaa12334 stHead = (aaa12334)m_aTTTTT.Dequeue();
            byte[] bytes = (byte[])m_aRRRRR.Dequeue();

            iRecvNum += 18;
            iRecvNum += bytes.Length;

string ppSQL = m_strTTTT + " Send:" + m_fSendBufSize + " Receive:" + iRecvNum + " Receive type: " + stHead.iPackType + " Length:" + stHead.iBodyLen + "-" + stHead.iRecvUserID + "-" + stHead.iTemp + "-" + stHead.iTemp1 + "-" + stHead.iTemp2;
            MessageBox.DEBUG(ppSQL);

            m_dtSocketTimeout = System.DateTime.Now;
            //TsuCode
            if (stHead.iPackType == 36112)
            {
                CallBack36112(stHead, bytes, 0);
            }
            else
            {
                ////
                if (DoRouter(stHead, bytes, 0) == 0)
                {//未找到对应的处理转到外部方法处理
Debug.LogError(m_strTTTT + " No corresponding handler found " + stHead.iPackType);

                }
                //TsuCode
            }
            //
            if (stHead.iPackType <= 0)
            {
                f_Printf("Sai package ", bytes);
                //m_bRunThread = false;
                MessageBox.ASSERT(m_strTTTT + " EEEEE");
            }
            else
            {
                //MessageBox.DEBUG("OK");
            }

            //f_Printf(stHead.iPackType.ToString(), bytes);
            //MessageBox.DEBUG("发:" + m_fSendBufSize + " 收:" + iRecvNum);
        }
    }
    //////TsuCode
    public void CallBack36112(aaa12334 stHead, byte[] bytes, int iPos)
    {
        MessageBox.ASSERT("TsuLog TsuCallBack36112");
        //SockBaseDT sockBaseDt2 = (SockBaseDT)aaaaaa33333.dfdasdasf(bytes, a.GetType(), 0);
        //int sizeGiDo = Marshal.SizeOf((object)sockBaseDt2);
        //MessageBox.ASSERT("TsuLog Sizee: " + sizeGiDo);
        //
        //move 53bytes to new byte[]

        int arrayLength = (bytes.Length - 16) / 53;
        MessageBox.ASSERT("TsuLog Count: " + bytes.Length + ":" + arrayLength);

        List<TurntableLotteryInfo> listInfo = new List<TurntableLotteryInfo>();
        int posNew = 16;
        var data = new TurntableLotteryInfo();
        ///Convert byte[] -> list TurntableLotteryInfo
        for (int i = 0; i < arrayLength; i++)
        {
            byte[] newBytes = new byte[53];
            int size = 16 + ((i + 1) * 53);
        
            for (int j = posNew; j < size; j++)
            {

                newBytes[j - (i * 53) - 16] = bytes[j];
            }
            //convert bytes to TurntableLotteryInfo
            GCHandle gcHandle = GCHandle.Alloc(newBytes, GCHandleType.Pinned);
            data = (TurntableLotteryInfo)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(TurntableLotteryInfo));
            gcHandle.Free();
            listInfo.Add(data);
            if (posNew < bytes.Length - 53)
            {
                posNew = posNew + 53;
            }
            ///
        }
        ///Update RecordList
        if(listInfo.Count>1)
		    Data_Pool.m_TurntablePool.m_RecordList.Clear();
        for (int i = 0; i < listInfo.Count; i++)
        {

            //TurntablePool tn = new TurntablePool();
            //tn.m_RecordList.Insert(0, turntableInfo);
            Data_Pool.m_TurntablePool.m_RecordList.Add(listInfo[i]);
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_TurntablePage_UpdateRecord);

        ////////////////////////
        //
        //byte[] newBytes = new byte[53];

        //for (int j = 16; j < 69; j++)
        //{
        //    newBytes[j - 16] = bytes[j];
        //}
        ////convert bytes to TurntableLotteryInfo
        //GCHandle gcHandle = GCHandle.Alloc(newBytes, GCHandleType.Pinned);
        //var data = (TurntableLotteryInfo)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(TurntableLotteryInfo));
        //gcHandle.Free();

        //Data_Pool.m_TurntablePool.m_RecordList.Add(data);
        //MessageBox.ASSERT("TsuLog Size: " + Data_Pool.m_TurntablePool.m_RecordList.Count);
        //glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_TurntablePage_UpdateRecord);
        ////////////
    }
    ///////////////////

    private int iSendNum;
    private int iRecvNum;
    private float m_fSendBufSize = 0;
    private CatchBuf _CurSendCatchBuf = null;
    public virtual void f_Update()
    {

        RouterUpdate();
        m_MessagePool.f_Update();
        _SocketMachineManger.f_Update();
        
    }

    private int DoRouter(aaa12334 stHead, byte[] bytes, int iPos)
    {
      
        return m_GMSocketMessagePool.f_Router(stHead, bytes, iPos);
    }

    public void f_DispSendCatchBuf()
    {
        if (m_aOutCatchBuf.Count > 0)
        {
            if (_CurSendCatchBuf == null)
            {
                _CurSendCatchBuf = (CatchBuf)m_aOutCatchBuf.Dequeue();
            }      
            iSendNum = m_Socket.f_Send(_CurSendCatchBuf.bytes);
            if (-1 == iSendNum)
            {
MessageBox.DEBUG(m_strTTTT + " Send failed " + m_aOutCatchBuf.Count);
                //Debug.LogError(m_strTTTT + " 发送失败 " + m_aOutCatchBuf.Count);
                f_Close();
                return;
            }
            _CurSendCatchBuf = null;
MessageBox.DEBUG(m_strTTTT + " Send: " + iSendNum);
            //Debug.LogError(m_strTTTT + " 发送: " + iSendNum);
        }
    }

    public int f_SendBuf(SocketCommand tSocketCommand, byte[] bBodyBuf)
    {
        return f_SendBuf((int)tSocketCommand, bBodyBuf);
    }

    public int f_SendBuf(ChatSocketCommand tSocketCommand, byte[] bBodyBuf)
    {
        return f_SendBuf((int)tSocketCommand, bBodyBuf);
    }

    public int f_SendBuf(LegionSocketCmd tSocketCommand, byte[] bBodyBuf)
    {
        return f_SendBuf((int)tSocketCommand, bBodyBuf);
    }

    public int f_SendBuf(int wAssistantCmd, byte[] bBodyBuf)
    {
        byte[] bytes = m_Socket.f_CreateSocketPack(wAssistantCmd, bBodyBuf);
        CatchBuf tCatchBuf = new CatchBuf();
        tCatchBuf.bytes = bytes;
        m_aOutCatchBuf.Enqueue(tCatchBuf);
        //Debug.Log(string.Format("发:{0} 长度{1}",wAssistantCmd,bBodyBuf.Length));
        return bytes.Length;
    }

    public int f_SendBuf2Force(int wAssistantCmd, byte[] bBodyBuf)
    {
        return m_Socket.f_SendBuf(wAssistantCmd, bBodyBuf);
    }

    public void f_Printf(string strHead, byte[] bytes)
    {
        string ppSQL = strHead + " >>>>> ";
        int iLen = bytes.Length / 4;
        for (int i = 0; i < iLen; i++)
        {
            int DDD = BitConverter.ToInt32(bytes, i * 4);
            ppSQL = ppSQL + string.Format(" {0} ", DDD);
        }
        ppSQL = ppSQL + " >>> ";
        MessageBox.DEBUG(ppSQL);
    }

    #endregion

    #region 外部消息处理

    private bool _bInitMessage = false;
    protected virtual void InitMessage()
    {
        if (_bInitMessage)
        {
            return;
        }
        _bInitMessage = true;
        
    }

    protected virtual void UnInitMessage()
    {
        m_GMSocketMessagePool.f_Clear();
        m_GMSocketMessagePool = null;
    }
    #endregion


    #region 操作返回

    private Dictionary<int, List<sCommandReturn>> _aGameCommandReturn = new Dictionary<int, List<sCommandReturn>>();
    public void f_RegCommandReturn(int tEroeMsgOperateType, ccCallback tccCallbackSuc, ccCallback tccCallbackFail, bool bAlive = false)
    {
        List<sCommandReturn> ttList = null;
        sCommandReturn tsCommandReturn = new sCommandReturn();
        tsCommandReturn.m_SocketCallbackDT.m_ccCallbackSuc = tccCallbackSuc;
        tsCommandReturn.m_SocketCallbackDT.m_ccCallbackFail = tccCallbackFail;
        tsCommandReturn.m_bAlive = bAlive;
        if (_aGameCommandReturn.TryGetValue(tEroeMsgOperateType, out ttList))
        {
            ttList.Add(tsCommandReturn);
        }
        else
        {
            ttList = new List<sCommandReturn>();
            ttList.Add(tsCommandReturn);
            _aGameCommandReturn.Add((int)tEroeMsgOperateType, ttList);
        }
    }

    public void f_UnRegCommandReturnAll(int tEroeMsgOperateType)
    {
        List<sCommandReturn> ttList = null;
        if (_aGameCommandReturn.TryGetValue(tEroeMsgOperateType, out ttList))
        {
            _aGameCommandReturn.Remove((int)tEroeMsgOperateType);
        }
    }

    public void f_UnRegCommandReturn(int tEroeMsgOperateType, ccCallback tccCallbackSuc, ccCallback tccCallbackFail)
    {
        List<sCommandReturn> ttList = null;
        if (_aGameCommandReturn.TryGetValue(tEroeMsgOperateType, out ttList))
        {
            foreach (sCommandReturn tsCommandReturn in ttList)
            {
                if (tsCommandReturn.m_SocketCallbackDT.m_ccCallbackFail == tccCallbackFail && tsCommandReturn.m_SocketCallbackDT.m_ccCallbackSuc == tccCallbackSuc)
                {
                    ttList.Remove(tsCommandReturn);
                    return;
                }
            }
        }
    }

    List<sCommandReturn> _aWaitDelList = new List<sCommandReturn>();
    protected void On_CMsg_GameCommandReturn(object Obj)
    {
        if (Obj == null)
        {
            return;
        }
        stGameCommandReturn tstGameCommandReturn = (stGameCommandReturn)Obj;

        //if (GloData.m_bDebug)
        //{
        //    eMsgOperateType teMsgOperateType = (eMsgOperateType)tstGameCommandReturn.iCommand;
        //    if (tstGameCommandReturn.iResult != 0)
        //    {
        //        MessageBox.DEBUG("操作失败：" + tstGameCommandReturn.iCommand + ">>" + teMsgOperateType.ToString() + " >> " + tstGameCommandReturn.iResult);
        //    }
        //}

        List<sCommandReturn> ttList = null;
        if (_aGameCommandReturn.TryGetValue(tstGameCommandReturn.iCommand, out ttList))
        {
            _aWaitDelList.Clear();
            //foreach (sCommandReturn tsCommandReturn in ttList)
            for(int i = 0; i < ttList.Count; i++)
            {
                sCommandReturn tsCommandReturn = ttList[i];
                if (tstGameCommandReturn.iResult == (int)eMsgOperateResult.OR_Succeed)
                {
                    if (tsCommandReturn.m_SocketCallbackDT.m_ccCallbackSuc != null)
                    {
                        tsCommandReturn.m_SocketCallbackDT.m_ccCallbackSuc(tstGameCommandReturn.iResult);
                    }
                }
                else
                {
                    if (tsCommandReturn.m_SocketCallbackDT.m_ccCallbackFail != null)
                    {
                        tsCommandReturn.m_SocketCallbackDT.m_ccCallbackFail(tstGameCommandReturn.iResult);
                    }
                }
                if (!tsCommandReturn.m_bAlive)
                {
                    _aWaitDelList.Add(tsCommandReturn);
                }
            }
            foreach (sCommandReturn tsCommandReturn in _aWaitDelList)
            {
                ttList.Remove(tsCommandReturn);
            }
        }

    }
    #endregion



    #region 时间

    public virtual void f_Ping()
    {
       
    }

    protected void On_Ping(object Obj)
    {
        if (Obj == null)
        {
            return;
        }
        basicNode1 tPing = (basicNode1)Obj;
        UpdateServerTime(tPing.value1);
MessageBox.DEBUG(m_strTTTT + " Ping returns " + tPing.value1);
    }

    System.DateTime _LastUpdateTime;
    public void UpdateServerTime(int iTime)
    {
        if (m_strTTTT == "GameSocket")
        {
            StaticValue.m_iNewServerTime = iTime;
        }
        m_NewServerTime = iTime;
        _LastUpdateTime = System.DateTime.Now;
        if (_Socket_Loop == null)
        {
            _Socket_Loop = (Socket_Loop)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Loop);
        }
        _Socket_Loop.f_UpdateTestTime();
    }


    public int f_GetServerTime()
    {
        System.TimeSpan tt = System.DateTime.Now - _LastUpdateTime;
        int second = tt.Seconds;
        return m_NewServerTime + second - 5;        // 延迟Time.deltaTime;
    }

    #endregion



}//END Class
