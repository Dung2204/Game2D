using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TripleMoneyDT : NBaseSCDT {

	public int iEventTime;
	///<summary>
	///Nap
	///</summary>
	public int iPayNum;
	
	///<summary>
	///Mo Ta
	///</summary>
	public string _szPayDesc;
	public string szPayDesc
	{
		get
		{
			return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szPayDesc);
		}
	}

	///<summary>
	///Moc 1
	///</summary>
	public int iFirstPayNum;
	
	///<summary>
	///Moc 2
	///</summary>
	public int iSecondPayNum;
	
	///<summary>
	///Moc 3
	///</summary>
	public int iThirdPayNum;
	
	///<summary>
	///VIP EXP
	///</summary>
	public int iPayCount;
	
	///<summary>
	///So Lan
	///</summary>
	public int iMaxNum;
	
	///<summary>
	///Id Web
	///</summary>
	public string szProductID_web;
	
	///<summary>
	///id Ios
	///</summary>
	public string szProductID_ios;
	
	///<summary>
	///Id Android
	///</summary>
	public string szProductID_android;
	
	///<summary>
	///
	///</summary>
	public string szPayShow;
	
}
