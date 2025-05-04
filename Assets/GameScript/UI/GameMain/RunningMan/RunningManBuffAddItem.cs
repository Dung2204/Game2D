using UnityEngine;
using System.Collections.Generic;

public class RunningManBuffAddItem : MonoBehaviour 
{
    public GameObject mBg;
    public GameObject mSelectIcon;
    public UILabel mPropertyLabel;
    public UILabel mStarCostLabel;
    public UILabel mPro;
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="idx">[1,3]</param>
    public void f_UpdateByInfo(int selectIdx,int idx,int buffId)
    {
        RunningManBuffDT buffDt = (RunningManBuffDT)glo_Main.GetInstance().m_SC_Pool.m_RunningManBuffSC.f_GetSC(buffId);
        int cost = idx * GameParamConst.RMTollgateNumPreChap;
        int leftStar = Data_Pool.m_RunningManPool.m_iLeftStarNum;
        int buffValue = 0;
        if(idx == 1)
            buffValue = buffDt.iValue1;
        else if(idx == 2)
            buffValue = buffDt.iValue2;
        else if(idx == 3)
            buffValue = buffDt.iValue3;
        bool isEnough = leftStar >= cost;
        mPropertyLabel.text = string.Format("{0} +{1}%", UITool.f_GetProName((EM_RoleProperty)buffDt.iAttrId), buffValue / 10000.0f * 100);
        mStarCostLabel.text = string.Format("{0}{1}",isEnough?"[FFFFFF]":"[FF0000]", cost);
        //mPro.text = "+"+(buffValue / 10000.0f * 100) + "%";
        mSelectIcon.SetActive(idx == selectIdx);
    }
}
