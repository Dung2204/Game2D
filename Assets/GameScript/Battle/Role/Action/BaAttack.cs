using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class BaAttack : ccMachineStateBase
{
    /// <summary>
    /// GameSocket
    /// </summary>
    protected RoleControl _RoleControl;
    public BaAttack(RoleControl tRoleControl) : base((int)EM_AttackState.BaAttack)
    {
        _RoleControl = tRoleControl;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        _RoleControl.m_CharActionController.f_SetDepthForOther(6);
        _RoleControl.m_CharActionController.f_PlayBeAttack();
    }

    //public override void f_Execute()
    //{

    //}





}