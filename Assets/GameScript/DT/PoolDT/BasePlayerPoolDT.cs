using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class BasePlayerPoolDT : BasePoolDT<long>

{
    public BasePlayerPoolDT()
    {
        m_eReadPlayerStep = EM_ReadPlayerStep.Base;
        f_ResetVigorData();
    }

    #region 基础信息

    //玩家ID

    private int _iSex;
    /// <summary>
    /// 玩家性别
    /// </summary>
    public int m_iSex
    {
        get
        {
            return _iSex;
        }
    }

    public int m_CardId
    {
        get
        {
            if (m_iSex == (int)EM_RoleSex.Man)
            {
                return GameParamConst.ManCardId;
            }
            else if (m_iSex == (int)EM_RoleSex.Woman)
            {
                return GameParamConst.WomanCardId;
            }
            else
            {
                return m_iSex;
            }
        }
    }

    private string _szName;
    /// <summary>
    /// 玩家名称
    /// </summary>
    public string m_szName
    {
        get
        {
            return _szName;
        }
    }

    private int _iVip;
    /// <summary>
    /// VIP标志
    /// </summary>
    public int m_iVip
    {
        get
        {
            return _iVip;
        }
    }

    private int _iTitle;
    /// <summary>
    /// 称号
    /// </summary>
    public int m_iTitle
    {
        get
        {
            return _iTitle;
        }
    }

    private int _iFrameId;
    /// <summary>
    /// 边框
    /// </summary>
    public int m_iFrameId
    {
        get
        {
            return _iFrameId;
        }
    }

    //public CardDT m_CardDT
    //{
    //    get
    //    {
    //        return glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(m_CardId) as CardDT;
    //    }
    //}
    #endregion


    #region 好友信息

    private int _iBattlePower;
    /// <summary>
    /// 战斗力
    /// </summary>
    public int m_iBattlePower
    {
        get
        {
            return _iBattlePower;
        }
    }

    private string _szLegion;
    /// <summary>
    /// 军团名
    /// </summary>
    public string m_szLegion
    {
        get
        {
            return _szLegion;
        }
    }

    private int _iLv;
    /// <summary>
    /// 等级
    /// </summary>
    public int m_iLv
    {
        get
        {
            return _iLv;
        }
    }

    private int _offlineTime;
    /// <summary>
    /// 离线时间 0代表在线
    /// </summary>
    public int m_iOfflineTime
    {
        get
        {
            return _offlineTime;
        }
    }

    private int _iDungeonStars;
    /// <summary>
    /// 副本星数
    /// </summary>
    public int m_iDungeonStars
    {
        get
        {
            return _iDungeonStars;
        }
    }

    /// <summary>
    /// 可赠送体力
    /// </summary>
    public bool mCanDonateVigor
    {
        get;
        private set;
    }

    /// <summary>
    /// 可领取体力
    /// </summary>
    public bool mCanRecvVigor
    {
        get;
        private set;
    }

    /// <summary>
    /// 捐赠时间
    /// </summary>
    public int mDonateTime
    {
        get;
        private set;
    }

    /// <summary>
    /// 更新精力数据
    /// </summary>
    /// <param name="mask"></param>
    public void f_UpdateVigorByServer(byte mask, int donateTime)
    {
        if(mask == (byte)EM_VigorFlag.Donate)
        {
            mCanDonateVigor = false;
        }
        else if(mask == (byte)EM_VigorFlag.CanGet)
        {
            mCanRecvVigor = true;
        }
        else if(mask == (byte)EM_VigorFlag.AlreadyGet)
        {
            mCanRecvVigor = false;
        }
        mDonateTime = donateTime;
    }

    /// <summary>
    /// 重置精力数据
    /// </summary>
    public void f_ResetVigorData()
    {
        mCanDonateVigor = true;
        mCanRecvVigor = false;
    }

    #endregion


    #region 军团信息

    private int _iLegionPostion;
    /// <summary>
    /// 军团职位
    /// </summary>
    public int m_iLegionPostion
    {
        get
        {
            return _iLegionPostion;
        }
    }

    private int _iTotalContri;
    /// <summary>
    /// 总军团贡献
    /// </summary>
    public int m_iTotalContri
    {
        get
        {
            return _iTotalContri;
        }
    }

    private int _iTodayContri;
    /// <summary>
    /// 今日祭天贡献
    /// </summary>
    public int m_iTodayContri
    {
        get
        {
            return _iTodayContri;
        }
    }

    #endregion

    private EM_ReadPlayerStep _eReadPlayerStep;
    /// <summary>
    /// 资料读取进度
    /// </summary>
    public EM_ReadPlayerStep m_eReadPlayerStep
    {
        get
        {
            return _eReadPlayerStep;
        }
        set
        {
            if((int)_eReadPlayerStep < (int)value)
            {
                _eReadPlayerStep = value;
            }
        }
    }

    /// <summary>
    /// 保存基础信息
    /// </summary>
    /// <param name="szName"></param>
    /// <param name="iLogo"></param>
    /// <param name="iVip"></param>
    /// <param name="iTitle"></param>
    public void f_SaveBase(string szName, int iSex, int iFrameId, int iVip, int iTitle)
    {
        _szName = szName;
        //int id = Random.Range(0, 8);
        _iSex = iSex;
        _iVip = iVip;
        _iTitle = iTitle;
        _iFrameId = iFrameId;
        m_eReadPlayerStep = EM_ReadPlayerStep.Base;
    }

    public void f_ChangeName(string name) {
        _szName = name;
    }

    /// <summary>
    /// 保存好友信息
    /// </summary>
    /// <param name="szLegion"></param>
    /// <param name="iLv"></param>
    /// <param name="iBattlePower"></param>
    public void f_SaveExtrend(string szLegion, int iLv, int iBattlePower, int iOfflineTime)
    {
        _szLegion = szLegion;
        _iLv = iLv;
        _iBattlePower = iBattlePower;
        _offlineTime = iOfflineTime;
        m_eReadPlayerStep = EM_ReadPlayerStep.Extend1;       
    }

    public void f_UpdateDungeonStar(int iDungeonStars)
    {
        _iDungeonStars = iDungeonStars;
    }

    /// <summary>
    /// 保存军团信息
    /// </summary>
    /// <param name="totalContri"></param>
    /// <param name="todayContri"></param>
    public void f_SaveLegion(int legionPostion, int todayContri, int totalContri)
    {
        _iLegionPostion = legionPostion;
        _iTotalContri = totalContri;
        _iTodayContri = todayContri;
        m_eReadPlayerStep = EM_ReadPlayerStep.Extend2;
    }

}