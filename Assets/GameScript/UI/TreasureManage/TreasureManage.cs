using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System.Threading;
using System;

public class TreasureManage : UIFramwork
{
    /// <summary>
    /// 装备信息
    /// </summary>
    Transform _EquipCase;
    UILabel _EquipName;
    UILabel _FitOut;
    UILabel _Color;
    UI2DSprite _EquipIcon;
    public static byte s_realTimes;
    public static byte s_critTimes;
    public static bool s_bisIntensify;
	private EM_FormationPos m_curFormationPos = EM_FormationPos.eFormationPos_Main;
    long[] DogFood = new long[4];
    TreasurePoolDT m_TreasurePoolDT;
    UIWrapComponent _treasure;
    TreasureBox tmp;
    TreasureBox.BoxType tmpBoxType;

    List<BasePoolDT<long>> DogFoodList;
    /// <summary>
    /// 特效父级
    /// </summary>
    Transform EffectParent;
    GameObject Refine_Effect;
    GameObject Inten_Effect;

    TreasurePoolDT[] tmpTrea = new TreasurePoolDT[4];    //强化选择的道具
    List<BasePoolDT<long>> tbasepool = new List<BasePoolDT<long>>();  //用来显示的


    //展示强化属性
    private int _TmpTreasureLevel;
    private int _TmpTreasureExp;
	private int m_curMasterLevel = 0;
	private int m_curMasterLevel2 = 0;
    private TreasureUpConsumeDT _TmpUpConsume;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        tmp = (TreasureBox)e;
        tmpBoxType = tmp.tType;
        m_TreasurePoolDT = tmp.tTreasurePoolDT;
		m_curFormationPos = LineUpPage.CurrentSelectCardPos;

        _TmpTreasureLevel = m_TreasurePoolDT.m_lvIntensify;
        _TmpTreasureExp = m_TreasurePoolDT.m_ExpIntensify;

        Initialize();
        f_GetObject("Btn_Chage").SetActive(tmp.IsShowChange == 0);
        f_GetObject("Btn_Down").SetActive(tmp.IsShowChange == 0);
        f_GetObject("Btn_Left").SetActive(tmp.IsShowChange == 0);
        f_GetObject("Btn_Right").SetActive(tmp.IsShowChange == 0);
        f_GetObject("Btn_Intensify").SetActive(tmp.tType != TreasureBox.BoxType.GetWay && m_TreasurePoolDT.m_TreasureDT.iSite != 7);
        f_GetObject("Btn_Refine").SetActive(tmp.tType != TreasureBox.BoxType.GetWay && m_TreasurePoolDT.m_TreasureDT.iSite != 7 && m_TreasurePoolDT.m_TreasureDT.iImportant >= (int)EM_Important.Oragen);
        f_GetObject("Btn_GetWay").SetActive(tmp.tType == TreasureBox.BoxType.GetWay);
        f_GetObject("Btn").GetComponent<UIGrid>().Reposition();
        CheckRedPoint();
        if (tmp.IsSelectAward != 1)
            UI_ChangeUI(tmp.tType);
        else
        {
            UI_ChangeUI(TreasureBox.BoxType.Intro);
        }

    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Btn_Introduce", UI_ChangeTab, TreasureBox.BoxType.Intro);
        f_RegClickEvent("Btn_Intensify", UI_ChangeTab, TreasureBox.BoxType.Inten);
        f_RegClickEvent("Btn_Refine", UI_ChangeTab, TreasureBox.BoxType.Refine);
        f_RegClickEvent("Btn_GetWay", UI_ChangeTab, TreasureBox.BoxType.GetWay);
        f_RegClickEvent("Btn_Back", UI_CloseIntroduce);
        f_RegClickEvent("UpSelection", UI_CloseUpSelection);
        f_RegClickEvent("Intensify_AutoAdd", UI_AutoAdd);
        f_RegClickEvent("Btn_Chage", UI_Change);
        f_RegClickEvent("Btn_Left", UI_ChangeTreasure);
        f_RegClickEvent("Btn_Right", UI_ChangeTreasure);
        f_RegClickEvent("Btn_Down", Btn_DownTreasure);
    }
    #region 红点提示
    private void CheckRedPoint()
    {
        bool isCanLvUp = false;//有装备且可以升级
        bool isCanEquip = false;//无装备有新装备或更高品质
        bool CanRefine = false;
        Data_Pool.m_TeamPool.f_CheckTeamTreasureRedPoint(m_TreasurePoolDT, (EM_EquipPart)m_TreasurePoolDT.m_TreasureDT.iSite, ref isCanLvUp, ref isCanEquip, ref CanRefine);
        UITool.f_UpdateReddot(f_GetObject("Btn_Intensify"), isCanLvUp ? 1 : 0, new Vector3(100, 28, 0), 1100);
        UITool.f_UpdateReddot(f_GetObject("Btn_Chage"), isCanEquip ? 1 : 0, new Vector3(105, 20, 0), 2500);
        UITool.f_UpdateReddot(f_GetObject("Btn_Refine"), CanRefine ? 1 : 0, new Vector3(100, 28, 0), 1100);
    }
    #endregion
    #region UI
    /// <summary>
    /// 信息界面 属性简介
    /// </summary>
    Transform Introduce_ProTab;
    UILabel ProTab_Level;
    UILabel ProTab_Pro;
    UILabel ProTab_ProName;
    UILabel ProTab_ProName2;
    UILabel ProTab_Pro2;
    UILabel ProTab_Master1;
    UILabel ProTab_Master2;
    /// <summary>
    /// 信息界面  精炼简介 
    /// </summary>
    Transform Introduce_RefineTab;
    UILabel RefineTab_Level;
    UILabel RefineTab_Pro1;
    UILabel RefineTab_Pro1Name;
    UILabel RefineTab_Pro2;
    UILabel RefineTab_Pro2Name;
    UILabel RefineTab_Master;
    /// <summary>
    /// 信息界面   说明简介
    /// </summary>
    Transform Introduce_ParticularsTab;
    UILabel ParticularsTab_Label;
    /// <summary>
    /// 升级界面
    /// </summary>
    Transform _Intensify;
    UILabel Intensify_StartLevel;
    UILabel Intensify_LastLevel;
    UILabel Intensify_ProName;
    UILabel Intensify_Pro;
    UILabel Intensify_AddPro;
    UILabel Intensify_ProName2;
    UILabel Intensify_Pro2;
    UILabel Intensify_AddPro2;
    UILabel Intensify_LastProName;
    UILabel Intensify_LastProName2;
    UILabel Intensify_Consume;
    Transform Intensify_Item;
    UISlider Intensify_Exp;
    GameObject Intensify_Butten;
    GameObject Intensify_AutoAdd;
    UIPlayTween Intensify_TweenManage;
    /// <summary>
    /// 精炼界面
    /// </summary>
    Transform _Refine;
    UILabel Refine_LevelStart;
    UILabel Refine_LevelEnd;
    UILabel Refine_Pro1Name;
    UILabel Refine_Pro1;
    UILabel Refine_Pro1Add;
    UILabel Refine_Pro2Name;
    UILabel Refine_LastPro1Name;
    UILabel Refine_LastPro2Name;
    UILabel Refine_Pro2;
    UILabel Refine_Pro2Add;
    Transform Refine_Pill;
    UI2DSprite Refine_PillIcon;
    UILabel Refine_PillName;
    UILabel Refine_PillNum;
    Transform Refine_Equip;
    UI2DSprite Refine_EquipIcon;
    UILabel Refine_EquipNum;
    UILabel Refine_EquipName;
    GameObject Refine_BtnRefine;
    /// <summary>
    /// 狗粮选择
    /// </summary>
    Transform _DogFood;
    #endregion

    #region 按钮消息
    void UI_ChangeTab(GameObject go, object obj1, object obj2)
    {
        UI_ChangeUI((TreasureBox.BoxType)obj1);
    }

    void UI_ChangeUI(TreasureBox.BoxType tBox)
    {
        f_GetObject("Introduce").SetActive(tBox == TreasureBox.BoxType.Intro);
        f_GetObject("Intensify").SetActive(tBox == TreasureBox.BoxType.Inten);
        f_GetObject("Refine").SetActive(tBox == TreasureBox.BoxType.Refine);
        f_GetObject("GetWay").SetActive(tBox == TreasureBox.BoxType.GetWay);

        UI_ChangeBtn(f_GetObject("Btn_Introduce"), tBox == TreasureBox.BoxType.Intro);
        UI_ChangeBtn(f_GetObject("Btn_Intensify"), tBox == TreasureBox.BoxType.Inten);
        UI_ChangeBtn(f_GetObject("Btn_Refine"), tBox == TreasureBox.BoxType.Refine);
        UI_ChangeBtn(f_GetObject("Btn_GetWay"), tBox == TreasureBox.BoxType.GetWay);
        switch (tBox)
        {
            case TreasureBox.BoxType.GetWay:
            case TreasureBox.BoxType.Intro:
                UI_OpenIntroduce();
                break;
            case TreasureBox.BoxType.Inten:
                UI_OpenIntensify();
                break;
            case TreasureBox.BoxType.Refine:
                UI_OpenRefine();
                break;
        }
        tmpTrea = new TreasurePoolDT[4];
        tmp.tType = tBox;
    }
    void UI_CloseIntroduce(GameObject go, object obj1, object obj2)
    {
        if (tmp.IsMastr == 1)
        {
            ccUIHoldPool.GetInstance().f_UnHold();
            ccUIHoldPool.GetInstance().f_UnHold();
        }
        else if (tmpBoxType == TreasureBox.BoxType.GetWay)
        {
            ccUIHoldPool.GetInstance().f_UnHold();
            ccUIHoldPool.GetInstance().f_UnHold();
        }
        else
        {
            ccUIHoldPool.GetInstance().f_UnHold();
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureManage, UIMessageDef.UI_CLOSE);

    }
    void UI_OpenIntroduce()
    {
        tmp.tType = TreasureBox.BoxType.Intro;
        UpdateIntroduce();
    }
    void UI_OpenIntensify()
    {
        tmpTrea = new TreasurePoolDT[4];
        NeedMoney = 0;
        tmp.tType = TreasureBox.BoxType.Inten;
        UpdateIntensify();
    }
    void UI_OpenRefine()
    {
        tmp.tType = TreasureBox.BoxType.Refine;
        UpdateRefine();
    }
    void UI_Change(GameObject go, object obj1, object obj2)
    {
        if (UITool.f_CheckHasEquipLeft(LineUpPage.CurrentSelectEquipPart))
        {
            ccUIHoldPool.GetInstance().f_UnHold();
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureManage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectEquipPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            //如果是左右法宝跟普通装备表不一样，表不一样得换
            if (LineUpPage.CurrentSelectEquipPart == EM_EquipPart.eEquipPare_MagicLeft
                || LineUpPage.CurrentSelectEquipPart == EM_EquipPart.eEquipPare_MagicRight)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1782));
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1783));
            }
        }
    }
    void UI_ChangeBtn(GameObject go, bool show)
    {
        go.transform.Find("Up").gameObject.SetActive(show);
        go.transform.Find("Down").gameObject.SetActive(!show);
    }
    void UI_ChangeTreasure(GameObject go, object obj1, object obj2)
    {
        if (m_TreasurePoolDT.m_TreasureDT.iSite == (int)EM_EquipPart.eEquipPare_MagicLeft && tmp.m_TeamPoolDT.m_aTreamPoolDT[(int)EM_EquipPart.eEquipPare_MagicRight - 5] != null)
            m_TreasurePoolDT = tmp.m_TeamPoolDT.m_aTreamPoolDT[(int)EM_EquipPart.eEquipPare_MagicRight - 5];
        else if (m_TreasurePoolDT.m_TreasureDT.iSite == (int)EM_EquipPart.eEquipPare_MagicRight && tmp.m_TeamPoolDT.m_aTreamPoolDT[(int)EM_EquipPart.eEquipPare_MagicLeft - 5] != null)
            m_TreasurePoolDT = tmp.m_TeamPoolDT.m_aTreamPoolDT[(int)EM_EquipPart.eEquipPare_MagicLeft - 5];
        UpdateMain();
        switch (tmp.tType)
        {
            case TreasureBox.BoxType.Intro:
                UpdateIntroduce();
                break;
            case TreasureBox.BoxType.Inten:
                UpdateIntensify();
                break;
            case TreasureBox.BoxType.Refine:
                UpdateRefine();
                break;
        }

    }

    void Btn_DownTreasure(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT OnChangeEquipCallback = new SocketCallbackDT();//更换装备回调
        OnChangeEquipCallback.m_ccCallbackSuc = EquipDownSuc;
        OnChangeEquipCallback.m_ccCallbackFail = EquipDownFail;
        Data_Pool.m_TeamPool.f_ChangeTeamTreasure((long)tmp.m_TeamPoolDT.m_eFormationPos, 0, (byte)tmp.tTreasurePoolDT.m_TreasureDT.iSite, OnChangeEquipCallback);
    }
    #endregion

    void EquipDownSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1784));
        UI_CloseIntroduce(null, null, null);

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    void EquipDownFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1785) + UITool.f_GetError((int)obj));
    }
    /// <summary>
    /// 初始化
    /// </summary>
    void Initialize()
    {
        EffectParent = f_GetObject("Particle").transform;
        ////////
        _EquipCase = f_GetObject("EquipCase").transform;
        Introduce_ProTab = f_GetObject("Introduce").transform.Find("ProTab");
        Introduce_RefineTab = f_GetObject("Introduce").transform.Find("RefineTab");
        Introduce_ParticularsTab = f_GetObject("Introduce").transform.Find("ParticularsTab");
        _Intensify = f_GetObject("Intensify").transform;
        _Refine = f_GetObject("Refine").transform;
        _DogFood = f_GetObject("UpSelection").transform.Find("DodFood/DogFoodItem");
        ///////////////////////////////////////////////////////
        Refine_LevelStart = f_GetObject("Refine_LevelStart").GetComponent<UILabel>();
        Refine_LevelEnd = f_GetObject("Refine_LevelEnd").GetComponent<UILabel>();
        Refine_Pro1Name = f_GetObject("Refine_Pro1Name").GetComponent<UILabel>();
        Refine_LastPro1Name = f_GetObject("Refine_LastPro1Name").GetComponent<UILabel>();
        Refine_LastPro2Name = f_GetObject("Refine_LastPro2Name").GetComponent<UILabel>();
        Refine_Pro1 = f_GetObject("Refine_Pro1").GetComponent<UILabel>();
        Refine_Pro1Add = f_GetObject("Refine_Pro1Add").GetComponent<UILabel>();
        Refine_Pro2Name = f_GetObject("Refine_Pro2Name").GetComponent<UILabel>();
        Refine_Pro2 = f_GetObject("Refine_Pro2").GetComponent<UILabel>();
        Refine_Pro2Add = f_GetObject("Refine_Pro2Add").GetComponent<UILabel>();
        Refine_Pill = f_GetObject("Refine_Pill").transform;
        Refine_PillIcon = Refine_Pill.Find("RefinePill").GetComponent<UI2DSprite>();
        Refine_PillName = Refine_Pill.Find("RefineName").GetComponent<UILabel>();
        Refine_PillNum = Refine_Pill.Find("RefineNum").GetComponent<UILabel>();
        Refine_Equip = f_GetObject("Refine_Equip").transform;
        Refine_EquipIcon = Refine_Equip.Find("RefineEquip").GetComponent<UI2DSprite>();
        Refine_EquipName = Refine_Equip.Find("RefineEquipName").GetComponent<UILabel>();
        Refine_EquipNum = Refine_Equip.Find("RefineEquipNum").GetComponent<UILabel>();
        Refine_BtnRefine = f_GetObject("Refine_BtnRefine");
        ////////////////////////////////////////////////////////
        Intensify_StartLevel = f_GetObject("Intensify_StartLevel").GetComponent<UILabel>();
        Intensify_LastLevel = f_GetObject("Intensify_LastLevel").GetComponent<UILabel>();
        Intensify_ProName = f_GetObject("Intensify_ProName").GetComponent<UILabel>();
        Intensify_LastProName = f_GetObject("Intensify_LastProName").GetComponent<UILabel>();
        Intensify_LastProName2 = f_GetObject("Intensify_LastPro2Name").GetComponent<UILabel>();
        Intensify_Pro = f_GetObject("Intensify_Pro").GetComponent<UILabel>();
        Intensify_AddPro = f_GetObject("Intensify_AddPro").GetComponent<UILabel>();
        Intensify_ProName2 = f_GetObject("Intensify_ProName2").GetComponent<UILabel>();
        Intensify_Pro2 = f_GetObject("Intensify_Pro2").GetComponent<UILabel>();
        Intensify_AddPro2 = f_GetObject("Intensify_AddPro2").GetComponent<UILabel>();
        Intensify_Consume = f_GetObject("Intensify_Consume").GetComponent<UILabel>();
        Intensify_Butten = f_GetObject("Intensify_Butten");
        Intensify_AutoAdd = f_GetObject("Intensify_AutoAdd").gameObject;
        Intensify_Item = f_GetObject("Intensify_Item").transform;
        Intensify_Exp = f_GetObject("Intensify_Exp").GetComponent<UISlider>();
        Intensify_TweenManage = f_GetObject("TweenManage").GetComponent<UIPlayTween>();
        //////////////////////////////////////////////////////////
        ParticularsTab_Label = f_GetObject("ParticularsTab_Label").GetComponent<UILabel>();
        ///////////////////////////////////////////////////////////////////////////
        RefineTab_Level = f_GetObject("RefineTab_Level").GetComponent<UILabel>();
        RefineTab_Pro1 = f_GetObject("RefineTab_Pro1").GetComponent<UILabel>();
        RefineTab_Pro2 = f_GetObject("RefineTab_Pro2").GetComponent<UILabel>();
        RefineTab_Master = f_GetObject("RefineTab_Master").GetComponent<UILabel>();
        RefineTab_Pro1Name = f_GetObject("RefineTab_Pro1Name").GetComponent<UILabel>();
        RefineTab_Pro2Name = f_GetObject("RefineTab_Pro2Name").GetComponent<UILabel>();
        ////////////////////////////////////////////////////////////
        ProTab_Level = f_GetObject("ProTab_Level").GetComponent<UILabel>();
        ProTab_Pro = f_GetObject("ProTab_Pro").GetComponent<UILabel>();
        ProTab_Master1 = f_GetObject("ProTab_Master1").GetComponent<UILabel>();
        ProTab_Master2 = f_GetObject("ProTab_Master2").GetComponent<UILabel>();
        ProTab_ProName = f_GetObject("ProTab_ProName").GetComponent<UILabel>();
        ProTab_ProName2 = f_GetObject("ProTab_ProName2").GetComponent<UILabel>();
        ProTab_Pro2 = f_GetObject("ProTab_Pro2").GetComponent<UILabel>();
        ///////////////////////////////////////////////////////////
        _EquipName = _EquipCase.Find("EquipName").GetComponent<UILabel>();
        _EquipIcon = _EquipCase.Find("Equip").GetComponent<UI2DSprite>();
        _FitOut = _EquipCase.Find("PutOn").GetComponent<UILabel>();
        _Color = _EquipCase.Find("Colour").GetComponent<UILabel>();
        //_Grade = _EquipCase.Find("Character").GetComponent<UILabel>();
        f_RegClickEvent(Intensify_Butten, TreasureInten);
        for (int i = 0; i < Intensify_Item.childCount; i++)
        {
            if (Intensify_Item.GetChild(i).gameObject.activeSelf)
                f_RegClickEvent(Intensify_Item.GetChild(i).gameObject, UI_OpenUpSelection, Intensify_Item.GetChild(i).gameObject);
        }
        f_RegClickEvent(Refine_BtnRefine, Treasure_Refine);
        UpdateIntroduce();
        UpdateRefine();
    }
    /// <summary>
    /// 更新主界面
    /// </summary>
    void UpdateMain()
    {
        /////////////装备于谁  图标   未填写   _FitOut   _EquipIcon//////////////////////
        string name = m_TreasurePoolDT.m_TreasureDT.szName;
        _EquipIcon.transform.GetChild(0).GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(m_TreasurePoolDT.m_TreasureDT.iImportant, ref name);
        _EquipName.text = name;
        _Color.text = UITool.f_GetEquipColur((EM_Important)m_TreasurePoolDT.m_TreasureDT.iImportant);
        _FitOut.text = UITool.f_GetHowEquip(m_TreasurePoolDT.iId);
        _EquipIcon.sprite2D = UITool.f_GetIconSprite(m_TreasurePoolDT.m_TreasureDT.iIcon);
    }
    /// <summary>
    /// 更新信息界面
    /// </summary>
    void UpdateIntroduce()
    {
		m_curMasterLevel = Data_Pool.m_TeamPool.f_GetMasterLevel(EM_Master.TreasureIntensify, m_curFormationPos);
		m_curMasterLevel2 = Data_Pool.m_TeamPool.f_GetMasterLevel(EM_Master.TreasureRefine, m_curFormationPos);
        UpdateMain();
        float _LineSpace = 20;
        float _TabSpace = 40;
        Transform RefineTab = f_GetObject("RefineTab").transform;
        Transform ParticularsTab = f_GetObject("ParticularsTab").transform;
        Transform ProTab = f_GetObject("ProTab").transform;
        ////////信息界面  属性简介/////////////
        ProTab_Level.text = m_TreasurePoolDT.m_lvIntensify.ToString();
        ProTab_ProName.text = UITool.f_GetProName((EM_RoleProperty)m_TreasurePoolDT.m_TreasureDT.iIntenProId1) + ":";
        if (m_TreasurePoolDT.m_TreasureDT.iIntenProId2 == 0)
        {
            ProTab_ProName2.gameObject.SetActive(false);
            ProTab_Pro2.gameObject.SetActive(false);
        }
        else
        {
            ProTab_ProName2.gameObject.SetActive(true);
            ProTab_Pro2.gameObject.SetActive(true);
        }
        ProTab_ProName2.text = UITool.f_GetProName((EM_RoleProperty)m_TreasurePoolDT.m_TreasureDT.iIntenProId2) + ":";
        UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iIntenProId1, ProTab_Pro, UITool.f_GetTreasurePro(m_TreasurePoolDT)[0]);
        UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iIntenProId2, ProTab_Pro2, UITool.f_GetTreasurePro(m_TreasurePoolDT)[1]);
        // ProTab_Master1.text = UITool.f_GetTreasureIntenMaster(m_TreasurePoolDT)[0];
		if(m_curMasterLevel > 0)
		{
ProTab_Master1.text = "Enhanced Master" + string.Format(CommonTools.f_GetTransLanguage(1711), m_curMasterLevel) + ": Attack +" + 80*m_curMasterLevel + ", HP +" + 800*m_curMasterLevel + ", Defense + " + 40*m_curMasterLevel+ ", French room +" + 40*m_curMasterLevel;
		}
		else
		{
			ProTab_Master1.text = "";
		}
        ProTab_Master2.transform.localPosition = new Vector3(-32, ProTab_Master1.text == "" ? 0 : -ProTab_Master1.height - _LineSpace, 0);
        ProTab_Master2.text = UITool.f_GetTreasureIntenMaster(m_TreasurePoolDT)[1];
		
        ////////////////信息界面   精炼简介///////////////////
        if (m_TreasurePoolDT.m_lvRefine > 0)
        {
            RefineTab.localPosition = new Vector3(-32, ProTab.localPosition.y + ProTab_Master1.transform.localPosition.y - (ProTab_Master1.text == "" ? 0 : ProTab_Master1.height) - (ProTab_Master2.text == "" ? 0 : ProTab_Master2.height) -
                _TabSpace - (ProTab_Master1.text == "" && ProTab_Master2.text == "" ? 0 : _LineSpace), 0);
            RefineTab_Level.text = m_TreasurePoolDT.m_lvRefine.ToString();
            RefineTab_Pro1Name.text = UITool.f_GetProName((EM_RoleProperty)m_TreasurePoolDT.m_TreasureDT.iRefinProId1) + ":";
            UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iRefinProId1, RefineTab_Pro1, UITool.f_GetTreasureRefinePro(m_TreasurePoolDT)[0]);

            RefineTab_Pro2Name.text = UITool.f_GetProName((EM_RoleProperty)m_TreasurePoolDT.m_TreasureDT.iRefinProId2) + ":";
            UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iRefinProId2, RefineTab_Pro2, UITool.f_GetTreasureRefinePro(m_TreasurePoolDT)[1]);
            // RefineTab_Master.text = UITool.f_GetTreasureRefineMaster(m_TreasurePoolDT);
			if(m_curMasterLevel2 > 0)
			{
RefineTab_Master.text = "Refined Master" + string.Format(CommonTools.f_GetTransLanguage(1711), m_curMasterLevel2) + ": Increase ST +" + 2*m_curMasterLevel2 + "%, Decrease ST +" + 2*m_curMasterLevel2 + "% , PvP ST gain +" + 1*m_curMasterLevel2 + "%, PvP ST reduction +" + 1*m_curMasterLevel2 + "%";
			}
			else
			{
				RefineTab_Master.text = "";
			}
        }
        RefineTab.gameObject.SetActive(m_TreasurePoolDT.m_lvRefine > 0);
        //////////////////信息界面   说明简介/////////////////////////
        if (m_TreasurePoolDT.m_lvRefine > 0)
            ParticularsTab.transform.localPosition = new Vector3(-32, RefineTab.localPosition.y + RefineTab_Master.transform.localPosition.y - (RefineTab_Master.text == "" ? 0 : RefineTab_Master.height) - _TabSpace, 0);
        else
            ParticularsTab.transform.localPosition = new Vector3(-32, ProTab.localPosition.y + ProTab_Master1.transform.localPosition.y - (ProTab_Master1.text == "" ? 0 : ProTab_Master1.height) - (ProTab_Master2.text == "" ? 0 : ProTab_Master2.height) -
                _TabSpace - (ProTab_Master1.text == "" && ProTab_Master2.text == "" ? 0 : _LineSpace), 0);
        ParticularsTab_Label.text = m_TreasurePoolDT.m_TreasureDT.szDescribe;
    }
    /// <summary>
    /// 更新强化界面
    /// </summary>
    void UpdateIntensify()
    {
        ///////////////升级界面/////////////////////
        TreasureUpConsumeDT tConsumeDT = UITool.f_GetTreasureUpDT(m_TreasurePoolDT);
        Intensify_StartLevel.text = string.Format("{0}", m_TreasurePoolDT.m_lvIntensify);
        if (m_TreasurePoolDT.m_lvIntensify + 1 > UITool.f_GetTreasureIntenManx(m_TreasurePoolDT))
            Intensify_LastLevel.text = "Max";
        else
            Intensify_LastLevel.text = string.Format("{0}", m_TreasurePoolDT.m_lvIntensify + 1);
        Intensify_LastProName.text = Intensify_ProName.text = UITool.f_GetProName((EM_RoleProperty)m_TreasurePoolDT.m_TreasureDT.iIntenProId1) + ":";
        UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iIntenProId1, Intensify_Pro, UITool.f_GetTreasurePro(m_TreasurePoolDT)[0]);
        UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iIntenProId1, Intensify_AddPro, m_TreasurePoolDT.m_TreasureDT.iAddPro1);
        f_GetObject("Intensify_ProName2").SetActive(UITool.f_GetTreasurePro(m_TreasurePoolDT)[1] != 0);
        Intensify_LastProName2.text = Intensify_ProName2.text = UITool.f_GetProName((EM_RoleProperty)m_TreasurePoolDT.m_TreasureDT.iIntenProId2) + ":";
        UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iIntenProId2, Intensify_Pro2, UITool.f_GetTreasurePro(m_TreasurePoolDT)[1]);
        UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iIntenProId2, Intensify_AddPro2, m_TreasurePoolDT.m_TreasureDT.iAddPro2);
        Intensify_Consume.text = NeedMoney.ToString();
        if (tConsumeDT != null)
        {
            Intensify_Exp.gameObject.SetActive(true);
            Intensify_Exp.value = (float)m_TreasurePoolDT.m_ExpIntensify / (float)(UITool.f_GetTreasureUpDT(m_TreasurePoolDT).iIntensifyExp);
            Intensify_Exp.transform.Find("Num").GetComponent<UILabel>().text = string.Format("{0}/{1}", m_TreasurePoolDT.m_ExpIntensify, UITool.f_GetTreasureUpDT(m_TreasurePoolDT).iIntensifyExp);
        }
        else
        {
            Intensify_Exp.gameObject.SetActive(false);
        }
        for (int i = 0; i < Intensify_Item.childCount - 1; i++)
        {
            Intensify_Item.GetChild(i).transform.GetChild(0).GetComponent<UI2DSprite>().sprite2D = null;
            Intensify_Item.GetChild(i).transform.GetChild(1).GetComponent<UISprite>().spriteName = "Icon_Blue";
            Intensify_Item.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    void UpdateAddPro(int ProId, UILabel Label, int AddPro)
    {
        UITool.f_UpdateAddPro(ProId, Label, AddPro);
    }
    /// <summary>
    /// 更新精炼界面
    /// </summary>
    void UpdateRefine()
    {
        /////////////////////////精炼界面///////////////////////////////
        TreasureRefineConsumeDT tmpTrea = UITool.f_GetTreasureRefinePillNum(m_TreasurePoolDT);
        UILabel Consume_Money = f_GetObject("Consume_Money").GetComponent<UILabel>();
        if (m_TreasurePoolDT.m_lvRefine + 1 > 20)
            Refine_LevelEnd.text = "Max";
        else
            Refine_LevelEnd.text = string.Format("{0}/{1}", m_TreasurePoolDT.m_lvRefine + 1, 20);
        Refine_LevelStart.text = string.Format("{0}/{1}", m_TreasurePoolDT.m_lvRefine, 20);
        Refine_LastPro1Name.text = Refine_Pro1Name.text = UITool.f_GetProName((EM_RoleProperty)m_TreasurePoolDT.m_TreasureDT.iRefinProId1) + ":";
        Refine_LastPro2Name.text = Refine_Pro2Name.text = UITool.f_GetProName((EM_RoleProperty)m_TreasurePoolDT.m_TreasureDT.iRefinProId2) + ":";
        UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iRefinProId1, Refine_Pro1, UITool.f_GetTreasureRefinePro(m_TreasurePoolDT)[0]);
        UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iRefinProId2, Refine_Pro2, UITool.f_GetTreasureRefinePro(m_TreasurePoolDT)[0]);
        UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iRefinProId1, Refine_Pro1Add, m_TreasurePoolDT.m_TreasureDT.iRefinPro1);
        UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iRefinProId2, Refine_Pro2Add, m_TreasurePoolDT.m_TreasureDT.iRefinPro2);

        if (tmpTrea == null)
        {
            f_GetObject("Consume").SetActive(false);
            return;
        }
        else
            f_GetObject("Consume").SetActive(true);


        string name = UITool.f_GetGoodsName(tmpTrea.iRefinePillId);
        Refine_Pill.GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(((BaseGoodsDT)glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(tmpTrea.iRefinePillId)).iImportant, ref name);
        UITool.f_SetIconSprite(Refine_PillIcon, EM_ResourceType.Good, tmpTrea.iRefinePillId);
        Refine_PillName.text = name;
        if (UITool.f_GetGoodsNum(tmpTrea.iRefinePillId) >= tmpTrea.iRefineNum)
            Refine_PillNum.text = string.Format("{0}/{1}", UITool.f_GetGoodsNum(tmpTrea.iRefinePillId), tmpTrea.iRefineNum);
        else
            Refine_PillNum.text = string.Format("[ff0000]{0}/{1}[-]", UITool.f_GetGoodsNum(tmpTrea.iRefinePillId), tmpTrea.iRefineNum);
        Consume_Money.text = tmpTrea.iGoldNum < Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money) ? tmpTrea.iGoldNum.ToString() : string.Format("[ff0000]{0}[-]", tmpTrea.iGoldNum);
        if (tmpTrea.iTreasureNum == 0)
            Refine_Equip.gameObject.SetActive(false);
        else
        {
            string Treasurename = m_TreasurePoolDT.m_TreasureDT.szName;
            Refine_Equip.gameObject.SetActive(true);
            Refine_Equip.GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(m_TreasurePoolDT.m_TreasureDT.iImportant, ref Treasurename);
            Refine_EquipName.text = Treasurename;

            Refine_EquipIcon.sprite2D = UITool.f_GetIconSprite(m_TreasurePoolDT.m_TreasureDT.iIcon);
            int treasureNum = UITool.f_GetTreasureNum(m_TreasurePoolDT);
            if (treasureNum < tmpTrea.iTreasureNum)
                Refine_EquipNum.text = string.Format("[ff0000]{0}/{1}[-]", treasureNum, tmpTrea.iTreasureNum);
            else
                Refine_EquipNum.text = string.Format("{0}/{1}", treasureNum, tmpTrea.iTreasureNum);
        }
    }


    /// <summary>
    /// 更新升级选择界面
    /// </summary>
    void UpdateUpSelection(GameObject DogFoodSele = null)
    {
        this.DogFoodSele = DogFoodSele;
        TreasurePoolDT item = new TreasurePoolDT();
        tbasepool.Clear();
        int _num = 0;
        for (int i = 0; i < Data_Pool.m_TreasurePool.f_GetAll().Count; i++)
        {
            item = Data_Pool.m_TreasurePool.f_GetAll()[i] as TreasurePoolDT;
            if (item.iId == m_TreasurePoolDT.iId)
            {
                continue;
            }
            item.m_iTempNum = item.m_Num;

            if (item.m_iTempNum <= 0)
            {
                continue;
            }
            //橙色以下    纯净  经验法宝  没有装备
            if ((item.m_TreasureDT.iImportant == (int)EM_Important.Blue
                || item.m_TreasureDT.iImportant == (int)EM_Important.Green
               || item.m_TreasureDT.iImportant == (int)EM_Important.Purple
               || item.m_TreasureDT.iId == 5002
               || item.m_TreasureDT.iId == 5003
               || item.m_TreasureDT.iId == 5001
               || item.m_TreasureDT.iId == 5000
               || item.m_TreasureDT.iImportant == (m_TreasurePoolDT.m_TreasureDT.iImportant == (int)EM_Important.Red ? (int)EM_Important.Oragen : -99))
               && item.m_lvIntensify == 1 && item.m_lvRefine == 0 && item.m_ExpIntensify == 0
               && !Data_Pool.m_TeamPool.f_GetIsEquip((EM_EquipPart)item.m_TreasureDT.iSite, item.iId))
            {
                //遍历的当前法宝已经在选择界面中
                if (Array.IndexOf(tmpTrea, item) >= 0)
                {    //已经选择的法宝中有经验石道具
                    if (item.m_TreasureDT.iId == 5002 || item.m_TreasureDT.iId == 5003
                        || item.m_TreasureDT.iId == 5001 || item.m_TreasureDT.iId == 5000)
                    {
                        //判断有几个用来扣除
                        for (int j = 0; j < tmpTrea.Length; j++)
                            if (tmpTrea[j] == item)
                                _num++;

                        //如果当前拥有的大于已经选择的经验法宝就添加进去 
                        if (tmpTrea[Array.IndexOf(tmpTrea, item)].m_iTempNum - _num > 0)
                        {
                            tbasepool.Add(item);
                            continue;
                        }
                        else
                            continue;
                    }
                    else   //含有的 并且不是经验法宝的就下一个
                    {
                        continue;
                    }
                }
                //没有含有的就下一个
                tbasepool.Add(item);
            }
        }
    }
    GameObject DogFoodSele;
    private void UpdateSelect(Transform item, BasePoolDT<long> dt)
    {
        TreasurePoolDT treasure = dt as TreasurePoolDT;

        int _num = 0;
        if (Array.IndexOf(tmpTrea, treasure) >= 0)
        {
            for (int i = 0; i < tmpTrea.Length; i++)
                if (tmpTrea[i] == treasure)
                    _num++;
            if (treasure.m_iTempNum > 1)
                item.Find("DogFoodNum").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1786), treasure.m_iTempNum - _num);
            else
                item.Find("DogFoodNum").GetComponent<UILabel>().text = null;
        }
        else
        {
            if (treasure.m_iTempNum > 1)
                item.Find("DogFoodNum").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1786), treasure.m_iTempNum);
            else
                item.Find("DogFoodNum").GetComponent<UILabel>().text = null;
        }
        item.Find("DogFoodCase").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(treasure.m_TreasureDT.iImportant);

        item.Find("DogFoodIcon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(treasure.m_TreasureDT.iIcon);
        item.Find("DogFoodName").GetComponent<UILabel>().text = UITool.f_GetImporentForName(treasure.m_TreasureDT.iImportant, treasure.m_TreasureDT.szName);
        item.Find("DogFoodLv/Lv").GetComponent<UILabel>().text = treasure.m_lvIntensify.ToString();
        item.Find("DogFoodExp/Exp").GetComponent<UILabel>().text = treasure.m_TreasureDT.iExp.ToString();
        //f_RegClickEvent(item.gameObject, UpSelection, DogFoodSele, treasure);

    }
    private void UpdateCklic(Transform item, BasePoolDT<long> dt)
    {

        DogFoodSele.transform.GetChild(0).GetComponent<UI2DSprite>().sprite2D =
            UITool.f_GetIconSprite(((TreasurePoolDT)dt).m_TreasureDT.iIcon);
        DogFoodSele.transform.GetChild(1).GetComponent<UISprite>().spriteName =
            UITool.f_GetImporentCase(((TreasurePoolDT)dt).m_TreasureDT.iImportant);
        DogFoodSele.transform.GetChild(2).gameObject.SetActive(false);

        if (tmpTrea[int.Parse(DogFoodSele.name.Substring(6, 1)) - 1] != null)
            NeedMoney -= tmpTrea[int.Parse(DogFoodSele.name.Substring(6, 1)) - 1].m_TreasureDT.iExp;
        tmpTrea[int.Parse(DogFoodSele.name.Substring(6, 1)) - 1] = (TreasurePoolDT)dt;
        NeedMoney += ((TreasurePoolDT)dt).m_TreasureDT.iExp;
        if (NeedMoney > Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Money))
            Intensify_Consume.text = string.Format("[ff0000]{0}[-]", NeedMoney);
        else
            Intensify_Consume.text = string.Format("{0}", NeedMoney);

        f_GetObject("UpSelection").SetActive(false);

        UI_ShowLastLevel();
    }
    /// <summary>
    /// 自动添加
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    void UI_AutoAdd(GameObject go, object obj1, object obj2)
    {
        DogFoodList = Data_Pool.m_TreasurePool.f_GetAll();
        DogFoodList.Sort((BasePoolDT<long> a, BasePoolDT<long> b) =>
        {
            if ((a as TreasurePoolDT).m_TreasureDT.iList > (b as TreasurePoolDT).m_TreasureDT.iList)
                return -1;
            else
                return 1;
        });
        tmpTrea = new TreasurePoolDT[4];
        TreasurePoolDT item = new TreasurePoolDT();
        int j = 0;   //狗粮的index
        NeedMoney = 0;

        for (int i = 0; i < 4; i++)
        {
            Intensify_Item.GetChild(i).GetChild(0).GetComponent<UI2DSprite>().sprite2D = null;
            Intensify_Item.GetChild(i).GetChild(1).GetComponent<UISprite>().spriteName = "Icon_Blue";
            Intensify_Item.GetChild(i).GetChild(2).gameObject.SetActive(false);
            Intensify_Item.GetChild(i).GetChild(2).gameObject.SetActive(true);
        }

        for (int i = 0; i < DogFoodList.Count; i++)
        {
            if (j >= tmpTrea.Length)
                break;
            item = DogFoodList[i] as TreasurePoolDT;
            if (item.iId == m_TreasurePoolDT.iId)
            {
                continue;
            }
            item.m_iTempNum = item.m_Num;
            //(蓝色  绿色    经验石  )  && 纯净卡牌  &&没有装备
            if ((item.m_TreasureDT.iImportant == (int)EM_Important.Blue
                || item.m_TreasureDT.iImportant == (int)EM_Important.Green
                || item.m_TreasureDT.iId == 5002
                || item.m_TreasureDT.iId == 5003
                || item.m_TreasureDT.iId == 5001
                || item.m_TreasureDT.iId == 5000)
                && item.m_lvIntensify == 1 && item.m_lvRefine == 0 && item.m_ExpIntensify == 0
                && !Data_Pool.m_TeamPool.f_GetIsEquip((EM_EquipPart)item.m_TreasureDT.iSite, item.iId))
            {
                //经验法宝    数量-1
                for (int TreasureIndx = 0; TreasureIndx < item.m_iTempNum; TreasureIndx++)
                {
                    if (j >= tmpTrea.Length)
                        break;
                    tmpTrea[j] = item;
                    Intensify_Item.GetChild(j).GetChild(0).GetComponent<UI2DSprite>().sprite2D =
                        UITool.f_GetIconSprite(tmpTrea[j].m_TreasureDT.iIcon);
                    Intensify_Item.GetChild(j).GetChild(1).GetComponent<UISprite>().spriteName =
                        UITool.f_GetImporentCase(tmpTrea[j].m_TreasureDT.iImportant);
                    Intensify_Item.GetChild(j).GetChild(2).gameObject.SetActive(false);
                    NeedMoney += tmpTrea[j].m_TreasureDT.iExp;
                    j += 1;
                }
                item.m_iTempNum -= 1;
                //经验法宝  数量大于0   就继续用这一个
            }
        }
        if (NeedMoney >= Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Money))
            Intensify_Consume.text = string.Format("[ff0000]{0}[-]", NeedMoney);
        else
            Intensify_Consume.text = string.Format("{0}", NeedMoney);
        if (tmpTrea[0] == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1787));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Treasure, 5000, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
        }
        else
        {
            UI_ShowLastLevel();
        }
    }
    private void UI_ShowLastLevel()
    {


        _TmpTreasureExp = m_TreasurePoolDT.m_ExpIntensify;
        _TmpTreasureLevel = m_TreasurePoolDT.m_lvIntensify + 1;


        _TmpTreasureExp += NeedMoney;
        while (true)
        {
            //取的不对  
            _TmpUpConsume = UITool.f_GetTreasureUpDT(m_TreasurePoolDT.m_TreasureDT.iImportant, _TmpTreasureLevel);
            if (_TmpUpConsume == null)
            {
                UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iIntenProId1, Intensify_AddPro, m_TreasurePoolDT.m_TreasureDT.iAddPro1 * (_TmpTreasureLevel - m_TreasurePoolDT.m_lvIntensify - 1));
                UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iIntenProId2, Intensify_AddPro2, m_TreasurePoolDT.m_TreasureDT.iAddPro2 * (_TmpTreasureLevel - m_TreasurePoolDT.m_lvIntensify - 1));
                Intensify_AddPro.text = "+" + Intensify_AddPro.text;
                Intensify_AddPro2.text = "+" + Intensify_AddPro2.text;
                Intensify_LastLevel.text = "[2DF233]" + (_TmpTreasureLevel - 1).ToString();
                Intensify_Exp.value = 1;
                //Intensify_Exp.transform.Find("Num").GetComponent<UILabel>().text = string.Format("{0}/{1}", _TmpTreasureExp, _TmpUpConsume.iIntensifyExp);
                break;
            }

            if (_TmpUpConsume.iIntensifyExp <= _TmpTreasureExp)
            {
                _TmpTreasureLevel++;
                _TmpTreasureExp -= _TmpUpConsume.iIntensifyExp;
            }
            else
            {
                UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iIntenProId1, Intensify_AddPro, m_TreasurePoolDT.m_TreasureDT.iAddPro1 * (_TmpTreasureLevel - m_TreasurePoolDT.m_lvIntensify - 1));
                UpdateAddPro(m_TreasurePoolDT.m_TreasureDT.iIntenProId2, Intensify_AddPro2, m_TreasurePoolDT.m_TreasureDT.iAddPro2 * (_TmpTreasureLevel - m_TreasurePoolDT.m_lvIntensify - 1));
                Intensify_AddPro.text = "+" + Intensify_AddPro.text;
                Intensify_AddPro2.text = "+" + Intensify_AddPro2.text;
                Intensify_LastLevel.text = "[2DF233]" + (_TmpTreasureLevel - 1).ToString();
                Intensify_Exp.value = (float)_TmpTreasureExp / (float)_TmpUpConsume.iIntensifyExp;
                Intensify_Exp.transform.Find("Num").GetComponent<UILabel>().text = string.Format("{0}/{1}", _TmpTreasureExp, _TmpUpConsume.iIntensifyExp);
                break;
            }

        }
    }


    void UI_OpenUpSelection(GameObject go, object obj1, object obj2)
    {
        UpdateUpSelection((GameObject)obj1);


        if (tbasepool.Count != 0)
            f_GetObject("UpSelection").SetActive(true);
        else
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1788));


        tbasepool.Sort((BasePoolDT<long> a, BasePoolDT<long> b) => { return (a as TreasurePoolDT).m_TreasureDT.iList > (b as TreasurePoolDT).m_TreasureDT.iList ? -1 : 1; });
        if (_treasure == null)
            _treasure = new UIWrapComponent(177, 1, 748, 5, f_GetObject("DogFoodParent"), f_GetObject("DogFoodItem"), tbasepool, UpdateSelect, UpdateCklic);
        else
        {
            _treasure.f_UpdateList(tbasepool);
            _treasure.f_ResetView();
            _treasure.f_UpdateView();
        }
    }
    void UI_CloseUpSelection(GameObject go, object obj1, object obj2)
    {
        f_GetObject("UpSelection").SetActive(false);
    }
    int NeedMoney = 0;
    /// <summary>
    /// 升级选择
    /// </summary>
    void UpSelection(GameObject go, object obj1, object obj2)
    {
        ((GameObject)obj1).transform.GetChild(0).GetComponent<UI2DSprite>().sprite2D =
            UITool.f_GetIconSprite(((TreasurePoolDT)obj2).m_TreasureDT.iIcon);
        tmpTrea[int.Parse(((GameObject)obj1).name.Substring(6, 1)) - 1] = (TreasurePoolDT)obj2;
        NeedMoney += ((TreasurePoolDT)obj2).m_TreasureDT.iExp;
        if (NeedMoney > Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Money))
            Intensify_Consume.text = string.Format("[ff0000]{0}[-]", NeedMoney);
        else
            Intensify_Consume.text = string.Format("{0}", NeedMoney);
        f_GetObject("UpSelection").SetActive(false);
    }
    /// <summary>
    /// 法宝精炼
    /// </summary>
    void Treasure_Refine(GameObject go, object obj1, object obj2)
    {
        TreasureRefineConsumeDT tmp = UITool.f_GetTreasureRefinePillNum(m_TreasurePoolDT);

        if (tmp == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1789));
            return;
        }
        if (tmp.iGoldNum > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1790));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        if (tmp.iRefineNum > UITool.f_GetGoodsNum(tmp.iRefinePillId))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1791));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, tmp.iRefinePillId, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        if (tmp.iTreasureNum > UITool.f_GetTreasureNum(m_TreasurePoolDT))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1792));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT Callback = new SocketCallbackDT();
        Callback.m_ccCallbackFail = RefineFail;
        Callback.m_ccCallbackSuc = RefineSuc;
        Data_Pool.m_TreasurePool.f_Refine(m_TreasurePoolDT.iId, Callback);
    }
    /// <summary>
    /// 精炼成功
    /// </summary>
    void RefineSuc(object data)
    {
        CheckRedPoint();
        UITool.f_OpenOrCloseWaitTip(false);
        Refine_Effect = UITool.f_CreateEffect_Old(UIEffectName.zr_fabaojinglian_01, EffectParent, new Vector3(-0.84f, -0.06f, 0), 0.2f, 2.13f, UIEffectName.UIEffectAddress1);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1793));
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.TreasureRfine);
        UpdateRefine();

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    /// <summary>
    /// 精炼失败
    /// </summary>
    /// <param name="data"></param>
    void RefineFail(object data)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(UITool.f_GetError((int)data));
    }

    /// <summary>
    /// 法宝强化
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    void TreasureInten(GameObject go, object obj1, object obj2)
    {
        if (m_TreasurePoolDT.m_lvIntensify >= UITool.f_GetTreasureIntenManx(m_TreasurePoolDT))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1794));
            return;
        }

        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money) < NeedMoney)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1795));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        long[] tmpLong = new long[4];
        bool IsHavePupr = false;
        for (int i = 0; i < tmpLong.Length; i++)
        {
            if (tmpTrea[i] != null)
            {
                tmpLong[i] = tmpTrea[i].iId;
                if (tmpTrea[i].m_TreasureDT.iImportant >= (int)EM_Important.Purple)
                {
                    if (tmpTrea[i].m_TreasureDT.iId == 5000
                     || tmpTrea[i].m_TreasureDT.iId == 5002
                     || tmpTrea[i].m_TreasureDT.iId == 5003
                     || tmpTrea[i].m_TreasureDT.iId == 5001) { }
                    else
                        IsHavePupr = true;
                }
            }
            else
                tmpLong[i] = 0;
        }
        int j = 0;
        for (int i = 0; i < tmpLong.Length; i++)
        {
            if (tmpLong[i] == 0)
                j++;
            if (j == 4)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1796));
                return;
            }
        }
        if (!IsHavePupr)
        {
            _SendTreasureInten(tmpLong);
        }
        else
        {
            PopupMenuParams tPopupMenuPageParm = new PopupMenuParams(CommonTools.f_GetTransLanguage(1797), CommonTools.f_GetTransLanguage(1798), CommonTools.f_GetTransLanguage(1799), _SendTreasureInten, CommonTools.f_GetTransLanguage(1800), _ClosePopupMenu, tmpLong);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tPopupMenuPageParm);
        }
    }
    private void _SendTreasureInten(object obj1)
    {
        long[] tmpLong = (long[])obj1;
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tmp = new SocketCallbackDT();
        tmp.m_ccCallbackSuc = IntenSuc;
        tmp.m_ccCallbackFail = IntenFill;
        Data_Pool.m_TreasurePool.f_Intensify(m_TreasurePoolDT.iId, tmpLong[0],
            tmpLong[1], tmpLong[2], tmpLong[3], 0, tmp);
        tmpLong = new long[5];
        tmpTrea = new TreasurePoolDT[4];
    }
    private void _ClosePopupMenu(object obj1)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_CLOSE);
    }

    void IntenSuc(object data)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        Inten_Effect = UITool.f_CreateEffect_Old(UIEffectName.zr_fabaoqianghua_01, EffectParent, new Vector3(-0.84f, -0.06f, 0), 0.2f, 2.09f, UIEffectName.UIEffectAddress1);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1801));
        tmpTrea = new TreasurePoolDT[4];
        NeedMoney = 0;
        UpdateIntensify();
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.TreasureInten);
        //强化完成更新红点
        CheckRedPoint();

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }

    void IntenFill(object data)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        NeedMoney = 0;
        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1802), UITool.f_GetError((int)data)));
        UpdateIntensify();
    }
    #region 图鉴打开
    /// <summary>
    /// 图鉴打开
    /// </summary>
    private void UI_OpenHandbook()
    {
        GetWayTools.ShowContent(f_GetObject("GetWayScrollview"), f_GetObject("GetWay_ItemParent"), f_GetObject("GetWay_Item"),
            new GetWayPageParam(EM_ResourceType.Treasure, m_TreasurePoolDT.m_TreasureDT.iId, null), this);
    }
    /// <summary>
    /// 页面关闭
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        StaticValue.mGetWayToBattleParam.f_Empty();
    }
    /// <summary>
    /// UI UnHold
    /// </summary>
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        GetWayTools.ShowContent();
    }
    #endregion
}
public struct TreasureBox : Box
{
    public TreasurePoolDT tTreasurePoolDT;
    public BoxType tType;
    public int IsShowChange;    //为0显示   其他都不显示
    public TeamPoolDT m_TeamPoolDT;
    public byte IsMastr;   //是否在大师界面打开    1为是
    public byte IsSelectAward;  //是否在选择奖励打开    0为否 其他是真

    public enum BoxType
    {
        Intro,  //信息
        Inten,  //强化
        Refine,  //精炼
        GetWay,  //获取途径
    }
}
