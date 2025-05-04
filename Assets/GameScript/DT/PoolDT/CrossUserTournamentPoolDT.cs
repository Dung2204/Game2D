using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class CrossUserTournamentPoolDT : BasePoolDT<long>

{

    private long _userId;

    public long m_userId
    {
        get
        {
            return _userId;
        }
    }


    private int _iSex;

    public int m_iSex
    {
        get
        {
            return _iSex;
        }
    }

    private int _iJob;

    public int m_iJob
    {
        get
        {
            return _iJob;
        }
    }

    public int m_CardId
    {
        get
        {
            //if (m_iSex == (int)EM_RoleSex.Man)
            //{
            //    return GameParamConst.ManCardId;
            //}
            //else if (m_iSex == (int)EM_RoleSex.Woman)
            //{
            //    return GameParamConst.WomanCardId;
            //}
            //else
            //{
            //    return m_iSex;
            //}
            return UITool.GetMainCardId((EM_RoleSex)_iSex, (EM_CardFightType)_iJob);
        }
    }

    private string _szName;

    public string m_szName
    {
        get
        {
            return _szName;
        }
    }

    private int _iVip;

    public int m_iVip
    {
        get
        {
            return _iVip;
        }
    }

    private int _iTitle;

    public int m_iTitle
    {
        get
        {
            return _iTitle;
        }
    }

    private int _iFrameId;
    public int m_iFrameId
    {
        get
        {
            return _iFrameId;
        }
    }

    private int _iBattlePower;

    public int m_iBattlePower
    {
        get
        {
            return _iBattlePower;
        }
    }

    private int _iLv;
    public int m_iLv
    {
        get
        {
            return _iLv;
        }
    }

    private int _iRank;
    public int m_iRank
    {
        get
        {
            return _iRank;
        }
    }

    private int _iServerId;
    public int m_iServerId
    {
        get
        {
            return _iServerId;
        }
    }

    public string m_ServerName
    {
        get
        {
            ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(_iServerId);
            return serverInfo != null ? serverInfo.szName : _iServerId.ToString();
        }
         
    }

    private long _Damage;
    public long m_Damage
    {
        get
        {
            return _Damage;
        }
    }
    private int _MainCardId;
   
    public int m_MainCardId
    {
        get
        {
            return _MainCardId;
        }
    }

    private int _iPoint;
    public int m_iPoint
    {
        get
        {
            return _iPoint;
        }
    }
    private CrossTournamentFightCardPoolDT[] _fighterCard = new CrossTournamentFightCardPoolDT[6];
    public List<BasePoolDT<long>> m_fighterCard
    {
        get
        {
            List<BasePoolDT<long>> result = new List<BasePoolDT<long>>();
            for (int i = 0; i < 6; i++)
            {
                CrossTournamentFightCardPoolDT item = _fighterCard[i];
                result.Add(item);
            }
            return result;
        }
    }
    //public List<BasePoolDT<long>> GetFightCard()
    //{
    //    List<BasePoolDT<long>> result = new List<BasePoolDT<long>>();
    //    for (int i = 0; i < 6; i++)
    //    {
    //        CrossTournamentFightCardPoolDT item = _fighterCard[i];
    //        result.Add(item);
    //    }
    //    return result;
    //}

    public int Icon
    {
        get
        {
            CardDT tCardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(m_CardId);
            int iModelId = tCardDT.iStatelId1;
            if (iModelId == 0)
            {
                iModelId = tCardDT.iStatelId2;
            }
            RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(iModelId);
            return roleModle.iIcon;
        }
    }
    public int iImportant
    {
        get
        {
            //Tạm ẩn
            CardDT tCardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(m_CardId);
            return tCardDT.iImportant;
        }
    }
    public CrossUserTournamentPoolDT(long userId)
    {
        _userId = userId;
    }
    public void f_UpdateInfo(CMsg_CrossTournamentUserInfo info)
    {
        _iRank = info.uRank;
        _MainCardId = info.uMainCard;
        _iLv = info.iLv;
        _iServerId = info.iServerId;
        _iBattlePower = info.iBattlePower;
        _Damage = info.totalDamage;
        _iPoint = info.iPoint;

        _iSex = info.userView.uSex;
        _iJob = info.userView.uJob;
        _iTitle = info.userView.uTitleId;
        _iVip = info.userView.uVipLv;
        _iFrameId = info.userView.uFrameId;
        _szName = info.userView.m_szName;
        _fighterCard = new CrossTournamentFightCardPoolDT[6];
        for (int i = 0; i < 6; i++)
        {
            CrossTournamentFightCardPoolDT item = new CrossTournamentFightCardPoolDT();
            item.f_UpdateInfo(info.fighterCard[i]);
            _fighterCard[i] = item;
        }
        
        
    }
}