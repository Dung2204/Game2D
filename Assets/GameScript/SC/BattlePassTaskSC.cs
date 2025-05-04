using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BattlePassTaskSC : NBaseSC {
	public BattlePassTaskSC() {
		Create("BattlePassTaskDT", true);
	}

	public override void f_LoadSCForData(string strData) {
		DispSaveData(strData);
	}

	private void DispSaveData(string ppSQL) {
		string[] ttt = ppSQL.Split(new string[] { "1#QW"}, System.StringSplitOptions.None);
		BattlePassTaskDT DataDT;
		string[] tData;	string[] tFoddScData = ttt[1].Split(new string[] { "|" },System.StringSplitOptions.None);
		for (int i = 0; i < tFoddScData.Length; i++) {
			try {
				if (tFoddScData[i] == "") {
					MessageBox.DEBUG(m_strRegDTName + "脚本存在空记录, " + i);
					continue;
				}
				tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
				int a = 0;
				DataDT = new BattlePassTaskDT();
				DataDT.iId = ccMath.atoi(tData[a++]);
				if (DataDT.iId <= 0) {
					MessageBox.ASSERT("Id错误");
				}
				DataDT.iType = ccMath.atoi(tData[a++]);
				DataDT.iOpen = ccMath.atoi(tData[a++]);
				DataDT.iClose = ccMath.atoi(tData[a++]);
				DataDT.iIconId = ccMath.atoi(tData[a++]); 
				DataDT._szDesc = tData[a++];
				DataDT.iCondition = ccMath.atoi(tData[a++]); 
				DataDT.iConditionParam1 = ccMath.atoi(tData[a++]);
				DataDT.iScore1 = ccMath.atoi(tData[a++]);
				DataDT.iConditionParam2 = ccMath.atoi(tData[a++]);
				DataDT.iScore2 = ccMath.atoi(tData[a++]);
				DataDT.iConditionParam3 = ccMath.atoi(tData[a++]);
				DataDT.iScore3 = ccMath.atoi(tData[a++]);
				DataDT.iConditionParam4 = ccMath.atoi(tData[a++]);
				DataDT.iScore4 = ccMath.atoi(tData[a++]);
				DataDT.iConditionParam5 = ccMath.atoi(tData[a++]);
				DataDT.iScore5 = ccMath.atoi(tData[a++]);
				DataDT.iConditionParam6 = ccMath.atoi(tData[a++]);
				DataDT.iScore6 = ccMath.atoi(tData[a++]);
				DataDT.iConditionParam7 = ccMath.atoi(tData[a++]);
				DataDT.iScore7 = ccMath.atoi(tData[a++]);
				DataDT.iGotoId = ccMath.atoi(tData[a++]);
				SaveItem(DataDT);
			}
			catch {
				MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
				continue;
			}
		}
	}

	
}
