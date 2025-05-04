using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BattlePassRankingSC : NBaseSC {
	public BattlePassRankingSC() {
		Create("BattlePassRankingDT", true);
	}

	public override void f_LoadSCForData(string strData) {
		DispSaveData(strData);
	}

	private void DispSaveData(string ppSQL) {
		string[] ttt = ppSQL.Split(new string[] { "1#QW"}, System.StringSplitOptions.None);
		BattlePassRankingDT DataDT;
		string[] tData;	string[] tFoddScData = ttt[1].Split(new string[] { "|" },System.StringSplitOptions.None);
		for (int i = 0; i < tFoddScData.Length; i++) {
			try {
				if (tFoddScData[i] == "") {
					MessageBox.DEBUG(m_strRegDTName + "脚本存在空记录, " + i);
					continue;
				}
				tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
				int a = 0;
				DataDT = new BattlePassRankingDT();
				DataDT.iId = ccMath.atoi(tData[a++]);
				if (DataDT.iId <= 0) {
					MessageBox.ASSERT("Id错误");
				}
				DataDT.iRankDown = ccMath.atoi(tData[a++]);
				DataDT.iRankUp = ccMath.atoi(tData[a++]);
				DataDT.szAward = tData[a++];
				SaveItem(DataDT);
			}
			catch {
				MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
				continue;
			}
		}
	}

	public BattlePassRankingDT GetAwardDTByRank(int iRank)
	{
		List<NBaseSCDT> nBaseSCDTs = f_GetAll();
		for (int i = 0; i < nBaseSCDTs.Count; i++)
		{
			BattlePassRankingDT battlePassRankingDT = (BattlePassRankingDT)nBaseSCDTs[i];
			if (battlePassRankingDT != null)
			{
				int first = battlePassRankingDT.iRankDown;
				int end = battlePassRankingDT.iRankUp;

				if (first <= iRank && iRank <= end)
					return battlePassRankingDT;

			}
		}
		return null;
	}

}
