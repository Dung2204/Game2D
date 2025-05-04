using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TrialTowerSC : NBaseSC {
	public TrialTowerSC() {
		Create("TrialTowerDT", true);
	}

	public override void f_LoadSCForData(string strData) {
		DispSaveData(strData);
	}

	private void DispSaveData(string ppSQL) {
		string[] ttt = ppSQL.Split(new string[] { "1#QW"}, System.StringSplitOptions.None);
		TrialTowerDT DataDT;
		string[] tData;	string[] tFoddScData = ttt[1].Split(new string[] { "|" },System.StringSplitOptions.None);
		for (int i = 0; i < tFoddScData.Length; i++) {
			try {
				if (tFoddScData[i] == "") {
MessageBox.DEBUG(m_strRegDTName + "String with empty record, " + i);
					continue;
				}
				tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
				int a = 0;
				DataDT = new TrialTowerDT();
				DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT.szTollgateName = tData[a++];
                DataDT.iModeId = ccMath.atoi(tData[a++]);
                DataDT.szMonsterId = tData[a++];
                DataDT.iFightValue = ccMath.atoi(tData[a++]);
                DataDT.iPond = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0) {
MessageBox.ASSERT("Error Id");
				}
				SaveItem(DataDT);
			}
			catch {
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
				continue;
			}
		}
	}

	
}
