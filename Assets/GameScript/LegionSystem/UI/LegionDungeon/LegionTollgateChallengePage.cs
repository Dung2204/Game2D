using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class LegionTollgateChallengePage : UIFramwork
{
    private UISlider mHpSlider;
    private UILabel mHpLabel;
    private UISprite mCampSprite;
    private UILabel mTollgateName;
    private GameObject[] mCampTipLabels;
    private UILabel mContriLabel;
    private UILabel mFiniExpLabel;
    private UILabel mKillContriLabel;
    private UILabel mChallengeTimesLabel;
    private GameObject mItemParent;
    private GameObject mItem;

    private ResourceCommonItem mShowItem;

    LegionTollgateChallengePageParam mCurParam;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mHpSlider = f_GetObject("HpSlider").GetComponent<UISlider>();
        mHpLabel = f_GetObject("HpLabel").GetComponent<UILabel>();
        mCampSprite = f_GetObject("CampSprite").GetComponent<UISprite>();
        mTollgateName = f_GetObject("TollgateName").GetComponent<UILabel>();
        mCampTipLabels = new GameObject[4];
        for (int i = 0; i < mCampTipLabels.Length; i++)
        {
            mCampTipLabels[i] = f_GetObject(string.Format("CampTipLabel{0}", i + 1));
        }
        mContriLabel = f_GetObject("ContriLabel").GetComponent<UILabel>();
        mFiniExpLabel = f_GetObject("FiniExpLabel").GetComponent<UILabel>(); ;
        mKillContriLabel = f_GetObject("KillContriLabel").GetComponent<UILabel>();
        mChallengeTimesLabel = f_GetObject("ChallengeTimesLabel").GetComponent<UILabel>();
        mItemParent = f_GetObject("ItemParent");
        mItem = f_GetObject("NameAndIconItem");
        f_RegClickEvent("ChallengeBtn", f_ChallengeBtn);
        f_RegClickEvent("CloseBtn", f_CloseBtn);
        f_RegClickEvent("MaskClose", f_CloseBtn);
        f_RegClickEvent("LineupBtn", f_LineupBtn);
        mShowItem = ResourceCommonItem.f_Create(mItemParent, mItem);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is LegionTollgateChallengePageParam))
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(670));//军团副本
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateChallengePage, UIMessageDef.UI_CLOSE);
            return;
        }
        mCurParam = (LegionTollgateChallengePageParam)e;
        LegionTollgateDT tollgateDt = (LegionTollgateDT)glo_Main.GetInstance().m_SC_Pool.m_LegionTollgateSC.f_GetSC(mCurParam.mInfo.mTollgateTemplate.iId);
        float hpPrecent = (float)mCurParam.mInfo.mHp / mCurParam.mInfo.mHpMax;
        mHpSlider.value = hpPrecent;
        mHpLabel.text = string.Format(CommonTools.f_GetTransLanguage(671), mCurParam.mInfo.mHp, mCurParam.mInfo.mHpMax);//生命值
        LegionTool.f_SetCampSpriteByCamp(mCampSprite, mCurParam.mInfo.mCamp);
        mCampSprite.MakePixelPerfect();
        mCampSprite.transform.localScale = new Vector3(1.3f, 1.3f, 1.0f);
        mTollgateName.text = mCurParam.mInfo.mTollgateTemplate.szName;
        mCampTipLabels[0].SetActive(mCurParam.mInfo.mCamp == EM_CardCamp.eCardWei);
        mCampTipLabels[1].SetActive(mCurParam.mInfo.mCamp == EM_CardCamp.eCardShu);
        mCampTipLabels[2].SetActive(mCurParam.mInfo.mCamp == EM_CardCamp.eCardWu);
        mCampTipLabels[3].SetActive(mCurParam.mInfo.mCamp == EM_CardCamp.eCardGroupHero);
        mContriLabel.text = string.Format(CommonTools.f_GetTransLanguage(672), tollgateDt.iContri, tollgateDt.iContri + LegionConst.LEGION_DUNGEON_EXTRA_CONTRI);//数量
        mFiniExpLabel.text = string.Format(CommonTools.f_GetTransLanguage(673), tollgateDt.iFiniExp);//军团经验
        mKillContriLabel.text = string.Format(CommonTools.f_GetTransLanguage(674), tollgateDt.iKillContri);//伤害越高奖励越多,击杀者也可或者军团贡献
        int tExtraTimes = LegionMain.GetInstance().m_LegionDungeonPool.f_GetDungeonExtraTimes(ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime()));
        mChallengeTimesLabel.text = string.Format(CommonTools.f_GetTransLanguage(675), LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonLeftTimes + tExtraTimes);//挑战次数
        mShowItem.f_UpdateByInfo((int)EM_ResourceType.Money, (int)EM_UserAttr.eUserAttr_LegionContribution, 0);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessTheNextDay, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessTheNextDay, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    private void f_ProcessTheNextDay(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.LegionDungeonPool)
            return;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateChallengePage, UIMessageDef.UI_CLOSE);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateChallengePage, UIMessageDef.UI_CLOSE);
    }

    private void f_CloseBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateChallengePage, UIMessageDef.UI_CLOSE);
    }

    private void f_ChallengeBtn(GameObject go, object value1, object value2)
    {
        int tExtraTimes = LegionMain.GetInstance().m_LegionDungeonPool.f_GetDungeonExtraTimes(ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime()));
        int tLeftTimes = LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonLeftTimes + tExtraTimes;
        if (tLeftTimes <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(676));//挑战次数已用完
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_Challenge;
        socketCallbackDt.m_ccCallbackFail = f_Callback_Challenge;
        StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_LegionDungeon, mCurParam.mInfo.mChapterId, mCurParam.mInfo.mTollgateTemplate.iId, mCurParam.mInfo.mTollgateTemplate.iScene);
        LegionMain.GetInstance().m_LegionDungeonPool.f_Challenge((byte)mCurParam.mInfo.mCamp, mCurParam.mInfo.mTollgateTemplate.iScene, socketCallbackDt);
    }

    private void f_Callback_Challenge(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if (mCurParam != null && mCurParam.mCallback_ChallengeResult != null)
        {
            mCurParam.mCallback_ChallengeResult(result);
        }
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
            //失败就刷新切界面处理参数
            StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.None);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateChallengePage, UIMessageDef.UI_CLOSE);
    }

    private void f_LineupBtn(GameObject go, object value1, object value2)
    {
        //打开布阵
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }
}

public class LegionTollgateChallengePageParam
{
    public LegionTollgateChallengePageParam(LegionTollgatePoolDT info,ccCallback callbackHandle)
    {
        mInfo = info;
        mCallback_ChallengeResult = callbackHandle;
    }

    public LegionTollgatePoolDT mInfo
    {
        private set;
        get;
    }

    public ccCallback mCallback_ChallengeResult
    {
        private set;
        get;
    }
}
