using UnityEngine;
using System.Collections;
using ccU3DEngine;

/// <summary>
/// Socket_SelCharacter与LoginPage通讯接口包
/// 接口由Socket_SelCharacter发起，负责回收
/// 
/// </summary>
public class LoginPage2SelCharacer
{
    ////////////////////////////////////////////////
    //Socket_SelCharacter提供接口

    //申请一个新的名字
    public ccCallback Sel_RequestNewName;           //void Sel_RequestNewName(object obj)

    //确认选角结束，提交数据向服务器确认
   // public ccCallback_IS Sel_RequestSubmit;                //void Sel_Submit(int iData1, string szData)
    public ccCallback_IIS Sel_RequestSubmit;

    ////////////////////////////////////////////////
    //LoginPage提供接口

    //申请一个新的名字返回接口
    public ccCallback Login_RequestNewNameResp;            //void Login_RecvNewName(object obj)

    //确认选角结果返回接口
    public ccCallback Login_SubmitResp;             //void Login_SubmitResp(object obj)

    

}