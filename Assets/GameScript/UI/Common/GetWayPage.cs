using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 获取途径参数
/// </summary>
public class GetWayPageParam
{
    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="resourceType">资源类型</param>
    /// <param name="resourceId">资源id</param>
    /// <param name="m_UIBase">获取途径背景页面（可为空）</param>
    public GetWayPageParam(EM_ResourceType resourceType, int resourceId, ccUIBase m_UIBase)
    {
        this.resourceType = resourceType;
        this.resourceId = resourceId;
        this.m_UIBase = m_UIBase;
    }
    /// <summary>
    /// 资源类型
    /// </summary>
    public EM_ResourceType resourceType;
    /// <summary>
    /// 资源id
    /// </summary>
    public int resourceId;
    /// <summary>
    /// 获取途径背景页面（可为空）
    /// </summary>
    public ccUIBase m_UIBase;
}
/// <summary>
/// 获取途径页面item数据
/// </summary>
public class GetWayPageItem
{
    /// <summary>
    /// 页面ID（UINameConst）
    /// </summary>
    public string mGetWayID;
    /// <summary>
    /// 页面带参数
    /// </summary>
    public int mParam;
    /// <summary>
    /// 获取途径的标题名字
    /// </summary>
    public string mGetWayName;
    /// <summary>
    /// 次数提示(副本剩余次数)
    /// </summary>
    public string timesHint = "";
    /// <summary>
    /// 描述
    /// </summary>
    public string mDecri;
    /// <summary>
    /// 该获取方式是否已经开放
    /// </summary>
    public bool isOpen;
    /// <summary>
    /// 该获取方式对应的副本（副本专用）
    /// </summary>
    public DungeonPoolDT mDungeonPoolDT;
    /// <summary>
    /// 该获取方式对应副本下的关卡（副本专用）
    /// </summary>
    public int SelectDungeonTollgateIndex;
    /// <summary>
    /// 资源类型
    /// </summary>
    public int mResourceType;
    /// <summary>
    /// 资源id
    /// </summary>
    public int mResourceId;
}
public class GetWayTools
{
    private static GetWayPageParam getWayPageParam;//获取途径参数
    private static List<GetWayPageItem> listGetWayPageItem = new List<GetWayPageItem>();//获取途径列表
    static int needInitDungeonCount = 0;//副本需要初始化才知道其攻打次数
    static int currentInitDungeonCount = 0;//当前副本已初始化数量
    static ccUIBase thisUIBase;
    private static GameObject ObjItemParent;
    private static GameObject ObjItem;
    private static GameObject ObjScroll;
    /// <summary>
    /// 获取途径工具
    /// </summary>
    /// <param name="ItemParent">获取途径父物体（结构需参考GetWayPage）</param>
    /// <param name="item">获取途径item示例（结构需参考GetWayPage）</param>
    /// <param name="param">获取途径参数</param>
    /// <param name="uiBaseCurrent">当前界面</param>
    public static void ShowContent(GameObject ItemScoll, GameObject ItemParent, GameObject item, GetWayPageParam param, ccUIBase uiBaseCurrent)
    {
        getWayPageParam = (GetWayPageParam)param;
        ObjItemParent = ItemParent;
        ObjItem = item;
        thisUIBase = uiBaseCurrent;
        ObjScroll = ItemScoll;
        //2.显示获取途径的内容
        ShowContent();
    }
    /// <summary>
    /// 页面返回
    /// </summary>
    public static void UnHold()
    {
        ShowContent();
        //2.如果底图场景不为空，则显示底图
        if (getWayPageParam.m_UIBase != null)
        {
            ccUIHoldPool.GetInstance().f_UnHold();
        }
    }

    /// <summary>
    /// 显示或更新获取途径内容
    /// </summary>
    public static void ShowContent()
    {
        //1.清空列表内容
        listGetWayPageItem.Clear();
        //2.赋值列表内容（该资源的5个途径）
		MessageBox.ASSERT("Id: " + getWayPageParam.resourceId);
        GetWayDT getWayDT = glo_Main.GetInstance().m_SC_Pool.m_GetWaySC.f_GetSC(getWayPageParam.resourceId) as GetWayDT;
        if (getWayDT == null)
        {
            ResourceCommonDT tCommonDT = new ResourceCommonDT();
            tCommonDT.f_UpdateInfo((byte)getWayPageParam.resourceType, getWayPageParam.resourceId, 2);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(1653), tCommonDT.mName));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_CLOSE);
            return;
        }
        needInitDungeonCount = 0;
        currentInitDungeonCount = 0;
        //3.赋值
        InitListAndDicData(getWayDT);
        //4.显示内容
        if (needInitDungeonCount == 0)
        {
            SortListGetWay();
            GridUtil.f_SetGridView<GetWayPageItem>(ObjItemParent, ObjItem, listGetWayPageItem, OnUpdateItem);
            ObjItemParent.GetComponent<UIGrid>().Reposition();
            ObjScroll.GetComponent<UIScrollView>().ResetPosition();
        }
        else
        {
            for (int i = 0; i < listGetWayPageItem.Count; i++)
            {
                if (listGetWayPageItem[i].mGetWayID == UINameConst.DungeonChapterPageNew)
                {
                    Data_Pool.m_DungeonPool.f_ExecuteAfterInitDungeon(listGetWayPageItem[i].mDungeonPoolDT, f_Callback_DungeonTollgate);
                }
            }
        }
    }
    /// <summary>
    /// 排序副本item(未开启的放后面)
    /// </summary>
    private static void SortListGetWay()
    {
        for (int i = 0; i < listGetWayPageItem.Count; i++)
        {
            GetWayPageItem pageItemI = listGetWayPageItem[i] as GetWayPageItem;
            if (!pageItemI.isOpen)
            {
                for (int j = i + 1; j < listGetWayPageItem.Count; j++)
                {
                    GetWayPageItem pageItemJ = listGetWayPageItem[j] as GetWayPageItem;
                    if (pageItemJ.isOpen)
                    {
                        GetWayPageItem Temp = pageItemI;
                        listGetWayPageItem[i] = pageItemJ;
                        listGetWayPageItem[j] = Temp;
                        break;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 所有副本初始化完成后设置副本获取途径内容(章节数据更新也会更新)
    /// </summary>
    public static void UpdateDungeonTitle()
    {
        for (int i = 0; i < listGetWayPageItem.Count; i++)
        {
            if (listGetWayPageItem[i].mGetWayID == UINameConst.DungeonChapterPageNew)
            {
                GetWayPageItem pageItem = listGetWayPageItem[i];
                DungeonTollgatePoolDT tollgatePoolDT = pageItem.mDungeonPoolDT.mTollgateList[pageItem.SelectDungeonTollgateIndex];
                pageItem.mDungeonPoolDT.f_UpdateInitByServerState(true);
                int times = tollgatePoolDT.mTollgateTemplate.iCountLimit - tollgatePoolDT.mTimes;
                pageItem.timesHint = "  (" + times + "/" + string.Format(CommonTools.f_GetTransLanguage(1654), tollgatePoolDT.mTollgateTemplate.iCountLimit) + ")";
            }
        }
        SortListGetWay();
        GridUtil.f_SetGridView<GetWayPageItem>(ObjItemParent, ObjItem, listGetWayPageItem, OnUpdateItem);
        ObjItemParent.GetComponent<UIGrid>().Reposition();
        ObjScroll.GetComponent<UIScrollView>().ResetPosition();
    }
    /// <summary>
    /// 副本里的关卡item初始化完
    /// </summary>
    private static void f_Callback_DungeonTollgate(object data)
    {
        currentInitDungeonCount++;
        if (currentInitDungeonCount >= needInitDungeonCount)//所有副本都已初始化完
        {
            UpdateDungeonTitle();
        }
    }

    /// <summary>
    /// 设置副本的获取途径
    /// </summary>
    /// <param name="GetWayId">跳转页面id</param>
    /// <param name="GetWayParam">跳转页面参数</param>
    /// <param name="GetWayName">获取途径名称</param>
    /// <param name="resourceId">要获取的资源id</param>
    private static void GetDungeItem(string GetWayId, int GetWayParam, string GetWayName, int resourceId, string getWayDesc)
    {
        //不需要跳转到指定关卡
        if (GetWayParam != (int)EM_Fight_Enum.eFight_DungeonMain && GetWayParam != (int)EM_Fight_Enum.eFight_DungeonElite
            && GetWayParam != (int)EM_Fight_Enum.eFight_Legend)
        {
            GetWayPageItem pageItem = new GetWayPageItem();
            pageItem.mGetWayID = GetWayId;
            pageItem.mParam = GetWayParam;
            pageItem.mGetWayName = GetWayParam == (int)EM_Fight_Enum.eFight_DailyPve ? CommonTools.f_GetTransLanguage(1655) : CommonTools.f_GetTransLanguage(1656);
            pageItem.mDecri = GetWayParam == (int)EM_Fight_Enum.eFight_DailyPve ? CommonTools.f_GetTransLanguage(1657) : CommonTools.f_GetTransLanguage(1658);
            pageItem.isOpen = true;
            listGetWayPageItem.Add(pageItem);
            return;
        }

        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_DungeonChapterSC.f_GetAll();
        List<NBaseSCDT> listTargetData = listData.FindAll((NBaseSCDT item) =>
        {
            DungeonChapterDT dt = item as DungeonChapterDT;
            return null != dt && dt.iChapterType == GetWayParam;
        });
        for (int i = 0; i < listTargetData.Count; i++)
        {
            //获取章节的所有关卡id
            DungeonChapterDT dt = listTargetData[i] as DungeonChapterDT;
            string toDtStr = dt.szTollgateId;
            string[] toDtArray = toDtStr.Split(';');
            List<int> tollgateIdList = new List<int>();
            for (int a = 0; a < toDtArray.Length; a++)
            {
                int tollgateId = int.Parse(toDtArray[a]);
                if (tollgateId <= 0) continue;
                tollgateIdList.Add(tollgateId);
            }

            //先判断章节首通奖励
            int lastTollgateId = tollgateIdList[tollgateIdList.Count - 1];
            DungeonTollgateDT temp = glo_Main.GetInstance().m_SC_Pool.m_DungeonTollgateSC.f_GetSC(lastTollgateId) as DungeonTollgateDT;
            if (dt.szFirstAward.Contains(string.Format(";{0};", resourceId)))
            {
                GetWayPageItem pageItem = new GetWayPageItem();
                pageItem.mGetWayID = GetWayId;
                pageItem.mParam = GetWayParam;
                pageItem.mGetWayName = GetWayName;
                pageItem.mDecri = "[" + dt.szChapterName + "-" + temp.szTollgateName + "]" + getWayDesc;
                pageItem.isOpen = CheckIsOpen(GetWayParam, dt, tollgateIdList.Count, ref pageItem.mDungeonPoolDT);
                pageItem.SelectDungeonTollgateIndex = tollgateIdList.Count - 1;
                listGetWayPageItem.Add(pageItem);
            }

            //再判断关卡奖励
            for (int a = 0; a < tollgateIdList.Count; a++)
            {
                AddDungeItem(GetWayId, GetWayParam, GetWayName, resourceId, dt, tollgateIdList[a], a);
            }
        }
    }
    /// <summary>
    /// 增加副本item
    /// </summary>
    /// <param name="GetWayId">跳转页面id</param>
    /// <param name="GetWayParam">跳转页面参数</param>
    /// <param name="GetWayName">获取途径名称</param>
    /// <param name="resourceId">要获取的资源id</param>
    /// <param name="dt">副本dt</param>
    /// <param name="tolldataId">副本里的关卡id</param>
    /// <param name="tollgateIndex">该关卡在副本里的排序</param>
    private static void AddDungeItem(string GetWayId, int GetWayParam, string GetWayName, int resourceId, DungeonChapterDT dt, int tolldataId, int tollgateIndex)
    {
        if (tolldataId > 0)
        {
            DungeonTollgateDT temp = glo_Main.GetInstance().m_SC_Pool.m_DungeonTollgateSC.f_GetSC(tolldataId) as DungeonTollgateDT;
            List<AwardPoolDT> listAward = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(temp.iPond);
            for (int b = 0; b < listAward.Count; b++)
            {
                if (listAward[b].mTemplate.mResourceId == resourceId)
                {
                    needInitDungeonCount++;
                    GetWayPageItem pageItem = new GetWayPageItem();
                    pageItem.mGetWayID = GetWayId;
                    pageItem.mParam = GetWayParam;
                    pageItem.mGetWayName = GetWayName;
                    pageItem.mDecri = "[" + dt.szChapterName + "-" + temp.szTollgateName + "]" + CommonTools.f_GetTransLanguage(1659);
                    pageItem.isOpen = CheckIsOpen(GetWayParam, dt, tollgateIndex + 1, ref pageItem.mDungeonPoolDT);
                    pageItem.SelectDungeonTollgateIndex = tollgateIndex;
                    listGetWayPageItem.Add(pageItem);
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 检测是否开启
    /// </summary>
    /// <param name="GetWayParam"></param>
    /// <param name="dt"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private static bool CheckIsOpen(int GetWayParam, DungeonChapterDT dt, int index, ref DungeonPoolDT dungeonPoolDT)
    {
        List<BasePoolDT<long>> listDate = Data_Pool.m_DungeonPool.f_GetAllForData1(GetWayParam);
        for (int i = 0; i < listDate.Count; i++)
        {
            DungeonPoolDT poolDt = listDate[i] as DungeonPoolDT;
            if (poolDt.iId == dt.iId)
            {
                bool bLock = Data_Pool.m_DungeonPool.f_CheckChapterLockState(poolDt);
                dungeonPoolDT = poolDt;
                if (bLock == true)
                    return false;
                else
                {
                    return (poolDt.mTollgatePassNum + 1) >= index;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 设置单个item数据
    /// </summary>
    /// <param name="getWayGoPage">跳转页面id</param>
    /// <param name="goPageParam">跳转页面参数</param>
    /// <param name="getWayName">页面名称</param>
    /// <param name="getWayDesc">页面描述</param>
    private static void InitSingleItemData(string getWayGoPage, int goPageParam, string getWayName, string getWayDesc, int resourceType, int resourceId)
    {
        //如果是副本（遍历副本奖池表）如果为副本则会增加一些项目
        if (getWayGoPage == UINameConst.DungeonChapterPageNew)
        {
            GetDungeItem(getWayGoPage, goPageParam, getWayName, resourceId, getWayDesc);
        }
        else
        {
            GetWayPageItem pageItem = new GetWayPageItem();
            pageItem.mGetWayID = getWayGoPage;
            pageItem.mParam = goPageParam;
            pageItem.mGetWayName = getWayName;
            pageItem.mDecri = getWayDesc;
            pageItem.isOpen = true;
            pageItem.mResourceType = resourceType;
            pageItem.mResourceId = resourceId;
            int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            if (pageItem.mGetWayID == UINameConst.PatrolPage)
            {
                pageItem.mDecri = UITool.f_GetGoodName(EM_ResourceType.Card, resourceId) + getWayDesc;
                pageItem.isOpen = userLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.PatrolLevel);
            }
            else if (pageItem.mGetWayID == UINameConst.RunningManPage)
                pageItem.isOpen = userLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel);
            else if (pageItem.mGetWayID == UINameConst.GrabTreasurePage)
                pageItem.isOpen = userLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel);
            else if (pageItem.mGetWayID == UINameConst.RebelArmy)
                pageItem.isOpen = userLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel);
            else if (pageItem.mGetWayID == UINameConst.ArenaPageNew)
                pageItem.isOpen = userLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel);
            else if (pageItem.mGetWayID == UINameConst.ShopMutiCommonPage)
            {
                switch (pageItem.mParam)
                {
                    case (int)EM_ShopMutiType.Reputation:
                        pageItem.isOpen = userLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel);
                        break;
                    case (int)EM_ShopMutiType.Legion:
                        pageItem.isOpen = userLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.LegionLevel);
                        break;
                    case (int)EM_ShopMutiType.BattleFeatShop:
                        pageItem.isOpen = userLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel);
                        break;
                    case (int)EM_ShopMutiType.RunningMan:
                        pageItem.isOpen = userLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel);
                        break;
                    case (int)EM_ShopMutiType.CrossServerBattle:
                        pageItem.isOpen = userLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle);
                        break;
                    //TsuCode - ChaosBattle
                    case (int)EM_ShopMutiType.ChaosBattle:
                        pageItem.isOpen = userLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.ChaosBattle);
                        break;
                        //
                }
            }
            listGetWayPageItem.Add(pageItem);
        }
    }
    /// <summary>
    /// 初始化内容数据
    /// </summary>
    /// <param name="getWayDT"></param>
    private static void InitListAndDicData(GetWayDT getWayDT)
    {
        if (getWayDT.szGetWayGoPage1 != string.Empty)
        {
            InitSingleItemData(getWayDT.szGetWayGoPage1, getWayDT.iParam1, getWayDT.szGetWayName1, getWayDT.szGetWayDesc1, getWayDT.iResType, getWayDT.iId);
        }
        if (getWayDT.szGetWayGoPage2 != string.Empty)
        {
            InitSingleItemData(getWayDT.szGetWayGoPage2, getWayDT.iParam2, getWayDT.szGetWayName2, getWayDT.szGetWayDesc2, getWayDT.iResType, getWayDT.iId);
        }
        if (getWayDT.szGetWayGoPage3 != string.Empty)
        {
            InitSingleItemData(getWayDT.szGetWayGoPage3, getWayDT.iParam3, getWayDT.szGetWayName3, getWayDT.szGetWayDesc3, getWayDT.iResType, getWayDT.iId);
        }
        if (getWayDT.szGetWayGoPage4 != string.Empty)
        {
            InitSingleItemData(getWayDT.szGetWayGoPage4, getWayDT.iParam4, getWayDT.szGetWayName4, getWayDT.szGetWayDesc4, getWayDT.iResType, getWayDT.iId);
        }
        if (getWayDT.szGetWayGoPage5 != string.Empty)
        {
            InitSingleItemData(getWayDT.szGetWayGoPage5, getWayDT.iParam5, getWayDT.szGetWayName5, getWayDT.szGetWayDesc5, getWayDT.iResType, getWayDT.iId);
        }

    }
    /// <summary>
    /// item更新
    /// </summary>
    /// <param name="item">item物体</param>
    /// <param name="dt">获取途径item</param>
    private static void OnUpdateItem(GameObject item, GetWayPageItem pageItem)
    {
        item.transform.Find("Name").GetComponent<UILabel>().text = pageItem.mGetWayName + pageItem.timesHint;
        item.transform.Find("Descri").GetComponent<UILabel>().text = pageItem.mDecri;
        item.transform.Find("SprGoto").gameObject.SetActive(pageItem.isOpen && pageItem.mGetWayID != UINameConst.TurntablePage && pageItem.mGetWayID != UINameConst.LimitGodEquipActPage);
        item.transform.Find("LabelLock").gameObject.SetActive(!pageItem.isOpen);
        item.transform.Find("Icon").gameObject.GetComponent<UISprite>().spriteName = pageItem.mGetWayID;
        if (pageItem.mGetWayID == UINameConst.DungeonChapterPageNew && pageItem.mParam != 0)//副本图标特殊处理
        {
            item.transform.Find("Icon").gameObject.GetComponent<UISprite>().spriteName = pageItem.mGetWayID + pageItem.mParam;
        }
        item.transform.Find("Icon").gameObject.GetComponent<UISprite>().MakePixelPerfect();
        if (pageItem.isOpen)
            thisUIBase.f_RegClickEvent(item.transform.Find("SprGoto").gameObject, OnGotoClick, pageItem);
    }
    /// <summary>
    /// 点击前往
    /// </summary>
    private static void OnGotoClick(GameObject go, object obj1, object obj2)
    {
        GetWayPageItem pageItem = (GetWayPageItem)obj1;
        bool isTurntablePage = pageItem.mGetWayID == UINameConst.TurntablePage;
        if (getWayPageParam.m_UIBase != null)
        {
            if (!isTurntablePage)
            {
                //特殊处理轮盘，，它是弹窗，不能关闭上个界面，要不就没有背景
                ccUIHoldPool.GetInstance().f_Hold(getWayPageParam.m_UIBase);
                if (getWayPageParam.m_UIBase.name == UINameConst.UserInfoPage)
                {
                    //特殊处理主界面的玩家信息界面的缺物跳转，，，因为它是弹窗，，但是此时只关了弹窗没关主界面就会和商城界面重叠
                    ccUIBase uiMain = ccUIManage.GetInstance().f_GetUIHandler(UINameConst.MainMenu);
                    if (null != uiMain) ccUIHoldPool.GetInstance().f_Hold(uiMain);
                }
            }
        }


        bool isNeedChangeToggle = pageItem.mParam == (int)EM_Fight_Enum.eFight_DungeonMain || pageItem.mParam == (int)EM_Fight_Enum.eFight_DungeonElite
                || pageItem.mParam == (int)EM_Fight_Enum.eFight_Legend;
        if (pageItem.mGetWayID == UINameConst.DungeonChapterPageNew && isNeedChangeToggle)//此副本跳转要定义的到具体的关卡
        {
            if (pageItem.mParam == (int)EM_Fight_Enum.eFight_DungeonElite)
            {
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.DungeonEliteLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1934), UITool.f_GetSysOpenLevel(EM_NeedLevel.DungeonEliteLevel)));
                    if (getWayPageParam.m_UIBase != null)
                        ccUIHoldPool.GetInstance().f_UnHold();
                    return;
                }
            }
            else if (pageItem.mParam == (int)EM_Fight_Enum.eFight_Legend)
            {
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.LegendLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1935), UITool.f_GetSysOpenLevel(EM_NeedLevel.LegendLevel)));
                    if (getWayPageParam.m_UIBase != null)
                        ccUIHoldPool.GetInstance().f_UnHold();
                    return;
                }
            }
            DungeonTollgatePageParam param = new DungeonTollgatePageParam();
            param.mDungeonPoolDT = pageItem.mDungeonPoolDT;
            param.SelectIndex = pageItem.SelectDungeonTollgateIndex;
            ccUIHoldPool.GetInstance().f_Hold(thisUIBase);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonTollgatePageNew, UIMessageDef.UI_OPEN, param);
        }
        else
        {
            bool isChangePage = false;
            isChangePage = UITool.f_GotoPage(thisUIBase, pageItem.mGetWayID, pageItem.mParam, null, null, pageItem.mResourceType, pageItem.mResourceId);
            if (!isChangePage)
            {
                if (getWayPageParam.m_UIBase != null && !isTurntablePage)
                    ccUIHoldPool.GetInstance().f_UnHold();
                //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1660));
            }
        }
    }
}
/// <summary>
/// 获取方式界面
/// </summary>
public class GetWayPage : UIFramwork
{
    private static GetWayPageParam getWayPageParam;//获取途径参数
    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e">获取途径参数</param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (Data_Pool.m_GuidancePool.IsEnter)//新手引导不弹
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_CLOSE);

        getWayPageParam = (GetWayPageParam)e;
        GetWayTools.ShowContent(f_GetObject("ScrollView"), f_GetObject("ItemParent"), f_GetObject("Item"), getWayPageParam, this);
        //1.初始化标题数据（需要获取的物品信息）
        InitTitleUI();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_DUNGEON_CHAPTER_UPDATE, f_UpdateByChapterUpdate, this);
    }

    private void f_UpdateByChapterUpdate(object result)
    {
        GetWayTools.UpdateDungeonTitle();
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_DUNGEON_CHAPTER_UPDATE, f_UpdateByChapterUpdate, this);
    }

    /// <summary>
    /// UI UnHold
    /// </summary>
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        //1.更新UI
        InitTitleUI();
        GetWayTools.UnHold();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_DUNGEON_CHAPTER_UPDATE, f_UpdateByChapterUpdate, this);
    }

    /// <summary>
    /// 初始化标题信息
    /// </summary>
    private void InitTitleUI()
    {
        GameObject GoodTitle = f_GetObject("GoodTitle");
        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)getWayPageParam.resourceType, getWayPageParam.resourceId, UITool.f_GetGoodNum(getWayPageParam.resourceType, getWayPageParam.resourceId));
        UITool.f_SetIconSprite(GoodTitle.transform.Find("Icon").GetComponent<UI2DSprite>(), getWayPageParam.resourceType, getWayPageParam.resourceId);
        GoodTitle.transform.Find("Name").GetComponent<UILabel>().text = dt.mName;
        GoodTitle.transform.Find("Descri").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1661) + dt.mResourceNum;
        GoodTitle.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(dt.mImportant);
        f_RegClickEvent(GoodTitle.transform.Find("Icon").gameObject, OnIconClick);
    }
    /// <summary>
    /// 点击icon弹出详细信息
    /// </summary>
    private void OnIconClick(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)getWayPageParam.resourceType, getWayPageParam.resourceId, 1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, dt);
    }
    /// <summary>
    /// 页面关闭
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_DUNGEON_CHAPTER_UPDATE, f_UpdateByChapterUpdate, this);
        StaticValue.mGetWayToBattleParam.f_Empty();
    }
    /// <summary>
    /// 初始化注册事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("MaskClose", OnCloseBlackClick);
    }
    /// <summary>
    /// 点击界面黑色背景
    /// </summary>
    private void OnCloseBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_CLOSE);
    }
}
