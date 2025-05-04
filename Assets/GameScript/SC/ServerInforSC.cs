
//============================================
//
//    ServerInfor来自ServerInfor.xlsx文件自动生成脚本
//    2018/1/22 11:03:22
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ServerInforSC : NBaseSC
{
	bool m_loadDebugServer;

    public ServerInforSC()
    {
        Create("ServerInforDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
		if(!m_loadDebugServer)
			DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        ServerInforDT DataDT;
        string[] tData;
        string[] tFoddScData = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
MessageBox.DEBUG(m_strRegDTName + "String with empty record, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new ServerInforDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szName = tData[a++];
                DataDT.szChannel = tData[a++];
                //DataDT.szResIP = tData[a++];
                //DataDT.iResPort = ccMath.atoi(tData[a++]);
                DataDT.szIP = tData[a++];
                DataDT.iPort = ccMath.atoi(tData[a++]);
                //DataDT.szVer = tData[a++];
                DataDT.iServerId = ccMath.atoi(tData[a++]);
                //DataDT.iPayType = ccMath.atoi(tData[a++]);
                DataDT.iServState = ccMath.atoi(tData[a++]);
                //DataDT.iTimes = ccMath.atoi(tData[a++]);
                DataDT.szLockChanel = tData[a++];
                DataDT.szAutoOpenTime = tData[a++];
                SaveItem(DataDT);
            }
            catch
            {
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
                continue;
            }
        }
    }

	public void f_LoadDebugServerData(string ip,int port)
	{
		//m_loadDebugServer = true;

		ServerInforDT DataDT = new ServerInforDT();
		DataDT.iId = 100;
		if (DataDT.iId <= 0)
		{
MessageBox.ASSERT("Error Id");
		}
DataDT.szName = "Internal test server";
		DataDT.szChannel = "L1";
		//DataDT.szResIP = tData[a++];
		//DataDT.iResPort = ccMath.atoi(tData[a++]);
		DataDT.szIP = ip;			// local:127.0.0.1 other:192.168.105.97/104    
		DataDT.iPort = port;        //网关服务器端口 local: 58500  other:38001
		//DataDT.szVer = tData[a++];
		DataDT.iServerId = 100;
		//DataDT.iPayType = ccMath.atoi(tData[a++]);
		DataDT.iServState = 2;
		//DataDT.iTimes = ccMath.atoi(tData[a++]);
		DataDT.szLockChanel = "";
		SaveItem(DataDT);
	}
}
