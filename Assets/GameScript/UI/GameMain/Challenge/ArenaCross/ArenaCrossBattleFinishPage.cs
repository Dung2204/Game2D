using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class ArenaCrossBattleFinishPage : UIFramwork
{
    private GameObject mMaskClose;

    private GameObject m_WinObj;
    private GameObject m_LoseObj;
    private UILabel m_WinScore;
    private UILabel m_LoseScore;
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
        m_WinScore = f_GetObject("WinScore").GetComponent<UILabel>();
        m_LoseScore = f_GetObject("LoseScore").GetComponent<UILabel>();
        m_WinTitleParent = f_GetObject("WinTitleParent").transform;
        f_RegClickEvent("CloseMask", f_CloseMask);
        f_RegClickEvent("BtnGetMoreCard", f_BattleLoseProcess, EM_BattleLoseProcess.GetMoreCard);
        f_RegClickEvent("BtnCardIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.CardIntensify);
        f_RegClickEvent("BtnEquipIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.EquipIntensify);
        f_RegClickEvent("BtnLineupChange", f_BattleLoseProcess, EM_BattleLoseProcess.LineupChange);
    }

    protected override void f_Create()
    {
        _InitReference();
        base.f_Create();
    }
    private void _InitReference()
    {
        AddGOReference("Panel/Anchor-Center/CloseMask");
        AddGOReference("Panel/Anchor-Center/WinObj");
        AddGOReference("Panel/Anchor-Center/LoseObj");
        AddGOReference("Panel/Anchor-Center/WinObj/WinScore");
        AddGOReference("Panel/Anchor-Center/LoseObj/LoseScore");
        AddGOReference("Panel/Anchor-Center/WinObj/WinTitleParent");
        AddGOReference("Panel/Anchor-Center/BG/BecomePower/BtnGrid/BtnGetMoreCard");
        AddGOReference("Panel/Anchor-Center/BG/BecomePower/BtnGrid/BtnCardIntensify");
        AddGOReference("Panel/Anchor-Center/BG/BecomePower/BtnGrid/BtnEquipIntensify");
        AddGOReference("Panel/Anchor-Center/BG/BecomePower/BtnGrid/BtnLineupChange");
        AddGOReference("Panel/Anchor-Center/VS");
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        bool isWin = Data_Pool.m_CrossArenaPool.BattleResult.iResult > 0;
        m_WinObj.SetActive(isWin);
        m_LoseObj.SetActive(!isWin);
        CMsg_ArenaCrossInfo enemy = Data_Pool.m_CrossArenaPool.EnemyInfo;
        m_WinScore.text = string.Format("Điểm: +{0}",Data_Pool.m_CrossArenaPool.BattleResult.iResultScore);
        m_LoseScore.text = string.Format("Điểm: +{0}", Data_Pool.m_CrossArenaPool.BattleResult.iResultScore);

        f_UpdateSelfRole(f_GetObject("VS"));
        f_UpdateEnemyRole(f_GetObject("VS"), enemy);
        if (isWin)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleVictory, false);
            //UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinTitleEffect, m_WinTitleParent);
        }
        else
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleFail, false);
        }
    }

    private void f_UpdateSelfRole(GameObject parent)
    {
        //parent.transform.Find("").GetComponent<UILabel>().text = Data_Pool.m_UserData.m_strServerName;
        parent.transform.Find("Label_MyName").GetComponent<UILabel>().text = UITool.f_GetImporentForName(Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iImportant, Data_Pool.m_UserData.m_szRoleName);
        parent.transform.Find("Label_MyFightPower").GetComponent<UILabel>().text = Data_Pool.m_TeamPool.f_GetTotalBattlePower() + "";
        parent.transform.Find("Label_MyLevel").GetComponent<UILabel>().text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level).ToString();
        parent.transform.Find("Icon_My").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteBySexId(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard) % 2);
    }

    private void f_UpdateEnemyRole(GameObject parent, CMsg_ArenaCrossInfo enemyInfo)
    {
        //mEnemyServerInfo.text = enemyInfo.ServerName;
        ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(enemyInfo.userExtra.offlineTime);
        parent.transform.Find("Label_EnemyName").GetComponent<UILabel>().text = "(" + serverInfo.szName + ")" + UITool.f_GetImporentForName(enemyInfo.userView.uFrameId, enemyInfo.userView.m_szName);
        parent.transform.Find("Label_EnemyFightPower").GetComponent<UILabel>().text = enemyInfo.userExtra.iBattlePower + "";
		if(enemyInfo.userView.uSex < 100)
		{
			parent.transform.Find("Icon_Enemy").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteBySexId(enemyInfo.userView.uSex);
		}
		else
		{
			parent.transform.Find("Icon_Enemy").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteByCardId(enemyInfo.userView.uSex);
		}
        parent.transform.Find("Label_EnemyLevel").GetComponent<UILabel>().text = enemyInfo.userExtra.iLv + "";

        
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_CloseMask(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaCrossBattleFinishPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }

    private void f_BattleLoseProcess(GameObject go, object value1, object value2)
    {
        UITool.f_ProcessBattleLose((EM_BattleLoseProcess)value1);
    }
}
