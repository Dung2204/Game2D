using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BattlePassTaskDT : NBaseSCDT {
	
	///<summary>
	///ngay mo sv / date
	///</summary>
	public int iType;
	
	///<summary>
	///thoi gian mo
	///</summary>
	public int iOpen;
	
	///<summary>
	///thoi gian tat
	///</summary>
	public int iClose;
	
	///<summary>
	///Icon
	///</summary>
	public int iIconId;
	
	///<summary>
	///ten nv 
	///</summary>
	public string _szDesc;
	public string szDesc
	{
		get
		{
			return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szDesc);
		}
	}

	///<summary>
	///So lan/Bac 1
	///</summary>
	public int iCondition;

	///<summary>
	///So lan/Bac 1
	///</summary>
	public int iConditionParam1;
	
	///<summary>
	///Point 1
	///</summary>
	public int iScore1;
	
	///<summary>
	///So lan/Bac 2
	///</summary>
	public int iConditionParam2;
	
	///<summary>
	///Point 2
	///</summary>
	public int iScore2;
	
	///<summary>
	///So lan/Bac 3
	///</summary>
	public int iConditionParam3;
	
	///<summary>
	///Point 3
	///</summary>
	public int iScore3;
	
	///<summary>
	///So lan/Bac 4
	///</summary>
	public int iConditionParam4;
	
	///<summary>
	///Point 4
	///</summary>
	public int iScore4;
	
	///<summary>
	///So lan/Bac 5
	///</summary>
	public int iConditionParam5;
	
	///<summary>
	///Point 5
	///</summary>
	public int iScore5;
	
	///<summary>
	///So lan/Bac 6
	///</summary>
	public int iConditionParam6;
	
	///<summary>
	///Point 6
	///</summary>
	public int iScore6;
	
	///<summary>
	///So lan/Bac 7
	///</summary>
	public int iConditionParam7;
	
	///<summary>
	///Point 7
	///</summary>
	public int iScore7;

	///<summary>
	///Point 7
	///</summary>
	public int iGotoId;
}
