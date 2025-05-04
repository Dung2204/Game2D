//
//  NextgenSDKInterfaceUnity.h
//  Unity-iPhone
//
//  Created by Mirufa on 23-04-2021.
//
//

#ifndef _NEXTGENSDKINTERFACE_UNITY_H_
#define _NEXTGENSDKINTERFACE_UNITY_H_


#if defined(__cplusplus)
extern "C"{
#endif
	extern void nextgensdk_nativeLogin();
	extern void nextgensdk_nativeInit();
    extern void nextgensdk_nativeSetListener(const char *gameObjectName);
    extern void nextgensdk_nativeExit();
    extern void nextgensdk_nativeLogout();
	extern void nextgensdk_nativeswitchAccount();
	extern void nextgensdk_nativeExitGame();
    extern void nextgensdk_nativeshowDashBoard();
    extern void nextgensdk_nativePay(const char *goodsId, const char *goodsName, const char *goodsDesc, const char *quantifier, const char *cpOrderId, const char *callbackUrl, const char *extrasParams, double price, double amount, int count, const char *serverId, const char *serverName, const char *gameRoleName, const char *gameRoleId, const char *gameRoleBalance, const char *vipLevel, const char *gameRoleLevel, const char *partyName);

    extern void nextgensdk_nativeUpdateRoleInfo(const char *serverId, const char *serverName, const char *gameRoleName, const char *gameRoleId, const char *gameRoleBalance, const char *vipLevel, const char *gameRoleLevel, const char *partyName, const char *creatTime, BOOL isCreate);
	extern const char *nextgensdk_nativeChannelType();
#if defined(__cplusplus)
}
#endif

#endif
