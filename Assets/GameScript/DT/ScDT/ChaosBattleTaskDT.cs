using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ChaosBattleTaskDT : NBaseSCDT {
	
	///<summary>
	///type
	///</summary>
	public int iTaskType;

	///<summary>
	///name
	///</summary>
	public string _szTaskName;
	public string szTaskName
	{
		get
		{
			return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szTaskName);
		}
	}

	///<summary>
	///condition
	///</summary>
	public int iCondition;

	///<summary>
	///award list
	///</summary>
	public string _szAward;
	public string szAward
	{
		get
		{
			return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szAward);
		}
	}

}
