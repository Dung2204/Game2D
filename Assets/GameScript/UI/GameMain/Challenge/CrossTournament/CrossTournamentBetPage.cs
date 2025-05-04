using ccU3DEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossTournamentBetPage : UIFramwork
{
    private UIWrapComponent CardWrapComponentA = null;
    private UIWrapComponent CardWrapComponentB = null;
    private GameObject[] mSelectedTabBtnList;
    GameObject mInfoA;
    GameObject mInfoB;
    GameObject mBtnA;
    GameObject mBtnB;
    UILabel mLbTime;
    UILabel mLbTime1;
    GameObject mBetView;
    UILabel tUIInput;

    private int timeLeft;
    private int timeLeft1;
    private int mResult = -1;
    private int mValue = 0;
    CrossTournamentTheBetInfoPoolDT m_BetInfo;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.CrossTournament_Bet_Info, f_UpdateContent, this);
        m_BetInfo = Data_Pool.m_CrossTournamentPool.m_BetInfo;
        UpdateContent();
    }
    private void f_UpdateContent(object value)
    {
        m_BetInfo = Data_Pool.m_CrossTournamentPool.m_BetInfo;
        UpdateContent();
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        f_RegClickEvent("BtnA", OnBtnAClick);
        f_RegClickEvent("BtnB", OnBtnBClick);
        f_RegClickEvent("BtnConfirm", SendCode);
        f_RegClickEvent("BtnCancel", OnBtnCancelClick);
        f_RegClickEvent("BtnReduceOne", OnBtnReduceOneClick);
        f_RegClickEvent("BtnReduceTen", OnBtnReduceTenClick);
        f_RegClickEvent("BtnAddOne", OnBtnAddOneClick);
        f_RegClickEvent("BtnAddTen", OnBtnAddTenClick);

        f_RegClickEvent("BtnChoose1", OnBtnChoose1Click);
        f_RegClickEvent("BtnChoose2", OnBtnChoose2Click);
        f_RegClickEvent("BtnChoose3", OnBtnChoose3Click);
    }
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentBetPage, UIMessageDef.UI_CLOSE);
    }

    private void OnBtnCancelClick(GameObject go, object obj1, object obj2)
    {
        mResult = -1;
        mBetView.SetActive(false);
    }
    private void OnBtnAClick(GameObject go, object obj1, object obj2)
    {
        // mở UI chọn xu đạt cược -> xác nhận gửi lên server
        if (m_BetInfo.m_MyBet.m_userId > 0)
        {
            UITool.Ui_Trip("Chỉ đặt cược 1 lần trong 1 trận.");
            return;
        }
        mResult = 1;
        ShowBetView();
    }


    private void OnBtnBClick(GameObject go, object obj1, object obj2)
    {
        // mở UI chọn xu đạt cược -> xác nhận gửi lên server
        CrossTournamentTheBetInfoPoolDT m_BetInfo = Data_Pool.m_CrossTournamentPool.m_BetInfo;
        if (m_BetInfo.m_MyBet.m_userId > 0)
        {
            UITool.Ui_Trip("Chỉ đặt cược 1 lần trong 1 trận.");
            return;
        }
        mResult = 2;
        ShowBetView();
    }
    private void UpdateInput()
    {
        f_GetObject("InputUseCount").GetComponent<UILabel>().text = mValue.ToString();
    }
    private void OnBtnReduceOneClick(GameObject go, object obj1, object obj2)
    {
        mValue -= 1;
        if (mValue < 0)
        {
            mValue = 0;
        }
        UpdateInput();
    }
    private void OnBtnReduceTenClick(GameObject go, object obj1, object obj2)
    {
        mValue -= 10;
        if (mValue < 0)
        {
            mValue = 0;
        }
        UpdateInput();
    }
    private void OnBtnAddOneClick(GameObject go, object obj1, object obj2)
    {
        mValue += 1;
        int money = Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_TournamentPoint);
        if (mValue > money / 2)
        {
            mValue = money / 2;
        }
        UpdateInput();
    }
    private void OnBtnAddTenClick(GameObject go, object obj1, object obj2)
    {
        mValue += 10;
        int money = Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_TournamentPoint);
        if (mValue > money / 2)
        {
            mValue = money / 2;
        }
        UpdateInput();
    }
    private void OnBtnChoose1Click(GameObject go, object obj1, object obj2)
    {
        int money = Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_TournamentPoint);
        mValue = (int)(money * 0.1);
        UpdateInput();
    }
    private void OnBtnChoose2Click(GameObject go, object obj1, object obj2)
    {
        int money = Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_TournamentPoint);
        mValue = (int)(money * 0.25);
        UpdateInput();
    }
    private void OnBtnChoose3Click(GameObject go, object obj1, object obj2)
    {
        int money = Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_TournamentPoint);
        mValue = (int)(money * 0.5);
        UpdateInput();
    }

    private void ShowBetView()
    {
        mBetView.SetActive(true);
        tUIInput = f_GetObject("InputUseCount").GetComponent<UILabel>();
        int money = Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_TournamentPoint);
        InputValueInit(money / 2);
        mValue = money / 2;
        UILabel LbDesc = f_GetObject("LbDesc").GetComponent<UILabel>();
        LbDesc.text = "Đặt cược tối đa lần này: " + mValue;
        //mLbTime1.text = "GĐ Dự Đoán:\n" + CommonTools.f_GetStringBySecond(timeLeft);
        UILabel LbPoint = f_GetObject("LbPoint").GetComponent<UILabel>();
        LbPoint.text = "Có xu dự đoán: " + money;
    }
    private void InputValueInit(int money)
    {
        if (tUIInput != null)
        {
            tUIInput.text = "" + money;
        }
    }
    private void SendCode(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.CrossTournament_Bet_SUC, f_SendBetCallback, this);
        if (mValue < 100)
        {
            UITool.Ui_Trip("Tối thiểu 100 điểm vấn đỉnh.");
            return;
        }
        int money = Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_TournamentPoint);
        if (mValue > money / 2)
        {
            UITool.Ui_Trip("Tối đa 1/2 điểm vấn đỉnh hiện có.");
            return;
        }
        Data_Pool.m_CrossTournamentPool.f_TheBet(m_BetInfo.m_Id, mResult, mValue);
    }
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }
    protected override void InitGUI()
    {
        //base.InitGUI();
        mInfoA = f_GetObject("InfoA");
        mInfoB = f_GetObject("InfoB");
        mBtnA = f_GetObject("BtnA");
        mBtnB = f_GetObject("BtnB");
        mLbTime = f_GetObject("lbTimes").GetComponent<UILabel>();
        mLbTime1 = f_GetObject("LbTime1").GetComponent<UILabel>();
        mBetView = f_GetObject("ShowBet");
    }

    private void f_SendBetCallback(object value)
    {
        m_BetInfo = Data_Pool.m_CrossTournamentPool.m_BetInfo;
        mBetView.SetActive(false);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.CrossTournament_Bet_SUC, f_SendBetCallback, this);
        UILabel LbNumA = f_GetObject("LbNumA").GetComponent<UILabel>();
        UILabel LbNumB = f_GetObject("LbNumB").GetComponent<UILabel>();
        UILabel LbMyBet = f_GetObject("LbMyBet").GetComponent<UILabel>();
        LbNumA.text = "Số người đạt cược: " + m_BetInfo.m_iBetCountA;
        LbNumB.text = "Số người đạt cược: " + m_BetInfo.m_iBetCountB;
        LbMyBet.text = "Đạt cược của tôi: 0";

        if (m_BetInfo.m_MyBet.m_userId > 0)
        {
            if (m_BetInfo.m_MyBet.IsBet(m_BetInfo.m_userA.m_userId))
            {
                mBtnA.SetActive(true);
                mBtnA.transform.Find("Select").gameObject.SetActive(true);
                mBtnB.SetActive(false);
            }
            else
            {
                mBtnA.SetActive(false);
                mBtnB.SetActive(true);
                mBtnB.transform.Find("Select").gameObject.SetActive(true);
            }
            LbMyBet.text = "Đạt cược của tôi: " + m_BetInfo.m_MyBet.m_Bet;
        }
        else
        {
            mBtnA.SetActive(true);
            mBtnB.SetActive(true);
            mBtnA.transform.Find("Select").gameObject.SetActive(false);
            mBtnB.transform.Find("Select").gameObject.SetActive(false);
        }

    }

    private void UpdateContent()
    {
        _IsUpdate = false;
        mBetView.SetActive(false);
        if (m_BetInfo == null || m_BetInfo.m_Id <= 0)
        {
            //UITool.UI_ShowFailContent("Chưa có trận đấu");
            UITool.Ui_Trip("Chưa có trận đấu");
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentBetPage, UIMessageDef.UI_CLOSE);
            return;
        }

        UpdateInfo(mInfoA.transform, m_BetInfo.m_userA, 1);
        UpdateInfo(mInfoB.transform, m_BetInfo.m_userB, 2);
        Int32 unixTimestamp = GameSocket.GetInstance().f_GetServerTime();
        if (m_BetInfo.m_startTime > unixTimestamp)
        {
            mLbTime.text = "GĐ Dự Đoán:\nChưa mở";
            mLbTime1.text = "GĐ Dự Đoán:\nChưa mở";
            TimerControl(false);
        }
        else
        {
            timeLeft = (int)(m_BetInfo.m_endTime - unixTimestamp);
            TimerControl(true);
        }

        UILabel LbNumA = f_GetObject("LbNumA").GetComponent<UILabel>();
        UILabel LbNumB = f_GetObject("LbNumB").GetComponent<UILabel>();
        UILabel LbMyBet = f_GetObject("LbMyBet").GetComponent<UILabel>();
        LbNumA.text = "Số người đạt cược: " + m_BetInfo.m_iBetCountA;
        LbNumB.text = "Số người đạt cược: " + m_BetInfo.m_iBetCountB;
        LbMyBet.text = "Đạt cược của tôi: 0";
        if (m_BetInfo.m_MyBet.m_userId > 0)
        {
            if (m_BetInfo.m_MyBet.IsBet(m_BetInfo.m_userA.m_userId))
            {
                mBtnA.SetActive(true);
                mBtnA.transform.Find("Select").gameObject.SetActive(true);
                mBtnB.SetActive(false);
            }
            else
            {
                mBtnA.SetActive(false);
                mBtnB.SetActive(true);
                mBtnB.transform.Find("Select").gameObject.SetActive(true);
            }
            LbMyBet.text = "Đạt cược của tôi: " + m_BetInfo.m_MyBet.m_Bet;
        }
        else
        {
            mBtnA.SetActive(true);
            mBtnB.SetActive(true);
            mBtnA.transform.Find("Select").gameObject.SetActive(false);
            mBtnB.transform.Find("Select").gameObject.SetActive(false);
        }
        GameObject Awin = f_GetObject("AWin");
        Awin.SetActive(m_BetInfo.m_iResult==1);
        GameObject Bwin = f_GetObject("BWin");
        Bwin.SetActive(m_BetInfo.m_iResult == 2);
        
    }

    private void UpdateInfo(Transform go, CrossUserTournamentPoolDT data, int index)
    {
        //UI2DSprite Head = go.Find("Head").GetComponent<UI2DSprite>();
        UILabel LbName = go.Find("LbName").GetComponent<UILabel>();
        UILabel LbPower = go.Find("LbPower").GetComponent<UILabel>();
        UILabel LbDamage = go.Find("LbDamage").GetComponent<UILabel>();
        Transform cardScrollView = go.Find("CardScrollView").transform;
        GameObject CardItemParent = cardScrollView.Find("CardItemParent").gameObject;
        GameObject CardItem = cardScrollView.Find("CardItem").gameObject;

        //Head.sprite2D = UITool.f_GetIconSpriteBySexId(data.m_iSex);
        LbName.text = data.m_szName + " [S" + data.m_ServerName + "]";
        LbPower.text = "Lực Chiến: " + data.m_iBattlePower;
        CrossTournamentThePoolDT knockData = Data_Pool.m_CrossTournamentPool.f_GetKnockById(m_BetInfo.m_Id);
        if (index == 1)
        {
            if (knockData.m_Result > 0) 
            {
                LbDamage.text = "Chiến Tích: " + knockData.m_totalDamageA;
            }
            else
            {
                LbDamage.text = "";
            }
            if (CardWrapComponentA == null)
            {
                CardWrapComponentA = new UIWrapComponent(130, 1, 130, 6, CardItemParent, CardItem, data.m_fighterCard, CardItemUpdateByInfo, null);
            }
            CardWrapComponentA.f_UpdateList(data.m_fighterCard);
            CardWrapComponentA.f_ResetView();
        }
        else
        {
            if (knockData.m_Result > 0)
            {
                LbDamage.text = "Chiến Tích: " + knockData.m_totalDamageB;
            }
            else
            {
                LbDamage.text = "";
            }
            if (CardWrapComponentB == null)
            {
                CardWrapComponentB = new UIWrapComponent(130, 1, 130, 6, CardItemParent, CardItem, data.m_fighterCard, CardItemUpdateByInfo, null);
            }
            CardWrapComponentB.f_UpdateList(data.m_fighterCard);
            CardWrapComponentB.f_ResetView();
        }
    }
    private void CardItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        CrossTournamentFightCardPoolDT mData = dt as CrossTournamentFightCardPoolDT;
        CardDT m_CardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(mData.m_iCardId);
        UI2DSprite SprHeadIcon = item.Find("SprHeadIcon").GetComponent<UI2DSprite>();
        SprHeadIcon.sprite2D = f_GetIconSpriteByCardId(m_CardDT);
        UISprite SprBorder = item.Find("SprBorder").GetComponent<UISprite>();
        SprBorder.spriteName = UITool.f_GetImporentCase(m_CardDT != null ? m_CardDT.iImportant : 1);
        UILabel LbLevel = item.Find("LbLevel").GetComponent<UILabel>();
        LbLevel.text = "Lv:" + mData.m_iLevel;
    }
    public Sprite f_GetIconSpriteByCardId(CardDT tCardDT)
    {
        if(tCardDT != null)
		{
	        if (tCardDT.iStatelId1 != 0)
			{
				RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(tCardDT.iStatelId1);
				if (roleModle != null)
					return UITool.f_GetCardIcon(roleModle.iIcon, "L1_");
            }
            else if (tCardDT.iStatelId2 != 0)
            {
                RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(tCardDT.iStatelId2);
                if (roleModle != null)
                    return UITool.f_GetCardIcon(roleModle.iIcon, "L1_");
            }
		}
        return UITool.f_GetCardIcon(999999, "L1_");
    }
    private void TimerControl(bool isStart)
    {
        CancelInvoke("ReduceTime");
        if (isStart)
        {
            InvokeRepeating("ReduceTime", 0f, 1f);
        }
    }
    private bool _IsUpdate =false;
    private void updatedamge()
    {
        if (!_IsUpdate)
        {
            _IsUpdate = true;
            CrossTournamentThePoolDT knockData = Data_Pool.m_CrossTournamentPool.f_GetKnockById(m_BetInfo.m_Id);
            UILabel LbDamageA =  mInfoA.transform.Find("LbDamage").GetComponent<UILabel>();
            UILabel LbDamageB = mInfoB.transform.Find("LbDamage").GetComponent<UILabel>();
            LbDamageA.text = "Chiến tích: " + knockData.m_totalDamageA;
            LbDamageB.text = "Chiến tích: " + knockData.m_totalDamageB;
        }
    }
    private void ReduceTime()
    {
        timeLeft--;
        if (timeLeft <= 0)
        {
            //TimerControl(false);
            mLbTime.text = "GĐ Chiến Đấu";
            mLbTime1.text = "GĐ Chiến Đấu";
            CrossTournamentThePoolDT knockData = Data_Pool.m_CrossTournamentPool.f_GetKnockById(m_BetInfo.m_Id);
            if (knockData!=null && knockData.m_Result > 0)
            {
                Int32 unixTimestamp = GameSocket.GetInstance().f_GetServerTime();
                timeLeft1 = Math.Max((int)(m_BetInfo.m_endTime+90 - unixTimestamp),0);
                mLbTime.text = "GĐ Phát Quà:\n" + CommonTools.f_GetStringBySecond(timeLeft1);
                updatedamge();
                if (timeLeft1<=0)
                {
                    TimerControl(false);
                }
            }
        }
        else
        {
            mLbTime.text = "GĐ Dự Đoán:\n" + CommonTools.f_GetStringBySecond(timeLeft);
            mLbTime1.text = "GĐ Dự Đoán:\n" + CommonTools.f_GetStringBySecond(timeLeft);
        }
    }
}
