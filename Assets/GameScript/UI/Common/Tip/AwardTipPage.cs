using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 奖励展示Tip，无点击效果，
/// </summary>
public class AwardTipPage : UIFramwork
{
    private GameObject mShowItemParent;
    private GameObject mShowItem;

    //单位为 毫秒
    private long ShowTimespan = 500;

    /// <summary>
    /// 上次显示的时间 毫秒
    /// </summary>
    private long LastShowTime = 0;

    const int PanelDepthInitValue = 2000;

    private int mBasePanelDepth = 2000;


    private Queue<List<AwardPoolDT>> mAwardDataQueue;
    private List<AwardTipItem> mShowItems;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mAwardDataQueue = new Queue<List<AwardPoolDT>>();
        mShowItems = new List<AwardTipItem>();
        mShowItemParent = f_GetObject("Panel");
        mShowItem = f_GetObject("AwardTipItem");
    }
    
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e != null && e is AwardTipPageParam)
        {
            f_AddShowParam(e as AwardTipPageParam);
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_AddShowParam(AwardTipPageParam param)
    {
        List<AwardPoolDT> tList = f_ParseAwardId(param.m_iAwardId);
        if (tList.Count > 0)
        {
            long tNow = System.DateTime.Now.Ticks / (10 * 1000);
            if (tNow - LastShowTime > ShowTimespan)
            {
Debug.LogWarning("Immediately visible");
                LastShowTime = tNow;
                f_ShowNow(mBasePanelDepth, param.m_szTitle, tList, f_ItemShowFinish);
                mBasePanelDepth++;
            }
            else
            {
Debug.LogWarning("Display delay");
                LastShowTime = tNow;
                mAwardDataQueue.Enqueue(tList);
                ccTimeEvent.GetInstance().f_RegEvent(mAwardDataQueue.Count*ShowTimespan / 1000.0f, false,param.m_szTitle,f_ShowDelay);
            }
        }
    }

    private void f_ShowNow(int panelDepth,string title,List<AwardPoolDT> awardList,EventDelegate.Callback onFinish)
    {
        for (int i = 0; i < mShowItems.Count; i++)
        {
            if (!mShowItems[i].m_bInUse)
            {
                mShowItems[i].f_Show(panelDepth,title, awardList,onFinish);
                return;
            }           
        }
        GameObject go = NGUITools.AddChild(mShowItemParent, mShowItem);
        AwardTipItem tItem = go.GetComponent<AwardTipItem>();
        tItem.f_Show(panelDepth, title, awardList, onFinish);
        mShowItems.Add(tItem);
    }

    private void f_ShowDelay(object value)
    {
        string tTitle = (string)value;
        if (mAwardDataQueue.Count > 0)
        {
            List<AwardPoolDT> tList = mAwardDataQueue.Dequeue();
            f_ShowNow(mBasePanelDepth, tTitle, tList, f_ItemShowFinish);
            mBasePanelDepth++;
        }
        else
        {
MessageBox.ASSERT("Display error reward data！！！！！");
        }
    }

    private void f_ItemShowFinish()
    {
        for (int i = 0; i < mShowItems.Count; i++)
        {
            if (mShowItems[i].m_bInUse)
                return;
        }
        //全部展示完后重置Panel深度
        mBasePanelDepth = PanelDepthInitValue;
    }

    private List<AwardPoolDT> f_ParseAwardId(int awardId)
    {
        List<AwardPoolDT> tAwardList = new List<AwardPoolDT>();
        AwardDT tAwardDT = (AwardDT)glo_Main.GetInstance().m_SC_Pool.m_AwardSC.f_GetSC(awardId);
        string parseSource = tAwardDT == null ? "" : tAwardDT.szAward;
        string[] awardGroup = parseSource.Split('A');
        for (int i = 0; i < awardGroup.Length; i++)
        {
            string[] awardSubGroup = awardGroup[i].Split('#');
            for (int j = 0; j < awardSubGroup.Length; j++)
            {
                string[] awardItem = awardSubGroup[j].Split(';');
                if (awardItem.Length == 3)
                {
                    int tType = int.Parse(awardItem[0]);
                    if (tType == 0)
                        continue;
                    int tId = int.Parse(awardItem[1]);
                    int tNum = int.Parse(awardItem[2]);
                    //如果找到相同的就忽略不加入
                    AwardPoolDT lastItem = tAwardList.Find(delegate (AwardPoolDT dt) { return dt.mTemplate.mResourceType == tType && dt.mTemplate.mResourceId == tId; });
                    if (lastItem != null)
                        continue;
                    AwardPoolDT item = new AwardPoolDT();
                    item.f_UpdateByInfo((byte)tType, tId, tNum);
                    tAwardList.Add(item);
                }
                else
                {
MessageBox.ASSERT("Error converting string, reward Id= " + awardId);
                }
            }
        }
        return tAwardList;
    }

}

public class AwardTipPageParam
{
    public AwardTipPageParam(string title,int awardId)
    {
        m_szTitle = title;
        m_iAwardId = awardId;
    }
    
    public string m_szTitle
    {
        private set;
        get;
    }

    public int m_iAwardId
    {
        private set;
        get;
    } 
}
