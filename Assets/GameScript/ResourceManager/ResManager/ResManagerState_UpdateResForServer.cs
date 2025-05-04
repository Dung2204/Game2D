using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.IO;
using System.Collections.Generic;

public class ResManagerState_UpdateResForServer : ccMachineStateBase
{
    private string _strLocalResource;
    private string _strLocalResourceTmp;
    private ResourceCatch _ResourceCatch = new ResourceCatch();
    public DownloadManager _DownloadManager = new DownloadManager();

    static EM_ResManagerStatic m_EM_AIStatic = EM_ResManagerStatic.UpdateResForServer;

    public ResManagerState_UpdateResForServer()
            : base((int)m_EM_AIStatic)
        {
        _strLocalResource = ResourceTools.f_GetLocalMainPath() + "/Resource.csv";
        _strLocalResourceTmp = _strLocalResource + "tmp";

    }

    public override void f_Enter(object Obj)
    {
MessageBox.DEBUG("Update data");
        StartUpdateServerFile();
    }
    
    private int _iNum;
    private int _iMaxNum;
    private void StartUpdateServerFile()
    {
        List<ResourceDT> aList = glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetNeedUpdateFileList();
        if (aList.Count == 0)
        {
            CallBack_UpdateServerFileComplete(null);
        }
        else
        {
            _iMaxNum = _iNum = aList.Count;
MessageBox.DEBUG("Ready to update resources." + _iMaxNum);
            _DownloadManager.f_RegCompleteCallBack(CallBack_UpdateServerFileComplete);
            for (int i = 0; i < aList.Count; i++)
            {
                _DownloadManager.f_RegDownload(ResourceTools.f_GetServerMainPath() + "/" + aList[i].strPath, ResourceTools.f_GetLocalMainPath() + "/" + aList[i].strPath, CallBack_DownLoading, aList[i]);
            }
        }
    }

    /// <summary>
    /// 更新资源成功
    /// </summary>
    /// <param name="Obj"></param>
    private void CallBack_UpdateServerFileComplete(object Obj)
    {
        string strResourceMd5 = ((ResManagerState_Ver)f_GetOtherStateBase((int)EM_ResManagerStatic.Ver)).f_GetResourceMD5();
        glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_UpdateData2Local();
        File.Copy(_strLocalResourceTmp, _strLocalResource, true);
        File.Delete(_strLocalResourceTmp);
        PlayerPrefs.SetString("ResMD5", strResourceMd5);

        //去掉自动加载基本资源改由异步，以节省内存开销
        // _ResourceCatch.f_StartLoad(CallBack_ResourceCatchComplete);
MessageBox.DEBUG("Original resource load successful");
        f_SetComplete((int)EM_ResManagerStatic.Login);
    }
        
    private void CallBack_DownLoading(object Obj)
    {
        ResourceDT tResourceDT = (ResourceDT)Obj;
        glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_SaveDownLoadSucResource(tResourceDT);
        
        _iNum--;
        //MessageBox.DEBUG("AAAA: " + _iNum + "/" + _iMaxNum);
        //glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_RESOURCELOADPROGRESS, _iNum + ";" + _iMaxNum);

        //string ppSQL = "60;100;资源更新...60/100";
        //glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_RESOURCELOADPROGRESS, ppSQL);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_RESOURCELOADPROGRESS, _iNum + ";" + _iMaxNum);
    }
    
    public override void f_Execute()
    {
        base.f_Execute();

        _ResourceCatch.f_Update();
        _DownloadManager.f_Update();
    }

}
