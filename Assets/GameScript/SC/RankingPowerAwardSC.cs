using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingPowerAwardSC : NBaseSC {
	public RankingPowerAwardSC() {
		Create("RankingPowerAwardDT", true);
	}

	public override void f_LoadSCForData(string strData) {
		DispSaveData(strData);
	}

	private void DispSaveData(string ppSQL) {
		string[] ttt = ppSQL.Split(new string[] { "1#QW"}, System.StringSplitOptions.None);
		RankingPowerAwardDT DataDT;
		string[] tData;	string[] tFoddScData = ttt[1].Split(new string[] { "|" },System.StringSplitOptions.None);
		for (int i = 0; i < tFoddScData.Length; i++) {
			try {
				if (tFoddScData[i] == "") {
					MessageBox.DEBUG(m_strRegDTName + "脚本存在空记录, " + i);
					continue;
				}
				tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
				int a = 0;
				DataDT = new RankingPowerAwardDT();
				DataDT.iId = ccMath.atoi(tData[a++]);
				if (DataDT.iId <= 0) {
					MessageBox.ASSERT("Id错误");
				}
				DataDT.iEventTime = ccMath.atoi(tData[a++]);
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

	public Dictionary<int, List<NBaseSCDT>> dictsEvent = new Dictionary<int, List<NBaseSCDT>>();
	public List<NBaseSCDT> f_GetSCByEventTimeId(int iId)
	{
		return f_GetAll().Where(o => o is RankingPowerAwardDT data && data.iEventTime == iId).ToList();
	}

	public RankingPowerAwardDT GetAwardDTByRank(int iId ,int iRank)
    {
		//Debug.LogError(iId + "_" + iRank);
        if (dictsEvent.ContainsKey(iId))
        {
			List<NBaseSCDT> nBaseSCDTs = dictsEvent[iId];
            for (int i = 0; i < nBaseSCDTs.Count; i++)
            {
				RankingPowerAwardDT rankingPowerAwardDT = (RankingPowerAwardDT)nBaseSCDTs[i];
				if(rankingPowerAwardDT != null)
                {
					int first = rankingPowerAwardDT.iRankDown;
					int end = rankingPowerAwardDT.iRankUp;

					if (first <= iRank && iRank <= end)
						return rankingPowerAwardDT;

				}
			}
		}
        else
        {
			List<NBaseSCDT> nBaseSCDTs = f_GetAll().Where(o => o is RankingPowerAwardDT data && data.iEventTime == iId).ToList();
			dictsEvent.Add(iId, nBaseSCDTs);
			for (int i = 0; i < nBaseSCDTs.Count; i++)
			{
				RankingPowerAwardDT rankingPowerAwardDT = (RankingPowerAwardDT)nBaseSCDTs[i];
				if (rankingPowerAwardDT != null)
				{
					int first = rankingPowerAwardDT.iRankDown;
					int end = rankingPowerAwardDT.iRankUp;

					if (first <= iRank && iRank <= end)
						return rankingPowerAwardDT;

				}
			}
		}
		return null;
	}
}
