using UnityEngine;
using System.Collections;

public class LegionTollgateItem : MonoBehaviour
{
    public GameObject mItem;
    public UILabel mTollgateName; 
    public GameObject mBreakTip;
    public UISprite mCampTip;
    public UILabel mKillerTip;
    public UISlider mHpSlider;
    public UILabel mHpLabel;

    public void f_UpdateByInfo(LegionTollgatePoolDT info)
    {
        mTollgateName.text = info.mTollgateTemplate.szName;
        mBreakTip.SetActive(info.mHp <= 0);
        LegionTool.f_SetCampSpriteByCamp(mCampTip, info.mCamp); 
        mCampTip.MakePixelPerfect();
        mCampTip.transform.localScale = new Vector3(0.6f, 0.6f, 1);
        float hpPrecent = (float)info.mHp / info.mHpMax;
        mHpSlider.value = hpPrecent;
        mHpLabel.text = string.Format("{0}%", Mathf.CeilToInt(hpPrecent * 100));
        mKillerTip.text = string.Empty;
        if(info.mKillerId != 0)
            Data_Pool.m_GeneralPlayerPool.f_ReadInfor(info.mKillerId, EM_ReadPlayerStep.Base,f_Callback_ReadInfo);
    }

    private void f_Callback_ReadInfo(object value)
    {
        BasePlayerPoolDT playerInfo = (BasePlayerPoolDT)value;
        if (playerInfo != null)
        {
			mKillerTip.text = string.Format("Terminator\n{0}", playerInfo.m_szName);
        }
    }
}
