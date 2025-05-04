using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 等级礼包活动
/// </summary>
public class RankGiftPage : UIFramwork
{
    private UIWrapComponent _levelWrapComponet = null;
    private UIWrapComponent _contentWrapComponet = null;
    private Dictionary<long,RankGiftDT> listRankGiftData = new Dictionary<long, RankGiftDT>();//等级和DT对应
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();
    private SocketCallbackDT RequestQueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT BuyRankGiftCallback = new SocketCallbackDT();//购买等级礼包回调
    private int currentSelectLevel;
    private Dictionary<int, RankGiftLevelItemCtl> dicAllLevelButton = new Dictionary<int, RankGiftLevelItemCtl>();
    private RankGiftPoolDT currentSelectPoolDT;//当前点击的等级礼包

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        RequestQueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        RequestQueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        BuyRankGiftCallback.m_ccCallbackSuc = OnBuySucCallback;
        BuyRankGiftCallback.m_ccCallbackFail = OnBuyFailCallback;
        InitLevelScro();

        OnQueryGift();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        CancelInvoke("ChangeTimes");
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnLvUp", OnLevelItemGotoClick);
        f_RegClickEvent("BtnBlack", OnBtnReturnClick);
    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        ccUIHoldPool.GetInstance().f_UnHold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.MainMenu));
    }
    /// <summary>
    /// 点击返回按钮
    /// </summary>
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RankGiftPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 更新等级滚动条
    /// </summary>
    private void InitLevelScro()
    {
        List<BasePoolDT<long>> listLevelScro = new List<BasePoolDT<long>>();
        dicAllLevelButton.Clear();
        listRankGiftData.Clear();
        List<int> listTemp = new List<int>();
        List<NBaseSCDT> listLevelData = glo_Main.GetInstance().m_SC_Pool.m_RankGiftSC.f_GetAll();
        for (int index = 0; index < listLevelData.Count; index++)
        {
            RankGiftDT rankGiftDT = listLevelData[index] as RankGiftDT;
            if (rankGiftDT.iOpenLevel <= 0) {
                continue;
            }
            BasePoolDT<long> item = new BasePoolDT<long>();
            item.iId = rankGiftDT.iOpenLevel;
            if (!listTemp.Contains((int)item.iId))
            {
                listTemp.Add((int)item.iId);
                listLevelScro.Add(item);
                listRankGiftData.Add(item.iId, rankGiftDT);
            }
        }

        if (_levelWrapComponet == null)
        {
            _levelWrapComponet = new UIWrapComponent(540, 1, 500, 5, f_GetObject("LevelGridParent"), f_GetObject("LevelBgItem"), listLevelScro, OnLevelContentItemUpadate, null);
        }
        _levelWrapComponet.f_ResetView();
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        //注：根据玩家自身等级判断默认选中哪个等级
        int DefaultSelectIndex = 0;
        if (playerLevel <= listTemp[0])
            DefaultSelectIndex = 0;
        else if (playerLevel >= listTemp[listTemp.Count - 1])
            DefaultSelectIndex = listTemp.Count - 1;
        else
        {
            for (int i = 0; i < listTemp.Count -1; i++)
            {
                if (playerLevel >= listTemp[i] && playerLevel < listTemp[i + 1])
                {
                    DefaultSelectIndex = i;
                    break;
                }
            }
        }
        _levelWrapComponet.f_ViewGotoRealIdx(DefaultSelectIndex, 1);
        currentSelectLevel = listTemp[DefaultSelectIndex];
        if (dicAllLevelButton.ContainsKey(currentSelectLevel) && null != dicAllLevelButton[currentSelectLevel])
        {
            //根据玩家自身等级判断默认选中
            OnLevelItemClick(dicAllLevelButton[currentSelectLevel].gameObject, currentSelectLevel, null);
        }    
    }
    /// <summary>
    /// 等级拖动的更新
    /// </summary>
    /// <param name="item"></param>
    /// <param name="data"></param>
    private void OnLevelContentItemUpadate(Transform item, BasePoolDT<long> data)
    {
        RankGiftDT ranGiftDT = listRankGiftData[data.iId];
        bool isGetLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= ranGiftDT.iOpenLevel;
        bool isRedPoint = false;
        listContent.Clear();
        listContent = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_RankGiftPool.f_GetAll());
        bool isShowMask = !isGetLv;
        if (isGetLv)
        {
            for (int i = listContent.Count - 1; i >= 0; i--)
            {
                if ((listContent[i] as RankGiftPoolDT).m_RankGiftDT.iOpenLevel == (int)data.iId)
                {
                    RankGiftPoolDT rankGiftPoolDT = listContent[i] as RankGiftPoolDT;
                    int timeGone = GameSocket.GetInstance().f_GetServerTime() - rankGiftPoolDT.m_levelTime;
                    if (timeGone > 24 * 60 * 60)
                    {
                        //已过期
                        break;
                    }
                    else
                    {
                        int buyTimesLeft = rankGiftPoolDT.m_RankGiftDT.iBuyTime - rankGiftPoolDT.m_buyTimes;
                        if (buyTimesLeft > 0)
                        {
                            isRedPoint = true;
                            break;
                        }
                    }
                }
            }
        }

        if (!isShowMask)
        {
            //未开放或已过期要遮罩，，这里再判断是否已过期
            int serverTime = GameSocket.GetInstance().f_GetServerTime();
            int levelUpTime = getLevelUpTime(ranGiftDT.iOpenLevel);
            int timeLeft = (24 * 60 * 60) - (serverTime - levelUpTime);
            isShowMask = timeLeft <= 0;
        }

        RankGiftLevelItemCtl itemCtl = item.GetComponent<RankGiftLevelItemCtl>();
        if (null == itemCtl) return;
        itemCtl.SetData((int)data.iId, ranGiftDT.szDescribe == null ? "" : ranGiftDT.szDescribe,ranGiftDT.szTitile,ranGiftDT.szGetWayGoPage, isGetLv, isRedPoint);
        if (!dicAllLevelButton.ContainsKey((int)data.iId))
            dicAllLevelButton.Add((int)data.iId, itemCtl);
        else
            dicAllLevelButton[(int)data.iId] = itemCtl;
        f_RegClickEvent(itemCtl.m_BtnItem, OnLevelItemClick, (int)data.iId);
        itemCtl.mObjSelectedFlag.SetActive(currentSelectLevel == data.iId);
        itemCtl.m_ObjLvMask.SetActive(isShowMask);
        f_RegClickEvent(itemCtl.m_BtnGoto, OnGotoPage, ranGiftDT.szGetWayGoPage, (int)data.iId);
    }
    
    /// <summary>
    /// 点击等级前往
    /// </summary>
    private void OnGotoPage(GameObject go,object obj1,object obj2)
    {
        if (currentSelectLevel != (int)obj2)
            return;
        string goPage = (string)obj1;
        switch (goPage)
        {
            case UINameConst.LegionMenuPage:
                if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
                {
                    //还没有军团
                    UITool.f_OpenOrCloseWaitTip(true);
                    LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(false, true, f_Callback_LegionListInfo);
                }
                else
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(true, true, f_Callback_LegionSelfInfo);
                }
                break;
            default:
                ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.MainMenu));
                UITool.f_GotoPage(this, goPage, 0);
                break;
        }
    }
    private void f_Callback_LegionListInfo(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //打开军团列表界面
            ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.MainMenu));
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionListPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1459) + result);
        }
    }

    private void f_Callback_LegionSelfInfo(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.MainMenu));
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionMenuPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1460) + result);
        }
    }
    /// <summary>
    /// 点击前往(切换至主线副本关卡最新界面)
    /// </summary>
    private void OnLevelItemGotoClick(GameObject go, object obj1, object obj2)
    {
        int tIdx = Data_Pool.m_DungeonPool.f_GetFightChapterIdx((int)EM_Fight_Enum.eFight_DungeonMain);
        DungeonPoolDT temp = Data_Pool.m_DungeonPool.f_GetAllForData1((int)EM_Fight_Enum.eFight_DungeonMain)[tIdx] as DungeonPoolDT;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_DungeonPool.f_ExecuteAfterInitDungeon(temp, f_Callback_DungeonTollgate);
    }
    private void f_Callback_DungeonTollgate(object value)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if (value is DungeonPoolDT)
        {
            DungeonPoolDT poolDt = (DungeonPoolDT)value;
            DungeonTollgatePageParam param = new DungeonTollgatePageParam();
            param.mDungeonPoolDT = poolDt;
            ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.MainMenu));
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonTollgatePageNew, UIMessageDef.UI_OPEN, param);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1461) + value);
        }
    }
    private void OnLevelItemClick(GameObject go,object obj1,object obj2)
    {
        int level = (int)obj1;
        CancelInvoke("ChangeTimes");
        int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        UpdateContent(level);
        currentSelectLevel = (int)obj1;

        //设置选中效果
        Dictionary<int, RankGiftLevelItemCtl>.Enumerator enumerator = dicAllLevelButton.GetEnumerator();
        while (enumerator.MoveNext())
        {
            RankGiftLevelItemCtl value = enumerator.Current.Value;
            value.mObjSelectedFlag.SetActive(false);
        }
        if (dicAllLevelButton.ContainsKey(currentSelectLevel) && null != dicAllLevelButton[currentSelectLevel])
        {
            dicAllLevelButton[currentSelectLevel].mObjSelectedFlag.SetActive(true);
        }


        int serverTime = GameSocket.GetInstance().f_GetServerTime();
        int levelUpTime = getLevelUpTime(level);
        int timeLeft = (24 * 60 * 60) - (serverTime - levelUpTime);
        f_GetObject("SprTimeHint").SetActive(true);
        f_GetObject("LvLessContent").SetActive(false);
        UILabel lableTips = f_GetObject("LabelTips").GetComponent<UILabel>();
        if (userLevel < level)//已过期||未到达等级
        {
            f_GetObject("LvLessContent").SetActive(true);
            f_GetObject("SprTimeHint").SetActive(false);
            GameObject LvLessContent = f_GetObject("LvLessContent");
            Transform ModelPoint = LvLessContent.transform.Find("ModelPoint");
            if (ModelPoint.Find("Model") == null)//生成模型
            {
                UITool.f_GetStatelObject(1105, ModelPoint, new Vector3(0, 180, 0), Vector3.zero, 16, "Model", 70 );
            }
        }
        else if (timeLeft < 0)
        {
            f_GetObject("LabelTime").GetComponent<UILabel>().text = "";
            lableTips.text = CommonTools.f_GetTransLanguage(1462);
            lableTips.transform.localPosition = new Vector3(-130, 8, 1);
            lableTips.color = Color.red;
        }
        else
        {
            lableTips.text = CommonTools.f_GetTransLanguage(1463);
            lableTips.transform.localPosition = new Vector3(-62, 8, 0);
            lableTips.color = norTimeTipsColor;
            timeLevelLeft = timeLeft;
            InvokeRepeating("ChangeTimes", 0f, 1f);
        }
        //
    }
    private int timeLevelLeft;//过期倒计时剩余秒数
    private Color norTimeTipsColor= new Color(182f/255,167f/255,145f/255);
    /// <summary>
    /// 改变倒计时
    /// </summary>
    private void ChangeTimes()
    {
        UILabel labelTime = f_GetObject("LabelTime").GetComponent<UILabel>();
        labelTime.text = CommonTools.f_GetStringBySecond(timeLevelLeft);       
        timeLevelLeft --;
        if (timeLevelLeft < 0)//已过期
        {
            CancelInvoke("ChangeTimes");
            labelTime.text = "";
            UILabel lableTips = f_GetObject("LabelTips").GetComponent<UILabel>();
            lableTips.text = CommonTools.f_GetTransLanguage(1462);
            lableTips.transform.localPosition = new Vector3(-130, 8, 1);
            lableTips.color = Color.red;
        }
    }
    /// <summary>
    /// 根据其等级获取其升级时间
    /// </summary>
    /// <param name="level">等级</param>
    /// <returns></returns>
    private int getLevelUpTime(int level)
    {
        List<BasePoolDT<long>> listRankGiftPoolDT = Data_Pool.m_RankGiftPool.f_GetAll();
        for (int i = 0; i < listRankGiftPoolDT.Count; i++)
        {
            RankGiftPoolDT dt = listRankGiftPoolDT[i] as RankGiftPoolDT;
            if (dt.m_RankGiftDT.iOpenLevel == level)
            {
                return dt.m_levelTime;
            }
        }
        return 0;
    }
    /// <summary>
    /// 根据等级
    /// </summary>
    /// <param name="Level"></param>
    private void UpdateContent(int Level)
    {
        listContent.Clear();
        listContent = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_RankGiftPool.f_GetAll());
        for (int i = listContent.Count -1; i >= 0; i--)
        {
            if ((listContent[i] as RankGiftPoolDT).m_RankGiftDT.iOpenLevel != Level)
            {
                listContent.RemoveAt(i);
            }
        }
        //排序(冒泡)
        for (int a = 0; a < listContent.Count - 1; a++)
        {
            RankGiftPoolDT rankGiftPoolDTA = listContent[a] as RankGiftPoolDT;
            if (rankGiftPoolDTA.m_buyTimes >= rankGiftPoolDTA.m_RankGiftDT.iBuyTime)
            {
                for (int b = a + 1; b < listContent.Count; b++)
                {
                    RankGiftPoolDT rankGiftPoolDTB = listContent[b] as RankGiftPoolDT;
                    if (rankGiftPoolDTB.m_buyTimes < rankGiftPoolDTB.m_RankGiftDT.iBuyTime)
                    {
                        RankGiftPoolDT temp = rankGiftPoolDTB;
                        listContent[b] = rankGiftPoolDTA;
                        listContent[a] = temp;
                        break;
                    }
                }
            }
        }
        if (listContent.Count<5) {
            int Count = 5- listContent.Count;
            for (int i = 0; i < Count; i++)
            {
                RankGiftPoolDT nullPoolDt = new RankGiftPoolDT();
                nullPoolDt.m_RankGiftDT = null;
                listContent.Add(nullPoolDt);
            }
            
        }
        if (_contentWrapComponet == null)
        {
            _contentWrapComponet = new UIWrapComponent(350, 1, 374, 10, f_GetObject("GridContentParent"), f_GetObject("ContentItem"), listContent, OnContentItemUpdate, null);
        }
        _contentWrapComponet.f_UpdateList(listContent);
        _contentWrapComponet.f_ResetView();
        _contentWrapComponet.f_UpdateView();
    }
    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        RankGiftPoolDT rankGiftPoolDT = data as RankGiftPoolDT;
        RankGiftDT rankGiftDT = rankGiftPoolDT.m_RankGiftDT;
        ActRankGiftItemCtl actRankGiftItemCtl = item.GetComponent<ActRankGiftItemCtl>();
        if (rankGiftDT==null) {
            actRankGiftItemCtl.SetGray();
            return;
        }
        ActRankGiftItemCtl.GiftState giftState = ActRankGiftItemCtl.GiftState.WaitGet;
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < rankGiftDT.iOpenLevel)
        {
            giftState = ActRankGiftItemCtl.GiftState.WaitGet;
        }
        else
        {
            int timeGone = GameSocket.GetInstance().f_GetServerTime() - rankGiftPoolDT.m_levelTime;
            if (timeGone > 24 * 60 * 60)
            {
                //已过期
                giftState = ActRankGiftItemCtl.GiftState.OutOfTime;
            }
            else
            {
                int buyTimesLeft = rankGiftDT.iBuyTime - rankGiftPoolDT.m_buyTimes;
                if (buyTimesLeft <= 0)
                {
                    giftState = ActRankGiftItemCtl.GiftState.HasGet;
                }
                else
                {
                    giftState = ActRankGiftItemCtl.GiftState.CanGet;
                }
            }
        }

        f_GetObject("DragContent").SetActive(giftState != ActRankGiftItemCtl.GiftState.WaitGet);
        if (giftState == ActRankGiftItemCtl.GiftState.WaitGet)
        {
            f_GetObject("LvLessContent").transform.GetComponentInChildren<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1465), rankGiftDT.iOpenLevel, rankGiftDT.szTitile);
        }
        f_GetObject("LvLessContent").SetActive(giftState == ActRankGiftItemCtl.GiftState.WaitGet);
       

        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)rankGiftDT.iRewardType, rankGiftDT.iRewardId, rankGiftDT.iRewardCount);
        string name = dt.mName;
        string borderSprName = UITool.f_GetImporentColorName(dt.mImportant, ref name);
        
        actRankGiftItemCtl.SetData(rankGiftDT.iDiscount, (EM_ResourceType)rankGiftDT.iRewardType, rankGiftDT.iRewardId, borderSprName,
            rankGiftDT.iRewardCount, name,
            giftState, rankGiftDT.iOpenLevel, rankGiftDT.iBuyPrice, rankGiftDT.iBuyTime , rankGiftPoolDT.m_buyTimes);
        f_RegClickEvent(actRankGiftItemCtl.BtnGet, OnBtnGetnClick, rankGiftPoolDT);
        f_RegClickEvent(actRankGiftItemCtl.SprIcon.gameObject, OnAwardIconClick, rankGiftPoolDT);
    }
    /// <summary>
    /// 查询等级礼包
    /// </summary>
    private void OnQueryGift()
    {
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_RankGiftPool.f_RequestRankGiftInfo(RequestQueryCallback);
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        RankGiftPoolDT rankGiftPoolDT = obj1 as RankGiftPoolDT;
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)rankGiftPoolDT.m_RankGiftDT.iRewardType, rankGiftPoolDT.m_RankGiftDT.iRewardId, rankGiftPoolDT.m_RankGiftDT.iRewardCount);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnBtnGetnClick(GameObject go, object obj1, object obj2)
    {
        RankGiftPoolDT rankGiftPoolDT = obj1 as RankGiftPoolDT;
        currentSelectPoolDT = rankGiftPoolDT;
        Debug.Log(CommonTools.f_GetTransLanguage(1466) + (int)rankGiftPoolDT.iId);
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < rankGiftPoolDT.m_RankGiftDT.iBuyPrice)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1467));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_RankGiftPool.f_BuyRankGift((int)rankGiftPoolDT.iId,BuyRankGiftCallback);
    }
    /// <summary>
    /// 等级礼包信息回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuerySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        OnLevelItemClick(null, currentSelectLevel, null);
    }
    private void OnQueryFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1468) + CommonTools.f_GetTransLanguage((int)obj));
    }

    /// <summary>
    /// 购买等级礼包回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnBuySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        _contentWrapComponet.f_UpdateView();
        //更新等级item红点信息
        UpdateLevelItemRedPoint(dicAllLevelButton[currentSelectLevel].transform, currentSelectLevel);
        _contentWrapComponet.f_UpdateView();


        RankGiftDT rankGiftDT = currentSelectPoolDT.m_RankGiftDT;
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)rankGiftDT.iRewardType, rankGiftDT.iRewardId, rankGiftDT.iRewardCount);
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
    }
    private void OnBuyFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1469) + CommonTools.f_GetTransLanguage((int)obj));
    }
    private void UpdateLevelItemRedPoint(Transform item, int level)
    {
        RankGiftDT ranGiftDT = listRankGiftData[level];
        bool isGetLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= ranGiftDT.iOpenLevel;
        bool isRedPoint = false;
        List<BasePoolDT<long>> listContent = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_RankGiftPool.f_GetAll());
        if (isGetLv)
        {
            for (int i = listContent.Count - 1; i >= 0; i--)
            {
                if ((listContent[i] as RankGiftPoolDT).m_RankGiftDT.iOpenLevel == level)
                {
                    RankGiftPoolDT rankGiftPoolDT = listContent[i] as RankGiftPoolDT;
                    int timeGone = GameSocket.GetInstance().f_GetServerTime() - rankGiftPoolDT.m_levelTime;
                    if (timeGone > 24 * 60 * 60)
                    {
                        //已过期
                        break;
                    }
                    else
                    {
                        int buyTimesLeft = rankGiftPoolDT.m_RankGiftDT.iBuyTime - rankGiftPoolDT.m_buyTimes;
                        if (buyTimesLeft > 0)
                        {
                            isRedPoint = true;
                            break;
                        }
                    }
                }
            }
        }
        item.GetComponent<RankGiftLevelItemCtl>().ShowRedPoint(isRedPoint);
    }
}
