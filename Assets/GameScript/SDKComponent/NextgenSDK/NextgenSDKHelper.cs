using System;
using System.Text;
using Beebyte.Obfuscator;
using nextgen;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[Skip]
public class NextgenSDKHelper : ISDKHelper
{

    private NextgenSDKEventHandle m_nextgenSDKEventHandle = null;
    private Action m_SureExit = null;
    private GameRoleInfo m_CurRoleInfo = null;

    public NextgenSDK m_NextgenSDK = null;
    [Skip]
    public void f_Exit()
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK exit.");
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_EXIT_BY_GAMEUI, m_SureExit);
#else
        //m_NextgenSDK.login();
        m_NextgenSDK.exit(); //TsuCode
#endif
    }
    [Skip]
    public string f_GetSdkChannelType()
    {
        return m_NextgenSDK.channelType();
    }
    [Skip]
    public void f_Init(GameObject eventHandleGo)
    {
        m_nextgenSDKEventHandle = eventHandleGo.AddComponent<NextgenSDKEventHandle>();
        if (m_nextgenSDKEventHandle == null)
            throw new ArgumentNullException("m_nextgenSDKEventHandle is Null.");

        m_NextgenSDK = NextgenSDK.getInstance();
        m_nextgenSDKEventHandle.f_Init(this);
        if (m_NextgenSDK == null)
            throw new ArgumentNullException("m_NextgenSDK is Null.");
        m_NextgenSDK.setListener(m_nextgenSDKEventHandle);

        m_SureExit = f_OnSureExit;
        MessageBox.DEBUG("NextgenSDK init.");
    }
    [Skip]
    public void f_Login()
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK login.");
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_LOGIN_RESULT, 0);
#else
        m_NextgenSDK.login();
#endif

    }
    [Skip]
    public void f_Logout()
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK logout.");
#else
        m_NextgenSDK.logout();
#endif
    }

    /*public void f_Pccaccy(int goodId, string goodName, double amount, int count, string cpOrderId, string extrasParams, float price, string quantifier, string goodsDesc)
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK no sdk pay.");
        SDKPccaccyResult tPayResutl = new SDKPccaccyResult(EM_PccaccyResult.Whitelist, goodId, string.Empty);
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_PAY_RESULT, tPayResutl);
#else
        if (m_CurRoleInfo == null)
            throw new ArgumentNullException("m_CurRoleInfo is null,can't pay");
        OrderInfo orderInfo = new OrderInfo();
        orderInfo.goodsID = goodId.ToString();
        orderInfo.goodsName = goodName;
        orderInfo.amount = amount / 100f; //Đơn vị sử dụng của SDK
        orderInfo.count = count;
        orderInfo.callbackUrl = "_";
        orderInfo.cpOrderID = cpOrderId;
        //Thông tin mở rộng payment
        extrasParams = string.Format("{0}#{1}#{2}", m_CurRoleInfo.serverID, extrasParams, m_CurRoleInfo.gameRoleID);
        orderInfo.extrasParams = extrasParams;
        //Đã tắt, bạn không thể tải lên
        orderInfo.price = price;
        orderInfo.quantifier = quantifier;
        orderInfo.goodsDesc = goodsDesc;
        m_NextgenSDK.pay(orderInfo, m_CurRoleInfo);
#endif
    }

    public void f_SetRoleInfo(EM_SetRoleInfoType setType, int serverId, string serverName, string roleName, long roleId, int roleBalance, int vipLv, int roleLv, string legionName, string createTime, string roleGender, int rolePower, long legionId, int roleJobId, string roleJobName, int legionRoleId, string legionRoleName, string friendList)
    {
        GameRoleInfo roleInfo = new GameRoleInfo();
        roleInfo.serverID = serverId.ToString();
        roleInfo.serverName = serverName;
        roleInfo.gameRoleName = roleName;
        roleInfo.gameRoleID = roleId.ToString();
        roleInfo.gameRoleBalance = roleBalance.ToString();
        roleInfo.vipLevel = vipLv.ToString();
        roleInfo.gameRoleLevel = roleLv.ToString();
        roleInfo.partyName = legionName;
        ///Phải truyền các kênh UC, Dangle và 1881, giá trị là dấu thời gian 10 chữ số
        roleInfo.roleCreateTime = createTime;
        //Thông số kênh 360
        roleInfo.gameRoleGender = roleGender;
        roleInfo.gameRolePower = rolePower.ToString();
        roleInfo.partyId = legionId.ToString();
        roleInfo.professionId = roleJobId.ToString();
        roleInfo.profession = roleJobName.ToString();
        roleInfo.partyRoleId = legionRoleId.ToString();
        roleInfo.partyRoleName = legionRoleName;
        roleInfo.friendlist = friendList;
        m_CurRoleInfo = roleInfo;
        StringBuilder tStringBuilder = new StringBuilder();
        switch (setType)
        {
            case EM_SetRoleInfoType.EnterGame:
                m_NextgenSDK.enterGame(roleInfo);
                tStringBuilder.Append("NextgenSDK_EnterGame,roleinfo:");
                break;
            case EM_SetRoleInfoType.CreateRole:
                m_NextgenSDK.createRole(roleInfo);
                tStringBuilder.Append("NextgenSDK_CreateGame,roleinfo:");
                break;
            case EM_SetRoleInfoType.LvUpRole:
                m_NextgenSDK.updateRole(roleInfo);
                tStringBuilder.Append("NextgenSDK_LvUpRole,roleinfo:");
                break;
            default:
                break;
        }
        tStringBuilder.AppendFormat(" serverId = {0}; serverName = {1}; roleName = {2}; roleId = {3}; roleBalance = {4}; vipLv = {5}; roleLv = {6}; legionName = {7};CreateTime = {8}; roleGender = {9}; rolePower = {10}; legionId = {11}; roleJobId = {12}; roleJobName = {13}; legionRoleId = {14}; legionRoleName = {15}; friendList:{16};",
                                       serverId, serverName, roleName, roleId, roleBalance, vipLv, roleLv, legionName, createTime, roleGender, rolePower, legionId, roleJobId, roleJobName, legionRoleId, legionRoleName, friendList);
        tStringBuilder.AppendFormat(" Date:{0}", DateTime.Now.ToString("HH-mm-ss"));
        MessageBox.DEBUG(tStringBuilder.ToString());
    }*/
    [Skip]
    public void f_SetRoleInfo(EM_SetRoleInfoType setType, int serverId, string serverName, string roleName, long roleId, int roleBalance, int vipLv, int roleLv, string legionName,
        string createTime, string roleGender, int rolePower, long legionId, int roleJobId, string roleJobName, int legionRoleId, string legionRoleName, string friendList)
    {
        GameRoleInfo roleInfo = new GameRoleInfo();
        roleInfo.serverID = serverId.ToString();
        roleInfo.serverName = serverName;
        roleInfo.gameRoleName = roleName;
        roleInfo.gameRoleID = roleId.ToString();
        roleInfo.gameRoleBalance = roleBalance.ToString();
        roleInfo.vipLevel = vipLv.ToString();
        roleInfo.gameRoleLevel = roleLv.ToString();
        roleInfo.partyName = legionName;
        ////UC，当乐与1881渠道必传，值为10位数时间戳
        roleInfo.roleCreateTime = createTime;
        //360渠道参数
        roleInfo.gameRoleGender = roleGender;
        roleInfo.gameRolePower = rolePower.ToString();
        roleInfo.partyId = legionId.ToString();
        roleInfo.professionId = roleJobId.ToString();
        roleInfo.profession = roleJobName.ToString();
        roleInfo.partyRoleId = legionRoleId.ToString();
        roleInfo.partyRoleName = legionRoleName;
        roleInfo.friendlist = friendList;
        m_CurRoleInfo = roleInfo;
        StringBuilder tStringBuilder = new StringBuilder();
        switch (setType)
        {
            case EM_SetRoleInfoType.EnterGame:
                m_NextgenSDK.enterGame(roleInfo);
                tStringBuilder.Append("QuickSDK_EnterGame,roleinfo:");
                break;
            case EM_SetRoleInfoType.CreateRole:
                m_NextgenSDK.createRole(roleInfo);
                tStringBuilder.Append("QuickSDK_CreateGame,roleinfo:");
                break;
            case EM_SetRoleInfoType.LvUpRole:
                //m_NextgenSDK.updateRole(roleInfo);
                m_NextgenSDK.updateRoleLvUp(roleInfo);
                tStringBuilder.Append("QuickSDK_LvUpRole,roleinfo:");
                break;
            default:
                break;
        }
        tStringBuilder.AppendFormat(" serverId = {0}; serverName = {1}; roleName = {2}; roleId = {3}; roleBalance = {4}; vipLv = {5}; roleLv = {6}; legionName = {7};CreateTime = {8}; roleGender = {9}; rolePower = {10}; legionId = {11}; roleJobId = {12}; roleJobName = {13}; legionRoleId = {14}; legionRoleName = {15}; friendList:{16};",
                                       serverId, serverName, roleName, roleId, roleBalance, vipLv, roleLv, legionName, createTime, roleGender, rolePower, legionId, roleJobId, roleJobName, legionRoleId, legionRoleName, friendList);
        tStringBuilder.AppendFormat(" Date:{0}", DateTime.Now.ToString("HH-mm-ss"));
        MessageBox.DEBUG(tStringBuilder.ToString());
    }
    [Skip]
    public void f_Pccaccy(int goodId, string goodName, double amount, int count, string cpOrderId, string extrasParams, float price, string quantifier, string goodsDesc)
    {
#if UNITY_EDITOR
        SDKPccaccyResult tPayResutl = new SDKPccaccyResult(EM_PccaccyResult.Whitelist, goodId, string.Empty);
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_PAY_RESULT, tPayResutl);
#else
        if (m_CurRoleInfo == null)
            throw new ArgumentNullException("m_CurRoleInfo is null,can't pay");
        OrderInfo orderInfo = new OrderInfo();
        orderInfo.goodsID = goodId.ToString();
        orderInfo.goodsName = goodName;
        orderInfo.amount = amount / 100f; //sdk单位是元   分/100
        orderInfo.count = count;
        orderInfo.cpOrderID = cpOrderId;
        //Quick 扩展信息添加个serverid和玩家的 UserId
        extrasParams = string.Format("{0}#{1}#{2}", m_CurRoleInfo.serverID, extrasParams, m_CurRoleInfo.gameRoleID);
        orderInfo.extrasParams = extrasParams;
        //已停用 可以不传
        orderInfo.price = price;
        orderInfo.quantifier = quantifier;
        orderInfo.goodsDesc = goodsDesc;
        m_NextgenSDK.pay(orderInfo, m_CurRoleInfo);

#if ANJIU
        PayResult t = new PayResult();
        t.cpOrderId = "-99";
        t.extraParam = extrasParams;
        t.orderId = "-99";
        m_nextgenSDKEventHandle.onPaySuccess(t);  
#endif
#endif

        MessageBox.DEBUG(string.Format("Nextgen_Pay,payInfo: goodId = {0}; goodName = {1}; amount = {2}; count = {3}; cpOrderId = {4}; extrasParams = {5}; price = {6}; quantifier = {7}; goodsDesc = {8}; Date:{9}",
                                                                goodId, goodName, amount, count, cpOrderId, extrasParams, price, quantifier, goodsDesc, DateTime.Now.ToString("HH-mm-ss")));
    }
    [Skip]
    public void f_SwitchAccount(bool needReset)
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK Not support Unity Editor");
#else
        MessageBox.DEBUG(string.Format("NextgenSDK_SwitchAccount, needReset = {0},Date:{1}", needReset, DateTime.Now.ToString("HH-mm-ss")));
        m_NextgenSDK.switchAccount(needReset);
#endif
    }
    [Skip]
    private void f_OnSureExit()
    {
        MessageBox.DEBUG("Nextgen sure exit.");
#if UNITY_EDITOR 
        Application.Quit();
       // EditorApplication.isPlaying = false;
#else
        m_NextgenSDK.exitGame();
#endif
    }

    #region TsuCode

    //TsuCode - DashBoard
    [Skip]
    public void f_ShowDashBoard()
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK Not support Unity Editor");
#else
        MessageBox.DEBUG("NextgenSDK ShowDashBoard");
        m_NextgenSDK.showDashBoard();
#endif
    }
    //TsuCode - Tutorial Completion
    [Skip]
    public void f_Tutorial_Completion()
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK Not support Unity Editor");
#else
        MessageBox.DEBUG("NextgenSDK TutorialCompletion");
        m_NextgenSDK.tutorialCompletion();
#endif
    }
    //TsuCode - Open Store
    [Skip]
    public void f_OpenStore()
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK Not support Unity Editor");
#else
        MessageBox.DEBUG("NextgenSDK OpenStore");
        m_NextgenSDK.openStore();
#endif
    }
    //TsuCode - Open Facebook
    [Skip]
    public void f_OpenFacebook(string url, string text)
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK Not support Unity Editor");
#else
        MessageBox.DEBUG("NextgenSDK OpenFacebook");
        m_NextgenSDK.openFacebook(url, text);
#endif
    }
    //TsuCode - QuitApp
    [Skip]
    public void f_QuitApp()
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK Not support Unity Editor");
#else
        MessageBox.DEBUG("NextgenSDK QuitApp");
        m_NextgenSDK.quitApp();
#endif
    }
    //TsuCode - Show SDK pay popup
    [Skip]
    public void f_ShowSDKPay()
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK Not support Unity Editor");
#else
         if (m_CurRoleInfo == null)
            throw new ArgumentNullException("m_CurRoleInfo is null");
        m_NextgenSDK.showSDKPay(m_CurRoleInfo);
#endif
    }
    //TsuCode - Do something
    [Skip]
    public void f_DoSomethingElse()
    {
#if UNITY_EDITOR
        MessageBox.DEBUG("NextgenSDK Not support Unity Editor");
#else
        MessageBox.DEBUG("NextgenSDK DoSomethingElse");
        m_NextgenSDK.doSomethingElse();
#endif
    }
    //--------------------------------------

    #endregion TsuCode
}