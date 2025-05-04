using UnityEngine;
using System.Collections;
using ccU3DEngine;
using Spine.Unity;

public class RebelAymyTriggen : UIFramwork
{
    RebelInfo tRebelInfo = new RebelInfo();

    private string triggenUIName;

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Data_Pool.m_GuidancePool.f_ChangeGuidanceType(EM_GuidanceType.RebelArmy);
        triggenUIName = (string)e;
        tRebelInfo = Data_Pool.m_RebelArmyPool.tRebelInfo;
        UpdateUI();
    }

    void UpdateUI()
    {
        RebelArmyDeployDT m_RebelArmyDeploy = (RebelArmyDeployDT)glo_Main.GetInstance().m_SC_Pool.m_RebelArmyDeploySC.f_GetSC(tRebelInfo.uDeployId);
        int[] CardId = ccMath.f_String2ArrayInt((m_RebelArmyDeploy.szMonsterId), ";");

        CardDT tcard = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(CardId[4]) as CardDT;
        //GameObject ArmyPos = UITool.f_GetStatelObject(tcard);
        //ArmyPos.transform.parent = f_GetObject("RebelArmyPos").transform;
        //ArmyPos.transform.localScale = Vector3.one * 200;
        //ArmyPos.transform.localPosition = Vector3.zero;
        //ArmyPos.transform.localEulerAngles = Vector3.zero;
        //ArmyPos.layer = 5;
        //ArmyPos.GetComponent<MeshRenderer>().sortingOrder = 27;
        ////ArmyPos.GetComponent<SkeletonAnimation>().AnimationName = "Stand";
        ////ArmyPos.GetComponent<SkeletonAnimation>().loop = true;
        //SkeletonAnimation SkeAni = ArmyPos.GetComponent<SkeletonAnimation>();
        //SkeAni.state.SetAnimation(0, "Stand", true);

        string name = tcard.szName;
        UITool.f_GetImporentColorName(tRebelInfo.color, ref name);
        f_GetObject("introduce").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1082), name, tRebelInfo.uRebelLv);


        f_RegClickEvent("Close", UI_Close);
        f_RegClickEvent("Suc", UI_OpenRebel);
    }
    void UI_Close(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelAymyTriggen, UIMessageDef.UI_CLOSE);
    }
    void UI_OpenRebel(GameObject go, object obj1, object obj2)
    {
		if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel))
		{
			if (Data_Pool.m_GuidancePool.IsOpenSween)
			{
				ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonSweepPage, UIMessageDef.UI_CLOSE);
			}

			ccUIBase uiTollgate = ccUIManage.GetInstance().f_GetUIHandler(UINameConst.DungeonTollgatePageNew);
			ccUIBase uiChapter = ccUIManage.GetInstance().f_GetUIHandler(UINameConst.DungeonChapterPageNew);
			if (uiTollgate!=null && uiTollgate.m_Panel && uiTollgate.m_Panel.gameObject.activeInHierarchy)
			{
				ccUIHoldPool.GetInstance().f_Hold(uiTollgate);
			}
			else if (uiChapter != null && uiChapter.m_Panel && uiChapter.m_Panel.gameObject.activeInHierarchy)
			{
				ccUIHoldPool.GetInstance().f_Hold(uiChapter);
			}
			ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonChallengePage, UIMessageDef.UI_CLOSE);
			ccUIManage.GetInstance().f_SendMsg(UINameConst.MainLineTreasureBookPage, UIMessageDef.UI_CLOSE);
			ccUIManage.GetInstance().f_SendMsg(UINameConst.MainLineOneKeySweepPage, UIMessageDef.UI_CLOSE);
			ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelAymyTriggen, UIMessageDef.UI_CLOSE);
			ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelArmy, UIMessageDef.UI_OPEN);
		}
		else
		{
			UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(742),UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel)));
		}
    }
}
