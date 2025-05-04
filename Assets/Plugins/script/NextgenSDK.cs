using System.Collections;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using UnityEngine;

namespace nextgen
{
    [Skip]
    public class NextgenSDK
    {
        private static NextgenSDK _instance;
        [Skip]
        public static NextgenSDK getInstance()
        {
            if (null == _instance)
            {
                _instance = new NextgenSDK();
            }
            return _instance;
        }
        [Skip]
        public void setListener(NextgenSDKListener listener)
        {
            NextgenSDKImp.getInstance().setListener(listener);
        }

        [Skip]
        public void init()
        {
            NextgenSDKImp.getInstance().init();
        }
        [Skip]
        public void exit()
        {
            NextgenSDKImp.getInstance().exit();
        }
        [Skip]
        public void login()
        {
            NextgenSDKImp.getInstance().login();
        }
        [Skip]
        public void logout()
        {
            NextgenSDKImp.getInstance().logout();
        }
        [Skip]
        public void switchAccount(bool needReset)
        {
            NextgenSDKImp.getInstance().switchAccount(needReset);
        }
        [Skip]
        public void pay(OrderInfo orderInfo, GameRoleInfo gameRoleInfo)
        {
            NextgenSDKImp.getInstance().pay(orderInfo, gameRoleInfo);
        }
        [Skip]
        public void createRole(GameRoleInfo gameRoleInfo)
        {
            NextgenSDKImp.getInstance().createRole(gameRoleInfo);//Sự kiện tạo nhân vật
        }
        [Skip]
        public void enterGame(GameRoleInfo gameRoleInfo)
        {
            NextgenSDKImp.getInstance().enterGame(gameRoleInfo);//Sự kiện bắt đầu trò chơi
        }
        [Skip]
        public void updateRole(GameRoleInfo gameRoleInfo)
        {
            NextgenSDKImp.getInstance().updateRole(gameRoleInfo);//Sự kiện nhân vật lên level
        }
        public void updateRoleLvUp(GameRoleInfo gameRoleInfo)
        {
            NextgenSDKImp.getInstance().updateRoleUpLevel(gameRoleInfo);//Sự kiện nhân vật lên level
        }
        [Skip]
        public string channelType()                 //Nhận danh mục kênh số nhận dạng duy nhất
        {
            return NextgenSDKImp.getInstance().channelType();
#if UNITY_EDITOR
            return "NextgenEditor";
#elif UNITY_IOS && !UNITY_EDITOR
            return "NextgenIOS";
#elif UNITY_ANDROID && !UNITY_EDITOR
            return "NextgenAndroid";
#endif
        }
        [Skip]
        public void exitGame()
        {
            NextgenSDKImp.getInstance().exitGame();
        }

        #region TsuCode
        //TsuCode - DashBoard
        [Skip]
        public void showDashBoard()
        {
            NextgenSDKImp.getInstance().showDashBoard();
        }
        //TsuCode - Tutorial completion - tracking kết thúc tân thủ
        [Skip]
        public void tutorialCompletion()
        {
            NextgenSDKImp.getInstance().tutorialCompletion();
        }
        //TsuCode - Open store (vote app)
        [Skip]
        public void openStore()
        {
            NextgenSDKImp.getInstance().openStore();
        }
        //TsuCode - Open facebook
        [Skip]
        public void openFacebook(string url, string text)
        {
            NextgenSDKImp.getInstance().openFacebook(url,text);
        }
        //TsuCode - quit app - tracking quitApp
        [Skip]
        public void quitApp()
        {
            NextgenSDKImp.getInstance().quitApp();
        }
        //TsuCode - Show sđk pay popup - mở giao diện thanh toán của SDK
        [Skip]
        public void showSDKPay(GameRoleInfo gameRoleInfo)
        {
            NextgenSDKImp.getInstance().showSDKPay(gameRoleInfo);
        }
        //TsuCode - do something Else
        [Skip]
        public void doSomethingElse()
        {
            NextgenSDKImp.getInstance().doSomethingElse();
        }
        //-----------------------------------------
        #endregion TsuCode
    }
}
