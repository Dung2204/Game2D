using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 军团排行榜界面
/// </summary>
public class LegionRankPage : UIFramwork
{
    private UIWrapComponent _LevelItemComponent = null;//组件
    private List<BasePoolDT<long>> _LevellistData = new List<BasePoolDT<long>>();//数据
    private UIWrapComponent _DungItemComponent = null;//组件
    private List<BasePoolDT<long>> _DunglistData = new List<BasePoolDT<long>>();//数据
    private EM_PageIndex curPageIndex = EM_PageIndex.LevelRank;//当前切页
    private SocketCallbackDT InfoCallback = new SocketCallbackDT();//查询回调
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        curPageIndex = EM_PageIndex.LevelRank;
        //请求等级排行榜信息
        LegionMain.GetInstance().m_LegionRankPool.f_GetLvRank(InfoCallback);
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnLevelTap", OnBtnLevelTapClick);
        f_RegClickEvent("BtnDungeonTap", OnBtnLuckTapClick);
        f_RegClickEvent("MaskClose", OnMaskClick);
        f_RegClickEvent("BtnClose", OnBtnCloseClick);
        f_RegClickEvent("BtnClose2", OnBtnCloseClick);
        InfoCallback.m_ccCallbackSuc = OnInfoSucCall;
        InfoCallback.m_ccCallbackFail = OnInfoFailCall;
    }
    #region 服务器回调
    /// <summary>
    /// 查询信息成功
    /// </summary>
    private void OnInfoSucCall(object obj)
    {
        ShowContent(curPageIndex);
    }
    /// <summary>
    /// 查询失败
    /// </summary>
    private void OnInfoFailCall(object obj)
    {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Query failed！");
    }
    #endregion
    /// <summary>
    /// 显示内容
    /// </summary>
    /// <param name="pageIndex">分页类型</param>
    private void ShowContent(EM_PageIndex pageIndex)
    {
        f_GetObject("LevelTap").SetActive(pageIndex == EM_PageIndex.LevelRank);
        f_GetObject("DungeonTap").SetActive(pageIndex == EM_PageIndex.DungeonRank);
        f_GetObject("BtnLevelTap").transform.Find("SprSelect").gameObject.SetActive(pageIndex == EM_PageIndex.LevelRank);
        f_GetObject("BtnDungeonTap").transform.Find("SprSelect").gameObject.SetActive(pageIndex == EM_PageIndex.DungeonRank);
        if (pageIndex == EM_PageIndex.LevelRank)//刷新数据
        {
            _LevellistData = LegionMain.GetInstance().m_LegionRankPool.listLevelRankLegion;
            if (_LevelItemComponent == null)
            {
                _LevelItemComponent = new UIWrapComponent(220, 1, 820, 9, f_GetObject("LevelItemParent"), f_GetObject("LevelItem"), _LevellistData, OnLevelItemUpdate, null);
            }
            _LevelItemComponent.f_UpdateList(_LevellistData);
            _LevelItemComponent.f_ResetView();
            _LevelItemComponent.f_UpdateView();
        }
        else
        {
            _DunglistData = LegionMain.GetInstance().m_LegionRankPool.listDungeonRankLegion;
            if (_DungItemComponent == null)
            {
                _DungItemComponent = new UIWrapComponent(220, 1, 820, 9, f_GetObject("DungeonItemParent"), f_GetObject("DungeonItem"), _DunglistData, OnDungeonItemUpdate, null);
            }
            _DungItemComponent.f_UpdateList(_DunglistData);
            _DungItemComponent.f_ResetView();
            _DungItemComponent.f_UpdateView();
        }
    }
    /// <summary>
    /// item更新
    /// </summary>
    private void OnLevelItemUpdate(Transform t, BasePoolDT<long> dt)
    {
        LegionLevelRankItem item = dt as LegionLevelRankItem;
        LegionInfoPoolDT legionInfoPoolDT = item.legionInfoPoolDT;
        int lv = legionInfoPoolDT.f_GetProperty((int)EM_LegionProperty.Lv);
        int maxMember = (glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetSC(lv) as LegionLevelDT).iCountMax;
t.GetComponent<LegionRankItem>().SetData(item.legionInfoPoolDT.f_GetProperty((int)EM_LegionProperty.Icon), item.Rank, legionInfoPoolDT.LegionName, lv, "Captain: "+ item.m_szLeaderName,
            legionInfoPoolDT.f_GetProperty((int)EM_LegionProperty.MemberNum), maxMember, "");
    }
    /// <summary>
    /// item更新
    /// </summary>
    private void OnDungeonItemUpdate(Transform t, BasePoolDT<long> dt)
    {
        LegionDungeRankItem item = dt as LegionDungeRankItem;
        LegionInfoPoolDT legionInfoPoolDT = item.legionInfoPoolDT;
        int lv = legionInfoPoolDT.f_GetProperty((int)EM_LegionProperty.Lv);
        int maxMember = (glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetSC(lv) as LegionLevelDT).iCountMax;
        string pveProgress = (glo_Main.GetInstance().m_SC_Pool.m_LegionTollgateSC.f_GetSC(item.progress) as LegionTollgateDT).szName;
t.GetComponent<LegionRankItem>().SetData(item.legionInfoPoolDT.f_GetProperty((int)EM_LegionProperty.Icon), item.Rank, legionInfoPoolDT.LegionName, lv, "Captain: " + item.m_szLeaderName,
            legionInfoPoolDT.f_GetProperty((int)EM_LegionProperty.MemberNum), maxMember, pveProgress);
    }
    #region 按钮事件
    /// <summary>
    /// 点击黑色遮罩背景，关闭页面
    /// </summary>
    private void OnMaskClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionRankPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    private void OnBtnCloseClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionRankPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击等级排行按钮
    /// </summary>
    private void OnBtnLevelTapClick(GameObject go, object obj1, object obj2)
    {
        curPageIndex = EM_PageIndex.LevelRank;
        ShowContent(EM_PageIndex.LevelRank);
        LegionMain.GetInstance().m_LegionRankPool.f_GetLvRank(InfoCallback);
    }
    /// <summary>
    /// 点击副本排行按钮
    /// </summary>
    private void OnBtnLuckTapClick(GameObject go, object obj1, object obj2)
    {
        curPageIndex = EM_PageIndex.DungeonRank;
        ShowContent(EM_PageIndex.DungeonRank);
        LegionMain.GetInstance().m_LegionRankPool.f_GetPveRank(InfoCallback);
    }
    #endregion
    /// <summary>
    /// 分页类型
    /// </summary>
    private enum EM_PageIndex
    {
        /// <summary>
        /// 等级排行榜分页
        /// </summary>
        LevelRank = 1,
        /// <summary>
        /// 副本排行榜分页
        /// </summary>
        DungeonRank = 2,
    }
}
