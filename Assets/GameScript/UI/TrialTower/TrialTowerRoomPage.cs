using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using System;

public class TrialTowerRoomPage :  UIFramwork{

    private UIWrapComponent _UIWrap;

    private int ActEndTimeTicks;

    private UILabel mResTimeLabel;
    private DateTime mEndDateTime;


    private int Time_UpdateEndTime;
    private UILabel mConsume;

    private string strBg = "UI/TextureRemove/TrialTower/qxmd_bg_a";

    private UIWrapComponent mUIWrap {
        get {
            if (_UIWrap==null) {
                List<BasePoolDT<long>> ttt = Data_Pool.m_TrialTowerPool.f_GetAll();
                _UIWrap = new UIWrapComponent(180,1,1202,5,f_GetObject("ItemParent"),f_GetObject("Item"),ttt, UpdateITem,null);
            }
            return _UIWrap;
        }
    }
    private SocketCallbackDT EnterCallBack=new SocketCallbackDT();
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        mResTimeLabel = f_GetObject("ResTime").GetComponent<UILabel>();
        if (f_GetObject("Bg").GetComponent<UITexture>().mainTexture==null) {
            f_GetObject("Bg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strBg);
        }
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);

        //断线重连如果切去其他界面，这时候不处理，服务器消息返回再打开界面就会和其他界面重叠
        if (!UITool.f_IsCanOpenChallengePage(UINameConst.TrialTowerRoomPage))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerRoomPage, UIMessageDef.UI_CLOSE);
            return;
        }

        mUIWrap.f_UpdateView();
        ActEndTimeTicks = Data_Pool.m_TrialTowerPool.mNowEndTime;
        int ActResTime = ActEndTimeTicks - GameSocket.GetInstance().f_GetServerTime();
        if (ActResTime <= 0)
        {
            TimerControl(false);
            f_Close(null, null, null);
            return;
        }
        TimerControl(true);
        //UpdateTime(null);
        //Time_UpdateEndTime = ccTimeEvent.GetInstance().f_RegEvent(30,true,null, UpdateTime);
        SetConsume();

        string ConsumeNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(209) < 1 ?"[ff0000]x1":"[3b992c]x1";
        f_GetObject("Consume").GetComponent<UILabel>().text= ConsumeNum;
    }

    private void TimerControl(bool isStart)
    {
        CancelInvoke("UpdateTime");
        if (isStart)
        {
            InvokeRepeating("UpdateTime", 0f, 1f);
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        TimerControl(false);
        if(Data_Pool.m_TrialTowerPool.oneEnter)
            ccUIHoldPool.GetInstance().f_UnHold();
        //ccTimeEvent.GetInstance().f_UnRegEvent(Time_UpdateEndTime);
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Btn_jinru",f_jinru);
        f_RegClickEvent("Btn_Back",f_Close);
        f_RegClickEvent("SevenStar", f_SevenStar); 
    }

    private void UpdateITem(Transform tran,BasePoolDT<long> dt) {
        TrialTowerPoolDT PoolDT = (TrialTowerPoolDT)dt;
        UILabel Num = tran.Find("Num").GetComponent<UILabel>();
        GameObject AwardParent = tran.Find("ItemScroll/ItemParent").gameObject;
        GameObject AwardItem = tran.Find("ItemScroll/Item").gameObject;
        // UISprite Bg = tran.Find("bg").GetComponent<UISprite>();
        // int remainder = (int)dt.iId % 3;
        // if (remainder == 0) {
            // Bg.spriteName = "qxmd_pic_b";
        // } else if (remainder == 1) {
            // Bg.spriteName = "qxmd_pic_d";
        // } else  {
            // Bg.spriteName = "qxmd_pic_c";
        // }


        Num.text = "Tầng " + dt.iId + "";//ccMath.NumberToChinese((int)dt.iId);

        List<AwardPoolDT> awardlist = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(PoolDT.mTemp.iPond);
        GridUtil.f_SetGridView<AwardPoolDT>(AwardParent, AwardItem, awardlist, UpdateAward);
        //AwardParent.GetComponent<UIGrid>().Reposition();
        //AwardParent.GetComponent<UIGrid>().repositionNow = true;
    }

    private void UpdateAward(GameObject oneItem, AwardPoolDT dataInfo) {
        Transform tran = oneItem.transform;
        UISprite Case = tran.Find("Case").GetComponent<UISprite>();
        UI2DSprite ICon = tran.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Num = tran.Find("Num").GetComponent<UILabel>();

        Case.spriteName = UITool.f_GetImporentCase(dataInfo.mTemplate.mImportant);
        UITool.f_SetIconSprite(ICon,(EM_ResourceType)dataInfo.mTemplate.mResourceType,dataInfo.mTemplate.mResourceId);
        Num.text = dataInfo.mTemplate.mResourceNum.ToString();

        f_RegClickEvent(oneItem, OnClickItem, dataInfo.mTemplate);
    }


    private void OnClickItem(GameObject go, object obj1, object obj2) {
        ResourceCommonDT data = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, data);
    }
    private void f_jinru(GameObject go,object obj1,object obj2) {
        if (Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(209) < 1 && !Data_Pool.m_TrialTowerPool.isEnter && !Data_Pool.m_TrialTowerPool.isFirst)
        {
UITool.Ui_Trip("Không đủ điều kiện");
            return;
        }

        if (Data_Pool.m_TrialTowerPool.oneEnter)
        {
            EnterCallBack.m_ccCallbackFail = EnterFail;
            EnterCallBack.m_ccCallbackSuc = EnterSuc;
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_TrialTowerPool.f_Enter(EnterCallBack);
        }
        else {
            EnterSuc(null);
        }
        //ccUIHoldPool.GetInstance().f_Hold(this);

    }
    private void f_Close(GameObject go, object obj1, object obj2) {
        
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerRoomPage, UIMessageDef.UI_CLOSE);
    }

    private void f_SevenStar(GameObject go, object obj1, object obj2) {
        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.SevenStarOpenLv))
        {
UITool.Ui_Trip("Cấp không đủ");
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SevenStarPage, UIMessageDef.UI_OPEN);
    }

    private void UpdateTime() {
        int ActResTime = ActEndTimeTicks-  GameSocket.GetInstance().f_GetServerTime();
        if (ActResTime <= 0)
        {
            TimerControl(false);
            f_Close(null, null, null);
        }
        else {
            //mEndDateTime = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(ActResTime);
            //mResTimeLabel.text = string.Format("Thời gian hoạt động: [-][3B992CFF]{0} ngày {1}:{2}", mEndDateTime.Day - 1, mEndDateTime.Hour, mEndDateTime.Minute);
            mResTimeLabel.text = string.Format("Thời gian hoạt động: [-][3B992CFF]{0}", CommonTools.f_GetStringBySecond(ActResTime));
        }

    }

    private void SetConsume() {
        f_GetObject("First").SetActive(Data_Pool.m_TrialTowerPool.isFirst);
        f_GetObject("Consume").SetActive(!Data_Pool.m_TrialTowerPool.isFirst);
    }

    private void EnterSuc(object obj) {
        UITool.f_OpenOrCloseWaitTip(false);
        Data_Pool.m_TrialTowerPool.oneEnter = false;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerRoomPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerPage, UIMessageDef.UI_OPEN);
    }

    private void EnterFail(object obj) {
        UITool.f_OpenOrCloseWaitTip(false);
        // Debug.Log("进入失败"+obj.ToString());
UITool.Ui_Trip("Không thành công" + obj.ToString());
    }
}
