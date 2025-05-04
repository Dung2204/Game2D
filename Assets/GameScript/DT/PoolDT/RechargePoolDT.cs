using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

public class RechargePoolDT:BasePoolDT<long>
{
    public RechargePoolDT(int templateId)
    {
        iId = templateId;
        times = 0;
    }

    public int times; 
}
