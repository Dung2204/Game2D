using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;

public class ResManagerState_LoadSC : ccMachineStateBase
{
    static EM_ResManagerStatic m_EM_AIStatic = EM_ResManagerStatic.LoadSC;
    
    WWW w = null;
    private bool _bUpdate = false;

    public ResManagerState_LoadSC()
        : base((int)m_EM_AIStatic)
    {

    }

    public override void f_Enter(object Obj)
    {
        _bUpdate = (bool)Obj;
        if (_bUpdate)
        {
MessageBox.DEBUG("Update script");
            string strUrl = "";

#if UNITY_WEBPLAYER
            strUrl = GloData.glo_strLoadAllSC + "ccData_W.bytes"  + "?v=" + UnityEngine.Random.Range(0, 100000);
#elif UNITY_ANDROID
            strUrl = GloData.glo_strLoadAllSC + "ccData_A.bytes" + "?v=" + UnityEngine.Random.Range(0, 100000);
#elif UNITY_IPHONE
            strUrl = GloData.glo_strLoadAllSC + "ccData_I.bytes"  + "?v=" + UnityEngine.Random.Range(0, 100000);
#else
            strUrl = GloData.glo_strLoadAllSC + "ccData_W.bytes"  + "?v=" + UnityEngine.Random.Range(0, 100000);
#endif
            //long starttime = DateTime.Now.Ticks;
            //WWWForm form = new WWWForm();
            //form.AddField("iUserId", 1);
            w = new WWW(strUrl);
        }
        else
        {
            MessageBox.DEBUG(Application.persistentDataPath);
MessageBox.DEBUG("Load script");
            LoadSuc(ccFile.f_ReadFileForByte(Application.persistentDataPath + "/" + GloData.glo_ProName + "/", "ccData.xlscc"));
        }
    }


    public override void f_Execute()
    {
        if (w == null)
        {
            return;
        }
        if (!w.isDone)
            return;

        if (w.error != null)
        {
MessageBox.DEBUG("Network Error");
            LoadFail();
        }
        else if (w.text.Length < 4)
        {
MessageBox.DEBUG("Network error 2");
            LoadFail();
        }
        else
        {
            LoadSuc(w.bytes);
        }
        w.Dispose();
        w = null;

MessageBox.DEBUG("Loading script successfully");
    }


    private void LoadFail()
    {
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEMESSAGEBOX, (int)eMsgOperateResult.OR_Error_WIFIConnectTimeOut);
    }

    private void LoadSuc(byte[] aBytes)
    {
        if (_bUpdate)
        {
            ccFile.f_SaveFileForByte(Application.persistentDataPath + "/" + GloData.glo_ProName + "/", "ccData.xlscc", aBytes);
            string strServerVer = PlayerPrefs.GetString("RVer");
            PlayerPrefs.SetString("Ver", strServerVer);
MessageBox.DEBUG("Updated version：" + strServerVer);
        }
        f_SetComplete((int)EM_ResManagerStatic.DispSC, aBytes);
    }

 


}
