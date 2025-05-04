using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using UnityEngine;
class GuidanceEnd : ccMachineStateBase
{
    GuidanceManage _GuidanceManage;
    public GuidanceEnd(GuidanceManage guidancemanage) : base((int)EM_Guidance.GuidanceEnd)
    {
        _GuidanceManage = guidancemanage;
    }
    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        Data_Pool.m_GuidancePool.f_SetCurClickButtonNull();
        Data_Pool.m_GuidancePool.m_GuidanceArr.SetActive(false);
        Data_Pool.m_GuidancePool.f_SetArrNull();
        GameObject.Destroy(Data_Pool.m_GuidancePool.m_GuidanceManage.gameObject);
        Data_Pool.m_GuidancePool.IsEnter = false;
        Data_Pool.m_GuidancePool.IsChange = false;
       

    }
}
