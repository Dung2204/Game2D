using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.IO;

public class ResManagerState_UpdateResForLocal : ccMachineStateBase
{
    static EM_ResManagerStatic m_EM_AIStatic = EM_ResManagerStatic.UpdateResForLocal;
    private string _strLocalResource;
    private bool _bUpdateResource;
    public ResManagerState_UpdateResForLocal()
            : base((int)m_EM_AIStatic)
        {
        _strLocalResource = ResourceTools.f_GetLocalMainPath() + "/Resource.csv";
    }

    public override void f_Enter(object Obj)
    {
        string strResourceMd5 = ((ResManagerState_Ver)f_GetOtherStateBase((int)EM_ResManagerStatic.Ver)).f_GetResourceMD5();
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
            _bUpdateResource = true;
        }

        if (!_bUpdateResource)
        {
            if (CheckNeedUpdateResource(strResourceMd5))
            {
                _bUpdateResource = true;
            }
        }

MessageBox.DEBUG("Update local resources");
        string strLocalResource = ResourceTools.f_GetLocalMainPath() + "/Resource.csv";
        TextAsset __Ab = Resources.Load<TextAsset>("Resource"); 
        if (__Ab == null)
        {
MessageBox.ASSERT("Local resource load failed" + strLocalResource);
        }
        else
        {
            glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_LoadSCForData(__Ab.text);
        }
        CallBack_LoadGameResourceComplete(null);
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
    }

    private void CallBack_LoadGameResourceComplete(object Obj)
    {
MessageBox.DEBUG("Loaded Successfully");
        f_SetComplete((int)EM_ResManagerStatic.Login);
    }


    private void ShowDownLoadPage()
    {
        //LoginPageMain tLoginPageMain = (LoginPageMain)FindObjectOfType(typeof(LoginPageMain));
        //tLoginPageMain.f_StarLoginPage();

    }



}
