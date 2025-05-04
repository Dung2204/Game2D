using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class AttackEnd : ccMachineStateBase
{
    float _fAlpha;
    private ccCallback _Callback_AttackEnd;
    private int _iStep = 0;
    /// <summary>
    /// GameSocket
    /// </summary>
    protected RoleControl _RoleControl;
    public AttackEnd(RoleControl tRoleControl, ccCallback tccCallback) : base((int)EM_AttackState.AttackEnd)
    {
        _RoleControl = tRoleControl;
        _Callback_AttackEnd = tccCallback;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        _fAlpha = 1;
        _iStep = 0;
        DispEndBuf();
        //MessageBox.DEBUG("角色攻击 AttackEnd");
    }

    public override void f_Execute()
    {
        //多段伤害检验
        //if (!_RoleControl.f_GetRoleAttack().f_CheckIsEnd())
        //{
        //    MessageBox.ASSERT("发现未使用完的伤害段 " + _RoleControl.f_GetModelId());
        //}


        if (_RoleControl.f_GetMagicAttackPos() == 99)
        {
            _RoleControl.m_CharActionController.f_SetAlpha(1);
            _RoleControl.UpdateDirection();
            CallEnd();
        }
        else
        {
            if (_iStep == 0)
            {
                //_fAlpha = _fAlpha - 0.1f;
                _RoleControl.m_CharActionController.f_SetAlpha(_fAlpha);
                //if (_fAlpha < 0)
                //{
                    _iStep = 1;
                //}
            }
            else 
            if (_iStep == 1)
            {
                 _RoleControl.f_GoHome((e)=> { _iStep = 2; });
                
            }
            else if (_iStep == 2)
            {
                //_fAlpha = _fAlpha + 0.1f;
                _RoleControl.m_CharActionController.f_SetAlpha(_fAlpha);
                //if (_fAlpha >= 1)
                //{
                    _iStep = 3;
                //}
            }
            else if (_iStep == 3)
            {
                _RoleControl.m_CharActionController.f_SetAlpha(1);
                CallEnd();
            }
        }
    }

    private void CallEnd()
    {
        _RoleControl.f_ShowBuf();
        _Callback_AttackEnd(null);
    }

    private void DispEndBuf()
    {
        RoleAttack tRoleAttack = _RoleControl.f_GetRoleAttack();
        for (int i = 0; i < tRoleAttack.m_aData.Count; i++)
        {
            if ((EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack == EM_AttackType.eBufEnd)
            {
                RoleControl tRoleControl = _RoleControl;
                if (tRoleAttack.m_aData[i].m_iId != _RoleControl.m_iId)
                {
                    tRoleControl = BattleManager.GetInstance().f_GetRoleControl(tRoleAttack.m_aData[i].m_iId);
                }
                tRoleControl.f_BeAttack((EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack, tRoleAttack.m_aData[i].m_iHp, tRoleAttack.m_aData[i].m_iCirt);
                if (tRoleControl.f_CheckIsDie())
                {
                    _RoleControl.f_SaveFaceDirection(0);
                }
            }
            if ((EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack == EM_AttackType.eRoleEndEffect)
            {
                RoleControl tRoleControl = _RoleControl;
                if (tRoleAttack.m_aData[i].m_iId != _RoleControl.m_iId)
                {
                    tRoleControl = BattleManager.GetInstance().f_GetRoleControl(tRoleAttack.m_aData[i].m_iId);
                }
                tRoleControl.f_UpdateBuf(tRoleAttack.m_aData[i].m_iBuf1);
            }
            if ((EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack == EM_AttackType.eBase)
            {
                RoleControl tRoleControl = _RoleControl;
                if (tRoleAttack.m_aData[i].m_iId != _RoleControl.m_iId)
                {
                    tRoleControl = BattleManager.GetInstance().f_GetRoleControl(tRoleAttack.m_aData[i].m_iId);
                }
                EM_BattleMpType tEM_BattleMpType = EM_BattleMpType.Default;
                int iChangeAnger = 0;
                int iDefaultAnger = 0;
                iDefaultAnger = tRoleAttack.m_aData[i].m_iAnger % 10;
                if (tRoleAttack.m_aData[i].m_iAnger > 100 && tRoleAttack.m_aData[i].m_iAnger < 200)
                {//减少x怒气           
                    iChangeAnger = (tRoleAttack.m_aData[i].m_iAnger - 100) / 10;
                    tEM_BattleMpType = EM_BattleMpType.Lost;
                }
                else if (tRoleAttack.m_aData[i].m_iAnger > 200)
                {//增加x怒气 
                    iChangeAnger = (tRoleAttack.m_aData[i].m_iAnger - 200) / 10;
                    tEM_BattleMpType = EM_BattleMpType.Add;
                }
                else
                {
                    iDefaultAnger = tRoleAttack.m_aData[i].m_iAnger;
                }
                //怒气更新
                tRoleControl.f_SetMP(iDefaultAnger, iChangeAnger, tEM_BattleMpType);
            }
        }
    }
    

}