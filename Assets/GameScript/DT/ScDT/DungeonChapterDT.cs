
//============================================
//
//    DungeonChapter来自DungeonChapter.xlsx文件自动生成脚本
//    2017/11/24 14:49:52
//    
//
//============================================
using System;
using System.Collections.Generic;



public class DungeonChapterDT : NBaseSCDT
{

    /// <summary>
    /// 章节类型
    /// </summary>
    public int iChapterType;
    /// <summary>
    /// 章节名称
    /// </summary>
    public string _szChapterName;
    public string szChapterName
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szChapterName);
        }
    }
    /// <summary>
    /// 章节说明
    /// </summary>
    public string _szChapterDesc;
    public string szChapterDesc
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szChapterDesc);
        }
    }
    /// <summary>
    /// 章节图片
    /// </summary>
    public string szChapterImage;
    /// <summary>
    /// 章节角色图片
    /// </summary>
    public int iRoleImage;
    /// <summary>
    /// 关卡（;)
    /// </summary>
    public string szTollgateId;
    /// <summary>
    /// 星级宝箱1
    /// </summary>
    public int iBox1;
    /// <summary>
    /// 星级要求1
    /// </summary>
    public int iNeedStar1;
    /// <summary>
    /// 星级宝箱2
    /// </summary>
    public int iBox2;
    /// <summary>
    /// 星级要求2
    /// </summary>
    public int iNeedStar2;
    /// <summary>
    /// 星级宝箱3
    /// </summary>
    public int iBox3;
    /// <summary>
    /// 星级要求3
    /// </summary>
    public int iNeedStar3;
    /// <summary>
    /// 首通奖励
    /// </summary>
    public string szFirstAward;
    /// <summary>
    /// 章节地图
    /// </summary>
    public string szCheckpointMap;
    /// <summary>
    /// 战斗场景地图索引
    /// </summary>
    public int iBattleSceneMap;
}
