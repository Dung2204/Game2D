using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class AttackH : ccMachineStateBase
{
    private int _iCreateMaggicBallNum = 0;
    private int _iDoHarmMagicBallNum = 0;
    /// <summary>
    /// GameSocket
    /// </summary>
    protected RoleControl _RoleControl;
    public AttackH(RoleControl tRoleControl) : base((int)EM_AttackState.AttackH)
    {
        _RoleControl = tRoleControl;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        
        //MessageBox.DEBUG("角色攻击 AttackH");
        _iCreateMaggicBallNum = 0;
        _iDoHarmMagicBallNum = 0;
        if (_RoleControl.f_GetRoleAttack().m_iTrajectoryH > 0)
        {
            if (_RoleControl.f_GetRoleAttack().m_iMagicType == 2)
            {
                //播放命中效果
                GameObject tSpine = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(_RoleControl.f_GetRoleAttack().m_iTrajectoryH);
                if (tSpine == null)
                {
                    MessageBox.ASSERT("Attacking effect does not exist " + _RoleControl.f_GetRoleAttack().m_iTrajectoryH + ", default.");
					tSpine = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(1201112);
                }
                tSpine.layer = 5;
                MagicControl tMagicControl = tSpine.GetComponent<MagicControl>();
                if (tMagicControl == null)
                {
                    tMagicControl = tSpine.AddComponent<MagicControl>();    
                }
                
                RoleControl tBeAttackRoleControl = null;
                for (int i = 0; i < _RoleControl.f_GetRoleAttack().m_aData.Count; i++)
                {
                    if (_RoleControl.f_GetRoleAttack().m_aData[i].m_iIsBaseAttack == 0)
                    {
                        tBeAttackRoleControl = BattleManager.GetInstance().f_GetRoleControl(_RoleControl.f_GetRoleAttack().m_aData[i].m_iId);                        
                        break;
                    }
                }
                bool bFace = RoleTools.f_CheckFace(_RoleControl, tBeAttackRoleControl);
                if (!bFace)
                {
                    tSpine.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    tSpine.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (tBeAttackRoleControl != null)
                {
                    tMagicControl.f_Create(tBeAttackRoleControl.transform.position, tSpine, CallBack_MagicHarm, _RoleControl.f_GetRoleAttack().m_aData[0]);
                    if (_RoleControl.GetMagicIndex() == 3)
                    {
                        // tMagicControl.m_CharActionController.f_SetSkillDepth(true);
                        tMagicControl.m_CharActionController.SetUnScaledTime(true);
                    }
                    _iCreateMaggicBallNum = 0;
                }
            }
            else
            {
                for (int i = 0; i < _RoleControl.f_GetRoleAttack().m_aData.Count; i++)
                {
                    if (_RoleControl.f_GetRoleAttack().m_aData[i].m_iIsBaseAttack == 0)
                    {
                        //播放命中效果
                        GameObject tSpine = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(_RoleControl.f_GetRoleAttack().m_iTrajectoryH);
                        if (tSpine == null)
                        {
                            MessageBox.ASSERT("Attacking effect does not exist " + _RoleControl.f_GetRoleAttack().m_iTrajectoryH + ", default.");
							tSpine = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(1201112);
                        }
                        tSpine.layer = 5;
                        MagicControl tMagicControl = tSpine.GetComponent<MagicControl>();
                        if (tMagicControl == null)
                        {
                            tMagicControl = tSpine.AddComponent<MagicControl>();
                        }
                        
                        RoleControl tBeAttackRoleControl = BattleManager.GetInstance().f_GetRoleControl(_RoleControl.f_GetRoleAttack().m_aData[i].m_iId);
                        bool bFace = RoleTools.f_CheckFace(_RoleControl, tBeAttackRoleControl);
                        if (!bFace)
                        {
                            tSpine.transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                        else
                        {
                            tSpine.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        if (tBeAttackRoleControl != null)
                        {
                            tMagicControl.f_Create(tBeAttackRoleControl.transform.position, tSpine, CallBack_MagicHarm, _RoleControl.f_GetRoleAttack().m_aData[i]);
                            if (_RoleControl.GetMagicIndex() == 3)
                            {
                                // tMagicControl.m_CharActionController.f_SetSkillDepth(true);
                                tMagicControl.m_CharActionController.SetUnScaledTime(true);
                            }
                            _iCreateMaggicBallNum++;
                        }
                    }
                }
            }
        }
        else
        {
            f_RoleDispHarm(null);
        }
    }

    private void CallBack_MagicHarm(object Obj)
    {
        if (_RoleControl.f_GetRoleAttack().m_iMagicType == 2)
        {
            f_RoleDispHarm(null);
        }
        else
        {
            f_RoleDispHarm(Obj);
        }
    }

    public void f_RoleDispHarm(object Obj)
    {
        int iHpPercent = 0;
        if (Obj == null)
        {
            _iDoHarmMagicBallNum++;
            iHpPercent = _RoleControl.f_GetRoleAttack().f_GetCurHarmPercent(true);
            int harmNum = _RoleControl.f_GetRoleAttack().m_iCurHarmNum;
            for (int i = 0; i < _RoleControl.f_GetRoleAttack().m_aData.Count; i++)
            {
                stBeAttackInfor tstBeAttackInfor = _RoleControl.f_GetRoleAttack().m_aData[i];
                //MessageBox.DEBUG(_RoleControl.m_iId + " 1第" + _RoleControl.f_GetRoleAttack().m_iCurHarmNum + "段攻击 " + iHpPercent);
                if ((EM_AttackType)tstBeAttackInfor.m_iIsBaseAttack == EM_AttackType.eBase)
                {
                    DispHarm(tstBeAttackInfor, iHpPercent, _RoleControl.f_GetRoleAttack().f_CheckIsEnd(), harmNum);
                }
            }
        }
        else
        {
            _iDoHarmMagicBallNum++;
            if (_iDoHarmMagicBallNum == _iCreateMaggicBallNum)
            {
                iHpPercent = _RoleControl.f_GetRoleAttack().f_GetCurHarmPercent();
                _iDoHarmMagicBallNum = 0;
            }
            else
            {
                iHpPercent = _RoleControl.f_GetRoleAttack().f_GetCurHarmPercent(false);
            }
            stBeAttackInfor tstBeAttackInfor = (stBeAttackInfor)Obj;
            //MessageBox.DEBUG(_RoleControl.m_iId + " 11第" + _RoleControl.f_GetRoleAttack().m_iCurHarmNum + "段攻击 " + iHpPercent);
            DispHarm(tstBeAttackInfor, iHpPercent, _RoleControl.f_GetRoleAttack().f_CheckIsEnd());
        }
    }

    private void DispHarm(stBeAttackInfor tstBeAttackInfor, int iHpPercent, bool bIsEnd,int harmNum = 0)
    {
        RoleControl tBeAttackRoleControl = BattleManager.GetInstance().f_GetRoleControl(tstBeAttackInfor.m_iId);

        if (tBeAttackRoleControl != null)
        {
            //4，减少x怒气 5，增加x怒气 6，清除中毒和灼烧效果 7，清除不利BUFF 8，清除增益BUFF  
            if (tstBeAttackInfor.m_iHp == 9999999)
            {
                tBeAttackRoleControl.f_BeAttack((EM_AttackType)tstBeAttackInfor.m_iIsBaseAttack, tstBeAttackInfor.m_iHp, tstBeAttackInfor.m_iCirt, false, harmNum);
            }
            else
            {
                if (tstBeAttackInfor.m_iHp == 0)
                {
                    if (iHpPercent == 10)
                    {
                        tBeAttackRoleControl.f_BeAttack((EM_AttackType)tstBeAttackInfor.m_iIsBaseAttack, 0, tstBeAttackInfor.m_iCirt);
                        MessageBox.DEBUG(_RoleControl.m_iId + " >> " + tBeAttackRoleControl.m_iId + " HP 0/" + tstBeAttackInfor.m_iHp);
                    }

                }
                else
                {
                    int iHp = tstBeAttackInfor.m_iHp;
                    if (iHpPercent == 10)
                    {
                        tBeAttackRoleControl.f_BeAttack((EM_AttackType)tstBeAttackInfor.m_iIsBaseAttack, iHp, tstBeAttackInfor.m_iCirt);
                    }
                    else
                    {
                        iHp = tstBeAttackInfor.m_iHp * iHpPercent / 10;
                        tBeAttackRoleControl.f_BeAttack((EM_AttackType)tstBeAttackInfor.m_iIsBaseAttack, iHp, tstBeAttackInfor.m_iCirt, false, harmNum);
                    }
                    //MessageBox.DEBUG(_RoleControl.m_iId + " >> " + tBeAttackRoleControl.m_iId + " HP " + iHp + "/" + tstBeAttackInfor.m_iHp);
                }
            }
        }

    }

    public override void f_Execute()
    {

    }




}
