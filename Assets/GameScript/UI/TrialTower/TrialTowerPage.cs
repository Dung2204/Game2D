using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialTowerPage : UIFramwork
{
    private int mSelectId;
    private TrialTowerPoolDT mSelectPoolDT;

    private UILabel MaxPass;
    private UILabel NeedBattle;
    private UILabel mTimeLabel;
    private GameObject mRole;

    private SocketCallbackDT EnterCall = new SocketCallbackDT();
    private SocketCallbackDT SweepCall = new SocketCallbackDT();
    private SocketCallbackDT ChellengCall = new SocketCallbackDT();
    private SocketCallbackDT ResetCall = new SocketCallbackDT();

    private List<AwardPoolDT> AwardList = new List<AwardPoolDT>();
    private int LastTower;

    private int Time_UpdateLabel;

    private string TexturePath = "UI/TextureRemove/TrialTower/qxmd_bg_b";

    private UIWrapComponent _UIWrapComponent;

    public UIWrapComponent mUIWrapComponent
    {
        get
        {

            if (_UIWrapComponent == null)
            {
                List<BasePoolDT<long>> tList = Data_Pool.m_TrialTowerPool.f_GetAll();
                tList.Reverse();
                //_UIWrapComponent = new UIWrapComponent(247, 1, 148, 5, f_GetObject("ItemParent"), f_GetObject("Item"), tList, UpdateItem,null);
            }
            return _UIWrapComponent;
        }
    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);

        //断线重连如果切去其他界面，这时候不处理，服务器消息返回再打开界面就会和其他界面重叠
        if (!UITool.f_IsCanOpenChallengePage(UINameConst.TrialTowerPage))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerPage, UIMessageDef.UI_CLOSE);
            return;
        }

        UpdateItemScrollView();

        if (!Data_Pool.m_TrialTowerPool.oneEnter)
        {
            EnterSuc(null);
        }
        else
        {
            EnterCall.m_ccCallbackFail = EnterFill;
            EnterCall.m_ccCallbackSuc = EnterSuc;
            Data_Pool.m_TrialTowerPool.f_Enter(EnterCall);
        }
        f_InitTexture();
    }

    private void f_InitTexture()
    {
        UITexture bg = f_GetObject("Bg").GetComponent<UITexture>();

        if (bg.mainTexture == null)
        {
            Texture2D tt = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(TexturePath);

            if (tt != null)
            {
                bg.mainTexture = tt;
            }
        }

    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        TimerControl(false);
        //ccTimeEvent.GetInstance().f_UnRegEvent(Time_UpdateLabel);
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Btn_Challenge", f_Challenge);
        f_RegClickEvent("Btn_Back", f_Back);
        f_RegClickEvent("Btn_Sweep", f_Sweep);
        f_RegClickEvent("Btn_bz", f_bz);
        f_RegClickEvent("Btn_Reset", f_Reset);
        f_RegClickEvent("Btn_qxmd", f_qxmd);
        f_RegClickEvent("BtnHelp", f_Help);
    }

    #region 按钮事件
    private void f_Back(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_UnHold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerPage, UIMessageDef.UI_CLOSE);
    }

    private void f_Sweep(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_TrialTowerPool.NowTower >= Data_Pool.m_TrialTowerPool.Max)
        {
UITool.Ui_Trip("Đã ở tầng cao nhất hiện tại");
            return;
        }

        if (Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(212) < 1)
        {
UITool.Ui_Trip("Không đủ vật phẩm cần thiết");
            return;
        }
        ResourceCommonDT tData = new ResourceCommonDT();
        tData.f_UpdateInfo((byte)EM_ResourceType.Good,212,1);
PopupMenuGoodsParams tGoodsParams = new PopupMenuGoodsParams("Nhắc nhở","Quét sẽ tự động hoàn thành đến tầng cao nhất hiện tại","Quét",
            "Hủy",PopSweepFail, "Quét", PopSweepSuc, null,null, tData);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuGoodsPage,UIMessageDef.UI_OPEN, tGoodsParams);
    }

    private void f_qxmd(GameObject go, object obj1, object obj2)
    {
        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.SevenStarOpenLv))
        {
UITool.Ui_Trip("Không đủ cấp độ");
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SevenStarPage, UIMessageDef.UI_OPEN);
    }

    private void f_bz(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }
    private void f_Help(GameObject go, object obj1, object obj2) {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN,9);
    }

    private void f_Challenge(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        //SocketCallbackDT ChellengCall = new SocketCallbackDT();
        ChellengCall.m_ccCallbackFail = ChellengFill;
        ChellengCall.m_ccCallbackSuc = ChellengSuc;
        Data_Pool.m_TrialTowerPool.f_Challeng(ChellengCall);
    }

    private void f_Reset(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        ResetCall.m_ccCallbackFail = ResetFill;
        ResetCall.m_ccCallbackSuc = ResetSuc;
        Data_Pool.m_TrialTowerPool.f_Reset(ResetCall);
    }

    private void ChellengFill(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
UITool.Ui_Trip("Khiêu chiến thất bại, Code:" + obj.ToString());
    }

    private void ChellengSuc(object obj)
    {
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        //展示加载界面 并加载战斗场景
        //StaticValue.m_CurBattleConfig.m_eBattleType = EM_Fight_Enum.eFight_TrialTower;
        StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_TrialTower, 0, 0, 0);
        StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.TrialTower, null);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
        UITool.f_OpenOrCloseWaitTip(false);
    }


    private void SweepSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
UITool.Ui_Trip("Hoàn thành quét");
        OpenUI();
        UpdateUI();
        AwardList.Clear();
        int MaxTower = Data_Pool.m_TrialTowerPool.Max;
        TrialTowerPoolDT tTrialTowerPoolDT;
        for (int i = LastTower; i <= MaxTower; i++)
        {
            tTrialTowerPoolDT = Data_Pool.m_TrialTowerPool.f_GetForId(i) as TrialTowerPoolDT;
            List<AwardPoolDT> t = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(tTrialTowerPoolDT.mTemp.iPond);
            for (int j = 0; j < t.Count; j++)
            {
                AwardPoolDT tPoolDT = AwardList.Find((AwardPoolDT a) =>
                {
                    return a.mTemplate.mResourceId == t[j].mTemplate.mResourceId;
                });
                if (tPoolDT != null)
                {
                    tPoolDT.mTemplate.f_AddNum(t[j].mTemplate.mResourceNum);
                }
                else
                {
                    AwardList.Add(t[j]);
                }
            }

            //AwardList.AddRange();
        }

        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { AwardList });
    }

    private void SweepFill(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
    }

    private void EnterSuc(object obj)
    {
        InitUI();
        OpenUI();
        UpdateUI();
    }

    private void EnterFill(object obj)
    {
UITool.Ui_Trip("Thất bại, obj:" + obj);
        ccUIHoldPool.GetInstance().f_UnHold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerPage, UIMessageDef.UI_CLOSE);
    }

    private void ResetSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //f_Back(null, null, null);
        //ccUIHoldPool.GetInstance().f_UnHold();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerPage,UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerRoomPage, UIMessageDef.UI_OPEN);
        Data_Pool.m_TrialTowerPool.isEnter = false;
        Data_Pool.m_TrialTowerPool.isFirst = false;
        Data_Pool.m_TrialTowerPool.oneEnter = true;
        //OpenUI();
        //UpdateUI();
    }

    private void ResetFill(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
    }

    private void PopSweepSuc(object obj) {
        LastTower = Data_Pool.m_TrialTowerPool.NowTower;
        UITool.f_OpenOrCloseWaitTip(true);
        // SocketCallbackDT ChallengCall = new SocketCallbackDT();
        SweepCall.m_ccCallbackFail = SweepFill;
        SweepCall.m_ccCallbackSuc = SweepSuc;
        Data_Pool.m_TrialTowerPool.f_Sweep(SweepCall);
    }

    private void PopSweepFail(object obj) {

    }
    #endregion

    #region 刷新事件


    private void InitUI()
    {
        MaxPass = f_GetObject("MaxPass").GetComponent<UILabel>();
        NeedBattle = f_GetObject("NeedBattle").GetComponent<UILabel>();
        mTimeLabel = f_GetObject("Time").GetComponent<UILabel>();
    }
    private void OpenUI()
    {
        mSelectId = Data_Pool.m_TrialTowerPool.NowTower;
        if (mSelectId == 0)
        {
            mSelectId = 1;
        }
        mSelectPoolDT = Data_Pool.m_TrialTowerPool.f_GetForId(mSelectId) as TrialTowerPoolDT;
        UpdateItemScrollView();
        //mUIWrapComponent.f_UpdateView();
        UpdateMax();
        UpdateNeedBattle(mSelectPoolDT);
    }
    private void TimerControl(bool isStart)
    {
        CancelInvoke("UpdateTime");
        if (isStart)
        {
            InvokeRepeating("UpdateTime", 0f, 1f);
        }
    }
    private void UpdateUI()
    {
        CreateRole(mSelectPoolDT);
        UpdateAwardItem(mSelectPoolDT);
        SetScrollBar();
        UpdateBtn();
        //ccTimeEvent.GetInstance().f_UnRegEvent(Time_UpdateLabel);
        //UpdateTime(null);
        //Time_UpdateLabel = ccTimeEvent.GetInstance().f_RegEvent(30f,true,null, UpdateTime);
        int ResTime = Data_Pool.m_TrialTowerPool.mNowEndTime - GameSocket.GetInstance().f_GetServerTime();
        if (ResTime <= 0)
        {
            TimerControl(false);
            return;
        }
        TimerControl(true);
    }


    private void UpdateItemScrollView()
    {
        //Data_Pool.m_TrialTowerPool.f_GetAll().Reverse();
        GridUtil.f_SetGridView<BasePoolDT<long>>(f_GetObject("ItemParent"), f_GetObject("Item"), Data_Pool.m_TrialTowerPool.f_GetAll(), UpdateItem);
        //f_GetObject("Bar").GetComponent<UIProgressBar>().value = 0;
    }
    private void UpdateItem(GameObject titem, BasePoolDT<long> dt)
    {
        TrialTowerPoolDT Pooldt = dt as TrialTowerPoolDT;
        Transform item = titem.transform;
        bool isShowPass = Pooldt.iId != Data_Pool.m_TrialTowerPool.f_GetAll().Count;
        GameObject Pass = item.Find("Pass").gameObject;
        GameObject SliderBg = item.Find("SliderBg").gameObject;
        //Pass.SetActive(Pooldt.iId == Data_Pool.m_TrialTowerPool.f_GetAll().Count);
        GameObject Select = item.Find("Select").gameObject;
        GameObject SelectState = item.Find("SelectState").gameObject;


        UI2DSprite Icon = item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Label = item.Find("Label").GetComponent<UILabel>();
        UISprite NodeState = item.Find("NodeState").GetComponent<UISprite>();
        Pass.SetActive(Data_Pool.m_TrialTowerPool.NowTower > Pooldt.iId);
        Select.SetActive(Pooldt.iId == mSelectId);
        SelectState.SetActive(mSelectId == Pooldt.iId);

        //MonsterDT tMonster = glo_Main.GetInstance().m_SC_Pool.m_MonsterSC.f_GetSC(Pooldt.mTemp.iModeId) as MonsterDT;
        Icon.sprite2D = UITool.f_GetIconSpriteByModelId(Pooldt.mTemp.iModeId);
        bool tPass = Data_Pool.m_TrialTowerPool.NowTower > Pooldt.iId;
        UITool.f_Set2DSpriteGray(Icon, tPass);
        UITool.f_SetSpriteGray(NodeState, tPass);
Label.text = string.Format("Tầng {0}", Pooldt.iId);

        //Pass.SetActive(isShowPass);
        SliderBg.SetActive(isShowPass);
        //f_RegClickEvent(titem, UpdateClick, (int)Pooldt.iId, Pooldt);
    }

    private void UpdateClick(GameObject go, object obj1, object obj2)
    {
        mSelectId = (int)obj1;
        //mUIWrapComponent.f_UpdateView();
        mSelectPoolDT = (TrialTowerPoolDT)obj2;
        UpdateItemScrollView();
        CreateRole(mSelectPoolDT);
        UpdateAwardItem(mSelectPoolDT);
        UpdateBtn();
    }

    private void UpdateMax()
    {
MaxPass.text = string.Format("Tầng cao nhất: {0}", Data_Pool.m_TrialTowerPool.Max);
    }

    private void UpdateNeedBattle(TrialTowerPoolDT TrialTower)
    {
        NeedBattle.text = TrialTower.mTemp.iFightValue.ToString();
    }

    private void CreateRole(TrialTowerPoolDT TrialTower)
    {
        eTrialTowerState nowState = Data_Pool.m_TrialTowerPool.NowState;
        UITool.f_CreateRoleByModeId(TrialTower.mTemp.iModeId, ref mRole, f_GetObject("Role").transform, 15);
        mRole.transform.localScale = Vector3.one * 110;
        f_GetObject("YJSLabel").SetActive(false);
        f_GetObject("QBTGLabel").SetActive(nowState == eTrialTowerState.Suc);
        f_GetObject("TZSBLabel").SetActive(nowState == eTrialTowerState.Fail);
        if (nowState == eTrialTowerState.Fail || nowState == eTrialTowerState.Suc)
        {
            SetModelColor.SetColor(mRole, SetModelColor.gray);
        }
        else
        {
            SetModelColor.SetColor(mRole, SetModelColor.normal);
        }
    }

    private void UpdateAwardItem(TrialTowerPoolDT TrialTower)
    {
        List<AwardPoolDT> t = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(TrialTower.mTemp.iPond);
        GridUtil.f_SetGridView<AwardPoolDT>(f_GetObject("AwardParent"), f_GetObject("Award"), t, UpdateAwardItem);

    }

    private void UpdateBtn()
    {
        f_GetObject("Btn_Reset").SetActive(!(Data_Pool.m_TrialTowerPool.NowState == eTrialTowerState.NoEnter));
        f_GetObject("Btn_Challenge").SetActive(Data_Pool.m_TrialTowerPool.NowState == eTrialTowerState.NoEnter);
        f_GetObject("isGet").SetActive(Data_Pool.m_TrialTowerPool.NowState == eTrialTowerState.Suc);
    }

    private void UpdateAwardItem(GameObject oneItem, AwardPoolDT dataInfo)
    {
        UISprite Case = oneItem.transform.Find("Case").GetComponent<UISprite>();
        UI2DSprite Icon = oneItem.transform.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Num = oneItem.transform.Find("Num").GetComponent<UILabel>();

        Case.spriteName = UITool.f_GetImporentCase(dataInfo.mTemplate.mImportant);
        UITool.f_SetIconSprite(Icon, (EM_ResourceType)dataInfo.mTemplate.mResourceType, dataInfo.mTemplate.mResourceId);
        Num.text = dataInfo.mTemplate.mResourceNum.ToString();
        f_RegClickEvent(oneItem, OnClickItem, dataInfo.mTemplate);
    }

    private void OnClickItem(GameObject go,object obj1,object obj2) {
        ResourceCommonDT data = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, data);
    }
    private void SetScrollBar()
    {
        float value = (float)(mSelectId - 1) / (float)(Data_Pool.m_TrialTowerPool.f_GetAll().Count);
        //mUIWrapComponent.f_ViewGotoRealIdx(Data_Pool.m_TrialTowerPool.f_GetAll().Count-mSelectId, 4);
        f_GetObject("ItemParent").GetComponent<UIGrid>().repositionNow = true;
        f_GetObject("Bar").GetComponent<UIProgressBar>().value = 1f - value;
    }

    private void UpdateTime() {
        int ResTime = Data_Pool.m_TrialTowerPool.mNowEndTime - GameSocket.GetInstance().f_GetServerTime();
        if(ResTime <= 0)
        {
            TimerControl(false);
        }
        int Day = ResTime / 86400;
        ResTime -= Day * 86400;
        int Hours = ResTime / 3600;
        ResTime -= Hours * 3600;

        mTimeLabel.text = string.Format("Thời gian hoạt động: [-][00FF02]{0} ngày {1}:{2}", Day, Hours, ResTime / 60);
    }
    #endregion
}
