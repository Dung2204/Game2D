using ccU3DEngine;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 法宝转换
/// </summary>
public class TransmigrationTreasurePage : UIFramwork
{
    private GameObject _treasureChangeParent;
    private GameObject _treasureChangeItem;
    private GameObject _treasureAreaObj;
    private UI2DSprite _treasureInfoIcon;
    private UISprite _treasureInfoImportant;
    private UILabel _treasureName;
    private UILabel _treasureArr;
    private UILabel _treasureSpoil;
    private UILabel _treasureTitle;
    private UILabel _treasureCost;
    private GameObject _btnChangeSelect;
    private GameObject _btnChangeTreas;
    private GameObject _treasureChange;
    private GameObject _btnAddTreasure;
    private GameObject _selectTreasPop;
    private GameObject _btnCloseSelectTrea;
    private UIWrapComponent Treasure_WrapComponet;
    private UIWrapComponent TreasureSelect_WrapComponet;
    private List<BasePoolDT<long>> _treasureList;
    private List<BasePoolDT<long>> _scrTreasList;
    private GameObject _selectTreasParent;
    private GameObject _selectTreasItem;
    private GameObject _btnTreasureCostItem;
    private UI2DSprite _treasureCostIcon;
    private UISprite _treasureCostImp;
    private UI2DSprite _showTreasureIcon;
    private int _targetTreasID;
    private long[] _arrayTreasSeverId;
    private UILabel _costText;
    //初始化事件
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        InitUI();
        //关闭
        f_RegClickEvent(f_GetObject("BlackBG"),OnCloseClick);
        f_RegClickEvent(_btnChangeSelect, OnAutomaticSelectClick);
        f_RegClickEvent(_btnChangeTreas, OnTreasClick);
        f_RegClickEvent(_btnAddTreasure, OnAddTreasureClick);
        f_RegClickEvent(_btnCloseSelectTrea,OnCloseSelectClick);
        f_RegClickEvent(_btnTreasureCostItem, OnAddTreasureClick);
    }

    //打开界面
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        _arrayTreasSeverId = null;
        InitData();
        f_SetSelectTreasureInfo();
        f_AcquiescenceSelectTreasure();
        
    }

    //关闭界面
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    //数据初始化
    private void InitData()
    {
        _treasureCostIcon.sprite2D = null;
        _showTreasureIcon.sprite2D = null;
        _targetTreasID = 0;
        _btnTreasureCostItem.SetActive(false);
    }

    //初始化ui
    private void InitUI()
    {
        _treasureChangeParent = f_GetObject("SeleTreasItemParent");
        _treasureChangeItem = f_GetObject("SeleTreasItem");
        _treasureAreaObj = f_GetObject("TreasureItem");
        _treasureInfoIcon = _treasureAreaObj.transform.Find("Icon").GetComponent<UI2DSprite>();
        _treasureInfoImportant = _treasureAreaObj.transform.Find("Important").GetComponent<UISprite>();
        _treasureName = f_GetObject("TreasuName").transform.GetComponent<UILabel>();
        _treasureArr = f_GetObject("TreasuAtt").transform.GetComponent<UILabel>();
        _treasureSpoil = f_GetObject("TreasuSpoil").transform.GetComponent<UILabel>();
        _treasureTitle = f_GetObject("LabelTitle").transform.GetComponent<UILabel>();
        _treasureCost = f_GetObject("SysceeCount").transform.GetComponent<UILabel>();
        _btnChangeSelect = f_GetObject("BtnChange");
        _btnChangeTreas = f_GetObject("BtnChangeTreas");
        _treasureChange = f_GetObject("TreasureChange");
        _btnAddTreasure = f_GetObject("BtnTreasSprite");
        _selectTreasPop = f_GetObject("ChangeTresItem");
        _btnCloseSelectTrea = f_GetObject("TreasAlphe");
        _selectTreasParent = f_GetObject("TreasSelectItemParent");
        _selectTreasItem = f_GetObject("TreasSelectItem");
        _btnTreasureCostItem = _btnAddTreasure.transform.Find("TreasureCost").gameObject;
        _treasureCostIcon = _btnTreasureCostItem.transform.Find("Icon").GetComponent<UI2DSprite>();
        _treasureCostImp = _btnTreasureCostItem.transform.Find("Important").GetComponent<UISprite>();
        _showTreasureIcon = _treasureChange.transform.Find("Icon").GetComponent<UI2DSprite>();
        _costText = f_GetObject("LabelTiShi").transform.GetComponent<UILabel>();
    }


    //选择转换法宝链表信息
    private void f_SetSelectTreasureInfo()
    {
        List<BasePoolDT<long>> _treasureList = Data_Pool.m_TransmigrationTreasurePool.f_GetAll();
        if (Treasure_WrapComponet == null)
        {
            Treasure_WrapComponet = new UIWrapComponent(200, 1, 850, 4, _treasureChangeParent, _treasureChangeItem, _treasureList, OnContentTreasureItemUpdate, TreasureClick);
        }
        //Treasure_WrapComponet.f_UpdateList(_cardList);
        Treasure_WrapComponet.f_ResetView();
    }

    //能转换的法宝信息
    private void OnContentTreasureItemUpdate(Transform item, BasePoolDT<long> dt)
    {
        UILabel treasureName = item.Find("Name").GetComponent<UILabel>();
        UI2DSprite treasureIcon = item.Find("Icon").GetComponent<UI2DSprite>();
        UISprite treasureImportant = item.Find("Important").GetComponent<UISprite>();
        TransmigrationTreasurePoolDT data = (TransmigrationTreasurePoolDT)dt;
        TreasureDT treasureDT = (TreasureDT)glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(data.m_TransmigrationTreasureDT.iTreasureID);
        treasureName.text = treasureDT.szName;
        treasureIcon.sprite2D = UITool.f_GetIconSprite(treasureDT.iIcon);
        string tempName = treasureDT.szName;
        treasureImportant.spriteName = UITool.f_GetImporentColorName(treasureDT.iImportant, ref tempName);
    }

    //选择法宝事件
    private void TreasureClick(Transform item, BasePoolDT<long> dt)
    {
        f_TreasureInfo(dt);
    }


    //初始默认选择第一个法宝
    private void f_AcquiescenceSelectTreasure()
    {
        List<BasePoolDT<long>> _treasureList = Data_Pool.m_TransmigrationTreasurePool.f_GetAll();
        if (_treasureList == null)
        {
            return;
        }
        BasePoolDT<long> dt = _treasureList[0] as BasePoolDT<long>;
        f_TreasureInfo(dt);
    }

    //法宝信息展示
    private void f_TreasureInfo(BasePoolDT<long> dt)
    {
        TransmigrationTreasurePoolDT data = (TransmigrationTreasurePoolDT)dt;
        TreasureDT treasureDT = (TreasureDT)glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(data.m_TransmigrationTreasureDT.iTreasureID);
        _targetTreasID = data.m_TransmigrationTreasureDT.iId;
        _costText.text = string.Format(CommonTools.f_GetTransLanguage(797), data.m_TransmigrationTreasureDT.iCost);
        _treasureList = GetCanTransmigrationList(_targetTreasID);
        _treasureInfoIcon.sprite2D = UITool.f_GetIconSprite(treasureDT.iIcon);
        _treasureInfoImportant.spriteName = UITool.f_GetImporentCase(treasureDT.iImportant);
        //_treasureInfoImportant.spriteName = UITool.f_GetImporentColorName(treasureDT.iImportant, ref treasureDT.szName);
        _treasureArr.text = UITool.f_GetProName((EM_RoleProperty)treasureDT.iIntenProId1)+"+"+ f_GetTreasurePro(treasureDT)[0].ToString();
        _treasureSpoil.text = UITool.f_GetProName((EM_RoleProperty)treasureDT.iIntenProId2) + "+" + f_GetTreasurePro(treasureDT)[1].ToString();
        _treasureTitle.text = string.Format(CommonTools.f_GetTransLanguage(798), treasureDT.szName);
        _treasureName.text = string.Format(CommonTools.f_GetTransLanguage(799), treasureDT.szName);
        _treasureCost.text = CommonTools.f_GetListCommonDT(data.m_TransmigrationTreasureDT.szCost)[0].mResourceNum.ToString();
        TreasureFragmentsDT treasureFragmentDT = (TreasureFragmentsDT)glo_Main.GetInstance().m_SC_Pool.m_TreasureFragmentsSC.f_GetSC(data.m_TransmigrationTreasureDT.iId);
        UI2DSprite Icon = _treasureChange.transform.Find("Icon").GetComponent<UI2DSprite>();
        UISprite Important = _treasureChange.transform.Find("Important").GetComponent<UISprite>();
        Icon.sprite2D = UITool.f_GetIconSprite(treasureFragmentDT.iIcon);
        string tempName = treasureDT.szName;
        Important.spriteName = UITool.f_GetImporentColorName(treasureFragmentDT.iImportant, ref tempName);
        _treasureCostIcon.sprite2D = null;
        _btnTreasureCostItem.SetActive(false);
    }

    private int[] f_GetTreasurePro(TreasureDT dt)
    {
        int[] tmp = new int[2];
        tmp[0] = dt.iAddPro1 * (1 - 1) + dt.iStartPro1;
        tmp[1] = dt.iAddPro2 * (1 - 1) + dt.iStartPro2;
        return tmp;
    }

    //关闭事件
    private void OnCloseClick(GameObject go,object obj1,object obj2)
    {
        InitData();
        //Data_Pool.m_TransmigrationTreasurePool.f_CheckTransTreasureRedPoint();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TransmigrationTreasurePage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasurePage, UIMessageDef.UI_OPEN, GrabTreasurePage.curSelectTreasure.iId);
    }


    //选择法宝界面信息
    private void f_SetSelectInfoTreasure()
    {
        if (TreasureSelect_WrapComponet == null)
        {
            TreasureSelect_WrapComponet = new UIWrapComponent(220, 1, 770, 5,
            _selectTreasParent, _selectTreasItem, _treasureList, TreasureChangeSelectInfo, TreasureChangeSelectClick);
        }
        TreasureSelect_WrapComponet.f_UpdateList(_treasureList);
        TreasureSelect_WrapComponet.f_ResetView();
    }

    //法宝链表信息
    private void TreasureChangeSelectInfo(Transform item, BasePoolDT<long> dt)
    {
        TreasurePoolDT treasureData = (TreasurePoolDT)dt;
        UI2DSprite Icon = item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Name = item.Find("Name").GetComponent<UILabel>();
        UILabel Lv = item.Find("Lv").GetComponent<UILabel>();
        UILabel SkyLife = item.Find("SkyLift").GetComponent<UILabel>();
        UISprite Important = item.Find("Important").GetComponent<UISprite>();

        int count = UITool.f_GetTreasureNum(treasureData);
        Name.text = treasureData.m_TreasureDT.szName+"："+ count.ToString();
        Icon.sprite2D = UITool.f_GetIconSprite(treasureData.m_TreasureDT.iIcon);
        string tempName = treasureData.m_TreasureDT.szName;
        Important.spriteName = UITool.f_GetImporentColorName(treasureData.m_TreasureDT.iImportant, ref tempName);
        Lv.text = UITool.f_GetProName((EM_RoleProperty)treasureData.m_TreasureDT.iIntenProId1) + "+" + UITool.f_GetTreasurePro(treasureData)[0].ToString();
        SkyLife.text = UITool.f_GetProName((EM_RoleProperty)treasureData.m_TreasureDT.iIntenProId2) + "+" + UITool.f_GetTreasurePro(treasureData)[1].ToString();

    }

    //选择此刻选择的法宝事件
    private void TreasureChangeSelectClick(Transform item, BasePoolDT<long> dt)
    {
        TreasurePoolDT treasureData = (TreasurePoolDT)dt;
        _btnTreasureCostItem.SetActive(true);
        _arrayTreasSeverId = null;
        _arrayTreasSeverId = f_GetSeverTreasureId(treasureData.m_TreasureDT.iId);
        _treasureCostIcon.sprite2D = UITool.f_GetIconSprite(treasureData.m_TreasureDT.iIcon);
        string tempName = treasureData.m_TreasureDT.szName;
        _treasureCostImp.spriteName = UITool.f_GetImporentColorName(treasureData.m_TreasureDT.iImportant, ref tempName);
        _selectTreasPop.SetActive(false);
    }

    //筛选可转换消耗的法宝
    private List<BasePoolDT<long>> GetCanTransmigrationList(int treasureId)
    {
        TransmigrationTreasurePoolDT poolDT = Data_Pool.m_TransmigrationTreasurePool.f_GetForId((long)treasureId) as TransmigrationTreasurePoolDT;
        //_arrayTreasSeverId = new long[poolDT.m_TransmigrationTreasureDT.iCost];
        List<BasePoolDT<long>> _treasureList = Data_Pool.m_TreasurePool.f_GetAll();
        List<BasePoolDT<long>> tempList = new List<BasePoolDT<long>>();
        _scrTreasList = new List<BasePoolDT<long>>();
        Data_Pool.m_TreasurePool.f_SortList();
        for (int i=0;i< _treasureList.Count;i++)
        {
            TreasurePoolDT treasureData = _treasureList[i] as TreasurePoolDT;
            if (treasureData.m_lvIntensify != 1 || UITool.f_GetHowEquip(treasureData.iId) != "" 
                || treasureData.m_ExpIntensify != 0 || treasureData.m_lvRefine != 0 || treasureData.m_TreasureDT.iImportant != (int)EM_Important.Oragen
                || treasureData.m_TreasureDT.iId == 5002)
                continue;
            else
            {
                int countTreasure = UITool.f_GetTreasureNum(treasureData);//UITool.f_GetResourceHaveNum((int)EM_ResourceType.Treasure, treasureData.m_TreasureDT.iId);
                if (countTreasure >= poolDT.m_TransmigrationTreasureDT.iCost)
                {
                    tempList.Add(treasureData);
                    _scrTreasList.Add(treasureData);
                }
            }
        }
        //排除相同法宝
        for (int i = 0; i < tempList.Count; i++)
        {
            TreasurePoolDT treasuerDT1 = (TreasurePoolDT)tempList[i];
            for (int j = tempList.Count - 1; j > i; j--)
            {
                TreasurePoolDT treasuerDT2 = (TreasurePoolDT)tempList[j];
                if (treasuerDT1.m_TreasureDT.iId == treasuerDT2.m_TreasureDT.iId)
                {
                    tempList.RemoveAt(j);
                }
            }
        }
        for(int i = 0;i< tempList.Count;i++)
        {

        }
        return tempList;
    }

    //根据模板id获取服务器id
    private long[] f_GetSeverTreasureId(int templetId)
    {
        TransmigrationTreasurePoolDT poolDT = Data_Pool.m_TransmigrationTreasurePool.f_GetForId((long)_targetTreasID) as TransmigrationTreasurePoolDT;
        List<BasePoolDT<long>> treaSeverIDList = new List<BasePoolDT<long>>();
        long[] severTreasId = new long[poolDT.m_TransmigrationTreasureDT.iCost];
        for (int i = 0; i < _scrTreasList.Count; i++)
        {
            TreasurePoolDT treasure = (TreasurePoolDT)_scrTreasList[i];
            if (templetId == treasure.m_TreasureDT.iId)
            {
                BasePoolDT<long> temp = new BasePoolDT<long>();
                temp.iId = treasure.iId;
                if (UITool.f_GetHowEquip(treasure.iId) != "")
                    continue;
                else
                    treaSeverIDList.Add(temp);
            }
        }
        for (int i = 0; i < treaSeverIDList.Count; i++)
        {
            if (treaSeverIDList.Count < severTreasId.Length)
                return severTreasId;
            if (i + 1 <= severTreasId.Length)
            {
                severTreasId[i] = treaSeverIDList[i].iId;
            }
        }
        return severTreasId;
    }

    //自动选择
    private void OnAutomaticSelectClick(GameObject go, object obj1, object obj2)
    {
        if (_treasureList == null || _treasureList.Count < 1)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(800));
            return;
        }
        if (_treasureCostIcon.sprite2D != null)
        {
            return;
        }
        TreasurePoolDT treasureData = _treasureList[0] as TreasurePoolDT;
        _btnTreasureCostItem.SetActive(true);
        _arrayTreasSeverId = null;
        _arrayTreasSeverId = f_GetSeverTreasureId(treasureData.m_TreasureDT.iId);
        _treasureCostIcon.sprite2D = UITool.f_GetIconSprite(treasureData.m_TreasureDT.iIcon);
        string tempName = treasureData.m_TreasureDT.szName;
        _treasureCostImp.spriteName = UITool.f_GetImporentColorName(treasureData.m_TreasureDT.iImportant, ref tempName);

    }

    //熔炼
    private void OnTreasClick(GameObject go, object obj1, object obj2)
    {
        if (_treasureCostIcon.sprite2D == null || _showTreasureIcon.sprite2D == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(801));
            return;
        }
        SocketCallbackDT callbackDT = new SocketCallbackDT();
        callbackDT.m_ccCallbackSuc = f_GetChangeTreasureSuc;
        callbackDT.m_ccCallbackFail = f_GetChangeTreasureFail;
        TransmigrationTreasurePoolDT poolDT = Data_Pool.m_TransmigrationTreasurePool.f_GetForId((long)_targetTreasID) as TransmigrationTreasurePoolDT;
        Data_Pool.m_TransmigrationTreasurePool.f_GetTransTreasureRequest(_targetTreasID, poolDT.m_TransmigrationTreasureDT.iCost, _arrayTreasSeverId, callbackDT);

    }

    //请求转换成功
    private void f_GetChangeTreasureSuc(object obj)
    {
        _treasureList = GetCanTransmigrationList(_targetTreasID);
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item = new AwardPoolDT();
        item.f_UpdateByInfo((byte)EM_ResourceType.TreasureFragment, _targetTreasID, 1);
        awardList.Add(item);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
                                           new object[] { awardList });
        UITool.f_OpenOrCloseWaitTip(false);
        _treasureCostIcon.sprite2D = null;
        _arrayTreasSeverId = null;
        _btnTreasureCostItem.SetActive(false);
    }

    //请求转换失败
    private void f_GetChangeTreasureFail(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(802) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }

    //添加法宝事件
    private void OnAddTreasureClick(GameObject go, object obj1, object obj2)
    {
        if (_treasureList == null || _treasureList.Count < 1)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(803));
            return;
        }
        _selectTreasPop.SetActive(true);
        f_SetSelectInfoTreasure();
    }

    //关闭选择法宝界面
    private void OnCloseSelectClick(GameObject go, object obj1, object obj2)
    {
        _selectTreasPop.SetActive(false);
    }

}
