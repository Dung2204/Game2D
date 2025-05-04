using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 迎财神
/// </summary>
public class ActWealthManCtl : UIFramwork
{
    private SocketCallbackDT RequestWelcomeMoneyCallback = new SocketCallbackDT();//回调
    private SocketCallbackDT RequestQueryCallback = new SocketCallbackDT();//回调
    private SocketCallbackDT GetOtherAwardCallback = new SocketCallbackDT();//领取宝箱奖励回调
    private int timeLeft;//剩余时间
    private int maxWealthTimes = 3;//每天最大的迎财次数

    private int tempGetOherAwardCount;//保存领取宝箱银币数量
    private string strTexBgRoot = "UI/TextureRemove/Activity/Tex_WealthManBg";
    private string strTexBtnTreasureBowlRoot = "UI/TextureRemove/Activity/Tex_WealthMan";
    private string strTexIconSyceeRoot = "UI/TextureRemove/Activity/Tex_WealthManSycee";
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
        TimerControl(false);
    }
    public void f_ShowView()
    {
        gameObject.SetActive(true);
        TimerControl(false);
        f_RegClickEvent("BtnWealth", OnWelcomeMoneyClick); 
        f_RegClickEvent("IconSycee", OnBtnTreasureBowlClick);
        RequestWelcomeMoneyCallback.m_ccCallbackSuc = OnWelcomeMoneySucCallback;
        RequestWelcomeMoneyCallback.m_ccCallbackFail = OnWelcomeMoneyFailCallback;

        RequestQueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        RequestQueryCallback.m_ccCallbackFail = OnQueryFailCallback;

        GetOtherAwardCallback.m_ccCallbackSuc = OnGetOtherAwardSucCallback;
        GetOtherAwardCallback.m_ccCallbackFail = OnGetOtherAwardFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_QueryWealthManInfo(RequestQueryCallback);
        f_LoadTexture();
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;

            //UITexture TexBtnTreasureBowl = f_GetObject("BtnTreasureBowl").GetComponent<UITexture>();
            //Texture2D tTexBtnTreasureBowl = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBtnTreasureBowlRoot);
            //TexBtnTreasureBowl.mainTexture = tTexBtnTreasureBowl;

            UITexture TexIconSycee = f_GetObject("IconSycee").GetComponent<UITexture>();
            Texture2D tTexIconSycee = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexIconSyceeRoot);
            TexIconSycee.mainTexture = tTexIconSycee;
        }
    }
    /// <summary>
    /// 点击聚宝盆
    /// </summary>
    private void OnBtnTreasureBowlClick(GameObject go,object obj1,object obj2)
    {
        int totalTimes = Data_Pool.m_ActivityCommonData.WealthTotalTimes;
        if (totalTimes < 6 || !Data_Pool.m_ActivityCommonData.WealthBoxCanGet)
            return;
        int level = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        WealthManDT wealthManDT = glo_Main.GetInstance().m_SC_Pool.m_WealthManSC.f_GetSC(level) as WealthManDT;
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT tPool1 = new AwardPoolDT();
        tPool1.f_UpdateByInfo((byte)wealthManDT.iTotalRewardType, wealthManDT.iTotalRewardId, Data_Pool.m_ActivityCommonData.WealthTotalFortune);
        awardList.Add(tPool1);
        EM_AwardGetState getState = EM_AwardGetState.Lock;
        if (Data_Pool.m_ActivityCommonData.WealthBoxCanGet)
            getState = EM_AwardGetState.CanGet;
        else if (Data_Pool.m_ActivityCommonData.WealthTotalTimes >= 6)
            getState = EM_AwardGetState.AlreadyGet;
        AwardGetSubPageParam param = new AwardGetSubPageParam(getState, CommonTools.f_GetTransLanguage(1322), CommonTools.f_GetTransLanguage(1557), awardList, OnGetHandle, null, this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AwardGetSubPage, UIMessageDef.UI_OPEN, param);
    }
    /// <summary>
    /// 点击领取奖励
    /// </summary>
    /// <param name="data"></param>
    private void OnGetHandle(object data)
    {
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "领取成功!");
        tempGetOherAwardCount = Data_Pool.m_ActivityCommonData.WealthTotalFortune;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_WealthManFortune(GetOtherAwardCallback);
    }
    /// <summary>
    /// 点击迎财
    /// </summary>
    /// <param name="go"></param>
    private void OnWelcomeMoneyClick(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_ActivityCommonData.WealthDayTimes >= maxWealthTimes)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(1323), maxWealthTimes));
            return;
        }
        if (timeLeft > 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1324) + CommonTools.f_GetStringBySecond(timeLeft));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_WealthManGet(RequestWelcomeMoneyCallback);
    }
    /// <summary>
    /// 是否开启定时
    /// </summary>
    /// <param name="isStart"></param>
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
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            TimerControl(false);
            f_GetObject("LabelTimeLeft").SetActive(false);
        }
        f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = CommonTools.f_GetStringBySecond(timeLeft);
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    private void InitUI()
    {
        //1.招财次数
        int times = Data_Pool.m_ActivityCommonData.WealthDayTimes;
        f_GetObject("LabelWealthTimes").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1325), times, maxWealthTimes);
        f_GetObject("LabelWealthTimes").SetActive(true);
        //2.时间倒计时
        int lastWealthTime = Data_Pool.m_ActivityCommonData.lastWealthTime;
        
        if (times > 0 && times < maxWealthTimes)
        {
            timeLeft = 15 * 60 - (GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_ActivityCommonData.lastWealthTime);
            TimerControl(true);
            f_GetObject("LabelTimeLeft").SetActive(true);
        }
        else
        {
            f_GetObject("LabelTimeLeft").SetActive(false);
        }
        //3.累积次数
        int totalTimes = Data_Pool.m_ActivityCommonData.WealthTotalTimes;
        //GameObject LabelTotalGet = f_GetObject("LabelTotalGet");
        //f_GetObject("IconSycee").SetActive(totalTimes > 0 ? true : false);
        //f_GetObject("SprGetHintBg").SetActive(totalTimes >= 0 ? true : false);
        //f_GetObject("IconSycee").transform.localPosition = new Vector3(-22, -160 + totalTimes * 30);
        //LabelTotalGet.SetActive(totalTimes > 0 ? true : false);
        if (totalTimes > 0)
        {
            int level = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            WealthManDT wealthManDT = glo_Main.GetInstance().m_SC_Pool.m_WealthManSC.f_GetSC(level) as WealthManDT;
            string goodName = UITool.f_GetGoodName((EM_ResourceType)wealthManDT.iTotalRewardType, wealthManDT.iTotalRewardId);
            int goodCount = Data_Pool.m_ActivityCommonData.WealthTotalFortune;
            string strGoodCount = UITool.f_CountToChineseStr(goodCount);
            string HintStr = string.Format(CommonTools.f_GetTransLanguage(1326), strGoodCount + goodName, 6 - totalTimes);
            string HintStr2 = string.Format(CommonTools.f_GetTransLanguage(1327), strGoodCount, goodName, 6 - totalTimes);
            if (totalTimes >= 6)
            {
                if (Data_Pool.m_ActivityCommonData.WealthBoxCanGet)
                {
                    HintStr = string.Format(CommonTools.f_GetTransLanguage(1328), strGoodCount + goodName);
                    HintStr2 = string.Format(CommonTools.f_GetTransLanguage(1329), strGoodCount, goodName);
                }
                else
                {
                    //f_GetObject("IconSycee").SetActive(false);
                    HintStr = CommonTools.f_GetTransLanguage(1330);
                    HintStr2 = CommonTools.f_GetTransLanguage(1331);
                }
            }
            //LabelTotalGet.GetComponent<UILabel>().text = HintStr;
            f_GetObject("SprGetHintBg").GetComponentInChildren<UILabel>().text = HintStr2;
        }
        if (totalTimes == 0)
        {
            f_GetObject("SprGetHintBg").GetComponentInChildren<UILabel>().text = CommonTools.f_GetTransLanguage(1331);// "[502a15]点聚宝盆，有惊喜哦~";
        }
    }
    #region 迎财回调
    /// <summary>
    /// 迎财回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnWelcomeMoneySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "迎财成功！");
        InitUI();
        //展示获得奖励
        int level = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        WealthManDT wealthManDT = glo_Main.GetInstance().m_SC_Pool.m_WealthManSC.f_GetSC(level) as WealthManDT;
        //List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT tPool1 = new AwardPoolDT();
        tPool1.f_UpdateByInfo((byte)wealthManDT.iOneRewardType, wealthManDT.iOneRewardId, wealthManDT.iOneRewardCount);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1332) + tPool1.mTemplate.mResourceNum + tPool1.mTemplate.mName);
        //awardList.Add(tPool1);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
        //    new object[] { awardList });
        ////Data_Pool.m_ActivityCommonData.SetWelthInitFromSer = false;
        ////Data_Pool.m_ActivityCommonData.f_QueryWealthManInfo(RequestQueryCallback);
    }
    private void OnWelcomeMoneyFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1333) + CommonTools.f_GetTransLanguage((int)obj));
    }

    #endregion
    #region 满6次领取奖励回调
    /// <summary>
    /// 满6次领取奖励回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetOtherAwardSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AwardGetSubPage, UIMessageDef.UI_CLOSE);
        InitUI();
        //展示获得奖励
        int level = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        WealthManDT wealthManDT = glo_Main.GetInstance().m_SC_Pool.m_WealthManSC.f_GetSC(level) as WealthManDT;
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT tPool1 = new AwardPoolDT();
        tPool1.f_UpdateByInfo((byte)wealthManDT.iOneRewardType, wealthManDT.iOneRewardId, tempGetOherAwardCount);
        awardList.Add(tPool1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
    }
    private void OnGetOtherAwardFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1334) + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion
    #region 请求信息回调
    /// <summary>
    /// 招财符信息回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuerySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        InitUI();
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "迎财信息成功！");
    }
    private void OnQueryFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1335) + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion
}
