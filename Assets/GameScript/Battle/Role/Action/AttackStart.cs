using UnityEngine;
using System.Collections;
using ccU3DEngine;
using DG.Tweening;

public class AttackStart : ccMachineStateBase
{
    private float _fAlpha = 0;
    private int _iStep = 0;
	bool isMoving = false;
    float walkSpeed = 2f;
    /// <summary>
    /// GameSocket
    /// </summary>
    protected RoleControl _RoleControl;
    public AttackStart(RoleControl tRoleControl) : base((int)EM_AttackState.AttackStart)
    {
        _RoleControl = tRoleControl;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        _iStep = 0;
        _fAlpha = 1;
        _RoleControl.m_CharActionController.f_PlayStand();
        //MessageBox.DEBUG("角色攻击 AttackStart");

        _RoleControl.f_DispBeginBuf();
        if (CheckIsPause() || _RoleControl.f_CheckIsDie())
        {
            f_SetComplete((int)EM_AttackState.AttackEnd);
        }
        else
        {
            _RoleControl.f_UnShowBuf();
        }
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

    public override void f_Execute()
    {
        
        int speed = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1);
        int posIndex = (int)_RoleControl.f_Get_FormationPos();
        int attackPosIndex = (int)_RoleControl.f_GetRoleAttack().m_iStayPos;
        if (_RoleControl.f_GetMagicAttackPos() == 99) // đứng tại chổ
        {
            if (_iStep == 0 && isMoving == false)
            {
                _RoleControl.f_SaveFaceDirection(0);
                float newX = _RoleControl.f_GetRoleAttack().m_v3AttackPos.x;
                Vector3 pos = _RoleControl.f_GetRoleAttack().m_v3AttackPos;
                _RoleControl.UpdateDirectionByMove(_RoleControl.transform.position.x, newX);
                
                if (attackPosIndex >= (int)EM_BattleIndex.A1 && attackPosIndex <= (int)EM_BattleIndex.A5)
                {
                    if (posIndex >= (int)EM_BattleIndex.A6) // xử lý hướng
                    {
                        _RoleControl.f_SaveFaceDirection(2);
                        _RoleControl.f_Face2Right();
                    }
                }
                else if (attackPosIndex >= (int)EM_BattleIndex.B1 && attackPosIndex <= (int)EM_BattleIndex.B5)
                {
                    
                    if (posIndex >= (int)EM_BattleIndex.A6) // xử lý hướng
                    {
                        _RoleControl.f_SaveFaceDirection(1);
                        _RoleControl.f_Face2Left();
                    }
                }
                else if(attackPosIndex == (int)EM_BattleIndex.MyLeft)
                {
                    _RoleControl.f_SaveFaceDirection(1);
                }
                else if (attackPosIndex == (int)EM_BattleIndex.MyRight)
                {
                    _RoleControl.f_SaveFaceDirection(2);
                }
                else if (attackPosIndex == (int)EM_BattleIndex.My)
                {
                    _RoleControl.UpdateDirection();
                }
                _iStep = 1; isMoving = false;
                //if (_RoleControl.NeedMove != null)
                //{
                //    float newX = _RoleControl.f_GetRoleAttack().m_v3AttackPos.x;
                //    Vector3 pos = _RoleControl.f_GetRoleAttack().m_v3AttackPos;
                //    _RoleControl.UpdateDirectionByMove(_RoleControl.transform.position.x, newX);
                //    if (_RoleControl.transform.position.x == newX)
                //    {
                //        _iStep = 1;
                //        return;
                //    }
                //    isMoving = true;
                //    //float newX = _RoleControl._v3StayPos.x + _RoleControl.NeedMove;
                //    // MessageBox.ASSERT("Move: " + _RoleControl.NeedMove); 
                //    // MessageBox.ASSERT("newX: " + newX);
                //    _RoleControl.m_CharActionController.f_PlayWalk(walkSpeed * speed);
                //    _RoleControl.transform.DOMove(pos, System.Math.Abs((newX + 100f) - (_RoleControl.transform.position.x + 100f)) * (walkSpeed / speed)).OnComplete(() => { _iStep = 1; isMoving = false; _RoleControl.m_CharActionController.f_PlayStand(); });

                //    //_RoleControl.transform.DOMoveX(newX, System.Math.Abs((newX + 100f) - (_RoleControl.transform.position.x + 100f)) * (walkSpeed)).OnComplete(() => { _iStep = 1; isMoving = false; _RoleControl.m_CharActionController.f_PlayStand(); });
                //    //MessageBox.ASSERT("New X: " + newX + " Distance: " + (newX - _RoleControl.transform.position.x));
                //}
                //else
                //{
                //    _iStep = 1; isMoving = false;
                //}
            }
            else if (_iStep == 1)
            {
                _RoleControl.m_CharActionController.f_SetAlpha(_fAlpha);
                f_SetComplete((int)EM_AttackState.AttackC);
            }
        }
        else
        {
            if (_iStep == 0)
            {
                //_fAlpha = _fAlpha - 0.1f;
                _RoleControl.m_CharActionController.f_SetAlpha(_fAlpha);
                //    if (_fAlpha < 0)
                //    {
                _iStep = 1;
                //    }
            }
            else
            if (_iStep == 1)
            {
                _RoleControl.f_UnShowHpPanel();
                //_RoleControl.f_GoAttackPos();
                _iStep = 2;
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
            else if (_iStep == 3 && isMoving == false)
            {
                _RoleControl.f_SaveFaceDirection(0);
                if (_RoleControl.NeedMove != null)
                {
                    float newX = _RoleControl.f_GetRoleAttack().m_v3AttackPos.x + _RoleControl.NeedMove;
                    Vector3 pos = _RoleControl.f_GetRoleAttack().m_v3AttackPos;
                    
                    if (!((posIndex >= (int)EM_BattleIndex.A1 && posIndex <= (int)EM_BattleIndex.A5) || (posIndex >= (int)EM_BattleIndex.B1 && posIndex <= (int)EM_BattleIndex.B5)))
                    {
                        _RoleControl.UpdateDirectionByMove(_RoleControl.transform.position.x, newX);
                    }
                    
                    if (_RoleControl.transform.position.x == newX)
                    {
                        _iStep = 4; isMoving = false;
                        return;
                    }
                    if (attackPosIndex >= (int)EM_BattleIndex.A1 && attackPosIndex <= (int)EM_BattleIndex.A5)
                    {
                        if (posIndex >= (int)EM_BattleIndex.A6 ) // xử lý hướng
                        {
                            _RoleControl.f_Face2Right();
                            _RoleControl.f_SaveFaceDirection(2);
                        }
                    }else if (attackPosIndex >= (int)EM_BattleIndex.B1 && attackPosIndex <= (int)EM_BattleIndex.B5)
                    {
                        if (posIndex >= (int)EM_BattleIndex.A6 ) // xử lý hướng
                        {
                            _RoleControl.f_Face2Left();
                            _RoleControl.f_SaveFaceDirection(1);
                        }
                        
                    }
                    
                    isMoving = true;
                    
                    //float newX = _RoleControl._v3StayPos.x + _RoleControl.NeedMove;
                    // MessageBox.ASSERT("Move: " + _RoleControl.NeedMove);
                    // MessageBox.ASSERT("newX: " + newX);
                    _RoleControl.m_CharActionController.f_PlayWalk(walkSpeed * speed);
                    _RoleControl.transform.DOMove(pos, System.Math.Abs((newX + 100f) - (_RoleControl.transform.position.x + 100f)) * (walkSpeed/ speed)).OnComplete(() => { _iStep = 4; isMoving = false; _RoleControl.m_CharActionController.f_PlayStand(); });
                    //_RoleControl.transform.DOMoveX(newX, System.Math.Abs((newX + 100f) - (_RoleControl.transform.position.x + 100f)) * (walkSpeed)).OnComplete(() => { _iStep = 4; isMoving = false; _RoleControl.m_CharActionController.f_PlayStand();});
                    //MessageBox.ASSERT("New X: " + newX + " Distance: " + (newX - _RoleControl.transform.position.x));
                }
                else {
                    _iStep = 4; isMoving = false;
                }
                
            }
            else if (_iStep == 4)
            {
                _RoleControl.m_CharActionController.f_SetAlpha(1);
                f_SetComplete((int)EM_AttackState.AttackC);
            }
        }
    }
    
}