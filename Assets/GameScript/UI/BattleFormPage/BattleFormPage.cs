using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 天命（阵图）系统
/// </summary>
public class BattleFormPage : UIFramwork
{
    private UIWrapComponent m_BattleFormMenuComponent = null;//阵图item列表
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT LightCallback = new SocketCallbackDT();//点亮回调
    List<BattleFormationsDT> listBattleFormName;
    List<string> listTitle1 = new List<string>();
    List<string> listTitle2 = new List<string>();
    /// <summary>
    /// 界面右侧9环选中的序号
    /// </summary>
    private int currentSelectRightIndex = 1;//当前选中的序号（1-9）
    /// <summary>
    /// 阵图左侧栏菜单序号
    /// </summary>
    private int currentLeftMenuProgress = 1;
    /// <summary>
    /// 当前选中的阵图左侧栏菜单序号
    /// </summary>
    private int currentSelectLeftMenuProgress = 1;
    /// <summary>
    /// dic阵图菜单
    /// </summary>
    private Dictionary<int, Transform> dicBattleForm = new Dictionary<int, Transform>();

    private bool RotateReset = false;//玩家移动转轮后，重设旋转
    private int DesRotateZ;//目标旋转值Z
    private int timeEventId;
    private bool LightComplete = true;//点亮是否完成

    private bool IsChangeCardMainIm = false;//是否改变主角品质
    private bool IsGetGood;//发生物品获得(专门指道具)
    private int GetGoodId;
    private int GetGoodCount;
    private bool isAward;//是否发生掉落
    private int AwardId;//奖池id

    private Transform[] Pos;
	//My Code
	GameParamDT AssetOpen;
	//

    private List<BattleFormationsDT> BattleFormation = new List<BattleFormationsDT>();
    /// <summary>
    /// 页面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        InitTitleList();
        UpdateProgressIndex();
        InitMoneyUI();
        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        LightCallback.m_ccCallbackSuc = OnLigthSucCallback;
        LightCallback.m_ccCallbackFail = OnLightFailCallback;
		//My Code
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		//
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_BattleFormPool.f_QueryBattleForm(QueryCallback);
        OnQuerySucCallback(null);
        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/Tex_GameBg";
    private string strTexMagicRoot = "UI/TextureRemove/MainMenu/Tex_Circle";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        //UITexture TexMagic = f_GetObject("TexMagic").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
			if(AssetOpen.iParam1 == 1)
			{
				tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_BattleFormBg");
			}
            TexBg.mainTexture = tTexture2D;

            //Texture2D tTexMagic = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexMagicRoot);
            //TexMagic.mainTexture = tTexMagic;
        }
    }
    private void InitTitleList()
    {
        listTitle1.Clear();
        listTitle2.Clear();
        listTitle1.Add(CommonTools.f_GetTransLanguage(297)); listTitle2.Add(CommonTools.f_GetTransLanguage(298));
        listTitle1.Add(CommonTools.f_GetTransLanguage(299)); listTitle2.Add(CommonTools.f_GetTransLanguage(300));
        listTitle1.Add(CommonTools.f_GetTransLanguage(301)); listTitle2.Add(CommonTools.f_GetTransLanguage(302));
        listTitle1.Add(CommonTools.f_GetTransLanguage(303)); listTitle2.Add(CommonTools.f_GetTransLanguage(304));
        listTitle1.Add(CommonTools.f_GetTransLanguage(305)); listTitle2.Add(CommonTools.f_GetTransLanguage(306));
        listTitle1.Add(CommonTools.f_GetTransLanguage(307)); listTitle2.Add(CommonTools.f_GetTransLanguage(308));
        listTitle1.Add(CommonTools.f_GetTransLanguage(309)); listTitle2.Add(CommonTools.f_GetTransLanguage(310));
        listTitle1.Add(CommonTools.f_GetTransLanguage(311)); listTitle2.Add(CommonTools.f_GetTransLanguage(312));
        listTitle1.Add(CommonTools.f_GetTransLanguage(313)); listTitle2.Add(CommonTools.f_GetTransLanguage(314));
        listTitle1.Add(CommonTools.f_GetTransLanguage(315)); listTitle2.Add(CommonTools.f_GetTransLanguage(316));
        listTitle1.Add(CommonTools.f_GetTransLanguage(317)); listTitle2.Add(CommonTools.f_GetTransLanguage(318));
        listTitle1.Add(CommonTools.f_GetTransLanguage(319)); listTitle2.Add(CommonTools.f_GetTransLanguage(320));
        listTitle1.Add(CommonTools.f_GetTransLanguage(321)); listTitle2.Add(CommonTools.f_GetTransLanguage(322));
        listTitle1.Add(CommonTools.f_GetTransLanguage(323)); listTitle2.Add(CommonTools.f_GetTransLanguage(324));
        listTitle1.Add(CommonTools.f_GetTransLanguage(325)); listTitle2.Add(CommonTools.f_GetTransLanguage(326));
        listTitle1.Add(CommonTools.f_GetTransLanguage(327)); listTitle2.Add(CommonTools.f_GetTransLanguage(328));
        listTitle1.Add(CommonTools.f_GetTransLanguage(329)); listTitle2.Add(CommonTools.f_GetTransLanguage(330));
        listTitle1.Add(CommonTools.f_GetTransLanguage(331)); listTitle2.Add(CommonTools.f_GetTransLanguage(332));
        listTitle1.Add(CommonTools.f_GetTransLanguage(333)); listTitle2.Add(CommonTools.f_GetTransLanguage(334));
        listTitle1.Add(CommonTools.f_GetTransLanguage(335)); listTitle2.Add(CommonTools.f_GetTransLanguage(336));
        listTitle1.Add(CommonTools.f_GetTransLanguage(337)); listTitle2.Add(CommonTools.f_GetTransLanguage(338));
    }
    private void UpdateProgressIndex()
    {
        BattleFormationsDT nextBattleFormationsDT = (glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(Data_Pool.m_BattleFormPool.iDestinyProgress + 1) as BattleFormationsDT);
        if (nextBattleFormationsDT == null)
        {
            //已经达到最大
            nextBattleFormationsDT = (glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(Data_Pool.m_BattleFormPool.iDestinyProgress) as BattleFormationsDT);
            currentLeftMenuProgress = nextBattleFormationsDT.iType;
        }
        else
        {
            currentLeftMenuProgress = nextBattleFormationsDT.iType;
        }
    }
    /// <summary>
    /// 页面关闭
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 初始化金钱UI
    /// </summary>
    private void InitMoneyUI()
    {
        return;
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eBattleFormFragment);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }
    /// <summary>
    /// 初始化左侧菜单页面
    /// </summary>
    private void InitLeftMenu()
    {
        List<BasePoolDT<long>> listDestinyMenu = new List<BasePoolDT<long>>();
        listDestinyMenu.Clear();
        listBattleFormName = GetTypeName();
        for (int i = 1; i < listBattleFormName.Count; i++)
        {
            BasePoolDT<long> item = new BasePoolDT<long>();
            item.iId = i;
            listDestinyMenu.Add(item);
        }
        if (m_BattleFormMenuComponent == null)
        {
            m_BattleFormMenuComponent = new UIWrapComponent(270, 1, 200, 7, f_GetObject("BattleFormItemParent"), f_GetObject("BtnBattleFormItem"), listDestinyMenu, UpdateMenuItem, OnMenuItemClick);
        }
        m_BattleFormMenuComponent.f_ResetView();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="battleFormID"></param>
    /// <returns></returns>
    public RoleProperty f_AddPropertyByFormID(int battleFormID)
    {
        List<NBaseSCDT> allData = glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetAll();
        RoleProperty _AddProperty = new RoleProperty();
        for (int i = 1; i < allData.Count; i++)
        {
            BattleFormationsDT data = allData[i] as BattleFormationsDT;
            if (data.iType == battleFormID)
            {
                _AddProperty.f_AddProperty((int)data.iAttrID, data.iAttrValue);
            }
        }
        return _AddProperty;
    }
    private void UpdateAward(string Desc, int Icon, int Imp, bool isCard, Transform tran)
    {
        tran.Find("Label").GetComponent<UILabel>().text = Desc;

        tran.Find("Case").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(Imp);
        if (isCard)
            tran.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteByCardId(Icon);
        else
            tran.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(Icon);
    }
    /// <summary>
    /// 设置需要的碎片数量和点亮后的效果
    /// </summary>
    private void SetNeedFragmentAndEffect(int BattleFormIndex, int position, bool isUpdateUI = false)
    {
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetAll();
        BattleFormation.Clear();
        //BattleFormationsDT battleFormationsDT = null;
        bool isShowCard = false;
        for (int i = 0; i < listData.Count; i++)
        {
            BattleFormationsDT DT = listData[i] as BattleFormationsDT;

            if (DT.iType == BattleFormIndex)
            {
                BattleFormation.Add(DT);
                if (!isShowCard)
                    isShowCard = UpdateCardAndAward(DT);
                //if (DT.iPosition == position)
                //{
                //    battleFormationsDT = DT;
                //}
                if (BattleFormation.Count >= 5)
                {
                    break;
                }
            }
        }
        Transform TexMagic = f_GetObject("TexMagic").transform;
        for (int i = 0; i < BattleFormation.Count; i++)
        {
            f_UpdateFormItem(TexMagic.Find("Pos" + i), position > i, BattleFormation[i]);
        }
        //UI2DSprite Icon2DSprite = TexMagic.transform.Find("Icon").GetComponent<UI2DSprite>();
        //Icon2DSprite.sprite2D = UITool.f_GetIconSprite(int.Parse(BattleFormation[0].szTypeIcon));
        UITexture Icon2DSprite = TexMagic.transform.Find("Icon").GetComponent<UITexture>();
        Icon2DSprite.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(BattleFormation[0].szTypeIcon);

        //UI2DSprite Icon2DSpriteGray = TexMagic.transform.Find("IconGray").GetComponent<UI2DSprite>();
        //Icon2DSpriteGray.sprite2D = UITool.f_GetIconSprite(int.Parse(BattleFormation[0].szTypeIcon));
        UITexture Icon2DSpriteGray = TexMagic.transform.Find("IconGray").GetComponent<UITexture>();
        Icon2DSpriteGray.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(BattleFormation[0].szTypeIcon);

        Icon2DSprite.fillAmount = (float)position / 5f;

        Icon2DSprite.MakePixelPerfect();
        Icon2DSpriteGray.MakePixelPerfect();
        //position++;
        f_GetObject("LabelLightNeedHint").SetActive(position < BattleFormation.Count && BattleFormation[0].iType <= currentLeftMenuProgress);
        f_GetObject("BtnLight").SetActive(false);
        f_GetObject("BtnWaitLight").SetActive(false);
        //f_GetObject("BtnHasLight").SetActive(position >= BattleFormation.Count && BattleFormation[0].iType <= currentLeftMenuProgress);
        f_GetObject("BtnHasLight").SetActive(false);
        if (position < BattleFormation.Count)
        {
            int hasActivityIDCount = UITool.f_GetGoodNum(EM_ResourceType.Good, BattleFormation[position].iActivePorpID);
            int NeedCount = BattleFormation[position].iActivePorpCount;
            bool isCatUp = hasActivityIDCount < NeedCount;
            if (isCatUp)
                f_GetObject("LabelLightNeedCount").GetComponent<UILabel>().text = "[FF0000]" + hasActivityIDCount + "/" + NeedCount;
            else
                f_GetObject("LabelLightNeedCount").GetComponent<UILabel>().text = "[1EFF00]" + hasActivityIDCount + "/" + NeedCount;
            f_GetObject("BtnLight").SetActive(!isCatUp && BattleFormation[0].iType <= currentLeftMenuProgress);
            f_GetObject("BtnWaitLight").SetActive(isCatUp && BattleFormation[0].iType <= currentLeftMenuProgress);
        }
        return;
        #region 备用

        //f_GetObject("BtnLight").SetActive(false);
        //f_GetObject("BtnWaitLight").SetActive(false);
        //f_GetObject("BtnHasLight").SetActive(false);

        //bool isLast = glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC((BattleFormIndex - 1) * 5 + position) == null ? true : false;
        //if (isLast)//最后一个特殊处理
        //{
        //    //f_GetObject("ShowAddPro").SetActive(true);
        //    RoleProperty addPro = f_AddPropertyByFormID(currentSelectLeftMenuProgress);
        //    //for (int i = 1; i < 5; i++)
        //    //{
        //    //    f_GetObject("ShowAddPro").transform.Find("AddPro" + i.ToString()).GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(339) + UITool.f_GetProName((EM_RoleProperty)i) + ":+" + addPro.f_GetProperty(i);
        //    //}
        //    bool showAward = false;
        //    for (int i = 0; i < BattleFormation.Count; i++)
        //    {
        //        if (BattleFormation[i].iRoleQuality != 0)
        //        {
        //            UpdateAward(BattleFormation[i].szDescribe, Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iId, BattleFormation[i].iRoleQuality, true, f_GetObject("ShowAward").transform);
        //            showAward = true;
        //            break;
        //        }
        //        else if (BattleFormation[i].iDropID != 0)
        //        {
        //            //isShowCard = true;

        //            List<AwardPoolDT> t = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(BattleFormation[i].iDropID);

        //            UpdateAward(BattleFormation[i].szDescribe, t[0].mTemplate.mIcon, t[0].mTemplate.mImportant, false, f_GetObject("ShowAward").transform);
        //            showAward = true;
        //            break;
        //        }
        //        else if (BattleFormation[i].iPropID != 0)
        //        {
        //            AwardPoolDT t = new AwardPoolDT();
        //            t.f_UpdateByInfo((int)EM_ResourceType.Good, BattleFormation[i].iPropID, BattleFormation[i].iPropCount);
        //            UpdateAward(BattleFormation[i].szDescribe, t.mTemplate.mIcon, t.mTemplate.mImportant, false, f_GetObject("ShowAward").transform);
        //            showAward = true;
        //            break;
        //        }
        //    }
        //    f_GetObject("ShowAward").SetActive(showAward);
        //    //f_GetObject("LabelLightEffect").SetActive(false);
        //    f_GetObject("LabelLightNeedHint").SetActive(false);
        //    GameObject TexMagicTemp = f_GetObject("TexMagic");
        //    int iSubBattleProgressTemp = GetSubBattleProgress(BattleFormIndex);
        //    if (isUpdateUI) return;
        //    for (int i = 0; i < 5; i++)
        //    {
        //        //string Light = i <= (position - 2) ? "jx_pic_highlight" : "jx_pic_normal";
        //        UI2DSprite t2DSprite = TexMagicTemp.transform.Find("Pos" + i.ToString() + "/Icon").GetComponent<UI2DSprite>();
        //        //TexMagicTemp.transform.Find("Pos" + i.ToString() + "/Sprite").GetComponent<UISprite>().spriteName = Light;
        //        t2DSprite.sprite2D = UITool.f_GetIconSprite(int.Parse(BattleFormation[i].szIconID));
        //        UITool.f_Set2DSpriteGray(t2DSprite, !(i <= (position - 2)));
        //    }
        //    //f_GetObject("SliderProgress").GetComponent<UISlider>().value = 1;
        //    return;
        //}

        //if (battleFormationsDT == null)
        //{
        //    MessageBox.DEBUG(CommonTools.f_GetTransLanguage(340));
        //    // f_GetObject("LabelLightEffect").GetComponent<UILabel>().text = "";
        //    f_GetObject("LabelLightNeedHint").GetComponent<UILabel>().text = "";
        //    f_GetObject("BtnWaitLight").SetActive(true);
        //    return;
        //}
        //int hasActivityIDCount = UITool.f_GetGoodNum(EM_ResourceType.Good, battleFormationsDT.iActivePorpID);
        ////if (hasActivityIDCount > battleFormationsDT.iActivePorpCount)
        ////{
        ////    hasActivityIDCount = battleFormationsDT.iActivePorpCount;
        ////}
        ////f_GetObject("LabelLightEffect").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(341) + "[FFFFFF]" + battleFormationsDT.szDescribe;
        //f_GetObject("LabelLightNeedHint").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(342) + UITool.f_GetGoodName(EM_ResourceType.Good, battleFormationsDT.iActivePorpID);
        //if (hasActivityIDCount < battleFormationsDT.iActivePorpCount)
        //    f_GetObject("LabelLightNeedCount").GetComponent<UILabel>().text = "[FF0000]" + hasActivityIDCount + "/" + battleFormationsDT.iActivePorpCount;
        //else
        //    f_GetObject("LabelLightNeedCount").GetComponent<UILabel>().text = "[1EFF00]" + hasActivityIDCount + "/" + battleFormationsDT.iActivePorpCount;
        ////f_GetObject("LabelLightNeedIcon").GetComponent<UISprite>().spriteName = UITool.f_GetMoneySpriteName(EM_MoneyType.eBattleFormFragment);
        ////f_GetObject("LabelLightNeedIcon").GetComponent<UISprite>().MakePixelPerfect();

        //BattleFormationsDT proGressSC = (glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(Data_Pool.m_BattleFormPool.iDestinyProgress + 1) as BattleFormationsDT);
        //int nextId;
        //f_GetObject("LightSuc").SetActive(proGressSC == null);
        //if (proGressSC == null)
        //{
        //    MessageBox.DEBUG(CommonTools.f_GetTransLanguage(343));
        //    BattleFormationsDT proCurrentSC = (glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(Data_Pool.m_BattleFormPool.iDestinyProgress) as BattleFormationsDT);
        //    nextId = proCurrentSC.iId + 1;
        //}
        //else
        //{
        //    nextId = proGressSC.iId;
        //}
        ////f_GetObject("LabelLightEffect").SetActive(true);
        ////f_GetObject("ShowAddPro").SetActive(false);
        //if (battleFormationsDT.iId < nextId && GetSubBattleProgress(currentSelectLeftMenuProgress) >= 5)
        //{
        //    //f_GetObject("ShowAddPro").SetActive(true);
        //    RoleProperty addPro = f_AddPropertyByFormID(currentSelectLeftMenuProgress);
        //    // for (int i = 1; i < 5; i++)
        //    //{
        //    //    f_GetObject("ShowAddPro").transform.Find("GameObject/AddPro" + i.ToString()).GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(344) + UITool.f_GetProName((EM_RoleProperty)i) + ":+" + addPro.f_GetProperty(i);
        //    // }
        //    bool showAward = false;
        //    for (int i = 0; i < BattleFormation.Count; i++)
        //    {
        //        if (BattleFormation[i].iRoleQuality != 0)
        //        {
        //            UpdateAward(BattleFormation[i].szDescribe, Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iId, BattleFormation[i].iRoleQuality, true, f_GetObject("ShowAward").transform);
        //            showAward = true;
        //            break;
        //        }
        //        else if (BattleFormation[i].iDropID != 0)
        //        {
        //            // isShowCard = true;

        //            List<AwardPoolDT> t = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(BattleFormation[i].iDropID);

        //            UpdateAward(BattleFormation[i].szDescribe, t[0].mTemplate.mIcon, t[0].mTemplate.mImportant, false, f_GetObject("ShowAward").transform);
        //            showAward = true;
        //            break;
        //        }
        //        else if (BattleFormation[i].iPropID != 0)
        //        {
        //            AwardPoolDT t = new AwardPoolDT();
        //            t.f_UpdateByInfo((int)EM_ResourceType.Good, BattleFormation[i].iPropID, BattleFormation[i].iPropCount);
        //            UpdateAward(BattleFormation[i].szDescribe, t.mTemplate.mIcon, t.mTemplate.mImportant, false, f_GetObject("ShowAward").transform);
        //            showAward = true;
        //            break;
        //        }
        //    }
        //    f_GetObject("ShowAward").SetActive(showAward);


        //    //f_GetObject("LabelLightEffect").SetActive(false);
        //    f_GetObject("LabelLightNeedHint").SetActive(false);
        //    //f_GetObject("BtnHasLight").SetActive(true);

        //}
        //else if (battleFormationsDT.iId == nextId)
        //{
        //    f_GetObject("LabelLightNeedHint").SetActive(true);
        //    f_GetObject("BtnLight").SetActive(true);
        //}
        //else
        //{
        //    f_GetObject("LabelLightNeedHint").SetActive(true);
        //    f_GetObject("BtnWaitLight").SetActive(true);
        //}

        ////GameObject TexMagic = f_GetObject("TexMagic");
        //int iSubBattleProgress = GetSubBattleProgress(BattleFormIndex);

        ////if (!isUpdateUI)
        ////{
        ////    for (int i = 0; i < 5; i++)
        ////    {
        ////        if (iSubBattleProgress == 5)
        ////        {
        ////            TexMagic.transform.Find("Pos" + i.ToString()).gameObject.SetActive(false);
        ////            continue;
        ////        }
        ////        else
        ////        {
        ////            TexMagic.transform.Find("Pos" + i.ToString()).gameObject.SetActive(true);
        ////        }
        ////        f_UpdateFormItem(TexMagic.transform.Find("Pos" + i.ToString()), !(i <= iSubBattleProgress - 1),);
        ////        //UI2DSprite t2DSprite = TexMagic.transform.Find("Pos" + i.ToString() + "/Icon").GetComponent<UI2DSprite>();
        ////        //// string Light = i <= iSubBattleProgress - 1 ? "jx_pic_highlight" : "jx_pic_normal";
        ////        ////TexMagic.transform.Find("Pos" + i.ToString() + "/Sprite").GetComponent<UISprite>().spriteName = Light;

        ////        //t2DSprite.sprite2D = UITool.f_GetIconSprite(int.Parse(BattleFormation[i].szIconID));
        ////        //UITool.f_Set2DSpriteGray(t2DSprite, !(i <= iSubBattleProgress - 1));
        ////    }
        ////}

        ////for (int i = 0; i < 5; i++)
        ////{
        ////    TexMagic.transform.Find("Sprite" + i).gameObject.SetActive(iSubBattleProgress != 5);
        ////}
        ////UI2DSprite Icon2DSprite = TexMagic.transform.Find("Icon").GetComponent<UI2DSprite>();
        ////Icon2DSprite.sprite2D = UITool.f_GetIconSprite(int.Parse(battleFormationsDT.szTypeIcon));

        ////UI2DSprite Icon2DSpriteGray = TexMagic.transform.Find("IconGray").GetComponent<UI2DSprite>();
        ////Icon2DSpriteGray.sprite2D = UITool.f_GetIconSprite(int.Parse(battleFormationsDT.szTypeIcon));

        ////Icon2DSprite.fillAmount = 5 / iSubBattleProgress;
        ////UITool.f_Set2DSpriteGray(Icon2DSprite, !(iSubBattleProgress == 5));
        ////f_GetObject("PositionHint").SetActive(iSubBattleProgress != 5);
        ////f_GetObject("ShowGoodsAndCard").SetActive(iSubBattleProgress != 5);
        //Icon2DSprite.MakePixelPerfect();
        //Icon2DSpriteGray.MakePixelPerfect();
        //OnPosItemClick(null, currentSelectRightIndex, null);
        ////f_GetObject("SliderProgress").GetComponent<UISlider>().value = iSubBattleProgress * 1.0f / 9;
        #endregion
    }
    /// <summary>
    /// 获取换行的字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string GetStrWithLineFeed(string str)
    {
        char[] charArray = str.ToCharArray();
        string strNew = "";
        for (int i = 0; i < charArray.Length; i++)
        {
            strNew += charArray[i];
            if (i != charArray.Length - 1)
            {
                strNew += "\n";
            }
        }
        return strNew;
    }

    private void f_UpdateFormItem(Transform tran, bool isLock, BattleFormationsDT dt)
    {
        UILabel Desc = tran.Find("Desc").GetComponent<UILabel>();
        UISprite Icon = tran.Find("Icon").GetComponent<UISprite>();
        UISprite Line = null;
        Transform LineTran = tran.Find("Line");
        if (LineTran != null)
            Line = LineTran.GetComponent<UISprite>();
        Desc.text = dt.szDescribe;
        Icon.spriteName = !isLock ? "zhujian_pic_b" : "zhujian_pic_a";
        Icon.MakePixelPerfect();
        if (Line != null)
        {
            Line.spriteName = !isLock ? "Line1b" : "Line1a";
        }
    }
    /// <summary>
    /// 设置阵图名称
    /// </summary>
    /// <param name="title"></param>
    private void UpdateTitle(string title, string HintStr1, string HintStr2)
    {
        f_GetObject("LabelTitle").GetComponent<UILabel>().text = GetStrWithLineFeed(title) + "\n" + CommonTools.f_GetTransLanguage(345);
        //string HintStr1 = "如箭之失，攻若锋芒，";
        //string HintStr2 = "中央突破，势若猛虎。";
        //f_GetObject("LabelTitleHint1").GetComponent<UILabel>().text = GetStrWithLineFeed(HintStr1);
        //f_GetObject("LabelTitleHint2").GetComponent<UILabel>().text = GetStrWithLineFeed(HintStr2);
    }
    /// <summary>
    /// 获取天命种类名字
    /// </summary>
    /// <returns></returns>
    private List<BattleFormationsDT> GetTypeName()
    {
        List<BattleFormationsDT> listTypeName = new List<BattleFormationsDT>();
        listTypeName.Add(null);
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            if (i % 5 == 0)
            {
                listTypeName.Add(listData[i] as BattleFormationsDT);
            }
        }
        return listTypeName;
    }
    /// <summary>
    /// 更新菜单UI
    /// </summary>
    private void UpdateMenuItem(Transform t, BasePoolDT<long> data)
    {
        bool isAct = false;
        if (data.iId <= currentLeftMenuProgress)
            isAct = true;
        t.GetComponent<BattleFormItemCtl>().SetData(listBattleFormName[(int)data.iId], data.iId == currentSelectLeftMenuProgress ? true : false, isAct);
        if (!dicBattleForm.ContainsKey((int)data.iId))
        {
            dicBattleForm.Add((int)data.iId, t);
        }
        else
        {
            dicBattleForm[(int)data.iId] = t;
        }
    }
    /// <summary>
    /// 点击菜单UI
    /// </summary>
    /// <param name="t"></param>
    /// <param name="data"></param>
    private void OnMenuItemClick(Transform t, BasePoolDT<long> data)
    {
        UpdateTitle(listBattleFormName[(int)data.iId].szTypeName, listTitle1[(int)data.iId - 1], listTitle2[(int)data.iId - 1]);
        currentSelectLeftMenuProgress = (int)data.iId;
        m_BattleFormMenuComponent.f_UpdateView();
        f_GetObject("TexMagic").transform.localEulerAngles = Vector3.zero;
        //ResetMove();
        UpdateBattleFormProgress();
        SetNeedFragmentAndEffect(currentSelectLeftMenuProgress, currentSelectRightIndex);
        //SimulateMove(currentSelectRightIndex - 1);
    }
    private Vector3 Simulate;
    private int Time_SimulateMove;
    private bool SimulateMoveBool;
    private void SimulateMove(int currentSelectRightIndex)
    {
        return;
        if (currentSelectRightIndex == 0)
        {
            return;
        }
        if (!f_GetObject("TexMagic").transform.Find("Pos" + 0).gameObject.activeSelf)
        {
            return;
        }
        StartTouchPosX = FristClickPos.x;

        isRight = false;
        if (currentSelectRightIndex == 1 || currentSelectRightIndex == 2)
        {
            isRight = true;
        }
        else if (currentSelectRightIndex == 3 || currentSelectRightIndex == 4)
        {
            isRight = false;
        }
        SimulateMoveBool = true;
        //ccTimeEvent.GetInstance().f_UnRegEvent(Time_SimulateMove);
        //Time_SimulateMove = ccTimeEvent.GetInstance().f_RegEvent(0.05f, true, currentSelectRightIndex, MoveTexture);
    }

    private void MoveTexture(object obj)
    {


        return;
        int currentSelectRightIndex = (int)obj;
        if (f_GetObject("TexMagic") == null)
        {
            isRePositionModel = false;
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_SimulateMove);
            return;
        }
        if (f_GetObject("TexMagic").transform.Find("Pos0").GetSiblingIndex() == currentSelectRightIndex)
        {
            isRePositionModel = false;
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_SimulateMove);
            return;
        }
        if (!SimulateMoveBool)
        {
            return;
        }
        else
        {
            FristClickPos = Simulate = new Vector3(1079, 323, 0);
        }
        if (isRight)
        {
            Simulate += new Vector3(200, 0, 0);
        }
        else
        {
            Simulate -= new Vector3(200, 0, 0);
        }
        //MoveFram(Simulate);
        //isRePositionModel = true;
        //SimulateMoveBool = false;
    }
    private void UpdateBattleFormProgress()
    {
        int progressIndex = GetSubBattleProgress(currentSelectLeftMenuProgress);
        //progressIndex++;
        RotateReset = false;
        switch (progressIndex)
        {
            case 1:
                DesRotateZ = 0;
                break;
            case 2:
                DesRotateZ = 320;
                break;
            case 3:
                DesRotateZ = 280;
                break;
            case 4:
                DesRotateZ = 240;
                break;
            case 5:
                DesRotateZ = 200;
                break;
            default:
                RotateReset = false;
                break;
        }
        currentSelectRightIndex = progressIndex <= 5 ? progressIndex : currentSelectRightIndex;
        //SetNeedFragmentAndEffect(currentSelectLeftMenuProgress, currentSelectRightIndex);
    }
    private int GetSubBattleProgress(int m_battleFormMenuIndex)
    {
        if (m_battleFormMenuIndex < currentLeftMenuProgress)
            return 5;
        else if (m_battleFormMenuIndex == currentLeftMenuProgress)
        {
            BattleFormationsDT proGressSC = glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(Data_Pool.m_BattleFormPool.iDestinyProgress + 1) as BattleFormationsDT;
            int subProgress;
            if (proGressSC == null)
            {
                MessageBox.DEBUG(CommonTools.f_GetTransLanguage(343));
                proGressSC = (glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(Data_Pool.m_BattleFormPool.iDestinyProgress) as BattleFormationsDT);
                subProgress = proGressSC.iPosition;
            }
            else
            {
                subProgress = proGressSC.iPosition - 1;
            }
            return subProgress;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        f_RegClickEvent("BtnLight", OnBtnLightClick); 
        f_RegClickEvent("BtnWaitLight", OnBtnWaitLightClick); 
        f_RegClickEvent("BtnAddPro", OnBtnAddProClick);
        f_RegClickEvent("ShowAddProBlack", OnShowAddProBlackClick);
        GameObject TexMagic = f_GetObject("TexMagic");
        //for (int i = 0; i < 5; i++)
        //{
        //    //f_RegPressEvent(TexMagic.transform.Find("Pos" + i.ToString()).gameObject, onClickFram, i);
        //}
    }
    /// <summary>
    /// 点击位置item
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void OnPosItemClick(GameObject go, object obj1, object obj2)
    {
        //ccTimeEvent.GetInstance().f_UnRegEvent(timeEventId);
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetAll();
        BattleFormationsDT battleFormationsDT = null;
        for (int i = 0; i < listData.Count; i++)
        {
            BattleFormationsDT DT = listData[i] as BattleFormationsDT;
            if (DT.iType == currentSelectLeftMenuProgress && DT.iPosition == (int)obj1)
            {
                battleFormationsDT = DT;
                break;
            }
        }
        GameObject PositionHint = f_GetObject("PositionHint");
        if (battleFormationsDT == null)
        {
            PositionHint.GetComponentInChildren<UILabel>().text = CommonTools.f_GetTransLanguage(340);
        }
        else
        {
            PositionHint.GetComponentInChildren<UILabel>().text = battleFormationsDT.szDescribe;
        }
        //PositionHint.transform.position = go.transform.position;
        //timeEventId  = ccTimeEvent.GetInstance().f_RegEvent(1f, false, null, ClosePosHint);
    }
    private void ClosePosHint(object data)
    {
        f_GetObject("PositionHint").SetActive(false);
    }
    /// <summary>
    /// 成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuerySucCallback(object obj)
    {
        UpdateProgressIndex();
        InitLeftMenu();
        currentSelectLeftMenuProgress = currentLeftMenuProgress;
        m_BattleFormMenuComponent.f_ViewGotoRealIdx(currentSelectLeftMenuProgress, 3);
        UpdateTitle(listBattleFormName[currentSelectLeftMenuProgress].szTypeName, listTitle1[currentSelectLeftMenuProgress - 1], listTitle2[currentSelectLeftMenuProgress - 1]);
        //ResetMove();
        UpdateBattleFormProgress();
        SetNeedFragmentAndEffect(currentSelectLeftMenuProgress, currentSelectRightIndex);
        UITool.f_OpenOrCloseWaitTip(false);

        //SimulateMove(currentSelectRightIndex - 1);
        //f_GetObject("TexMagic").transform.Find("Pos0").localPosition = _GetPosForIndex(1)-Vector3.one*20;


        //isDragStart = true;

    }

    private void OnQueryFailCallback(object obj)
    {
        UpdateProgressIndex();
        InitLeftMenu();
        currentSelectLeftMenuProgress = currentLeftMenuProgress;
        m_BattleFormMenuComponent.f_ViewGotoRealIdx(currentSelectLeftMenuProgress, 3);
        UpdateTitle(listBattleFormName[currentSelectLeftMenuProgress].szTypeName, listTitle1[currentSelectLeftMenuProgress - 1], listTitle2[currentSelectLeftMenuProgress - 1]);
        //ResetMove();
        UpdateBattleFormProgress();
        SetNeedFragmentAndEffect(currentSelectLeftMenuProgress, currentSelectRightIndex);
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 点亮回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnLigthSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.BattleFormLight);
        //播放特效
        UITool.f_CreateEffect_Old(UIEffectName.ztdl_dianliang_01, f_GetObject("Icon").transform, Vector3.zero, 0.2f, 2.5f, UIEffectName.UIEffectAddress2);
        ccTimeEvent.GetInstance().f_RegEvent(1.7f, false, null, OnEffectEnd);

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    /// <summary>
    /// 点亮特效播放结束
    /// </summary>
    private void OnEffectEnd(object obj)
    {
        if (IsChangeCardMainIm)
        {
            CardPoolDT mainCardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(EM_FormationPos.eFormationPos_Main);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(346), UITool.f_GetImportantColorName((EM_Important)mainCardPoolDT.m_CardDT.iImportant)));
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
        }
        else if (IsGetGood)
        {
            List<AwardPoolDT> awardList = new List<AwardPoolDT>();
            AwardPoolDT item1 = new AwardPoolDT();
            item1.f_UpdateByInfo((byte)2, GetGoodId, GetGoodCount);
            awardList.Add(item1);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
                new object[] { awardList });
        }
        else if (isAward)
        {
            List<AwardPoolDT> awardList = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(AwardId);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
                new object[] { awardList });
        }
        else
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(347));

        IsChangeCardMainIm = false;
        IsGetGood = false;
        isAward = false;
        LightComplete = true;
        UpdateProgressIndex();
        currentSelectLeftMenuProgress = currentLeftMenuProgress;
        BattleFormationsDT proGressSC = (glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(Data_Pool.m_BattleFormPool.iDestinyProgress + 1) as BattleFormationsDT);
        if (proGressSC == null)
        {
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(343));
            SetNeedFragmentAndEffect(currentSelectLeftMenuProgress,5);
            return;
        }
        //ResetMove();
        currentLeftMenuProgress = proGressSC.iType;
        SetNeedFragmentAndEffect(currentSelectLeftMenuProgress, proGressSC.iPosition - 1);

        if (currentSelectRightIndex < 5)
        {
            GameObject TexMagic = f_GetObject("TexMagic");
            //StartRotateReset(TexMagic.transform.localEulerAngles.z, dir);
            //SimulateMove(currentSelectRightIndex);
        }
        else
        {
            int nextBattleForm = currentSelectLeftMenuProgress + 1;
            if (dicBattleForm.ContainsKey((int)nextBattleForm))
            {
                BasePoolDT<long> data = new BasePoolDT<long>();
                data.iId = nextBattleForm;
                currentSelectRightIndex = 1;
                OnMenuItemClick(dicBattleForm[nextBattleForm], data);
            }
        }
        
        m_BattleFormMenuComponent.f_ViewGotoRealIdx(currentSelectLeftMenuProgress, 3);
        UpdateTitle(listBattleFormName[currentSelectLeftMenuProgress].szTypeName, listTitle1[currentSelectLeftMenuProgress - 1], listTitle2[currentSelectLeftMenuProgress - 1]);
    }
    private void OnLightFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(348));
    }
    #region 按钮事件
    /// <summary>
    /// 点击属性加成按钮
    /// </summary>
    private void OnBtnAddProClick(GameObject go, object obj1, object obj2)
    {
        GameObject WindowShowAddPro = f_GetObject("WindowShowAddPro");
        WindowShowAddPro.SetActive(true);
        RoleProperty addPro = Data_Pool.m_BattleFormPool.f_GetDestinyAddProperty();
        for (int i = 1; i <= 4; i++)
        {
            WindowShowAddPro.transform.Find("AddPro" + i.ToString()).GetComponent<UILabel>().text = string.Format("[5A5230]{0}:[-] [0DC623]+{1}[-]", UITool.f_GetProName((EM_RoleProperty)i), addPro.f_GetProperty(i));
        }
    }
    /// <summary>
    /// 点击属性加成界面黑色背景
    /// </summary>
    private void OnShowAddProBlackClick(GameObject go, object obj1, object obj2)
    {
        f_GetObject("WindowShowAddPro").SetActive(false);
    }
    /// <summary>
    /// 点击点亮
    /// </summary>
    private void OnBtnLightClick(GameObject go, object obj1, object obj2)
    {
        if (RotateReset)//正在旋转，则不响应
            return;
        if (LightComplete == false)//防止多次点击 
            return;
        BattleFormationsDT battleFormationsDT = (glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(Data_Pool.m_BattleFormPool.iDestinyProgress + 1) as BattleFormationsDT);
        int hasActivityIDCount = UITool.f_GetGoodNum(EM_ResourceType.Good, battleFormationsDT.iActivePorpID);
        string PorpName = UITool.f_GetGoodName(EM_ResourceType.Good, battleFormationsDT.iActivePorpID);
        if (hasActivityIDCount < battleFormationsDT.iActivePorpCount)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(349), PorpName));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, battleFormationsDT.iActivePorpID, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        //检测是否改变主角品质
        IsChangeCardMainIm = battleFormationsDT.iRoleQuality != 0 ? true : false;
        IsGetGood = battleFormationsDT.iPropID != 0 ? true : false;
        GetGoodId = battleFormationsDT.iPropID;
        GetGoodCount = battleFormationsDT.iPropCount;
        isAward = battleFormationsDT.iDropID != 0 ? true : false;
        AwardId = battleFormationsDT.iDropID;

        LightComplete = false;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_BattleFormPool.f_BattleForm(LightCallback);
    }

    /// <summary>
    /// 点击灰色点亮按钮
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void OnBtnWaitLightClick(GameObject go, object obj1, object obj2)
    {
        if (RotateReset)//正在旋转，则不响应
            return;
        BattleFormationsDT battleFormationsDT = (glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(Data_Pool.m_BattleFormPool.iDestinyProgress + 1) as BattleFormationsDT);
        int hasActivityIDCount = UITool.f_GetGoodNum(EM_ResourceType.Good, battleFormationsDT.iActivePorpID);
        string PorpName = UITool.f_GetGoodName(EM_ResourceType.Good, battleFormationsDT.iActivePorpID);
        if (hasActivityIDCount < battleFormationsDT.iActivePorpCount)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(349), PorpName));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, battleFormationsDT.iActivePorpID, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>

    /// <summary>
    /// 
    /// </summary>
    /// <param name="oriAngleZ"></param>
    /// <param name="dir">1,向右，-1向左</param>
    private void StartRotateReset(float oriAngleZ, int dir)
    {
        // 0-40-80-120-160-200-240-280-320-0
        SetNeedFragmentAndEffect(currentSelectLeftMenuProgress, currentSelectRightIndex);
    }
    /// <summary>
    /// 点击返回退出
    /// </summary>
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFormPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private bool isPlay;
    private Vector3 FristClickPos;
    private bool isRight;
    private bool isRePositionModel;
    private bool isRealDrag = false;
    private bool isDragStart = false;
    private float StartTouchPosX;
    protected override void f_Update()
    {
        base.f_Update();
        if (isDragStart)
        {
            //MoveFram(Input.mousePosition);
        }
        else
        {
            if (isRePositionModel)
            {
                //RePositionModelState();
            }
        }
    }

    private void MoveFram(Vector3 MovePos)
    {
        Vector3 DragPos = MovePos;
        //print(MovePos);
        Vector3 mouseOffsetPos = DragPos - FristClickPos;
        if (Mathf.Abs(mouseOffsetPos.x) > 15f)//拖动
        {
            //isRealDrag = true;
            GameObject ModelPoint = f_GetObject("TexMagic");
            ModelPoint.SetActive(true);
            bool isRight = mouseOffsetPos.x > 0 ? true : false;
            float sliderOneDistance = 1000;//滑动一节需要移动的x距离
            float sliderProgress = Mathf.Abs(mouseOffsetPos.x) * 1.0f / sliderOneDistance;
            for (int i = 0; i < 5; i++)
            {
                Transform Pos = ModelPoint.transform.Find("Pos" + i);
                int nextSliderIndex = _GetNextIndex(i, isRight);
                Vector2 oriPos = _GetPosForIndex(i);
                Vector2 nextPos = _GetPosForIndex(nextSliderIndex);
                Vector2 offsetPos = (nextPos - oriPos) * sliderProgress;
                //float offsetScale = (GetScaleByModelIndex(nextSliderIndex) - GetScaleByModelIndex(i)) * sliderProgress;
                Vector2 afterPos = oriPos + offsetPos;
                afterPos = CalculateEllipsePos(i, isRight, afterPos, sliderProgress);
                if (float.IsNaN(afterPos.x) || float.IsNaN(afterPos.y))
                {
                    continue;
                }
                else
                    Pos.transform.localPosition = afterPos;
                //float afterScale = GetScaleByModelIndex(i) + offsetScale;
                //CardPoint.transform.localScale = new Vector3(afterScale, afterScale, 1);
            }
        }
        CheckPosCross(isRight);
    }

    private void onClickFram(GameObject go, object obj1, object obj2)
    {
        //if (isRePositionModel)//正在重设模型坐标，不接受新的
        //    return;
        if ((bool)obj1)
        {
            // f_GetObject("PositionHint").SetActive(false);
            //if (!isDragStart)
            //{
            //    isDragStart = true;
            //    isRealDrag = false;
            //    FristClickPos = Input.mousePosition;
            //    StartTouchPosX = FristClickPos.x;
            //}
        }
        else
        {
            isDragStart = false;
            if (isRealDrag)
            {
                isRight = (Input.mousePosition.x - StartTouchPosX) > 0 ? true : false;
                isRePositionModel = true;//重置模型
            }
        }
    }

    private Vector3 _GetPosForIndex(int index)
    {
        switch (index)
        {
            case 0: return new Vector2(0, -121);
            case 1: return new Vector2(-480, 0);
            case 2: return new Vector2(-231, 125);  //
            case 3: return new Vector2(231, 125); //231, 125
            case 4: return new Vector2(480, 0); // 480, 0
            default: return Vector3.zero;
        }
    }
    private int _GetNextIndex(int NowIndex, bool isRight)
    {
        switch (NowIndex)
        {
            case 0: return isRight ? 4 : 1;
            case 1: return isRight ? 0 : 2;
            case 2: return isRight ? 1 : 3;
            case 3: return isRight ? 2 : 4;
            case 4: return isRight ? 3 : 0;
            default: return 0;
        }
    }
    /// <summary>
    /// 根据X来计算椭圆在y的位置
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private Vector2 CalculateEllipsePos(int i, bool isSliderRight, Vector2 afterPos, float sliderProgress)
    {
        int a = 520;
        int b = 151;
        if (isSliderRight)
        {
            if (i == 1 || i == 0 || i == 3)
            {
                afterPos.x *= Mathf.Pow(sliderProgress, 1 / 4);
                afterPos.y = Mathf.Sqrt((1 - Mathf.Pow(afterPos.x, 2) / Mathf.Pow(a, 2)) * Mathf.Pow(b, 2));
                afterPos.y *= (i == 1 || i == 0) ? -1 : 1;
            }
            else
            {
                afterPos.y *= Mathf.Pow(sliderProgress, 1 / 4);
                afterPos.x = Mathf.Sqrt((1 - Mathf.Pow(afterPos.y, 2) / Mathf.Pow(b, 2)) * Mathf.Pow(a, 2));
                afterPos.x *= i == 2 ? -1 : 1;
            }
        }
        else
        {
            if (i == 4 || i == 0 || i == 2)
            {
                afterPos.x *= Mathf.Pow(sliderProgress, 1 / 4);
                afterPos.y = Mathf.Sqrt((1 - Mathf.Pow(afterPos.x, 2) / Mathf.Pow(a, 2)) * Mathf.Pow(b, 2));
                afterPos.y *= (i == 4 || i == 0) ? -1 : 1;
            }
            else
            {
                afterPos.y *= Mathf.Pow(sliderProgress, 1 / 4);
                afterPos.x = Mathf.Sqrt((1 - Mathf.Pow(afterPos.y, 2) / Mathf.Pow(b, 2)) * Mathf.Pow(a, 2));
                afterPos.x *= i == 1 ? -1 : 1;
            }
        }
        return afterPos;
    }
    private void RePositionModelState()
    {
        GameObject ModelPoint = f_GetObject("TexMagic");
        for (int i = 0; i < 5; i++)
        {
            Transform CardPoint = ModelPoint.transform.Find("Pos" + i);
            int nextSliderIndex = _GetNextIndex(i, isRight);
            Vector2 oriPos = _GetPosForIndex(i);
            Vector2 nextPos = _GetPosForIndex(nextSliderIndex);
            Vector2 addPos = (nextPos - oriPos) * 0.05f;

            Vector2 cardPos = new Vector2(CardPoint.transform.localPosition.x, CardPoint.transform.localPosition.y);
            Vector2 afterPos = cardPos + addPos;
            float sliderProgress = GetAutoRotateProgress(i, isRight, afterPos, nextPos, oriPos);
            afterPos = CalculateEllipsePos(i, isRight, afterPos, sliderProgress);
            if (float.IsNaN(afterPos.x) || float.IsNaN(afterPos.y))
            {
                continue;
            }
            else
                CardPoint.transform.localPosition = afterPos;
            //float afterScale = GetScaleByModelIndex(i) + (GetScaleByModelIndex(nextSliderIndex) - GetScaleByModelIndex(i)) * sliderProgress;
            //CardPoint.transform.localScale = new Vector3(afterScale, afterScale, 1);
            //ParticleScaler[] particleScalerArray = CardPoint.transform.GetComponentsInChildren<ParticleScaler>();
            //for (int j = 0; j < particleScalerArray.Length; j++)
            //{
            //    particleScalerArray[j].particleScale = 0.2f * afterScale;
            //}
        }
        //CheckResetModelPos(isRight);
    }
    private float GetAutoRotateProgress(int i, bool sliderRight, Vector2 afterPos, Vector2 nextPos, Vector2 oriPos)
    {
        if (sliderRight)
        {
            if (i == 1 || i == 0 || i == 3)
                return (afterPos.x - oriPos.x) / (nextPos.x - oriPos.x);
            else
                return (afterPos.y - oriPos.y) / (nextPos.y - oriPos.y);
        }
        else
        {
            if (i == 4 || i == 0 || i == 2)
                return (afterPos.x - oriPos.x) / (nextPos.x - oriPos.x);
            else
                return (afterPos.y - oriPos.y) / (nextPos.y - oriPos.y);
        }
    }
    private void CheckResetModelPos(bool isSliderRight)
    {
        GameObject ModelPoint = f_GetObject("TexMagic");
        Transform CardPoint0 = ModelPoint.transform.Find("Pos0");
        Vector2 desPos = _GetPosForIndex(isSliderRight ? 4 : 1);
        if (isSliderRight)
        {
            if (CardPoint0.transform.localPosition.x >= desPos.x)
            {
                CheckPosCross(isSliderRight);
                ResetModelPos();
            }
        }
        else
        {
            if (CardPoint0.transform.localPosition.x <= desPos.x)
            {
                CheckPosCross(isSliderRight);
                ResetModelPos();
            }
        }
    }
    private void CheckPosCross(bool isSliderRight)
    {
        GameObject ModelPoint = f_GetObject("TexMagic");
        Transform CardPoint0 = ModelPoint.transform.Find("Pos0");
        int nextSliderIndex = _GetNextIndex(0, isSliderRight);
        Vector2 nextPos = _GetPosForIndex(nextSliderIndex);
        if (isSliderRight)
        {
            if (CardPoint0.transform.localPosition.x >= nextPos.x)
            {
                //重置初始化鼠标坐标，重置命名
                FristClickPos = Input.mousePosition;
                ModelPoint.transform.Find("Pos1").gameObject.name = "Pos0";
                ModelPoint.transform.Find("Pos2").gameObject.name = "Pos1";
                ModelPoint.transform.Find("Pos3").gameObject.name = "Pos2";
                ModelPoint.transform.Find("Pos4").gameObject.name = "Pos3";
                CardPoint0.gameObject.name = "Pos4";
                ResetModelSortingLayerOrder();
                UpdateUI();
            }
        }
        else
        {
            if (CardPoint0.transform.localPosition.x <= nextPos.x)
            {
                //重置初始化鼠标坐标，重置命名
                FristClickPos = Input.mousePosition;
                ModelPoint.transform.Find("Pos4").gameObject.name = "Pos0";
                ModelPoint.transform.Find("Pos3").gameObject.name = "Pos4";
                ModelPoint.transform.Find("Pos2").gameObject.name = "Pos3";
                ModelPoint.transform.Find("Pos1").gameObject.name = "Pos2";

                CardPoint0.gameObject.name = "Pos1";
                ResetModelSortingLayerOrder();
                UpdateUI();
            }
        }



    }
    private void ResetModelPos()
    {
        GameObject ModelPoint = f_GetObject("TexMagic");
        isRePositionModel = false;
        for (int i = 0; i < 5; i++)
        {
            Transform CardPoint = ModelPoint.transform.Find("Pos" + i);
            Vector2 pos = _GetPosForIndex(i);
            CardPoint.localPosition = new Vector3(pos.x, pos.y, 0);
            // float scale = _GetPosForIndex(i);
            //CardPoint.localScale = new Vector3(scale, scale, 1);
        }
        ResetModelSortingLayerOrder();
    }
    private void ResetModelSortingLayerOrder()
    {
        isRePositionModel = false;
    }

    /// <summary>
    /// 滚动之后刷新界面
    /// </summary>
    private void UpdateUI()
    {
        currentSelectRightIndex = f_GetObject("TexMagic").transform.Find("Pos0").GetSiblingIndex() + 1;
        OnPosItemClick(null, currentSelectRightIndex, null);
        SetNeedFragmentAndEffect(currentSelectLeftMenuProgress, currentSelectRightIndex, true);
        isRePositionModel = false;
        SimulateMoveBool = true;
    }

    private void ResetMove()
    {
        for (int i = 0; i < 5; i++)
        {
            Transform tran = f_GetObject("TexMagic").transform.GetChild(i);

            tran.name = "Pos" + i;

            tran.localPosition = _GetPosForIndex(i);
        }
    }
    #endregion
    /// <summary>
    /// 更新奖励
    /// </summary>
    private bool UpdateCardAndAward(BattleFormationsDT tBattleFormationDT)
    {
        bool isShowCard = false;

        if (tBattleFormationDT.iRoleQuality != 0)
        {
            isShowCard = true;
            UpdateAward(tBattleFormationDT.szDescribe, Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iId, tBattleFormationDT.iRoleQuality, true, f_GetObject("ShowGoodsAndCard").transform);
        }
        else if (tBattleFormationDT.iDropID != 0)
        {
            isShowCard = true;

            List<AwardPoolDT> t = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(tBattleFormationDT.iDropID);

            UpdateAward(tBattleFormationDT.szDescribe, t[0].mTemplate.mIcon, t[0].mTemplate.mImportant, false, f_GetObject("ShowGoodsAndCard").transform);
        }
        else if (tBattleFormationDT.iPropID != 0)
        {
            isShowCard = true;
            AwardPoolDT t = new AwardPoolDT();
            t.f_UpdateByInfo((int)EM_ResourceType.Good, tBattleFormationDT.iPropID, tBattleFormationDT.iPropCount);
            UpdateAward(tBattleFormationDT.szDescribe, t.mTemplate.mIcon, t.mTemplate.mImportant, false, f_GetObject("ShowGoodsAndCard").transform);
        }
        return isShowCard;
    }
}
