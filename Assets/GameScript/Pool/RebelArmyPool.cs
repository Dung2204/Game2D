
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
public class RebelArmyPool : BasePool
{
    public RebelArmyPool() : base("RebelArmyPoolDT", true)
    {
    }

    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        RebelInfo tCresadeRebrlList = (RebelInfo)Obj;
        RebelArmyPoolDT tDateDT = (RebelArmyPoolDT)f_GetForId(tCresadeRebrlList.discovererId);
        if (tDateDT == null)    //为空就添加
        {
MessageBox.DEBUG("Add");
            tDateDT = new RebelArmyPoolDT();
            tDateDT.iId = tCresadeRebrlList.discovererId;

            tDateDT._ITempleteId = tCresadeRebrlList.uDeployId;
            tDateDT.m_RevelLv = tCresadeRebrlList.uRebelLv;
            tDateDT.m_Color = tCresadeRebrlList.color;
            tDateDT.m_EndTime = tCresadeRebrlList.EndTime;
            tDateDT.hpPercent = tCresadeRebrlList.hpPercent;
            if (tDateDT.iId == Data_Pool.m_UserData.m_iUserId)
                m_UserFindRebelarmy = tDateDT;
            f_Save(tDateDT);
        }
        else     //不为空去更新
            f_Socket_UpdateData(tCresadeRebrlList);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
MessageBox.DEBUG("Update");
        RebelInfo tCresadeRebrlList = (RebelInfo)Obj;
        RebelArmyPoolDT tDateDT = (RebelArmyPoolDT)f_GetForId(tCresadeRebrlList.discovererId);
        tDateDT.iId = tCresadeRebrlList.discovererId;
        tDateDT._ITempleteId = tCresadeRebrlList.uDeployId;
        tDateDT.m_RevelLv = tCresadeRebrlList.uRebelLv;
        tDateDT.m_Color = tCresadeRebrlList.color;
        tDateDT.m_EndTime = tCresadeRebrlList.EndTime;
        tDateDT.hpPercent = tCresadeRebrlList.hpPercent;
    }

    protected override void RegSocketMessage()
    {
        RebelInfo tRebelInfo = new RebelInfo();
        ChatSocket.GetInstance().f_RegMessage_Int0((int)ChatSocketCommand.RC_CrusadeRebelList, tRebelInfo, SaveList);
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrusadeRebelTrigger, tRebelInfo, CrusadeRebelTrigger);

        //ChatSocket.GetInstance().f_RegMessage((int)ChatSocketCommand.RC_CrusadeRebelList, tRebelInfo, SaveList);
        SC_CrusadeRebelInit tRebelInit = new SC_CrusadeRebelInit();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrusadeRebelInit, tRebelInit, RebelInit);

        SC_CrusadeRebelDmgRank tExploitRank = new SC_CrusadeRebelDmgRank();
        ChatSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrusadeRebelExploitRank, tExploitRank, Exploit);

        SC_CrusadeRebelDmgRank tDmgRank = new SC_CrusadeRebelDmgRank();
        ChatSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrusadeRebelDmgRank, tDmgRank, DmgRank);

        SC_RebelArmyFinish tSC_RebelArmyFinish = new SC_RebelArmyFinish();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrusadeRebelRet, tSC_RebelArmyFinish, f_RebelArmyFinish);
    }

    private void SaveList(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            f_Socket_AddData(tData, true);
        }

        for (int i = 0; i < ccback.Count; i++)
        {
            ccback[i](true);
        }
        ccback.Clear();
    }

    private void CrusadeRebelTrigger(object obj)
    {
MessageBox.DEBUG("Treason activation");
        tRebelInfo = (RebelInfo)obj;
        if (Trigger != null)
            Trigger(true);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RebelArmy);
        Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.RebelArmy);
    }
    public SC_RebelArmyFinish mRebelArmyFinish;    //结算信息
    public bool _bDungeonFinish;                   //是否已经结算
    protected void f_RebelArmyFinish(object obj)
    {
        SC_RebelArmyFinish tSC_RebelArmyFinish = (SC_RebelArmyFinish)obj;
MessageBox.DEBUG("Treason eliminated");
        mRebelArmyFinish = tSC_RebelArmyFinish;
        _bDungeonFinish = true;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public SC_CrusadeRebelInit tRebelInit;    //个人的叛军所用信息
    List<ccCallback> ccback = new List<ccCallback>();          //叛军列表 , 刷新叛军界面用
    ccCallback ExploitBack;     //功勋排行返回
    ccCallback DPSBack;         //伤害排行返回 
    ccCallback ShowAward;       //功勋奖励返回
    public ccCallback Trigger;         //触发返回
    public RebelInfo tRebelInfo;      //触发叛军用
    private RebelArmyPoolDT m_UserFindRebelarmy; //自己发现的
    private string[] strAwardFlag;
    private float m_lastRequestCrusadeRebelListTime;

    //List<NBaseSCDT> _AwardList;    //功勋奖励全部列表
    //List<NBaseSCDT> _tAward;       //功勋奖励同一档

    public RebelArmyPoolDT M_UserFindRebelarmy
    {
        get
        {
            return m_UserFindRebelarmy;
        }
    }
    
    public string[] m_AwardFlag {
        get {
            return strAwardFlag;
        }
    }

    /// <summary>
    /// 初始化功勋排名
    /// </summary>
    public void f_CrusadeRebelExploitRank(ccCallback back)
    {
        ExploitBack = back;
        byte[] tb = new byte[1];
        tb[0] = 1;
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrusadeRebelExploitRank, tb);
    }
    public void f_CrusadeRebelInit(ccCallback back = null)
    {       
        byte[] tb = new byte[1];
        tb[0] = 1;
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrusadeRebelInit, tb);

        //打开界面后主界面刷新红点会把回调置空，导致界面没刷新，所以空的过滤掉
        if (null == back) {
            return;
        }
        ShowAward = back;
    }
    public void f_CrusadeRebelDPSRank(ccCallback back)
    {
        DPSBack = back;
        byte[] tb = new byte[1];
        tb[0] = 1;
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrusadeRebelDmgRank, tb);
    }

    public void f_CrusadeRebelShare()
    {

    }
    private void Exploit(object obj)
    {
        //m_ExploitRank = new CrusadeRebelRank[5];
        ExploitBack(obj);
    }
    private void DmgRank(object obj)
    {
        DPSBack(obj);
    }
    private void RebelInit(object obj)
    {
        tRebelInit = (SC_CrusadeRebelInit)obj;
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RebelArmyAward);
        if (strAwardFlag==null) {
            strAwardFlag = new string[10];
            for (int i = 0; i < strAwardFlag.Length; i++)
            {
                strAwardFlag[i] = string.Empty;
            }
        }

        ExploitLvDT tlv = null;
        List<NBaseSCDT> tLvList = glo_Main.GetInstance().m_SC_Pool.m_ExploitLvSC.f_GetAll();
        for (int i = 0; i < tLvList.Count; i++)
        {
            if ((tLvList[i] as ExploitLvDT).iLv >= Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level))
            {
                tlv = tLvList[i] as ExploitLvDT;
                break;
            }
        }
        if (tlv == null)
        {
            tlv = tLvList[tLvList.Count - 1] as ExploitLvDT;
        }

        List<NBaseSCDT> _AwardList = glo_Main.GetInstance().m_SC_Pool.m_ExploitAwardSC.f_GetAll();
        int index = 0;
        int flagN = 0;
        int flagM = 0;
        ExploitAwardDT _AwardDT;
        for (int i = 0; i < _AwardList.Count; i++)
        {
            if ((_AwardList[i] as ExploitAwardDT).iLvId == tlv.iId)
            {
                flagN = index / 10;    //N段
                flagM = index - flagN * 10;    //M档
                string flag = tRebelInit.uAwardFlag[flagN].ToString();

                for (int j = flag.Length; j < 10; j++)
                {
                    flag = '0' + flag;
                }
                if (strAwardFlag[flagN]!= flag) {
                    strAwardFlag[flagN] = flag;
                }
                _AwardDT = _AwardList[i] as ExploitAwardDT;


                _AwardDT.iIndex = index+1;

                if (flag[flagM] == '0' && tRebelInit.uExploit >= _AwardDT.iRequirement)
                {
                    _AwardDT.iState = 1;
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.RebelArmyAward);
                }
                else if (flag[flagM] == '1')
                {
                    _AwardDT.iState = 2;
                }
                else {
                    _AwardDT.iState = 0;
                }
                index++;
            }
        }


        if (ShowAward == null)
        {
            return;
        }
        ShowAward(true);
        ShowAward = null;
    }
    /// <summary>
    /// 初始化列表
    /// </summary>
    /// <param name="ccback"></param>
    public void f_CrusadeRebelList(ccCallback ccback)
    {
        this.ccback.Add(ccback);
        if (this.ccback.Count > 1 && (UnityEngine.Time.realtimeSinceStartup - m_lastRequestCrusadeRebelListTime) < 8) return;
        m_lastRequestCrusadeRebelListTime = UnityEngine.Time.realtimeSinceStartup;
        RebelArmyPoolDT tDateDT = new RebelArmyPoolDT();
        byte[] tb = new byte[1];
        tb[0] = 1;
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_CrusadeRebelList, tb);
MessageBox.DEBUG("Request sent");
    }

    public bool m_isAllOut;  //消耗多少叛军令


    /// <summary>
    /// 征讨
    /// I64u userId;		// 用于服务器转发
    /// I64u discovererId;  // 发现者id
    /// I8u isAllOut;		// 全力一击
    /// </summary>
    public void CR_CrusadeRebel(long discovererId, byte isAllOut, SocketCallbackDT tCallBack)
    {
        if (isAllOut == 0)
            m_isAllOut = false;
        else
            m_isAllOut = true;
        //更新战斗数据
        DungeonTollgatePoolDT poolDt = Data_Pool.m_DungeonPool.f_GetTollgatePoolDTByType(EM_Fight_Enum.eFight_CrusadeRebel);
        StaticValue.m_CurBattleConfig.f_UpdateInfo((EM_Fight_Enum)poolDt.mChapterType, poolDt.mChapterId, poolDt.mTollgateId, poolDt.mTollgateTemplate.iSceneId);

        GameSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_CrusadeRebel, tCallBack.m_ccCallbackSuc, tCallBack.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((long)Data_Pool.m_UserData.m_iUserId);
        tCreateSocketBuf.f_Add(discovererId);
        tCreateSocketBuf.f_Add(isAllOut);
        byte[] tb = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_CrusadeRebel, tb);
        _bDungeonFinish = false;
    }

    public void f_CrusadeRebelExploitAward(byte idx, SocketCallbackDT tCallBack)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrusadeRebelExploitAward, tCallBack.m_ccCallbackSuc, tCallBack.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(idx);
        byte[] tb = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrusadeRebelExploitAward, tb);
    }
    /// <summary>
    /// 分享叛军
    /// </summary>
    /// <param name="tCallBack"></param>
    public void f_CrusadeRebelShare(SocketCallbackDT tCallBack)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrusadeRebelShare, tCallBack.m_ccCallbackSuc, tCallBack.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(0);
        byte[] tb = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrusadeRebelShare, tb);
    }

}
