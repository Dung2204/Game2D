using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class RebelArmyFinish : UIFramwork
{
    SC_RebelArmyFinish tRebelArmt;
    
    private Transform mLoseHead;
    private Transform mEffectParent;
    private GameObject tipwin;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        mLoseHead = f_GetObject("LoseHead").transform;
        mEffectParent = f_GetObject("EffectParent").transform;
        tipwin = f_GetObject("TipWin");
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Bg", UI_Close);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        tRebelArmt = Data_Pool.m_RebelArmyPool.mRebelArmyFinish;     
        ulong lDmg = tRebelArmt.uDmg;//tRebelArmt.uDmg.tostring()神奇地在真机会异常！！！
f_GetObject("DPSNum").GetComponent<UILabel>().text = lDmg >= 10000 ? lDmg / 10000 + "0K" : lDmg.ToString();
        uint uExploit = tRebelArmt.uExploit;//避免像tRebelArmt.uDmg.ToString()一样地异常！！
        f_GetObject("ExploitNum").GetComponent<UILabel>().text = uExploit.ToString();
        int uBattleFeat = tRebelArmt.uBattleFeat;//避免像tRebelArmt.uDmg.ToString()一样地异常！！
        f_GetObject("BattleFeatNum").GetComponent<UILabel>().text = uBattleFeat.ToString();
        if (tRebelArmt.isWin > 0)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleVictory,false);
            mLoseHead.gameObject.SetActive(false);
            tipwin.gameObject.SetActive(true);
            //UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.RebelArmyWinTitleEffect, mEffectParent);
            UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinTitleEffect, mEffectParent);
        }
        else
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleFail);
            mLoseHead.gameObject.SetActive(true);
            tipwin.gameObject.SetActive(false);

        }

    }

    void UI_Close(GameObject go,object obj1,object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelArmyFinish, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }
}
