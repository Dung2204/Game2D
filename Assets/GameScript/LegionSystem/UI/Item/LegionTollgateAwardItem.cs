using UnityEngine;
using System.Collections;

public class LegionTollgateAwardItem : MonoBehaviour
{
    public GameObject mBoxClose;
    public GameObject mBoxOpen;
    public UILabel mBoxIdx;
    public GameObject mAwardItem;
    public GameObject mAwardParent;
    private ResourceCommonItem mShowItem;
    public UILabel mGetPlayerName;
    public GameObject mBorder;

    public void f_UpdateByInfo(ccU3DEngine.BasePoolDT<long> dt)
    {
        LegionTollgateAwardPoolDT info = (LegionTollgateAwardPoolDT)dt;
        if (mShowItem == null)
            mShowItem = ResourceCommonItem.f_Create(mAwardParent, mAwardItem);
        if (info.m_iGetPlayer != 0 && info.m_cAwardTemplate != null)
        {
            mShowItem.f_UpdateByInfo(info.m_cAwardTemplate.m_iAwardType, info.m_cAwardTemplate.m_iAwardId, info.m_cAwardTemplate.m_iAwardCount);
        }
        else
        {
            mShowItem.f_Disable();
        }
        mBoxClose.SetActive(info.m_iGetPlayer == 0);
        mBoxOpen.SetActive(info.m_iGetPlayer != 0);
        mBoxIdx.text = info.m_iGetPlayer == 0 ? info.m_iIdx.ToString() : string.Empty;
        mGetPlayerName.text = string.Empty;
        if (info.m_iGetPlayer != 0)
            Data_Pool.m_GeneralPlayerPool.f_ReadInfor(info.m_iGetPlayer, EM_ReadPlayerStep.Base, f_Callback_ReadInfo);
        mBorder.SetActive(transform.name == "2" || transform.name == "7" || transform.name == "15" || transform.name == "10");
    }

    private void f_Callback_ReadInfo(object value)
    {
        BasePlayerPoolDT info = (BasePlayerPoolDT)value;
        mGetPlayerName.text = info.m_szName;
    }
}
