using ccU3DEngine;
using UnityEngine;

/// <summary>
/// 副本宝箱领取界面
/// </summary>
public class BoxGetSubPage : UIFramwork
{
    private UILabel mTitle;
    private UILabel mCondition;
    private GameObject mLockBtn;
    private GameObject mGetBtn;
    private GameObject mAlreadyGetTip;
    private GameObject mCloseMask;

    private UISprite mTitleTxt;
    //奖励展示
    private GameObject awardItem;
    private UIGrid awardGrid;
    private ResourceCommonItemComponent _awardShowComponent;
    private ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(awardGrid, awardItem);
            return _awardShowComponent;
        }
    }

    //特定数据
    private UILabel mTollgateName;
    private UILabel mStarNum;

    private ccU3DEngine.ccCallback CallBack_ClickGet;

    private int _curBoxId;
    private int _curIdx;
    private EM_BoxGetState _curBoxState;
    private EM_BoxType _curBoxType;
    private string _curExtendInfo;
    private ccUIBase _needHoldUI;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mTitle = f_GetObject("Title").GetComponent<UILabel>();
        mCondition = f_GetObject("Condition").GetComponent<UILabel>();
        mLockBtn = f_GetObject("LockBtn");
        mAlreadyGetTip = f_GetObject("GetTip");
        mCloseMask = f_GetObject("CloseMask");
        awardItem = f_GetObject("ResourceCommonItem");
        awardGrid = f_GetObject("CommonItemGrid").GetComponent<UIGrid>();
        mTollgateName = f_GetObject("TollgateName").GetComponent<UILabel>();
        mStarNum = f_GetObject("StarNum").GetComponent<UILabel>();
        mGetBtn = f_GetObject("GetBtn");
        mTitleTxt = f_GetObject("TitleTxt").GetComponent<UISprite>();
        ccUIEventListener.Get(mGetBtn).onClickV2 = f_GetBtn;
        ccUIEventListener.Get(mCloseMask).onClickV2 = f_MaskClose;
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is BoxGetSubPageParam))
MessageBox.ASSERT("BoxGetSubPage must be passed to BoxGetSubPageParam");
        BoxGetSubPageParam tParam = (BoxGetSubPageParam)e;
        CallBack_ClickGet = tParam.mClickHandle;
        _curBoxId = tParam.mBoxId;
        _curIdx = tParam.mBoxIdx;
        _curBoxType = tParam.mBoxType;
        _curBoxState = tParam.mBoxState;
        _curExtendInfo = tParam.mExtradInfo;
        _needHoldUI = tParam.mNeedHoldUI;
        mLockBtn.SetActive(_curBoxState == EM_BoxGetState.Lock);
        mGetBtn.SetActive(_curBoxState == EM_BoxGetState.CanGet);
        mAlreadyGetTip.SetActive(_curBoxState == EM_BoxGetState.AlreadyGet);
        f_UpdateByType();
        mAwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(tParam.mBoxId),EM_CommonItemShowType.All,EM_CommonItemClickType.AllTip,this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        if (_needHoldUI != null)
        {
            ccUIHoldPool.GetInstance().f_Hold(_needHoldUI);
        }
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        if (_needHoldUI != null)
        {
            ccUIHoldPool.GetInstance().f_UnHold();
        }
    }

    private void f_MaskClose(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_CLOSE);
    }

    private void f_GetBtn(GameObject go,object obj1,object obj2)
    {
        if (CallBack_ClickGet != null)
        {
            CallBack_ClickGet(_curIdx);
        }
    }

    private void f_UpdateByType()
    {
        if (_curBoxType == EM_BoxType.Chapter)
        {
mTitle.text = "Sao chép phần thưởng";
mCondition.text = "Hoàn thành";
            mStarNum.text = _curExtendInfo;
            mTitleTxt.spriteName = "ptfb_title_fbjl";
        }
        else if (_curBoxType == EM_BoxType.Tollgate)
        {
mTitle.text = "Thưởng";
mCondition.text = "Phần thưởng khi vượt qua";
            mTollgateName.text = _curExtendInfo;
            mTitleTxt.spriteName = "ptfb_title_fbjl";

        }
        else if (_curBoxType == EM_BoxType.Task)
        {
mTitle.text = "Phần thưởng nhiệm vụ";
mCondition.text = "Hoàn thành";
            mTollgateName.text = _curExtendInfo;
            mTitleTxt.spriteName = "qrhd_font_bxyl";
        }
        else
        {
            mTitle.text = string.Empty;
            mCondition.text = string.Empty;
            mTitleTxt.spriteName = "ptfb_title_fbjl";

        }
        mTollgateName.gameObject.SetActive(_curBoxType == EM_BoxType.Tollgate || _curBoxType == EM_BoxType.Task);
        mStarNum.gameObject.SetActive(_curBoxType == EM_BoxType.Chapter);
    }
}

public class BoxGetSubPageParam
{
    public BoxGetSubPageParam(int boxId, int idx, string extraInfo, EM_BoxType boxType, EM_BoxGetState boxState, ccU3DEngine.ccCallback clickHandle,ccUIBase needHoldUI = null)
    {
        mBoxId = boxId;
        mBoxIdx = idx;
        mExtradInfo = extraInfo;
        mBoxType = boxType;
        mBoxState = boxState;
        mClickHandle = clickHandle;
        mNeedHoldUI = needHoldUI;
    }

    public int mBoxId
    {
        get;
        private set;
    }

    public int mBoxIdx
    {
        get;
        private set;
    }

    public string mExtradInfo
    {
        get;
        private set;
    }

    public EM_BoxType mBoxType
    {
        get;
        private set;
    }

    public EM_BoxGetState mBoxState
    {
        get;
        private set;
    }

    public ccU3DEngine.ccCallback mClickHandle
    {
        get;
        private set;
    }

    public ccUIBase mNeedHoldUI
    {
        get;
        private set;
    }
}
