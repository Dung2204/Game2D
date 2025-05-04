using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaFinishPage : UIFramwork
{
    private GameObject mMaskClose;
    private GameObject _awardItem;

    private GameObject mWinObj;
    private UILabel mNameLabel;
    private UILabel mRankOld;
    private UILabel mRankCur;
    private UITable mRankTable;
    private GameObject mUpTip;
    private UIGrid _winAwardGrid;
    private ResourceCommonItemComponent _winAwardShowComponent;
    private ResourceCommonItemComponent mWinAwardShowComponent
    {
        get
        {
            if (_winAwardShowComponent == null)
                _winAwardShowComponent = new ResourceCommonItemComponent(_winAwardGrid, _awardItem);
            return _winAwardShowComponent;
        }
    }
    private UILabel mWinLvLabel;
    private UISlider mWinLvSlider;


    private GameObject mLoseObj;
    private UIGrid _loseAwardGrid;
    private ResourceCommonItemComponent _loseAwardShowComponent;
    private ResourceCommonItemComponent mLoseAwardShowComponent
    {
        get
        {
            if (_loseAwardShowComponent == null)
                _loseAwardShowComponent = new ResourceCommonItemComponent(_loseAwardGrid, _awardItem);
            return _loseAwardShowComponent;
        }
    }
    private UILabel mLoseLvLabel;
    private UISlider mLoseLvSlider;


    private CMsg_SC_ArenaRet mCurResult;
    
    //特效
    private Transform mWinTitleParent;
    private Transform mExpEffectParent;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mMaskClose = f_GetObject("MaskClose");
        _awardItem = f_GetObject("LabelAwardItem");
        mWinObj = f_GetObject("WinObj");
        mNameLabel = f_GetObject("NameLabel").GetComponent<UILabel>();
        mRankOld = f_GetObject("RankOld").GetComponent<UILabel>();
        mRankCur = f_GetObject("RankCur").GetComponent<UILabel>();
        mRankTable = f_GetObject("RankTable").GetComponent<UITable>();
        mUpTip = f_GetObject("UpTip");
        _winAwardGrid = f_GetObject("WinAwardGrid").GetComponent<UIGrid>();
        mWinLvLabel = f_GetObject("WinLvLabel").GetComponent<UILabel>();
        mWinLvSlider = f_GetObject("WinLvSlider").GetComponent<UISlider>();

        mLoseObj = f_GetObject("LoseObj");
        _loseAwardGrid = f_GetObject("LoseAwardGrid").GetComponent<UIGrid>();
        mLoseLvLabel = f_GetObject("LoseLvLabel").GetComponent<UILabel>();
        mLoseLvSlider = f_GetObject("LoseLvSlider").GetComponent<UISlider>();

        mWinTitleParent = f_GetObject("WinTitleParent").transform;
        mExpEffectParent = f_GetObject("ExpEffectParent").transform;

        f_RegClickEvent(mMaskClose, f_MaskClose);
        f_RegClickEvent("BtnGetMoreCard", f_BattleLoseProcess, EM_BattleLoseProcess.GetMoreCard);
        f_RegClickEvent("BtnCardIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.CardIntensify);
        f_RegClickEvent("BtnEquipIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.EquipIntensify);
        f_RegClickEvent("BtnLineupChange", f_BattleLoseProcess, EM_BattleLoseProcess.LineupChange);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mCurResult = Data_Pool.m_ArenaPool.m_ArenaRet;
        mWinObj.SetActive(mCurResult.isWin != (int)EM_ArenaResult.Lose);
        mLoseObj.SetActive(mCurResult.isWin == (int)EM_ArenaResult.Lose);
        
        if (mCurResult.isWin != (int)EM_ArenaResult.Lose)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleVictory,false);
mNameLabel.text = string.Format("Xin chúc mừng vì đã đánh bại [3688D4FF]{0}[-]", mCurResult.szRivalName);
            mRankTable.gameObject.SetActive(mCurResult.oldRank != mCurResult.curRank);
            if (mCurResult.oldRank != mCurResult.curRank)
            {
                mRankOld.text = mCurResult.oldRank.ToString();
                mRankCur.text = mCurResult.curRank.ToString();
                mUpTip.SetActive(mCurResult.oldRank - mCurResult.curRank > 0);
                mRankTable.repositionNow = true;
            }
            mWinAwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetArenaAwardByResult(mCurResult.isWin));
mWinLvLabel.text = string.Format("Cấp {0}",Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level));
            mWinLvSlider.value = Data_Pool.m_UserData.mExpPercent;
            
            UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinTitleEffect, mWinTitleParent);
            ccTimeEvent.GetInstance().f_RegEvent(0.2f, false, mExpEffectParent, f_ShowExpEffect);
        }
        else
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleFail);
            mLoseAwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetArenaAwardByResult(mCurResult.isWin));
mLoseLvLabel.text = string.Format("Cấp {0}", Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level));
            mLoseLvSlider.value = Data_Pool.m_UserData.mExpPercent;
        }

    }

    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        if (mCurResult.isWin != (int)EM_ArenaResult.Lose)
        {
            //ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaFinishPage, UIMessageDef.UI_CLOSE);
            //ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaChooseAwardPage, UIMessageDef.UI_OPEN);
            Data_Pool.m_ArenaPool.f_ArenaChooseAward((byte)1, f_ShowAwardCallback);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
        }
    }

    private void f_BattleLoseProcess(GameObject go, object value1, object value2)
    {
        UITool.f_ProcessBattleLose((EM_BattleLoseProcess)value1);
    }

    /// <summary>
    /// 显示抽牌界面
    /// </summary>
    /// <param name="result"></param>
    private void f_ShowAwardCallback(object result)
    {
        CMsg_SC_ArenaChooseAward ret = (CMsg_SC_ArenaChooseAward)result;
        ChooseAwardParam param = new ChooseAwardParam();
        List<ResourceCommonDT> listCommonData = new List<ResourceCommonDT>();
        for (int i = 0; i < ret.awards.Length; i++)
        {
            ResourceCommonDT commonDT = new ResourceCommonDT();
            SC_Award scAward = ret.awards[i];
            commonDT.f_UpdateInfo(scAward.resourceType, scAward.resourceId, scAward.resourceNum);
            listCommonData.Add(commonDT);
        }
        param.mListData = listCommonData;
        param.mChooseIndex = ret.idx;
        param.mOnReturnCallback = OnChooseAwardCallback;
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChooseAwardPage, UIMessageDef.UI_OPEN, param);
    }

    /// <summary>
    /// 抽牌回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnChooseAwardCallback(object obj)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }
    private void f_ShowExpEffect(object value)
    {
        Transform tf = (Transform)value;
        UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinExpEffect, tf, 0f, 0f, false, 1f, 1.5f);
    }

}
