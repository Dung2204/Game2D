using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beebyte.Obfuscator;

namespace nextgen
{
    [Skip]
    public class NextgenSDKEventHandle : NextgenSDKListener
    {
        private NextgenSDKHelper m_NextgenSDKHelper = null;
        [Skip]
        public void f_Init(NextgenSDKHelper quickSDKHelper)
        {
            m_NextgenSDKHelper = quickSDKHelper;
        }
        [Skip]
        public override void onInitSuccess()
        {
            MessageBox.DEBUG(string.Format("Nextgen init Success. Date:{0}", DateTime.Now.ToString("HH-mm-ss")));
        }
        [Skip]
        public override void onInitFailed(ErrorMsg message)
        {
            MessageBox.DEBUG(string.Format("Nextgen init failed. ErrorMsg : {0}. Date:{1}", message.errMsg, DateTime.Now.ToString("HH-mm-ss")));
        }
        [Skip]
        public override void onLoginSuccess(UserInfo userInfo)
        {
            MessageBox.DEBUG(string.Format("Nextgen login Success.UserInfo uid:{0}; userName:{1}; token:{2}; errMsg:{3}; Date:{4} ", userInfo.uid, userInfo.userName, userInfo.token, userInfo.errMsg, DateTime.Now.ToString("HH-mm-ss")));
            glo_Main.GetInstance().m_SDKCmponent.ChannelRoleInfo.f_UpdateInfo(userInfo.uid.ToString(), m_NextgenSDKHelper.m_NextgenSDK.channelType(), userInfo.token);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_LOGIN_RESULT, 0);
        }
        [Skip]
        public override void onLoginFailed(ErrorMsg errMsg)
        {
            MessageBox.DEBUG(string.Format("Nextgen login failed. ErrorMsg: {0}. Date:{1}", errMsg.errMsg, DateTime.Now.ToString("HH-mm-ss")));
            //如果游戏没有登录按钮，应在这里再次调用登录接口
            m_NextgenSDKHelper.f_Login();
        }
        [Skip]
        public override void onPaySuccess(PayResult payResult)
        {
            MessageBox.DEBUG(string.Format("Nextgen pay success.PayResult OrderId:{0}; CPOrderId:{1}; ExtraParam:{2}; Date:{3}", payResult.orderId, payResult.cpOrderId, payResult.extraParam, DateTime.Now.ToString("HH-mm-ss")));
            int tPayTemplateId = 0;
            string[] extraArr = ccMath.f_String2ArrayString(payResult.extraParam, "#");
            if (extraArr.Length < 2 || !int.TryParse(extraArr[1], out tPayTemplateId))
                MessageBox.ASSERT("Nextgen pay success extraParam error");
            SDKPccaccyResult tPayResutl = new SDKPccaccyResult(EM_PccaccyResult.Success, tPayTemplateId, payResult.orderId);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_PAY_RESULT, tPayResutl);
        }
        [Skip]
        public override void onPayCancel(PayResult payResult)
        {
            MessageBox.DEBUG(string.Format("Nextgen pay cancel.PayResult OrderId:{0}; CPOrderId:{1}; ExtraParam:{2}; Date:{4}", payResult.orderId, payResult.cpOrderId, payResult.extraParam, DateTime.Now.ToString("HH-mm-ss")));
            int tPayTemplateId = 0;
            string[] extraArr = ccMath.f_String2ArrayString(payResult.extraParam, "#");
            if (extraArr.Length < 2 || !int.TryParse(extraArr[1], out tPayTemplateId))
                MessageBox.ASSERT("Nextgen pay cancel extraParam error");
            SDKPccaccyResult tPayResutl = new SDKPccaccyResult(EM_PccaccyResult.Cancel, tPayTemplateId, payResult.orderId);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_PAY_RESULT, tPayResutl);
        }
        [Skip]
        public override void onPayFailed(PayResult payResult)
        {
            MessageBox.DEBUG(string.Format("Nextgen pay failed.PayResult OrderId:{0}; CPOrderId:{1}; ExtraParam:{2}; Date:{4}", payResult.orderId, payResult.cpOrderId, payResult.extraParam, DateTime.Now.ToString("HH-mm-ss")));
            int tPayTemplateId = 0;
            string[] extraArr = ccMath.f_String2ArrayString(payResult.extraParam, "#");
            if (extraArr.Length < 2 || !int.TryParse(extraArr[1], out tPayTemplateId))
                MessageBox.DEBUG("Nextgen pay failed extraParam error");
            SDKPccaccyResult tPayResutl = new SDKPccaccyResult(EM_PccaccyResult.Failed, tPayTemplateId, payResult.orderId);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_PAY_RESULT, tPayResutl);
        }
        [Skip]
        public override void onLogoutSuccess()
        {
            //MessageBox.DEBUG(string.Format("Nextgen Logout Success.NeedReset:{0}; Data:{1};", m_NextgenSDKHelper.NeedReset, DateTime.Now.ToString("HH-mm-ss")));
            glo_Main.GetInstance().m_SDKCmponent.ChannelRoleInfo.f_UpdateInfo(string.Empty, string.Empty, string.Empty);
            //游戏应该清除当前角色信息，回到登陆界面，并自动调用一次登录接口
            //if (m_NextgenSDKHelper.NeedReset)
            {
                glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_CHANGE_ACCOUNT, EM_SDKAccountState.LoginOut);
            }
            //else
            //{
                //m_NextgenSDKHelper.f_Login();
                //调用一次登录初始化需要登录状态
                //m_NextgenSDKHelper.f_InitNeedReset();
            //}
        }
        [Skip]
        public override void onSwitchAccountSuccess(UserInfo userInfo)
        {
            MessageBox.DEBUG(string.Format("Nextgen switch account success. UserInfo UId:{0}; UserName:{1}; Token:{2}; ErrMsg:{3}; Date:{4}", userInfo.uid, userInfo.userName, userInfo.token, userInfo.errMsg, DateTime.Now.ToString("HH-mm-ss")));
            //一些渠道在悬浮框有切换账号的功能，此回调即切换成功后的回调。游戏应清除当前的游戏角色信息。在切换账号成功后回到选择服务器界面，请不要再次调用登录接口。
            glo_Main.GetInstance().m_SDKCmponent.ChannelRoleInfo.f_UpdateInfo(userInfo.uid.ToString(), m_NextgenSDKHelper.m_NextgenSDK.channelType(), userInfo.token);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.SDK_CHANGE_ACCOUNT, EM_SDKAccountState.SwitchSuc);
        }
        [Skip]
        public override void onExitSuccess()
        {
            MessageBox.DEBUG(string.Format("Nextgen exit success. Date:{0}", DateTime.Now.ToString("HH-mm-ss")));
            m_NextgenSDKHelper.m_NextgenSDK.exitGame();
        }
    }
}
