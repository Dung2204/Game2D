using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using UnityEngine;
class GuidanceDialogEnd : ccMachineStateBase
{
    public GuidanceDialogEnd() : base((int)EM_Guidance.GuidanceDialogEnd)
    {
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        f_SetComplete((int)EM_Guidance.GuidanceRead);
    }
}

