
//============================================
//
//    ServerInfor来自ServerInfor.xlsx文件自动生成脚本
//    2018/1/22 11:03:22
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ServerInforDT : NBaseSCDT
{

    /// <summary>
    /// 服务器名
    /// </summary>
    public string szName;
    /// <summary>
    /// 分类(全部大写)
    /// </summary>
    public string szChannel;
    ///// <summary>
    ///// 资源服务器IP
    ///// </summary>
    //public string szResIP;
    ///// <summary>
    ///// iResPort
    ///// </summary>
    //public int iResPort;
    /// <summary>
    /// 游戏服网关IP
    /// </summary>
    public string szIP;
    /// <summary>
    /// iPort
    /// </summary>
    public int iPort;
    ///// <summary>
    ///// 当前版本号
    ///// </summary>
    //public string szVer;
    /// <summary>
    /// 服务器Id
    /// </summary>
    public int iServerId;
    ///// <summary>
    ///// 特殊充值类型
    ///// </summary>
    //public int iPayType;
    /// <summary>
    /// 服务器状态
    /// </summary>
    public int iServState;
    ///// <summary>
    ///// 开服批次
    ///// </summary>
    //public int iTimes;
    /// <summary>
    /// 锁定限制登陆渠道
    /// </summary>
    public string szLockChanel;
    /// <summary>
    /// 自动开始时间，为空则无效
    /// </summary>
    public string szAutoOpenTime;
}
