using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AuraTypePage : UIFramwork
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
        UpdateMessFightType();

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

        AddGOReference("Panel/Anchor-Center/MessFightType");
        AddGOReference("Panel/Anchor-Center/MessFightType/IconFightType");
        AddGOReference("Panel/Anchor-Center/MessFightType/Level");

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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraTypePage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

    private void UpdateMessFightType()
    {
        Dictionary<int, int> dict_FightType = new Dictionary<int, int>();
        RolePropertyTools.GetDictAuraType(ref dict_FightType);
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_AuraTypeSC.f_GetAll();
        string mes = "";
        string name = "";
        string icon = "";
        string lbLevel = "";
        string pro = "";
        for (int i = 0; i < dict_FightType.Count; i++)
        {
            int type = dict_FightType.ElementAt(i).Key;
            int level = dict_FightType.ElementAt(i).Value;
            if (level <= 1) continue;
            switch ((EM_CardFightType)type)
            {
                case EM_CardFightType.eCardPhyAtt:
                    name = CommonTools.f_GetTransLanguage(2315);
                    break;
                case EM_CardFightType.eCardMagAtt:
                    name = CommonTools.f_GetTransLanguage(2316);
                    break;
                case EM_CardFightType.eCardSup:
                    name = CommonTools.f_GetTransLanguage(2317);
                    break;
                case EM_CardFightType.eCardTank:
                    name = CommonTools.f_GetTransLanguage(2318);
                    break;
                case EM_CardFightType.eCardKiller:
                    name = CommonTools.f_GetTransLanguage(2322);
                    break;
                case EM_CardFightType.eCardPhysician:
                    name = CommonTools.f_GetTransLanguage(2323);
                    break;
                case EM_CardFightType.eCardLogistics:
                    name = CommonTools.f_GetTransLanguage(2324);
                    break;
                case EM_CardFightType.eCardAll:
                    name = CommonTools.f_GetTransLanguage(2325);
                    break;
            }

            mes += string.Format(CommonTools.f_GetTransLanguage(2320), name, level) + "\n";
            if (icon == "")
            {
                icon += "IconType_" + type;
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
            List<AuraTypeDT> auraTypeDTs = RolePropertyTools.GetConfigAuraTypeByType(listData, type);
            if((EM_CardFightType)type == EM_CardFightType.eCardAll)
            {
                AuraTypeDT auraTypeDT = auraTypeDTs[0] as AuraTypeDT;
                if (auraTypeDT.szParam == null) continue;
                int iMagicId = int.Parse(auraTypeDT.szParam);
                MagicDT tMagic = glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(iMagicId) as MagicDT;
                string s = tMagic.szReadme;
                //BufDT tbuff = glo_Main.GetInstance().m_SC_Pool.m_BufSC.f_GetSC(tMagic.iBufId1) as BufDT;
                //string s = tbuff.strReadme;
                pro += "[EC2727]" + string.Format(CommonTools.f_GetTransLanguage(2326), CommonTools.f_GetTransLanguage(2319)) + s + "\n";
                continue;
            }
            for (int k = 0; k < auraTypeDTs.Count; k++)
            {
                AuraTypeDT auraTypeDT = auraTypeDTs[k];
                if (auraTypeDT.szParam == null) continue;
                int iMagicId = int.Parse(auraTypeDT.szParam);
                MagicDT tMagic = glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(iMagicId) as MagicDT;
                string s = tMagic.szReadme;
                //BufDT tbuff = glo_Main.GetInstance().m_SC_Pool.m_BufSC.f_GetSC(tMagic.iBufId1) as BufDT;
                //string s = tbuff.strReadme;
                string mApply = name;
                if (auraTypeDT.iIsAll == 1)
                {
                    mApply = CommonTools.f_GetTransLanguage(2319);
                }
                AuraTypeDT nextAuraTypeDT = k + 1 < auraTypeDTs.Count ? auraTypeDTs[k + 1] : null;
                if (auraTypeDT.iLevel == level || (nextAuraTypeDT != null && level > auraTypeDT.iLevel && level < nextAuraTypeDT.iLevel))
                {
                    pro += "[EC2727]" + string.Format(CommonTools.f_GetTransLanguage(2321), auraTypeDT.iLevel, mApply) + s + "\n";
                }
                else
                {
                    pro += "[827F7F]" + string.Format(CommonTools.f_GetTransLanguage(2321), auraTypeDT.iLevel, mApply) + s + "\n";
                }
            }
        }
        UISprite IconFightType = f_GetObject("IconFightType").GetComponent<UISprite>();
        IconFightType.spriteName = icon;//"IconType_" + fightType;
        UILabel Level = f_GetObject("Level").GetComponent<UILabel>();
        Level.text = lbLevel;
        UILabel MessFightType = f_GetObject("MessFightType").GetComponent<UILabel>();
        MessFightType.text = mes;
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
        UISprite SprIconCamp = item.Find("SprIconFightType").GetComponent<UISprite>();
        SprIconCamp.spriteName = "IconType_" + tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardFightType;
    }
}
