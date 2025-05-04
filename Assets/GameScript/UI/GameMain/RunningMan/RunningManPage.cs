using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class RunningManPage : UIFramwork
{
    private RunningManItem[] mTollgateItems;

    private UILabel mPrestigeLabel;
    //private UITable mHistoryStarTable;
    private UILabel mHistoryStarLabel;
    //private UITable mCurStarTable;
    private UILabel mCurStarLabel;
    //private UITable mLeftStarTable;
    private UILabel mLeftStarLabel;

    private UILabel mResetTimesLabel;

    private int curPassChapterId;
    private int chapterMaxId;
    private int curPassTollgateIdx;

    //通关宝箱的星数
    private int passChapBoxStarNum;

    //单前通关的Pool数据
    private RunningManPoolDT curPassPoolDt;
    //单前的Pool数据
    private RunningManPoolDT curPoolDt;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mTollgateItems = new RunningManItem[GameParamConst.RMTollgateNumPreChap];
        for (int i = 0; i < mTollgateItems.Length; i++)
        {
            mTollgateItems[i] = f_GetObject(string.Format("TollgateItem{0}", i)).GetComponent<RunningManItem>();
        }

        mPrestigeLabel = f_GetObject("PrestigeLabel").GetComponent<UILabel>();
        //mHistoryStarTable = f_GetObject("HistoryStarTable").GetComponent<UITable>();
        mHistoryStarLabel = f_GetObject("HistoryStarLabel").GetComponent<UILabel>();
        //mCurStarTable = f_GetObject("CurStarTable").GetComponent<UITable>();
        mCurStarLabel = f_GetObject("CurStarLabel").GetComponent<UILabel>();
        //mLeftStarTable = f_GetObject("LeftStarTable").GetComponent<UITable>();
        mLeftStarLabel = f_GetObject("LeftStarLabel").GetComponent<UILabel>();
        mResetTimesLabel = f_GetObject("ResetTimesLabel").GetComponent<UILabel>();

        f_RegClickEvent("BtnBack", f_BtnBack);
        f_RegClickEvent("BtnChallenge", f_BtnChallenge);
        f_RegClickEvent("BtnReset", f_BtnReset);
        f_RegClickEvent("BtnPassAward", f_BtnPassAward);
        mLabel_MoreAttr = f_GetObject("Label_MoreAttr");
        f_RegClickEvent("Label_MoreAttr", f_BtnMoreAttr);
        f_RegClickEvent("BtnRankList", f_BtnRankList);
        f_RegClickEvent("BtnSweep", f_BtnSweep);
        f_RegClickEvent("BtnEliteChallenge", f_BtnEliteChallenge);
        f_RegClickEvent("BtnRMShop", f_BtnRMShop);
        f_RegClickEvent("BtnClothArray", f_BtnClothArray);
        f_RegClickEvent("BtnNorChallenge", f_BtnNorChallenge);
		f_RegClickEvent("Btn_Help", f_OnHelpBtnClick);


        mNullProperty = f_GetObject("NullProperty");
        mShowItem = f_GetObject("ShowItem");
        mItemGrid = f_GetObject("ItemGrid").GetComponent<UIGrid>();
        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
    }
    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RunningManEliteLeftTimes, f_GetObject("BtnEliteChallenge"), ReddotCallback_Show_Btn_RunningMan);
        UpdateReddotUI();

    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.RunningManEliteLeftTimes, f_GetObject("BtnEliteChallenge"));
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RunningManEliteLeftTimes);
    }

    private void ReddotCallback_Show_Btn_RunningMan(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnEliteChallenge = f_GetObject("BtnEliteChallenge");
        UITool.f_UpdateReddot(BtnEliteChallenge, iNum, new Vector3(50, 53, 0), 200);
    }

    #endregion
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);

        //断线重连如果切去其他界面，这时候不处理，服务器消息返回再打开界面就会和其他界面重叠
        if (!UITool.f_IsCanOpenChallengePage(UINameConst.RunningManPage))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_CLOSE);
            return;
        }

        if (e != null && e is Battle2MenuProcessParam)
        {
            Battle2MenuProcessParam tParam = (Battle2MenuProcessParam)e;
            if (tParam.m_emType == EM_Battle2MenuProcess.RunningManElite)
            {
                f_BtnEliteChallenge(null, null, null);
                tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
            }
        }
        f_RefreshUI();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessNextDay, this);
        //货币
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);

        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/RunningMan/ggzj_bg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture Texture_BG = f_GetObject("Texture_BG").GetComponent<UITexture>();
        if (Texture_BG.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            Texture_BG.mainTexture = tTexture2D;
        }
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        f_UpdateLabelInfo();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }



    private void f_RefreshUI()
    {
        curPassChapterId = Data_Pool.m_RunningManPool.m_iCurPassChapter;
        chapterMaxId = Data_Pool.m_RunningManPool.m_iChapterMaxId;
        if (curPassChapterId > 0 && curPassChapterId <= chapterMaxId)
        {
            curPassPoolDt = (RunningManPoolDT)Data_Pool.m_RunningManPool.f_GetForId(curPassChapterId);
            if (curPassPoolDt.m_iBuffIdx == 0)
            {
                //打开兑换加Buff的界面
                ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManBuffAddPage, UIMessageDef.UI_OPEN, new RunningManBuffAddPageParam(curPassPoolDt, f_OnFinishBuffAdd));
            }
            else if (curPassPoolDt.m_iBoxTimes == 0)
            {
                UITool.f_OpenOrCloseWaitTip(true);
                //已经领取Buff但是没有领宝箱 发送领取宝箱协议
                SocketCallbackDT tSocketCallbackDt = new SocketCallbackDT();
                tSocketCallbackDt.m_ccCallbackSuc = f_RunningManChapBoxHandle;
                tSocketCallbackDt.m_ccCallbackFail = f_RunningManChapBoxHandle;
                Data_Pool.m_RunningManPool.f_RunningManChapBox((ushort)curPassPoolDt.iId, tSocketCallbackDt);
            }
        }
        f_UpdateByCurPoolDt((RunningManPoolDT)Data_Pool.m_RunningManPool.f_GetForId(Mathf.Min(curPassChapterId + 1, chapterMaxId)));
        f_ShowReferrerProperty(Data_Pool.m_RunningManPool.m_BuffPropertyList);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessNextDay, this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    /// <summary>
    /// 跨天处理
    /// </summary>
    /// <param name="value"></param>
    private void f_ProcessNextDay(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.RunningManPool)
            return;
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(806));
        f_RefreshUI();
    }

    private void f_UpdateByCurPoolDt(RunningManPoolDT poolDt)
    {
        curPoolDt = poolDt;
        //计算已经通关的关卡Idx
        curPassTollgateIdx = 0;
        for (int i = 0; i < curPoolDt.m_TollgatePoolDTs.Length; i++)
        {
            if (curPoolDt.m_TollgatePoolDTs[i].m_iResult > 0)
                curPassTollgateIdx = i + 1;
        }
        //更新界面
        for (int i = 0; i < mTollgateItems.Length; i++)
        {
            mTollgateItems[i].f_UpdateByInfo(curPassTollgateIdx, i, curPoolDt.m_TollgatePoolDTs[i]);
        }
        UISprite uISprite = f_GetObject("TollgateBottom").GetComponent<UISprite>();
        uISprite.spriteName = curPoolDt.m_TollgatePoolDTs[0].m_iTollgateId == 1 ? "Icon_BottomTollgate" : "Icon_NorTollgate";
        UISprite uISprite2 = f_GetObject("TollgateBottom2").GetComponent<UISprite>();
        uISprite2.spriteName = curPoolDt.m_TollgatePoolDTs[0].m_iTollgateId == 1 ? "Icon_BottomTollgate2" : "";
        //更新文字信息
        f_UpdateLabelInfo();


    }

    private void f_UpdateLabelInfo()
    {
        //更新文字信息
        mPrestigeLabel.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Prestige).ToString();
        mHistoryStarLabel.text = Data_Pool.m_RunningManPool.m_iHistoryStarNum.ToString();
        mCurStarLabel.text = Data_Pool.m_RunningManPool.m_iCurStarNum.ToString();
        mLeftStarLabel.text = Data_Pool.m_RunningManPool.m_iLeftStarNum.ToString();
        //mHistoryStarTable.repositionNow = true;
        //mCurStarTable.repositionNow = true;
        //mLeftStarTable.repositionNow = true;
        mResetTimesLabel.text = (Data_Pool.m_RunningManPool.m_iResetTimesLimit - Data_Pool.m_RunningManPool.m_iResetTimes).ToString();
    }

    private void f_BtnBack(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_BtnPassAward(GameObject go, object value1, object value2)
    {
        //展示通关奖励
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManAwardPage, UIMessageDef.UI_OPEN, curPoolDt);
    }

    //更多属性界面
    private void f_BtnMoreAttr(GameObject go, object value1, object value2)
    {
        //展示更多属性
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManAttrPlusPage, UIMessageDef.UI_OPEN, curPoolDt);
    }

    private void f_BtnRankList(GameObject go, object value1, object value2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        //展示排行榜
        Data_Pool.m_RunningManPool.f_ExecuteAferRankList(true, f_Callback_RankList);
    }

    private void f_Callback_RankList(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManRankPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(808) + result);
        }
    }

    private void f_BtnSweep(GameObject go, object value1, object value2)
    {
        if (Data_Pool.m_RunningManPool.m_bIsLose)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(809));
            return;
        }
        int tHistory3StarChap = Data_Pool.m_RunningManPool.m_iHistory3StarsChapter;
        int tCurPassChap = Data_Pool.m_RunningManPool.m_iCurPassChapter;
        if (tCurPassChap >= tHistory3StarChap)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(810));
            return;
        }
        string tContent = string.Format(CommonTools.f_GetTransLanguage(811), Mathf.Min(Data_Pool.m_RunningManPool.m_iHistory3StarsChapter, Data_Pool.m_RunningManPool.m_iChapterMaxId) * GameParamConst.RMTollgateNumPreChap);
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(812), tContent, CommonTools.f_GetTransLanguage(813), f_SureSweep, CommonTools.f_GetTransLanguage(814));
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_SureSweep(object result)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        //申请扫荡
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_RunningManSweepHandle;
        socketCallbackDt.m_ccCallbackFail = f_RunningManSweepHandle;
        Data_Pool.m_RunningManPool.f_RunningManSweep(socketCallbackDt);
    }

    private void f_RunningManSweepHandle(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManSweepPage, UIMessageDef.UI_OPEN);
            f_RefreshUI();
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(815));
        }
    }

    private void f_BtnEliteChallenge(GameObject go, object value1, object value2)
    {
        //精英挑战
        if (Data_Pool.m_RunningManPool.m_iHistoryChapter <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(816));
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManElitePage, UIMessageDef.UI_OPEN);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_CLOSE);
    }


    private void f_BtnNorChallenge(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManElitePage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_OPEN);
    }
    

    private void f_BtnRMShop(GameObject go, object value1, object value2)
    {
        //过关斩将商店
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.RunningMan);
    }


    private void f_BtnChallenge(GameObject go, object value1, object value2)
    {
        if (Data_Pool.m_RunningManPool.m_bIsLose)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(817));
            return;
        }
        else if (curPassTollgateIdx >= GameParamConst.RMTollgateNumPreChap)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(818));
            return;
        }
        //打开关卡挑战界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManChallengePage, UIMessageDef.UI_OPEN, curPoolDt.m_TollgatePoolDTs[curPassTollgateIdx]);
    }

    private void f_BtnReset(GameObject go, object value1, object value2)
    {
        int tmpPassNum = curPassChapterId * GameParamConst.RMTollgateNumPreChap + (curPassTollgateIdx >= GameParamConst.RMTollgateNumPreChap ? 0 : curPassTollgateIdx);
        if (Data_Pool.m_RunningManPool.m_iResetTimes >= Data_Pool.m_RunningManPool.m_iResetTimesLimit)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(819));
            return;
        }
        else if (tmpPassNum == 0 && !Data_Pool.m_RunningManPool.m_bIsLose)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(820));
            return;
        }
        string tContent = string.Format(CommonTools.f_GetTransLanguage(821), tmpPassNum);
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(812), tContent, CommonTools.f_GetTransLanguage(813), f_SureReset, CommonTools.f_GetTransLanguage(814));
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_SureReset(object value)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        //重置
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_RunningManResetHandle;
        socketCallbackDt.m_ccCallbackFail = f_RunningManResetHandle;
        Data_Pool.m_RunningManPool.f_RunningManReset(socketCallbackDt);
    }

    private void f_RunningManChapBoxHandle(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //展示领取的宝箱
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(822));
            int tChapBoxStar = 0;
            int awardPoolId = 0;
            for (int i = 0; i < curPassPoolDt.m_TollgatePoolDTs.Length; i++)
            {
                tChapBoxStar += curPassPoolDt.m_TollgatePoolDTs[i].m_iResult;
            }
            if (tChapBoxStar >= 9)
            {
                awardPoolId = curPassPoolDt.m_ChapterTemplate.iBox9;
            }
            else if (tChapBoxStar >= 6)
            {
                awardPoolId = curPassPoolDt.m_ChapterTemplate.iBox6;
            }
            else if (tChapBoxStar >= 3)
            {
                awardPoolId = curPassPoolDt.m_ChapterTemplate.iBox3;
            }
            //展示宝箱内容
            AwardTipPageParam tParam = new AwardTipPageParam(CommonTools.f_GetTransLanguage(823), awardPoolId);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.AwardTipPage, UIMessageDef.UI_OPEN, tParam);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(824) + result);
        }
    }

    private void f_RunningManResetHandle(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //重置成功刷新界面
            f_RefreshUI();
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(825) + result);
        }
    }
	private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 18);
    }
    private void f_BtnClothArray(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

    /// <summary>
    /// 选择buff完成处理
    /// </summary>
    /// <param name="result"></param>
    private void f_OnFinishBuffAdd(object result)
    {
        f_UpdateLabelInfo();
        f_ShowReferrerProperty(Data_Pool.m_RunningManPool.m_BuffPropertyList);
    }

    #region Buff显示

    private GameObject mNullProperty;
    private GameObject mLabel_MoreAttr;
    private GameObject mShowItem;
    private UIGrid mItemGrid;
    private UIScrollView mScrollView;
    private List<UILabel> mShowItems = new List<UILabel>();

    private void f_ShowReferrerProperty(List<RunningManBuffProperty> dataList)
    {
        mNullProperty.SetActive(dataList.Count == 0);
        //mLabel_MoreAttr.SetActive(dataList.Count > 2);
        for (int i = 0; i < dataList.Count; i++)
        {
            if (i < mShowItems.Count)
            {
                mShowItems[i].gameObject.SetActive(true);
                mShowItems[i].text = UITool.f_GetProName((EM_RoleProperty)dataList[i].m_iPropertyType) + ":";
                mShowItems[i].transform.Find("NumProperty").GetComponent<UILabel>().text = "[00FF00FF]+" + dataList[i].m_iPropertyValue / 10000.0f * 100 + "%";
                continue;
            }
            GameObject tItem = NGUITools.AddChild(mItemGrid.gameObject, mShowItem.gameObject);
            tItem.SetActive(true);
            UILabel tLabel = tItem.GetComponent<UILabel>();
            tLabel.text = UITool.f_GetProName((EM_RoleProperty)dataList[i].m_iPropertyType) + ":";
            UILabel nLabel = tItem.transform.Find("NumProperty").GetComponent<UILabel>();
            nLabel.text = "[00FF00FF]+" + dataList[i].m_iPropertyValue / 10000.0f * 100 + "%";
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

    #endregion
}
