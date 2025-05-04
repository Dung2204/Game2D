using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class RunningManEliteFinishPage : UIFramwork
{
    private GameObject mWinObj;
    private UILabel mWinDesc;

    private GameObject mLoseObj;
    private UILabel mLoseAwardLabel;
    private UILabel mLoseDesc;

    private Transform mWinTitleParent;

    private UIGrid _awardGrid;
    private GameObject _awardItem;
    private ResourceCommonItemComponent _awardShowComponent;

    private int Time_ExpTween;
    private ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(_awardGrid, _awardItem);
            return _awardShowComponent;
        }
    }

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mWinObj = f_GetObject("WinObj");
        mWinDesc = f_GetObject("WinDesc").GetComponent<UILabel>();
        _awardGrid = f_GetObject("AwardGrid").GetComponent<UIGrid>();
        _awardItem = f_GetObject("RMEliteAwardItem");
        mLoseObj = f_GetObject("LoseObj");
        //mLoseAwardLabel = f_GetObject("LoseAwardLabel").GetComponent<UILabel>();
        mLoseDesc = f_GetObject("LoseDesc").GetComponent<UILabel>();
        f_RegClickEvent("MaskClose", f_MaskClose);
        mWinTitleParent = f_GetObject("WinTitleParent").transform;
        f_RegClickEvent("BtnGetMoreCard", f_BattleLoseProcess, EM_BattleLoseProcess.GetMoreCard);
        f_RegClickEvent("BtnCardIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.CardIntensify);
        f_RegClickEvent("BtnEquipIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.EquipIntensify);
        f_RegClickEvent("BtnLineupChange", f_BattleLoseProcess, EM_BattleLoseProcess.LineupChange);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        CMsg_SC_RunningManEliteRet tRet = Data_Pool.m_RunningManPool.m_EliteChallengeFinishRet;
        mWinObj.SetActive(tRet.isWin > 0);
        mLoseObj.SetActive(tRet.isWin == 0);
        
        if (tRet.isWin > 0)
        {
            RunningManElitePoolDT tPoolDt = Data_Pool.m_RunningManPool.f_GetEliteData((long)tRet.uTollgate);
            mWinDesc.text = string.Empty;
            List<AwardPoolDT> tList = Data_Pool.m_AwardPool.f_GetAwardByString(tPoolDt.m_Template.szAward);
            if (tRet.isFirst > 0)
            {
mWinDesc.text = "First pass";
                AwardPoolDT tAwardPoolDt = new AwardPoolDT();
                tAwardPoolDt.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Prestige, tPoolDt.m_Template.iAwardFirst);
                tList.Insert(0,tAwardPoolDt);
            }
            mAwardShowComponent.f_Show(tList);
            UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinTitleEffect, mWinTitleParent);
        }
        else
        {
            //mLoseAwardLabel.text = string.Empty;
            mLoseDesc.text = string.Empty;
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManEliteFinishPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }

    private void f_BattleLoseProcess(GameObject go, object value1, object value2)
    {
        UITool.f_ProcessBattleLose((EM_BattleLoseProcess)value1);
    }
}
