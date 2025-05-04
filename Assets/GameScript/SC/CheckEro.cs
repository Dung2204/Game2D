using UnityEngine;
using System.Collections;

public class CheckEro
{

    public static bool f_CheckId(int iId)
    {
        if (iId <= 0)
        {
            return false;
        }

        return true;
    }

    public static bool f_CheckId(long iId)
    {
        if (iId <= 0)
        {
            return false;
        }
        return true;
    }


}
