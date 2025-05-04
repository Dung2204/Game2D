// #define QUICK_SDK
// #define NEXTGEN_SDK

using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 用户个人信息页面
/// </summary>
public class UserInfoPage : UIFramwork
{
    private int EnergyTimeLeft;//能量恢复倒计时
    private int VigorTimeLeft;//精力恢复倒计时
    private int CrusadeTokenTimeLeft; //征讨令恢复倒计时
    /// <summary>
    /// 打开UI
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        UpdateUI(null);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_USERINFOR, UpdateUI, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_MODELINFOR, UpdateUI, this);
        InvokeRepeating("ChangeTimes", 0f, 1f);
        selectFashionId = 0;
        InitFashion();
        HandleSelectLanguage();
    }


    private List<string> _languageList = new List<string>();
    private UIPopupList _languagePopupList;
    private UILabel _languageSelected;
    private void HandleSelectLanguage()
    {
        _languageList.Clear();
        _languageList.Add("Vietnamese");
        _languageList.Add("English");
        _languageList.Add("ThaiLand");

        _languagePopupList = f_GetObject("languagePopUp").GetComponent<UIPopupList>();
        _languagePopupList.Clear();

        //_languageSelected = f_GetObject("selectedPopUp").GetComponent<UILabel>();
        foreach (var item in _languageList) { _languagePopupList.AddItem(item); }
        int languageSet = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_LanguageSetting, 1);
        //_languageSelected.text =
        _languagePopupList.value = _languageList[languageSet];
        EventDelegate.Add(_languagePopupList.onChange, () => {
            UITool.OnLanguageSelectionChaged(_languageList, _languageSelected);
        });
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    public void UpdateUI(object obj)
    {
        //设置头像信息
        TeamPoolDT teamPoolDT = Data_Pool.m_TeamPool.f_GetForId((int)EM_FormationPos.eFormationPos_Main) as TeamPoolDT;
        f_GetObject("SpriteHead").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteByCardId(teamPoolDT.m_CardPoolDT);
        string cardName = Data_Pool.m_UserData.m_szRoleName;
        f_GetObject("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(teamPoolDT.m_CardPoolDT.m_CardDT.iImportant, ref cardName);
        //设置UI数据
        f_GetObject("PlayerIdContent").GetComponent<UILabel>().text = Data_Pool.m_UserData.m_iUserId.ToString();
        f_GetObject("UserName").GetComponent<UILabel>().text = cardName;
        f_GetObject("ServerIdContent").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1263) +Data_Pool.m_UserData.m_iServerId + (Data_Pool.m_UserData.m_strServerName == null ? "" : Data_Pool.m_UserData.m_strServerName);
        f_GetObject("LabelCardPowValue").GetComponent<UILabel>().text = Data_Pool.m_TeamPool.f_GetTotalBattlePower().ToString();//2.更新战斗力
        int Level = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        CarLvUpDT carLvUpDT = (CarLvUpDT)glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(Level + 1);
        int exMax = carLvUpDT == null ? 1 : carLvUpDT.iCardType; 
        f_GetObject("LabelLevel").GetComponent<UILabel>().text = "LV." + Level.ToString();
        int ex = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Exp);
        f_GetObject("LabelEx").GetComponent<UILabel>().text = ex.ToString() + "/" + (carLvUpDT == null ? "max" : exMax.ToString());
        f_GetObject("SliderLevelBg").GetComponent<UISlider>().value = carLvUpDT == null ? 1 : (ex * 1.0f / exMax);
        float mEnergyLimit = UITool.f_GetNowVipPrivilege((int)EM_VipPrivilege.eVip_EnergyLimit) * 1f;
        f_GetObject("SliderStamina").GetComponent<UISlider>().value = (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy) * 1.0f / mEnergyLimit);
		f_GetObject("SliderVigor").GetComponent<UISlider>().value = (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor) * 1.0f / 30f);
		f_GetObject("SliderChallenge").GetComponent<UISlider>().value = (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_CrusadeToken) * 1.0f / 10f);
        f_GetObject("LabelVip").GetComponent<UILabel>().text = UITool.f_GetNowVipLv().ToString();
        f_GetObject("LabelVersion").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1262) +GloData.glo_strVer;
        f_RegClickEvent("BtnSwitchAccount", OnBtnSwitchAccountClick);
		f_RegClickEvent("BtnDeleteAccount", OnBtnDeleteAccountClick);
#if QUICK_SDK || Y_SDK || NEXTGEN_SDK
        f_GetObject("BtnSwitchAccount").SetActive(true);
#else
        f_GetObject("BtnSwitchAccount").SetActive(false);
        #endif
		
#if UNITY_IOS || UNITY_EDITOR
        f_GetObject("BtnDeleteAccount").SetActive(true);
#else
        f_GetObject("BtnDeleteAccount").SetActive(false);
        #endif

    }
    /// <summary>
    /// 页面关闭，关闭定时任务
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        CancelInvoke("ChangeTimes");
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_USERINFOR, UpdateUI, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_MODELINFOR, UpdateUI, this);
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_USERINFOR, UpdateUI, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_MODELINFOR, UpdateUI, this);
    }
    /// <summary>
    /// 更新时间
    /// </summary>
    private void ChangeTimes()
    {
        int mEnergyLimit = UITool.f_GetNowVipPrivilege((int)EM_VipPrivilege.eVip_EnergyLimit);
        f_GetObject("EnergyValue").GetComponent<UILabel>().text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy) + "/" + mEnergyLimit;
        f_GetObject("VigorValue").GetComponent<UILabel>().text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor) + "/30";
        f_GetObject("ChallengeValue").GetComponent<UILabel>().text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_CrusadeToken).ToString() + "/10";
        int EnergyTimeGone = GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_UserData.m_lastEnergyRestoreTime;
        int VigorTimesGone = GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_UserData.m_lastVigorRestoreTime;
        int CrusadeTokenTimesGone = GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_UserData.m_lastCrusadeRestoreTime;
        GameParamDT gameParamDT = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.EnergyRecover) as GameParamDT;
        GameParamDT gameParamDT2 = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.VigorRecover) as GameParamDT;
        GameParamDT gameParamDT3 = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.CrusadeTokenRecover) as GameParamDT;
        EnergyTimeLeft = gameParamDT.iParam1 * 60 - EnergyTimeGone;
        VigorTimeLeft = gameParamDT2.iParam1 * 60 - VigorTimesGone;
        CrusadeTokenTimeLeft = gameParamDT3.iParam1 * 60 - CrusadeTokenTimesGone;
        EnergyTimeLeft = EnergyTimeLeft < 0 ? 0 : EnergyTimeLeft;
        VigorTimeLeft = VigorTimeLeft < 0 ? 0 : VigorTimeLeft;
        CrusadeTokenTimeLeft = CrusadeTokenTimeLeft < 0 ? 0 : CrusadeTokenTimeLeft;
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy) >= mEnergyLimit)
            f_GetObject("EnergyTimeLeftValue").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1240);
        else
            f_GetObject("EnergyTimeLeftValue").GetComponent<UILabel>().text = CommonTools.f_GetStringBySecond(EnergyTimeLeft);
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor) >= 30)
            f_GetObject("VigorTimeLeftValue").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1240);
        else
            f_GetObject("VigorTimeLeftValue").GetComponent<UILabel>().text = CommonTools.f_GetStringBySecond(VigorTimeLeft);
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_CrusadeToken) >= 10)
            f_GetObject("ChallengeTimeLeftValue").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1240);
        else
            f_GetObject("ChallengeTimeLeftValue").GetComponent<UILabel>().text = CommonTools.f_GetStringBySecond(CrusadeTokenTimeLeft);
    }
    /// <summary>
    /// 初始化消息，注册事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlackBg", OnBtnBlackBgClick);
        f_RegClickEvent("BtnReviseName", OnBtnReviseNameClick);
        f_RegClickEvent("BtnSystemSet", OnBtnSystemSetClick);
        f_RegClickEvent("BtnUserInfo", OnBtnUserInfoClick);
        f_RegClickEvent("BtnExitLogin", OnBtnExitLoginClick);

        f_RegClickEvent("BtnAddEnergy", OnBtnAddEnergyClick);
        f_RegClickEvent("BtnAddVigor", OnBtnAddVigorClick);
        f_RegClickEvent("BtnAddChallenge", OnBtnAddChallengeClick);

        f_RegClickEvent("BtnConnectGM", OnBtnConnectGMClick);
        f_RegClickEvent("BtnNotice", OnBtnNoticeClick);
        f_RegClickEvent("BtnBugReport", OnBtnBugReportClick);

        f_RegClickEvent("MusicSetting", OnBtnSetOpenOrCloseMusic);
        f_RegClickEvent("EffectSetting", OnBtenSetOpenOrCloseAudio);
		f_RegClickEvent("CopyBtn", OnClickCopy);


        UnloadBeforeEquipCallback.m_ccCallbackSuc = UnloadBeforeEquipSucCallback;
        UnloadBeforeEquipCallback.m_ccCallbackFail = UnloadBeforeEquipFailCallback;
        EquipCallback.m_ccCallbackSuc = OnEquipSucCallback;
        EquipCallback.m_ccCallbackFail = OnEquipFailCallback;
        UnloadCallback.m_ccCallbackSuc = OnUnloadSucCallback;
        UnloadCallback.m_ccCallbackFail = OnUnloadFailCallback;
#if UNITY_IOS && !UNITY_EDITOR
        f_GetObject("BtnExitLogin").SetActive(false);
#endif
    }
    #region 按钮消息事件
    /// <summary>
    /// 点击黑色背景，关闭界面
    /// </summary>
    private void OnBtnBlackBgClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.UserInfoPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MainMenu, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击联系GM
    /// </summary>
    private void OnBtnConnectGMClick(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        StartCoroutine(StartCheckGMInfor());
    }
    /// <summary>
    /// 检测有没GM信息
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCheckGMInfor()
    {
        WWW www = new WWW(GloData.glo_strGMInfor);
        yield return www;
        if (www.error != null)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1241));
            yield return null;
        }
        if (www.isDone && www.error == null)
        {
            if (www.text != null && www.text != "")
            {
                MessageBox.DEBUG(www.text);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ConnectGMPage, UIMessageDef.UI_OPEN, www.text);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1241));
            }
        }
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 点击公告
    /// </summary>
    private void OnBtnNoticeClick(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        StartCoroutine(StartCheckNotice());
    }
	
	private void OnBtnDeleteAccountClick(GameObject go, object obj1, object obj2)
    {
PopupMenuParams tParams = new PopupMenuParams("Nhắc nhở", "Xác nhận xóa tài khoản?", "Đồng ý", f_OnSureDeleteAccount, "Hủy", f_OnCancelDeleteAccount);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParams);
    }

    private void f_OnSureDeleteAccount(object Obj)
    {

    }

    private void f_OnCancelDeleteAccount(object Obj)
    {

    }
    /// <summary>
    /// 检测有没有公告
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCheckNotice()
    {
        //WWW www = new WWW(GloData.glo_strNotice + "?channel=" + glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType() + "&version=" + Application.version);
        WWW www = new WWW(ResourceTools.f_CreateNoticeUrl());
        yield return www;
        if (www.error != null)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1242));
            yield return null;
        }
        if (www.isDone && www.error == null)
        {
            if (www.text != null && www.text != "")
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.NoticePanel, UIMessageDef.UI_OPEN, www.text);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1242));
            }
        }
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 点击问题反馈
    /// </summary>
    private void OnBtnBugReportClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BugReportPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击增加体力
    /// </summary>
    private void OnBtnAddEnergyClick(GameObject go, object obj1, object obj2)
    {
        UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Energy, 0, true, true, this);
    }
    /// <summary>
    /// 点击增加精力
    /// </summary>
    private void OnBtnAddVigorClick(GameObject go, object obj1, object obj2)
    {
        UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Vigor, 0, true, true, this);
    }
    /// <summary>
    /// 点击增加挑将令
    /// </summary>
    private void OnBtnAddChallengeClick(GameObject go, object obj1, object obj2)
    {
        UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_CrusadeToken, 0, true, true, this);
    }
    /// <summary>
    /// 退出登录
    /// </summary>
    private void OnBtnExitLoginClick(GameObject go, object obj1, object obj2)
    {
        //glo_Main.GetInstance().m_SDKCmponent.f_Exit();
        glo_Main.GetInstance().m_SDKCmponent.f_QuitApp(); //tsucode -tracking
        Application.Quit();
    }
    
    private void OnBtnSwitchAccountClick(GameObject go, object value1, object value2)
    {
        glo_Main.GetInstance().m_SDKCmponent.f_SwitchAccount(true);
    }

    /// <summary>
    /// 点击修改名字
    /// </summary>
    private void OnBtnReviseNameClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RenamePage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    /// <param name="pageIndex"></param>
    private void UpdateCotent(EM_PageIndex pageIndex)
    {
        bool isPlayerInfo = pageIndex == EM_PageIndex.PlayerInfo ? true : false;
        f_GetObject("UserInfoObj").SetActive(isPlayerInfo);
        f_GetObject("SystemInfoObj").SetActive(!isPlayerInfo);
        if (!isPlayerInfo)
        {
            SetMusicBtn(f_GetObject("MusicSetting"), StaticValue.m_isPlayMusic);
            SetMusicBtn(f_GetObject("EffectSetting"), StaticValue.m_isPlaySound);
        }
        f_GetObject("BtnUserInfo").transform.Find("Normal").gameObject.SetActive(!isPlayerInfo);
        f_GetObject("BtnUserInfo").transform.Find("Press").gameObject.SetActive(isPlayerInfo);
        f_GetObject("BtnSystemSet").transform.Find("Normal").gameObject.SetActive(isPlayerInfo);
        f_GetObject("BtnSystemSet").transform.Find("Press").gameObject.SetActive(!isPlayerInfo);
    }
    /// <summary>
    /// 点击系统设置
    /// </summary>
    private void OnBtnSystemSetClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        UpdateCotent(EM_PageIndex.SettingPage);
    }
    /// <summary>
    /// 点击个人信息按钮
    /// </summary>
    private void OnBtnUserInfoClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        UpdateCotent(EM_PageIndex.PlayerInfo);
    }

    private void OnBtnSetOpenOrCloseMusic(GameObject go, object obj1, object obj2)
    {
        StaticValue.m_isPlayMusic = !StaticValue.m_isPlayMusic;
        LocalDataManager.f_SetLocalData<float>(LocalDataType.Float_MusicVolumn, StaticValue.m_isPlayMusic ? 1 : 0);
        if (!StaticValue.m_isPlayMusic)
            glo_Main.GetInstance().m_AdudioManager.f_PauseAudioMusic();
        else
            glo_Main.GetInstance().m_AdudioManager.f_UnPauseAudioMusic();

        SetMusicBtn(go, StaticValue.m_isPlayMusic);
    }

    private void OnBtenSetOpenOrCloseAudio(GameObject go, object obj1, object obj2)
    {
        StaticValue.m_isPlaySound = !StaticValue.m_isPlaySound;
        LocalDataManager.f_SetLocalData<float>(LocalDataType.Float_EffectVolumn, StaticValue.m_isPlaySound ? 1 : 0);
        if (!StaticValue.m_isPlaySound)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PauseAudioButtle();
        }
        else
            glo_Main.GetInstance().m_AdudioManager.f_UnPauseAudioButtle();

        SetMusicBtn(go, StaticValue.m_isPlaySound);
    }

    private void SetMusicBtn(GameObject go, bool IsOpen)
    {
        go.transform.Find("CloseMusic").gameObject.SetActive(IsOpen);
        go.transform.Find("OpenMuisc").gameObject.SetActive(!IsOpen);
    }
	
	public void OnClickCopy(GameObject go, object obj1, object obj2)
	{
		string IDtext = f_GetObject("PlayerIdContent").GetComponent<UILabel>().text;	
UITool.Ui_Trip("sao chép");
		CopyToClipboard(IDtext);
	}
	
	public static void CopyToClipboard(string str)
    {
        GUIUtility.systemCopyBuffer = str;
    }
#endregion
    /// <summary>
    /// 分页类型
    /// </summary>
    private enum EM_PageIndex
    {
        PlayerInfo = 1,
        SettingPage = 2,
    }
#region 时装相关

    private SocketCallbackDT UnloadBeforeEquipCallback = new SocketCallbackDT();//穿时装前如果有时装先请求脱时装回调
    private SocketCallbackDT EquipCallback = new SocketCallbackDT();//请求穿时装回调
    private SocketCallbackDT UnloadCallback = new SocketCallbackDT();//请求脱时装回调
    /// <summary>
    /// 设置时装UI
    /// </summary>
    private void InitFashion()
    {
        // MessageBox.ASSERT("INIT FASHIONNNN");
        List<BasePoolDT<long>> listPoolDT = Data_Pool.m_FanshionableDressPool.f_GetAll();
        //GridUtil.f_SetGridView<BasePoolDT<long>>(f_GetObject("FashionItemParent"), f_GetObject("FashionItem"), listPoolDT, OnFashionUpdate);
        //f_GetObject("LabelNoFashion").SetActive(listPoolDT.Count <= 0);
        //MessageBox.ASSERT("List "+listPoolDT.Count);

        //TsuCode
        //FanshionableDressPoolDT fashion = new FanshionableDressPoolDT();
        //fashion.iId = 2;
        //fashion.m_iCaridId = 1000;
        //fashion.m_iLimitTime = 999;
        //fashion.m_iTempId = 2;
        //fashion.m_iCaridId = 1000;
        //listPoolDT.Add(fashion);
        GridUtil.f_SetGridView<BasePoolDT<long>>(f_GetObject("FashionItemParent"), f_GetObject("FashionItem"), listPoolDT, OnFashionUpdate);

        //f_GetObject("LabelNoFashion").SetActive(listPoolDT.Count <= 0);
        if (listPoolDT.Count <= 0)
        {
            f_GetObject("LabelNoFashion").SetActive(true);
        }
        else
        {
            f_GetObject("LabelNoFashion").SetActive(false);
        }
        // MessageBox.ASSERT("List " + listPoolDT.Count);
       // MessageBox.ASSERT("Templid " + fashion.m_FashionableDressDT.iModel);
        //MessageBox.ASSERT("fashion " + fashion.m_FashionableDressDT.szName);
        ////////////

    }
    private long selectFashionId;//选中的时装id
    /// <summary>
    /// 时装更新
    /// </summary>
    private void OnFashionUpdate(GameObject go,BasePoolDT<long> data)
    {
        TeamPoolDT teamPoolDT = Data_Pool.m_TeamPool.f_GetForId((long)EM_FormationPos.eFormationPos_Main) as TeamPoolDT;
        FanshionableDressPoolDT tFanshionableDressPoolDT = data as FanshionableDressPoolDT;
        FashionItem fashionItem = go.GetComponent<FashionItem>();
        fashionItem.SetData(tFanshionableDressPoolDT);
        fashionItem.mBtnEquip.SetActive(selectFashionId == tFanshionableDressPoolDT.iId && teamPoolDT.m_CardPoolDT.iId != tFanshionableDressPoolDT.m_iCaridId);
        fashionItem.mBtnUnload.SetActive(selectFashionId == tFanshionableDressPoolDT.iId && teamPoolDT.m_CardPoolDT.iId == tFanshionableDressPoolDT.m_iCaridId);
        f_RegClickEvent(fashionItem.mIcon.gameObject, OnFashionClick, tFanshionableDressPoolDT);
        f_RegClickEvent(fashionItem.mBtnEquip, OnFashionEquipClick, tFanshionableDressPoolDT);
        f_RegClickEvent(fashionItem.mBtnUnload, OnFashionUnloadClick, tFanshionableDressPoolDT);

    }
    /// <summary>
    /// 点击时装
    /// </summary>
    private void OnFashionClick(GameObject go,object obj1,object obj2)
    {
        FanshionableDressPoolDT tFanshionableDressPoolDT = obj1 as FanshionableDressPoolDT;
        selectFashionId = tFanshionableDressPoolDT.iId;
        InitFashion();
    }
    /// <summary>
    /// 点击时装装备
    /// </summary>
    private void OnFashionEquipClick(GameObject go, object obj1, object obj2)
    {
        FanshionableDressPoolDT tFanshionableDressPoolDT = obj1 as FanshionableDressPoolDT;
        tCurFanshionableDressPoolDT = tFanshionableDressPoolDT;
        CardPoolDT mainCardPoolDT = (Data_Pool.m_TeamPool.f_GetForId((int)EM_FormationPos.eFormationPos_Main) as TeamPoolDT).m_CardPoolDT;
        if (mainCardPoolDT.m_FanshionableDressPoolDT != null)
        {
            Data_Pool.m_FanshionableDressPool.f_UnEquip(mainCardPoolDT.m_FanshionableDressPoolDT.iId, UnloadBeforeEquipCallback);
        }
        else
        {
            Data_Pool.m_FanshionableDressPool.f_Equip(mainCardPoolDT.iId, tFanshionableDressPoolDT.iId, EquipCallback);
        }
        UITool.f_OpenOrCloseWaitTip(true);
    }
    private FanshionableDressPoolDT tCurFanshionableDressPoolDT;
    /// <summary>
    /// 点击卸下时装
    /// </summary>
    private void OnFashionUnloadClick(GameObject go, object obj1, object obj2)
    {
        FanshionableDressPoolDT tFanshionableDressPoolDT = obj1 as FanshionableDressPoolDT;
        tCurFanshionableDressPoolDT = tFanshionableDressPoolDT;
        Data_Pool.m_FanshionableDressPool.f_UnEquip(tFanshionableDressPoolDT.iId, UnloadCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    /// <summary>
    /// 穿时装回调
    /// </summary>
    private void OnEquipSucCallback(object obj)
    {
        InitFashion();
        //弹出增加的属性
        RoleProperty roleProperty = new RoleProperty();
        roleProperty.f_AddProperty(tCurFanshionableDressPoolDT.m_FashionableDressDT.iPropertyId1, tCurFanshionableDressPoolDT.m_FashionableDressDT.iPropertyNum1);
        roleProperty.f_AddProperty(tCurFanshionableDressPoolDT.m_FashionableDressDT.iPropertyId2, tCurFanshionableDressPoolDT.m_FashionableDressDT.iPropertyNum2);
        List<object> param = new List<object>();
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1243));
        for (int i = (int)EM_RoleProperty.Atk; i < (int)EM_RoleProperty.End; i++)
        {
            if (roleProperty.f_GetProperty(i) > 0)
            {
                AddProTripPageParam addProParam = new AddProTripPageParam();
                addProParam.addProId = i;
                addProParam.addProValue = roleProperty.f_GetProperty(i);
                param.Add(addProParam);
            }
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AddProTripPage, UIMessageDef.UI_OPEN, param);
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnEquipFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1244) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 穿时装前先脱时装回调
    /// </summary>
    private void UnloadBeforeEquipSucCallback(object obj)
    {
        CardPoolDT mainCardPoolDT = (Data_Pool.m_TeamPool.f_GetForId((int)EM_FormationPos.eFormationPos_Main) as TeamPoolDT).m_CardPoolDT;
        Data_Pool.m_FanshionableDressPool.f_Equip(mainCardPoolDT.iId, tCurFanshionableDressPoolDT.iId, EquipCallback);
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void UnloadBeforeEquipFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1244) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 脱时装回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnUnloadSucCallback(object obj)
    {
        //本地清楚时装
        (Data_Pool.m_TeamPool.f_GetForId((int)EM_FormationPos.eFormationPos_Main) as TeamPoolDT).m_CardPoolDT.m_FanshionableDressPoolDT = null;
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);//更新模型数据
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_USERINFOR);//头像信息
        InitFashion();
        //弹出减少的属性
        RoleProperty roleProperty = new RoleProperty();
        roleProperty.f_AddProperty(tCurFanshionableDressPoolDT.m_FashionableDressDT.iPropertyId1, tCurFanshionableDressPoolDT.m_FashionableDressDT.iPropertyNum1);
        roleProperty.f_AddProperty(tCurFanshionableDressPoolDT.m_FashionableDressDT.iPropertyId2, tCurFanshionableDressPoolDT.m_FashionableDressDT.iPropertyNum2);
        List<object> param = new List<object>();
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1245));
        for (int i = (int)EM_RoleProperty.Atk; i < (int)EM_RoleProperty.End; i++)
        {
            if (roleProperty.f_GetProperty(i) > 0)
            {
                AddProTripPageParam addProParam = new AddProTripPageParam();
                addProParam.addProId = i;
                addProParam.addProValue = -1 * roleProperty.f_GetProperty(i);
                param.Add(addProParam);
            }
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AddProTripPage, UIMessageDef.UI_OPEN, param);
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnUnloadFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1246) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
#endregion
}
