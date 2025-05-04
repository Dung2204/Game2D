using UnityEngine;
using System.Collections;

public class LegionTollgateAwardSelectBtn : MonoBehaviour
{
    public GameObject mSelectBtn;
    public UISlider mHpSlider;
    public GameObject mSelectTip;
    public UILabel mTollgateName;
    public UI2DSprite mFlagIcon;
    public UISprite mNameBg;
   // public TweenScale mTweenScale;

    public void f_UpdateByInfo(EM_CardCamp curCamp, EM_CardCamp selfCamp,LegionTollgatePoolDT info)
    {
        mHpSlider.value = (float)info.mHp / info.mHpMax;
        mSelectTip.SetActive(curCamp == selfCamp);
        mTollgateName.text = info.mTollgateTemplate.szName;
        mFlagIcon.sprite2D = UITool.f_GetDungeonSprite(info.mTollgateTemplate.iBoxImage);
        mFlagIcon.MakePixelPerfect();
       // mNameBg.spriteName = string.Format("Icon_TollgateAwardTitleBg{0}", (int)selfCamp);
        //if (curCamp == selfCamp)
        //{
        //    if (!mTweenScale.enabled)
        //    {
        //        mTweenScale.ResetToBeginning();
        //        mTweenScale.Play(true);
        //    }
        //}
        //else
        //{
        //    if (mTweenScale.enabled)
        //    {
        //        mTweenScale.enabled = false;
        //        mTweenScale.transform.localScale = Vector3.one;
        //    }
        //}
    }
}
