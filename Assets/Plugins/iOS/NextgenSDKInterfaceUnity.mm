//
//  PlatformInterface_Unity.m
//  Unity-iPhone
//
//  Created by Mirufa on 23-04-2021.
//
//

#import <Foundation/Foundation.h>
#import "NextgenSDKInterfaceUnity.h"
#import "NextgenSDK.h"
#import <SohaSDK/Soha.h>

#if defined(__cplusplus)
extern "C"{
#endif
    void nextgensdk_nativeLogin(){
        [[NextgenSDK shareInstance] login];
	}
	void nextgensdk_nativeInit(){
        [[NextgenSDK shareInstance] initialize];
	}
    void nextgensdk_nativeSetListener(const char *gameObjectName){
        [[NextgenSDK shareInstance] setListener:[NSString stringWithUTF8String:gameObjectName]];
	}
    void nextgensdk_nativeExit(){
        [[NextgenSDK shareInstance] exit];
	}
    void nextgensdk_nativeLogout(){
        [[NextgenSDK shareInstance] logout];
	}
	void nextgensdk_nativeswitchAccount(){
        [[NextgenSDK shareInstance] switchAccount];
	}
	void nextgensdk_nativeExitGame(){
        [[NextgenSDK shareInstance] exitGame];
	}
    void nextgensdk_nativeshowDashBoard(){
        [Soha sohaDashboard];
    }
    void nextgensdk_nativePay(const char *goodsId, const char *goodsName, const char *goodsDesc, const char *quantifier, const char *cpOrderId, const char *callbackUrl, const char *extrasParams, double price, double amount, int count, const char *serverId, const char *serverName, const char *gameRoleName, const char *gameRoleId, const char *gameRoleBalance, const char *vipLevel, const char *gameRoleLevel, const char *partyName){
		NSString * _goodsID = [NSString stringWithUTF8String:goodsId ? goodsId : ""];
        NSString * _goodsName = [NSString stringWithUTF8String:goodsName ? goodsName : ""];
        NSString * _goodsDesc = [NSString stringWithUTF8String:goodsDesc ? goodsDesc : ""];
        NSString * _quantifier = [NSString stringWithUTF8String:quantifier ? quantifier : ""];
        NSString * _cpOrderId = [NSString stringWithUTF8String:cpOrderId ? cpOrderId : ""];
        NSString * _callbackUrl = [NSString stringWithUTF8String:callbackUrl ? callbackUrl : ""];
        NSString * _extrasParams = [NSString stringWithUTF8String:extrasParams ? extrasParams : ""];
        NSString * _serverId = [NSString stringWithUTF8String:serverId ? serverId : ""];
        NSString * _serverName = [NSString stringWithUTF8String:serverName ? serverName : ""];
        NSString * _gameRoleName = [NSString stringWithUTF8String:gameRoleName ? gameRoleName : ""];
        NSString * _gameRoleId = [NSString stringWithUTF8String:gameRoleId ? gameRoleId : ""];
        NSString * _gameRoleBalance = [NSString stringWithUTF8String:gameRoleBalance ? gameRoleBalance : ""];
        NSString * _vipLevel = [NSString stringWithUTF8String:vipLevel ? vipLevel : ""];
        NSString * _gameRoleLevel = [NSString stringWithUTF8String:gameRoleLevel ? gameRoleLevel : ""];
        NSString * _partyName = [NSString stringWithUTF8String:partyName ? partyName : ""];
        [[NextgenSDK shareInstance] pay:_goodsID :_goodsName :_goodsDesc :_quantifier :_cpOrderId :_callbackUrl :_extrasParams :price :amount :count :_serverId :_serverName :_gameRoleName :_gameRoleId :_gameRoleBalance :_vipLevel :_gameRoleLevel :_partyName];
	}
    void nextgensdk_nativeUpdateRoleInfo(const char *serverId, const char *serverName, const char *gameRoleName, const char *gameRoleId, const char *gameRoleBalance, const char *vipLevel, const char *gameRoleLevel, const char *partyName, const char *creatTime, BOOL isCreate){
        NSString * _serverId = [NSString stringWithUTF8String:serverId ? serverId : ""];
        NSString * _serverName = [NSString stringWithUTF8String:serverName ? serverName : ""];
        NSString * _gameRoleName = [NSString stringWithUTF8String:gameRoleName ? gameRoleName : ""];
        NSString * _gameRoleId = [NSString stringWithUTF8String:gameRoleId ? gameRoleId : ""];
        NSString * _gameRoleBalance = [NSString stringWithUTF8String:gameRoleBalance ? gameRoleBalance : ""];
        NSString * _vipLevel = [NSString stringWithUTF8String:vipLevel ? vipLevel : ""];
        NSString * _gameRoleLevel = [NSString stringWithUTF8String:gameRoleLevel ? gameRoleLevel : ""];
        NSString * _partyName = [NSString stringWithUTF8String:partyName ? partyName : ""];
//        NSString * _creatTime = [NSString stringWithUTF8String:creatTime ? creatTime : ""];
        [[NextgenSDK shareInstance] updateRoleInfo:_serverId :_serverName :_gameRoleName :_gameRoleId :_gameRoleBalance :_vipLevel :_gameRoleLevel :_partyName :@"" :isCreate];
    }

	const char *nextgensdk_nativeChannelType(){
		return [[[NextgenSDK shareInstance] getChanelType] UTF8String];
	}
#if defined(__cplusplus)
}
#endif
