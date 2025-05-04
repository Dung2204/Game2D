using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AuraCampPage : UIFramwork
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
        UpdateMessAuraCamp();

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

        AddGOReference("Panel/Anchor-Center/MessCamp");
        AddGOReference("Panel/Anchor-Center/MessCamp/IconCamp");
        AddGOReference("Panel/Anchor-Center/MessCamp/Level");

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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraCampPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

    private void CardItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        TeamPoolDT tTeamPoolDT = dt as TeamPoolDT;

        UI2DSprite SprHeadIcon = item.Find("SprHeadIcon").GetComponent<UI2DSprite>();
        SprHeadIcon.sprite2D = UITool.f_GetIconSpriteByCardId(tTeamPoolDT.m_CardPoolDT);
        UISprite SprBorder = item.Find("SprBorder").GetComponent<UISprite>();
        SprBorder.spriteName = UITool.f_GetImporentCase(tTeamPoolDT.m_CardPoolDT.m_CardDT.iImportant);
        UISprite SprIconCamp = item.Find("SprIconCamp").GetComponent<UISprite>();
        SprIconCamp.spriteName = "IconCamp_" + tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardCamp;
    }

    private void UpdateMessAuraCamp()
    {
        string mes = "";
        string nameCamp = "";
        string icon = "";
        string lbLevel = "";
        string pro = "";
        Dictionary<int, int> dict_AuraCamp = new Dictionary<int, int>();
        RolePropertyTools.GetDictAuraCamp(ref dict_AuraCamp);
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_AuraCampSC.f_GetAll();
        for (int i = 0; i < dict_AuraCamp.Count; i++)
        {
            int camp = dict_AuraCamp.ElementAt(i).Key;
            int level = dict_AuraCamp.ElementAt(i).Value;
            if (level <= 1) continue;
            switch ((EM_CardCamp)camp)
            {
                case EM_CardCamp.eCardMain:
                    nameCamp = CommonTools.f_GetTransLanguage(111);
                    break;
                case EM_CardCamp.eCardWei:
                    nameCamp = CommonTools.f_GetTransLanguage(112);
                    break;
                case EM_CardCamp.eCardShu:
                    nameCamp = CommonTools.f_GetTransLanguage(113);
                    break;
                case EM_CardCamp.eCardWu:
                    nameCamp = CommonTools.f_GetTransLanguage(114);
                    break;
                case EM_CardCamp.eCardGroupHero:
                    nameCamp = CommonTools.f_GetTransLanguage(115);
                    break;
                case EM_CardCamp.eCardNo:
                    break;
            }
            if (camp == 0)
            {
                mes = CommonTools.f_GetTransLanguage(2281);
            }
            else
            {
                mes += string.Format(CommonTools.f_GetTransLanguage(2279), nameCamp, level) + "\n";
            }
            if (icon == "")
            {
                icon += "IconCamp_" + camp;
            }
            else
            {
                icon += "_" + camp;
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
            pro += "[FFFFFF]" + nameCamp + ":\n";
            List<AuraCampDT> auraCampDTs = RolePropertyTools.GetConfigAuraCampByType(listData, 1);
            for (int k = 0; k < auraCampDTs.Count; k++)
            {
                AuraCampDT auraCampDT = auraCampDTs[k];
                if (auraCampDT.szParam == null) continue;
                string[] propertyGroup = auraCampDT.szParam.Split('#');
                string s = "";
                for (int j = 0; j < propertyGroup.Length; j++)
                {
                    string[] propertyItem = propertyGroup[j].Split(';');
                    s += UITool.f_GetProName((EM_RoleProperty)int.Parse(propertyItem[0])) + " " + UITool.f_GetPercentagePro(int.Parse(propertyItem[0]), int.Parse(propertyItem[1])) + "    ";
                }
                AuraCampDT nextAuraCampDT = k + 1 < auraCampDTs.Count ? auraCampDTs[k + 1] : null;
                if (auraCampDT.iLevel == level || (nextAuraCampDT != null && level > auraCampDT.iLevel && level < nextAuraCampDT.iLevel))
                {
                    pro += "[EC2727]" + string.Format(CommonTools.f_GetTransLanguage(2280), auraCampDT.iLevel) + s + "\n";
                }
                else
                {
                    pro += "[827F7F]" + string.Format(CommonTools.f_GetTransLanguage(2280), auraCampDT.iLevel) + s + "\n";
                }

            }
        }
        UISprite IconCamp = f_GetObject("IconCamp").GetComponent<UISprite>();
        IconCamp.spriteName = icon;
        UILabel Level = f_GetObject("Level").GetComponent<UILabel>();
        Level.text = lbLevel;
        UILabel MessCamp = f_GetObject("MessCamp").GetComponent<UILabel>();
        MessCamp.text = mes;
        UILabel InfoProperty = f_GetObject("InfoProperty").GetComponent<UILabel>();
        InfoProperty.text = pro;
    }
}
