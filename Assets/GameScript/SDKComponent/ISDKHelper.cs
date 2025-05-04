using UnityEngine;
using System.Collections;
using Beebyte.Obfuscator;

[Skip]
public interface ISDKHelper
{
    //初始化SDK
    [Skip]
    void f_Init(GameObject eventHandleGo);

    //获取渠道类型
    [Skip]
    string f_GetSdkChannelType();

    //登录SDK
    [Skip]
    void f_Login();

    //设置角色信息
    [Skip]
    void f_SetRoleInfo(EM_SetRoleInfoType setType, int serverId,string serverName,string roleName,long roleId,int roleBalance,int vipLv,int roleLv,string legionName,
        string createTime,string roleGender,int rolePower,long legionId,int roleJobId,string roleJobName,int legionRoleId,string legionRoleName,string friendList);

    //定额支付
    [Skip]
    void f_Pccaccy(int goodId,string goodName,double amount, int count,string cpOrderId,string extrasParams,float price,string quantifier,string goodsDesc);

    //注销账号
    [Skip]
    void f_Logout();

    //切换账号 是否需要切换场景重置数据
    [Skip]
    void f_SwitchAccount(bool needReset);

    //离开
    [Skip]
    void f_Exit();

    #region TsuCode
    //TsuCode - DashBoard
    [Skip]
    void f_ShowDashBoard();

    //TsuCode - Tutorial Completion - kết thúc tân thủ
    [Skip]
    void f_Tutorial_Completion();

    //TsuCode - Open store (vote app)
    [Skip]
    void f_OpenStore();

    //TsuCode - Open facebook app
    [Skip]
    void f_OpenFacebook(string url, string text);

    //TsuCode - Tracking quit App
    [Skip]
    void f_QuitApp();

    //TsuCode - Show SDK pay popup
    [Skip]
    void f_ShowSDKPay();

    //TsuCode - something else
    [Skip]
    void f_DoSomethingElse();

    #endregion TsuCode
}