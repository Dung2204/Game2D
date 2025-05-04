using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class AttackWait : ccMachineStateBase
{
    /// <summary>
    /// GameSocket
    /// </summary>
    protected RoleControl _RoleControl;
    public AttackWait(RoleControl tRoleControl) : base((int)EM_AttackState.AttackWait)
    {
        _RoleControl = tRoleControl;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        //MessageBox.DEBUG(_RoleControl.m_iId + "角色攻击 AttackWait");
        _RoleControl.m_CharActionController.f_PlayStand();
    }

    //public override void f_Execute()
    //{

    //}


}