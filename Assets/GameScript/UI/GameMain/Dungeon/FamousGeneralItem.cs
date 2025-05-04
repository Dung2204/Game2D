using UnityEngine;
using System.Collections;

public class FamousGeneralItem : MonoBehaviour
{
    public UILabel tTitle;
    public UILabel tIndex;
    public UILabel tSubIndex;
    public UI2DSprite tImage;
    public UI2DSprite tRoleImage;
    public GameObject tLockIcon;
    public GameObject tChapterPassTip;
    public UILabel tDesc;
    public GameObject tFightItem;
    public GameObject tBossTip;
    public GameObject tBossTipGray;

    private DungeonPoolDT mData;

    public void f_UpdateByInfo(ccU3DEngine.BasePoolDT<long> poolDt)
    {
        this.mData = (DungeonPoolDT)poolDt;
        bool bLock = Data_Pool.m_DungeonPool.f_CheckChapterLockState(mData);
        bool bFight = Data_Pool.m_DungeonPool.f_CheckIsFightChapter(mData);
        tLockIcon.SetActive(bLock);
        if (bLock)
        {
            tTitle.text = string.Format("[C4BDAAFF]{0}", mData.m_ChapterTemplate.szChapterName);
tIndex.text = string.Format("[D6D6CCFF]Chapter {0}", mData.mIndex + 1);
            tSubIndex.text = string.Format("[D6D6CCFF]{0}/{1}", mData.mTollgatePassNum, mData.mTollgateMaxNum);
            tDesc.text = string.Format("[D6D6CCFF]{0}", mData.m_ChapterTemplate.szChapterDesc);
            if (mData.mTollgateMaxNum > GameParamConst.LegendDungeonRestIdx)
            {
                tBossTip.SetActive(false);
                tBossTipGray.SetActive(true);
            }
            else
            {
                tBossTip.SetActive(false);
                tBossTipGray.SetActive(false);
            }
        }
        else
        {
            tTitle.text = string.Format("[FFD571FF]{0}", mData.m_ChapterTemplate.szChapterName);
tIndex.text = string.Format("[F4E0B2FF]Chapter {0}", mData.mIndex + 1);
            tSubIndex.text = string.Format("[F4E0B2FF]{0}/{1}", mData.mTollgatePassNum, mData.mTollgateMaxNum);
            tDesc.text = string.Format("[F4E0B2FF]{0}", mData.m_ChapterTemplate.szChapterDesc);
            if (mData.mTollgateMaxNum > GameParamConst.LegendDungeonRestIdx)
            {
                tBossTip.SetActive(true);
                tBossTipGray.SetActive(false);
            }
            else
            {
                tBossTip.SetActive(false);
                tBossTipGray.SetActive(false);
            }
        }
        //tImage.sprite2D = UITool.f_GetDungeonSprite(mData.m_ChapterTemplate.szChapterImage);
        UITool.f_Set2DSpriteGray(tImage, bLock);
        tRoleImage.sprite2D = UITool.f_GetDungeonSprite(mData.m_ChapterTemplate.iRoleImage);
        tRoleImage.MakePixelPerfect();
        tRoleImage.transform.localScale = new Vector3(1.12f, 1.12f, 1.0f);
        UITool.f_Set2DSpriteGray(tRoleImage, bLock);
        tFightItem.SetActive(bFight);

        //bool isRedpoint = Data_Pool.m_DungeonPool.f_CheckLegionRedDot();
        bool isCanGetBox = Data_Pool.m_DungeonPool.f_CheckHasBoxCanGet(mData);
        //bool isAllHaveTimes = false;
        //for (int i = 0; i < mData.mTollgateList.Count; i++)
        //{
        //    if (i < mData.mTollgatePassNum)
        //    {
        //        if (mData.mTollgateList[i].mTollgateTemplate.iBoxId > 0)
        //            BoxCount++;
        //    }
        //    else
        //        break;
        //}
        transform.Find("BoxOpen").gameObject.SetActive(isCanGetBox && !bLock);
        transform.Find("Reddot").gameObject.SetActive(isCanGetBox && !bLock);
        tChapterPassTip.SetActive(mData.mTollgatePassNum == mData.mTollgateMaxNum);
    }
}
