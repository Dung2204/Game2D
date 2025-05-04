using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class LegionChapterAwardItem : MonoBehaviour
{
    public UILabel mChapterName;
    public GameObject mGetBtn;
    public GameObject mLockBtn;
    public GameObject mAlreadyGetBtn;

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

    public void f_UpdateByInfo(NBaseSCDT dt)
    {
        LegionChapterDT info = (LegionChapterDT)dt;
        byte passChap = LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonPassChap;
        byte awardChap = LegionMain.GetInstance().m_LegionDungeonPool.m_iAwardChapter;
		//{0} số thứ tự, {1} tên ải
mChapterName.text = string.Format("Pass {1}", info.iId, info.szName);
        mGetBtn.SetActive(info.iId <= passChap && info.iId > awardChap);
        mLockBtn.SetActive(info.iId > passChap);
        mAlreadyGetBtn.SetActive(info.iId <= passChap && info.iId <= awardChap);
        mAwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetAwardByString(info.szAward));
    }
}
