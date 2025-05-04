using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class RunningManBuffAddPage : UIFramwork
{
    private RunningManBuffAddItem[] mAddItems;

    private UITable mStarNumTable;
    private UILabel mStarNumLabel;

    private RunningManPoolDT mData;
    private ccCallback mFinishHandle;
    private int buffIdx = 0;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mAddItems = new RunningManBuffAddItem[GameParamConst.RMModeNumPreTollgate];
        for (int i = 0; i < mAddItems.Length; i++)
        {
            mAddItems[i] = f_GetObject(string.Format("AddItem{0}", i)).GetComponent<RunningManBuffAddItem>();
        }
        mNullProperty = f_GetObject("NullProperty");
        mShowItem = f_GetObject("ShowItem");
        mItemGrid = f_GetObject("ItemGrid").GetComponent<UIGrid>();
        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
        mStarNumTable = f_GetObject("StarNumTable").GetComponent<UITable>();
        mStarNumLabel = f_GetObject("StarNumLabel").GetComponent<UILabel>();
        f_RegClickEvent("BtnChallenge", f_BtnChallenge);
        f_RegClickEvent("CloseMask", f_BtnChallenge);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if(e == null || !(e is RunningManBuffAddPageParam))
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(850));
        RunningManBuffAddPageParam tParam = (RunningManBuffAddPageParam)e;
        mData = tParam.m_PoolDt;
        mFinishHandle = tParam.m_FinishHandle;
        buffIdx = 0;
        for (int i = 0; i < mAddItems.Length; i++)
        {
            mAddItems[i].f_UpdateByInfo(buffIdx,i+1,mData.m_Buff[i]);
            f_RegClickEvent(mAddItems[i].mBg,f_BtnAddItem,i+1);
        }
        f_ShowReferrerProperty(Data_Pool.m_RunningManPool.m_BuffPropertyList);
        mStarNumLabel.text = Data_Pool.m_RunningManPool.m_iLeftStarNum.ToString();
        //mStarNumTable.repositionNow = true;
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessNextDay, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessNextDay, this);
    }
	
	void Update()
	{
		if(Data_Pool.m_RunningManPool.m_iLeftStarNum < 3 || mData.m_iBoxTimes != 0)
		{
			ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManBuffAddPage, UIMessageDef.UI_CLOSE);
		}
	}

    private void f_ProcessNextDay(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.RunningManPool)
            return;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManBuffAddPage, UIMessageDef.UI_CLOSE);
    }

    private void f_BtnAddItem(GameObject go, object value1, object value2)
    {
        int tIdx = (int)value1;
        int cost = tIdx * GameParamConst.RMTollgateNumPreChap;
        int leftStar = Data_Pool.m_RunningManPool.m_iLeftStarNum;
        if (leftStar < cost)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(851));
            return;
        }
        buffIdx = tIdx;
        for (int i = 0; i < mAddItems.Length; i++)
        {
            mAddItems[i].f_UpdateByInfo(tIdx, i+1, mData.m_Buff[i]);
        }
    }

    private void f_BtnChallenge(GameObject go, object value1, object value2)
    {
        if (buffIdx == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(852));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_RuningManBuffHandle;
        socketCallbackDt.m_ccCallbackFail = f_RuningManBuffHandle;
        Data_Pool.m_RunningManPool.f_RunningManBuff((ushort)mData.iId,(byte)buffIdx,socketCallbackDt);
    }

    private void f_RuningManBuffHandle(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(853));
            if (mData.m_iBoxTimes == 0)
            {
                UITool.f_OpenOrCloseWaitTip(true);
                SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
                socketCallbackDt.m_ccCallbackSuc = f_RunningManChapBoxHandle;
                socketCallbackDt.m_ccCallbackFail = f_RunningManChapBoxHandle;
                Data_Pool.m_RunningManPool.f_RunningManChapBox((ushort)mData.iId, socketCallbackDt);
            }
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(854) + result);
        }
    }

    private void f_RunningManChapBoxHandle(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if (mFinishHandle != null)
            mFinishHandle(eMsgOperateResult.OR_Succeed);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManBuffAddPage, UIMessageDef.UI_CLOSE);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(855)); 
            int tChapBoxStar = 0;
            int awardPoolId = 0;
            for (int i = 0; i < mData.m_TollgatePoolDTs.Length; i++)
            {
                tChapBoxStar += mData.m_TollgatePoolDTs[i].m_iResult;
            }
            if (tChapBoxStar >= 9)
            {
                awardPoolId = mData.m_ChapterTemplate.iBox9;
            }
            else if (tChapBoxStar >= 6)
            {
                awardPoolId = mData.m_ChapterTemplate.iBox6;
            }
            else if (tChapBoxStar >= 3)
            {
                awardPoolId = mData.m_ChapterTemplate.iBox3;
            }
            //展示宝箱内容
            AwardTipPageParam tParam = new AwardTipPageParam(CommonTools.f_GetTransLanguage(856), awardPoolId);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.AwardTipPage, UIMessageDef.UI_OPEN, tParam);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(857) + result);
        }
    }


    private GameObject mNullProperty;
    private GameObject mShowItem;
    private UIGrid mItemGrid;
    private UIScrollView mScrollView;
    private List<UILabel> mShowItems = new List<UILabel>();

    private void f_ShowReferrerProperty(List<RunningManBuffProperty> dataList)
    {
        mNullProperty.SetActive(dataList.Count == 0);
        for (int i = 0; i < dataList.Count; i++)
        {
            if (i < mShowItems.Count)
            {
                mShowItems[i].gameObject.SetActive(true);
                mShowItems[i].text = string.Format("{0}", UITool.f_GetProName((EM_RoleProperty)dataList[i].m_iPropertyType));//, dataList[i].m_iPropertyValue / 10000.0f*100);
                mShowItems[i].transform.Find("Pro").GetComponent<UILabel>().text = (dataList[i].m_iPropertyValue / 10000.0f * 100) + "%";
                continue;
            }
            GameObject tItem = NGUITools.AddChild(mItemGrid.gameObject, mShowItem.gameObject);
            tItem.SetActive(true);
            UILabel tLabel = tItem.GetComponent<UILabel>();
            tLabel.text = string.Format("{0}", UITool.f_GetProName((EM_RoleProperty)dataList[i].m_iPropertyType));//, dataList[i].m_iPropertyValue / 10000.0f*100);
            tItem.transform.Find("NumProperty").GetComponent<UILabel>().text= (dataList[i].m_iPropertyValue / 10000.0f * 100)+"%";
            mShowItems.Add(tLabel);
        }
        if (mShowItems.Count > dataList.Count)
        {
            for (int i = dataList.Count; i < mShowItems.Count; i++)
            {
                mShowItems[i].gameObject.SetActive(false);
            }
        }
        mItemGrid.repositionNow = true;
        mItemGrid.Reposition();
        mScrollView.ResetPosition();
    }
}


public class RunningManBuffAddPageParam
{
    public RunningManBuffAddPageParam(RunningManPoolDT poolDt,ccCallback finishHandle)
    {
        m_PoolDt = poolDt;
        m_FinishHandle = finishHandle;
    }

    public RunningManPoolDT m_PoolDt
    {
        private set;
        get;
    }

    public ccCallback m_FinishHandle
    {
        private set;
        get;
    }
}