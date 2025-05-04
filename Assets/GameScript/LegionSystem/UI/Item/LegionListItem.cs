using UnityEngine;
using System.Collections;

public class LegionListItem : MonoBehaviour
{
    public UI2DSprite mIcon;
    public UILabel mLevel;
    public UILabel mLegionName;
    public UILabel mChiefName;
    public UILabel mMemNum;
    public UILabel mManifesto;
    public GameObject mFullBtn;
    public GameObject mCanelApplyBtn;
    public GameObject mApplyBtn;

    public void f_UpdateByInfo(ccU3DEngine.BasePoolDT<long> dt)
    {
        LegionInfoPoolDT info = (LegionInfoPoolDT)dt;
        int lv = info.f_GetProperty((int)EM_LegionProperty.Lv);
        int memMax = ((LegionLevelDT)glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetSC(lv)).iCountMax;
        int memNum = info.f_GetProperty((int)EM_LegionProperty.MemberNum);
        bool isApplying = LegionMain.GetInstance().m_LegionInfor.f_CheckIsApplying(info.iId);
        mIcon.sprite2D = UITool.f_GetIconSprite(info.f_GetProperty((int)EM_LegionProperty.Icon));
        mLevel.text = string.Format("{0}",lv);
        mLegionName.text = info.LegionName;
        BasePlayerPoolDT playerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(info.MasterUserId);
        mChiefName.text = string.Format(CommonTools.f_GetTransLanguage(446), playerInfo != null?playerInfo.m_szName:string.Empty);
        mMemNum.text = string.Format(CommonTools.f_GetTransLanguage(447), memNum,memMax);
        mManifesto.text = string.Format("{0}", string.IsNullOrEmpty(info.Manifesto)? LegionConst.LEGION_MANIFESTO_DEFAULT_VALUE: info.Manifesto);
        mFullBtn.SetActive(memNum >= memMax);
        mCanelApplyBtn.SetActive(memNum < memMax && isApplying);
        mApplyBtn.SetActive(memNum < memMax && !isApplying);
    }
}
