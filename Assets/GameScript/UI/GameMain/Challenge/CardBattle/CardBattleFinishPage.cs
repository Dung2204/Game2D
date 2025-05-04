using ccU3DEngine;
using UnityEngine;

public class CardBattleFinishPage : UIFramwork
{
    private GameObject mMaskClose;

    private GameObject m_WinObj;
    private GameObject m_LoseObj;
    private UILabel m_WinAward;
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
        m_LoseObj = f_GetObject("LoseObj");
        m_WinAward = f_GetObject("WinAward").GetComponent<UILabel>();
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
        bool isWin = Data_Pool.m_CardBattlePool.Result > 0;
        m_WinObj.SetActive(isWin);
        m_LoseObj.SetActive(!isWin);
m_WinAward.text = "Challenge all 10 matches, go to the Rewards Center to receive rewards.";
        if (isWin)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleVictory, false);
            UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinTitleEffect, m_WinTitleParent);
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattleFinishPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }

    private void f_BattleLoseProcess(GameObject go, object value1, object value2)
    {
        UITool.f_ProcessBattleLose((EM_BattleLoseProcess)value1);
    }

}
