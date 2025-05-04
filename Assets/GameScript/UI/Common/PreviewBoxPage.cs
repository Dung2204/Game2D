using UnityEngine;
using ccU3DEngine;
using System.Collections;

public class PreviewBoxPage : UIFramwork
{

    private PreviewBoxPageParam _tParam;

    private UILabel mTitle;
    private UILabel mCondition;
    private GameObject mLockBtn;
    private GameObject mGetBtn;
    private GameObject mAlreadyGetTip;
    private GameObject mCloseMask;
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

    private int _curBoxId;
    private int _curIdx;
    private EM_BoxGetState _curBoxState;

    private ccCallback _GetBoxCallback;

    protected  void Info()
    {
        mTitle = f_GetObject("Title").GetComponent<UILabel>();
        mCondition = f_GetObject("Condition").GetComponent<UILabel>();
        mLockBtn = f_GetObject("LockBtn");
        mAlreadyGetTip = f_GetObject("GetTip");
        mCloseMask = f_GetObject("CloseMask");
        awardItem = f_GetObject("ResourceCommonItem");
        awardGrid = f_GetObject("CommonItemGrid").GetComponent<UIGrid>();
        mGetBtn = f_GetObject("GetBtn");
        ccUIEventListener.Get(mGetBtn).onClickV2 = _GetBtn;
        ccUIEventListener.Get(mCloseMask).onClickV2 = _MaskClose;
    }



    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Info();
        if (e == null && !(e is PreviewBoxPageParam))
        {
            MessageBox.ASSERT("");
        }
        _tParam = (PreviewBoxPageParam)e;
        _GetBoxCallback = _tParam.m_GetBoxCallback;
        _curBoxId = _tParam.m_iBoxId;
        _curIdx = _tParam.m_iAward;
        
        _curBoxState = _tParam.m_AwardGetState;
        mCondition.text = _tParam.m_szDesc;
        mTitle.text = _tParam.m_szTitle;
        mLockBtn.SetActive(_curBoxState == EM_BoxGetState.Lock);
        mGetBtn.SetActive(_curBoxState == EM_BoxGetState.CanGet);
        mAlreadyGetTip.SetActive(_curBoxState == EM_BoxGetState.AlreadyGet);
        mAwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(_tParam.m_iAward), EM_CommonItemShowType.All, EM_CommonItemClickType.AllTip, this);
    }

    private void _GetBtn(GameObject go, object obj1, object obj2)
    {
        if (_GetBoxCallback != null)
            _GetBoxCallback(_tParam.m_iBoxId);
    }
    private void _MaskClose(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonBoxPage, UIMessageDef.UI_CLOSE);
    }
}

public class PreviewBoxPageParam
{

    public PreviewBoxPageParam(int iAwardid, int iBoxId, string szTitle, string szDesc, string szBtnName, EM_BoxGetState tAwardGetState, ccCallback CallbackSuc)
    {
        _iAwardId = iAwardid;
        _iBoxId = iBoxId;
        _szTitle = szTitle;
        _szBtnName = szBtnName;
        _szDesc = szDesc;
        m_GetBoxCallback = CallbackSuc;
        _AwardGetState = tAwardGetState;
    }

    private int _iAwardId;
    private int _iBoxId;
    private string _szTitle;
    private string _szBtnName;
    private string _szDesc;
    private EM_BoxGetState _AwardGetState;

    public int m_iAward
    {
        get { return _iAwardId; }
    }
    public int m_iBoxId
    {
        get { return _iBoxId; }
    }
    public string m_szTitle
    {
        get { return _szTitle; }
    }
    public string m_szBtnName
    {
        get { return _szBtnName; }
    }
    public string m_szDesc
    {
        get { return _szDesc; }
    }
    public EM_BoxGetState m_AwardGetState
    {
        get { return _AwardGetState; }
    }


    public ccCallback m_GetBoxCallback;
}
