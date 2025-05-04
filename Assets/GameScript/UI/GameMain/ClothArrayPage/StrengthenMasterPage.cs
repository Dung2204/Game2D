using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 强化大师页面控制
/// </summary>
public class StrengthenMasterPage : UIFramwork
{
    private EM_Master m_curMasterType = EM_Master.EquipIntensify;
    private EM_FormationPos m_curFormationPos = EM_FormationPos.eFormationPos_Main;
    private int m_curMasterLevel = 0;

    Dictionary<int, int> noPercenPro = new Dictionary<int, int>();
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        noPercenPro.Add((int)EM_RoleProperty.Atk, 1);
        noPercenPro.Add((int)EM_RoleProperty.Hp, 1);
        noPercenPro.Add((int)EM_RoleProperty.Def, 1);
        noPercenPro.Add((int)EM_RoleProperty.MDef, 1);
        noPercenPro.Add((int)EM_RoleProperty.Anger, 1);
        noPercenPro.Add((int)EM_RoleProperty.Energy, 1);
        noPercenPro.Add((int)EM_RoleProperty.RcvAnger, 1);
    }

    /// <summary>
    /// 页面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        //计算当前大师等级
        m_curFormationPos = LineUpPage.CurrentSelectCardPos;
        //检测需要的选项
        InitItemMenu(m_curFormationPos);
    }
    /// <summary>
    /// 检测选项菜单
    /// </summary>
    /// <param name="eFormationPos"></param>
    private void InitItemMenu(EM_FormationPos eFormationPos)
    {
        TeamPoolDT teamPoolDT = Data_Pool.m_TeamPool.f_GetForId((int)eFormationPos) as TeamPoolDT;
        bool isEquip = true;
        bool isTreasure = true;
        m_curMasterType = EM_Master.EquipIntensify;
        for (int i = 0; i < teamPoolDT.m_aEquipPoolDT.Length; i++)
        {
            if (teamPoolDT.m_aEquipPoolDT[i] == null)
            {
                isEquip = false;
                m_curMasterType = EM_Master.TreasureIntensify;
                OnBtnTapClick(f_GetObject("BtnTreasureIntensify").gameObject, EM_Master.TreasureIntensify, null);
                break;
            }
        }
        for (int i = 0; i < teamPoolDT.m_aTreamPoolDT.Length; i++)
        {
            if (teamPoolDT.m_aTreamPoolDT[i] == null)
            {
                isTreasure = false;
                break;
            }
        }
        if (m_curMasterType == EM_Master.EquipIntensify)
            OnBtnTapClick(f_GetObject("BtnEquipIntensify").gameObject, EM_Master.EquipIntensify, null);
        f_GetObject("BtnEquipIntensify").SetActive(isEquip);
        f_GetObject("BtnEquipRefine").SetActive(isEquip);
        f_GetObject("BtnTreasureIntensify").SetActive(isTreasure);
        f_GetObject("BtnTreasureRefine").SetActive(isTreasure);
        f_GetObject("UIGridMenu").GetComponent<UIGrid>().Reposition();
    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        UpdateUIByMasterTypeCoutent(m_curMasterType);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
    }
    /// <summary>
    /// 返回当前类型大师等级
    /// </summary>
    private int CheckMasterLevel(EM_Master masterType)
    {
        int level = 0;

        return level;
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlack", OnCloseClick);
        f_RegClickEvent("BtnEquipIntensify", OnBtnTapClick, EM_Master.EquipIntensify);
        f_RegClickEvent("BtnEquipRefine", OnBtnTapClick, EM_Master.EquipRefine);
        f_RegClickEvent("BtnTreasureIntensify", OnBtnTapClick, EM_Master.TreasureIntensify);
        f_RegClickEvent("BtnTreasureRefine", OnBtnTapClick, EM_Master.TreasureRefine);
    }
    /// <summary>
    /// 设置分页到正常状态
    /// </summary>
    /// <param name="btnTap"></param>
    private void ChangeTapToNormal(GameObject btnTap, bool isNormal = true)
    {
        btnTap.transform.Find("Normal").gameObject.SetActive(isNormal);
        btnTap.transform.Find("Press").gameObject.SetActive(!isNormal);
    }
    /// <summary>
    /// 强化大师等级
    /// <param name="masterType"></param>
    /// <param name="masterLevel"></param>
    /// <returns></returns>
    private MasterDT GetMasterDT(EM_Master masterType, int masterLevel)
    {
        List<NBaseSCDT> listAllData = glo_Main.GetInstance().m_SC_Pool.m_MasterSC.f_GetAll();
        int masterLevelTemp = 0;
        MasterDT masterDT = null;
        for (int i = 0; i < listAllData.Count; i++)
        {
            masterDT = listAllData[i] as MasterDT;
            if (masterDT.iType == (int)masterType)
            {
                masterLevelTemp++;
                if (masterLevelTemp == masterLevel)
                {
                    return masterDT;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    /// <param name="masterType"></param>
    private void UpdateUIByMasterTypeCoutent(EM_Master masterType)
    {
        m_curMasterLevel = Data_Pool.m_TeamPool.f_GetMasterLevel(masterType, m_curFormationPos);
        //1.更换标题
        //2.更新4个装备icon和名字
        //3.获取当前强化大师等级
        //4.设置强化大师等级数据
        string title1 = "";
        string title2 = "";
        string title3 = "";
        string title4 = "";
        TeamPoolDT teamPoolDT = Data_Pool.m_TeamPool.f_GetForId((int)m_curFormationPos) as TeamPoolDT;
        MasterDT curMasterDT = GetMasterDT(masterType, m_curMasterLevel);
        MasterDT nextMasterDT = GetMasterDT(masterType, m_curMasterLevel + 1);
        EquipPoolDT equipPoolDT;
        TreasurePoolDT treasurePoolDT;
        string EquipName = "";
        string BorderSprName = "";
        switch (masterType)
        {
            case EM_Master.EquipIntensify:
                title1 = CommonTools.f_GetTransLanguage(1695);
                title2 = CommonTools.f_GetTransLanguage(1696);
                title3 = CommonTools.f_GetTransLanguage(1697);
                title4 = CommonTools.f_GetTransLanguage(1698);
                equipPoolDT = teamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Weapon);
                EquipName = equipPoolDT.m_EquipDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(equipPoolDT.m_EquipDT.iColour, ref EquipName);
                SetItemData(1, UITool.f_GetIconSprite(equipPoolDT.m_EquipDT.iIcon), BorderSprName, EquipName, equipPoolDT.m_lvIntensify, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnEquipItemClick, equipPoolDT, EquipBox.BoxTye.Inten);
                equipPoolDT = teamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Armour);
                EquipName = equipPoolDT.m_EquipDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(equipPoolDT.m_EquipDT.iColour, ref EquipName);
                SetItemData(2, UITool.f_GetIconSprite(equipPoolDT.m_EquipDT.iIcon), BorderSprName, EquipName, equipPoolDT.m_lvIntensify, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnEquipItemClick, equipPoolDT, EquipBox.BoxTye.Inten);
                equipPoolDT = teamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Helmet);
                EquipName = equipPoolDT.m_EquipDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(equipPoolDT.m_EquipDT.iColour, ref EquipName);
                SetItemData(3, UITool.f_GetIconSprite(equipPoolDT.m_EquipDT.iIcon), BorderSprName, EquipName, equipPoolDT.m_lvIntensify, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnEquipItemClick, equipPoolDT, EquipBox.BoxTye.Inten);
                equipPoolDT = teamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Belt);
                EquipName = equipPoolDT.m_EquipDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(equipPoolDT.m_EquipDT.iColour, ref EquipName);
                SetItemData(4, UITool.f_GetIconSprite(equipPoolDT.m_EquipDT.iIcon), BorderSprName, EquipName, equipPoolDT.m_lvIntensify, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnEquipItemClick, equipPoolDT, EquipBox.BoxTye.Inten);
                break;
            case EM_Master.EquipRefine:
                title1 = CommonTools.f_GetTransLanguage(1699);
                title2 = CommonTools.f_GetTransLanguage(1700);
                title3 = CommonTools.f_GetTransLanguage(1701);
                title4 = CommonTools.f_GetTransLanguage(1702);
                equipPoolDT = teamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Weapon);
                EquipName = equipPoolDT.m_EquipDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(equipPoolDT.m_EquipDT.iColour, ref EquipName);
                SetItemData(1, UITool.f_GetIconSprite(equipPoolDT.m_EquipDT.iIcon), BorderSprName, EquipName, equipPoolDT.m_lvRefine, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnEquipItemClick, equipPoolDT, EquipBox.BoxTye.Refine);
                equipPoolDT = teamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Armour);
                EquipName = equipPoolDT.m_EquipDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(equipPoolDT.m_EquipDT.iColour, ref EquipName);
                SetItemData(2, UITool.f_GetIconSprite(equipPoolDT.m_EquipDT.iIcon), BorderSprName, EquipName, equipPoolDT.m_lvRefine, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnEquipItemClick, equipPoolDT, EquipBox.BoxTye.Refine);
                equipPoolDT = teamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Helmet);
                EquipName = equipPoolDT.m_EquipDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(equipPoolDT.m_EquipDT.iColour, ref EquipName);
                SetItemData(3, UITool.f_GetIconSprite(equipPoolDT.m_EquipDT.iIcon), BorderSprName, EquipName, equipPoolDT.m_lvRefine, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnEquipItemClick, equipPoolDT, EquipBox.BoxTye.Refine);
                equipPoolDT = teamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Belt);
                EquipName = equipPoolDT.m_EquipDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(equipPoolDT.m_EquipDT.iColour, ref EquipName);
                SetItemData(4, UITool.f_GetIconSprite(equipPoolDT.m_EquipDT.iIcon), BorderSprName, EquipName, equipPoolDT.m_lvRefine, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnEquipItemClick, equipPoolDT, EquipBox.BoxTye.Refine);
                break;
            case EM_Master.TreasureIntensify:
                title1 = CommonTools.f_GetTransLanguage(1703);
                title2 = CommonTools.f_GetTransLanguage(1704);
                title3 = CommonTools.f_GetTransLanguage(1705);
                title4 = CommonTools.f_GetTransLanguage(1706);
                treasurePoolDT = teamPoolDT.f_GetTreasurePoolDT(EM_Treasure.eEquipPare_MagicLeft);
                EquipName = treasurePoolDT.m_TreasureDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(treasurePoolDT.m_TreasureDT.iImportant, ref EquipName);
                SetItemData(3, UITool.f_GetIconSprite(treasurePoolDT.m_TreasureDT.iIcon), BorderSprName, EquipName, treasurePoolDT.m_lvIntensify, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnTreasureItemClick, treasurePoolDT, TreasureBox.BoxType.Inten);
                treasurePoolDT = teamPoolDT.f_GetTreasurePoolDT(EM_Treasure.eEquipPare_MagicRight);
                EquipName = treasurePoolDT.m_TreasureDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(treasurePoolDT.m_TreasureDT.iImportant, ref EquipName);
                SetItemData(1, UITool.f_GetIconSprite(treasurePoolDT.m_TreasureDT.iIcon), BorderSprName, EquipName, treasurePoolDT.m_lvIntensify, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnTreasureItemClick, treasurePoolDT, TreasureBox.BoxType.Inten);
                f_GetObject("Item2").SetActive(false);
                f_GetObject("Item4").SetActive(false);
                break;
            case EM_Master.TreasureRefine:
                title1 = CommonTools.f_GetTransLanguage(1707);
                title2 = CommonTools.f_GetTransLanguage(1708);
                title3 = CommonTools.f_GetTransLanguage(1709);
                title4 = CommonTools.f_GetTransLanguage(1710);
                treasurePoolDT = teamPoolDT.f_GetTreasurePoolDT(EM_Treasure.eEquipPare_MagicLeft);
                EquipName = treasurePoolDT.m_TreasureDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(treasurePoolDT.m_TreasureDT.iImportant, ref EquipName);
                SetItemData(3, UITool.f_GetIconSprite(treasurePoolDT.m_TreasureDT.iIcon), BorderSprName, EquipName, treasurePoolDT.m_lvRefine, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnTreasureItemClick, treasurePoolDT, TreasureBox.BoxType.Refine);
                treasurePoolDT = teamPoolDT.f_GetTreasurePoolDT(EM_Treasure.eEquipPare_MagicRight);
                EquipName = treasurePoolDT.m_TreasureDT.szName;
                BorderSprName = UITool.f_GetImporentColorName(treasurePoolDT.m_TreasureDT.iImportant, ref EquipName);
                SetItemData(1, UITool.f_GetIconSprite(treasurePoolDT.m_TreasureDT.iIcon), BorderSprName, EquipName, treasurePoolDT.m_lvRefine, nextMasterDT == null ? -1 : nextMasterDT.iLv,
                    OnTreasureItemClick, treasurePoolDT, TreasureBox.BoxType.Refine);
                f_GetObject("Item2").SetActive(false);
                f_GetObject("Item4").SetActive(false);
                break;
        }
        f_GetObject("LabelMasterTitle1").GetComponent<UILabel>().text = title1;
        f_GetObject("LabelMasterTitle2").GetComponent<UILabel>().text = title2;
        f_GetObject("LabelMasterTitle4").GetComponent<UILabel>().text = title4;

        GameObject masterNow = f_GetObject("MasterNow");
        if (curMasterDT != null)
        {
            SetMasterContent(masterNow, title2 + string.Format(CommonTools.f_GetTransLanguage(1711), m_curMasterLevel), curMasterDT);
        }
        else
        {
            SetMasterContentIsAct(masterNow, false);
            masterNow.transform.Find("LabelLevel").GetComponent<UILabel>().text = title2 + string.Format(CommonTools.f_GetTransLanguage(1711), m_curMasterLevel); 
            masterNow.transform.Find("LabelHint").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1712), title3); 
        }
        GameObject masterNext = f_GetObject("MasterNext");
        if (nextMasterDT != null)
        {
            SetMasterContent(masterNext, title2 + string.Format(CommonTools.f_GetTransLanguage(1711), (m_curMasterLevel + 1)), nextMasterDT); 
        }
        else
        {
            SetMasterContentIsAct(masterNext, false);
            masterNext.transform.Find("LabelLevel").GetComponent<UILabel>().text = title2 + string.Format(CommonTools.f_GetTransLanguage(1711), m_curMasterLevel); 
            masterNext.transform.Find("LabelHint").GetComponent<UILabel>().text = title2 + CommonTools.f_GetTransLanguage(1713);
        }
    }
    private void SetMasterContentIsAct(GameObject master, bool isAct)
    {
        master.transform.Find("LabelProp1").gameObject.SetActive(isAct);
        master.transform.Find("LabelProp2").gameObject.SetActive(isAct);
        master.transform.Find("LabelProp3").gameObject.SetActive(isAct);
        master.transform.Find("LabelProp4").gameObject.SetActive(isAct);
        master.transform.Find("LabelHint").gameObject.SetActive(!isAct);
    }

    /// <summary>
    /// 根据属性类型获取字符
    /// </summary>
    /// <param name="propValue"></param>
    private string GetStrByPropValue(int propValue, int iPropId)
    {
        //2019.08.06 修改为根据属性判断是比率还是定值
        if (noPercenPro.ContainsKey(iPropId))
        {
            return propValue.ToString();
        }
        else
        {
            return CommonTools.f_GetPercentStrTenThousandStr(propValue);
        }
//        if (iType == 2 || iType == 4)//装备精炼和法宝精炼
//        {
//            return CommonTools.f_GetPercentStrTenThousandStr(propValue);
//        }
//        else
//        {
//            return propValue.ToString();
//        }
    }
    /// <summary>
    /// 设置强化大师内容
    /// </summary>
    private void SetMasterContent(GameObject master, string title, MasterDT masterDT)
    {
        SetMasterContentIsAct(master, true);
        master.transform.Find("LabelLevel").GetComponent<UILabel>().text = title;
        int index = 1;
        int indexDef = -1;
        int indexMDef = -1;
        int doubleDef = 0;//双防
        if (masterDT.iPro1 > 0)
        {
            string propValue = GetStrByPropValue(masterDT.iPro1, masterDT.iProID1);
            master.transform.Find("LabelProp" + index.ToString()).GetComponent<UILabel>().text
                = UITool.f_GetProName((EM_RoleProperty)masterDT.iProID1) + "+" + propValue;
            indexDef = (masterDT.iProID1 == (int)EM_RoleProperty.Def) ? index : indexDef;
            indexMDef = (masterDT.iProID1 == (int)EM_RoleProperty.MDef) ? index : indexMDef;
            doubleDef = (masterDT.iProID1 == (int)EM_RoleProperty.Def || masterDT.iProID1 == (int)EM_RoleProperty.MDef) ? masterDT.iPro1 : doubleDef;
            index++;
        }

        if (masterDT.iPro > 0)
        {
            string propValue = GetStrByPropValue(masterDT.iPro, masterDT.iProId2);
            master.transform.Find("LabelProp" + index.ToString()).GetComponent<UILabel>().text
                = UITool.f_GetProName((EM_RoleProperty)masterDT.iProId2) + "+" + propValue;
            indexDef = (masterDT.iProId2 == (int)EM_RoleProperty.Def) ? index : indexDef;
            indexMDef = (masterDT.iProId2 == (int)EM_RoleProperty.MDef) ? index : indexMDef;
            doubleDef = (masterDT.iProId2 == (int)EM_RoleProperty.Def || masterDT.iProId2 == (int)EM_RoleProperty.MDef) ? masterDT.iPro : doubleDef;
            index++;
        }

        if (masterDT.iPro3 > 0)
        {
            string propValue = GetStrByPropValue(masterDT.iPro3, masterDT.iProId3);
            master.transform.Find("LabelProp" + index.ToString()).GetComponent<UILabel>().text
                = UITool.f_GetProName((EM_RoleProperty)masterDT.iProId3) + "+" + propValue;
            indexDef = (masterDT.iProId3 == (int)EM_RoleProperty.Def) ? index : indexDef;
            indexMDef = (masterDT.iProId3 == (int)EM_RoleProperty.MDef) ? index : indexMDef;
            doubleDef = (masterDT.iProId3 == (int)EM_RoleProperty.Def || masterDT.iProId3 == (int)EM_RoleProperty.MDef) ? masterDT.iPro3 : doubleDef;
            index++;
        }

        if (masterDT.iPro4 > 0)
        {
            string propValue = GetStrByPropValue(masterDT.iPro4, masterDT.iProId4);
            master.transform.Find("LabelProp" + index.ToString()).GetComponent<UILabel>().text
                = UITool.f_GetProName((EM_RoleProperty)masterDT.iProId4) + "+" + propValue;
            indexDef = (masterDT.iProId4 == (int)EM_RoleProperty.Def) ? index : indexDef;
            indexMDef = (masterDT.iProId4 == (int)EM_RoleProperty.MDef) ? index : indexMDef;
            doubleDef = (masterDT.iProId4 == (int)EM_RoleProperty.Def || masterDT.iProId4 == (int)EM_RoleProperty.MDef) ? masterDT.iPro4 : doubleDef;
            index++;
        }
        if (masterDT.iPro5 > 0)
        {
            if (index <= 4)
            {
                string propValue = GetStrByPropValue(masterDT.iPro5, masterDT.iProId5);
                master.transform.Find("LabelProp" + index.ToString()).GetComponent<UILabel>().text
                    = UITool.f_GetProName((EM_RoleProperty)masterDT.iProId5) + "+" + propValue;
            }
            else//5个时，根据策划，必有物防和法防，且物防=法防
            {
                string propValue = GetStrByPropValue(doubleDef, (int)EM_RoleProperty.Def);
                string propValue2 = GetStrByPropValue(masterDT.iPro5, masterDT.iProId5);
                if (indexDef == -1 || indexMDef == -1)
                    MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1714));
                master.transform.Find("LabelProp" + indexDef.ToString()).GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1715)+"+" + propValue;
                master.transform.Find("LabelProp" + indexMDef.ToString()).GetComponent<UILabel>().text
                    = UITool.f_GetProName((EM_RoleProperty)masterDT.iProId5) + "+" + propValue2;
            }
            index++;
        }
        for (int i = 1; i <= 4; i++)
        {
            master.transform.Find("LabelProp" + i.ToString()).gameObject.SetActive(index > i);
        }
    }
    /// <summary>
    /// 设置item数据
    /// </summary>
    /// <param name="ItemIndex">item序号</param>
    /// <param name="icon">图标</param>
    /// <param name="borderSprName">边框</param>
    /// <param name="Name">名字</param>
    /// <param name="curProgress">当前等级进度</param>
    /// <param name="desProgress">目标等级,-1表示已达到最高级，没有下一级的强化大师等级</param>
    /// <param name="onItemClickCallback">点击icon回调</param>
    /// <param name="poolDT">回调数据传参1</param>
    /// <param name="boxType">回调数据传参2</param>
    private void SetItemData(int ItemIndex, Sprite icon, string borderSprName, string Name, int curProgress, int desProgress,
        ccUIEventListener.VoidDelegateV2 onItemClickCallback, BasePoolDT<long> poolDT, object boxType)
    {
        GameObject item = f_GetObject("Item" + ItemIndex.ToString());
        item.SetActive(true);
        item.transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = icon;
        item.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = borderSprName;
        item.transform.Find("LabelName").GetComponent<UILabel>().text = Name;
        item.transform.Find("SliderProgress").GetComponent<UISlider>().value = desProgress == -1 ? 1 : (curProgress * 1.0f / desProgress);
        item.transform.Find("SliderProgress/LabelProgress").GetComponent<UILabel>().text = curProgress + "/" + (desProgress == -1 ? "max" : desProgress.ToString());
        f_RegClickEvent(item.transform.Find("Icon").gameObject, onItemClickCallback, poolDT, boxType);
    }
    #region 按钮事件
    private void OnEquipItemClick(GameObject go, object obj1, object obj2)
    {
        EquipBox tmp = new EquipBox();
        tmp.tEquipPoolDT = (EquipPoolDT)obj1;
        tmp.tType = (EquipBox.BoxTye)obj2;


        if (tmp.tType == EquipBox.BoxTye.Refine)
        {
            if (!UITool.f_GetIsOpensystem(EM_NeedLevel.EquipRefine))
            {
                UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1716), UITool.f_GetSysOpenLevel(EM_NeedLevel.EquipRefine)));
                return;
            }
        }

        tmp.oType = EquipBox.OpenType.Master;
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.LineUpPage));
        ccUIManage.GetInstance().f_SendMsg(UINameConst.EquipManage, UIMessageDef.UI_OPEN, tmp);
    }
    private void OnTreasureItemClick(GameObject go, object obj1, object obj2)
    {
        TreasureBox tmpT = new TreasureBox();
        tmpT.tTreasurePoolDT = (TreasurePoolDT)obj1;
        tmpT.tType = (TreasureBox.BoxType)obj2;
        if (tmpT.tType == TreasureBox.BoxType.Refine)
        {
            if (tmpT.tTreasurePoolDT.m_TreasureDT.iImportant <= (int)EM_Important.Purple)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1717));
                return;
            }
        }


        tmpT.IsShowChange = 1;
        tmpT.IsMastr = 1;
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.LineUpPage));
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureManage, UIMessageDef.UI_OPEN, tmpT);
    }
    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    private void OnCloseClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.StrengthenMasterPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击分页按钮
    /// </summary>
    private void OnBtnTapClick(GameObject go, object obj1, object obj2)
    {
        m_curMasterType = (EM_Master)obj1;
        ChangeTapToNormal(f_GetObject("BtnEquipIntensify"));
        ChangeTapToNormal(f_GetObject("BtnEquipRefine"));
        ChangeTapToNormal(f_GetObject("BtnTreasureIntensify"));
        ChangeTapToNormal(f_GetObject("BtnTreasureRefine"));

        ChangeTapToNormal(go, false);
        UpdateUIByMasterTypeCoutent(m_curMasterType);
    }
    #endregion
}
