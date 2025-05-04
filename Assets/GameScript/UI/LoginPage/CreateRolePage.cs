
using UnityEngine;
using System.Collections;
using ccU3DEngine;
using Spine.Unity;

/// <summary>
/// 创建角色页面
/// </summary>
public class CreateRolePage : UIFramwork
{
    LoginPage2SelCharacer _LoginPage2SelCharacer = null;//回调和请求
    private EM_RoleSex selectRoleType = EM_RoleSex.Man;//选中的角色类别
    private float textFillValue = 0;//0-1
	//My Code
	GameParamDT AssetOpen;
	GameObject TexBgKD;
	SkeletonAnimation SkeAni;
    //
    UITexture BtnRoleBoy;
    UITexture BtnRoleGirl;

    UITexture Tex_Bg;
    private GameObject Ani;
    private EM_CardFightType selectRoleJob = EM_CardFightType.eCardPhyAtt;// nghề
    private GameObject Btn1;
    private GameObject Btn2;
    private GameObject Btn3;
    private GameObject Btn4;
    private GameObject Btn5;
    private GameObject Btn6;
    private GameObject Btn7;
    private GameObject JobName;

    /// <summary>
    /// 加载角色图片资源
    /// </summary>
    /// <param name="roleType">角色类型</param>
    /// <param name="isSelect">是否选中</param>
    private void LoadTexture(UITexture uITexture,EM_RoleSex roleType, bool isSelect)
    {
        string TexturePath = "UI/UITexture/Login/";
        switch (roleType)
        {
            case EM_RoleSex.Man:
                TexturePath += "Tex_BoySelect";
                break;
            case EM_RoleSex.Woman:
                TexturePath += "Tex_GirlSelect";
                break;
        }
        uITexture.mainTexture =  glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(TexturePath);
        UITool.f_SetTextureGray(uITexture, !isSelect);
    }
    /// <summary>
    /// 页面打开
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
		//My Code
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		//
        f_LoadTexture();
        InitUI();
        UITool.f_OpenOrCloseWaitTip(true);
		// glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.Intro_Boy));
        _LoginPage2SelCharacer = (LoginPage2SelCharacer)e;
        _LoginPage2SelCharacer.Login_RequestNewNameResp = OnGetRandomNameCallback;
        _LoginPage2SelCharacer.Login_SubmitResp = OnSumitCallback;
        _LoginPage2SelCharacer.Sel_RequestNewName((int)this.selectRoleType);
        //glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.LoginBg, true);

    }
    private string strTexBgRoot = "UI/TextureRemove/Login/Tex_SelectRoleBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
		//My Code
		TexBgKD = GameObject.Find("18");
		if(TexBgKD != null && AssetOpen != null)
		{
			if(AssetOpen.iParam2 == 0)
			{
				TexBgKD.SetActive(false);
			}
		}
		//
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        UITool.f_CreateMagicById((int)EM_MagicId.eCreateRoleBg, ref Ani, TexBg.transform, 10, null);
        Ani.transform.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "animation", true);
		
		float windowAspect = (float)Screen.width /  (float) Screen.height ;
		MessageBox.ASSERT("" + windowAspect);
		// if(windowAspect >= 2.1)
		// {
			// f_GetObject("TexBg").transform.localPosition = new Vector3(300f, 80f, 0f);
			// f_GetObject("Anchor-Center").transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
			// f_GetObject("SkillTree").transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
			// f_GetObject("SkillTree2").transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
			// f_GetObject("DiceName").transform.localPosition = new Vector3(-360f, -370f, 0f);
		// }
    }
    
    /// <summary>
    /// 初始化UI
    /// </summary>
    private void InitUI()
    {
        _Select(selectRoleType);
        BtnRoleBoy = f_GetObject("BtnRoleBoy").GetComponent<UITexture>();
        BtnRoleGirl = f_GetObject("BtnRoleGirl").GetComponent<UITexture>();
        LoadTexture(BtnRoleBoy, EM_RoleSex.Man, this.selectRoleType == EM_RoleSex.Man);
        LoadTexture(BtnRoleGirl, EM_RoleSex.Woman, this.selectRoleType == EM_RoleSex.Woman);
        Tex_Bg = f_GetObject("Sprite").GetComponent<UITexture>();
        ChangeTextureBg();
        JobName = f_GetObject("JobName");
        Transform jobParent = f_GetObject("JobParent").transform;
        Btn1 = jobParent.Find("Btn_Job" + (int)EM_CardFightType.eCardPhyAtt).gameObject;
        Btn2 = jobParent.Find("Btn_Job" + (int)EM_CardFightType.eCardMagAtt).gameObject;
        Btn3 = jobParent.Find("Btn_Job" + (int)EM_CardFightType.eCardSup).gameObject;
        Btn4 = jobParent.Find("Btn_Job" + (int)EM_CardFightType.eCardTank).gameObject;
        Btn5 = jobParent.Find("Btn_Job" + (int)EM_CardFightType.eCardKiller).gameObject;
        Btn6 = jobParent.Find("Btn_Job" + (int)EM_CardFightType.eCardPhysician).gameObject;
        Btn7 = jobParent.Find("Btn_Job" + (int)EM_CardFightType.eCardLogistics).gameObject;
        ShowJobContent(this.selectRoleJob);
        ShowContent();
    }
    private void ChangeTextureBg()
    {
        string TexturePath = "UI/UITexture/Login/";
        if(this.selectRoleType == EM_RoleSex.Man)
        {
            TexturePath += "Tex_BGBoy";
        }
        else
        {
            TexturePath += "Tex_BGGirl";
        }
        Tex_Bg.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(TexturePath);
    }
    private void ShowContent()
    {
        int cardId = UITool.GetMainCardId(this.selectRoleType, this.selectRoleJob);
        Transform ModelPoint = f_GetObject("ModelPoint").transform;
        if (ModelPoint.Find("Model") != null)
        {
            UITool.f_DestoryStatelObject(ModelPoint.Find("Model").gameObject);
        }
        GameObject go = UITool.f_GetStatelObject(cardId, f_GetObject("ModelPoint").transform, new Vector3(0, 180, 0), Vector3.zero, 30, "Model", 115);
        SkeAni = go.GetComponent<SkeletonAnimation>();
        // show skill
        CardDT tCardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardId);
        if(tCardDT!= null)
        {
            MagicDT[] tmpMagic = UITool.f_GetCardMagic(tCardDT);
            UpdateSkill(f_GetObject("Info1").transform, tmpMagic[0]);
            UpdateSkill(f_GetObject("Info2").transform, tmpMagic[1]);
        }
        // show chỉ số
        UITexture TexText2 = f_GetObject("TexText2").GetComponent<UITexture>();
        string TexturePath = "UI/UITexture/Login/Tex_Chart_"+ (int)this.selectRoleJob;
        TexText2.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(TexturePath);
        
    }
    /// <summary>
    /// 注册按钮事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        InitUI();
        f_RegClickEvent("BtnBeginGame", OnBeginGameClick);
        f_RegClickEvent("BtnRoleBoy", OnBtnRoleClick, EM_RoleSex.Man);
        f_RegClickEvent("BtnRoleGirl", OnBtnRoleClick, EM_RoleSex.Woman);
        f_RegClickEvent("Dice", OnBtnDiceClick);
		f_RegClickEvent("Skill_1", OnSkill1Click);
		f_RegClickEvent("Closeinfo1", OnCloseSkill1Click);
		f_RegClickEvent("Skill_2", OnSkill2Click);
		f_RegClickEvent("Closeinfo2", OnCloseSkill2Click);


        f_RegClickEvent(Btn1, OnTapItemClick, EM_CardFightType.eCardPhyAtt);
        f_RegClickEvent(Btn2, OnTapItemClick, EM_CardFightType.eCardMagAtt);
        f_RegClickEvent(Btn3, OnTapItemClick, EM_CardFightType.eCardSup);
        f_RegClickEvent(Btn4, OnTapItemClick, EM_CardFightType.eCardTank);
        f_RegClickEvent(Btn5, OnTapItemClick, EM_CardFightType.eCardKiller);
        f_RegClickEvent(Btn6, OnTapItemClick, EM_CardFightType.eCardPhysician);
        f_RegClickEvent(Btn7, OnTapItemClick, EM_CardFightType.eCardLogistics);
    }

    private void OnTapItemClick(GameObject go, object obj1, object obj2)
    {
        EM_CardFightType job = (EM_CardFightType)obj1;//卡牌阵营
        ShowJobContent(job);
        ShowContent();
    }
    private void SetTapState(GameObject tapItem, bool isSelect)
    {
        tapItem.transform.Find("Select").gameObject.SetActive(isSelect);
    }
    private void ShowJobContent(EM_CardFightType job)
    {
        if(job == EM_CardFightType.eCardAll)
        {
            job = EM_CardFightType.eCardPhyAtt;
        }
        this.selectRoleJob = job;
        SetTapState(Btn1, job == EM_CardFightType.eCardPhyAtt);
        SetTapState(Btn2, job == EM_CardFightType.eCardMagAtt);
        SetTapState(Btn3, job == EM_CardFightType.eCardSup);
        SetTapState(Btn4, job == EM_CardFightType.eCardTank);
        SetTapState(Btn5, job == EM_CardFightType.eCardKiller);
        SetTapState(Btn6, job == EM_CardFightType.eCardPhysician);
        SetTapState(Btn7, job == EM_CardFightType.eCardLogistics);
        string name = "";
        switch (job)
        {
            case EM_CardFightType.eCardPhyAtt:
                name = CommonTools.f_GetTransLanguage(2315);
                break;
            case EM_CardFightType.eCardMagAtt:
                name = CommonTools.f_GetTransLanguage(2316);
                break;
            case EM_CardFightType.eCardSup:
                name = CommonTools.f_GetTransLanguage(2317);
                break;
            case EM_CardFightType.eCardTank:
                name = CommonTools.f_GetTransLanguage(2318);
                break;
            case EM_CardFightType.eCardKiller:
                name = CommonTools.f_GetTransLanguage(2322);
                break;
            case EM_CardFightType.eCardPhysician:
                name = CommonTools.f_GetTransLanguage(2323);
                break;
            case EM_CardFightType.eCardLogistics:
                name = CommonTools.f_GetTransLanguage(2324);
                break;
            case EM_CardFightType.eCardAll:
                name = CommonTools.f_GetTransLanguage(2325);
                break;
        }
        JobName.GetComponent<UILabel>().text = name;
    }

    private void UpdateSkill(Transform go,MagicDT magicDT)
    {
        Transform panel = go.Find("Panel").transform;
        UILabel name = panel.Find("name").GetComponent<UILabel>();
        UILabel desc = panel.Find("info").GetComponent<UILabel>();
        UI2DSprite icon = panel.Find("icon").GetComponent<UI2DSprite>();
        name.text = magicDT.szName;
        desc.text = magicDT.szReadme;
        icon.sprite2D = UITool.f_GetSkillIcon(magicDT.iId + "");
    }
    /// <summary>
    /// 点击开始游戏按钮
    /// </summary>
    private void OnBeginGameClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        string inputRandomName = f_GetObject("InputRoleName").GetComponent<UIInput>().value;
        if (inputRandomName == "")
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1662));
            return;
        }
        string name = inputRandomName;
        if (!Data_Pool.m_BlockWordPool.f_CheckValidity(ref name))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1663));
            f_GetObject("InputRoleName").GetComponent<UIInput>().value = name;
            return;
        }
        if (CommonTools.f_CheckLength(inputRandomName) < 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1664));
            return;
        }
        int size = ccMath.f_GetStringBytesLength(inputRandomName);
        if (size > GameParamConst.RoleNameByteMaxNum)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1665));
            return;
        }
        else if (size < GameParamConst.RoleNameByteMinNum)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1666));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        _LoginPage2SelCharacer.Sel_RequestSubmit((int)selectRoleType,(int)selectRoleJob, inputRandomName);
    }

    /// <summary>
    /// 获取用户名
    /// </summary>
    /// <param name="obj">返回随机名字</param>
    private void OnGetRandomNameCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        string getName = (string)obj;
        f_GetObject("InputRoleName").GetComponent<UIInput>().value = getName;
    }
    /// <summary>
    /// 提交角色信息结果回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnSumitCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        switch (teMsgOperateResult)
        {
            case eMsgOperateResult.OR_Succeed:
                //剧情战斗
                if (GloData.glo_StarGuidance)
                {
                    ////先请求第一关的数据，避免战斗回来请求数据还能看到章节界面
                    DungeonPoolDT tDungeonPoolDt = (DungeonPoolDT)Data_Pool.m_DungeonPool.f_GetForId(1);
                    if (!Data_Pool.m_DungeonPool.f_CheckChapterLockState(tDungeonPoolDt))
                    {
                        Data_Pool.m_DungeonPool.f_ExecuteAfterInitDungeon(tDungeonPoolDt, (object obj1) => { });
                    }

                    //有新手引导才开剧情战斗
                    SocketCallbackDT tCallBackDT = new SocketCallbackDT();
                    tCallBackDT.m_ccCallbackSuc = CallBack_PlotFight;
                    tCallBackDT.m_ccCallbackFail = CallBack_PlotFight;
                    DungeonTollgatePoolDT tDungeonTollgatePoolDT = new DungeonTollgatePoolDT((int)EM_Fight_Enum.eFight_DungeonMain, 0, 1, GameParamConst.PLOT_TOLLGATEID);
                    Data_Pool.m_DungeonPool.f_DungeonChallenge(tDungeonTollgatePoolDT, tCallBackDT);
                }
                else
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
                }
                break;
            case eMsgOperateResult.eOR_DuplicateRoleName:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1667));
                break;
            default:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1668) + teMsgOperateResult.ToString());
                break;
        }
    }

    /// <summary>
    /// 剧情战斗回调
    /// </summary>
    /// <param name="result"></param>
    private void CallBack_PlotFight(object result)
    {
        int nResult = -1;
        if (null != result && result is int)
        {
            nResult = (int)result;
        }
        UITool.f_OpenOrCloseWaitTip(false);
        //f_GetObject("BoyModelPoint").SetActive(false);
        //f_GetObject("GirlModelPoint").SetActive(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CreateRolePage, UIMessageDef.UI_CLOSE);
        if (nResult == 0)
        {
            //正确则进行剧情战斗
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
        }
        else
        {
            //否则跳过剧情战斗，直接到主界面
            StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.None);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
        }
    }

    /// <summary>
    /// 点击骰子按钮，随机一个名字
    /// </summary>
    private void OnBtnDiceClick(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        _LoginPage2SelCharacer.Sel_RequestNewName((int)this.selectRoleType);
    }
    /// <summary>
    /// 点击了角色按钮
    /// </summary>
    private void OnBtnRoleClick(GameObject go, object obj1, object obj2)
    {
        EM_RoleSex roleType = (EM_RoleSex)obj1;
        if (this.selectRoleType != roleType)
        {
            UITool.f_OpenOrCloseWaitTip(true);
            this.selectRoleType = roleType;
            
            //_Select(selectRoleType);
            ShowContent();
            ChangeTextureBg();
            LoadTexture(BtnRoleBoy, EM_RoleSex.Man, selectRoleType == EM_RoleSex.Man);
            LoadTexture(BtnRoleGirl, EM_RoleSex.Woman, selectRoleType == EM_RoleSex.Woman);
           
            _LoginPage2SelCharacer.Sel_RequestNewName((int)selectRoleType);
            textFillValue = 0;
            //f_GetObject("TexText").GetComponent<UITexture>().fillAmount = textFillValue;
            f_GetObject("TexText2").GetComponent<UITexture>().fillAmount = textFillValue;
        }
    }
    //My Code
    private void OnSkill1Click(GameObject go, object boj1, object obj2)
    {
        f_GetObject("Info1").SetActive(true);
    }
	private void OnCloseSkill1Click(GameObject go, object boj1, object obj2)
    {
        f_GetObject("Info1").SetActive(false);
    }
	private void OnSkill2Click(GameObject go, object boj1, object obj2)
    {
        f_GetObject("Info2").SetActive(true);
    }
	private void OnCloseSkill2Click(GameObject go, object boj1, object obj2)
    {
        f_GetObject("Info2").SetActive(false);
    }
    /// <summary>
    /// 自动更新
    /// </summary>
    protected override void f_Update()
    {
        base.f_Update();
        if (this.textFillValue < 1)
        {
            textFillValue += 0.95f * Time.deltaTime;
            //f_GetObject("TexText").GetComponent<UITexture>().fillAmount = this.textFillValue;
            f_GetObject("TexText2").GetComponent<UITexture>().fillAmount = 100f * Time.deltaTime;
            //switch (this.selectRoleType)
            //{
            //    case EM_RoleSex.Man:
            //        textFillValue += 0.95f * Time.deltaTime;
            //        f_GetObject("BoyTexText").GetComponent<UITexture>().fillAmount = this.textFillValue;
            //        f_GetObject("BoyTexText2").GetComponent<UITexture>().fillAmount = 100f * Time.deltaTime;
            //        break;
            //    case EM_RoleSex.Woman:
            //        textFillValue += 0.95f * Time.deltaTime;
            //        f_GetObject("GirlTexText").GetComponent<UITexture>().fillAmount = this.textFillValue;
            //        f_GetObject("GirlTexText2").GetComponent<UITexture>().fillAmount = 100f * Time.deltaTime;
            //        break;
            //}
        }
        //GameObject.Find("DiceName").transform.position = f_GetObject("BoyModelPoint").transform.position + (new Vector3(-0.018f,-0.14f,0f));
    }
    
    private void _Select(EM_RoleSex tSelect)
    {
        

        //f_GetObject("BtnRoleBoy").transform.Find("NameBg").GetComponent<UITexture>().mainTexture = tSelect == EM_RoleSex.Man ? NameBgBig : NameBgSmall;
        //f_GetObject("BtnRoleBoy").transform.Find("NameBg").GetComponent<UITexture>().MakePixelPerfect();
        //f_GetObject("BtnRoleGirl").transform.Find("NameBg").GetComponent<UITexture>().mainTexture = tSelect == EM_RoleSex.Woman ? NameBgBig : NameBgSmall;
        //f_GetObject("BtnRoleGirl").transform.Find("NameBg").GetComponent<UITexture>().MakePixelPerfect();

        //f_GetObject("BtnRoleGirl").transform.Find("Select").gameObject.SetActive(tSelect == EM_RoleSex.Woman);
        //f_GetObject("BtnRoleBoy").transform.Find("Select").gameObject.SetActive(tSelect == EM_RoleSex.Man);
    }
}
