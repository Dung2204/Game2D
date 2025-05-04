using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelGiftDT : NBaseSCDT {

	///<summary>
	///loai 1 mo theo moc cap , 2 dang nhap lan dau trong ngay, 3 theo tuong, 4 trang bi, 5 phap bao
	///</summary>
	public int iType;

	///<summary>
	///EventTimeid
	///</summary>
	public int iEventTime;

	///<summary>
	///iParam1
	///</summary>
	public int iParam1;

	///<summary>
	///iParam2
	///</summary>
	public string szParam2;

	///<summary>
	///iParam3
	///</summary>
	public int iParam3;

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
	///????
	///</summary>
	public string szAward;
	
	///<summary>
	///VIP EXP
	///</summary>
	public int iPayCount;
	
	///<summary>
	///So Gio
	///</summary>
	public int iHour;
	
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
