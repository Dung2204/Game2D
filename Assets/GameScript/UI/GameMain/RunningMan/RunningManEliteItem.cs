using UnityEngine;
using System.Collections;

public class RunningManEliteItem : MonoBehaviour
{
    public UI2DSprite mIcon;
    public UILabel mTollgateName;
    public UILabel mIdxLabel;

    public GameObject mLockObj;
    public UILabel mLockDesc;
    public GameObject mSelectItem;


    public void f_UpdateByInfo(RunningManElitePoolDT info,int selectId)
    {
        mSelectItem.SetActive(info.m_Template.iId == selectId);
        mIdxLabel.text = info.m_Template.iId.ToString();
        //mIcon.sprite2D = UITool.f_GetIconSprite(info.m_ShowModelDT.iIcon);
        mTollgateName.text = info.m_Template.szName; 
        if (info.iId > Data_Pool.m_RunningManPool.m_iHistoryChapter)
        {
            mLockObj.SetActive(true);
mLockDesc.text = string.Format("Pass {0} open",info.iId * GameParamConst.RMTollgateNumPreChap);
            return;
        }
        else if (info.iId > Mathf.Min(Data_Pool.m_RunningManPool.m_iEliteFirstProg + 1, Data_Pool.m_RunningManPool.m_iEliteTollgateMax))
        {
            mLockObj.SetActive(true);
mLockDesc.text = string.Format("Pass {0} open",info.m_szPreTollgateName);
            return;
        }
        mLockDesc.text = string.Empty;
        mLockObj.SetActive(false);
    }
}
