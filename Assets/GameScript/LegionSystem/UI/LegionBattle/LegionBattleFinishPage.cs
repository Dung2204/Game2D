using ccU3DEngine;
using UnityEngine;

public class LegionBattleFinishPage : UIFramwork
{
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
    private Transform[] mStar;

    private int timeEvent_GoodShow;
    private int[] timeEvent_StarShow;
    private int[] timeEvent_StarMusic;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }
    protected override void InitGUI()
    {
        base.InitGUI();
        m_WinObj = f_GetObject("WinObj");
        m_AwardGrid = f_GetObject("AwardGrid").GetComponent<UIGrid>();
        m_AwardItem = f_GetObject("IconAndNumItem");
        m_LoseObj = f_GetObject("LoseObj");
        m_WinTitleParent = f_GetObject("WinTitleParent").transform;
        mStar = new Transform[3];
        timeEvent_StarShow = new int[3];
        timeEvent_StarMusic = new int[3];
        for (int i = 0; i < 3; i++)
        {
            mStar[i] = f_GetObject(string.Format("Star{0}", i)).transform;
        }
        f_RegClickEvent("CloseMask", f_OnCloseMaskClick);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        int ret = LegionMain.GetInstance().m_LegionBattlePool.m_iChallengeRet;
        m_WinObj.SetActive(ret > 0);
        m_LoseObj.SetActive(ret <= 0);
        if (ret > 0)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleVictory, false);
            UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinTitleEffect, m_WinTitleParent);
        }
        else
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleFail, false);
        }
        int tDelayIdx = 1;
        for (int i = 0; i < ret; i++)
        {
            timeEvent_StarShow[i] = ccTimeEvent.GetInstance().f_RegEvent(0.2f * (tDelayIdx++), false, mStar[i], f_ShowStarEffect);
            timeEvent_StarMusic[i] = ccTimeEvent.GetInstance().f_RegEvent(0.2f * (tDelayIdx++), false, AudioEffectType.Victory1 + i, f_PlayAudio);
        }
        int tAwardId = LegionMain.GetInstance().m_LegionBattlePool.f_GetAwardByStar(ret);
        m_AwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(tAwardId));
        timeEvent_GoodShow = ccTimeEvent.GetInstance().f_RegEvent(0.2f* (tDelayIdx++), false, null, f_ShowGoodEffect);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_OnCloseMaskClick(GameObject go,object value1,object value2)
    {
        for (int i = 0; i < timeEvent_StarShow.Length; i++)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(timeEvent_StarShow[i]);
            ccTimeEvent.GetInstance().f_UnRegEvent(timeEvent_StarMusic[i]);
        }
        ccTimeEvent.GetInstance().f_UnRegEvent(timeEvent_GoodShow);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleFinishPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }

    private void f_PlayAudio(object obj)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect((AudioEffectType)obj);
    }

    private void f_ShowStarEffect(object value)
    {
        Transform tf = (Transform)value;
        UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinStarEffect, tf);
    }

    private void f_ShowGoodEffect(object value)
    {
        m_AwardShowComponent.f_ShowEffect();
    }

}
