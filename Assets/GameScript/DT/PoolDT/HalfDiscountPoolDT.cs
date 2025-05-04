using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

class HalfDiscountPoolDT : BasePoolDT<long>
{

    public int _ITempleteId;
    public byte DayIdx;    //天数对应
    public short BuyTime;  //购买次数
    public HalfDiscountDT m_HalfDiscountDT;
}

