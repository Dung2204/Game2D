using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class LegionUpSuc : UIFramwork {
    UILabel LegionLv;
    UILabel LegionPeople;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        UpdateMain();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Bg", UI_Close);
        f_RegClickEvent("Suc", UI_Up);
        f_RegClickEvent("MaskClose", UI_Close);
    }

    void UI_Close(GameObject go,object obj1,object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionUpSuc, UIMessageDef.UI_CLOSE);
    }

    void UI_Up(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tsocket = new SocketCallbackDT();
        tsocket.m_ccCallbackSuc = f_Callback_LevelUpResult;
        tsocket.m_ccCallbackFail = f_Callback_LevelUpResult;
        LegionMain.GetInstance().m_LegionInfor.f_LegionUpLv(tsocket);
    }

    void UpdateMain()
    {
        LegionLv = f_GetObject("LvNum").GetComponent<UILabel>();
        LegionPeople = f_GetObject("PeopleNum").GetComponent<UILabel>();
        LegionLevelDT tLevel = glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetSC(LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().f_GetProperty((int)EM_LegionProperty.Lv)) as LegionLevelDT;
        LegionLevelDT lastLevel= glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetSC(LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().f_GetProperty((int)EM_LegionProperty.Lv)+1) as LegionLevelDT;

        LegionLv.text = string.Format("{0}     {1}", LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().f_GetProperty((int)EM_LegionProperty.Lv), LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().f_GetProperty((int)EM_LegionProperty.Lv)+1);
        LegionPeople.text = string.Format("{0}     {1}", tLevel.iCountMax, lastLevel.iCountMax);
    }

    private void f_Callback_LevelUpResult(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Cập nhật thành công");
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionUpSuc, UIMessageDef.UI_CLOSE);
        }
        else
        {
UITool.Ui_Trip("Cập nhật lỗi");
            MessageBox.ASSERT(string.Format("Legion levelup error, code:{0}",result));
        }
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionUpSuc, UIMessageDef.UI_CLOSE);
    }
}
