using UnityEngine;
using System.Collections;

public class RunningManSweepItem : MonoBehaviour
{
    public UILabel mTollgateName;
    public UILabel mMoneyLabel;
    public UILabel mPrestigeLabel;

    public UILabel mBuffDesc;
    public UILabel mStarCost;

    public UIGrid mAwardGrid;
    public GameObject mAwardItem;
	public UIScrollView mScrollView;

    public ResourceCommonItemComponent mShowAward;
   
    
    public void f_UpdateByInfo(RunningManSweepResult info)
    {
        if (info.m_eResultType == EM_RunningManSweepResultType.Tollgate)
        {
            mTollgateName.text = string.Format(CommonTools.f_GetTransLanguage(864), info.m_iTollgateId);
			mMoneyLabel.text = string.Format("{0}{1}",info.m_iMoney,info.m_szMoneyDesc);
			mPrestigeLabel.text = string.Format("{0}{1}",info.m_iPrestige,info.m_szPrestigeDesc);
        }
        else if (info.m_eResultType == EM_RunningManSweepResultType.Buff)
        {
            mBuffDesc.text = string.Format("[F5BF3DFF]{0}[-] [579425FF]+{1}%", UITool.f_GetProName((EM_RoleProperty)info.m_BuffProperty.m_iPropertyType), info.m_BuffProperty.m_iPropertyValue / 10000.0f * 100);
            mStarCost.text = string.Format(CommonTools.f_GetTransLanguage(865), info.m_iBuffIdx * GameParamConst.RMTollgateNumPreChap);
        }
        else if (info.m_eResultType == EM_RunningManSweepResultType.ChapBox)
        {
            if (mShowAward == null)
                mShowAward = new ResourceCommonItemComponent(mAwardGrid, mAwardItem);
            mShowAward.f_Show(info.m_ChapBoxAward);
        }
        else if (info.m_eResultType == EM_RunningManSweepResultType.Total)
        {
            mMoneyLabel.text = info.m_iMoney.ToString();
            mPrestigeLabel.text = info.m_iPrestige.ToString();
            if (mShowAward == null)
                mShowAward = new ResourceCommonItemComponent(mAwardGrid, mAwardItem);
            mShowAward.f_Show(info.m_ChapBoxAward);
			mScrollView.ResetPosition ();
        }
    }
}
