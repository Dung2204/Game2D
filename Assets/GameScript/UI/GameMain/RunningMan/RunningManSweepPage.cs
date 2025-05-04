using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunningManSweepPage : UIFramwork
{
    private UIScrollView mScrollView;
    private UISlider mScrollViewBar;

    private GameObject mItemRoot;
    private GameObject mTollgateItem;
    private GameObject mBuffItem;
    private GameObject mChapBoxItem;
    private GameObject mTotalItem;

    private GameObject mBtnSweepFinish;

    private List<RunningManSweepResult> tollgateList;
    private List<RunningManSweepResult> buffList;
    private List<RunningManSweepResult> chapBoxList;
    private RunningManSweepResult totalResult;

    private const int TollgateItemHeight = 130;
    private const int BuffItemHeight = 130;
    private const int ChapBoxItemHeight = 190;
    private const int TotalItemHeight = 330;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
        mScrollViewBar = f_GetObject("ScrollViewBar").GetComponent<UISlider>();
        mItemRoot = f_GetObject("ItemRoot");
        mTollgateItem = f_GetObject("TollgateItem");
        mBuffItem = f_GetObject("BuffItem");
        mChapBoxItem = f_GetObject("ChapBoxItem");
        mTotalItem = f_GetObject("TotalItem");
        mBtnSweepFinish = f_GetObject("BtnSweepFinish");
        f_RegClickEvent(mBtnSweepFinish, f_BtnSweepFinish);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mBtnSweepFinish.SetActive(false);
        tollgateList = Data_Pool.m_RunningManPool.m_SweepTollgateList;
        buffList = Data_Pool.m_RunningManPool.m_SweepBuffList;
        chapBoxList = Data_Pool.m_RunningManPool.m_SweepChapBoxList;
        totalResult = Data_Pool.m_RunningManPool.m_SweepTotalItem;
        StartCoroutine(f_ShowResult());
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_BtnSweepFinish(GameObject go,object value1,object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManSweepPage, UIMessageDef.UI_CLOSE);
        for (int i = 0; i < mItemRoot.transform.childCount; i++)
        {
            Destroy(mItemRoot.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 展示时间间隔
    /// </summary>
    private const float ShowTimeDis = 0.4f;
    IEnumerator f_ShowResult()
    {
        int totalIdx = 0;
        int curItemHeight = 0;

        int tChapIdx = 0;
        for (int i = 0; i < tollgateList.Count; i++)
        {
            RunningManSweepResult tNode = tollgateList[i];
            int tIdx = (tNode.m_iTollgateId-1) % GameParamConst.RMTollgateNumPreChap;
            if (tNode.m_iTollgateId != 1 && tIdx == 0)
            {
                if (tChapIdx < buffList.Count)
                {
                    f_AddShowItem(totalIdx, buffList[tChapIdx].m_eResultType, curItemHeight, buffList[tChapIdx]);
                    curItemHeight += BuffItemHeight;
                    totalIdx++;
                    yield return new WaitForSeconds(ShowTimeDis);
                }
                if (tChapIdx < chapBoxList.Count)
                {
                    f_AddShowItem(totalIdx, chapBoxList[tChapIdx].m_eResultType, curItemHeight, chapBoxList[tChapIdx]);
                    curItemHeight += ChapBoxItemHeight;
                    totalIdx++;
                    yield return new WaitForSeconds(ShowTimeDis);
                }
                tChapIdx++;
            }
            f_AddShowItem(totalIdx, tNode.m_eResultType, curItemHeight, tNode);
            curItemHeight += TollgateItemHeight;
            totalIdx++;
            yield return new WaitForSeconds(ShowTimeDis);
        }
		if (tChapIdx < buffList.Count)
		{
			f_AddShowItem(totalIdx, buffList[tChapIdx].m_eResultType, curItemHeight, buffList[tChapIdx]);
			curItemHeight += BuffItemHeight;
			totalIdx++;
			yield return new WaitForSeconds(ShowTimeDis);
		}
		if (tChapIdx < chapBoxList.Count)
		{
			f_AddShowItem(totalIdx, chapBoxList[tChapIdx].m_eResultType, curItemHeight, chapBoxList[tChapIdx]);
			curItemHeight += ChapBoxItemHeight;
			totalIdx++;
			yield return new WaitForSeconds(ShowTimeDis);
		}
        f_AddShowItem(totalIdx, totalResult.m_eResultType, curItemHeight, totalResult);
        yield return new WaitForSeconds(ShowTimeDis);
        mBtnSweepFinish.SetActive(true);


        //    if (tollgateList[i].m_iTollgateId != 1 && (tollgateList[i].m_iTollgateId - 1) % 3 == 0)
        //    {
        //        int tIdx = (tollgateList[i].m_iTollgateId - 1) / 3 - 1;
        //        if (tIdx < buffList.Count)
        //        {
        //            f_AddShowItem(totalIdx, buffList[tIdx].m_eResultType, curItemHeight, buffList[tIdx]);
        //            curItemHeight += BuffItemHeight;
        //            totalIdx++;
        //            yield return new WaitForSeconds(ShowTimeDis);
        //        }
        //        if (tIdx < chapBoxList.Count)
        //        {
        //            f_AddShowItem(totalIdx, chapBoxList[tIdx].m_eResultType, curItemHeight, chapBoxList[tIdx]);
        //            curItemHeight += ChapBoxItemHeight;
        //            totalIdx++;
        //            yield return new WaitForSeconds(ShowTimeDis);
        //        }
        //    }
        //    if (i < tollgateList.Count)
        //    {
        //        f_AddShowItem(totalIdx, tollgateList[i].m_eResultType, curItemHeight, tollgateList[i]);
        //        curItemHeight += TollgateItemHeight;
        //        totalIdx++;
        //        yield return new WaitForSeconds(ShowTimeDis);
        //    } 
        //}
    }

    private void f_AddShowItem(int idx,EM_RunningManSweepResultType resultType,int yValue,RunningManSweepResult data)
    {
        GameObject go;
        if (resultType == EM_RunningManSweepResultType.Tollgate)
        {
            go = NGUITools.AddChild(mItemRoot, mTollgateItem);
        }
        else if (resultType == EM_RunningManSweepResultType.Buff)
        {
            go = NGUITools.AddChild(mItemRoot, mBuffItem);
        }
        else if (resultType == EM_RunningManSweepResultType.ChapBox)
        {
            go = NGUITools.AddChild(mItemRoot, mChapBoxItem);
        }
        else
        {
            go = NGUITools.AddChild(mItemRoot, mTotalItem);
        }
        RunningManSweepItem tItem = go.GetComponent<RunningManSweepItem>();
        tItem.f_UpdateByInfo(data);
        go.transform.localPosition = new Vector3(0, -yValue, 0);
        NGUITools.MarkParentAsChanged(go);
        if (idx == 0)
            mScrollView.ResetPosition();
        mScrollView.UpdateScrollbars();
        if (idx >= 4)
            mScrollViewBar.value = 1;
    }
}

public enum EM_RunningManSweepResultType
{
    //
    Tollgate = 0,
    Buff,
    ChapBox,
    Total,
}

public class RunningManSweepResult
{
    public RunningManSweepResult(EM_RunningManSweepResultType resultType)
    {
        m_eResultType = resultType;
        m_iTollgateId = 0;
        m_szTollgateName = string.Empty;
        m_iMoney = 0;
        m_iPrestige = 0;
        m_BuffProperty = new RunningManBuffProperty();
        if (resultType == EM_RunningManSweepResultType.Total)
            m_ChapBoxAward = new List<AwardPoolDT>();
    }
    
    
    public EM_RunningManSweepResultType m_eResultType
    {
        private set;
        get;
    }

    #region Tollgate

    public void f_UpdateTollgateData(int tollgateId, string tollgateName, int money,int prestige,string moneyDesc,string prestigeDesc)
    {
        m_iTollgateId = tollgateId;
        m_szTollgateName = tollgateName;
        m_iMoney = money;
        m_iPrestige = prestige;
        m_szMoneyDesc = moneyDesc;
        m_szPrestigeDesc = prestigeDesc;
    }

    public int m_iTollgateId
    {
        private set;
        get;
    }

    public string m_szTollgateName
    {
        private set;
        get;
    }

    public int m_iMoney
    {
        private set;
        get;
    }

    public int m_iPrestige
    {
        private set;
        get;
    }

    public string m_szMoneyDesc
    {
        private set;
        get;
    }

    public string m_szPrestigeDesc
    {
        private set;
        get;
    }

    #endregion

    #region chapbox

    public void f_UpdateChapBoxData(int awardId)
    {
        m_ChapBoxAward = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(awardId, true);
    }

    public List<AwardPoolDT> m_ChapBoxAward
    {
        get;
        private set;
    }

    #endregion

    #region Buff

    public void f_UpdateBuffData(int buffIdx,int propertyType,int propertyValue)
    {
        m_iBuffIdx = buffIdx;
        m_BuffProperty.f_UpdateProperty(propertyType, propertyValue);
    }

    public RunningManBuffProperty m_BuffProperty
    {
        private set;
        get;
    }

    public int m_iBuffIdx
    {
        private set;
        get;
    }

    #endregion

    #region Total

    public void f_AddTollgateAward(int money,int prestige)
    {
        m_iMoney += money;
        m_iPrestige += prestige;
    }

    public void f_AddChapBoxAward(List<AwardPoolDT> awardList)
    {
        for (int i = 0; i < awardList.Count; i++)
        {
			bool needNew = true;
            for (int j = 0; j < m_ChapBoxAward.Count; j++)
            {
                if (m_ChapBoxAward[j].mTemplate.mResourceType == awardList[i].mTemplate.mResourceType
                    && m_ChapBoxAward[j].mTemplate.mResourceId == awardList[i].mTemplate.mResourceId)
                {
                    m_ChapBoxAward[j].mTemplate.f_AddNum(awardList[i].mTemplate.mResourceNum);
					needNew = false;
                    break;
                }
            }
			if (!needNew)
				continue;
            AwardPoolDT tPoolDt = new AwardPoolDT();
            tPoolDt.f_UpdateByInfo((byte)awardList[i].mTemplate.mResourceType, awardList[i].mTemplate.mResourceId, awardList[i].mTemplate.mResourceNum);
            m_ChapBoxAward.Add(tPoolDt);
        }
    }

    #endregion
}
