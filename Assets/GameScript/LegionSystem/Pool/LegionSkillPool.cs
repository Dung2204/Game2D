using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 军团技能上限等级pool
/// </summary>
public class LegionSkillPool : BasePool {
    /// <summary>
    /// 构造
    /// </summary>
    public LegionSkillPool() : base("LegionSkillPoolDT", true)
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        for (int i = (int)EM_LegionSkillType.SkillOne; i <= (int)EM_LegionSkillType.SkillNine; i++)
        {
            LegionSkillPoolDT poolDT = new LegionSkillPoolDT();
            poolDT.iId = i;
            poolDT.m_SkillLevel = 0;
            poolDT.m_SkillLevelMax = 0;
            f_Save(poolDT);
        }
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void RegSocketMessage()
    {
        SC_LegionLimitSkillInfo scLegionLimitSkillInfo = new SC_LegionLimitSkillInfo();
        GameSocket.GetInstance().f_RegMessage((int)LegionSocketCmd.SC_LegionSkillInfo, scLegionLimitSkillInfo, OnCallbackSkillLvLimitInfo);

        SC_LegionPersionSkillInfo scLegionPersionSkillInfo = new SC_LegionPersionSkillInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)LegionSocketCmd.SC_LegionSkillLv, scLegionPersionSkillInfo, OnLegionPersionSkillInfoCallback);
    }
    /// <summary>
    /// 军团技能上限信息服务器回调
    /// </summary>
    private void OnCallbackSkillLvLimitInfo(object data)
    {
        SC_LegionLimitSkillInfo scLegionLimitSkillInfo = (SC_LegionLimitSkillInfo)data;
        List<BasePoolDT<long>> listpoolDT = f_GetAll();
        for (int i = 0; i < listpoolDT.Count; i++)
        {
            LegionSkillPoolDT poolDT = listpoolDT[i] as LegionSkillPoolDT;
            poolDT.m_SkillLevelMax = scLegionLimitSkillInfo.lvLimitArray[i];
        }
    }
    /// <summary>
    /// 个人技能信息服务器回调
    /// </summary>
    protected void OnLegionPersionSkillInfoCallback(int iData1, int iData2, int iNum, ArrayList aData)
    {
        for (int i = 0; i < aData.Count; i++)
        {
            SC_LegionPersionSkillInfo scLegionPersionSkillInfo = (SC_LegionPersionSkillInfo)aData[i];
            LegionSkillPoolDT poolDT = f_GetForId(scLegionPersionSkillInfo.skillId) as LegionSkillPoolDT;
            if (poolDT != null)
            {
                poolDT.m_SkillLevel = scLegionPersionSkillInfo.lv;
                poolDT.m_LegionSkillDT = f_GetLegionSkillDT(scLegionPersionSkillInfo.skillId, scLegionPersionSkillInfo.lv);
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
    /// 获取军团技能增加的属性
    /// </summary>
    /// <returns></returns>
    public RoleProperty f_GetLegionSkillAddProperty()
    {
        RoleProperty _AddProperty = new RoleProperty();
        List<BasePoolDT<long>> listPoolDT = f_GetAll();
        for (int i = 0; i < listPoolDT.Count; i++)
        {
            LegionSkillPoolDT poolDT = listPoolDT[i] as LegionSkillPoolDT;
            if (poolDT.m_LegionSkillDT != null)
            {
                if (poolDT.m_LegionSkillDT.iBuffID != 0)
                {
                    _AddProperty.f_AddProperty((int)poolDT.m_LegionSkillDT.iBuffID, poolDT.m_LegionSkillDT.iBuffCount);
                }
            }
        }
        return _AddProperty;
    }
    /// <summary>
    /// 通过技能类型和技能等级获取DT
    /// </summary>
    /// <param name="skillType">技能类型</param>
    /// <param name="skillLevel">技能等级</param>
    /// <returns></returns>
    public LegionSkillDT f_GetLegionSkillDT(int skillType, int skillLevel)
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_LegionSkillSC.f_GetAll();
        for (int i = 0; i < listDT.Count; i++)
        {
            LegionSkillDT dt = listDT[i] as LegionSkillDT;
            if (dt.iType == skillType && dt.iLevel == skillLevel)
                return dt;
        }
        return null;
    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_QueryInfo(SocketCallbackDT tSocketCallbackDT)
    {
        //军团上限技能信息
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionSkillInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        //GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionSkillLv, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionSkillInfo, bBuf);

        //个人技能信息
        //CreateSocketBuf tCreateSocketBuf2 = new CreateSocketBuf();
        //byte[] bBuf2 = tCreateSocketBuf2.f_GetBuf();
        //GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionSkillLv, bBuf2);
    }
    /// <summary>
    /// 请求开启或提升技能上限
    /// </summary>
    /// <param name="skillTypeId">需要开启或提升的id</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_OpenSkill(int skillTypeId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionSkillOpen, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)skillTypeId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionSkillOpen, bBuf);
    }
    /// <summary>
    /// 请求学习或升级技能等级
    /// </summary>
    /// <param name="skillTypeId">需要学习或升级的id</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_UpSkill(int skillTypeId,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionSkillUp, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)skillTypeId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionSkillUp, bBuf);
    }

    #endregion
}
