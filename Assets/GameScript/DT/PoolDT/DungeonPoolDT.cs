using ccU3DEngine;
using System.Collections.Generic;

/// <summary>
/// _Data1 = ChapterTemplate.ChapterType
/// </summary>
public class DungeonPoolDT : BasePoolDT<long>
{
    public DungeonPoolDT(int idx)
    {
        mIndex = idx;
        mTollgatePassNum = 0;
        mStarNum = 0;
        mBox1Get = false;
        mBox2Get = false;
        mBox3Get = false;
        mInitByServer = false;
    }

    /// <summary>
    /// 是否已经向服务器请求过数据
    /// </summary>
    public bool mInitByServer
    {
        get;
        private set;
    }

    /// <summary>
    /// 更新是否初始化
    /// </summary>
    /// <param name="state"></param>
    public void f_UpdateInitByServerState(bool state)
    {
        mInitByServer = state;
    }

    /// <summary>
    /// 章节索引
    /// </summary>
    public int mIndex
    {
        get;
        private set;
    }

    /// <summary>
    /// 通关关卡数目
    /// </summary>
    public int mTollgatePassNum
    {
        get;
        private set;  
    }
    /// <summary>
    /// 关卡宝箱已领取数量
    /// </summary>
    public int mToBoxGetNum
    {
        get;
        set;
    }
    /// <summary>
    /// 章节星数
    /// </summary>
    public int mStarNum
    {
        get;
        private set;
    }

    /// <summary>
    /// 宝箱1领取 true:已领取
    /// </summary>
    public bool mBox1Get
    {
        get;
        private set;
    }

    /// <summary>
    /// 宝箱2领取 true:已领取
    /// </summary>
    public bool mBox2Get
    {
        get;
        private set;
    }

    /// <summary>
    /// 宝箱3领取 true:已领取
    /// </summary>
    public bool mBox3Get
    {
        get;
        private set;
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    public void f_UpdateInfo(byte tollgates,byte uToBoxGetNum, byte stars,byte boxFlag)
    {
        mTollgatePassNum = tollgates;
        mToBoxGetNum = uToBoxGetNum;
        mStarNum = stars;
        //处理掩码
        int tBox1 = (boxFlag / 100) % 10;
        int tBox2 = (boxFlag / 10) % 10;
        int tBox3 = boxFlag % 10;
        mBox1Get = tBox1 != 0;
        mBox2Get = tBox2 != 0;
        mBox3Get = tBox3 != 0;
    }

    public void f_UpdateBoxInfo(int uToBoxGetNum, int[] boxFlag)
    {       
        mToBoxGetNum += uToBoxGetNum;
        //处理掩码
        for (int i = 0; i < boxFlag.Length; i++)
        {
            if (boxFlag[i] == 1) mBox1Get = true;
            else if (boxFlag[i] == 2) mBox2Get = true;
            else if (boxFlag[i] == 3) mBox3Get = true;
        }
    }

    public void f_UpdateTollgateInfo(int tollgaetId,byte stars,byte boxTimes,byte times,byte resetTimes)
    {
        DungeonTollgatePoolDT dt = f_GetTollgateData(tollgaetId);
        if (dt == null)
        {
MessageBox.ASSERT(string.Format("DungeonTollgatePoolDT update failed, data does not exist, ChapterId:{0}, TollgateId:{1}",m_iChapterTemplateId, tollgaetId));
        }
        dt.f_UpdateInfo(stars, boxTimes, times, resetTimes);
    }

    /// <summary>
    /// 章节数据模板Id
    /// </summary>
    private int _iChapterTemplateId;
    public int m_iChapterTemplateId
    {
        get
        {
            return _iChapterTemplateId;
        }
        set
        {
            if (_iChapterTemplateId != value)
            {
                _iChapterTemplateId = value;
                m_ChapterTemplate = (DungeonChapterDT)glo_Main.GetInstance().m_SC_Pool.m_DungeonChapterSC.f_GetSC(_iChapterTemplateId);
                if (m_ChapterTemplate == null)
                {
MessageBox.ASSERT(string.Format("DungeonChapterSC data does not exist Id:{0}",_iChapterTemplateId));
                }
                _iData1 = m_ChapterTemplate.iChapterType;
                InitTollgateByChapter();
            }
        }
    }

    public DungeonChapterDT m_ChapterTemplate
    {
        get;
        private set;
    }

    public List<DungeonTollgatePoolDT> mTollgateList
    {
        get;
        private set;
    }
    private void InitTollgateByChapter()
    {
        mTollgateList = new List<DungeonTollgatePoolDT>();
        string[] tollgates = m_ChapterTemplate.szTollgateId.Split(';');
        for (int i = 0; i < tollgates.Length; i++)
        {
            int tId = int.Parse(tollgates[i]);
            DungeonTollgatePoolDT tDt = new DungeonTollgatePoolDT(m_ChapterTemplate.iChapterType,i,m_ChapterTemplate.iId, tId);
            mTollgateList.Add(tDt);
        }
    }

    public DungeonTollgatePoolDT f_GetTollgateData(int tollgateId)
    {
        return mTollgateList.Find(delegate (DungeonTollgatePoolDT item) { return item.mTollgateId == tollgateId; });
    }

    public int mTollgateMaxNum
    {
        get
        {
            return mTollgateList.Count;
        }
    }

    /// <summary>
    /// 跨天重置副本次数
    /// </summary>
    public void f_ResetTollgateData()
    {
        for (int i = 0; i < mTollgateList.Count; i++)
        {
            mTollgateList[i].f_Reset();
        }
    }
}

/// <summary>
/// 关卡数据
/// </summary>
public class DungeonTollgatePoolDT
{

    public DungeonTollgatePoolDT(int chapterType,int idx,int chapterId,int tollgateId)
    {
        mChapterType = chapterType;
        mIndex = idx;
        mChapterId = chapterId;
        mTollgateId = tollgateId;
        mTollgateTemplate = (DungeonTollgateDT)glo_Main.GetInstance().m_SC_Pool.m_DungeonTollgateSC.f_GetSC(mTollgateId);
        mStarNum = 0;
        mBoxTimes = 0;
        mTimes = 0;
        m_bResetByNextDay = !(mChapterType == (int)EM_Fight_Enum.eFight_Legend && idx >= GameParamConst.LegendDungeonRestIdx);
        if (mTollgateTemplate == null)
        {
MessageBox.ASSERT(string.Format("DungeonTollgateSC data does not exist, ChapterId:{0}, TollgateId:{1}",mChapterId, mTollgateId));
        }
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    public void f_UpdateInfo(byte stars,byte boxTimes,byte times,byte resetTimes)
    {
        mStarNum = stars;
        mBoxTimes = boxTimes;
        mTimes = times;
        mResetTimes = resetTimes;
    }

    public void f_UpdateBoxInfo(int boxTimes)
    {
        mBoxTimes = boxTimes;
    }

    /// <summary>
    /// 获取宝箱状态
    /// </summary>
    public EM_BoxGetState f_GetBoxState()
    {
        if (mBoxTimes > 0)
            return EM_BoxGetState.AlreadyGet;
        if (mStarNum > 0)
            return EM_BoxGetState.CanGet;
        return EM_BoxGetState.Lock;
    }

    /// <summary>
    /// 是否能夸天重置
    /// </summary>
    public bool m_bResetByNextDay
    {
        private set;
        get;
    }

    /// <summary>
    /// 重置数据，只需重置副本次数
    /// </summary>
    public void f_Reset()
    {
        if(m_bResetByNextDay)
            mTimes = 0;
    }

    /// <summary>
    /// 章节类型 副本类型
    /// </summary>
    public int mChapterType
    {
        get;
        private set;
    }

    /// <summary>
    /// 关卡索引
    /// </summary>
    public int mIndex
    {
        get;
        private set;
    }

    /// <summary>
    /// 章节Id
    /// </summary>
    public int mChapterId
    {
        get;
        private set;
    }

    /// <summary>
    /// 关卡Id
    /// </summary>
    public int mTollgateId
    {
        get;
        private set;
    }

    /// <summary>
    /// 关卡模板
    /// </summary>
    public DungeonTollgateDT mTollgateTemplate
    {
        get;
        private set;
    }

    /// <summary>
    /// 星数
    /// </summary>
    public int mStarNum
    {
        get;
        private set;
    }

    /// <summary>
    /// 宝箱奖励领取次数
    /// </summary>
    public int mBoxTimes
    {
        get;
        private set;
    }

    /// <summary>
    /// 挑战次数
    /// </summary>
    public int mTimes
    {
        get;
        private set;
    }
    /// <summary>
    /// 重置次数
    /// </summary>
    public int mResetTimes
    {
        get;
        private set;
    }
}

/// <summary>
/// 副本一个滑动选项卡的数据结构
/// （后面优化成几条章节+背景作为一条选项卡处理，一个选项卡就是一个屏幕大小，背景与背景间无缝拼接滑动）
/// </summary>
public class DungeonPoolDTOfPage : BasePoolDT<long>
{
    //类型其实是DungeonPoolDT数组
    public BasePoolDT<long>[] DungeonPoolDTList;
}
