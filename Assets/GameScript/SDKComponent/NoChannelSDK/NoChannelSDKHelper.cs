#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;

/// <summary>
/// 不用渠道登录时候 用这个sdkhelper
/// </summary>
public class NoChannelSDKHelper : ISDKHelper
{
    private Action m_SureExit = null;
    public void f_Init(GameObject eventHandleGo)
    {
        MessageBox.DEBUG("NoChannelSDK init.");
        m_SureExit = f_OnSureExit;
    }

    public void f_Login()
    {
        //不是渠道登录 直接SDK登录成功
        MessageBox.DEBUG("NoChannelSDKHelper login.");
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_LOGIN_RESULT, 0);
    }
    
    public string f_GetSdkChannelType()
    {
        return "NoChannelSDK";
    }
    public void f_SetRoleInfo(EM_SetRoleInfoType setType, int serverId, string serverName, string roleName, long roleId, int roleBalance, int vipLv, int roleLv, string legionName, string createTime, string roleGender, int rolePower, long legionId, int roleJobId, string roleJobName, int legionRoleId, string legionRoleName, string friendList)
    {
        //不是渠道登录 直接只打印log  看看调用  对不对
        switch (setType)
        {
            case EM_SetRoleInfoType.EnterGame:
                MessageBox.DEBUG("NoChannelSDK enter game.");
                break;
            case EM_SetRoleInfoType.CreateRole:
                MessageBox.DEBUG("NoChannelSDK create role.");
                break;
            case EM_SetRoleInfoType.LvUpRole:
                MessageBox.DEBUG("NoChannelSDK lvup role.");
                break;
            default:
                break;
        }
    }

    public void f_Pccaccy(int goodId, string goodName, double amount, int count, string cpOrderId, string extrasParams, float price, string quantifier, string goodsDesc)
    {
        //不是渠道登录 直接向服务器请求购买
        MessageBox.DEBUG("NoChannelSDK pay.");
        SDKPccaccyResult tPayResutl = new SDKPccaccyResult(EM_PccaccyResult.Whitelist, goodId, string.Empty);
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_PAY_RESULT, tPayResutl);
    }

    public void f_Logout()
    {
        MessageBox.DEBUG("NoChannelSDK logout.");
    }

    public void f_Exit()
    {
        MessageBox.DEBUG("NoChannelSDK exit.");
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_EXIT_BY_GAMEUI, m_SureExit); 
    }

    private void f_OnSureExit()
    {
        MessageBox.DEBUG("NoChannelSDK sure exit.");
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    public void f_SwitchAccount(bool needReset)
    {
MessageBox.DEBUG("NoChannelSDK for changing accounts has not been deployed");
    }

    //TsuCode - DashBoard
    public void f_ShowDashBoard()
    {
        MessageBox.DEBUG("NoChannelSDK showDashBoard");
    }
    public void f_Tutorial_Completion()
    {
        MessageBox.DEBUG("NoChannelSDK f_Tutorial_Completion");
    }
    public void f_OpenStore()
    {
        MessageBox.DEBUG("NoChannelSDK f_OpenStore");
    }
    public void f_OpenFacebook(string url, string text)
    {
        MessageBox.DEBUG("NoChannelSDK f_OpenFacebook");
    }
    public void f_QuitApp()
    {
        MessageBox.DEBUG("NoChannelSDK f_QuitApp");
    }
    public void f_ShowSDKPay()
    {
        MessageBox.DEBUG("NoChannelSDK f_ShowSDKPay");
    }
    public void f_DoSomethingElse()
    {
        MessageBox.DEBUG("NoChannelSDK f_DoSomethingElse");
    }
    //----------------------
}
