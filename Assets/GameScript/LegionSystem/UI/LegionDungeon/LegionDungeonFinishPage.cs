using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class LegionDungeonFinishPage : UIFramwork
{
    private Transform mEffectParent;
    private UILabel mContributionLabel;
    private UILabel mDamageLabel;
    private UILabel mLegionExpLabel;
    private UIGrid mLabelGrid;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mContributionLabel = f_GetObject("ContributionLabel").GetComponent<UILabel>();
        mDamageLabel = f_GetObject("DamageLabel").GetComponent<UILabel>();
        mLegionExpLabel = f_GetObject("LegionExpLabel").GetComponent<UILabel>();
        mLabelGrid = f_GetObject("LabelGrid").GetComponent<UIGrid>();
        mEffectParent = f_GetObject("EffectParent").transform;
        f_RegClickEvent("BackBtn", f_BackBtn);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleVictory,false);
        CMsg_GTC_LegionDungeonChallengeRet ret = LegionMain.GetInstance().m_LegionDungeonPool.m_sChallengeRet;
        UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.RebelArmyWinTitleEffect, mEffectParent);
mContributionLabel.text = string.Format("[F5BF3D]Dedication Points： [-]{0}", ret.uContri);
mDamageLabel.text = string.Format("[F5BF3D]Damage: [-]{0}", ret.uDmg);
        mLegionExpLabel.gameObject.SetActive(ret.uExp != 0);
        if (ret.uExp != 0)
mLegionExpLabel.text = string.Format("[F5BF3D]EXP Corps： [-]{0}", ret.uExp);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_BackBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonFinishPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }

}
