using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 红包排行榜界面
/// </summary>
public class LegionRedRankPage : UIFramwork
{
    private UIWrapComponent _ItemComponent = null;//组件
    private List<BasePoolDT<long>> _listData = new List<BasePoolDT<long>>();//数据
    private SocketCallbackDT InfoCallback = new SocketCallbackDT();//查询回调
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        ShowContent();
        //请求排行榜信息
        LegionMain.GetInstance().m_LegionRedPacketPool.f_GetRankInfo(InfoCallback);
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("MaskClose", OnMaskClick);
        f_RegClickEvent("BtnClose", OnBtnCloseClick);
        InfoCallback.m_ccCallbackSuc = OnInfoSucCall;
        InfoCallback.m_ccCallbackFail = OnInfoFailCall;
    }
    /// <summary>
    /// 显示内容
    /// </summary>
    private void ShowContent()
    {
        f_GetObject("BlissTap").transform.Find("LabelHint/LabelSycCount").GetComponent<UILabel>().text
                = LegionMain.GetInstance().m_LegionRedPacketPool.m_MySendSyceeCount.ToString();
        _listData = LegionMain.GetInstance().m_LegionRedPacketPool.listBlissRank;
        int myRank = GetMyRank(_listData);
f_GetObject("LabelMyRank").GetComponent<UILabel>().text = myRank == -1? "[F5BF3DFF]Myself：[00FE0A]No Rating" : "[F5BF3DFF]Myself：[00FE0A]" + myRank;
        if (_ItemComponent == null)
        {
            _ItemComponent = new UIWrapComponent(220, 1, 820, 9, f_GetObject("ItemParent"), f_GetObject("Item"), _listData, OnItemUpdate, null);
        }
        _ItemComponent.f_UpdateList(_listData);
        _ItemComponent.f_ResetView();
        _ItemComponent.f_UpdateView();
        f_GetObject("BlissTap").transform.Find("LabelNoDataHint").gameObject.SetActive(_listData.Count <= 0 ? true : false);
    }
    /// <summary>
    /// 获取我的排名，没有则返回-1
    /// </summary>
    /// <returns></returns>
    private int GetMyRank(List<BasePoolDT<long>> listData)
    {
        for (int i = 0; i < listData.Count; i++)
        {
            LegionRedPacketRankPoolDT dt = listData[i] as LegionRedPacketRankPoolDT;
            if (dt.iId == Data_Pool.m_UserData.m_iUserId)
            {
                return dt.m_Rank;
            }
        }
        return -1;
    }
    /// <summary>
    /// item更新
    /// </summary>
    private void OnItemUpdate(Transform t, BasePoolDT<long> dt)
    {
        LegionRedPacketRankPoolDT item = dt as LegionRedPacketRankPoolDT;
        f_RegClickEvent(t.GetComponent<LegionRedRankItem>().m_SprIcon.gameObject, OnPlayerIconClick, item.m_BasePlayerPoolDT);
		//My Code
		int sexId = item.m_BasePlayerPoolDT.m_iSex;
		FashionableDressDT tFashionableDressDT = glo_Main.GetInstance().m_SC_Pool.m_FashionableDressSC.f_GetSC(sexId) as FashionableDressDT;
        if (tFashionableDressDT != null)
        {
            sexId = tFashionableDressDT.iIcon;
        }
		//
        if (item.iId == Data_Pool.m_UserData.m_iUserId)
        {
            string playerName = Data_Pool.m_UserData.m_szRoleName;
            TeamPoolDT teamPoolDT = Data_Pool.m_TeamPool.f_GetForId((int)EM_FormationPos.eFormationPos_Main) as TeamPoolDT;
            int cardIcon = (int)teamPoolDT.m_CardPoolDT.m_CardDT.iId;
            int vip = UITool.f_GetNowVipLv();
            int level = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            string LegionName = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().LegionName;
            t.GetComponent<LegionRedRankItem>().SetData(cardIcon, teamPoolDT.m_CardPoolDT.m_CardDT.iImportant, vip, item.m_Rank, playerName, level, item.m_SyceeCount, LegionName, sexId);
        }
        else
        {
            t.GetComponent<LegionRedRankItem>().SetData(item.m_BasePlayerPoolDT.m_CardId, item.m_BasePlayerPoolDT.m_iFrameId, item.m_BasePlayerPoolDT.m_iVip, item.m_Rank, item.m_BasePlayerPoolDT.m_szName,
                item.m_BasePlayerPoolDT.m_iLv, item.m_SyceeCount, item.m_BasePlayerPoolDT.m_szLegion, sexId);
        }


    }
    /// <summary>
    /// 点击玩家头像
    /// </summary>
    private void OnPlayerIconClick(GameObject go,object obj1,object obj2)
    {
        BasePlayerPoolDT tData = (BasePlayerPoolDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_OPEN, tData);
    }
    #region 服务器回调
    /// <summary>
    /// 查询信息成功
    /// </summary>
    private void OnInfoSucCall(object obj)
    {
        //显示UI
        ShowContent();
    }
    /// <summary>
    /// 查询失败
    /// </summary>
    private void OnInfoFailCall(object obj)
    {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không thể truy vấn thông tin biểu đồ!");
    }
    #endregion
    #region 按钮事件
    /// <summary>
    /// 点击黑色遮罩背景，关闭页面
    /// </summary>
    private void OnMaskClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionRedRankPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    private void OnBtnCloseClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionRedRankPage, UIMessageDef.UI_CLOSE);
    }
    #endregion
}
