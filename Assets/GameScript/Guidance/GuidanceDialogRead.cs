using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using UnityEngine;

class GuidanceDialogRead : ccMachineStateBase
{
    GuidanceDialogDT tDialog;
   
    public GuidanceDialogRead() : base((int)EM_Guidance.GuidanceDialogRead)
    {
    }
    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        if (Obj != null)
        {
            tDialog = (GuidanceDialogDT)glo_Main.GetInstance().m_SC_Pool.m_GuidanceDialogSC.f_GetSC((int)Obj);
            
        }
        f_SetComplete((int)EM_Guidance.GuidanceDialogPlay, tDialog);
    }
    

}

