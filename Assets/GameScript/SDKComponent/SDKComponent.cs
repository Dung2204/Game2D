//#define QUICK_SDK
//#define NEXTGEN_SDK

using UnityEngine;
using System;
using Beebyte.Obfuscator;

[Skip]
public class SDKComponent : MonoBehaviour
{
#if QUICK_SDK
    private string m_SDKHelperTypeName = "QuickSDKHelper";
    private bool m_IsChannel = true;
#elif LCAT_SDK
    private string m_SDKHelperTypeName = "LcatSDKHelper";
    private bool m_IsChannel = true;
#elif Y_SDK
    private string m_SDKHelperTypeName = "YSDKHelper";
    private bool m_IsChannel = true;
#elif IOSGen_SDK
    private string m_SDKHelperTypeName = "GenuineIosSDKHelper";
    private bool m_IsChannel = true;
#elif NEXTGEN_SDK
    private string m_SDKHelperTypeName = "NextgenSDKHelper";
#if UNITY_EDITOR
    private bool m_IsChannel = false;
#else
    private bool m_IsChannel = true;
#endif
#else
    private string m_SDKHelperTypeName = "NoChannelSDKHelper";
    private bool m_IsChannel = false;
#endif

    private ISDKHelper m_SDKHelper = null;

    /// <summary>
    /// 是否是渠道登录
    /// </summary>
    public bool IsChannel
    {
        get
        {
            return m_IsChannel;
        }
    }
    
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="eventHandleGo">接受回调的GameObject</param>
    public void f_Init(GameObject eventHandleGo)
    {
#if IOSGen_SDK
        m_ChannelRoleInfo = new SDKChannelRoleInfo();
        m_SDKHelper = new GenuineIosSDKHelper();
        if (m_SDKHelper == null)
            throw new ArgumentNullException("GenuineIosSDKHelper is Null.");
        m_SDKHelper.f_Init(eventHandleGo);
#else
        m_ChannelRoleInfo = new SDKChannelRoleInfo();
        Type sdkHelperType = Type.GetType(m_SDKHelperTypeName);
        if (sdkHelperType == null)
            throw new ArgumentNullException("SDKHelper type Can't find.");
        m_SDKHelper = (ISDKHelper)Activator.CreateInstance(sdkHelperType);
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_Init(eventHandleGo);
#endif
    }

    /// <summary>
    /// 登录SDK
    /// </summary>
    public void f_Login()
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_Login();
    }

    public string f_GetSdkChannelType()
    {
        return m_SDKHelper.f_GetSdkChannelType();
    }
    /// <summary>
    /// 设置角色信息
    /// </summary>
    /// <param name="setType">设置的操作类型</param>
    /// <param name="serverId">服务器Id</param>
    /// <param name="serverName">服务器名字</param>
    /// <param name="roleName">角色名字</param>
    /// <param name="roleId">角色Id</param>
    /// <param name="roleBalance">角色余额</param>
    /// <param name="vipLv">vip等级</param>
    /// <param name="roleLv">角色等级</param>
    /// <param name="legionName">军团名字</param>
    /// <param name="createTime">创建角色时间</param>
    /// <param name="roleGender">角色性别 “男”，“女”</param>
    /// <param name="rolePower">角色战斗力</param>
    /// <param name="legionId">军团Id</param>
    /// <param name="roleJobId">角色职业Id</param>
    /// <param name="roleJobName">角色职业名字</param>
    /// <param name="legionRoleId">军团职位Id </param>
    /// <param name="legionRoleName">军团职位名字</param>
    /// <param name="friendList">好友关系列表（暂填“无”）</param>
    public void f_SetRoleInfo(EM_SetRoleInfoType setType, int serverId, string serverName, string roleName, long roleId, int roleBalance, int vipLv, int roleLv, string legionName,
        string createTime, string roleGender, int rolePower, long legionId, int roleJobId, string roleJobName, int legionRoleId, string legionRoleName, string friendList)
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_SetRoleInfo(setType,serverId,serverName,roleName,roleId,roleBalance,vipLv,roleLv,legionName,
            createTime,roleGender,rolePower,legionId,roleJobId,roleJobName,legionRoleId,legionRoleName,friendList);
    }

    //amount 单位：分
    public void f_Pccaccy(int goodId, string goodName, double amount, int count, string cpOrderId, string extrasParams, float price, string quantifier, string goodsDesc)
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_Pccaccy(goodId, goodName, amount, count, cpOrderId, extrasParams, price, quantifier, goodsDesc);
    }

    public void f_Logout()
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_Logout();
    }

    public void f_Exit()
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_Exit();
    }

    public void f_SwitchAccount(bool needReset)
    {
        if(m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_SwitchAccount(needReset);
    }

    /// <summary>
    /// 生成游戏方订单号 最大长度：45
    /// </summary>
    /// <returns></returns>
    public string f_CreateCPOrderId()
    {
        string result = string.Empty;
        long id = Data_Pool.m_UserData.m_iUserId;   //max:20
        int time = GameSocket.GetInstance().f_GetServerTime(); //max:10
        int rand = UnityEngine.Random.Range(1, 100);  //1-99 max:2 
        int serverId = Data_Pool.m_UserData.m_iServerId;  //max:10
        int millisecond = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime()).Millisecond; //max:3
        result = string.Format("{0}{1}{2}{3}{4:d2}", id, time, rand, serverId, millisecond);  // max 20+10+2+10+3 = 45
        MessageBox.DEBUG(string.Format("CreateCPOrderId:{0} \n Id:{1}; Time:{2}; Rand{3}; serverId:{4}; Millisecond:{5}; len:{6}", result, id, time, rand, serverId, millisecond , result.Length));
        return result;
    }

    private SDKChannelRoleInfo m_ChannelRoleInfo = null;
    public SDKChannelRoleInfo ChannelRoleInfo
    {
        get
        {
            return m_ChannelRoleInfo;
        }
    }

    private EM_SDKAccountState m_AccountState = EM_SDKAccountState.Invalid;
    public void f_UpdateAccountState(EM_SDKAccountState accountState)
    {
        m_AccountState = accountState;
    }
    public EM_SDKAccountState AccountState
    {
        get
        {
            return m_AccountState;
        }
    }


    //TsuCode - DashBoard
    public void f_ShowDashBoard()
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_ShowDashBoard();
    }
    //TsuCode - Tutorial completion
    public void f_Tutorial_Completion()
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_Tutorial_Completion();
    }
    //TsuCode - Open store (vote app)
    public void f_OpenStore()
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_OpenStore();
    }
    //TsuCode - open Facbook app
    public void f_OpenFacebook(string url, string text)
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_OpenFacebook(url, text);
    }
    //TsuCode - Quit App tracking
    public void f_QuitApp()
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_QuitApp();
    }
    //TsuCode - show SDK pay popup
    public void f_ShowSDKPay()
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_ShowSDKPay();
    }
    //TsuCode - some thing else
    public void f_DoSomethingElse()
    {
        if (m_SDKHelper == null)
            throw new ArgumentNullException("SDKHelper is Null.");
        m_SDKHelper.f_DoSomethingElse();
    }

    //----------------------------
}

public class SDKChannelRoleInfo
{
    public const int CHANNEL_USERID_MAX_LEN = 64;
    public const int CHANNEL_FLAG_MAX_LEN = 32;
    public const int TOKEN_MAX_LEN = 512;

    private string m_ChannelUserId;
    private string m_ChannelFlag;
    private string m_Token;

    /// <summary>
    /// 渠道UserId
    /// </summary>
    public string ChannelUserId
    {
        get
        {
            return m_ChannelUserId;
        }
    }

    /// <summary>
    /// 渠道标记 （ChannelId或者其他标记）
    /// </summary>
    public string ChannelFlag
    {
        get
        {
            return m_ChannelFlag;
        }
    }

    /// <summary>
    /// 密令
    /// </summary>
    public string Token
    {
        get
        {
            return m_Token;
        }
    }

    public SDKChannelRoleInfo()
    {
        m_ChannelUserId = string.Empty;
        m_ChannelFlag = string.Empty;
        m_Token = string.Empty;
    }

    public void f_UpdateInfo(string channelUserId,string channelFlag,string token)
    {
        m_ChannelUserId = channelUserId;
        m_ChannelFlag = channelFlag;
        m_Token = token;
        //检查是否合法
        if (m_ChannelUserId.Length > CHANNEL_USERID_MAX_LEN)
        {
            MessageBox.ASSERT(string.Format("Channel userId len({0}) is over max len({1}).",m_ChannelUserId.Length, CHANNEL_USERID_MAX_LEN));
            m_ChannelUserId = m_ChannelUserId.Substring(0, CHANNEL_USERID_MAX_LEN);
        }
        if (m_ChannelFlag.Length > CHANNEL_FLAG_MAX_LEN)
        {
            MessageBox.ASSERT(string.Format("Channel flag len({0}) is over max len({1}).",m_ChannelFlag.Length, CHANNEL_FLAG_MAX_LEN));
            m_ChannelFlag = m_ChannelFlag.Substring(0,CHANNEL_FLAG_MAX_LEN);
        }
        if (m_Token.Length > TOKEN_MAX_LEN)
        {
            MessageBox.ASSERT(string.Format("Token len({0}) is over max len({1}).", m_Token.Length, TOKEN_MAX_LEN));
            m_Token = m_Token.Substring(0, TOKEN_MAX_LEN);
        }
        MessageBox.DEBUG("SDK component update SDKChannelRoleInfo. " + this.ToString());
            
    }
    public override string ToString()
    {
        return string.Format("SDKChannelRoleInfo ChannelUserId:{0} len:{1}; ChannelFlag:{2} len:{3}; Token:{4} len:{5}",m_ChannelUserId,m_ChannelUserId.Length,m_ChannelFlag, m_ChannelFlag.Length, m_Token,m_Token.Length);
    }
}


/// <summary>
/// SDKComponent 不同SDK生成对应的数据
/// </summary>
public class SDKPccaccyResult
{
    public int m_iEventId = 0;
    public int m_iTimes = 0;

    /// <summary>
    /// 流水号的最大长度
    /// </summary>
    public const int ORDERID_MAX_LEN = 33;

    private EM_PccaccyResult result = EM_PccaccyResult.Invalid;
    private int payTemplateId = 0;
    private string orderId = string.Empty;

    /// <summary>
    /// SDK充值结果构造函数
    /// </summary>
    /// <param name="result">充值结果</param>
    /// <param name="payTemplateId">充值的模板Id</param>
    /// <param name="orderId">充值流水号</param>
    public SDKPccaccyResult(EM_PccaccyResult result, int payTemplateId, string orderId)
    {
        this.result = result;
        this.payTemplateId = payTemplateId;
        this.orderId = orderId;
    }

    /// <summary>
    /// 充值结果
    /// </summary>
    public EM_PccaccyResult m_Result
    {
        get
        {
            return result;
        }
    }

    /// <summary>
    /// 充值的模板Id
    /// </summary>
    public int m_PayTemplateId
    {
        get
        {
            return payTemplateId;
        }
    }

    /// <summary>
    /// 流水号 向服务器认证的凭证
    /// </summary>
    public string m_OrderId
    {
        get
        {
            if (orderId.Length > ORDERID_MAX_LEN)
            {
                MessageBox.ASSERT(String.Format("Pay result orderId is over max len. OrderId content:{0} len:{1},maxLen:{2}",orderId,orderId.Length,ORDERID_MAX_LEN));
            }
            return orderId;
        }
    }
}

public enum EM_PccaccyResult
{
    /// <summary>
    /// 支付成功
    /// </summary>
    Success = 0,
    /// <summary>
    /// 支付取消
    /// </summary>
    Cancel = 1,
    /// <summary>
    /// 支付失败
    /// </summary>
    Failed = 2,

    /// <summary>
    /// 充值未处理订单
    /// </summary>
    UnsettledPay = 9997,

    /// <summary>
    /// 白名单支付
    /// </summary>
    Whitelist = 9998,

    /// <summary>
    /// 无效的
    /// </summary>
    Invalid = 9999,
}

public enum EM_SDKAccountState
{
    Invalid = 0,
    //登出
    LoginOut = 1,
    //切换账号 切换场景
    SwitchSuc = 2,
}

