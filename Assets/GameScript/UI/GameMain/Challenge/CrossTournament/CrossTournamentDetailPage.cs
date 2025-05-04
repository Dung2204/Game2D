using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossTournamentDetailPage : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    public UIWrapComponent mContentWrapComponet
    {
        get
        {
            if (_contentWrapComponet == null)
            {
                List<NBaseSCDT> listData = GetListData(mCurTabType);
                _contentWrapComponet = new UIWrapComponent(180, 1, 150, 8, f_GetObject("ItemParent"), f_GetObject("Item"), listData, OnContentItemUpdate, null, 0);
            }
            return _contentWrapComponet;
        }
    }
    private GameObject[] mSelectedTabBtnList;
    private int mCurTabType = 0;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        UpdateContent();
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnClose", OnBtnCloseClick);
    }
    private void OnBtnCloseClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentDetailPage, UIMessageDef.UI_CLOSE);
    }
    //protected override void On_Destory()
    //{
    //    base.On_Destory();
    //}
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }
    protected override void InitGUI()
    {
        string[] TabNameList = new string[4];
        mSelectedTabBtnList = new GameObject[4];
        TabNameList[0] = "Btn0";
        TabNameList[1] = "Btn1";
        TabNameList[2] = "Btn2";
        TabNameList[3] = "Btn3";
        Transform tabParent = f_GetObject("TabBtn").transform;
        for (int i = 0; i < 4; i++)
        {
            GameObject itemBtn = tabParent.Find(TabNameList[i]).gameObject;
            f_RegClickEvent(itemBtn, OnClickTabBtn, i);
            mSelectedTabBtnList[i] = itemBtn.transform.Find("Select").gameObject;
        }
    }

    private void OnContentItemUpdate(Transform item, NBaseSCDT data)
    {
        CrossTournamentItemDetail crossTournamentItemDetail = item.GetComponent<CrossTournamentItemDetail>();
        crossTournamentItemDetail.InitData(data);
    }

    private void UpdateContent()
    {
        for (var i = 0; i < 4; i++)
        {
            mSelectedTabBtnList[(int)i].SetActive(i == mCurTabType);
        }
        mContentWrapComponet.f_UpdateView();
        mContentWrapComponet.f_ResetView();

    }
    private List<NBaseSCDT> GetListData(int index)
    {
        List<NBaseSCDT> result = new List<NBaseSCDT>();
        switch (index)
        {
            case 0:
                result = glo_Main.GetInstance().m_SC_Pool.m_CrossTournamentQualifyingRoundSC.f_GetAll();
                break;
            case 1:
                result = glo_Main.GetInstance().m_SC_Pool.m_CrossTournamentKnockSC.f_GetAll();
                break;
            case 2:
                NBaseSCDT data = glo_Main.GetInstance().m_SC_Pool.m_CrossTournamentSC.f_GetSC(2);
                result.Add(data);
                break;
            case 3:
                NBaseSCDT data1 = glo_Main.GetInstance().m_SC_Pool.m_CrossTournamentSC.f_GetSC(1);
                result.Add(data1);
                break;
            default:
                break;
        }
        return result;
    }
    private void OnClickTabBtn(GameObject go, object value1, object value2)
    {
       
        UpdateInfoByType((int)value1);
    }
    private void UpdateInfoByType(int type)
    {
        mCurTabType = type;
        for (var i = 0; i < 4; i++)
        {
            mSelectedTabBtnList[(int)i].SetActive(i == type);
        }
        List<NBaseSCDT> listData = GetListData(mCurTabType);
        mContentWrapComponet.f_UpdateList(listData);
        mContentWrapComponet.f_ResetView();
    }
}
