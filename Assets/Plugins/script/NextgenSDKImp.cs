using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Beebyte.Obfuscator;
using UnityEngine;

namespace nextgen
{
    [Skip]
    public class OrderInfo
    {
        public string goodsID;
        public string goodsName;
        public string goodsDesc;
        public string quantifier; //Chất lượng sản phẩm
        public string cpOrderID;
        public string callbackUrl;
        public string extrasParams;
        public double price;
        public double amount;
        public int count;
    };
    [Skip]
    public class GameRoleInfo
    {
        public string serverName;
        public string serverID;
        public string gameRoleName;
        public string gameRoleID;
        public string gameRoleBalance;
        public string vipLevel;
        public string gameRoleLevel;
        public string partyName;
        public string roleCreateTime;

        public string gameRoleGender;
        public string gameRolePower;
        public string partyId;

        public String professionId;
        public String profession;
        public String partyRoleId;
        public String partyRoleName;
        public String friendlist;
    };
    [Skip]
    // Thông báo lỗi
    public class ErrorMsg
    {
        public string errMsg;
    }
    [Skip]
    // Thông tin người dùng, được sử dụng trong cuộc gọi lại đăng nhập
    public class UserInfo : ErrorMsg
    {
        public string uid;
        public string userName;
        public string token;
    }
    [Skip]
    //Thông tin thanh toán, được sử dụng trong việc gọi lại thanh toán
    public class PayResult
    {
        public string orderId;
        public string cpOrderId;
        public string extraParam;
    }
    [Skip]
    public class NextgenSDKImp
    {
        [Skip]
        private static NextgenSDKImp _instance;
        [Skip]
        public static NextgenSDKImp getInstance()
        {
            if (null == _instance)
            {
                _instance = new NextgenSDKImp();
            }
            return _instance;
        }
        /// <summary>
        /// Set ObjectListener for Unity Game
        /// </summary>
        /// <param name="listener">Đối tượng lắng nghe sự kiện</param>
        [Skip]
        public void setListener(NextgenSDKListener listener)
        {
#if UNITY_IOS && !UNITY_EDITOR
			string gameObjectName = listener.gameObject.name;
			nextgensdk_nativeSetListener(gameObjectName);
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.setListener(listener);
#endif
        }
        /// <summary>
        /// Hàm khởi tạo SDK 
        /// </summary>
        [Skip]
        public void init()
        {
#if UNITY_IOS && !UNITY_EDITOR
            nextgensdk_nativeInit();
#elif UNITY_ANDROID && !UNITY_EDITOR
            NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
            androidSupport.init();
#endif
        }
        /// <summary>
        /// Hàm gọi đăng nhập của SDK Native
        /// </summary>
        [Skip]
        public void login()
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativeLogin();
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.login();
#endif
        }
        /// <summary>
        /// Hàm gọi thoát của SDK Native
        /// </summary>
        [Skip]
        public void exit()
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativeExit();
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.exit();
#endif
        }
        [Skip]
        public void logout()
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativeLogout();
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.logout();
#endif
        }
        [Skip]
        public void switchAccount(bool needReset)
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativeswitchAccount();
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.switchAccount();
#endif
        }
        [Skip]
        public void pay(OrderInfo orderInfo, GameRoleInfo gameRoleInfo)
        {
#if UNITY_IOS && !UNITY_EDITOR
            MessageBox.DEBUG(string.Format("Nextgensdk_Pay:{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}_{13}_{14}_{15}_{16}_{17}",orderInfo.goodsID, orderInfo.goodsName, orderInfo.goodsDesc, orderInfo.quantifier, orderInfo.cpOrderID, "-", orderInfo.extrasParams, orderInfo.price, orderInfo.amount, orderInfo.count,
			                   gameRoleInfo.serverID, gameRoleInfo.serverName, gameRoleInfo.gameRoleName, gameRoleInfo.gameRoleID, gameRoleInfo.gameRoleBalance, gameRoleInfo.vipLevel, gameRoleInfo.gameRoleLevel, "-"));
			nextgensdk_nativePay(orderInfo.goodsID, orderInfo.goodsName, orderInfo.goodsDesc, orderInfo.quantifier, orderInfo.cpOrderID, "-", orderInfo.extrasParams, orderInfo.price, orderInfo.amount, orderInfo.count,
			                   gameRoleInfo.serverID, gameRoleInfo.serverName, gameRoleInfo.gameRoleName, gameRoleInfo.gameRoleID, gameRoleInfo.gameRoleBalance, gameRoleInfo.vipLevel, gameRoleInfo.gameRoleLevel, "-");
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
            MessageBox.DEBUG(string.Format("NextgensdkAndroid_Pay:{0}_{1}_{2}", orderInfo.goodsID, orderInfo.extrasParams, orderInfo.cpOrderID));
			androidSupport.pay(orderInfo, gameRoleInfo);
#endif
        }
        [Skip]
        public void createRole(GameRoleInfo gameRoleInfo)
        {
            updateRoleInfo(gameRoleInfo,true,false);
        }
        [Skip]
        public void enterGame(GameRoleInfo gameRoleInfo)
        {
            updateRoleInfo(gameRoleInfo,false,false);
        }
        [Skip]
        public void updateRole(GameRoleInfo gameRoleInfo)
        {
            updateRoleInfo(gameRoleInfo,false,false);
        }
        public void updateRoleUpLevel(GameRoleInfo gameRoleInfo)
        {
            updateRoleInfo(gameRoleInfo, false, true);
        }
        [Skip]
        private void updateRoleInfo(GameRoleInfo gameRoleInfo,bool isCreateRole,bool isUpLevel)
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativeUpdateRoleInfo(gameRoleInfo.serverID, gameRoleInfo.serverName, gameRoleInfo.gameRoleName, gameRoleInfo.gameRoleID, gameRoleInfo.gameRoleBalance, gameRoleInfo.vipLevel, gameRoleInfo.gameRoleLevel, gameRoleInfo.partyName, isCreateRole, isUpLevel);
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.updateRoleInfo(gameRoleInfo, isCreateRole,isUpLevel);
#endif
        }
        [Skip]
        public void exitGame()
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativeExitGame();
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.exitGame();
#endif
        }
        [Skip]
        public string channelType()
        {
#if UNITY_IOS && !UNITY_EDITOR
			IntPtr intPtr = nextgensdk_nativeChannelType();
			return Marshal.PtrToStringAnsi(intPtr);
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			return androidSupport.channelType();
#endif
            return "Editor";
        }

        //TsuCode - DashBoard
        [Skip]
        public void showDashBoard()
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativeshowDashBoard();
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.showDashBoard();
#endif
        }
        //TsuCode - tutorial Completion - tracking kết thúc tân thủ
        [Skip]
        public void tutorialCompletion()
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativetutorialCompletion();
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.tutorialCompletion();
#endif
        }
        //TsuCode - Open Store (vote app)
        [Skip]
        public void openStore()
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativeopenStore();
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.openStore();
#endif
        }
        //TsuCode - Open facebook app
        [Skip]
        public void openFacebook(string url, string text)
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativeopenFacebook(url,text);
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.openFacebook(url, text);
#endif
        }
        //TsuCode - QuitApp - tracking quit app
        [Skip]
        public void quitApp()
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativequitApp();
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.quitApp();
#endif
        }
        //TsuCode - Show SDK Pay popup - hiển thị cửa sổ nạp của SDK
        [Skip]
        public void showSDKPay(GameRoleInfo gameRoleInfo)
        {
#if UNITY_IOS && !UNITY_EDITOR
			//nextgensdk_nativeshowSDKPay();
            nextgensdk_nativeshowSDKPay(gameRoleInfo.serverID, gameRoleInfo.serverName, gameRoleInfo.gameRoleName, gameRoleInfo.gameRoleID, gameRoleInfo.gameRoleBalance, gameRoleInfo.vipLevel, gameRoleInfo.gameRoleLevel, "-");
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.showSDKPay(gameRoleInfo);
#endif
        }
        //TsuCode - do something else
        [Skip]
        public void doSomethingElse()
        {
#if UNITY_IOS && !UNITY_EDITOR
			nextgensdk_nativedoSomethingElse();
#elif UNITY_ANDROID && !UNITY_EDITOR
			NextgenUnitySupportAndroid androidSupport = NextgenUnitySupportAndroid.getInstance();
			androidSupport.doSomethingElse();
#endif
        }

        //--------------------------------------



        // __Internal các Function dành cho IOS
#if UNITY_IOS && !UNITY_EDITOR
		[DllImport("__Internal")]
		private static extern void nextgensdk_nativeSetListener(string gameObjectName);
        [DllImport("__Internal")]
		private static extern void nextgensdk_nativeInit();
		[DllImport("__Internal")]
		private static extern void nextgensdk_nativeLogin();
		[DllImport("__Internal")]
		private static extern void nextgensdk_nativeExit();
        [DllImport("__Internal")]
		private static extern void nextgensdk_nativeLogout();
        [DllImport("__Internal")]
		private static extern void nextgensdk_nativeswitchAccount();

        //TsuCode - DashBoard
        [DllImport("__Internal")]
		private static extern void nextgensdk_nativeshowDashBoard();
        [DllImport("__Internal")]
		private static extern void nextgensdk_nativetutorialCompletion();
        [DllImport("__Internal")]
		private static extern void nextgensdk_nativeopenStore();
        [DllImport("__Internal")]
		private static extern void nextgensdk_nativeopenFacebook(string url, string text);
        [DllImport("__Internal")]
		private static extern void nextgensdk_nativequitApp();
        [DllImport("__Internal")]
		private static extern void nextgensdk_nativeshowSDKPay(string serverId, string serverName, string gameRoleName, string gameRoleId, string gameRoleBalance, string vipLevel, string gameRoleLevel, string partyName);
        [DllImport("__Internal")]
		private static extern void nextgensdk_nativedoSomethingElse();

        //-----------------------

		[DllImport("__Internal")]
		private static extern void nextgensdk_nativePay(string goodsId, string goodsName, string goodsDesc, string quantifier, string cpOrderId, string callbackUrl, string extrasParams, double price, double amount, int count,
		                                              string serverId, string serverName, string gameRoleName, string gameRoleId, string gameRoleBalance, string vipLevel, string gameRoleLevel, string partyName);
		[DllImport("__Internal")]
		private static extern void nextgensdk_nativeUpdateRoleInfo(string serverId, string serverName, string gameRoleName, string gameRoleId, string gameRoleBalance, string vipLevel, string gameRoleLevel, string partyName, bool isCreate,bool isUpLevel);
		[DllImport("__Internal")]
		private static extern IntPtr nextgensdk_nativeExitGame();
        [DllImport("__Internal")]
		private static extern IntPtr nextgensdk_nativeChannelType();
#endif

    }
    // __Internal các Function dành cho Android
#if UNITY_ANDROID && !UNITY_EDITOR
    [Skip]
    public class NextgenUnitySupportAndroid {
        AndroidJavaObject ao;

        private static NextgenUnitySupportAndroid instance;

        private NextgenUnitySupportAndroid() {
            
            AndroidJavaClass ac = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            ao = ac.GetStatic<AndroidJavaObject>("currentActivity");
        }
        [Skip]
        public static NextgenUnitySupportAndroid getInstance()
        {
            if (instance == null)
            {
                instance = new NextgenUnitySupportAndroid();
            }

            return instance;
        }
        [Skip]
		public void setListener(NextgenSDKListener listener)
        {
            Debug.Log("gameObject is " + listener.gameObject.name);
            if (listener == null)
            {
                Debug.LogError("set NextgenSDKListener error, listener is null");
                return;
            }
            string gameObjectName = listener.gameObject.name;
            if (ao == null)
            {
                Debug.LogError("setListener error, current activity is null");
            }
            else
            {
                ao.Call("setUnityGameObjectName", gameObjectName);
            }
        }
        [Skip]
        public void init()
        {
            ao.Call("requestInit");
        }
        [Skip]
        public void exit()
        {
            ao.Call("requestExit");
        }
        [Skip]
        public void login()
        {
            ao.Call("requestLogin");
        }
        [Skip]
        public void logout()
        {
            ao.Call("requestLogout");
        }
        [Skip]
        public void switchAccount()
        {
            ao.Call("requestSwitchAccount");
        }
        //TsuCode - DashBoard
        [Skip]
        public void showDashBoard()
        {
            ao.Call("requestShowDashBoard");
        }
          [Skip]
        public void tutorialCompletion()
        {
            ao.Call("requestTutorialCompletion");
        }
          [Skip]
        public void openStore()
        {
            ao.Call("requestOpenStore");
        }
          [Skip]
        public void openFacebook(string url, string text)
        {
            ao.Call("requestOpenFacebook",url, text);
        }
          [Skip]
        public void quitApp()
        {
            ao.Call("requestQuitApp");
        }
          [Skip]
        public void showSDKPay(GameRoleInfo gameRoleInfo)
        {
            string serverName = String.IsNullOrEmpty(gameRoleInfo.serverName) ? "" : gameRoleInfo.serverName;
            string serverId = String.IsNullOrEmpty(gameRoleInfo.serverID) ? "" : gameRoleInfo.serverID;
            string roleName = String.IsNullOrEmpty(gameRoleInfo.gameRoleName) ? "" : gameRoleInfo.gameRoleName;
            string roleId = String.IsNullOrEmpty(gameRoleInfo.gameRoleID) ? "" : gameRoleInfo.gameRoleID;
            string roleBalance = String.IsNullOrEmpty(gameRoleInfo.gameRoleBalance) ? "" : gameRoleInfo.gameRoleBalance;
            string vipLevel = String.IsNullOrEmpty(gameRoleInfo.vipLevel) ? "" : gameRoleInfo.vipLevel;
            string roleLevel = String.IsNullOrEmpty(gameRoleInfo.gameRoleLevel) ? "" : gameRoleInfo.gameRoleLevel;
            string partyName = String.IsNullOrEmpty(gameRoleInfo.partyName) ? "" : gameRoleInfo.partyName;
            string roleCreateTime = String.IsNullOrEmpty(gameRoleInfo.roleCreateTime) ? "" : gameRoleInfo.roleCreateTime;

            ao.Call("requestShowSDKPay",
                gameRoleInfo.serverName, gameRoleInfo.serverID,
                gameRoleInfo.gameRoleName, gameRoleInfo.gameRoleID,
                gameRoleInfo.gameRoleBalance, gameRoleInfo.vipLevel,
                gameRoleInfo.gameRoleLevel, gameRoleInfo.partyName, gameRoleInfo.roleCreateTime);
        }
          [Skip]
        public void doSomethingElse()
        {
            ao.Call("requestDoSomethingElse");
        }
        //-----------------------
        [Skip]
        public void pay(OrderInfo orderInfo, GameRoleInfo gameRoleInfo)
        {
            if (orderInfo == null)
            {
                Debug.LogError("call pay error, orderInfo is null");
                return;
            }
            ao.Call("requestPay",
                orderInfo.goodsID, orderInfo.goodsName, 
                orderInfo.goodsDesc, orderInfo.quantifier, 
                orderInfo.cpOrderID, orderInfo.callbackUrl, 
                orderInfo.extrasParams, orderInfo.price+"", 
                orderInfo.amount + "", orderInfo.count+"",
                
                gameRoleInfo.serverName, gameRoleInfo.serverID,
                gameRoleInfo.gameRoleName, gameRoleInfo.gameRoleID,
                gameRoleInfo.gameRoleBalance, gameRoleInfo.vipLevel,
                gameRoleInfo.gameRoleLevel, gameRoleInfo.partyName, gameRoleInfo.roleCreateTime);
        }
        [Skip]
        public void updateRoleInfo(GameRoleInfo gameRoleInfo, bool isCreate,bool isUpLevel)
        {
            if (gameRoleInfo.Equals(null))
            {
                Debug.LogError("updateRoleInfo is error, gameRoleInfo is null");
                return;
            }

            string serverName = String.IsNullOrEmpty(gameRoleInfo.serverName) ? "" : gameRoleInfo.serverName;
            string serverId = String.IsNullOrEmpty(gameRoleInfo.serverID) ? "" : gameRoleInfo.serverID;
            string roleName = String.IsNullOrEmpty(gameRoleInfo.gameRoleName) ? "" : gameRoleInfo.gameRoleName;
            string roleId = String.IsNullOrEmpty(gameRoleInfo.gameRoleID) ? "" : gameRoleInfo.gameRoleID;
            string roleBalance = String.IsNullOrEmpty(gameRoleInfo.gameRoleBalance) ? "" : gameRoleInfo.gameRoleBalance;
            string vipLevel = String.IsNullOrEmpty(gameRoleInfo.vipLevel) ? "" : gameRoleInfo.vipLevel;
            string roleLevel = String.IsNullOrEmpty(gameRoleInfo.gameRoleLevel) ? "" : gameRoleInfo.gameRoleLevel;
            string partyName = String.IsNullOrEmpty(gameRoleInfo.partyName) ? "" : gameRoleInfo.partyName;
            string roleCreateTime = String.IsNullOrEmpty(gameRoleInfo.roleCreateTime) ? "" : gameRoleInfo.roleCreateTime;

			string gameRoleGender = String.IsNullOrEmpty(gameRoleInfo.gameRoleGender) ? "" : gameRoleInfo.gameRoleGender;
			string gameRolePower = String.IsNullOrEmpty(gameRoleInfo.gameRolePower) ? "" : gameRoleInfo.gameRolePower;
			string partyId = String.IsNullOrEmpty(gameRoleInfo.partyId) ? "" : gameRoleInfo.partyId;

			string professionId = String.IsNullOrEmpty(gameRoleInfo.professionId) ? "" : gameRoleInfo.professionId;
			string profession = String.IsNullOrEmpty(gameRoleInfo.profession) ? "" : gameRoleInfo.profession;
			string partyRoleId = String.IsNullOrEmpty(gameRoleInfo.partyRoleId) ? "" : gameRoleInfo.partyRoleId;
			string partyRoleName = String.IsNullOrEmpty(gameRoleInfo.partyRoleName) ? "" : gameRoleInfo.partyRoleName;
			string friendlist = String.IsNullOrEmpty(gameRoleInfo.friendlist) ? "" : gameRoleInfo.friendlist;


            ao.Call("requestUpdateRoleInfo",
                serverId,
                serverName,
                roleName,
                roleId,
                roleBalance,
                vipLevel,
                roleLevel,
                partyName,
			    roleCreateTime,
			    gameRoleGender,
			    gameRolePower,
			    partyId,
			    professionId,
			    profession,
			    partyRoleId,
			    partyRoleName,
			    friendlist,
                isCreate + "","-",isUpLevel+"");
            Debug.LogWarning("updateRoleInfo executed");
        }
        [Skip]
        public void exitGame()
        {
            ao.Call("requestExitGame");
        }
        [Skip]
        public string channelType(){
            return ao.Call<string>("getChannelType");
        }
    }
#endif
}
