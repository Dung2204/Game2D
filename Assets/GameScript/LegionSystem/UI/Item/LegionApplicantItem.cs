using UnityEngine;
using System.Collections;

public class LegionApplicantItem : MonoBehaviour
{
    public UI2DSprite mIcon;
    public UISprite mFrame;
    public UILabel mVipLv;
    public UILabel mNameLabel;
    public UILabel mLevelLabel;
    public UILabel mPowerLabel;

    public GameObject mAcceptBtn;
    public GameObject mDisacceptBtn;

    public void f_UpdateByInfo(ccU3DEngine.BasePoolDT<long> dt)
    {
        LegionPlayerPoolDT poolDt = (LegionPlayerPoolDT)dt;
        mIcon.sprite2D = UITool.f_GetIconSpriteBySexId(poolDt.PlayerInfo.m_iSex); 
        string tName = poolDt.PlayerInfo.m_szName;
        int iFrame = poolDt.PlayerInfo.m_iFrameId;
        mFrame.spriteName = UITool.f_GetImporentColorName(iFrame, ref tName);
        mNameLabel.text = tName;
        mVipLv.text = poolDt.PlayerInfo.m_iVip.ToString();
mLevelLabel.text = string.Format("[F5BF3B]Level： [-]{0}", poolDt.PlayerInfo.m_iLv);
mPowerLabel.text = string.Format("[F5BF3B]Force: [-]{0}", poolDt.PlayerInfo.m_iBattlePower);
    }
}
