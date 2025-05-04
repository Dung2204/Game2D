using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
using Spine;
using Spine.Unity;
using System.Linq;
/// <summary>
/// 活动首充界面
/// </summary>
public class AFKInfoPage : UIFramwork
{
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调
    GameObject Magic;
    SkeletonAnimation ani;
    int Time_OpenUI;
    private FirstRechargePoolDT NowSelect;

    private int NowSelectId;
    private int nowSelectedDayId; //TsuCode - FirstRechargeNew - NapDau
    private UILabel m_EventCDTimeShow;
    private UILabel m_AFKTimeShow;
    private UILabel m_AFKTimeShow2;
    private GameObject m_BtnGetAward;
    private GameObject m_BtnGetInfo;
    private float updateTime = 0f;
    private int timeGap = 0;
    private int timeHour;
    private int timeMin;
    private int timeSecond;
    private int eventTimeGap;
    private int eventTimeMin;
    private int eventTimeSecond;
    //My Code
    GameParamDT RoleList;
    //
    private static Dictionary<EM_CloseArrayPos, Vector3> dicCardPos = new Dictionary<EM_CloseArrayPos, Vector3>(); 
    private static Dictionary<EM_CloseArrayPos, Vector3> dicEnemyPos = new Dictionary<EM_CloseArrayPos, Vector3>();
    private static Dictionary<EM_CloseArrayPos, Transform> TeamAttackPos = new Dictionary<EM_CloseArrayPos, Transform>();
    private static Dictionary<EM_CloseArrayPos, Transform> EnemyAttackPos = new Dictionary<EM_CloseArrayPos, Transform>();
    private int addSortingLayer = -10;
    private bool init;
    private int _hpA = 9999;
    private int _hpB = 9;

    private List<ModelAfkController> TeammodelAfkControllers = new List<ModelAfkController>();
    private List<ModelAfkController> EnemymodelAfkControllers = new List<ModelAfkController>();

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        f_GetObject("UI").SetActive(false);

        init = false;
        TeammodelAfkControllers.Clear();
        EnemymodelAfkControllers.Clear();
        TimeBatle = 0;

        UpdateUI();
        //UITool.f_OpenOrCloseWaitTip(true);



        //SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
        //QueryCallback.m_ccCallbackFail = OnAFKTimeFailCallback;
        //QueryCallback.m_ccCallbackSuc = OnAFKTimeSucCallback;
        //Data_Pool.m_AFKPool.f_QueryAFKTimeInfo(QueryCallback);
        // f_LoadTexture();


        init = true;
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {

    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        init = false;
        TimeBatle = 0;
        TeammodelAfkControllers.Clear();
        EnemymodelAfkControllers.Clear();

    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBg", OnCloseThis);
        f_RegClickEvent("btnclose", OnCloseThis);
        m_AFKTimeShow2 = f_GetObject("AFKTimeShow2").GetComponent<UILabel>();
        m_EventCDTimeShow = f_GetObject("EventCDTimeShow").GetComponent<UILabel>();

        //m_BtnGetAward = f_GetObject("GetInfo");
        m_BtnGetInfo = f_GetObject("GetAward");
        f_RegClickEvent("GetInfo",BtnGetInfoClick);
        f_RegClickEvent("GetAward", BtnGetAwardClick);
		f_RegClickEvent("Btn_Help", f_OnHelpBtnClick);

        dicCardPos.Clear();
        dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosOne, f_GetObject("BtnOneHand").transform.localPosition);
        dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosTwo, f_GetObject("BtnTwoHand").transform.localPosition);
        dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosThree, f_GetObject("BtnThreeHand").transform.localPosition);
        dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosFour, f_GetObject("BtnFourHand").transform.localPosition);
        dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosFive, f_GetObject("BtnFiveHand").transform.localPosition);
        dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosSix, f_GetObject("BtnSixHand").transform.localPosition);
        dicEnemyPos.Clear();
        dicEnemyPos.Add(EM_CloseArrayPos.eCloseArray_PosOne, f_GetObject("EBtnOneHand").transform.localPosition);
        dicEnemyPos.Add(EM_CloseArrayPos.eCloseArray_PosTwo, f_GetObject("EBtnTwoHand").transform.localPosition);
        dicEnemyPos.Add(EM_CloseArrayPos.eCloseArray_PosThree, f_GetObject("EBtnThreeHand").transform.localPosition);
        dicEnemyPos.Add(EM_CloseArrayPos.eCloseArray_PosFour, f_GetObject("EBtnFourHand").transform.localPosition);
        dicEnemyPos.Add(EM_CloseArrayPos.eCloseArray_PosFive, f_GetObject("EBtnFiveHand").transform.localPosition);
        dicEnemyPos.Add(EM_CloseArrayPos.eCloseArray_PosSix, f_GetObject("EBtnSixHand").transform.localPosition);
        TeamAttackPos.Clear();
        TeamAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosOne, f_GetObject("TeamOne").transform);
        TeamAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosTwo, f_GetObject("TeamTwo").transform);
        TeamAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosThree, f_GetObject("TeamThree").transform);
        TeamAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosFour, f_GetObject("TeamFour").transform);
        TeamAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosFive, f_GetObject("TeamFive").transform);
        TeamAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosSix, f_GetObject("TeamSix").transform);
        EnemyAttackPos.Clear();
        EnemyAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosOne, f_GetObject("EnemyOne").transform);
        EnemyAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosTwo, f_GetObject("EnemyTwo").transform);
        EnemyAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosThree, f_GetObject("EnemyThree").transform);
        EnemyAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosFour, f_GetObject("EnemyFour").transform);
        EnemyAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosFive, f_GetObject("EnemyFive").transform);
        EnemyAttackPos.Add(EM_CloseArrayPos.eCloseArray_PosSix, f_GetObject("EnemySix").transform);

    }


    private void QueryFill(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
    }

    private const string BgFile = "UI/TextureRemove/Tex_BattleBg";
    private void UpdateUI()
    {
        f_GetObject("UI").SetActive(true);
        string BattleBg = "battlebg" + ccMath.f_GetRand(1, 9);
        GameObject BattleBG = f_GetObject("BattleBG");
        UITexture tSpriteRenderer = BattleBG.GetComponent<UITexture>();
        Texture2D Texture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(BattleBg);
        if(Texture2D == null)
            Texture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(BgFile);
        tSpriteRenderer.mainTexture = Texture2D;

        InitTeamModel();

        List<BasePoolDT<long>> _mainLineList = Data_Pool.m_DungeonPool.f_GetAllForData1((int)EM_Fight_Enum.eFight_DungeonMain);
        int ChapterPassIdx = Data_Pool.m_DungeonPool.f_GetChapterPassIdx(EM_Fight_Enum.eFight_DungeonMain);

        DungeonPoolDT dt = (DungeonPoolDT)_mainLineList[ChapterPassIdx];
        int mTollgatePassNum = dt.mTollgatePassNum;
        if (mTollgatePassNum <= 0)
        {
            ChapterPassIdx--;
            if(ChapterPassIdx <= 0)
            {
                ChapterPassIdx = 0;
            }
            dt = (DungeonPoolDT)_mainLineList[ChapterPassIdx];
            mTollgatePassNum = dt.mTollgatePassNum;
        }
        mTollgatePassNum--;
        if (mTollgatePassNum <= 0)
            mTollgatePassNum = 0;

        DungeonTollgatePoolDT dungeonTollgatePoolDT = (DungeonTollgatePoolDT)dt.mTollgateList[mTollgatePassNum];

        DungeonTollgateDT tollgateDT = dungeonTollgatePoolDT.mTollgateTemplate;



        InitETeamModel(ccMath.f_String2ArrayInt(tollgateDT.szMonsterId, ";"));

        int Lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        AFKConfigDT AFKConfigDT = (AFKConfigDT)glo_Main.GetInstance().m_SC_Pool.m_AFKConfigSC.f_GetSC(Lv);
        if (AFKConfigDT == null) return;
        f_GetObject("textmoney").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(2277), AFKConfigDT.iMoney*60);
        f_GetObject("textexp").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(2277), AFKConfigDT.iExp*60);


    }
    private int localScale = 60;
    private void InitTeamModel()
    {
        Dictionary<EM_CloseArrayPos, EM_FormationPos> temp = Data_Pool.m_ClosethArrayData.m_dicClothArrayToPos;
        //List<EM_FormationPos> temp = Data_Pool.m_ClosethArrayData.m_aClothArrayTeamPoolID;
        string cardName = null;
        TeamPoolDT teamPoolDt = GetObj(EM_FormationPos.eFormationPos_Main);
        cardName = teamPoolDt.m_CardPoolDT.m_CardDT.szName + ":" + teamPoolDt.m_eFormationPos.ToString();
        UpdateModel((EM_CloseArrayPos)1, EM_FormationPos.eFormationPos_Main, cardName);
        //for (int i = 0; i < temp.Count; i++)
        //{
        //    string cardName = null;
        //    EM_FormationPos value = temp.ElementAt(i).Value;//[i];
        //    if (value != EM_FormationPos.eFormationPos_INVALID)
        //    {
        //        TeamPoolDT teamPoolDt = GetObj(value);
        //        cardName = teamPoolDt.m_CardPoolDT.m_CardDT.szName + ":" + teamPoolDt.m_eFormationPos.ToString();
        //    }
        //    UpdateModel((EM_CloseArrayPos)temp.ElementAt(i).Key, value, cardName);
        //}
    }
    private void InitETeamModel(int[] CardId)
    {
        for (int i = 0; i < CardId.Length; i++)
        {
            bool value = CardId[i] == 0;
            UpdateEModel((EM_CloseArrayPos)i, value, CardId[i]);
        }
    }

    private TeamPoolDT GetObj(EM_FormationPos pos)
    {
        if (pos != EM_FormationPos.eFormationPos_INVALID)
        {
            List<BasePoolDT<long>> temp = Data_Pool.m_TeamPool.f_GetAll();
            for (int i = 0; i < temp.Count; i++)
            {
                TeamPoolDT data = temp[i] as TeamPoolDT;
                if ((int)data.m_eFormationPos == (int)pos)
                {
                    return data;
                }
            }
        }
        return null;
    }
    private void UpdateModel(EM_CloseArrayPos cpos, EM_FormationPos fPos, string cardName)
    {

        bool isInvalid = (fPos == EM_FormationPos.eFormationPos_INVALID ? true : false);
        GameObject model = null;
        CardPoolDT cardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(fPos);

        switch ((EM_CloseArrayPos)fPos)
        {
            case EM_CloseArrayPos.eCloseArray_PosOne:
                model = f_GetObject("BtnOneHand");
                model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosOne];
                model.SetActive(!isInvalid);
                break;
            case EM_CloseArrayPos.eCloseArray_PosTwo:
                model = f_GetObject("BtnTwoHand");
                model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosTwo];
                model.SetActive(!isInvalid);
                break;
            case EM_CloseArrayPos.eCloseArray_PosThree:
                model = f_GetObject("BtnThreeHand");
                model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosThree];
                model.SetActive(!isInvalid);
                break;
            case EM_CloseArrayPos.eCloseArray_PosFour:
                model = f_GetObject("BtnFourHand");
                model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosFour];
                model.SetActive(!isInvalid);
                break;
            case EM_CloseArrayPos.eCloseArray_PosFive:
                model = f_GetObject("BtnFiveHand");
                model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosFive];
                model.SetActive(!isInvalid);
                break;
            case EM_CloseArrayPos.eCloseArray_PosSix:
                model = f_GetObject("BtnSixHand");
                model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosSix];
                model.SetActive(!isInvalid);
                break;
        }
        if (model.transform.Find("ModelObj") != null)
        {
            UITool.f_DestoryStatelObject(model.transform.Find("ModelObj").gameObject);
        }
        if (!isInvalid && model != null)
        {
            int sortingOrder = 1;
            if ((EM_CloseArrayPos)fPos == EM_CloseArrayPos.eCloseArray_PosOne)
                sortingOrder = 2;
            else if (cpos == EM_CloseArrayPos.eCloseArray_PosFour)
                sortingOrder = 1;
            if ((EM_CloseArrayPos)fPos == EM_CloseArrayPos.eCloseArray_PosFive)
                sortingOrder = 2;
            else if (cpos == EM_CloseArrayPos.eCloseArray_PosTwo)
                sortingOrder = 3;
            if ((EM_CloseArrayPos)fPos == EM_CloseArrayPos.eCloseArray_PosSix)
                sortingOrder = 4;
            else if ((EM_CloseArrayPos)fPos == EM_CloseArrayPos.eCloseArray_PosThree)
                sortingOrder = 5;
            sortingOrder += addSortingLayer;
            GameObject Role = UITool.f_GetStatelObject(cardPoolDT, model.transform, new Vector3(0, 180, 0), new Vector3(0, -80, 0), sortingOrder, "ModelObj", localScale, false);
            Role.SetActive(false);
            Role.SetActive(true);
            if(Role.GetComponent< ModelAfkController>() == null)
            {
                Role.AddComponent<ModelAfkController>().InitControl(_hpA, cpos, this, 0);
            }
            else
            {
                Role.GetComponent<ModelAfkController>().SetEM_CloseArrayPos(cpos);
            }
           
            TeammodelAfkControllers.Add(Role.GetComponent<ModelAfkController>());
          }
    }
    private void UpdateEModel(EM_CloseArrayPos cpos, bool isInvalid, int cardId)
    {

        GameObject model = null;

        switch (cpos)
        {
            case EM_CloseArrayPos.eCloseArray_PosOne:
                model = f_GetObject("EBtnOneHand");
                model.transform.localPosition = dicEnemyPos[EM_CloseArrayPos.eCloseArray_PosOne];
                model.SetActive(!isInvalid);
                break;
            case EM_CloseArrayPos.eCloseArray_PosTwo:
                model = f_GetObject("EBtnTwoHand");
                model.transform.localPosition = dicEnemyPos[EM_CloseArrayPos.eCloseArray_PosTwo];
                model.SetActive(!isInvalid);
                break;
            case EM_CloseArrayPos.eCloseArray_PosThree:
                model = f_GetObject("EBtnThreeHand");
                model.transform.localPosition = dicEnemyPos[EM_CloseArrayPos.eCloseArray_PosThree];
                model.SetActive(!isInvalid);
                break;
            case EM_CloseArrayPos.eCloseArray_PosFour:
                model = f_GetObject("EBtnFourHand");
                model.transform.localPosition = dicEnemyPos[EM_CloseArrayPos.eCloseArray_PosFour];
                model.SetActive(!isInvalid);
                break;
            case EM_CloseArrayPos.eCloseArray_PosFive:
                model = f_GetObject("EBtnFiveHand");
                model.transform.localPosition = dicEnemyPos[EM_CloseArrayPos.eCloseArray_PosFive];
                model.SetActive(!isInvalid);
                break;
            case EM_CloseArrayPos.eCloseArray_PosSix:
                model = f_GetObject("EBtnSixHand");
                model.transform.localPosition = dicEnemyPos[EM_CloseArrayPos.eCloseArray_PosSix];
                model.SetActive(!isInvalid);
                break;
        }
        if (model.transform.Find("ModelObj") != null)
        {
            UITool.f_DestoryStatelObject(model.transform.Find("ModelObj").gameObject);
        }
        if (!isInvalid && model != null)
        {
            int sortingOrder = 1;
            if (cpos == EM_CloseArrayPos.eCloseArray_PosOne)
                sortingOrder = 2;
            else if (cpos == EM_CloseArrayPos.eCloseArray_PosFour)
                sortingOrder = 1;
            if (cpos == EM_CloseArrayPos.eCloseArray_PosFive)
                sortingOrder = 2;
            else if (cpos == EM_CloseArrayPos.eCloseArray_PosTwo)
                sortingOrder = 3;
            if (cpos == EM_CloseArrayPos.eCloseArray_PosSix)
                sortingOrder = 4;
            else if (cpos == EM_CloseArrayPos.eCloseArray_PosThree)
                sortingOrder = 5;
            sortingOrder += addSortingLayer;
            GameObject Role = null;
            MonsterDT monsterDT = (MonsterDT)glo_Main.GetInstance().m_SC_Pool.m_MonsterSC.f_GetSC(cardId);
            if (monsterDT == null) return;
            UITool.f_CreateRoleByModeId(monsterDT.iStatelId1, ref Role, model.transform, -5, false);
            Role.SetActive(false);
            Role.SetActive(true);
            Role.name = "ModelObj";
            Role.transform.localScale = new Vector3(localScale, localScale, localScale);
            if (Role.GetComponent<ModelAfkController>() == null)
            {
                Role.AddComponent<ModelAfkController>().InitControl(_hpB, cpos, this, 1);
            }
            EnemymodelAfkControllers.Add(Role.GetComponent<ModelAfkController>());
        }
    }

    private void OnCloseThis(GameObject go, object obj1, object obj2)
    {
       
        f_GetObject("UI").SetActive(false);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_OpenUI);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AFKInfoPage, UIMessageDef.UI_CLOSE);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.FirstRechargePage, UIMessageDef.UI_CLOSE);
    }
    //TsuCode- FirstRechargeNew - NapDau

  private void BtnGetInfoClick(GameObject go, object obj1, object obj2)
    {
        UITool.Ui_Trip("Đang lấy thông tin");
    }
    private void BtnGetAwardClick(GameObject go, object obj1, object obj2)
    {
        //UITool.Ui_Trip("Get Award");
        if((int)(GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_AFKPool.afkTime) < 60)
        {
			UITool.Ui_Trip("Chưa đến thời gian nhận thưởng");
            return;
        }
        SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
        QueryCallback.m_ccCallbackFail = OnAFKTimeFailCallback;
        QueryCallback.m_ccCallbackSuc = OnAFKTimeSucCallback;
        Data_Pool.m_AFKPool.f_AFKGetAward(QueryCallback);

    }
    private string strTexRewardRoot = "UI/TextureRemove/MainMenu/";
    private string _checkstrTexRewardRoot = "";
    private void f_UpdateBySecond()
    {
        int maxTime = 28800;

        //Check thời gian bonus theo cấp vip lấy từ config VipPrivilege
        int bonusTime = 0;
        bonusTime = Data_Pool.m_RechargePool.f_GetVipPriValue(EM_VipPrivilege.eVip_AFKBonus, UITool.f_GetNowVipLv());

        //Tính toán thời gian online realTime bằng công thức: afkTime = TimeServer - Điểm save loginTime gần nhất chưa nhận quà
        timeGap = (int)(GameSocket.GetInstance().f_GetServerTime()- Data_Pool.m_AFKPool.afkTime);


        GameParamDT afkParam = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.AfkInfo);
        if (afkParam != null) maxTime = afkParam.iParam1 *3600;

        //Nếu là lần đầu, time =0
        if (Data_Pool.m_AFKPool.isFirst == 1)
        {
            timeGap = 0;
            Data_Pool.m_AFKPool.isFirst = 0;
        }
        else
        //Thời gian tối đa mỗi tài khoản là param 1 id 107 ở gameParam, nếu k có param này thì thời gian tối đa mặc định là 8 giờ (28800 giây) và thêm số phút theo cấp vip
               if (timeGap >= maxTime + (bonusTime * 60)) timeGap = maxTime + (bonusTime * 60);

        //Update time
        if (timeGap < 0)
        {
            m_EventCDTimeShow.text = string.Empty;
            m_AFKTimeShow2.text = string.Empty;
        }
        else
        {
            eventTimeGap = timeGap % (60 *1);
            eventTimeMin = eventTimeGap / 60;
            eventTimeSecond = eventTimeGap % 60;
            timeHour = timeGap / 60 / 60;
            timeMin = timeGap / 60 % 60;
            timeSecond = timeGap % 60;
            m_EventCDTimeShow.text = string.Format(CommonTools.f_GetTransLanguage(889), eventTimeMin, eventTimeSecond);
            m_AFKTimeShow2.text = string.Format("{0:d2}:{1:d2}:{2:d2}[-]", timeHour, timeMin, timeSecond);
        }
        string strTexPath = strTexRewardRoot;
        if(timeGap>=8*3600 && timeGap<= 16 * 3600)
        {
            strTexPath += "Btn_AFKAward1";
        }
        else if (timeGap > 16 * 3600) 
        {
            strTexPath += "Btn_AFKAward2";
        }
        else
        {
            strTexPath += "Btn_AFKAward";
        }
        if(_checkstrTexRewardRoot != strTexPath)
        {
            m_BtnGetInfo.GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexPath);
            m_BtnGetInfo.GetComponent<UITexture>().MakePixelPerfect();
            _checkstrTexRewardRoot = strTexPath;
        }
        //m_BtnPacifyRiot.SetActive(userId != Data_Pool.m_UserData.m_iUserId && data.m_State == EM_PatrolState.Patroling && data.m_bIsRiot);
    }

    protected override void f_Update()
    {
        base.f_Update();
            updateTime += Time.deltaTime;
            if (updateTime > 0.5f)
            {
                updateTime = 0f;
                f_UpdateBySecond();
                f_UpdateAfkBale();
            }
    }
    private int TimeBatle = 0;
    private int TimeMove = 2;
    private int TimeFight = 5;

    private void f_UpdateAfkBale()
    {
        if (!init) return;
        TimeBatle++;
        CheckTargetNull();
        if (TimeBatle == TimeMove)//todo dàn trận
        {
            foreach (ModelAfkController item in TeammodelAfkControllers)
            {
                item.GoBatle(TeamAttackPos[item._EM_CloseArrayPos]);
            }
            foreach (ModelAfkController item in EnemymodelAfkControllers)
            {
                item.GoBatle(EnemyAttackPos[item._EM_CloseArrayPos]);
            }
        }
        if(TimeBatle == TimeFight)// tìm target
        {
            Fight();
        }
        CheckEndBattle(1);
    }
    private void CheckTargetNull()
    {
        foreach (ModelAfkController item in TeammodelAfkControllers)
        {
            if (item.Target == null) continue;
            if(item.Target._Hp <= 0)
            {
                ChangeTarget(item);
            }
        }
        foreach (ModelAfkController item in EnemymodelAfkControllers)
        {
            if (item.Target == null) continue;
            if (item.Target._Hp <= 0)
            {
                ChangeTarget(item);
            }
        }
    }
    public void Fight()
    {
        foreach (ModelAfkController item in TeammodelAfkControllers)
        {

            ModelAfkController enemy = CheckNearEnemy(item, EnemymodelAfkControllers);
            if (enemy != null)
            {
                item.ChangeTarget(enemy);
            }
        }
        foreach (ModelAfkController item in EnemymodelAfkControllers)
        {
            ModelAfkController enemy = CheckNearEnemy(item, TeammodelAfkControllers);
            if (enemy != null)
            {
                item.ChangeTarget(enemy);
            }
        }
    }
	
	private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 22);
    }

    public void ChangeTarget(ModelAfkController my)
    {
        ModelAfkController enemy = null;
        switch ((int)my._team)
        {
            case 0:
                enemy = CheckNearEnemy(my, EnemymodelAfkControllers);
                break;

            case 1:
                enemy = CheckNearEnemy(my, TeammodelAfkControllers);
                break;
        }
        if(enemy == null)
        {
            //todo đã hết địch reset batle

        }
        else
        {
            my.ChangeTarget(enemy);
        }
    }

    public void CheckEndBattle(int team)
    {
        bool check = true;
        if(team == 1)
        {
            for (int i = 0; i < EnemymodelAfkControllers.Count; i++)
            {
                if(EnemymodelAfkControllers[i]._Hp > 0)
                {
                    check = false;
                    break;
                }
            }
        }

        if (check)
        {
            // khởi tạo batle mới
            NextBattle();
        }
    }

    int enemyRevive = 0;
    private void NextBattle()
    {
        init = false;
        foreach (ModelAfkController item in TeammodelAfkControllers)
        {
            item._Hp = _hpA;
            item.callback = false;
        }
        enemyRevive = 0;
        foreach (ModelAfkController item in EnemymodelAfkControllers)
        {
            item.ModelRevive(_hpB);
            item.callback = false;
        }



    }

    public void ReviveEnemy()
    {
        enemyRevive++;
        if(enemyRevive >= EnemymodelAfkControllers.Count)
        {
            TimeBatle = TimeMove + 1;
            init = true;
        }
    }

    private ModelAfkController CheckNearEnemy(ModelAfkController my, List<ModelAfkController> enemys)
    {
        float rangedefaut = 9999;
        ModelAfkController model = null;
        for (int i = 0; i < enemys.Count; i++)
        {
            float range = Vector3.Distance(my.transform.position, enemys[i].transform.position);
            if (enemys[i]._Hp > 0 && range < rangedefaut)
            {
                rangedefaut = range;
                model = enemys[i];
            }
        }
        return model;
    }
    #region TsuCode - AFK module
    private void OnAFKTimeSucCallback(object obj)
    {
        MessageBox.ASSERT("TSUlog AFK check " + Data_Pool.m_AFKPool.afkTime);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.AFKInfoPage, UIMessageDef.UI_CLOSE);
        if ((int)obj == (int)eMsgOperateResult.eOR_Default)
        {

        }
    }
    private void OnAFKTimeFailCallback(object obj)
    {
        MessageBox.ASSERT("TSUlog AFK CS_AFKInfoTIme FAIl");

    }

   
    #endregion TsuCode - AFK module



}
//------------------------------------------

//ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
//             new object[] { CommonTools.f_GetListAwardPoolDT(NowSelect.m_FirstRechargeDT.szAward) });
//ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
//               new object[] { awardList });

