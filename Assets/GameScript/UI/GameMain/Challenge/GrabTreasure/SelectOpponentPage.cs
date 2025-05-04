using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 夺宝系统的选择对手界面
/// </summary>
public class SelectOpponentPage : UIFramwork
{
    public List<SC_GrabTreasureInfoNode> listRobotData = new List<SC_GrabTreasureInfoNode>();//机器人列表
    private int treasureFragID;
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT GrabCallback = new SocketCallbackDT();//夺宝回调
    private SocketCallbackDT GrabSweepCallback = new SocketCallbackDT();//夺宝扫荡回调
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        treasureFragID = (int)e;
        Data_Pool.m_GrabTreasurePool.f_RequestRobotInfo((short)treasureFragID, QueryCallback);
        f_GetObject("LabelVigorNow").GetComponent<UILabel>().text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor) + "/30";
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_GuidancePool.IsOpponent = false;
    }
    /// <summary>
    /// 页面关闭
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }
    /// <summary>
    /// 初始化机器人列表
    /// </summary>
    private void InitRobotData()
    {
        listRobotData.Clear();
        for (int i = 0; i < Data_Pool.m_GrabTreasurePool.robotInfoArray.Length; i++)
        {
            listRobotData.Add(Data_Pool.m_GrabTreasurePool.robotInfoArray[i]);
        }
        GridUtil.f_SetGridView<SC_GrabTreasureInfoNode>(f_GetObject("ItemParent"), f_GetObject("RobotItem"), listRobotData, UpdataRobotItem);
        f_GetObject("ItemParent").GetComponentInParent<UIScrollView>().ResetPosition();
    }
    /// <summary>
    /// 更新机器人item
    /// </summary>
    private void UpdataRobotItem(GameObject go, SC_GrabTreasureInfoNode item)
    {
        RobotItem robotItem = go.GetComponent<RobotItem>();
        string robotName = RandRobotName();
        int robotLevel = RandLevel();
        string probability = "";
        List<int> listData = new List<int>();
        bool isValid = false;
        for (int i = 0; i < item.tempCardIDArray.Length; i++)
        {
            if (item.tempCardIDArray[i] > 0)
            {
                isValid = true;
                listData.Add(item.tempCardIDArray[i]);
            }
        }
        int count = listData.Count;
        for (int j = 0; j < 6 - count; j++)
        {
            listData.Add(0);//未满6个补0
        }
        if (!isValid)
MessageBox.ASSERT("Bot error");
        int card1ID = listData[0];
        int card2ID = listData[1];
        int card3ID = listData[2];
        int card4ID = listData[3];
        int card5ID = listData[4];
        int card6ID = listData[5];
        robotItem.SetData(robotName, robotLevel, probability, card1ID, card2ID, card3ID, card4ID, card5ID, card6ID);
        f_RegClickEvent(robotItem.BtnFiveGrab, OnBtnFiveGrabClick, item.index);
        f_RegClickEvent(robotItem.BtnGrab, OnBtnGrabClick, item.index);
    }
    /// <summary>
    /// 随机一个机器人的名字
    /// </summary>
    /// <returns></returns>
    private string RandRobotName()
    {
        //随机一个机器人的名字
        string name = "";
        while (name.Length < 2)
        {
            int count = glo_Main.GetInstance().m_SC_Pool.m_RandNameSC.f_GetAll().Count;
            int random = Random.Range(1, count + 1);
            string name1 = (glo_Main.GetInstance().m_SC_Pool.m_RandNameSC.f_GetSC(random) as RandNameDT).szName1;
            random = Random.Range(1, count + 1);
            string name2 = (glo_Main.GetInstance().m_SC_Pool.m_RandNameSC.f_GetSC(random) as RandNameDT).szName2;
            random = Random.Range(1, count + 1);
            RandNameDT dt = glo_Main.GetInstance().m_SC_Pool.m_RandNameSC.f_GetSC(random) as RandNameDT;
            string name3 = Random.Range(1, 3) == 1 ? dt.szNameM : dt.szNameW;
            name = name1 + name2 + name3;
        }
        return name;
    }
    /// <summary>
    /// 随机一个等级（玩家当前等级上下浮动3）
    /// </summary>
    /// <returns></returns>
    private int RandLevel()
    {
        int PlayerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        int level = PlayerLevel + Random.Range(0, 7) - 3;
        return level;
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlackBg", OnBtnCloseClick);
        f_RegClickEvent("BtnClose", OnBtnCloseClick);
        f_RegClickEvent("BtnChange", OnBtnChangeClick);

        GrabCallback.m_ccCallbackSuc = OnGrabSucCallback;
        GrabCallback.m_ccCallbackFail = OnGrabFailCallback;
        GrabSweepCallback.m_ccCallbackSuc = OnGrabSweepSucCallback;
        GrabSweepCallback.m_ccCallbackFail = OnGrabSweepFailCallback;
        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
    }
    #region 请求夺宝回调
    /// <summary>
    /// 请求夺宝成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGrabSucCallback(object obj)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        //展示加载界面 并加载战斗场景
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectOpponentPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnGrabFailCallback(object obj)
    {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không thành công!");
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 请求夺宝扫荡成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGrabSweepSucCallback(object obj)
    {

    }
    private void OnGrabSweepFailCallback(object obj)
    {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không thành công!");
        UITool.f_OpenOrCloseWaitTip(false);
    }
    //扫荡结果回调
    private void OnGrabSweepCompled(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasurePage, UIMessageDef.UI_OPEN, GrabTreasurePage.curSelectTreasure.iId);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabSweepResultPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 查询成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuerySucCallback(object obj)
    {
        //更新UI显示
        InitRobotData();
        UITool.f_OpenOrCloseWaitTip(false);
        Data_Pool.m_GuidancePool.IsOpponent = true;
    }
    private void OnQueryFailCallback(object obj)
    {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không thành công!");
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
    #region 按钮事件
    /// <summary>
    /// 点击关闭按钮事件
    /// </summary>
    private void OnBtnCloseClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectOpponentPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击更换对手
    /// </summary>
    private void OnBtnChangeClick(GameObject go, object obj1, object obj2)
    {
        Data_Pool.m_GrabTreasurePool.f_RequestRobotInfo((short)treasureFragID, QueryCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    /// <summary>
    /// 点击抢夺按钮
    /// </summary>
    private void OnBtnGrabClick(GameObject go, object obj1, object obj2)
    {
        if (!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Vigor, 2, true, true, this))
            return;
        Data_Pool.m_GrabTreasurePool.f_GrabTreasure((short)treasureFragID, (byte)obj1, GrabCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    /// <summary>
    /// 点击抢夺5次
    /// </summary>
    private void OnBtnFiveGrabClick(GameObject go, object obj1, object obj2)
    {
        if (!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Vigor, 2, true, true, this))
            return;
        if (UITool.f_GetIsOpensystem(EM_NeedLevel.GrabTreasureFiveLevel) || Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_LootTreasure5Times) > 0)
        {
            //等级相关信息
            int lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            int exp = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Exp);
            StaticValue.m_sLvInfo.f_UpdateInfo(lv, exp);
            Data_Pool.m_GrabTreasurePool.f_GrabTreasureSweep((short)treasureFragID, (byte)obj1, GrabSweepCallback, OnGrabSweepCompled);
            UITool.f_OpenOrCloseWaitTip(true);
        }
        else
        {
            int tNeeLv = UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureFiveLevel);
            int tNeedVip = Data_Pool.m_RechargePool.f_GetVipLvLimit(EM_VipPrivilege.eVip_LootTreasure5Times);
            if (tNeeLv == 0)
            {
UITool.Ui_Trip(string.Format("Thắng 5 lần, VIP{0} mở", tNeedVip));
            }
            else if (tNeedVip == 0)
            {
UITool.Ui_Trip(string.Format("Thắng 5 lần, cấp {0} mở", tNeeLv));
            }
            else
            {
UITool.Ui_Trip(string.Format("Thắng 5 lần, cấp {0} hoặc VIP{1} mở", tNeeLv, tNeedVip));
            }
        }
    }
    #endregion
}
