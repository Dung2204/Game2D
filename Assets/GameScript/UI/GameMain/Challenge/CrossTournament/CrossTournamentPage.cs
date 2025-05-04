using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIButtonColor;

public class CrossTournamentPage : UIFramwork
{
    private GameObject[] mSelectedTabBtnList;
    private GameObject[] mSelectedBtnList;
    private GameObject mItemParent;
    private GameObject mItem;
    private UIScrollView mItemScrollView;
    private GameObject MatchSchedulePanel;
    //
    private GameObject GeneralityPanel;
    private GameObject[] mSelectedBtnTheList;
    private GameObject mKnockItemParent;
    private GameObject mKnockItem;
    private GameObject mKnockContent;
    private GameObject mFinalContent;

    private int mCurTabType = 0;
    private int[] wdays= { 2, 3, 4, 5, 6, 0 };
    private int[] _iThes = { 64, 32, 16};
    private int mCurDayIndex = 0;
    private int mCurTheIndex = 0;

    private UIWrapComponent _WrapComponet;
    public UIWrapComponent mWrapComponet
    {
        get
        {
            if (_WrapComponet == null)
            {
                int day = 0;
                if (mCurDayIndex < wdays.Length - 1) day = mCurDayIndex;
                List<BasePoolDT<long>> _mList = Data_Pool.m_CrossTournamentPool.f_GetGroupStageListByDay(wdays[day]);
                _WrapComponet = new UIWrapComponent(200, 1, 200, 5, mItemParent, mItem, _mList, f_ItemUpdateByInfo, f_OnItemClick);
            }
            return _WrapComponet;
        }
    }

    private UIWrapComponent _TheWrapComponet;
    public UIWrapComponent mTheWrapComponet
    {
        get
        {
            if (_TheWrapComponet == null)
            {
                int iThe = 0;
                if (mCurTheIndex < _iThes.Length) iThe = mCurTheIndex;
                List<BasePoolDT<long>> _mList = Data_Pool.m_CrossTournamentPool.f_GetKnock(_iThes[iThe]);
                _TheWrapComponet = new UIWrapComponent(200,2, 1020, 5, mKnockItemParent, mKnockItem, _mList, f_TheItemUpdateByInfo, f_OnTheItemClick);
            }
            return _TheWrapComponet;
        }
    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eUserAttr_TournamentPoint);
        UpdateContent();

    }
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }
    protected override void InitGUI()
    {
        base.InitGUI();
        mCurTabType = 0;
        mCurDayIndex = 0;
        string[] TabNameList = new string[2];
        mSelectedTabBtnList = new GameObject[2];
        TabNameList[0] = "MatchScheduleBtn"; 
        TabNameList[1] = "GeneralityBtn";
        Transform tabParent = f_GetObject("TabBtn").transform;
        for (int i = 0; i < 2; i++)
        {
            GameObject itemBtn = tabParent.Find(TabNameList[i]).gameObject;
            f_RegClickEvent(itemBtn, OnClickTabBtn, i);
            mSelectedTabBtnList[i] = itemBtn.transform.Find(TabNameList[i]+"Select").gameObject;
        }
        mItemParent = f_GetObject("ItemParent");
        mItem = f_GetObject("Item");

        MatchSchedulePanel = f_GetObject("MatchSchedulePanel");
        GeneralityPanel = f_GetObject("GeneralityPanel");

        string[] BtnNameList = new string[6];
        mSelectedBtnList = new GameObject[6];
        BtnNameList[0] = "Btn0";
        BtnNameList[1] = "Btn1";
        BtnNameList[2] = "Btn2";
        BtnNameList[3] = "Btn3";
        BtnNameList[4] = "Btn4";
        BtnNameList[5] = "Btn5";
        Transform btnParent = f_GetObject("BtnTab").transform;
        for (int i = 0; i < 6; i++)
        {
            GameObject itemBtn = btnParent.Find(BtnNameList[i]).gameObject;
            f_RegClickEvent(itemBtn, OnClickBtn, i);
            mSelectedBtnList[i] = itemBtn.transform.Find("Select").gameObject;
        }
        
        mKnockItemParent = f_GetObject("KnockItemParent");
        mKnockItem = f_GetObject("KnockItem");
        string[] BtnTheList = new string[4];
        mSelectedBtnTheList = new GameObject[4];
        BtnTheList[0] = "BtnThe64";
        BtnTheList[1] = "BtnThe32";
        BtnTheList[2] = "BtnThe16";
        BtnTheList[3] = "BtnThe8";
       
        Transform btnParent1 = f_GetObject("BtnTab1").transform;
        for (int i = 0; i < 4; i++)
        {
            GameObject itemBtn = btnParent1.Find(BtnTheList[i]).gameObject;
            f_RegClickEvent(itemBtn, OnClickBtn1, i);
            mSelectedBtnTheList[i] = itemBtn.transform.Find("Select").gameObject;
        }

        mKnockContent = f_GetObject("KnockContent");
        mFinalContent = f_GetObject("FinalContent");
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        f_RegClickEvent("Btn_Bet", OnBtnBetClick);
        f_RegClickEvent("Btn_Team", OnBtnTeamClick);
    }
    private void OnBtnBetClick(GameObject go, object value1, object value2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentBetPage, UIMessageDef.UI_OPEN);
    }
    private void OnBtnTeamClick(GameObject go, object value1, object value2)
    {
        int iType = Data_Pool.m_CrossTournamentPool.m_Info.iType;
        if (iType == 1 || iType == 4 || iType == 6 || iType == 8 || iType == 10 || iType == 13)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            UITool.Ui_Trip("Đã hết thời gian xếp đội.");
        }
        
    }
    private void OnClickTabBtn(GameObject go, object value1, object value2)
    {
        UpdateInfoByType((int)value1);
    }
    private void UpdateInfoByType(int type)
    {
        //if (Data_Pool.m_CrossTournamentPool.m_Info.hasRegister == 99)
        //{
        //    type = 0;
        //}
        mCurTabType = type;
        for (var i = 0; i < 2; i++)
        {
            mSelectedTabBtnList[(int)i].SetActive(i == type);
            //mSelectedTabBtnList[i].GetComponent<UIButton>().SetState(State.Pressed, i == type);
        }
        MatchSchedulePanel.SetActive(mCurTabType == 0);
        GeneralityPanel.SetActive(mCurTabType == 1);
        if (mCurTabType == 0)
        {
            UpdateViewMatchSchedulePanel();
        }
        else
        {
            UpdateViewGeneralityPanel();
        }

    }
   // lịch đấu
    private void f_ItemUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        CrossTournamentGroupStagePoolDT data = (CrossTournamentGroupStagePoolDT)dt;
        if (dt == null) return;
        UILabel labelDesc = tf.Find("LbDesc").GetComponent<UILabel>();
        UILabel labelTimes = tf.Find("LbTimes").GetComponent<UILabel>();
        Transform infoA = tf.Find("InfoA").transform;
        UI2DSprite headIconA = infoA.Find("HeadA").GetComponent<UI2DSprite>();
        UILabel labelNameA = infoA.Find("LbNameA").GetComponent<UILabel>();
        UILabel labelServerA = infoA.Find("LbServerA").GetComponent<UILabel>();
        Transform infoB = tf.Find("InfoB").transform;
        UI2DSprite headIconB = infoB.Find("HeadB").GetComponent<UI2DSprite>();
        UILabel labelNameB = infoB.Find("LbNameB").GetComponent<UILabel>();
        UILabel labelServerB = infoB.Find("LbServerB").GetComponent<UILabel>();
        

        labelDesc.text = "Vòng loại " + data.m_iIndex + " - Thứ " + GetDay(data.m_iWday) + " "+ GetTime(data.m_iTime);
        labelTimes.text = data.m_Result == 0 ? "0:0" : data.m_Result == 1 ? "1:0":"0:1";

        labelNameA.text = data.m_userA.m_szName;
        labelServerA.text = "[S" + data.m_userA.m_ServerName + "]";
        headIconA.sprite2D = UITool.f_GetCardIcon(data.m_userA.Icon, "L1_");//UITool.f_GetIconSpriteBySexId(data.m_userA.m_iSex);

        labelNameB.text = data.m_userB.m_szName;
        labelServerB.text = "[S" + data.m_userB.m_ServerName + "]";
        headIconB.sprite2D = UITool.f_GetCardIcon(data.m_userB.Icon, "L1_");//UITool.f_GetIconSpriteBySexId(data.m_userB.m_iSex);
    }
    private string GetTime(int index)
    {
        int hour = 10;
        int time = 30 * (index - 1);
        int m_Hour = (int)time / 60;
        int m_Minute = (int)time % 60;
        hour += m_Hour;
        return hour + ":"+  (m_Minute == 0 ? "00": m_Minute.ToString());
    }
    private string GetDay(int wday)
    {
        string str = "";
        switch (wday)
        {
            case 0:
                str = "";
                break;
            case 1:
                str = "2";
                break;
            case 2:
                str = "3";
                break;
            case 3:
                str = "4";
                break;
            case 4:
                str = "5";
                break;
            case 5:
                str = "6";
                break;
            case 6:
                str = "7";
                break;
        }
        return str;
    }
    private void f_OnItemClick(Transform tf, BasePoolDT<long> dt)
    {

    }
    private void OnClickBtn(GameObject go, object value1, object value2)
    {
        mCurDayIndex = (int)value1;
        for (var i = 0; i < 6; i++)
        {
            mSelectedBtnList[(int)i].SetActive(i == mCurDayIndex);
        }
        int day = 0;
        if (mCurDayIndex < wdays.Length - 1) day = wdays[mCurDayIndex];
        List<BasePoolDT<long>> _mList = Data_Pool.m_CrossTournamentPool.f_GetGroupStageListByDay(day);
        mWrapComponet.f_UpdateList(_mList);
        mWrapComponet.f_ResetView();
    }
    // end
    // tổng quan
    private void OnClickBtn1(GameObject go, object value1, object value2)
    {
        mCurTheIndex = (int)value1;
        
        for (var i = 0; i < 4; i++)
        {
            mSelectedBtnTheList[(int)i].SetActive(i == mCurTheIndex);
        }
        if (mCurTheIndex == 3)
        {
            mKnockContent.SetActive(false);
            mFinalContent.SetActive(true);
            UpdateViewGeneralityPanel_FinalContent();
            return;
        }
        int iThe = 0;
        if (mCurTheIndex < _iThes.Length) iThe = mCurTheIndex;
        List<BasePoolDT<long>> _mList = Data_Pool.m_CrossTournamentPool.f_GetKnock(_iThes[iThe]);
        mTheWrapComponet.f_UpdateList(_mList);
        mTheWrapComponet.f_ResetView();
        mKnockContent.SetActive(true);
        mFinalContent.SetActive(false);
    }
    private void f_OnTheItemClick(Transform tf, BasePoolDT<long> dt)
    {

    }
    private void f_TheItemUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        CrossTournamentThePoolDT data = (CrossTournamentThePoolDT)dt;
        CrossTournamentKnockItem _Item = tf.GetComponent<CrossTournamentKnockItem>();
        _Item.f_UpdateByInfo(data, mKnockItemParent);
    }
    //end
    private void ShowTopMoneyPage(params EM_MoneyType[] listMoneyType)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, new List<EM_MoneyType>(listMoneyType));
    }
    // update content data
    private void UpdateContent()
    {
        int iType = Data_Pool.m_CrossTournamentPool.m_Info.iType;
        //int isOpen = Data_Pool.m_CrossTournamentPool.m_Info.isOpen;
        if (iType == 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentMainPage, UIMessageDef.UI_CLOSE);
            UITool.Ui_Trip("Chưa mở");
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN);
            ccUIHoldPool.GetInstance().f_UnHold();
            return;
        }
        if (mCurTabType == 0)
            UpdateViewMatchSchedulePanel();
        else
            UpdateViewGeneralityPanel();
        MatchSchedulePanel.SetActive(mCurTabType == 0);
        GeneralityPanel.SetActive(mCurTabType == 1);
    }
    private void UpdateViewMatchSchedulePanel()
    {
        for (int i = 0; i < 2; i++)
        {
            mSelectedTabBtnList[i].SetActive(mCurTabType == i);
        }
        if (Data_Pool.m_CrossTournamentPool.m_Info.hasRegister == 99)
        {
            f_GetObject("NormalContent").SetActive(false);
            f_GetObject("NoRegisterContent").SetActive(true);
            return;
        }
        f_GetObject("NormalContent").SetActive(true);
        f_GetObject("NoRegisterContent").SetActive(false);
        TeamPoolDT teamPoolDT = Data_Pool.m_TeamPool.f_GetForId((int)EM_FormationPos.eFormationPos_Main) as TeamPoolDT;
        f_GetObject("SpriteHead").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetCardIcon(teamPoolDT.m_CardPoolDT.Icon, "L1_");//UITool.f_GetIconSpriteByCardId(teamPoolDT.m_CardPoolDT);
        string cardName = Data_Pool.m_UserData.m_szRoleName;
        f_GetObject("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(teamPoolDT.m_CardPoolDT.m_CardDT.iImportant, ref cardName);
        CrossUserTournamentPoolDT info = Data_Pool.m_CrossTournamentPool.m_MyInfo;
        if (info != null)
        {
            f_GetObject("LbName").GetComponent<UILabel>().text = info.m_szName;
            f_GetObject("LbPoint").GetComponent<UILabel>().text = info.m_iPoint.ToString();
            f_GetObject("LbRank").GetComponent<UILabel>().text = info.m_iRank == 0 ? "Tạm không" : info.m_iRank.ToString();
            f_GetObject("LbDamage").GetComponent<UILabel>().text = info.m_Damage.ToString();
        }
        else
        {
            f_GetObject("LbName").GetComponent<UILabel>().text = "";
            f_GetObject("LbPoint").GetComponent<UILabel>().text = "0";
            f_GetObject("LbRank").GetComponent<UILabel>().text = "Tạm không";
            f_GetObject("LbDamage").GetComponent<UILabel>().text = "0";
        }
        mWrapComponet.f_UpdateView();
        mWrapComponet.f_ResetView();
        for (int i = 0; i < 6; i++)
        {
            mSelectedBtnList[i].SetActive(mCurDayIndex == i);
        }
    }
    private void UpdateViewGeneralityPanel()
    {
        for (int i = 0; i < 4; i++)
        {
            mSelectedBtnTheList[i].SetActive(mCurTheIndex == i);
        }
        if (mCurTheIndex == 3)
        {
            mKnockContent.SetActive(false);
            mFinalContent.SetActive(true);
            UpdateViewGeneralityPanel_FinalContent();
            return;
        }
        mKnockContent.SetActive(true);
        mFinalContent.SetActive(false);
        mTheWrapComponet.f_UpdateView();
        mTheWrapComponet.f_ResetView();
    }
    private void UpdateViewGeneralityPanel_FinalContent()
    {
        int id = 57;
        for(int i = 0; i < 8; i++)
        {
            GameObject KnockItem = f_GetObject("KnockItem" + i);
            OnShowKnockItemData(KnockItem,id+i);
        }
    }
    private void OnShowKnockItemData(GameObject go, int id)
    {
        CrossTournamentThePoolDT aData = Data_Pool.m_CrossTournamentPool.f_GetKnockById(id);
        CrossTournamentKnockItem _Item = go.transform.GetComponent<CrossTournamentKnockItem>();
        _Item.f_UpdateByInfo(aData,f_GetObject("FinalContent"));
        if(id == 64)
        {
            Transform Top1 = f_GetObject("Top1").transform;
            UI2DSprite head = Top1.Find("Head").GetComponent<UI2DSprite>();
            UILabel LbName = Top1.Find("LbName").GetComponent<UILabel>();
            if (aData.m_Result == 0) 
            {
                head.sprite2D = UITool.f_GetCardIcon(999999, "L1_");
                LbName.text = "Đang Chờ";
            }
            else
            {
                head.sprite2D = UITool.f_GetCardIcon(aData.m_userA.Icon, "L1_");
                LbName.text = aData.m_Result == 1 ? aData.m_userA.m_szName + "[S" + aData.m_userA.m_ServerName + "]" : aData.m_userB.m_szName + "[S" + aData.m_userB.m_ServerName + "]";
            }
        }
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);

    }
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
}
