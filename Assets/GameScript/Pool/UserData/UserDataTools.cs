using ccU3DEngine;
using System;
using System.Collections.Generic;

public class UserDataTools
{

    /// <summary>
    /// 登陆成功后要把服务器传送来的服务器数据转换成游戏数据
    /// </summary>
    public static void f_ServerData2GameData(CMsg_GTC_AccountLoginRelt tCMsg_GTC_AccountLoginRelt)
    {
        Data_Pool.m_UserData.m_CreateTime = tCMsg_GTC_AccountLoginRelt.createTime;
        Data_Pool.m_UserData.f_Copy(tCMsg_GTC_AccountLoginRelt.attr, 0, tCMsg_GTC_AccountLoginRelt.attr.Length);
        Data_Pool.m_UserData.m_szRoleName = tCMsg_GTC_AccountLoginRelt.szRoleName;
        Data_Pool.m_UserData.m_LoginDays = tCMsg_GTC_AccountLoginRelt.loginDays;
    }



    public static void f_ChangePlayerData(EM_UserAttr tEM_UserAttr, int nValues)
    {
        //string ppSQL = tEM_UserAttr.ToString();
        if (tEM_UserAttr == EM_UserAttr.eUserAttr_Energy || tEM_UserAttr == EM_UserAttr.eUserAttr_Vigor)
        {
            int oldEnergy = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy);
            int oldVigor = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor);
            if ((oldEnergy > nValues && tEM_UserAttr == EM_UserAttr.eUserAttr_Energy))
            {
                StaticValue.mOldEnergyValue = nValues;
                StaticValue.mOldVigorValue = oldVigor;
            }
            if (oldVigor > nValues && tEM_UserAttr == EM_UserAttr.eUserAttr_Vigor)
            {
                StaticValue.mOldEnergyValue = oldEnergy;
                StaticValue.mOldVigorValue = nValues;
            }
        }
        else
        {
           
        }
        Data_Pool.m_UserData.f_UpdateProperty((int)tEM_UserAttr, nValues);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_USERINFOR);
    }

    public static bool f_CheckMobileHaveAccount()
    {
        if (StaticValue.m_LoginName != "")
        {
            return true;
        }

        return false;
    }
}
