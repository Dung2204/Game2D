using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class LegionBattleListPage : UIFramwork
{
    private UILabel mTimeTip;
    private UILabel mDesc;
    private GameObject mNullDataTip;

    private List<BasePoolDT<long>> battleList;
    private GameObject battleListParent;
    private GameObject battleListItem;
    private UIWrapComponent _battleAwardWrapComponent;
    private UIWrapComponent mBattleAwardWrapComponent
    {
        get
        {
            if (_battleAwardWrapComponent == null)
            {
                battleList = LegionMain.GetInstance().m_LegionBattlePool.m_TableList;
                _battleAwardWrapComponent = new UIWrapComponent(86, 1, 800, 10, battleListParent, battleListItem, battleList, f_BattleListItemUpdateByInfo, null);
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
        mTimeTip = f_GetObject("TimeTip").GetComponent<UILabel>();
        mDesc = f_GetObject("Desc").GetComponent<UILabel>();
        mNullDataTip = f_GetObject("NullDataTip");
        battleListParent = f_GetObject("BattleListParent");
        battleListItem = f_GetObject("BattleListItem");
        f_RegClickEvent("BtnClose", f_OnBtnCloseClick);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mBattleAwardWrapComponent.f_ResetView();
        mNullDataTip.SetActive(battleList.Count == 0);
        int tTime = LegionMain.GetInstance().m_LegionBattlePool.m_iListTime;
        if (tTime == 0)
        {
            mTimeTip.text = string.Empty;
        }
        else
        {
            System.DateTime tTimeDate = ccMath.time_t2DateTime(tTime);
            mTimeTip.text = string.Format(CommonTools.f_GetTransLanguage(511),LegionMain.GetInstance().m_LegionBattlePool.m_iPeriod,tTimeDate.Month,tTimeDate.Day);
        }
        System.Text.StringBuilder tSb = new System.Text.StringBuilder();
        tSb.Append(CommonTools.f_GetTransLanguage(512));
        for (int i = 0; i < LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS.Length; i++)
        {
            tSb.Append(UITool.f_DayOfWeek2String((System.DayOfWeek)LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[i]));
            if (i != LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS.Length - 1)
                tSb.Append("，");
        }
        tSb.Append(CommonTools.f_GetTransLanguage(513));
        mDesc.text = tSb.ToString();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_BattleListItemUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        LegionBattleListItem tItem = tf.GetComponent<LegionBattleListItem>();
        tItem.f_UpdateByInfo((LegionBattleTableNode)dt);
    }

    private void f_OnBtnCloseClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleListPage, UIMessageDef.UI_CLOSE);
    }

}
