using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGiftSC : NBaseSC {
	public LevelGiftSC() {
		Create("LevelGiftDT", true);
	}

	public override void f_LoadSCForData(string strData) {
		DispSaveData(strData);
	}

	private void DispSaveData(string ppSQL) {
		string[] ttt = ppSQL.Split(new string[] { "1#QW"}, System.StringSplitOptions.None);
		LevelGiftDT DataDT;
		string[] tData;	string[] tFoddScData = ttt[1].Split(new string[] { "|" },System.StringSplitOptions.None);
		for (int i = 0; i < tFoddScData.Length; i++) {
			try {
				if (tFoddScData[i] == "") {
					MessageBox.DEBUG(m_strRegDTName + "脚本存在空记录, " + i);
					continue;
				}
				tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
				int a = 0;
				DataDT = new LevelGiftDT();
				DataDT.iId = ccMath.atoi(tData[a++]);
				if (DataDT.iId <= 0) {
					MessageBox.ASSERT("Id错误");
				}
				DataDT.iType = ccMath.atoi(tData[a++]);
				DataDT.iEventTime = ccMath.atoi(tData[a++]);
				DataDT.iParam1 = ccMath.atoi(tData[a++]);
				DataDT.szParam2 = tData[a++];
				DataDT.iParam3 = ccMath.atoi(tData[a++]);
				DataDT.iPayNum = ccMath.atoi(tData[a++]);
				DataDT._szPayDesc = tData[a++];
				DataDT.szAward = tData[a++];
				DataDT.iPayCount = ccMath.atoi(tData[a++]);
				DataDT.iHour = ccMath.atoi(tData[a++]);
				DataDT.iMaxNum = ccMath.atoi(tData[a++]);
				DataDT.szProductID_web = tData[a++];
				DataDT.szProductID_ios = tData[a++];
				DataDT.szProductID_android = tData[a++];
				DataDT.szPayShow = tData[a++];
				SaveItem(DataDT);
			}
			catch {
				MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
				continue;
			}
		}
	}

	public List<NBaseSCDT> f_GetSCByEventTimeId(int iId)
	{
		return f_GetAll().Where(o => o is LevelGiftDT data && data.iEventTime == iId).ToList();
	}
}
