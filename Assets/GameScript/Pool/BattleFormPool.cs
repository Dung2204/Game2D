using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 天命（阵图）
/// </summary>
public class BattleFormPool : BasePool
{
    private bool isInitServerData = false;
    public int iDestinyProgress = 0;
    public BattleFormPool() : base("", false)
    {

    }
    protected override void f_Init()
    {

    }

    protected override void RegSocketMessage()
    {
        SC_BattleForm scBattleForm = new SC_BattleForm();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_BattleForm, scBattleForm, OnBattleFormationCallback);
    }
    private void OnBattleFormationCallback(object data)
    {
        SC_BattleForm scBattleForm = (SC_BattleForm)data;
        iDestinyProgress = scBattleForm.id;
        isInitServerData = true;
        CheckRedPoint();
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_USERINFOR);//更新战力
    }
    /// <summary>
    /// 检查红点
    /// </summary>
    private void CheckRedPoint()
    {
        BattleFormationsDT battleFormationsDT = (glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(Data_Pool.m_BattleFormPool.iDestinyProgress + 1) as BattleFormationsDT);
        
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.BattleForm_CanAct);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.BattleForm_CanAct);
        if (battleFormationsDT != null && UITool.f_GetIsOpensystem(EM_NeedLevel.BattleFormLevel))
        {
            int hasActivityIDCount = UITool.f_GetGoodNum(EM_ResourceType.Good, battleFormationsDT.iActivePorpID);
            if (hasActivityIDCount >= battleFormationsDT.iActivePorpCount)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.BattleForm_CanAct);
            }
        }
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #region 外部接口
    /// <summary>
    /// 获取增加的属性
    /// </summary>
    /// <returns></returns>
    public RoleProperty f_GetDestinyAddProperty()
    {
        return RolePropertyTools.f_GetDestinyAddProperty(Data_Pool.m_BattleFormPool.iDestinyProgress);
    }
    public void f_QueryBattleForm(SocketCallbackDT tSocketCallbackDT)
    {
        CheckRedPoint();
        ////向服务器提交数据
        if (isInitServerData)
        {
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        if (tSocketCallbackDT != null)
        {
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_QueryBattleFormation, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        }
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QueryBattleFormation, bBuf);
    }
    public void f_BattleForm(SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_BattleFormation, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_BattleFormation, bBuf);
    }
    #endregion
}
