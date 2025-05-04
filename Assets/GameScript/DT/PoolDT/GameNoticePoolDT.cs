using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

class GameNoticePoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public int m_iStarTime;
    /// <summary>
    /// 结束时间
    /// </summary>
    public int m_iEndTime;
    /// <summary>
    /// 是否锁游戏
    /// </summary>
    public int m_iIsLockGame;
    /// <summary>
    /// 是否退出游戏
    /// </summary>
    public int m_iQuitGame;


    public char[] _SetTitle
    {
        set
        {
            CommonTools._CharArrToString(value, out m_szTitle);
        }
    }
    public char[] _SetContext
    {
        set
        {
            CommonTools._CharArrToString(value, out m_szContext);
        }
    }
    /// <summary>
    /// 公告标题
    /// </summary>
    public string m_szTitle;
    /// <summary>
    /// 公告文字
    /// </summary>
    public string m_szContext;
}

