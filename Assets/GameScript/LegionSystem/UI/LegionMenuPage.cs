using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class LegionMenuPage : UIFramwork
{
    private GameObject mBackBtn;
    private GameObject mHallBtn;

    private UILabel mLegionNameLabel;
    private UISlider mExpSlider;
    private UILabel mExpLabel;
    private UILabel mContributeLabel;
    private UILabel mLegionLv;

    private LegionInfoPoolDT mData;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
        ShowCloundRandom();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mLegionNameLabel = f_GetObject("LegionNameLabel").GetComponent<UILabel>();
        mExpSlider = f_GetObject("ExpSlider").GetComponent<UISlider>();
        mExpLabel = f_GetObject("ExpLabel").GetComponent<UILabel>();
        mContributeLabel = f_GetObject("ContributeLabel").GetComponent<UILabel>();
        mLegionLv = f_GetObject("LegionLv").GetComponent<UILabel>();

        f_RegClickEvent("BackBtn", f_BackBtn);
        f_RegClickEvent("HallBtn", f_HallBtn);
        f_RegClickEvent("ShopBtn", f_ShopBtn);
        f_RegClickEvent("SkillBtn", f_SkillBtn);
        f_RegClickEvent("RedPacketBtn", f_RedPacketBtn);
        f_RegClickEvent("Btn_Sacrifice", f_Sacrifice);
        f_RegClickEvent("Btn_LegionUp", f_LegionUpLv);
        f_RegClickEvent("Btn_LegionRank", f_LegionRankBtn);
        f_RegClickEvent("DungeonBtn", f_DungeonBtn);
        f_RegClickEvent("BtnLegionBattle", f_OnBtnLegionBattleClick);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
		glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.Legion);
        f_OpenOrCloseMonenyPage(true);
        bool updateBySelfRequest = false;
        if (e != null && e is bool)
            updateBySelfRequest = (bool)e;
        else if (e != null && e is Battle2MenuProcessParam)
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            Battle2MenuProcessParam tParam = (Battle2MenuProcessParam)e;
            if (tParam.m_emType == EM_Battle2MenuProcess.LegionDungeon)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgatePage, UIMessageDef.UI_OPEN, tParam);
            }
            else if (tParam.m_emType == EM_Battle2MenuProcess.LegionBattle)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattlePage, UIMessageDef.UI_OPEN, tParam);
            }
            updateBySelfRequest = true;
        }
        if (updateBySelfRequest)
        {
            UITool.f_OpenOrCloseWaitTip(true);
            LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(true, true, f_Callback_UpdateSelfRequest);
        }
        f_RequestSacrificeInfo();
        f_RegEvent();
        f_UpdateLegionInfoView(LegionMain.GetInstance().m_LegionInfor.f_getUserLegion());
    }
    #region 云随机漂浮相关
    /// <summary>
    /// 显示云漂浮功能
    /// </summary>
    private void ShowCloundRandom()
    {
        RandomCloud(1, true);
        RandomCloud(1, true);
        RandomCloud(2, true);
        RandomCloud(3, true);
        Invoke("ShowCloundLater", 3f);
        Invoke("ShowCloundLater", 6f);
    }
    /// <summary>
    /// 延时显示云
    /// </summary>
    private void ShowCloundLater()
    {
        RandomCloud(Random.Range(1, 4));
    }
    /// <summary>
    /// 根据序号随机云位置
    /// </summary>
    /// <param name="cloudIndex">云序号</param>
    private void RandomCloud(int cloudIndex, bool isFirstInit = false)
    {
        GameObject clound;
        switch (cloudIndex)
        {
            case 1: clound = GameObject.Instantiate(f_GetObject("TexExampleCloud1")) as GameObject; break;
            case 2: clound = GameObject.Instantiate(f_GetObject("TexExampleCloud2")) as GameObject; break;
            case 3: clound = GameObject.Instantiate(f_GetObject("TexExampleCloud3")) as GameObject; break;
            default: goto case 1;
        }
        GameObject cloudRoot = f_GetObject("CloudRoot");
        int randomHeight = Random.Range(30, 390);
        TweenPosition tp = clound.transform.GetComponent<TweenPosition>();
        clound.transform.SetParent(cloudRoot.transform);
        clound.transform.localPosition = new Vector3(tp.from.x, randomHeight, 0);
        clound.transform.localEulerAngles = Vector3.zero;
        clound.transform.localScale = Vector3.one;
        tp.duration = 25 + (randomHeight - 30) / 5;

        if (isFirstInit)
        {
            int randomOffset = Random.Range(0, 2000);
            float offsetPosX = tp.from.x - randomOffset;
            clound.transform.localPosition = new Vector3(offsetPosX, randomHeight, 0);
            tp.duration *= (offsetPosX - tp.to.x) / (tp.from.x - tp.to.x);
            tp.from.x = offsetPosX;
        }

        tp.from.y = randomHeight;
        tp.to.y = randomHeight;
        clound.name = cloudIndex.ToString();
        EventDelegate ed = new EventDelegate(this, "OnCloudTweenEnd");
        ed.parameters[0].obj = clound;
        tp.AddOnFinished(ed);
    }
    /// <summary>
    /// 云tween动画结束
    /// </summary>
    private void OnCloudTweenEnd(GameObject clound)
    {
        RandomCloud(Random.Range(1, 4));
        GameObject.Destroy(clound);
    }
    #endregion
    private void f_Callback_UpdateSelfRequest(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            f_UpdateLegionInfoView(LegionMain.GetInstance().m_LegionInfor.f_getUserLegion());
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1568) + result);
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        f_OpenOrCloseMonenyPage(false);
        f_UnRegEvent();
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        f_OpenOrCloseMonenyPage(true);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        f_OpenOrCloseMonenyPage(false);
    }

    #region 事件相关处理

    private void f_RegEvent()
    {
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_WARN_SHOW, f_ProcessByMsg_WarnShow, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_USERINFOR, f_ProcessByMsg_UserInfo, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_SELF_LEGION_INFO_UPDATE, f_ProcessByMsg_SelfLegionInfo, this);
    }

    private void f_UnRegEvent()
    {
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_WARN_SHOW, f_ProcessByMsg_WarnShow);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_USERINFOR, f_ProcessByMsg_UserInfo);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_SELF_LEGION_INFO_UPDATE, f_ProcessByMsg_SelfLegionInfo);
    }

    private void f_ProcessByMsg_WarnShow(object value)
    {
        EM_LegionOutType outType = (EM_LegionOutType)value;
        if (outType == EM_LegionOutType.Dissolve)
        {
            string tContent = string.Format(CommonTools.f_GetTransLanguage(473));
            PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(474), tContent, CommonTools.f_GetTransLanguage(475), f_ProcessWardnShow);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
        }
        else if (outType == EM_LegionOutType.Quit)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(476));
            //自己退出则发送强制关闭军团界面事件
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_FORCE_CLOSE);
        }
    }

    private void f_ProcessWardnShow(object value)
    {
        //确定后发送强制关闭军团界面事件
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_FORCE_CLOSE);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        //强制退出军团界面
        ccUIHoldPool.GetInstance().f_ClearHold();
        UITool.f_OpenOrCloseWaitTip(false);
        StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.None);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionMenuPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MainMenu, UIMessageDef.UI_OPEN);
    }

    private void f_ProcessByMsg_UserInfo(object value)
    {
        //更新军团贡献
        mContributeLabel.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_LegionContribution).ToString();
    }

    private void f_ProcessByMsg_SelfLegionInfo(object value)
    {
        //更新军团信息
        f_UpdateLegionInfoView(LegionMain.GetInstance().m_LegionInfor.f_getUserLegion());
    }

    #endregion


    #region 相关按钮处理

    void f_LegionUpLv(GameObject go, object value1, object value2)
    {
        if (!LegionTool.f_IsEnoughPermission(EM_LegionOperateType.LevelUpLegion))
            return;
        //打开升级界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionUpSuc, UIMessageDef.UI_OPEN);
    }

    private void f_BackBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionMenuPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_HallBtn(GameObject go, object value1, object value2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionPlayerPool.f_ExecuteAfterLegionMemInit(f_ExecuteAfterLegionMemInit);
    }
    /// <summary>
    /// 商店按钮
    /// </summary>
    private void f_ShopBtn(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.Legion);
    }
    /// <summary>
    /// 祭天按钮
    /// </summary>
    private void f_Sacrifice(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionPlayerPool.f_ExecuteAfterLegionMemInit(f_ExecuteSacrificeAfterLegionMemInit);

    }

    /// <summary>
    /// 军团技能
    /// </summary>
    private void f_SkillBtn(GameObject go, object obj1, object ojb2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionPlayerPool.f_ExecuteAfterLegionMemInit(f_ExecuteOpenSkillAfterLegionMemInit);
    }
    /// <summary>
    /// 点击军团福利（红包）
    /// </summary>
    private void f_RedPacketBtn(GameObject go, object obj, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionPlayerPool.f_ExecuteAfterLegionMemInit(f_ExecuteAfterLegionMemInit2);
    }
    /// <summary>
    /// 点击军团排行
    /// </summary>
    private void f_LegionRankBtn(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionRankPage, UIMessageDef.UI_OPEN);
    }

    private void f_DungeonBtn(GameObject go, object value1, object value2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionPlayerPool.f_ExecuteAfterLegionMemInit(f_ExecuteInitDungeonAfterLegionMemInit);
    }

    private void f_OnBtnLegionBattleClick(GameObject go, object value1, object value2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionPlayerPool.f_ExecuteAfterLegionMemInit(f_ExecuteInitBattleAfterLegionMemInit);
    }

    #endregion

    private void f_ExecuteInitDungeonAfterLegionMemInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.f_OpenOrCloseWaitTip(true);
            LegionMain.GetInstance().m_LegionDungeonPool.f_ExecuteAfterInitFiniChapterAndInitCurChapter(f_ExecuteOpenDungeonAfterInitFiniChapterAndInitCurChapter);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(477) + result);
        }
    }

    private void f_ExecuteInitBattleAfterLegionMemInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattlePage, UIMessageDef.UI_OPEN);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(477) + result);
        }
    }

    private void f_ExecuteOpenDungeonAfterInitFiniChapterAndInitCurChapter(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterPage, UIMessageDef.UI_OPEN);
        }
    }

    private void f_ExecuteOpenSkillAfterLegionMemInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //打开技能界面
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionSkillPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(477) + result);
        }
    }
    private void f_ExecuteAfterLegionMemInit2(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionRedPacketPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(477) + result);
        }
    }
    private void f_ExecuteAfterInitFiniChapterAndInitCurChapter(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterPage, UIMessageDef.UI_OPEN);
        }
    }

    private void f_ExecuteAfterLegionMemInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //打开议事大厅界面
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionHallPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(477) + result);
        }
    }

    private void f_UpdateLegionInfoView(BasePoolDT<long> poolDt)
    {
        mData = (LegionInfoPoolDT)poolDt;
        int ExpMax = LegionTool.f_GetLvUpExpValue(mData.f_GetProperty((int)EM_LegionProperty.Lv));
        int ExpCur = mData.f_GetProperty((int)EM_LegionProperty.Exp);
        mLegionNameLabel.text = mData.LegionName;
        mExpSlider.value = ExpMax == 0 ? 1.0f : ExpCur / (float)ExpMax;
        mExpLabel.text = string.Format("{0}/{1}", ExpCur, ExpMax);
        mContributeLabel.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_LegionContribution).ToString();
        mLegionLv.text = mData.f_GetProperty((int)EM_LegionProperty.Lv).ToString();
        //更新升级按钮
        if (mData.f_GetProperty((int)EM_LegionProperty.Lv) > 0 && mData.f_GetProperty((int)EM_LegionProperty.Lv) < 10)
        {
            LegionLevelDT tLegionLv = glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetSC(mData.f_GetProperty((int)EM_LegionProperty.Lv) + 1) as LegionLevelDT;
            if (mData.f_GetProperty((int)EM_LegionProperty.Exp) > tLegionLv.iExp)
            {
                f_GetObject("Btn_LegionUp").SetActive(true);
            }
            else
                f_GetObject("Btn_LegionUp").SetActive(false);
        }
        else
            f_GetObject("Btn_LegionUp").SetActive(false);
        f_GetObject("BtnBottomRightGrid").GetComponent<UIGrid>().Reposition();
    }

    private void f_RequestSacrificeInfo()
    {
        LegionMain.GetInstance().m_LegionInfor.f_SendSacrificeInfo(null);
    }


    private void f_ExecuteSacrificeAfterLegionMemInit(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(478) + result);
        }
        LegionMain.GetInstance().m_LegionInfor.f_SendSacrificeInfo(f_Callback_SendSacrificeInfo);
    }

    private void f_Callback_SendSacrificeInfo(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionSacrifice, UIMessageDef.UI_OPEN);
    }

    private void f_OpenOrCloseMonenyPage(bool isOpen)
    {
        if (isOpen)
        {
            List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
            listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
            listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
            listMoneyType.Add(EM_MoneyType.eUserAttr_LegionContribution);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        }
    }

    #region 红点

    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LegionApplicantList, f_GetObject("HallBtn"), ReddotCallback_Show_HallBtn, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LegionChapetAward, f_GetObject("DungeonBtn"), ReddotCallback_Show_DungeonBtn, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LegionRedPacket, f_GetObject("RedPacketBtn"), ReddotCallback_Show_RedPacketBtn, true);
        UpdateReddotUI();
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegionApplicantList);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegionChapetAward);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegionRedPacket);
    }

    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.LegionApplicantList, f_GetObject("HallBtn"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.LegionChapetAward, f_GetObject("DungeonBtn"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.LegionRedPacket, f_GetObject("RedPacketBtn"));
    }

    private void ReddotCallback_Show_HallBtn(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnHall = f_GetObject("HallBtn");
        UITool.f_UpdateReddot(BtnHall, iNum, new Vector3(20, 70, 0), 401);
    }

    private void ReddotCallback_Show_DungeonBtn(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnHall = f_GetObject("DungeonBtn");
        UITool.f_UpdateReddot(BtnHall, iNum, new Vector3(20, 70, 0), 401);
    }

    private void ReddotCallback_Show_RedPacketBtn(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnRedPacket = f_GetObject("RedPacketBtn");
        UITool.f_UpdateReddot(BtnRedPacket, iNum, new Vector3(48, 45, 0), 401);
    }

    #endregion

}
