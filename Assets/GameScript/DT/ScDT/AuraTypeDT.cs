
//============================================
//
//    AuraType.xlsx vòng sáng nghề
//    2018/1/15 17:04:17
//    maco
//
//============================================
using System;
using System.Collections.Generic;



public class AuraTypeDT : NBaseSCDT
{
    /// <summary>
    /// nghề
    /// </summary>
    public int iType;
    /// <summary>
    /// là buff toàn đồi (0: cùng nghề,1: toàn đội)
    /// </summary>
    public int iIsAll;
    /// <summary>
    /// cấp vòng sáng
    /// </summary>
    public int iLevel;
    /// <summary>
    /// chuỗi thuộc tính 
    /// </summary>
    public string szParam;
}
