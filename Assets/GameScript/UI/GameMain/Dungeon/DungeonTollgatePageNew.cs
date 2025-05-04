using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 参数
/// </summary>
public class DungeonTollgatePageParam
{
    public DungeonPoolDT mDungeonPoolDT;
    /// <summary>
    /// 默认选中的序号（在该副本，关卡列表中的排序，0开始，-1表示转到该副本最新关卡）
    /// </summary>
    public int SelectIndex = -1;
}

/// <summary>
/// 主线，精英，名将关卡界面
/// </summary>
public class DungeonTollgatePageNew : UIFramwork
{
    private float adaptiveScale = ScreenControl.Instance.mScaleRatio;
    private string[] titleSpriteNames = new string[5];
    private UISprite mspriteTitle;
    private const int ItemNum = 10;
    private UIScrollView _selectItemScrollView;
    private UIProgressBar _selectItemScrollBar;
    private TollgateSelectItem[] _enemyItems;
	private TollgateSelectItem[] _enemyItems_2;
    private GameObject _famousGeneralRemainTimes;
    private UILabel _txtfamousGeneralRemainTimes;

    //章节地图
    private UITexture[] _map;

    //关卡位置,根据敌人个数不同设置不同位置
    private Vector3[] mvecCheckpointPosOf4Enemy;
    private Vector3[] mvecCheckpointPosOf5Enemy;
    private Vector3[] mvecCheckpointPosOf6Enemy;
    private Vector3[] mvecCheckpointPosOf8Enemy;
    private Vector3[] mvecCheckpointPosOf10Enemy;
    private Dictionary<string, Vector3> mCheckpointAdjustPos; //key：“场景地图_关卡总数_关卡索引” ，value:调整的位置（地图路径不一致，导致不同人数的人物位置有些要微调！！！）

    //章节宝箱
    private string[][] mBoxSpriteNames;
    private GameObject mChapterBoxParent;
    private Transform mChapterBox1;
    private Transform mChapterBox2;
    private Transform mChapterBox3;
    private UISlider mChapterBoxSlider;
    private UILabel mChapterBoxLabel;
    private Transform boxEffectParent1;
    private Transform boxEffectParent2;
    private Transform boxEffectParent3;
    private GameObject boxEffectItem1;
    private GameObject boxEffectItem2;
    private GameObject boxEffectItem3;
	//My Code
	GameParamDT AssetOpen;
	GameObject TexBgKD;
	bool isFamousGeneral = false;
	int resetPosition = 0;
	//

    //当前副本章节数据
    private DungeonPoolDT mData;

    //单前副本宝箱 idx
    private int mCurChapterBoxIdx;

    //跳到相应Item相关
    private const int ItemNumPrePage = 5;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();

        //设置标题名字
        titleSpriteNames[(int)EM_Fight_Enum.eFight_DungeonMain] = "fb_title_a_ptfb";
        titleSpriteNames[(int)EM_Fight_Enum.eFight_DungeonElite] = "fb_title_a_jjfb";
        titleSpriteNames[(int)EM_Fight_Enum.eFight_Legend] = "fb_title_a_mjfb";
        titleSpriteNames[(int)EM_Fight_Enum.eFight_DailyPve] = "fb_title_a_rcfb";
		
		//My Code
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		//

        //根据敌人位置设置敌人位置
        mvecCheckpointPosOf4Enemy = new Vector3[4];
        mvecCheckpointPosOf4Enemy[0] = new Vector3(241.9f, 145.3f);
        mvecCheckpointPosOf4Enemy[1] = new Vector3(686, 47.6f);
        mvecCheckpointPosOf4Enemy[2] = new Vector3(1169.6f, 131);
        mvecCheckpointPosOf4Enemy[3] = new Vector3(1590, 53);
        mvecCheckpointPosOf5Enemy = new Vector3[5];
        mvecCheckpointPosOf5Enemy[0] = new Vector3(451, 100);
        mvecCheckpointPosOf5Enemy[1] = new Vector3(1200, 112);
        mvecCheckpointPosOf5Enemy[2] = new Vector3(1996, 122);
        mvecCheckpointPosOf5Enemy[3] = new Vector3(2751.9f, 80);
        mvecCheckpointPosOf5Enemy[4] = new Vector3(3306, 48);
        mvecCheckpointPosOf6Enemy = new Vector3[6];
        mvecCheckpointPosOf6Enemy[0] = new Vector3(302.8f, 140);
        mvecCheckpointPosOf6Enemy[1] = new Vector3(862, 110);
        mvecCheckpointPosOf6Enemy[2] = new Vector3(1481, 50);
        mvecCheckpointPosOf6Enemy[3] = new Vector3(2217, 102.9f);
        mvecCheckpointPosOf6Enemy[4] = new Vector3(2854, 105);
        mvecCheckpointPosOf6Enemy[5] = new Vector3(3588, 57);
        mvecCheckpointPosOf8Enemy = new Vector3[8];
        mvecCheckpointPosOf8Enemy[0] = new Vector3(345, 120);
        mvecCheckpointPosOf8Enemy[1] = new Vector3(705, 48.8f);
        mvecCheckpointPosOf8Enemy[2] = new Vector3(1214, 126.9f);
        mvecCheckpointPosOf8Enemy[3] = new Vector3(1660, 30);
        mvecCheckpointPosOf8Enemy[4] = new Vector3(2203, 112.7f);
        mvecCheckpointPosOf8Enemy[5] = new Vector3(2619, 49.5f);
        mvecCheckpointPosOf8Enemy[6] = new Vector3(3067, 127.7f);
        mvecCheckpointPosOf8Enemy[7] = new Vector3(3560, 24);
        mvecCheckpointPosOf10Enemy = new Vector3[10];
        mvecCheckpointPosOf10Enemy[0] = new Vector3(241.9f, 145.3f);
        mvecCheckpointPosOf10Enemy[1] = new Vector3(563.5f, 80);
        mvecCheckpointPosOf10Enemy[2] = new Vector3(949.7f, 95);
        mvecCheckpointPosOf10Enemy[3] = new Vector3(1307, 65);
        mvecCheckpointPosOf10Enemy[4] = new Vector3(1626, 41);
        mvecCheckpointPosOf10Enemy[5] = new Vector3(2013, 137);
        mvecCheckpointPosOf10Enemy[6] = new Vector3(2452, 90);
        mvecCheckpointPosOf10Enemy[7] = new Vector3(2856, 120);
        mvecCheckpointPosOf10Enemy[8] = new Vector3(3275, 77);
        mvecCheckpointPosOf10Enemy[9] = new Vector3(3638, 73);

        //每张地图路径不一致，以草原地图为基准，设置需要调整的位置，设置四个人的位置差别
        mCheckpointAdjustPos = new Dictionary<string, Vector3>();
        // mCheckpointAdjustPos["Battle_4_0"] = new Vector3(242,60);
        // mCheckpointAdjustPos["Battle_4_1"] = new Vector3(686, 133);
        // mCheckpointAdjustPos["Battle_4_2"] = new Vector3(1182, 277);
        // mCheckpointAdjustPos["Battle_4_3"] = new Vector3(1575, 275);
        // mCheckpointAdjustPos["Cave_4_0"] = new Vector3(227, 35);
        // mCheckpointAdjustPos["Desert_4_1"] = new Vector3(686, 188);
        // mCheckpointAdjustPos["Snowfield_4_0"] = new Vector3(242, 0);
        // mCheckpointAdjustPos["Snowfield_4_0"] = new Vector3(686, 135);
        // mCheckpointAdjustPos["Snowfield_4_1"] = new Vector3(1170, 189);
        mCheckpointAdjustPos["Street_4_0"] = new Vector3(360, 225);
        mCheckpointAdjustPos["Street_4_1"] = new Vector3(530, -70);
        mCheckpointAdjustPos["Street_4_2"] = new Vector3(1245, -50);
		mCheckpointAdjustPos["Street_4_3"] = new Vector3(1350, 280);
		mCheckpointAdjustPos["Battle_4_0"] = new Vector3(360, 75);
        mCheckpointAdjustPos["Battle_4_1"] = new Vector3(980, 75);
        mCheckpointAdjustPos["Battle_4_2"] = new Vector3(1245, 310);
        mCheckpointAdjustPos["Battle_4_3"] = new Vector3(570, 275);
		//Pho Ban Danh Tuong
        mCheckpointAdjustPos["Cave_4_0"] = new Vector3(720, -50);
        mCheckpointAdjustPos["Cave_4_1"] = new Vector3(960, -50);
        mCheckpointAdjustPos["Cave_4_2"] = new Vector3(1200, -50);
        mCheckpointAdjustPos["Cave_4_3"] = new Vector3(1440, -50);
		//
        mCheckpointAdjustPos["Snowfield_4_0"] = new Vector3(1515, -25);
        mCheckpointAdjustPos["Snowfield_4_1"] = new Vector3(925, -250);
        mCheckpointAdjustPos["Snowfield_4_2"] = new Vector3(415, 200);
        mCheckpointAdjustPos["Snowfield_4_3"] = new Vector3(1460, 315);
        mCheckpointAdjustPos["Gardens_4_0"] = new Vector3(485, 275);
        mCheckpointAdjustPos["Gardens_4_1"] = new Vector3(765, -210);
        mCheckpointAdjustPos["Gardens_4_2"] = new Vector3(1280, -50);
        mCheckpointAdjustPos["Gardens_4_3"] = new Vector3(1350, 295);
        mCheckpointAdjustPos["Desert_4_0"] = new Vector3(1635, -35);
        mCheckpointAdjustPos["Desert_4_1"] = new Vector3(1065, -205);
        mCheckpointAdjustPos["Desert_4_2"] = new Vector3(775, 300);
        mCheckpointAdjustPos["Desert_4_3"] = new Vector3(1315, 270);
        mCheckpointAdjustPos["Grassland_4_0"] = new Vector3(1730, 120);
        mCheckpointAdjustPos["Grassland_4_1"] = new Vector3(1500, -50);
        mCheckpointAdjustPos["Grassland_4_2"] = new Vector3(775, -230);
        mCheckpointAdjustPos["Grassland_4_3"] = new Vector3(505, 240);

        //处理5个人的场景位置差别
        mCheckpointAdjustPos["Battle_5_0"] = new Vector3(450, -50);
        mCheckpointAdjustPos["Battle_5_1"] = new Vector3(715, -50);
        mCheckpointAdjustPos["Battle_5_2"] = new Vector3(980, -50);
		mCheckpointAdjustPos["Battle_5_3"] = new Vector3(1245, -50);
		mCheckpointAdjustPos["Battle_5_4"] = new Vector3(1510, -50);
		mCheckpointAdjustPos["Cave_5_0"] = new Vector3(450, -50);
        mCheckpointAdjustPos["Cave_5_1"] = new Vector3(690, -50);
        mCheckpointAdjustPos["Cave_5_2"] = new Vector3(930, -50);
		mCheckpointAdjustPos["Cave_5_3"] = new Vector3(1170, -50);
		mCheckpointAdjustPos["Cave_5_4"] = new Vector3(1410, -50);
		mCheckpointAdjustPos["Snowfield_5_0"] = new Vector3(450, -50);
        mCheckpointAdjustPos["Snowfield_5_1"] = new Vector3(715, -50);
        mCheckpointAdjustPos["Snowfield_5_2"] = new Vector3(980, -50);
		mCheckpointAdjustPos["Snowfield_5_3"] = new Vector3(1245, -50);
		mCheckpointAdjustPos["Snowfield_5_4"] = new Vector3(1510, -50);
		mCheckpointAdjustPos["Gardens_5_0"] = new Vector3(450, -50);
        mCheckpointAdjustPos["Gardens_5_1"] = new Vector3(715, -50);
        mCheckpointAdjustPos["Gardens_5_2"] = new Vector3(980, -50);
		mCheckpointAdjustPos["Gardens_5_3"] = new Vector3(1245, -50);
		mCheckpointAdjustPos["Gardens_5_4"] = new Vector3(1510, -50);
		mCheckpointAdjustPos["Desert_5_0"] = new Vector3(450, -50);
        mCheckpointAdjustPos["Desert_5_1"] = new Vector3(715, -50);
        mCheckpointAdjustPos["Desert_5_2"] = new Vector3(980, -50);
		mCheckpointAdjustPos["Desert_5_3"] = new Vector3(1245, -50);
		mCheckpointAdjustPos["Desert_5_4"] = new Vector3(1510, -50);
		mCheckpointAdjustPos["Grassland_5_0"] = new Vector3(450, -50);
        mCheckpointAdjustPos["Grassland_5_1"] = new Vector3(715, -50);
        mCheckpointAdjustPos["Grassland_5_2"] = new Vector3(980, -50);
		mCheckpointAdjustPos["Grassland_5_3"] = new Vector3(1245, -50);
		mCheckpointAdjustPos["Grassland_5_4"] = new Vector3(1510, -50);
        mCheckpointAdjustPos["Street_5_0"] = new Vector3(360, 225);
		mCheckpointAdjustPos["Street_5_1"] = new Vector3(900, 150);
        mCheckpointAdjustPos["Street_5_2"] = new Vector3(715, -80);
        mCheckpointAdjustPos["Street_5_3"] = new Vector3(1200, -130);
		mCheckpointAdjustPos["Street_5_4"] = new Vector3(1350, 280);

        //处理6个人的场景位置差别
        mCheckpointAdjustPos["Battle_6_0"] = new Vector3(390, 70);
        mCheckpointAdjustPos["Battle_6_1"] = new Vector3(960, -140);
        mCheckpointAdjustPos["Battle_6_2"] = new Vector3(1460, 25);
        mCheckpointAdjustPos["Battle_6_3"] = new Vector3(1400, 360);
        mCheckpointAdjustPos["Battle_6_4"] = new Vector3(850, 295);
        mCheckpointAdjustPos["Battle_6_5"] = new Vector3(575, 275);
        mCheckpointAdjustPos["Cave_6_0"] = new Vector3(300, 55);
        mCheckpointAdjustPos["Cave_6_2"] = new Vector3(1481, 112);
        mCheckpointAdjustPos["Desert_6_1"] = new Vector3(862, 200);
        mCheckpointAdjustPos["Desert_6_2"] = new Vector3(1481, 148);
        mCheckpointAdjustPos["Desert_6_4"] = new Vector3(2854, 196);
        mCheckpointAdjustPos["Snowfield_6_0"] = new Vector3(302, 0);
        mCheckpointAdjustPos["Snowfield_6_2"] = new Vector3(1481, 180);
        mCheckpointAdjustPos["Snowfield_6_3"] = new Vector3(2217, 14);
        mCheckpointAdjustPos["Street_6_0"] = new Vector3(360, 225);
		mCheckpointAdjustPos["Street_6_1"] = new Vector3(890, 165);
        mCheckpointAdjustPos["Street_6_2"] = new Vector3(540, -55);
		mCheckpointAdjustPos["Street_6_3"] = new Vector3(930, -75);
        mCheckpointAdjustPos["Street_6_4"] = new Vector3(1350, -25);
        mCheckpointAdjustPos["Street_6_5"] = new Vector3(1350, 280);

        //处理8个人的场景位置差别
        mCheckpointAdjustPos["Battle_8_0"] = new Vector3(360, 75);
        mCheckpointAdjustPos["Battle_8_1"] = new Vector3(675, -30);
        mCheckpointAdjustPos["Battle_8_2"] = new Vector3(980, 75);
        mCheckpointAdjustPos["Battle_8_3"] = new Vector3(1180, -150);
        mCheckpointAdjustPos["Battle_8_4"] = new Vector3(1580, -60);
        mCheckpointAdjustPos["Battle_8_5"] = new Vector3(1245, 310);
        mCheckpointAdjustPos["Battle_8_6"] = new Vector3(855, 295);
        mCheckpointAdjustPos["Battle_8_7"] = new Vector3(570, 275);
        mCheckpointAdjustPos["Cave_8_0"] = new Vector3(317, 50);
        mCheckpointAdjustPos["Cave_8_3"] = new Vector3(1660, 88);
        mCheckpointAdjustPos["Cave_8_7"] = new Vector3(3560, 105);
        mCheckpointAdjustPos["Desert_8_1"] = new Vector3(777, 197);
        mCheckpointAdjustPos["Desert_8_5"] = new Vector3(2619, 190);
        mCheckpointAdjustPos["Desert_8_6"] = new Vector3(3048, 187);
        mCheckpointAdjustPos["Desert_8_7"] = new Vector3(3560, 124);
        mCheckpointAdjustPos["Snowfield_8_0"] = new Vector3(345, 20);
        mCheckpointAdjustPos["Snowfield_8_1"] = new Vector3(775, 128);
        mCheckpointAdjustPos["Snowfield_8_2"] = new Vector3(1214, 200);
        mCheckpointAdjustPos["Snowfield_8_3"] = new Vector3(1660, 101);
        mCheckpointAdjustPos["Snowfield_8_4"] = new Vector3(2143, 20);
        mCheckpointAdjustPos["Snowfield_8_5"] = new Vector3(2619, 146);
        mCheckpointAdjustPos["Snowfield_8_6"] = new Vector3(3090, 152);
        mCheckpointAdjustPos["Snowfield_8_7"] = new Vector3(3560, 104);
        mCheckpointAdjustPos["Street_8_1"] = new Vector3(777, 176);
        mCheckpointAdjustPos["Street_8_5"] = new Vector3(2619, 216);

        //处理10个人的场景位置差别
        mCheckpointAdjustPos["Battle_10_0"] = new Vector3(355, 75);
        mCheckpointAdjustPos["Battle_10_1"] = new Vector3(685, -25);
        mCheckpointAdjustPos["Battle_10_2"] = new Vector3(975, 105);
        mCheckpointAdjustPos["Battle_10_3"] = new Vector3(1005, -145);
        mCheckpointAdjustPos["Battle_10_4"] = new Vector3(1425, -145);
		mCheckpointAdjustPos["Battle_10_5"] = new Vector3(1265, 30);
        mCheckpointAdjustPos["Battle_10_6"] = new Vector3(1400, 205);
        mCheckpointAdjustPos["Battle_10_7"] = new Vector3(1145, 260);
        mCheckpointAdjustPos["Battle_10_8"] = new Vector3(850, 290);
        mCheckpointAdjustPos["Battle_10_9"] = new Vector3(565, 275);
        mCheckpointAdjustPos["Snowfield_10_0"] = new Vector3(1515, -25);
        mCheckpointAdjustPos["Snowfield_10_1"] = new Vector3(1405, -185);
        mCheckpointAdjustPos["Snowfield_10_2"] = new Vector3(1165, -160);
        mCheckpointAdjustPos["Snowfield_10_3"] = new Vector3(925, -250);
        mCheckpointAdjustPos["Snowfield_10_4"] = new Vector3(780, -65);
		mCheckpointAdjustPos["Snowfield_10_5"] = new Vector3(625, 120);
        mCheckpointAdjustPos["Snowfield_10_6"] = new Vector3(415, 200);
        mCheckpointAdjustPos["Snowfield_10_7"] = new Vector3(725, 325);
        mCheckpointAdjustPos["Snowfield_10_8"] = new Vector3(1015, 265);
        mCheckpointAdjustPos["Snowfield_10_9"] = new Vector3(1460, 315);
        mCheckpointAdjustPos["Street_10_0"] = new Vector3(360, 225);
		mCheckpointAdjustPos["Street_10_1"] = new Vector3(655, 245);
		mCheckpointAdjustPos["Street_10_2"] = new Vector3(900, 150);
		mCheckpointAdjustPos["Street_10_3"] = new Vector3(540, 75);
        mCheckpointAdjustPos["Street_10_4"] = new Vector3(715, -80);
		mCheckpointAdjustPos["Street_10_5"] = new Vector3(955, -70);
        mCheckpointAdjustPos["Street_10_6"] = new Vector3(1200, -130);
		mCheckpointAdjustPos["Street_10_7"] = new Vector3(1365, 10);
		mCheckpointAdjustPos["Street_10_8"] = new Vector3(1120, 190);
		mCheckpointAdjustPos["Street_10_9"] = new Vector3(1350, 280);
		mCheckpointAdjustPos["Gardens_10_0"] = new Vector3(485, 275);
		mCheckpointAdjustPos["Gardens_10_1"] = new Vector3(545, 50);
		mCheckpointAdjustPos["Gardens_10_2"] = new Vector3(485, -185);
		mCheckpointAdjustPos["Gardens_10_3"] = new Vector3(765, -210);
        mCheckpointAdjustPos["Gardens_10_4"] = new Vector3(775, 25);
		mCheckpointAdjustPos["Gardens_10_5"] = new Vector3(980, 75);
        mCheckpointAdjustPos["Gardens_10_6"] = new Vector3(1190, -25);
		mCheckpointAdjustPos["Gardens_10_7"] = new Vector3(1340, 50);
		mCheckpointAdjustPos["Gardens_10_8"] = new Vector3(1140, 235);
		mCheckpointAdjustPos["Gardens_10_9"] = new Vector3(1350, 295);
		mCheckpointAdjustPos["Grassland_10_0"] = new Vector3(1730, 120);
		mCheckpointAdjustPos["Grassland_10_1"] = new Vector3(1400, 240);
		mCheckpointAdjustPos["Grassland_10_2"] = new Vector3(1075, 195);
		mCheckpointAdjustPos["Grassland_10_3"] = new Vector3(1310, 55);
        mCheckpointAdjustPos["Grassland_10_4"] = new Vector3(1300, -170);
		mCheckpointAdjustPos["Grassland_10_5"] = new Vector3(955, -225);
        mCheckpointAdjustPos["Grassland_10_6"] = new Vector3(575, -215);
		mCheckpointAdjustPos["Grassland_10_7"] = new Vector3(775, -55);
		mCheckpointAdjustPos["Grassland_10_8"] = new Vector3(575, 25);
		mCheckpointAdjustPos["Grassland_10_9"] = new Vector3(505, 240);
		mCheckpointAdjustPos["Cave_10_0"] = new Vector3(625, 245);
		mCheckpointAdjustPos["Cave_10_1"] = new Vector3(515, 40);
		mCheckpointAdjustPos["Cave_10_2"] = new Vector3(735, 15);
		mCheckpointAdjustPos["Cave_10_3"] = new Vector3(735, -240);
        mCheckpointAdjustPos["Cave_10_4"] = new Vector3(1060, -200);
		mCheckpointAdjustPos["Cave_10_5"] = new Vector3(1070, 30);
        mCheckpointAdjustPos["Cave_10_6"] = new Vector3(1315, -65);
		mCheckpointAdjustPos["Cave_10_7"] = new Vector3(1595, -50);
		mCheckpointAdjustPos["Cave_10_8"] = new Vector3(1260, 160);
		mCheckpointAdjustPos["Cave_10_9"] = new Vector3(1440, 315);
		mCheckpointAdjustPos["Desert_10_0"] = new Vector3(1635, -35);
		mCheckpointAdjustPos["Desert_10_1"] = new Vector3(1220, 60);
		mCheckpointAdjustPos["Desert_10_2"] = new Vector3(895, -15);
		mCheckpointAdjustPos["Desert_10_3"] = new Vector3(1065, -205);
        mCheckpointAdjustPos["Desert_10_4"] = new Vector3(730, -155);
		mCheckpointAdjustPos["Desert_10_5"] = new Vector3(580, 25);
        mCheckpointAdjustPos["Desert_10_6"] = new Vector3(520, 290);
		mCheckpointAdjustPos["Desert_10_7"] = new Vector3(775, 300);
		mCheckpointAdjustPos["Desert_10_8"] = new Vector3(1015, 220);
		mCheckpointAdjustPos["Desert_10_9"] = new Vector3(1315, 270);


        //单个敌人相关

        mspriteTitle = f_GetObject("Title").GetComponent<UISprite>();
        _enemyItems = new TollgateSelectItem[ItemNum];
		_enemyItems_2 = new TollgateSelectItem[ItemNum];
		//My Code
		// isFamousGeneral = mData.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_Legend;
		GameObject objEnemyItem = f_GetObject("SelectItem");
        Transform objEnemyParent = f_GetObject("ItemParent").transform;
		GameObject objEnemyItem_2 = f_GetObject("SelectItem_2");
        Transform objEnemyParent_2 = f_GetObject("ItemParent_2").transform;
		for (int i = 0; i < ItemNum; i++)
        {
            GameObject obj = GameObject.Instantiate(objEnemyItem);
            obj.name = "SelectItem" + (i + 1);
            obj.transform.parent = objEnemyParent;
            obj.transform.localScale = Vector3.one;
            obj.transform.localEulerAngles = Vector3.zero;
            _enemyItems[i] = obj.GetComponent<TollgateSelectItem>();
        }
		for (int i = 0; i < ItemNum; i++)
        {
            GameObject obj_2 = GameObject.Instantiate(objEnemyItem_2);
            obj_2.name = "SelectItem_2_" + (i + 1);
            obj_2.transform.parent = objEnemyParent_2;
            obj_2.transform.localScale = Vector3.one;
            obj_2.transform.localEulerAngles = Vector3.zero;
            _enemyItems_2[i] = obj_2.GetComponent<TollgateSelectItem>();
        }
		//

        //适配滑动面板，滑动item的背景布满屏幕，，为了在不同分辨率下布满屏幕且不变形，所以以大的比例同比放大（放大也不会使item重要的内容在屏幕外，背景图部分在屏幕外不关心！）
        // _selectItemScrollView = f_GetObject("SelectItemScrollView").GetComponent<UIScrollView>();        
        // Vector2 scrollViewScale = _selectItemScrollView.transform.GetComponent<UIPanel>().GetViewSize();
        // scrollViewScale *= adaptiveScale;
        // _selectItemScrollView.transform.GetComponent<UIPanel>().SetRect(0, 0, scrollViewScale.x, scrollViewScale.y);
        // _selectItemScrollBar = f_GetObject("SelectItemScrollBar").GetComponent<UIProgressBar>();

        //关卡地图
        _map = new UITexture[1];
        _map[0] = f_GetObject("Texture_BG1").GetComponent<UITexture>();
        // _map[1] = f_GetObject("Texture_BG2").GetComponent<UITexture>();

        //宝箱相关
        mChapterBoxParent = f_GetObject("ChapterBoxParent");
        mBoxSpriteNames = new string[3][];
        mBoxSpriteNames[0] = new string[(int)EM_BoxGetState.Invalid];
        mBoxSpriteNames[0][(int)EM_BoxGetState.Lock] = "ptfb_get_aa";
        mBoxSpriteNames[0][(int)EM_BoxGetState.CanGet] = "ptfb_get_aa";
        mBoxSpriteNames[0][(int)EM_BoxGetState.AlreadyGet] = "ptfb_get_a";
        mBoxSpriteNames[1] = new string[(int)EM_BoxGetState.Invalid];
        mBoxSpriteNames[1][(int)EM_BoxGetState.Lock] = "ptfb_get_bb";
        mBoxSpriteNames[1][(int)EM_BoxGetState.CanGet] = "ptfb_get_bb";
        mBoxSpriteNames[1][(int)EM_BoxGetState.AlreadyGet] = "ptfb_get_b";
        mBoxSpriteNames[2] = new string[(int)EM_BoxGetState.Invalid];
        mBoxSpriteNames[2][(int)EM_BoxGetState.Lock] = "ptfb_get_cc";
        mBoxSpriteNames[2][(int)EM_BoxGetState.CanGet] = "ptfb_get_cc";
        mBoxSpriteNames[2][(int)EM_BoxGetState.AlreadyGet] = "ptfb_get_c";
        mChapterBox1 = f_GetObject("ChapterBox1").transform;
        mChapterBox2 = f_GetObject("ChapterBox2").transform;
        mChapterBox3 = f_GetObject("ChapterBox3").transform;
        mChapterBoxSlider = f_GetObject("ChapterBoxSlider").GetComponent<UISlider>();
        mChapterBoxLabel = f_GetObject("ChapterBoxLabel").GetComponent<UILabel>();        
        boxEffectParent1 = f_GetObject("BoxEffectParent1").transform;
        boxEffectParent2 = f_GetObject("BoxEffectParent2").transform;
        boxEffectParent3 = f_GetObject("BoxEffectParent3").transform;

        //名将副本剩余挑战次数
        _famousGeneralRemainTimes = f_GetObject("FamousGeneralRemainTimes");
        _txtfamousGeneralRemainTimes = f_GetObject("Label_FamousGeneralRemainTimes").GetComponent<UILabel>();

        f_RegClickEvent(mChapterBox1.gameObject, f_ChapterBoxClick, 1);
        f_RegClickEvent(mChapterBox2.gameObject, f_ChapterBoxClick, 2);
        f_RegClickEvent(mChapterBox3.gameObject, f_ChapterBoxClick, 3);
        f_RegClickEvent("BackBtn", BackBtnHandle);
        
        f_RegClickEvent("Icon_LineUp", OnClickLineup);
        f_RegClickEvent("Icon_Rank", OnClickRank);
        f_RegClickEvent("Sprite_LastChapter", OnClickLastChapter);
        f_RegClickEvent("Sprite_NextChapter", OnClickNextChapter);    
    }
       
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Data_Pool.m_GuidancePool.OpenLevelPageUIName = UINameConst.DungeonTollgatePageNew;
		//My Code
		TexBgKD = GameObject.Find("TexBgKD");
		if(TexBgKD != null)
		{
			if(AssetOpen.iParam2 == 0)
			{
				TexBgKD.SetActive(false);
			}
		}
		
		float windowAspect = (float)Screen.width /  (float) Screen.height ;
		MessageBox.ASSERT("" + windowAspect);
		if(windowAspect <= 1.55)
		{
			f_GetObject("Checkpoint").transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
		
		//
        if (e == null && !(e is DungeonTollgatePageParam))
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1058));
        }
        DungeonTollgatePageParam param = (DungeonTollgatePageParam)e;
        f_OpenMoneyUI();
        
        mData = param.mDungeonPoolDT;

        //设置地图
        Texture2D map = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/" + mData.m_ChapterTemplate.szCheckpointMap);
		Texture2D map2 = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/Tex_LegendMap");
		//My Code
        // Texture2D map = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/Tex_GameBg");
		// if(AssetOpen.iParam1 == 1)
		// {
			// map = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("" + mData.m_ChapterTemplate.szCheckpointMap);
		// }
		//
        // for (int i = 0; i < _map.Length; i++)
        // {
            _map[0].mainTexture = map;
        // }

        Battle2MenuProcessParam tParam = StaticValue.m_Battle2MenuProcessParam;
        if(null != mData.m_ChapterTemplate && mData.m_ChapterTemplate.iChapterType > 0 && mData.m_ChapterTemplate.iChapterType < titleSpriteNames.Length)
        mspriteTitle.spriteName = titleSpriteNames[mData.m_ChapterTemplate.iChapterType];
        if (tParam != null && tParam.m_emType == EM_Battle2MenuProcess.Dungeon &&
            (EM_Fight_Enum)tParam.m_Params[0] != EM_Fight_Enum.eFight_DailyPve)//副本：主线，精英和名将。（不包含每日副本）
        {
            int tTollgateId = (int)StaticValue.m_Battle2MenuProcessParam.m_Params[2];
            if (tTollgateId == GameParamConst.PLOT_TOLLGATEID)
            {
                //剧情回来从第一关开始
                tTollgateId = 1;
            }
            DungeonTollgatePoolDT tToolgatePoolDt = mData.f_GetTollgateData(tTollgateId);
            f_SetCurrentFightModel2RightPos(tToolgatePoolDt.mIndex);
            f_UpdateEnemysInfo(tToolgatePoolDt.mIndex);
            StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.None);
            if (Data_Pool.m_RebelArmyPool.tRebelInfo.discovererId != 0)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelAymyTriggen, UIMessageDef.UI_OPEN, UINameConst.DungeonTollgatePageNew);
                Data_Pool.m_RebelArmyPool.tRebelInfo.discovererId = 0;
            }
        }
        else if (mData.mTollgatePassNum >= mData.mTollgateMaxNum)
        {
            //选关  名将副本特殊处理，已通过大魔王就选上一关
            if (mData.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_Legend && mData.mTollgatePassNum >= GameParamConst.LegendDungeonRestIdx)
            {
                DungeonTollgatePoolDT tToolgatePoolDt = mData.mTollgateList[mData.mTollgateMaxNum - 2];
                f_SetCurrentFightModel2RightPos(mData.mTollgateMaxNum - 2);
                f_UpdateEnemysInfo(tToolgatePoolDt.mIndex);
            }
            else
            {
                DungeonTollgatePoolDT tToolgatePoolDt = mData.mTollgateList[mData.mTollgateMaxNum - 1];
                f_SetCurrentFightModel2RightPos(mData.mTollgateMaxNum - 1);
                f_UpdateEnemysInfo(tToolgatePoolDt.mIndex);
            }
        }
        else
        {
            DungeonTollgatePoolDT tToolgatePoolDt = mData.mTollgateList[mData.mTollgatePassNum];
            f_SetCurrentFightModel2RightPos(mData.mTollgatePassNum);
            f_UpdateEnemysInfo(tToolgatePoolDt.mIndex);
        }
        //如果大于0，设置默认选中关卡
        if (param.SelectIndex >= 0 && param.SelectIndex < mData.mTollgateList.Count)
        {
            DungeonTollgatePoolDT tToolgatePoolDt = mData.mTollgateList[param.SelectIndex];
            f_SetCurrentFightModel2RightPos(param.SelectIndex);
            f_UpdateEnemysInfo(tToolgatePoolDt.mIndex);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonChallengePage, UIMessageDef.UI_OPEN, tToolgatePoolDt);
        }
		// MessageBox.ASSERT("Num: " + mData.mTollgatePassNum);
		if(mData.mTollgatePassNum < 3)
		{
			f_GetObject("LegendDrag").transform.localPosition = new Vector3(-40f, 30f, 0f);
		}
		else
		{
			f_GetObject("LegendDrag").transform.localPosition = new Vector3(80f, 30f, 0f);
		}
        f_UpdateChapterBoxs();

        //根据是否名将副本显隐剩余次数
        isFamousGeneral = mData.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_Legend;
        _famousGeneralRemainTimes.SetActive(isFamousGeneral);
		f_GetObject("LegendBorder").SetActive(false);
        if (isFamousGeneral) {
            _txtfamousGeneralRemainTimes.text = (Data_Pool.m_DungeonPool.mDungeonLegendLeftTimes).ToString();
			_map[0].mainTexture = map2;
			f_GetObject("LegendBorder").SetActive(true);		
			f_GetObject("LegendTitle").GetComponent<UILabel>().text = mData.m_ChapterTemplate.szChapterName.Replace(" ","\n");
			f_GetObject("LegendNum").GetComponent<UILabel>().text = (mData.mIndex + 1) + "";
        }

        //设置上一章下一章按钮显隐
        bool isShowLastChapter = mData.iId > (mData.m_ChapterTemplate.iChapterType - 1) * 1000 + 1;
        f_GetObject("Sprite_LastChapter").SetActive(isShowLastChapter);
        BasePoolDT<long> data = Data_Pool.m_DungeonPool.f_GetForId(mData.iId + 1);
        if (null == data)
        {
            f_GetObject("Sprite_NextChapter").SetActive(false);
            return;
        }
        DungeonPoolDT dungeonDT = data as DungeonPoolDT;
        bool dungeonIsLock = Data_Pool.m_DungeonPool.f_CheckChapterLockState(dungeonDT, false);
        f_GetObject("Sprite_NextChapter").SetActive(!dungeonIsLock);
    }
	
	void Update()
	{
		if(!isFamousGeneral)
		{
			resetPosition = 0;
		}
		if(resetPosition < 2)
		{
			f_GetObject("SelectItemScrollBar").GetComponent<UIScrollBar>().value = 1f;		
			resetPosition++;
		}
	}

    /// <summary>
    /// 更新关卡敌人信息
    /// </summary>
    private void f_UpdateEnemysInfo(int selectIdx)
    {
        Vector3[] enemyPos = mvecCheckpointPosOf10Enemy;
        if (mData.mTollgateMaxNum == 4) enemyPos = mvecCheckpointPosOf4Enemy;
        if (mData.mTollgateMaxNum == 5) enemyPos = mvecCheckpointPosOf5Enemy;
        if (mData.mTollgateMaxNum == 6) enemyPos = mvecCheckpointPosOf6Enemy;
        if (mData.mTollgateMaxNum == 8) enemyPos = mvecCheckpointPosOf8Enemy;
		if(mData.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_Legend)
		{
			f_GetObject("ItemParent_2").SetActive(true);
			f_GetObject("ItemParent").SetActive(false);
			for (int i = 0; i < _enemyItems_2.Length; i++)
			{
				if (i < mData.mTollgateMaxNum)
				{
					Vector3 pos = enemyPos[i];
					string posKey = string.Format("{0}_{1}_{2}", mData.m_ChapterTemplate.szCheckpointMap, mData.mTollgateMaxNum, i);
					if (mCheckpointAdjustPos.ContainsKey(posKey)) {
						pos = mCheckpointAdjustPos[posKey];
					}
					_enemyItems_2[i].f_Init(mData.m_ChapterTemplate.iChapterType, mData.mIndex, mData.mTollgatePassNum, 
						selectIdx, mData.mTollgateList[i], f_SelectItemClik, this, pos, (i + 1) == mData.mTollgateMaxNum);
				}
				else
				{
					_enemyItems_2[i].f_Disable();
				}
			}
		}
		else
		{
			f_GetObject("ItemParent").SetActive(true);
			f_GetObject("ItemParent_2").SetActive(false);
			for (int i = 0; i < _enemyItems.Length; i++)
			{
				if (i < mData.mTollgateMaxNum)
				{
					Vector3 pos = enemyPos[i];
					string posKey = string.Format("{0}_{1}_{2}", mData.m_ChapterTemplate.szCheckpointMap, mData.mTollgateMaxNum, i);
					if (mCheckpointAdjustPos.ContainsKey(posKey)) {
						pos = mCheckpointAdjustPos[posKey];
					}
					_enemyItems[i].f_Init(mData.m_ChapterTemplate.iChapterType, mData.mIndex, mData.mTollgatePassNum, 
						selectIdx, mData.mTollgateList[i], f_SelectItemClik, this, pos, (i + 1) == mData.mTollgateMaxNum);
				}
				else
				{
					_enemyItems[i].f_Disable();
				}
			}
		}
    }

    /// <summary>
    /// 最后一个可以打的模型永远在最右边
    /// </summary>
    /// <param name="idxNum"></param>
    private void f_SetCurrentFightModel2RightPos(int idxNum)
    {
        idxNum += 1;
        // _selectItemScrollView.ResetPosition();

        //当前挑战的模型都在屏幕最右边，，蛋疼的是策划要求每一页摆放的模型数量不固定，按章节配的怪物数量摆放，所以适配也要随之改变
        float percent = 0;
        float limitPerent = 0;
        if (mData.mTollgateMaxNum >= ItemNumPrePage) {
            //大于5个则策划要求摆满两屏，此时就要根据关卡总数来滑动到指定位置及滑动限制
            int realEnemyNumOfPage = mData.mTollgateMaxNum / 2;
            float startValue = 0;
            float space = 1.0f / realEnemyNumOfPage;
            if (mData.mTollgateMaxNum == 5)
            {
                startValue = -0.15f;
            }
            percent = idxNum <= realEnemyNumOfPage ? 0 : (startValue + space * (idxNum - realEnemyNumOfPage));

            //根据当前通关数来限定滑动范围
            int limitTollgateNum = mData.mTollgatePassNum >= mData.mTollgateMaxNum ? mData.mTollgatePassNum : mData.mTollgatePassNum + 1;
            limitPerent = limitTollgateNum <= realEnemyNumOfPage ? 0 : (startValue + space * (limitTollgateNum - realEnemyNumOfPage));           
        }       
        // _selectItemScrollBar.value = percent;
        // _selectItemScrollView.mCanDragLimitPos = new Vector2(-limitPerent * _selectItemScrollView.panel.GetViewSize().x, 0);
        // _selectItemScrollView.enabled = mData.mTollgateMaxNum >= ItemNumPrePage && limitPerent > 0;
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);       
        f_CloseMoneyUI();
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        f_CloseMoneyUI();
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
		//My Code
		TexBgKD = GameObject.Find("TexBgKD");
		if(TexBgKD != null)
		{
			if(AssetOpen.iParam2 == 0)
			{
				TexBgKD.SetActive(false);
			}
		}
		//
        Data_Pool.m_GuidancePool.OpenLevelPageUIName = UINameConst.DungeonTollgatePageNew;
        f_OpenMoneyUI();
    }

    /// <summary>
    /// 打开货币界面
    /// </summary>
    private void f_OpenMoneyUI()
    {
        //打开货币界面
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }

    /// <summary>
    /// 关闭货币界面
    /// </summary>
    private void f_CloseMoneyUI()
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    #region 事件函数
    //关闭处理
    private void BackBtnHandle(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonTollgatePageNew, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold(mData);
    }

    //点击阵容
    private void OnClickLineup(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        if (ccUIManage.GetInstance().f_CheckUIIsOpen(UINameConst.DungeonChapterPageNew))
        {
            ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.DungeonChapterPageNew));
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LineUpPage, UIMessageDef.UI_OPEN);
    }

    //点击排行榜
    private void OnClickRank(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        if (ccUIManage.GetInstance().f_CheckUIIsOpen(UINameConst.DungeonChapterPageNew))
        {
            ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.DungeonChapterPageNew));
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RankListPage, UIMessageDef.UI_OPEN, EM_RankListType.Duplicate);
    }

    //点击上一章
    private void OnClickLastChapter(GameObject go, object obj1, object obj2)
    {
        if (mData.iId <= 1)
        {
            return;
        }
        long chapterId = mData.iId - 1;
        ChangeChapter(chapterId);
    }

    //点击下一章
    private void OnClickNextChapter(GameObject go, object obj1, object obj2)
    {
        long chapterId = mData.iId + 1;
        ChangeChapter(chapterId);
    }

    //切换章节
    private void ChangeChapter(long chapterId)
    {
        BasePoolDT<long> data = Data_Pool.m_DungeonPool.f_GetForId(chapterId);
        if (null == data)
        {
            return;
        }
        DungeonPoolDT dungeonDT = data as DungeonPoolDT;
        if (Data_Pool.m_DungeonPool.f_CheckChapterLockState(dungeonDT, true))
        {
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_DungeonPool.f_ExecuteAfterInitDungeon(dungeonDT, (object value) =>
        {
            UITool.f_OpenOrCloseWaitTip(false);
            if (value is DungeonPoolDT)
            {
                DungeonPoolDT poolDt = (DungeonPoolDT)value;
                DungeonTollgatePageParam param = new DungeonTollgatePageParam();
                param.mDungeonPoolDT = poolDt;
                ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonTollgatePageNew, UIMessageDef.UI_OPEN, param);
            }
            else
            {
                UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1054) + value);
            }
        });
		f_GetObject("LegendScroll").GetComponent<UIScrollView>().ResetPosition();
		if((f_GetObject("SelectItemScrollBar").GetComponent<UIScrollBar>().value) == 0)
		{
			f_GetObject("SelectItemScrollBar").GetComponent<UIScrollBar>().value = 1f;
		}
    }
   
    /// <summary>
    /// 点击关卡人物，弹出挑战界面
    /// </summary>
    /// <param name="value"></param>
    private void f_SelectItemClik(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonChallengePage, UIMessageDef.UI_OPEN, value);
    }
    #endregion

    #region 宝箱相关
    /// <summary>
    /// 更新关卡宝箱
    /// </summary>
    private void f_UpdateChapterBoxs()
    {
        mChapterBoxParent.SetActive(true);
        if (mData.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_Legend)
        {
            mChapterBoxParent.SetActive(false);
            return;
        }
        f_ProcessChapterBox(0,mChapterBox1, mData.mBox1Get, mData.mStarNum, mData.m_ChapterTemplate.iNeedStar1, boxEffectParent1, ref boxEffectItem1);
        f_ProcessChapterBox(1,mChapterBox2, mData.mBox2Get, mData.mStarNum, mData.m_ChapterTemplate.iNeedStar2, boxEffectParent2, ref boxEffectItem2);
        f_ProcessChapterBox(2,mChapterBox3, mData.mBox3Get, mData.mStarNum, mData.m_ChapterTemplate.iNeedStar3, boxEffectParent3, ref boxEffectItem3);

        mChapterBoxLabel.text = string.Format("{0}/{1}", mData.mStarNum, mData.mTollgateMaxNum * GameParamConst.StarNumPreTollgate);
		if(mData.m_ChapterTemplate.iNeedStar3 == 30)
		{
			mChapterBoxSlider.value = (float)mData.mStarNum/(float)mData.m_ChapterTemplate.iNeedStar3;
		}
		else
		{
			MessageBox.ASSERT("Value: " + ((float)mData.mStarNum/(float)mData.m_ChapterTemplate.iNeedStar1 * 1/3));
			if (mData.mStarNum < mData.m_ChapterTemplate.iNeedStar1)
				mChapterBoxSlider.value = (((float)mData.mStarNum/(float)mData.m_ChapterTemplate.iNeedStar1) * 1/3);
			else if (mData.mStarNum < mData.m_ChapterTemplate.iNeedStar2)
				mChapterBoxSlider.value = (((float)mData.mStarNum/(float)mData.m_ChapterTemplate.iNeedStar2) * 2/3);
			else if (mData.mStarNum < mData.m_ChapterTemplate.iNeedStar3)
				mChapterBoxSlider.value = (float)mData.mStarNum/(float)mData.m_ChapterTemplate.iNeedStar3;
			else
				mChapterBoxSlider.value = 1.0f;
		}
    }

    /// <summary>
    /// 设置关卡宝箱
    /// </summary>
    /// <param name="boxIndex"></param>
    /// <param name="chapterBox"></param>
    /// <param name="isAlreadyGet"></param>
    /// <param name="curStar"></param>
    /// <param name="needStar"></param>
    /// <param name="effectParent"></param>
    /// <param name="effectItem"></param>
    private void f_ProcessChapterBox(int boxIndex,Transform chapterBox, bool isAlreadyGet, int curStar, int needStar, Transform effectParent, ref GameObject effectItem)
    {
        EM_BoxGetState state;
        if (isAlreadyGet)
            state = EM_BoxGetState.AlreadyGet;
        else
            state = curStar >= needStar ? EM_BoxGetState.CanGet : EM_BoxGetState.Lock;
        effectParent.transform.Find("Reddot").gameObject.SetActive(state == EM_BoxGetState.CanGet);
        chapterBox.GetComponent<UISprite>().spriteName = mBoxSpriteNames[boxIndex][(int)state];
        // chapterBox.GetComponent<UISprite>().MakePixelPerfect();
        if (state == EM_BoxGetState.Lock)
        {            
            if (effectItem != null && !effectItem.name.Equals(string.Format("{0}(Clone)", UIEffectName.BoxLockEffect1)))
            {
                GameObject.Destroy(effectItem);
                effectItem = null;
            }
            if (effectItem == null)
            {
                //todo:屏蔽老光效，以后有再打开
                //effectItem = UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.BoxLockEffect1, effectParent);
            }
        }
        else if (state == EM_BoxGetState.CanGet)
        {
            if (effectItem != null && !effectItem.name.Equals(string.Format("{0}(Clone)", UIEffectName.BoxCanGetEffect1)))
            {
                GameObject.Destroy(effectItem);
                effectItem = null;
            }
            if (effectItem == null)
            {
                //todo:屏蔽老光效，以后有再打开
                //effectItem = UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.BoxCanGetEffect1, effectParent);
            }
        }
        else if (state == EM_BoxGetState.AlreadyGet)
        {
            if (effectItem != null)
            {
                GameObject.Destroy(effectItem);
                effectItem = null;
            }
        }
        chapterBox.Find("Num").GetComponent<UILabel>().text = needStar.ToString();
        chapterBox.Find("Sprite_Select").gameObject.SetActive(state == EM_BoxGetState.AlreadyGet || state == EM_BoxGetState.CanGet);
    }

     
    private void f_ChapterBoxClick(GameObject go, object value1, object value2)
    {
        int tIdx = (int)value1;
        if (tIdx == 1)
        {
            BoxGetSubPageParam tParam;
            if (mData.mStarNum < mData.m_ChapterTemplate.iNeedStar1)
            {
                tParam = new BoxGetSubPageParam(mData.m_ChapterTemplate.iBox1, tIdx, mData.m_ChapterTemplate.iNeedStar1.ToString(), EM_BoxType.Chapter, EM_BoxGetState.Lock, DungeonChapterBox_GetClick, this);
            }
            else if (mData.mBox1Get)
            {
                tParam = new BoxGetSubPageParam(mData.m_ChapterTemplate.iBox1, tIdx, mData.m_ChapterTemplate.iNeedStar1.ToString(), EM_BoxType.Chapter, EM_BoxGetState.AlreadyGet, DungeonChapterBox_GetClick, this);
            }
            else
            {
                tParam = new BoxGetSubPageParam(mData.m_ChapterTemplate.iBox1, tIdx, mData.m_ChapterTemplate.iNeedStar1.ToString(), EM_BoxType.Chapter, EM_BoxGetState.CanGet, DungeonChapterBox_GetClick, this);
            }
            ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_OPEN, tParam);
        }
        else if (tIdx == 2)
        {
            BoxGetSubPageParam tParam;
            if (mData.mStarNum < mData.m_ChapterTemplate.iNeedStar2)
            {
                tParam = new BoxGetSubPageParam(mData.m_ChapterTemplate.iBox2, tIdx, mData.m_ChapterTemplate.iNeedStar2.ToString(), EM_BoxType.Chapter, EM_BoxGetState.Lock, DungeonChapterBox_GetClick, this);
            }
            else if (mData.mBox2Get)
            {
                tParam = new BoxGetSubPageParam(mData.m_ChapterTemplate.iBox2, tIdx, mData.m_ChapterTemplate.iNeedStar2.ToString(), EM_BoxType.Chapter, EM_BoxGetState.AlreadyGet, DungeonChapterBox_GetClick, this);
            }
            else
            {
                tParam = new BoxGetSubPageParam(mData.m_ChapterTemplate.iBox2, tIdx, mData.m_ChapterTemplate.iNeedStar2.ToString(), EM_BoxType.Chapter, EM_BoxGetState.CanGet, DungeonChapterBox_GetClick, this);
            }
            ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_OPEN, tParam);
        }
        else if (tIdx == 3)
        {
            BoxGetSubPageParam tParam;
            if (mData.mStarNum < mData.m_ChapterTemplate.iNeedStar3)
            {
                tParam = new BoxGetSubPageParam(mData.m_ChapterTemplate.iBox3, tIdx, mData.m_ChapterTemplate.iNeedStar3.ToString(), EM_BoxType.Chapter, EM_BoxGetState.Lock, DungeonChapterBox_GetClick, this);
            }
            else if (mData.mBox3Get)
            {
                tParam = new BoxGetSubPageParam(mData.m_ChapterTemplate.iBox3, tIdx, mData.m_ChapterTemplate.iNeedStar3.ToString(), EM_BoxType.Chapter, EM_BoxGetState.AlreadyGet, DungeonChapterBox_GetClick, this);
            }
            else
            {
                tParam = new BoxGetSubPageParam(mData.m_ChapterTemplate.iBox3, tIdx, mData.m_ChapterTemplate.iNeedStar3.ToString(), EM_BoxType.Chapter, EM_BoxGetState.CanGet, DungeonChapterBox_GetClick, this);
            }
            ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_OPEN, tParam);
        }
    }

    //宝箱界面领取响应
    private void DungeonChapterBox_GetClick(object value)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        mCurChapterBoxIdx = (int)value;
        SocketCallbackDT tCallBackDT = new SocketCallbackDT();
        tCallBackDT.m_ccCallbackSuc = CallBack_DungeonChapterBox_Suc;
        tCallBackDT.m_ccCallbackFail = CallBack_DungeonChapterBox_Fail;
        //发送章节宝箱协议
        Data_Pool.m_DungeonPool.f_DungeonChapterBox(mData.m_iChapterTemplateId, (byte)mCurChapterBoxIdx, tCallBackDT);
    }

    private void CallBack_DungeonChapterBox_Suc(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        MessageBox.DEBUG("DungeonChapterBox Suc! code:" + result);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_CLOSE);
        Data_Pool.m_GuidancePool.m_OtherSave = true;
        //刷新章节宝箱
        f_UpdateChapterBoxs();
        //打开奖励界面
        if (mCurChapterBoxIdx == 1)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(mData.m_ChapterTemplate.iBox1), this });
        }
        else if (mCurChapterBoxIdx == 2)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(mData.m_ChapterTemplate.iBox2), this });
        }
        else if (mCurChapterBoxIdx == 3)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(mData.m_ChapterTemplate.iBox3), this });
        }
    }

    private void CallBack_DungeonChapterBox_Fail(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.UI_ShowFailContent("DungeonChapterBox Error! code:" + result);
    }
    #endregion
}
