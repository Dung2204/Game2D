using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 日常副本选择难度界面
/// </summary>
public class SelectDifficultyPage : UIFramwork {
    private DailyPveInfoPoolDT dailyPvePoolDT;//日常副本poolDT //选中的难度 普通、精英、英雄、史诗、传奇、神话，6个难度
    private List<SelectDifficultyItem> itemList;

    /// <summary>
    /// 页面打开
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        dailyPvePoolDT = (DailyPveInfoPoolDT)e;
        InitUI();
    }
    /// <summary>
    /// 设置UI
    /// </summary>
    private void InitUI()
    {
        List<DailyPveGateDT> listData = GetIndex(dailyPvePoolDT.m_DailyPveInfoDT.iId);
        GameObject ItemPrefab = f_GetObject("Item");
        Transform ItemParent = f_GetObject("ItemParent").transform;
        if(null == itemList) itemList = new List<SelectDifficultyItem>();
        for (int i = 0; i < listData.Count; i++)
        {
            DailyPveGateDT info = listData[i];
            if (i >= itemList.Count) {
                //不够则克隆
                GameObject Item = GameObject.Instantiate(ItemPrefab);
                Item.SetActive(true);
                Item.transform.parent = ItemParent;
                Item.transform.localEulerAngles = Vector3.zero;
                Item.transform.localScale = Vector3.one;
                Item.transform.localPosition = Vector3.zero;
                SelectDifficultyItem itemCtl = Item.GetComponent<SelectDifficultyItem>();
                itemList.Add(itemCtl);
            }

            SelectDifficultyItem selectDifficultyItem = itemList[i];
            selectDifficultyItem.InitData(info);
            f_RegClickEvent(selectDifficultyItem.BtnCharllenge, OnBtnCharllengeClick, info);
        }
        ItemParent.GetComponent<UIGrid>().Reposition();
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("CloseMask", OnBtnBlackClick);
        f_RegClickEvent("BtnClose", OnBtnBlackClick);
    }
    private void CallSuc(object result)
    {
        MessageBox.DEBUG("DailyPve Suc! code:" + result);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        //展示加载界面 并加载战斗场景
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectDifficultyPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
        UITool.f_OpenOrCloseWaitTip(false);
    }

    private void CallFail(object result)
    {
        MessageBox.DEBUG("DailyPve Error! code:" + result);
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #region 按钮事件
    /// <summary>
    /// 点击黑色背景，关闭界面
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectDifficultyPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击挑战
    /// </summary>
    private void OnBtnCharllengeClick(GameObject go, object obj1, object obj2)
    {
        SocketCallbackDT callback = new SocketCallbackDT();
        callback.m_ccCallbackSuc = CallSuc;
        callback.m_ccCallbackFail = CallFail;
        DailyPveGateDT info = (DailyPveGateDT)obj1;
        int PlayerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (PlayerLevel < info.iLevelLimit1)
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Cấp độ không đủ!");
            return;
        }
        string StrAward = info.szAward;
        string[] ArrayStrAward = StrAward.Split(';');
        List<AwardPoolDT> listAwardData = new List<AwardPoolDT>();
        AwardPoolDT awardPoolDT = new AwardPoolDT();
        awardPoolDT.f_UpdateByInfo(byte.Parse(ArrayStrAward[0]), int.Parse(ArrayStrAward[1]), int.Parse(ArrayStrAward[2]));
        listAwardData.Add(awardPoolDT);
        
        Data_Pool.m_DailyPveInfoPool.f_Challenge(info.iId,info.iType, listAwardData, callback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    /// <summary>
    /// 通过日常副本类型获取解锁列表
    /// </summary>
    /// <param name="dailyType"></param>
    /// <returns></returns>
    private List<DailyPveGateDT> GetIndex(int dailyType)
    {
        List<DailyPveGateDT> listNewData = new List<DailyPveGateDT>();
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_DailyPveGateSC.f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            DailyPveGateDT dt = listData[i] as DailyPveGateDT;
            if (dt.iType == dailyType)
            {
                listNewData.Add(dt);
            }
        }
        return listNewData;
    }
    #endregion
}
