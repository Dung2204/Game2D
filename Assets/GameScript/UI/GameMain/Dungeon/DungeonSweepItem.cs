using UnityEngine;
using System.Collections.Generic;

public class DungeonSweepItem : MonoBehaviour
{
    public GameObject root;
    public UILabel mIdx;
    public UILabel mExpLabel;
    public UILabel mMoneyLabel;
    public UIGrid mAwardGird;
    public GameObject mAllTitle;
    public GameObject expParent;
    public GameObject mAwardItem;

    private ResourceCommonItemComponent mShowComponent;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="result"></param>
    /// <param name="isCalAll">最后一条数据</param>
    public void f_UpdateByInfo(int idx, DungeonSweepResult result)
    {
        root.SetActive(true);
        mIdx.text = string.Format(CommonTools.f_GetTransLanguage(1090),idx+1);
        string strAddExp = result.m_AddExp > 0 ? "[FFF700FF]（+" + result.m_AddExp + "）" : "";
        mExpLabel.text = result.m_iExp.ToString() + strAddExp;
        mMoneyLabel.text = result.m_iMoney.ToString();
        if (mShowComponent == null)
            mShowComponent = new ResourceCommonItemComponent(mAwardGird, mAwardItem);
        mShowComponent.f_Show(result.m_AwardList);
        bool isCalAll = result.m_bIsCalAllData;
        mAllTitle.SetActive(isCalAll);
        mIdx.gameObject.SetActive(!isCalAll);
        float x = isCalAll ? -230f : 0f;
        expParent.transform.localPosition = new Vector3(x, 0, 0);
        if(isCalAll) NGUITools.MarkParentAsChanged(this.gameObject);
    }

    public void f_Disable()
    {
        root.SetActive(false);
    }
}

public class DungeonSweepResult
{
    public DungeonSweepResult(int exp,int addExp,int money,List<AwardPoolDT> awardList, bool isCalAllData = false)
    {
        m_iExp = exp;
        m_AddExp = addExp;
        m_iMoney = money;
        m_AwardList = awardList;
        m_bIsCalAllData = isCalAllData;
    }
    public int m_iExp
    {
        private set;
        get;
    }
    public int m_AddExp
    {
        private set;
        get;
    }
    public int m_iMoney
    {
        private set;
        get;
    }
    /// <summary>
    /// 是否是累计数据
    /// </summary>
    public bool m_bIsCalAllData
    {
        private set;
        get;
    }
    /// <summary>
    /// 掉落列表
    /// </summary>
    public List<AwardPoolDT> m_AwardList
    {
        private set;
        get;
    }
}