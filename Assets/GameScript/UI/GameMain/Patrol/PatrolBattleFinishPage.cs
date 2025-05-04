using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class PatrolBattleFinishPage : UIFramwork
{
    private GameObject mMaskClose;

    private GameObject m_WinObj;
    private UIGrid m_AwardGrid;
    private GameObject m_AwardItem;
    private ResourceCommonItemComponent _awardShowComponent;
    private ResourceCommonItemComponent m_AwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(m_AwardGrid, m_AwardItem);
            return _awardShowComponent;
        }
    }

    private GameObject m_LoseObj;

    //特效
    private Transform m_WinTitleParent;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_WinObj = f_GetObject("WinObj");
        m_AwardGrid = f_GetObject("WinAwardGrid").GetComponent<UIGrid>();
        m_AwardItem = f_GetObject("IconAndNumItem");
        m_LoseObj = f_GetObject("LoseObj");
        m_WinTitleParent = f_GetObject("WinTitleParent").transform;
        f_RegClickEvent("CloseMask", f_CloseMask);
        f_RegClickEvent("BtnGetMoreCard", f_BattleLoseProcess, EM_BattleLoseProcess.GetMoreCard);
        f_RegClickEvent("BtnCardIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.CardIntensify);
        f_RegClickEvent("BtnEquipIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.EquipIntensify);
        f_RegClickEvent("BtnLineupChange", f_BattleLoseProcess, EM_BattleLoseProcess.LineupChange);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        bool isWin = Data_Pool.m_PatrolPool.m_iIsWin > 0;
        m_WinObj.SetActive(isWin);
        m_LoseObj.SetActive(!isWin);
        if (isWin)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleVictory, false);
            UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinTitleEffect, m_WinTitleParent);
            PatrolLandDT landTemplate = (PatrolLandDT)glo_Main.GetInstance().m_SC_Pool.m_PatrolLandSC.f_GetSC(StaticValue.m_CurBattleConfig.m_iTollgateId);
            m_AwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetAwardByString(landTemplate.szPassAward));
        }
        else
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleFail, false);
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_CloseMask(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolBattleFinishPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }

    private void f_BattleLoseProcess(GameObject go, object value1, object value2)
    {
        UITool.f_ProcessBattleLose((EM_BattleLoseProcess)value1);
    }
}
