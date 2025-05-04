using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DungeonTollgateDT : NBaseSCDT {

	///<summary>
	///关卡名称
	///</summary>
	public string _szTollgateName;
	public string szTollgateName
	{
		get
		{
			return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szTollgateName);
		}
	}

	///<summary>
	///关卡介绍
	///</summary>
	public string _szTollgateDesc;
	public string szTollgateDesc
	{
		get
		{
			return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szTollgateDesc);
		}
	}

	///<summary>
	///模型对话
	///</summary>
	public string _szModeDialog;
	public string szModeDialog
	{
		get
		{
			return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szModeDialog);
		}
	}

	///<summary>
	///模型Id
	///</summary>
	public int iModeId;
	
	///<summary>
	///怪物
	///</summary>
	public string szMonsterId;
	
	///<summary>
	///体力消耗
	///</summary>
	public int iEnergyCost;
	
	///<summary>
	///可挑战次数/天
	///</summary>
	public int iCountLimit;
	
	///<summary>
	///奖池ID
	///</summary>
	public int iPond;
	
	///<summary>
	///关卡宝箱ID
	///</summary>
	public int iBoxId;
	
	///<summary>
	///推荐战斗力
	///</summary>
	public int iFightValue;
	
	///<summary>
	///场景Id
	///</summary>
	public int iSceneId;
	
	///<summary>
	///首通奖励元宝
	///</summary>
	public int iFirstWinSycee;
	
	///<summary>
	///是否播放剧情
	///</summary>
	public int iPlot;
	
	///<summary>
	///播放剧情我方替代怪物
	///</summary>
	public string szPlotMonsterId;
	
	///<summary>
	///播放剧情敌方替代怪物
	///</summary>
	public string szPlotEnemyMonsterId;
	
	///<summary>
	///对应的章节
	///</summary>
	public int iDungeonChapter;
	
}
