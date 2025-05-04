using UnityEngine;
using System;
using System.Collections;
using Mono.Data.Sqlite;
using ccU3DEngine;

/// <summary>
/// DT UserData(玩家資料).xlsx
/// </summary>
public class UserDataUnit : BaseProperty
{
    /// <summary>
    /// 建号时间
    /// </summary>
    public int m_CreateTime;

    //是否已有角色  走创建角色流程 就为false
    private bool isHaveRole;
    public bool m_isHaveRole
    {
        get
        {
            return isHaveRole;
        }
    }
    public int m_LoginDays;
    /// <summary>
    /// 需要创建角色
    /// </summary>
    public void f_NeedCreateRole()
    {
        isHaveRole = false;
    }

    /// <summary>
    /// 玩家唯一Id
    /// </summary>
    public long m_iUserId;//用户id
    /// <summary>
    /// 玩家名字
    /// </summary>
    public string m_szRoleName;//名字
    /// <summary>
    /// 上次体力恢复时间
    /// </summary>
    public int m_lastEnergyRestoreTime = 0;
    /// <summary>
    /// 上次
    /// 精力恢复时间
    /// </summary>
    public int m_lastVigorRestoreTime = 0;
    /// <summary>
    /// 上次征讨令恢复时间
    /// </summary>
    public int m_lastCrusadeRestoreTime = 0;
    public UserDataUnit() : base((int)EM_UserAttr.End)
    {
        m_iUserId = -99;
        SC_RestoreTime scEnergyRestoreTime = new SC_RestoreTime();
        SC_RestoreTime scVigorRestoreTime = new SC_RestoreTime();
        SC_RestoreTime scCrusadeRestoreTime = new SC_RestoreTime();

        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrusadeTokenTime, scCrusadeRestoreTime, OnCrusadeRestoreTimeCallbakc);
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_EnergyTime, scEnergyRestoreTime, OnEnergyRestoreTimeCallback);
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_VigorTime, scVigorRestoreTime, OnVigorRestoreTimeCallbakc);

        SC_ReturnRandRoleName tCS_QueryRandRoleName = new SC_ReturnRandRoleName();
        GameSocket.GetInstance().f_AddListener((int)SocketCommand.SC_ReturnRandRoleName, tCS_QueryRandRoleName, On_SC_ReturnRandRoleName);

        isHaveRole = true;
    }

    public int m_iServerId;

    private string serverName;
    public string m_strServerName
    {
        set
        {
            serverName = value;
        }

        get
        {
            return serverName == null ? string.Empty : serverName;
        }
    }
    public void OnEnergyRestoreTimeCallback(object obj)
    {
        m_lastEnergyRestoreTime = ((SC_RestoreTime)obj).RestoreTime;
    }
    public void OnVigorRestoreTimeCallbakc(object obj)
    {
        m_lastVigorRestoreTime = ((SC_RestoreTime)obj).RestoreTime;
    }
    public void OnCrusadeRestoreTimeCallbakc(object obj)
    {
        m_lastCrusadeRestoreTime = ((SC_RestoreTime)obj).RestoreTime;
    }
    public override void f_Copy(int[] aData, int iStartPos, int iLen)
    {
        base.f_Copy(aData, iStartPos, iLen);
        //Vip信息更新事件
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
           new int[] { (int)EM_AchievementTaskCondition.eAchv_VipLv, UITool.f_GetNowVipLv() });
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerVipUpdate, UITool.f_GetNowVipLv());
    }

    public override void f_UpdateProperty(int iIndex, int iData)
    {

        if (iIndex != (int)EM_UserAttr.eUserAttr_Vip && iIndex != (int)EM_UserAttr.eUserAttr_MainCard && iIndex != (int)EM_UserAttr.eUserAttr_IconFrame && iIndex != (int)EM_UserAttr.eUserAttr_Energy
            && iIndex != (int)EM_UserAttr.eUserAttr_Vigor)
        {
            Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.Money, iIndex, iData - f_GetProperty(iIndex));
        }

        base.f_UpdateProperty(iIndex, iData);
        //Vip信息更新事件
        if (iIndex == (int)EM_UserAttr.eUserAttr_Vip)
        {
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
            new int[] { (int)EM_AchievementTaskCondition.eAchv_VipLv, UITool.f_GetNowVipLv() });
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerVipUpdate, UITool.f_GetNowVipLv());
        }

    }
    public void InitDataDefault()
    {
        if (m_lastEnergyRestoreTime == 0 && m_lastVigorRestoreTime == 0)
        {
            m_lastEnergyRestoreTime = GameSocket.GetInstance().f_GetServerTime();//设置默认值
            m_lastVigorRestoreTime = GameSocket.GetInstance().f_GetServerTime();
        }        
    }
    /// <summary>
    /// 其他正常获取，等级和经验获取在主角卡那里获取
    /// </summary>
    /// <param name="iIndex"></param>
    /// <returns></returns>
    public override int f_GetProperty(int iIndex)
    {
        if (iIndex == (int)EM_UserAttr.eUserAttr_Level)
        {
            return Data_Pool.m_CardPool.f_GetRoleLevel();
        }
        else if (iIndex == (int)EM_UserAttr.eUserAttr_Exp)
        {
            return Data_Pool.m_CardPool.f_GetRoleExp();
        }
        else if (iIndex == (int)EM_UserAttr.eUserAttr_TaskScore)
        {
            return Data_Pool.m_TaskDailyPool.mScore;
        }
        else
            return base.f_GetProperty(iIndex);
    }

    /// <summary>
    /// 获取单前经验的百分比
    /// </summary>
    public float mExpPercent
    {
        get
        {
            int expMax = 0;
            CarLvUpDT carLvUpDT = (CarLvUpDT)glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(f_GetProperty((int)EM_UserAttr.eUserAttr_Level) + 1);
            if (carLvUpDT != null)
            {
                expMax = carLvUpDT.iCardType;
            }
            return Data_Pool.m_CardPool.f_GetRoleExp() / (float)expMax;
        }
    }
    /// <summary>
    /// 向服务器发送报告信息
    /// </summary>
    /// <param name="message"></param>
    public void f_SendReport(string message)
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(message, 256);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_UserReport, bBuf);
    }
    /// <summary>
    /// 角色改名
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_ChangeName(string name, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ChangeName, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(name, 28);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ChangeName, bBuf);
    }
    /// <summary>
    /// 申请一个新的名字
    /// </summary>
    /// <param name="obj">性别</param>
    /// <param name="randNameCallbackRsp">回调</param>
    public void f_RequestNewName(object obj, ccCallback randNameCallbackRsp)
    {
        randNameCallback = randNameCallbackRsp;
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((int)obj);
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_QueryRandRoleName, tCreateSocketBuf.f_GetBuf());
    }
    private ccCallback randNameCallback = null;
    /// <summary>
    /// 随机名字服务器回调
    /// </summary>
    private void On_SC_ReturnRandRoleName(object Obj)
    {
        if (randNameCallback != null)
        {
            SC_ReturnRandRoleName tSC_ReturnRandRoleName = (SC_ReturnRandRoleName)Obj;
            randNameCallback(tSC_ReturnRandRoleName.szRoleName);
        }
    }

    /// <summary>
    /// 设置服务器信息
    /// </summary>
    public void f_SetServerInfo(int serverId, string serverName)
    {
        m_iServerId = serverId;
        m_strServerName = serverName;
    }
    public string m_szSexDesc
    {
        get
        {
            int tSex = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard) % 2;
            if (tSex == (int)EM_RoleSex.Man)
            {
                return "Nam";
            }
            else
            {
return "Female";
            }
        }
    }
}



