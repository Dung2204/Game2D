using UnityEngine;
using System.Collections;

public class RunningManItem : MonoBehaviour
{

    public GameObject mCurTip;
    public GameObject mPassTip;
    public GameObject mLabelParent;
    public UILabel mIdxLabel;
    public UILabel mNameLabel;
    public GameObject mStarsParent;
    public UISprite[] mStars;
    public Transform mRoleParent;
    public GameObject mFailParent;
    public UILabel mFailDesc;

    private GameObject role;

    public void f_UpdateByInfo(int passIdx, int curIdx, RunningManTollgatePoolDT info)
    {
        //transform.localScale = curIdx == passIdx ? Vector3.one : new Vector3(0.7f, 0.7f, 1.0f);
        mCurTip.SetActive(curIdx == passIdx);
        mPassTip.SetActive(!(curIdx == passIdx));
        //mLabelParent.SetActive(info.m_iResult <= 0);
        //mStarsParent.SetActive(info.m_iResult > 0);
        mIdxLabel.text = info.m_TollgateTemplate.iId.ToString();
        mNameLabel.text = info.m_TollgateTemplate.szName;
        //if (info.m_iResult > 0)
        //{
        //    if (role != null)
        //    {
        //        UITool.f_DestoryStatelObject(role);
        //    }
        //}
        //else
        //{
        //    UITool.f_CreateRoleByModeId(info.m_iMonsterId, ref role, mRoleParent, 1);
        //}
        //mFailParent.SetActive(curIdx == passIdx && Data_Pool.m_RunningManPool.m_bIsLose);
        //if (curIdx == passIdx && Data_Pool.m_RunningManPool.m_bIsLose)
        //{
        //    int tmpPassNum = Data_Pool.m_RunningManPool.m_iCurPassChapter * GameParamConst.RMTollgateNumPreChap + (passIdx >= GameParamConst.RMTollgateNumPreChap ? 0 : passIdx);
        //    mFailDesc.text = string.Format("Chúc mừng，bạn đã vượt ải{0} thành công", tmpPassNum);
        //}
        //else
        //    mFailDesc.text = string.Empty;
    }
}
