using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartDebugScreen:MonoBehaviour {

	private bool m_bUseGameServer;
	private string m_strGameServer;
	private int m_iGamePort;
	private bool m_bShowLog;

	private System.Action m_EndCallback;
	public void f_SetCallback(System.Action cb)
	{
		m_EndCallback = cb;
	}

	private void Awake()
	{
		GloData.glo_strHttpServer = PlayerPrefs.GetString(GloData.glo_ProName + ".httpServer", GloData.glo_strHttpServer);
		GloData.glo_strCDNServer = PlayerPrefs.GetString(GloData.glo_ProName + ".cdnServer", GloData.glo_strCDNServer);

		m_bUseGameServer = PlayerPrefs.GetInt(GloData.glo_ProName + ".useGameServer", 1) == 1;
		m_strGameServer = PlayerPrefs.GetString(GloData.glo_ProName + ".gameServer", GloData.glo_strHttpServer);
		m_iGamePort = PlayerPrefs.GetInt(GloData.glo_ProName + ".gamePort", 38001);

		GloData.glo_StarGuidance= PlayerPrefs.GetInt(GloData.glo_ProName + ".starGuidance", 1) == 1;
	}

	private void OnGUI()
	{
		GUI.skin = Resources.Load<GUISkin>("debugScreenSkin");

		//web服务器IP设置
		GUILayout.BeginHorizontal();		
		GUILayout.Label("httpServer:");
		GloData.glo_strHttpServer=GUILayout.TextField(GloData.glo_strHttpServer);
		GUILayout.Space(100);
		GUILayout.Label("cdnServer:");
		GloData.glo_strCDNServer=GUILayout.TextField(GloData.glo_strCDNServer);
		GUILayout.EndHorizontal();

		//游戏服务器IP和端口设置
		GUILayout.BeginHorizontal();
		m_bUseGameServer = GUILayout.Toggle(m_bUseGameServer, "customGameServer");
		GUILayout.EndHorizontal();
		if (m_bUseGameServer)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("gameIP:");
			m_strGameServer = GUILayout.TextField(m_strGameServer);
			GUILayout.Space(100);
			GUILayout.Label("gamePort:");
			m_iGamePort = int.Parse(GUILayout.TextField(m_iGamePort.ToString()));
			GUILayout.EndHorizontal();
		}

		//新手引导
		GUILayout.BeginHorizontal();
		GloData.glo_StarGuidance = GUILayout.Toggle(GloData.glo_StarGuidance, "guidance");
		GUILayout.EndHorizontal();

		//Start
		GUILayout.BeginVertical();
		if (GUILayout.Button("Start"))
		{
			PlayerPrefs.SetString(GloData.glo_ProName + ".httpServer", GloData.glo_strHttpServer);
			PlayerPrefs.SetString(GloData.glo_ProName + ".cdnServer", GloData.glo_strCDNServer);
			if (m_bUseGameServer)
			{
				glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_LoadDebugServerData(m_strGameServer, m_iGamePort);
			}
			ResourceTools.f_InitURL();

			PlayerPrefs.SetInt(GloData.glo_ProName + ".useGameServer", m_bUseGameServer ? 1 : 0);
			PlayerPrefs.SetString(GloData.glo_ProName + ".gameServer", m_strGameServer);
			PlayerPrefs.SetInt(GloData.glo_ProName + ".gamePort", m_iGamePort);

			PlayerPrefs.SetInt(GloData.glo_ProName + ".starGuidance", GloData.glo_StarGuidance ? 1 : 0);
			m_EndCallback();
		}
		GUILayout.EndVertical();
	}
}
