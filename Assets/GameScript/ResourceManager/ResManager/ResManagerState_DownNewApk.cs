using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class ResManagerState_DownNewApk : ccMachineStateBase
{
    static EM_ResManagerStatic m_EM_AIStatic = EM_ResManagerStatic.DownNewApk;
    private string _strResourceMd5;
    WWW w = null;
    bool _bInitWWW = false;
    private bool _bSaveCatchBuf = false;

    public ResManagerState_DownNewApk()
            : base((int)m_EM_AIStatic)
        {

    }

    public override void f_Enter(object Obj)
    {
        //空等待，加载页初始时检测
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DownLoadNewApk, UIMessageDef.UI_OPEN);
    }
    


}