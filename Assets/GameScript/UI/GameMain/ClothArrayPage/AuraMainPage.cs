using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraMainPage : UIFramwork
{
    public GameObject [] m_aBtn;
    public GameObject[] m_aContent;
    private int mSelect = 0;
    private UIWrapComponent CampWrapComponent = null;
    private UIWrapComponent mCampWrapComponent
    {
        get
        {
            if (CampWrapComponent == null)
            {
                CampWrapComponent = new UIWrapComponent(180, 1, 1300, 5, f_GetObject("CampParent"), f_GetObject("CampItem"), GetListCamp(), ItemCampUpdateByInfo, null);
            }
            return CampWrapComponent;
        }
    }
    private UIWrapComponent TypeWrapComponent = null;
    private UIWrapComponent mTypeWrapComponent
    {
        get
        {
            if (TypeWrapComponent == null)
            {
                TypeWrapComponent = new UIWrapComponent(180, 1, 1300, 5, f_GetObject("TypeParent"), f_GetObject("TypeItem"), GetListType(), ItemTypeUpdateByInfo, null);
            }
            return TypeWrapComponent;
        }
    }
    private UIWrapComponent EleWrapComponent = null;
    private UIWrapComponent mEleWrapComponent
    {
        get
        {
            if (EleWrapComponent == null)
            {
                EleWrapComponent = new UIWrapComponent(180, 1, 1300, 5, f_GetObject("EleParent"), f_GetObject("EleItem"), GetListEle(), ItemEleUpdateByInfo, null);
            }
            return EleWrapComponent;
        }
    }
    BattleAuraData mBattleAuraData;
    
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e != null)
        {
            mBattleAuraData = (BattleAuraData)e;
        }
        mSelect = 0;

        mCampWrapComponent.f_UpdateList(GetListCamp());
        mTypeWrapComponent.f_UpdateList(GetListType());
        mEleWrapComponent.f_UpdateList(GetListEle());

        mCampWrapComponent.f_ResetView();
        mTypeWrapComponent.f_ResetView();
        mEleWrapComponent.f_ResetView();

        UpdateContent();
    }
    
    protected override void f_Create()
    {
        base.f_Create();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBG", OnBtnBlackClick);
        f_RegClickEvent("BtnClose", OnBtnBlackClick);

        f_RegClickEvent("BtnAuraCamp", OnBtnClick, 0);
        f_RegClickEvent("BtnAuraType", OnBtnClick, 1);
        f_RegClickEvent("BtnAuraEle", OnBtnClick, 2);

        //for (int i = 0; i < 3; i++)
        //{
        //    f_RegClickEvent(m_aContent[i], OnBtnClick, i);
        //}
        
    }

    private void OnBtnClick(GameObject go, object obj1, object obj2)
    {
        mSelect = (int)obj1;
        UpdateContent();
    }
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraMainPage, UIMessageDef.UI_CLOSE);
    }

    public void UpdateContent()
    {
        for(int i = 0; i < 3; i++)
        {
            SelectBtn(m_aBtn[i].transform, i == mSelect);
            m_aContent[i].SetActive(i == mSelect);
        }
    }
    private void SelectBtn(Transform go,bool select)
    {
        go.Find("Select").gameObject.SetActive(select);
    }
    public class PopupAuraClass : NBaseSCDT
    {
        public int index = 0;
    }

    public List<NBaseSCDT> GetListCamp()
    {
        Dictionary<int, int> dict_AuraCamp = new Dictionary<int, int>();
        if (mBattleAuraData != null)
        {
            dict_AuraCamp = mBattleAuraData.dict_AuraCamp;
        }
        else
        {
            RolePropertyTools.GetDictAuraCamp(ref dict_AuraCamp);
        }
        List<NBaseSCDT> mList = new List<NBaseSCDT>();
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_AuraCampSC.f_GetAll();
        for (int i = (int)EM_CardCamp.eCardWei; i < (int)EM_CardCamp.eCardNo; i++)
        {
            PopupAuraClass item = new PopupAuraClass();
            item.iId = i;
            item.index = i;
            List<AuraCampDT> auraDTs = RolePropertyTools.GetConfigAuraCampByType(listData, i);
            for (int k = 0; k < auraDTs.Count; k++)
            {
                AuraCampDT auraDT = auraDTs[k];
                if (auraDT.szParam == null) continue;
                int value = 0;
                if (dict_AuraCamp.TryGetValue(auraDT.iCamp, out value))
                {
                    if (auraDT.iLevel <= value)
                    {
                        item.index = 10000 + i;
                        break;
                    }
                }
            }
            mList.Add(item);
        }
        mList.Sort(delegate (NBaseSCDT a, NBaseSCDT b)
        {
            PopupAuraClass value1 = a as PopupAuraClass;
            PopupAuraClass value2 = b as PopupAuraClass;
            return value2.index - value1.index;
        });
        return mList;
    }

    public List<NBaseSCDT> GetListType()
    {
        Dictionary<int, int> dict_FightAuraType = new Dictionary<int, int>();

        if (mBattleAuraData != null)
        {
            dict_FightAuraType = mBattleAuraData.dict_AuraType;
        }
        else
        {
            RolePropertyTools.GetDictAuraType(ref dict_FightAuraType);
        }
        List<NBaseSCDT> mList = new List<NBaseSCDT>();
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_AuraTypeSC.f_GetAll();
        for (int i = (int)EM_CardFightType.eCardAll; i < (int)EM_CardFightType.eCardLogistics; i++)
        {
            PopupAuraClass item = new PopupAuraClass();
            item.iId = i;
            item.index = i;
            List<AuraTypeDT> auraDTs = RolePropertyTools.GetConfigAuraTypeByType(listData, i);
            for (int k = 0; k < auraDTs.Count; k++)
            {
                AuraTypeDT auraDT = auraDTs[k];
                if (auraDT.szParam == null) continue;
                int value = 0;
                if (dict_FightAuraType.TryGetValue(auraDT.iType, out value))
                {

                    if (auraDT.iLevel <= value)
                    {
                        item.index = 10000 + i;
                        break;
                    }
                }
            }
            mList.Add(item);
        }
        mList.Sort(delegate (NBaseSCDT a, NBaseSCDT b)
        {
            PopupAuraClass value1 = a as PopupAuraClass;
            PopupAuraClass value2 = b as PopupAuraClass;
            return value2.index - value1.index;
        });
        return mList;
    }

    public List<NBaseSCDT> GetListEle()
    {
        Dictionary<int, int> dict_AuraEle = new Dictionary<int, int>();
        if (mBattleAuraData != null)
        {
            dict_AuraEle = mBattleAuraData.dict_AuraEle;
        }
        else
        {
            RolePropertyTools.GetDictAuraFiveEle(ref dict_AuraEle);
        }
        List<NBaseSCDT> mList = new List<NBaseSCDT>();
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_AuraFiveElementsSC.f_GetAll();
        for (int i = (int)EM_CardEle.eCardEle1; i <= (int)EM_CardEle.eCardEle5; i++)
        {
            PopupAuraClass item = new PopupAuraClass();
            item.iId = i;
            item.index = i;
            List<AuraFiveElementsDT> auraDTs = RolePropertyTools.GetConfigAuraFiveEleByType(listData, i);

            for (int k = 0; k < auraDTs.Count; k++)
            {
                AuraFiveElementsDT auraDT = auraDTs[k];
                if (auraDT.szParam == null) continue;
                int value = 0;
                if (dict_AuraEle.TryGetValue(auraDT.iType, out value))
                {
                    if (auraDT.iLevel <= value)
                    {
                        item.index = 10000 + i;
                        break;
                    }
                }
            }
            mList.Add(item);
        }
        mList.Sort(delegate (NBaseSCDT a, NBaseSCDT b)
        {
            PopupAuraClass value1 = a as PopupAuraClass;
            PopupAuraClass value2 = b as PopupAuraClass;
            return value2.index - value1.index;
        });
        return mList;

    }

    private void ItemCampUpdateByInfo(Transform item, NBaseSCDT dt)
    {
        Dictionary<int, int> dict_AuraCamp = new Dictionary<int, int>();
        if (mBattleAuraData != null)
        {
            dict_AuraCamp = mBattleAuraData.dict_AuraCamp;
        }
        else
        {
            RolePropertyTools.GetDictAuraCamp(ref dict_AuraCamp);
        }
        
        UISprite Icon = item.Find("icon").GetComponent<UISprite>();
        Icon.spriteName = "IconCamp_" + dt.iId;
        UILabel Name = item.Find("name").GetComponent<UILabel>();
        string strName = "";
        switch ((EM_CardCamp)dt.iId)
        {
            case EM_CardCamp.eCardMain:
                strName = CommonTools.f_GetTransLanguage(111);
                break;
            case EM_CardCamp.eCardWei:
                strName = CommonTools.f_GetTransLanguage(112);
                break;
            case EM_CardCamp.eCardShu:
                strName = CommonTools.f_GetTransLanguage(113);
                break;
            case EM_CardCamp.eCardWu:
                strName = CommonTools.f_GetTransLanguage(114);
                break;
            case EM_CardCamp.eCardGroupHero:
                strName = CommonTools.f_GetTransLanguage(115);
                break;
            case EM_CardCamp.eCardNo:
                break;
        }
        Name.text = strName;
        UILabel Desc = item.Find("desc").GetComponent<UILabel>();
        string strDesc = "";
        List <NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_AuraCampSC.f_GetAll();
        List<AuraCampDT> auraDTs = RolePropertyTools.GetConfigAuraCampByType(listData, dt.iId);
        for (int k = 0; k < auraDTs.Count; k++)
        {
            
            AuraCampDT auraDT = auraDTs[k];
            if (auraDT.szParam == null) continue;
            string[] propertyGroup = auraDT.szParam.Split('#');
            string s = "";
            for (int j = 0; j < propertyGroup.Length; j++)
            {
                string[] propertyItem = propertyGroup[j].Split(';');
                s += UITool.f_GetProName((EM_RoleProperty)int.Parse(propertyItem[0])) + " " + UITool.f_GetPercentagePro(int.Parse(propertyItem[0]), int.Parse(propertyItem[1])) + "    ";
            }
            if (strDesc != "") strDesc += "\n";
            int value = 0;
            AuraCampDT nextAuraDT = k + 1 < auraDTs.Count ? auraDTs[k + 1] : null;
            if (dict_AuraCamp.TryGetValue(auraDT.iCamp, out value))
            {
                if (auraDT.iLevel == value || (nextAuraDT != null && value > auraDT.iLevel && value < nextAuraDT.iLevel))
                {
                    strDesc += "[09a109]" + string.Format(CommonTools.f_GetTransLanguage(2280), auraDT.iLevel) + s;
                    continue;
                }
            }
            strDesc += "[5D250C]" + string.Format(CommonTools.f_GetTransLanguage(2280), auraDT.iLevel) + s;
        }
        Desc.text = strDesc;
    }
    private void ItemTypeUpdateByInfo(Transform item, NBaseSCDT dt)
    {
        Dictionary<int, int> dict_FightType = new Dictionary<int, int>();
        if (mBattleAuraData != null)
        {
            dict_FightType = mBattleAuraData.dict_AuraType;
        }
        else
        {
            RolePropertyTools.GetDictAuraType(ref dict_FightType);
        }
        
        UISprite Icon = item.Find("icon").GetComponent<UISprite>();
        Icon.spriteName = "IconType_" + dt.iId;
        UILabel Name = item.Find("name").GetComponent<UILabel>();
        string strName = "";
        switch ((EM_CardFightType)dt.iId)
        {
            case EM_CardFightType.eCardPhyAtt:
                strName = CommonTools.f_GetTransLanguage(2315);
                break;
            case EM_CardFightType.eCardMagAtt:
                strName = CommonTools.f_GetTransLanguage(2316);
                break;
            case EM_CardFightType.eCardSup:
                strName = CommonTools.f_GetTransLanguage(2317);
                break;
            case EM_CardFightType.eCardTank:
                strName = CommonTools.f_GetTransLanguage(2318);
                break;
            case EM_CardFightType.eCardKiller:
                strName = CommonTools.f_GetTransLanguage(2322);
                break;
            case EM_CardFightType.eCardPhysician:
                strName = CommonTools.f_GetTransLanguage(2323);
                break;
            case EM_CardFightType.eCardLogistics:
                strName = CommonTools.f_GetTransLanguage(2324);
                break;
            case EM_CardFightType.eCardAll:
                strName = CommonTools.f_GetTransLanguage(2325);
                break;
        }
        Name.text = strName;
        UILabel Desc = item.Find("desc").GetComponent<UILabel>();
        string strDesc = "";
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_AuraTypeSC.f_GetAll();
        List<AuraTypeDT> auraDTs = RolePropertyTools.GetConfigAuraTypeByType(listData, dt.iId);
        for (int k = 0; k < auraDTs.Count; k++)
        {
            AuraTypeDT auraDT = auraDTs[k];
            if (auraDT.szParam == null) continue;
            int iMagicId = int.Parse(auraDT.szParam);
            MagicDT tMagic = glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(iMagicId) as MagicDT;
            string s = tMagic.szReadme;
            string mApply = strName;
            if (auraDT.iIsAll == 1)
            {
                mApply = CommonTools.f_GetTransLanguage(2319);
            }
            if (strDesc != "") strDesc += "\n";
            int value = 0;
            AuraTypeDT nextAuraDT = k + 1 < auraDTs.Count ? auraDTs[k + 1] : null;
            if (dict_FightType.TryGetValue(auraDT.iType, out value))
            {
                if (auraDT.iLevel == value || (nextAuraDT != null && value > auraDT.iLevel && value < nextAuraDT.iLevel))
                {
                    strDesc += "[09a109]" + string.Format(CommonTools.f_GetTransLanguage(2321), auraDT.iLevel, mApply) + s;
                    continue;
                }
            }
            strDesc += "[5D250C]" + string.Format(CommonTools.f_GetTransLanguage(2321), auraDT.iLevel, mApply) + s;
        }
        Desc.text = strDesc;
    }
    private void ItemEleUpdateByInfo(Transform item, NBaseSCDT dt)
    {
        Dictionary<int, int> dict_AuraEle = new Dictionary<int, int>();
        if (mBattleAuraData != null)
        {
            dict_AuraEle = mBattleAuraData.dict_AuraEle;
        }
        else
        {
            RolePropertyTools.GetDictAuraFiveEle(ref dict_AuraEle);
        }
        UISprite Icon = item.Find("icon").GetComponent<UISprite>();
        Icon.spriteName = "IconEle_" + dt.iId;
        UILabel Name = item.Find("name").GetComponent<UILabel>();
        string strName = "";
        switch ((EM_CardEle)dt.iId)
        {
            case EM_CardEle.eCardEle1:
                strName = CommonTools.f_GetTransLanguage(2328);
                break;
            case EM_CardEle.eCardEle2:
                strName = CommonTools.f_GetTransLanguage(2329);
                break;
            case EM_CardEle.eCardEle3:
                strName = CommonTools.f_GetTransLanguage(2330);
                break;
            case EM_CardEle.eCardEle4:
                strName = CommonTools.f_GetTransLanguage(2331);
                break;
            case EM_CardEle.eCardEle5:
                strName = CommonTools.f_GetTransLanguage(2332);
                break;
        }
        Name.text = strName;
        UILabel Desc = item.Find("desc").GetComponent<UILabel>();
        string strDesc = "";
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_AuraFiveElementsSC.f_GetAll();
        List<AuraFiveElementsDT> auraDTs = RolePropertyTools.GetConfigAuraFiveEleByType(listData, dt.iId);
        for (int k = 0; k < auraDTs.Count; k++)
        {
            AuraFiveElementsDT auraDT = auraDTs[k];
            if (auraDT.szParam == null) continue;
            int iMagicId = int.Parse(auraDT.szParam);
            MagicDT tMagic = glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(iMagicId) as MagicDT;
            string s = tMagic.szReadme;
            string mApply = strName;
            if (auraDT.iIsAll == 1)
            {
                mApply = CommonTools.f_GetTransLanguage(2319);
            }
            if (strDesc != "") strDesc += "\n";
            int value = 0;
            AuraFiveElementsDT nextAuraDT = k + 1 < auraDTs.Count ? auraDTs[k + 1] : null;
            if (dict_AuraEle.TryGetValue(auraDT.iType, out value))
            {
                if(auraDT.iLevel == value || (nextAuraDT != null && value > auraDT.iLevel && value < nextAuraDT.iLevel))
                {
                    strDesc += "[09a109]" + string.Format(CommonTools.f_GetTransLanguage(2334), auraDT.iLevel, mApply) + s;
                    continue;
                }
            }
            
            strDesc += "[5D250C]" + string.Format(CommonTools.f_GetTransLanguage(2334), auraDT.iLevel, mApply) + s;
            
        }
        Desc.text = strDesc;
    }
}
