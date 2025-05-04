using UnityEngine;
using System.Collections;

public class FriendItem : MonoBehaviour
{
    public UI2DSprite mIcon;
    public UISprite mIconBorder;
    public UILabel mName;
    public UILabel mLevelLabel;
    public UILabel mPowerLabel;
    public UILabel mGuildLabel;
    public UILabel mVipLabel;
    public UILabel mTimeLabel;

    public GameObject mDonateVigorBtn;
    public GameObject mAlreadyDonateTip;

    public void f_UpdateByInfo(BasePlayerPoolDT info)
    {
        mIcon.sprite2D = UITool.f_GetIconSpriteBySexId(info.m_iSex);
        string name = info.m_szName;
        int iFrame = info.m_iFrameId;
        if (info.m_iFrameId <= 0)
        {
            //Debug.LogError("颜色边框没有设置");
            iFrame = (int)EM_Important.White;
        }
        mIconBorder.spriteName = UITool.f_GetImporentColorName(iFrame, ref name);
        mName.text = name;
mLevelLabel.text = "Grant" + info.m_iLv;
        mPowerLabel.text = UITool.f_CountToChineseStr(info.m_iBattlePower);
        mGuildLabel.text = info.m_szLegion;
        mVipLabel.text = string.Format("VIP {0}", info.m_iVip);
        mTimeLabel.text = UITool.f_GetTimeDescFromNow(info.m_iOfflineTime);
        mDonateVigorBtn.SetActive(info.mCanDonateVigor);
        mAlreadyDonateTip.SetActive(!info.mCanDonateVigor);
    }
}
