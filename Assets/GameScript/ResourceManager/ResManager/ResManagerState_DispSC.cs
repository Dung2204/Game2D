using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class ResManagerState_DispSC : ccMachineStateBase
{
    static EM_ResManagerStatic m_EM_AIStatic = EM_ResManagerStatic.DispSC;

    private bool m_bSaveCatchBuf;

    public ResManagerState_DispSC()
        : base((int)m_EM_AIStatic)
    {

    }

    public override void f_Enter(object Obj)
    {
        byte[] aBytes = (byte[])Obj;
        glo_Main.GetInstance().m_SC_Pool.f_LoadSC(aBytes);
        //glo_Main.GetInstance().StartCoroutine(glo_Main.GetInstance().m_SC_Pool.f_LoadSC(aBytes));        
    }


    public override void f_Execute()
    {
        if (glo_Main.GetInstance().m_SC_Pool.f_CheckLoadSuc())
        {
            if (GloData.glo_iHaveNewApk == 1)
            {
                f_SetComplete((int)EM_ResManagerStatic.DownNewApk);
            }
            else
            {

                f_SetComplete((int)EM_ResManagerStatic.UpdateRes);
            }
        }
    }

    

}