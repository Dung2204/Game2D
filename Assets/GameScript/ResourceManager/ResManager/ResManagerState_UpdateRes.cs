using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.IO;
using System.Collections.Generic;

public class ResManagerState_UpdateRes : ccMachineStateBase
{
    private bool _bUpdateResource;
    private string _strLocalResource;
    private string _strLocalResourceTmp;
    private ResourceCatch _ResourceCatch = new ResourceCatch();
    public DownloadManager _DownloadManager = new DownloadManager();
    //private bool extractRes = false;
    static EM_ResManagerStatic m_EM_AIStatic = EM_ResManagerStatic.UpdateRes;

    public ResManagerState_UpdateRes()
            : base((int)m_EM_AIStatic)
    {
        _strLocalResource = ResourceTools.f_GetLocalMainPath() + "/Resource.csv";
        _strLocalResourceTmp = _strLocalResource + "tmp";
        //PlayerPrefs.SetInt("extractRes", 0);

    }

    public override void f_Enter(object Obj)
    {
        int extractRes = PlayerPrefs.GetInt("extractRes");
MessageBox.DEBUG("Update Res");
        string strResourceMd5 = ((ResManagerState_Ver)f_GetOtherStateBase((int)EM_ResManagerStatic.Ver)).f_GetResourceMD5();
#if UNITY_IPHONE
        if (!ccFile.f_ExistsFile(_strLocalResource))
        {
            //MessageBox.DEBUG("Thuc hien xu ly copy local resource first");
            MessageBox.DEBUG(Application.streamingAssetsPath + "/" + GloData.glo_ProName + "/");
            MessageBox.DEBUG(Application.persistentDataPath + "/" + GloData.glo_ProName + "/");
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/" + GloData.glo_ProName + "/");
            FileInfo[] files = directoryInfo.GetFiles();
            for (int i=0;i<files.Length;i++)
            {
                System.IO.File.Copy(files[i].FullName, Application.persistentDataPath + "/" + GloData.glo_ProName + "/" + files[i].Name,true);
            }
            
            //MessageBox.DEBUG("Hoan tat copy local resource");
        }
//TsuCode - aab full resources 
#elif UNITY_ANDROID

         //MessageBox.DEBUG("Thuc hien xu ly copy local resource first Cho Android tsu" + Application.persistentDataPath);
         //   if (File.Exists(Application.persistentDataPath + "/" + GloData.glo_ProName + "/" + "MG_Data.data") == false)
         //   {
         //       MessageBox.DEBUG("Xử lý handle");
         //       MessageBox.DEBUG("Xử lý handle data path" + extractRes);
         //   //copy tgz to directory where we can extract it
         //   if (!File.Exists(Application.persistentDataPath + "/" + GloData.glo_ProName + "/" + "Flag.txt"))
         //       if (extractRes==0)
         //           {
         //               MessageBox.DEBUG("Data.tgz Exits" + Application.streamingAssetsPath);
         //               WWW www = new WWW(Application.streamingAssetsPath + "/Data.tgz");
         //               while (!www.isDone) { }
         //               System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + GloData.glo_ProName + "/" + "Data.tgz", www.bytes);
         //           System.IO.File.WriteAllText(Application.persistentDataPath + "/" + GloData.glo_ProName + "/" + "Flag.txt","1");
         //           //extract it
         //           Utility_SharpZipCommands.ExtractTGZ(Application.persistentDataPath + "/" + GloData.glo_ProName + "/" + "Data.tgz", Application.persistentDataPath + "/" + GloData.glo_ProName + "/");
         //               //delete tgz
         //               File.Delete(Application.persistentDataPath + "/" + GloData.glo_ProName + "/" + "Data.tgz");
         //               PlayerPrefs.SetInt("extractRes", 1);
         //           extractRes = PlayerPrefs.GetInt("extractRes");
         //           MessageBox.DEBUG("Xử lý handle Done " + extractRes);
         //           }
         //   }
         //   else
         //   {
         //       MessageBox.DEBUG("MG_Data.data does exist, will not extract default data.");
         //   }
#endif
        f_StartLoad(strResourceMd5);
    }
    
    private void f_StartLoad(string strResourceMd5)
    {
        _bUpdateResource = false;
        if (File.Exists(_strLocalResource))
        {
            string strFileTxtData;
            byte[] bBuf = null;
            if (DownloadManager.f_LoadLocalFile2Byte(_strLocalResource, out bBuf))
            {
                strFileTxtData = System.Text.Encoding.Default.GetString(bBuf);
                glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_LoadSCForData(strFileTxtData);
            }
        }
        else
        {
MessageBox.DEBUG("The current platform needs to update the resource list because tmp file cannot be found " + _strLocalResource);
            _bUpdateResource = true;
        }

        if (!_bUpdateResource)
        {
            if (CheckNeedUpdateResource(strResourceMd5))
            {
                _bUpdateResource = true;
            }
        }

        if (_bUpdateResource)
        {//true 需要更新 
MessageBox.DEBUG("The current platform needs to update the resource list");
            LoadServerResource();
        }
        else
        {//不需要更新
MessageBox.DEBUG("The current platform does not need to update the resource list");
            f_SetComplete((int)EM_ResManagerStatic.Login);
        }
    }

    private void LoadServerResource()
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowLogoPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResLoadPage, UIMessageDef.UI_OPEN, true);
        string strRemoteResource = ResourceTools.f_GetServerMainPath() + "/Resource.csv";
        _DownloadManager.f_RegDownload(strRemoteResource, _strLocalResourceTmp, CallBack_DownLoadResource, null);
    }
    
    private void CallBack_DownLoadResource(object Obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)Obj;
        if (teMsgOperateResult == eMsgOperateResult.OR_Succeed)
        {
            if (_bUpdateResource && GloData.glo_iPackType == 1)
            {
                //整包资源需要更新的情况
                TextAsset __Ab = Resources.Load<TextAsset>("Resource");
                if (__Ab == null)
                {
MessageBox.ASSERT("Resource Load Failed for Entire Resource Pack");
                }
                else
                {
                    glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_LoadSCForData(__Ab.text);
                }
            }

            string strFileTxtData;
            byte[] bBuf = null;
            if (DownloadManager.f_LoadLocalFile2Byte(_strLocalResourceTmp, out bBuf))
            {
                strFileTxtData = System.Text.Encoding.Default.GetString(bBuf);
                glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_LoadServerResourceData(strFileTxtData);
            }
            else
            {
MessageBox.ASSERT("Resource Load Failed");
            }

MessageBox.DEBUG("Original Resource List Loaded Successfully");
            f_SetComplete((int)EM_ResManagerStatic.UpdateResForServer);
        }
        else
        {
MessageBox.ASSERT("Remote resource load failed");
        }

    }
    
    /// <summary>
    /// true 需要更新  false 不需要更新
    /// </summary>
    /// <param name="strResourceMd5"></param>
    /// <returns></returns>
    private bool CheckNeedUpdateResource(string strResourceMd5)
    {
        string strResMD5 = PlayerPrefs.GetString("ResMD5");
        bool bRet = true;
        if (strResourceMd5.Equals(strResMD5))
        {
            bRet = false;
        }
        return bRet;
    }

    public override void f_Execute()
    {
        base.f_Execute();

        _ResourceCatch.f_Update();
        _DownloadManager.f_Update();
    }
    

}
