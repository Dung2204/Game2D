using ccU3DEngine;
using System.Collections;

public class RunningManPoolDT : BasePoolDT<long>
{
    private RunningManTollgatePoolDT[] tollgatePoolDTs;
    public RunningManPoolDT(RunningManChapterDT chapterDt)
    {
        iId = chapterDt.iId;
        chapterTemplate = chapterDt;
        int[] tTollgateIds = ccMath.f_String2ArrayInt(chapterDt.szTollgateIds, ";");
        int[] tMonsterIds = ccMath.f_String2ArrayInt(chapterDt.szShowMonsters, ";");
        if (tTollgateIds.Length != GameParamConst.RMTollgateNumPreChap)
MessageBox.ASSERT("Incorrect level data,Id: " + chapterDt.iId);
        tollgatePoolDTs = new RunningManTollgatePoolDT[GameParamConst.RMTollgateNumPreChap];
        for (int i = 0; i < tollgatePoolDTs.Length; i++)
        {
            tollgatePoolDTs[i] = new RunningManTollgatePoolDT(chapterDt.iId, tTollgateIds[i], tMonsterIds[i]);
        }
        buff = new int[GameParamConst.RMModeNumPreTollgate];
        f_Reset(); 
    }

    public void f_Reset()
    {
        for (int i = 0; i < tollgatePoolDTs.Length; i++)
        {
            tollgatePoolDTs[i].f_Reset();
        }
        boxTimes = 0;
        buffIdx = 0;
        for (int i = 0; i < buff.Length; i++)
        {
            buff[i] = 0;
        }
    }

    public void f_UpdateInfo(byte boxTimes,byte buffIdx,byte[] buff,sbyte[] iRet)
    {
        this.boxTimes = boxTimes;
        this.buffIdx = buffIdx;
        for (int i = 0; i < buff.Length; i++)
        {
            this.buff[i] = buff[i];
        }
        for (int i = 0; i < tollgatePoolDTs.Length; i++)
        {
            tollgatePoolDTs[i].f_UpdateInfo(iRet[i]);
        }
    }

    private int boxTimes;
    private int buffIdx;
    private int[] buff;
    private RunningManChapterDT chapterTemplate;


    public int m_iBoxTimes
    {
        get
        {
            return boxTimes;
        }
    }

    public int m_iBuffIdx
    {
        get
        {
            return buffIdx;
        }
    }

    public int[] m_Buff
    {
        get
        {
            return buff;
        }
    }

    public RunningManChapterDT m_ChapterTemplate
    {
        get
        {
            return chapterTemplate;
        }
    }

    public RunningManTollgatePoolDT[] m_TollgatePoolDTs
    {
        get
        {
            return tollgatePoolDTs;
        }
    }
}

public class RunningManTollgatePoolDT
{
    public RunningManTollgatePoolDT(int chapId,int tollgateId,int monsterId)
    {
        chapterId = chapId;
        this.tollgateId = tollgateId;
        this.monsterId = monsterId;
        tollgateTemplate = (RunningManTollgateDT)glo_Main.GetInstance().m_SC_Pool.m_RunningManTollgateSC.f_GetSC(tollgateId);
        if (tollgateTemplate == null)
MessageBox.ASSERT("The level data does not exist Id：" + tollgateId);
        f_Reset();
    }

    public void f_Reset()
    {
        result = 0;
    }

    public void f_UpdateInfo(int ret)
    {
        result = ret;
    }

    private int chapterId;
    public int m_iChapterId
    {
        get
        {
            return chapterId;
        }
    }

    private int tollgateId;
    public int m_iTollgateId
    {
        get
        {
            return tollgateId;
        }
    }

    private int monsterId;
    public int m_iMonsterId
    {
        get
        {
            return monsterId;
        }
    }

    private RunningManTollgateDT tollgateTemplate;
    public RunningManTollgateDT m_TollgateTemplate
    {
        get
        {
            return tollgateTemplate;
        }
    }

    public int result;
    /// <summary>
    ///  -1失败 0未挑战 1~3挑战成功星数
    /// </summary>
    public int m_iResult
    {
        get
        {
            return result;
        }
    }
}
