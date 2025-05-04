using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class RecommendFriendItem : MonoBehaviour
{
    public UI2DSprite mIcon;
    public UISprite mIconBorder;
    public UILabel mName;
    public UILabel mLevelLabel;
    public UILabel mPowerLabel;
    public UILabel mGuildLabel;
    public UILabel mVipLabel;

    public GameObject mApplyBtn;

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
        mLevelLabel.text = info.m_iLv.ToString();
        mPowerLabel.text = UITool.f_CountToChineseStr(info.m_iBattlePower);
        mGuildLabel.text = info.m_szLegion;
        mVipLabel.text = info.m_iVip.ToString();
    }
}
