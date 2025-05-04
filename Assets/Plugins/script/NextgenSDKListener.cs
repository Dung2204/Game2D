using System.Collections;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using UnityEngine;

namespace nextgen
{
    [Skip]
    public abstract class NextgenSDKListener : MonoBehaviour
    {
        [Skip]
        public abstract void onInitSuccess();
        [Skip]
        public abstract void onInitFailed(ErrorMsg message);
        [Skip]
        public abstract void onLoginSuccess(UserInfo userInfo);
        [Skip]
        public abstract void onSwitchAccountSuccess(UserInfo userInfo);
        [Skip]
        public abstract void onLoginFailed(ErrorMsg errMsg);
        [Skip]
        public abstract void onLogoutSuccess();


        [Skip]
        public abstract void onPaySuccess(PayResult payResult);
        [Skip]
        public abstract void onPayFailed(PayResult payResult);
        [Skip]
        public abstract void onPayCancel(PayResult payResult);
        [Skip]
        public abstract void onExitSuccess();


        //callback end

        [Skip]
        public void onInitSuccess(string msg)
        {
            onInitSuccess();
        }
        [Skip]
        public void onInitFailed(string msg)
        {
            var data = NextgenJSON.JSONNode.Parse(msg);
            ErrorMsg errMsg = new ErrorMsg();
            errMsg.errMsg = data["msg"].Value;

            onInitFailed(errMsg);
        }
        [Skip]
        public void onLoginSuccess(string msg)
        {
            var data = NextgenJSON.JSONNode.Parse(msg);
            UserInfo userInfo = new UserInfo();
            userInfo.uid = data["userId"].Value;
            userInfo.token = data["userToken"].Value;
            userInfo.userName = data["userName"].Value;
            userInfo.errMsg = data["msg"].Value;

            onLoginSuccess(userInfo);
        }
        [Skip]
        public void onSwitchAccountSuccess(string msg)
        {
            var data = NextgenJSON.JSONNode.Parse(msg);
            UserInfo userInfo = new UserInfo();
            userInfo.uid = data["userId"].Value;
            userInfo.token = data["userToken"].Value;
            userInfo.userName = data["userName"].Value;
            userInfo.errMsg = data["msg"].Value;

            onSwitchAccountSuccess(userInfo);
        }
        [Skip]
        public void onLoginFailed(string msg)
        {
            var data = NextgenJSON.JSONNode.Parse(msg);
            ErrorMsg errMsg = new ErrorMsg();
            errMsg.errMsg = data["msg"].Value;

            onLoginFailed(errMsg);
        }
        [Skip]
        public void onLogoutSuccess(string msg)
        {
            onLogoutSuccess();
        }


        [Skip]
        public void onPaySuccess(string msg)
        {
            var data = NextgenJSON.JSONNode.Parse(msg);
            PayResult result = new PayResult();
            result.cpOrderId = data["cpOrderId"].Value;
            result.orderId = data["orderId"].Value;
            result.extraParam = data["extraParam"].Value;

            onPaySuccess(result);
        }
        [Skip]
        public void onPayFailed(string msg)
        {
            var data = NextgenJSON.JSONNode.Parse(msg);
            PayResult result = new PayResult();
            result.cpOrderId = data["cpOrderId"].Value;
            result.orderId = data["orderId"].Value;
            result.extraParam = data["extraParam"].Value;

            onPayFailed(result);
        }
        [Skip]
        public void onPayCancel(string msg)
        {
            var data = NextgenJSON.JSONNode.Parse(msg);
            PayResult result = new PayResult();
            result.cpOrderId = data["cpOrderId"].Value;
            result.orderId = data["orderId"].Value;
            result.extraParam = data["extraParam"].Value;

            onPayCancel(result);
        }
        [Skip]
        public void onExitSuccess(string msg)
        {
            onExitSuccess();
        }
    }
}
