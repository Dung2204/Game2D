using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using System.Collections;

public class TariningAndTacticalPool : BasePool
{

    private TariningInfo _TariningInfo;
    private TacticalInfo _TacticalInfo;

    public bool _sakInfo;
    private bool _sakTacticalInfo;



    public TariningAndTacticalPool() : base("TariningAndTacticalPool", false)
    {
        _TariningInfo = new TariningInfo();
        _sakInfo = false;
        _sakTacticalInfo = false;

        _TacticalInfo = new TacticalInfo();
        _TacticalInfo.formatInfo = new byte[6];
        _TacticalInfo.maxFormatId = 0;
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
        TariningInfo tTariningInfoItem = new TariningInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_TacticalTransInfo, tTariningInfoItem, TariningInfoItem);

        TacticalInfo tTacticalInfo = new TacticalInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_TacticalFormat, tTacticalInfo, TacticalInfo);
    }

    private void TariningInfoItem(object obj)
    {
        _TariningInfo = (TariningInfo)obj;
        for(int i = 0; i < _TariningInfo.m_TariningInfo.Length; i++)
        {
            if(_TariningInfo.m_TariningInfo[i].TimeStamp != 0)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TariningGetAward);
            }
        }

    }

    private void TacticalInfo(object obj)
    {
        _TacticalInfo = (TacticalInfo)obj;

        for(int i = 0; i < _TacticalInfo.formatInfo.Length; i++)
        {
            //if(Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(Data_Pool.m_ClosethArrayData.m_dicClothArrayToPos[(EM_CloseArrayPos)i]) != null)
            //    Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(Data_Pool.m_ClosethArrayData.m_dicClothArrayToPos[(EM_CloseArrayPos)i]).m_TacticalId = _TacticalInfo.formatInfo[i];
            EM_FormationPos pos = Data_Pool.m_ClosethArrayData.m_dicClothArrayToPos.ElementAt(i).Value;
            if (Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(pos) != null)
                Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(pos).m_TacticalId = _TacticalInfo.formatInfo[i];
        }
        CheckTacticalRed();
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_USERINFOR);
        
    }

    private void CheckTacticalRed()
    {
        TacticalDT mTacticalDT = glo_Main.GetInstance().m_SC_Pool.m_TacticalSC.f_GetSC(_TacticalInfo.maxFormatId + 1) as TacticalDT;
        if(mTacticalDT != null)
        {
            if(mTacticalDT.iNeedConsume <= Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(TacticalGoodsId))
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TacticalStdySkill);
            }
            else
            {
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TacticalStdySkill);
            }
        }
    }

    #region 外部调用入口

    public TariningInfo m_TariningInfo
    {
        get { return _TariningInfo; }
    }
    public TacticalInfo m_TacticalInfo
    {
        get { return _TacticalInfo; }
    }

    //public int m_GetTariningOpenLv {
    //    get {
    //        return glo_Main
    //    }
    //}

    public int TariningGoodsId = 131;

    public int TacticalGoodsId = 132;
    /// <summary>
    /// 练兵请求初始化
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_TrainingSendInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if(_sakInfo)
        {
            return;
        }
        _sakInfo = true;
        if(tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TacticalTransInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);

        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_TacticalTransInfo, new byte[1]);

    }
    /// <summary>
    /// 阵法请求初始化
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_TacticalSendInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if(_sakTacticalInfo)
        {
            CheckTacticalRed();
        }

        if(_sakTacticalInfo)
        {
            return;
        }
        _sakTacticalInfo = true;
        if(tSocketCallbackDT != null)
        {
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TacticalFormat, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        }

        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_TacticalFormat, new byte[1]);
    }

    /// <summary>
    /// 练兵
    /// </summary>
    /// <param name="Trainingidx"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_TrainingTransSend(byte Trainingidx, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TacticalTrans, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf buf = new CreateSocketBuf();
        buf.f_Add(Trainingidx);
        byte[] ttt = buf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_TacticalTrans, ttt);
    }

    /// <summary>
    /// 一键练兵
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_OneKeyMoneyTrainingTransSend(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TacticalTransOnekey, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_TacticalTransOnekey, new byte[1]);
    }

    /// <summary>
    /// 一键领取
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_TacticalPickupOneKey(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TacticalPickupOneKey, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_TacticalPickupOneKey, new byte[1]);
    }
    /// <summary>
    /// 领取
    /// </summary>
    /// <param name="Trainingidx"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_TrainingGetAward(byte Trainingidx, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TacticalPickup, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf buf = new CreateSocketBuf();
        buf.f_Add(Trainingidx);
        byte[] ttt = buf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_TacticalPickup, ttt);
    }

    public void f_TacticalTransSycee(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TacticalTransSycee, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_TacticalTransSycee, new byte[1]);
    }

    /// <summary>
    /// 阵法技能附加在卡牌身上
    /// </summary>
    /// <param name="TeamIdx"></param>
    /// <param name="SkillId"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_TacticalAppCard(byte TeamIdx, byte SkillId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TacticalUse, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf buf = new CreateSocketBuf();
        buf.f_Add(TeamIdx);
        buf.f_Add(SkillId);
        byte[] ttt = buf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_TacticalUse, ttt);
    }
    public void f_TacticalAppCard(byte[] SkillId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TacticalUse, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf buf = new CreateSocketBuf();
        for(int i = 0; i < SkillId.Length; i++)
        {
            buf.f_Add(SkillId[i]);
        }
        byte[] ttt = buf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_TacticalUse, ttt);
    }

    /// <summary>
    /// 阵法学习
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_TacticalStudy(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TacticalStudy, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_TacticalStudy, new byte[1]);
    }
    #endregion
}
