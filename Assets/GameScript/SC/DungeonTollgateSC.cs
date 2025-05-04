using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DungeonTollgateSC : NBaseSC {
	public DungeonTollgateSC() {
		Create("DungeonTollgateDT", true);
	}

	public override void f_LoadSCForData(string strData) {
		DispSaveData(strData);
	}

	private void DispSaveData(string ppSQL) {
		string[] ttt = ppSQL.Split(new string[] { "1#QW"}, System.StringSplitOptions.None);
		DungeonTollgateDT DataDT;
		string[] tData;	string[] tFoddScData = ttt[1].Split(new string[] { "|" },System.StringSplitOptions.None);
		for (int i = 0; i < tFoddScData.Length; i++) {
			try {
				if (tFoddScData[i] == "") {
MessageBox.DEBUG(m_strRegDTName + "String with empty record, " + i);
					continue;
				}
				tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
				int a = 0;
				DataDT = new DungeonTollgateDT();
				DataDT.iId = ccMath.atoi(tData[a++]);
				if (DataDT.iId <= 0) {
MessageBox.ASSERT("Error Id");
				}
				DataDT._szTollgateName = tData[a++];
				DataDT._szTollgateDesc = tData[a++];
				DataDT._szModeDialog = tData[a++];
				DataDT.iModeId = ccMath.atoi(tData[a++]);
				DataDT.szMonsterId = tData[a++];
				DataDT.iEnergyCost = ccMath.atoi(tData[a++]);
				DataDT.iCountLimit = ccMath.atoi(tData[a++]);
				DataDT.iPond = ccMath.atoi(tData[a++]);
				DataDT.iBoxId = ccMath.atoi(tData[a++]);
				DataDT.iFightValue = ccMath.atoi(tData[a++]);
				DataDT.iSceneId = ccMath.atoi(tData[a++]);
				DataDT.iFirstWinSycee = ccMath.atoi(tData[a++]);
				DataDT.iPlot = ccMath.atoi(tData[a++]);
				DataDT.szPlotMonsterId = tData[a++];
				DataDT.szPlotEnemyMonsterId = tData[a++];
				DataDT.iDungeonChapter = ccMath.atoi(tData[a++]);
				SaveItem(DataDT);
			}
			catch {
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
				continue;
			}
		}
	}

	
}
