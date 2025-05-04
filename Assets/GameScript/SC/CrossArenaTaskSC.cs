using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CrossArenaTaskSC : NBaseSC {
	public CrossArenaTaskSC() {
		Create("CrossArenaTaskDT", true);
	}

	public override void f_LoadSCForData(string strData) {
		DispSaveData(strData);
	}

	private void DispSaveData(string ppSQL) {
		string[] ttt = ppSQL.Split(new string[] { "1#QW"}, System.StringSplitOptions.None);
		CrossArenaTaskDT DataDT;
		string[] tData;	string[] tFoddScData = ttt[1].Split(new string[] { "|" },System.StringSplitOptions.None);
		for (int i = 0; i < tFoddScData.Length; i++) {
			try {
				if (tFoddScData[i] == "") {
					MessageBox.DEBUG(m_strRegDTName + "脚本存在空记录, " + i);
					continue;
				}
				tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
				int a = 0;
				DataDT = new CrossArenaTaskDT();
				DataDT.iId = ccMath.atoi(tData[a++]);
				if (DataDT.iId <= 0) {
					MessageBox.ASSERT("Id错误");
				}
				DataDT.szTaskName = tData[a++];
				DataDT.iCondition = ccMath.atoi(tData[a++]);
				DataDT.szAward = tData[a++];
				SaveItem(DataDT);
			}
			catch {
				MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
				continue;
			}
		}
	}

	
}
