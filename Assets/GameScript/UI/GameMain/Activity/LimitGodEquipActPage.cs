using ccU3DEngine;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LimitGodEquipActPage : UIFramwork
{
    private string strTextBg = "UI/TextureRemove/Activity/Tex_LimitGodBg";
    private string strTextArm = "UI/TextureRemove/Activity/Icon_LimitArm";
    private string strTextArmour = "UI/TextureRemove/Activity/Icon_LimitArmour";
    private string strTextBelt = "UI/TextureRemove/Activity/Icon_LimitBelt";
    private string strTextHelmet = "UI/TextureRemove/Activity/Icon_LimitHelmet";
	private string strTextQuanVu = "UI/TextureRemove/Activity/1201";
	private string strTextGiaCatLuong = "UI/TextureRemove/Activity/1200";
    private GameObject[] _BoxParant;//宝箱父节点
    private GameObject _BoxCommonItem;//公用宝箱预设
    private BoxCommonItem[] _taskBoxItems;//公用宝箱Item
    private GameObject _rankIntegralItemParent;//排行积分父节点
    private GameObject _rankIntegralItem;//排行积分Item
    private GameObject _rankAwardParent;//排行奖励父节点
    private GameObject _rankAwardInfoItem;//排行item
    private GameObject _flopAwardParent;//掉落奖励父节点
    private GameObject _flopAwardItem;//掉落奖励Item
    private GameObject _integralSlider;//积分进度
    private GameObject _btnBuyOne;//购买一次
    private GameObject _btnBuyTen;//购买十次
    private UILabel _buyOneMoney;//购买一次
    private UILabel _buyTenMoney;//购买十次
    private UILabel _actTime;//活动时间
    private UILabel _actLastTime;//剩余时间
    private UILabel _actLastCount;//活动次数
    private UILabel _myRank;//我的排名
    private UILabel _myIntegral;//我的积分
    private UILabel _accumulativeIntefral;
    private GameObject _sorDiscount;
    private GameObject _noManRankSign;
    private long iTime;
    private DateTime tDate;
    private int Time_ShowTime;
    private GodDressPoolDT m_GodDressPoolDT;
    private UIWrapComponent _RankAwardWrapComponent;

    private GameObject BgMagic;

    private Transform EffectParent;   //特效父节点
    private UIWrapComponent mRankAwardWrapComponent
    {
        get
        {
            if (_RankAwardWrapComponent == null)
            {
                //只显示前一百名
                List<BasePoolDT<long>> listRankData = m_GodDressPoolDT.m_GodRankData.GetRange(0, 7);
                _RankAwardWrapComponent = new UIWrapComponent(100, 1, 600, 8, _rankAwardParent, _rankAwardInfoItem, listRankData, RankIntegralItemUpdateByInfo, null);
            }
            return _RankAwardWrapComponent;
        }
    }
    //初始化界面
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        InitUI();
        //宝箱初始化
        _BoxParant = new GameObject[4];
        for (int i = 0; i < _BoxParant.Length; i++)
        {
            _BoxParant[i] = f_GetObject("BoxParent" + (i + 1).ToString());
        }
        _BoxCommonItem = f_GetObject("BoxCommonItem");
        _taskBoxItems = new BoxCommonItem[_BoxParant.Length];
        for (int i = 0; i < _taskBoxItems.Length; i++)
        {
            _taskBoxItems[i] = BoxCommonItem.f_Create(_BoxParant[i], _BoxCommonItem);
            _taskBoxItems[i].f_UpdateClickHandle(f_BoxClickHandle, i);
        }

        //返回按钮事件
        f_RegClickEvent(f_GetObject("BackBtn"), OnCloseClick);
        f_RegClickEvent(_btnBuyOne, OnBuyOneClick);
        f_RegClickEvent(_btnBuyTen, OnBuyTenClick);
        f_RegClickEvent("RankAward", f_OpenRankAwardPage);
    }

    //初始化ui
    private void InitUI()
    {
        //积分进度
        _integralSlider = f_GetObject("BoxSlider");

        _actTime = f_GetObject("ActTimeText").GetComponent<UILabel>();
        _actLastTime = f_GetObject("EndActTimeText").GetComponent<UILabel>();
        _actLastCount = f_GetObject("ActCountNameText").GetComponent<UILabel>();

        //掉落奖励
        _flopAwardParent = f_GetObject("AwardParent");
        _flopAwardItem = f_GetObject("AwardItem");

        //排行积分
        _rankIntegralItemParent = f_GetObject("RankIntegralGrid");
        _rankIntegralItem = f_GetObject("RankIntegralItem");
        //排行奖励
        _rankAwardParent = f_GetObject("GridContentParent");
        _rankAwardInfoItem = f_GetObject("ContentItem");

        _myRank = f_GetObject("RankText").GetComponent<UILabel>();
        _myIntegral = f_GetObject("IntegralText").GetComponent<UILabel>();

        _btnBuyOne = f_GetObject("BtnBuyOne");
        _btnBuyTen = f_GetObject("BtnBuyTen");

        _buyOneMoney = _btnBuyOne.transform.Find("IconProp/LabelCount").GetComponent<UILabel>();
        _buyTenMoney = _btnBuyTen.transform.Find("IconProp/LabelCount").GetComponent<UILabel>();

        _sorDiscount = f_GetObject("SprDiscount");
        _noManRankSign = f_GetObject("ActIntegralRankSign");
        _accumulativeIntefral = f_GetObject("ScoreLabel").GetComponent<UILabel>();

        EffectParent = f_GetObject("EffectParent").transform;
    }

    //打开界面
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        m_GodDressPoolDT = Data_Pool.m_GodDressPool.f_GetCurPoolDt();
        f_LoadTextTure();
        f_LoadEquipTexture();
        InitMoneyInfo();//初始化货币信息
        f_SetTimeCountInfo();
        f_SetFlopProbabilityInfo();//掉落奖励
        f_UpdateTaskBox();//宝箱状态
        f_SetRankIntegralInfo();//设置积分排行信息
        //mRankAwardWrapComponent.f_ResetView();//排行奖励信息
        InitEffect();
        if (f_CheckLimitGodEnd())
        {
            _btnBuyOne.SetActive(true);
            _btnBuyTen.SetActive(true);
        }
        else
        {
            _btnBuyOne.SetActive(false);
            _btnBuyTen.SetActive(false);
            _sorDiscount.SetActive(false);
        }
    }

    //关闭界面
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }


    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }


    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        InitMoneyInfo();
    }

    //加载背景
    private void f_LoadTextTure()
    {
        UITexture textBg = f_GetObject("Texture_BG").GetComponent<UITexture>();
        if (textBg.mainTexture == null)
        {
            Texture2D texTure2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTextBg);
            textBg.mainTexture = texTure2D;
        }
    }

    //加载神装装备图片
    private void f_LoadEquipTexture()
    {
        List<ResourceCommonDT> resData = CommonTools.f_GetListCommonDT(m_GodDressPoolDT.m_GodDressDT.szAward);
        UITexture textArmSprite = f_GetObject("IconArmSprite").GetComponent<UITexture>();
        textArmSprite.transform.localPosition = new Vector3(-4, 28, 0);
        if (resData[0].mResourceId == 3044)
        {
            Texture2D texTure2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTextArm);
            textArmSprite.mainTexture = texTure2D;
            textArmSprite.transform.localPosition = new Vector3(-4,93,0);
        }
        if (resData[0].mResourceId == 3045)
        {
            Texture2D texTure2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTextArmour);
            textArmSprite.mainTexture = texTure2D;
        }
        if (resData[0].mResourceId == 3047)
        {
            Texture2D texTure2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTextBelt);
            textArmSprite.mainTexture = texTure2D;
        }
        if (resData[0].mResourceId == 3046)
        {
            Texture2D texTure2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTextHelmet);
            textArmSprite.mainTexture = texTure2D;
            
        }
		if (resData[0].mResourceId == 1201)
        {
            Texture2D texTure2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTextQuanVu);
            textArmSprite.mainTexture = texTure2D;
			textArmSprite.transform.localPosition = new Vector3(-45,55,0);
            
        }
		if (resData[0].mResourceId == 1200)
        {
            Texture2D texTure2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTextGiaCatLuong);
            textArmSprite.mainTexture = texTure2D;
			textArmSprite.transform.localPosition = new Vector3(0,10,0);
            
        }
        textArmSprite.MakePixelPerfect();
    }

    //检测活动是否结束
    public bool f_CheckLimitGodEnd()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_GodDressSC.f_GetAll();
        for (int i = 0; i < listDT.Count; i++)
        {
            GodDressDT dt = listDT[i] as GodDressDT;
            int timeNow = GameSocket.GetInstance().f_GetServerTime();

            long timeStart = ccMath.DateTime2time_t(GetTimeByTimeStr(dt.iBeginTime));
            long timeEnd = ccMath.DateTime2time_t(GetTimeByTimeStr(dt.iEndTime));
            //Debug.LogError("timeNow:" + timeNow+ ">>timeStart:"+ timeStart+ ">>timeEnd:"+ timeEnd);
            if (timeStart < timeNow && timeNow < timeEnd)
            {
                return true;
            }
        }
        return false;
    }

    //初始化货币信息
    private void InitMoneyInfo()
    {
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }

    //设置时间次数等信息
    private void f_SetTimeCountInfo()
    {
        DateTime BeginTime= Data_Pool.m_GodDressPool.GetTimeByTimeStr(m_GodDressPoolDT.m_GodDressDT.iBeginTime);
        DateTime EndTime = Data_Pool.m_GodDressPool.GetTimeByTimeStr(m_GodDressPoolDT.m_GodDressDT.iEndTime);
        //string beginTime = m_GodDressPoolDT.m_GodDressDT.iBeginTime.ToString();
        //string endTime = m_GodDressPoolDT.m_GodDressDT.iEndTime.ToString();
        //int bYear = int.Parse(beginTime.Substring(0, 4));
        //int bMonth = int.Parse(beginTime.Substring(4, 2));
        //int bDay = int.Parse(beginTime.Substring(6, 2));
        //int eYear = int.Parse(endTime.Substring(0, 4));
        //int eMonth = int.Parse(endTime.Substring(4, 2));
        //int eDay = int.Parse(endTime.Substring(6, 2));
        //long timeTemp = ccMath.DateTime2time_t(GetTimeByTimeStr(m_GodDressPoolDT.m_GodDressDT.iEndTime.ToString())) - 86400;
        //DateTime tDateDay = ccMath.time_t2DateTime(timeTemp);
        _actTime.text = BeginTime.Month + CommonTools.f_GetTransLanguage(1500) + BeginTime.Day + CommonTools.f_GetTransLanguage(1501) + "-" + EndTime.Month + CommonTools.f_GetTransLanguage(1500) + EndTime.Day + CommonTools.f_GetTransLanguage(1501);
        Time_ShowTime = ccTimeEvent.GetInstance().f_RegEvent(1f, true, null, _UpdateTimeData);
        _buyTenMoney.text = m_GodDressPoolDT.m_GodDressDT.iTenPrice.ToString();
        f_UpdataCountMoeny();
    }

    //更新次数与购买元宝
    private void f_UpdataCountMoeny()
    {
        if (m_GodDressPoolDT.m_TodayCurCount == 0)
        {
            _sorDiscount.SetActive(true);
            _buyOneMoney.text = (m_GodDressPoolDT.m_GodDressDT.iOnePrice * 0.1).ToString();
        }

        else
        {
            _sorDiscount.SetActive(false);
            _buyOneMoney.text = m_GodDressPoolDT.m_GodDressDT.iOnePrice.ToString();
        }
        //当前次数 / 最大次数
        _actLastCount.text = m_GodDressPoolDT.m_GodDressDT.szTheme; //m_GodDressPoolDT.m_TodayCurCount.ToString() + "/" + m_GodDressPoolDT.m_GodDressDT.iBuyTimes.ToString();
        _myRank.text = m_GodDressPoolDT.m_MyRank.ToString();
        _myIntegral.text = m_GodDressPoolDT.m_Integral.ToString();

    }

    // 时间
    private void _UpdateTimeData(object boj)
    {
        long timeEnd = ccMath.DateTime2time_t(GetTimeByTimeStr(m_GodDressPoolDT.m_GodDressDT.iEndTime));
        iTime = timeEnd  - GameSocket.GetInstance().f_GetServerTime();
        MessageBox.ASSERT("ID: " + m_GodDressPoolDT.m_GodDressDT.iId + " Ten " + m_GodDressPoolDT.m_GodDressDT.szTheme);
        //MessageBox.ASSERT("End Time: "+ GetTimeByTimeStr(m_GodDressPoolDT.m_GodDressDT.iEndTime) + " TImeSV " + ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime()) +" iTime : "+iTime);
        if (iTime <= 0)
        {
            //活动结束
            _actLastTime.text = CommonTools.f_GetTransLanguage(1502);
            return;
        }
        tDate = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime);
        if (tDate.Day != 1)
            _actLastTime.text = string.Format(CommonTools.f_GetTransLanguage(1503), tDate.Day - 1, tDate.Hour, tDate.Minute) + tDate.Second + CommonTools.f_GetTransLanguage(1505);
        else
            _actLastTime.text = string.Format(CommonTools.f_GetTransLanguage(1504), tDate.Hour, tDate.Minute) + tDate.Second + CommonTools.f_GetTransLanguage(1505);

        DateTime tDate3 = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        if ((GameSocket.GetInstance().mNextDayTime - tDate3).TotalSeconds < 0)
        {
            _Next2Day();
        }
    }

    /// <summary>
    /// 跨天时间
    /// </summary>
    private void _Next2Day()
    {
        SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
        QueryCallback.m_ccCallbackSuc = OnGodDressSucCallback;
        Data_Pool.m_GodDressPool.f_QueryGodDressInfo(QueryCallback);
    }

    //查询成功回调
    private void OnGodDressSucCallback(object obj)
    {
        f_LoadEquipTexture();
        InitMoneyInfo();//初始化货币信息
        f_SetTimeCountInfo();
        f_SetFlopProbabilityInfo();//掉落奖励
        f_UpdateTaskBox();//宝箱状态
        f_SetRankIntegralInfo();//设置积分排行信息
        //mRankAwardWrapComponent.f_ResetView();//排行奖励信息
    }


    private DateTime GetTimeByTimeStr(int StrTime)
    {
        //int TimeTike = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + (StrTime-1) * 86400;

        ////int bYear = int.Parse(StrTime.Substring(0, 4));
        ////int bMonth = int.Parse(StrTime.Substring(4, 2));
        ////int bDay = int.Parse(StrTime.Substring(6, 2));

        //DateTime time = ccMath.time_t2DateTime(TimeTike);
        ////Debug.LogError("time:"+ time);
        //return time;

        //TsuCode
        String a = StrTime.ToString(); //TsuCode
        int bYear = int.Parse(a.Substring(0, 4)); //TsuCode - OpenComment
        int bMonth = int.Parse(a.Substring(4, 2)); //TsuCode - OpenComment
        int bDay = int.Parse(a.Substring(6, 2)); //TsuCode - OpenComment
        DateTime start = new DateTime(bYear, bMonth, bDay, 0, 0, 0, 0); //TsuCode
        return start;
        //
    }

    //设置掉落几率道具e
    private void f_SetFlopProbabilityInfo()
    {
        List<ResourceCommonDT> listCommonDT = new List<ResourceCommonDT>();
        List<AwardPoolDT> awardList = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(m_GodDressPoolDT.m_GodDressDT.iBuyAward);
        for (int i = 0; i < awardList.Count; i++)
        {
            if (i >= 3)
            {
                break;
            }
            ResourceCommonDT commonData = new ResourceCommonDT();
            commonData.f_UpdateInfo((byte)awardList[i].mTemplate.mResourceType, awardList[i].mTemplate.mResourceId, awardList[i].mTemplate.mResourceNum);
            listCommonDT.Add(commonData);
        }
        GridUtil.f_SetGridView<ResourceCommonDT>(_flopAwardParent, _flopAwardItem, listCommonDT, OnAwardItemUpdate);
    }

    //设置积分排行信息
    private void f_SetRankIntegralInfo()
    {
        List<RankUserInfo> listRankInfo = new List<RankUserInfo>();
        SC_GodDressRankInfo _GodDressRank = Data_Pool.m_GodDressPool.m_GodDressRankInfo;
        //if (_GodDressRank.m_RankUserInfo.Length == 0) return; //TsuCode
        for (int i = 0; i < _GodDressRank.m_RankUserInfo.Length; i++)
        {
            RankUserInfo userRankData = (RankUserInfo)_GodDressRank.m_RankUserInfo[i];

            if (userRankData.idx == 0)
                continue;
            listRankInfo.Add(userRankData);
        }
        if (listRankInfo.Count == 0)
            _noManRankSign.SetActive(true);
        else
            _noManRankSign.SetActive(false);
        GridUtil.f_SetGridView<RankUserInfo>(_rankIntegralItemParent, _rankIntegralItem, listRankInfo, OnRankIntegralItemUpdate);
    }

    //排行积分Item更新
    private void OnRankIntegralItemUpdate(GameObject item, RankUserInfo data)
    {
        UILabel name = item.transform.Find("Name").GetComponent<UILabel>();
        UILabel point = item.transform.Find("Point").GetComponent<UILabel>();
        UILabel roleName = item.transform.Find("NameRole").GetComponent<UILabel>();
        name.text = "[F9EB77FF]" + data.idx.ToString() + "[-]" + ".";
        roleName.text = data.szRoleName;
        point.text = data.uScore.ToString();
    }


    //排行奖励Item更新
    private void RankIntegralItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        GodRankData data = (GodRankData)dt;
        UILabel rankText = item.Find("Index").GetComponent<UILabel>();
        GameObject awardParent = item.Find("ScrollView/Grid").gameObject;
        GameObject awardItem = item.Find("ResourceCommonItem").gameObject;
        //名次
        if (data.m_BeginRank == data.m_End_Rank)
        {
            rankText.text = string.Format(CommonTools.f_GetTransLanguage(1506), data.m_BeginRank.ToString());
        }
        else
        {
            rankText.text = string.Format(CommonTools.f_GetTransLanguage(1507), data.m_BeginRank.ToString(), data.m_End_Rank > 101 ? "∞" : data.m_End_Rank.ToString());
        }
        //设置奖励
        GridUtil.f_SetGridView<ResourceCommonDT>(awardParent, awardItem, data.m_ResourceCommonDTList, CommonAwarUpdate);
    }

    //奖励item更新
    private void OnAwardItemUpdate(GameObject item, ResourceCommonDT data)
    {
        //item.GetComponent<ResourceCommonItem>().f_UpdateByInfo(data);
        if ((EM_ResourceType)data.mResourceType == EM_ResourceType.EquipFragment)
            item.transform.Find("fragementLabel_").gameObject.SetActive(true);
        else
            item.transform.Find("fragementLabel_").gameObject.SetActive(false);
        string sailResName = data.mName;
        item.transform.Find("LabelCount").GetComponent<UILabel>().text = data.mResourceNum.ToString();
        item.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(data.mImportant, ref sailResName);
        item.transform.GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(data.mIcon);
        f_RegClickEvent(item, UITool.f_OnItemIconClick, data);
    }

    private void CommonAwarUpdate(GameObject item, ResourceCommonDT data) {
        item.GetComponent<ResourceCommonItem>().f_UpdateByInfo(data);
    }


    // 更新宝箱状态
    private void f_UpdateTaskBox()
    {
        float value = (float)m_GodDressPoolDT.m_CurAccumIntegral / (float)Data_Pool.m_GodDressPool.maxIntegral;
        if (value >= 1)
            _integralSlider.GetComponent<UISlider>().value = 1;
        else
            _integralSlider.GetComponent<UISlider>().value = value;
        _accumulativeIntefral.text = m_GodDressPoolDT.m_CurAccumIntegral.ToString();
        for (int i = 0; i < _taskBoxItems.Length; i++)
        {
            _taskBoxItems[i].f_UpdateInfo(EM_BoxType.Task, Data_Pool.m_GodDressPool.f_GetBoxStateByIdx(i), Data_Pool.m_GodDressPool.f_GetBoxExtraInfo(i), true);
        }
    }
    private int mCurBoxIdx = 0;
    //点击宝箱信息
    private void f_BoxClickHandle(object value)
    {
        int id = (int)(value);
        mCurBoxIdx = id;
        GodEquipBoxInfo tBoxInfo = Data_Pool.m_GodDressPool.f_GetBoxInfo(id);
        BoxGetSubPageParam tParam = new BoxGetSubPageParam(tBoxInfo.mBoxId, tBoxInfo.mIdx, tBoxInfo.mBoxScore.ToString(), EM_BoxType.Task, tBoxInfo.mBoxState, f_BoxGetHandle, this);
        //打开宝箱显示界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_OPEN, tParam);
    }
   
    //宝箱领取事件
    private void f_BoxGetHandle(object value)
    {
        int id = (int)value;
        EM_BoxGetState tBoxState = Data_Pool.m_GodDressPool.f_GetBoxStateByIdx(id);
        if (tBoxState == EM_BoxGetState.CanGet)
        {
            SocketCallbackDT callbackDT = new SocketCallbackDT();
            callbackDT.m_ccCallbackSuc = f_GetBoxAwardSuc;
            callbackDT.m_ccCallbackFail = f_GetBoxAwardFail;
            Data_Pool.m_GodDressPool.f_QueryGodDressBox((byte)id, callbackDT);
            UITool.f_OpenOrCloseWaitTip(true);
        }
    }
    //领取成功回调
    private void f_GetBoxAwardSuc(object obj)
    {
        GodEquipBoxInfo tBoxInfo = Data_Pool.m_GodDressPool.f_GetBoxInfo(mCurBoxIdx);
        //关闭宝箱领取界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_CLOSE);
        //展示获得奖励
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(tBoxInfo.mBoxId), this });
        f_UpdateTaskBox();
        UITool.f_OpenOrCloseWaitTip(false);
    }

    //领取失败回调
    private void f_GetBoxAwardFail(object obj)
    {
        UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1508) + obj);
        MessageBox.ASSERT("GetBoxAwardFail. Fail Code:" + obj);
        //关闭宝箱领取界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_CLOSE);
        f_UpdateTaskBox();
        UITool.f_OpenOrCloseWaitTip(false);
    }


    //关闭界面点击事件
    private void OnCloseClick(GameObject go, object value1, object value2)
    {
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_ShowTime);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LimitGodEquipActPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    //购买一次事件
    private void OnBuyOneClick(GameObject go, object value1, object value2)
    {
       
        int syceeCount = UITool.f_GetResourceHaveNum((int)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee);
    
        int buyMoeny = m_GodDressPoolDT.m_GodDressDT.iOnePrice;
       
        if (m_GodDressPoolDT.m_TodayCurCount == 0)
        {
            buyMoeny = (int)(buyMoeny * 0.1);
        }
        if (syceeCount < buyMoeny)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1509));
            return;
        }
        if (m_GodDressPoolDT.m_GodDressDT.iBuyTimes - m_GodDressPoolDT.m_TodayCurCount <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1510));
            return;
        }
        SocketCallbackDT callbackDT = new SocketCallbackDT();
        callbackDT.m_ccCallbackSuc = f_GetBuyOneSuc;
        callbackDT.m_ccCallbackFail = f_GetBuyOneFail;
        Data_Pool.m_GodDressPool.f_QueryGodDressBuy((short)m_GodDressPoolDT.iId, (byte)1, callbackDT);
        UITool.f_OpenOrCloseWaitTip(true);
     
    }

    private void f_OpenRankAwardPage(GameObject go, object value1, object value2)
    {
        CommonShowAwardParam param = new CommonShowAwardParam();
        param.m_Item = f_GetObject("Item");

        //param.m_PoolDTLise = m_GodDressPoolDT.m_GodRankData.GetRange(0, 7);
        param.m_PoolDTLise = m_GodDressPoolDT.m_GodRankData.GetRange(0, 6);
        param.m_PoolDTUpdate = RankIntegralItemUpdateByInfo;

        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonShowAward, UIMessageDef.UI_OPEN, param);
    }

    //购买一次成功回调
    private void f_GetBuyOneSuc(object obj)
    {
     
        f_UpdataCountMoeny();
        f_SetRankIntegralInfo();
        f_UpdateTaskBox();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
                   new object[] { Data_Pool.m_GodDressPool.m_GodAwardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    //购买一次失败
    private void f_GetBuyOneFail(object obj)
    {
      
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1511) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);

    }

    //购买十次事件
    private void OnBuyTenClick(GameObject go, object value1, object value2)
    {
        int syceeCount = UITool.f_GetResourceHaveNum((int)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee);
        int buyMoeny = m_GodDressPoolDT.m_GodDressDT.iTenPrice;
        if (syceeCount < buyMoeny)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1509));
            return;
        }
        int num = m_GodDressPoolDT.m_GodDressDT.iBuyTimes - m_GodDressPoolDT.m_TodayCurCount;
        if (num < 10 && num > 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1513));
            return;
        }
        if (num <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1514));
            return;
        }
        SocketCallbackDT callbackDT = new SocketCallbackDT();
        callbackDT.m_ccCallbackSuc = f_GetBuyTenSuc;
        callbackDT.m_ccCallbackFail = f_GetBuyTenFail;
        Data_Pool.m_GodDressPool.f_QueryGodDressBuy((short)m_GodDressPoolDT.iId, (byte)2, callbackDT);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    //购买十次成功回调
    private void f_GetBuyTenSuc(object obj)
    {
        f_UpdataCountMoeny();
        f_SetRankIntegralInfo();
        f_UpdateTaskBox();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
           new object[] { Data_Pool.m_GodDressPool.m_GodAwardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    //购买十次失败
    private void f_GetBuyTenFail(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1515) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }



    #region 特效

    private void InitEffect()
    {
        if (EffectParent.childCount == 0)
        {
            UITool.f_CreateEffect(UIEffectName.UIEffectAddress4, UIEffectName.shenzhuangbg, EffectParent);
            UITool.f_CreateMagicById((int)EM_MagicId.eLimiGodEquip,ref BgMagic, EffectParent,0,null);
            BgMagic.transform.localScale = Vector3.one * 110;

        }
    }


    #endregion
}

