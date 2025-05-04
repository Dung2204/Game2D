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
public class BattlePassPage : UIFramwork
{
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调
    private SocketCallbackDT RequestGetTaskAwardCallback = new SocketCallbackDT();//领取回调
    private SocketCallbackDT RequestGetAwardCallback = new SocketCallbackDT();//领取回调
    private SocketCallbackDT RequestGetRabkListCallback = new SocketCallbackDT();//领取回调
    private bool initUI;
    EventTimeDT eventTimeDT;
    List<NBaseSCDT> BattlePassDTs;
    EventTimePoolDT tPoolDataDT;
    private UILabel ActivityEndTime;   //活
    private bool mFirstPage = true;
    private UIScrollView mScrollView;
    private GameObject mGrid;
    private GameObject mCommonTab;

    private GameObject BuyScorePanel;

    string path1 = "UI/TextureRemove/LevelGift/Char1";
    string path2 = "UI/TextureRemove/LevelGift/Char2";
    string path3 = "UI/TextureRemove/LevelGift/Char3";

    private int Idinfo = 99999;
    private GameParamDT param;

    private UIWrapComponent _rankWrapComponet;
    public UIWrapComponent mRankWrapComponet
    {
        get
        {
            if (_rankWrapComponet == null)
            {
                List<BasePoolDT<long>> _rankList = Data_Pool.m_EventTimePool.f_GetRankList(eventTimeDT.iId);
                _rankWrapComponet = new UIWrapComponent(140, 1, 140, 10, mGrid, mCommonTab, _rankList, f_AwardItemUpdateByInfo, f_OnAwardItemClick);
            }
            return _rankWrapComponet;
        }
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (initUI)
        {
            EventTimeDT newEvent = (EventTimeDT)e;

            if (newEvent.iId != eventTimeDT.iId)
            {
                initUI = false;
            }
        }

        eventTimeDT = (EventTimeDT)e;
        Debug.Log(eventTimeDT.iId);
        UpdateBtnTab(0);
        param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(116);
        //param.iParam1 mo cao vip tinh = kim phieu
        //param.iParam2 ti le 1 diem = param2 * knb
    }

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }
    protected override void InitGUI()
    {
        base.InitGUI();
        ActivityEndTime = f_GetObject("ActivityEndTime").GetComponent<UILabel>();
        BuyScorePanel = f_GetObject("BuyScorePanel");
        BuyScorePanel.SetActive(false);
        mGrid = f_GetObject("Grid");
        mCommonTab = f_GetObject("CommonTab");

        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
        mScrollView.onDragFinished = f_OnMomnetEnds;
    }

    protected override void f_Create()
    {
        _InitReference();
        base.f_Create();
    }
    private void _InitReference()
    {
        AddGOReference("Panel/Anchor-Center/BlackBg");
        AddGOReference("Panel/Anchor-Center/UI/BtnClose");

        AddGOReference("Panel/Anchor-Center/UI/TaskPanel");
        AddGOReference("Panel/Anchor-Center/UI/AwardPanel");
        AddGOReference("Panel/Anchor-Center/UI/RankPanel");


        AddGOReference("Panel/Anchor-Center/UI/DayNum/ActivityEndTime");
        AddGOReference("Panel/Anchor-Center/UI/Info");

        AddGOReference("Panel/Anchor-Center/UI/TabBtnGrid/Tab1");
        AddGOReference("Panel/Anchor-Center/UI/TabBtnGrid/Tab2");
        AddGOReference("Panel/Anchor-Center/UI/TabBtnGrid/Tab3");
        AddGOReference("Panel/Anchor-Center/UI/BuyScorePanel");

        AddGOReference("Panel/Anchor-Center/UI/BuyScorePanel/BtnBlack");
        AddGOReference("Panel/Anchor-Center/UI/BuyScorePanel/BtnConfirm");
        AddGOReference("Panel/Anchor-Center/UI/BuyScorePanel/BtnCancel");
        AddGOReference("Panel/Anchor-Center/UI/BuyScorePanel/BtnReduce");
        AddGOReference("Panel/Anchor-Center/UI/BuyScorePanel/BtnAdd");
        AddGOReference("Panel/Anchor-Center/UI/BuyScorePanel/InputBuyCountBg");

        //AddGOReference("Panel/Anchor-Center/UI/RankPanel");
        AddGOReference("Panel/Anchor-Center/UI/RankPanel/ScrollView/Grid");
        AddGOReference("Panel/Anchor-Center/UI/RankPanel/CommonTab");
        AddGOReference("Panel/Anchor-Center/UI/RankPanel/ScrollView");
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
        UpdateBtnTab(0);
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBg", OnCloseThis);
        f_RegClickEvent("BtnClose", OnCloseThis);

        f_RegClickEvent("Tab1", OnBtnTabThis, 0);
        f_RegClickEvent("Tab2", OnBtnTabThis, 1);
        f_RegClickEvent("Tab3", OnBtnTabThis, 2);

        f_RegClickEvent("BtnBlack", f_CloseBuyScore);
        f_RegClickEvent("BtnCancel", f_CloseBuyScore);
        f_RegClickEvent("BtnConfirm", f_BtnPlus);
        f_RegClickEvent("BtnAdd", OnBuyOneClick, 100);
        f_RegClickEvent("BtnReduce", OnBuyOneClick, -100);

        //glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_RANKING_POWER_LIST, OnGetSucCallback, this);

        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;

        RequestGetTaskAwardCallback.m_ccCallbackSuc = OnGetTaskAwardSucCallback;
        RequestGetTaskAwardCallback.m_ccCallbackFail = OnGetFailCallback;

        RequestGetAwardCallback.m_ccCallbackSuc = OnGetAwardSucCallback;
        RequestGetAwardCallback.m_ccCallbackFail = OnGetFailCallback;

        RequestGetRabkListCallback.m_ccCallbackSuc = OnGetRankListSucCallback;
        RequestGetRabkListCallback.m_ccCallbackFail = OnGetFailCallback;
    }

    private void OnCloseThis(GameObject go, object obj1, object obj2)
    {
        mFirstPage = true;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattlePassPage, UIMessageDef.UI_CLOSE);
        f_DestoryView();
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void OnBtnTabThis(GameObject go, object obj1, object obj2)
    {
        UpdateBtnTab((int)obj1);
    }

    int curtab = 0;
    private void UpdateBtnTab(int tab)
    {
        f_GetObject("Tab1").transform.Find("Down").gameObject.SetActive(tab != 0);
        f_GetObject("Tab1").transform.Find("Up").gameObject.SetActive(tab == 0);
        f_GetObject("Tab1").transform.Find("Reddot").gameObject.SetActive(false);
        f_GetObject("Tab2").transform.Find("Down").gameObject.SetActive(tab != 1);
        f_GetObject("Tab2").transform.Find("Up").gameObject.SetActive(tab == 1);
        f_GetObject("Tab2").transform.Find("Reddot").gameObject.SetActive(false);
        f_GetObject("Tab3").transform.Find("Down").gameObject.SetActive(tab != 2);
        f_GetObject("Tab3").transform.Find("Up").gameObject.SetActive(tab == 2);
        UpdateContent(tab);
        curtab = tab;
        f_CheckReddot();
    }


    private void f_ConfirmRecharge(object value)
    {
        int[] param = (int[])value;

        int iEventTime = param[0];
        int orderId = param[1];

        SocketCallbackDT whitelistCallbackDt = new SocketCallbackDT();
        whitelistCallbackDt.m_ccCallbackSuc = f_OnRechargeCoin;
        whitelistCallbackDt.m_ccCallbackFail = f_OnRechargeCoin;
        Data_Pool.m_RechargePool.RechargeBattlePass(iEventTime, orderId, whitelistCallbackDt);
    }

    private void f_OnRechargeCoin(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Mua hàng thành công");
            UpdateBtnTab(curtab);
            UpdateInfo();
        }
        else
        {
UITool.Ui_Trip("Mua hàng không thành công");
            MessageBox.ASSERT("Recharge whitelist failed,code:" + (int)result);
        }
    }

    private void f_UpdateTaskPanel()
    {

        Transform Root = f_GetObject("TaskPanel").transform;
        Transform TaskItemParent = Root.Find("TaskScrollView/TaskItemParent");
        Transform TaskItem = Root.Find("TaskItem");

        List<NBaseSCDT> temp_BattlePassDTs = glo_Main.GetInstance().m_SC_Pool.m_BattlePassTaskSC.f_GetAll();

        /**
        temp_BattlePassDTs.Sort(delegate (NBaseSCDT node1, NBaseSCDT node2)
        {
            try
            {
                BattlePassTaskDT item1 = (BattlePassTaskDT)node1;
                BattlePassTaskDT item2 = (BattlePassTaskDT)node2;

                EventTimePoolDT eventTimePoolDT1 = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(eventTimeDT.iId, item1.iId);
                int numiConditionParam1 = item1.iConditionParam1;
                switch (eventTimePoolDT1.idata2)
                {
                    case 0:
                        numiConditionParam1 = item1.iConditionParam1;
                        break;
                    case 1:
                        numiConditionParam1 = item1.iConditionParam2;
                        break;
                    case 2:
                        numiConditionParam1 = item1.iConditionParam3;
                        break;
                    case 3:
                        numiConditionParam1 = item1.iConditionParam4;
                        break;
                    case 4:
                        numiConditionParam1 = item1.iConditionParam5;
                        break;
                    case 5:
                        numiConditionParam1 = item1.iConditionParam6;
                        break;
                    case 6:
                        numiConditionParam1 = item1.iConditionParam7;
                        break;
                    case 7:
                        numiConditionParam1 = item1.iConditionParam7;
                        break;
                }
                EventTimePoolDT eventTimePoolDT2 = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(eventTimeDT.iId, item2.iId);
                int numiConditionParam2 = item2.iConditionParam1;
                switch (eventTimePoolDT2.idata2)
                {
                    case 0:
                        numiConditionParam2 = item2.iConditionParam1;
                        break;
                    case 1:
                        numiConditionParam2 = item2.iConditionParam2;
                        break;
                    case 2:
                        numiConditionParam2 = item2.iConditionParam3;
                        break;
                    case 3:
                        numiConditionParam2 = item2.iConditionParam4;
                        break;
                    case 4:
                        numiConditionParam2 = item2.iConditionParam5;
                        break;
                    case 5:
                        numiConditionParam2 = item2.iConditionParam6;
                        break;
                    case 6:
                        numiConditionParam2 = item2.iConditionParam7;
                        break;
                    case 7:
                        numiConditionParam2 = item2.iConditionParam7;
                        break;
                }
                bool get1 = eventTimePoolDT1.idata1 >= numiConditionParam1 && eventTimePoolDT1.idata2 < 7;

                bool get2 = eventTimePoolDT2.idata1 >= numiConditionParam2 && eventTimePoolDT2.idata2 < 7;
                if (get1)
                    return -1;
                else if (get2)
                    return 1;
                return item2.iId.CompareTo(item1.iId);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            return 0;
            
            
        });
        **/
        temp_BattlePassDTs = temp_BattlePassDTs.OrderBy(o => {
            BattlePassTaskDT item1 = (BattlePassTaskDT)o;
            EventTimePoolDT eventTimePoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(eventTimeDT.iId, item1.iId);
            int numiConditionParam1 = item1.iConditionParam1;
            switch (eventTimePoolDT.idata2)
            {
                case 0:
                    numiConditionParam1 = item1.iConditionParam1;
                    break;
                case 1:
                    numiConditionParam1 = item1.iConditionParam2;
                    break;
                case 2:
                    numiConditionParam1 = item1.iConditionParam3;
                    break;
                case 3:
                    numiConditionParam1 = item1.iConditionParam4;
                    break;
                case 4:
                    numiConditionParam1 = item1.iConditionParam5;
                    break;
                case 5:
                    numiConditionParam1 = item1.iConditionParam6;
                    break;
                case 6:
                    numiConditionParam1 = item1.iConditionParam7;
                    break;
                case 7:
                    numiConditionParam1 = item1.iConditionParam7;
                    break;
            }
            if (eventTimePoolDT.idata1 >= numiConditionParam1 && eventTimePoolDT.idata2 < 7)
            {
                return 0;
            }
            return 1;
        }).ToList();
        GridUtil.f_SetGridView<NBaseSCDT>(TaskItemParent.gameObject, TaskItem.gameObject, temp_BattlePassDTs, UpdateItem);
        TaskItemParent.GetComponent<UIGrid>().Reposition();
        Transform TaskScrollView = Root.Find("TaskScrollView");
        TaskScrollView.GetComponent<UIScrollView>().ResetPosition();

    }

    private void f_OnAwardItemClick(Transform tf, BasePoolDT<long> dt)
    {

    }
    int iTime1;
    DateTime tDate1;
    private void UpdateContent(int index = 0)
    {
        f_GetObject("TaskPanel").SetActive(index == 0);
        f_GetObject("AwardPanel").SetActive(index == 1);
        f_GetObject("RankPanel").SetActive(index == 2);

        switch (index)
        {
            case 0://mở task battlepass
                List<NBaseSCDT> temp_BattlePassDTs = glo_Main.GetInstance().m_SC_Pool.m_BattlePassTaskSC.f_GetAll();
                if (temp_BattlePassDTs.Count <= 0)
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.BattlePassPage, UIMessageDef.UI_CLOSE);
Debug.Log("don't set gift id for this event time");
                    return;
                }
                f_UpdateTaskPanel();
                break;
            case 1://mở award battlepass
                List<NBaseSCDT> temp_BattlePassAwardDTs = glo_Main.GetInstance().m_SC_Pool.m_BattlePassAwardSC.f_GetAll();
                if (temp_BattlePassAwardDTs.Count <= 0)
                {
Debug.Log("don't set gift id for this event time");
                    return;
                }
                f_UpdateAwardPanel();
                break;
            case 2://mở ranking battlepass
                Data_Pool.m_EventTimePool.f_GetBattlePassRankList(eventTimeDT.iId, RequestGetRabkListCallback);
                break;
            default:
                break;
        }
        switch (eventTimeDT.iType)
        {
            case 2:
                iTime1 = Data_Pool.m_EventTimePool.OpenSeverTime + eventTimeDT.iEndTime * 86400 - GameSocket.GetInstance().f_GetServerTime();
                break;
            case 3:

                DateTime endTime = new DateTime(int.Parse(eventTimeDT.iEndTime.ToString().Substring(0, 4)), int.Parse(eventTimeDT.iEndTime.ToString().Substring(4, 2)), int.Parse(eventTimeDT.iEndTime.ToString().Substring(6, 2))); //TsuCode
                iTime1 = (int)ccMath.DateTime2time_t(endTime) - GameSocket.GetInstance().f_GetServerTime();
                break;
        }


        tDate1 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime1);

        if (iTime1 > 0)
        {
            ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2112), tDate1.Day - 1, tDate1.Hour, tDate1.Minute) + tDate1.Second + CommonTools.f_GetTransLanguage(2114);
        }
        else
        {
            ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2115));
        }

        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_EventTimePool.f_BattlePassInfoPool(eventTimeDT.iId, RequestGetCallback);
    }

    private int InitKey(int idA, int idB)
    {
        string key = idA + "" + idB;
        return int.Parse(key);
    }

    private void UpdateItem(GameObject go, NBaseSCDT Data)
    {
        BattlePassTaskDT node = (BattlePassTaskDT)Data;
        Transform tran = go.transform;
        TaskItem tUpdateItem = go.GetComponent<TaskItem>();

        EventTimePoolDT eventTimePoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(eventTimeDT.iId, node.iId);
        //eventTimePoolDT.idata1 process  tổng số lần tham gia
        //eventTimePoolDT.idata2 iConditionParam ++ tăng bậc
        //eventTimePoolDT.idata3 hoàn thành tât cả == 1
        //eventTimePoolDT.idata4 

        //Unfinished == 0
        //Finish == 1
        //AlreadyAward == 2
        int numiConditionParam = node.iConditionParam1;
        int score = node.iScore1;
        switch (eventTimePoolDT.idata2)
        {
            case 0:
                numiConditionParam = node.iConditionParam1;
                score = node.iScore1;
                break;
            case 1:
                numiConditionParam = node.iConditionParam2;
                score = node.iScore2;
                break;
            case 2:
                numiConditionParam = node.iConditionParam3;
                score = node.iScore3;
                break;
            case 3:
                numiConditionParam = node.iConditionParam4;
                score = node.iScore4;
                break;
            case 4:
                numiConditionParam = node.iConditionParam5;
                score = node.iScore5;
                break;
            case 5:
                numiConditionParam = node.iConditionParam6;
                score = node.iScore6;
                break;
            case 6:
                numiConditionParam = node.iConditionParam7;
                score = node.iScore7;
                break;
            case 7:
                numiConditionParam = node.iConditionParam7;
                score = node.iScore7;
                break;
        }

        bool get = eventTimePoolDT.idata1 >= numiConditionParam && eventTimePoolDT.idata2 < 7;
        tUpdateItem.mGotoBtn.SetActive(!get);
        tUpdateItem.mGetAwardBtn.SetActive(get);
        tUpdateItem.mArealyGetTip.SetActive(eventTimePoolDT.idata2 == 7);

        tUpdateItem.mTitleLable.text = node.szDesc.Replace(GameParamConst.ReplaceFlag, numiConditionParam.ToString());
        tUpdateItem.mProgressLabel.text = string.Format("({0}/{1})", Mathf.Min(eventTimePoolDT.idata1, numiConditionParam), numiConditionParam);
        tUpdateItem.mAwardLabel.text = string.Format("x{0}", score);
        f_RegClickEvent(tUpdateItem.mGotoBtn, f_GotoClick, node);
        f_RegClickEvent(tUpdateItem.mGetAwardBtn, f_GetAward, node);
    }

    private void f_GotoClick(GameObject go, object value1, object value2)
    {
        BattlePassTaskDT node = (BattlePassTaskDT)value1;
        UITool.f_DailyGoto(this, (EM_DailyTaskCondition)node.iCondition);
    }

    private void f_GetAward(GameObject go, object value1, object value2)
    {
        BattlePassTaskDT node = (BattlePassTaskDT)value1;
        Data_Pool.m_EventTimePool.f_GetBattlePassTaskAward(eventTimeDT.iId, node.iCondition, RequestGetTaskAwardCallback);
    }

    private void CreareEffect(GameObject EquipObj, int Imporent)
    {
        string EffectName = "";
        switch ((EM_Important)Imporent)
        {
            case EM_Important.White:
            case EM_Important.Green:
                EffectName = UIEffectName.biankuangliuguang_green;
                break;
            case EM_Important.Blue:
                EffectName = UIEffectName.biankuangliuguang_bue;
                break;
            case EM_Important.Purple:
                EffectName = UIEffectName.biankuangliuguang_purple;
                break;
            case EM_Important.Oragen:
                EffectName = UIEffectName.biankuangliuguang_oragen;
                break;
            case EM_Important.Red:
            case EM_Important.Gold:
                EffectName = UIEffectName.biankuangliuguang_red;
                break;
        }

        for (int j = EquipObj.transform.childCount - 1; j >= 0; j--)//删除特效
            Destroy(EquipObj.transform.GetChild(j).gameObject);

        GameObject SetEquipEffect = UITool.f_CreateEffect_Old(EffectName, EquipObj.transform, Vector3.zero, 1f, 0, UIEffectName.UIEffectAddress1);
        SetEquipEffect.GetComponent<ParticleScaler>().TrailRenderSortingOrder = 1;
        SetEquipEffect.transform.parent.localScale = Vector3.one * 120;
        SetEquipEffect.transform.parent.localPosition = Vector3.zero;
        SetEquipEffect.transform.localPosition = Vector3.zero;
        SetEquipEffect.transform.localScale = Vector3.one;
    }
    private float updateTime = 0f;
    protected override void f_Update()
    {
        base.f_Update();
        updateTime += Time.deltaTime;
        if (updateTime > 1f)
        {
            updateTime = 0f;
            if (iTime1 > 0)
            {
                iTime1--;
                tDate1 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime1);
                ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2112), tDate1.Day - 1, tDate1.Hour, tDate1.Minute) + tDate1.Second + CommonTools.f_GetTransLanguage(2114);
            }
            else
            {
                ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2115));
            }
        }
    }


    private void OnClickItem(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT commonData = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }

    //查询成功回调
    private void OnSucCallback(object obj)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
    }

    private void OnGetSucCallback(object value)
    {
        //server đã gởi về pool

        UITool.f_OpenOrCloseWaitTip(false);
        // update info
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        Transform info = f_GetObject("Info").transform;
        UILabel Level = info.Find("FrameLevel/Level").GetComponent<UILabel>();
        UILabel Score = info.Find("Slider/Score").GetComponent<UILabel>();
        UISlider Slider = info.Find("Slider").GetComponent<UISlider>();

        f_RegClickEvent(info.Find("BtnBuy").gameObject, f_BtnBuy);
        f_RegClickEvent(info.Find("Plus").gameObject, f_OpenBuyScore);


        EventTimePoolDT eventTimePoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(eventTimeDT.iId, Idinfo);
        if (eventTimePoolDT == null) return;
        UITool.f_SetSpriteGray(info.Find("BtnBuy").gameObject, eventTimePoolDT.idata3 > 0);
        BattlePassAwardDT battlePassAwardDT = GetLevelTask(eventTimePoolDT);

        BattlePassAwardDT battlePassAwardDT_NextLvel = null;
        if (battlePassAwardDT == null)
        {
            battlePassAwardDT_NextLvel = (BattlePassAwardDT)glo_Main.GetInstance().m_SC_Pool.m_BattlePassAwardSC.f_GetSC(1);
        }
        else
        {
            battlePassAwardDT_NextLvel = (BattlePassAwardDT)glo_Main.GetInstance().m_SC_Pool.m_BattlePassAwardSC.f_GetSC(battlePassAwardDT.iLevel + 1);
        }

        Level.text = battlePassAwardDT == null ? "0" : battlePassAwardDT.iLevel.ToString();
        Score.text = battlePassAwardDT_NextLvel == null ? "Max" : eventTimePoolDT.idata1.ToString() + "/" + battlePassAwardDT_NextLvel.iScore.ToString();
        Slider.value = battlePassAwardDT_NextLvel == null ? 1 : (float)eventTimePoolDT.idata1 / (float)battlePassAwardDT_NextLvel.iScore;
    }


    private void f_BtnBuy(GameObject go, object value1, object value2)
    {
        EventTimePoolDT eventTimeInfoPoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(eventTimeDT.iId, Idinfo);

        if (eventTimeInfoPoolDT.idata3 > 0)
        {
UITool.Ui_Trip("Đã kích hoạt!");
            return;
        }
        int uCoin = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);


        if (uCoin >= param.iParam1)
        {
            MessageBox.ASSERT("TsuLog: Co the mua " + uCoin + " >= " + param.iParam1);

PopupMenuParams tParam = new PopupMenuParams("Xác Nhận", string.Format("Dùng {0} Kim Phiếu \n để mua {1}", param.iParam1, "Chiến Lệnh Nâng Cao"), "Đồng ý", f_ConfirmRecharge, " Hủy bỏ", null, new int[] { eventTimeDT.iId, Idinfo });
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
            MessageBox.ASSERT("TsuLog: Co the mua " + uCoin + " >= " + param.iParam1);
        }
        else
        {
            MessageBox.ASSERT("TsuLog: Khong du kiem phieu de giao dich " + uCoin + " < " + param.iParam1);
UITool.Ui_Trip("Không đủ Kim Phiếu!");
            glo_Main.GetInstance().m_SDKCmponent.f_ShowSDKPay();
        }
    }
    private void f_OpenBuyScore(GameObject go, object value1, object value2)
    {
        BuyScorePanel.SetActive(true);
        count = 0;
        f_UpdateBuyScorePanel();
    }

    private void f_CloseBuyScore(GameObject go, object value1, object value2)
    {
        BuyScorePanel.SetActive(false);
    }

    private void f_UpdateBuyScorePanel()
    {
        UIInput _InputCount = f_GetObject("InputBuyCountBg").GetComponent<UIInput>();
        _InputCount.value = count.ToString();

        UILabel NoteCost = BuyScorePanel.transform.Find("NoteCost").GetComponent<UILabel>();
NoteCost.text = string.Format("Cần {0} KNB để mua {1} điểm", param.iParam2* count, count);
    }
    private int count = 0;
    private void OnBuyOneClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        count += (int)obj1;
        if (count <= 0) count = 0;
        f_UpdateBuyScorePanel();
    }

    private void f_BtnPlus(GameObject go, object value1, object value2)
    {
        if (count <= 0)
        {     
            return;
        }
        BuyScorePanel.SetActive(false);
        EventTimePoolDT eventTimeInfoPoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(eventTimeDT.iId, Idinfo);

        int Sycee = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);

        int cost = param.iParam2 * count;

        if (Sycee >= cost)
        {
            MessageBox.ASSERT("TsuLog: Co the mua " + Sycee + " >= " + cost);

PopupMenuParams tParam = new PopupMenuParams("Xác Nhận", string.Format("Dùng {0} KNB \n mua {1} điểm", cost, count), "Đồng ý", f_ConfirmScoreRecharge, "Hủy bỏ", null, new int [] { eventTimeDT.iId, count });
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
            MessageBox.ASSERT("TsuLog: Co the mua " + Sycee + " >= " + count);
        }
        else
        {
            MessageBox.ASSERT("TsuLog: Khong du KNB de giao dich " + Sycee + " < " + count);

            string tipContent = string.Format(CommonTools.f_GetTransLanguage(1120));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
            //glo_Main.GetInstance().m_SDKCmponent.f_ShowSDKPay();
        }
    }

    private void f_ConfirmScoreRecharge(object value)
    {
        int[] param = (int[])value;

        int iEventTime = param[0];
        int count = param[1];

        SocketCallbackDT whitelistCallbackDt = new SocketCallbackDT();
        whitelistCallbackDt.m_ccCallbackSuc = f_OnRechargeCoin;
        whitelistCallbackDt.m_ccCallbackFail = f_OnRechargeCoin;
        Data_Pool.m_RechargePool.RechargeScoreBattlePass(iEventTime, count, whitelistCallbackDt);
    }


    private void OnGetTaskAwardSucCallback(object value)
    {
        //server đã gởi về pool
        UpdateBtnTab(0);
        UpdateInfo();
    }

    private BattlePassAwardDT GetLevelTask(EventTimePoolDT eventTimePoolDT)
    {
        List<NBaseSCDT> BattlePassAwardDTs = new List<NBaseSCDT>(glo_Main.GetInstance().m_SC_Pool.m_BattlePassAwardSC.f_GetAll());
        BattlePassAwardDTs.Reverse();
        for (int i = 0; i < BattlePassAwardDTs.Count; i++)
        {
            BattlePassAwardDT battlePassAwardDT = (BattlePassAwardDT)BattlePassAwardDTs[i];
            if (eventTimePoolDT.idata1 >= battlePassAwardDT.iScore)
            {
                return battlePassAwardDT;
            }
        }
        return null;
    }

    private void OnGetFailCallback(object obj)
    {
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1274));
    }

    public void f_DestoryView()
    {


    }

    public bool CheckDoneEvent(int eventTimeId)
    {

        return false;
    }

    private void f_UpdateAwardPanel()
    {

        Transform Root = f_GetObject("AwardPanel").transform;
        Transform ItemParent = Root.Find("AwardScrollView/TaskItemParent");
        Transform Item = Root.Find("AwardItem");

        List<NBaseSCDT> temp_BattlePassAwardDTs = glo_Main.GetInstance().m_SC_Pool.m_BattlePassAwardSC.f_GetAll();
        GameObject BtnGetAward = Root.Find("BtnGetAward").gameObject;
        GameObject Clock = Root.Find("Title/Vip/Clock").gameObject;
        f_RegClickEvent(BtnGetAward, f_BtnGetAward);

        UITool.f_SetSpriteGray(BtnGetAward, !CheckGetAward());
        Root.Find("Reddot").gameObject.SetActive(CheckGetAward());
        EventTimePoolDT eventTimeInfoPoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(eventTimeDT.iId, Idinfo);
        Clock.SetActive(eventTimeInfoPoolDT.idata3 <= 0);
        GridUtil.f_SetGridView<NBaseSCDT>(ItemParent.gameObject, Item.gameObject, temp_BattlePassAwardDTs, UpdateAwardItem);
        ItemParent.GetComponent<UIGrid>().Reposition();
        Transform AwardScrollView = Root.Find("AwardScrollView");
        AwardScrollView.GetComponent<UIScrollView>().ResetPosition();
    }

    private void UpdateAwardItem(GameObject go, NBaseSCDT Data)
    {
        BattlePassAwardDT node = (BattlePassAwardDT)Data;
        Transform tran = go.transform;

        UILabel level = tran.Find("Level").GetComponent<UILabel>();
        level.text = string.Format("lv.{0}", node.iLevel);


        EventTimePoolDT eventTimeInfoPoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(eventTimeDT.iId, Idinfo);

        List<ResourceCommonDT> listNorCommonDT = CommonTools.f_GetListCommonDT(node.szAwardLow);
        List<ResourceCommonDT> listVipCommonDT = CommonTools.f_GetListCommonDT(node.szAwardHight);

        GameObject norItem = tran.Find("Nor/Item1").gameObject;
        UpdateItemAward(norItem, listNorCommonDT[0], eventTimeInfoPoolDT.idata2 >= node.iLevel, node.iScore > eventTimeInfoPoolDT.idata1);


        GameObject vipItem1 = tran.Find("Vip/Item1").gameObject;
        UpdateItemAward(vipItem1, listVipCommonDT[0], eventTimeInfoPoolDT.idata4 >= node.iLevel, eventTimeInfoPoolDT.idata3 == 0 || node.iScore > eventTimeInfoPoolDT.idata1);


        GameObject vipItem2 = tran.Find("Vip/Item2").gameObject;

        if (listVipCommonDT.Count < 2)
        {
            vipItem2.SetActive(false);
        }
        else
        {
            vipItem2.SetActive(true);
            UpdateItemAward(vipItem2, listVipCommonDT[1], eventTimeInfoPoolDT.idata4 >= node.iLevel, eventTimeInfoPoolDT.idata3 == 0 || node.iScore > eventTimeInfoPoolDT.idata1);
        }




        //eventTimeInfoPoolDT.idata1 score
        //eventTimeInfoPoolDT.idata2 reward nor
        //eventTimeInfoPoolDT.idata3 active vip  1 / 0 chưa acctive
        //eventTimeInfoPoolDT.idata4 reward vip

        UISprite NorClose = tran.Find("Nor").GetComponent<UISprite>();
        NorClose.enabled = eventTimeInfoPoolDT.idata1 < node.iScore;

        UISprite VipClose = tran.Find("Vip").GetComponent<UISprite>();
        VipClose.enabled = eventTimeInfoPoolDT.idata3 == 0 || eventTimeInfoPoolDT.idata1 < node.iScore;


        //bool get = eventTimePoolDT.idata1 >= numiConditionParam && eventTimePoolDT.idata2 < 7;
        //tUpdateItem.mGotoBtn.SetActive(!get);
        //tUpdateItem.mGetAwardBtn.SetActive(get);
        //tUpdateItem.mArealyGetTip.SetActive(eventTimePoolDT.idata2 == 7);

        //tUpdateItem.mTitleLable.text = node.szDesc.Replace(GameParamConst.ReplaceFlag, numiConditionParam.ToString());
        //tUpdateItem.mProgressLabel.text = string.Format("({0}/{1})", Mathf.Min(eventTimePoolDT.idata1, numiConditionParam), numiConditionParam);
        //tUpdateItem.mAwardLabel.text = string.Format("x{0}", score);
        //f_RegClickEvent(tUpdateItem.mGotoBtn, f_GotoClick, node);
        //f_RegClickEvent(tUpdateItem.mGetAwardBtn, f_GetAward, node);

    }

    private void UpdateItemAward(GameObject go, ResourceCommonDT Data, bool done, bool _lock)
    {
        Transform tran = go.transform;

        UI2DSprite Icon = tran.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Num = tran.Find("Score").GetComponent<UILabel>();
        GameObject Done = tran.Find("Done").gameObject;
        GameObject Lock = tran.Find("Clock").gameObject;

        Icon.sprite2D = UITool.f_GetIconSprite(Data.mIcon);
        Num.text = Data.mResourceNum.ToString();
        Done.SetActive(done);
        Lock.SetActive(_lock);

        //Case.spriteName = UITool.f_GetImporentCase(Data.mImportant);
        f_RegClickEvent(Icon.gameObject, OnClickItem, Data);
    }


    private void f_BtnGetAward(GameObject go, object value1, object value2)
    {
        if (CheckGetAward())
        {
            Data_Pool.m_EventTimePool.f_GetBattlePassAward(eventTimeDT.iId, RequestGetAwardCallback);
        }
    }


    private bool CheckGetAward()
    {
        List<NBaseSCDT> temp_BattlePassAwardDTs = glo_Main.GetInstance().m_SC_Pool.m_BattlePassAwardSC.f_GetAll();
        EventTimePoolDT eventTimeInfoPoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(eventTimeDT.iId, Idinfo);

        for (int i = 0; i < temp_BattlePassAwardDTs.Count; i++)
        {
            BattlePassAwardDT battlePassAwardDT = (BattlePassAwardDT)temp_BattlePassAwardDTs[i];
            if (battlePassAwardDT.iLevel > eventTimeInfoPoolDT.idata2 && battlePassAwardDT.iScore <= eventTimeInfoPoolDT.idata1)
            {
                return true;
            }
            if (eventTimeInfoPoolDT.idata3 > 0 && battlePassAwardDT.iLevel > eventTimeInfoPoolDT.idata4 && battlePassAwardDT.iScore <= eventTimeInfoPoolDT.idata1)
            {
                return true;
            }
        }
        Debug.Log("ko co qua de nhan");
        return false;
    }

    private void OnGetAwardSucCallback(object value)
    {
        //server đã gởi về pool
        UpdateBtnTab(1);
    }

    private void f_AwardItemUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        RankingPowerAwardPoolDT rankingPowerAwardPoolDT = (RankingPowerAwardPoolDT)dt;
        if (null == rankingPowerAwardPoolDT) return;

        //UI2DSprite headIcon = tf.Find("HeadIcon").GetComponent<UI2DSprite>();
        //UISprite headFrame = tf.Find("HeadFrame").GetComponent<UISprite>();
        //GameObject objFightPower = tf.Find("Sprite_FightPower").gameObject;
        UILabel labelPower = tf.Find("PowerLabel").GetComponent<UILabel>();
        UISprite spriteRank = tf.Find("Sprite_Rank").GetComponent<UISprite>();
        UILabel labelRank = tf.Find("RankLabel").GetComponent<UILabel>();
        UILabel labelName = tf.Find("NameLabel").GetComponent<UILabel>();
        //UILabel labelGuildName = tf.Find("LegionLabel").GetComponent<UILabel>();
        //UILabel labelBepraised = tf.Find("BepraisedTimesLabel").GetComponent<UILabel>();
        //GameObject spriteBepraiseBtn = labelBepraised.transform.Find("Fabulous").gameObject;
        //GameObject objSelectedFlag = tf.Find("Sprite_Select").gameObject;
        //GameObject objStar = tf.Find("Sprite_Star").gameObject;
        //UILabel labelStarNum = objStar.transform.Find("StarNumLabel").GetComponent<UILabel>();
        //UILabel labelChapterInfo = objStar.transform.Find("ChapterInfoLabel").GetComponent<UILabel>();
        //UILabel labelLv = tf.Find("LabelLv").GetComponent<UILabel>();

        labelPower.text = rankingPowerAwardPoolDT.Ft.ToString();
        int rank = rankingPowerAwardPoolDT.Rank;
        spriteRank.gameObject.SetActive(rank <= 3);
        labelRank.gameObject.SetActive(rank > 3);

        if (rank > 3)
        {
            labelRank.text = rank.ToString();
        }
        else
        {
            spriteRank.spriteName = "rank" + rank;
        }

        labelName.text = rankingPowerAwardPoolDT.UserName;


        Transform GoodsParent = tf.Find("GoodsParent");
        Transform Item = GoodsParent.Find("Item");

        BattlePassRankingDT BattlePassRankingDT = glo_Main.GetInstance().m_SC_Pool.m_BattlePassRankingSC.GetAwardDTByRank(rank);


        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(BattlePassRankingDT.szAward);
        GridUtil.f_SetGridView<ResourceCommonDT>(GoodsParent.gameObject, Item.gameObject, listCommonDT, UpdateItem);
        GoodsParent.GetComponent<UIGrid>().Reposition();

    }

    private void UpdateItem(GameObject go, ResourceCommonDT Data)
    {
        Transform tran = go.transform;

        UI2DSprite Icon = tran.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Num = tran.Find("Num").GetComponent<UILabel>();
        UISprite Case = tran.Find("Case").GetComponent<UISprite>();

        Icon.sprite2D = UITool.f_GetIconSprite(Data.mIcon);
        Num.text = Data.mResourceNum.ToString();
        Case.spriteName = UITool.f_GetImporentCase(Data.mImportant);
        //CreareEffect(tran.Find("Effect").gameObject, Data.mImportant);

        f_RegClickEvent(Case.gameObject, OnClickItem, Data);
    }


    private void f_OnMomnetEnds()
    {
        Vector3 constraint = mScrollView.panel.CalculateConstrainOffset(mScrollView.bounds.min, mScrollView.bounds.min);
        if (constraint.y <= 0)
        {
            mFirstPage = false;
            UpdateContent(2);
        }
    }

    private void OnGetRankListSucCallback(object value)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //server đã gởi về pool
        //f_UpdateRankPanel();
        List<BasePoolDT<long>> _rankList = null;

        _rankList = Data_Pool.m_EventTimePool.f_GetRankList(eventTimeDT.iId);

        mRankWrapComponet.f_UpdateList(_rankList);
        mRankWrapComponet.f_UpdateView();
        if (mFirstPage)
        {
            mScrollView.ResetPosition();
        }

        f_UpdateMyRankInfo();
    }

    private void f_UpdateMyRankInfo()
    {
        Transform Root = f_GetObject("RankPanel").transform;
        int rank = Data_Pool.m_EventTimePool.f_GetMyRank(eventTimeDT.iId);
        EventTimePoolDT eventTimeInfoPoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(eventTimeDT.iId, Idinfo);


        UILabel PowerLabel = Root.Find("MyInfoItem/PowerLabel").GetComponent<UILabel>();
        UILabel RankLabel = Root.Find("MyInfoItem/RankLabel").GetComponent<UILabel>();

        if(rank == -1 || rank > 50)
        {
            RankLabel.text = "---";
        }
        else
        {
            RankLabel.text = rank.ToString();
        }
        PowerLabel.text = eventTimeInfoPoolDT.idata1.ToString();
    }

    private void f_CheckReddot()
    {
        if (Data_Pool.m_EventTimePool.CheckReddotBattlePassTask(eventTimeDT.iId))
        {
            f_GetObject("Tab1").transform.Find("Reddot").gameObject.SetActive(true);
        }
        if (Data_Pool.m_EventTimePool.CheckReddotBattlePassAward(eventTimeDT.iId, Idinfo))
        {
            f_GetObject("Tab2").transform.Find("Reddot").gameObject.SetActive(true);
        }

    }
}

