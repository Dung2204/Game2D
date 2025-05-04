using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
using Spine;
using Spine.Unity;
/// <summary>
/// 活动首充界面
/// </summary>
public class RankingPowerPage : UIFramwork
{
    private bool initUI;
    EventTimeDT eventTimeDT;
    List<NBaseSCDT> RankingPowerAwardDT;
    EventTimePoolDT tPoolDataDT;
    private UILabel ActivityEndTime;   //活
    private bool mFirstPage = true;
    private UIScrollView mScrollView;
    private GameObject mAwardItemParent;
    private GameObject mAwardItem;
    private UIWrapComponent _rankWrapComponet;

    private GameObject endEvent;
    public UIWrapComponent mRankWrapComponet
    {
        get
        {
            if (_rankWrapComponet == null)
            {
                List<BasePoolDT<long>> _rankList = new List<BasePoolDT<long>>();
                switch (eventTimeDT.szNameConst)
                {
                    case "RankingPowerPage":
                        _rankList = Data_Pool.m_RankingPowerAwardPool.f_GetRankList();
                        break;
                    case "RankingGodEquipPage":
                        _rankList = Data_Pool.m_RankingPowerAwardPool.f_GetRankGodEquipList();
                        break;
                    case "RankingTariningPage":
                        _rankList = Data_Pool.m_RankingPowerAwardPool.f_GetRankTariningList();

                        break;
                }

                _rankWrapComponet = new UIWrapComponent(140, 1, 140, 3, mAwardItemParent, mAwardItem, _rankList, f_AwardItemUpdateByInfo, f_OnAwardItemClick);
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
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_RANKING_POWER_LIST, OnGetSucCallback, this);
        Debug.Log(eventTimeDT.iId);
        f_LoadTexture();
        UpdateContent();

    }

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }
    protected override void InitGUI()
    {
        base.InitGUI();

        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
        mScrollView.onDragFinished = f_OnMomnetEnds;
        mAwardItemParent = f_GetObject("Grid");
        mAwardItem = f_GetObject("CommonTab");
        ActivityEndTime = f_GetObject("ActivityEndTime").GetComponent<UILabel>();
        endEvent = f_GetObject("endevent");
    }

    private void f_OnMomnetEnds()
    {
        Vector3 constraint = mScrollView.panel.CalculateConstrainOffset(mScrollView.bounds.min, mScrollView.bounds.min);
        if (constraint.y <= 0)
        {
            mFirstPage = false;
            GetListRankByEventName();
        }
    }

    protected override void f_Create()
    {
        _InitReference();
        base.f_Create();
    }
    private void _InitReference()
    {
        AddGOReference("Panel/Anchor-Center/BlackBg");
        //AddGOReference("Panel/Anchor-Center/UI/BtnClose");
        AddGOReference("Panel/Anchor-Center/UI/RightPanel/ScrollView");
        AddGOReference("Panel/Anchor-Center/UI/RightPanel/ScrollView/Grid");
        AddGOReference("Panel/Anchor-Center/UI/RightPanel/CommonTab");
        AddGOReference("Panel/Anchor-Center/UI/RightPanel/MyInfoItem");
        AddGOReference("Panel/Anchor-Center/UI/RightPanel/DayNum/ActivityEndTime");
        AddGOReference("Panel/Anchor-Center/UI/Panel/endevent");
        AddGOReference("Panel/Anchor-Center/UI/Banner1");
        AddGOReference("Panel/Anchor-Center/UI/Banner2");
        AddGOReference("Panel/Anchor-Center/UI/BG /Popup/Title/Power");
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        string szCenterBgFile = "UI/TextureRemove/RankList/";
        string baner1 = szCenterBgFile + eventTimeDT.szNameConst + "1";
        string baner2 = szCenterBgFile + eventTimeDT.szNameConst + "2";
        f_GetObject("Banner1").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(baner1);
        f_GetObject("Banner2").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(baner2);

    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBg", OnCloseThis);
        //f_RegClickEvent("BtnClose", OnCloseThis);


    }

    private void OnCloseThis(GameObject go, object obj1, object obj2)
    {
        Data_Pool.m_RankingPowerAwardPool.ClearListRank();
        mRankWrapComponet.f_ResetView();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RankingPowerPage, UIMessageDef.UI_CLOSE);
        f_DestoryView();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_RANKING_POWER_LIST, OnGetSucCallback, this);

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
        //labelRank.gameObject.SetActive(rank > 3);
        if (rank > 3)
        {
            //labelRank.text = rank.ToString();
        }
        else
        {
            spriteRank.spriteName = "" + rank;
            spriteRank.MakePixelPerfect();
        }
        labelRank.text = rank.ToString();


        labelName.text = rankingPowerAwardPoolDT.UserName;


        Transform GoodsParent = tf.Find("GoodsParent");
        Transform Item = GoodsParent.Find("Item");

        RankingPowerAwardDT rankingPowerAwardDT = glo_Main.GetInstance().m_SC_Pool.m_RankingPowerAwardSC.GetAwardDTByRank(eventTimeDT.iId, rank);

        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(rankingPowerAwardDT.szAward);
        GridUtil.f_SetGridView<ResourceCommonDT>(GoodsParent.gameObject, Item.gameObject, listCommonDT, UpdateItem);
        GoodsParent.GetComponent<UIGrid>().Reposition();

    }

    private void f_UpdateMyRankInfo()
    {
        GameObject gameObject = f_GetObject("MyInfoItem");
        if (gameObject == null) return;
        Transform tf = f_GetObject("MyInfoItem").transform;

        //UI2DSprite headIcon = tf.Find("HeadIcon").GetComponent<UI2DSprite>();
        //UISprite headFrame = tf.Find("HeadFrame").GetComponent<UISprite>();
        //GameObject objFightPower = tf.Find("Sprite_FightPower").gameObject;
        UILabel labelPower = tf.Find("PowerLabel").GetComponent<UILabel>();
        UILabel titlePower = f_GetObject("Power").GetComponent<UILabel>();
        //UISprite spriteRank = tf.Find("Sprite_Rank").GetComponent<UISprite>();
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


        int iMyRank = Data_Pool.m_RankingPowerAwardPool.f_GetMyRank();
        int iMyScore = Data_Pool.m_RankingPowerAwardPool.f_GetMyScore();


        //spriteRank.gameObject.SetActive(rank <= 3);
        //labelRank.gameObject.SetActive(rank > 3);
        //if (rank > 3)
        //{
        //    labelRank.text = rank.ToString();
        //}
        //else
        //{
        //    spriteRank.spriteName = "" + rank;
        //}

        labelRank.text = iMyRank == 0 ? CommonTools.f_GetTransLanguage(1037) : string.Format(CommonTools.f_GetTransLanguage(2283), iMyRank.ToString()) ;

        labelName.text = string.Format(CommonTools.f_GetTransLanguage(2284), Data_Pool.m_UserData.m_szRoleName) ;

        switch (eventTimeDT.szNameConst)
        {
            case "RankingPowerPage":
                labelPower.text = string.Format(CommonTools.f_GetTransLanguage(2285), iMyScore);
                titlePower.text = CommonTools.f_GetTransLanguage(2313);
                break;
            case "RankingGodEquipPage":
            case "RankingTariningPage":
                labelPower.text = string.Format(CommonTools.f_GetTransLanguage(2311), iMyScore);
                titlePower.text = CommonTools.f_GetTransLanguage(2312);
                break;
        }
    }

    private void f_OnAwardItemClick(Transform tf, BasePoolDT<long> dt)
    { 
    
    }
    int iTime1;
    DateTime tDate1;
    private void UpdateContent()
    {

        RankingPowerAwardDT = glo_Main.GetInstance().m_SC_Pool.m_RankingPowerAwardSC.f_GetSCByEventTimeId(eventTimeDT.iId);
        if(RankingPowerAwardDT.Count <= 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RankingPowerPage, UIMessageDef.UI_CLOSE);
Debug.Log("don't set gift id for this event time");
            return;
        }

        switch (eventTimeDT.iType)
        {
            case 0: break;
            case 1:
                break;
            case 2:
                iTime1 = (Data_Pool.m_EventTimePool.OpenSeverTime + eventTimeDT.iEndTime*3600*24) - GameSocket.GetInstance().f_GetServerTime();
                break;
            case 3:
                iTime1 = CommonTools.f_GetActStarTimeForOpenSeverTime(eventTimeDT.iEndTime);
                break;
            case 4:
                // ngay tao nhan vat
                break;
            case 5:
                //mo theo pool tra ve
                break;
            default:

                break;
        }

        tDate1 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime1);

        if(iTime1 > 0)
        {
            ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2112), tDate1.Day - 1, tDate1.Hour, tDate1.Minute) + tDate1.Second + CommonTools.f_GetTransLanguage(2114);
            endEvent.SetActive(false);
        }
        else
        {
            ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2115));
        }

        GetListRankByEventName();
    }

    private void GetListRankByEventName()
    {
        switch (eventTimeDT.szNameConst)
        {
            case "RankingPowerPage":
                Data_Pool.m_RankingPowerAwardPool.f_RankList();
                break;
            case "RankingGodEquipPage":
                Data_Pool.m_RankingPowerAwardPool.f_RankGodEquipList();
                break;
            case "RankingTariningPage":
                Data_Pool.m_RankingPowerAwardPool.f_RankTariningList();
                break;
        }
    }

    private int InitKey(int idA, int idB)
    {
        string key = idA + "" + idB;
        return int.Parse(key);
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
        CreareEffect(tran.Find("Effect").gameObject, Data.mImportant);

        f_RegClickEvent(Icon.gameObject, OnClickItem, Data);
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
        SetEquipEffect.transform.parent.localScale = Vector3.one * 140;
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
            if(iTime1 > 0)
            {
                iTime1--;
                tDate1 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime1);
                ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2112), tDate1.Day - 1, tDate1.Hour, tDate1.Minute) + tDate1.Second + CommonTools.f_GetTransLanguage(2114);
                endEvent.gameObject.SetActive(false);
            }
            else
            {
                ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2115));
                endEvent.gameObject.SetActive(true);
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
        //server đã gởi về pool rank power
        UITool.f_OpenOrCloseWaitTip(false);
        List<BasePoolDT<long>> _rankList = null;

        switch (eventTimeDT.szNameConst)
        {
            case "RankingPowerPage":
                _rankList = Data_Pool.m_RankingPowerAwardPool.f_GetRankList();
                break;
            case "RankingGodEquipPage":
                _rankList = Data_Pool.m_RankingPowerAwardPool.f_GetRankGodEquipList();
                break;
            case "RankingTariningPage":
                _rankList = Data_Pool.m_RankingPowerAwardPool.f_GetRankTariningList();

                break;
        }

        mRankWrapComponet.f_UpdateList(_rankList);
        mRankWrapComponet.f_UpdateView();

        //f_GetObject("ScrollView").GetComponent<UIScrollView>().ResetPosition();
        f_UpdateMyRankInfo();
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

}

