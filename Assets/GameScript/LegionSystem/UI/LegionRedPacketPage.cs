using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 发红包界面
/// </summary>
public class LegionRedPacketPage : UIFramwork
{
    private UIWrapComponent _RedItemComponent = null;//红包组件
    private List<BasePoolDT<long>> _listRedPacketData = new List<BasePoolDT<long>>();//数据
    private EM_RedPacket emRedPacket = EM_RedPacket.Packet200;//选中的红包类型
    private SocketCallbackDT InfoCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT GetCallback = new SocketCallbackDT();//领取回调
    private SocketCallbackDT SendPacketCallback = new SocketCallbackDT();//发红包回调

    private string szCenterBgFile = "UI/TextureRemove/NewServer/Texture_SevenDayAwardBg";
    private const int ShowModeId = 11191;
    private Transform mRoleParent;
    private GameObject role;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mRoleParent = f_GetObject("RoleParent").transform;
    }

    private void f_LoadTexture()
    {
        //f_GetObject("ReceiveTapTex").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szCenterBgFile);
        //f_GetObject("SendTapTex").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szCenterBgFile);
    }

    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        ShowContent(EM_PageIndex.ReceiveMoney);//默认显示收红包页面
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionRedPacketPool.f_GetRedPacketInfo(InfoCallback);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        UITool.f_CreateRoleByModeId(ShowModeId, ref role, mRoleParent, 1);
        f_LoadTexture();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        if (role != null)
        {
            UITool.f_DestoryStatelObject(role);
            role = null;
        }
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnSendPage", OnBtnSendMoneyTapClick);
        f_RegClickEvent("BtnReceiveTap", OnBtnReceiveMoneyTapClick);
        f_RegClickEvent("BtnSendTap", OnBtnSendMoneyTapClick);
        f_RegClickEvent("BackBtn", OnBtnBackClick);
        f_RegClickEvent("BtnSendMoney", OnBtnSendMoneyClick);
        f_RegClickEvent("BtnRed200", OnBtnRedToggleClick, EM_RedPacket.Packet200);
        f_RegClickEvent("BtnRed500", OnBtnRedToggleClick, EM_RedPacket.Packet500);
        f_RegClickEvent("BtnRed1000", OnBtnRedToggleClick, EM_RedPacket.Packet1000);
        f_RegClickEvent("BtnRank", OnBtnRankClick);
        InfoCallback.m_ccCallbackSuc = OnInfoSucCall;
        InfoCallback.m_ccCallbackFail = OnInfoFailCall;
        GetCallback.m_ccCallbackSuc = OnGetSucCall;
        GetCallback.m_ccCallbackFail = OnGetFailCall;
        SendPacketCallback.m_ccCallbackSuc = OnSendPacketSucCall;
        SendPacketCallback.m_ccCallbackFail = OnSendPacketFailCall;
    }
    /// <summary>
    /// 显示内容
    /// </summary>
    /// <param name="pageIndex">分页类型</param>
    private void ShowContent(EM_PageIndex pageIndex)
    {
        f_GetObject("ReceiveMoneyTap").SetActive(pageIndex == EM_PageIndex.ReceiveMoney);
        f_GetObject("SendMoneyTap").SetActive(pageIndex == EM_PageIndex.SendMoney);
        f_GetObject("BtnReceiveTap").transform.Find("SprSelect").gameObject.SetActive(pageIndex == EM_PageIndex.ReceiveMoney);
        f_GetObject("BtnSendTap").transform.Find("SprSelect").gameObject.SetActive(pageIndex == EM_PageIndex.SendMoney);
        int curSendPacketTimes = LegionMain.GetInstance().m_LegionRedPacketPool.m_curSendPacketTimes;
        int maxSendPacketTimes = LegionMain.GetInstance().m_LegionRedPacketPool.m_DaySendPacketTimes;
        f_GetObject("LabelSendHintLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(718)+ curSendPacketTimes + "/" + maxSendPacketTimes;//今日可发次数
        f_GetObject("TodayLeftRecvTimes").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(719), LegionMain.GetInstance().m_LegionRedPacketPool.m_TodayLeftRecvTimes);//剩余可领取次数
        if (pageIndex == EM_PageIndex.ReceiveMoney)//刷新数据
        {
            _listRedPacketData = LegionMain.GetInstance().m_LegionRedPacketPool.f_GetAll();
            if (_RedItemComponent == null)
            {
                _RedItemComponent = new UIWrapComponent(207, 2, 650, 5, f_GetObject("ItemParent"), f_GetObject("RedPacketItem"), _listRedPacketData, OnItemUpdate, null);
                _RedItemComponent.f_ResetView();
            }
            _RedItemComponent.f_UpdateList(_listRedPacketData);
            _RedItemComponent.f_ResetView();
            f_GetObject("ReceiveMoneyTap").transform.Find("Panel/BtnSendPage").gameObject.SetActive(_listRedPacketData.Count <= 0 ? true : false);
        }
    }
    /// <summary>
    /// 红包item更新
    /// </summary>
    private void OnItemUpdate(Transform t, BasePoolDT<long> dt)
    {
        f_RegClickEvent(t.Find("BtnGet").gameObject, OnGetClick, dt);
        LegionRedPacketPoolDT pooldt = dt as LegionRedPacketPoolDT;
        int totalSycee = LegionMain.GetInstance().m_LegionRedPacketPool.m_dicRedPacketToSycee[pooldt.em_redPacket];
        int totalCount = LegionMain.GetInstance().m_LegionRedPacketPool.m_dicRedPacketToCount[pooldt.em_redPacket];
        t.Find("BtnLock").gameObject.SetActive(pooldt.m_CurSycee >= totalSycee || pooldt.m_CurCount >= totalCount);
        t.Find("BtnGet").gameObject.SetActive(pooldt.m_CurSycee < totalSycee && pooldt.m_CurCount < totalCount && !pooldt.m_MyIsGet);
        t.Find("HasGet").gameObject.SetActive(pooldt.m_CurSycee < totalSycee && pooldt.m_CurCount < totalCount && pooldt.m_MyIsGet);
        string tUserName = pooldt.m_BasePlayerPoolDT != null ? pooldt.m_BasePlayerPoolDT.m_szName : string.Empty;
        t.GetComponent<RedPacketItem>().SetData(GetPackSpriteNameByType(pooldt.em_redPacket), tUserName, totalSycee + CommonTools.f_GetTransLanguage(720), pooldt.m_CurCount,//元宝
            LegionMain.GetInstance().m_LegionRedPacketPool.m_dicRedPacketToCount[pooldt.em_redPacket], pooldt.m_CurSycee, totalSycee);
    }
    /// <summary>
    /// 获取红包spriteName
    /// </summary>
    private string GetPackSpriteNameByType(EM_RedPacket redPacketType)
    {
        string spriteName = "";
        switch (redPacketType)
        {
            case EM_RedPacket.Packet200: spriteName = "Icon_RedPacket5"; break;
            case EM_RedPacket.Packet500: spriteName = "Icon_RedPacket10"; break;
            case EM_RedPacket.Packet1000: spriteName = "Icon_RedPacket20"; break;
        }
        return spriteName;
    }

    private long curGetRedPacketId = 0;
    /// <summary>
    /// 点击红包领取
    /// </summary>
    private void OnGetClick(GameObject go, object obj1, object obj2)
    {
        BasePoolDT<long> item = (BasePoolDT<long>)obj1;
        if (LegionMain.GetInstance().m_LegionRedPacketPool.m_TodayLeftRecvTimes <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(721));//今日剩余领取次数已用完
            return;
        }
        curGetRedPacketId = item.iId;
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionRedPacketPool.f_ReceiveRedPacket(item.iId, GetCallback, OnGetSyceeCallback);
    }
    #region 服务器回调
    /// <summary>
    /// 查询信息成功
    /// </summary>
    private void OnInfoSucCall(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ShowContent(EM_PageIndex.ReceiveMoney);//默认显示收红包页面
        if (_RedItemComponent != null)
            _RedItemComponent.f_ResetView();
    }
    /// <summary>
    /// 查询失败
    /// </summary>
    private void OnInfoFailCall(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(722));//查询失败 
    }
    /// <summary>
    /// 领红包收到多少元宝回调
    /// </summary>
    private void OnGetSyceeCallback(object obj)
    {
        int syceeCount = (int)obj;
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee, syceeCount);
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
    }
    /// <summary>
    /// 领取成功回调
    /// </summary>
    private void OnGetSucCall(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false); 
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(723));//领取成功
        if (_RedItemComponent != null)
            _RedItemComponent.f_UpdateView();
        LegionMain.GetInstance().m_LegionRedPacketPool.f_CheckReddot();
        f_GetObject("TodayLeftRecvTimes").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(724), LegionMain.GetInstance().m_LegionRedPacketPool.m_TodayLeftRecvTimes);//剩余可领取次数
    }
    /// <summary>
    /// 领取失败回调
    /// </summary>
    private void OnGetFailCall(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)obj == (int)eMsgOperateResult.eOR_OutOfTimeRange)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(725));//红包已过期
            //删除过期红包
            if (!LegionMain.GetInstance().m_LegionRedPacketPool.f_Delete(curGetRedPacketId))
                MessageBox.ASSERT("Legion RedPacket Delete error, RedPacket id = " + curGetRedPacketId);
        }
           
        else if ((int)obj == (int)eMsgOperateResult.eOR_TimesLimit)
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(726));//红包剩余领取次数已用完
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(727) + (int)obj);//红包领取错误
            MessageBox.ASSERT("Legion RedPacket get error,code;" + obj);
        }
        //更新界面
        if (_RedItemComponent != null)
            _RedItemComponent.f_UpdateView();
        LegionMain.GetInstance().m_LegionRedPacketPool.f_CheckReddot();               //剩余可领取次数
        f_GetObject("TodayLeftRecvTimes").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(728), LegionMain.GetInstance().m_LegionRedPacketPool.m_TodayLeftRecvTimes);
    }
    /// <summary>
    /// 发红包成功回调
    /// </summary>
    private void OnSendPacketSucCall(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(729));//发放成功
        if (_RedItemComponent != null)
            _RedItemComponent.f_ResetView();
        int curSendPacketTimes = LegionMain.GetInstance().m_LegionRedPacketPool.m_curSendPacketTimes;
        int maxSendPacketTimes = LegionMain.GetInstance().m_LegionRedPacketPool.m_DaySendPacketTimes;
        f_GetObject("LabelSendHintLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(730) + curSendPacketTimes + "/" + maxSendPacketTimes;
    }
    /// <summary>
    /// 发红包失败回调
    /// </summary>
    private void OnSendPacketFailCall(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        MessageBox.ASSERT("Legion RedPacket send error,code:" + obj);
    }
    #endregion
    #region 按钮事件

    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    private void OnBtnBackClick(GameObject go,object obj1,object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionRedPacketPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击收红包分页按钮
    /// </summary>
    private void OnBtnReceiveMoneyTapClick(GameObject go, object obj1, object obj2)
    {
        ShowContent(EM_PageIndex.ReceiveMoney);
    }
    /// <summary>
    /// 点击发红包分页按钮
    /// </summary>
    private void OnBtnSendMoneyTapClick(GameObject go, object obj1, object obj2)
    {
        ShowContent(EM_PageIndex.SendMoney);
    }
    /// <summary>
    /// 点击选中的发红包类型
    /// </summary>
    private void OnBtnRedToggleClick(GameObject go, object obj1, object obj2)
    {
        int value = (int)obj1;
        emRedPacket = (EM_RedPacket)obj1;
    }
    /// <summary>
    /// 点击发红包按钮
    /// </summary>
    private void OnBtnSendMoneyClick(GameObject go, object obj1, object obj2)
    {
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(731) + emRedPacket);//点击发红包
        int curSendPacketTimes = LegionMain.GetInstance().m_LegionRedPacketPool.m_curSendPacketTimes;
        int maxSendPacketTimes = LegionMain.GetInstance().m_LegionRedPacketPool.m_DaySendPacketTimes;
        int syceeCost = LegionMain.GetInstance().m_LegionRedPacketPool.m_dicRedPacketToSycee[emRedPacket];

        if (curSendPacketTimes >= maxSendPacketTimes)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(732), maxSendPacketTimes));//"每人每天发红包不可超过" + maxSendPacketTimes + "次");
            return;
        }
        else if (!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Sycee, syceeCost,true,true,this))
            return; 
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionRedPacketPool.f_SendRedPacket(emRedPacket, SendPacketCallback);
    }
    /// <summary>
    /// 点击排行榜按钮
    /// </summary>
    private void OnBtnRankClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionRedRankPage, UIMessageDef.UI_OPEN);
    }
    #endregion

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionRedPacketPage, UIMessageDef.UI_CLOSE);
    }

    /// <summary>
    /// 分页类型
    /// </summary>
    private enum EM_PageIndex
    {
        /// <summary>
        /// 收红包分页
        /// </summary>
        ReceiveMoney = 1,
        /// <summary>
        /// 发红包分页
        /// </summary>
        SendMoney = 2,
    }

    #region 红点

    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LegionRedPacket, f_GetObject("BtnReceiveTap"), ReddotCallback_Show_BtnReceiveTap, true);
        UpdateReddotUI();
    }

    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegionRedPacket);
    }

    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.LegionRedPacket, f_GetObject("BtnReceiveTap"));
    }

    private void ReddotCallback_Show_BtnReceiveTap(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnReceiveTap = f_GetObject("BtnReceiveTap");
        UITool.f_UpdateReddot(BtnReceiveTap, iNum, new Vector3(114, 30, 0), 401);
    }

    #endregion
}
