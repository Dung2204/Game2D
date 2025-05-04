using ccU3DEngine;
using UnityEngine;
using Spine.Unity;

public class TollgateSelectItem : MonoBehaviour
{
    public GameObject mItem;
    public UILabel mTitleLabel;
	public UI2DSprite m_spriteIcon;
    public UISprite[] mStar;
    public GameObject mBoxItemParent;
    public GameObject mBoxItem;
    public GameObject mNewTip;
    public GameObject mLegendParent;
    public UILabel mlabelHadChallenged;
    public GameObject mOtherParent;
    public GameObject mModelParent;
    private GameObject _role;
    private GameObject _legendBossMagic = null;
    public GameObject mobjModelRDialog;
    public UILabel mtxtModeRDialog;
    public GameObject mobjModelLDialog;
    public UILabel mtxtModeLDialog;
    public TweenPosition modelTweenPosition;
    public UIPanel uiParentPanel;

    private BoxCommonItem _boxGetItem;
    private BoxCommonItem mBoxGetItem
    {
        get
        {
            if (_boxGetItem == null)
            {
                _boxGetItem = BoxCommonItem.f_Create(mBoxItemParent, mBoxItem);
            }
            return _boxGetItem;
        }
    }

    private DungeonTollgatePoolDT _tollgateData;
    private bool _lockState = true;
    private ccU3DEngine.ccCallback _btnHandle;
    private ccUIBase _parentUI;

    public void f_Init(int chapterType, int chapterIdx, int tollgatePassNum, int selectIdx, 
        DungeonTollgatePoolDT tollgateDT, ccU3DEngine.ccCallback btnHandle, ccUIBase parentUI,Vector3 pos,bool isLastCheckpoint)
    {
        _lockState = tollgateDT.mIndex > tollgatePassNum;
        if (_lockState)
        {
            mItem.SetActive(!_lockState);
            return;
        }
        transform.localPosition = pos;
        _tollgateData = tollgateDT;
        _btnHandle = btnHandle;
        _parentUI = parentUI;        
        if (!mItem.activeSelf)
            mItem.SetActive(true);
        mNewTip.SetActive(tollgateDT.mIndex == tollgatePassNum);
        string titleColor = "[FCC83EFF]";
        uiParentPanel.depth = 300 + tollgateDT.mIndex;
		 m_spriteIcon.sprite2D = UITool.f_GetCardIcon(tollgateDT.mTollgateTemplate.iModeId, "L4_");

        //设置星星
        bool isLegend = tollgateDT.mChapterType == (int)EM_Fight_Enum.eFight_Legend;
        for (int i = 0; i < mStar.Length; i++)
        {
            mStar[i].gameObject.SetActive(!isLegend);
        }
        if (!isLegend)
        {
            UITool.f_UpdateStarNum(mStar, tollgateDT.mStarNum);
            if (isLastCheckpoint)
            {
                titleColor = "[9932CD]";
            }
            else if (_tollgateData.mTollgateTemplate.iBoxId > 0)
            {
                titleColor = "[1351FF]";
            }
            else
            {
                titleColor = "[FCF7E5]";
            }
        }

        //名将已挑战
        mLegendParent.SetActive(isLegend && tollgateDT.mTimes > 0);
        if (_tollgateData.mIndex >= GameParamConst.LegendDungeonRestIdx)
        {
            //名将魔神关
            mlabelHadChallenged.text = "[dd3939]" + CommonTools.f_GetTransLanguage(1057) +"[-]";
        }
        else
        {
            //名将普通关
            mlabelHadChallenged.text = CommonTools.f_GetTransLanguage(1055);
        }
        mOtherParent.SetActive(!isLegend);
        mTitleLabel.text = titleColor + string.Format("{0}-{1} {2}", chapterIdx + 1, _tollgateData.mIndex + 1, _tollgateData.mTollgateTemplate.szTollgateName);
		if(isLegend)
		{
			mTitleLabel.text = titleColor + string.Format("{0}-{1}\n{2}", chapterIdx + 1, _tollgateData.mIndex + 1, _tollgateData.mTollgateTemplate.szTollgateName);
		}

        //更新对话框
        bool isShowDiaLog = tollgateDT.mTollgateTemplate.szModeDialog != "" && tollgateDT.mIndex == tollgatePassNum;
        mobjModelRDialog.SetActive(isShowDiaLog && tollgateDT.mIndex == 0);
        mtxtModeRDialog.text = tollgateDT.mTollgateTemplate.szModeDialog;
        mobjModelLDialog.SetActive(isShowDiaLog && tollgateDT.mIndex > 0);
        mtxtModeLDialog.text = tollgateDT.mTollgateTemplate.szModeDialog;

        //更新模型      
        UITool.f_CreateRoleByModeId(tollgateDT.mTollgateTemplate.iModeId, ref _role, mModelParent.transform, 1, false);
        if (null != _role && isLegend && _tollgateData.mIndex >= GameParamConst.LegendDungeonRestIdx)
        {
            UITool.f_CreateMagicById((int)EM_MagicId.eLegeongBoss, ref _legendBossMagic, _role.transform, 0, null);
            if (null != _legendBossMagic)
            {
                SkeletonAnimation skeletonAnimation = _legendBossMagic.GetComponent<SkeletonAnimation>();
                skeletonAnimation.state.SetAnimation(0, "animation", true);
            }
        }

        //模型飞入动画
        int tollgateId = PlayerPrefs.GetInt(StaticValue.LastChallengeDungeonIdKey + tollgateDT.mChapterType, 0) + 1;
        if (tollgateId == tollgateDT.mTollgateId)
        {
            modelTweenPosition.ResetToBeginning();
            modelTweenPosition.PlayForward();
            PlayerPrefs.SetInt(StaticValue.LastChallengeDungeonIdKey + tollgateDT.mChapterType, -1);
        }

        ccUIEventListener.Get(gameObject).onClickV2 = ClickHandle;
        if (_tollgateData.mTollgateTemplate.iBoxId > 0)
        {
            mBoxGetItem.f_UpdateClickHandle(f_TaskBoxClickHandle, tollgateDT.mTollgateTemplate.iBoxId);
            mBoxGetItem.f_UpdateInfo(EM_BoxType.Tollgate, _tollgateData.f_GetBoxState(), string.Empty);
        }
        else
        {
            mBoxGetItem.f_UpdateInfo(EM_BoxType.Tollgate, EM_BoxGetState.Invalid, string.Empty);
        }
    }

    public void f_Disable()
    {
        mItem.SetActive(false);
    }

    private void ClickHandle(GameObject go, object obj1, object obj2)
    {
        if (_lockState)
        {
UITool.Ui_Trip("Unopened");
            return;
        }
        if (_btnHandle != null && _tollgateData != null)
        {
            _btnHandle(_tollgateData);
        }
    }

    /// <summary>
    /// 宝箱点击
    /// </summary>
    private void f_TaskBoxClickHandle(object value)
    {
        if (_tollgateData == null)
            return;
        Data_Pool.m_GuidancePool.Condition = _tollgateData.mTollgateTemplate.iBoxId;
        BoxGetSubPageParam tParam = new BoxGetSubPageParam(_tollgateData.mTollgateTemplate.iBoxId, _tollgateData.mIndex, _tollgateData.mTollgateTemplate.szTollgateName, EM_BoxType.Tollgate, _tollgateData.f_GetBoxState(), f_TaskBoxGetHandle, _parentUI);
        //打开宝箱显示界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_TaskBoxGetHandle(object value)
    {
        if (_tollgateData == null)
            return;

        EM_BoxGetState tBoxState = _tollgateData.f_GetBoxState();
        if (tBoxState == EM_BoxGetState.CanGet)
        {
            SocketCallbackDT callbackDT = new SocketCallbackDT();
            callbackDT.m_ccCallbackSuc = f_GetTollgateBoxAwardSuc;
            callbackDT.m_ccCallbackFail = f_GetTollgateBoxAwardFail;
            //发送关卡宝箱协议
            Data_Pool.m_DungeonPool.f_DungeonTollgateBox(_tollgateData.mTollgateId, callbackDT);
        }
    }

    private void f_GetTollgateBoxAwardSuc(object value)
    {
        MessageBox.DEBUG("GetTollgateBoxAwardSuc");
        Data_Pool.m_GuidancePool.m_OtherSave = true;
        //关闭宝箱领取界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_CLOSE);
        //展示获得奖励
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(_tollgateData.mTollgateTemplate.iBoxId), _parentUI });
        //更新宝箱状态
        mBoxGetItem.f_UpdateInfo(EM_BoxType.Tollgate, _tollgateData.f_GetBoxState(), string.Empty);
        Data_Pool.m_DungeonPool.f_ResetDungeonGetBoxCountData(_tollgateData.mTollgateId);
    }

    private void f_GetTollgateBoxAwardFail(object value)
    {
UITool.UI_ShowFailContent("Get often failed，Fail Code:" + value);
        MessageBox.ASSERT("GetTollgateBoxAwardFail. Fail Code:" + value);
        //关闭宝箱领取界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_CLOSE);
        //更新宝箱状态
        mBoxGetItem.f_UpdateInfo(EM_BoxType.Tollgate, _tollgateData.f_GetBoxState(), string.Empty);
    }
}
