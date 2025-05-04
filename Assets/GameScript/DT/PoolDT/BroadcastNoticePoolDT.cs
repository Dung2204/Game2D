using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class BroadcastNoticePoolDT : BasePoolDT<long>
{
    public int uStartTime;
    public int uOverTime;
    public string szContext;
    public int uShowDeleTime;//显示跑马灯间隔时间
    public int lastShowTime;//上一次跑马灯的显示时间
}
