using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DungeonSweepPage : UIFramwork
{
    private const float ShowTimeDis = 0.3f;

    private UISlider mScrollBar;
    private UIScrollView mScrollView;
    private UITable mItemGrid;

    private GameObject mSourceItem;
    private DungeonSweepItem[] mItems;
    private GameObject mFinishBtn;

    private List<DungeonSweepResult> resultList;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mScrollBar = f_GetObject("ScrollBar").GetComponent<UISlider>();
        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
        mItemGrid = f_GetObject("ItemGrid").GetComponent<UITable>();
        mSourceItem = f_GetObject("SourceItem");
        mFinishBtn = f_GetObject("BtnFinish");
        f_RegClickEvent(mFinishBtn, f_FinishBtn);
        //mItems = new DungeonSweepItem[1];
        //for (int i = 0; i < mItems.Length; i++)
        //{
            //GameObject go = NGUITools.AddChild(mItemGrid.gameObject, mSourceItem);
            //go.name = i.ToString();
            //NGUITools.MarkParentAsChanged(go);
            //mItems[i] = go.GetComponent<DungeonSweepItem>();
            //mItems[i].f_Disable();
        //}
        //mItemGrid.repositionNow = true;
        //mItemGrid.Reposition();
        //mScrollView.ResetPosition();
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Data_Pool.m_GuidancePool.IsOpenSween = true;
        if (e == null || !(e is List<DungeonSweepResult>))
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1085));
        mFinishBtn.SetActive(false);
        resultList = (List<DungeonSweepResult>)e;

        List <AwardPoolDT> awardPoolDts = new List<AwardPoolDT>();
        int exp = 0;
        int addExp = 0;
        int money = 0;
        for (int i = 0; i < resultList.Count; i++)
        {
            exp += resultList[i].m_iExp;
            addExp += resultList[i].m_AddExp;
            money += resultList[i].m_iMoney;
            List<AwardPoolDT> awardList = resultList[i].m_AwardList;
            for (int j = 0; j < awardList.Count; j++)
            {
                AwardPoolDT item = awardList[j];
                AwardPoolDT lastItem = awardPoolDts.Find(delegate(AwardPoolDT dt)
                {
                    return dt.mTemplate.mResourceType == item.mTemplate.mResourceType &&
                           dt.mTemplate.mResourceId == item.mTemplate.mResourceId;
                });
                if (lastItem != null)
                {
                    lastItem.mTemplate.f_AddNum(item.mTemplate.mResourceNum);
                    continue;
                }
                AwardPoolDT newItem = new AwardPoolDT();
                newItem.f_UpdateByInfo((byte)item.mTemplate.mResourceType, item.mTemplate.mResourceId, item.mTemplate.mResourceNum);
                awardPoolDts.Add(newItem);
            }
        }

        DungeonSweepResult calAll = new DungeonSweepResult(exp, addExp, money, awardPoolDts, true);
        resultList.Add(calAll);


        for (int i = mItemGrid.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(mItemGrid.transform.GetChild(i).gameObject);
        }

        mItems = new DungeonSweepItem[resultList.Count];
        for (int i = 0; i < mItems.Length; i++)
        {
            GameObject go = NGUITools.AddChild(mItemGrid.gameObject, mSourceItem);
            go.name = i.ToString();
            NGUITools.MarkParentAsChanged(go);
            mItems[i] = go.GetComponent<DungeonSweepItem>();
            mItems[i].f_Disable();
        }
        mItemGrid.repositionNow = true;
        mItemGrid.Reposition();
        mScrollView.ResetPosition();

        StartCoroutine(f_ShowResult());
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        Data_Pool.m_GuidancePool.IsOpenSween = false;
        for (int i = 0; i < mItems.Length; i++)
        {
            mItems[i].f_Disable();
        }
    }
    IEnumerator f_ShowResult()
    {
        
        curValue = 0;
        targetValue = 0;
        mScrollBar.value = 0;
        for (int i = 0; i < resultList.Count; i++)
        {
            if (i >= mItems.Length)
                continue;
            mItems[i].f_UpdateByInfo(i, resultList[i]);
            mItemGrid.repositionNow = true;
            
            yield return new WaitForSeconds(ShowTimeDis);
            if (i >= 3)
            {
                targetValue = (i + 1.0f - 3) / (resultList.Count - 3);
            }
            else
            {
                mScrollView.ResetPosition();
                mScrollBar.value = 0;
            }
        }
        mFinishBtn.SetActive(true);
        //触发叛军
        if (Data_Pool.m_RebelArmyPool.tRebelInfo.discovererId != 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelAymyTriggen, UIMessageDef.UI_OPEN, UINameConst.DungeonChapterPageNew);
            Data_Pool.m_RebelArmyPool.tRebelInfo.discovererId = 0;
        }
    }

    private float curValue = 0;
    private float targetValue = 0;
    private float speedValue = 0.1f;

    protected override void f_Update()
    {
        base.f_Update();
        if (Mathf.Abs(targetValue - curValue) <= 0.01)
            return;
        curValue = Mathf.Lerp(curValue, targetValue, speedValue);
        mScrollBar.value = curValue;
    }

    private void f_FinishBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonSweepPage, UIMessageDef.UI_CLOSE);
    }
}
