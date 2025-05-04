using UnityEngine;
using System.Collections;

public class LegionChapterItem : MonoBehaviour
{
    public UILabel mChapterIdx;
    public UILabel mChapterName;
    public UILabel mChapterTip;
    public GameObject mChapterPassTip;
    public UISlider mHpSlider;
    public UILabel mHpLabel;
    public UI2DSprite mImage;

    public void f_UpdateByInfo(ccU3DEngine.BasePoolDT<long> dt)
    {
        byte curChapId = LegionMain.GetInstance().m_LegionDungeonPool.m_iCurDungeonChapId;
        bool inTime = LegionMain.GetInstance().m_LegionDungeonPool.f_IsInOpenTime(false);
        LegionDungeonPoolDT info = (LegionDungeonPoolDT)dt;
        mChapterIdx.text = string.Format("C.{0}", info.mChapterId);
        mChapterName.text = string.Format("{0}", info.mChapterTemplate.szName);
mChapterTip.text = string.Format("{0}:00-{1}:00 daily entered", LegionConst.LEGION_DUNGEON_BEGIN_TIME, LegionConst.LEGION_DUNGEON_END_TIME);
        if (info.mChapterId < curChapId)
        {
            mHpSlider.gameObject.SetActive(false);
            mChapterPassTip.SetActive(true);
        }
        else if (info.mChapterId == curChapId)
        {
            mHpSlider.gameObject.SetActive(true);
            float hpPrecent = (float)info.mTotalHp / info.mTotalHpMax;
            mHpSlider.value = hpPrecent;
            mHpLabel.text = string.Format("{0}/{1}",info.mTotalHp,info.mTotalHpMax);
            mChapterPassTip.SetActive(info.mTotalHp == 0); 
        }
        else
        {
            mHpSlider.gameObject.SetActive(false);
            mChapterPassTip.SetActive(false);
        }
        mImage.sprite2D = UITool.f_GetIconSprite(info.mChapterTemplate.iImage);  
        //临时的
        //mImage.sprite2D = Resources.Load<Sprite>("LegionRes/UITexture/Temp_11111");
        mImage.MakePixelPerfect();
        UITool.f_Set2DSpriteGray(mImage, info.mChapterId > curChapId);
    }
}
