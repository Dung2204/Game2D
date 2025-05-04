using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AwakenCardSC : NBaseSC {
	public AwakenCardSC() {
		Create("AwakenCardDT", true);
	}

	public override void f_LoadSCForData(string strData) {
		DispSaveData(strData);
	}

	private void DispSaveData(string ppSQL) {
		string[] ttt = ppSQL.Split(new string[] { "1#QW"}, System.StringSplitOptions.None);
		AwakenCardDT DataDT;
		string[] tData;	string[] tFoddScData = ttt[1].Split(new string[] { "|" },System.StringSplitOptions.None);
		for (int i = 0; i < tFoddScData.Length; i++) {
			try {
				if (tFoddScData[i] == "") {
MessageBox.DEBUG(m_strRegDTName + "String with empty record, " + i);
					continue;
				}
				tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
				int a = 0;
				DataDT = new AwakenCardDT();
				DataDT.iId = ccMath.atoi(tData[a++]);
				if (DataDT.iId <= 0) {
MessageBox.ASSERT("Error Id");
				}
				DataDT.iCardNeedLv = ccMath.atoi(tData[a++]);
				DataDT.iNeedMoeny = ccMath.atoi(tData[a++]);
				DataDT.iNeedGoods = ccMath.atoi(tData[a++]);
				DataDT.iNeedCard = ccMath.atoi(tData[a++]);
				DataDT.iEquipID1 = ccMath.atoi(tData[a++]);
				DataDT.iEquipID2 = ccMath.atoi(tData[a++]);
				DataDT.iEquipID3 = ccMath.atoi(tData[a++]);
				DataDT.iEquipID4 = ccMath.atoi(tData[a++]);
				DataDT.iAddProId1 = ccMath.atoi(tData[a++]);
				DataDT.iAddPro1 = ccMath.atoi(tData[a++]);
				DataDT.iAddProId2 = ccMath.atoi(tData[a++]);
				DataDT.iAddPro2 = ccMath.atoi(tData[a++]);
				DataDT.iAddProId3 = ccMath.atoi(tData[a++]);
				DataDT.iAddPro3 = ccMath.atoi(tData[a++]);
				DataDT.iAddProId4 = ccMath.atoi(tData[a++]);
				DataDT.iAddPro4 = ccMath.atoi(tData[a++]);
				DataDT._szDesc = tData[a++];
				DataDT.iStarProId1 = ccMath.atoi(tData[a++]);
				DataDT.iStarPro1 = ccMath.atoi(tData[a++]);
				DataDT.iStarProId2 = ccMath.atoi(tData[a++]);
				DataDT.iStarPro2 = ccMath.atoi(tData[a++]);
				DataDT.iStarProId3 = ccMath.atoi(tData[a++]);
				DataDT.iStarPro3 = ccMath.atoi(tData[a++]);
				DataDT.iStarProId4 = ccMath.atoi(tData[a++]);
				DataDT.iStarPro4 = ccMath.atoi(tData[a++]);
				SaveItem(DataDT);
			}
			catch {
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
				continue;
			}
		}
	}

	
}
