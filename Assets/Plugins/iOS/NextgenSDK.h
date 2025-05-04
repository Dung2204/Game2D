//
//  NextgenSDK.h
//  Unity-iPhone
//
//  Created by Admin on 26/04/2021.
//

#import <Foundation/Foundation.h>
#import <SohaSDK/Soha.h>

NS_ASSUME_NONNULL_BEGIN

@interface NextgenSDK : NSObject{
    NSString *_gameObjectName;
    int initState; //
    BOOL bU3dInited; //setListener u3d
    NSDictionary * payInfo;
}
+ (NextgenSDK *)shareInstance;
-(id)init;
-(void)initialize;
-(void)login;
-(void)exit;
-(void)exitGame;
-(void)logout;
-(void)switchAccount;
-(void)setListener:(NSString *)gameObjectName;
-(void)pay:(NSString*)goodsId : (NSString*)goodsName : (NSString*)goodsDesc : (NSString*)quantifier:(NSString*)cpOrderId: (NSString*)callbackUrl: (NSString*)extrasParams: (double) price: (double) amount: (int) count: (NSString*)serverId: (NSString*)serverName: (NSString*)gameRoleName:(NSString*)gameRoleId:(NSString*)gameRoleBalance: (NSString*)vipLevel: (NSString*)gameRoleLevel: (NSString*)partyName;
-(void)updateRoleInfo:(NSString*)serverId:(NSString*)serverName:(NSString*)gameRoleName:(NSString*)gameRoleId:(NSString*)gameRoleBalance:(NSString*)vipLevel: (NSString*)gameRoleLevel: (NSString*)partyName: (NSString*)creatTime: (BOOL) isCreate;
-(NSString*)getChanelType;
-(void)sendU3dMessage:(NSString *)messageName :(NSDictionary *)dict;
-(void)onLoginSuccess:(SohaUser *)user;
-(void)onLoginError:(NSNotification *) notification;
-(void)onLogoutSuccess:(SohaUser *)user;
-(void)onLogoutError:(NSNotification *) notification;
-(void)onPaymentSuccess:(SohaTransaction *)transaction;
-(void)onPaymentError:(SohaTransaction *)transaction;
@end

NS_ASSUME_NONNULL_END
