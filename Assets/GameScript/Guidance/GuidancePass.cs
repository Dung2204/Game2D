using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using UnityEngine;
class GuidancePass : ccMachineStateBase
{
    private string[] szCheckUiName;
    public GuidancePass(string[] CheckUi) : base((int)EM_Guidance.GuidancePass)
    {
        szCheckUiName = CheckUi;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        ccTimeEvent.GetInstance().f_Pause();
        Data_Pool.m_GuidancePool.f_SetCurClickButtonNull();
    }

    public override void f_Execute()
    {
        base.f_Execute();

        bool IsInit = true;
        for (int i = 0; i < szCheckUiName.Length; i++)
        {
            if (ccUIManage.GetInstance().f_CheckUIIsOpen(szCheckUiName[i]))
            {
                IsInit = false;
                f_SetComplete((int)EM_Guidance.GuidancePass);
                break;
            }
            IsInit = true;
        }
        if (IsInit)
        {
            f_SetComplete((int)EM_Guidance.GuidancePlay, Data_Pool.m_GuidancePool.m_GuidanceDT);
            //Data_Pool.m_GuidancePool.f_SetCurClickButton(Data_Pool.m_GuidancePool.GuidanceBtnName, Data_Pool.m_GuidancePool.m_GuidanceCallback);
        }

    }
}

