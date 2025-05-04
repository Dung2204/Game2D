using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class LegionBattleAwardPage : UIFramwork
{
    private List<NBaseSCDT> awardList;

    private GameObject battleAwardParent;
    private GameObject battleAwardItem;
    private UIWrapComponent _battleAwardWrapComponent;
    private UIWrapComponent mBattleAwardWrapComponent
    {
        get
        {
            if (_battleAwardWrapComponent == null)
            {
                _battleAwardWrapComponent = new UIWrapComponent(230, 1, 800, 6, battleAwardParent, battleAwardItem, awardList, f_BattleAwardItemUpdateByInfo, null);
            }
            return _battleAwardWrapComponent;
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
        //构造奖励列表数据
        awardList = new List<NBaseSCDT>();
        for (int i = 0; i < LegionConst.LEGION_BATTLE_AWARD_STAR.Length; i++)
        {
            awardList.Add(new LegionBattleAwardNode(i, LegionConst.LEGION_BATTLE_AWARD_STAR[i]));
        }
        battleAwardParent = f_GetObject("BattleAwardParent");
        battleAwardItem = f_GetObject("BattleAwardItem");
        f_RegClickEvent("BtnClose", f_OnBtnCloseClick);
        //f_RegClickEvent("BtnCancel", f_OnBtnCloseClick);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        mBattleAwardWrapComponent.f_ResetView();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    private void f_OnBtnCloseClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleAwardPage, UIMessageDef.UI_CLOSE);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleAwardPage, UIMessageDef.UI_CLOSE);
    }

    private void f_BattleAwardItemUpdateByInfo(Transform tf, NBaseSCDT dt)
    {
        LegionBattleAwardItem tItem = tf.GetComponent<LegionBattleAwardItem>();
        LegionBattleAwardNode tInfo = (LegionBattleAwardNode)dt;
        tItem.f_UpdateByInfo(tInfo);
    }
}
