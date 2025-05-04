using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CrossTournamentMainPage : UIFramwork
{
    private int timeLeft;
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        f_RegClickEvent("Btn_Shop", OnBtnShopClick);
        f_RegClickEvent("Btn_RegEdit", OnBtnRegeditClick);
        f_RegClickEvent("Btn_Go", OnBtnGoClick);
        f_RegClickEvent("Btn_Help", OnBtnHelpClick);
        f_RegClickEvent("Btn_Team", OnBtnTeamClick);
        f_RegClickEvent("Btn_Detail", OnBtnDetailClick);
        f_RegClickEvent("Btn_Bet", OnBtnBetClick);

    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Data_Pool.m_CrossTournamentPool.f_TheBetInfo();

        ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eUserAttr_TournamentPoint);
        UpdateContent();

    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eUserAttr_TournamentPoint);

    }
    private void ShowTopMoneyPage(params EM_MoneyType[] listMoneyType)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, new List<EM_MoneyType>(listMoneyType));
    }
    private void UpdateContent()
    {
        f_GetObject("Btn_RegEdit").SetActive(false);
        f_GetObject("LbTip").SetActive(false);
        int iType = Data_Pool.m_CrossTournamentPool.m_Info.iType;
        int isOpen = Data_Pool.m_CrossTournamentPool.m_Info.isOpen;
        long mTime = Data_Pool.m_CrossTournamentPool.m_Info.time;
        if (iType == 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentMainPage, UIMessageDef.UI_CLOSE);
            UITool.Ui_Trip("Chưa mở");
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN);
            ccUIHoldPool.GetInstance().f_UnHold();
            return;
        }
        f_GetObject("Btn_Go").SetActive(iType != 1);
        int mRegedit = Data_Pool.m_CrossTournamentPool.m_Info.hasRegister;
        
        if(iType == 1) // thời gian đăng ký
        {
            if(mRegedit == 99)
            {
                f_GetObject("Btn_RegEdit").SetActive(true);
                f_GetObject("LbTip").SetActive(false);
                //f_GetObject("LbTime").GetComponent<UILabel>().text = "Chưa báo danh";
            }
            else
            {
                f_GetObject("Btn_RegEdit").SetActive(false);
                f_GetObject("LbTip").SetActive(false);
                //f_GetObject("LbTime").GetComponent<UILabel>().text = "Đã báo danh" + " - cd";// cần server trả 1 time đấu
            }
        }
        else
        {
            f_GetObject("Btn_RegEdit").SetActive(false);
            f_GetObject("LbTip").SetActive(true);
            //if (mRegedit == 99)
            //{
            //    f_GetObject("LbTime").GetComponent<UILabel>().text = "Chưa báo danh";
            //}
            //else
            //{
            //    f_GetObject("LbTime").GetComponent<UILabel>().text = "Đã báo danh - Đang đấu" ;
            //}
        }
        if(iType == 26)
        {
            f_GetObject("LbTime").GetComponent<UILabel>().text = "Vấn đỉnh kết thúc";
        }
        else
        {
            timeLeft = (int)(mTime - GameSocket.GetInstance().f_GetServerTime());
            TimerControl(true);
        }
        
        long m_top1 = Data_Pool.m_CrossTournamentPool.m_Info.top1;
        long m_top2 = Data_Pool.m_CrossTournamentPool.m_Info.top2;
        long m_top3 = Data_Pool.m_CrossTournamentPool.m_Info.top3;
        CrossUserTournamentPoolDT top1Data = Data_Pool.m_CrossTournamentPool.GetUser(m_top1);
        CrossUserTournamentPoolDT top2Data = Data_Pool.m_CrossTournamentPool.GetUser(m_top2);
        CrossUserTournamentPoolDT top3Data = Data_Pool.m_CrossTournamentPool.GetUser(m_top3);
        OnShowTopData(f_GetObject("Top1").gameObject, top1Data, 1);
        OnShowTopData(f_GetObject("Top2").gameObject, top2Data, 2);
        OnShowTopData(f_GetObject("Top3").gameObject, top3Data, 3);
    }
    private void OnShowTopData(GameObject go, CrossUserTournamentPoolDT topData,int top)
    {
        CrossTournamentTopItem _Item = go.transform.GetComponent<CrossTournamentTopItem>();
        _Item.f_UpdateByInfo(topData, top, f_GetObject("TopParent"));
    }

    private void TimerControl(bool isStart)
    {
        CancelInvoke("ReduceTime");
        if (isStart)
        {
            InvokeRepeating("ReduceTime", 0f, 1f);
        }
    }
    private void ReduceTime()
    {
        timeLeft--;
        int mRegedit = Data_Pool.m_CrossTournamentPool.m_Info.hasRegister;
        if (timeLeft <= 0)
        {
            TimerControl(false);
            f_GetObject("LbTime").GetComponent<UILabel>().text = "Chiến đấu";
        }
        else
        {
            f_GetObject("LbTime").GetComponent<UILabel>().text = "Thời gian chuẩn bị: " + CommonTools.f_GetStringBySecond(timeLeft);
        }
    }

    private void OnBtnShopClick(GameObject go, object value1, object value2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.CrossTournament);
    }
    private void OnBtnGoClick(GameObject go, object value1, object value2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentPage, UIMessageDef.UI_OPEN);
    }
    private void OnBtnRegeditClick(GameObject go, object value1, object value2)
    {
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.CrossTournament_Regedit_SUC, f_SendRegeditCallback, this);
        Data_Pool.m_CrossTournamentPool.SendRegedit(0);
    }
    private void f_SendRegeditCallback(object value)
    {
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.CrossTournament_Regedit_SUC, f_SendRegeditCallback, this);
        int iType = Data_Pool.m_CrossTournamentPool.m_Info.iType;
        long mRegedit = Data_Pool.m_CrossTournamentPool.m_Info.hasRegister;
        if (iType == 1) // thời gian đăng ký
        {
            if (mRegedit == 99)
            {
                f_GetObject("Btn_RegEdit").SetActive(true);
                f_GetObject("LbTip").SetActive(false);
                f_GetObject("LbTime").GetComponent<UILabel>().text = "Chưa báo danh";
            }
            else
            {
                f_GetObject("Btn_RegEdit").SetActive(false);
                f_GetObject("LbTip").SetActive(false);
                f_GetObject("LbTime").GetComponent<UILabel>().text = "Đã báo danh" + " - cd";// cần server trả 1 time đấu
            }
        }
        else
        {
            f_GetObject("Btn_RegEdit").SetActive(false);
            f_GetObject("LbTip").SetActive(true);
            if (mRegedit == 99)
            {
                f_GetObject("LbTime").GetComponent<UILabel>().text = "Chưa báo danh";
            }
            else
            {
                f_GetObject("LbTime").GetComponent<UILabel>().text = "Đã báo danh - Đang đấu";
            }
        }
    }
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentMainPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    private void OnBtnHelpClick(GameObject go, object value1, object value2)
    {
		ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 26);
    }
    private void OnBtnTeamClick(GameObject go, object value1, object value2)
    {
        int iType = Data_Pool.m_CrossTournamentPool.m_Info.iType;
        if (iType == 1 || iType == 4 || iType == 6 || iType == 8 || iType == 10 || iType == 13)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            UITool.Ui_Trip("Đã hết thời gian xếp đội.");
        }
    }
    private void OnBtnDetailClick(GameObject go, object value1, object value2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentDetailPage, UIMessageDef.UI_OPEN);
    }
    private void OnBtnBetClick(GameObject go, object value1, object value2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentBetPage, UIMessageDef.UI_OPEN);
    }
}
