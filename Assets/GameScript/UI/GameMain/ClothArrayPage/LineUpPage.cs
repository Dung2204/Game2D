using System;
using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 阵容页面
/// </summary>
public class LineUpPage : UIFramwork
{
    public static EM_FormationPos CurrentSelectCardPos = EM_FormationPos.eFormationPos_Main;//当前选中的卡牌位置
    public static EM_EquipPart CurrentSelectEquipPart = EM_EquipPart.eEquipPart_INVALID;//当前选中的装备位置
    public static EM_ReinforcePos CurrentSelectReinforcePos = EM_ReinforcePos.eReinforcePos_B1;//当前选中的援军位
    private Dictionary<EM_FormationPos, TeamPoolDT> dicTeamPoolDT = new Dictionary<EM_FormationPos, TeamPoolDT>();//保存数据
    private int[] tBefore = new int[4];
    private int[] tLastPro = new int[4];
    private long tBeforeHp;
    private long tLastHp;
    private int[] tMasterDT;
	string id;
	//string skillId;
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        CurrentSelectCardPos = e == null ? CurrentSelectCardPos : (EM_FormationPos)e;//特殊情况进入阵容页面有时候有指定选中的情况
        //InitMoneyUI();
        InitTeamData();

        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_MODELINFOR, UpdateModelView, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_FATE_TRIP, UpdateFateTrip, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_ClothArrayChanged, ShowForceModelContent, this);
        f_LoadTexture();

    }
    private string strTexBlackNoCardRoot = "UI/UITexture/MainMenu/TexBlackCard";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        UITexture TexBlackNoCard = f_GetObject("TexBlackNoCard").GetComponent<UITexture>();
        if (TexBlackNoCard.mainTexture == null)
        {
            Texture2D tTexBlackNoCard = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBlackNoCardRoot);
            TexBlackNoCard.mainTexture = tTexBlackNoCard;
        }
    }
    /// <summary>
    /// 新手引导相关
    /// </summary>
    private void Guidance()
    {

    }
    /// <summary>
    /// 页面关闭，关闭金钱UI
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);

        CancelInvoke("ChangeCardRedPointHint");
        CurrentSelectCardPos = EM_FormationPos.eFormationPos_Main;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FateTrip, UIMessageDef.UI_CLOSE);

        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_MODELINFOR, UpdateModelView, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_FATE_TRIP, UpdateFateTrip, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_ClothArrayChanged, ShowForceModelContent, this);
    }
    /// <summary>
    /// 设置左菜单卡牌按钮UI
    /// </summary>
    private void SetCardBtnContent(GameObject go, TeamPoolDT data)
    {
        go.transform.Find("SprBorder").gameObject.SetActive(true);
        go.transform.Find("SprBorder").GetComponent<UISprite>().spriteName = "Icon_White";
        go.transform.Find("SprHeadIcon").gameObject.SetActive(true);
        go.transform.Find("SprHeadIcon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteByCardId(data.m_CardPoolDT);

        if (data != null)
        {
            int iEvolveLv = data.m_CardPoolDT.m_iEvolveLv;
            string cardName = data.m_CardPoolDT.m_CardDT.szName;
            string borderName = UITool.f_GetImporentColorName(data.m_CardPoolDT.m_CardDT.iImportant, ref cardName);

            go.transform.Find("SprBorder").GetComponent<UISprite>().spriteName = borderName;
        }
    }
    /// <summary>
    /// 根据选中的位置获取左侧卡牌按钮
    /// </summary>
    /// <param name="emFormationPos">卡牌位置</param>
    /// <returns></returns>
    private GameObject GetCardBtnByIndex(EM_FormationPos emFormationPos)
    {
        GameObject cardBtn = null;
        switch (emFormationPos)
        {
            case EM_FormationPos.eFormationPos_Main:
                cardBtn = f_GetObject("BtnCardMain");
                break;
            case EM_FormationPos.eFormationPos_Assist1:
                cardBtn = f_GetObject("BtnCardAssist1");
                break;
            case EM_FormationPos.eFormationPos_Assist2:
                cardBtn = f_GetObject("BtnCardAssist2");
                break;
            case EM_FormationPos.eFormationPos_Assist3:
                cardBtn = f_GetObject("BtnCardAssist3");
                break;
            case EM_FormationPos.eFormationPos_Assist4:
                cardBtn = f_GetObject("BtnCardAssist4");
                break;
            case EM_FormationPos.eFormationPos_Assist5:
                cardBtn = f_GetObject("BtnCardAssist5");
                break;
            case EM_FormationPos.eFormationPos_Assist6:
                cardBtn = f_GetObject("BtnCardAssist6");
                break;
            case EM_FormationPos.eFormationPos_Pet:
                cardBtn = f_GetObject("BtnCardPet");
                break;
            case EM_FormationPos.eFormationPos_Reinforce:
                cardBtn = f_GetObject("BtnCardReinforce");
                break;
        }
        return cardBtn;
    }
    /// <summary>
    /// 初始化阵容数据
    /// </summary>
    private void InitTeamData()
    {
        List<BasePoolDT<long>> TeamBasePoolDT = Data_Pool.m_TeamPool.f_GetAll();
        dicTeamPoolDT.Clear();
        for (int i = 0; i < TeamBasePoolDT.Count; i++)
        {
            TeamPoolDT data = (TeamPoolDT)TeamBasePoolDT[i];
            dicTeamPoolDT.Add(data.m_eFormationPos, data);
            SetCardBtnContent(GetCardBtnByIndex(data.m_eFormationPos), data);
        }
        int EM_NeedLevelVlaue = (int)EM_NeedLevel.OpenTwoCardLevel - 1;
        for (int i = 1; i < GameParamConst.MAX_FIGHT_POS; i++)
        {
            int index = EM_NeedLevelVlaue + i;
            if (i >= 6)
            {
                index = 130 + i-6;
            }
            Transform go = GetCardBtnByIndex((EM_FormationPos)i).transform;
            //go.Find("Plus").gameObject.SetActive(UITool.f_GetIsOpensystem((EM_NeedLevel)index));
            go.Find("NeedLevel").gameObject.SetActive(!UITool.f_GetIsOpensystem((EM_NeedLevel)index));
            if (!UITool.f_GetIsOpensystem((EM_NeedLevel)index))
            {
                switch ((EM_NeedLevel)index)
                {
                    case EM_NeedLevel.OpenTwoCardLevel:
                        go.Find("NeedLevel").GetComponent<UILabel>().text = string.Format("{0}", UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenTwoCardLevel));
                        break;
                    case EM_NeedLevel.OpenThereCardLevel:
                        go.Find("NeedLevel").GetComponent<UILabel>().text = string.Format("{0}", UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenThereCardLevel));
                        break;
                    case EM_NeedLevel.OpenFourCardLevel:
                        go.Find("NeedLevel").GetComponent<UILabel>().text = string.Format("{0}", UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenFourCardLevel));
                        break;
                    case EM_NeedLevel.OpenFiveCardLevel:
                        go.Find("NeedLevel").GetComponent<UILabel>().text = string.Format("{0}", UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenFiveCardLevel));
                        break;
                    case EM_NeedLevel.OpenSixCardLevel:
                        go.Find("NeedLevel").GetComponent<UILabel>().text = string.Format("{0}", UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenSixCardLevel));
                        break;
                    case EM_NeedLevel.Open7CardLevel:
                        go.Find("NeedLevel").GetComponent<UILabel>().text = string.Format("{0}", UITool.f_GetSysOpenLevel(EM_NeedLevel.Open7CardLevel));
                        break;
                }
            }
        }

        //根据参数表设置一键装备是否可见
        UIGrid btnGrid = f_GetObject("BtnStrengthRoot").GetComponent<UIGrid>();
        f_GetObject("BtnEquipQuick").SetActive(UITool.f_GetIsOpensystem(EM_NeedLevel.EquipQuick));
        btnGrid.repositionNow = true;

        f_GetObject("ModelHero").SetActive(true);
        f_GetObject("TexHeroInfoBg").SetActive(true);
        f_GetObject("BtnStrengthRoot").SetActive(true);
        f_GetObject("Reinforce").SetActive(false);
        UpdateModelData(CurrentSelectCardPos);
        f_GetObject("CardProgressBar").GetComponent<UIProgressBar>().value = (int)CurrentSelectCardPos * 1.0f / (int)EM_FormationPos.eFormationPos_Reinforce;
        if (Data_Pool.m_GuidancePool.IsEnter)
        {
            if (Data_Pool.m_GuidancePool.IGuidanceID == 2040 || Data_Pool.m_GuidancePool.IGuidanceID == 2041 || Data_Pool.m_GuidancePool.IGuidanceID == 2000)
            {
                f_GetObject("CardProgressBar").GetComponent<UIProgressBar>().value = 1;
            }
        }
        UpdateFate();
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        f_RegClickEvent("BtnClothArray", OnBtnClothArrayClick);
        f_RegClickEvent("ModelHero", OnModelHeroClick);//卡牌角色，点击卡牌进入卡牌详细信息界面
        //卡牌按钮事件
        f_RegClickEvent("BtnCardMain", OnCardClick, EM_FormationPos.eFormationPos_Main);
        f_RegClickEvent("BtnCardAssist1", OnCardClick, EM_FormationPos.eFormationPos_Assist1);
        f_RegClickEvent("BtnCardAssist2", OnCardClick, EM_FormationPos.eFormationPos_Assist2);
        f_RegClickEvent("BtnCardAssist3", OnCardClick, EM_FormationPos.eFormationPos_Assist3);
        f_RegClickEvent("BtnCardAssist4", OnCardClick, EM_FormationPos.eFormationPos_Assist4);
        f_RegClickEvent("BtnCardAssist5", OnCardClick, EM_FormationPos.eFormationPos_Assist5);
        f_RegClickEvent("BtnCardAssist6", OnCardClick, EM_FormationPos.eFormationPos_Assist6);
        f_RegClickEvent("BtnCardPet", OnCardClick, EM_FormationPos.eFormationPos_Pet);
        //f_RegClickEvent("BtnCardReinforce", OnCardClick, EM_FormationPos.eFormationPos_Reinforce);// đóng tính năng đồng hành k dùng đến
        f_RegClickEvent("BtnStrengthQuick", OnBtnOneInten);
        f_RegClickEvent("BtnStrengthMaster", OnStrenthMaster);
        f_RegClickEvent("BtnEquipQuick", OnEquipQuick);
        f_RegClickEvent("BtnCardPropertyDetail", OnBtnCardPropertyDetailClick);
        f_RegClickEvent("BtnFateDetail", OnBtnFateDetailClick);
        //装备点击事件
        f_RegClickEvent("BtnHelmet", OnEquipClick, EM_EquipPart.eEquipPart_Helmet);//头盔
        f_RegClickEvent("BtnWeapon", OnEquipClick, EM_EquipPart.eEquipPart_Weapon);//武器
        f_RegClickEvent("BtnBelt", OnEquipClick, EM_EquipPart.eEquipPart_Belt);//腰带
        f_RegClickEvent("BtnArmour", OnEquipClick, EM_EquipPart.eEquipPart_Armour);//铠甲
        f_RegClickEvent("BtnMagicLeft", OnEquipClick, EM_EquipPart.eEquipPare_MagicLeft);//法宝左
        f_RegClickEvent("BtnMagicRight", OnEquipClick, EM_EquipPart.eEquipPare_MagicRight);//法宝右

        f_RegClickEvent("BtnGodWeapon", OnEquipClick, EM_EquipPart.eEquipPart_GodWeapon);//than binh


        f_RegClickEvent("BtnForceB1", OnForceItemClick, EM_ReinforcePos.eReinforcePos_B1);//援军位点击
        f_RegClickEvent("BtnForceB2", OnForceItemClick, EM_ReinforcePos.eReinforcePos_B2);
        f_RegClickEvent("BtnForceB3", OnForceItemClick, EM_ReinforcePos.eReinforcePos_B3);
        f_RegClickEvent("BtnForceB4", OnForceItemClick, EM_ReinforcePos.eReinforcePos_B4);
        f_RegClickEvent("BtnForceB5", OnForceItemClick, EM_ReinforcePos.eReinforcePos_B5);
        f_RegClickEvent("BtnForceB6", OnForceItemClick, EM_ReinforcePos.eReinforcePos_B6);
        f_RegClickEvent("BtnViewFateEffect", OnViewFateEffectClick);
        f_RegClickEvent("BtnShowFateReturn", OnShowFateReturnClick);
        //点击技能icon
        f_RegClickEvent("SprNorAttackBg", OnSprNorAttackBgClick);
        f_RegClickEvent("SprAngerAttackBg", OnSprAngerAttackBgClick);
        f_RegClickEvent("SprFixAttackBg", OnSprFixAttackBgClick);

        f_RegClickEvent("BtnEquip0", OnEquipNewClick,0);
        f_RegClickEvent("BtnEquip1", OnEquipNewClick,1);
        f_RegClickEvent("BtnEquip2", OnEquipNewClick,2);
        f_RegClickEvent("BtnEquip3", OnEquipNewClick,3);

    }
    /// <summary>
    /// 销毁
    /// </summary>
    protected override void On_Destory()
    {
        base.On_Destory();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_MODELINFOR, UpdateModelView, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_FATE_TRIP, UpdateFateTrip, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_ClothArrayChanged, ShowForceModelContent, this);
    }
    /// <summary>
    /// 更新模型（上阵完卡牌，装备完装备或法宝更新UI事件）
    /// </summary>
    private void UpdateModelView(object data)
    {
        if (dicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
        {
            tBefore = new int[4];
            for (int i = 0; i < tBefore.Length; i++)
                tBefore[i] = tLastPro[i];
            tBeforeHp = tLastHp;
            UpdateCardBtnState(CurrentSelectCardPos);//设置左侧卡牌按钮选中状态
            UpdateModelData(CurrentSelectCardPos);
        }
    }
    /// <summary>
    /// 更新缘分消息
    /// </summary>
    private void UpdateFateTrip(object data)
    {
        if (dicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
        {
            FateTripParam tFateTrip;
            tMasterDT = new int[4];
            for (int i = 0; i < tBefore.Length; i++)
            {
                tBefore[i] = tLastPro[i] - tBefore[i];
                tMasterDT[i] = Data_Pool.m_TeamPool.f_GetMasterLevel((EM_Master)i + 1, CurrentSelectCardPos);
            }
            tBeforeHp = tLastHp - tBeforeHp;
            if (data != null)
                tFateTrip = new FateTripParam(dicTeamPoolDT[CurrentSelectCardPos], tBefore, tBeforeHp, tMasterDT);
            else
                tFateTrip = new FateTripParam(dicTeamPoolDT[CurrentSelectCardPos], new int[4], 0, tMasterDT);


            ccUIManage.GetInstance().f_SendMsg(UINameConst.FateTrip, UIMessageDef.UI_OPEN, tFateTrip);
            UpdateFate();//更新缘分
        }
    }
    /// <summary>
    /// 更新金钱英雄等UI信息
    /// </summary>
    private void InitMoneyUI()
    {
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }
    #region 按钮事件
    /// <summary>
    /// 点击武将详细属性加成界面（图中感叹号按钮）
    /// </summary>
    private void OnBtnCardPropertyDetailClick(GameObject go, object obj1, object obj2)
    {
        if (dicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
        {
            TeamPoolDT data = dicTeamPoolDT[CurrentSelectCardPos];
            RoleProperty tmpRole = RolePropertyTools.f_Disp(data.m_CardPoolDT, new List<EquipPoolDT>(data.m_aEquipPoolDT), Data_Pool.m_BattleFormPool.iDestinyProgress,
                new List<TreasurePoolDT>(data.m_aTreamPoolDT));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CardPropertyDetailPage, UIMessageDef.UI_OPEN, tmpRole);
        }
    }
    /// <summary>
    /// 点击右侧缘分详细信息
    /// </summary>
    private void OnBtnFateDetailClick(GameObject go, object obj1, object obj2)
    {
        if (dicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.FateIntroDetailPage, UIMessageDef.UI_OPEN, CurrentSelectCardPos);
        }
    }
    /// <summary>
    /// 点击左侧栏卡片按钮事件
    /// </summary>
    private void OnCardClick(GameObject go, object obj1, object obj2)
    {

        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        switch ((EM_FormationPos)obj1)
        {
            case EM_FormationPos.eFormationPos_Main:
                break;
            case EM_FormationPos.eFormationPos_Assist1:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.OpenTwoCardLevel))
                    return;
                break;
            case EM_FormationPos.eFormationPos_Assist2:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.OpenThereCardLevel))
                    return;
                break;
            case EM_FormationPos.eFormationPos_Assist3:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.OpenFourCardLevel))
                    return;
                break;
            case EM_FormationPos.eFormationPos_Assist4:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.OpenFiveCardLevel))
                    return;
                break;
            case EM_FormationPos.eFormationPos_Assist5:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.OpenSixCardLevel))
                    return;
                break;
            case EM_FormationPos.eFormationPos_Assist6:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.OpenSixCardLevel))
                    return;
                break;
            case EM_FormationPos.eFormationPos_Pet:
                break;
            //case EM_FormationPos.eFormationPos_Reinforce:// đóng tính năng đồng hành k dùng đến
            //    if (!UITool.f_GetIsOpensystem(EM_NeedLevel.OpenReinforceLevel))
            //    {
            //        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(62), UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenReinforceLevel)));
            //        return;
            //    }
            //    break;
            case EM_FormationPos.eFormationPos_INVALID:
                break;
            default:
                break;
        }
        CurrentSelectCardPos = (EM_FormationPos)obj1;//隐藏其他选中状态
        UpdateCardBtnState(CurrentSelectCardPos);//设置左侧卡牌按钮选中状态
        if (CurrentSelectCardPos == EM_FormationPos.eFormationPos_Pet)
        {
            f_GetObject("ModelHero").SetActive(false);
            f_GetObject("TexHeroInfoBg").SetActive(false);
            f_GetObject("BtnStrengthRoot").SetActive(false);
            f_GetObject("Reinforce").SetActive(false);
        }
        //else if (CurrentSelectCardPos == EM_FormationPos.eFormationPos_Reinforce) // đóng tính năng đồng hành k dùng đến
        //{
        //    f_GetObject("ModelHero").SetActive(false);
        //    f_GetObject("TexHeroInfoBg").SetActive(false);
        //    f_GetObject("BtnStrengthRoot").SetActive(false);
        //    f_GetObject("Reinforce").SetActive(true);
        //    ShowForceModelContent(null);//更新援军
        //}
        else
        {
			//MessageBox.ASSERT("Id: " + dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardDT.iId);		
            f_GetObject("ModelHero").SetActive(true);
            f_GetObject("TexHeroInfoBg").SetActive(true);
            f_GetObject("BtnStrengthRoot").SetActive(true);
            f_GetObject("Reinforce").SetActive(false);
			
			int Lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
			int tRandom = UnityEngine.Random.Range(1, 3);
			try
			{
			  if (Lv >= 4)
				{
					id = "" + dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardDT.iId;
					//MessageBox.ASSERT("Id: " + id);
					if (id != "")
						glo_Main.GetInstance().m_AdudioManager.f_PlayAudioVoice(id + "" + tRandom);
				}
			}
			catch (Exception e)
			{
				//MessageBox.ASSERT(e.Message);
				UpdateModelData(CurrentSelectCardPos);//更新阵容数据
				UpdateFate();//更新缘分
			}
			
            UpdateModelData(CurrentSelectCardPos);//更新阵容数据
            UpdateFate();//更新缘分
        }
    }
    /// <summary>
    /// 点击装备按钮
    /// </summary>
    private void OnEquipClick(GameObject go, object obj1, object obj2)
    {
        if (!dicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
            return;
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        CurrentSelectEquipPart = (EM_EquipPart)obj1;
        if (dicTeamPoolDT[CurrentSelectCardPos].m_aEqupId[(int)CurrentSelectEquipPart - 1] != 0)//已经装备
        {
            switch ((EM_EquipPart)obj1)
            {
                case EM_EquipPart.eEquipPart_Weapon:
                case EM_EquipPart.eEquipPart_Armour:
                case EM_EquipPart.eEquipPart_Helmet:
                case EM_EquipPart.eEquipPart_Belt:
                    EquipBox tmp = new EquipBox();
                    tmp.tEquipPoolDT = dicTeamPoolDT[CurrentSelectCardPos].m_aEquipPoolDT[(int)CurrentSelectEquipPart - 1];
                    if (tmp.tEquipPoolDT == null)
                    {
                        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(63));
                        return;
                    }
                    tmp.tType = EquipBox.BoxTye.Intro;
                    tmp.m_TeamPool = dicTeamPoolDT[CurrentSelectCardPos];
                    ccUIHoldPool.GetInstance().f_Hold(this);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.EquipManage, UIMessageDef.UI_OPEN, tmp);
                    break;
                case EM_EquipPart.eEquipPare_MagicLeft:
                case EM_EquipPart.eEquipPare_MagicRight:
                    TreasureBox tmpT = new TreasureBox();
                    tmpT.tTreasurePoolDT = dicTeamPoolDT[CurrentSelectCardPos].m_aTreamPoolDT[(int)CurrentSelectEquipPart - 5];
                    if (tmpT.tTreasurePoolDT == null)
                    {
                        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(64));
                        return;
                    }
                    tmpT.tType = TreasureBox.BoxType.Intro;
                    tmpT.m_TeamPoolDT = dicTeamPoolDT[CurrentSelectCardPos];
                    ccUIHoldPool.GetInstance().f_Hold(this);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureManage, UIMessageDef.UI_OPEN, tmpT);
                    break;
                case EM_EquipPart.eEquipPart_GodWeapon:
                    GodEquipBox tmpG = new GodEquipBox();
                    tmpG.tEquipPoolDT = dicTeamPoolDT[CurrentSelectCardPos].m_aGodEquipPoolDT[(int)CurrentSelectEquipPart - 7];
                    if (tmpG.tEquipPoolDT == null)
                    {
                        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(2300));
                        return;
                    }
                    tmpG.tType = GodEquipBox.BoxTye.Intro;
                    tmpG.m_TeamPool = dicTeamPoolDT[CurrentSelectCardPos];
                    ccUIHoldPool.GetInstance().f_Hold(this);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.GodEquipManage, UIMessageDef.UI_OPEN, tmpG);
                    break;
            }
        }
        else//没有装备
        {
            //没有装备需要弹出获取途径
            if (CurrentSelectEquipPart == EM_EquipPart.eEquipPare_MagicLeft || CurrentSelectEquipPart == EM_EquipPart.eEquipPare_MagicRight)
            {
                int openLevel = UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel);
                int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
                if (userLevel < openLevel)//提示法宝未开放
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(65), openLevel));
                }
                else if (UITool.f_CheckHasEquipLeft(CurrentSelectEquipPart))//检测是否还有剩余可装备的法宝
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectEquipPage, UIMessageDef.UI_OPEN);
                }
                else//前往夺宝界面获取
                {
                    UITool.f_GotoPage(this, UINameConst.GrabTreasurePage, 0);
                }

            }
            else if (CurrentSelectEquipPart == EM_EquipPart.eEquipPart_GodWeapon)
            {
                int openLevel = UITool.f_GetSysOpenLevel(EM_NeedLevel.GodEquip);
                int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);

                GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(117);
                //param.iParam1 = 50; test
                if (userLevel < openLevel)//Nhắc rằng nhắc rằng thần binh chưa mở
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(2275), openLevel));

                }else if (!CommonTools.f_CheckOpenServerDay(param.iParam1))
                {
                    string message = string.Format(CommonTools.f_GetTransLanguage(2297), param.iParam1);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, message);
                }
                else if (UITool.f_CheckHasEquipLeft(CurrentSelectEquipPart))//Kiểm tra xem còn thần binh nào để trang bị không
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectEquipPage, UIMessageDef.UI_OPEN);
                }
                else//Vào giao diện Săn tìm thần binh để lấy
                {
                    //UITool.f_GotoPage(this, UINameConst.GrabGodEquipPage, 0);
                }
            }
            else
            {
                if (UITool.f_CheckHasEquipLeft(CurrentSelectEquipPart))//检测是否还有剩余可装备的装备
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectEquipPage, UIMessageDef.UI_OPEN);
                else
                {
                    //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "你没有任何可穿戴的此类装备！");
                    int equipId = GetGreenEquipId(CurrentSelectEquipPart);
                    if (equipId > 0)
                    {
                        StaticValue.mGetWayToBattleParam.f_UpdateDataInfo(EM_GetWayToBattle.LineUpPage, EM_ResourceType.Equip, equipId);
                        GetWayPageParam getWayPageParam = new GetWayPageParam(EM_ResourceType.Equip, equipId, this);
                        ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, getWayPageParam);
                    }
                }
            }

        }
    }
    /// <summary>
    /// 获取绿装物品id
    /// </summary>
    /// <param name="equipPart">装备部位</param>
    /// <returns></returns>
    private int GetGreenEquipId(EM_EquipPart equipPart)
    {
        if (equipPart == EM_EquipPart.eEquipPare_MagicLeft || equipPart == EM_EquipPart.eEquipPare_MagicRight)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(66));
            return 0;
        }
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_EquipSC.f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            EquipDT equipDT = listData[i] as EquipDT;
            if (equipDT.iSite == (int)equipPart && equipDT.iColour == (int)EM_Important.Green)
            {
                return equipDT.iId;
            }
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(67));
        return 0;
    }
    /// <summary>
    /// 返回按钮事件
    /// </summary>
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LineUpPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    /// <summary>
    /// 点击布阵按钮触发事件
    /// </summary>
    private void OnBtnClothArrayClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 强化大师
    /// </summary>
    private void OnStrenthMaster(GameObject go, object boj1, object obj2)
    {
        if (!dicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
            return;
        TeamPoolDT teamPoolDT = dicTeamPoolDT[CurrentSelectCardPos];
        bool isEquip = true;
        bool isTreasure = true;
        for (int i = 0; i < teamPoolDT.m_aEquipPoolDT.Length; i++)
        {
            if (teamPoolDT.m_aEquipPoolDT[i] == null)
                isEquip = false;
        }
        for (int i = 0; i < teamPoolDT.m_aTreamPoolDT.Length; i++)
        {
            if (teamPoolDT.m_aTreamPoolDT[i] == null)
                isTreasure = false;
        }
        if (!isEquip && !isTreasure)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(68));
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.StrengthenMasterPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击普攻技能icon
    /// </summary>
    private void OnSprNorAttackBgClick(GameObject go, object boj1, object obj2)
    {
        if (dicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
        {
            MagicDT[] tmpMagic = UITool.f_GetCardMagic(dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardDT);
            if (tmpMagic.Length >= 1 && tmpMagic[0] != null)
            {
                //string intro = "[F1B049]" + tmpMagic[0].szName + ":[C2B8A7]" + tmpMagic[0].szReadme;
                int cardId =  dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardDT.iId;
                object[] infoMagic = new object[2] { tmpMagic[0], cardId };
                ccUIManage.GetInstance().f_SendMsg(UINameConst.SkillIntroDetailPage, UIMessageDef.UI_OPEN, infoMagic);
            }
        }
    }
    /// <summary>
    /// 点击怒攻技能icon
    /// </summary>
    private void OnSprAngerAttackBgClick(GameObject go, object boj1, object obj2)
    {
        if (dicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
        {
            MagicDT[] tmpMagic = UITool.f_GetCardMagic(dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardDT);
            if (tmpMagic.Length >= 2 && tmpMagic[1] != null)
            {
                //string intro = "[F1B049]" + tmpMagic[1].szName + ":[C2B8A7]" + tmpMagic[1].szReadme;
                int cardId = dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardDT.iId;
                object[] infoMagic = new object[2] { tmpMagic[1], cardId };
                ccUIManage.GetInstance().f_SendMsg(UINameConst.SkillIntroDetailPage, UIMessageDef.UI_OPEN, infoMagic);
            }
        }
    }
    private static bool checkCardTempIdIsLineUp(int cardTempId)
    {
        List<BasePoolDT<long>> aData = Data_Pool.m_TeamPool.f_GetAll();
        for (int i = 0; i < aData.Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)aData[i];
            if (tTeamPoolDT.m_eFormationPos == CurrentSelectCardPos)//去除当前选中的位置
                continue;
            if (tTeamPoolDT.m_CardPoolDT.m_CardDT.iId == cardTempId)
                return true;
        }
        return false;
    }

    private static bool CheckJointAttack(CardDT cardDT)
    {
        List<MagicDT> tmpMagic = new List<MagicDT>(UITool.f_GetCardMagic(cardDT));
        //加上已上阵的
        List<BasePoolDT<long>> aData = Data_Pool.m_TeamPool.f_GetAll();
        for (int i = 0; i < aData.Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)aData[i];
            if (tTeamPoolDT.m_eFormationPos == CurrentSelectCardPos)//去除当前选中的位置
                continue;
            tmpMagic.AddRange(UITool.f_GetCardMagic(tTeamPoolDT.m_CardPoolDT.m_CardDT));
        }
        for (int i = 0; i < tmpMagic.Count; i++)
        {
            if (tmpMagic[i] != null && tmpMagic[i].iClass == 3)//合击
            {
                int iGroup1 = tmpMagic[i].iGroupHero1;
                int iGroup2 = tmpMagic[i].iGroupHero2;
                int iGroup3 = tmpMagic[i].iGroupHero3;
                if (iGroup1 <= 0 && iGroup2 <= 0 && iGroup3 <= 0)//都为0表示没有合击
                {
                    continue;
                }
                if (iGroup1 != cardDT.iId && iGroup2 != cardDT.iId && iGroup3 != cardDT.iId)//当前卡牌必须要在里面
                {
                    continue;
                }
                if (iGroup1 > 0 && iGroup1 != cardDT.iId && !checkCardTempIdIsLineUp(iGroup1))
                {
                    continue;
                }
                if (iGroup2 > 0 && iGroup2 != cardDT.iId && !checkCardTempIdIsLineUp(iGroup2))
                {
                    continue;
                }
                if (iGroup3 > 0 && iGroup3 != cardDT.iId && !checkCardTempIdIsLineUp(iGroup3))
                {
                    continue;
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 点击合击技能icon
    /// </summary>
    private void OnSprFixAttackBgClick(GameObject go, object boj1, object obj2)
    {
        if (dicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
        {
            MagicDT[] tmpMagic = UITool.f_GetCardMagic(dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardDT);
            if (tmpMagic.Length >= 3 && tmpMagic[2] != null)
            {
                //string intro = "[F1B049]" + tmpMagic[2].szName + ":[C2B8A7]" + tmpMagic[2].szReadme;
                int cardId = dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardDT.iId;
                object[] infoMagic = new object[2] { tmpMagic[2], cardId };
                ccUIManage.GetInstance().f_SendMsg(UINameConst.SkillIntroDetailPage, UIMessageDef.UI_OPEN, infoMagic);
            }
        }
    }
    private void OnEquipNewClick(GameObject go, object boj1, object obj2)
    {
        UITool.Ui_Trip("Chưa mở!");
    }
    
    #endregion
    /// <summary>
    /// 设置卡牌按钮状态
    /// </summary>
    private void UpdateCardBtnState(EM_FormationPos emFormationPos)
    {
        bool HasCardLeft = Data_Pool.m_CardPool.f_CheckHasCardLeft();


        for (int i = 0; i < (int)EM_FormationPos.eFormationPos_INVALID; i++)
        {
            if (GetCardBtnByIndex((EM_FormationPos)i) == null)
            {
                continue;
            }
            bool redPoint = false;
            //设置红点
            //if ((EM_FormationPos)i == EM_FormationPos.eFormationPos_Pet)
            //    ;
            //else 
            if ((EM_FormationPos)i == EM_FormationPos.eFormationPos_Reinforce)
                redPoint = Data_Pool.m_TeamPool.f_CheckForceRedPoint();
            else
            {
                if (dicTeamPoolDT.ContainsKey((EM_FormationPos)i))
                    redPoint = Data_Pool.m_TeamPool.f_CheckTeamPoolDTRedPoint(dicTeamPoolDT[(EM_FormationPos)i], (EM_FormationPos)i);
                else
                    redPoint = Data_Pool.m_TeamPool.f_CheckTeamPoolDTRedPoint(null, (EM_FormationPos)i);
            }
            int num = redPoint ? 1 : 0;
			if (!UITool.f_GetIsOpensystem(EM_NeedLevel.OpenReinforceLevel) && i == 7)
            {
			}
			else
			{
				UITool.f_UpdateReddot(GetCardBtnByIndex((EM_FormationPos)i), num, new Vector3(50, 50, 0), 102);
			}
			//MessageBox.ASSERT("index: " + i + " " + GetCardBtnByIndex((EM_FormationPos)i));
            GetCardBtnByIndex((EM_FormationPos)i).transform.Find("SelectEffect").gameObject.SetActive(true);
            GetCardBtnByIndex((EM_FormationPos)i).transform.Find("SelectEffect").GetComponent<UISprite>().color
                = emFormationPos == (EM_FormationPos)i ? Color.white : new Color(1, 1, 1, 0);
        }
    }
    /// <summary>
    /// 通过装备位获取对应装备按钮
    /// </summary>
    /// <param name="emEquipPart">装备部位</param>
    /// <returns></returns>
    private GameObject GetEquipBtnByIndex(EM_EquipPart emEquipPart)
    {
        GameObject EquipBtn = null;
        switch (emEquipPart)
        {
            case EM_EquipPart.eEquipPart_Weapon:
                EquipBtn = f_GetObject("BtnWeapon");
                break;
            case EM_EquipPart.eEquipPart_Armour:
                EquipBtn = f_GetObject("BtnArmour");
                break;
            case EM_EquipPart.eEquipPart_Helmet:
                EquipBtn = f_GetObject("BtnHelmet");
                break;
            case EM_EquipPart.eEquipPart_Belt:
                EquipBtn = f_GetObject("BtnBelt");
                break;
            case EM_EquipPart.eEquipPare_MagicLeft:
                EquipBtn = f_GetObject("BtnMagicLeft");
                break;
            case EM_EquipPart.eEquipPare_MagicRight:
                EquipBtn = f_GetObject("BtnMagicRight");
                break;
            case EM_EquipPart.eEquipPart_GodWeapon:
                EquipBtn = f_GetObject("BtnGodWeapon");
                break;
        }
        return EquipBtn;
    }
    /// <summary>
    /// 通过装备位获取对应装备按钮
    /// </summary>
    /// <param name="emEquipPart">装备部位</param>
    /// <returns></returns>
    private string GetEquipNameBtnByIndex(EM_EquipPart emEquipPart)
    {
        string EquipName = null;
        switch (emEquipPart)
        {
            case EM_EquipPart.eEquipPart_Weapon:
                EquipName = CommonTools.f_GetTransLanguage(69);
                break;
            case EM_EquipPart.eEquipPart_Armour:
                EquipName = CommonTools.f_GetTransLanguage(70);
                break;
            case EM_EquipPart.eEquipPart_Helmet:
                EquipName = CommonTools.f_GetTransLanguage(71);
                break;
            case EM_EquipPart.eEquipPart_Belt:
                EquipName = CommonTools.f_GetTransLanguage(72);
                break;
            case EM_EquipPart.eEquipPare_MagicLeft:
                EquipName = CommonTools.f_GetTransLanguage(73);
                break;
            case EM_EquipPart.eEquipPare_MagicRight:
                EquipName = CommonTools.f_GetTransLanguage(73);
                break;
            case EM_EquipPart.eEquipPart_GodWeapon:
                EquipName = CommonTools.f_GetTransLanguage(2298);
                break;
            default: goto case EM_EquipPart.eEquipPart_Weapon;
        }
        return EquipName;
    }
    /// <summary>
    /// 当前选中位有卡牌上阵时，显示某些显示，否则关闭部位显示
    /// </summary>
    /// <param name="isContainKey">该位置是否有上阵卡牌</param>
    private void ShowContentAct(bool isContainKey)
    {
        if (null == f_GetObject("SprFight"))
            return;
        f_GetObject("SprFight").SetActive(isContainKey);
        f_GetObject("IconNoCard").SetActive(!isContainKey);
        f_GetObject("SprProporty").SetActive(isContainKey);//属性
        f_GetObject("SprFetters").SetActive(isContainKey);//缘分
        f_GetObject("SprNorAttackBg").transform.Find("Icon").gameObject.SetActive(isContainKey);//技能icon栏
        f_GetObject("SprAngerAttackBg").transform.Find("Icon").gameObject.SetActive(isContainKey);//技能icon栏
        f_GetObject("SprFixAttackBg").transform.Find("Icon").gameObject.SetActive(isContainKey);//技能icon栏
                                                                                                //f_GetObject("SprFixAttackEffect").transform.gameObject.SetActive();

        if (!isContainKey)
        {
            for (int i = 1; i <= 6; i++)//如果该位置没有卡牌设置装备为空
            {
                GameObject EquipObj = GetEquipBtnByIndex((EM_EquipPart)i);
                UI2DSprite spriteIcon3 = EquipObj.transform.Find("Icon").GetComponent<UI2DSprite>();
                spriteIcon3.gameObject.SetActive(false);
                EquipObj.transform.Find("IconBorder").gameObject.SetActive(false);
                EquipObj.transform.Find("LabelName").GetComponent<UILabel>().text = "";
                f_GetObject("LabelCardName").GetComponent<UILabel>().text = "";
                f_GetObject("LabelCardLevel").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(74);
                f_GetObject("LabelCardQualification").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(75);
                for (int j = EquipObj.transform.Find("Effect").childCount - 1; j >= 0; j--)//删除特效
                    Destroy(EquipObj.transform.Find("Effect").GetChild(j).gameObject);
            }
        }
    }
    /// <summary>
    /// 更新模型数据
    /// </summary>
    /// <param name="index">选中的卡牌部位</param>
    private void UpdateModelData(EM_FormationPos index)
    {
        if (index == EM_FormationPos.eFormationPos_Reinforce)
        {
            //ShowForceModelContent(null);s
            UpdateModelData(EM_FormationPos.eFormationPos_Main);
            OnCardClick(null, EM_FormationPos.eFormationPos_Reinforce, null);
            return;
        }
        bool isContainKey = dicTeamPoolDT.ContainsKey(index);//该位置是否有卡牌上阵
        UpdateCardBtnState(index);//设置左侧卡牌按钮选中状态
        ShowContentAct(isContainKey);

        GameObject ModelPoint = f_GetObject("ModelPoint");
        if (null == ModelPoint)
            return;
        ModelPoint.SetActive(isContainKey);
        if (ModelPoint.transform.Find("Model") != null)//删除旧的卡牌模型
            UITool.f_DestoryStatelObject(ModelPoint.transform.Find("Model").gameObject);
        RoleProperty tmpRole = new RoleProperty();
        if (isContainKey)
        {
            TeamPoolDT data = dicTeamPoolDT[index];
			//Icon Skill
			//skillId = "" + dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardDT.iId;//id tướng
            MagicDT[] tmpMagic = UITool.f_GetCardMagic(dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardDT);
            f_GetObject("SprNorAttackBg").transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetSkillIcon(tmpMagic[0].iId.ToString());
            f_GetObject("SprAngerAttackBg").transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetSkillIcon(tmpMagic[1].iId.ToString());
            f_GetObject("SprFixAttackBg").transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = tmpMagic[2] != null ? UITool.f_GetSkillIcon(tmpMagic[2].iId.ToString()) : null;// UITool.f_GetSkillIcon(tmpMagic[2] != null ? tmpMagic[2].iId.ToString() : "");
            //f_GetObject("SprNorAttackBg").transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetSkillIcon(skillId + 1);
            //f_GetObject("SprAngerAttackBg").transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetSkillIcon(skillId + 2);
            //f_GetObject("SprFixAttackBg").transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetSkillIcon(skillId + 3);
            //
            tmpRole = RolePropertyTools.f_Disp(data.m_CardPoolDT, new List<EquipPoolDT>(data.m_aEquipPoolDT),
                Data_Pool.m_BattleFormPool.iDestinyProgress, new List<TreasurePoolDT>(data.m_aTreamPoolDT),
                new List<GodEquipPoolDT>(data.m_aGodEquipPoolDT));
            for (int i = 0; i < tLastPro.Length; i++)
            {
                tLastPro[i] = CommonTools.f_GetPercentValueTenThousandInt32(tmpRole.f_GetProperty(i + 1), tmpRole.f_GetProperty(i + 5));
            }
            tLastHp = CommonTools.f_GetPercentValueTenThousandInt64(tmpRole.f_GetProperty((int)EM_RoleProperty.Hp), tmpRole.f_GetProperty((int)EM_RoleProperty.AddHp));
            f_GetObject("LabelFightValue").GetComponent<UILabel>().text = RolePropertyTools.f_GetBattlePower(tmpRole).ToString();
            int iEvolveLv = data.m_CardPoolDT.m_iEvolveLv;
            string cardName = data.m_CardPoolDT.m_CardDT.szName + (iEvolveLv > 0 ? ("+" + iEvolveLv) : "");
            if (index == EM_FormationPos.eFormationPos_Main)//如果是主卡，则显示的是玩家的名字
                cardName = Data_Pool.m_UserData.m_szRoleName + (iEvolveLv > 0 ? ("+" + iEvolveLv) : "");
            UITool.f_GetImporentColorName(data.m_CardPoolDT.m_CardDT.iImportant, ref cardName);
            cardName += UITool.f_GetFiveElementNameById(data.m_CardPoolDT.m_CardDT.iCardEle);
            f_GetObject("LabelCardName").GetComponent<UILabel>().text = cardName;
            f_GetObject("LabelCardLevel").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(74) + data.m_CardPoolDT.m_iLv;
            f_GetObject("LabelCardQualification").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(75) + data.m_CardPoolDT.m_CardDT.iImportant;
            InitEquipAndTreasureData(data);
            UITool.f_GetStatelObject(data.m_CardPoolDT, ModelPoint.transform, Vector3.zero, new Vector3(0f, -50f, 0f), 1, "Model", 180);
            CheckTeamPoolRedPoint(data, CurrentSelectCardPos == EM_FormationPos.eFormationPos_Main);

            //MagicDT[] tmpMagic = UITool.f_GetCardMagic(dicTeamPoolDT[index].m_CardPoolDT.m_CardDT);
            f_GetObject("SprNorAttackBg").SetActive(tmpMagic.Length >= 1 && tmpMagic[0] != null);
            f_GetObject("SprAngerAttackBg").SetActive(tmpMagic.Length >= 2 && tmpMagic[1] != null);
            f_GetObject("SprFixAttackBg").SetActive(tmpMagic.Length >= 3 && tmpMagic[2] != null);
            bool hasJoinAttack = CheckJointAttack(dicTeamPoolDT[index].m_CardPoolDT.m_CardDT);
            f_GetObject("SprFixAttackBg").transform.Find("SprFixAttackEffect").gameObject.SetActive(false);
            //tmpMagic[2].iGroupHero1
            f_GetObject("SkillIconRoot").GetComponent<UIGrid>().Reposition();
        }
        else
        {
            CloseEquipRedPoint();
            InitEquipAndTreasureData(null, false);
            bool HasCard = Data_Pool.m_CardPool.f_CheckHasCardLeft();
            int openLevel = UITool.f_GetTeamPosOpenLevel(index);
            int Lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            bool isOpen = Lv >= openLevel;
            f_GetObject("CardRedPointHint").SetActive(HasCard && isOpen);
            f_GetObject("CardRedPointHint").GetComponentInChildren<UILabel>().text = (HasCard && isOpen) ? CommonTools.f_GetTransLanguage(76) : "";
        }

        f_GetObject("LabelHPValue").GetComponent<UILabel>().text = tmpRole.f_GetHp().ToString(); //CommonTools.f_GetPercentValueTenThousandInt64(tmpRole.f_GetHp(),
                                                                                                 //tmpRole.f_GetProperty((int)EM_RoleProperty.AddHp)).ToString();
        f_GetObject("LabelAttackValue").GetComponent<UILabel>().text = tmpRole.f_GetProperty((int)EM_RoleProperty.Atk).ToString();// CommonTools.f_GetPercentValueTenThousandStr(tmpRole.f_GetProperty((int)EM_RoleProperty.Atk),
                                                                                                                                  //tmpRole.f_GetProperty((int)EM_RoleProperty.AddAtk)).ToString();
        f_GetObject("LabelDefenseValue").GetComponent<UILabel>().text = tmpRole.f_GetProperty((int)EM_RoleProperty.Def).ToString();// CommonTools.f_GetPercentValueTenThousandStr(tmpRole.f_GetProperty((int)EM_RoleProperty.Def),
                                                                                                                                   //tmpRole.f_GetProperty((int)EM_RoleProperty.AddDef)).ToString();
        f_GetObject("LabelMagDefenseValue").GetComponent<UILabel>().text = tmpRole.f_GetProperty((int)EM_RoleProperty.MDef).ToString();// CommonTools.f_GetPercentValueTenThousandStr(tmpRole.f_GetProperty((int)EM_RoleProperty.MDef),
                                                                                                                                       // tmpRole.f_GetProperty((int)EM_RoleProperty.AddMDef)).ToString();
    }
    /// <summary>
    /// 检查红点
    /// </summary>
    /// <param name="data"></param>
    private void CheckTeamPoolRedPoint(TeamPoolDT data, bool isMainCard)
    {
        //检查装备红点
        bool EquipCanLvUp = false;//有装备且可以升级
        bool EquipCanEquip = false;//无装备有新装备或更高品质
        Data_Pool.m_TeamPool.f_CheckTeamEquipRedPoint(data.f_GetEquipPoolDT(EM_Equip.eEquipPart_Weapon), EM_EquipPart.eEquipPart_Weapon, ref EquipCanLvUp, ref EquipCanEquip);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPart_Weapon), (EquipCanLvUp || EquipCanEquip) ? 1 : 0, new Vector3(40, 40, 0), 400);
        Data_Pool.m_TeamPool.f_CheckTeamEquipRedPoint(data.f_GetEquipPoolDT(EM_Equip.eEquipPart_Armour), EM_EquipPart.eEquipPart_Armour, ref EquipCanLvUp, ref EquipCanEquip);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPart_Armour), (EquipCanLvUp || EquipCanEquip) ? 1 : 0, new Vector3(40, 40, 0), 400);
        Data_Pool.m_TeamPool.f_CheckTeamEquipRedPoint(data.f_GetEquipPoolDT(EM_Equip.eEquipPart_Helmet), EM_EquipPart.eEquipPart_Helmet, ref EquipCanLvUp, ref EquipCanEquip);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPart_Helmet), (EquipCanLvUp || EquipCanEquip) ? 1 : 0, new Vector3(40, 40, 0), 400);
        Data_Pool.m_TeamPool.f_CheckTeamEquipRedPoint(data.f_GetEquipPoolDT(EM_Equip.eEquipPart_Belt), EM_EquipPart.eEquipPart_Belt, ref EquipCanLvUp, ref EquipCanEquip);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPart_Belt), (EquipCanLvUp || EquipCanEquip) ? 1 : 0, new Vector3(40, 40, 0), 400);
        //kiểm tra thân binh red point
        bool GodEquipCanLvUp = false;//Được trang bị và có thể nâng cấp
        bool GodEquipCanEquip = false;//Không có thiết bị có thiết bị mới hoặc chất lượng cao hơn
        bool GodEquipCanRefine = false;//Không có thiết bị có thiết bị mới hoặc chất lượng cao hơn
        Data_Pool.m_TeamPool.f_CheckTeamGodEquipRedPoint(data.f_GetGodEquipPoolDT(EM_GodEquip.eEquipPart_GodWeapon), EM_EquipPart.eEquipPart_GodWeapon, ref GodEquipCanLvUp, ref GodEquipCanEquip, ref GodEquipCanRefine);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPart_GodWeapon), (EquipCanLvUp || EquipCanEquip || GodEquipCanRefine) ? 1 : 0, new Vector3(40, 40, 0), 400);
        //检查法宝红点
        bool TreasureCanLvUp = false;
        bool TreasureCanEquip = false;
        bool TreasureCanRefine = false;
        Data_Pool.m_TeamPool.f_CheckTeamTreasureRedPoint(data.f_GetTreasurePoolDT(EM_Treasure.eEquipPare_MagicLeft), EM_EquipPart.eEquipPare_MagicLeft, ref TreasureCanLvUp, ref TreasureCanEquip, ref TreasureCanRefine);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPare_MagicLeft), (TreasureCanLvUp || TreasureCanEquip || TreasureCanRefine) ? 1 : 0, new Vector3(40, 40, 0), 400);
        Data_Pool.m_TeamPool.f_CheckTeamTreasureRedPoint(data.f_GetTreasurePoolDT(EM_Treasure.eEquipPare_MagicRight), EM_EquipPart.eEquipPare_MagicRight, ref TreasureCanLvUp, ref TreasureCanEquip, ref TreasureCanRefine);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPare_MagicRight), (TreasureCanLvUp || TreasureCanEquip || TreasureCanRefine) ? 1 : 0, new Vector3(40, 40, 0), 400);

        //检查卡牌红点
        CardCanLvUp = false;
        CardCanEnvolve = false;
        CancelInvoke("ChangeCardRedPointHint");
        Data_Pool.m_TeamPool.f_CheckTeamCardRedPoint(data.m_CardPoolDT, ref CardCanLvUp, ref CardCanEnvolve);
        f_GetObject("CardRedPointHint").SetActive(CardCanLvUp || CardCanEnvolve);
        m_EM_ChangeCardRedPointHint = CardCanEnvolve ? EM_ChangeCardRedPointHint.isCanEnvolve : EM_ChangeCardRedPointHint.isCanLvUp;
        string CardUpHint = CommonTools.f_GetTransLanguage(77);
        string redPointHint2 = CardCanEnvolve ? CommonTools.f_GetTransLanguage(78) : CommonTools.f_GetTransLanguage(79);
        redPointHint2 = CardCanLvUp ? CommonTools.f_GetTransLanguage(79) : redPointHint2;
        f_GetObject("CardRedPointHint").GetComponentInChildren<UILabel>().text = string.Format(CardUpHint, redPointHint2, redPointHint2);
    }
    bool CardCanLvUp = false;
    bool CardCanEnvolve = false;
    EM_ChangeCardRedPointHint m_EM_ChangeCardRedPointHint;
    private enum EM_ChangeCardRedPointHint
    {
        isCanLvUp,
        isCanEnvolve,
    }
    private void CloseEquipRedPoint()
    {
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPart_Weapon), 0, new Vector3(40, 40, 0), 400);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPart_Armour), 0, new Vector3(40, 40, 0), 400);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPart_Helmet), 0, new Vector3(40, 40, 0), 400);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPart_Belt), 0, new Vector3(40, 40, 0), 400);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPare_MagicLeft), 0, new Vector3(40, 40, 0), 400);
        UITool.f_UpdateReddot(GetEquipBtnByIndex(EM_EquipPart.eEquipPare_MagicRight), 0, new Vector3(40, 40, 0), 400);
        f_GetObject("CardRedPointHint").SetActive(false);
        CancelInvoke("ChangeCardRedPointHint");
    }
    /// <summary>
    /// 设置装备和法宝的UI数据
    /// </summary>
    /// <param name="data">阵容PoolDT</param>
    private void InitEquipAndTreasureData(TeamPoolDT data, bool IsHaveEquip = true)
    {
        if (!IsHaveEquip)
        {
            for (int i = 1; i <= 6; i++)
            {
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), GetEquipNameBtnByIndex((EM_EquipPart)i), -1, "", 0);
            }
            return;
        }
        Data_Pool.m_TeamPool.f_UpdateSetEquip(data);
        string name = "";
        string borderSprName = "";
        int IsTwoEquip = 0;
        for (int i = 1; i <= 4; i++)//装备
        {
            if (data.m_aEquipPoolDT[i - 1] != null)
            {
                for (int j = 0; j < data.m_aEquipPoolDT[i - 1].m_SetEquip.m_aSetIsOk.Length; j++)
                {
                    if (data.m_aEquipPoolDT[i - 1].m_SetEquip.m_aSetIsOk[j])
                        IsTwoEquip++;
                }
                EquipPoolDT equipPoolDT = data.m_aEquipPoolDT[i - 1];
                name = equipPoolDT.m_EquipDT.szName;
                borderSprName = "";
                borderSprName = UITool.f_GetImporentColorName(equipPoolDT.m_EquipDT.iColour, ref name);
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), name, equipPoolDT.m_EquipDT.iIcon, borderSprName, equipPoolDT.m_lvIntensify);
                if (IsTwoEquip >= 2)
                    CreareEffect(GetEquipBtnByIndex((EM_EquipPart)i), data.m_aEquipPoolDT[i - 1].m_EquipDT.iColour);
            }
            else
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), GetEquipNameBtnByIndex((EM_EquipPart)i), -1, "", 0);
            IsTwoEquip = 0;
        }
        for (int i = 5; i <= 6; i++)//法宝
        {
            if (data.m_aTreamPoolDT[i - 5] != null)
            {
                TreasurePoolDT treasurePoolDT = data.m_aTreamPoolDT[i - 5];
                name = treasurePoolDT.m_TreasureDT.szName;
                borderSprName = "";
                borderSprName = UITool.f_GetImporentColorName(treasurePoolDT.m_TreasureDT.iImportant, ref name);
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), name, treasurePoolDT.m_TreasureDT.iIcon, borderSprName, treasurePoolDT.m_lvIntensify);
                CreareEffect(GetEquipBtnByIndex((EM_EquipPart)i), treasurePoolDT.m_TreasureDT.iImportant);
            }
            else
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), GetEquipNameBtnByIndex((EM_EquipPart)i), -1, "", 0);
        }

        for (int i = 7; i <= 7; i++)//vũ khí ma thuật
        {
            if (data.m_aGodEquipPoolDT[i - 7] != null)
            {
                GodEquipPoolDT equipPoolDT = data.m_aGodEquipPoolDT[i - 7];
                name = equipPoolDT.m_EquipDT.szName;
                borderSprName = "";
                borderSprName = UITool.f_GetImporentColorName(equipPoolDT.m_EquipDT.iColour, ref name);
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), name, equipPoolDT.m_EquipDT.iIcon, borderSprName, equipPoolDT.m_lvIntensify);
                CreareEffect(GetEquipBtnByIndex((EM_EquipPart)i), equipPoolDT.m_EquipDT.iColour);
            }
            else
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), GetEquipNameBtnByIndex((EM_EquipPart)i), -1, "", 0);
        }
        for (int i = 0; i < 4; i++)//trang bị mới chưa mở
        {
            SetEquipPosData1(f_GetObject("BtnEquip"+i), "", -1, "", 0,true);
        }
        f_GetObject("BtnCardReinforce").SetActive(false);
        f_GetObject("Reinforce").SetActive(false);
    }
    
    private void SetEquipPosData1(GameObject EquipObj, string Name, int iconID, string sprBorderSpriteName, int level,bool isLock)
    {
        EquipObj.GetComponentInChildren<UILabel>().text = Name;
        UI2DSprite spriteIcon3 = EquipObj.transform.Find("Icon").GetComponent<UI2DSprite>();
        spriteIcon3.gameObject.SetActive(iconID == -1 ? false : true);
        spriteIcon3.sprite2D = UITool.f_GetIconSprite(iconID);
        EquipObj.transform.Find("IconBorder").gameObject.SetActive(sprBorderSpriteName == "" ? false : true);
        EquipObj.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = sprBorderSpriteName;
        EquipObj.transform.Find("LabelLevel").GetComponent<UILabel>().text = level.ToString();
        EquipObj.transform.Find("LabelLevel").gameObject.SetActive(iconID == -1 ? false : true);
        //EquipObj.transform.Find("Plus").gameObject.SetActive(iconID == -1 ? true : false);
        for (int j = EquipObj.transform.Find("Effect").childCount - 1; j >= 0; j--)//删除特效
            Destroy(EquipObj.transform.Find("Effect").GetChild(j).gameObject);
        EquipObj.transform.Find("isLock").gameObject.SetActive(isLock);
    }
    /// <summary>
    /// 设置装备位内容
    /// </summary>
    /// <param name="EquipObj">装备位按钮</param>
    /// <param name="Name">装备名称</param>
    /// <param name="iconID">装备图标id</param>
    /// <param name="sprBorderSpriteName">装备边框</param>
    private void SetEquipPosData(GameObject EquipObj, string Name, int iconID, string sprBorderSpriteName, int level)
    {
        EquipObj.GetComponentInChildren<UILabel>().text = Name;
        UI2DSprite spriteIcon3 = EquipObj.transform.Find("Icon").GetComponent<UI2DSprite>();
        spriteIcon3.gameObject.SetActive(iconID == -1 ? false : true);
        spriteIcon3.sprite2D = UITool.f_GetIconSprite(iconID);
        EquipObj.transform.Find("IconBorder").gameObject.SetActive(sprBorderSpriteName == "" ? false : true);
        EquipObj.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = sprBorderSpriteName;
        EquipObj.transform.Find("LabelLevel").GetComponent<UILabel>().text = level.ToString();
        EquipObj.transform.Find("LabelLevel").gameObject.SetActive(iconID == -1 ? false : true);
        //EquipObj.transform.Find("Plus").gameObject.SetActive(iconID == -1 ? true : false);
        for (int j = EquipObj.transform.Find("Effect").childCount - 1; j >= 0; j--)//删除特效
            Destroy(EquipObj.transform.Find("Effect").GetChild(j).gameObject);

    }
    private void CreareEffect(GameObject EquipObj, int Imporent)
    {
        string EffectName = "";
        switch ((EM_Important)Imporent)
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
            case EM_Important.Gold:
                EffectName = UIEffectName.biankuangliuguang_hong;
                break;
        }
        GameObject SetEquipEffect = UITool.f_CreateEffect_Old(EffectName, EquipObj.transform.Find("Effect"), Vector3.zero, 1f, 0, UIEffectName.UIEffectAddress1);
        SetEquipEffect.GetComponent<ParticleScaler>().TrailRenderSortingOrder = 1;
        SetEquipEffect.transform.parent.localScale = Vector3.one * 160;
        SetEquipEffect.transform.parent.localPosition = Vector3.zero;
        SetEquipEffect.transform.localPosition = Vector3.zero;
        SetEquipEffect.transform.localScale = Vector3.one;
    }
    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        CancelInvoke("ChangeCardRedPointHint");
    }
    /// <summary>
    /// 由卡牌、装备、法宝养成界面返回时需要更新UI
    /// </summary>
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        //InitMoneyUI();
        UpdateModelData(CurrentSelectCardPos);
    }
    /// <summary>
    /// 点击卡牌模型
    /// 1.如果角色不为空，进入卡牌养成等详细信息界面
    /// 2.如果角色为空，进入选择卡牌上阵界面
    /// </summary>
    private void OnModelHeroClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        bool isContainKey = dicTeamPoolDT.ContainsKey(CurrentSelectCardPos);
        if (isContainKey)
        {
            CardBox tmp = new CardBox();
            tmp.m_Card = dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT;
            tmp.m_bType = CardBox.BoxType.Intro;
            tmp.m_oType = CardBox.OpenType.battleArray;
            //通知HoldPool保存当前页
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CardProperty, UIMessageDef.UI_OPEN, tmp);
        }
        else
        {
            if (CurrentSelectCardPos == EM_FormationPos.eFormationPos_Pet || CurrentSelectCardPos == EM_FormationPos.eFormationPos_Reinforce)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(80));
                return;
            }
            if (!Data_Pool.m_CardPool.f_CheckHasCardLeft())
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(81));
            else
                ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectCardPage, UIMessageDef.UI_OPEN);
        }
    }

    #region 一键装备部分
    /// <summary>
    /// 一键装备
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void OnEquipQuick(GameObject go, object obj1, object obj2)
    {
        if (!dicTeamPoolDT.ContainsKey(CurrentSelectCardPos)) return;

        //处理所有装备
        bool isNeedChangeEqup = false;
        long[] equipId = new long[6];
        EquipPoolDT[] allEquipPoolDT = dicTeamPoolDT[CurrentSelectCardPos].m_aEquipPoolDT;
        for (int equipPart = 1; equipPart <= allEquipPoolDT.Length; equipPart++)
        {
            //筛选出所有目标格子的装备
            List<BasePoolDT<long>> allUserEquip = Data_Pool.m_EquipPool.f_GetAll();
            List<EquipPoolDT> equipList = allUserEquip.ConvertAll<EquipPoolDT>(item => (EquipPoolDT)item);
            List<EquipPoolDT> targetEquipList = equipList.FindAll((EquipPoolDT item) => { return item.m_EquipDT.iSite == (int)equipPart && !UITool.f_CheckIsWear(item.iId); });

            //如果没有可穿戴装备，则退出
            if (targetEquipList.Count <= 0) continue;

            //按品质》羁绊》装备星数》精炼等级》强化等级优先级获取待更换装备
            EquipPoolDT targetEquip = null == allEquipPoolDT[equipPart - 1] ? targetEquipList[0] : allEquipPoolDT[equipPart - 1];
            int targetFateCount = CheckFateCount(targetEquip.m_EquipDT.iId, false);
            int targetPriority = targetEquip.m_lvIntensify + targetEquip.m_lvRefine * 1000 + targetEquip.m_sstars * 100000 + targetFateCount * 10000000 + targetEquip.m_EquipDT.iColour * 100000000;
            for (int i = 0; i < targetEquipList.Count; i++)
            {
                //优先级权重 = 强化等级 + 精炼等级 * 1000 + 装备星数 * 100000 + 缘分个数 * 10000000 + 品质 * 100000000
                EquipPoolDT equipPoolDT = targetEquipList[i];
                int fateCount = CheckFateCount(equipPoolDT.m_EquipDT.iId, false);
                int equipPriority = equipPoolDT.m_lvIntensify + equipPoolDT.m_lvRefine * 1000 + equipPoolDT.m_sstars * 100000 + fateCount * 10000000 + equipPoolDT.m_EquipDT.iColour * 100000000;
                if (equipPriority > targetPriority)
                {
                    targetPriority = equipPriority;
                    targetEquip = equipPoolDT;
                }
            }
            if (allEquipPoolDT[equipPart - 1] == targetEquip) continue;
            isNeedChangeEqup = true;
            equipId[equipPart - 1] = targetEquip.iId;
        }

        //处理所有法宝
        int openLevel = UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel);
        int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (userLevel >= openLevel)
        {
            TreasurePoolDT[] allTreamPoolDT = dicTeamPoolDT[CurrentSelectCardPos].m_aTreamPoolDT;
            int treasureStart = (int)EM_EquipPart.eEquipPare_MagicLeft;
            for (int equipPart = treasureStart; equipPart < treasureStart + allTreamPoolDT.Length; equipPart++)
            {
                //筛选出所有目标格子的装备
                List<BasePoolDT<long>> allUserTreasure = Data_Pool.m_TreasurePool.f_GetAll();
                List<TreasurePoolDT> treasureList = allUserTreasure.ConvertAll<TreasurePoolDT>(item => (TreasurePoolDT)item);
                List<TreasurePoolDT> targetTreasureList = treasureList.FindAll((TreasurePoolDT item) => { return item.m_TreasureDT.iSite == (int)equipPart && !UITool.f_CheckIsWear(item.iId); });

                //如果没有可穿戴装备，则退出
                if (targetTreasureList.Count <= 0) continue;

                //按品质》羁绊》装备星数》精炼等级》强化等级优先级获取待更换装备
                TreasurePoolDT targetTreasure = null == allTreamPoolDT[equipPart - treasureStart] ? targetTreasureList[0] : allTreamPoolDT[equipPart - treasureStart];
                int targetFateCount = CheckFateCount(targetTreasure.m_TreasureDT.iId, true);
                int targetPriority = targetTreasure.m_lvIntensify + targetTreasure.m_lvRefine * 1000 + targetFateCount * 10000000 + targetTreasure.m_TreasureDT.iImportant * 100000000;
                for (int i = 0; i < targetTreasureList.Count; i++)
                {
                    //优先级权重 = 强化等级 + 精炼等级 * 1000 + 装备星数 * 100000 + 缘分个数 * 10000000 + 品质 * 100000000
                    TreasurePoolDT treasurePoolDT = targetTreasureList[i];
                    int fateCount = CheckFateCount(treasurePoolDT.m_TreasureDT.iId, true);
                    int equipPriority = treasurePoolDT.m_lvIntensify + treasurePoolDT.m_lvRefine * 1000 + fateCount * 10000000 + treasurePoolDT.m_TreasureDT.iImportant * 100000000;
                    if (equipPriority > targetPriority)
                    {
                        targetPriority = equipPriority;
                        targetTreasure = treasurePoolDT;
                    }
                }
                if (allTreamPoolDT[equipPart - treasureStart] == targetTreasure) continue;
                isNeedChangeEqup = true;
                equipId[equipPart - 1] = targetTreasure.iId;
            }
        }

        //一键穿戴请求
        if (!isNeedChangeEqup)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2246));
            return;
        }

        SocketCallbackDT tSocketCallbackDT = new SocketCallbackDT();
        tSocketCallbackDT.m_ccCallbackSuc = (object obj) =>
        {
            //穿戴成功
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Trang bị thành công!");
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.WearEquipment));
            UpdateModelData(CurrentSelectCardPos);
            UpdateFate();
            UITool.f_OpenOrCloseWaitTip(false);

            //重新计算战斗力
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
        };
        tSocketCallbackDT.m_ccCallbackFail = (object obj) =>
        {
            //穿戴失败
            UITool.f_OpenOrCloseWaitTip(false);
            eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Trang bị lỗi，" + CommonTools.f_GetTransLanguage((int)obj));
        };
        Data_Pool.m_TeamPool.f_ChangeEquipOneKey((byte)CurrentSelectCardPos, equipId, tSocketCallbackDT);
    }

    /// <summary>
    /// 检查装备此装备后可激活的缘分数量
    /// </summary>
    /// <returns></returns>
    private static int CheckFateCount(long equipTempId, bool isTreasure)
    {
        CardPoolDT cardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(CurrentSelectCardPos);
        List<CardFateDataDT> m_aFateList = cardPoolDT.m_CardFatePoolDT.m_aFateList;
        int count = 0;
        EM_ResourceType resoureceType = isTreasure ? EM_ResourceType.Treasure : EM_ResourceType.Equip;
        for (int i = 0; i < m_aFateList.Count; i++)
        {
            if (m_aFateList[i].iGoodsType == (int)resoureceType)
            {
                string[] EquipIdArray = m_aFateList[i].szGoodsId.Split(';');
                for (int j = 0; j < EquipIdArray.Length; j++)
                {
                    if (long.Parse(EquipIdArray[j]) == equipTempId)
                    {
                        count++;
                        break;
                    }
                }
            }
        }
        return count;
    }

    #endregion

    #region 一键强化和缘分
    Transform OneInten;
    GameObject Add;
    GameObject Add10;
    GameObject Minus;
    GameObject Minus10;
    UILabel Num;
    UILabel Gold;
    GameObject Close;
    GameObject Suc;
    int IntenNum;
    byte IsHaveEquip = 0;
    /// <summary>
    /// 打开一键强化界面
    /// </summary>
    void OnBtnOneInten(GameObject go, object obj1, object obj2)
    {
        bool isEquip = false;
        for (int i = 0; i < dicTeamPoolDT[CurrentSelectCardPos].m_aEquipPoolDT.Length; i++)
        {
            if (dicTeamPoolDT[CurrentSelectCardPos].m_aEquipPoolDT[i] != null)
            {
                isEquip = true;
                break;
            }
        }
        if (!isEquip)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(82));
            return;
        }
        if (!dicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
            return;
        IntenNum = UITool.f_GetEquipIntenMax();
        if (IntenLvAll() == IntenNum)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(84));
            return;
        }

        OneInten = f_GetObject("OneInten").transform;
        f_GetObject("OneInten").SetActive(true);
        Add = OneInten.Find("add").gameObject;
        Add10 = OneInten.Find("add10").gameObject;
        Minus = OneInten.Find("minus").gameObject;
        Minus10 = OneInten.Find("minus10").gameObject;
        Close = OneInten.Find("Close").gameObject;
        Suc = OneInten.Find("Suc").gameObject;
        Num = OneInten.Find("Num/iNum").GetComponent<UILabel>();
        Gold = OneInten.Find("Gold/Num").GetComponent<UILabel>();

        //判断金钱足够的等级     
        for (; IntenNum > IntenLvAll(); IntenNum--)
        {
            if (IntenNumAll() < Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money))
            {
                break;
            }
        }
        if (IntenNum == IntenLvAll())
        {
            //金钱不足
            f_GetObject("OneInten").SetActive(false);
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }

        Num.text = IntenNum.ToString();
        Gold.text = IntenNumAll().ToString();
        f_RegClickEvent(Add, AddIntenNum, 1);
        f_RegClickEvent(Add10, AddIntenNum, 10);
        f_RegClickEvent(Minus, AddIntenNum, -1);
        f_RegClickEvent(Minus10, AddIntenNum, -10);
        f_RegClickEvent(Close, (GameObject go2, object obj3, object obj4) =>
        {
            f_GetObject("OneInten").SetActive(false);
            IsHaveEquip = 0;
        });
        f_RegClickEvent(Suc, OneIntenBtn);
    }

    /// <summary>
    /// 一键强化
    /// </summary>
    void OneIntenBtn(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money) < IntenNumAll())
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(83));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }

        TeamPoolDT tTeam = dicTeamPoolDT[CurrentSelectCardPos];
        for (int i = 0; i < 4; i++)
        {
            if (tTeam.m_aEqupId[i] == 0)
            {
                continue;
            }
            if (IntenNum - UITool.f_GetEquipDTForIid(tTeam.m_aEqupId[i]).m_lvIntensify < 0)
                continue;
            UITool.f_OpenOrCloseWaitTip(true);
            SocketCallbackDT t = new SocketCallbackDT();
            t.m_ccCallbackFail = (object obj) =>
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(85));
                UITool.f_OpenOrCloseWaitTip(false);
            };
            t.m_ccCallbackSuc = OnwInterSuc;
            Data_Pool.m_EquipPool.f_OneInten(tTeam.m_aEqupId[i],
                (short)IntenNum, t, InterSuc);

            UITool.f_OpenOrCloseWaitTip(true);
        }
        //NumGoldUI();
    }

    void OnwInterSuc(object obj)
    {
    }
    private void InterSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        NumGoldUI();
        SC_EquipIntensify tmpIntStar = (SC_EquipIntensify)obj;
        string[] tmpStr = new string[2];
        tmpStr[0] = CommonTools.f_GetTransLanguage(86) + " + " + tmpIntStar.realTimes;
        tmpStr[1] = CommonTools.f_GetTransLanguage(87) + tmpIntStar.critTimes;
        UITool.Ui_Trip(string.Join("\n", tmpStr));
        OneInten.gameObject.SetActive(false);
        UpdateModelData(CurrentSelectCardPos);

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    void NumGoldUI()
    {
        IntenNum = IntenLvAll();
        Num.text = IntenLvAll().ToString();
        Gold.text = IntenNumAll().ToString();
    }
    /// <summary>
    /// 获取强化全部强化消耗
    /// </summary>
    /// <returns></returns>
    int IntenNumAll()
    {
        int All = 0;
        TeamPoolDT tTeam = dicTeamPoolDT[CurrentSelectCardPos];
        for (int i = 0; i < 4; i++)
        {
            if (tTeam.m_aEqupId[i] == 0)
                continue;
            All += UITool.f_GetEquipIntenCon(UITool.f_GetEquipDTForIid(tTeam.m_aEqupId[i]), IntenNum - UITool.f_GetEquipDTForIid(tTeam.m_aEqupId[i]).m_lvIntensify);
        }
        return All;
    }
    /// <summary>
    /// 获取最低等级
    /// </summary>
    /// <returns></returns>
    int IntenLvAll()
    {
        IsHaveEquip = 0;
        TeamPoolDT tTeam = dicTeamPoolDT[CurrentSelectCardPos];
        int lv = 500;
        for (int i = 0; i < 4; i++)
        {
            if (tTeam.m_aEqupId[i] == 0)
            {
                IsHaveEquip++;
                continue;
            }
            if (lv > UITool.f_GetEquipDTForIid(tTeam.m_aEqupId[i]).m_lvIntensify)
                lv = (short)UITool.f_GetEquipDTForIid(tTeam.m_aEqupId[i]).m_lvIntensify;
        }
        if (IsHaveEquip == 4)
            lv = 0;
        return lv;
    }
    /// <summary>
    /// 没有装备就要提示
    /// </summary>
    /// <returns></returns>
    bool TripIsHaveEquip()
    {
        if (IsHaveEquip == 4)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(88));
            return true;
        }
        return false;
    }
    /// <summary>
    /// 一键强化选择的数值
    /// </summary>
    void AddIntenNum(GameObject go, object obj1, object obj2)
    {
        int sNum = (int)obj1;
        IntenNum += (short)sNum;
        if (IntenNumAll() > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money))
        {
            IntenNum -= (short)sNum;
            if (IntenNum != 0)
                UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(89), IntenNum));
            else
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(90));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        if (TripIsHaveEquip())
            return;
        if (IntenNum >= UITool.f_GetEquipIntenMax())
            IntenNum = (short)UITool.f_GetEquipIntenMax();
        int upMinLv = IntenLvAll() + 1;
        if (IntenNum <= upMinLv)
            IntenNum = upMinLv;
        Num.text = IntenNum.ToString();
        Gold.text = IntenNumAll().ToString();
    }
    /// <summary>
    /// 刷新缘分
    /// </summary>
    void UpdateFate()
    {
        if (!dicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
            return;
        Data_Pool.m_TeamPool.f_UpdateCardFate(dicTeamPoolDT[CurrentSelectCardPos]);
        if (dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardFatePoolDT.m_aFateList.Count != 8)
        {
            for (int i = 0; i < 8 - dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardFatePoolDT.m_aFateList.Count; i++)
            {
                f_GetObject("FateLabel" + (8 - i)).GetComponent<UILabel>().text = "";
                f_GetObject("FateLabel" + (8 - i)).transform.Find("lineEffect").gameObject.SetActive(false);
            }

        }
        for (int i = 0; i < dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardFatePoolDT.m_aFateList.Count; i++)
        {
            if (dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardFatePoolDT.m_aFateIsOk[i])
            {
                GameObject o = f_GetObject("FateLabel" + (i + 1));
                o.transform.GetComponent<UILabel>().text = "[61B870]" + dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardFatePoolDT.m_aFateList[i].szName + "[-]";
                o.transform.Find("lineEffect").gameObject.SetActive(true);
            }

            else
            {
                GameObject o = f_GetObject("FateLabel" + (i + 1));
                o.transform.GetComponent<UILabel>().text = "" + dicTeamPoolDT[CurrentSelectCardPos].m_CardPoolDT.m_CardFatePoolDT.m_aFateList[i].szName + "[-]";
                o.transform.Find("lineEffect").gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region 援军部分
    /// <summary>
    /// 展示援军模型
    /// </summary>
    private void ShowForceModelContent(object data)
    {
        f_GetObject("ObjForceRoleContent").SetActive(true);
        f_GetObject("ObjForceShowFate").SetActive(false);
        GameObject ObjForceRoleContent = f_GetObject("ObjForceRoleContent");
        int totalFateCount = 0;
        //展示卡牌模型
        for (int i = 0; i < 6; i++)
        {
            Transform ModelParent = ObjForceRoleContent.transform.Find("Model/Model" + i);
            Transform ModelInfo = ObjForceRoleContent.transform.Find("ModelInfo/Model" + i);
            if (ModelParent.transform.Find("Model") != null)//删除旧的卡牌模型
                UITool.f_DestoryStatelObject(ModelParent.transform.Find("Model").gameObject);
            //EM_FormationPos formationPos = Data_Pool.m_ClosethArrayData.m_aClothArrayTeamPoolID[i];
            EM_FormationPos formationPos = (EM_FormationPos)i;
            ModelInfo.gameObject.SetActive(formationPos != EM_FormationPos.eFormationPos_INVALID);
            ModelParent.gameObject.SetActive(formationPos != EM_FormationPos.eFormationPos_INVALID);
            if (formationPos == EM_FormationPos.eFormationPos_INVALID)//该位置没有上阵位
            {
                continue;
            }
            CardPoolDT cardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(formationPos);
            Data_Pool.m_TeamPool.f_UpdateCardFate(Data_Pool.m_TeamPool.f_GetForId((long)formationPos) as TeamPoolDT);
            //显示模型
            UITool.f_GetStatelObject(cardPoolDT, ModelParent, Vector3.zero, Vector3.zero, -1, "Model", 80, false);
            //显示模型和缘分信息
            int iEvolveLv = cardPoolDT.m_iEvolveLv;
            string cardName = cardPoolDT.m_CardDT.szName + "+" + iEvolveLv;
            if (formationPos == EM_FormationPos.eFormationPos_Main)//如果是主卡，则显示的是玩家的名字
                cardName = Data_Pool.m_UserData.m_szRoleName + "+" + iEvolveLv;
            UITool.f_GetImporentColorName(cardPoolDT.m_CardDT.iImportant, ref cardName);
            ModelInfo.Find("LabelName").GetComponent<UILabel>().text = cardName;
            int count = 0;
            for (int j = 0; j < cardPoolDT.m_CardFatePoolDT.m_aFateIsOk.Count; j++)
            {
                if (cardPoolDT.m_CardFatePoolDT.m_aFateIsOk[j])
                    count++;
            }
            totalFateCount += count;
            ModelInfo.Find("LabelFate").GetComponent<UILabel>().text = count + CommonTools.f_GetTransLanguage(91);
        }
        f_GetObject("LabelFateCount").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(92);
        bool HasCardLeft = Data_Pool.m_CardPool.f_CheckHasCardLeft();
        SetForceItemLockState(EM_ReinforcePos.eReinforcePos_B1, HasCardLeft);
        SetForceItemLockState(EM_ReinforcePos.eReinforcePos_B2, HasCardLeft);
        SetForceItemLockState(EM_ReinforcePos.eReinforcePos_B3, HasCardLeft);
        SetForceItemLockState(EM_ReinforcePos.eReinforcePos_B4, HasCardLeft);
        SetForceItemLockState(EM_ReinforcePos.eReinforcePos_B5, HasCardLeft);
        SetForceItemLockState(EM_ReinforcePos.eReinforcePos_B6, HasCardLeft);
    }
    private void SetForceItemLockState(EM_ReinforcePos pos, bool HasCardLeft)
    {
        int openLevel = GetForceItemOpenLevel(pos);
        GameObject ReinforceItem = null;
        CardPoolDT cardPoolDt = null;
        if (Data_Pool.m_TeamPool.dicReinforceCardId.ContainsKey(pos))
        {
            cardPoolDt = Data_Pool.m_TeamPool.dicReinforceCardId[pos] as CardPoolDT;
        }
        switch (pos)
        {
            case EM_ReinforcePos.eReinforcePos_B1:
                ReinforceItem = f_GetObject("BtnForceB1");
                break;
            case EM_ReinforcePos.eReinforcePos_B2:
                ReinforceItem = f_GetObject("BtnForceB2");
                break;
            case EM_ReinforcePos.eReinforcePos_B3:
                ReinforceItem = f_GetObject("BtnForceB3");
                break;
            case EM_ReinforcePos.eReinforcePos_B4:
                ReinforceItem = f_GetObject("BtnForceB4");
                break;
            case EM_ReinforcePos.eReinforcePos_B5:
                ReinforceItem = f_GetObject("BtnForceB5");
                break;
            case EM_ReinforcePos.eReinforcePos_B6:
                ReinforceItem = f_GetObject("BtnForceB6");
                break;
        }
        if (ReinforceItem != null)
        {
            ReinforceItem.transform.Find("SprHeadIcon").gameObject.SetActive(false);
            //ReinforceItem.transform.Find("SprBorder").gameObject.SetActive(false);
            ReinforceItem.transform.Find("LabelName").GetComponent<UILabel>().text = "";//名字置空
            ReinforceItem.transform.Find("Lock").gameObject.SetActive(!CheckForceItemIsOpen(pos));
            ReinforceItem.transform.Find("Lock/LabelLevel").GetComponent<UILabel>().text = openLevel.ToString();
            UITool.f_UpdateReddot(ReinforceItem, HasCardLeft && CheckForceItemIsOpen(pos) && Data_Pool.m_TeamPool.dicReinforceCardId[pos] == null ? 1 : 0, new Vector3(50, 50, 0), 102);
        }
        ReinforceItem.transform.Find("Add").gameObject.SetActive(cardPoolDt == null);
        if (cardPoolDt != null)
        {
            ReinforceItem.transform.Find("Lock").gameObject.SetActive(false);
            ReinforceItem.transform.Find("SprHeadIcon").gameObject.SetActive(true);
            //ReinforceItem.transform.Find("SprBorder").gameObject.SetActive(true);
            string cardName = cardPoolDt.m_CardDT.szName;
            ReinforceItem.transform.Find("SprHeadIcon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteByCardId(cardPoolDt);
            ReinforceItem.transform.Find("SprBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(cardPoolDt.m_CardDT.iImportant, ref cardName);
            ReinforceItem.transform.Find("LabelName").GetComponent<UILabel>().text = cardName;
            UITool.f_UpdateReddot(ReinforceItem, 0, new Vector3(50, 50, 0), 102);
        }
    }
    private int GetForceItemOpenLevel(EM_ReinforcePos pos)
    {
        switch (pos)
        {
            case EM_ReinforcePos.eReinforcePos_B1: return 29;
            case EM_ReinforcePos.eReinforcePos_B2: return 35;
            case EM_ReinforcePos.eReinforcePos_B3: return 40;
            case EM_ReinforcePos.eReinforcePos_B4: return 45;
            case EM_ReinforcePos.eReinforcePos_B5: return 50;
            case EM_ReinforcePos.eReinforcePos_B6: return 55;
        }
        return 0;
    }
    private bool CheckForceItemIsOpen(EM_ReinforcePos pos)
    {
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (playerLevel < GetForceItemOpenLevel(pos))
            return false;
        return true;
    }
    /// <summary>
    /// 点击援军上阵位
    /// </summary>
    private void OnForceItemClick(GameObject go, object obj1, object obj2)
    {
        EM_ReinforcePos pos = (EM_ReinforcePos)obj1;
        CurrentSelectReinforcePos = pos;
        if (!CheckForceItemIsOpen(CurrentSelectReinforcePos))//未开放
        {
            return;
        }
		//My Code ( Hide original)
        // if (Data_Pool.m_TeamPool.dicReinforceCardId[CurrentSelectReinforcePos] != null)
        // {
            // CardBox tmp = new CardBox();
            // tmp.m_Card = Data_Pool.m_TeamPool.dicReinforceCardId[CurrentSelectReinforcePos];
            // tmp.m_bType = CardBox.BoxType.Intro;
            // tmp.m_oType = CardBox.OpenType.battleArray;
            // 通知HoldPool保存当前页
            // ccUIHoldPool.GetInstance().f_Hold(this);
            // ccUIManage.GetInstance().f_SendMsg(UINameConst.CardProperty, UIMessageDef.UI_OPEN, tmp);
        // }
        // else
        // {
            if (Data_Pool.m_CardPool.f_CheckHasCardLeft())
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectCardPage, UIMessageDef.UI_OPEN);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(93));
            }
        // }
    }
    /// <summary>
    /// 点击查看缘分
    /// </summary>
    private void OnViewFateEffectClick(GameObject go, object obj1, object obj2)
    {
        f_GetObject("ObjForceRoleContent").SetActive(false);
        f_GetObject("ObjForceShowFate").SetActive(true);
        List<BasePoolDT<long>> listTeam = Data_Pool.m_TeamPool.f_GetAll();
        int titleHeight = 50;
        int _FountSpacing = 26;
        int totalHeight = 0;
        int fateItemHeight = 0;
        GameObject FateGrid = f_GetObject("FateGrid");
        //删除子物体
        for (int i = FateGrid.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(FateGrid.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < listTeam.Count; i++)
        {
            fateItemHeight = 0;
            TeamPoolDT dt = listTeam[i] as TeamPoolDT;
            Data_Pool.m_TeamPool.f_UpdateCardFate(dt);
            CardFatePoolDT fateDt = dt.m_CardPoolDT.m_CardFatePoolDT;
            GameObject item = GameObject.Instantiate(f_GetObject("Intro_Fate")) as GameObject;
            item.transform.SetParent(FateGrid.transform);
            item.SetActive(true);
            item.transform.localPosition = new Vector3(0, totalHeight * -1f, 0);
            item.transform.localScale = Vector3.one;
            item.transform.localEulerAngles = Vector3.zero;
            if (dt.m_eFormationPos == EM_FormationPos.eFormationPos_Main)
                item.GetComponent<UILabel>().text = Data_Pool.m_UserData.m_szRoleName;
            else
                item.GetComponent<UILabel>().text = dt.m_CardPoolDT.m_CardDT.szName;
            totalHeight += titleHeight;
            for (int j = 0; j < fateDt.m_aFateList.Count; j++)
            {
                GameObject FateLabelItem = GameObject.Instantiate(f_GetObject("Introduce_Fate")) as GameObject;
                FateLabelItem.transform.SetParent(item.transform.Find("Ability1"));
                FateLabelItem.SetActive(true);
                FateLabelItem.transform.localPosition = new Vector3(0, fateItemHeight * -1f, 0);
                FateLabelItem.transform.localEulerAngles = Vector3.zero;
                FateLabelItem.transform.localScale = Vector3.one;
                if (fateDt.m_aFateIsOk[j])
                {
                    FateLabelItem.GetComponent<UILabel>().text = string.Format("[d68637][{0}][-]", fateDt.m_aFateList[j].szName);
                    FateLabelItem.transform.GetChild(0).GetComponent<UILabel>().text = "[c4b19e]" + fateDt.m_aFateList[j].szReadme.Substring(fateDt.m_aFateList[j].szName.Length + 1);
                }
                else
                {
                    FateLabelItem.GetComponent<UILabel>().text = string.Format("[534d48][{0}][-]", fateDt.m_aFateList[j].szName);
                    FateLabelItem.transform.GetChild(0).GetComponent<UILabel>().text = "[534d48]" + fateDt.m_aFateList[j].szReadme.Substring(fateDt.m_aFateList[j].szName.Length + 1);
                }
                fateItemHeight += FateLabelItem.transform.GetChild(0).GetComponent<UILabel>().height + _FountSpacing;
                totalHeight += FateLabelItem.transform.GetChild(0).GetComponent<UILabel>().height + _FountSpacing;
            }
        }
        FateGrid.GetComponentInParent<UIScrollView>().ResetPosition();
    }
    /// <summary>
    /// 缘分效果返回
    /// </summary>
    private void OnShowFateReturnClick(GameObject go, object obj1, object obj2)
    {
        f_GetObject("ObjForceRoleContent").SetActive(true);
        f_GetObject("ObjForceShowFate").SetActive(false);
    }
    #endregion
}
