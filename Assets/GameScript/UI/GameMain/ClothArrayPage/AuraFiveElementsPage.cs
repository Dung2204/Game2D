using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AuraFiveElementsPage : UIFramwork
{
    private UIWrapComponent CardWrapComponent = null;

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);


        if (CardWrapComponent == null)
        {
            CardWrapComponent = new UIWrapComponent(195, 2, 805, 5, f_GetObject("CardItemParent"), f_GetObject("CardItem"), Data_Pool.m_TeamPool.f_GetAll(), CardItemUpdateByInfo, null);
        }
        CardWrapComponent.f_UpdateList(Data_Pool.m_TeamPool.f_GetAll());
        CardWrapComponent.f_ResetView();
        f_GetObject("CardItemParent").GetComponent<UIGrid>().Reposition();
        UpdateMess();

    }

    protected override void f_Create()
    {
        _InitReference();
        base.f_Create();
    }

    private void _InitReference()
    {
        AddGOReference("Panel/Anchor-Center/BtnClose");
        AddGOReference("Panel/Anchor-Center/BtnBlack");
        AddGOReference("Panel/Anchor-Center/CardScrollView/CardItemParent");
        AddGOReference("Panel/Anchor-Center/CardScrollView/CardItem");

        AddGOReference("Panel/Anchor-Center/Mess");
        AddGOReference("Panel/Anchor-Center/Mess/Icon");
        AddGOReference("Panel/Anchor-Center/Mess/Level");

        AddGOReference("Panel/Anchor-Center/Property/InfoProperty");
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlack", OnBtnBlackClick);
        f_RegClickEvent("BtnClose", OnBtnBlackClick);
    }

    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraFiveElementsPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

    private void UpdateMess()
    {
        Dictionary<int, int> dict_FightType = new Dictionary<int, int>();
        RolePropertyTools.GetDictAuraFiveEle(ref dict_FightType);
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_AuraFiveElementsSC.f_GetAll();
        string mes = "";
        string name = "";
        string icon = "";
        string lbLevel = "";
        string pro = "";
        for (int i = 0; i < dict_FightType.Count; i++)
        {
            int type = dict_FightType.ElementAt(i).Key;
            int level = dict_FightType.ElementAt(i).Value;
            if (level <= 2) continue;
            switch ((EM_CardEle)type)
            {
                case EM_CardEle.eCardEle1:
                    name = CommonTools.f_GetTransLanguage(2328);
                    break;
                case EM_CardEle.eCardEle2:
                    name = CommonTools.f_GetTransLanguage(2329);
                    break;
                case EM_CardEle.eCardEle3:
                    name = CommonTools.f_GetTransLanguage(2330);
                    break;
                case EM_CardEle.eCardEle4:
                    name = CommonTools.f_GetTransLanguage(2331);
                    break;
                case EM_CardEle.eCardEle5:
                    name = CommonTools.f_GetTransLanguage(2332);
                    break;
            }
            mes += string.Format(CommonTools.f_GetTransLanguage(2333), name, level) + "\n";
            if (icon == "")
            {
                icon += "IconEle_" + type;
            }
            else
            {
                icon += "_" + type;
            }
            if (lbLevel == "")
            {
                lbLevel += "" + level;
            }
            else
            {
                lbLevel += "/" + level;
            }
            // lấy config type
            pro += "[FFFFFF]" + name + ":\n";
            List<AuraFiveElementsDT> auraFiveElementsDTs = RolePropertyTools.GetConfigAuraFiveEleByType(listData, type);
            if (auraFiveElementsDTs.Count == 0) continue;
            for (int k = 0; k < auraFiveElementsDTs.Count; k++)
            {
                AuraFiveElementsDT auraFiveElementsDT = auraFiveElementsDTs[k];
                if (auraFiveElementsDT.szParam == null) continue;
                int iMagicId = int.Parse(auraFiveElementsDT.szParam);
                MagicDT tMagic = glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(iMagicId) as MagicDT;
                string s = tMagic.szReadme;
                //BufDT tbuff = glo_Main.GetInstance().m_SC_Pool.m_BufSC.f_GetSC(tMagic.iBufId1) as BufDT;
                //string s = tbuff.strReadme;
                string mApply = name;
                if (auraFiveElementsDT.iIsAll == 1)
                {
                    mApply = CommonTools.f_GetTransLanguage(2319);
                }
                AuraFiveElementsDT nextAuraFiveElementsDT = k + 1 < auraFiveElementsDTs.Count ? auraFiveElementsDTs[k + 1] : null;
                if (auraFiveElementsDT.iLevel == level || (nextAuraFiveElementsDT != null && level > auraFiveElementsDT.iLevel && level < nextAuraFiveElementsDT.iLevel))
                {
                    pro += "[EC2727]" + string.Format(CommonTools.f_GetTransLanguage(2334), auraFiveElementsDT.iLevel, mApply) + s + "\n";
                }
                else
                {
                    pro += "[827F7F]" + string.Format(CommonTools.f_GetTransLanguage(2334), auraFiveElementsDT.iLevel, mApply) + s + "\n";
                }
            }
        }
        if(pro == "")
        {
            pro = mes = CommonTools.f_GetTransLanguage(2335); // không kích hoạt được nguyên tố nào
        }
        
        UISprite Icon = f_GetObject("Icon").GetComponent<UISprite>();
        Icon.spriteName = icon;//"IconType_" + fightType;
        UILabel Level = f_GetObject("Level").GetComponent<UILabel>();
        Level.text = lbLevel;
        int fiveEle = RolePropertyTools.f_GetElementalSeason();
        string nameEle = UITool.f_GetFiveElementNameById(fiveEle);
        string str = string.Format(CommonTools.f_GetTransLanguage(2336), nameEle, nameEle);
        UILabel Mess = f_GetObject("Mess").GetComponent<UILabel>();
        Mess.text = mes + "\n [FFFFFF]" + str;
        UILabel InfoProperty = f_GetObject("InfoProperty").GetComponent<UILabel>();
        InfoProperty.text = pro;
    }

    private void CardItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        TeamPoolDT tTeamPoolDT = dt as TeamPoolDT;

        UI2DSprite SprHeadIcon = item.Find("SprHeadIcon").GetComponent<UI2DSprite>();
        SprHeadIcon.sprite2D = UITool.f_GetIconSpriteByCardId(tTeamPoolDT.m_CardPoolDT);
        UISprite SprBorder = item.Find("SprBorder").GetComponent<UISprite>();
        SprBorder.spriteName = UITool.f_GetImporentCase(tTeamPoolDT.m_CardPoolDT.m_CardDT.iImportant);
        UISprite SprIcon = item.Find("SprIcon").GetComponent<UISprite>();
        SprIcon.spriteName = "IconEle_" + tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardEle;
    }
}
