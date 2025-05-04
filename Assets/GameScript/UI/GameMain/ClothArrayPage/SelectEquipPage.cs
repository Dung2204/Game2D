using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 更换法宝或者更换装备界面
/// </summary>
public class SelectEquipPage : UIFramwork
{
    private UIWrapComponent EquipWrapComponent = null;//装备拖动组件 
    private UIWrapComponent TreasureWrapComponent = null;//法宝拖动组件 
    private UIWrapComponent GodEquipWrapComponent = null;// 
    private List<BasePoolDT<long>> _EquipList = new List<BasePoolDT<long>>();//可以装备的列表（注：如果为更换法宝则为法宝列表）
    private static EM_FormationPos needReplaceCardPos;
    private static EM_EquipPart needReplacePos;//需要装备的位置
    private SocketCallbackDT OnChangeEquipCallback = new SocketCallbackDT();//更换装备回调
    private SocketCallbackDT OnChangeGodEquipCallback = new SocketCallbackDT();//更换装备回调

    /// <summary>
    /// 检测是否还有可装备的剩余
    /// </summary>
    /// <returns></returns>
    private List<BasePoolDT<long>> CheckHasEquipLeft(EM_EquipPart equipPart)
    {
        //如果是左右法宝跟普通装备表不一样，表不一样得换
        if (equipPart == EM_EquipPart.eEquipPare_MagicLeft || equipPart == EM_EquipPart.eEquipPare_MagicRight)
        {
            Data_Pool.m_TreasurePool.f_SortList();//排序
            List<BasePoolDT<long>> allUserEquip = Data_Pool.m_TreasurePool.f_GetAll();
            _EquipList.Clear();
            for (int i = 0; i < allUserEquip.Count; i++)
            {
                TreasurePoolDT equipPoolDT = allUserEquip[i] as TreasurePoolDT;
                //屏蔽非同类型的装备（如衣服不能穿在武器栏上）
                if (equipPoolDT.m_TreasureDT.iSite == (int)equipPart && !UITool.f_CheckIsWear(equipPoolDT.iId))
                    _EquipList.Add(allUserEquip[i]);
            }
            //排序2（可增加缘分的往前，缘分相同的看品质）
            int ChangeIndex = 0;
            for (int i = 0; i < _EquipList.Count; i++)
            {
                TreasurePoolDT poolDT = _EquipList[i] as TreasurePoolDT;
                int fateCount = CheckFateCount(poolDT.m_TreasureDT.iId);
                if (fateCount > 0)
                {
                    _EquipList.Remove(poolDT);
                    _EquipList.Insert(ChangeIndex, poolDT);
                    ChangeIndex++;
                }
            }
            return _EquipList;
        }
        else if (equipPart == EM_EquipPart.eEquipPart_GodWeapon)
        {
            Data_Pool.m_GodEquipPool.f_SortList();//排序
            List<BasePoolDT<long>> allUserEquip = Data_Pool.m_GodEquipPool.f_GetAll();
            _EquipList.Clear();
            for (int i = 0; i < allUserEquip.Count; i++)
            {
                GodEquipPoolDT equipPoolDT = allUserEquip[i] as GodEquipPoolDT;
                //Chặn các loại thiết bị khác nhau
                if (equipPoolDT.m_EquipDT.iSite == (int)equipPart && !UITool.f_CheckIsWear(equipPoolDT.iId))
                    _EquipList.Add(allUserEquip[i]);
            }
            int ChangeIndex = 0;
            for (int i = 0; i < _EquipList.Count; i++)
            {
                GodEquipPoolDT poolDT = _EquipList[i] as GodEquipPoolDT;
                int fateCount = CheckFateCount(poolDT.m_EquipDT.iId);
                if (fateCount > 0)
                {
                    _EquipList.Remove(poolDT);
                    _EquipList.Insert(ChangeIndex, poolDT);
                    ChangeIndex++;
                }
            }
            return _EquipList;
        }
        else
        {
            Data_Pool.m_EquipPool.f_SortList();//排序
            List<BasePoolDT<long>> allUserEquip = Data_Pool.m_EquipPool.f_GetAll();
            _EquipList.Clear();
            for (int i = 0; i < allUserEquip.Count; i++)
            {
                EquipPoolDT equipPoolDT = allUserEquip[i] as EquipPoolDT;
                //屏蔽非同类型的装备（如衣服不能穿在武器栏上）
                if (equipPoolDT.m_EquipDT.iSite == (int)equipPart && !UITool.f_CheckIsWear(equipPoolDT.iId))
                    _EquipList.Add(allUserEquip[i]);
            }
            //排序2（可增加缘分的往前，缘分相同的看品质）
            int ChangeIndex = 0;
            for (int i = 0; i < _EquipList.Count; i++)
            {
                EquipPoolDT poolDT = _EquipList[i] as EquipPoolDT;
                int fateCount = CheckFateCount(poolDT.m_EquipDT.iId);
                if (fateCount > 0)
                {
                    _EquipList.Remove(poolDT);
                    _EquipList.Insert(ChangeIndex, poolDT);
                    ChangeIndex++;
                }
            }
            return _EquipList;
        }
    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        needReplaceCardPos = LineUpPage.CurrentSelectCardPos;
        needReplacePos = LineUpPage.CurrentSelectEquipPart;
        CheckHasEquipLeft(needReplacePos);
       // bool isTresure = needReplacePos == EM_EquipPart.eEquipPare_MagicLeft || needReplacePos == EM_EquipPart.eEquipPare_MagicRight;
        f_GetObject("TreasureDragObj").SetActive(needReplacePos == EM_EquipPart.eEquipPare_MagicLeft || needReplacePos == EM_EquipPart.eEquipPare_MagicRight);
        f_GetObject("EquipDragObj").SetActive(needReplacePos == EM_EquipPart.eEquipPart_Belt || needReplacePos == EM_EquipPart.eEquipPart_Helmet || needReplacePos == EM_EquipPart.eEquipPart_Armour || needReplacePos == EM_EquipPart.eEquipPart_Weapon);
        f_GetObject("GodEquipDragObj").SetActive(needReplacePos == EM_EquipPart.eEquipPart_GodWeapon);

        //如果是左右法宝跟普通装备表不一样，表不一样得换
        if (needReplacePos == EM_EquipPart.eEquipPare_MagicLeft || needReplacePos == EM_EquipPart.eEquipPare_MagicRight)
        {
            f_GetObject("LabelTitle").GetComponent<UISprite>().spriteName = "title_bawxz";
            if (TreasureWrapComponent == null)
            {
                TreasureWrapComponent = new UIWrapComponent(206, 2, 740, 5, f_GetObject("TreasureItemParent"), f_GetObject("TreasureItem"), _EquipList, TreasureItemUpdateByInfo, null);
            }
            TreasureWrapComponent.f_ResetView();
        }
        else if (needReplacePos == EM_EquipPart.eEquipPart_GodWeapon)
        {
            f_GetObject("LabelTitle").GetComponent<UISprite>().spriteName = "title_godbag";
            if (GodEquipWrapComponent == null)
            {
                GodEquipWrapComponent = new UIWrapComponent(206, 2, 740, 5, f_GetObject("GodEquipItemParent"), f_GetObject("GodEquipItem"), _EquipList, GodEquipItemUpdateByInfo, null);
            }
            GodEquipWrapComponent.f_ResetView();
        }
        else
        {
            f_GetObject("LabelTitle").GetComponent<UISprite>().spriteName = "Label_gehzb";
            if (EquipWrapComponent == null)
            {
                EquipWrapComponent = new UIWrapComponent(206, 2, 740, 5, f_GetObject("EquipItemParent"), f_GetObject("EquipItem"), _EquipList, EquipItemUpdateByInfo, null);
            }
            EquipWrapComponent.f_ResetView();
        }
    }
    #region 装备/法宝item更新
    /// <summary>
    /// 更新法宝(部分拷贝来源法宝背包)
    /// </summary>
    private void TreasureItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        TreasurePoolDT TreasureDT = dt as TreasurePoolDT;
        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UILabel tmpQuality = item.Find("QualityTitle/Quality").GetComponent<UILabel>();
        UILabel tmpQualityName = item.Find("QualityTitle").GetComponent<UILabel>();
        UILabel tmpQuality2 = item.Find("QualityTitle2/Quality").GetComponent<UILabel>();
        Transform tmpQuality2Tran = item.Find("QualityTitle2");
        UILabel tmpQualityName2 = item.Find("QualityTitle2").GetComponent<UILabel>();
        UILabel tmpLevel = item.Find("LevelTitle/Level").GetComponent<UILabel>();
        //UILabel tmpRefine = item.Find("Refine").GetComponent<UILabel>();
        UILabel tmpFitOut = item.Find("FitOut").GetComponent<UILabel>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();
        //UILabel tmpNum = item.Find("Num").GetComponent<UILabel>();
        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        string name = TreasureDT.m_TreasureDT.szName;
        tmpCase.spriteName = UITool.f_GetImporentColorName(TreasureDT.m_TreasureDT.iImportant, ref name);
        tmpName.text = name;
        tmpQualityName.text = UITool.f_GetProName((EM_RoleProperty)TreasureDT.m_TreasureDT.iIntenProId1);
        tmpQualityName2.text = UITool.f_GetProName((EM_RoleProperty)TreasureDT.m_TreasureDT.iIntenProId2);
        if (UITool.f_GetTreasurePro(TreasureDT)[1] == 0)
            tmpQuality2Tran.gameObject.SetActive(false);
        else
            tmpQuality2Tran.gameObject.SetActive(true);
        tmpQuality.text = UITool.f_GetTreasurePro(TreasureDT)[0].ToString();
        tmpQuality2.text = UITool.f_GetTreasurePro(TreasureDT)[1] / 100f + "%";
        tmpLevel.text = TreasureDT.m_lvIntensify.ToString();
        //if (TreasureDT.m_lvRefine != 0)
        //    tmpRefine.text = string.Format("精炼{0}级", TreasureDT.m_lvRefine);
        //else
        //    tmpRefine.text = "";
tmpFitOut.text = UITool.f_GetHowEquip(TreasureDT.iId) == "" ? "" : UITool.f_GetHowEquip(TreasureDT.iId) + " equipping";
        tmpIcon.sprite2D = UITool.f_GetIconSprite(TreasureDT.m_TreasureDT.iIcon);
        //if (TreasureDT.m_Num > 1)
        //    tmpNum.text = string.Format("拥有数量:{0}", TreasureDT.m_Num);
        //else
        //    tmpNum.text = "";

        f_RegClickEvent(item.Find("BtnEquip").gameObject, OnBtnEquipClick, dt);//此行非背包代码
        int count = CheckFateCount(TreasureDT.m_TreasureDT.iId);
        item.Find("FateHint").GetComponent<UILabel>().text = count <= 0 ? "" : "Active charm + " + count.ToString();//可激活缘分提示
    }
    /// <summary>
    /// 更新装备(部分拷贝来源装备背包)
    /// </summary>
    private void EquipItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        EquipPoolDT equipDT = dt as EquipPoolDT;
        if (equipDT.m_iTempleteId == (int)EM_EquipPart.Canglang)
        {
            item.name = "CangLang";
        }
        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UILabel tmpQuality = item.Find("QualityTitle/Quality").GetComponent<UILabel>();
        UILabel tmpQuality2 = item.Find("QualityTitle2/Quality").GetComponent<UILabel>();
        UILabel tmpQualityName = item.Find("QualityTitle").GetComponent<UILabel>();
        UILabel tmpQualityName2 = item.Find("QualityTitle2").GetComponent<UILabel>();
        UILabel tmpLevel = item.Find("LevelTitle/Level").GetComponent<UILabel>();
        //UILabel tmpRefine = item.Find("Refine").GetComponent<UILabel>();
        UILabel tmpFitOut = item.Find("FitOut").GetComponent<UILabel>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();
        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        Transform tStar3 = item.Find("star3");
        Transform tStar5 = item.Find("star5");
        string name = equipDT.m_EquipDT.szName;
        tmpCase.spriteName = UITool.f_GetImporentColorName(equipDT.m_EquipDT.iColour, ref name);
        tmpName.text = name;
        tmpQualityName.text = UITool.f_GetProName((EM_RoleProperty)equipDT.m_EquipDT.iIntenProId) + ":";
        tmpQuality.text = UITool.f_GetEquipPro(equipDT) + "";
        tmpLevel.text = equipDT.m_lvIntensify.ToString();
        if (equipDT.m_lvRefine != 0)
        {
            item.Find("QualityTitle2").gameObject.SetActive(true);
            tmpQualityName2.text = UITool.f_GetProName((EM_RoleProperty)equipDT.m_EquipDT.iRefinProId2) + ":";
            tmpQuality2.text = RolePropertyTools.CalculatePropertyStartLv1(0, equipDT.m_EquipDT.iRefinPro2, equipDT.m_lvRefine + 1) / 100f + "%";
            //tmpRefine.text = string.Format("精炼{0}级", equipDT.m_lvRefine);
        }
        else
        {
            item.Find("QualityTitle2").gameObject.SetActive(false);
            //tmpRefine.text = "";
        }

        //UISprite[] tstar;

        //switch ((EM_Important)equipDT.m_EquipDT.iColour)
        //{
        //    case EM_Important.Red:
        //        tStar5.gameObject.SetActive(true);
        //        tStar3.gameObject.SetActive(false);
        //        tstar = new UISprite[5];
        //        for (int i = 0; i < tStar5.childCount; i++)
        //            tstar[i] = tStar5.GetChild(i).GetComponent<UISprite>();
        //        UITool.f_UpdateStarNum(tstar, equipDT.m_sstars);
        //        break;
        //    case EM_Important.Oragen:
        //        tStar5.gameObject.SetActive(false);
        //        tStar3.gameObject.SetActive(true);
        //        tstar = new UISprite[3];
        //        for (int i = 0; i < tStar3.childCount; i++)
        //            tstar[i] = tStar3.GetChild(i).GetComponent<UISprite>();
        //        UITool.f_UpdateStarNum(tstar, equipDT.m_sstars);
        //        break;
        //    default:
        //        break;
        //}
tmpFitOut.text = UITool.f_GetHowEquip(equipDT.iId) == "" ? "" : UITool.f_GetHowEquip(equipDT.iId) + " equipping";
        tmpIcon.sprite2D = UITool.f_GetIconSprite(equipDT.m_EquipDT.iIcon);

        f_RegClickEvent(item.Find("BtnEquip").gameObject, OnBtnEquipClick, dt);//此行非背包代码
        int count = CheckFateCount(equipDT.m_EquipDT.iId);
        item.Find("FateHint").GetComponent<UILabel>().text = count <= 0 ? "" : "Duyên kích hoạt + " + count.ToString();//可激活缘分提示
    }
    #endregion
    /// <summary>
    /// 检查装备此装备后可激活的缘分数量
    /// </summary>
    /// <returns></returns>
    private static int CheckFateCount(long equipTempId)
    {
        CardPoolDT cardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(needReplaceCardPos);
        List<CardFateDataDT> m_aFateList = cardPoolDT.m_CardFatePoolDT.m_aFateList;
        int count = 0;
        EM_ResourceType resoureceType = EM_ResourceType.Equip;
        if (needReplacePos == EM_EquipPart.eEquipPare_MagicLeft || needReplacePos == EM_EquipPart.eEquipPare_MagicRight)
            resoureceType = EM_ResourceType.Treasure;
        if (needReplacePos == EM_EquipPart.eEquipPart_GodWeapon)
            resoureceType = EM_ResourceType.GodEquipFragment;
        for (int i = 0; i < m_aFateList.Count; i++)
        {
            if (m_aFateList[i].iGoodsType == (int)resoureceType)
            {
                string[] EquipIdArray = m_aFateList[i].szGoodsId.Split(';');
                for (int j = 0; j < EquipIdArray.Length; j++)
                {
                    if (long.Parse(EquipIdArray[j]) == equipTempId)
                    {
                        count++;
                        break;
                    }
                }
            }
        }
        return count;
    }
    /// <summary>
    /// 初始化事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlack", OnBtnBlackClick);
        OnChangeEquipCallback.m_ccCallbackSuc = OnChangeEquipSuccess;
        OnChangeEquipCallback.m_ccCallbackFail = OnChangeEquipFail;

        OnChangeGodEquipCallback.m_ccCallbackSuc = OnChangeGodEquipSuccess;
        OnChangeGodEquipCallback.m_ccCallbackFail = OnChangeGodEquipFail;
    }
    #region 按钮事件
    /// <summary>
    /// 点击界面黑色背景关闭界面
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectEquipPage, UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
    }
    /// <summary>
    /// 点击item里面装备按钮
    /// </summary>
    private void OnBtnEquipClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        UITool.f_OpenOrCloseWaitTip(true);
        if (needReplacePos == EM_EquipPart.eEquipPare_MagicLeft || needReplacePos == EM_EquipPart.eEquipPare_MagicRight)
        {
            TreasurePoolDT treasureDT = obj1 as TreasurePoolDT;
            Data_Pool.m_TeamPool.f_ChangeTeamTreasure((long)needReplaceCardPos, treasureDT.iId, (byte)treasureDT.m_TreasureDT.iSite, OnChangeEquipCallback);
        }
        else if (needReplacePos == EM_EquipPart.eEquipPart_GodWeapon)
        {
            GodEquipPoolDT equipPoolDT = obj1 as GodEquipPoolDT;
            Data_Pool.m_TeamPool.f_ChangeTeamGodEquip((long)needReplaceCardPos, equipPoolDT.iId, (byte)equipPoolDT.m_EquipDT.iSite, OnChangeGodEquipCallback);
        }
        else
        {
            EquipPoolDT equipPoolDT = obj1 as EquipPoolDT;
            Data_Pool.m_TeamPool.f_ChangeTeamEquip((long)needReplaceCardPos, equipPoolDT.iId, (byte)equipPoolDT.m_EquipDT.iSite, OnChangeEquipCallback);
        }
    }
    #endregion
    private void OnChangeEquipSuccess(object obj)
    {
        Data_Pool.m_GuidancePool.m_OtherSave = true;
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Trang bị thành công!");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectEquipPage, UIMessageDef.UI_CLOSE);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LineUpPage, UIMessageDef.UI_OPEN);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FATE_TRIP, 2);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.WearEquipment));
        UITool.f_OpenOrCloseWaitTip(false);

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    private void OnChangeEquipFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Trang bị thất bại，" + CommonTools.f_GetTransLanguage((int)obj));
    }

    /// <summary>
    /// danh sách thần binh
    /// </summary>
    private void GodEquipItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        GodEquipPoolDT equipDT = dt as GodEquipPoolDT;
        if (equipDT.m_iTempleteId == (int)EM_EquipPart.Canglang)
        {
            item.name = "CangLang";
        }
        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UILabel tmpQuality = item.Find("QualityTitle/Quality").GetComponent<UILabel>();
        UILabel tmpQuality2 = item.Find("QualityTitle2/Quality").GetComponent<UILabel>();
        UILabel tmpQualityName = item.Find("QualityTitle").GetComponent<UILabel>();
        UILabel tmpQualityName2 = item.Find("QualityTitle2").GetComponent<UILabel>();
        UILabel tmpLevel = item.Find("LevelTitle/Level").GetComponent<UILabel>();
        //UILabel tmpRefine = item.Find("Refine").GetComponent<UILabel>();
        UILabel tmpFitOut = item.Find("FitOut").GetComponent<UILabel>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();
        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        Transform tStar3 = item.Find("star3");
        Transform tStar5 = item.Find("star5");
        string name = equipDT.m_EquipDT.szName;
        tmpCase.spriteName = UITool.f_GetImporentColorName(equipDT.m_EquipDT.iColour, ref name);
        tmpName.text = name;
        tmpQualityName.text = UITool.f_GetProName((EM_RoleProperty)equipDT.m_EquipDT.iIntenProId) + ":";
        tmpQuality.text = UITool.f_GetGodEquipPro(equipDT) + "";
        tmpLevel.text = equipDT.m_lvIntensify.ToString();
        if (equipDT.m_lvRefine != 0)
        {
            item.Find("QualityTitle2").gameObject.SetActive(true);
            tmpQualityName2.text = UITool.f_GetProName((EM_RoleProperty)equipDT.m_EquipDT.iRefinProId2) + ":";
            tmpQuality2.text = RolePropertyTools.CalculatePropertyStartLv1(0, equipDT.m_EquipDT.iRefinPro2, equipDT.m_lvRefine + 1) / 100f + "%";
            //tmpRefine.text = string.Format("精炼{0}级", equipDT.m_lvRefine);
        }
        else
        {
            item.Find("QualityTitle2").gameObject.SetActive(false);
            //tmpRefine.text = "";
        }

        //UISprite[] tstar;

        //switch ((EM_Important)equipDT.m_EquipDT.iColour)
        //{
        //    case EM_Important.Red:
        //        tStar5.gameObject.SetActive(true);
        //        tStar3.gameObject.SetActive(false);
        //        tstar = new UISprite[5];
        //        for (int i = 0; i < tStar5.childCount; i++)
        //            tstar[i] = tStar5.GetChild(i).GetComponent<UISprite>();
        //        UITool.f_UpdateStarNum(tstar, equipDT.m_sstars);
        //        break;
        //    case EM_Important.Oragen:
        //        tStar5.gameObject.SetActive(false);
        //        tStar3.gameObject.SetActive(true);
        //        tstar = new UISprite[3];
        //        for (int i = 0; i < tStar3.childCount; i++)
        //            tstar[i] = tStar3.GetChild(i).GetComponent<UISprite>();
        //        UITool.f_UpdateStarNum(tstar, equipDT.m_sstars);
        //        break;
        //    default:
        //        break;
        //}
        tmpFitOut.text = UITool.f_GetHowEquip(equipDT.iId) == "" ? "" : UITool.f_GetHowEquip(equipDT.iId) + " đang trang bị";
        tmpIcon.sprite2D = UITool.f_GetIconSprite(equipDT.m_EquipDT.iIcon);

        f_RegClickEvent(item.Find("BtnEquip").gameObject, OnBtnEquipClick, dt);//此行非背包代码
        int count = CheckFateCount(equipDT.m_EquipDT.iId);
        item.Find("FateHint").GetComponent<UILabel>().text = count <= 0 ? "" : "Duyên kích hoạt + " + count.ToString();//可激活缘分提示
    }

    private void OnChangeGodEquipSuccess(object obj)
    {
        Data_Pool.m_GuidancePool.m_OtherSave = true;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Trang bị thành công！");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectEquipPage, UIMessageDef.UI_CLOSE);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LineUpPage, UIMessageDef.UI_OPEN);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
        //glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FATE_TRIP, 2);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.WearEquipment));
        UITool.f_OpenOrCloseWaitTip(false);

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }

    private void OnChangeGodEquipFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        string mess = UITool.f_GetError((int)teMsgOperateResult);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, mess);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1339) + CommonTools.f_GetTransLanguage((int)obj));
    }
}
