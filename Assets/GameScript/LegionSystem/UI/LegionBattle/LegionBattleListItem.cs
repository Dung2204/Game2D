using UnityEngine;
using System.Collections;

public class LegionBattleListItem : MonoBehaviour
{
    public UISprite mBg;
    public UILabel mLegionNameA;
    public UISprite mLegionRetTipA;
    public UILabel mLegionNameB;
    public UISprite mLegionRetTipB;
    public GameObject mRetALabelWin;
    public GameObject mRetALabelLose;
    
    public GameObject mRetBLabelWin;
    public GameObject mRetBLabelLose;
    public void f_UpdateByInfo(LegionBattleTableNode info)
    {
        //mBg.spriteName = info.m_iIdx % 2 == 0 ? "Border_BattleListBg1" : "Border_BattleListBg2";
        mLegionRetTipA.gameObject.SetActive(true);
        mLegionRetTipB.gameObject.SetActive(true);
        mRetALabelWin.SetActive(info.m_Ret == EM_LegionTableRet.AWin);
        mRetALabelLose.SetActive(info.m_Ret == EM_LegionTableRet.BWin);
        mRetBLabelWin.SetActive(info.m_Ret == EM_LegionTableRet.BWin);
        mRetBLabelLose.SetActive(info.m_Ret == EM_LegionTableRet.AWin);
        if (info.m_Ret == EM_LegionTableRet.NoFinished)
        {
            mLegionRetTipA.gameObject.SetActive(false);
            mLegionRetTipB.gameObject.SetActive(false);
        }
        else if (info.m_Ret == EM_LegionTableRet.AWin)
        {
            mLegionRetTipA.spriteName = "Border_djlbd";
            mLegionRetTipB.spriteName = "jt_jtz_pic_b";
            
        }
        else if (info.m_Ret == EM_LegionTableRet.BWin)
        {
            mLegionRetTipA.spriteName = "jt_jtz_pic_b";
            mLegionRetTipB.spriteName = "Border_djlbd";
        }
        mLegionNameA.text = info.m_szLegionNameA == "" ? CommonTools.f_GetTransLanguage(508) : info.m_szLegionNameA;
        mLegionNameB.text = info.m_szLegionNameB == "" ? CommonTools.f_GetTransLanguage(508) : info.m_szLegionNameB;
    }
}
