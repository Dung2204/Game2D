using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 夺宝页面
/// </summary>
public class GrabTreasurePage : UIFramwork
{
    private List<TreasureDT> listNeedFixTreasure = new List<TreasureDT>();//需要合成的宝物列表
    private Dictionary<TreasureDT, GameObject> listObjNeedFixTreasure = new Dictionary<TreasureDT, GameObject>();//法宝列表id和其对应的物体
    public static TreasureDT curSelectTreasure = null;
    private static int curTreasureFlagClickID;//被点击的碎片id
    public static bool mbIsAutoGoods;
    private int mixTreasureCount = 1;
    private List<TreasureFragmentsDT> listCurSelectTreaFragment = new List<TreasureFragmentsDT>();
    private SocketCallbackDT MixSucCallback = new SocketCallbackDT();
    private SocketCallbackDT TreasureOneKey = new SocketCallbackDT();
    private GameObject _btnFurnace;
    private int OneSytheNum;
    /// <summary>
    /// 页面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        //断线重连如果切去其他界面，这时候不处理，服务器消息返回再打开界面就会和其他界面重叠
        if (!UITool.f_IsCanOpenChallengePage(UINameConst.GrabTreasurePage))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasurePage, UIMessageDef.UI_CLOSE);
            return;
        }

        Data_Pool.m_GuidancePool.OpenLevelPageUIName = UINameConst.GrabTreasurePage;
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        f_RegClickEvent("BtnSmelting", OnBtnSmeltingClick);
        f_RegClickEvent("BtnQuickGrab", OnBtnQuickGrabClick);
        f_RegClickEvent("BtnMix", OnBtnMixClick);
		f_RegClickEvent("Btn_Help", f_OnHelpBtnClick);
        f_RegClickEvent(_btnFurnace, OnFurnaceClick);//熔炼
        _btnFurnace.SetActive(GetIsShowFurnaceBtn());
        InitListNeedFixTreasure();
        InitScrollContent();
        OpenMoneyPage();
        if(e != null && e is TreasureFragmentPoolDT)
        {
            curSelectTreasure = glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(((TreasureFragmentPoolDT)e).m_TreasureFragmentsDT.iTreasureId) as TreasureDT;
            OnTreasureIconClick(null, curSelectTreasure, null);
            int index = 0;
            for(int i = 0; i < listNeedFixTreasure.Count; i++)
            {
                if(listNeedFixTreasure[i].iId == curSelectTreasure.iId)
                {
                    index = i;
                    break;
                }
            }
            f_GetObject("TreaProgressBar").GetComponent<UIProgressBar>().value = index * 1.0f / (listNeedFixTreasure.Count - 1);
        }
        else if(e != null && e is int)//法宝id
        {
            int index = -1;
            for(int i = 0; i < listNeedFixTreasure.Count; i++)
            {
                if(listNeedFixTreasure[i].iId == (int)e)
                {
                    index = i;
                    break;
                }
            }
            if(index == -1)
            {
                OnTreasureIconClick(null, listNeedFixTreasure[0], null);
                f_GetObject("TreaProgressBar").GetComponent<UIProgressBar>().value = 0;
            }
            else
            {
                curSelectTreasure = listNeedFixTreasure[index];
                OnTreasureIconClick(null, curSelectTreasure, null);
                f_GetObject("TreaProgressBar").GetComponent<UIProgressBar>().value = index * 1.0f / (listNeedFixTreasure.Count - 1);
            }
        }
        else
        {
            OnTreasureIconClick(null, listNeedFixTreasure[0], null);
            f_GetObject("TreaProgressBar").GetComponent<UIProgressBar>().value = 0;
        }
        f_LoadTexture();
        Data_Pool.m_TransmigrationTreasurePool.f_CheckTransTreasureRedPoint();

    }

    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TransmigrationTreasure, _btnFurnace, ReddotCallback_Show_Btn_Furnace);
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.TransmigrationTreasure, _btnFurnace);
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TransmigrationTreasure);
    }

    private void ReddotCallback_Show_Btn_Furnace(object Obj)
    {
        int iNum = (int)Obj;
        _btnFurnace.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }

    private string strTexBgRoot = "UI/TextureRemove/Challenge/Texture_GrabTreasure";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if(TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;
        }
    }
    /// <summary>
    /// 打开货币页面
    /// </summary>
    private void OpenMoneyPage()
    {
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Vigor);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }
    /// <summary>
    /// 初始化左侧法宝栏(调用前先初始化法宝id列表)
    /// </summary>
    private void InitScrollContent()
    {
        listObjNeedFixTreasure.Clear();
        GridUtil.f_SetGridView<TreasureDT>(f_GetObject("ItemParent"), f_GetObject("TreasureItem"), listNeedFixTreasure, OnShowItemData);

        f_GetObject("MenuScrollView").GetComponent<UIScrollView>().ResetPosition();

        if(curSelectTreasure == null)
        {
            OnTreasureIconClick(null, listNeedFixTreasure[0], null);
            f_GetObject("TreaProgressBar").GetComponent<UIProgressBar>().value = 0;
        }
        else
        {
            if(listObjNeedFixTreasure.ContainsKey(curSelectTreasure))
            {
                OnTreasureIconClick(null, curSelectTreasure, null);
                f_GetObject("TreaProgressBar").GetComponent<UIProgressBar>().value = listNeedFixTreasure.IndexOf(curSelectTreasure) * 1.0f / (listNeedFixTreasure.Count - 1);
                if(Data_Pool.m_GrabTreasurePool.m_IsNeedChangeToRobotPage)//切换至机器人信息页面
                {
                    Data_Pool.m_GrabTreasurePool.m_IsNeedChangeToRobotPage = false;
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectOpponentPage, UIMessageDef.UI_OPEN, curTreasureFlagClickID);
                }
            }
            else
            {
                OnTreasureIconClick(null, listNeedFixTreasure[0], null);
                f_GetObject("TreaProgressBar").GetComponent<UIProgressBar>().value = 0;
            }
        }
    }
    /// <summary>
    /// 显示法宝栏数据
    /// </summary>
    /// <param name="go">物体</param>
    /// <param name="treasureID">法宝id</param>
    private void OnShowItemData(GameObject go, TreasureDT treasureDT)
    {
        listObjNeedFixTreasure.Add(treasureDT, go);
        //go.transform.name
        if(treasureDT.iId == (int)EM_Treasure.HuFu)
        {
            go.transform.name = "HuFu";
        }
        go.transform.Find("SprHeadIcon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(treasureDT.iIcon);
        string treasureName = treasureDT.szName;
        go.transform.Find("SprBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(treasureDT.iImportant, ref treasureName);
        go.transform.Find("SelectEffect").gameObject.SetActive(treasureDT == curSelectTreasure);
        f_RegClickEvent(go, OnTreasureIconClick, treasureDT);
        //红点提示检查
        int FixedCount = Data_Pool.m_TreasurePool.f_CheckTreasureCanMixMaxCount(treasureDT.iId);
        go.transform.Find("Reddot").gameObject.SetActive(FixedCount > 0 ? true : false);
    }
    /// <summary>
    /// 点击左侧法宝栏菜单
    /// </summary>
    private void OnTreasureIconClick(GameObject go, object obj1, object obj2)
    {
        TreasureDT treasureDT = (TreasureDT)obj1;
        curSelectTreasure = treasureDT;
        mixTreasureCount = Data_Pool.m_TreasurePool.f_CheckTreasureCanMixMaxCount(curSelectTreasure.iId);
        //f_GetObject("BtnMix").GetComponentInChildren<UILabel>().text = mixTreasureCount > 1 ? CommonTools.f_GetTransLanguage(768) : CommonTools.f_GetTransLanguage(769);
        //f_GetObject("BtnOnKey").SetActive(mixTreasureCount < 1);

        UITool.f_SetSpriteGray(f_GetObject("BtnOnKey"), mixTreasureCount > 0);
        //f_GetObject("AutoCommVigor").SetActive(mixTreasureCount < 1);


        f_RegClickEvent("BtnOnKey", OnBtnOpenOnKeyGrabTreasure, treasureDT);

        Dictionary<TreasureDT, GameObject>.Enumerator enumerator = listObjNeedFixTreasure.GetEnumerator();
        while(enumerator.MoveNext())
        {
            TreasureDT key = enumerator.Current.Key;
            enumerator.Current.Value.transform.Find("SelectEffect").gameObject.SetActive(key == treasureDT);
        }
        ShowContent(treasureDT, mixTreasureCount);
    }
    /// <summary>
    /// 显示item
    /// </summary>
    /// <param name="dt"></param>
    private void ShowContent(TreasureDT dt, int mixTreasureCount)
    {
        f_GetObject("LabelTreasureName").GetComponent<UILabel>().text = UITool.f_GetImporentForName(dt.iImportant, UITool.f_ReplaceName(dt.szName," ", "\n"));
        listCurSelectTreaFragment.Clear();
        List<NBaseSCDT> listTreasureFrag = glo_Main.GetInstance().m_SC_Pool.m_TreasureFragmentsSC.f_GetAll();
        for(int i = 0; i < listTreasureFrag.Count; i++)
        {
            TreasureFragmentsDT fragDT = listTreasureFrag[i] as TreasureFragmentsDT;
            if(fragDT.iTreasureId == dt.iId)
                listCurSelectTreaFragment.Add(fragDT);
        }
        f_GetObject("ItemCircle3").SetActive(3 == listCurSelectTreaFragment.Count);
        f_GetObject("ItemCircle4").SetActive(4 == listCurSelectTreaFragment.Count);
        f_GetObject("ItemCircle5").SetActive(5 == listCurSelectTreaFragment.Count);
        f_GetObject("ItemCircle6").SetActive(6 == listCurSelectTreaFragment.Count);
        GameObject itemCircle = f_GetObject("ItemCircle" + listCurSelectTreaFragment.Count);
        itemCircle.transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(dt.iIcon);
        string treaName = dt.szName;
        itemCircle.transform.Find("Icon_Border").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(dt.iImportant, ref treaName);

        if(itemCircle.transform.Find("effect") != null)
            GameObject.DestroyImmediate(itemCircle.transform.Find("effect").gameObject);
        if(mixTreasureCount > 0)//法宝可合成加特效
            UITool.f_CreateEquipEffect(itemCircle.transform, "effect", (EM_Important)dt.iImportant, new Vector3(0, 0, 0), new Vector3(160, 160, 160));

        f_RegClickEvent(itemCircle.transform.Find("Icon").gameObject, OnItemTreasureIconClick, dt);
        for(int i = 0; i < listCurSelectTreaFragment.Count; i++)
        {
            int count = Data_Pool.m_TreasureFragmentPool.f_GetHaveNumByTemplate(listCurSelectTreaFragment[i].iId);
            itemCircle.transform.Find((i + 1).ToString()).GetComponentInChildren<UILabel>().text = count.ToString();
            itemCircle.transform.Find((i + 1).ToString()).transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(listCurSelectTreaFragment[i].iIcon);
            itemCircle.transform.Find((i + 1).ToString()).transform.Find("Icon").GetComponent<UI2DSprite>().color = count <= 0 ? new Color(0f, 0.2f, 0.2f) : Color.white;
            string name = listCurSelectTreaFragment[i].szName;
            itemCircle.transform.Find((i + 1).ToString()).transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(listCurSelectTreaFragment[i].iImportant, ref name);
            itemCircle.transform.Find((i + 1).ToString()).transform.Find("IconBorder").GetComponent<UISprite>().color = count <= 0 ? new Color(0f, 0.2f, 0.2f) : Color.white;
            f_RegClickEvent(itemCircle.transform.Find((i + 1).ToString()).gameObject, OnItemCircleClick, listCurSelectTreaFragment[i], count);
        }
    }
    /// <summary>
    /// 点击圆圈中间法宝图标
    /// </summary>
    private void OnItemTreasureIconClick(GameObject go, object obj1, object obj2)
    {
        TreasureDT dt = (TreasureDT)obj1;
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)EM_ResourceType.Treasure, dt.iId, 1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 点击碎片按钮
    /// </summary>
    private void OnItemCircleClick(GameObject go, object obj1, object obj2)
    {
        TreasureFragmentsDT fragDT = (TreasureFragmentsDT)obj1;
        int num = (int)obj2;
        if(num <= 0)
        {
            curTreasureFlagClickID = fragDT.iId;
            ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectOpponentPage, UIMessageDef.UI_OPEN, fragDT.iId);
        }
        else
        {
            ResourceCommonDT commonData = new ResourceCommonDT();
            commonData.f_UpdateInfo((byte)EM_ResourceType.TreasureFragment, fragDT.iId, 1);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
            Debug.Log(CommonTools.f_GetTransLanguage(770) + fragDT.szName);
        }
    }
    /// <summary>
    /// 初始化需合成的法宝列表id
    /// </summary>
    private void InitListNeedFixTreasure()
    {
        listNeedFixTreasure.Clear();
        //1.加入必须使用蓝色法宝列表
        listNeedFixTreasure.Add(glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(5004) as TreasureDT);
        listNeedFixTreasure.Add(glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(5005) as TreasureDT);
        listNeedFixTreasure.Add(glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(5006) as TreasureDT);
        listNeedFixTreasure.Add(glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(5007) as TreasureDT);
        //2.检查玩家拥有的法宝碎片
        List<BasePoolDT<long>> listData = Data_Pool.m_TreasureFragmentPool.f_GetAll();
        for(int i = 0; i < listData.Count; i++)
        {
            TreasureFragmentPoolDT poolDT = listData[i] as TreasureFragmentPoolDT;
            int fixID = poolDT.m_TreasureFragmentsDT.iTreasureId;
            TreasureDT dt = glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(fixID) as TreasureDT;
            if(!listNeedFixTreasure.Contains(dt))
                listNeedFixTreasure.Add(dt);
        }
        //排序
        for(int i = 0; i < listNeedFixTreasure.Count - 1; i++)
        {
            TreasureDT TreasureI = listNeedFixTreasure[i];
            int mixcountI = Data_Pool.m_TreasurePool.f_CheckTreasureCanMixMaxCount(TreasureI.iId);
            for(int j = i + 1; j < listNeedFixTreasure.Count; j++)
            {
                TreasureDT TreasureJ = listNeedFixTreasure[j];
                int mixcountJ = Data_Pool.m_TreasurePool.f_CheckTreasureCanMixMaxCount(TreasureJ.iId);
                if(mixcountI > 0)
                    break;
                else if(mixcountJ > 0)
                {
                    TreasureDT temp = TreasureJ;
                    listNeedFixTreasure[j] = listNeedFixTreasure[i];
                    listNeedFixTreasure[i] = temp;
                    TreasureI = listNeedFixTreasure[i];
                    break;
                }
                if(TreasureJ.iImportant > TreasureI.iImportant)
                {
                    TreasureDT temp = TreasureJ;
                    listNeedFixTreasure[j] = listNeedFixTreasure[i];
                    listNeedFixTreasure[i] = temp;
                    TreasureI = listNeedFixTreasure[i];
                }
            }
        }
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        _btnFurnace = f_GetObject("BtnFurnace");
        MixSucCallback.m_ccCallbackSuc = OnMixSucCallback;
        MixSucCallback.m_ccCallbackFail = OnMixFailCallback;
        TreasureOneKey.m_ccCallbackSuc = TreasureOneKeySuc;
        TreasureOneKey.m_ccCallbackFail = TreasureOneKeyFail;
    }
    /// <summary>
    /// 页面关闭
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }
    #region 服务器回调
    private void OnMixSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIHoldPool.GetInstance().f_Hold(this);
        EquipSythesis tEquipSythesis = new EquipSythesis(curSelectTreasure.iId, OneSytheNum, EquipSythesis.ResonureType.Treasure);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainEquipShowPage, UIMessageDef.UI_OPEN, tEquipSythesis);
        if(curSelectTreasure != null && !Data_Pool.m_TreasurePool.f_CheckHasTreasureFragment(curSelectTreasure.iId))
        {
            curSelectTreasure = null;
            f_GetObject("TreaProgressBar").GetComponent<UIProgressBar>().value = 0;
            InitListNeedFixTreasure();
        }
        InitScrollContent();
    }
    private void OnMixFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(771));
    }

    private void TreasureOneKeySuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //if (Data_Pool.m_TreasurePool.f_CheckTreasureCanMixMaxCount(curSelectTreasure.iId) >= 1)
        //{

        return;
        //}

        //if (!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Vigor , 2 , false))
        //{
        //    if (Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(201) > 0)
        //    {
        //        SocketCallbackDT tmp = new SocketCallbackDT();
        //        tmp.m_ccCallbackFail = TreasureOneKeyGoodsFail;
        //        tmp.m_ccCallbackSuc = TreasureOneKeyGoodsSuc;
        //        Data_Pool.m_BaseGoodsPool.f_Use(Data_Pool.m_BaseGoodsPool.f_GetForData5(201).iId , 1 , 0 , tmp);
        //        return;
        //    }
        //    else
        //        return;
        //}
        //_OnKeySuc(obj);
    }


    private void TreasureOneKeyGoodsSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        _OnKeySuc(obj);
    }
    private void TreasureOneKeyGoodsFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(772) + obj.ToString());
    }
    private void TreasureOneKeyFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(773) + obj.ToString());
    }

    #endregion
    #region 按钮事件
    /// <summary>
    /// 点击返回按钮事件
    /// </summary>
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasurePage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    /// <summary>
    /// 点击熔炼按钮
    /// </summary>
    private void OnBtnSmeltingClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(774));
    }
    /// <summary>
    /// 点击一键夺宝
    /// </summary>
    private void OnBtnQuickGrabClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(775));
    }
    /// <summary>
    /// 点击合成按钮事件
    /// </summary>
    private void OnBtnMixClick(GameObject go, object obj1, object obj2)
    {
        OneSytheNum = 0;
        if (mixTreasureCount <= 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelCenterTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(776));
        }
        else if(mixTreasureCount == 1)
        {
            Data_Pool.m_GrabTreasurePool.f_TreasureSynthes(curSelectTreasure.iId, mixTreasureCount, MixSucCallback);
            UITool.f_OpenOrCloseWaitTip(true);
        }
        else
        {
            MutiOperateParam param = new MutiOperateParam();
            param.canUserTimes = mixTreasureCount;
            param.iId = curSelectTreasure.iId;
            param.onConfirmOperateCallback = ConfirmOperateCallback;
            param.resourceType = EM_ResourceType.Treasure;
            param.resourceID = curSelectTreasure.iId;
            param.resourceCount = 1;
            param.title = "wj_hc_font_hc";
            param.userHint = string.Format(CommonTools.f_GetTransLanguage(778),mixTreasureCount);//"本次最多可合成" + mixTreasureCount + "个";
            ccUIManage.GetInstance().f_SendMsg(UINameConst.MutiOperatePage, UIMessageDef.UI_OPEN, param);
        }
    }

    //熔炼
    private void OnFurnaceClick(GameObject go, object obj1, object obj2)
    {
        if(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.TransmigrationTreasure))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, UITool.f_GetSysOpenLevel(EM_NeedLevel.TransmigrationTreasure) + CommonTools.f_GetTransLanguage(779));
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TransmigrationTreasurePage, UIMessageDef.UI_OPEN);
    }

    //判断熔炼按钮是否显示
    private bool GetIsShowFurnaceBtn()
    {
        if(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= UITool.f_GetSysOpenLevel(EM_NeedLevel.TransmigrationTreasure))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 确认操作回调
    /// </summary>
    /// <param name="iId"></param>
    /// <param name="type"></param>
    /// <param name="resourceId"></param>
    /// <param name="resourceCount"></param>
    /// <param name="UseCount"></param>
    public void ConfirmOperateCallback(long iId, EM_ResourceType type, int resourceId, int resourceCount, int UseCount)
    {
        OneSytheNum = UseCount;
        Data_Pool.m_GrabTreasurePool.f_TreasureSynthes(resourceId, UseCount, MixSucCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }


    /// <summary>
    /// 打开一键夺宝界面（需要检测VIP等级和人物等级）
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    public void OnBtnOpenOnKeyGrabTreasure(GameObject go, object obj1, object obj2)
    {
        int OpenLv = Data_Pool.m_GrabTreasurePool.m_OnKeyGrabTreasureOpenLv;

        int VipLvNow = Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_LootTreasureOnkey);
        if(Data_Pool.m_CardPool.f_GetRoleLevel() >= OpenLv ||
           VipLvNow != 0)
        {
            _OnKeySuc(null);
            //PopupMenuParams tmpPopMenuParams = new PopupMenuParams("提示", "当精力不足时是否自动消耗精力丹，勾选复选框消耗背包精力丹，不勾选只消耗当前精力",
            //                                                      "确定", _OnKeySuc, "取消", null, obj1);

            //ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tmpPopMenuParams);

            return;
        }
        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(780), OpenLv, Data_Pool.m_RechargePool.f_GetVipLvLimit(EM_VipPrivilege.eVip_LootTreasureOnkey)));

    }
	
	private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 17);
    }

    private void _OnKeySuc(object obj)
    {
        mbIsAutoGoods = f_GetObject("AutoCommVigor").GetComponent<UIToggle>().value;
        if(Data_Pool.m_TreasurePool.f_CheckTreasureCanMixMaxCount(curSelectTreasure.iId) >= 1)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(781));
            return;
        }


        if(!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Vigor, 2, false, true, this))
        {

            if(!mbIsAutoGoods)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(782));
                GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, GameParamConst.VigorGoodId, this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                return;
            }
            else
            {
                if(Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(201) <= 0)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(783));
                    return;
                }
            }
        }



        ccUIManage.GetInstance().f_SendMsg(UINameConst.OneKeyGrabTreasurePage, UIMessageDef.UI_OPEN, curSelectTreasure);
        ////TreasureDT tmpTreasureDT = (TreasureDT)obj;
        //UITool.f_OpenOrCloseWaitTip(true);
        //Data_Pool.m_GrabTreasurePool.m_GrabTreasureOneKeyResult.Clear();
        //Data_Pool.m_GrabTreasurePool.f_GrabTreasureOneKey(curSelectTreasure.iId, TreasureOneKey);
    }
    #endregion



    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            OnBtnMixClick(null, null, null);
            OnBtnOpenOnKeyGrabTreasure(null, null, null);
        }
    }
}
