using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 升级了页面参数
/// </summary>
public class LevelUpPageParam
{
    public int mOldLevel;//老等级
    public int mNewLevel;//新等级
    public int mOldEnergy;//老体力
    public int mNewEnergy;//新体力
    public int mOldVigor;//老精力
    public int mNewVigor;//新精力
    public ccUIBase mHoldUI;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="oldLevel">老等级</param>
    /// <param name="newLevel">新等级</param>
    /// <param name="oldEnergy">老体力</param>
    /// <param name="newEnergy">新体力</param>
    /// <param name="oldVigor">老精力</param>
    /// <param name="newVigor">新精力</param>
    public LevelUpPageParam(int oldLevel, int newLevel, int oldEnergy, int newEnergy, int oldVigor, int newVigor)
    {
        this.mOldLevel = oldLevel;
        this.mNewLevel = newLevel;
        this.mOldEnergy = oldEnergy;
        this.mNewEnergy = newEnergy;
        this.mOldVigor = oldVigor;
        this.mNewVigor = newVigor;
    }
}
/// <summary>
/// 升级了的提示消息
/// </summary>
public class LevelUpPage : UIFramwork
{
    LevelUpPageParam param;
	//My Code
	GameParamDT CardBattleOpen;
	int CardBattleOpenLvl = 30;
	//
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        param = (LevelUpPageParam)e;
        f_GetObject("OldLevel").GetComponent<UILabel>().text = param.mOldLevel.ToString();
        f_GetObject("NewLevel").GetComponent<UILabel>().text = param.mNewLevel.ToString();
        f_GetObject("OldEnergy").GetComponent<UILabel>().text = param.mOldEnergy.ToString();
        f_GetObject("NewEnergy").GetComponent<UILabel>().text = param.mNewEnergy.ToString();
        f_GetObject("OldVigor").GetComponent<UILabel>().text = param.mOldVigor.ToString();
        f_GetObject("NewVigor").GetComponent<UILabel>().text = param.mNewVigor.ToString();

        //f_GetObject("AddLevel").GetComponent<UILabel>().text = (param.mNewLevel - param.mOldLevel).ToString();
        //f_GetObject("AddEnergy").GetComponent<UILabel>().text = (param.mNewEnergy - param.mOldEnergy).ToString();
        //f_GetObject("AddVigor").GetComponent<UILabel>().text = (param.mNewVigor - param.mOldVigor).ToString();

        //f_GetObject("AddEnergyRoot").gameObject.SetActive((param.mNewEnergy - param.mOldEnergy) > 0 ? true : false);
        //f_GetObject("AddVigorRoot").gameObject.SetActive((param.mNewVigor - param.mOldVigor) > 0 ? true : false);

        string HasOpenFunc;
        string NextOpenFuc;
        string OpenFuncSprite;
        string NextOpenFuncSprite;
        int nextOpenLevel;
        UITool.f_GetCurLevelOpenSystemDes(out HasOpenFunc, out NextOpenFuc, out nextOpenLevel,out OpenFuncSprite,out NextOpenFuncSprite);
        f_GetObject("SpriNorLv").SetActive(HasOpenFunc != null);
        f_GetObject("LabelHasOpen").GetComponent<UILabel>().text = HasOpenFunc ?? "";
        f_GetObject("SpriNextLv").SetActive(NextOpenFuc != null);
        f_GetObject("LabelNextOpen").GetComponent<UILabel>().text = NextOpenFuc ?? "";
f_GetObject("LabelNextOpenTitle").GetComponent<UILabel>().text = "Cấp " + nextOpenLevel + " sẽ mở：";
        f_GetObject("SpriNorLv").GetComponent<UISprite>().spriteName = OpenFuncSprite;
        f_GetObject("SpriNextLv").GetComponent<UISprite>().spriteName = NextOpenFuncSprite;
        f_GetObject("OpenGrid").GetComponent<UIGrid>().Reposition();
		// if(OpenFuncSprite == "Icon_CrossServer")
		// {
			// f_GetObject("LabelHasOpen").GetComponent<UILabel>().text = "Mở Đỉnh Phong";
		// }
		// if(nextOpenLevel == 2)
		// {
			// f_GetObject("SpriNextLv").GetComponent<UISprite>().spriteName = "LineUpPage6";
			// f_GetObject("LabelNextOpen").GetComponent<UILabel>().text = "Mở tất cả ô tướng";
		// }
		// if(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < 5)
		// {
			// f_GetObject("SpriNorLv").GetComponent<UISprite>().spriteName = "LineUpPage6";
			// f_GetObject("LabelHasOpen").GetComponent<UILabel>().text = "Mở tất cả ô tướng";
		// }
		// if(nextOpenLevel == 52)
		// {
			// f_GetObject("SpriNextLv").GetComponent<UISprite>().spriteName = "Icon_CrossServer";
			// f_GetObject("LabelNextOpen").GetComponent<UILabel>().text = "Mở Đỉnh Phong";
		// }
		// if(nextOpenLevel == 55 || (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) > 51 && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < 55))
		// {
			// f_GetObject("LabelNextOpenTitle").GetComponent<UILabel>().text = "Cấp " + "55" + " sẽ mở：";
			// f_GetObject("SpriNextLv").GetComponent<UISprite>().spriteName = "Icon_CardBattle";
			// f_GetObject("LabelNextOpen").GetComponent<UILabel>().text = "Mở Hội Võ";
		// }
		// if(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) > 54 && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < 60)
		// {
			// f_GetObject("SpriNorLv").GetComponent<UISprite>().spriteName = "Icon_CardBattle";
			// f_GetObject("LabelHasOpen").GetComponent<UILabel>().text = "Mở Hội Võ";
		// }
		// if(nextOpenLevel == 300)
		// {
			// f_GetObject("SpriNextLv").SetActive(false);
		// }
		
		//My Code
		CardBattleOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(96) as GameParamDT);
		CardBattleOpenLvl = CardBattleOpen.iParam3 * 1;
		//
		
		if(OpenFuncSprite == "Icon_CrossServer")
		{
f_GetObject("LabelHasOpen").GetComponent<UILabel>().text = "Đấu Đỉnh Phong";
		}
		if(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) == 2)
		{
			f_GetObject("SpriNorLv").GetComponent<UISprite>().spriteName = "LineUpPage2";
f_GetObject("LabelHasOpen").GetComponent<UILabel>().text = "Mở rộng đội hình";
		}
		if(nextOpenLevel == UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle))
		{
			f_GetObject("SpriNextLv").GetComponent<UISprite>().spriteName = "Icon_CrossServer";
f_GetObject("LabelNextOpen").GetComponent<UILabel>().text = "Đấu Đỉnh Phong";
		}
		if(nextOpenLevel == CardBattleOpenLvl)
		{
f_GetObject("LabelNextOpenTitle").GetComponent<UILabel>().text = " Cấp " + CardBattleOpenLvl + " mở：";
			f_GetObject("SpriNextLv").GetComponent<UISprite>().spriteName = "Icon_CardBattle";
f_GetObject("LabelNextOpen").GetComponent<UILabel>().text = "Mở Hội Võ";
		}
		if(OpenFuncSprite == "Icon_CardBattle" && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= CardBattleOpenLvl)
		{
f_GetObject("LabelHasOpen").GetComponent<UILabel>().text = " Hội Võ";
		}
		if(nextOpenLevel == UITool.f_GetSysOpenLevel(EM_NeedLevel.TariningLevel))
		{
f_GetObject("LabelNextOpen").GetComponent<UILabel>().text = "Mở Trận Pháp";
		}
		if(OpenFuncSprite == "Icon_Tactical")
		{
f_GetObject("LabelHasOpen").GetComponent<UILabel>().text = " Trận Pháp";
		}
		if(nextOpenLevel == UITool.f_GetSysOpenLevel(EM_NeedLevel.EquipUpStar))
		{
			f_GetObject("SpriNextLv").GetComponent<UISprite>().spriteName = "UpStar";
f_GetObject("LabelNextOpen").GetComponent<UILabel>().text = "Mở Thăng Tinh";
		}
		if(nextOpenLevel == 300)
		{
			f_GetObject("SpriNextLv").SetActive(false);
		}

        GameObject TexLevelUp = f_GetObject("TexLevelUp");
        if (TexLevelUp.transform.GetComponent<TweenScale>() != null)
        {
            Destroy(TexLevelUp.transform.GetComponent<TweenScale>());
        }
        TweenScale ts = TexLevelUp.AddComponent<TweenScale>();
        ts.from = new Vector3(0, 0, 1);
        ts.to = new Vector3(1, 1, 1);
        ts.animationCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
MessageBox.DEBUG("The promotion interface is closed");
    }
    /// <summary>
    /// 注册消息事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBG", OnBtnBlackClick);
        f_RegClickEvent("SpriNorLv", OnBtnGoToPage);

    }
    #region 按钮事件
    /// <summary>
    /// 点击黑色背景关闭按钮
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LevelUpPage, UIMessageDef.UI_CLOSE);
    }

    private void OnBtnGoToPage(GameObject go, object obj1, object obj2)
    {
        int Level = param.mNewLevel;
        if (Data_Pool.m_GuidancePool.IsOpenSween)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonSweepPage, UIMessageDef.UI_CLOSE);
        }
        if (Data_Pool.m_GuidancePool.IsOpenArenaSween)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaSweepPage, UIMessageDef.UI_CLOSE);
        }
        if (Data_Pool.m_GuidancePool.IsOpponent)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectOpponentPage, UIMessageDef.UI_CLOSE);
        }
        if (Data_Pool.m_GuidancePool.IsOpponentSween)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabSweepResultPage, UIMessageDef.UI_CLOSE);
        }
        if (Data_Pool.m_GuidancePool.IsOpenOneKeyGrabTreasure) {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.OneKeyGrabTreasurePage, UIMessageDef.UI_CLOSE);
        }
        if (Data_Pool.m_GuidancePool.IsOpenChanllenge)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonChallengePage, UIMessageDef.UI_CLOSE);
        }
        if (Data_Pool.m_ArenaPool.mBreakRankInfo.m_bShowInfo)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaBreakAwardPage, UIMessageDef.UI_CLOSE);
        }
        ccUIBase tUi = ccUIManage.GetInstance().f_GetUIHandler(Data_Pool.m_GuidancePool.OpenLevelPageUIName);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LevelUpPage, UIMessageDef.UI_CLOSE);



        switch (Level)
        {
            case 10:
                UITool.f_GotoPage(tUi, UINameConst.ArenaPageNew, 0);
                break;
            case 14:
                UITool.f_GotoPage(tUi, UINameConst.GrabTreasurePage, 5004);
                break;
            case 25:
                UITool.f_GotoPage(tUi, UINameConst.RunningManPage, 0);
                break;
            case 28:
                UITool.f_GotoPage(tUi, UINameConst.DungeonChapterPageNew, (int)EM_Fight_Enum.eFight_Legend);
                break;
            case 40:
                UITool.f_GotoPage(tUi, UINameConst.LineUpPage, 0);
                break;
            case 35:
                UITool.f_GotoPage(tUi, UINameConst.PatrolPage, 0);
                break;
            default:
                break;
        }

    }
    #endregion
}
