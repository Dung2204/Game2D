
//============================================
//
//    Pay来自Pay.xlsx文件自动生成脚本
//    2017/4/1 15:35:11
//    
//
//============================================
using System;
using System.Collections.Generic;



public class PccaccyDT : NBaseSCDT
{
    /// <summary>
    /// Recharge amount
    /// </summary>
    public int iPccaccyNum;
    /// <summary>
    /// Recharge description
    /// </summary>
    public string _szPccaccyDesc;
    public string szPccaccyDesc
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szPccaccyDesc);
        }
    }
    /// <summary>
    /// First flush gift amount
    /// </summary>
    public int iFirstPccaccyNum;
    /// <summary>
    /// Promotion amount
    /// </summary>
    public int iPresentPccaccyNum;
    /// <summary>
    /// exchange rate
    /// </summary>
    public float iRate;
    /// <summary>
    /// Basic ingot
    /// </summary>
    public int iPayCount;
    /// <summary>
    /// Tên gói ProductId thanh toán trên Store Web
    /// </summary>
    public string szProductID_web;
    /// <summary>
    /// Tên gói ProductId thanh toán trên Store IOS
    /// </summary>
    public string szProductID_ios;
    /// <summary>
    /// Tên gói ProductId thanh toán trên Store Android
    /// </summary>
    public string szProductID_android;

    ///// <summary>
    ///// Tên gói ProductId thanh toán trên Store
    ///// </summary>
    //public string szProductId;

    /// <summary>
    /// Hiển thị mệnh giá gói đơn vị $
    /// </summary>
    public string szPayShow; //TsuCode

    public string szAward; //TsuCode

    
}
