using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;

public class GodEquipManage : UIFramwork
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
    GodEquipPoolDT _EquipPool;
    int RefinePillIndex;   //选择第几个精炼石
    int[] Tempid = new int[4];    //精炼石的ID
    GodEquipBox tmpEquip;
    //特效
    Transform EffectParent;
    GameObject Inten_Effect;
    GameObject Refine_GoodsEffect;
    GameObject Refine_EquipEffect;
    GameObject UpStar_EquipEffect;
    //计时器
    int Time_SendRefine = 1;  //发送精炼信息的TimeId
    int Time_ClickRefine = 2;   //模拟循环按钮的TimeId
    int Time_ClickRefineInvokeTime = 3; //延时模拟循环按钮TimeId
    int Time_SedUpStar = 4;
    int _UpStarType = 0;

    GameObject oModel = null;

    private bool bReturnBtn = false;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        tmpEquip = (GodEquipBox)e;
        _EquipPool = tmpEquip.tEquipPoolDT;
        RefinePillIndex = 0;
        Tempid = UITool.f_GetGoodsForEffect(EM_GoodsEffect.GodEquipRefineExp);
        Initialize();
        f_Message();
        UI_ChangeUI(tmpEquip.tType);
        UI_ShowBtn();
        UpdateMainStar();
        tmpIntStar = _EquipPool.m_sstars;
        f_GetObject("Btn").GetComponent<UIGrid>().Reposition();
        CheckRedPoint();
    }

    protected override void f_Create()
    {
        _InitReference();
        base.f_Create();
    }

    private void _InitReference()
    {
        //f_Message
        AddGOReference("Panel/Center/Btn");
        AddGOReference("Panel/Center/EquipCase/Down");
        AddGOReference("Panel/Anchor-TopLeft/Btn_Back");
        AddGOReference("Panel/Center/Btn/Btn_GetWay");
        AddGOReference("Panel/Center/Btn/Btn_Intensify");
        AddGOReference("Panel/Center/Btn/Btn_Refine");
        AddGOReference("Panel/Center/Btn/Btn_Introduce");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Consume/Material/Grid/Refine_Prop1");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Consume/Material/Grid/Refine_Prop2");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Consume/Material/Grid/Refine_Prop3");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Consume/Material/Grid/Refine_Prop4");
        AddGOReference("Panel/Center/Btn/Btn_UpStars");
        AddGOReference("Panel/Center/EquipCase/Chage");
        AddGOReference("Panel/Center/Btn/Btn_EquipSet");
        AddGOReference("Panel/Center/Btn_Left");
        AddGOReference("Panel/Center/Btn_Right");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/UpStar_UpOptions/GoldUp");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/UpStar_UpOptions/MoneyUp");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/UpStar_UpOptions/FragmentUp");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/OneKeyRefine");
        //Initialize
        AddGOReference("Panel/Center/Particle");
        AddGOReference("Panel/Center/EquipCase");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce");
        AddGOReference("Panel/Center/Ui_Introduce/Intensify");
        AddGOReference("Panel/Center/Ui_Introduce/Refine");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars");
        AddGOReference("Panel/Center/Ui_Introduce/GetWay");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/UpStar_EndStarNum");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/UpStar_StarExp");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/UpStar_StarExp/UpStar_StarNeedExp");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/StartPro/UpStar_StartProName");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/StartPro/UpStar_StartProName/UpStar_StarPro");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/StartPro/UpStar_StartProName/UpStar_NowStarpro");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/UpStar_EndStarNum/EndProName/UpStar_EndPro");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/StartPro/ExtraAdd/UpStar_AddPro");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/SuccessRate/UpStar_SucRate");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/Lucky/UpStar_Lucky");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/UpStar_UpOptions");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/UpStar_BtnUp");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/UpStar_BtnAuto");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/RefineLv/Refine_LevelStart");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/RefineLv/Refine_LevelEnd");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Exp/Refine_Exp");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Refine_Pro1Name");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Refine_Pro1Name/Refine_Pro1");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Refine_Pro1Name/Refine_Pro1Add");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Refine_Pro2Name");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Refine_Pro2Name/Refine_Pro2");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Refine_Pro2Name/Refine_Pro2Add");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Exp/Refine_ShowExp");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Consume/Material/Grid/Refine_Prop1/Refine_Prop1Num");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Consume/Material/Grid/Refine_Prop2/Refine_Prop2Num");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Consume/Material/Grid/Refine_Prop3/Refine_Prop3Num");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/Consume/Material/Grid/Refine_Prop4/Refine_Prop4Num");
        AddGOReference("Panel/Center/Ui_Introduce/Refine/EffectPos");
        AddGOReference("Panel/Center/Ui_Introduce/Intensify/IntensifyLv/Intensify_StartLevel");
        AddGOReference("Panel/Center/Ui_Introduce/Intensify/IntensifyLv/Intensify_LastLevel");
        AddGOReference("Panel/Center/Ui_Introduce/Intensify/Intensify_ProName/Intensify_ProRight");
        AddGOReference("Panel/Center/Ui_Introduce/Intensify/Intensify_ProName");
        AddGOReference("Panel/Center/Ui_Introduce/Intensify/Intensify_ProName/Intensify_Pro");
        AddGOReference("Panel/Center/Ui_Introduce/Intensify/Intensify_ProName/Intensify_AddPro");
        AddGOReference("Panel/Center/Ui_Introduce/Intensify/Consume/Intensify_Consume");
        AddGOReference("Panel/Center/Ui_Introduce/Intensify/Intensify_Butten");
        AddGOReference("Panel/Center/Ui_Introduce/Intensify/Intensify_Butten5");
        AddGOReference("Panel/Center/Ui_Introduce/Intensify/IntensifyLv/Intensify_LastLeveLeft");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/ParticularsTab/ParticularsTab_Label");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/RefineTab/Level/RefineTab_Level");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/RefineTab/RefineTab_Pro1Name/RefineTab_Pro1");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/RefineTab/RefineTab_Pro2Name/RefineTab_Pro2");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/RefineTab/RefineTab_Master");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/RefineTab/RefineTab_Pro1Name");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/RefineTab/RefineTab_Pro2Name");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/ProTab/Level/ProTab_Level");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/ProTab/ProTab_ProName/ProTab_Pro");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/ProTab/ProTab_Master1");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/ProTab/ProTab_Master1/ProTab_Master2");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/ProTab/ProTab_ProName");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/ProTab/SprNorAttackBg");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/ProTab/SprNorAttackBg/SkillName");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/ProTab/SprNorAttackBg/SkillIcon");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/ProTab/SprNorAttackBg/SkillDes");

        AddGOReference("Panel/Center/EquipCase/_EquipName");
        AddGOReference("Panel/Center/EquipCase/_EquipIcon");
        AddGOReference("Panel/Center/EquipCase/_FitOut");
        AddGOReference("Panel/Center/EquipCase/_Color");

        AddGOReference("Panel/Center/Ui_Introduce/UpStars/StarGrid5");
        AddGOReference("Panel/Center/Ui_Introduce/UpStars/StarGrid3");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/RefineTab");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/ParticularsTab");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/SetEuqipTab");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/SetEuqipTab/Equip/SetEquipTab_Body");
        AddGOReference("Panel/Center/Ui_Introduce/Introduce/SetEuqipTab/SetEquipPro");
        AddGOReference("Panel/Center/EquipCase/Model");

    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        GetWayTools.ShowContent();
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        SendRefine(null);
        StaticValue.mGetWayToBattleParam.f_Empty();
        if (tmpEquip.oType == GodEquipBox.OpenType.Master)
        {
            ccUIHoldPool.GetInstance().f_UnHold();
        }
        else if (tmpEquip.oType == GodEquipBox.OpenType.SelectAward)
        {
            ccUIHoldPool.GetInstance().f_UnHold();
        }
    }
    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
    }
    protected void f_Message()
    {
        f_RegClickEvent("Btn_Introduce", UI_ChangeTab, EquipBox.BoxTye.Intro);
        f_RegClickEvent("Btn_GetWay", UI_ChangeTab, EquipBox.BoxTye.GetWay);
        f_RegClickEvent("Btn_Intensify", UI_ChangeTab, EquipBox.BoxTye.Inten);
        f_RegClickEvent("Btn_Refine", UI_ChangeTab, EquipBox.BoxTye.Refine);
        f_RegClickEvent(Intensify_Butten, EquipInten);
        f_RegClickEvent(Intensify_Butten5, EquipInten5);
        f_RegPressEvent("Refine_Prop1", Equip_Refine, Data_Pool.m_BaseGoodsPool.f_GetForData5(Tempid[0]));
        f_RegPressEvent("Refine_Prop2", Equip_Refine, Data_Pool.m_BaseGoodsPool.f_GetForData5(Tempid[1]));
        f_RegPressEvent("Refine_Prop3", Equip_Refine, Data_Pool.m_BaseGoodsPool.f_GetForData5(Tempid[2]));
        f_RegPressEvent("Refine_Prop4", Equip_Refine, Data_Pool.m_BaseGoodsPool.f_GetForData5(Tempid[3]));
        f_RegClickEvent("Btn_Back", UI_CloseIntroduce);
        f_RegClickEvent("Btn_UpStars", UI_ChangeTab, EquipBox.BoxTye.UpStar);
        f_RegClickEvent(UpStar_BtnUp, Equip_UpStar);
        f_RegClickEvent(UpStar_BtnAuto, Equip_UpStarAuto);
        f_RegClickEvent("Down", UI_DownEquip);
        f_RegClickEvent("Chage", UI_OpenSele);
        f_RegClickEvent("Btn_EquipSet", UI_ChangeTab);
        f_RegClickEvent("Btn_Left", UI_ChageEquip, -1);
        f_RegClickEvent("Btn_Right", UI_ChageEquip, 1);

        f_RegClickEvent("GoldUp", _ChangeUpStarNum, 1);
        f_RegClickEvent("MoneyUp", _ChangeUpStarNum, 2);
        f_RegClickEvent("FragmentUp", _ChangeUpStarNum, 3);

        f_RegClickEvent("OneKeyRefine", Btn_OneKeyRefine);
    }

    #region 红点提示
    private void CheckRedPoint()
    {
        bool isCanLvUp = false;
        bool isCanEquip = false;
        bool CanRefine = false;
        Data_Pool.m_TeamPool.f_CheckTeamGodEquipRedPoint(_EquipPool, (EM_EquipPart)_EquipPool.m_EquipDT.iSite, ref isCanLvUp, ref isCanEquip, ref CanRefine);
        UITool.f_UpdateReddot(f_GetObject("Btn_Intensify"), isCanLvUp ? 1 : 0, new Vector3(168, 27, 0), 1100);
        UITool.f_UpdateReddot(f_GetObject("Btn_Refine"), CanRefine ? 1 : 0, new Vector3(168, 27, 0), 1100);
        UITool.f_UpdateReddot(f_GetObject("Chage"), isCanEquip ? 1 : 0, new Vector3(105, 20, 0), 2500);
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
    UILabel ProTab_Master1;
    UILabel ProTab_Master2;
    UISprite[] tstar;
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
    UILabel Intensify_Consume;
    UILabel Intensify_StarLevelLeft;
    UILabel Intensify_ProRight;
    GameObject Intensify_Butten;
    GameObject Intensify_Butten5;
    /// <summary>
    /// 精炼界面
    /// </summary>
    Transform _Refine;
    UILabel Refine_LevelStart;
    UILabel Refine_LevelEnd;
    UILabel Refine_Exp;
    UILabel Refine_Pro1Name;
    UILabel Refine_Pro1RightName;
    UILabel Refine_Pro1;
    UILabel Refine_Pro1Add;
    UILabel Refine_Pro2Name;
    UILabel Refine_Pro2RightName;
    UILabel Refine_Pro2;
    UILabel Refine_Pro2Add;
    UISlider Refine_ShowExp;
    GameObject Refine_Prop1;
    UILabel Refine_Prop1Num;
    GameObject Refine_Prop2;
    UILabel Refine_Prop2Num;
    GameObject Refine_Prop3;
    UILabel Refine_Prop3Num;
    GameObject Refine_Prop4;
    UILabel Refine_Prop4Num;
    //特效
    Transform[] Refine_EffectPos = new Transform[4];


    /// <summary>
    /// 升星界面
    /// </summary>
    Transform _UpStar;
    Transform UpStar_Star3;
    UISprite[] UpStar_StarArr3 = new UISprite[3];
    Transform UpStar_Star5;
    UISprite[] UpStar_StarArr5 = new UISprite[5];
    UILabel UpStar_EndStarNum;
    UILabel UpStar_StartProName;
    UILabel UpStar_StarPro;
    UILabel UpStar_NowStarpro;
    UILabel UpStar_EndPro;
    UILabel UpStar_AddPro;
    UILabel UpStar_SucRate;
    UILabel UpStar_Lucky;
    UISlider UpStar_StarExp;
    UILabel UpStar_StarNeedExp;
    GameObject UpStar_BtnUp;
    GameObject UpStar_BtnAuto;
    Transform UpStar_UpOptions;
    GameObject[] UpStar_UpOptionsBtn = new GameObject[3];   // 0银币 1元宝  2碎片
    #endregion

    #region 按钮事件


    private void UI_ChangeTab(GameObject go, object obj1, object obj2)
    {
        UI_ChangeUI((GodEquipBox.BoxTye)obj1);
    }
    private void UI_ShowBtn()
    {
        //f_GetObject("Btn_Left").SetActive(tmpEquip.oType == GodEquipBox.OpenType.Battle);
        //f_GetObject("Btn_Right").SetActive(tmpEquip.oType == GodEquipBox.OpenType.Battle);
        f_GetObject("Chage").SetActive(tmpEquip.oType == GodEquipBox.OpenType.Battle);
        f_GetObject("Btn_GetWay").SetActive(tmpEquip.oType == GodEquipBox.OpenType.SelectAward);
        f_GetObject("Btn_Intensify").SetActive(tmpEquip.oType != GodEquipBox.OpenType.SelectAward);

        GameParamDT gameParam = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_NeedLevel.GodEquipRefine) as GameParamDT;
        int showLv = null == gameParam ? 999999 : gameParam.iParam2;
        int myLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        f_GetObject("Btn_Refine").SetActive(myLv >= showLv && tmpEquip.oType != GodEquipBox.OpenType.SelectAward);


        f_GetObject("Btn_UpStars").SetActive(_EquipPool.m_EquipDT.iColour > (int)EM_Important.Purple && UITool.f_GetIsOpensystem(EM_NeedLevel.GodEquipUpStar) && tmpEquip.oType != GodEquipBox.OpenType.SelectAward);
        f_GetObject("Down").SetActive(tmpEquip.oType == GodEquipBox.OpenType.Battle);
        //f_GetObject("Btn_UpStars").SetActive(true);
    }
    private void UI_ChangeUI(GodEquipBox.BoxTye tBox)
    {
        if (tBox == GodEquipBox.BoxTye.Refine)
        {
            //判断是否达到精炼等级
            if (!UITool.f_GetIsOpensystem(EM_NeedLevel.GodEquipRefine))
            {
                UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(275), UITool.f_GetSysOpenLevel(EM_NeedLevel.GodEquipRefine)));
                return;
            }
        }

        f_GetObject("Introduce").SetActive(tBox == GodEquipBox.BoxTye.Intro);
        f_GetObject("Intensify").SetActive(tBox == GodEquipBox.BoxTye.Inten);
        f_GetObject("Refine").SetActive(tBox == GodEquipBox.BoxTye.Refine);
        f_GetObject("UpStars").SetActive(tBox == GodEquipBox.BoxTye.UpStar);
        f_GetObject("GetWay").SetActive(tBox == GodEquipBox.BoxTye.GetWay);

        UI_ChangeBtn(f_GetObject("Btn_Introduce"), tBox == GodEquipBox.BoxTye.Intro);
        UI_ChangeBtn(f_GetObject("Btn_Intensify"), tBox == GodEquipBox.BoxTye.Inten);
        UI_ChangeBtn(f_GetObject("Btn_Refine"), tBox == GodEquipBox.BoxTye.Refine);
        UI_ChangeBtn(f_GetObject("Btn_UpStars"), tBox == GodEquipBox.BoxTye.UpStar);
        UI_ChangeBtn(f_GetObject("Btn_GetWay"), tBox == GodEquipBox.BoxTye.GetWay);
        switch (tBox)
        {
            case GodEquipBox.BoxTye.Intro:
                UpdateIntroduce();
                break;
            case GodEquipBox.BoxTye.Inten:
                UpdateIntensify();
                break;
            case GodEquipBox.BoxTye.Refine:
                UpdateRefine();
                break;
            case GodEquipBox.BoxTye.UpStar:
                UpdateUpStar();
                break;
            case GodEquipBox.BoxTye.GetWay:
                UI_OpenHandbook();
                break;
        }
        tmpEquip.tType = tBox;

        ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
    }
    private void UI_ChangeBtn(GameObject go, bool show)
    {
        go.transform.Find("Up").gameObject.SetActive(show);
        go.transform.Find("Down").gameObject.SetActive(!show);
    }
    void UI_ChageEquip(GameObject go, object obj1, object obj2)
    {
        if (bReturnBtn)
        {
            return;
        }

        int tpos = (int)obj1 + _EquipPool.m_EquipDT.iSite;
        int time = 0;
        for (; time < 4; tpos += (int)obj1)
        {
            if (tpos >= (int)EM_EquipPart.eEquipPare_MagicLeft)
                tpos = (int)EM_EquipPart.eEquipPart_Weapon;
            if (tpos == (int)EM_EquipPart.eEquipPart_NONE)
                tpos = (int)EM_EquipPart.eEquipPart_Belt;
            if (tmpEquip.m_TeamPool.m_aEquipPoolDT[tpos - 1] != null)
            {
                tmpEquip.tEquipPoolDT = tmpEquip.m_TeamPool.m_aGodEquipPoolDT[tpos - 1];
                _EquipPool = tmpEquip.tEquipPoolDT;
                break;
            }
            time++;
        }
		f_GetObject("Btn_UpStars").SetActive(_EquipPool.m_EquipDT.iColour > (int)EM_Important.Purple && UITool.f_GetIsOpensystem(EM_NeedLevel.EquipUpStar) && tmpEquip.oType != GodEquipBox.OpenType.SelectAward);
        if(!(_EquipPool.m_EquipDT.iColour > (int)EM_Important.Purple && UITool.f_GetIsOpensystem(EM_NeedLevel.EquipUpStar) && tmpEquip.oType != GodEquipBox.OpenType.SelectAward))
		{
			UI_ChangeUI(GodEquipBox.BoxTye.Intro);
		}
		UpdateMain();
        switch (tmpEquip.tType)
        {
            case GodEquipBox.BoxTye.Intro:
                UpdateIntroduce();
                break;
            case GodEquipBox.BoxTye.Inten:
                UpdateIntensify();
                break;
            case GodEquipBox.BoxTye.Refine:
                UpdateRefine();
                break;
            case GodEquipBox.BoxTye.UpStar:
                UpdateUpStar();
                break;
        }
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
    }

    void UI_CloseIntroduce(GameObject go, object obj1, object obj2)
    {
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
        ccUIHoldPool.GetInstance().f_UnHold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GodEquipManage, UIMessageDef.UI_CLOSE);
    }
    void _ChangeUpStarNum(GameObject go, object obj1, object obj2)
    {
        _ChangeUpStarType(go, (int)obj1);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
    }

    void _ChangeUpStarType(GameObject go, int type)
    {
        f_GetObject("GoldUp").transform.Find("Pitch").gameObject.SetActive(type == 1);
        f_GetObject("MoneyUp").transform.Find("Pitch").gameObject.SetActive(type == 2);
        f_GetObject("FragmentUp").transform.Find("Pitch").gameObject.SetActive(type == 3);
        _UpStarType = type;
    }
    void UI_OpenSele(GameObject go, object obj1, object obj2)
    {
        if (UITool.f_CheckHasEquipLeft(LineUpPage.CurrentSelectEquipPart))
        {
            ccUIHoldPool.GetInstance().f_UnHold();
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GodEquipManage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectEquipPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            //Nếu ma khí bên trái và bên phải khác với đồng hồ trang bị thông thường, đồng hồ phải được thay đổi nếu nó khác
            if (LineUpPage.CurrentSelectEquipPart == EM_EquipPart.eEquipPare_MagicLeft
                || LineUpPage.CurrentSelectEquipPart == EM_EquipPart.eEquipPare_MagicRight)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1726));
            }else if(LineUpPage.CurrentSelectEquipPart == EM_EquipPart.eEquipPart_GodWeapon)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1726));// cân đổi
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1727));
            }
        }
    }

    void UI_DownEquip(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT OnChangeEquipCallback = new SocketCallbackDT();//更换装备回调
        OnChangeEquipCallback.m_ccCallbackSuc = EquipDownSuc;
        OnChangeEquipCallback.m_ccCallbackFail = EquipDownFail;
        Data_Pool.m_TeamPool.f_ChangeTeamGodEquip((long)tmpEquip.m_TeamPool.m_eFormationPos, 0, (byte)tmpEquip.tEquipPoolDT.m_EquipDT.iSite, OnChangeEquipCallback);
    }
    void EquipDownSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1728));
        UI_CloseIntroduce(null, null, null);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.WearEquipment));

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    void EquipDownFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1729) + UITool.f_GetError((int)obj));
    }
    //fix
    private void Btn_OneKeyRefine(GameObject go, object obj1, object obj2)
    {

        int OpenLv = Data_Pool.m_GodEquipPool.m_OneKeyRefineLv;

        int VipLvNow = Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_RefineOnekey);
        if (Data_Pool.m_CardPool.f_GetRoleLevel() >= OpenLv ||
           VipLvNow != 0)
        {

            if (_EquipPool.m_lvRefine == 50)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1730));
                return;
            }
            OneKeyGodRefineParam ttttt = new OneKeyGodRefineParam();
            ttttt.m_EquipPoolDT = _EquipPool;
            ttttt.m_CallUIBase = this;
            ttttt.m_Callback = (object obj) =>
            {
                UpdateRefine();
            };
            ccUIManage.GetInstance().f_SendMsg(UINameConst.OneKeyGodEquipRefine, UIMessageDef.UI_OPEN, ttttt);
            return;
        }

        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1731), OpenLv, Data_Pool.m_RechargePool.f_GetVipLvLimit(EM_VipPrivilege.eVip_RefineOnekey)));

    }
    #endregion
    /// <summary>
    /// 初始化
    /// </summary>
    void Initialize()
    {
        EffectParent = f_GetObject("Particle").transform;
        ////
        _EquipCase = f_GetObject("EquipCase").transform;
        Introduce_ProTab = f_GetObject("Introduce").transform.Find("ProTab");
        Introduce_RefineTab = f_GetObject("Introduce").transform.Find("RefineTab");
        Introduce_ParticularsTab = f_GetObject("Introduce").transform.Find("ParticularsTab");
        _Intensify = f_GetObject("Intensify").transform;
        _Refine = f_GetObject("Refine").transform;
        _UpStar = f_GetObject("UpStars").transform;
        ///////////////////////////////////////////////////////
        UpStar_Star3 = _UpStar.Find("Stars3");
        for (int i = 0; i < 3; i++)
            UpStar_StarArr3[i] = UpStar_Star3.GetChild(i).GetComponent<UISprite>();
        UpStar_Star5 = _UpStar.Find("Stars5");
        for (int i = 0; i < 5; i++)
            UpStar_StarArr5[i] = UpStar_Star5.GetChild(i).GetComponent<UISprite>();
        //UpStar_UpOptionsBtn[0] = Instantiate(Resources.Load<GameObject>("UIPrefab/GameMain/EquipManage/GoldUp"));
        //UpStar_UpOptionsBtn[1] = Instantiate(Resources.Load<GameObject>("UIPrefab/GameMain/EquipManage/FragmentUp"));
        //UpStar_UpOptionsBtn[2] = Instantiate(Resources.Load<GameObject>("UIPrefab/GameMain/EquipManage/MoneyUp"));
        UpStar_EndStarNum = f_GetObject("UpStar_EndStarNum").GetComponent<UILabel>();
        UpStar_Star5 = _UpStar.Find("Stars5");
        UpStar_StarExp = f_GetObject("UpStar_StarExp").GetComponent<UISlider>();
        UpStar_StarNeedExp = f_GetObject("UpStar_StarNeedExp").GetComponent<UILabel>();
        UpStar_StartProName = f_GetObject("UpStar_StartProName").GetComponent<UILabel>();
        UpStar_StarPro = f_GetObject("UpStar_StarPro").GetComponent<UILabel>();
        UpStar_NowStarpro = f_GetObject("UpStar_NowStarpro").GetComponent<UILabel>();
        //UpStar_EndProName = _UpStar.Find("EndPro/EndProName").GetComponent<UILabel>();
        UpStar_EndPro = f_GetObject("UpStar_EndPro").GetComponent<UILabel>();
        UpStar_AddPro = f_GetObject("UpStar_AddPro").GetComponent<UILabel>();
        UpStar_SucRate = f_GetObject("UpStar_SucRate").GetComponent<UILabel>();
        UpStar_Lucky = f_GetObject("UpStar_Lucky").GetComponent<UILabel>();
        UpStar_UpOptions = f_GetObject("UpStar_UpOptions").transform;
        UpStar_BtnUp = f_GetObject("UpStar_BtnUp");
        UpStar_BtnAuto = f_GetObject("UpStar_BtnAuto");
        ///////////////////////////////////////////////////////
        Refine_LevelStart = f_GetObject("Refine_LevelStart").GetComponent<UILabel>();
        Refine_LevelEnd = f_GetObject("Refine_LevelEnd").GetComponent<UILabel>();
        Refine_Exp = f_GetObject("Refine_Exp").GetComponent<UILabel>();
        Refine_Pro1Name = f_GetObject("Refine_Pro1Name").GetComponent<UILabel>();
        Refine_Pro1RightName = Refine_Pro1Name.transform.Find("Refine_Pro1NameRight").GetComponent<UILabel>();
        Refine_Pro1 = f_GetObject("Refine_Pro1").GetComponent<UILabel>();
        Refine_Pro1Add = f_GetObject("Refine_Pro1Add").GetComponent<UILabel>();
        Refine_Pro2Name = f_GetObject("Refine_Pro2Name").GetComponent<UILabel>();
        Refine_Pro2RightName = Refine_Pro2Name.transform.Find("Refine_Pro2NameRight").GetComponent<UILabel>();
        Refine_Pro2 = f_GetObject("Refine_Pro2").GetComponent<UILabel>();
        Refine_Pro2Add = f_GetObject("Refine_Pro2Add").GetComponent<UILabel>();
        Refine_ShowExp = f_GetObject("Refine_ShowExp").GetComponent<UISlider>();
        Refine_Prop1 = f_GetObject("Refine_Prop1");
        Refine_Prop1Num = f_GetObject("Refine_Prop1Num").GetComponent<UILabel>();
        Refine_Prop2 = f_GetObject("Refine_Prop2");
        Refine_Prop2Num = f_GetObject("Refine_Prop2Num").GetComponent<UILabel>();
        Refine_Prop3 = f_GetObject("Refine_Prop3");
        Refine_Prop3Num = f_GetObject("Refine_Prop3Num").GetComponent<UILabel>();
        Refine_Prop4 = f_GetObject("Refine_Prop4");
        Refine_Prop4Num = f_GetObject("Refine_Prop4Num").GetComponent<UILabel>();
        for (int i = 0; i < f_GetObject("EffectPos").transform.childCount; i++)
            Refine_EffectPos[i] = f_GetObject("EffectPos").transform.GetChild(i);
        ////////////////////////////////////////////////////////
        Intensify_StartLevel = f_GetObject("Intensify_StartLevel").GetComponent<UILabel>();
        Intensify_LastLevel = f_GetObject("Intensify_LastLevel").GetComponent<UILabel>();
        Intensify_ProRight = f_GetObject("Intensify_ProRight").GetComponent<UILabel>();
        Intensify_ProName = f_GetObject("Intensify_ProName").GetComponent<UILabel>();
        Intensify_Pro = f_GetObject("Intensify_Pro").GetComponent<UILabel>();
        Intensify_AddPro = f_GetObject("Intensify_AddPro").GetComponent<UILabel>();
        Intensify_Consume = f_GetObject("Intensify_Consume").GetComponent<UILabel>();
        Intensify_Butten = f_GetObject("Intensify_Butten").gameObject;
        Intensify_Butten5 = f_GetObject("Intensify_Butten5").gameObject;
        Intensify_StarLevelLeft = f_GetObject("Intensify_LastLeveLeft").GetComponent<UILabel>();
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
        ///////////////////////////////////////////////////////////
        _EquipName = f_GetObject("_EquipName").GetComponent<UILabel>();
        _EquipIcon = f_GetObject("_EquipIcon").GetComponent<UI2DSprite>();
        _FitOut = f_GetObject("_FitOut").GetComponent<UILabel>();
        _Color = f_GetObject("_Color").GetComponent<UILabel>();
        //_Grade = _EquipCase.Find("Character").GetComponent<UILabel>();
        UpdateIntroduce();
    }
    /// <summary>
    /// 刷新主场景
    /// </summary>
    void UpdateMain()
    {
        /////////////装备于谁  图标   未填写   _FitOut   _EquipIcon//////////////////////
        UpdateMainStar();
        string name = _EquipPool.m_EquipDT.szName;
        _EquipIcon.transform.GetChild(0).GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(_EquipPool.m_EquipDT.iColour, ref name);
        _EquipName.text = name;
        _Color.text = UITool.f_GetEquipColur((EM_Important)_EquipPool.m_EquipDT.iColour);
        string howEquip = UITool.f_GetHowEquip(_EquipPool.iId);
        _FitOut.text = howEquip == "" ? "" : string.Format(CommonTools.f_GetTransLanguage(1732), howEquip);
        //_EquipIcon.sprite2D = UITool.f_GetIconSprite(_EquipPool.m_EquipDT.iIcon);
        if(oModel != null) {
            //DestroyObject(oModel);
            DestroyImmediate(oModel);
            oModel = null;
        }
        UITool.f_CreateMagicById(_EquipPool.m_EquipDT.iId, ref oModel, f_GetObject("Model").transform, 23, "animation", null, true, 100);
    }
    void UpdateMainStar()
    {
        //f_GetObject("StarGrid5").SetActive(false);
        //f_GetObject("StarGrid3").SetActive(false);
        //return;
        f_GetObject("StarGrid5").SetActive(UITool.f_GetIsOpensystem(EM_NeedLevel.GodEquipUpStar) && _EquipPool.m_EquipDT.iColour >= (int)EM_Important.Red);
        f_GetObject("StarGrid3").SetActive(UITool.f_GetIsOpensystem(EM_NeedLevel.GodEquipUpStar) && _EquipPool.m_EquipDT.iColour == (int)EM_Important.Oragen);
        if (UITool.f_GetIsOpensystem(EM_NeedLevel.GodEquipUpStar))
        {
            switch ((EM_Important)_EquipPool.m_EquipDT.iColour)
            {
                case EM_Important.Red:
                case EM_Important.Gold:
                    tstar = new UISprite[5];
                    for (int i = 0; i < f_GetObject("StarGrid5").transform.childCount; i++)
                        tstar[i] = f_GetObject("StarGrid5").transform.GetChild(i).GetComponent<UISprite>();
                    UITool.f_UpdateStarNum(tstar, _EquipPool.m_sstars);
                    break;
                case EM_Important.Oragen:
                    tstar = new UISprite[3];
                    for (int i = 0; i < f_GetObject("StarGrid3").transform.childCount; i++)
                        tstar[i] = f_GetObject("StarGrid3").transform.GetChild(i).GetComponent<UISprite>();
                    UITool.f_UpdateStarNum(tstar, _EquipPool.m_sstars);
                    break;
                default:
                    break;
            }
        }
    }

    void UpdateIntroduce()
    {
        UpdateMain();
        float _LineSpace = 10;
        float _TabSpace = 20;
        Transform RefineTab = f_GetObject("RefineTab").transform;
        Transform ParticularsTab = f_GetObject("ParticularsTab").transform;
        //Transform SetEuqipTab = f_GetObject("SetEuqipTab").transform;
        UIGrid SetEquipTab_Body = f_GetObject("SetEquipTab_Body").GetComponent<UIGrid>();
        UILabel SetEquipPro = f_GetObject("SetEquipPro").GetComponent<UILabel>();

        ////////信息界面  属性简介/////////////
        ProTab_Level.text = _EquipPool.m_lvIntensify.ToString();
        ProTab_ProName.text = UITool.f_GetProName((EM_RoleProperty)_EquipPool.m_EquipDT.iIntenProId) + ":";
        ProTab_Pro.text = UITool.f_GetGodEquipPro(_EquipPool).ToString();
        //ProTab_Master1.text = UITool.f_GetEquipIntenMaster(_EquipPool)[0];
        //ProTab_Master2.transform.localPosition = new Vector3(0, ProTab_Master1.text == "" ? 0 : -ProTab_Master1.height - _LineSpace, 0);
        //ProTab_Master2.text = UITool.f_GetEquipIntenMaster(_EquipPool)[1];
        ////////////////信息界面   精炼简介///////////////////
        if (_EquipPool.m_lvRefine > 0)
        {
            RefineTab_Level.text = _EquipPool.m_lvRefine.ToString();
            //RefineTab.localPosition = new Vector3(0, ProTab_Master1.transform.localPosition.y - _TabSpace, 0);
            //RefineTab.localPosition = new Vector3(0, ProTab_Master1.transform.localPosition.y - (ProTab_Master1.text == "" ? 0 : _LineSpace + ProTab_Master1.height) - ProTab_Master2.height - _TabSpace, 0);
            RefineTab_Pro1Name.text = UITool.f_GetProName((EM_RoleProperty)_EquipPool.m_EquipDT.iRefinProId1) + ":";
            UITool.f_UpdateAddPro(_EquipPool.m_EquipDT.iRefinProId1, RefineTab_Pro1, UITool.f_GetGodEquipRefinePro(_EquipPool)[0]);
            RefineTab_Pro2Name.text = UITool.f_GetProName((EM_RoleProperty)_EquipPool.m_EquipDT.iRefinProId2) + ":";
            UITool.f_UpdateAddPro(_EquipPool.m_EquipDT.iRefinProId2, RefineTab_Pro2, UITool.f_GetGodEquipRefinePro(_EquipPool)[1]);
            //RefineTab_Master.text = UITool.f_GetEquipRefineMaster(_EquipPool);
        }
        RefineTab.gameObject.SetActive(_EquipPool.m_lvRefine > 0);
        ////////////////////////套装简介//////////////////////////////
        //if (_EquipPool.m_lvRefine > 0)
        //    SetEuqipTab.transform.localPosition = new Vector3(-97.6f, -98, 0);
        //else
        //    //SetEuqipTab.transform.localPosition = new Vector3(0, RefineTab.localPosition.y - 0 - _TabSpace, 0);
        //    SetEuqipTab.transform.localPosition = new Vector3(-97.6f, 126.6f, 0);
        //SetEquipDT tSetEquipDT = Data_Pool.m_EquipPool.f_GetSetEquipDT(_EquipPool.m_EquipDT.iId);
        //bool tShowSetEquip = tSetEquipDT == null;
        //SetEuqipTab.gameObject.SetActive(!tShowSetEquip);
        //int[] tEquipid = { tSetEquipDT.iEquipId1, tSetEquipDT.iEquipId2, tSetEquipDT.iEquipId3, tSetEquipDT.iEquipId4 };
        //for (int i = 0; i < 4; i++)
        //{
        //    SetEquipTab_Body.transform.GetChild(i).GetComponent<ResourceCommonItem>().f_UpdateByInfo((int)EM_ResourceType.Equip, tEquipid[i], 0);
        //    SetEquipTab_Body.transform.GetChild(i).Find("Num").GetComponent<UILabel>().text = "";
        //    for (int j = 0; j < SetEquipTab_Body.transform.GetChild(i).GetChild(4).childCount; j++)
        //    {
        //        Destroy(SetEquipTab_Body.transform.GetChild(i).GetChild(4).GetChild(j).gameObject);
        //    }
        //}
        //SetEquipPro.text = string.Format(CommonTools.f_GetTransLanguage(1733) + "[-][b6a791]{0}+{1}\n", UITool.f_GetProName((EM_RoleProperty)tSetEquipDT.iTwoEquipProId), tSetEquipDT.iTwoPro);
        //SetEquipPro.text += string.Format(CommonTools.f_GetTransLanguage(1734) + "[-][b6a791]{0}+{1}\n", UITool.f_GetProName((EM_RoleProperty)tSetEquipDT.iThreeEquipProId), tSetEquipDT.iThreePro);
        //SetEquipPro.text += string.Format(CommonTools.f_GetTransLanguage(1735) + "[-][b6a791]{0}+{1}%  ", UITool.f_GetProName((EM_RoleProperty)tSetEquipDT.iFourEquipProId1), tSetEquipDT.iFourPro1 / 100);
        //SetEquipPro.text += string.Format("{0}+{1}%", UITool.f_GetProName((EM_RoleProperty)tSetEquipDT.iFourEquipProId2), tSetEquipDT.iFourPro2 / 100);
        ////////////////////信息界面   说明简介/////////////////////////
        //if (_EquipPool.m_lvRefine > 0)
        //    ParticularsTab.transform.localPosition = new Vector3(-73.3f, -507f, 0); 
        //else
        //    ParticularsTab.transform.localPosition = new Vector3(-73.3f, -282f, 0);

        ParticularsTab_Label.text = _EquipPool.m_EquipDT.szDescribe;

        UISprite SprNorAttackBg = f_GetObject("SprNorAttackBg").GetComponent<UISprite>();
        UILabel SkillName = f_GetObject("SkillName").GetComponent<UILabel>();
        UI2DSprite SkillIcon = f_GetObject("SkillIcon").GetComponent<UI2DSprite>();
        UILabel SkillDes = f_GetObject("SkillDes").GetComponent<UILabel>();

        SkillName.text = _EquipPool.m_MagicDT.szName;
        SkillDes.text = _EquipPool.m_MagicDT.szReadme;

        SkillIcon.sprite2D = UITool.f_GetSkillIcon(_EquipPool.m_MagicDT.iId.ToString());



    }
    void CreareSetEquipEffect(Transform SetEquipIndex, int id)
    {
        string EffectName = string.Empty;
        EquipDT tequipDT = glo_Main.GetInstance().m_SC_Pool.m_EquipSC.f_GetSC(id) as EquipDT;
        switch ((EM_Important)tequipDT.iColour)
        {
            case EM_Important.White:
            case EM_Important.Green:
                EffectName = UIEffectName.biankuangliuguang_lv;
                break;
            case EM_Important.Blue:
                EffectName = UIEffectName.biankuangliuguang_lan;
                break;
            case EM_Important.Purple:
                EffectName = UIEffectName.biankuangliuguang_zi;
                break;
            case EM_Important.Oragen:
                EffectName = UIEffectName.biankuangliuguang_cheng;
                break;
            case EM_Important.Red:
                EffectName = UIEffectName.biankuangliuguang_hong;
                break;
            case EM_Important.Gold:
                break;
            default:
                break;
        }
        GameObject SetEquipEffect = UITool.f_CreateEffect_Old(EffectName, EffectParent, Vector3.zero, 1f, 0, UIEffectName.UIEffectAddress1);
        SetEquipEffect.GetComponent<ParticleScaler>().particleScale = 1f;
        SetEquipEffect.SetActive(false);
        SetEquipEffect.SetActive(true);
        SetEquipEffect.transform.parent = SetEquipIndex;
        SetEquipEffect.transform.parent.localScale = Vector3.one * 160;
        SetEquipEffect.transform.localPosition = Vector3.zero;
        SetEquipEffect.transform.localScale = Vector3.one;
    }
    void UpdateIntensify()
    {
        ///////////////升级界面/////////////////////
        Intensify_StarLevelLeft.text = Intensify_StartLevel.text = string.Format("{0}/{1}", _EquipPool.m_lvIntensify, UITool.f_GetGodEquipIntenMax());
        if (_EquipPool.m_lvIntensify + 1 > UITool.f_GetGodEquipIntenMax())
            Intensify_LastLevel.text = string.Format("{0}/{1}[-]", _EquipPool.m_lvIntensify + 1, UITool.f_GetGodEquipIntenMax());
        else
            Intensify_LastLevel.text = string.Format("{0}/{1}[-]", _EquipPool.m_lvIntensify + 1, UITool.f_GetGodEquipIntenMax());
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money) < UITool.f_GetGodEquipIntenCon(_EquipPool, 1))
            Intensify_Consume.text = string.Format("[ff0000]{0}", UITool.f_GetGodEquipIntenCon(_EquipPool, 1).ToString());
        else
            Intensify_Consume.text = UITool.f_GetGodEquipIntenCon(_EquipPool, 1).ToString();
        Intensify_ProRight.text = Intensify_ProName.text = UITool.f_GetProName((EM_RoleProperty)_EquipPool.m_EquipDT.iIntenProId) + ":";
        Intensify_Pro.text = UITool.f_GetGodEquipPro(_EquipPool).ToString();
        UITool.f_UpdateAddPro(_EquipPool.m_EquipDT.iIntenProId, Intensify_AddPro, _EquipPool.m_EquipDT.iAddPro);
    }
    bool FirtLoad = true;
    GameObject[] tgo = new GameObject[4];
    void UpdateRefine()
    {
        if (FirtLoad)
        {
            tgo[0] = Refine_Prop1;
            tgo[1] = Refine_Prop2;
            tgo[2] = Refine_Prop3;
            tgo[3] = Refine_Prop4;
            FirtLoad = false;
        }

        Refine_LevelStart.text = string.Format("{0}/{1}", _EquipPool.m_lvRefine, 50);
        if (_EquipPool.m_lvRefine + 1 > 50)
            Refine_LevelEnd.text = "Max";
        else
            Refine_LevelEnd.text = string.Format("{0}/{1}", _EquipPool.m_lvRefine + 1, 50);


        Refine_Pro1Name.text = UITool.f_GetProName((EM_RoleProperty)_EquipPool.m_EquipDT.iRefinProId1) + ":";
        Refine_Pro1RightName.text = Refine_Pro1Name.text;
        Refine_Pro2Name.text = UITool.f_GetProName((EM_RoleProperty)_EquipPool.m_EquipDT.iRefinProId2) + ":";
        Refine_Pro2RightName.text = Refine_Pro2Name.text;
        UITool.f_UpdateAddPro(_EquipPool.m_EquipDT.iRefinProId1, Refine_Pro1, UITool.f_GetGodEquipRefinePro(_EquipPool)[0]);
        UITool.f_UpdateAddPro(_EquipPool.m_EquipDT.iRefinProId2, Refine_Pro2, UITool.f_GetGodEquipRefinePro(_EquipPool)[1]);

        UITool.f_UpdateAddPro(_EquipPool.m_EquipDT.iRefinProId1, Refine_Pro1Add, _EquipPool.m_EquipDT.iRefinPro1);
        UITool.f_UpdateAddPro(_EquipPool.m_EquipDT.iRefinProId2, Refine_Pro2Add, _EquipPool.m_EquipDT.iRefinPro2);
        if (_EquipPool.m_lvRefine != 50)
        {
            Refine_ShowExp.value = (float)_EquipPool.m_iexpRefine / UITool.f_GetGodEquipRefineExp(_EquipPool);
            Refine_Exp.text = string.Format("{0}/{1}", _EquipPool.m_iexpRefine, UITool.f_GetGodEquipRefineExp(_EquipPool));
        }
        else
        {
            Refine_ShowExp.value = 1;
            Refine_Exp.text = "MAX";
        }

        Refine_Prop1Num.text = UITool.f_GetGoodsNum(Tempid[0]).ToString();
        Refine_Prop2Num.text = UITool.f_GetGoodsNum(Tempid[1]).ToString();
        Refine_Prop3Num.text = UITool.f_GetGoodsNum(Tempid[2]).ToString();
        Refine_Prop4Num.text = UITool.f_GetGoodsNum(Tempid[3]).ToString();
        for (int i = 0; i < tgo.Length; i++)
            UITool.f_SetIconSprite(tgo[i].GetComponent<UI2DSprite>(), EM_ResourceType.Good, 133 + i);
        if (_EquipPool.m_lvRefine == 50)
            return;
        /////////////////////////精炼界面///////////////////////////////

    }
    void UpdateUpStar(bool ischange = true)
    {
        UIGrid tmpGrid = UpStar_UpOptions.GetComponent<UIGrid>();
        int[] tmpBtnUp = new int[3];
        GodEquipUpStarDT tmpUpStar = UITool.f_GetGodEquipUpStar(_EquipPool);
        if (tmpUpStar == null) return;
        tmpBtnUp[0] = tmpUpStar.iSilverNum1;
        tmpBtnUp[1] = tmpUpStar.iGoldNum2;
        tmpBtnUp[2] = tmpUpStar.iDebrisNum;
        UpStar_UpOptions.Find("GoldUp").gameObject.SetActive(tmpBtnUp[0] > 0);
        UpStar_UpOptions.Find("MoneyUp").gameObject.SetActive(tmpBtnUp[1] > 0);
        UpStar_UpOptions.Find("FragmentUp").gameObject.SetActive(tmpBtnUp[2] > 0);
        if (tmpBtnUp[0] > 0)
        {
            UpStar_UpOptions.Find("GoldUp/Gold/GoldNum").GetComponent<UILabel>().text = tmpUpStar.iSilverNum1 > Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Money) ? string.Format("[ff0000]{0}[-]", tmpUpStar.iSilverNum1) : tmpUpStar.iSilverNum1.ToString();
        }
        if (tmpBtnUp[1] > 0)
        {
            UpStar_UpOptions.Find("MoneyUp/Money/MoneyNum").GetComponent<UILabel>().text = tmpUpStar.iGoldNum2 > Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Sycee) ? string.Format("[ff0000]{0}[-]", tmpUpStar.iGoldNum2) : tmpUpStar.iGoldNum2.ToString();
        }
        if (tmpBtnUp[2] > 0)
        {
            UpStar_UpOptions.Find("FragmentUp/Fragment/FragmentNum").GetComponent<UILabel>().text = tmpUpStar.iDebrisNum > UITool.f_GetGodEquipFragmentNum(tmpUpStar.iDebrisId) ? string.Format("[ff0000]{0}[-]", tmpUpStar.iDebrisNum) : tmpUpStar.iDebrisNum.ToString();
        }
        if (ischange)
        {
            for (int i = 0; i < 3; i++)
            {
                MessageBox.DEBUG(UpStar_UpOptions.GetChild(i).name);
                if (UpStar_UpOptions.GetChild(i).gameObject.activeSelf)
                {
                    _ChangeUpStarType(UpStar_UpOptions.GetChild(i).gameObject, i + 1);
                    break;
                }
            }
        }
        tmpGrid.Reposition();
        UpdateUpStarHead();
    }

    void UpdateUpStarHead()
    {
        int[] tmpBtnUp = new int[3];
        GodEquipUpStarDT tmpUpStar = UITool.f_GetGodEquipUpStar(_EquipPool);
        tmpBtnUp[0] = tmpUpStar.iSilverNum1;
        tmpBtnUp[1] = tmpUpStar.iGoldNum2;
        tmpBtnUp[2] = tmpUpStar.iDebrisNum;
        int[] tmpInt = UITool.f_GetGodEquipStarPro(_EquipPool);
        switch (_EquipPool.m_EquipDT.iColour)
        {
            case (int)EM_Important.Oragen:
                UpStar_Star3.gameObject.SetActive(false);
                UpStar_Star5.gameObject.SetActive(false);
                UITool.f_UpdateStarNum(UpStar_StarArr3, _EquipPool.m_sstars);
                if (_EquipPool.m_sstars < 3)
                {
                    UpStar_EndStarNum.text = string.Format(CommonTools.f_GetTransLanguage(1736), _EquipPool.m_sstars + 1);
                    UpStar_EndPro.text = tmpInt[1].ToString();
                    UpStar_AddPro.text = tmpInt[2].ToString();
                    UpStar_StarNeedExp.text = string.Format("{0}/{1}", _EquipPool.m_sexpStars, tmpUpStar.iUpExp);
                }
                else
                {
                    UpStar_EndStarNum.text = CommonTools.f_GetTransLanguage(1737);
                    UpStar_EndPro.text = "Max";
                    UpStar_AddPro.text = "Max";
                    UpStar_StarNeedExp.text = "Max";
                }
                break;
            case (int)EM_Important.Red:
            case (int)EM_Important.Gold:
                UpStar_Star5.gameObject.SetActive(false);
                UpStar_Star3.gameObject.SetActive(false);
                UITool.f_UpdateStarNum(UpStar_StarArr5, _EquipPool.m_sstars);
                if (_EquipPool.m_sstars < 5)
                {
                    UpStar_EndStarNum.text = string.Format(CommonTools.f_GetTransLanguage(1736), _EquipPool.m_sstars + 1);
                    UpStar_EndPro.text = tmpInt[1].ToString();
                    UpStar_AddPro.text = tmpInt[2].ToString();
                    UpStar_StarNeedExp.text = string.Format("{0}/{1}", _EquipPool.m_sexpStars, tmpUpStar.iUpExp);
                }
                else
                {
                    UpStar_EndStarNum.text = CommonTools.f_GetTransLanguage(1737);
                    UpStar_EndPro.text = "Max";
                    UpStar_AddPro.text = "Max";
                    UpStar_StarNeedExp.text = "Max";
                }
                break;
        }
        UpStar_StartProName.text = UITool.f_GetProName((EM_RoleProperty)tmpUpStar.iProId)+":";
        UpStar_StarPro.text = tmpInt[0].ToString();
        UpStar_NowStarpro.text = ((int)(((float)_EquipPool.m_sexpStars / (float)tmpUpStar.iUpExp) * tmpUpStar.iAddPro)).ToString();
        //UpStar_EndProName.text = UITool.f_GetProName((EM_RoleProperty)tmpUpStar.iProId);
        UpStar_SucRate.text = UITool.f_GetGodStarSucRate(_EquipPool);
        UpStar_StarExp.value = (float)_EquipPool.m_sexpStars / (float)tmpUpStar.iUpExp;
        UpStar_Lucky.text = _EquipPool.m_slucky.ToString();
    }

    /// <summary>
    /// 装备升星
    /// </summary>
    void Equip_UpStars()
    {
        GodEquipUpStarDT tmpStar = UITool.f_GetGodEquipUpStar(_EquipPool);
        SocketCallbackDT Callback = new SocketCallbackDT();
        Callback.m_ccCallbackSuc = EquipUpStarSuc;
        Callback.m_ccCallbackFail = EquipUpStaFail;
        switch (_EquipPool.m_EquipDT.iColour)
        {
            case (int)EM_Important.Oragen:
                if (_EquipPool.m_sstars == 3)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1738));
                    ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
                    return;
                }
                break;
            case (int)EM_Important.Red:
            case (int)EM_Important.Gold:
                if (_EquipPool.m_sstars == 5)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1738));
                    ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
                    return;
                }
                break;
        }
        switch (_UpStarType)
        {
            case 1:
                if (tmpStar.iSilverNum1 >
                           Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money))
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1739));
                    GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                    ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
                    return;
                }
                Data_Pool.m_GodEquipPool.f_GodEquipUpStar(_EquipPool.iId, EM_GodEquipUpStarType.GoldUp, Callback);
                break;
            case 3:
                if (tmpStar.iDebrisNum >
                           UITool.f_GetGodEquipFragmentNum(tmpStar.iDebrisId))
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1740));
                    ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
                    return;
                }
                Data_Pool.m_GodEquipPool.f_GodEquipUpStar(_EquipPool.iId, EM_GodEquipUpStarType.FragmentUp, Callback);
                break;
            case 2:
                if (tmpStar.iGoldNum2 >
                          Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee))
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1741));
                    ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
                    return;
                }
                Data_Pool.m_GodEquipPool.f_GodEquipUpStar(_EquipPool.iId, EM_GodEquipUpStarType.MoneyUp, Callback);
                break;
        }
    }
    /// <summary>
    /// 升星
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    void Equip_UpStar(GameObject go, object obj1, object obj2)
    {
        Equip_UpStars();
    }
    /// <summary>
    /// 自动升星
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    void Equip_UpStarAuto(GameObject go, object obj1, object obj2)
    {
        int NeedLv = Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_EquipUpstarOnkey);
        int OpenLv = Data_Pool.m_RechargePool.f_GetVipLvLimit(EM_VipPrivilege.eVip_EquipUpstarOnkey);
        if (NeedLv <= 0)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1742), OpenLv));
            return;
        }
        switch (_EquipPool.m_EquipDT.iColour)
        {
            case (int)EM_Important.Oragen:
                if (_EquipPool.m_sstars >= 3)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1738));
                    return;
                }
                break;
            case (int)EM_Important.Red:
            case (int)EM_Important.Gold:
                if (_EquipPool.m_sstars >= 5)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1738));
                    return;
                }
                break;
        }
        GodEquipUpStarDT tmpStar = UITool.f_GetGodEquipUpStar(_EquipPool);
        switch (_UpStarType)
        {
            case 1:
                if (tmpStar.iSilverNum1 >
                           Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money))
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1739));
                    GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                    ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
                    return;
                }
                break;
            case 3:
                if (tmpStar.iDebrisNum >
                           UITool.f_GetGodEquipFragmentNum(tmpStar.iDebrisId))
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1740));
                    ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
                    return;
                }
                break;
            case 2:
                if (tmpStar.iGoldNum2 >
                          Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee))
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1741));
                    ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
                    return;
                }
                break;
        }


        ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
        Time_SedUpStar = ccTimeEvent.GetInstance().f_RegEvent(0.2f, true, _UpStarType, UpStar);

    }
    int RefineTimes;
    /// <summary>
    /// 装备精炼
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    void Equip_Refine(GameObject go, object obj1, object obj2)
    {
        //kiểm tra lev
        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.GodEquipRefine))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(275), UITool.f_GetSysOpenLevel(EM_NeedLevel.GodEquipRefine)));// thông báo
            return;
        }

        switch (go.name)
        {
            case "Refine_Prop1":
                RefinePillIndex = 1;
                break;
            case "Refine_Prop2":
                RefinePillIndex = 2;
                break;
            case "Refine_Prop3":
                RefinePillIndex = 3;
                break;
            case "Refine_Prop4":
                RefinePillIndex = 4;
                break;
        }

        int refineEquipId = 0;
        if (RefinePillIndex >= 1 && RefinePillIndex <= 4)
        {
            refineEquipId = Tempid[RefinePillIndex - 1];
        }

        if (_EquipPool.m_lvRefine >= 50)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1743));
            return;
        }
        BaseGoodsPoolDT tGoods = (BaseGoodsPoolDT)obj2;
        RefineTimes = 1;
        if (tGoods == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1744));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, refineEquipId, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        if ((bool)obj1)
        {
            if (tGoods.m_iNum - RefineTimes < 0)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1744));
                GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, refineEquipId, this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                return;
            }
            Refine_GoodsEffect = UITool.f_CreateEffect_Old(UIEffectName.EquipRefine_Case, tgo[RefinePillIndex - 1].transform, Vector3.zero, 1f, 2f, UIEffectName.UIEffectAddress1);
            ClickRefine(tGoods);
            Time_ClickRefineInvokeTime = ccTimeEvent.GetInstance().f_RegEvent(1f, false, tGoods, InvokeClickRefine);
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_SendRefine);
            bReturnBtn = true;
        }
        else
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_ClickRefineInvokeTime);
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_ClickRefine);
            RefinePillIndex = 0;
            Time_SendRefine = ccTimeEvent.GetInstance().f_RegEvent(1.5f, false, tGoods, SendRefine);
            bReturnBtn = false;
        }

    }
    private int[] RefinePillNum = new int[4];
    private Vector3 EffectEndPos;
    private int RefineSendNum = 0;  //精炼发送的次数
    private int RefineAcceptNum = 0;   //精炼接收的次数
    void InvokeClickRefine(object Obj)
    {
        Time_ClickRefine = ccTimeEvent.GetInstance().f_RegEvent(0.01f, true, (BaseGoodsPoolDT)Obj, ClickRefine);
    }
    void ClickRefine(object Obj)
    {
        BaseGoodsPoolDT tGoods = (BaseGoodsPoolDT)Obj;
        if (tGoods.m_iNum - RefineTimes < 0)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_ClickRefine);
            return;
        }
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(1745));
        GameObject t2Dsprite = new GameObject();
        RefinePillNum[RefinePillIndex - 1] += RefineTimes;
        _EquipPool.m_iexpRefine += tGoods.m_BaseGoodsDT.iEffectData * RefineTimes;
        tGoods.m_iNum -= RefineTimes;
        t2Dsprite.transform.parent = Refine_EffectPos[RefinePillIndex - 1];
        EffectEndPos = new Vector3(t2Dsprite.transform.parent.localPosition.x * -1f, 330, 0);
        //飞出去的特效
        UI2DSprite t2d = t2Dsprite.AddComponent<UI2DSprite>();
        UITool.f_SetIconSprite(t2d, EM_ResourceType.Good, Tempid[RefinePillIndex - 1]);
        t2d.depth = 3000;
        t2d.MakePixelPerfect();
        t2Dsprite.transform.localPosition = Vector3.zero;
        TweenPosition tTweenPos = t2Dsprite.AddComponent<TweenPosition>();
        tTweenPos.from = Vector3.zero; tTweenPos.to = EffectEndPos;
        TweenAlpha tTweenAlp = t2Dsprite.AddComponent<TweenAlpha>();
        tTweenAlp.from = 1; tTweenAlp.to = 0;
        TweenScale tTweenSca = t2Dsprite.AddComponent<TweenScale>();
        tTweenSca.from = Vector3.one; tTweenSca.to = Vector3.one * 0.5f;
        tTweenPos.PlayForward();
        tTweenAlp.PlayForward();
        Destroy(t2Dsprite, 2f);
        UpdateRefine();
        if (_EquipPool.m_iexpRefine >= UITool.f_GetGodEquipRefineExp(_EquipPool))
        {
            Refine_EquipEffect = UITool.f_CreateEffect_Old(UIEffectName.zhenrong_zbjl_01, EffectParent, new Vector3(-0.4f, -0.05f, 0), 0.2f, 2f, UIEffectName.UIEffectAddress1);
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.EquipRefine);
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1746));
            _EquipPool.m_iexpRefine -= UITool.f_GetGodEquipRefineExp(_EquipPool);
            _EquipPool.m_lvRefine += 1;
            SendRefine(null);
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_ClickRefineInvokeTime);
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_ClickRefine);
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_SendRefine);
            UpdateRefine();
        }

        if (StaticValue.m_EquipRefine)
            RefineTimes += 10;
        else
        {
            if (RefinePillNum[RefinePillIndex - 1] % 50 == 0)
            {
                RefineTimes += 1;
            }
        }
        if (_EquipPool.m_iexpRefine + tGoods.m_BaseGoodsDT.iEffectData * RefineTimes >= UITool.f_GetGodEquipRefineExp(_EquipPool))
        {
            RefineTimes = 1;
        }

    }

    void SendRefine(object obj)
    {
        if (RefinePillNum[0] > 0)
            SendEquipRefine(0);
        if (RefinePillNum[1] > 0)
            SendEquipRefine(1);
        if (RefinePillNum[2] > 0)
            SendEquipRefine(2);
        if (RefinePillNum[3] > 0)
            SendEquipRefine(3);
    }

    void SendEquipRefine(int index)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT Callback = new SocketCallbackDT();
        Callback.m_ccCallbackFail = RefineFail;
        Callback.m_ccCallbackSuc = RefineSuc;
        Data_Pool.m_GodEquipPool.f_Refine(_EquipPool.iId, Tempid[index], RefinePillNum[index], Callback);
        MessageBox.DEBUG(string.Format(CommonTools.f_GetTransLanguage(1747), _EquipPool.iId, Tempid[index], RefinePillNum[index]));
        RefinePillNum[index] = 0;
        RefineSendNum++;
    }
    /// <summary>
    /// 精炼成功
    /// </summary>
    void RefineSuc(object data)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        RefineAcceptNum++;
        MessageBox.DEBUG(string.Format(CommonTools.f_GetTransLanguage(1748), RefineSendNum, RefineAcceptNum));

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
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(1749) + UITool.f_GetError((int)data));
    }
    /// <summary>
    /// 升星成功
    /// </summary>
    void EquipUpStarSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UpdateUpStarHead();
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.EquipStarUp));
        //UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1750));
        if (tmpIntStar != _EquipPool.m_sstars)
        {
            UpdateMainStar();
            UpdateUpStar();
            UpStar_EquipEffect = UITool.f_CreateEffect_Old(UIEffectName.zr_zbsx_01, EffectParent, new Vector3(-0.4f, -0.06f), 0.2f, 2f, UIEffectName.UIEffectAddress1);
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.EquipStar);
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1751));
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_SedUpStar);
            tmpIntStar = _EquipPool.m_sstars;
        }

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    /// <summary>
    /// 升星失败
    /// </summary>
    /// <param name="obj"></param>
    void EquipUpStaFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(UITool.f_GetError((int)obj));
    }
    /// <summary>
    /// 装备强化
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    void EquipInten(GameObject go, object obj1, object obj2)
    {
        if (_EquipPool.m_lvIntensify >= UITool.f_GetGodEquipIntenMax())
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1752));//cần đổi
            return;
        }

        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money) < UITool.f_GetGodEquipIntenCon(_EquipPool, 1))// nếu ko đủ tiền thì sẽ gợi ý 
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1753));//cần đổi
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_GodEquipPool.f_LevelUp(_EquipPool.iId, (short)(1 + _EquipPool.m_lvIntensify), InterSuc);
    }
    /// <summary>
    /// 强化五次
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    void EquipInten5(GameObject go, object obj1, object obj2)
    {
        //if (!UITool.f_GetIsOpensystem(EM_NeedLevel.EquipIntenFiveLevel))
        //{
        //    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1754), UITool.f_GetSysOpenLevel(EM_NeedLevel.EquipIntenFiveLevel)));
        //    return;
        //}
        if (_EquipPool.m_lvIntensify >= UITool.f_GetGodEquipIntenMax())
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1755));
            return;
        }

        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money) < UITool.f_GetGodEquipIntenCon(_EquipPool, 5))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1756));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        if (_EquipPool.m_lvIntensify + 5 >= UITool.f_GetGodEquipIntenMax())
            Data_Pool.m_GodEquipPool.f_LevelUp(_EquipPool.iId, (short)(UITool.f_GetGodEquipIntenMax()), InterSuc);
        else
            Data_Pool.m_GodEquipPool.f_LevelUp(_EquipPool.iId, (short)(5 + _EquipPool.m_lvIntensify), InterSuc);

    }
    int tmpIntStar;

    void InterSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        Inten_Effect = UITool.f_CreateEffect_Old(UIEffectName.zhenrong_zbqh_01, EffectParent, new Vector3(-0.41f, 0f, 0f), 0.2f, 2f, UIEffectName.UIEffectAddress1);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.EquipInten);
        SC_GodEquipIntensify tmpIntStar = (SC_GodEquipIntensify)obj;
        string[] tmpStr = new string[3];
        tmpStr[0] = CommonTools.f_GetTransLanguage(1757) + " + " + (tmpIntStar.realTimes + tmpIntStar.critTimes);
        tmpStr[1] = CommonTools.f_GetTransLanguage(1758) + " " + tmpIntStar.critTimes;
        tmpStr[2] = string.Format("{0}+{1}", UITool.f_GetProName((EM_RoleProperty)_EquipPool.m_EquipDT.iIntenProId), tmpIntStar.realTimes * _EquipPool.m_EquipDT.iAddPro);
        UITool.Ui_Trip(string.Join("\n", tmpStr));
        UpdateIntensify();
        //强化完成更新红点
        CheckRedPoint();

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    /// <summary>
    /// 一键升星
    /// </summary>
    /// <param name="ui"></param>
    /// <returns></returns>
    void UpStar(object obj)
    {
        int ui = (int)obj;
        Equip_UpStars();
        _ChangeUpStarType(UpStar_UpOptions.GetChild(ui-1).gameObject, ui);
    }
    #region 图鉴打开
    /// <summary>
    /// 图鉴打开
    /// </summary>
    private void UI_OpenHandbook()
    {
        GetWayTools.ShowContent(f_GetObject("GetWayScrollview"), f_GetObject("GetWay_ItemParent"), f_GetObject("GetWay_Item"),
            new GetWayPageParam(EM_ResourceType.Equip, _EquipPool.m_EquipDT.iId, null), this);
    }
    #endregion
}
/// <summary>
/// 装箱
/// </summary>
public struct GodEquipBox : Box
{
    public GodEquipPoolDT tEquipPoolDT;
    public BoxTye tType;
    public OpenType oType;
    public TeamPoolDT m_TeamPool;
    public enum BoxTye
    {
        Intro,    //信息
        Inten,    //强化
        Refine,    // 精炼
        UpStar,  //升星
        GetWay,   //获取途径
    }
    public enum OpenType
    {
        Battle,   //阵容
        Bage,    //背包
        Master,   //大师
        SelectAward,  //选择界面
    }
}
/// <summary>
/// 装备升星类型
/// </summary>
public enum EM_GodEquipUpStarType
{
    /// <summary>
    /// 银币升星
    /// </summary>
    GoldUp = 1,
    /// <summary>
    /// 碎片升星
    /// </summary>
    FragmentUp = 3,
    /// <summary>
    /// 元宝升星
    /// </summary>
    MoneyUp = 2,
}
