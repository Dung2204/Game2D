using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// 副本界面
/// </summary>
public class DungeonChapterPageNew : UIFramwork
{
    private const int ShowNumEachPage      = 1;                           //一页显示元素的item数量
    private const int buildingNumOfOnePage = 4;                           //普通和精英副本每页4个建筑，，
    private int[] legendBuildingNumOfPage  = new int[3] { 3, 3, 4 };      //万恶的名将副本每页建筑不是相等的，，每页依次有3，3，4个建筑！！！

    //主线、精英、传说副本公共滑动面板相关
    private float adaptiveScale = ScreenControl.Instance.mScaleRatio;
    private GameObject _commonDragObj;
    private GameObject _commonItemParent;
	//My Code
	GameParamDT ActivityOpen;
	GameObject TexBgKD;
	int count  = 0;
	bool isDailyPve = false;
	//
    private UIScrollView _commonScrollView;
    private UIWrapComponent _commonWrapComponent;
    private UIWrapComponent commonWrapComponent
    {
        get
        {
            if (_commonWrapComponent == null)
            {
                //适配滑动面板，滑动item的背景布满屏幕，，为了在不同分辨率下布满屏幕且不变形，所以以大的比例同比放大
                Vector2 scrollViewScale = _commonScrollView.transform.GetComponent<UIPanel>().GetViewSize();
                scrollViewScale *= adaptiveScale;
                _commonScrollView.transform.GetComponent<UIPanel>().SetRect(0, 0, scrollViewScale.x, scrollViewScale.y);

                //默认用主线数据
                List<BasePoolDT<long>>  _mainLineList = Data_Pool.m_DungeonPool.f_GetAllForData1((int)EM_Fight_Enum.eFight_DungeonMain);
                List<BasePoolDT<long>> _mainLinePageDataList = Data_Pool.m_DungeonPool.f_AllDungeonPoolDTs2DungeonPoolDTOfPage(_mainLineList, new int[1] { buildingNumOfOnePage });
                _commonWrapComponent = new UIWrapComponent((int)scrollViewScale.x, 1, 3840, 3, _commonItemParent, _DungeonCommonItem, _mainLinePageDataList, CommonItemUpdateByInfo, null);

                //适配每个“拖动选项卡页”，（同比放大会导致部分重要的ui元素再屏幕外，所以这一部分再同比缩小）
                for (int i = 0; i < _commonItemParent.transform.childCount; i++)
                {
                    Transform chapterPage = _commonItemParent.transform.GetChild(i);
                    chapterPage.localScale = Vector3.one * adaptiveScale;

                    //缩放每个章节数据
                    for (int j = 1; j <= buildingNumOfOnePage; j++) {
                        Transform item = chapterPage.Find("CommonItem" + j);
                        item.localScale = Vector3.one / adaptiveScale;
                    }
                }
            }
            return _commonWrapComponent;
        }
    }
    private Texture[] _comonMapTexture;               //主线和精英地图贴图
    private Vector3[] _commonMapBuildingPos;          //主线和精英地图建筑位置
    private Texture[] _FamousGeneralMapTexture;       //名将地图贴图
    private Vector3[] _FamousGeneralMapBuildingPos;   //名将地图建筑位置
	private Texture[] _eliteMapTexture;

    private GameObject _DungeonCommonItem;
    private Transform _mainLineBtn;
    private Transform _eliteBtn;
    private Transform _famousGeneralBtn;
    private UILabel _famousGeneralRemainTimes;
    private UISprite _spriteTitle;
    private string[] titleSpriteNames = new string[5];
    private string[] dungeonTypesSprite = new string[(int)EM_Fight_Enum.eFight_DailyPve];

    //切换副本效果相关
    private TweenAlpha _swtichDisplayAlpha;
    private UISprite _spriteDungeonType;    

    private EM_Fight_Enum _curDungeonType;
	//My Code
	GameParamDT AssetOpen;
	//
    

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
		//My Code
		ActivityOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(90) as GameParamDT);
		//
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();

        //设置章节底图贴图
		//My Code
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		f_RegClickEvent("BackBtn", BackBtnHandle); 
        f_RegClickEvent("Sprite_CardPieceGuide", OnCardPieceGuide); 
        f_RegClickEvent("Sprite_OneKeyRewad", OnOneKeyRewad);
        f_RegClickEvent("BtnHelp", BtnOpenHelp);
		//
        _comonMapTexture = new Texture[10];
		//My Code
        _comonMapTexture[0] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/chapter_bg_1");
		_comonMapTexture[1] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/chapter_bg_2");
		_comonMapTexture[2] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/chapter_bg_3");
		_comonMapTexture[3] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/chapter_bg_4");
		_comonMapTexture[4] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/chapter_bg_5");
		_comonMapTexture[5] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/chapter_bg_6");
		_comonMapTexture[6] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/chapter_bg_7");
		_comonMapTexture[7] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/chapter_bg_8");
		_comonMapTexture[8] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/chapter_bg_9");
		_comonMapTexture[9] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/chapter_bg_10");
		_eliteMapTexture = new Texture[10];
		_eliteMapTexture[0] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/elite_chapter_bg_1");
		_eliteMapTexture[1] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/elite_chapter_bg_2");
		_eliteMapTexture[2] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/elite_chapter_bg_3");
		_eliteMapTexture[3] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/elite_chapter_bg_4");
		_eliteMapTexture[4] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/elite_chapter_bg_5");
		_eliteMapTexture[5] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/elite_chapter_bg_6");
		_eliteMapTexture[6] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/elite_chapter_bg_7");
		_eliteMapTexture[7] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/elite_chapter_bg_8");
		_eliteMapTexture[8] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/elite_chapter_bg_9");
		_eliteMapTexture[9] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/elite_chapter_bg_10");
        _FamousGeneralMapTexture = new Texture[3];
        _FamousGeneralMapTexture[0] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/FamousGeneral1");
        _FamousGeneralMapTexture[1] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/FamousGeneral2");
        _FamousGeneralMapTexture[2] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/UITexture/Dungeon/FamousGeneral3");
		// if(AssetOpen.iParam1 == 1)
		// {
			// _comonMapTexture[0] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("ChapterMap");
			// _FamousGeneralMapTexture[0] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("FamousGeneral1");
			// _FamousGeneralMapTexture[1] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("FamousGeneral2");
			// _FamousGeneralMapTexture[2] = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("FamousGeneral3");
		// }
		//

        //设置章节底图位置
        _commonMapBuildingPos = new Vector3[4];
        _commonMapBuildingPos[0] = new Vector3(-710,26);
        _commonMapBuildingPos[1] = new Vector3(-162,176);
        _commonMapBuildingPos[2] = new Vector3(321,228);
        _commonMapBuildingPos[3] = new Vector3(732,34);
        _FamousGeneralMapBuildingPos = new Vector3[10];
        _FamousGeneralMapBuildingPos[0] = new Vector3(-595,42);
        _FamousGeneralMapBuildingPos[1] = new Vector3(33,153);
        _FamousGeneralMapBuildingPos[2] = new Vector3(777,195);
        _FamousGeneralMapBuildingPos[3] = new Vector3(-635,-75);
        _FamousGeneralMapBuildingPos[4] = new Vector3(-89,30);
        _FamousGeneralMapBuildingPos[5] = new Vector3(422,184);
        _FamousGeneralMapBuildingPos[6] = new Vector3(-980,5);
        _FamousGeneralMapBuildingPos[7] = new Vector3(-423,-138);
        _FamousGeneralMapBuildingPos[8] = new Vector3(165,-101);
        _FamousGeneralMapBuildingPos[9] = new Vector3(692,85);

        titleSpriteNames[(int)EM_Fight_Enum.eFight_DungeonMain] = "fb_title_a_ptfb";
        titleSpriteNames[(int)EM_Fight_Enum.eFight_DungeonElite] = "fb_title_a_jjfb";
        titleSpriteNames[(int)EM_Fight_Enum.eFight_Legend] = "fb_title_a_mjfb";
        titleSpriteNames[(int)EM_Fight_Enum.eFight_DailyPve] = "fb_title_a_rcfb";
        _spriteTitle = f_GetObject("Title").GetComponent<UISprite>();
        _commonDragObj = f_GetObject("CommonDragObj");
        _commonItemParent = f_GetObject("CommonItemParent");
        _commonScrollView = f_GetObject("CommonScrollView").GetComponent<UIScrollView>();
        _DungeonCommonItem = f_GetObject("CommonPage");
        _mainLineBtn = f_GetObject("MainLineBtn").transform;
        _eliteBtn = f_GetObject("EliteBtn").transform;

        dungeonTypesSprite[(int)EM_Fight_Enum.eFight_DungeonMain - 1] = "fb_pic_font_ptfb";
        dungeonTypesSprite[(int)EM_Fight_Enum.eFight_DungeonElite - 1] = "fb_pic_font_jjfb";
        dungeonTypesSprite[(int)EM_Fight_Enum.eFight_Legend - 1] = "fb_pic_font_mjfb";
        dungeonTypesSprite[(int)EM_Fight_Enum.eFight_DailyPve - 1] = "fb_pic_font_rcfb";

        //名将副本
        _famousGeneralBtn = f_GetObject("FamousGeneralBtn").transform;
        _famousGeneralRemainTimes = f_GetObject("Label_FamousGeneralRemainTimes").GetComponent<UILabel>();

        //任务副本
        _DailyPveBtn = f_GetObject("DailyPveBtn").transform;
        _DailyPveDragObj = f_GetObject("DailyPveDragObj");
        _DailyPveItemParent = f_GetObject("DailyPveItemParent");
        _DailyPveItem = f_GetObject("DailyPveItem");
        _DailyChallengeIcon = f_GetObject("Sprite_DailyChallengeIcon").GetComponent<UISprite>();
        _DailyChallengeBtn = f_GetObject("Sprite_DailyChallengeBtn").GetComponent<UISprite>();
        _DailyCost = f_GetObject("Label_DailyCost").GetComponent<UILabel>();
        _DailyChallengeTips = f_GetObject("Label_DailyChallengeTips").GetComponent<UILabel>();
        _DailyPveScrollView = f_GetObject("DailyPveScrollView").GetComponent<UIScrollView>();
        
        //切换副本效果相关
        _spriteDungeonType = f_GetObject("Sprite_DungeonType").GetComponent<UISprite>(); 
        _swtichDisplayAlpha = f_GetObject("SwitchDisplay").GetComponent<TweenAlpha>(); 

        f_RegClickEvent(_mainLineBtn.gameObject, SelectBtnHandle, EM_Fight_Enum.eFight_DungeonMain);
        f_RegClickEvent(_eliteBtn.gameObject, SelectBtnHandle, EM_Fight_Enum.eFight_DungeonElite);
        f_RegClickEvent(_famousGeneralBtn.gameObject, SelectBtnHandle, EM_Fight_Enum.eFight_Legend);
        f_RegClickEvent(_DailyPveBtn.gameObject, SelectBtnHandle, EM_Fight_Enum.eFight_DailyPve);       
        RequestQueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        RequestQueryCallback.m_ccCallbackFail = OnQueryFailCallback;  

         Vector3 posContent = f_GetObject("Content").transform.localPosition;
        posContent.x *= ScreenControl.Instance.mFunctionRatio;
        posContent.y *= ScreenControl.Instance.mFunctionRatio;
        f_GetObject("Content").transform.localPosition = posContent;
		if(ActivityOpen.iParam1 == 0)
		{
			f_GetObject("EliteBtn").SetActive(false);
			f_GetObject("FamousGeneralBtn").SetActive(false);
			f_GetObject("DailyPveBtn").SetActive(false);
		}
    }

    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.DailyPve, _DailyPveBtn.gameObject, ReddotCallback_Show_DailyPveBtn);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LegendHavaTimes, _famousGeneralBtn.gameObject, ReddotCallback_Show_famousGeneralBtn);
        UpdateReddotUI();

    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.DailyPve, _DailyPveBtn.gameObject);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.LegendHavaTimes, _famousGeneralBtn.gameObject);
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();

        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.DailyPve);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegendHavaTimes);
        bool MainDungeonRedPoint = Data_Pool.m_DungeonPool.f_CheckMainDungeonBoxGetRedPoint();
        UITool.f_UpdateReddot(_mainLineBtn.gameObject, MainDungeonRedPoint ? 1 : 0, new Vector3(215, -22, 0), 36);
        bool EliteDungeonRedPoint = Data_Pool.m_DungeonPool.f_CheckEliteDungeonBoxGetRedPoint();
        UITool.f_UpdateReddot(_eliteBtn.gameObject, EliteDungeonRedPoint ? 1 : 0, new Vector3(215, -22, 0), 36);
        bool OneKeyRewardRedPoint = Data_Pool.m_DungeonPool.f_CheckMainDungeonTollgateBoxGetRedPoint(_curDungeonType);
        UITool.f_UpdateReddot(f_GetObject("Sprite_OneKeyRewad"), OneKeyRewardRedPoint ? 1 : 0, new Vector3(30, 40, 0), 45);
    }

    private void ReddotCallback_Show_DailyPveBtn(object Obj)
    {
        int iNum = (int)Obj;
        UITool.f_UpdateReddot(_DailyPveBtn.gameObject, iNum, new Vector3(48, 48, 0), 74);
    }
    private bool isFamousReddot = false;
    private void ReddotCallback_Show_famousGeneralBtn(object Obj)
    {
        int iNum = (int)Obj;
        isFamousReddot = iNum > 0 ? true : false;
        UITool.f_UpdateReddot(_famousGeneralBtn.gameObject, iNum, new Vector3(48, 48, 0), 74);
    }
    #endregion

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        //case1.副本从战斗场景切换回主场景处理
		glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.Dungeon);
		//My Code
		TexBgKD = GameObject.Find("TexBgKD");
		if(TexBgKD != null)
		{
			if(AssetOpen.iParam2 == 0)
			{
				TexBgKD.SetActive(false);
			}
		}
		count = 0;
		//
        if (e != null && e is Battle2MenuProcessParam)
        {
            Battle2MenuProcessParam tParam = (Battle2MenuProcessParam)e;
            EM_Fight_Enum em_Fight = (EM_Fight_Enum)tParam.m_Params[0];
            f_UpdateByDungeonType(em_Fight);           
            int tIid = (int)tParam.m_Params[1];
            if (em_Fight == EM_Fight_Enum.eFight_DailyPve)//日常副本
            {
                return;
            }
            else//主线、精英和传说
            {
                if (tIid == GameParamConst.PLOT_TOLLGATEID)
                {
                    //剧情回来从第一关开始
                    tIid = 1;
                }
                DungeonPoolDT tDungeonPoolDt = (DungeonPoolDT)Data_Pool.m_DungeonPool.f_GetForId((long)tIid);
                if (Data_Pool.m_DungeonPool.f_CheckChapterLockState(tDungeonPoolDt))
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1046));
                    return;
                }
                UITool.f_OpenOrCloseWaitTip(true);
                Data_Pool.m_DungeonPool.f_ExecuteAfterInitDungeon(tDungeonPoolDt, f_Callback_DungeonTollgate);
                return;
            }
        }

        //case2.选择副本类型
        if (e != null && e is EM_Fight_Enum)
        {
            switch ((EM_Fight_Enum)e)
            {
                case EM_Fight_Enum.eFight_DungeonMain: //主线
                case EM_Fight_Enum.eFight_DungeonElite: //精英
                case EM_Fight_Enum.eFight_Legend: //传说
                case EM_Fight_Enum.eFight_DailyPve: //日常
                    SelectBtnHandle(null, (EM_Fight_Enum)e, null);
                    break;
                default:
                    SelectBtnHandle(null, EM_Fight_Enum.eFight_DungeonMain, null);
                    break;
            }
        }
        else
        {
            SelectBtnHandle(null, EM_Fight_Enum.eFight_DungeonMain, null);
        }
        if (Data_Pool.m_RebelArmyPool.tRebelInfo.discovererId != 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelAymyTriggen, UIMessageDef.UI_OPEN, UINameConst.DungeonChapterPageNew);
            Data_Pool.m_RebelArmyPool.tRebelInfo.discovererId = 0;
        }
		
		f_OpenMoneyUI();
    }
    /// <summary>
    /// 一键领奖回调
    /// </summary>
    /// <param name="value1"></param>
	void Update()
	{
		if(!isDailyPve)
		{
			count = 0;
		}
		if(count < 2 && isDailyPve && _DailyPveItemParent.transform.Find("0") != null)
		{
			_DailyPveItemParent.transform.Find("0").transform.localPosition = new Vector3( -720, -35 ,0);
			_DailyPveItemParent.transform.Find("1").transform.localPosition = new Vector3( 240, -190 ,0);
			_DailyPveItemParent.transform.Find("2").transform.localPosition = new Vector3( 860, -180 ,0);
			_DailyPveItemParent.transform.Find("3").transform.localPosition = new Vector3( 280f, 270f ,0);
			_DailyPveItemParent.transform.Find("4").transform.localPosition = new Vector3( -590f, 265f ,0);
			_DailyPveItemParent.transform.Find("5").transform.localPosition = new Vector3( -180, 90 ,0);
			f_GetObject("DailyPveScrollView").transform.localPosition = new Vector3( 0, 15 ,0);
			count++;
		}
	}
    private void f_CallBackByOneKeyGetReward(object value1)
    {
        //更新红点
        UpdateReddotUI();
        List<BasePoolDT<long>> _dungeonDataList = Data_Pool.m_DungeonPool.f_GetAllForData1((int)_curDungeonType);
        int[] buildPageCount = _curDungeonType == EM_Fight_Enum.eFight_Legend ? legendBuildingNumOfPage : new int[1] { buildingNumOfOnePage };
        commonWrapComponent.f_UpdateList(Data_Pool.m_DungeonPool.f_AllDungeonPoolDTs2DungeonPoolDTOfPage(_dungeonDataList, buildPageCount));
        commonWrapComponent.f_UpdateView();

        List<AwardPoolDT> _awardList1 = Data_Pool.m_DungeonPool.f_GetOneKeyGetRewardInfo();
        if (_awardList1.Count != 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { _awardList1, this });
        }
        else
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không có phần thưởng");
        }
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
        f_OpenMoneyUI();
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
        if (_curDungeonType == EM_Fight_Enum.eFight_DungeonMain || _curDungeonType == EM_Fight_Enum.eFight_DungeonElite ||
           _curDungeonType == EM_Fight_Enum.eFight_Legend)
        {
            commonWrapComponent.f_UpdateView();
            if (e != null && e is DungeonPoolDT)
            {
                DungeonPoolDT tChapterPoolDt = (DungeonPoolDT)e;
                int tIdx = Data_Pool.m_DungeonPool.f_GetFightChapterIdx((int)_curDungeonType, tChapterPoolDt.mIndex);             
                int realIndex =  tIdx / buildingNumOfOnePage;
                if (_curDungeonType == EM_Fight_Enum.eFight_Legend)
                {
                    //万恶的名将建筑根据页数可变的，，依次为3，3，4
                    realIndex = 0;
                    for (int j = tIdx; j > 0;)
                    {
                        for (int i = 0; i < legendBuildingNumOfPage.Length; i++)
                        {
                            j -= legendBuildingNumOfPage[i];
                            if (j <= 0) break;
                            realIndex++;
                        }
                    }
                }
                commonWrapComponent.f_ViewGotoRealIdx(realIndex, ShowNumEachPage);
            }
        }
        Data_Pool.m_DungeonPool.f_CheckLegionRedDot();
        UpdateReddotUI();
    }

    //关闭处理
    private void BackBtnHandle(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonChapterPageNew, UIMessageDef.UI_CLOSE);
		glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.MainBg);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void BtnOpenHelp(GameObject go, object obj1, object obj2) {
        switch (_curDungeonType)
        {
            case EM_Fight_Enum.eFight_Invalid:
                break;
            case EM_Fight_Enum.eFight_DungeonMain:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 1);
                break;
            case EM_Fight_Enum.eFight_DungeonElite:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 2);
                break;
            case EM_Fight_Enum.eFight_Legend:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 3);
                break;
            case EM_Fight_Enum.eFight_DailyPve:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 4);
                break;
        
        }

       
    }

    //主线宝鉴
    private void OnCardPieceGuide(GameObject go, object obj1, object obj2)
    {
        int lv = UITool.f_GetSysOpenLevel(EM_NeedLevel.MainLineTreasureBookLimitLv);
        if ((Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= lv))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.MainLineTreasureBookPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1), lv));
        }
    }

    //一键领取
    private void OnOneKeyRewad(GameObject go, object obj1, object obj2)
    {
        int lv = UITool.f_GetSysOpenLevel(EM_NeedLevel.MainLineOneKeyGetRewardLimitLv);
        if ((Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= lv))
        {
            SocketCallbackDT tSocketCallbackDT = new SocketCallbackDT();
            tSocketCallbackDT.m_ccCallbackSuc = f_CallBackByOneKeyGetReward;
            tSocketCallbackDT.m_ccCallbackFail = f_CallBackByOneKeyGetReward;
            Data_Pool.m_DungeonPool.f_GetCheckpointAndStarReward((ushort)_curDungeonType, tSocketCallbackDT);
        }
        else
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1), lv));
        }
    }

    private void SelectBtnHandle(GameObject go, object value1, object value2)
    {
		count = 0;
        EM_Fight_Enum tType = (EM_Fight_Enum)value1;
        switch (tType)
        {
            case EM_Fight_Enum.eFight_Invalid:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_DungeonMain:
				f_GetObject("Text").SetActive(true);
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_DungeonElite:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.DungeonEliteLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1047), UITool.f_GetSysOpenLevel(EM_NeedLevel.DungeonEliteLevel) ));
                    return;
                }
				f_GetObject("Text").SetActive(true);
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_Legend:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.LegendLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1048), UITool.f_GetSysOpenLevel(EM_NeedLevel.LegendLevel) ));
                    return;
                }
				f_GetObject("Text").SetActive(true);
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_DailyPve:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.DailyPveLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1049), UITool.f_GetSysOpenLevel(EM_NeedLevel.DailyPveLevel) ));
                    return;
                }
				f_GetObject("Text").SetActive(false);
				_DailyPveItemParent.SetActive(true);
                break;
            case EM_Fight_Enum.eFight_Rebel:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_Friend:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_Guild:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_Arena:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_ArenaSweep:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_CrusadeRebel:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_Boss:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_LegionDungeon:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_GrabTreasure:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_GrabTreasureSweep:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_RunningMan:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_RunningManElite:
				_DailyPveItemParent.SetActive(false);
                break;
            case EM_Fight_Enum.eFight_Patrol:
				_DailyPveItemParent.SetActive(false);
                break;
        }
        if (_curDungeonType == tType)
            return;
        f_UpdateByDungeonType(tType);
    }

    /// <summary>
    /// 根据副本类型更新
    /// </summary>
    /// <param name="dungeonType"></param>
    private void f_UpdateByDungeonType(EM_Fight_Enum dungeonType)
    {
        if (dungeonType != EM_Fight_Enum.eFight_DungeonMain && dungeonType != EM_Fight_Enum.eFight_DungeonElite &&
            dungeonType != EM_Fight_Enum.eFight_Legend && dungeonType != EM_Fight_Enum.eFight_DailyPve) return;
        _curDungeonType = dungeonType;
        UpdateReddotUI();
        ResetButton();
        SetButton(dungeonType);
        isDailyPve = dungeonType == EM_Fight_Enum.eFight_DailyPve;
        _commonDragObj.SetActive(!isDailyPve);
        _DailyPveDragObj.SetActive(isDailyPve);
        f_GetObject("Sprite_CardPieceGuide").SetActive(dungeonType == EM_Fight_Enum.eFight_DungeonMain);
        f_GetObject("Sprite_EliteFlag").SetActive(dungeonType == EM_Fight_Enum.eFight_DungeonElite);
        f_GetObject("Sprite_OneKeyRewad").SetActive(!isDailyPve);
        f_GetObject("FamousGeneralRemainTimes").SetActive(dungeonType == EM_Fight_Enum.eFight_Legend);
        _famousGeneralRemainTimes.text = (Data_Pool.m_DungeonPool.mDungeonLegendLeftTimes).ToString();

        if (isDailyPve)
        {
            //更新日常副本数据
            UITool.f_OpenOrCloseWaitTip(true);
            _IsFirstUnLockDailyItem = true;
            Data_Pool.m_DailyPveInfoPool.f_DailyPveInfo(RequestQueryCallback);
            f_CloseMoneyUI();
        }
        else
        {
            //更新主线、精英、名将副本章节数据
            _commonScrollView.enabled = true;
            List<BasePoolDT<long>> _ChapterDataList = null;
            int currentChapter = 0;
            currentChapter = Data_Pool.m_DungeonPool.f_GetFightChapterIdx((int)dungeonType);
            List<BasePoolDT<long>> dungeonList = Data_Pool.m_DungeonPool.f_GetAllForData1((int)dungeonType);
            int[] buildingPageCount = _curDungeonType == EM_Fight_Enum.eFight_Legend ? legendBuildingNumOfPage : new int[1] { buildingNumOfOnePage };
            _ChapterDataList = Data_Pool.m_DungeonPool.f_AllDungeonPoolDTs2DungeonPoolDTOfPage(dungeonList, buildingPageCount);
            if (currentChapter == 0) currentChapter = 1;
            commonWrapComponent.f_UpdateList(_ChapterDataList);
            int realIndex = currentChapter / buildingNumOfOnePage;
            if (_curDungeonType == EM_Fight_Enum.eFight_Legend)
            {
                //万恶的名将建筑根据页数可变的，，依次为3，3，4
                realIndex = 0;
                for (int j = currentChapter;j > 0;)
                {
                    for (int i = 0; i < legendBuildingNumOfPage.Length; i++)
                    {
                        j -= legendBuildingNumOfPage[i];
                        if (j <= 0) break;
                        realIndex++;
                    }
                }
            }
            commonWrapComponent.f_ViewGotoRealIdx(realIndex, ShowNumEachPage);

            //拖动限制
            Vector2 scrollViewScale = _commonScrollView.transform.GetComponent<UIPanel>().GetViewSize();
            Vector2 limitDragPos = new Vector2((currentChapter - 1) * (-scrollViewScale.x / buildingNumOfOnePage), 0);
            if (_curDungeonType == EM_Fight_Enum.eFight_Legend)
            {
                //3个屏有10个建筑，，所以*3/10
                int totalCount = 0;
                for (int i = 0; i < legendBuildingNumOfPage.Length; i++)
                {
                    totalCount += legendBuildingNumOfPage[i];
                }
                limitDragPos = new Vector2(currentChapter * (-scrollViewScale.x  * legendBuildingNumOfPage.Length / totalCount), 0);
            }
            _commonScrollView.mCanDragLimitPos = limitDragPos;
            _commonScrollView.enabled = dungeonType == EM_Fight_Enum.eFight_Legend ? currentChapter > 1 : currentChapter > 2;

            f_OpenMoneyUI();
        }

        //设置标题
        if (dungeonType > 0 && (int)dungeonType < titleSpriteNames.Length)
        _spriteTitle.spriteName = titleSpriteNames[(int)dungeonType];

        //切换副本效果
        _spriteDungeonType.spriteName = dungeonTypesSprite[(int)dungeonType - 1];
        _swtichDisplayAlpha.gameObject.SetActive(true);
        _swtichDisplayAlpha.ResetToBeginning();
        _swtichDisplayAlpha.PlayForward();
        EventDelegate.Add(_swtichDisplayAlpha.onFinished,()=>
        {
            _swtichDisplayAlpha.gameObject.SetActive(false);
        });
		if((int)dungeonType != 4)
		{
			_DailyPveItemParent.SetActive(false);
		}
    }

    private void ResetButton()
    {
        _mainLineBtn.Find("SelectIcon").gameObject.SetActive(false);
        _eliteBtn.Find("SelectIcon").gameObject.SetActive(false);
        _famousGeneralBtn.Find("SelectIcon").gameObject.SetActive(false);
    }

    private void SetButton(EM_Fight_Enum dungeonType)
    {
        _mainLineBtn.Find("SelectIcon").gameObject.SetActive(dungeonType == EM_Fight_Enum.eFight_DungeonMain);
        _eliteBtn.Find("SelectIcon").gameObject.SetActive(dungeonType == EM_Fight_Enum.eFight_DungeonElite);
        _famousGeneralBtn.Find("SelectIcon").gameObject.SetActive(dungeonType == EM_Fight_Enum.eFight_Legend);
        _DailyPveBtn.Find("SelectIcon").gameObject.SetActive(dungeonType == EM_Fight_Enum.eFight_DailyPve);
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

    //通用副本Item更新
    private void CommonItemUpdateByInfo(Transform pageItem, BasePoolDT<long> dt)
    {
        DungeonPoolDTOfPage pageData = (DungeonPoolDTOfPage)dt;
        UITexture itemTexture = pageItem.Find("Texture_BG").GetComponent<UITexture>();
        bool isLegend = _curDungeonType == EM_Fight_Enum.eFight_Legend;
		bool isElite = _curDungeonType == EM_Fight_Enum.eFight_DungeonElite;
        // Texture[] _mapTexture = isLegend ? _FamousGeneralMapTexture : _comonMapTexture;
		Texture[] _mapTexture = _comonMapTexture;
		if(isLegend)
		{
			_mapTexture = _FamousGeneralMapTexture;
		}
		if(isElite)
		{
			_mapTexture = _eliteMapTexture;
		}
        Vector3[] _mapBuildingPos = isLegend ? _FamousGeneralMapBuildingPos : _commonMapBuildingPos;
        for (int i = 0; i < pageData.DungeonPoolDTList.Length; i++) {
            Transform item = pageItem.Find("CommonItem" + (i + 1));
            if (null == item)
            {
                continue;
            }
            if (null == pageData.DungeonPoolDTList[i]) {
                item.gameObject.SetActive(false);
                continue;
            }
            item.gameObject.SetActive(true);

            //设置地图建筑位置
            DungeonPoolDT node = (DungeonPoolDT)pageData.DungeonPoolDTList[i];
            int mapPosIndex = node.mIndex % _mapBuildingPos.Length;
            item.localPosition = _mapBuildingPos[mapPosIndex];

            //设置地图
            if (i == 0) {
                pageItem.name = "DungeonChapter" + (node.mIndex + 1);
                int mapIndex = node.mIndex / buildingNumOfOnePage ;
                if (isLegend)
                {
                    //万恶的名将建筑根据页数可变的，，依次为3，3，4
                    for (int j = node.mIndex + 1; j > 0;)
                    {
                        for (int k = 0; k < legendBuildingNumOfPage.Length; k++)
                        {
                            j -= legendBuildingNumOfPage[k];
                            if (j <= 0)
                            {
                                mapIndex = k;
                                break;
                            }
                        }
                    }
                }
                mapIndex = mapIndex % _mapTexture.Length;
                itemTexture.mainTexture = _mapTexture[mapIndex];
            }

            //获取ui元素
            f_RegClickEvent(item.gameObject, CommonItemClickHandleNew, node);
            UILabel tTitle = item.Find("Title").GetComponent<UILabel>();
            UILabel tIndex = item.Find("Index").GetComponent<UILabel>();
            UILabel tStarNum = item.Find("StarIcon/StarNum").GetComponent<UILabel>();
            UISprite tImage = item.Find("Building").GetComponent<UISprite>();
            UISprite tStarIcon = item.Find("StarIcon").GetComponent<UISprite>();
            UISprite spriteBg1 = item.Find("Sprite").GetComponent<UISprite>();
            UISprite spriteBg2 = item.Find("StarIcon/Sprite2").GetComponent<UISprite>();
            UISprite spriteBg3 = item.Find("Sprite3").GetComponent<UISprite>();
            GameObject tChapterPassTip = item.Find("ChapterPassTip").gameObject; 
            UISprite BossPassFlag = item.Find("BossPassFlag").GetComponent<UISprite>(); 
            bool bLock = Data_Pool.m_DungeonPool.f_CheckChapterLockState(node);
            bool bFight = Data_Pool.m_DungeonPool.f_CheckIsFightChapter(node);
            //tTitle.applyGradient = !bLock;
           
            //名将不显示星星
            tStarIcon.gameObject.SetActive(!isLegend);

            //如果是名将副本判断是否有boss
            bool isHadLegendBoss = node.mTollgateMaxNum > GameParamConst.LegendDungeonRestIdx;
            BossPassFlag.gameObject.SetActive(isLegend && isHadLegendBoss);

            //node.mTollgatePassNum == node.mTollgateMaxNum表示已击杀
            BossPassFlag.spriteName = node.mTollgatePassNum == node.mTollgateMaxNum ? "fb_icon_bossb" : "fb_icon_boss";
            BossPassFlag.MakePixelPerfect();

            //判断是否通关
            bool isPass = isLegend ? node.mTollgatePassNum >= GameParamConst.LegendDungeonRestIdx : node.mTollgatePassNum == node.mTollgateMaxNum;
            tChapterPassTip.SetActive(isPass);

            if (bLock)
            {
                // tTitle.effectStyle = UILabel.Effect.None;
                tTitle.text = string.Format("[909494FF]{0}", node.m_ChapterTemplate.szChapterName);
                tIndex.text = (string.Format("{0}", node.mIndex + 1)).Replace(" ","\n");
                tIndex.effectStyle = UILabel.Effect.None;
                tStarNum.text = string.Format("[909494FF]{0}/{1}", node.mStarNum, node.mTollgateMaxNum * GameParamConst.StarNumPreTollgate);
                tStarNum.effectStyle = UILabel.Effect.None;
            }
            else
            {
                // tTitle.effectStyle = UILabel.Effect.Outline;
                tTitle.text = node.m_ChapterTemplate.szChapterName;
                tIndex.text = (string.Format("{0}", node.mIndex + 1)).Replace(" ","\n");
                tIndex.effectStyle = UILabel.Effect.Outline;
                if (node.mStarNum == node.mTollgateMaxNum * GameParamConst.StarNumPreTollgate)
                    tStarNum.text = string.Format("[FFD323FF]{0}/{1}", node.mStarNum, node.mTollgateMaxNum * GameParamConst.StarNumPreTollgate);
                else
                    tStarNum.text = string.Format("[ff0000]{0}[-][FFD323FF]/{1}[-]", node.mStarNum, node.mTollgateMaxNum * GameParamConst.StarNumPreTollgate);
                tStarNum.effectStyle = UILabel.Effect.Outline;
            }
            tImage.spriteName = node.m_ChapterTemplate.szChapterImage;
            UITool.f_SetSpriteGray(tImage, bLock);
            // tImage.MakePixelPerfect();
            UITool.f_SetSpriteGray(BossPassFlag, bLock);
            UITool.f_SetSpriteGray(tStarIcon, bLock);
            UITool.f_SetSpriteGray(spriteBg1, bLock);
            UITool.f_SetSpriteGray(spriteBg2, bLock);
            UITool.f_SetSpriteGray(spriteBg3, bLock);            
            bool isHasReddot = Data_Pool.m_DungeonPool.f_CheckHasBoxCanGet(node);
            item.Find("Reddot").gameObject.SetActive(isHasReddot);
        }

        //隐藏多余的建筑
        for (int i = pageData.DungeonPoolDTList.Length; i < pageItem.childCount - 1; i++)
        {
            Transform item = pageItem.Find("CommonItem" + (i + 1));
            if (null == item)
            {
                continue;
            }
            item.gameObject.SetActive(false);
        }
    }


    //通用副本Item点击
    private void CommonItemClickHandleNew(GameObject go, object obj1, object obj2)
    {
        DungeonPoolDT dungeonDT = obj1 as DungeonPoolDT;
        if (Data_Pool.m_DungeonPool.f_CheckChapterLockState(dungeonDT,true))
        {
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_DungeonPool.f_ExecuteAfterInitDungeon(dungeonDT, f_Callback_DungeonTollgate);
    }

    private static void f_Callback_DungeonTollgate(object value)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if (value is DungeonPoolDT)
        {
            DungeonPoolDT poolDt = (DungeonPoolDT)value;
            DungeonTollgatePageParam param = new DungeonTollgatePageParam();
            param.mDungeonPoolDT = poolDt;
            ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.DungeonChapterPageNew));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonTollgatePageNew, UIMessageDef.UI_OPEN, param);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1054) + value);
        }
    }
   
    #region 日常副本相关
    private Transform  _DailyPveBtn;
    private GameObject _DailyPveDragObj;
    private GameObject _DailyPveItemParent;
	private GameObject _DailyPve;
    private UIScrollView _DailyPveScrollView;
    private GameObject _DailyPveItem;
    private UISprite   _DailyChallengeIcon;
    private UISprite   _DailyChallengeBtn;
    private UILabel    _DailyCost;
    private UILabel    _DailyChallengeTips;
    private bool       _IsFirstUnLockDailyItem = true;
    private GameObject mLastSelectedItem;
    private List<BasePoolDT<long>> _DailyPveList;
    private UIWrapComponent _DailyPveWrapComponent;
    private SocketCallbackDT RequestQueryCallback = new SocketCallbackDT();//查询回调
    private UIWrapComponent mDailyPveWrapComponent
    {
        get
        {
            if (_DailyPveWrapComponent == null)
            {
                _DailyPveList = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_DailyPveInfoPool.f_GetAll());
                //排序，已开放的放前面
                for (int i = 0; i < _DailyPveList.Count - 1; i++)
                {
                    for (int j = i + 1; j < _DailyPveList.Count; j++)
                    {
                        string temp = "";
                        DailyPveInfoPoolDT oriData = _DailyPveList[i] as DailyPveInfoPoolDT;
                        bool isLockOri = !Data_Pool.m_DailyPveInfoPool.f_CheckTime(oriData.m_DailyPveInfoDT, GameSocket.GetInstance().f_GetServerTime(), out temp);
                        DailyPveInfoPoolDT nextData = _DailyPveList[j] as DailyPveInfoPoolDT;
                        bool isLockNext = !Data_Pool.m_DailyPveInfoPool.f_CheckTime(nextData.m_DailyPveInfoDT, GameSocket.GetInstance().f_GetServerTime(), out temp);
                        if (isLockOri && !isLockNext)
                        {
                            DailyPveInfoPoolDT tempData = oriData;
                            _DailyPveList[i] = nextData;
                            _DailyPveList[j] = tempData;
                            break;
                        }
                    }
                }
                _DailyPveWrapComponent = new UIWrapComponent(195, 1, 740, 6, _DailyPveItemParent, _DailyPveItem, _DailyPveList, f_DailyPveItemUpdateInfo, null);
            }
            return _DailyPveWrapComponent;
        }
    }

    private void f_DailyPveItemUpdateInfo(Transform tf, BasePoolDT<long> dt)
    {
        DailyPveInfoPoolDT poolDT = dt as DailyPveInfoPoolDT;
        string openStr;
        bool isLock = !Data_Pool.m_DailyPveInfoPool.f_CheckTime(poolDT.m_DailyPveInfoDT, GameSocket.GetInstance().f_GetServerTime(), out openStr);
        int times = poolDT.TodayPassTimes;
        if (times >= 1)
        {
            isLock = false;
            openStr = CommonTools.f_GetTransLanguage(1055);
        }

        //设置类型图标
        UISprite sprite = tf.GetComponent<UISprite>();
        sprite.spriteName = poolDT.m_DailyPveInfoDT.szIcon;
        UITool.f_SetSpriteGray(sprite, isLock);
        sprite.MakePixelPerfect();

        //设置红点
        tf.Find("Reddot").gameObject.SetActive(false);
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (!isLock && times == 0)
        {
            List<NBaseSCDT> listDailyPve = glo_Main.GetInstance().m_SC_Pool.m_DailyPveGateSC.f_GetAll();
            for (int a = 0; a < listDailyPve.Count; a++)
            {
                DailyPveGateDT DailyPveGateDT = listDailyPve[a] as DailyPveGateDT;
                if (DailyPveGateDT.iType == poolDT.m_DailyPveInfoDT.iId && playerLevel >= DailyPveGateDT.iLevelLimit1)
                {
                    tf.Find("Reddot").gameObject.SetActive(true);
                    break;
                }
            }
        }

        //默认选中第一个
        if (!isLock && _IsFirstUnLockDailyItem) {
            _IsFirstUnLockDailyItem = false;
            f_OnDailyPveItemClick(tf.gameObject, dt, isLock);
        }

        //设置提示
        UILabel tips = tf.Find("Label_DailyOpenTips").GetComponent<UILabel>();
        tips.gameObject.SetActive(isLock);
		tf.Find("Fx").transform.gameObject.SetActive(!isLock);
        if (isLock) {            
            tips.text = openStr;
            return;
        }
        f_RegClickEvent(tf.gameObject, f_OnDailyPveItemClick, dt, isLock);
    }

    private void OnQuerySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);        
        mDailyPveWrapComponent.f_UpdateView();
        mDailyPveWrapComponent.f_ResetView();
        _DailyPveScrollView.enabled = _DailyPveList.Count > 6;
    }
    private void OnQueryFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1056));
    }   

    /// <summary>
    /// 点击日常符文item
    /// </summary>
    private void f_OnDailyPveItemClick(GameObject go, object obj1, object obj2)
    {
        if (null != mLastSelectedItem)
        {
            mLastSelectedItem.SetActive(false);
            mLastSelectedItem = null;
        }
        GameObject selectedItem = go.transform.Find("Sprite_Selected").gameObject;
        selectedItem.SetActive(true);
        mLastSelectedItem = selectedItem;
        DailyPveInfoPoolDT poolDT = obj1 as DailyPveInfoPoolDT;
        bool isLock = (bool)obj2;

        //注册挑战监听
        // f_RegClickEvent(_DailyChallengeBtn.gameObject, OnDailyChallenge, obj1, false);
        // f_RegClickEvent(_DailyChallengeIcon.gameObject, OnDailyChallenge, obj1, false);

        //设置挑战提示
        int times = poolDT.TodayPassTimes;
        string tips = CommonTools.f_GetTransLanguage(1055);       
        Color color = new Color(220f/255, 68f/255, 42f/255);
        UILabel.Effect effectStyle = UILabel.Effect.None;
        if (times < 1)
        {
            //今日未挑战
            tips = CommonTools.f_GetTransLanguage(2176);
            color = new Color(77f/255, 253f/255, 141f/255);
            effectStyle = UILabel.Effect.Outline;
        }        
        _DailyChallengeTips.color = color;
        _DailyChallengeTips.effectStyle = effectStyle;
        _DailyChallengeTips.text = tips;

        //设置图标
        _DailyChallengeIcon.spriteName = poolDT.m_DailyPveInfoDT.szIcon.Replace("_icon", "");
        UITool.f_SetSpriteGray(_DailyChallengeIcon, isLock);
        _DailyChallengeIcon.MakePixelPerfect();
        UITool.f_SetSpriteGray(_DailyChallengeBtn, isLock || times >= 1); 
        _DailyCost.text = poolDT.m_DailyPveInfoDT.iTiLi.ToString();        
		
		//My Code
		if(count > 0)
		{
			OnDailyChallenge(_DailyChallengeBtn.gameObject, obj1, false);
		}
		else
		{
			count++;
		}
    }

    /// <summary>
    /// 日常副本挑战
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void OnDailyChallenge(GameObject go, object obj1, object obj2)
    {
        DailyPveInfoPoolDT poolDT = obj1 as DailyPveInfoPoolDT;
        bool isLock = (bool)obj2;
        if (!isLock)
        {
            if (poolDT.TodayPassTimes >= 1)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1057));
                return;
            }
            else if (!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Energy, poolDT.m_DailyPveInfoDT.iTiLi, true, true, this))
            {
                return;
            }
            ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectDifficultyPage, UIMessageDef.UI_OPEN, obj1);
        }
        else
        {
            string tOpenStr = string.Empty;
            Data_Pool.m_DailyPveInfoPool.f_CheckTime(poolDT.m_DailyPveInfoDT, GameSocket.GetInstance().f_GetServerTime(), out tOpenStr);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, tOpenStr);
        }
    }
    #endregion
}
