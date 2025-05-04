using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class AttackSkill3MV : ccMachineStateBase
{

    protected RoleControl _RoleControl;
    public AttackSkill3MV(RoleControl tRoleControl) : base((int)EM_AttackState.AttackSkill3MV)
    {
        _RoleControl = tRoleControl;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);

        _RoleControl.f_DispBeginBuf();
        if (CheckIsPause() || _RoleControl.f_CheckIsDie())
        {
            f_SetComplete((int)EM_AttackState.AttackEnd);
        }
        else
        {
            RoleAttack tRoleAttack = _RoleControl.f_GetRoleAttack();
            MagicDT tMagicDT = (MagicDT)glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(tRoleAttack.m_iMagicId);
            if (tMagicDT == null)
            {
                f_ChangeStartAttack(null);
            }
            FitSkillParam tFitSkill = new FitSkillParam(tMagicDT, _RoleControl, f_ChangeStartAttack);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.FitSkillPage, UIMessageDef.UI_OPEN, tFitSkill);
        }
    }

    public override void f_Execute()
    {

        //f_SetComplete((int)EM_AttackState.AttackStart);
    }

    public void f_ChangeStartAttack(object obj)
    {
        f_SetComplete((int)EM_AttackState.AttackStart);
    }

    private bool CheckIsPause()
    {
        RoleAttack tRoleAttack = _RoleControl.f_GetRoleAttack();
        if (tRoleAttack.m_iStayPos == 9990)
        {//被眩晕等
            return true;
        }
        return false;
    }



}