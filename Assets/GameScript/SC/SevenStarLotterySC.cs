using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SevenStarLotterySC : NBaseSC {
	public SevenStarLotterySC() {
		Create("SevenStarLotteryDT", true);
	}

	public override void f_LoadSCForData(string strData) {
		DispSaveData(strData);
	}

	private void DispSaveData(string ppSQL) {
		string[] ttt = ppSQL.Split(new string[] { "1#QW"}, System.StringSplitOptions.None);
		SevenStarLotteryDT DataDT;
		string[] tData;	string[] tFoddScData = ttt[1].Split(new string[] { "|" },System.StringSplitOptions.None);
		for (int i = 0; i < tFoddScData.Length; i++) {
			try {
				if (tFoddScData[i] == "") {
MessageBox.DEBUG(m_strRegDTName + "String with empty record, " + i);
					continue;
				}
				tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
				int a = 0;
				DataDT = new SevenStarLotteryDT();
				DataDT.iId = ccMath.atoi(tData[a++]);
				if (DataDT.iId <= 0) {
MessageBox.ASSERT("Error Id");
				}
				DataDT.iAwardType = ccMath.atoi(tData[a++]);
				DataDT.iAwardId = ccMath.atoi(tData[a++]);
				DataDT.iAwardNum = ccMath.atoi(tData[a++]);
				DataDT.iOpenTimes = ccMath.atoi(tData[a++]);
				DataDT.ibClear = ccMath.atoi(tData[a++]);
				DataDT.iWeight0 = ccMath.atoi(tData[a++]);
				DataDT.iWeight1 = ccMath.atoi(tData[a++]);
				DataDT.iWeight2 = ccMath.atoi(tData[a++]);
				DataDT.iWeight3 = ccMath.atoi(tData[a++]);
				DataDT.iWeight4 = ccMath.atoi(tData[a++]);
				DataDT.iWeight5 = ccMath.atoi(tData[a++]);
				DataDT.ibBoardcast = ccMath.atoi(tData[a++]);
				SaveItem(DataDT);
			}
			catch {
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
				continue;
			}
		}
	}

	
}
