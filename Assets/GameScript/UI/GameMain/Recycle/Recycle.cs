using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using Spine.Unity;

public class Recycle : UIFramwork
{
    EM_RecyleOrRebirth tType;
    EM_Magic tMagic;


    GameObject Card_Item;
    GameObject Card_ItemParent;
    GameObject Equip_Item;
    GameObject Equip_ItemParent;
    GameObject Treasure_Item;
    GameObject Treasure_ItemParent;
    GameObject Return_Item;
    GameObject Return_ItemParent;
    GameObject GodEquip_Item;
    GameObject GodEquip_ItemParent;
    GameObject[] RecyleParent = new GameObject[5];

    GameObject Money;
	//My Code
	GameParamDT AssetOpen;
	//


    BasePoolDT<long>[] RecycleEquipOrCard;


    private UIWrapComponent Card_WrapComponet;
    private UIWrapComponent Equip_WrapComponet;
    private UIWrapComponent Treasure_WrapComponet;
    private UIWrapComponent GodEquip_WrapComponet;

    List<BasePoolDT<long>> _CardList; //显示的卡牌
    BasePoolDT<long>[] _CardListArr;
    List<BasePoolDT<long>> _EquipList; //显示的装备
    BasePoolDT<long>[] _EquipListArr;
    List<BasePoolDT<long>> _TreasureList;  //显示的法宝
    BasePoolDT<long>[] _TreasureListArr;
    List<BasePoolDT<long>> _GodEquipList; //
    BasePoolDT<long>[] _GodEquipListArr;

    List<BasePoolDT<long>> _tCard = new List<BasePoolDT<long>>();   //选择的卡牌
    List<BasePoolDT<long>> _tEquip = new List<BasePoolDT<long>>();  //选择的装备
    List<BasePoolDT<long>> _Treasure = new List<BasePoolDT<long>>();//选择的法宝 
    List<BasePoolDT<long>> _tGodEquip = new List<BasePoolDT<long>>();  //

    long Gold;    //所需银币
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        tType = EM_RecyleOrRebirth.CardRecyle;
        Card_Item = f_GetObject("SeleCardItem");
        Card_ItemParent = f_GetObject("SeleCardItemParent");
        Equip_Item = f_GetObject("SeleEquipItem");
        Equip_ItemParent = f_GetObject("SeleEquipItemParent");
        Treasure_Item = f_GetObject("SeleTreasureItem");
        Treasure_ItemParent = f_GetObject("SeleTreasureItemParent");
        Return_Item = f_GetObject("ReturnItem");
        Return_ItemParent = f_GetObject("ReturnParent");
        GodEquip_Item = f_GetObject("SeleGodEquipItem");
        GodEquip_ItemParent = f_GetObject("SeleGodEquipItemParent");
        f_GetObject("Recycle").SetActive(true);
        f_GetObject("AutoAddBtn").SetActive(true);
        f_GetObject("RecycleSucBtn").SetActive(true);
        Invoke("Open", 0.1f);
        Return_Item.SetActive(false);
		//My Code
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		//

        ListInit();
        InitRecyclePos();
        OpenMoneyPanel();
        UpdateReddotUI();
        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/MainMenu/Tex_RecycleBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if(TexBg.mainTexture == null)
        {
			//My Code
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
			// if(AssetOpen.iParam1 == 1)
			// {
				// tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_RecycleBg");
			// }
			//
            TexBg.mainTexture = tTexture2D;
        }
    }
    //#region 红点提示
    //protected override void InitRaddot()
    //{
    //    base.InitRaddot();

    //    Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.CardRecycle, f_GetObject("CardRecycleBtn"), ReddotCallback_Show_CardRecycle);
    //    Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.EquipRecycle, f_GetObject("EquipRecycleBtn"), ReddotCallback_Show_EquipRecycle);
    //    UpdateReddotUI();
    //}
    //protected override void UpdateReddotUI()
    //{
    //    base.UpdateReddotUI();
    //    Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.CardRecycle);
    //    Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.EquipRecycle);
    //}
    //protected override void On_Destory()
    //{
    //    base.On_Destory();
    //    Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.CardRecycle, f_GetObject("CardRecycleBtn"));
    //    Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.EquipRecycle, f_GetObject("EquipRecycleBtn"));
    //}
    //private void ReddotCallback_Show_CardRecycle(object Obj)
    //{
    //    int iNum = (int)Obj;
    //    GameObject CardRecycleBtn = f_GetObject("CardRecycleBtn");
    //    UITool.f_UpdateReddot(CardRecycleBtn, iNum, new Vector3(95, 15, 0), 2001);
    //}
    //private void ReddotCallback_Show_EquipRecycle(object Obj)
    //{
    //    int iNum = (int)Obj;
    //    GameObject EquipRecycleBtn = f_GetObject("EquipRecycleBtn");
    //    UITool.f_UpdateReddot(EquipRecycleBtn, iNum, new Vector3(95, 15, 0), 2001);
    //}
    //#endregion
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        DestroyState();
        tType = EM_RecyleOrRebirth.CardRecyle;
        tMagic = EM_Magic.TopR;
        RecycleEquipOrCard = new BasePoolDT<long>[6];

        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        OpenMoneyPanel();
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Back", UI_Close);
        f_RegClickEvent("RecycleSucBtn", RecycleOrRebith);
        f_RegClickEvent("AutoAddBtn", UI_AutoAdd);
        f_RegClickEvent("LeftMagic", UI_OpenSele, EM_Magic.Left);
        f_RegClickEvent("RightMagic", UI_OpenSele, EM_Magic.Right);
        f_RegClickEvent("TopLeftMagic", UI_OpenSele, EM_Magic.TopL);
        f_RegClickEvent("TopRightMagic", UI_OpenSele, EM_Magic.TopR);
        f_RegClickEvent("BottomMagic", UI_OpenSele, EM_Magic.Bottom);
        f_RegClickEvent("CardAlphe", UI_CloseSele);
        f_RegClickEvent("SeleEquip_Ahple", UI_CloseSele);
        f_RegClickEvent("GodEquipAlphe", UI_CloseSele);
        f_RegClickEvent("TreasureAlphe", UI_CloseSele);
        f_RegClickEvent("CardRecycleBtn", UI_CardRecycleBtn);
        f_RegClickEvent("CardRebirthBtn", UI_CardRebirthBtn);
        f_RegClickEvent("EquipRecycleBtn", UI_EquipRecycleBtn);
        f_RegClickEvent("EquipRebirthBtn", UI_EquipRebirthBtn);
        f_RegClickEvent("RebirthBtn", UI_SeleEquipRebith);
        f_RegClickEvent("RebirthSucBtn", RecycleOrRebith);
        f_RegClickEvent("TreasureRebirthBtn", UI_TreasureRebirthBtn);
        f_RegClickEvent("GodEquipRebirthBtn", UI_GodEquipRebirthBtn);
        f_RegClickEvent("Preview_Close", UI_CloseRecycle);
        f_RegClickEvent("Preview_Suc", RecycleOrRebith);
        f_RegClickEvent("LeftClose", UI_Discharge, EM_Magic.Left);
        f_RegClickEvent("RightClose", UI_Discharge, EM_Magic.Right);
        f_RegClickEvent("TopLeftClose", UI_Discharge, EM_Magic.TopL);
        f_RegClickEvent("TopRightClose", UI_Discharge, EM_Magic.TopR);
        f_RegClickEvent("BottomClose", UI_Discharge, EM_Magic.Bottom);

        f_RegClickEvent("SelectCard_SucBtn", UI_SeleCardSuc, EM_RecyleOrRebirth.CardRecyle, tmpBox);
        f_RegClickEvent("SelectCard_CloseBtn", UI_SeleCardClose, EM_RecyleOrRebirth.CardRecyle);
        f_RegClickEvent("SelectEquip_SucBtn", UI_SeleCardSuc, EM_RecyleOrRebirth.EquipRecyle, tmpBox);
        f_RegClickEvent("SelectEquip_CloseBtn", UI_SeleCardClose, EM_RecyleOrRebirth.EquipRecyle);

        f_RegClickEvent("Btn_RMShop", UI_OpenShopMuti);
        f_RegClickEvent("Btn_CardShop", UI_OpenShopCommon);
		f_RegClickEvent("Btn_Help", f_OnHelpBtnClick);
    }
    #region 事件

    void Open()
    {
        f_GetObject("CardRecycleBtn").GetComponent<UIToggle>().value = true;
        if (recycleSpine)
            recycleSpine.SetActive(true);
        else
            UITool.f_CreateMagicById((int)EM_MagicId.eRecycle, ref recycleSpine, f_GetObject("spineParent").transform, 6,
                "huishou_loop", null, true, 70);
    }

    private void UI_OpenShopMuti(GameObject go, object obj1, object obj2)
    {
        if(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(352), UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel)));
            return;
        }
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.RunningMan);
    }
    private void UI_OpenShopCommon(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopCommonPage, UIMessageDef.UI_OPEN, EM_ShopType.Card);
    }

    /// <summary>
    /// 关闭当前界面
    /// </summary>
    void UI_Close(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.Recycle, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold(this);
    }
    /// <summary>
    /// 关闭分解预览
    /// </summary>
    void UI_CloseRecycle(GameObject go, object obj1, object obj2)
    {
        f_GetObject("Recycle_Preview").SetActive(false);
        Gold = 0;
        RemoReturn();
    }
    /// <summary>
    /// 自动添加
    /// </summary>
    void UI_AutoAdd(GameObject go, object obj1, object obj2)
    {
        _tEquip.Clear();
        _tCard.Clear();
        int Isimporent = 0;
        switch(tType)
        {
            case EM_RecyleOrRebirth.EquipRecyle:
                for(int i = 0; i < _EquipList.Count; i++)
                {
                    if((_EquipList[i] as EquipPoolDT).m_EquipDT.iColour < (int)EM_Important.Purple && Isimporent < 5 &&
                        !GetIsEquip(_EquipList[i].iId))
                    {
                        Isimporent++;
                        _tEquip.Add(_EquipList[i]);
                    }
                }
                UI_CreareModleForList(_tEquip);
                break;
            case EM_RecyleOrRebirth.CardRecyle:
                for(int i = 0; i < _CardList.Count; i++)
                {
                    //品质低于紫色的不添加  或者主角不添加
                    if((_CardList[i] as CardPoolDT).m_CardDT.iImportant < (int)EM_Important.Oragen &&
                        Isimporent < 5 &&
                       (_CardList[i] as CardPoolDT).m_CardDT.iCardType != (int)EM_CardType.RoleCard &&
                        !Data_Pool.m_TeamPool.dicReinforceCardId.ContainsValue((_CardList[i] as CardPoolDT)))
                    {
                        Isimporent++;
                        _tCard.Add(_CardList[i]);
                    }
                }
                UI_CreareModleForList(_tCard);
                break;
        }

        if (Isimporent == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(353));
            DestroyState();
        }
    }
    /// <summary>
    /// 关闭选择界面
    /// </summary>
    void UI_CloseSele(GameObject go, object obj1, object obj2)
    {
        Gold = 0;
        CloseSele();
    }
    /// <summary>
    /// 打开选择界面
    /// </summary>
    void UI_OpenSele(GameObject go, object obj1, object obj2)
    {
        DestroyState();
        tmpBox.Clear();
        tMagic = (EM_Magic)obj1;
        updataSele();
    }
    /// <summary>
    /// 卸下当前位置的
    /// </summary>
    void UI_Discharge(GameObject go, object obj1, object obj2)
    {
        DestroyState();
        switch(tType)
        {
            case EM_RecyleOrRebirth.None:
                break;
            case EM_RecyleOrRebirth.EquipRecyle:
            case EM_RecyleOrRebirth.EquipRebirth:
                EquipPoolDT tEquip = RecycleEquipOrCard[(int)(EM_Magic)obj1] as EquipPoolDT;
                _tEquip.Remove(tEquip);
                UI_CreareModleForList(_tEquip);
                break;
            case EM_RecyleOrRebirth.CardRecyle:
            case EM_RecyleOrRebirth.CardRebirth:
                CardPoolDT tCard = RecycleEquipOrCard[(int)(EM_Magic)obj1] as CardPoolDT;
                _tCard.Remove(tCard);
                UI_CreareModleForList(_tCard);
                break;
            case EM_RecyleOrRebirth.TreasureRebirth:
                break;
        }
    }

    private GameObject recycleSpine = null;
    /// <summary>
    /// 卡牌回收
    /// </summary>
    void UI_CardRecycleBtn(GameObject go, object obj1, object obj2)
    {
        tType = EM_RecyleOrRebirth.CardRecyle;
        _tCard.Clear();

        ListInit();
        DestroyState();
        RecycleEquipOrCard = new BasePoolDT<long>[6];

        if (recycleSpine)
            recycleSpine.SetActive(true);
        else
            UITool.f_CreateMagicById((int)EM_MagicId.eRecycle, ref recycleSpine, f_GetObject("spineParent").transform, 6,
                "huishou_loop", null, true, 70);
    }
    /// <summary>
    /// 卡牌重生
    /// </summary>
    void UI_CardRebirthBtn(GameObject go, object obj1, object obj2)
    {
        tType = EM_RecyleOrRebirth.CardRebirth;
        tMagic = EM_Magic.Rebirth;
        _tCard.Clear();

        ListInit();
        DestroyState();
        RecycleEquipOrCard = new BasePoolDT<long>[6];
        if (recycleSpine)
            recycleSpine.SetActive(false);
    }
    /// <summary>
    /// 装备回收
    /// </summary>
    void UI_EquipRecycleBtn(GameObject go, object obj1, object obj2)
    {
        tType = EM_RecyleOrRebirth.EquipRecyle;
        _tEquip.Clear();

        ListInit();
        DestroyState();
        RecycleEquipOrCard = new BasePoolDT<long>[6];
        if (recycleSpine)
            recycleSpine.SetActive(true);
        else
            UITool.f_CreateMagicById((int)EM_MagicId.eRecycle, ref recycleSpine, f_GetObject("spineParent").transform, 6,
                "huishou_loop", null, true, 70);
    }
    /// <summary>
    /// 装备重生
    /// </summary>
    void UI_EquipRebirthBtn(GameObject go, object obj1, object obj2)
    {
        tType = EM_RecyleOrRebirth.EquipRebirth;
        tMagic = EM_Magic.Rebirth;
        _tEquip.Clear();

        ListInit();
        DestroyState();
        RecycleEquipOrCard = new BasePoolDT<long>[6];
        if (recycleSpine)
            recycleSpine.SetActive(false);
    }
    /// <summary>
    /// 法宝重生
    /// </summary>
    void UI_TreasureRebirthBtn(GameObject go, object obj1, object obj2)
    {
        tType = EM_RecyleOrRebirth.TreasureRebirth;
        tMagic = EM_Magic.Rebirth;
        ListInit();
        DestroyState();
        RecycleEquipOrCard = new BasePoolDT<long>[6];
        if (recycleSpine)
            recycleSpine.SetActive(false);
    }
    /// <summary>
    /// 选择卡牌  装备  法宝
    /// </summary>
    void UI_SeleCardOrEquip(GameObject go, object obj1, object obj2)
    {
        if(obj2 != null)
        {
            int index = (int)obj2;
            UpdateMagin(RecycleEquipOrCard[index], index);
            CloseSele();
        }
        //Clear();
    }
    /// <summary>
    /// 打开选择界面
    /// </summary>
    void UI_SeleEquipRebith(GameObject go, object obj1, object obj2)
    {
        ListInit();
        updataSele();
    }
    /// <summary>
    /// 卡牌或装备重生按钮
    /// </summary>
    void EquipOrCardRebith(GameObject go, object obj1, object obj2)
    {
        switch(tType)
        {
            case EM_RecyleOrRebirth.CardRebirth:
            case EM_RecyleOrRebirth.EquipRebirth:
            case EM_RecyleOrRebirth.TreasureRebirth:
                if(RecycleEquipOrCard[(int)EM_Magic.Rebirth] == null)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(354));
                    return;
                }
                f_GetObject("Preview_Title").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(355);
                UpdateRecycle_Preview();
                break;
            case EM_RecyleOrRebirth.EquipRecyle:
            case EM_RecyleOrRebirth.CardRecyle:
                int IsAdd = 0;
                for(int i = 0; i < RecycleEquipOrCard.Length - 1; i++)
                {
                    if(RecycleEquipOrCard[i] == null)
                        IsAdd++;
                }
                if(IsAdd >= 5)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(354));
                    return;
                }
                f_GetObject("Preview_Title").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(356);
                UpdateRecycle_Preview();
                break;
        }
        f_GetObject("Recycle_Preview").SetActive(true);
        Return_ItemParent.transform.parent.GetComponent<UIScrollView>().ResetPosition();
    }
    /// <summary>
    /// 确认所选的重生
    /// </summary>
    void RecycleOrRebith(GameObject go, object obj1, object obj2)
    {
        SocketCallbackDT tBack = new SocketCallbackDT();
        Data_Pool.m_AwardPool.m_GetLoginAward.Clear();
        switch(tType)
        {
            case EM_RecyleOrRebirth.EquipRebirth:
            case EM_RecyleOrRebirth.CardRebirth:
            case EM_RecyleOrRebirth.TreasureRebirth:
            case EM_RecyleOrRebirth.GodEquipRebirth:
                if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < 50)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(357));
                    return;
                }
                break;
        }

        bool needReturn;
        switch(tType)
        {
            case EM_RecyleOrRebirth.None:
                break;
            case EM_RecyleOrRebirth.EquipRecyle:
                needReturn = true;
                for (int i = 0; i < RecycleEquipOrCard.Length; i++)
                {
                    if (RecycleEquipOrCard[i] != null)
                    {
                        needReturn = false;
                        break;
                    }
                }

                if (needReturn)
                {
                    return;
                }
                tBack.m_ccCallbackFail = RcycleFail;
                tBack.m_ccCallbackSuc = RecycleSuc;
                Data_Pool.m_EquipPool.f_EquipRecycle(RecycleEquipOrCard[0], RecycleEquipOrCard[1], RecycleEquipOrCard[2]
                    , RecycleEquipOrCard[3], RecycleEquipOrCard[4], tBack);
                break;
            case EM_RecyleOrRebirth.EquipRebirth:
                tBack.m_ccCallbackFail = RebithFail;
                tBack.m_ccCallbackSuc = RebithSuc;
                if(RecycleEquipOrCard[(int)EM_Magic.Rebirth] == null)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(358));
                    return;
                }
                Data_Pool.m_EquipPool.f_EquipRebirth(RecycleEquipOrCard[(int)EM_Magic.Rebirth].iId, tBack);
                break;
            case EM_RecyleOrRebirth.CardRecyle:
                needReturn = true;
                for (int i = 0; i < RecycleEquipOrCard.Length; i++)
                {
                    if (RecycleEquipOrCard[i] != null)
                    {
                        needReturn = false;
                        break;
                    }
                }

                if (needReturn)
                {
                    return;
                }
                tBack.m_ccCallbackFail = RcycleFail;
                tBack.m_ccCallbackSuc = RecycleSuc;
                Data_Pool.m_CardPool.f_CardRecycle(RecycleEquipOrCard[0], RecycleEquipOrCard[1], RecycleEquipOrCard[2]
                    , RecycleEquipOrCard[3], RecycleEquipOrCard[4], tBack);
                MessageBox.DEBUG(string.Format(CommonTools.f_GetTransLanguage(359), RecycleEquipOrCard[0].iId));
                break;
            case EM_RecyleOrRebirth.CardRebirth:
                tBack.m_ccCallbackFail = RebithFail;
                tBack.m_ccCallbackSuc = RebithSuc;
                if(RecycleEquipOrCard[(int)EM_Magic.Rebirth] == null)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(360));
                    return;
                }
                Data_Pool.m_CardPool.f_CardRebirth(RecycleEquipOrCard[(int)EM_Magic.Rebirth].iId, tBack);
                break;
            case EM_RecyleOrRebirth.TreasureRebirth:
                tBack.m_ccCallbackFail = RebithFail;
                tBack.m_ccCallbackSuc = RebithSuc;
                if(RecycleEquipOrCard[(int)EM_Magic.Rebirth] == null)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(361));
                    return;
                }
                Data_Pool.m_TreasurePool.f_TreasureRebirth(RecycleEquipOrCard[(int)EM_Magic.Rebirth].iId, tBack);
                break;
            case EM_RecyleOrRebirth.GodEquipRebirth:
                tBack.m_ccCallbackFail = RebithFail;
                tBack.m_ccCallbackSuc = RebithSuc;
                if (RecycleEquipOrCard[(int)EM_Magic.Rebirth] == null)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(358));
                    return;
                }
                Data_Pool.m_GodEquipPool.f_GodEquipRebirth(RecycleEquipOrCard[(int)EM_Magic.Rebirth].iId, tBack);
                break;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        f_GetObject("Recycle_Preview").SetActive(false);
        Gold = 0;
        RemoReturn();
    }
    int Time_CreaceRecycleEffect = 0;
    int Time_OverRecycleSuc = 0;
    void RecycleSuc(object obj)
    {
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(362));
        //Time_CreaceRecycleEffect = ccTimeEvent.GetInstance().f_RegEvent(0, false, null, CreaceRecycleEffect);
        //Time_OverRecycleSuc = ccTimeEvent.GetInstance().f_RegEvent(1f, false, null, CreaceRecycleSuc);
        Time_OverRecycleSuc = ccTimeEvent.GetInstance().f_RegEvent(1f, false, null, CreaceRecycleSuc);

        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.Recycle_02);
        if (recycleSpine)
        {
            recycleSpine.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "huishou", false);
            recycleSpine.GetComponent<SkeletonAnimation>().state.AddAnimation(0, "huishou_loop", true, 0);
            GameObject[] tempGo = new GameObject[5];
            float[] distance = new float[5];
            int num = 0;
            if (tType == EM_RecyleOrRebirth.CardRecyle)
            {
                num = _tCard.Count;
            }
            else if (tType == EM_RecyleOrRebirth.EquipRecyle)
            {
                num = _tEquip.Count;
            }
            for (int i = 0; i < num; i++)
            {
                tempGo[i] = NGUITools.AddChild(RecyleParent[i].transform.parent.gameObject, RecyleParent[i]);
                tempGo[i].transform.localPosition = RecyleParent[i].transform.localPosition;
                distance[i] = Vector3.Distance(tempGo[i].transform.localPosition,
                    f_GetObject("endPoint").transform.localPosition);
                TweenPosition com = tempGo[i].AddComponent<TweenPosition>();
                com.@from = tempGo[i].transform.localPosition;
                com.to = f_GetObject("endPoint").transform.localPosition;
                com.duration = 0.75f;
                EventDelegate.Callback call = delegate
                {
                    for (int j = 0; j < tempGo.Length; j++)
                    {
                        if (tempGo[j])
                        {
                            DestroyImmediate(tempGo[j].gameObject);
                        }
                        
                    }
                };
                com.SetOnFinished(call);
            }
            DestroyState();
        }
        //Data_Pool.m_EquipPool.f_CheckEquipRecycleRedPoint();
        //Data_Pool.m_CardPool.f_CheckCardRecycleRedPoint();
    }

    void CreaceRecycleEffect(object obj)
    {
        for(int i = 0; i < RecycleEquipOrCard.Length; i++)
        {
            if(RecycleEquipOrCard[i] != null)
            {
                GameObject tEffect = UITool.f_CreateEffect2(UIEffectName.huishou_fenjie_01, RecyleParent[i].transform.Find("Effect"), Vector3.zero, 0.2f, 0.95f);
            }
        }
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.Recycle_02);
    }
    void CreaceRecycleSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        RecycleEquipOrCard = new BasePoolDT<long>[6];
        ListInit();
        DestroyState();
        f_GetObject("RebirthShowPro").SetActive(true);
        List<AwardPoolDT> tGoods = Data_Pool.m_AwardPool.m_GetLoginAward;
        if(tGoods.Count >= 1)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tGoods });
    }
    void RcycleFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.f_GetError((int)obj);
        MessageBox.DEBUG(UITool.f_GetError((int)obj));
    }

    void RebithSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(363));
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.Rebirth));
        f_GetObject("RebirthShowPro").SetActive(false);
        RecycleEquipOrCard = new BasePoolDT<long>[6];
        ListInit();
        DestroyState();
        List<AwardPoolDT> tGoods = Data_Pool.m_AwardPool.m_GetLoginAward;
        if(tGoods.Count >= 1)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tGoods });

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    void RebithFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.f_GetError((int)obj);
        MessageBox.DEBUG(UITool.f_GetError((int)obj));
    }

    #endregion
    #region 刷新界面
    /// <summary>
    /// 刷新钱币
    /// </summary>
    void UpdateMoney()
    {
        //UILabel CardDsoul = f_GetObject("CardDsoul").transform.FindChild("Label").GetComponent<UILabel>();
        //UILabel EquipDsoul = f_GetObject("EquipDsoul").transform.FindChild("Label").GetComponent<UILabel>();
        //UILabel Sycee = f_GetObject("Sycee").transform.FindChild("Label").GetComponent<UILabel>();
        //CardDsoul.text = UITool.f_GetMoney(Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_GeneralSoul));    //将魂
        //EquipDsoul.text = UITool.f_GetMoney(Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Prestige));     //威名?
        //Sycee.text = UITool.f_GetMoney(Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Sycee));    //元宝

        _tCard = new List<BasePoolDT<long>>();   //选择的卡牌
        _tEquip = new List<BasePoolDT<long>>();  //选择的装备
        _Treasure = new List<BasePoolDT<long>>();//选择的法宝 
    }
    private void OpenMoneyPanel()
    {
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_GeneralSoul);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Prestige);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }

    void InitRecyclePos()
    {
        RecyleParent[(int)EM_Magic.Bottom] = f_GetObject("BottomMagic");
        RecyleParent[(int)EM_Magic.Left] = f_GetObject("LeftMagic");
        RecyleParent[(int)EM_Magic.Right] = f_GetObject("RightMagic");
        RecyleParent[(int)EM_Magic.TopL] = f_GetObject("TopLeftMagic");
        RecyleParent[(int)EM_Magic.TopR] = f_GetObject("TopRightMagic");
    }
    /// <summary>
    /// 打开选择界面更新
    /// </summary>
    void updataSele()
    {
        switch(tType)
        {
            case EM_RecyleOrRebirth.EquipRecyle:
            case EM_RecyleOrRebirth.EquipRebirth:
                if(_EquipList.Count == 0)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(364));
                    return;
                }
                f_GetObject("SeleEquip").SetActive(true);
                if(Equip_WrapComponet == null)
                    Equip_WrapComponet = new UIWrapComponent(165, 1, 770, 5,
                        Equip_ItemParent, Equip_Item, _EquipList, EquipRebirth, EquipClick);
                Equip_WrapComponet.f_UpdateList(_EquipList);
                Equip_WrapComponet.f_UpdateView();
                Equip_WrapComponet.f_ResetView();
                break;
            case EM_RecyleOrRebirth.CardRecyle:
            case EM_RecyleOrRebirth.CardRebirth:
                if(_CardList.Count == 0)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(365));
                    return;
                }
                f_GetObject("SeleCard").SetActive(true);
                if(Card_WrapComponet == null)
                    Card_WrapComponet = new UIWrapComponent(165, 1, 770, 5,
                    Card_ItemParent, Card_Item, _CardList, CardRebirth, CardClick);
                Card_WrapComponet.f_UpdateList(_CardList);
                Card_WrapComponet.f_UpdateView();
                Card_WrapComponet.f_ResetView();
                break;
            case EM_RecyleOrRebirth.TreasureRebirth:
                if(_TreasureList.Count == 0)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(366));
                    return;
                }
                f_GetObject("SeleTreasure").SetActive(true);
                if(Treasure_WrapComponet == null)
                    Treasure_WrapComponet = new UIWrapComponent(165, 1, 770, 5,
                    Treasure_ItemParent, Treasure_Item, _TreasureList, TreasureRebirth, TreasureClick);
                Treasure_WrapComponet.f_UpdateList(_TreasureList);
                Treasure_WrapComponet.f_UpdateView();
                Treasure_WrapComponet.f_ResetView();
                break;
            case EM_RecyleOrRebirth.GodEquipRebirth:
                if (_GodEquipList.Count == 0)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(364));
                    return;
                }
                f_GetObject("SeleGodEquip").SetActive(true);
                if (GodEquip_WrapComponet == null)
                    GodEquip_WrapComponet = new UIWrapComponent(200, 1, 770, 5,
                        GodEquip_ItemParent, GodEquip_Item, _GodEquipList, GodEquipRebirth, GodEquipClick);
                GodEquip_WrapComponet.f_UpdateList(_GodEquipList);
                GodEquip_WrapComponet.f_UpdateView();
                GodEquip_WrapComponet.f_ResetView();
                break;
        }
    }
    /// <summary>
    /// 关闭选择界面
    /// </summary>
    void CloseSele()
    {
        switch(tType)
        {
            case EM_RecyleOrRebirth.EquipRecyle:
            case EM_RecyleOrRebirth.EquipRebirth:
                f_GetObject("SeleEquip").SetActive(false);
                break;
            case EM_RecyleOrRebirth.CardRecyle:
            case EM_RecyleOrRebirth.CardRebirth:
                f_GetObject("SeleCard").SetActive(false);
                break;
            case EM_RecyleOrRebirth.TreasureRebirth:
                f_GetObject("SeleTreasure").SetActive(false);
                break;
            case EM_RecyleOrRebirth.GodEquipRebirth:
                f_GetObject("SeleGodEquip").SetActive(false);
                break;
        }
    }
    /// <summary>
    /// 更新界面(选择卡牌 装备或法宝之后刷新界面)
    /// </summary>
    void UpdateMagin(BasePoolDT<long> Dt, int indedx = 0)
    {
        if(Dt == null)
            return;
        long id = Dt.iId;
        switch(tType)
        {
            case EM_RecyleOrRebirth.EquipRecyle:
            case EM_RecyleOrRebirth.EquipRebirth:
                EquipPoolDT tEquip;
                tEquip = Dt as EquipPoolDT;
                CreateEquip(tEquip, (EM_Magic)indedx);
                RebirthPro(tEquip);
                break;
            case EM_RecyleOrRebirth.CardRecyle:
            case EM_RecyleOrRebirth.CardRebirth:
                CardPoolDT tCard;
                tCard = Dt as CardPoolDT;
                CreateCardState(tCard, (EM_Magic)indedx);
                RebirthPro(tCard);
                break;
            case EM_RecyleOrRebirth.TreasureRebirth:
                TreasurePoolDT tTreasure;
                tTreasure = Dt as TreasurePoolDT;
                CreateTreasure(tTreasure, tMagic);
                RebirthPro(tTreasure);
                break;
            case EM_RecyleOrRebirth.GodEquipRebirth:
                GodEquipPoolDT tGodEquip;
                tGodEquip = Dt as GodEquipPoolDT;
                CreateGodEquip(tGodEquip, tMagic);
                RebirthPro(tGodEquip);
                break;
        }
        UpdateDischarge((EM_Magic)indedx);
    }
    /// <summary>
    /// 展示重生的属性
    /// </summary>
    void RebirthPro(BasePoolDT<long> tGoods)
    {
        Transform tShowPro = f_GetObject("RebirthShowPro").transform;
        f_GetObject("RebirthShowPro").SetActive(true);
        tShowPro.Find("ProManage/Star").gameObject.SetActive(true);
        UISprite[] stars = new UISprite[6];
        switch(tType)
        {
            case EM_RecyleOrRebirth.EquipRebirth:
                EquipPoolDT tEquip = tGoods as EquipPoolDT;
                SetRebirthPro(tShowPro.Find("ProManage"), CommonTools.f_GetTransLanguage(367), tEquip.m_lvIntensify, CommonTools.f_GetTransLanguage(368), tEquip.m_lvRefine);
                switch((EM_Important)tEquip.m_EquipDT.iColour)
                {
                    case EM_Important.Oragen:
                        _UpdateStar(3, tShowPro, ref stars);
                        UITool.f_UpdateStarNum(stars, tEquip.m_sstars);
                        break;
                    case EM_Important.Red:
                        _UpdateStar(5, tShowPro, ref stars);
                        UITool.f_UpdateStarNum(stars, tEquip.m_sstars);
                        break;
                }
                break;
            case EM_RecyleOrRebirth.CardRebirth:
                CardPoolDT tCard = tGoods as CardPoolDT;
                SetRebirthPro(tShowPro.Find("ProManage"), CommonTools.f_GetTransLanguage(367), tCard.m_iLv, CommonTools.f_GetTransLanguage(369), tCard.m_iEvolveLv);
                _UpdateStar(6, tShowPro, ref stars);
                UITool.f_UpdateStarNum(stars, tCard.m_iLvAwaken / 10);
                break;
            case EM_RecyleOrRebirth.TreasureRebirth:
                tShowPro.Find("ProManage/Star").gameObject.SetActive(false);
                TreasurePoolDT tTreasure = tGoods as TreasurePoolDT;
                SetRebirthPro(tShowPro.Find("ProManage"), CommonTools.f_GetTransLanguage(367), tTreasure.m_lvIntensify, CommonTools.f_GetTransLanguage(368), tTreasure.m_lvRefine);
                break;
            default:
                tShowPro.gameObject.SetActive(false);
                break;
        }
        tShowPro.Find("ProManage").GetComponent<UIGrid>().enabled = true;
        tShowPro.Find("ProManage/Star/stars").GetComponent<UIGrid>().enabled = true;
    }

    private void _UpdateStar(int ShowStarNum, Transform tShowPro, ref UISprite[] tUispreite)
    {
        for(int i = 0; i < tShowPro.Find("ProManage/Star/stars").childCount; i++)
        {
            tShowPro.Find("ProManage/Star/stars").GetChild(i).gameObject.SetActive(i < ShowStarNum);
            if(i < ShowStarNum)
            {
                tUispreite[i] = tShowPro.Find("ProManage/Star/stars").GetChild(i).GetComponent<UISprite>();
            }
        }
    }
    /// <summary>
    /// 设置重生展示的属性
    /// </summary>
    void SetRebirthPro(Transform tShowPro, string ProName1, int Pro1, string ProName2, int Pro2)
    {
        tShowPro.Find("Lv").GetComponent<UILabel>().text = string.Format("[f5bf3d]{0}:[-]  {1}", ProName1, Pro1);
        tShowPro.Find("Fow").GetComponent<UILabel>().text = string.Format("[f5bf3d]{0}:[-]  {1}", ProName2, Pro2);
    }
    void UpdateDischarge(EM_Magic obj)
    {
        GameObject gogo = f_GetObject("TopLeftClose");
        switch(obj)
        {
            case EM_Magic.TopL:
                gogo = f_GetObject("TopLeftClose");
                break;
            case EM_Magic.Left:
                gogo = f_GetObject("LeftClose");
                break;
            case EM_Magic.Bottom:
                gogo = f_GetObject("BottomClose");
                break;
            case EM_Magic.Right:
                gogo = f_GetObject("RightClose");
                break;
            case EM_Magic.TopR:
                gogo = f_GetObject("TopRightClose");
                break;
        }
        //switch (tType)
        //{
        //    case EM_RecyleOrRebirth.EquipRecyle:
        //        gogo.transform.localPosition = new Vector2(-90, 180);
        //        break;
        //    case EM_RecyleOrRebirth.CardRecyle:
        //        switch (obj)
        //        {
        //            case EM_Magic.TopR:
        //            case EM_Magic.TopL:
        //                //gogo.transform.localPosition = new Vector2(-90, 180);
        //                //gogo.transform.localPosition = new Vector2(-120, 250);
        //                //break;
        //            case EM_Magic.Left:
        //            case EM_Magic.Right:
        //                //gogo.transform.localPosition = new Vector2(-90, 180);
        //                //gogo.transform.localPosition = new Vector2(-125, 330);
        //                break;
        //            case EM_Magic.Bottom:
        //                //gogo.transform.localPosition = new Vector2(-90, 180);
        //                //gogo.transform.localPosition = new Vector2(-120, 350);
        //                break;
        //        }
        //        break;
        //}
        gogo.transform.localPosition = new Vector2(-90, 180);
        gogo.SetActive(true);
    }
    /// <summary>
    /// 刷新预览界面
    /// </summary>
    void UpdateRecycle_Preview()
    {
        switch(tType)
        {
            case EM_RecyleOrRebirth.EquipRecyle:
                EquipRecyle();
                break;
            case EM_RecyleOrRebirth.EquipRebirth:
                EquipRebirth();
                break;
            case EM_RecyleOrRebirth.CardRecyle:
                CardRecyle();
                break;
            case EM_RecyleOrRebirth.CardRebirth:
                CardRebirth();
                break;
            case EM_RecyleOrRebirth.TreasureRebirth:
                TreasureRebirth();
                break;
            case EM_RecyleOrRebirth.GodEquipRebirth:
                GodEquipRebirth();
                break;
        }
    }

    /// <summary>
    /// 装备分解
    /// </summary>
    void EquipRecyle()
    {
        int Dsoul = 0;
        int Red = 0;
        List<BaseGoodsPoolDT> RefinePill = new List<BaseGoodsPoolDT>();
        for(int i = 0; i < RecycleEquipOrCard.Length - 1; i++)
        {
            if(RecycleEquipOrCard[i] != null)
            {
                equipHistory++;
                Dsoul += GetDsoul(RecycleEquipOrCard[i], (int)EM_ResourceType.Equip);  //装备是威名   卡牌是将魂
                EquipPoolDT tEquip = ListFind(_tEquip, RecycleEquipOrCard[i]) as EquipPoolDT;    //当前装备
                RefinePill.AddRange(GetEquipRefinePill(tEquip));     //获得精炼丹 
                if(tEquip.m_EquipDT.iColour == (int)EM_Important.Red)
                    Red += 40;
                UITool.f_OpenOrCloseWaitTip(true);
                Data_Pool.m_EquipPool.f_GetEquipCostHistory(RecycleEquipOrCard[i].iId, EquipHistory, new SocketCallbackDT());
            }
        }
        if(Dsoul > 0)
            CreateReturn(EM_ResourceType.Money, (int)EM_UserAttr.eUserAttr_Prestige, Dsoul);
        if(RefinePill.Count > 0)
        {
            for(int indedx = 0; indedx < RefinePill.Count; indedx++)
            {
                if(RefinePill[indedx].m_iNum > 0)
                    CreateReturn(EM_ResourceType.Good, RefinePill[indedx].m_BaseGoodsDT.iId, RefinePill[indedx].m_iNum);
            }
        }
        if(Red > 0)
            CreateReturn(EM_ResourceType.Good, (int)EM_MoneyType.eRedEquipElite, Red);

    }
    /// <summary>
    /// 卡牌分解
    /// </summary>
    void CardRecyle()
    {
        List<AwardPoolDT> tGainAward = new List<AwardPoolDT>();
        int Dsoul = 0;
        List<BaseGoodsPoolDT> tStone = new List<BaseGoodsPoolDT>();
        int tEvolve = 0;
        int tAwaken = 0;
        int tArtifact = 0;
        int tRefineArtifact = 0;

        CardSkyNum = 0;
        CardHistoryNum = 0;
        CardHistoryTime = 0;
        List<AwakenEquipPoolDT> tAwakenEquip = new List<AwakenEquipPoolDT>();
        int Red = 0;
        for(int i = 0; i < RecycleEquipOrCard.Length; i++)
        {
            if(RecycleEquipOrCard[i] != null)
            {
                Dsoul += GetDsoul(RecycleEquipOrCard[i], (int)EM_ResourceType.Card);   //将魂 
                CardPoolDT tCard = ListFind(_tCard, RecycleEquipOrCard[i]) as CardPoolDT;
                tStone.AddRange(GetExpPill(tCard, ref Gold));         //经验石
                tEvolve += GetCardEvolvePillNum(tCard);          //进阶丹
                tAwaken += GetAwakenPillNum(tCard, ref Gold);     //领悟丹
                tAwakenEquip.AddRange(GetAwakenGoodsNum(tCard));    //领悟道具
                if(tCard.m_CardDT.iImportant == (int)EM_Important.Red)
                    Red += 30;
                CardHistoryNum++;
                UITool.f_OpenOrCloseWaitTip(true);
                Data_Pool.m_CardPool.f_GetCardSkyNum(RecycleEquipOrCard[i].iId, _GetCardSkyNum);
                tArtifact += GetArtifact(tCard, out tRefineArtifact);
            }
        }
        if(Dsoul > 0)
            CreateReturn(EM_ResourceType.Money, (int)EM_UserAttr.eUserAttr_GeneralSoul, Dsoul);
        tStone = CountExpGoodsNum(tStone);
        if(tStone.Count > 0)
        {
            for(int index = 0; index < tStone.Count; index++)
            {
                if(tStone[index].m_iNum > 0)
                    CreateReturn(EM_ResourceType.Good, tStone[index].m_BaseGoodsDT.iId, tStone[index].m_iNum);
            }
        }
        if(tEvolve > 0)
            CreateReturn(EM_ResourceType.Good, (int)EM_MoneyType.eCardRefineStone, tEvolve);
        if(tAwaken > 0)
            CreateReturn(EM_ResourceType.Good, (int)EM_MoneyType.eCardAwakenStone, tAwaken);
        tAwakenEquip = CountAwakenGoodsNum(tAwakenEquip);
        if(tAwakenEquip.Count > 0)
        {
            for(int index = 0; index < tAwakenEquip.Count; index++)
            {
                if(tAwakenEquip[index].m_num > 0)
                    CreateReturn(EM_ResourceType.AwakenEquip, tAwakenEquip[index].m_AwakenEquipDT.iId, tAwakenEquip[index].m_num);
            }
        }
        if(Gold > 0)
            CreateGold();
        if(Red > 0)
            CreateReturn(EM_ResourceType.Good, (int)EM_MoneyType.eRedBattleToken, Red);

        if(tArtifact > 0)
            CreateReturn(EM_ResourceType.Good, (int)EM_MoneyType.eCardArtifact, tArtifact);
        if(tRefineArtifact > 0)
            CreateReturn(EM_ResourceType.Good, 130, tRefineArtifact);
    }
    void _GetCardSkyNum(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        CardHistoryTime++;
        SC_CardCostHistory ttt = (SC_CardCostHistory)obj;
        CardSkyNum += ttt.totalSkyDestinyItem;
        if(CardHistoryTime == CardHistoryNum)
        {
            if(CardSkyNum > 0)
                CreateReturn(EM_ResourceType.Good, 108, CardSkyNum);
        }
    }
    int EquipSycee = 0;    //元宝
    int EquipFraments = 0;  //碎片
    int equipHistory = 0;  //请求历史次数
    int CardHistoryNum = 0;   //卡牌请求次数
    int CardHistoryTime = 0;  //卡牌返回次数
    int getHistorytime = 0;   //返回历史
    int CardSkyNum = 0;
    /// <summary>
    /// 装备重生
    /// </summary>
    void EquipRebirth()
    {
        if(RecycleEquipOrCard[(int)EM_Magic.Rebirth] != null)
        {
            equipHistory = 1;
            EquipPoolDT tEquip = ListFind(_tEquip, RecycleEquipOrCard[(int)EM_Magic.Rebirth]) as EquipPoolDT;
            int EquipFragment = GetEquipFraments(tEquip);    //碎片
            List<BaseGoodsPoolDT> RefinePill = GetEquipRefinePill(tEquip);
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_EquipPool.f_GetEquipCostHistory(RecycleEquipOrCard[(int)EM_Magic.Rebirth].iId, EquipHistory, new SocketCallbackDT());

            if(RefinePill.Count > 0)
            {
                for(int indedx = 0; indedx < RefinePill.Count; indedx++)
                {
                    if(RefinePill[indedx].m_iNum > 0)
                        CreateReturn(EM_ResourceType.Good, RefinePill[indedx].m_BaseGoodsDT.iId, RefinePill[indedx].m_iNum);
                }
            }
        }
    }
    /// <summary>
    /// 卡牌重生
    /// </summary>
    void CardRebirth()
    {
        if(RecycleEquipOrCard[(int)EM_Magic.Rebirth] != null)
        {
            CardHistoryTime = 0;
            CardHistoryNum = 0;
            CardSkyNum = 0;


            CardPoolDT tCard = ListFind(_tCard, RecycleEquipOrCard[(int)EM_Magic.Rebirth]) as CardPoolDT;
            int tCardFrament = DismantleCard(tCard);   //碎片
            List<BaseGoodsPoolDT> tStone = new List<BaseGoodsPoolDT>();
            if(tCard.m_CardDT.iCardType != 1)
                tStone = GetExpPill(tCard, ref Gold);         //经验石
            int tEvolve = GetCardEvolvePillNum(tCard);          //进阶丹
            int tAwaken = GetAwakenPillNum(tCard, ref Gold);     //领悟丹
            int tRefineArtifact = 0;
            int tArtifact = GetArtifact(tCard, out tRefineArtifact);                     //神器丹
            List<AwakenEquipPoolDT> tAwakenEquip = GetAwakenGoodsNum(tCard);    //领悟道具
            CardHistoryNum++;
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_CardPool.f_GetCardSkyNum(RecycleEquipOrCard[(int)EM_Magic.Rebirth].iId, _GetCardSkyNum);  //命星
            if(tCard.m_CardDT.iCardType == 1)
                tAwaken *= 2;
            if(tCardFrament > 0)
                CreateReturn(EM_ResourceType.CardFragment, tCard.m_CardDT.iId, tCardFrament);

            if(tStone.Count > 0)
            {
                for(int index = 0; index < tStone.Count; index++)
                {
                    if(tStone[index].m_iNum > 0)
                        CreateReturn(EM_ResourceType.Good, tStone[index].m_BaseGoodsDT.iId, tStone[index].m_iNum);
                }
            }
            if(tEvolve > 0)
                CreateReturn(EM_ResourceType.Good, (int)EM_MoneyType.eCardRefineStone, tEvolve);
            if(tAwaken > 0)
                CreateReturn(EM_ResourceType.Good, (int)EM_MoneyType.eCardAwakenStone, tAwaken);
            if(tAwakenEquip.Count > 0)
            {
                for(int index = 0; index < tAwakenEquip.Count; index++)
                {
                    if(tAwakenEquip[index].m_num > 0)
                        CreateReturn(EM_ResourceType.AwakenEquip, tAwakenEquip[index].m_AwakenEquipDT.iId, tAwakenEquip[index].m_num);
                }
            }
            if(Gold > 0)
                CreateGold();

            if(tArtifact > 0)
                CreateReturn(EM_ResourceType.Good, (int)EM_MoneyType.eCardArtifact, tArtifact);
            if(tRefineArtifact > 0)
                CreateReturn(EM_ResourceType.Good, 130, tRefineArtifact);
        }
    }
    /// <summary>
    /// 法宝重生    
    /// </summary>
    void TreasureRebirth()
    {


        TreasurePoolDT tTreasure = ListFind(_Treasure, RecycleEquipOrCard[(int)EM_Magic.Rebirth]) as TreasurePoolDT;
        int EatTreasure = 0;    //被吃的道具
        int PillNum = 0;        //消耗的进阶丹
        List<TreasurePoolDT> tTreaExp = GetTreasureUpExp(tTreasure, ref Gold);   //获得法宝经验道具   数量可以为0
        GetTreasureRefineExp(tTreasure, ref EatTreasure, ref Gold, ref PillNum);

        if(EatTreasure > 0)
            CreateReturn(EM_ResourceType.Treasure, tTreasure.m_TreasureDT.iId, EatTreasure);
        if(PillNum > 0)
            CreateReturn(EM_ResourceType.Good, (int)EM_MoneyType.eTreasureRefinePill, PillNum);
        for(int i = 0; i < tTreaExp.Count; i++)
        {
            if(tTreaExp[i].m_Num > 0)
                CreateReturn(EM_ResourceType.Treasure, tTreaExp[i].m_TreasureDT.iId, tTreaExp[i].m_Num);
        }

        if(Gold > 0)
            CreateGold();


    }
    /// <summary>
    /// 获得装备历史银币
    /// </summary>
    void EquipHistory(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        SC_EquipCostHistory tHistory = (SC_EquipCostHistory)obj;
        if(tHistory.totalFragment > 0)
            //CreateReturn(1, "碎片", tHistory.totalFragment);
            EquipFraments += tHistory.totalFragment;
        if(tHistory.totalMoney > 0)
            Gold += tHistory.totalMoney;
        if(tHistory.totalSycee > 0)
            //CreateReturn(1, "元宝", tHistory.totalSycee);
            EquipSycee += tHistory.totalSycee;
        getHistorytime++;
        EquipPoolDT tEquip;
        if(getHistorytime == equipHistory)
        {
            if(Gold > 0)
                CreateReturn(EM_ResourceType.Money, (int)EM_UserAttr.eUserAttr_Money, 0, Gold);
            if(EquipSycee > 0)
                CreateReturn(EM_ResourceType.Money, (int)EM_UserAttr.eUserAttr_Sycee, EquipSycee);
            if(EquipFraments > 0)
            {
                for(int i = 0; i < RecycleEquipOrCard.Length - 1; i++)
                {
                    tEquip = RecycleEquipOrCard[i] as EquipPoolDT;
                    CreateReturn(EM_ResourceType.EquipFragment, glo_Main.GetInstance().m_SC_Pool.m_EquipFragmentsSC.f_GetSC(tEquip.m_EquipDT.iId + 1000).iId, EquipFraments);
                }
            }
            equipHistory = 0;
            getHistorytime = 0;
            EquipFraments = 0;
            EquipSycee = 0;
            Gold = 0;
        }

    }

    void CreateReturn(EM_ResourceType type, int id, int num = 0, long num2 = 0)
    {
        ResourceCommonItem tttItem = ResourceCommonItem.f_Create(Return_ItemParent, f_GetObject("ResourceCommonItem"));
        if(num != 0)
            tttItem.f_UpdateByInfo((int)type, id, num);
        else
            tttItem.f_UpdateByInfo((int)type, id, System.Convert.ToInt32(num2));
        Return_ItemParent.GetComponent<UIGrid>().enabled = true;

    }
	
	private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 19);
    }
    /// <summary>
    /// 创建银币Item
    /// </summary>
    void CreateGold()
    {
        if(Gold > 0)
            CreateReturn(EM_ResourceType.Money, (int)EM_UserAttr.eUserAttr_Money, 0, Gold);
    }

    private GameObject spineModel = null;
    /// <summary>
    /// 创建卡牌模型
    /// </summary>
    void CreateCardState(CardPoolDT card, EM_Magic Magic)
    {
        UI2DSprite tSprite = null;
        string name = card.m_CardDT.szName;
        string borderName = UITool.f_GetImporentColorName(card.m_CardDT.iImportant, ref name);
        switch(Magic)
        {
            case EM_Magic.TopL:
                CreateEquip("TopLeftMagic", ref tSprite, name, borderName, 1);
                break;
            case EM_Magic.Left:
                CreateEquip("LeftMagic", ref tSprite, name, borderName, 1);
                break;
            case EM_Magic.Bottom:
                CreateEquip("BottomMagic", ref tSprite, name, borderName, 1);
                break;
            case EM_Magic.Right:
                CreateEquip("RightMagic", ref tSprite, name, borderName, 1);
                break;
            case EM_Magic.TopR:
                CreateEquip("TopRightMagic", ref tSprite, name, borderName, 1);
                break;
            case EM_Magic.Rebirth:
                CreateEquip("RebirthBtn", ref tSprite, name, borderName, 1);
                break;
        }
        tSprite.sprite2D = UITool.f_GetIconSpriteByCardId(card);
        tSprite.MakePixelPerfect();
        tSprite.depth = 3000;
        #region
        //string name = card.m_CardDT.szName;
        //UITool.f_GetImporentColorName(card.m_CardDT.iImportant, ref name);
        //GameObject tState = UITool.f_GetStatelObject(card.m_CardDT);
        //switch (Magic)
        //{
        //    case EM_Magic.TopL:
        //        CreateCardState("TopLeftMagic", ref tState, name);
        //        tState.transform.localScale = Vector3.one * 120;
        //        tState.GetComponent<Renderer>().sortingOrder = 1;
        //        break;
        //    case EM_Magic.Left:
        //        CreateCardState("LeftMagic", ref tState, name);
        //        tState.transform.localScale = Vector3.one * 150;
        //        tState.GetComponent<Renderer>().sortingOrder = 3;
        //        break;
        //    case EM_Magic.Bottom:
        //        CreateCardState("BottomMagic", ref tState, name);
        //        tState.transform.localScale = Vector3.one * 180;
        //        tState.GetComponent<Renderer>().sortingOrder = 5;
        //        break;
        //    case EM_Magic.Right:
        //        CreateCardState("RightMagic", ref tState, name);
        //        tState.transform.localScale = Vector3.one * 150;
        //        tState.GetComponent<Renderer>().sortingOrder = 3;
        //        break;
        //    case EM_Magic.TopR:
        //        CreateCardState("TopRightMagic", ref tState, name);
        //        tState.transform.localScale = Vector3.one * 120;
        //        tState.GetComponent<Renderer>().sortingOrder = 1;
        //        break;
        //    case EM_Magic.Rebirth:
        //        CreateCardState("RebirthBtn", ref tState, name);
        //        tState.transform.localScale = Vector3.one * 150;
        //        tState.GetComponent<Renderer>().sortingOrder = 1;
        //        break;
        //}
        //tState.transform.localPosition = Vector3.zero;
        //tState.layer = 5;

        //tState.GetComponent<SkeletonAnimation>().AnimationName = "Stand";
        //tState.GetComponent<SkeletonAnimation>().loop = true;
        #endregion

        if (spineModel != null)
        {
            DestroyImmediate(spineModel);
            spineModel = null;
        }
        UITool.f_CreateRoleByCardId(card.m_CardDT.iId, ref spineModel, f_GetObject("RebirthBtn").transform.parent, 10, 100);
        spineModel.transform.localPosition = new Vector3(-30, -300, 0);
        // spineModel.transform.Find(GameParamConst.prefabShadowName).gameObject.SetActive(false);
        f_GetObject("RebirthBtn").SetActive(false);
    }

    void CreateCardState(string objectName, ref GameObject tState, string name)
    {
        f_GetObject(objectName).transform.
                   GetChild(0).GetComponent<UISprite>().enabled = false;
        if(f_GetObject(objectName).transform.GetChild(1).childCount > 0)
            UITool.f_DestoryStatelObject(f_GetObject(objectName).transform.GetChild(1).GetChild(0).gameObject);
        tState.transform.parent = f_GetObject(objectName).transform.GetChild(1);
        f_GetObject(objectName).transform.GetChild(2).GetComponent<UILabel>().text = name;
    }
    /// <summary>
    /// 创建装备模型
    /// </summary>
    void CreateEquip(EquipPoolDT equip, EM_Magic Magic)
    {
        UI2DSprite tSprite = null;
        string name = equip.m_EquipDT.szName;
        string borderName = UITool.f_GetImporentColorName(equip.m_EquipDT.iColour, ref name);
        switch(Magic)
        {
            case EM_Magic.TopL:
                CreateEquip("TopLeftMagic", ref tSprite, name, borderName);
                break;
            case EM_Magic.Left:
                CreateEquip("LeftMagic", ref tSprite, name, borderName);
                break;
            case EM_Magic.Bottom:
                CreateEquip("BottomMagic", ref tSprite, name, borderName);
                break;
            case EM_Magic.Right:
                CreateEquip("RightMagic", ref tSprite, name, borderName);
                break;
            case EM_Magic.TopR:
                CreateEquip("TopRightMagic", ref tSprite, name, borderName);
                break;
            case EM_Magic.Rebirth:
                CreateEquip("RebirthBtn", ref tSprite, name, borderName);
                break;
        }
        tSprite.sprite2D = UITool.f_GetIconSprite(equip.m_EquipDT.iIcon);
        tSprite.MakePixelPerfect();
        tSprite.depth = 3000;
    }

    /// <summary>
    /// 创建装备模型
    /// </summary>
    void CreateEquip(string ObjectName, ref UI2DSprite tSprite, string name, string borderName, int isEquip = -99)
    {
        f_GetObject(ObjectName).transform.
                    GetChild(0).GetComponent<UISprite>().enabled = false;
        if(f_GetObject(ObjectName).transform.
           GetChild(0).GetComponent<UI2DSprite>() == null)
            tSprite = f_GetObject(ObjectName).transform.
                GetChild(0).gameObject.AddComponent<UI2DSprite>();
        else
            tSprite = f_GetObject(ObjectName).transform.
                GetChild(0).gameObject.GetComponent<UI2DSprite>();
        f_GetObject(ObjectName).transform.GetChild(2).GetComponent<UILabel>().text = name;
        f_GetObject(ObjectName).transform.Find("Icon_Border").gameObject.SetActive(true);
        f_GetObject(ObjectName).transform.Find("Icon_Border").GetComponent<UISprite>().spriteName = borderName;
        f_GetObject(ObjectName).transform.Find("Icon_Border").GetComponent<UISprite>().height = (isEquip == -99 ? 120 : 120);
        f_GetObject(ObjectName).transform.Find("Icon_Border").GetComponent<UISprite>().width = (isEquip == -99 ? 120 : 120);
    }
    void RemoReturn()
    {
        for(int i = 0; i < Return_ItemParent.transform.childCount; i++)
        {
            Destroy(Return_ItemParent.transform.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 创建法宝模型
    /// </summary>
    void CreateTreasure(TreasurePoolDT Treasure, EM_Magic Magic)
    {
        UI2DSprite tSprite = null;
        string name = Treasure.m_TreasureDT.szName;
        string borderName = UITool.f_GetImporentColorName(Treasure.m_TreasureDT.iImportant, ref name);
        switch(Magic)
        {
            case EM_Magic.Rebirth:
                f_GetObject("RebirthBtn").transform.
                                    GetChild(0).GetComponent<UISprite>().enabled = false;
                if(f_GetObject("RebirthBtn").transform.
                   GetChild(0).GetComponent<UI2DSprite>() == null)
                    tSprite = f_GetObject("RebirthBtn").transform.
                        GetChild(0).gameObject.AddComponent<UI2DSprite>();
                else
                    tSprite = f_GetObject("RebirthBtn").transform.
                        GetChild(0).gameObject.GetComponent<UI2DSprite>();
                f_GetObject("RebirthBtn").transform.GetChild(2).GetComponent<UILabel>().text = name;
                f_GetObject("RebirthBtn").transform.Find("Icon_Border").gameObject.SetActive(true);
                f_GetObject("RebirthBtn").transform.Find("Icon_Border").GetComponent<UISprite>().spriteName = borderName;
                break;
        }
        tSprite.sprite2D = UITool.f_GetIconSprite(Treasure.m_TreasureDT.iIcon);
        tSprite.MakePixelPerfect();
        tSprite.depth = 3000;
    }
    #region 卡牌UI更新以及按钮事件

    void UI_SeleCardSuc(GameObject go, object obj1, object obj2)
    {
        List<BasePoolDT<long>> tList = (List<BasePoolDT<long>>)obj2;
        switch((EM_RecyleOrRebirth)obj1)
        {
            case EM_RecyleOrRebirth.EquipRecyle:
            case EM_RecyleOrRebirth.EquipRebirth:
                _tEquip.AddRange(tList);
                UI_CreareModleForList(_tEquip);
                break;
            case EM_RecyleOrRebirth.CardRecyle:
            case EM_RecyleOrRebirth.CardRebirth:
                _tCard.AddRange(tList);
                UI_CreareModleForList(_tCard);
                break;
            case EM_RecyleOrRebirth.TreasureRebirth:
                break;
            default:
                break;
        }
        tmpBox.Clear();
    }
    void UI_SeleCardClose(GameObject go, object obj1, object obj2)
    {
        tmpBox.Clear();
        CloseSele();
    }
    void CardRebirth(Transform item, BasePoolDT<long> dt)
    {
        CardPoolDT card = (CardPoolDT)dt;
        UI2DSprite Icon = item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Name = item.Find("Name").GetComponent<UILabel>();
        UILabel Lv = item.Find("Lv").GetComponent<UILabel>();
        UILabel Refine = item.Find("Evolve").GetComponent<UILabel>();
        UISprite Important = item.Find("Important").GetComponent<UISprite>();
        UIToggle tToggle = item.Find("Toggle").GetComponent<UIToggle>();
        UISprite[] StarSprite = item.Find("Star").GetComponentsInChildren<UISprite>();
        item.Find("Star").gameObject.SetActive(card.m_iLvAwaken > 0);
        UITool.f_UpdateStarNum(StarSprite, card.m_iLvAwaken / 10);

        tToggle.value = _tCard.Contains(dt);
        item.Find("Toggle").gameObject.SetActive(tMagic != EM_Magic.Rebirth);
        string tname = card.m_CardDT.szName;
        Important.spriteName = UITool.f_GetImporentColorName(card.m_CardDT.iImportant, ref tname);

        //GameObject SeleBtn = item.Find("SeleBtn").gameObject;
        //f_RegClickEvent(SeleBtn, UI_SeleCardOrEquip, card.iId);
        Lv.text = string.Format("{0}:{1}", "[f1b049]" + CommonTools.f_GetTransLanguage(367) + "[-]", card.m_iLv);
        if(card.m_iEvolveLv > 0)
            Refine.text = string.Format("{0}:{1}", "[f1b049]" + CommonTools.f_GetTransLanguage(369) + "[-]", card.m_iEvolveLv);
        else
            Refine.text = "";
        Icon.sprite2D = UITool.f_GetIconSpriteByCardId(card);
        Name.text = tname;
        item.gameObject.SetActive(true);
    }
    List<BasePoolDT<long>> tmpBox = new List<BasePoolDT<long>>();
    void CardClick(Transform item, BasePoolDT<long> dt)
    {
        if(tType == EM_RecyleOrRebirth.CardRebirth)
        {
            RecycleEquipOrCard[(int)EM_Magic.Rebirth] = dt;
            UI_SeleCardOrEquip(item.gameObject, dt, (int)EM_Magic.Rebirth);
        }
        else
        {
            if((dt as CardPoolDT).m_CardDT.iCardCamp == 0)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(370));
                return;
            }
            UIToggle tToggle = item.Find("Toggle").GetComponent<UIToggle>();
            tToggle.value = !tToggle.value;
            if(tToggle.value)
                tmpBox.Add(dt);
            else
            {
                if(_tCard.Contains(dt))
                    _tCard.Remove(dt);
                tmpBox.Remove(dt);
            }
            if(tmpBox.Count + _tCard.Count >= 5)
            {
                _tCard.AddRange(tmpBox);
                tmpBox.Clear();
                UI_CreareModleForList(_tCard);
            }
        }
    }
    #endregion
    /// <summary>
    /// 根据数组来创建模型
    /// </summary>
    void UI_CreareModleForList(List<BasePoolDT<long>> tList)
    {
        List_ToArray(tList);
        for(int i = 0; i < RecycleEquipOrCard.Length; i++)
        {
            UI_SeleCardOrEquip(null, RecycleEquipOrCard[i], i);
        }
    }

    void List_ToArray(List<BasePoolDT<long>> tList)
    {
        RecycleEquipOrCard = new BasePoolDT<long>[6];
        for(int i = 0; i < RecycleEquipOrCard.Length; i++)
        {
            if(i >= tList.Count)
                break;
            RecycleEquipOrCard[i] = tList[i];
        }
    }
    void EquipRebirth(Transform item, BasePoolDT<long> dt)
    {
        EquipPoolDT equip = dt as EquipPoolDT;
        UI2DSprite Icon = item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Name = item.Find("Name").GetComponent<UILabel>();
        UILabel Lv = item.Find("Lv").GetComponent<UILabel>();
        UILabel Refine = item.Find("Refine").GetComponent<UILabel>();
        UISprite Important = item.Find("Important").GetComponent<UISprite>();
        UISprite[] StarSprite = new UISprite[5];
        if(equip.m_sstars > 0)
        {
            if(equip.m_EquipDT.iColour == (int)EM_Important.Red)
            {
                StarSprite = item.Find("Star5").GetComponentsInChildren<UISprite>();
            }
            else if(equip.m_EquipDT.iColour == (int)EM_Important.Oragen)
            {
                StarSprite = item.Find("Star3").GetComponentsInChildren<UISprite>();
            }
            item.Find("Star5").gameObject.SetActive(equip.m_EquipDT.iColour == (int)EM_Important.Red);
            item.Find("Star3").gameObject.SetActive(equip.m_EquipDT.iColour == (int)EM_Important.Oragen);
            UITool.f_UpdateStarNum(StarSprite, equip.m_sstars);
        }
        else
        {
            item.Find("Star5").gameObject.SetActive(false);
            item.Find("Star3").gameObject.SetActive(false);
        }

        GameObject SeleBtn = item.Find("SeleBtn").gameObject;
        string tname = equip.m_EquipDT.szName;
        Important.spriteName = UITool.f_GetImporentColorName(equip.m_EquipDT.iColour, ref tname);
        UIToggle tToggle = item.Find("Toggle").GetComponent<UIToggle>();
        tToggle.value = _tEquip.Contains(dt);
        item.Find("Toggle").gameObject.SetActive(tMagic != EM_Magic.Rebirth);
        //f_RegClickEvent(SeleBtn, UI_SeleCardOrEquip, equip);
        Lv.text = string.Format("{0}:[f0eccb]{1}", "[f1b049]"+ CommonTools.f_GetTransLanguage(367) + "[-]", equip.m_lvIntensify);
        if(equip.m_lvRefine > 0)
            Refine.text = string.Format("{0}:[f0eccb]{1}", "[f1b049]"+ CommonTools.f_GetTransLanguage(371) + "[-]", equip.m_lvRefine);
        else
            Refine.text = "";
        Icon.sprite2D = UITool.f_GetIconSprite(equip.m_EquipDT.iIcon);
        Name.text = tname;
    }

    void EquipClick(Transform item, BasePoolDT<long> dt)
    {
        if(tType == EM_RecyleOrRebirth.EquipRebirth)
        {
            RecycleEquipOrCard[(int)EM_Magic.Rebirth] = dt;
            UI_SeleCardOrEquip(item.gameObject, dt, (int)EM_Magic.Rebirth);
        }
        else
        {
            UIToggle tToggle = item.Find("Toggle").GetComponent<UIToggle>();
            tToggle.value = !tToggle.value;
            if(tToggle.value)
                tmpBox.Add(dt);
            else
            {
                if(_tEquip.Contains(dt))
                    _tEquip.Remove(dt);
                tmpBox.Remove(dt);
            }
            if(tmpBox.Count + _tEquip.Count >= 5)
            {
                _tEquip.AddRange(tmpBox);
                tmpBox.Clear();
                UI_CreareModleForList(_tEquip);
            }
        }
    }
    void TreasureRebirth(Transform item, BasePoolDT<long> dt)
    {
        TreasurePoolDT tTreasure = dt as TreasurePoolDT;
        UI2DSprite Icon = item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Name = item.Find("Name").GetComponent<UILabel>();
        UILabel Refine = item.Find("Refine").GetComponent<UILabel>();
        UISprite Important = item.Find("Important").GetComponent<UISprite>();
        UILabel Level = item.Find("Level").GetComponent<UILabel>();
        UILabel RefineLevel = item.Find("RefineLevel").GetComponent<UILabel>();

        GameObject SeleBtn = item.Find("SeleBtn").gameObject;
        string tname = tTreasure.m_TreasureDT.szName;
        Important.spriteName = UITool.f_GetImporentColorName(tTreasure.m_TreasureDT.iImportant, ref tname);
        Icon.sprite2D = UITool.f_GetIconSprite(tTreasure.m_TreasureDT.iIcon);
        Name.text = tname;
        Level.text = string.Format("[f1b049]" + CommonTools.f_GetTransLanguage(372), "[f0eccb]" + tTreasure.m_lvIntensify);
        if(tTreasure.m_lvRefine > 0)
            RefineLevel.text = string.Format("[f1b049]" + CommonTools.f_GetTransLanguage(373), "[f0eccb]" + tTreasure.m_lvRefine);
        else
            RefineLevel.text = "";
        Refine.text = "";
    }

    void TreasureClick(Transform item, BasePoolDT<long> dt)
    {
        RecycleEquipOrCard[(int)EM_Magic.Rebirth] = dt;
        UI_SeleCardOrEquip(item.gameObject, dt, (int)EM_Magic.Rebirth);
    }
    /// <summary>
    /// 删除所在位置的模型
    /// </summary>
    void DestroyState()
    {
        ///////////////////////////////卡牌的/////////////////////////////////////////
        if(f_GetObject("TopLeftMagic").transform.GetChild(1).childCount > 0)
            UITool.f_DestoryStatelObject(f_GetObject("TopLeftMagic").transform.GetChild(1).GetChild(0).gameObject);
        if(f_GetObject("LeftMagic").transform.GetChild(1).childCount > 0)
            UITool.f_DestoryStatelObject(f_GetObject("LeftMagic").transform.GetChild(1).GetChild(0).gameObject);
        if(f_GetObject("BottomMagic").transform.GetChild(1).childCount > 0)
            UITool.f_DestoryStatelObject(f_GetObject("BottomMagic").transform.GetChild(1).GetChild(0).gameObject);
        if(f_GetObject("RightMagic").transform.GetChild(1).childCount > 0)
            UITool.f_DestoryStatelObject(f_GetObject("RightMagic").transform.GetChild(1).GetChild(0).gameObject);
        if(f_GetObject("TopRightMagic").transform.GetChild(1).childCount > 0)
            UITool.f_DestoryStatelObject(f_GetObject("TopRightMagic").transform.GetChild(1).GetChild(0).gameObject);
        if(f_GetObject("RebirthBtn").transform.GetChild(1).childCount > 0)
            UITool.f_DestoryStatelObject(f_GetObject("RebirthBtn").transform.GetChild(1).GetChild(0).gameObject);
        /////////////////////////////////装备//////////////////////////////////////////////
        if(f_GetObject("TopLeftMagic").transform.
                    GetChild(0).GetComponent<UI2DSprite>() != null)
        {
            f_GetObject("TopLeftMagic").transform.
                    GetChild(0).GetComponent<UI2DSprite>().sprite2D = null;
            f_GetObject("TopLeftMagic").transform.Find("Icon_Border").gameObject.SetActive(false);
        }
        if(f_GetObject("LeftMagic").transform.
                    GetChild(0).GetComponent<UI2DSprite>() != null)
        {
            f_GetObject("LeftMagic").transform.
                    GetChild(0).GetComponent<UI2DSprite>().sprite2D = null;
            f_GetObject("LeftMagic").transform.Find("Icon_Border").gameObject.SetActive(false);
        }
        if(f_GetObject("BottomMagic").transform.
                    GetChild(0).GetComponent<UI2DSprite>() != null)
        {
            f_GetObject("BottomMagic").transform.
                    GetChild(0).GetComponent<UI2DSprite>().sprite2D = null;
            f_GetObject("BottomMagic").transform.Find("Icon_Border").gameObject.SetActive(false);
        }
        if(f_GetObject("RightMagic").transform.
                    GetChild(0).GetComponent<UI2DSprite>() != null)
        {
            f_GetObject("RightMagic").transform.
                    GetChild(0).GetComponent<UI2DSprite>().sprite2D = null;
            f_GetObject("RightMagic").transform.Find("Icon_Border").gameObject.SetActive(false);
        }
        if(f_GetObject("TopRightMagic").transform.
                    GetChild(0).GetComponent<UI2DSprite>() != null)
        {
            f_GetObject("TopRightMagic").transform.
                    GetChild(0).GetComponent<UI2DSprite>().sprite2D = null;
            f_GetObject("TopRightMagic").transform.Find("Icon_Border").gameObject.SetActive(false);
        }
        if(f_GetObject("RebirthBtn").transform.
                    GetChild(0).GetComponent<UI2DSprite>() != null)
        {
            f_GetObject("RebirthBtn").transform.
                    GetChild(0).GetComponent<UI2DSprite>().sprite2D = null;
            f_GetObject("RebirthBtn").transform.Find("Icon_Border").gameObject.SetActive(false);
        }
        ////////////////////////////////////显示图标////////////////////////////////////////
        f_GetObject("TopLeftMagic").transform.
                GetChild(0).GetComponent<UISprite>().enabled = true;
        f_GetObject("LeftMagic").transform.
                GetChild(0).GetComponent<UISprite>().enabled = true;
        f_GetObject("BottomMagic").transform.
                GetChild(0).GetComponent<UISprite>().enabled = true;
        f_GetObject("RightMagic").transform.
                GetChild(0).GetComponent<UISprite>().enabled = true;
        f_GetObject("TopRightMagic").transform.
                GetChild(0).GetComponent<UISprite>().enabled = true;
        f_GetObject("RebirthBtn").transform.
                GetChild(0).GetComponent<UISprite>().enabled = true;
        ///////////////////////////////////////////////////////////////////
        f_GetObject("TopLeftMagic").transform.GetChild(2).GetComponent<UILabel>().text = "";
        f_GetObject("LeftMagic").transform.GetChild(2).GetComponent<UILabel>().text = "";
        f_GetObject("BottomMagic").transform.GetChild(2).GetComponent<UILabel>().text = "";
        f_GetObject("RightMagic").transform.GetChild(2).GetComponent<UILabel>().text = "";
        f_GetObject("TopRightMagic").transform.GetChild(2).GetComponent<UILabel>().text = "";
        f_GetObject("RebirthBtn").transform.GetChild(2).GetComponent<UILabel>().text = "";
        ////////////////////////////////////////////////////////////////
        f_GetObject("TopLeftClose").SetActive(false);
        f_GetObject("LeftClose").SetActive(false);
        f_GetObject("BottomClose").SetActive(false);
        f_GetObject("RightClose").SetActive(false);
        f_GetObject("TopRightClose").SetActive(false);
        if (spineModel != null)
        {
            DestroyImmediate(spineModel);
            spineModel = null;
        }
        f_GetObject("RebirthBtn").SetActive(true);
    }
    /// <summary>
    /// 删除单个模型
    /// </summary>
    /// <param name="obj1"></param>
    void DeleState(EM_Magic obj1)
    {
        switch((EM_Magic)obj1)
        {
            case EM_Magic.TopL:
                DeleState("TopLeftMagic");
                break;
            case EM_Magic.Left:
                DeleState("LeftMagic");
                break;
            case EM_Magic.Bottom:
                DeleState("BottomMagic");
                break;
            case EM_Magic.Right:
                DeleState("RightMagic");
                break;
            case EM_Magic.TopR:
                DeleState("TopRightMagic");
                break;
        }
    }

    void DeleState(string objectName)
    {
        f_GetObject(objectName).transform.GetChild(2).GetComponent<UILabel>().text = "";
        if(f_GetObject(objectName).transform.
                    GetChild(0).GetComponent<UI2DSprite>() != null)
            f_GetObject(objectName).transform.
                    GetChild(0).GetComponent<UI2DSprite>().sprite2D = null;
        else if(f_GetObject(objectName).transform.GetChild(1).childCount > 0)
            UITool.f_DestoryStatelObject(f_GetObject(objectName).transform.GetChild(1).GetChild(0).gameObject);
        f_GetObject(objectName).transform.GetChild(0).GetComponent<UISprite>().enabled = true;
        f_GetObject(objectName).transform.GetChild(3).gameObject.SetActive(false);
    }

    // <summary>
    /// 数组重新排列  同下
    /// </summary>
    void ListInit()
    {
        f_GetObject("RebirthShowPro").SetActive(false);
        switch(tType)
        {
            case EM_RecyleOrRebirth.None:
                break;
            case EM_RecyleOrRebirth.EquipRecyle:
                Data_Pool.m_EquipPool.f_SortList();
                _EquipListArr = new BasePoolDT<long>[Data_Pool.m_EquipPool.f_GetAll().Count];
                Data_Pool.m_EquipPool.f_GetAll().CopyTo(_EquipListArr);
                _EquipList = new List<BasePoolDT<long>>(_EquipListArr);

                for(int i = 0; i < _EquipList.Count; i++)
                {
                    if(GetIsEquip(_EquipList[i].iId))
                    {
                        _EquipList.RemoveAt(i);
                        i--;
                    }
                }
                _EquipList.Reverse();
                break;
            case EM_RecyleOrRebirth.EquipRebirth:
                Data_Pool.m_EquipPool.f_SortList();
                _EquipListArr = new BasePoolDT<long>[Data_Pool.m_EquipPool.f_GetAll().Count];
                Data_Pool.m_EquipPool.f_GetAll().CopyTo(_EquipListArr);
                _EquipList = new List<BasePoolDT<long>>(_EquipListArr);
                EquipPoolDT tEquipPool;
                for(int i = 0; i < _EquipList.Count; i++)
                {
                    tEquipPool = _EquipList[i] as EquipPoolDT;

                    if(GetIsEquip(_EquipList[i].iId))
                    {
                        _EquipList.RemoveAt(i);
                        i--;
                    }
                    //精炼   升星   升星经验  幸运值   强化等级  精炼经验
                    else if(tEquipPool.m_lvRefine == 0 && tEquipPool.m_sstars == 0
                        && tEquipPool.m_sexpStars == 0 && tEquipPool.m_lvIntensify == 1
                        && tEquipPool.m_iexpRefine == 0)
                    {
                        _EquipList.RemoveAt(i);
                        i--;
                    }
                    else if(tEquipPool.m_EquipDT.iColour < (int)EM_Important.Purple)
                    {
                        _EquipList.RemoveAt(i);
                        i--;
                    }
                }
                _EquipList.Reverse();
                break;
            case EM_RecyleOrRebirth.CardRecyle:
                Data_Pool.m_CardPool.f_SortList();
                _CardListArr = new BasePoolDT<long>[Data_Pool.m_CardPool.f_GetAll().Count];
                Data_Pool.m_CardPool.f_GetAll().CopyTo(_CardListArr);
                _CardList = new List<BasePoolDT<long>>();
                for(int i = 0; i < _CardListArr.Length; i++)
                {
                    if(Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(_CardListArr[i].iId))
                        continue;
                    else
                    {
                        if(Data_Pool.m_TeamPool.dicReinforceCardId.ContainsValue(_CardListArr[i] as CardPoolDT))
                            continue;
                        else
                            _CardList.Add(_CardListArr[i]);
                    }

                }
                _CardList.Reverse();
                break;
            case EM_RecyleOrRebirth.CardRebirth:
                Data_Pool.m_CardPool.f_SortList();
                _CardListArr = new BasePoolDT<long>[Data_Pool.m_CardPool.f_GetAll().Count];
                Data_Pool.m_CardPool.f_GetAll().CopyTo(_CardListArr);
                _CardList = new List<BasePoolDT<long>>();
                CardPoolDT tCardPoolDT;
                for(int i = 0; i < _CardListArr.Length; i++)
                {
                    tCardPoolDT = _CardListArr[i] as CardPoolDT;
                    if((_CardListArr[i] as CardPoolDT).m_CardDT.iCardType == (int)EM_CardType.RoleCard)
                    {
                        if (Data_Pool.m_TeamPool.dicReinforceCardId.ContainsValue(tCardPoolDT))
                            continue;                   //精炼等级    经验    等级  觉醒掩码  觉醒等级  天命经验  天命等级
                        else if (tCardPoolDT.m_iEvolveLv == 0 && tCardPoolDT.m_iFlagAwaken == 0
                            && tCardPoolDT.m_iLvAwaken == 0 && tCardPoolDT.uSkyDestinyExp == 0 && tCardPoolDT.uSkyDestinyLv == 0)
                        {
                            continue;
                        }
                        else
                        {
                            _CardList.Add(_CardListArr[i]);
                        }
                        continue;
                    }

                    if(Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(tCardPoolDT.iId))
                        continue;
                    else
                    {
                        if(Data_Pool.m_TeamPool.dicReinforceCardId.ContainsValue(tCardPoolDT))
                            continue;                   //精炼等级    经验    等级  觉醒掩码  觉醒等级  天命经验  天命等级
                        else if(tCardPoolDT.m_iEvolveLv == 0 && tCardPoolDT.m_iExp == 0 && tCardPoolDT.m_iLv == 1 && tCardPoolDT.m_iFlagAwaken == 0
                            && tCardPoolDT.m_iLvAwaken == 0 && tCardPoolDT.uSkyDestinyExp == 0 && tCardPoolDT.uSkyDestinyLv == 0)
                        {
                            continue;
                        }
                        else
                        {
                            _CardList.Add(_CardListArr[i]);
                        }
                    }

                }
                _CardList.Reverse();
                break;
            case EM_RecyleOrRebirth.TreasureRebirth:
                Data_Pool.m_TreasurePool.f_SortList();
                _TreasureListArr = new BasePoolDT<long>[Data_Pool.m_TreasurePool.f_GetAll().Count];
                Data_Pool.m_TreasurePool.f_GetAll().CopyTo(_TreasureListArr);
                _TreasureList = new List<BasePoolDT<long>>(_TreasureListArr);
                TreasurePoolDT tTreasurePool;
                for(int i = 0; i < _TreasureList.Count; i++)
                {
                    tTreasurePool = _TreasureList[i] as TreasurePoolDT;

                    if(Data_Pool.m_TeamPool.f_CheckTeamTreasure(tTreasurePool.iId))
                    {
                        _TreasureList.RemoveAt(i);
                        i--;
                    }
                    else if(tTreasurePool.m_ExpIntensify == 0 && tTreasurePool.m_lvIntensify == 1
                       && tTreasurePool.m_lvRefine == 0)
                    {
                        _TreasureList.RemoveAt(i);
                        i--;
                    }
                }
                _TreasureList.Reverse();
                break;
            case EM_RecyleOrRebirth.GodEquipRebirth:
                Data_Pool.m_GodEquipPool.f_SortList();
                _GodEquipListArr = new BasePoolDT<long>[Data_Pool.m_GodEquipPool.f_GetAll().Count];
                Data_Pool.m_GodEquipPool.f_GetAll().CopyTo(_GodEquipListArr);
                _GodEquipList = new List<BasePoolDT<long>>(_GodEquipListArr);
                GodEquipPoolDT tGodEquipPool;
                for (int i = 0; i < _GodEquipList.Count; i++)
                {
                    tGodEquipPool = _GodEquipList[i] as GodEquipPoolDT;

                    if (Data_Pool.m_TeamPool.f_CheckTeamGodEquip(tGodEquipPool.iId))
                    {
                        _GodEquipList.RemoveAt(i);
                        i--;
                    }
                    //精炼   升星   升星经验  幸运值   强化等级  精炼经验
                    else if (tGodEquipPool.m_lvRefine == 0 && tGodEquipPool.m_sstars == 0
                        && tGodEquipPool.m_sexpStars == 0 && tGodEquipPool.m_lvIntensify == 1
                        && tGodEquipPool.m_iexpRefine == 0)
                    {
                        _GodEquipList.RemoveAt(i);
                        i--;
                    }
                    else if (tGodEquipPool.m_EquipDT.iColour < (int)EM_Important.Purple)
                    {
                        _GodEquipList.RemoveAt(i);
                        i--;
                    }
                }
                _GodEquipList.Reverse();
                break;
            default:
                break;
        }
        RecycleEquipOrCard = new BasePoolDT<long>[6];
        _tCard = new List<BasePoolDT<long>>();
        _tEquip = new List<BasePoolDT<long>>();
        _Treasure = new List<BasePoolDT<long>>();
    }
    /// <summary>
    /// 查找
    /// </summary>
    BasePoolDT<long> ListFind(List<BasePoolDT<long>> tPool, BasePoolDT<long> dt)
    {
        return dt;
    }
    #region   计算卡牌分解

    /////////////////////////////////卡牌/////////////////////////////////////////////////////////

    /// <summary>
    /// 重生卡牌碎片
    /// </summary>
    int DismantleCard(CardPoolDT tCard)
    {
        int tFragmentNum = 0;
        //主角卡不需要退还碎片
        if(tCard.m_CardDT.iCardType == 1)
            tFragmentNum = 0;
        else
        {   // 进阶退还碎片
            if(tCard.m_iEvolveLv != 0)
            {
                for(int EvolveLv = 1; EvolveLv <= tCard.m_iEvolveLv; EvolveLv++)
                    tFragmentNum += (glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.
                       f_GetSC(tCard.m_CardDT.iId * 100 + EvolveLv) as CardEvolveDT).iNeedCardNum * GetFragmentNum(tCard);
            }
            //领悟退还碎片
            if(tCard.m_iLvAwaken != 0)
            {
                for(int AwakenLv = 1; AwakenLv <= tCard.m_iLvAwaken; AwakenLv++)
                    tFragmentNum += (glo_Main.GetInstance().m_SC_Pool.m_AwakenCardSC.
                    f_GetSC(AwakenLv) as AwakenCardDT).iNeedCard * GetFragmentNum(tCard);
            }
        }
        //卡牌升级共消耗多少银币
        return tFragmentNum;
    }
    /// <summary>
    /// 获取合成卡牌需要多少碎片
    /// </summary>
    int GetFragmentNum(CardPoolDT tCard)
    {
        return (glo_Main.GetInstance().m_SC_Pool.m_CardFragmentSC.
                    f_GetSC(tCard.m_CardDT.iId) as CardFragmentDT).iNeedNum;
    }
    /// <summary>
    /// 计算卡牌升级共消耗多少银币   经验与银币1:1
    /// </summary>
    int GetCardUpNeedGold(CardPoolDT tCard)
    {
        int Gold = tCard.m_iExp;
        for(int lv = 0; lv <= tCard.m_iLv; lv++)
        {
            Gold += CardCount(lv, tCard.m_CardDT.iImportant);
        }
        return Gold;
    }
    /// <summary>
    /// 卡牌升级计算
    /// </summary>
    int CardCount(int lv, int Important)
    {
        if(lv == 0 || lv == 1)
            return 0;
        CarLvUpDT tCardLvUp = glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(lv) as CarLvUpDT;
        switch((EM_Important)Important)
        {
            case EM_Important.White:
                return tCardLvUp.iWhiteCard;
            case EM_Important.Green:
                return tCardLvUp.iGreenCard;
            case EM_Important.Blue:
                return tCardLvUp.iBlueCard;
            case EM_Important.Purple:
                return tCardLvUp.iPurpleCard;
            case EM_Important.Oragen:
                return tCardLvUp.iOragenCard;
            case EM_Important.Red:
                return tCardLvUp.iRedCard;
            //TsuCode - tuong kim
            case EM_Important.Gold:
                return tCardLvUp.iGoldCard;
                //
        }
        return 0;
    }
    /// <summary>
    /// 获取分解返还的道具
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <param name="type">物品类型   EM_RonsonType</param>
    /// <returns></returns>
    int GetDsoul(BasePoolDT<long> id, int type)
    {
        if(id == null)
            return 0;
        int Dsoul = 0;
        switch((EM_ResourceType)type)
        {
            case EM_ResourceType.Card:
                CardPoolDT tCard = id as CardPoolDT;
                if(tCard.m_CardDT.iCardType == 1)
                    return 0;
                else
                {
                    Dsoul += GetCarDsoul(tCard);
                    if(tCard.m_iEvolveId != 0)
                    {
                        //进阶将魂
                        for(int EvolveLv = 1; EvolveLv <= tCard.m_iEvolveLv; EvolveLv++)
                            Dsoul += (glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.
                               f_GetSC(tCard.m_iTempleteId * 100 + EvolveLv) as CardEvolveDT).iNeedCardNum * GetCarDsoul(tCard);
                        ////进阶丹
                        //tInt[(int)EM_ResocnType.EvolePill] += (glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.
                        //   f_GetSC(tCard.m_CardDT.iId * 100 + tCard.m_iEvolveLv) as CardEvolveDT).iEvolvePill;
                    }
                    //领悟将魂
                    if(tCard.m_iLvAwaken != 0)
                    {
                        for(int AwkenLv = 1; AwkenLv <= tCard.m_iLvAwaken; AwkenLv++)
                            Dsoul += (glo_Main.GetInstance().m_SC_Pool.m_AwakenCardSC.
                                f_GetSC(AwkenLv) as AwakenCardDT).iNeedCard * GetCarDsoul(tCard);
                    }
                }
                break;
            case EM_ResourceType.Equip:
                EquipPoolDT tEquip = ListFind(_tEquip, id) as EquipPoolDT;
                Dsoul += GetEquipDsoul(tEquip);
                break;
        }
        return Dsoul;
    }
    /// <summary>
    /// 获取卡牌进阶丹数量   不需要返还银币
    /// </summary>
    int GetCardEvolvePillNum(CardPoolDT tCard)
    {
        int CardEvolvePillNum = 0;
        for(int EvolveLv = 1; EvolveLv <= tCard.m_iEvolveLv; EvolveLv++)
            CardEvolvePillNum += (glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.
                f_GetSC(tCard.m_CardDT.iId * 100 + EvolveLv) as CardEvolveDT).iEvolvePill;
        //Gold += (glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.
        //    f_GetSC(tCard.m_CardDT.iId * 100 + tCard.m_iEvolveLv) as CardEvolveDT).iMoney;
        return CardEvolvePillNum;
    }
    /// <summary>
    /// 获得领悟丹数量
    /// </summary>
    /// <param name="id"></param>
    /// <param name="Gold">获得银币</param>
    /// <param name="FragmentNum">碎片数量</param>
    /// <returns></returns>
    int GetAwakenPillNum(CardPoolDT tCard, ref long Gold)
    {
        int AwakenPillNum = 0;
        for(int AwakenLv = 1; AwakenLv <= tCard.m_iLvAwaken; AwakenLv++)
        {
            AwakenPillNum += (glo_Main.GetInstance().m_SC_Pool.m_AwakenCardSC.
                f_GetSC(AwakenLv) as AwakenCardDT).iNeedGoods;
            Gold += (glo_Main.GetInstance().m_SC_Pool.m_AwakenCardSC.
                f_GetSC(AwakenLv) as AwakenCardDT).iNeedMoeny;
        }
        return AwakenPillNum;
    }
    /// <summary>
    /// 获得领悟道具数量
    /// </summary>
    /// <param name="tCard"></param>
    /// <returns></returns>
    List<AwakenEquipPoolDT> GetAwakenGoodsNum(CardPoolDT tCard)
    {
        List<AwakenEquipPoolDT> tList = new List<AwakenEquipPoolDT>();
        if(tCard.m_iLvAwaken == 0)
            return tList;
        for(int i = 1; i <= tCard.m_iLvAwaken + 1; i++)
        {
            AwakenCardDT teuqip = glo_Main.GetInstance().m_SC_Pool.m_AwakenCardSC.f_GetSC(i) as AwakenCardDT;
            if(tCard.m_iLvAwaken < i)
            {
                string tFlag = tCard.m_iFlagAwaken.ToString();
                switch(tFlag.Length)
                {
                    case 1:
                        if(tFlag != "0")
                            AwakenEquipPoolAdd(ref tList, teuqip, 4);
                        break;
                    case 2:
                        for(int site = 0; site < tFlag.Length; site++)
                        {
                            if(tFlag[site] == '1')
                                AwakenEquipPoolAdd(ref tList, teuqip, 3 + site);
                        }
                        break;
                    case 3:
                        for(int site = 0; site < tFlag.Length; site++)
                        {
                            if(tFlag[site] == '1')
                                AwakenEquipPoolAdd(ref tList, teuqip, 2 + site);
                        }
                        break;
                    case 4:
                        for(int site = 0; site < tFlag.Length; site++)
                        {
                            if(tFlag[site] == '1')
                                AwakenEquipPoolAdd(ref tList, teuqip, 1 + site);
                        }
                        break;
                }
                break;
            }
            if(teuqip.iEquipID1 != 0)
                AwakenEquipPoolAdd(ref tList, teuqip, 1);
            if(teuqip.iEquipID2 != 0)
                AwakenEquipPoolAdd(ref tList, teuqip, 2);
            if(teuqip.iEquipID3 != 0)
                AwakenEquipPoolAdd(ref tList, teuqip, 3);
            if(teuqip.iEquipID4 != 0)
                AwakenEquipPoolAdd(ref tList, teuqip, 4);
        }
        return CountAwakenGoodsNum(tList);
    }
    /// <summary>
    /// 计算领悟道具的数量
    /// </summary>
    List<AwakenEquipPoolDT> CountAwakenGoodsNum(List<AwakenEquipPoolDT> tList)
    {
        if(tList.Count == 0)
            return tList;
        tList.Sort((AwakenEquipPoolDT a, AwakenEquipPoolDT b) =>
        {
            return a.m_iTempleteId > b.m_iTempleteId ? -1 : 1;
        });
        int tEquipId = tList[0].m_iTempleteId;
        int index = 0;
        for(int i = 1; i < tList.Count; i++)
        {
            if(tEquipId != tList[i].m_iTempleteId)
            {
                tEquipId = tList[i].m_iTempleteId;
                index++;
            }
            else if(tEquipId == tList[i].m_iTempleteId)
            {
                tList[index].m_num += tList[i].m_num;
                if(index != i)
                {
                    tList.RemoveAt(i);
                    i--;
                }
            }
        }
        return tList;
    }

    List<BaseGoodsPoolDT> CountExpGoodsNum(List<BaseGoodsPoolDT> tList)
    {
        if(tList.Count == 0)
            return tList;
        tList.Sort((BaseGoodsPoolDT a, BaseGoodsPoolDT b) =>
        {
            return a.m_iTempleteId > b.m_iTempleteId ? -1 : 1;
        });
        int tEquipId = tList[0].m_iTempleteId;
        int index = 0;
        for(int i = 1; i < tList.Count; i++)
        {
            if(tEquipId != tList[i].m_iTempleteId)
            {
                tEquipId = tList[i].m_iTempleteId;
                index++;
            }
            else if(tEquipId == tList[i].m_iTempleteId)
            {
                tList[index].m_iNum += tList[i].m_iNum;
                if(index != i)
                {
                    tList.RemoveAt(i);
                    i--;
                }
            }
        }
        return tList;
    }

    void AwakenEquipPoolAdd(ref List<AwakenEquipPoolDT> tlist, AwakenCardDT tAwaken, int Index)
    {
        AwakenEquipPoolDT tAwakenEquip = new AwakenEquipPoolDT();
        switch(Index)
        {
            case 1:
                tAwakenEquip.m_iTempleteId = tAwaken.iEquipID1;
                break;
            case 2:
                tAwakenEquip.m_iTempleteId = tAwaken.iEquipID2;
                break;
            case 3:
                tAwakenEquip.m_iTempleteId = tAwaken.iEquipID3;
                break;
            case 4:
                tAwakenEquip.m_iTempleteId = tAwaken.iEquipID4;
                break;
        }
        tAwakenEquip.m_num = 1;
        tlist.Add(tAwakenEquip);
    }
    /// <summary>
    /// 获取经验石数量   
    /// </summary>
    /// <param name="tCard"></param>
    /// <returns></returns>
    List<BaseGoodsPoolDT> GetExpPill(CardPoolDT tCard, ref long Gold)
    {
        List<BaseGoodsPoolDT> tList = new List<BaseGoodsPoolDT>();
        if(tCard.m_CardDT.iCardType == 1)
            return tList;
        Gold += GetCardUpNeedGold(tCard);
        int Exp = GetCardUpNeedGold(tCard);
        int[] tNum = new int[3];  //0  高级   1  中级  2  低级
        int[] tPill = { 20000, 10000, 5000 };
        int[] tPillId = { 129, 128, 127 };
        for(int i = 0; i < 3; i++)
        {
            if(Exp < 5000)
                break;
            tNum[i] = Exp / tPill[i];
            Exp -= tNum[i] * tPill[i];
            BaseGoodsPoolDT tBaseGoods = new BaseGoodsPoolDT();
            tBaseGoods.m_iTempleteId = tPillId[i];
            tBaseGoods.m_iNum = tNum[i];
            tList.Add(tBaseGoods);
        }
        return tList;
    }
    /// <summary>
    /// 获得將魂
    /// </summary>
    int GetCarDsoul(CardPoolDT tCard)
    {
        switch((EM_Important)tCard.m_CardDT.iImportant)
        {
            case EM_Important.Green:
                return 10;
            case EM_Important.Blue:
                return 40;
            case EM_Important.Purple:
                return 400;
            case EM_Important.Oragen:
                return 1000;
            case EM_Important.Red:
                return 2000;
        }
        return 0;
    }


    bool GetIsTeam(long id)
    {
        for(int i = 0; i < Data_Pool.m_TeamPool.f_GetAll().Count; i++)
        {
            if((Data_Pool.m_TeamPool.f_GetAll()[i] as TeamPoolDT).m_iCardId == id)
                return true;
        }
        return false;
    }


    int GetArtifact(CardPoolDT tCard, out int RefineGoodsNum)
    {
        RefineGoodsNum = 0;
        int GoodsNum = 0;
        ArtifactDT tArtifactDT;
        for(int i = 0; i < tCard.m_ArtifactPoolDT.Lv; i++)
        {
            //GoodsNum += tCard.m_ArtifactPoolDT.Template.iItemNum;
            tArtifactDT = (ArtifactDT)glo_Main.GetInstance().m_SC_Pool.m_ArtifactSC.f_GetSC(i + 1);
            if(tArtifactDT.iItemId == 107)
                GoodsNum += tArtifactDT.iItemNum;
            if(tArtifactDT.iItemId == 130)
                RefineGoodsNum += tArtifactDT.iItemNum;
        }
        return GoodsNum;
    }

    #endregion

    #region  计算装备分解
    /////////////////////////////////装备/////////////////////////////////////////
    /// <summary>
    /// 获得装备的精炼石
    /// </summary>
    /// <param name="tEquip"></param>
    /// <returns></returns>
    List<BaseGoodsPoolDT> GetEquipRefinePill(EquipPoolDT tEquip)
    {
        List<BaseGoodsPoolDT> tBasePool = new List<BaseGoodsPoolDT>();
        int[] RefinePill = UITool.f_GetGoodsForEffect(EM_GoodsEffect.EquipRefineExp);
        int EquipRefineExp = tEquip.m_iexpRefine;
        for(int lv = 1; lv <= tEquip.m_lvRefine; lv++)
        {
            EquipRefineExp += GetEquipRefineExp(lv, tEquip.m_EquipDT.iColour);
        }

        for(int i = 0; i < RefinePill.Length; i++)
        {
            BaseGoodsPoolDT tBaseGoods = new BaseGoodsPoolDT();
            tBaseGoods.m_iTempleteId = RefinePill[RefinePill.Length - i - 1];
            tBasePool.Add(tBaseGoods);

            if(EquipRefineExp >= tBaseGoods.m_BaseGoodsDT.iEffectData)
            {
                tBaseGoods.m_iNum = EquipRefineExp / tBaseGoods.m_BaseGoodsDT.iEffectData;
                EquipRefineExp -= tBaseGoods.m_iNum * tBaseGoods.m_BaseGoodsDT.iEffectData;
            }
            if(EquipRefineExp == 0)
                break;
        }

        return tBasePool;
    }
    /// <summary>
    /// 获得装备全部的精炼经验
    /// </summary>
    /// <param name="lv"></param>
    /// <param name="Important"></param>
    /// <returns></returns>
    int GetEquipRefineExp(int lv, int Important)
    {
        EquipConsumeDT tConsumDt;
        switch((EM_Important)Important)
        {
            case EM_Important.White:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_EquipConsumeSC.f_GetSC(1000 + lv) as EquipConsumeDT;
                return tConsumDt.iRefineExp;
            case EM_Important.Green:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_EquipConsumeSC.f_GetSC(2000 + lv) as EquipConsumeDT;
                return tConsumDt.iRefineExp;
            case EM_Important.Blue:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_EquipConsumeSC.f_GetSC(3000 + lv) as EquipConsumeDT;
                return tConsumDt.iRefineExp;
            case EM_Important.Purple:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_EquipConsumeSC.f_GetSC(4000 + lv) as EquipConsumeDT;
                return tConsumDt.iRefineExp;
            case EM_Important.Oragen:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_EquipConsumeSC.f_GetSC(5000 + lv) as EquipConsumeDT;
                return tConsumDt.iRefineExp;
            case EM_Important.Red:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_EquipConsumeSC.f_GetSC(6000 + lv) as EquipConsumeDT;
                return tConsumDt.iRefineExp;
        }
        return 0;
    }
    /// <summary>
    /// 获得装备强化所需金钱
    /// </summary>
    /// <returns></returns>
    //int GetEquipIntenGold(EquipPoolDT tEquip)
    //{
    //    int Gold = 0;
    //    for (int lv = 1; lv <= tEquip.m_lvIntensify; lv++)
    //    {
    //        Gold += GetEquipGold(lv, tEquip.m_EquipDT.iColour);
    //    }
    //    return Gold;
    //}
    /// <summary>
    /// 装备强化的消耗银币
    /// </summary>
    int GetEquipGold(int lv, int Important)
    {
        EquipIntensifyConsumeDT tEquipConsume = glo_Main.GetInstance().m_SC_Pool.
            m_EquipIntensifyConsumeSC.f_GetSC(lv) as EquipIntensifyConsumeDT;
        switch((EM_Important)Important)
        {
            case EM_Important.Green:
                return tEquipConsume.iGreen;
            case EM_Important.Blue:
                return tEquipConsume.iBule;
            case EM_Important.Purple:
                return tEquipConsume.iViolet;
            case EM_Important.Oragen:
                return tEquipConsume.iOrange;
            case EM_Important.Red:
                return tEquipConsume.iRed;
        }
        return 0;
    }
    /// <summary>
    /// 获得装备威名
    /// </summary>
    /// <param name="tEquip"></param>
    /// <returns></returns>
    int GetEquipDsoul(EquipPoolDT tEquip)
    {
        switch((EM_Important)tEquip.m_EquipDT.iColour)
        {
            case EM_Important.Green:
                return 80;
            case EM_Important.Blue:
                return 1000;
            case EM_Important.Purple:
                return 5000;
            case EM_Important.Oragen:
                return 20000;
            case EM_Important.Red:
                return 30000;
        }
        return 0;
    }
    /// <summary>
    /// 获得装备碎片
    /// </summary>
    /// <param name="tEquip"></param>
    /// <returns></returns>
    int GetEquipFraments(EquipPoolDT tEquip)
    {
        return (glo_Main.GetInstance().m_SC_Pool.m_EquipFragmentsSC.
            f_GetSC(tEquip.m_EquipDT.iId + 1000) as EquipFragmentsDT).iBondNum;
    }

    /// <summary>
    /// 检查装备是否在身上
    /// </summary>
    bool GetIsEquip(long id)
    {
        for(int i = 0; i < Data_Pool.m_TeamPool.f_GetAll().Count; i++)
        {
            for(int j = 0; j < (Data_Pool.m_TeamPool.f_GetAll()[i] as TeamPoolDT).m_aEqupId.Length; j++)
            {
                if((Data_Pool.m_TeamPool.f_GetAll()[i] as TeamPoolDT).m_aEqupId[j] == id)
                    return true;
            }
        }
        return false;
    }

    #endregion

    #region  计算法宝分解
    ///////////////////////////////////法宝///////////////////////////////////////
    /// <summary>
    /// 获得法宝强化道具数量  
    /// </summary>
    /// <returns></returns>
    List<TreasurePoolDT> GetTreasureUpExp(TreasurePoolDT tTreasurePoolDT, ref long Gold)
    {
        List<TreasurePoolDT> tTreasure = new List<TreasurePoolDT>();
        int IntenExp = tTreasurePoolDT.m_ExpIntensify;
        for(int lv = 1; lv <= tTreasurePoolDT.m_lvIntensify; lv++)
        {
            IntenExp += GetTreasureExp(lv, tTreasurePoolDT.m_TreasureDT.iImportant);
        }
        Gold = IntenExp;
        ///获得经验道具
        List<NBaseSCDT> tList = glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetAll();
        for(int i = 0; i < tList.Count; i++)
        {
            if((tList[i] as TreasureDT).iSite == (int)EM_EquipPart.eEquipPart_INVALID)
            {
                TreasurePoolDT trea = new TreasurePoolDT();
                trea.m_iTempleteId = tList[i].iId;
                tTreasure.Add(trea);
            }
        }
        tTreasure.Sort((TreasurePoolDT a, TreasurePoolDT b) =>
        {
            return a.m_TreasureDT.iExp > b.m_TreasureDT.iExp ? -1 : 1;
        });

        ///经验法宝数量

        for(int i = 0; i < tTreasure.Count; i++)
        {
            if(IntenExp >= tTreasure[i].m_TreasureDT.iExp)
            {
                tTreasure[i].m_Num = IntenExp / tTreasure[i].m_TreasureDT.iExp;
                IntenExp -= tTreasure[i].m_Num * tTreasure[i].m_TreasureDT.iExp;
            }
        }
        return tTreasure;
    }

    /// <summary>
    /// 获得法宝强化经验   银币经验1:1
    /// </summary>
    /// <param name="lv"></param>
    /// <param name="Important"></param>
    /// <returns></returns>
    int GetTreasureExp(int lv, int Important)
    {
        TreasureUpConsumeDT tTreasure = null;
        switch((EM_Important)Important)
        {
            case EM_Important.White:
                break;
            case EM_Important.Green:
                break;
            case EM_Important.Blue:
                tTreasure = glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetSC(3 * 1000 + lv) as TreasureUpConsumeDT;
                break;
            case EM_Important.Purple:
                tTreasure = glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetSC(4 * 1000 + lv) as TreasureUpConsumeDT;
                break;
            case EM_Important.Oragen:
                tTreasure = glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetSC(5 * 1000 + lv) as TreasureUpConsumeDT;
                break;
            case EM_Important.Red:
                tTreasure = glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetSC(6 * 1000 + lv) as TreasureUpConsumeDT;
                break;
            case EM_Important.Gold:
                tTreasure = glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetSC(7 * 1000 + lv) as TreasureUpConsumeDT;
                break;
        }
        return tTreasure.iIntensifyExp;
    }
    /// <summary>
    /// 获得法宝精炼道具
    /// </summary>
    /// <param name="lv">精炼等级</param>
    /// <param name="id">模版ID</param>
    /// <param name="TreasureNum">法宝数量</param>
    /// <param name="Gold">法宝消耗银币</param>
    /// <param name="PillNum">法宝消耗丹</param>
    void CountTreasureRefineExp(int lv, int id, ref int TreasureNum, ref long Gold, ref int PillNum)
    {
        TreasureRefineConsumeDT tRefine = glo_Main.GetInstance().m_SC_Pool.
            m_TreasureRefineConsumeSC.f_GetSC(id * 100 + lv) as TreasureRefineConsumeDT;
        TreasureNum += tRefine.iTreasureNum;
        Gold += tRefine.iGoldNum;
        PillNum += tRefine.iRefineNum;
    }

    void GetTreasureRefineExp(TreasurePoolDT Treasure, ref int TreasureNum, ref long Gold, ref int PillNum)
    {
        for(int lv = 1; lv <= Treasure.m_lvRefine; lv++)
        {
            CountTreasureRefineExp(lv, Treasure.m_iTempleteId, ref TreasureNum, ref Gold, ref PillNum);
        }
    }
    #endregion
    #endregion
    /// <summary>
    /// 回收方位
    /// </summary>
    enum EM_Magic
    {
        Bottom,//下
        Left,  //左 
        TopL,//上左 
        TopR,  //上右
        Right, //右
        Rebirth,//重生
    }

    void UI_GodEquipRebirthBtn(GameObject go, object obj1, object obj2)
    {
        tType = EM_RecyleOrRebirth.GodEquipRebirth;
        tMagic = EM_Magic.Rebirth;
        _tEquip.Clear();

        ListInit();
        DestroyState();
        RecycleEquipOrCard = new BasePoolDT<long>[6];
        if (recycleSpine)
            recycleSpine.SetActive(false);
    }

    void CreateGodEquip(GodEquipPoolDT GodEquip, EM_Magic Magic)
    {
        UI2DSprite tSprite = null;
        string name = GodEquip.m_EquipDT.szName;
        string borderName = UITool.f_GetImporentColorName(GodEquip.m_EquipDT.iColour, ref name);
        switch (Magic)
        {
            case EM_Magic.Rebirth:
                f_GetObject("RebirthBtn").transform.
                                    GetChild(0).GetComponent<UISprite>().enabled = false;
                if (f_GetObject("RebirthBtn").transform.
                   GetChild(0).GetComponent<UI2DSprite>() == null)
                    tSprite = f_GetObject("RebirthBtn").transform.
                        GetChild(0).gameObject.AddComponent<UI2DSprite>();
                else
                    tSprite = f_GetObject("RebirthBtn").transform.
                        GetChild(0).gameObject.GetComponent<UI2DSprite>();
                f_GetObject("RebirthBtn").transform.GetChild(2).GetComponent<UILabel>().text = name;
                f_GetObject("RebirthBtn").transform.Find("Icon_Border").gameObject.SetActive(true);
                f_GetObject("RebirthBtn").transform.Find("Icon_Border").GetComponent<UISprite>().spriteName = borderName;

                break;
        }
        tSprite.sprite2D = UITool.f_GetIconSprite(GodEquip.m_EquipDT.iIcon);
        tSprite.MakePixelPerfect();
        tSprite.depth = 3600;
    }

    void GodEquipRebirth(Transform item, BasePoolDT<long> dt)
    {
        GodEquipPoolDT equip = dt as GodEquipPoolDT;
        UI2DSprite Icon = item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Name = item.Find("Name").GetComponent<UILabel>();
        UILabel Lv = item.Find("Lv").GetComponent<UILabel>();
        UILabel Refine = item.Find("RefineLevel").GetComponent<UILabel>();
        UISprite Important = item.Find("Important").GetComponent<UISprite>();
        UISprite[] StarSprite = new UISprite[5];
        if (equip.m_sstars > 0)
        {
            if (equip.m_EquipDT.iColour >= (int)EM_Important.Red)
            {
                StarSprite = item.Find("Star5").GetComponentsInChildren<UISprite>();
            }
            else if (equip.m_EquipDT.iColour == (int)EM_Important.Oragen)
            {
                StarSprite = item.Find("Star3").GetComponentsInChildren<UISprite>();
            }
            item.Find("Star5").gameObject.SetActive(equip.m_EquipDT.iColour >= (int)EM_Important.Red);
            item.Find("Star3").gameObject.SetActive(equip.m_EquipDT.iColour == (int)EM_Important.Oragen);
            UITool.f_UpdateStarNum(StarSprite, equip.m_sstars);
        }
        else
        {
            item.Find("Star5").gameObject.SetActive(false);
            item.Find("Star3").gameObject.SetActive(false);
        }

        GameObject SeleBtn = item.Find("SeleBtn").gameObject;
        string tname = equip.m_EquipDT.szName;
        Important.spriteName = UITool.f_GetImporentColorName(equip.m_EquipDT.iColour, ref tname);
        UIToggle tToggle = item.Find("Toggle").GetComponent<UIToggle>();
        tToggle.value = _tEquip.Contains(dt);
        item.Find("Toggle").gameObject.SetActive(tMagic != EM_Magic.Rebirth);
        //f_RegClickEvent(SeleBtn, UI_SeleCardOrEquip, equip);
        Lv.text = string.Format("{0}:[f0eccb]{1}", "[f1b049]" + CommonTools.f_GetTransLanguage(367) + "[-]", equip.m_lvIntensify);
        if (equip.m_lvRefine > 0)
            Refine.text = string.Format("{0}:[f0eccb]{1}", "[f1b049]" + CommonTools.f_GetTransLanguage(371) + "[-]", equip.m_lvRefine);
        else
            Refine.text = "";
        Icon.sprite2D = UITool.f_GetIconSprite(equip.m_EquipDT.iIcon);
        Name.text = tname;
    }

    void GodEquipClick(Transform item, BasePoolDT<long> dt)
    {
        RecycleEquipOrCard[(int)EM_Magic.Rebirth] = dt;
        UI_SeleCardOrEquip(item.gameObject, dt, (int)EM_Magic.Rebirth);
    }

    void GodEquipRebirth()
    {
        if (RecycleEquipOrCard[(int)EM_Magic.Rebirth] != null)
        {
            equipHistory = 1;
            GodEquipPoolDT tEquip = ListFind(_tEquip, RecycleEquipOrCard[(int)EM_Magic.Rebirth]) as GodEquipPoolDT;
            int EquipFragment = GetGodEquipFraments(tEquip);    //碎片
            List<BaseGoodsPoolDT> RefinePill = GetGodEquipRefinePill(tEquip);
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_EquipPool.f_GetEquipCostHistory(RecycleEquipOrCard[(int)EM_Magic.Rebirth].iId, EquipHistory, new SocketCallbackDT());

            if (RefinePill.Count > 0)
            {
                for (int indedx = 0; indedx < RefinePill.Count; indedx++)
                {
                    if (RefinePill[indedx].m_iNum > 0)
                        CreateReturn(EM_ResourceType.Good, RefinePill[indedx].m_BaseGoodsDT.iId, RefinePill[indedx].m_iNum);
                }
            }
        }
    }

    int GetGodEquipFraments(GodEquipPoolDT tEquip)
    {
        return (glo_Main.GetInstance().m_SC_Pool.m_GodEquipFragmentsSC.
            f_GetSC(tEquip.m_EquipDT.iId + 1000) as GodEquipFragmentsDT).iBondNum;
    }

    List<BaseGoodsPoolDT> GetGodEquipRefinePill(GodEquipPoolDT tEquip)
    {
        List<BaseGoodsPoolDT> tBasePool = new List<BaseGoodsPoolDT>();
        int[] RefinePill = UITool.f_GetGoodsForEffect(EM_GoodsEffect.EquipRefineExp);
        int EquipRefineExp = tEquip.m_iexpRefine;
        for (int lv = 1; lv <= tEquip.m_lvRefine; lv++)
        {
            EquipRefineExp += GetGodEquipRefineExp(lv, tEquip.m_EquipDT.iColour);
        }

        for (int i = 0; i < RefinePill.Length; i++)
        {
            BaseGoodsPoolDT tBaseGoods = new BaseGoodsPoolDT();
            tBaseGoods.m_iTempleteId = RefinePill[RefinePill.Length - i - 1];
            tBasePool.Add(tBaseGoods);

            if (EquipRefineExp >= tBaseGoods.m_BaseGoodsDT.iEffectData)
            {
                tBaseGoods.m_iNum = EquipRefineExp / tBaseGoods.m_BaseGoodsDT.iEffectData;
                EquipRefineExp -= tBaseGoods.m_iNum * tBaseGoods.m_BaseGoodsDT.iEffectData;
            }
            if (EquipRefineExp == 0)
                break;
        }

        return tBasePool;
    }

    int GetGodEquipRefineExp(int lv, int Important)
    {
        GodEquipConsumeDT tConsumDt;
        switch ((EM_Important)Important)
        {
            case EM_Important.White:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_GodEquipConsumeSC.f_GetSC(1000 + lv) as GodEquipConsumeDT;
                return tConsumDt.iRefineExp;
            case EM_Important.Green:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_GodEquipConsumeSC.f_GetSC(2000 + lv) as GodEquipConsumeDT;
                return tConsumDt.iRefineExp;
            case EM_Important.Blue:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_GodEquipConsumeSC.f_GetSC(3000 + lv) as GodEquipConsumeDT;
                return tConsumDt.iRefineExp;
            case EM_Important.Purple:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_GodEquipConsumeSC.f_GetSC(4000 + lv) as GodEquipConsumeDT;
                return tConsumDt.iRefineExp;
            case EM_Important.Oragen:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_GodEquipConsumeSC.f_GetSC(5000 + lv) as GodEquipConsumeDT;
                return tConsumDt.iRefineExp;
            case EM_Important.Red:
                tConsumDt = glo_Main.GetInstance().m_SC_Pool.m_GodEquipConsumeSC.f_GetSC(6000 + lv) as GodEquipConsumeDT;
                return tConsumDt.iRefineExp;
        }
        return 0;
    }
}
