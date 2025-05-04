using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class Attacking : ccMachineStateBase
{
    /// <summary>
    /// GameSocket
    /// </summary>
    protected RoleControl _RoleControl;
    public Attacking(RoleControl tRoleControl) : base((int)EM_AttackState.Attacking)
    {
        _RoleControl = tRoleControl;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        _RoleControl.f_StartAttack();
        //MessageBox.DEBUG("角色攻击 Attacking");
    }

    public override void f_Execute()
    {

    }


}