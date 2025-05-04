//
//  NextgenSDK.m
//  Unity-iPhone
//
//  Created by Admin on 26/04/2021.
//

#import "NextgenSDK.h"
#import <CommonCrypto/CommonDigest.h>
//#import "MobizSDK.h"
//#import "MobizUserSession.h"
#import <SohaSDK/Soha.h>

#if defined(__cplusplus)
extern "C"{
#endif
    extern void UnitySendMessage(const char *, const char *, const char *);
#if defined(__cplusplus)
}
#endif

@implementation NextgenSDK

static NextgenSDK * __singleton__;
+ (NextgenSDK *)shareInstance {
    static dispatch_once_t predicate;
    dispatch_once( &predicate, ^{ __singleton__ = [[[self class] alloc] init]; } );
    
    return __singleton__;
}

-(void)login{
//    dispatch_async(dispatch_get_main_queue(), ^{
//        [VNTManager loginSDKWithBlock:^(ObjectUser *user, NSError *error) {
//                NSLog(@"Đăng nhập thành công");
//
//                NSString *token = user.accessToken;
//                NSString *userId = user.accountId;
//                NSString *UserName =user.userName;
//
//                NSDictionary *dict = @{ @"userId" : userId,@"userName" :UserName,@"userToken":token,@"msg":@"Thực hiện đăng nhập ..."};
//                [self sendU3dMessage:@"onLoginSuccess" :dict];
//            }];
//    });
    [Soha sohaLoginSDK];
};
-(void)initialize{
//    [SohaSetting sohaSettings].setWindow=UnityGetMainWindow();

}
-(id)init{
    self = [super init];
   return self;
}

- (void)setListener:(NSString *)gameObjectName
{
    _gameObjectName = gameObjectName;
#if !__has_feature(objc_arc)
    _gameObjectName = [gameObjectName retain];
#endif
    bU3dInited = YES;
    if (initState == -1)//Init thất bại
    {
    }
    else if (initState == 0)//Init Thất bại
    {
        NSDictionary *dic = [NSDictionary dictionaryWithObjectsAndKeys:@"init failed", @"msg", nil];
        [self sendU3dMessage:@"onInitFailed" :dic];
        initState = -1;
    }
    else if (initState == 1)//Khởi tạo thành công
    {
        [self sendU3dMessage:@"onInitSuccess" :nil];
        initState = -1;
    }
    
}
- (NSString *)jsonStrFromDictionary:(NSDictionary *)dic {
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dic options:0 error:&error];
    if ([error code]) {
        return @"";
    }
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}
-(void)sendU3dMessage:(NSString *)messageName :(NSDictionary *)dict
{
    if (dict != nil)
    {
//        NSError * err;
//        NSData * jsonData = [NSJSONSerialization dataWithJSONObject:dict options:0 error:&err];
//        NSString * jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        NSString *jsonString = [self jsonStrFromDictionary:dict];
        UnitySendMessage([_gameObjectName UTF8String], [messageName UTF8String], [jsonString UTF8String]);
    }
    else{
        UnitySendMessage([_gameObjectName UTF8String], [messageName UTF8String], "");
    }
}

//============
-(void)exit{
}
-(void)exitGame{
}
-(void)logout{
    [Soha sohaLogoutSDK];
}
-(void)switchAccount{
    [Soha sohaLogoutSDK];
}
-(void)pay:(NSString*)goodsId : (NSString*)goodsName : (NSString*)goodsDesc : (NSString*)quantifier:(NSString*)cpOrderId: (NSString*)callbackUrl: (NSString*)extrasParams: (double) price: (double) amount: (int) count: (NSString*)serverId: (NSString*)serverName: (NSString*)gameRoleName:(NSString*)gameRoleId:(NSString*)gameRoleBalance: (NSString*)vipLevel: (NSString*)gameRoleLevel: (NSString*)partyName{
    payInfo=@{
        @"cpOrderId" : cpOrderId,
        @"extraParam" :extrasParams,
        @"orderId":cpOrderId};
    NSArray * lstExtra=[extrasParams componentsSeparatedByString:@"#"];
    [Soha payProductWithID:lstExtra[1]];
//    [[MobizSDK alloc] doPayment:[UIApplication sharedApplication].keyWindow.rootViewController addproductid:goodsId  addgameserverid :serverId addcharacterid: gameRoleId addcharacterlevel: gameRoleLevel addextras_params: extrasParams];
}
-(void)updateRoleInfo:(NSString*)serverId:(NSString*)serverName:(NSString*)gameRoleName:(NSString*)gameRoleId:(NSString*)gameRoleBalance:(NSString*)vipLevel: (NSString*)gameRoleLevel: (NSString*)partyName: (NSString*)creatTime: (BOOL) isCreate{
    [Soha sendEventSetRoleName:gameRoleName roleID:gameRoleId roleLevel:gameRoleLevel serverName:serverName serverID:serverId];
}
-(NSString*)getChanelType{
    return @"Gzone";
}

-(void)onLoginSuccess:(SohaUser *)user{
//    NSDictionary *userInfo = notification.userInfo;
//    NSLog(@"Đăng nhập thành công");
//    NSString *token = [userInfo valueForKey:@"token"];
//    NSString *userId =  [userInfo valueForKey:@"userid"];
//    NSString *UserName = [userInfo valueForKey:@"username"];

    NSDictionary *dict = @{ @"userId" : user.userId,@"userName" :user.userName,@"userToken":user.accessToken,@"msg":@"Thực hiện đăng nhập ..."};
    [self sendU3dMessage:@"onLoginSuccess" :dict];
}
-(void)onLoginError:(NSNotification *) notification{
    NSDictionary *userInfo = notification.userInfo;
    NSDictionary *dict = @{ @"msg":[userInfo valueForKey:@"Message"]};
   [self sendU3dMessage:@"onLoginFailed" :dict];
}
-(void)onLogoutSuccess:(SohaUser *)user{
   [self sendU3dMessage:@"onLogoutSuccess" :nil];
}
-(void)onLogoutError:(NSNotification *) notification{
    NSDictionary *userInfo = notification.userInfo;
    NSDictionary *dict = @{ @"msg":[userInfo valueForKey:@"Message"]};
   [self sendU3dMessage:@"onLogoutFailed" :dict];
}
-(void)onPaymentSuccess:(SohaTransaction *)transaction{
    [self sendU3dMessage:@"onPaySuccess" :payInfo];
}
-(void)onPaymentError:(SohaTransaction *)transaction{
   [self sendU3dMessage:@"onPayFailed" :payInfo];
}
@end
