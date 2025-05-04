using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class RunningManBattleFinishPage : UIFramwork
{
    
    private GameObject mWinObj;
    private UILabel mWinMoneyLabel;
    private UILabel mWinPrestigeLabel;
    private UILabel mWinDesc;

    private GameObject mLoseObj;
    private UILabel mLoseMoneyLabel;
    private UILabel mLosePrestigeLabel;
    private UILabel mLoseDesc;

    private Transform mWinTitleParent;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mWinObj = f_GetObject("WinObj");
        mWinMoneyLabel = f_GetObject("WinMoneyLabel").GetComponent<UILabel>();
        mWinPrestigeLabel = f_GetObject("WinPrestigeLabel").GetComponent<UILabel>();
        mWinDesc = f_GetObject("WinDesc").GetComponent<UILabel>();
        mLoseObj = f_GetObject("LoseObj");
        mLoseMoneyLabel = f_GetObject("LoseMoneyLabel").GetComponent<UILabel>();
        mLosePrestigeLabel = f_GetObject("LosePrestigeLabel").GetComponent<UILabel>();
        mLoseDesc = f_GetObject("LoseDesc").GetComponent<UILabel>();
        f_RegClickEvent("MaskClose",f_MaskClose);
        mWinTitleParent = f_GetObject("WinTitleParent").transform;
        f_RegClickEvent("BtnGetMoreCard", f_BattleLoseProcess, EM_BattleLoseProcess.GetMoreCard);
        f_RegClickEvent("BtnCardIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.CardIntensify);
        f_RegClickEvent("BtnEquipIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.EquipIntensify);
        f_RegClickEvent("BtnLineupChange", f_BattleLoseProcess, EM_BattleLoseProcess.LineupChange);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        CMsg_SC_RunningManRet tRet = Data_Pool.m_RunningManPool.m_ChallengeFinishRet;
        RunningManTollgatePoolDT tPoolDt = ((RunningManPoolDT)Data_Pool.m_RunningManPool.f_GetForId(tRet.uChap)).m_TollgatePoolDTs[tRet.idx-1]; 
        bool tIsWin = tRet.iRet > 0;
        int tMoney = 0;
        int tPrestige = 0;
        mWinObj.SetActive(tIsWin);
        mLoseObj.SetActive(!tIsWin);
        if (tIsWin)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleVictory,false);
            int[] tMoneyArr = ccMath.f_String2ArrayInt(tPoolDt.m_TollgateTemplate.szMoneys, ";");
            int[] tPrestigeArr = ccMath.f_String2ArrayInt(tPoolDt.m_TollgateTemplate.szPrests, ";");
            tMoney = tMoneyArr[tRet.iRet - 1] * tRet.uAwardMoneyRate / 100;
            tPrestige = tPrestigeArr[tRet.iRet - 1] * tRet.uAwardPrestRate / 100;
mWinMoneyLabel.text = string.Format("Bạc：{0}{1}", tMoney,Data_Pool.m_RunningManPool.f_GetCritDesc(tRet.uAwardMoneyRate));
            mWinPrestigeLabel.text = string.Format("Uy danh：{0}{1}", tPrestige, Data_Pool.m_RunningManPool.f_GetCritDesc(tRet.uAwardPrestRate));
            
            int[] passArr = ccMath.f_String2ArrayInt(tPoolDt.m_TollgateTemplate.szPassParams,";");
            string passCondition = string.Format(Data_Pool.m_RunningManPool.f_GetPassTypeDesc(tPoolDt.m_TollgateTemplate.iPassType),passArr[tRet.iRet - 1]);
mWinDesc.text = string.Format("Xin chúc mừng vị tướng đã hoàn thành nhiệm vụ vượt qua:{0}，vượt qua thành công！", passCondition);
            UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinTitleEffect, mWinTitleParent);
        }
        else
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleFail);
mLoseMoneyLabel.text = string.Format("Bạc：{0}", tMoney);
            mLosePrestigeLabel.text = string.Format("Uy danh：{0}",tPrestige);
            int[] passArr = ccMath.f_String2ArrayInt(tPoolDt.m_TollgateTemplate.szPassParams, ";");
            string passCondition = string.Format(Data_Pool.m_RunningManPool.f_GetPassTypeDesc(tPoolDt.m_TollgateTemplate.iPassType), passArr[Mathf.Abs(tRet.iRet) - 1]);
mLoseDesc.text = string.Format("Không đáp ứng được điều kiện đạt: {0}, không đạt!", passCondition);
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManBattleFinishPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }

    private void f_BattleLoseProcess(GameObject go, object value1, object value2)
    {
        UITool.f_ProcessBattleLose((EM_BattleLoseProcess)value1);
    }
}
