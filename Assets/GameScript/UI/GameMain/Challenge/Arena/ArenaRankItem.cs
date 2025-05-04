using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class ArenaRankItem : UIFramwork
{
    public UI2DSprite mIcon;
    public UISprite mIconBorder;
    public UILabel mName;
    public UILabel mLevelLabel;
    public UILabel mRankLabel;
    public UISprite mRankSprite;
    public UILabel mBattlePowerLabel;

    public UIGrid mAwardGrid;
    public GameObject mAwardItem;
    private ResourceCommonItemComponent _awardShowComponent;
    private ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(mAwardGrid, mAwardItem);
            return _awardShowComponent;
        }
    }

    public void f_UpdateByInfo(ccU3DEngine.BasePoolDT<long> value)
    {
        ArenaPoolDT dt = (ArenaPoolDT)value;
        mIcon.sprite2D = UITool.f_GetIconSpriteBySexId(dt.m_PlayerInfo.m_iSex);
        string tName = dt.m_PlayerInfo.m_szName;
        int iFrame = dt.m_PlayerInfo.m_iFrameId;
        if (dt.m_PlayerInfo.m_iFrameId <= 0)
        {
            iFrame = (int)EM_Important.White;
        }
        string temp = "";
        mIconBorder.spriteName = UITool.f_GetImporentColorName(iFrame,  ref temp);
        mName.text = tName;
        mLevelLabel.text =  dt.m_PlayerInfo.m_iLv.ToString();
        mRankLabel.gameObject.SetActive(dt.m_iRank > 3);
        mRankSprite.gameObject.SetActive(dt.m_iRank <= 3);
        if (dt.m_iRank > 3)
        {
            mRankLabel.text = f_GetRankText(dt.m_iRank);
        }
        else
        {
            mRankSprite.spriteName = "Icon_rank" + dt.m_iRank;
        }        
        mBattlePowerLabel.text = UITool.f_CountToChineseStr(dt.m_ArenaCrossInfo.userExtra.iBattlePower);
        mAwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetArenaRankAward(dt.m_iRank));
        f_RegClickEvent(mIcon.gameObject, OnPlayerIconClick, dt.m_PlayerInfo);
    }

    public void f_UpdateByCrossInfo(ccU3DEngine.BasePoolDT<long> value)
    {
        ArenaPoolDT dt = (ArenaPoolDT)value;
        mIcon.sprite2D = UITool.f_GetIconSpriteBySexId(dt.m_ArenaCrossInfo.userView.uSex);
        ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(dt.m_ArenaCrossInfo.userExtra.offlineTime);

        string tName = "(" + serverInfo.szName + ")" + dt.m_ArenaCrossInfo.userView.m_szName;
        int iFrame = dt.m_ArenaCrossInfo.userView.uFrameId;
        if (dt.m_ArenaCrossInfo.userView.uFrameId <= 0)
        {
            iFrame = (int)EM_Important.White;
        }
        string temp = "";
        mIconBorder.spriteName = UITool.f_GetImporentColorName(iFrame, ref temp);
        mName.text = tName;
        mLevelLabel.text = dt.m_ArenaCrossInfo.userExtra.iLv.ToString();
        mRankLabel.gameObject.SetActive(dt.m_iRank > 3);
        mRankSprite.gameObject.SetActive(dt.m_iRank <= 3);
        if (dt.m_iRank > 3)
        {
            mRankLabel.text = f_GetRankText(dt.m_iRank);
        }
        else
        {
            mRankSprite.spriteName = "Icon_rank" + dt.m_iRank;
        }
        mBattlePowerLabel.text = UITool.f_CountToChineseStr(dt.m_ArenaCrossInfo.userExtra.iBattlePower);
        mAwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetArenaRankAward(dt.m_iRank));

    }
    /// <summary>
    /// 点击玩家头像
    /// </summary>
    private void OnPlayerIconClick(GameObject go, object obj1, object obj2)
    {
        BasePlayerPoolDT tData = (BasePlayerPoolDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_OPEN, tData);
    }
    private string f_GetRankText(int rank)
    {
        string rankText = string.Format(CommonTools.f_GetTransLanguage(765), rank);
        if (rank == 1)
        {
            rankText = string.Format(CommonTools.f_GetTransLanguage(766), rank);
        }
        else if(rank == 2)
        {
            rankText = string.Format(CommonTools.f_GetTransLanguage(767), rank);
        }
        return rankText;
    }
}
