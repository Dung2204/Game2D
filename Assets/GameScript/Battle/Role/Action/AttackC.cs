using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class AttackC : ccMachineStateBase
{
    /// <summary>
    /// GameSocket
    /// </summary>
    protected RoleControl _RoleControl;
    public AttackC(RoleControl tRoleControl) : base((int)EM_AttackState.AttackC)
    {
        _RoleControl = tRoleControl;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);        
        _RoleControl.f_StartAttack();
        
        //MessageBox.DEBUG("角色攻击 Attacking");
    }

    public void f_CrearteTrajectory()
    {
        if (_RoleControl.f_GetRoleAttack().m_iTrajectoryC > 0)
        {
MessageBox.ASSERT("Range Skill");
        }
        else
        {
            f_SetComplete((int)EM_AttackState.AttackH);
        }
    }

    public override void f_Execute()
    {

    }


}
