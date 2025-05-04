using UnityEngine;
using System.Collections;

public class LegionMemberItem : MonoBehaviour
{
    public UI2DSprite mIcon;
    public UISprite mFrame;
    public UILabel mVipLabel;
    public UISprite mPosFlag;
    public UILabel mNameLabel;
    public UILabel mLevelLabel;
    public UILabel mPowerLabel;
    public UILabel mTimeLabel;
    public UILabel mContributionLabel;
    public UILabel mDayContributionLabel;
    
    public UIGrid mBtnGrid;
    public GameObject mExiteBtn;
    public GameObject mAppointBtn;   //任命按钮
    public GameObject mBecomChiefBtn; //移交军团长按钮
    public GameObject mBecomDeputyBtn; //任命副军团长按钮
    public GameObject mDeposeBtn;   //免职按钮
    public GameObject mKickOutBtn;  //踢出按钮
    public GameObject mLookBtn; //查看按钮
    public GameObject mDelateBtn; //弹劾按钮
    public GameObject mAppointContent;
    public GameObject mMask;

    public void f_UpdateByInfo(ccU3DEngine.BasePoolDT<long> dt)
    {
        LegionPlayerPoolDT poolDt = (LegionPlayerPoolDT)dt;
        mIcon.sprite2D = UITool.f_GetIconSpriteBySexId(poolDt.PlayerInfo.m_iSex);
        mVipLabel.text = string.Format("{0}", poolDt.PlayerInfo.m_iVip);
        string tName = poolDt.PlayerInfo.m_szName;
        int iFrame = poolDt.PlayerInfo.m_iFrameId; 
        mFrame.spriteName = UITool.f_GetImporentColorName(iFrame, ref tName);
        mNameLabel.text = tName;
        mPosFlag.spriteName = LegionTool.f_GetPosFlagName(poolDt.PlayerInfo.m_iLegionPostion);
        mPosFlag.MakePixelPerfect();
        mLevelLabel.text = string.Format(CommonTools.f_GetTransLanguage(584), poolDt.PlayerInfo.m_iLv);  //等级
        mPowerLabel.text = string.Format(CommonTools.f_GetTransLanguage(585), poolDt.PlayerInfo.m_iBattlePower);//战力
        mTimeLabel.text = UITool.f_GetTimeDescFromNow(poolDt.PlayerInfo.m_iOfflineTime);
        mContributionLabel.text = string.Format(CommonTools.f_GetTransLanguage(586), poolDt.PlayerInfo.m_iTotalContri); //累计贡献
        mDayContributionLabel.text = string.Format(CommonTools.f_GetTransLanguage(587), poolDt.PlayerInfo.m_iTodayContri);//今日祭天贡献
        UIEventListener.Get(mMask).onClick = mMaskClick;
        f_HideAppointContetn();
        EM_LegionPostionEnum selfPos = (EM_LegionPostionEnum)LegionMain.GetInstance().m_LegionPlayerPool.m_iSelfPos;
        EM_LegionPostionEnum itemPos = (EM_LegionPostionEnum)poolDt.PlayerInfo.m_iLegionPostion;
        mExiteBtn.SetActive(poolDt.PlayerInfo.iId == Data_Pool.m_UserData.m_iUserId);
        mAppointBtn.SetActive(selfPos == EM_LegionPostionEnum.eLegion_Chief && itemPos != EM_LegionPostionEnum.eLegion_Chief);
        mBecomChiefBtn.SetActive(selfPos == EM_LegionPostionEnum.eLegion_Chief && itemPos != EM_LegionPostionEnum.eLegion_Chief);
        mBecomDeputyBtn.SetActive(selfPos == EM_LegionPostionEnum.eLegion_Chief && itemPos == EM_LegionPostionEnum.eLegion_Normal);
        mDeposeBtn.SetActive(selfPos == EM_LegionPostionEnum.eLegion_Chief && itemPos == EM_LegionPostionEnum.eLegion_Deputy);
        mKickOutBtn.SetActive(selfPos < itemPos);
        mLookBtn.SetActive(selfPos > EM_LegionPostionEnum.eLegion_Chief && poolDt.PlayerInfo.iId != Data_Pool.m_UserData.m_iUserId);
        mDelateBtn.SetActive(selfPos != EM_LegionPostionEnum.eLegion_Chief && itemPos == EM_LegionPostionEnum.eLegion_Chief);
        mBtnGrid.repositionNow = true;
        mBtnGrid.Reposition();
    }

    public void f_ShowAppointContent()
    {
        mAppointContent.SetActive(true);
    }

    public void f_HideAppointContetn()
    {
        mAppointContent.SetActive(false);
    }

    private void mMaskClick(GameObject go)
    {
        f_HideAppointContetn();
    }
}
