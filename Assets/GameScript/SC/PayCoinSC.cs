using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PayCoinSC : NBaseSC {
	public PayCoinSC() {
		Create("PayCoinDT", true);
	}

	public override void f_LoadSCForData(string strData) {
		DispSaveData(strData);
	}

	private void DispSaveData(string ppSQL) {
		string[] ttt = ppSQL.Split(new string[] { "1#QW"}, System.StringSplitOptions.None);
		PayCoinDT DataDT;
		string[] tData;	string[] tFoddScData = ttt[1].Split(new string[] { "|" },System.StringSplitOptions.None);
		for (int i = 0; i < tFoddScData.Length; i++) {
			try {
				if (tFoddScData[i] == "") {
					MessageBox.DEBUG(m_strRegDTName + "脚本存在空记录, " + i);
					continue;
				}
				tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
				int a = 0;
				DataDT = new PayCoinDT();
				DataDT.iId = ccMath.atoi(tData[a++]);
				if (DataDT.iId <= 0) {
					MessageBox.ASSERT("Id错误");
				}
				DataDT.szPackageID = tData[a++];
				DataDT.szPrice = tData[a++];
				DataDT.iPoint = ccMath.atoi(tData[a++]);
				DataDT.szName = tData[a++];
				DataDT.szAndroid = tData[a++];
				DataDT.szIOS = tData[a++];
				DataDT.iShow = ccMath.atoi(tData[a++]);

				if(DataDT.iShow==1)
					SaveItem(DataDT);
			}
			catch {
				MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
				continue;
			}
		}
	}

	
}
