
public enum EM_GuidanceType
{
    FirstLogin = 1,  //首次登录
    UpLevel,     //升x级时
    Dungeon,    //关卡开放时
    GetAward, //获取奖励时触发
    OnDungeon, //进入关卡时
    OutDungeon, //关卡战斗结束时
    DungeonResult,   //战斗结果
    RebelArmy,    //触发叛军
    Hand,
}

public enum EM_Guidance
{
    GuidanceRead = 1,   //读
    GuidancePlay,        //播放
    GuidancePass,            //暂时暂停状态
    GuidanceDialogRead,     //剧情读取
    GuidanceDialogPlay,     //剧情播放
    GuidanceDialogEnd,      //剧情结束
    GuidanceEnd = 10,   //结束
}

