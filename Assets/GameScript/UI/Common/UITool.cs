using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ccU3DEngine;
using System.Linq;
using System;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// UI相关的静态方法 放这里
/// </summary>
public class UITool
{

    public static void OnLanguageSelectionChaged(List<string> _languageList, UILabel _languageSelected)
    {
        Debug.LogError("OnDialog Confirm");
        string selectedValue = UIPopupList.current.value;
        int languageSet = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_LanguageSetting, 1);
        if(selectedValue == _languageList[languageSet])
        {
            return;
        }

        PopupMenuParams tParams = new PopupMenuParams("Chú ý", "Khởi động lại ứng dụng để áp dụng ngôn ngữ mới?", "Xác nhận", (x) => {
            //_languageSelected.text = selectedValue;
            LocalDataManager.f_SetLocalData<int>(LocalDataType.Int_LanguageSetting, _languageList.IndexOf(selectedValue));
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }, "Hủy", (x) => {

        });
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParams);
    }
    /// <summary>
    /// Icon显示同一使用这个方法,根据IconId获取sprite 
    /// </summary>
    /// <param name="iconId"></param>
    /// <returns></returns>
    public static Sprite f_GetIconSprite(int iconId)
    {
        return Data_Pool.m_IconDataPool.f_GetSprieById(iconId);
    }
	public static Sprite f_GetSkillIcon(string iconId)
    {
        Sprite tTexture2D = Resources.Load<Sprite>("UI/UIAltas/IconSprite/Skill_Icon/skill_" + iconId);
		if (tTexture2D == null)
		{
			tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("skill_" + iconId);
		}
        if (tTexture2D == null)
        {
            string strFile = "UI/UIAltas/IconSprite/Skill_Icon/skill_" + iconId;
            string[] strArrar = strFile.Split('/');
            string TextureFileName = strArrar[strArrar.Length - 1];
            strArrar[strArrar.Length - 1] = glo_Main.GetInstance().CodeName + "_" + TextureFileName;
            strFile = string.Join("/", strArrar);
            tTexture2D = Resources.Load<Sprite>(strFile);
        }
		if(tTexture2D == null)
		{
			tTexture2D = Resources.Load<Sprite>("UI/UIAltas/IconSprite/Skill_Icon/Temp_Icon");
		}
		return tTexture2D;
    }

    public static Sprite f_GetCardIcon(int cardId, string subString = "")
    {
        Sprite tTexture2D = null;
        if (tTexture2D == null)
        {
            tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite(subString + cardId.ToString());
        }
        if (tTexture2D == null)
        {
            tTexture2D = Resources.Load<Sprite>("UI/UIAltas/IconSprite/Card_Icon/" + cardId); ;
        }
        if (tTexture2D == null)
        {
            string strFile = "UI/UIAltas/IconSprite/Card_Icon/" + cardId;
            string[] strArrar = strFile.Split('/');
            string TextureFileName = strArrar[strArrar.Length - 1];
            strArrar[strArrar.Length - 1] = glo_Main.GetInstance().CodeName + "_" + TextureFileName;
            strFile = string.Join("/", strArrar);
            tTexture2D = Resources.Load<Sprite>(strFile);
        }
        if (tTexture2D == null)
        {
            tTexture2D = Resources.Load<Sprite>("UI/UIAltas/IconSprite/Card_Icon/1100");
			if(subString == "L4_")
				tTexture2D = Resources.Load<Sprite>("UI/UITexture/Dungeon/Legend/12011");
			if(subString == "L1_")
				tTexture2D = Resources.Load<Sprite>("UI/UIAltas/IconSprite/Skill_Icon/IconMob");
        }
        return tTexture2D;
    }

    public static Sprite f_GetGainCardIcon(int cardId, string subString = "")
    {
        Sprite tTexture2D = null;
        if (tTexture2D == null)
        {
            tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite(subString + cardId.ToString());
        }
        if (tTexture2D == null)
        {
            tTexture2D = Resources.Load<Sprite>("UI/UIAltas/IconSprite/Gain_Icon/" + cardId); ;
        }
        if (tTexture2D == null)
        {
            string strFile = "UI/UIAltas/IconSprite/Gain_Icon/" + cardId;
            string[] strArrar = strFile.Split('/');
            string TextureFileName = strArrar[strArrar.Length - 1];
            strArrar[strArrar.Length - 1] = glo_Main.GetInstance().CodeName + "_" + TextureFileName;
            strFile = string.Join("/", strArrar);
            tTexture2D = Resources.Load<Sprite>(strFile);
        }
        if (tTexture2D == null)
        {
            tTexture2D = Resources.Load<Sprite>("UI/UIAltas/IconSprite/Gain_Icon/1100");
        }
        return tTexture2D;
    }
    /// <summary>
    /// 根据模型ID获得RoleModel
    /// </summary>
    public static RoleModelDT f_GetRoleModelForiStatelId(int id)
    {
        return (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(id);
    }
    /// <summary>
    /// 设置物品Icon，如果为卡牌碎片和装备碎片则会添加“碎”字
    /// </summary>
    /// <param name="resourceType">资源类型</param>
    /// <param name="resourceID">资源id</param>
    /// <returns></returns>
    public static void f_SetIconSprite(UI2DSprite ui2dSprite, EM_ResourceType resourceType, int resourceID)
    {
        string fragmentLabelName = "fragementLabel_";
        Sprite sprite = null;
        if (resourceType == EM_ResourceType.Card)
        {
            sprite = f_GetIconSpriteByCardId(resourceID);
            ui2dSprite.sprite2D = sprite;
            if (ui2dSprite.transform.Find(fragmentLabelName) != null)
                GameObject.Destroy(ui2dSprite.transform.Find(fragmentLabelName).gameObject);
            return;
        }
        else if (resourceType == EM_ResourceType.CardFragment)//添加碎字
        {
            sprite = f_GetIconSpriteByCardId(resourceID);
            ui2dSprite.sprite2D = sprite;
            f_CreateFragmentLabelText(ui2dSprite);
            return;
        }
        else
        {
            ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
            resourceCommonDT.f_UpdateInfo((byte)resourceType, resourceID, 0);
            sprite = f_GetIconSprite(resourceCommonDT.mIcon);
            if (resourceType == EM_ResourceType.EquipFragment)//添加碎字
            {
                f_CreateFragmentLabelText(ui2dSprite);
            }
            else
            {
                if (ui2dSprite.transform.Find(fragmentLabelName) != null)
                    GameObject.Destroy(ui2dSprite.transform.Find(fragmentLabelName).gameObject);
            }
            ui2dSprite.sprite2D = sprite;
        }
    }
    public static void f_CreateFragmentLabelText(UI2DSprite ui2dSprite)
    {
        string fragmentLabelName = "fragementLabel_";
        if (ui2dSprite.transform.Find(fragmentLabelName) == null)
        {
            GameObject fragment = GameObject.Instantiate(Resources.Load("UI/UIPrefab/GameMain/LabelFragment")) as GameObject;
            fragment.name = fragmentLabelName;
            fragment.transform.SetParent(ui2dSprite.transform);
            fragment.transform.localEulerAngles = Vector3.zero;
            fragment.transform.localPosition = new Vector3(-28, -37, 0);
            fragment.transform.localScale = Vector3.one;
            fragment.transform.GetComponent<UILabel>().depth = ui2dSprite.depth + 1;
        }
    }
    public static string f_GetName(EM_ResourceType resourceType, int resourceID)
    {
        ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
        resourceCommonDT.f_UpdateInfo((byte)resourceType, resourceID, 0);
        return resourceCommonDT.mName;
    }


    public static  string GetGetWayBySocketId(int wayID)
    {
        string getWayMsg = "";
        switch (wayID)
        {
            case (int)SocketCommand.CS_UseItem:// 使用道具            “宝箱”
                getWayMsg = CommonTools.f_GetTransLanguage(10);
                break;
            case (int)SocketCommand.CS_ShopLotteryBuy:// 抽奖商店购买        “招募”
                getWayMsg = CommonTools.f_GetTransLanguage(11);
                break;
            case (int)SocketCommand.CS_CardSynthesis:// 卡牌合成               “卡牌合成”
                getWayMsg = CommonTools.f_GetTransLanguage(12);
                break;
            case (int)SocketCommand.CS_TurntableLottery://        “幸运转盘抽奖”
                getWayMsg = CommonTools.f_GetTransLanguage(2231);
                break;
        }
        return getWayMsg;
    }


    /// <summary>
    /// 根据品质来生成颜色
    /// 返回颜色对应的边框
    /// </summary>
    /// <returns> SpriteName</returns>
    public static string f_GetImporentColorName(int Imporent, ref string name)
    {
name = f_GetImporentForName(Imporent, name == "Protagonist 2" || name == "Protagonist 1" ?Data_Pool.m_UserData.m_szRoleName : name);
        return f_GetImporentCase(Imporent);
    }

    /// <summary>
    /// 获得品质颜色框
    /// </summary>
    /// <returns></returns>
    public static string f_GetImporentCase(int imporent)
    {
        if (imporent <= 0)
            imporent = (int)EM_Important.White;
        string spriteName = "Icon_";
        switch ((EM_Important)imporent)
        {
            case EM_Important.White:
                spriteName += "White";
                break;
            case EM_Important.Green:
                spriteName += "Green";
                break;
            case EM_Important.Blue:
                spriteName += "Blue";
                break;
            case EM_Important.Purple:
                spriteName += "Purple";
                break;
            case EM_Important.Oragen:
                spriteName += "Oragen";
                break;
            case EM_Important.Red:
                spriteName += "Red";
                break;
            case EM_Important.Gold:
                spriteName += "Gold";
                break;
        }
        return spriteName;
    }

    public static string f_GetImporentBGCase(int imporent)
    {
        if (imporent <= 0)
            imporent = (int)EM_Important.White;
        string spriteName = "Icon_BG";
        switch ((EM_Important)imporent)
        {
            case EM_Important.White:
                spriteName += "White";
                break;
            case EM_Important.Green:
                spriteName += "Green";
                break;
            case EM_Important.Blue:
                spriteName += "Blue";
                break;
            case EM_Important.Purple:
                spriteName += "Purple";
                break;
            case EM_Important.Oragen:
                spriteName += "Oragen";
                break;
            case EM_Important.Red:
                spriteName += "Red";
                break;
            case EM_Important.Gold:
                spriteName += "Gold";
                break;
        }
        return spriteName;
    }

    public static string f_GetCampName(int camp)
    {
        string campName = "";
        switch ((EM_CardCamp)camp)
        {
            case EM_CardCamp.eCardWei:
                campName += CommonTools.f_GetTransLanguage(1142);
                break;
            case EM_CardCamp.eCardShu:
                campName += CommonTools.f_GetTransLanguage(1143);
                break;
            case EM_CardCamp.eCardWu:
                campName += CommonTools.f_GetTransLanguage(1144);
                break;
            case EM_CardCamp.eCardGroupHero:
                campName += CommonTools.f_GetTransLanguage(1145);
                break;
        }
        return campName;
    }

    public static string f_ReplaceName(string name, string key, string to)
    {
        return name.Replace(key, to);
    }
    /// <summary>
    /// 获得名字的品质颜色
    /// </summary>
    /// <returns></returns>
    public static string f_GetImporentForName(int Imporent, string name)
    {
        string str = string.Empty;
        switch ((EM_Important)Imporent)
        {
            case EM_Important.White:
                str = "[fffaec]{0}[-]";
                break;
            case EM_Important.Green:
                str = "[35bd88]{0}[-]";
                break;
            case EM_Important.Blue:
                str = "[40b1f2]{0}[-]";
                break;
            case EM_Important.Purple:
                str = "[d867f0]{0}[-]";
                break;
            case EM_Important.Oragen:
                str = "[f16f29]{0}[-]";
                break;
            case EM_Important.Red:
                str = "[e73d34]{0}[-]";
                break;
            case EM_Important.Gold:
                str = "[e2cc3a]{0}[-]";
                break;
            case EM_Important.BlackGold:
                str = "[bb821e]{0}[-]";
                break;
            case EM_Important.ColorGload:
                str = "[ff8ecb]{0}[-]";
                break;
        }
        return string.Format(str, name);
    }
    /// <summary>
    /// 获得卡牌的势力
    /// </summary>
    /// <returns></returns>
    public static string f_GetCardCamp(int Camp)
    {
        switch ((EM_CardCamp)Camp)
        {
            case EM_CardCamp.eCardMain:
                return CommonTools.f_GetTransLanguage(1820);
            case EM_CardCamp.eCardWei:
                return CommonTools.f_GetTransLanguage(1821);
            case EM_CardCamp.eCardShu:
                return CommonTools.f_GetTransLanguage(1822);
            case EM_CardCamp.eCardWu:
                return CommonTools.f_GetTransLanguage(1823);
            case EM_CardCamp.eCardGroupHero:
                return CommonTools.f_GetTransLanguage(1824);
            case EM_CardCamp.eCardNo:
                break;
            default:
                break;
        }
        return "";
    }

    public static string f_GetCardCampForUISpriteName(int Camp)
    {
        switch ((EM_CardCamp)Camp)
        {
            case EM_CardCamp.eCardMain:
                return CommonTools.f_GetTransLanguage(1820);
            case EM_CardCamp.eCardWei:
                return "font_wei";
            case EM_CardCamp.eCardShu:
                return "font_shu";
            case EM_CardCamp.eCardWu:
                return "font_wu";
            case EM_CardCamp.eCardGroupHero:
                return "font_qun";
            case EM_CardCamp.eCardNo:
                break;
            default:
                break;
        }
        return "";
    }

    public static string f_GetCardImporent(int CardID)
    {
        CardDT tCardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(CardID) as CardDT;

        return f_GetImporentCase(tCardDT.iImportant);
    }
    /// <summary>
    /// 获得卡牌的战斗类型
    /// </summary>
    /// <returns></returns>
    public static string f_GetCardFightType(int type)
    {
        switch ((EM_CardFightType)type)
        {
            case EM_CardFightType.eCardPhyAtt:
                return CommonTools.f_GetTransLanguage(1825);
            case EM_CardFightType.eCardMagAtt:
                return CommonTools.f_GetTransLanguage(1826);
            case EM_CardFightType.eCardSup:
                return CommonTools.f_GetTransLanguage(1827);
            case EM_CardFightType.eCardTank:
                return CommonTools.f_GetTransLanguage(1828);
            case EM_CardFightType.eCardKiller:
                return CommonTools.f_GetTransLanguage(2322);
            case EM_CardFightType.eCardPhysician:
                return CommonTools.f_GetTransLanguage(2323);
            case EM_CardFightType.eCardLogistics:
                return CommonTools.f_GetTransLanguage(2324);
            case EM_CardFightType.eCardAll:
                return CommonTools.f_GetTransLanguage(2325);
        }
        return "";
    }
    /// <summary>
    /// Lấy tên nguyên tố
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string f_GetFiveElementNameById(int id)
    {
        string name = "";
        switch ((EM_CardEle)id)
        {
            case 0:
                name = CommonTools.f_GetTransLanguage(2327);
                break;
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
        if (name != "")
        {
            return "(" + name + ")";
        }
        return name;
    }
    /// <summary>
    /// 获得货币转换万(满10W转换为10W)
    /// </summary>
    /// <returns></returns>
    public static string f_GetMoney(int MoneyNum)
    {
        if (MoneyNum >= 100000)
            return (int)(MoneyNum / 1000) + CommonTools.f_GetTransLanguage(1829);
        return MoneyNum.ToString();
    }
    public static Sprite f_GetIconSpriteByCardCamp(int Camp)
    {
        switch ((EM_CardCamp)Camp)
        {
            case EM_CardCamp.eCardWei:
                return f_GetIconSprite(1);
            case EM_CardCamp.eCardShu:
                return f_GetIconSprite(2);
            case EM_CardCamp.eCardWu:
                return f_GetIconSprite(3);
            case EM_CardCamp.eCardGroupHero:
                return f_GetIconSprite(4);
            default:
                break;
        }
        return f_GetIconSprite(99999);
    }
    /// <summary>
    /// 根据卡牌Id显示头像(只可显示卡牌头像，不可显示碎片头像)
    /// </summary>
    /// <returns></returns>
    public static Sprite f_GetIconSpriteByCardId(CardPoolDT tCardPoolDT)
    {
        if (tCardPoolDT == null)
        {
            return null;
        }
        if (tCardPoolDT.m_FanshionableDressPoolDT != null)
        {
            return f_GetIconSprite(tCardPoolDT.m_FanshionableDressPoolDT.m_FashionableDressDT.iIcon);
        }
        else
        {
            if (tCardPoolDT.m_CardDT.iStatelId1 != 0)
            {
                RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(tCardPoolDT.m_CardDT.iStatelId1);
                if (roleModle != null)
                    return f_GetIconSprite(roleModle.iIcon);
            }
            else if (tCardPoolDT.m_CardDT.iStatelId2 != 0)
            {
                RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(tCardPoolDT.m_CardDT.iStatelId2);
                if (roleModle != null)
                    return f_GetIconSprite(roleModle.iIcon);
            }
        }
        return null;
    }

    /// <summary>
    /// 根据卡牌Id显示头像(只可显示卡牌头像，不可显示碎片头像)
    /// </summary>
    /// <returns></returns>
    public static Sprite f_GetIconSpriteByCardId(int cardId)
    {
        CardDT tCardDT;
		CardPoolDT tcardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(0);
        if (cardId == GameParamConst.PlayerRoleId)
        {
            CardPoolDT tCardPoolDT = Data_Pool.m_CardPool.f_GetPlayerRolePoolDT();
            tCardDT = tCardPoolDT.m_CardDT;
        }
        else
        {
            tCardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardId);
        }
		
		if(tCardDT != null)
		{
			if (tCardDT.iStatelId1 != 0)
			{
				RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(tCardDT.iStatelId1);
				if (roleModle != null)
					return f_GetIconSprite(roleModle.iIcon);
			}
			else if (tCardDT.iStatelId2 != 0)
			{
				RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(tCardDT.iStatelId2);
				if (roleModle != null)
					return f_GetIconSprite(roleModle.iIcon);
			}
		}
        return null;
    }
	
	public static Sprite f_GetIconSpriteBySexId(int sexId)
    {
		//My Code
        // if (tcardPoolDT.m_FanshionableDressPoolDT != null && (cardId == 1000 || cardId == 1001))
        // {
            // Sprite tSprite = f_GetIconSprite(tcardPoolDT.m_FanshionableDressPoolDT.m_FashionableDressDT.iIcon);
            // if (tSprite == null)
            // {
                // MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1830) + cardId);
                // return null;
            // }
            // else
            // {
                // return tSprite;
            // }
        // }
		
		//NEXT
		if (sexId == (int)EM_RoleSex.Man)
        {
            sexId = 10001;
        }
        else
        {
            FashionableDressDT tFashionableDressDT = glo_Main.GetInstance().m_SC_Pool.m_FashionableDressSC.f_GetSC(sexId) as FashionableDressDT;
            if (tFashionableDressDT != null)
            {
                sexId = tFashionableDressDT.iIcon;
            }
        }
		Sprite tSprite = f_GetIconSprite(sexId);
		return tSprite;
    }
	
	public static Sprite f_GetMyIcon(int cardId)
    {
		CardDT tCardDT;
		CardPoolDT tcardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(0);
		//My Code
        if (tcardPoolDT.m_FanshionableDressPoolDT != null && (cardId >= 1000 && cardId <= 1069))
        {
			MessageBox.ASSERT("" + cardId);
            Sprite tSprite = f_GetIconSprite(tcardPoolDT.m_FanshionableDressPoolDT.m_FashionableDressDT.iIcon);
            if (tSprite == null)
            {
                MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1830) + cardId);
                return null;
            }
            else
            {
                return tSprite;
            }
        }
		if (cardId == GameParamConst.PlayerRoleId)
        {
            CardPoolDT tCardPoolDT = Data_Pool.m_CardPool.f_GetPlayerRolePoolDT();
            tCardDT = tCardPoolDT.m_CardDT;
        }
        else
        {
            tCardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardId);
        }
		
        if (tCardDT.iStatelId1 != 0)
        {
            RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(tCardDT.iStatelId1);
            if (roleModle != null)
                return f_GetIconSprite(roleModle.iIcon);
        }
        else if (tCardDT.iStatelId2 != 0)
        {
            RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(tCardDT.iStatelId2);
            if (roleModle != null)
                return f_GetIconSprite(roleModle.iIcon);
        }
        return null;
    }

    public static Sprite f_GetIconSpriteByModelId(int modelId)
    {
        RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(modelId);
        if (roleModle != null)
            return f_GetIconSprite(roleModle.iIcon);
        return null;
    }

    public static Sprite f_GetIconSpriteByFashionable(int FashId)
    {
        FashionableDressDT tFashionableDressDT;
        tFashionableDressDT = glo_Main.GetInstance().m_SC_Pool.m_FashionableDressSC.f_GetSC(FashId) as FashionableDressDT;
        if (tFashionableDressDT == null)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1831) + FashId);
            return null;
        }
        else
        {
            return f_GetIconSprite(tFashionableDressDT.iIcon);
        }

    }
    /// <summary>
    /// 获得错误提示
    /// </summary>
    /// <param name="Error"></param>
    /// <returns></returns>
    public static string f_GetError(int Error)
    {
Debug.LogError("==========Error：======" + Error);
        switch ((eMsgOperateResult)Error)
        {
            case eMsgOperateResult.eOR_LevelLimit:
                return CommonTools.f_GetTransLanguage(1832);
            case eMsgOperateResult.eOR_TimesLimit:
                return CommonTools.f_GetTransLanguage(1833);
            case eMsgOperateResult.eOR_Sycee:
                return CommonTools.f_GetTransLanguage(1834);
            case eMsgOperateResult.eOR_Money:
                return CommonTools.f_GetTransLanguage(1835);
            case eMsgOperateResult.eOR_Energy:
                return CommonTools.f_GetTransLanguage(1836);
            case eMsgOperateResult.eOR_VipLimit:
                return CommonTools.f_GetTransLanguage(1837);
            case eMsgOperateResult.eOR_ItemError:
                return CommonTools.f_GetTransLanguage(1838);
            case eMsgOperateResult.eOR_CardError:
                return CommonTools.f_GetTransLanguage(1839);
            case eMsgOperateResult.eOR_CardFragmentError:
                return CommonTools.f_GetTransLanguage(1840);
            case eMsgOperateResult.eOR_EquipError:
                return CommonTools.f_GetTransLanguage(1841);
            case eMsgOperateResult.eOR_EquipFragmentError:
                return CommonTools.f_GetTransLanguage(1842);
            case eMsgOperateResult.eOR_TreasureError:
                return CommonTools.f_GetTransLanguage(1843);
            case eMsgOperateResult.eOR_TreasureFragmentError:
                return CommonTools.f_GetTransLanguage(1844);
            case eMsgOperateResult.eOR_AwakenEquipError:
                return CommonTools.f_GetTransLanguage(1845);
            default:
                return CommonTools.f_GetTransLanguage(1846) + ((eMsgOperateResult)Error).ToString();
        }
    }
    // <summary>
    /// 创建角色
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public static GameObject f_GetStatelObject(CardDT card, bool needToShowRedCard = true)
    {
        ResourceManager tmpResou = glo_Main.GetInstance().m_ResourceManager;
        //List<NBaseSCDT> tmpRole = glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetAll();
        //NBaseSCDT tmp = new NBaseSCDT();
        if (card.iStatelId1 != 0)
            return tmpResou.f_CreateRole(((RoleModelDT)glo_Main.GetInstance().
                m_SC_Pool.m_RoleModelSC.f_GetSC(card.iStatelId1)).iModel, needToShowRedCard, true, ((RoleModelDT)glo_Main.GetInstance().
                m_SC_Pool.m_RoleModelSC.f_GetSC(card.iStatelId1)).iId);
        if (card.iStatelId2 != 0)
            return tmpResou.f_CreateRole(((RoleModelDT)glo_Main.GetInstance().
                 m_SC_Pool.m_RoleModelSC.f_GetSC(card.iStatelId2)).iModel, needToShowRedCard, true, ((RoleModelDT)glo_Main.GetInstance().
                 m_SC_Pool.m_RoleModelSC.f_GetSC(card.iStatelId2)).iId);
        return null;
    }

    public static GameObject f_GetStatelObject(CardPoolDT tCardPoolDT, bool needToShowRedCard = true)
    {
        ResourceManager tmpResou = glo_Main.GetInstance().m_ResourceManager;
        //List<NBaseSCDT> tmpRole = glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetAll();
        //NBaseSCDT tmp = new NBaseSCDT();

        if (tCardPoolDT.m_FanshionableDressPoolDT != null)
        {//穿有时装显示时装效果
            return tmpResou.f_CreateRole(tCardPoolDT.m_FanshionableDressPoolDT.m_FashionableDressDT.iModel, needToShowRedCard);
        }
        else
        {
            return f_GetStatelObject(tCardPoolDT.m_CardDT, needToShowRedCard);
        }
    }

    /// <summary>
    /// 创建角色根据模板Id
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public static GameObject f_GetStatelObject(int cardId, bool needToShowRedCard = true)
    {
        ResourceManager tmpResou = glo_Main.GetInstance().m_ResourceManager;
        CardDT tCardDT;
        if (cardId == GameParamConst.PlayerRoleId)
        {
            CardPoolDT tCardPoolDT = Data_Pool.m_CardPool.f_GetPlayerRolePoolDT();
            if (tCardPoolDT.m_FanshionableDressPoolDT != null)
            {//穿有时装显示时装效果
                return tmpResou.f_CreateRole(tCardPoolDT.m_FanshionableDressPoolDT.m_FashionableDressDT.iModel, needToShowRedCard);
            }
            tCardDT = tCardPoolDT.m_CardDT;
        }
        else
        {
            tCardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardId);
        }
        if (tCardDT == null)
            return null;
        if (tCardDT.iStatelId1 != 0)
            return tmpResou.f_CreateRole(((RoleModelDT)glo_Main.GetInstance().
                m_SC_Pool.m_RoleModelSC.f_GetSC(tCardDT.iStatelId1)).iModel, needToShowRedCard, true, ((RoleModelDT)glo_Main.GetInstance().
                m_SC_Pool.m_RoleModelSC.f_GetSC(tCardDT.iStatelId1)).iId);
        if (tCardDT.iStatelId2 != 0)
            return tmpResou.f_CreateRole(((RoleModelDT)glo_Main.GetInstance().
                 m_SC_Pool.m_RoleModelSC.f_GetSC(tCardDT.iStatelId2)).iModel, needToShowRedCard, true, ((RoleModelDT)glo_Main.GetInstance().
                m_SC_Pool.m_RoleModelSC.f_GetSC(tCardDT.iStatelId2)).iId);
        return null;
    }
    /// <summary>
    /// 通过f_GetStatelObject创建
    /// 由这个删除
    /// </summary>
    /// <param name="obj"></param>
    public static void f_DestoryStatelObject(GameObject obj)
    {
        glo_Main.GetInstance().m_ResourceManager.f_DestorySD(obj);
    }
    /// <summary>
    /// 文字提示
    /// </summary>
    /// <param name="tmp"></param>
    public static void Ui_Trip(string tmp)
    {
        ccU3DEngine.ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, tmp);
    }

    /// <summary>
    /// 返回错误结果显示
    /// </summary>
    /// <param name="content"></param>
    public static void UI_ShowFailContent(string content)
    {
        ccU3DEngine.ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, new PopupMenuParams(CommonTools.f_GetTransLanguage(1847), content));
    }
    /// <summary>
    /// 获取卡牌数量
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public static int f_GetCardNum(CardDT card)
    {
        int i = 0;
        i = Data_Pool.m_CardPool.f_GetAllForData5(card.iId).Count;
        return i;
    }
    /// <summary>
    /// 获取副本章节图片 
    /// 是彩色 30011是灰色   IconId +1 代表灰色
    /// </summary>
    public static Sprite f_GetDungeonSprite(int iconId)
    {
        if (iconId == 0)
        {
            iconId = 30010;
        }
        return UITool.f_GetIconSprite(iconId);
    }

    /// <summary>
    /// 获取副本关卡图片
    /// </summary>
    public static Sprite f_GetTollgateSprite(int iconId)
    {
        if (iconId == 0)
        {
            iconId = 30020;
            MessageBox.ASSERT("TollgateSprite IconId = 0");
        }
        return UITool.f_GetIconSprite(iconId);
    }


    /// <summary>
    /// 获得卡牌名字
    /// </summary>
    /// <param name="dt">卡牌模板</param>
    /// <returns></returns>
    public static string f_GetCardName(CardDT dt)
    {
        if (dt.iCardType == (int)EM_CardType.RoleCard)
        {
            return Data_Pool.m_UserData.m_szRoleName;
        }
        else
        {
            return dt.szName;
        }
    }

    /// <summary>
    /// 获得卡牌名字
    /// </summary>
    /// <param name="dt">卡牌模板Id</param>
    /// <returns></returns>
    public static string f_GetCardName(int cardId)
    {
        if (cardId == GameParamConst.PlayerRoleId)
        {
            return Data_Pool.m_UserData.m_szRoleName;
        }
        CardDT tCardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardId);
        if (tCardDT == null)
            return string.Empty;
        if (tCardDT.iCardType == (int)EM_CardType.RoleCard)
        {
            return Data_Pool.m_UserData.m_szRoleName;
        }
        else
        {
            return tCardDT.szName;
        }
    }

    /// <summary>
    /// 获得卡牌全部天赋
    /// </summary>
    /// <param name="tcard"></param>
    /// <returns></returns>
    public static CardTalentDT[] f_GetCardTalent(CardPoolDT tcard)
    {
        List<CardTalentDT> tlist = new List<CardTalentDT>();
        CardEvolveDT tcardEvo = null;
        CardTalentDT tTalent = null;
        for (int i = 0; i < 20; i++)
        {
            tcardEvo = glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.f_GetSC(tcard.m_CardDT.iEvolveId + i) as CardEvolveDT;
            if (tcardEvo != null)
            {
                tTalent = glo_Main.GetInstance().m_SC_Pool.m_CardTalentSC.f_GetSC(tcardEvo.iTalentId) as CardTalentDT;
                if (tTalent != null)
                    tlist.Add(tTalent);
            }
        }
        return tlist.ToArray();
    }

    /// <summary>
    /// 获取装备属性
    /// </summary>
    /// <param name="EquipLv"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static int f_GetEquipPro(EquipPoolDT dt)
    {
        RoleProperty ttt = new RoleProperty();
        RolePropertyTools.f_DispEquip(ref ttt, dt);
        int num = 0;
        for (int i = (int)EM_RoleProperty.Atk; i <= (int)EM_RoleProperty.MDef; i++)
        {
            if (ttt.f_GetProperty(i) > num)
                num = ttt.f_GetProperty(i);
        }
        return num;
        //if (dt.m_sexpStars > 0)
        //{
        //    tUpstar = glo_Main.GetInstance().m_SC_Pool.m_EquipUpStarSC.f_GetSC(dt.m_EquipDT.iId * 100 + dt.m_sstars + 1) as EquipUpStarDT;
        //    if (tUpstar != null)
        //        pronum = Convert.ToInt32(((float)dt.m_sexpStars / (float)tUpstar.iUpExp) * tUpstar.iAddPro);
        //}
        //if (dt.m_lvRefine > 0)
        //{
        //    pronum += RolePropertyTools.CalculatePropertyStartLv1(0, dt.m_EquipDT.iRefinPro1, dt.m_lvRefine + 1);
        //}
        //for (int j = 1; j < dt.m_sstars; j++)
        //{
        //    tUpstar = glo_Main.GetInstance().m_SC_Pool.m_EquipUpStarSC.f_GetSC(dt.m_EquipDT.iId * 100 + j) as EquipUpStarDT;
        //    if (tUpstar != null)
        //        pronum = tUpstar.iAddNum + tUpstar.iAddPro;
        //}
        //return pronum + (dt.m_EquipDT.iStartPro + (dt.m_lvIntensify - 1) * dt.m_EquipDT.iAddPro) + dt.m_lvRefine * dt.m_EquipDT.iRefinPro1;
    }


    /// <summary>
    /// 根据进阶Id 得到进阶等级
    /// </summary>
    /// <param name="evolveId">进阶Id</param>
    /// <returns></returns>
    public static int f_GetEvolveLv(int evolveId)
    {
        if (evolveId == 0)
            return 0;
        else
        {
            CardEvolveDT tmp = glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.f_GetSC(evolveId) as CardEvolveDT;
            if (tmp != null)
                return tmp.iEvoLv;
            else
            {
                MessageBox.ASSERT(string.Format(CommonTools.f_GetTransLanguage(1848), evolveId));
                return 0;
            }
        }
    }

    /// <summary>
    /// 根据数量来显示星星
    /// </summary>
    /// <param name="stars">星星显示数组</param>
    /// <param name="num">显示的星数</param>
    public static void f_UpdateStarNum(UISprite[] stars, int num, string starName = "Icon_RMStar_4", string starGreyName = "Icon_RMStar2", int numMax = 0, bool isPerfect = true)
    {
        if (starGreyName == "")
        {
            for (int i = 0; i < stars.Length; i++)
            {
                if (stars[i] != null)
                {
                    if (numMax != 0 && i >= numMax && stars[i].gameObject.activeSelf)
                        stars[i].gameObject.SetActive(false);
                    else if ((numMax == 0 || i < numMax) && !stars[i].gameObject.activeSelf)
                        stars[i].gameObject.SetActive(true);
                    stars[i].spriteName = starName;
                    UITool.f_SetSpriteGray(stars[i], i >= num);
                }
            }
        }
        else
        {
            for (int i = 0; i < stars.Length; i++)
            {
                if (stars[i] != null)
                {
                    if (numMax != 0 && i >= numMax && stars[i].gameObject.activeSelf)
                        stars[i].gameObject.SetActive(false);
                    else if ((numMax == 0 || i < numMax) && !stars[i].gameObject.activeSelf)
                        stars[i].gameObject.SetActive(true);
                    stars[i].spriteName = i < num ? starName : starGreyName;
                    if (isPerfect)
                    {
                        //stars[i].MakePixelPerfect();
                    }
                }
            }
        }
    }
    /// <summary>
    /// 获取装备颜色
    /// </summary>
    /// <param name="Important"></param>
    /// <returns></returns>
    public static string f_GetEquipColur(EM_Important Important)
    {
        switch (Important)
        {
            case EM_Important.Green:
                return CommonTools.f_GetTransLanguage(1849);
            case EM_Important.Blue:
                return CommonTools.f_GetTransLanguage(1850);
            case EM_Important.Purple:
                return CommonTools.f_GetTransLanguage(1851);
            case EM_Important.Oragen:
                return CommonTools.f_GetTransLanguage(1852);
            case EM_Important.Red:
                return CommonTools.f_GetTransLanguage(1853);
        }
        return null;
    }
    public static string f_GetEquipPart(int Part)
    {
        switch ((EM_EquipPart)Part)
        {
            case EM_EquipPart.eEquipPart_Weapon:
                return CommonTools.f_GetTransLanguage(1854);
            case EM_EquipPart.eEquipPart_Armour:
                return CommonTools.f_GetTransLanguage(1855);
            case EM_EquipPart.eEquipPart_Helmet:
                return CommonTools.f_GetTransLanguage(1856);
            case EM_EquipPart.eEquipPart_Belt:
                return CommonTools.f_GetTransLanguage(1857);
            case EM_EquipPart.eEquipPare_MagicLeft:
                return CommonTools.f_GetTransLanguage(1858);
            case EM_EquipPart.eEquipPare_MagicRight:
                return CommonTools.f_GetTransLanguage(1859);
            case EM_EquipPart.eEquipPart_INVALID:
                return CommonTools.f_GetTransLanguage(1860);
        }
        return "";
    }
    /// <summary>
    /// 获取品质颜色
    /// </summary>
    /// <param name="Important"></param>
    /// <returns></returns>
    public static string f_GetImportantColorName(EM_Important Important)
    {
        switch (Important)
        {
            case EM_Important.White: return CommonTools.f_GetTransLanguage(1861);
            case EM_Important.Green: return CommonTools.f_GetTransLanguage(1862);
            case EM_Important.Blue: return CommonTools.f_GetTransLanguage(1863);
            case EM_Important.Purple: return CommonTools.f_GetTransLanguage(1864);
            case EM_Important.Oragen: return CommonTools.f_GetTransLanguage(1865);
            case EM_Important.Red: return CommonTools.f_GetTransLanguage(1866);
            case EM_Important.Gold: return CommonTools.f_GetTransLanguage(1867);
        }
        return null;
    }
    /// <summary>
    /// 获取属性名字
    /// </summary>
    /// <param name="Role"></param>
    /// <returns></returns>
    public static string f_GetProName(EM_RoleProperty Role)
    {
        switch (Role)
        {
            case EM_RoleProperty.Atk:
                return CommonTools.f_GetTransLanguage(1868);
            case EM_RoleProperty.Hp:
                return CommonTools.f_GetTransLanguage(1869);
            case EM_RoleProperty.Def:
                return CommonTools.f_GetTransLanguage(1870);
            case EM_RoleProperty.MDef:
                return CommonTools.f_GetTransLanguage(1871);
            case EM_RoleProperty.AddAtk:
                return CommonTools.f_GetTransLanguage(1872);
            case EM_RoleProperty.AddHp:
                return CommonTools.f_GetTransLanguage(1873);
            case EM_RoleProperty.AddDef:
                return CommonTools.f_GetTransLanguage(1874);
            case EM_RoleProperty.AddMDef:
                return CommonTools.f_GetTransLanguage(1875);
            case EM_RoleProperty.Anger:
                return CommonTools.f_GetTransLanguage(1876);
            case EM_RoleProperty.Energy:
                return CommonTools.f_GetTransLanguage(1877);
            case EM_RoleProperty.CritR:
                return CommonTools.f_GetTransLanguage(1878);
            case EM_RoleProperty.AntiknockR:
                return CommonTools.f_GetTransLanguage(1879);
            case EM_RoleProperty.HitR:
                return CommonTools.f_GetTransLanguage(1880);
            case EM_RoleProperty.DodgeR:
                return CommonTools.f_GetTransLanguage(1881);
            case EM_RoleProperty.DamageR:
                return CommonTools.f_GetTransLanguage(1882);
            case EM_RoleProperty.InjurySR:
                return CommonTools.f_GetTransLanguage(1883);
            case EM_RoleProperty.PInjuryAR:
                return CommonTools.f_GetTransLanguage(1884);
            case EM_RoleProperty.PInjurySR:
                return CommonTools.f_GetTransLanguage(1885);
            case EM_RoleProperty.Deadly:
                return CommonTools.f_GetTransLanguage(1886);
            case EM_RoleProperty.IgDefCR:
                return CommonTools.f_GetTransLanguage(1887);
            case EM_RoleProperty.IgDefR:
                return CommonTools.f_GetTransLanguage(1888);
            case EM_RoleProperty.SuckCR:
                return CommonTools.f_GetTransLanguage(1889);
            case EM_RoleProperty.SuckR:
                return CommonTools.f_GetTransLanguage(1890);
            case EM_RoleProperty.BounceCR:
                return CommonTools.f_GetTransLanguage(1891);
            case EM_RoleProperty.BounceR:
                return CommonTools.f_GetTransLanguage(1892);
            case EM_RoleProperty.MBounceCR:
                return CommonTools.f_GetTransLanguage(1893);
            case EM_RoleProperty.MBounceR:
                return CommonTools.f_GetTransLanguage(1894);
            case EM_RoleProperty.RcvHpCR:
                return CommonTools.f_GetTransLanguage(1895);
            case EM_RoleProperty.RcvHpR:
                return CommonTools.f_GetTransLanguage(1896);
            case EM_RoleProperty.RcvAnger:
                return CommonTools.f_GetTransLanguage(1897);
            case EM_RoleProperty.ExpR:
                return CommonTools.f_GetTransLanguage(1898);
            case EM_RoleProperty.End:
                break;
        }
        return null;
    }
    /// <summary>
    /// 更新Label显示的属性   有些按照百分比来显示
    /// </summary>
    /// <param name="ProId"></param>
    /// <param name="Label"></param>
    /// <param name="AddPro"></param>
    public static void f_UpdateAddPro(int ProId, UILabel Label, int AddPro, bool cover = false)
    {
        if (cover)
            Label.text += f_GetPercentagePro(ProId, AddPro);
        else
            Label.text = f_GetPercentagePro(ProId, AddPro);
    }
    /// <summary>
    /// 获得百分比属性
    /// </summary>
    public static string f_GetPercentagePro(int ProId, int ProNum)
    {
        if ((ProId >= (int)EM_RoleProperty.CritR && ProId != (int)EM_RoleProperty.RcvAnger) || (ProId >= (int)EM_RoleProperty.AddAtk && ProId <= (int)EM_RoleProperty.AddMDef))
            return (float)(ProNum / 100f) + "%";
        else
            return ProNum + "";
    }
    /// <summary>
    /// 获取领悟DT
    /// </summary>
    /// <param name="Lv"></param>
    /// <returns></returns>
    public static AwakenCardDT f_GetAwakenCardDT(int Lv)
    {
        if (Lv >= 60)
            return null;
        return (AwakenCardDT)glo_Main.GetInstance().m_SC_Pool.m_AwakenCardSC.f_GetSC(Lv + 1);
    }
    /// <summary>
    ///获取装备强化消耗
    /// </summary>
    /// <param name="tmp"></param>
    /// <returns></returns>
    public static int f_GetEquipIntenCon(EquipPoolDT tmp, int Times)
    {
        int lvIntensify = tmp.m_lvIntensify;
        int Con = 0;
        for (int times = 1; times <= Times; times++)
        {
            EquipIntensifyConsumeDT tConsume = glo_Main.GetInstance().m_SC_Pool.m_EquipIntensifyConsumeSC.f_GetSC(lvIntensify + times) as EquipIntensifyConsumeDT;
            if (tConsume == null)
                return 0;
            switch ((EM_Important)tmp.m_EquipDT.iColour)
            {
                case EM_Important.Green:
                    Con += tConsume.iGreen;
                    break;
                case EM_Important.Blue:
                    Con += tConsume.iBule;
                    break;
                case EM_Important.Purple:
                    Con += tConsume.iViolet;
                    break;
                case EM_Important.Oragen:
                    Con += tConsume.iOrange;
                    break;
                case EM_Important.Red:
                    Con += tConsume.iRed;
                    break;
                case EM_Important.Gold:
                    //return ((EquipIntensifyConsumeDT)inte[i]).i;
                    break;
                default:
                    break;
            }
        }
        return Con;
    }


    /// <summary>
    /// 获得已经装备的
    /// </summary>
    /// <param name="Lv"></param>
    /// <returns></returns>
    public static AwakenEquipDT[] f_GetAwakenEquipArr(int Lv)
    {
        AwakenCardDT tmp = f_GetAwakenCardDT(Lv);
        AwakenEquipDT[] tmpArr;
        if (tmp.iEquipID3 == 0)
            tmpArr = new AwakenEquipDT[2];
        else if (tmp.iEquipID4 == 0)
            tmpArr = new AwakenEquipDT[3];
        else
            tmpArr = new AwakenEquipDT[4];
        foreach (AwakenEquipDT item in glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetAll())
        {
            try
            {
                if (item.iId == tmp.iEquipID1)
                    tmpArr[0] = item;
                if (item.iId == tmp.iEquipID2)
                    tmpArr[1] = item;
                if (item.iId == tmp.iEquipID3)
                    tmpArr[2] = item;
                if (item.iId == tmp.iEquipID4)
                    tmpArr[3] = item;
            }
            catch { }
        }
        return tmpArr;
    }
    /// <summary>
    /// 根据唯一ID查找装备
    /// </summary>
    /// <returns></returns>
    public static EquipPoolDT f_GetEquipDTForIid(long Id)
    {
        return Data_Pool.m_EquipPool.f_GetForId(Id) as EquipPoolDT;
    }

    public static AwakenEquipPoolDT f_GetAwakenEquipPoolDT(int id)
    {
        if (Data_Pool.m_AwakenEquipPool.f_GetForData5(id) == null)
            return new AwakenEquipPoolDT();
        return Data_Pool.m_AwakenEquipPool.f_GetForData5(id) as AwakenEquipPoolDT;
    }
    /// <summary>
    /// 检查是否拥有这个装备
    /// </summary>
    /// <param name="equipArr"></param>
    /// <returns></returns>
    public static bool f_GetIsHaveAwakenEquip(AwakenEquipDT equipArr)
    {
        AwakenEquipPoolDT tawaken = Data_Pool.m_AwakenEquipPool.f_GetForData5(equipArr.iId) as AwakenEquipPoolDT;
        if (tawaken != null)
            return true;
        else
            return false;
    }
    /// <summary>
    /// 获取装备
    /// </summary>
    /// <param name="equipArr"></param>
    /// <returns></returns>
    public static AwakenEquipPoolDT[] f_GetAwakenEquip(int[] equipArr)
    {
        AwakenEquipPoolDT[] tmp = new AwakenEquipPoolDT[4];
        for (int i = 0; i < equipArr.Length; i++)
        {
            if (equipArr[i] != 0)
            {
                tmp[i] = Data_Pool.m_AwakenEquipPool.f_GetForData5(equipArr[i]) as AwakenEquipPoolDT;
            }
        }
        return tmp;
    }

    public static AwakenEquipPoolDT f_GetAwakenEquip(int Id)
    {
        return Data_Pool.m_AwakenEquipPool.f_GetForData5(Id) as AwakenEquipPoolDT;
    }

    /// <summary>
    /// 检查能否合这个装备
    /// </summary>
    /// <param name="equip"></param>
    /// <returns></returns>
    public static bool f_GetIsHaveAwakenSythe(AwakenEquipDT equip)
    {
        foreach (AwakenEquipPoolDT item in Data_Pool.m_AwakenEquipPool.f_GetAll())
        {
            if (item.m_AwakenEquipDT.iSynthesisId1 == equip.iId && equip.iSynthesisNum1 == item.m_num ||
                item.m_AwakenEquipDT.iSynthesisId2 == equip.iId && equip.iSynthesisNum2 == item.m_num ||
                item.m_AwakenEquipDT.iSynthesisId3 == equip.iId && equip.iSynthesisNum3 == item.m_num ||
                item.m_AwakenEquipDT.iSynthesisId4 == equip.iId && equip.iSynthesisNum4 == item.m_num
                )
                return true;
        }
        return false;
    }
    /// <summary>
    /// 获取装备精炼属性
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static int[] f_GetEquipRefinePro(EquipPoolDT dt)
    {
        int[] tmp = new int[2];
        tmp[0] = dt.m_EquipDT.iRefinPro1 * dt.m_lvRefine;
        tmp[1] = dt.m_EquipDT.iRefinPro2 * dt.m_lvRefine;
        return tmp;
    }
    /// <summary>
    /// 获取装备强化最大等级
    /// </summary>
    /// <returns></returns>
    public static int f_GetEquipIntenMax()
    {
        //以前跟主角等级挂钩，，但是现在的表只填了150级的消耗数据（超过150级就异常了），，为了以后能扩展等级，就修改为填多少上限就是多少吧！
        int maxByLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) * 2;
        int maxByTable = glo_Main.GetInstance().m_SC_Pool.m_EquipIntensifyConsumeSC.f_GetAll().Count;
        return maxByLv > maxByTable ? maxByTable : maxByLv;
    }
    /// <summary>
    /// 获取装备精炼所需经验
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static int f_GetEquipRefineExp(EquipPoolDT dt)
    {
        int tmp = 0;
        tmp = GetEquipRefineId(dt.m_EquipDT.iColour) + dt.m_lvRefine;
        if (glo_Main.GetInstance().m_SC_Pool.m_EquipConsumeSC.f_GetSC(tmp + 1) != null)
            return ((EquipConsumeDT)glo_Main.GetInstance().m_SC_Pool.m_EquipConsumeSC.f_GetSC(tmp + 1)).iRefineExp;
        else
            return 0;
    }

    public static int GetEquipRefineId(int tmp)
    {
        switch (tmp)
        {
            case 1:
                return 1000;
            case 2:
                return 2000;
            case 3:
                return 3000;
            case 4:
                return 4000;
            case 5:
                return 5000;
            case 6:
                return 6000;
            case 7:
                return 7000;
        }
        return 0;
    }
    /// <summary>
    /// 获取装备强化大师
    /// </summary>
    /// <returns></returns>
    public static string[] f_GetEquipIntenMaster(EquipPoolDT dt)
    {
        MasterDT[] tmp = new MasterDT[2];
        List<NBaseSCDT> tmpNB = glo_Main.GetInstance().m_SC_Pool.m_MasterSC.f_GetAll();
        for (int i = 0; i < tmpNB.Count; i++)
        {
            if ((tmpNB[i] as MasterDT).iType == 1)
            {
                if (dt.m_lvIntensify >= (tmpNB[i] as MasterDT).iLv)
                {
                    tmp[0] = (tmpNB[i] as MasterDT);
                    break;
                }
            }
        }
        string[] tmpStr = new string[2];
        string[] tmpArr1 = new string[5];
        string[] tmpArr0 = new string[5];
        if (tmp[0] != null)
        {
            if (tmp[0].iId != 20)
                tmp[1] = (MasterDT)glo_Main.GetInstance().m_SC_Pool.m_MasterSC.f_GetSC(tmp[0].iId + 1);
            tmpArr0[0] = string.Format(CommonTools.f_GetTransLanguage(1899), tmp[0].iId, f_GetProName((EM_RoleProperty)tmp[0].iProID1), f_GetPercentagePro(tmp[0].iProId2, tmp[0].iPro1));
            if (tmp[0].iProId2 != 0)
                tmpArr0[1] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[0].iProId2), f_GetPercentagePro(tmp[0].iProId2, tmp[0].iPro));
            if (tmp[0].iProId3 != 0)
                tmpArr0[2] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[0].iProId3), f_GetPercentagePro(tmp[0].iProId2, tmp[0].iPro3));
            if (tmp[0].iProId4 != 0)
                tmpArr0[3] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[0].iProId4), f_GetPercentagePro(tmp[0].iProId2, tmp[0].iPro4));
            if (tmp[0].iProId5 != 0)
                tmpArr0[4] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[0].iProId5), f_GetPercentagePro(tmp[0].iProId2, tmp[0].iPro5));
        }
        if (tmp[0] == null)
            tmp[1] = (MasterDT)glo_Main.GetInstance().m_SC_Pool.m_MasterSC.f_GetSC(1);
        if (tmp[1].iId < 20)
        {
            tmpArr1[0] = string.Format(CommonTools.f_GetTransLanguage(1899), tmp[1].iId, f_GetProName((EM_RoleProperty)tmp[1].iProID1), f_GetPercentagePro(tmp[1].iProId2, tmp[1].iPro1));
            if (tmp[1].iProId2 != 0)
                tmpArr1[1] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[1].iProId2), f_GetPercentagePro(tmp[1].iProId2, tmp[1].iPro));
            if (tmp[1].iProId3 != 0)
                tmpArr1[2] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[1].iProId3), f_GetPercentagePro(tmp[1].iProId2, tmp[1].iPro3));
            if (tmp[1].iProId4 != 0)
                tmpArr1[3] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[1].iProId4), f_GetPercentagePro(tmp[1].iProId2, tmp[1].iPro4));
            if (tmp[1].iProId5 != 0)
                tmpArr1[4] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[1].iProId5), f_GetPercentagePro(tmp[1].iProId2, tmp[1].iPro5));
        }
        else
            tmpStr[1] = null;

        tmpStr[0] = string.Join("", tmpArr0);

        tmpStr[1] = string.Join("", tmpArr1);
        return tmpStr;
    }
    /// <summary>
    /// 获取装备精炼大师
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string f_GetEquipRefineMaster(EquipPoolDT dt)
    {
        List<NBaseSCDT> tmpNB = glo_Main.GetInstance().m_SC_Pool.m_MasterSC.f_GetAll();
        string tmpStr = "";
        string[] tmpArr = new string[5];
        for (int i = 0; i > tmpNB.Count; i++)
        {
            if ((tmpNB[i] as MasterDT).iType == 2)
            {
                if ((tmpNB[i] as MasterDT).iLv >= dt.m_lvRefine)
                {
                    tmpArr[0] = string.Format(CommonTools.f_GetTransLanguage(1900), (tmpNB[i] as MasterDT).iId - 20, f_GetProName((EM_RoleProperty)(tmpNB[i] as MasterDT).iProID1), f_GetPercentagePro((tmpNB[i] as MasterDT).iProID1, (tmpNB[i] as MasterDT).iPro1));
                    if ((tmpNB[i] as MasterDT).iProId2 != 0)
                        tmpArr[1] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)(tmpNB[i] as MasterDT).iProId2), f_GetPercentagePro((tmpNB[i] as MasterDT).iProID1, (tmpNB[i] as MasterDT).iPro));
                    if ((tmpNB[i] as MasterDT).iProId3 != 0)
                        tmpArr[2] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)(tmpNB[i] as MasterDT).iProId3), f_GetPercentagePro((tmpNB[i] as MasterDT).iProID1, (tmpNB[i] as MasterDT).iPro3));
                    if ((tmpNB[i] as MasterDT).iProId4 != 0)
                        tmpArr[3] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)(tmpNB[i] as MasterDT).iProId4), f_GetPercentagePro((tmpNB[i] as MasterDT).iProID1, (tmpNB[i] as MasterDT).iPro4));
                    if ((tmpNB[i] as MasterDT).iProId5 != 0)
                        tmpArr[4] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)(tmpNB[i] as MasterDT).iProId5), f_GetPercentagePro((tmpNB[i] as MasterDT).iProID1, (tmpNB[i] as MasterDT).iPro5));
                    break;
                }
            }
        }
        tmpStr = string.Join("", tmpArr);
        return tmpStr;
    }
    /// <summary>
    /// 获得装备与谁
    /// </summary>
    /// <returns></returns>
    public static string f_GetHowEquip(long EquipId)
    {
        List<BasePoolDT<long>> tmpList = Data_Pool.m_TeamPool.f_GetAll();
        string tmpStr = "";
        TeamPoolDT item;     // TeamPoolDT item = new TeamPoolDT();
        for (int j = 0; j < tmpList.Count; j++)
        {
            item = tmpList[j] as TeamPoolDT;
            for (int i = 0; i < item.m_aEqupId.Length; i++)
            {
                if (item.m_aEqupId[i] == EquipId)
                {
                    tmpStr = item.m_CardPoolDT.m_CardDT.szName;
                    if (item.m_CardPoolDT.m_CardDT.iCardType == 1)
                        tmpStr = Data_Pool.m_UserData.m_szRoleName;
                    break;
                }
            }
        }
        return tmpStr;
    }
    public static TeamPoolDT f_GetTeamPoolDTForEquip(EquipPoolDT tEquip)
    {
        List<BasePoolDT<long>> tmpList = Data_Pool.m_TeamPool.f_GetAll();
        TeamPoolDT item;// = new TeamPoolDT();
        for (int j = 0; j < tmpList.Count; j++)
        {
            item = tmpList[j] as TeamPoolDT;
            for (int i = 0; i < item.m_aEqupId.Length; i++)
            {
                if (item.m_aEqupId[i] == tEquip.iId)
                {
                    return item;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 获取物品数量
    /// </summary>
    /// <param name="iId">模板</param>
    /// <returns></returns>
    public static int f_GetGoodsNum(int iId)
    {
        return f_GetGoodNum(EM_ResourceType.Good, iId);
    }

    /// <summary>
    /// 获得技能表
    /// </summary>
    public static MagicDT[] f_GetCardMagic(CardDT card)
    {
        return card.m_aMagicDT;
    }
    /// <summary>
    /// 获得道具名字
    /// </summary>
    public static string f_GetGoodsName(int id)
    {
        return f_GetGoodName(EM_ResourceType.Good, id);
    }
    /// 获取装备碎片数量
    /// </summary>
    /// <param name="iId"></param>
    /// <returns></returns>
    public static int f_GetEquipFragmentNum(int iId)
    {
        int tNum = 0;
        EquipFragmentPoolDT tEquipFragmentPoolDT;
        for (int i = 0; i < Data_Pool.m_EquipFragmentPool.f_GetAllForData1(iId).Count; i++)
        {
            tEquipFragmentPoolDT = Data_Pool.m_EquipFragmentPool.f_GetAllForData1(iId)[i] as EquipFragmentPoolDT;

            tNum += tEquipFragmentPoolDT.m_iNum;
        }


        return tNum;
    }
    /// <summary>
    /// 获取装备对应升星
    /// </summary>
    /// <param name="EquipId"></param>
    /// <returns></returns>
    public static EquipUpStarDT f_GetEquipUpStar(EquipPoolDT Equip)
    {
        if (Equip.m_sstars == 5 && Equip.m_EquipDT.iColour == (int)EM_Important.Red || Equip.m_sstars == 3 && Equip.m_EquipDT.iColour == (int)EM_Important.Oragen)
            return (EquipUpStarDT)glo_Main.GetInstance().m_SC_Pool.m_EquipUpStarSC.f_GetSC(Equip.m_EquipDT.iId * 100 + Equip.m_sstars);
        return (EquipUpStarDT)glo_Main.GetInstance().m_SC_Pool.m_EquipUpStarSC.f_GetSC(Equip.m_EquipDT.iId * 100 + Equip.m_sstars + 1);
    }
    /// <summary>
    /// 获得装备星级+1属性
    /// </summary>
    /// <param name="Edt"></param>
    /// <returns></returns>
    public static int[] f_GetEquipStarPro(EquipPoolDT Edt)
    {
        int[] tmpInt = new int[3];
        BetterList<EquipUpStarDT> tmpList = new BetterList<EquipUpStarDT>();
        foreach (EquipUpStarDT item in glo_Main.GetInstance().m_SC_Pool.m_EquipUpStarSC.f_GetAll())
        {
            if (Edt.m_EquipDT.iColour == (int)EM_Important.Oragen)
            {
                if (tmpList.size == 3)
                    break;
            }
            if (Edt.m_EquipDT.iColour == (int)EM_Important.Red)
            {
                if (tmpList.size == 5)
                    break;
            }
            if (item.iEquipId == Edt.m_EquipDT.iId)
            {
                tmpList.Add(item);
            }
        }
        for (int i = 0; i < Edt.m_sstars; i++)
        {
            if (Edt.m_sstars > i)
            {
                tmpInt[0] += tmpList[i].iAddPro + tmpList[i].iAddNum;
            }
            else if (Edt.m_sstars == i)
                tmpInt[0] += (int)((float)tmpList[i].iAddPro * (float)((float)((float)Edt.m_sexpStars / (float)tmpList[i].iUpExp) * (float)100) / (float)100);
        }
        try
        {
            tmpInt[1] = f_GetEquipUpStar(Edt).iAddPro;
            tmpInt[2] = f_GetEquipUpStar(Edt).iAddNum;
        }
        catch
        {
            tmpInt[1] = 0;
            tmpInt[2] = 0;
        }
        return tmpInt;
    }
    /// <summary>
    /// 获得成功率
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string f_GetStarSucRate(EquipPoolDT dt)
    {
        EquipUpStarDT tmp = f_GetEquipUpStar(dt);

        int num = tmp.iInitial + (dt.m_slucky / 150) * 500;

        if (num <= 3000)
        {
            return CommonTools.f_GetTransLanguage(1901);
        }
        if (num > 3100 && num <= 5000)
        {
            return CommonTools.f_GetTransLanguage(1902);
        }
        if (num > 5100 && num <= 7000)
        {
            return CommonTools.f_GetTransLanguage(1903);
        }
        if (num > 7100 && num <= 9000)
        {
            return CommonTools.f_GetTransLanguage(1904);
        }
        if (num > 9100)
        {
            return CommonTools.f_GetTransLanguage(1905);
        }
        return null;
    }

    /// <summary>
    /// 获取资源拥有的数量
    /// </summary>
    /// <param name="resourceType"></param>
    /// <param name="templateId"></param>
    /// <returns></returns>
    public static int f_GetResourceHaveNum(int resourceType, int templateId)
    {
        int result = 0;
        if (resourceType == (int)EM_ResourceType.Money)
        {
            result = Data_Pool.m_UserData.f_GetProperty(templateId);
        }
        else if (resourceType == (int)EM_ResourceType.Good)
        {
            result = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(templateId);
        }
        else if (resourceType == (int)EM_ResourceType.AwakenEquip)
        {
            result = Data_Pool.m_AwakenEquipPool.f_GetHaveNumByTemplate(templateId);
        }
        else if (resourceType == (int)EM_ResourceType.Card)
        {
            result = Data_Pool.m_CardPool.f_GetHaveNumByTemplate(templateId);
        }
        else if (resourceType == (int)EM_ResourceType.CardFragment)
        {
            result = Data_Pool.m_CardFragmentPool.f_GetHaveNumByTemplate(templateId);
        }
        else if (resourceType == (int)EM_ResourceType.Equip)
        {
            result = Data_Pool.m_EquipPool.f_GetHaveNumByTemplate(templateId);
        }
        else if (resourceType == (int)EM_ResourceType.EquipFragment)
        {
            result = Data_Pool.m_EquipFragmentPool.f_GetHaveNumByTemplate(templateId);
        }
        else if (resourceType == (int)EM_ResourceType.Treasure)
        {
            result = Data_Pool.m_TreasurePool.f_GetHaveNumByTemplate(templateId);
        }
        else if (resourceType == (int)EM_ResourceType.TreasureFragment)
        {
            result = Data_Pool.m_TreasureFragmentPool.f_GetHaveNumByTemplate(templateId);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1906) + resourceType);
        }
        return result;
    }

    /// <summary>
    /// 判断资源是否为碎片（暂时只有装备、卡牌、宝物碎片，写死判断的，如果有新碎片类型，注意修改方法！！！）
    /// </summary>
    /// <param name="resourceType"></param>
    /// <returns></returns>
    public static bool f_JudgeIsFragement(EM_ResourceType resourceType)
    {
        if (resourceType == EM_ResourceType.CardFragment || resourceType == EM_ResourceType.EquipFragment || resourceType == EM_ResourceType.TreasureFragment)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //////////////法宝/////////////////////////

    /// <summary>
    /// 获得法宝强化属性
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static int[] f_GetTreasurePro(TreasurePoolDT dt)
    {
        int[] tmp = new int[2];
        tmp[0] = dt.m_TreasureDT.iAddPro1 * (dt.m_lvIntensify - 1) + dt.m_TreasureDT.iStartPro1;
        tmp[1] = dt.m_TreasureDT.iAddPro2 * (dt.m_lvIntensify - 1) + dt.m_TreasureDT.iStartPro2;
        return tmp;
    }
    /// <summary>
    /// 获得法宝强化大师
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string[] f_GetTreasureIntenMaster(TreasurePoolDT dt)
    {
        MasterDT[] tmp = new MasterDT[2];
        List<NBaseSCDT> tmpNB = glo_Main.GetInstance().m_SC_Pool.m_MasterSC.f_GetAll();
        foreach (MasterDT item in tmpNB)
        {
            if (item.iType == 3)
            {
                if (dt.m_lvIntensify >= item.iLv)
                {
                    tmp[0] = item;
                    break;
                }
            }
        }
        string[] tmpStr = new string[2];
        string[] tmpArr1 = new string[5];
        string[] tmpArr0 = new string[5];
        if (tmp[0] != null)
        {
            if (tmp[0].iId != 20)
                tmp[1] = (MasterDT)glo_Main.GetInstance().m_SC_Pool.m_MasterSC.f_GetSC(tmp[0].iId + 1);
            tmpArr0[0] = string.Format(CommonTools.f_GetTransLanguage(1899), tmp[0].iId, f_GetProName((EM_RoleProperty)tmp[0].iProID1), f_GetPercentagePro(tmp[0].iProID1, tmp[0].iPro1));
            if (tmp[0].iProId2 != 0)
                tmpArr0[1] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[0].iProId2), f_GetPercentagePro(tmp[0].iProID1, tmp[0].iPro));
            if (tmp[0].iProId3 != 0)
                tmpArr0[2] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[0].iProId3), f_GetPercentagePro(tmp[0].iProID1, tmp[0].iPro3));
            if (tmp[0].iProId4 != 0)
                tmpArr0[3] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[0].iProId4), f_GetPercentagePro(tmp[0].iProID1, tmp[0].iPro4));
            if (tmp[0].iProId5 != 0)
                tmpArr0[4] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[0].iProId5), f_GetPercentagePro(tmp[0].iProID1, tmp[0].iPro5));
        }
        if (tmp[0] == null)
            tmp[1] = (MasterDT)glo_Main.GetInstance().m_SC_Pool.m_MasterSC.f_GetSC(1);
        if (tmp[1].iId < 20)
        {
            tmpArr1[0] = string.Format(CommonTools.f_GetTransLanguage(1899), tmp[1].iId, f_GetProName((EM_RoleProperty)tmp[1].iProID1), f_GetPercentagePro(tmp[1].iProID1, tmp[1].iPro1));
            if (tmp[1].iProId2 != 0)
                tmpArr1[1] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[1].iProId2), f_GetPercentagePro(tmp[1].iProID1, tmp[1].iPro));
            if (tmp[1].iProId3 != 0)
                tmpArr1[2] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[1].iProId3), f_GetPercentagePro(tmp[1].iProID1, tmp[1].iPro3));
            if (tmp[1].iProId4 != 0)
                tmpArr1[3] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[1].iProId4), f_GetPercentagePro(tmp[1].iProID1, tmp[1].iPro4));
            if (tmp[1].iProId5 != 0)
                tmpArr1[4] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)tmp[1].iProId5), f_GetPercentagePro(tmp[1].iProID1, tmp[1].iPro5));
        }
        else
            tmpStr[1] = null;

        tmpStr[0] = string.Join("", tmpArr0);

        tmpStr[1] = string.Join("", tmpArr1);
        return tmpStr;
    }
    /// <summary>
    /// 获得法宝精炼属性
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static int[] f_GetTreasureRefinePro(TreasurePoolDT dt)
    {
        int[] tmp = new int[2];
        tmp[0] = dt.m_TreasureDT.iRefinPro1 * dt.m_lvRefine;
        tmp[1] = dt.m_TreasureDT.iRefinPro2 * dt.m_lvRefine;
        return tmp;
    }
    /// <summary>
    /// 获得法宝精炼大师
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string f_GetTreasureRefineMaster(TreasurePoolDT dt)
    {
        List<NBaseSCDT> tmpNB = glo_Main.GetInstance().m_SC_Pool.m_MasterSC.f_GetAll();
        string tmpStr = "";
        string[] tmpArr = new string[5];
        foreach (MasterDT item in tmpNB)
        {
            if (item.iType == 4)
            {
                if (item.iLv >= dt.m_lvRefine)
                {
                    tmpArr[0] = string.Format(CommonTools.f_GetTransLanguage(1900), item.iId - 20, f_GetProName((EM_RoleProperty)item.iProID1), f_GetPercentagePro(item.iProID1, item.iPro1));
                    if (item.iProId2 != 0)
                        tmpArr[1] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)item.iProId2), f_GetPercentagePro(item.iProID1, item.iPro));
                    if (item.iProId3 != 0)
                        tmpArr[2] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)item.iProId3), f_GetPercentagePro(item.iProID1, item.iPro3));
                    if (item.iProId4 != 0)
                        tmpArr[3] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)item.iProId4), f_GetPercentagePro(item.iProID1, item.iPro4));
                    if (item.iProId5 != 0)
                        tmpArr[4] = string.Format("{0}+{1}", f_GetProName((EM_RoleProperty)item.iProId5), f_GetPercentagePro(item.iProID1, item.iPro5));
                    break;
                }
            }
        }
        tmpStr = string.Join("", tmpArr);
        return tmpStr;
    }
    /// <summary>
    /// 获得法宝强化最大值
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static int f_GetTreasureIntenManx(TreasurePoolDT dt)
    {
        int tmp = -1;
        TreasureUpConsumeDT item;
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetAll().Count; i++)
        {
            item = glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetAll()[i] as TreasureUpConsumeDT;
            if (item.iImportant == dt.m_TreasureDT.iImportant)
            {
                tmp = item.iLV;
            }
        }
        return tmp;
    }
    /// <summary>
    /// 获得法宝精炼升级道具
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static TreasureRefineConsumeDT f_GetTreasureRefinePillNum(TreasurePoolDT dt)
    {
        if (dt.m_lvRefine + 1 <= 20)
            return glo_Main.GetInstance().m_SC_Pool.m_TreasureRefineConsumeSC.f_GetSC(dt.m_iTempleteId * 100 + dt.m_lvRefine + 1) as TreasureRefineConsumeDT;
        return null;
    }
    /// <summary>
    /// 获得数量
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static int f_GetTreasureNum(TreasurePoolDT dt)
    {
        int tmp = 0;
        //foreach (TreasurePoolDT item in Data_Pool.m_TreasurePool.f_GetAll())
        //{
        //    if (item.m_TreasureDT.iId == dt.m_TreasureDT.iId)
        //    {
        //        tmp++;
        //    }
        //}

        if (Data_Pool.m_TreasurePool.f_GetForId(dt.iId) == null)
            return 0;
        List<BasePoolDT<long>> tmpList = Data_Pool.m_TreasurePool.f_GetAllForData1(dt.m_TreasureDT.iId);

        TreasurePoolDT tmpTreasurePool;
        for (int i = 0; i < tmpList.Count; i++)
        {
            tmpTreasurePool = tmpList[i] as TreasurePoolDT;
            if (tmpTreasurePool.m_lvIntensify == 1 && tmpTreasurePool.m_lvRefine == 0 && tmpTreasurePool.m_ExpIntensify == 0 &&
                !Data_Pool.m_TeamPool.f_CheckTeamTreasure(tmpTreasurePool.iId))
                tmp++;

        }
        if (tmp <= 0)
        {
            return 0;
        }
        return tmp;
    }
    /// <summary>
    /// 获取法宝升级消耗DT
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static TreasureUpConsumeDT f_GetTreasureUpDT(TreasurePoolDT dt)
    {
        TreasureUpConsumeDT tUpConsume;
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetAll().Count; i++)
        {
            tUpConsume = glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetAll()[i] as TreasureUpConsumeDT;

            if (tUpConsume.iImportant == dt.m_TreasureDT.iImportant)
            {
                if (tUpConsume.iLV == dt.m_lvIntensify + 1)
                {
                    return tUpConsume;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 获取法宝升级消耗DT    等级由外部来决定
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static TreasureUpConsumeDT f_GetTreasureUpDT(int Important, int _Level)
    {
        TreasureUpConsumeDT tUpConsume;
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetAll().Count; i++)
        {
            tUpConsume = glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetAll()[i] as TreasureUpConsumeDT;

            if (tUpConsume.iImportant == Important)
            {
                if (tUpConsume.iLV == _Level)
                {
                    return tUpConsume;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 获得合成法宝需要的全部DT
    /// </summary>
    /// <param name="iid"></param>
    /// <returns></returns>
    public static TreasureFragmentsDT[] f_GetTreasureFragmentDT(int iid)
    {
        TreasureFragmentsDT tmp = (TreasureFragmentsDT)glo_Main.GetInstance().m_SC_Pool.m_TreasureFragmentsSC.f_GetSC(iid);
        List<NBaseSCDT> tmpList = glo_Main.GetInstance().m_SC_Pool.m_TreasureFragmentsSC.f_GetAll();
        List<TreasureFragmentsDT> tmpFragmenyt = new List<TreasureFragmentsDT>();
        foreach (TreasureFragmentsDT item in tmpList)
        {
            if (item.iTreasureId == tmp.iTreasureId)
            {
                tmpFragmenyt.Add(item);
            }
        }
        TreasureFragmentsDT[] tmpTreasure = new TreasureFragmentsDT[tmpFragmenyt.Count];
        int i = 0;
        foreach (TreasureFragmentsDT item in tmpFragmenyt)
        {
            tmpTreasure[i] = item;
            i++;
        }
        return tmpTreasure;
    }

    public static int[] f_GetTreasureFragmentNum(int iid)
    {
        TreasureFragmentsDT[] tmp = f_GetTreasureFragmentDT(iid);
        List<ccU3DEngine.BasePoolDT<long>> tmpList = Data_Pool.m_TreasureFragmentPool.f_GetAll();
        int[] tmpInt = new int[tmp.Length];
        for (int i = 0; i < tmpInt.Length; i++)
        {
            foreach (TreasureFragmentPoolDT item in tmpList)
            {
                if (tmp[i].iId == item.m_TreasureFragmentsDT.iId)
                    tmpInt[i] = item.m_num;
            }
        }

        return tmpInt;
    }
    /// <summary>
    /// 获取物品数量
    /// </summary>
    public static int f_GetGoodNum(EM_ResourceType resourceType, int resourceID)
    {
        return f_GetResourceHaveNum((int)resourceType, resourceID);
    }
    /// <summary>
    /// 获得卡牌阵营名称
    /// </summary>
    /// <param name="cardCamp"></param>
    /// <returns></returns>
    public static string f_GetSpriteNameByCampName(EM_CardCamp cardCamp)
    {
        string CampName = "";
        switch (cardCamp)
        {
            case EM_CardCamp.eCardMain:
                break;
            case EM_CardCamp.eCardWu:
                CampName = "Icon_Wu";
                break;
            case EM_CardCamp.eCardShu:
                CampName = "Icon_Shu";
                break;
            case EM_CardCamp.eCardWei:
                CampName = "Icon_Wei";
                break;
            case EM_CardCamp.eCardGroupHero:
                CampName = "Icon_Qun";
                break;
        }
        return CampName;
    }
    public static string f_GetMoneySpriteName(EM_MoneyType moneyType)
    {
        switch (moneyType)
        {
            case EM_MoneyType.eUserAttr_Sycee:
                return "Icon_Sycee";
            case EM_MoneyType.eUserAttr_Money:
                return "Icon_Money";
            case EM_MoneyType.eUserAttr_Gold://（不存在）
                return "Icon_Gold";
            case EM_MoneyType.eUserAttr_GodSoul:
                return "Icon_GodSoul";
            case EM_MoneyType.eUserAttr_Fame:
                return "Icon_Fame";//Icon_Evolve
            case EM_MoneyType.eUserAttr_BattleFeat:
                return "Icon_BattleFeat";
            case EM_MoneyType.eUserAttr_GeneralSoul:
                return "Icon_GeneralSoul";
            case EM_MoneyType.eUserAttr_Prestige:
                return "Icon_Prestige";
            case EM_MoneyType.eUserAttr_Vigor:
                return "Icon_Vigor";
            case EM_MoneyType.eUserAttr_Energy:
                return "Icon_Energy";
            case EM_MoneyType.eUserAttr_CrusadeToken:
                return "Icon_CrusadeToken";
            case EM_MoneyType.eUserAttr_LegionContribution:
                return "Icon_LegionContribution";//Icon_Mage
            case EM_MoneyType.eNorAd:
                return "Icon_NorAd";
            case EM_MoneyType.eGenAd:
                return "Icon_GenAd";
            case EM_MoneyType.eBattleFormFragment:
                return "Icon_FormFragment";
            case EM_MoneyType.eRedBattleToken://红色武将精华
                return "Icon_BattleToken";
            case EM_MoneyType.eRedEquipElite:
                return "Icon_EquipElite";
            case EM_MoneyType.eFreshToken:
                return "Icon_FreshObj";
            case EM_MoneyType.eUserAttr_CrossServerScore:
                return "Icon_CrossServerScore";
			case EM_MoneyType.eUserAttr_ChaosScore:
				return "Icon_ChaosPoint";
            case EM_MoneyType.eUserAttr_Coin:
                return "Icon_KP";
            case EM_MoneyType.eUserAttr_ArenaCorssMoney:
                return "Icon_ArenaCorssMoney";
            case EM_MoneyType.eUserAttr_TournamentPoint:
                return "Icon_ArenaCorssMoney";
            case EM_MoneyType.eCampAd:
                return "Icon_CampAd";
            case EM_MoneyType.eLimitAd:
                return "Icon_LimitAd";
            case EM_MoneyType.eGemCamp:
                return "Icon_GemCampAd";
        }
Debug.LogError("Currency Icon not found");
        return "";
    }
    /// <summary>
    /// 物品名称
    /// </summary>
    public static string f_GetGoodName(EM_ResourceType resourceType, int resourceID)
    {
        switch (resourceType)
        {
            case EM_ResourceType.Money://货币
                switch ((EM_MoneyType)resourceID)
                {
                    case EM_MoneyType.eUserAttr_Sycee:
                        return CommonTools.f_GetTransLanguage(1907);
                    case EM_MoneyType.eUserAttr_Money:
                        return CommonTools.f_GetTransLanguage(1908);
                    case EM_MoneyType.eUserAttr_Gold:
                        return CommonTools.f_GetTransLanguage(1909);
                    case EM_MoneyType.eUserAttr_GodSoul:
                        return CommonTools.f_GetTransLanguage(1910);
                    case EM_MoneyType.eUserAttr_Fame:
                        return CommonTools.f_GetTransLanguage(1911);
                    case EM_MoneyType.eUserAttr_BattleFeat:
                        return CommonTools.f_GetTransLanguage(1912);
                    case EM_MoneyType.eUserAttr_GeneralSoul:
                        return CommonTools.f_GetTransLanguage(1913);
                    case EM_MoneyType.eUserAttr_Prestige:
                        return CommonTools.f_GetTransLanguage(1914);
                    case EM_MoneyType.eUserAttr_Vigor:
                        return CommonTools.f_GetTransLanguage(1915);
                    case EM_MoneyType.eUserAttr_Energy:
                        return CommonTools.f_GetTransLanguage(1916);
                    case EM_MoneyType.eUserAttr_CrusadeToken:
                        return CommonTools.f_GetTransLanguage(1917);
                    case EM_MoneyType.eUserAttr_LegionContribution:
                        return CommonTools.f_GetTransLanguage(1918);
                    case EM_MoneyType.eUserAttr_CrossServerScore:
                        return CommonTools.f_GetTransLanguage(1919);
                    default: return CommonTools.f_GetTransLanguage(1920);
                }
            default:
                ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
                resourceCommonDT.f_UpdateInfo((byte)resourceType, resourceID, 0);
                return resourceCommonDT.mName;
        }
    }
    /// <summary>
    /// 获取物品描述
    /// </summary>
    public static string f_GetGoodDescribe(EM_ResourceType resourceType, int resourceID)
    {
        ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
        resourceCommonDT.f_UpdateInfo((byte)resourceType, resourceID, 0);
        return resourceCommonDT.mDesc;
    }
    /// <summary>
    /// 获取vip次数
    /// </summary>
    /// <returns></returns>
    public static int GetTimesByVip(int id, int vipLevel)
    {
        VipPrivilegeDT vipPrivilegeDT = (VipPrivilegeDT)glo_Main.GetInstance().m_SC_Pool.m_VipPrivilegeSC.f_GetSC(id);
        switch (vipLevel)
        {
            case 0: return vipPrivilegeDT.iLv0;
            case 1: return vipPrivilegeDT.iLv1;
            case 2: return vipPrivilegeDT.iLv2;
            case 3: return vipPrivilegeDT.iLv3;
            case 4: return vipPrivilegeDT.iLv4;
            case 5: return vipPrivilegeDT.iLv5;
            case 6: return vipPrivilegeDT.iLv6;
            case 7: return vipPrivilegeDT.iLv7;
            case 8: return vipPrivilegeDT.iLv8;
            case 9: return vipPrivilegeDT.iLv9;
            case 10: return vipPrivilegeDT.iLv10;
            case 11: return vipPrivilegeDT.iLv11;
            case 12: return vipPrivilegeDT.iLv12;
            case 13: return vipPrivilegeDT.iLv13;
            case 14: return vipPrivilegeDT.iLv14;
            case 15: return vipPrivilegeDT.iLv15;
            default: return vipPrivilegeDT.iLv15;
        }
    }
    /// <summary>
    /// 根据物品类型来获得物品ID
    /// </summary>
    /// <param name="EffectType"></param>
    /// <returns></returns>
    public static int[] f_GetGoodsForEffect(EM_GoodsEffect EffectType)
    {
        BetterList<int> tmp = new BetterList<int>();
        List<NBaseSCDT> tList = glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetAll();
        for (int index = 0; index < tList.Count; index++)
        {
            if (((BaseGoodsDT)tList[index]).iEffect == (int)EffectType)
                tmp.Add(tList[index].iId);
        }
        tmp.Sort((int a, int b) => { return a > b ? 1 : -1; });
        return tmp.ToArray();
    }
    /// <summary>
    /// 获取vip等级和所需经验
    /// </summary>
    /// <returns></returns>
    public static void f_GetVipLvAndNeedExp(int Exp, ref int vipLv, ref int needExp)
    {
        VipPrivilegeDT tmp = (VipPrivilegeDT)glo_Main.GetInstance().m_SC_Pool.m_VipPrivilegeSC.f_GetSC(1);
        Dictionary<string, int> Field = Data_Pool.m_RechargePool.VipSc[0];
        needExp = 0;
        vipLv = 0;
        foreach (KeyValuePair<string, int> item in Field)
        {
            if (item.Key == "iId" || item.Key == "szDesc" || item.Key == "iSite")
                continue;
            needExp = item.Value;
            if (Exp >= needExp)
            {
                vipLv++;
            }
            else
                break;
        }
        //确保不超过VIP等级最大值
        vipLv = Math.Min(GameParamConst.VipLvMaxNuM, vipLv);
    }
    public static int f_GetNowVipLv()
    {
        return UITool.f_GetVipLv(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_TempVip));
    }
    public static int f_GetNowVipPrivilege(int id)
    {
        int nowVip = f_GetNowVipLv();
        return GetTimesByVip(id, nowVip);
    }
    public static void f_GetMaxNeedExp(int Exp, ref int needExp)
    {
        VipPrivilegeDT tmp = (VipPrivilegeDT)glo_Main.GetInstance().m_SC_Pool.m_VipPrivilegeSC.f_GetSC(1);
        Dictionary<string, int> Field = Data_Pool.m_RechargePool.VipSc[0];
        needExp = 0;
        foreach (KeyValuePair<string, int> item in Field)
        {
            if (item.Key == "iId" || item.Key == "szDesc" || item.Key == "iSite")
                continue;
            if (Exp >= item.Value)
            {
                needExp = item.Value;
            }
            else
                break;
        }
    }
    //获取VIP等级
    public static int f_GetVipLv(int exp)
    {
        VipPrivilegeDT tmp = (VipPrivilegeDT)glo_Main.GetInstance().m_SC_Pool.m_VipPrivilegeSC.f_GetSC(1);
        Dictionary<string, int> Field = Data_Pool.m_RechargePool.VipSc[0];
        int needExp = 0;
        int vipLv = 0;
        foreach (KeyValuePair<string, int> item in Field)
        {
            if (item.Key == "iId" || item.Key == "szDesc" || item.Key == "iSite")
                continue;
            needExp = item.Value;
            if (exp >= needExp)
            {
                vipLv++;
            }
        }
        vipLv = Math.Min(GameParamConst.VipLvMaxNuM, vipLv);
        return vipLv;
    }
    /// <summary>
    /// 检查物品是否足够,不够则弹出提示并返回false
    /// </summary>
    public static bool CheckGoodEnoughWishHint(EM_ResourceType resourceType, int resourId, int needValue)
    {
        bool isEnough = true;
        int hasNum = f_GetGoodNum(resourceType, resourId);
        string name = f_GetGoodName(resourceType, resourId);
        if (hasNum < needValue)
        {
            Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1921), name));
        }
        isEnough = hasNum < needValue ? false : true;
        return isEnough;
    }
    /// <summary>
    /// 通过等级获取该等级需要的经验值
    /// </summary>
    /// <returns></returns>
    public static int f_GetExByLevel(int Level)
    {
        CarLvUpDT carLvUpDT = (CarLvUpDT)glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(Level);
        if (carLvUpDT != null)
        {
            return carLvUpDT.iCardType;
        }
        return 0;
    }
    /// <summary>
    /// 获取VIP描述
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static BetterList<string> f_GetVipDesc(int lv)
    {
        BetterList<string> tmp = new BetterList<string>();
        for (int index = 1; index < Data_Pool.m_RechargePool.VipSc.Length; index++)
        {
            foreach (KeyValuePair<string, int> item in Data_Pool.m_RechargePool.VipSc[index])
            {
                if (item.Value != 0 && item.Key == ("iLv" + lv))
                {
                    tmp.Add(((VipPrivilegeDT)glo_Main.GetInstance().m_SC_Pool.m_VipPrivilegeSC.f_GetSC(index + 1)).
                        szDesc.Replace(GameParamConst.ReplaceFlag, index + 1 == 11 || index + 1 == 12 ? (item.Value / 100) + "%" : item.Value.ToString()));
                }
            }
        }
        return tmp;
    }

    public static AwardCentreDT f_GetAwardType(int itype)
    {
        List<NBaseSCDT> tmp = glo_Main.GetInstance().m_SC_Pool.m_AwardCentreSC.f_GetAll();
        for (int i = 0; i < tmp.Count; i++)
        {
            if (tmp[i].iId == itype)
                return (AwardCentreDT)tmp[i];
        }
        return null;
    }

    /// <summary>
    /// 通过时间戳 返回距离现在的时间描述  0：代表在线
    /// </summary>
    /// <param name="second">传入时间戳</param>
    /// <returns></returns>
    public static string f_GetTimeDescFromNow(int iTime)
    {
        if (iTime == 0)
            return CommonTools.f_GetTransLanguage(1922);
        int second = GameSocket.GetInstance().f_GetServerTime() - iTime;
        string timeStr = string.Empty;
        int day = second / 86400; //60*60*24
        if (day > 0)
        {
            timeStr = string.Format(CommonTools.f_GetTransLanguage(1923), day);
            return timeStr;
        }
        int hour = second / 3600;
        if (hour > 0)
        {
            timeStr = string.Format(CommonTools.f_GetTransLanguage(1924), hour);
            return timeStr;
        }
        int minute = (second - hour * 60 * 60) / 60;
        if (minute > 0)
        {
            timeStr = string.Format(CommonTools.f_GetTransLanguage(1925), minute);
            return timeStr;
        }
        timeStr = CommonTools.f_GetTransLanguage(1926);
        return timeStr;
    }
    #region 数字变成中文

    /// <summary>
    /// int 数字变成中文（银币、元宝、战斗力，位数达到6位数（十万）显示“XXX万”，位数到9位及以上显示"XXX亿XXX万“）
    /// </summary>
    public static string f_CountToChineseStr(int count)
    {
        string text = "";
        int str1 = count / 1000000000;
        int temp = count % 1000000000;
        int str2 = temp / 1000000;
        int str3 = temp % 1000000;
        int temp2 = str3 / 1000;
        if (str1 > 0)
        {
            text += str1.ToString() + CommonTools.f_GetTransLanguage(1927);
            if (str2 > 0)
            {
                //text += str2.ToString() + CommonTools.f_GetTransLanguage(2262) + temp2 + CommonTools.f_GetTransLanguage(1829); có phần K
				text += str2.ToString() + CommonTools.f_GetTransLanguage(2262);
            }
        }
        else
        {
            if (str2 > 0)
            {
                text += str2.ToString() + CommonTools.f_GetTransLanguage(2262) + temp2 + CommonTools.f_GetTransLanguage(1829);
            }
            else
            {
                text += str3.ToString();
            }
        }
        return text;
    }

    /// <summary>
    /// long 数字变成中文（银币、元宝、战斗力，位数达到6位数（十万）显示“XXX万”，位数到9位及以上显示"XXX亿XXX万“）
    /// </summary>
    public static string f_CountToChineseStr(long count)
    {
        string text = "";
        long str1 = count / 1000000000;
        long temp = count % 1000000000;
        long str2 = temp / 1000000;
        long str3 = temp % 1000000;
        long temp2 = str3 / 1000;
        if (str1 > 0)
        {
            text += str1.ToString() + CommonTools.f_GetTransLanguage(1927);
            if (str2 > 0)
            {
                //text += str2.ToString() + temp2 + CommonTools.f_GetTransLanguage(1829);
				text += str2.ToString() + CommonTools.f_GetTransLanguage(2262);
            }
        }
        else
        {
            if (str2 > 0)
            {
                text += str2.ToString() + CommonTools.f_GetTransLanguage(2262) + temp2 + CommonTools.f_GetTransLanguage(1829);
            }
            else
            {
                text += str3.ToString();
            }
        }
        return text;
    }


    public static string f_CountToChineseStr2(int count)
    {
        // if (count >= 10000000)
        // {
            // return f_CountToChineseStr(count);
        // }

        // string text = "";
        // int str1 = count / 100000000;
        // int temp = count % 100000000;
        // int str2 = temp / 10000000;
        // int str3 = temp % 10000000;
        // int temp2 = str3 / 10000;
        // if (str1 > 0)
        // {
            // text += str1.ToString() + CommonTools.f_GetTransLanguage(1927);
            // if (str2 > 0)
            // {
                // text += str2.ToString() + temp2 + CommonTools.f_GetTransLanguage(1829);
            // }
        // }
        // else
        // {
            // if (str2 > 0)
            // {
                // text += str2.ToString() + temp2 + CommonTools.f_GetTransLanguage(1829);
            // }
            // else
            // {
                // text += str3.ToString();
            // }
        // }
        // return text;
		
		string text = "";
        int str1 = count / 1000000000;
        int temp = count % 1000000000;
        int str2 = temp / 1000000;
        int str3 = temp % 1000000;
        int temp2 = str3 / 1000;
        if (str1 > 0)
        {
            text += str1.ToString() + CommonTools.f_GetTransLanguage(1927);
            if (str2 > 0)
            {
                //text += str2.ToString() + CommonTools.f_GetTransLanguage(2262) + temp2 + CommonTools.f_GetTransLanguage(1829); có phần K
				text += str2.ToString() + CommonTools.f_GetTransLanguage(2262);
            }
        }
        else
        {
            if (str2 > 0)
            {
                text += str2.ToString() + CommonTools.f_GetTransLanguage(2262) + temp2 + CommonTools.f_GetTransLanguage(1829);
            }
            else
            {
                text += str3.ToString();
            }
        }
        return text;
    }


    #endregion
    #region 创建模型相关的方法

    /// <summary>
    /// 创建角色 并播放默认待机动画
    /// </summary>
    /// <param name="cardId"></param>
    /// <param name="role"></param>
    /// <param name="roleParent"></param>
    /// <param name="order"></param>
    public static void f_CreateRoleByModeId(int modeId, ref GameObject role, Transform roleParent, int order, bool needToShowRedCard = true)
    {
        if (role != null)
        {
            try
            {
                UITool.f_DestoryStatelObject(role);
            }
            catch (Exception)
            {
                role = null;
            }

        }
        RoleModelDT tRoleModelDt = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(modeId);
        if (tRoleModelDt != null)
            role = glo_Main.GetInstance().m_ResourceManager.f_CreateRole(tRoleModelDt.iModel, needToShowRedCard, true, tRoleModelDt.iId);
        else
        {
MessageBox.ASSERT("The model could not be found in the model，id table:" + modeId);
        }
        if (role != null)
        {
            role.transform.parent = roleParent;
            role.transform.localPosition = Vector3.zero;
            role.transform.localRotation = Quaternion.identity;
            role.transform.localScale = Vector3.one;
            role.layer = LayerMask.NameToLayer(GameParamConst.UILayerName);
            SkeletonAnimation SkeAni = role.GetComponent<SkeletonAnimation>();
            
            //SkeAni.loop = true;
            //SkeAni.AnimationName = "Stand";
            SkeAni.state.SetAnimation(0, "Stand", true);
            role.GetComponent<Renderer>().sortingOrder = order;
            f_ShowBone(SkeAni, order);
        }
    }
    public static GameObject f_CreateRoleByModeId(int modeId, Transform modelParent, Vector3 localEulerAngles, Vector3 localPosition,
        int sortingOrder = 1, string ModelName = "Model", int scaleSize = 150, bool needToShowRedCard = true)
    {
        RoleModelDT tRoleModelDt = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(modeId);
        GameObject CardReson = glo_Main.GetInstance().m_ResourceManager.f_CreateRole(tRoleModelDt.iModel, needToShowRedCard, true, tRoleModelDt.iId);
        if (CardReson == null)
            return null;
        CardReson.transform.parent = modelParent;
        CardReson.transform.localEulerAngles = localEulerAngles;
        CardReson.transform.localPosition = localPosition;
        CardReson.transform.localScale = Vector3.one * scaleSize;
        CardReson.layer = LayerMask.NameToLayer(GameParamConst.UILayerName);
        SkeletonAnimation SkeAni = CardReson.GetComponent<SkeletonAnimation>();
      
        //SkeAni.loop = true;
        //SkeAni.AnimationName = "Stand";
        SkeAni.state.SetAnimation(0, "Stand", true);
        CardReson.GetComponent<Renderer>().sortingOrder = sortingOrder;
        CardReson.GetComponent<Renderer>().sortingLayerName = "Default";
        CardReson.name = ModelName;
        f_ShowBone(SkeAni, sortingOrder);
        return CardReson;
    }
    public static void f_CreateRoleByCardId(int cardId, ref GameObject role, Transform roleParent, int order, float scale = 1, bool Skill3 = false, bool needToShowRedCard = true)
    {
        if (role != null)
        {
            UITool.f_DestoryStatelObject(role);
            role = null;
        }
        role = UITool.f_GetStatelObject(cardId, needToShowRedCard);
        if (role != null)
        {
            role.transform.parent = roleParent;
            role.transform.localPosition = Vector3.zero;
			//My Code
			// if(cardId == 1201 && Skill3 == true)
			// {
				// role.transform.localPosition = new Vector3(-1.35f, -1.8f, 0);
			//}
			//
            role.transform.localRotation = Quaternion.identity;
            role.transform.localScale = new Vector3(scale, scale, scale);
            role.layer = LayerMask.NameToLayer(GameParamConst.UILayerName);
            SkeletonAnimation SkeAni = role.GetComponent<SkeletonAnimation>();
           
            //SkeAni.loop = true;
            //SkeAni.AnimationName = "Stand";
            SkeAni.state.SetAnimation(0, "Stand", true);
            role.GetComponent<Renderer>().sortingOrder = order;
            f_ShowBone(SkeAni, order);
        }
        else
        {
            f_CreateRoleByFashId(cardId, ref role, roleParent, order);
        }
    }

    public static void f_CreateRoleByFashId(int FashID, ref GameObject role, Transform roleParent, int order, bool needToShowRedCard = true)
    {
        FashionableDressDT tFashionableDressDT = glo_Main.GetInstance().m_SC_Pool.m_FashionableDressSC.f_GetSC(FashID) as FashionableDressDT;
        if (tFashionableDressDT == null)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1928) + FashID);
            return;
        }

        if (role != null)
        {
            UITool.f_DestoryStatelObject(role);
            role = null;
        }
        role = glo_Main.GetInstance().m_ResourceManager.f_CreateRole(tFashionableDressDT.iModel, needToShowRedCard);
		// MessageBox.ASSERT("FashID: " + tFashionableDressDT.iModel);
        if (role != null)
        {
            role.transform.parent = roleParent;
            role.transform.localPosition = Vector3.zero;
            role.transform.localRotation = Quaternion.identity;
            role.transform.localScale = Vector3.one;
            role.layer = LayerMask.NameToLayer(GameParamConst.UILayerName);
            SkeletonAnimation SkeAni = role.GetComponent<SkeletonAnimation>();
           
            //SkeAni.loop = true;
            //SkeAni.AnimationName = "Stand";
            SkeAni.state.SetAnimation(0, "Stand", true);
            role.GetComponent<Renderer>().sortingOrder = order;
            f_ShowBone(SkeAni, order);
        }
    }
	
	public static void f_CreateFashRoleByModeId(int modeId, ref GameObject role, Transform roleParent, int order, bool needToShowRedCard = true)
    {
        if (role != null)
        {
            try
            {
                UITool.f_DestoryStatelObject(role);
            }
            catch (Exception)
            {
                role = null;
            }

        }
        if (modeId != null)
            role = glo_Main.GetInstance().m_ResourceManager.f_CreateRole(modeId, needToShowRedCard);
        else
        {
MessageBox.ASSERT("The model could not be found in the model，id table:" + modeId);
        }
        if (role != null)
        {
            role.transform.parent = roleParent;
            role.transform.localPosition = Vector3.zero;
            role.transform.localRotation = Quaternion.identity;
            role.transform.localScale = Vector3.one;
            role.layer = LayerMask.NameToLayer(GameParamConst.UILayerName);
            SkeletonAnimation SkeAni = role.GetComponent<SkeletonAnimation>();
            
            //SkeAni.loop = true;
            //SkeAni.AnimationName = "Stand";
            SkeAni.state.SetAnimation(0, "Stand", true);
            role.GetComponent<Renderer>().sortingOrder = order;
            f_ShowBone(SkeAni, order);
        }
    }

    public static SkeletonAnimation f_CreateMagicById(int magicId, ref GameObject magic, Transform magicParent, int order, string animationName, Spine.AnimationState.TrackEntryDelegate aniEndEvent, bool loop = false, float scale = 1f)
    {
        if (magic != null)
        {
            UITool.f_DestoryStatelObject(magic);
            magic = null;
        }
        magic = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(magicId);

        if (magic != null)
        {
            magic.transform.parent = magicParent;
            magic.transform.localPosition = Vector3.zero;
            magic.transform.localRotation = Quaternion.identity;
            magic.transform.localScale = new Vector3(scale, scale, scale);
            magic.layer = LayerMask.NameToLayer(GameParamConst.UILayerName);
            SkeletonAnimation SkeAni = magic.GetComponent<SkeletonAnimation>();
            if (aniEndEvent != null)
			{
                SkeAni.state.Complete += aniEndEvent;   //End应该为加载结束   Complete为当前动画结束播放
			}
            if (null == SkeAni)
            {
Debug.LogError("SkeletonAnimation null，effect name:" + magic.name);
                return null;
            }

            if (animationName.Equals(""))
            {
                SkeAni.state.SetEmptyAnimation(0, 0);
            }
            else
            {
                SkeAni.state.SetAnimation(0, animationName, loop);
            }
            magic.GetComponent<Renderer>().sortingOrder = order;
            return SkeAni;
        }
        return null;
    }

    /// <summary>
    /// 生成骨骼gameobject
    /// </summary>
    /// <param name="SkeAni"></param>
    public static void f_ShowBone(SkeletonAnimation SkeAni, int order=0, int rarity = 0)
    {
        //2019.08.17修改，影子统一比角色少一个order
		
        Transform skeAniTf = SkeAni.transform;
        Transform shadow = skeAniTf.Find(GameParamConst.prefabShadowName);
        if (shadow == null)
        {
            // GameObject o = new GameObject(GameParamConst.prefabShadowName);
            // Transform t = o.transform;
            // t.parent = skeAniTf.gameObject.transform;
            // t.localPosition = Vector3.zero;
            // t.localRotation = Quaternion.identity;
            // t.localScale = Vector3.one;
            // o.layer = skeAniTf.gameObject.layer;
            // shadow = t;
            // shadow.name = GameParamConst.prefabShadowName;
            // BoneFollower bf = o.AddComponent<BoneFollower>();
            // bf.skeletonRenderer = SkeAni;
            // bool had = bf.SetBone(GameParamConst.createShadowBoneName);
            // GameObject magic = null;
			
            //f_CreateMagicById((int)EM_MagicId.eShadow, ref magic, shadow, 0, null);
            ////Renderer renderer = magic.transform.GetComponent<Renderer>();
            // if (magic!=null) {
                // magic.transform.localScale = Vector3.one * GameParamConst.shadowScalePerc;
                // RenderAuto c = magic.AddComponent<RenderAuto>();
                // c.init(skeAniTf.GetComponent<Renderer>());
            // }
//            GameObject go = glo_Main.GetInstance().m_ResourceManager.f_CreateShadow();
//            go.transform.parent = SkeAni.transform;
//            go.transform.localPosition = Vector3.zero;
//            go.transform.localScale = Vector3.one * GameParamConst.shadowScalePerc;
//            go.transform.name = GameParamConst.prefabShadowName;
        }
		
		//Aura
		/*Transform aura = skeAniTf.Find("Aura");
        if (aura == null)
        {
            GameObject o2 = new GameObject("Aura");
            Transform t2 = o2.transform;
            t2.parent = skeAniTf.gameObject.transform;
            t2.localPosition = Vector3.zero;
            t2.localRotation = Quaternion.identity;
            t2.localScale = Vector3.one;
            o2.layer = skeAniTf.gameObject.layer;
            aura = t2;
            aura.name = "Aura";
            BoneFollower bf2 = o2.AddComponent<BoneFollower>();
            bf2.skeletonRenderer = SkeAni;
            bool had2 = bf2.SetBone("AuraEff");
            GameObject magic = null;
			if(rarity == 3)
			{
				f_CreateMagicById(20043, ref magic, aura, 0, null, "animation_lan_a", true);
			}
			if(rarity == 4)
			{
				f_CreateMagicById(20043, ref magic, aura, 0, null, "animation_zi_b", true);
			}
			if(rarity == 5)
			{
				f_CreateMagicById(20043, ref magic, aura, 0, null, "animation_cheng_c", true);
			}
			if(rarity == 6)
			{
				f_CreateMagicById(20043, ref magic, aura, 0, null, "animation_hong_d", true);
			}
			if(rarity == 7)
			{
				f_CreateMagicById(20043, ref magic, aura, 0, null, "animation_jin", true);
			}
            //Renderer renderer = magic.transform.GetComponent<Renderer>();
            if (magic!=null) {
                magic.transform.localScale = Vector3.one * 1f;
                RenderAuto c = magic.AddComponent<RenderAuto>();
                c.init(skeAniTf.GetComponent<Renderer>());
            }
//            GameObject go = glo_Main.GetInstance().m_ResourceManager.f_CreateShadow();
//            go.transform.parent = SkeAni.transform;
//            go.transform.localPosition = Vector3.zero;
//            go.transform.localScale = Vector3.one * GameParamConst.shadowScalePerc;
//            go.transform.name = GameParamConst.prefabShadowName;
        }*/
		//
    }

    public static void f_CreateMagicById(int magicId, ref GameObject magic, Transform magicParent, int order, Spine.AnimationState.TrackEntryDelegate aniEndEvent, string animation = "animation", bool loop = false)
    {
        if (magic != null)
        {
            UITool.f_DestoryStatelObject(magic);
            magic = null;
        }
        magic = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(magicId);
        if (magic != null)
        {
            magic.transform.parent = magicParent;
            magic.transform.localPosition = Vector3.zero;
            magic.transform.localRotation = Quaternion.identity;
            magic.transform.localScale = Vector3.one;
            magic.layer = LayerMask.NameToLayer(GameParamConst.UILayerName);
            SkeletonAnimation SkeAni = magic.GetComponent<SkeletonAnimation>();
            if (null == SkeAni)
            {
Debug.LogError("SkeletonAnimation null，effect name:" + magic.name);
                return;
            }
            SkeAni.state.SetAnimation(0, animation, loop);
            magic.GetComponent<Renderer>().sortingOrder = order;
            if (aniEndEvent != null)
                SkeAni.state.End += aniEndEvent;
        }
    }

    /// <summary>
    /// 根据卡牌id创建角色模型（并设置初始化状态）
    /// </summary>
    /// <param name="cardId">卡牌id</param>
    /// <param name="modelParent">模型父物体</param>
    /// <param name="localEulerAngles">初始化的本地旋转状态</param>
    /// <param name="localPosition">初始化的本地位置</param>
    /// <param name="sortingOrder">模型的渲染层级设置（默认为1）</param>
    /// <param name="ModelName">模型名字（默认为'Model'）</param>
    /// <param name="scaleSize">模型的缩放（默认为150）</param>
    /// <param name="layer">层（默认为5）</param>
    /// <returns></returns>
    public static GameObject f_GetStatelObject(int cardId, Transform modelParent, Vector3 localEulerAngles, Vector3 localPosition,
        int sortingOrder = 1, string ModelName = "Model", int scaleSize = 150, bool needToShowRedCard = true)
    {
        GameObject CardReson = f_GetStatelObject(cardId, needToShowRedCard);
        if (CardReson == null)
            return null;
        return CreateRole(CardReson, modelParent, localEulerAngles, localPosition, sortingOrder, ModelName, scaleSize);
    }
    public static GameObject f_GetStatelObjectByFashId(int FashId, Transform modelParent, Vector3 localEulerAngles, Vector3 localPosition,
        int sortingOrder = 1, string ModelName = "Model", int scaleSize = 150, bool needToShowRedCard = true)
    {
        FashionableDressDT tFashionableDressDT = glo_Main.GetInstance().m_SC_Pool.m_FashionableDressSC.f_GetSC(FashId) as FashionableDressDT;
        GameObject CardReson = glo_Main.GetInstance().m_ResourceManager.f_CreateRole(tFashionableDressDT.iModel, needToShowRedCard);
        if (CardReson == null)
            return null;
        return CreateRole(CardReson, modelParent, localEulerAngles, localPosition, sortingOrder, ModelName, scaleSize);
    }


    /// <summary>
    /// 根据卡牌id创建角色模型（并设置初始化状态）
    /// </summary>
    /// <param name="cardId">卡牌id</param>
    /// <param name="modelParent">模型父物体</param>
    /// <param name="localEulerAngles">初始化的本地旋转状态</param>
    /// <param name="localPosition">初始化的本地位置</param>
    /// <param name="sortingOrder">模型的渲染层级设置（默认为1）</param>
    /// <param name="ModelName">模型名字（默认为'Model'）</param>
    /// <param name="scaleSize">模型的缩放（默认为150）</param>
    /// <param name="layer">层（默认为5）</param>
    /// <returns></returns>
    public static GameObject f_GetStatelObject(CardPoolDT tCardPoolDT, Transform modelParent, Vector3 localEulerAngles, Vector3 localPosition,
        int sortingOrder = 1, string ModelName = "Model", int scaleSize = 150, bool needToShowRedCard = true)
    {
        GameObject CardReson = f_GetStatelObject(tCardPoolDT, needToShowRedCard);
        if (CardReson == null)
            return null;
        return CreateRole(CardReson, modelParent, localEulerAngles, localPosition, sortingOrder, ModelName, scaleSize);
    }

    private static GameObject CreateRole(GameObject CardReson, Transform modelParent, Vector3 localEulerAngles, Vector3 localPosition,
        int sortingOrder = 1, string ModelName = "Model", int scaleSize = 150)
    {
        if (CardReson == null)
            return null;
        CardReson.transform.parent = modelParent;
        CardReson.transform.localEulerAngles = localEulerAngles;
        CardReson.transform.localPosition = localPosition;
        CardReson.transform.localScale = Vector3.one * scaleSize;
        CardReson.layer = LayerMask.NameToLayer(GameParamConst.UILayerName);
        SkeletonAnimation SkeAni = CardReson.GetComponent<SkeletonAnimation>();
        if (null == SkeAni)
        {
Debug.LogError("SkeletonAnimation null，modelname:" + CardReson.name);
            return CardReson;
        }
        //SkeAni.loop = true;
        //SkeAni.AnimationName = "Stand";
		//.TimeScale = 0.4f
        SkeAni.state.SetAnimation(0, "Stand", true);
        CardReson.GetComponent<Renderer>().sortingOrder = sortingOrder;
        CardReson.GetComponent<Renderer>().sortingLayerName = "Default";
        CardReson.name = ModelName;
        return CardReson;
    }


    #endregion

    #region 灰显

    /// <summary>
    /// 设置灰显
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="isGray"></param>
    public static void f_SetSpriteGray(UISprite sprite, bool isGray)
    {
		if(sprite != null)
		{
			sprite.color = isGray ? new Color(0, 0.2f, 0.2f, 1) : Color.white;
		}
    }
    public static void f_SetSpriteGray(GameObject go, bool isGray)
    {
        if (go == null) return;
        go.GetComponentsInChildren<UISprite>();
        foreach (UISprite item in go.GetComponentsInChildren<UISprite>())
        {
            item.color = isGray ? new Color(0, 0.2f, 0.2f, 1) : Color.white;
        }
        foreach (BoxCollider item in go.GetComponentsInChildren<BoxCollider>())
        {
            item.enabled = !isGray;
        }
    }
    /// <summary>
    /// 设置灰显
    /// </summary>
    /// <param name="twoDSprite"></param>
    /// <param name="isGray"></param>
    public static void f_Set2DSpriteGray(UI2DSprite twoDSprite, bool isGray)
    {
        twoDSprite.color = isGray ? new Color(0, 0.2f, 0.2f, 1) : Color.white;
    }

    public static void f_Set2DSpriteGray(GameObject go, bool isGray)
    {
        if (go == null) return;
        go.GetComponentsInChildren<UI2DSprite>();
        foreach (UI2DSprite item in go.GetComponentsInChildren<UI2DSprite>())
        {
            item.color = isGray ? new Color(0, 0.2f, 0.2f, 1) : Color.white;
        }
        foreach (BoxCollider item in go.GetComponentsInChildren<BoxCollider>())
        {
            item.enabled = !isGray;
        }
    }

    public static void f_SetTextureGray(UITexture twoDSprite, bool isGray)
    {
        twoDSprite.color = isGray ? new Color(0, 0.2f, 0.2f, 1) : Color.white;
    }

    #endregion

    #region 特效
    /// <summary>
    /// 创建装备特效
    /// </summary>
    /// <param name="effectParent"></param>
    /// <param name="name"></param>
    /// <param name="important"></param>
    /// <param name="localposition"></param>
    /// <param name="localScale"></param>
    public static void f_CreateEquipEffect(Transform effectParent, string name, EM_Important important, Vector3 localposition, Vector3 localScale)
    {
        string EffectName;
        switch (important)
        {
            case EM_Important.White:
            case EM_Important.Green:
                EffectName = UIEffectName.biankuangliuguang_lv;
                break;
            case EM_Important.Blue:
                EffectName = UIEffectName.biankuangliuguang_lan;
                break;
            case EM_Important.Purple:
                EffectName = UIEffectName.biankuangliuguang_zi;
                break;
            case EM_Important.Oragen:
                EffectName = UIEffectName.biankuangliuguang_cheng;
                break;
            case EM_Important.Red:
                EffectName = UIEffectName.biankuangliuguang_hong;
                break;
            case EM_Important.Gold:
                goto case EM_Important.Red;
            default:
                goto case EM_Important.Green;
        }
        GameObject SetEquipEffect = UITool.f_CreateEffect_Old(EffectName, effectParent, Vector3.zero, 1f, 0, UIEffectName.UIEffectAddress1);
        SetEquipEffect.GetComponent<ParticleScaler>().particleScale = 1f;
        SetEquipEffect.name = "effect";
        SetEquipEffect.transform.localPosition = localposition;
        SetEquipEffect.transform.localScale = localScale;
    }
    /// <summary>
    ///创建UI特效
    /// </summary>
    /// <param name="EffectName">特效名字</param>
    /// <returns></returns>
    public static GameObject f_CreateEffect_Old(string EffectName, Transform EffectParent, Vector3 EffectlocalPosition, float particleScale, float time, string effectAddress)
    {
        GameObject tEffect = Resources.Load<GameObject>(effectAddress + EffectName);
        tEffect.SetActive(true);
        GameObject Effect = GameObject.Instantiate(tEffect);
        Effect.transform.parent = EffectParent;
        Effect.AddComponent<ParticleScaler>().alsoScaleGameobject = false;
        Effect.transform.localScale = Vector3.one;
        Effect.transform.localPosition = EffectlocalPosition;
        Effect.transform.localScale = Vector3.one * particleScale;
        if (time > 0)
            GameObject.Destroy(Effect, time);
        return Effect;
    }


    /// <summary>
    /// 第二批特效
    /// </summary>
    /// <param name="EffectName"></param>
    /// <param name="EffectParent"></param>
    /// <param name="EffectlocalPosition"></param>
    /// <param name="particleScale"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static GameObject f_CreateEffect2(string EffectName, Transform EffectParent, Vector3 EffectlocalPosition, float particleScale, float time)
    {
        GameObject tEffect = Resources.Load<GameObject>(UIEffectName.UIEffectAddress2 + EffectName);
        tEffect.SetActive(false);
        GameObject Effect = GameObject.Instantiate(tEffect);
        Effect.transform.parent = EffectParent;
        Effect.AddComponent<ParticleScaler>().alsoScaleGameobject = false;
        Effect.transform.localScale = Vector3.one;
        Effect.transform.localPosition = EffectlocalPosition;
        Effect.transform.localScale = Vector3.one * particleScale;
        if (time > 0)
            GameObject.Destroy(Effect, time);
        return Effect;
    }
    /// <summary>
    /// <summary>
    /// 创建特效
    /// </summary>
    /// <param name="effectAddress">特效路径</param>
    /// <param name="effectName">特效名字</param>
    /// <param name="effectParent">特效父对象</param>
    /// <param name="scaleGameobj">是否改变GameObjectScale</param>
    /// <param name="effectScale">特效大小</param>
    /// <param name="time">删除特效时间</param>
    /// <returns></returns>
    public static GameObject f_CreateEffect(string effectAddress, string effectName, Transform effectParent, float posX = 0, float posY = 0, bool scaleGameobj = false, float effectScale = 1, float time = 0)
    {
		// MessageBox.ASSERT("Name Eff: " + effectAddress + effectName);
        GameObject effectRes = Resources.Load<GameObject>(effectAddress + effectName);
        effectRes.SetActive(true);
        GameObject effectInstant = GameObject.Instantiate(effectRes);
        effectInstant.transform.parent = effectParent;
        effectInstant.transform.localPosition = new Vector3(posX, posY, 0);
        if (!scaleGameobj)
            effectInstant.transform.localScale = Vector3.one;
        ParticleScaler particleScaler = effectInstant.AddComponent<ParticleScaler>();
        particleScaler.alsoScaleGameobject = scaleGameobj;
        particleScaler.particleScale = effectScale;
        if (time > 0)
            GameObject.Destroy(effectInstant, time);
        return effectInstant;
    }



    /// <summary>
    /// 创建UI特效
    /// </summary>
    /// <param name="EffectName">特效名字</param>
    /// <param name="EffectParent">特效父级</param>
    /// <param name="EffectlocalPosition">在父级中的相对位置</param>
    /// <param name="particleScale">缩小比例</param>
    /// <param name="time">销毁的时间</param>
    /// <param name="SortingOrder"> 如果有拖尾组件就来设置层级    没有就不用填写</param>
    /// <param name="alsoScaleGameobject"> 是否影响该特效的比例</param>
    /// <returns></returns>
    //public static GameObject f_CreateEffect(string EffectName, Transform EffectParent, Vector3 EffectlocalPosition, float particleScale, float time, int SortingOrder = 0, bool alsoScaleGameobject = true)
    //{
    //    GameObject tEffect = Resources.Load<GameObject>(UIEffectName.UIEffectAddress + EffectName);
    //    GameObject Effect = GameObject.Instantiate(tEffect);
    //    Effect.transform.parent = EffectParent;
    //    ParticleScaler tScaler = Effect.AddComponent<ParticleScaler>();
    //    tScaler.particleScale = particleScale;
    //    tScaler.alsoScaleGameobject = alsoScaleGameobject;
    //    if (SortingOrder > 0)
    //        tScaler.TrailRenderSortingOrder = SortingOrder;
    //    Effect.transform.localPosition = EffectlocalPosition;
    //    if (time > 0)
    //        GameObject.Destroy(Effect, time);
    //    return Effect;
    //}

    #endregion
    #region 切换页面/前往接口

    #region 任务界面前往相关
    private static ccUIBase taskUIBase;
    public static void f_DailyGoto(ccUIBase thisBase, EM_DailyTaskCondition condition)
    {
        taskUIBase = thisBase;
        switch (condition)
        {
            case EM_DailyTaskCondition.MainTollgate:
                UITool.f_GotoPage(thisBase, UINameConst.DungeonChapterPageNew, (int)EM_Fight_Enum.eFight_DungeonMain);
                break;
            case EM_DailyTaskCondition.LegendTollgate:
                UITool.f_GotoPage(thisBase, UINameConst.DungeonChapterPageNew, (int)EM_Fight_Enum.eFight_Legend);
                break;
            case EM_DailyTaskCondition.RunningMan:
                UITool.f_GotoPage(thisBase, UINameConst.RunningManPage, (int)EM_Fight_Enum.eFight_DungeonMain);
                break;
            case EM_DailyTaskCondition.Arena:
                int curLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
                if (Data_Pool.m_ArenaPool.f_CheckArenaLvLimit(curLv))
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1929), UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel)));
                else
                    f_RequestArenaList();
                break;
            case EM_DailyTaskCondition.TreasureSynthesis:
                if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel))
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1930), UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel)));
                else
                    UITool.f_GotoPage(thisBase, UINameConst.GrabTreasurePage, 0);
                break;
            case EM_DailyTaskCondition.EquipIntensify:
                UITool.f_GotoPage(thisBase, UINameConst.EquipBagPage, 0);
                break;
            case EM_DailyTaskCondition.TreasureIntensify:
                UITool.f_GotoPage(thisBase, UINameConst.TreasureBagPage, 0);
                break;
            case EM_DailyTaskCondition.Train:
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1931));
                break;
            case EM_DailyTaskCondition.EquipRefine:
                UITool.f_GotoPage(thisBase, UINameConst.EquipBagPage, 0);
                break;
            case EM_DailyTaskCondition.SendVigor2Friend:
                UITool.f_GotoPage(thisBase, UINameConst.FriendPage, 0);
                break;
            case EM_DailyTaskCondition.ShopBattleGeneral:
                UITool.f_GotoPage(thisBase, UINameConst.ShopPage, (int)ShopPage.EM_PageIndex.RecruitContent);
                break;
            case EM_DailyTaskCondition.ShopGodGeneral:
                UITool.f_GotoPage(thisBase, UINameConst.ShopPage, (int)ShopPage.EM_PageIndex.RecruitContent);
                break;
            case EM_DailyTaskCondition.CrusadeRebel:
                UITool.f_GotoPage(thisBase, UINameConst.RebelArmy, 0);
                break;
            case EM_DailyTaskCondition.ShareCrusadeRebel:
                UITool.f_GotoPage(thisBase, UINameConst.RebelArmy, 0);
                break;
            case EM_DailyTaskCondition.ShopVigorPill:
                UITool.f_GotoPage(thisBase, UINameConst.ShopPage, (int)ShopPage.EM_PageIndex.PropsContent);
                break;
            case EM_DailyTaskCondition.ShopEnergyPill:
                UITool.f_GotoPage(thisBase, UINameConst.ShopPage, (int)ShopPage.EM_PageIndex.PropsContent);
                break;
            case EM_DailyTaskCondition.HelpAgainstRiot:
                UITool.f_GotoPage(thisBase, UINameConst.PatrolPage, 1);
                break;
            case EM_DailyTaskCondition.Patrol:
                f_RequestPatrol();
                break;
            case EM_DailyTaskCondition.EliteTollgate:
                UITool.f_GotoPage(thisBase, UINameConst.DungeonChapterPageNew, (int)EM_Fight_Enum.eFight_DungeonElite);
                break;
            case EM_DailyTaskCondition.DailyTollgate:
                UITool.f_GotoPage(thisBase, UINameConst.DungeonChapterPageNew, (int)EM_Fight_Enum.eFight_DailyPve);
                break;
            case EM_DailyTaskCondition.CardLevelUp:
                UITool.f_GotoPage(thisBase, UINameConst.CardBagPage, 0);
                break;
            case EM_DailyTaskCondition.CardEnvolveUp:
                UITool.f_GotoPage(thisBase, UINameConst.CardBagPage, 0);
                break;
            case EM_DailyTaskCondition.EquipRefine2:
                goto case EM_DailyTaskCondition.EquipRefine;
            case EM_DailyTaskCondition.TreasureRefine:
                UITool.f_GotoPage(thisBase, UINameConst.LineUpPage, (int)EM_FormationPos.eFormationPos_Main);
                break;
            case EM_DailyTaskCondition.CardSky:
                UITool.f_GotoPage(thisBase, UINameConst.CardBagPage, 0);
                break;
            case EM_DailyTaskCondition.CardAwake:
                UITool.f_GotoPage(thisBase, UINameConst.CardBagPage, 0);
                break;
            case EM_DailyTaskCondition.CardUpStar:
                UITool.f_GotoPage(thisBase, UINameConst.LineUpPage, (int)EM_FormationPos.eFormationPos_Main);
                break;
            case EM_DailyTaskCondition.BattleForm:
                UITool.f_GotoPage(thisBase, UINameConst.BattleFormPage, 0);
                break;
            case EM_DailyTaskCondition.Reinforce:
                UITool.f_GotoPage(thisBase, UINameConst.LineUpPage, (int)EM_FormationPos.eFormationPos_Reinforce);
                break;
            case EM_DailyTaskCondition.MainCardLvUp:
                UITool.f_GotoPage(thisBase, UINameConst.LineUpPage, (int)EM_FormationPos.eFormationPos_Main);
                break;
            default:
                break;
        }
    }

    public static void f_AchievementGoto(ccUIBase thisBase, EM_AchievementTaskCondition conditon)
    {
        taskUIBase = thisBase;
        switch (conditon)
        {
            case EM_AchievementTaskCondition.eAchv_Level:
                UITool.f_GotoPage(thisBase, UINameConst.DungeonChapterPageNew, (int)EM_Fight_Enum.eFight_DungeonMain);
                break;
            case EM_AchievementTaskCondition.eAchv_MainTollgateStars:
                UITool.f_GotoPage(thisBase, UINameConst.DungeonChapterPageNew, (int)EM_Fight_Enum.eFight_DungeonMain);
                break;
            case EM_AchievementTaskCondition.eAchv_Power:
                UITool.f_GotoPage(thisBase, UINameConst.LineUpPage, 0);
                break;
            case EM_AchievementTaskCondition.eAchv_Formation6CardLv:
                UITool.f_GotoPage(thisBase, UINameConst.LineUpPage, 0);
                break;
            case EM_AchievementTaskCondition.eAchv_Formation6CardEvolve:
                UITool.f_GotoPage(thisBase, UINameConst.LineUpPage, 0);
                break;
            case EM_AchievementTaskCondition.eAchv_RunningManStars:
                UITool.f_GotoPage(thisBase, UINameConst.RunningManPage, 0);
                break;
            case EM_AchievementTaskCondition.eAchv_VipLv:
                //充值界面
                UITool.f_GotoPage(thisBase, UINameConst.ShowVip, (int)ShowVip.EM_PageIndex.Recharge);
                break;
            case EM_AchievementTaskCondition.eAchv_HelpAgainstRiot:
                //
                UITool.f_GotoPage(thisBase, UINameConst.PatrolPage, 1);
                break;
            case EM_AchievementTaskCondition.eAchv_Patrol:
                UITool.f_GotoPage(thisBase, UINameConst.PatrolPage, 0);
                break;
            case EM_AchievementTaskCondition.eAchv_CrusadeRebel:
                UITool.f_GotoPage(thisBase, UINameConst.RebelArmy, 0);
                break;
            case EM_AchievementTaskCondition.eAchv_EliteTollgateStars:
                UITool.f_GotoPage(thisBase, UINameConst.DungeonChapterPageNew, (int)EM_Fight_Enum.eFight_DungeonElite);
                break;
            case EM_AchievementTaskCondition.eAchv_Formation6CardAwaken:
                UITool.f_GotoPage(thisBase, UINameConst.LineUpPage, 0);
                break;
            case EM_AchievementTaskCondition.eAchv_EliteChapters:
                UITool.f_GotoPage(thisBase, UINameConst.DungeonChapterPageNew, (int)EM_Fight_Enum.eFight_DungeonElite);
                break;
            case EM_AchievementTaskCondition.eAchv_4EquipFormationCards:
                UITool.f_GotoPage(thisBase, UINameConst.LineUpPage, 0);
                break;
            case EM_AchievementTaskCondition.eAchv_2TreasureFormationCards:
                UITool.f_GotoPage(thisBase, UINameConst.LineUpPage, 0);
                break;
            default:
                break;
        }
    }
    private static void f_RequestArenaList()
    {
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_ArenaList;
        socketCallbackDt.m_ccCallbackFail = f_Callback_ArenaList;
        Data_Pool.m_ArenaPool.f_ArenaList(socketCallbackDt);
        UITool.f_OpenOrCloseWaitTip(true);
    }

    private static void f_Callback_ArenaList(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.f_GotoPage(taskUIBase, UINameConst.ArenaPageNew, 0);
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1932));
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1933) + result);
        }
    }

    private static void f_RequestPatrol()
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.PatrolLevel))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1939), UITool.f_GetSysOpenLevel(EM_NeedLevel.PatrolLevel)));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolTaskInit;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolTaskInit;
        Data_Pool.m_PatrolPool.f_PatrolInit(0, socketCallbackDt);
    }

    private static void f_Callback_PatrolTaskInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.f_GotoPage(taskUIBase, UINameConst.PatrolPage, 0);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1944) + result);
        }
    }
    #endregion
    /// <summary>
    /// 切换页面/前往接口
    /// </summary>
    /// <param name="uiThis">当前ui</param>
    /// <param name="pageName">需要前往页面名字</param>
    /// <param name="param">页面附带的参数</param>
    /// <returns></returns>
    public static bool f_GotoPage(ccUIBase uiThis, string pageName, int param, Box tBox = null, ccUIBase uiTwo = null,int resourceType = 0,int resourceId = 0)
    {
        switch (pageName)
        {
            case UINameConst.ShopPage://商城
                ShopPageParam shopPageParam = new ShopPageParam();
                shopPageParam.stSourceUIName = null == uiThis ? "" : uiThis.name;
                if (resourceType > 0 && resourceId > 0)
                {
                    //如果跳转商城，且有明确知道物品，则弹出商城界面的同时再弹出二级购买界面
                    shopPageParam.emPageIndex = ShopPage.EM_PageIndex.PropsContent;
                    List<BasePoolDT<long>>  propList = Data_Pool.m_ShopResourcePool.f_GetAll();
                    shopPageParam.shopResourcePoolDT = propList.Find((BasePoolDT<long> item) => {
                        ShopResourcePoolDT shopItem = item as ShopResourcePoolDT;
                        if (null == shopItem) return false;
                        ShopResourceDT shopResourceDT = shopItem.m_ShopResourceDT;
                        return null != shopResourceDT && shopResourceDT.iType == resourceType && shopResourceDT.iTempId == resourceId;
                    }) as ShopResourcePoolDT;
                }
                if(null == shopPageParam.shopResourcePoolDT)
                {
                    shopPageParam.emPageIndex = ShopPage.EM_PageIndex.RecruitContent;
                    if (param > 0)
                        shopPageParam.emPageIndex = (ShopPage.EM_PageIndex)param;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopPage, UIMessageDef.UI_OPEN, shopPageParam);
                return true;
            case UINameConst.DungeonChapterPageNew://副本
                switch ((EM_Fight_Enum)param)
                {
                    case EM_Fight_Enum.eFight_DungeonElite:
                        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.DungeonEliteLevel))
                        {
                            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1934), UITool.f_GetSysOpenLevel(EM_NeedLevel.DungeonEliteLevel)));
                            return false;
                        }
                        break;
                    case EM_Fight_Enum.eFight_Legend:
                        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.LegendLevel))
                        {
                            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1935), UITool.f_GetSysOpenLevel(EM_NeedLevel.LegendLevel)));
                            return false;
                        }
                        break;
                    case EM_Fight_Enum.eFight_DailyPve:
                        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.DailyPveLevel))
                        {
                            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1049), UITool.f_GetSysOpenLevel(EM_NeedLevel.DailyPveLevel)));
                            return false;
                        }
                        break;
                    case EM_Fight_Enum.eFight_CrusadeRebel:
                    case EM_Fight_Enum.eFight_Rebel:
                        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.RebelArmyLevel))
                        {
                            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1936), UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel)));
                            return false;
                        }
                        break;
                    case EM_Fight_Enum.eFight_Friend:
                        break;
                    case EM_Fight_Enum.eFight_Guild:
                        break;
                    case EM_Fight_Enum.eFight_Arena:
                        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.ArenaLevel))
                            return false;
                        break;
                    case EM_Fight_Enum.eFight_Boss:
                        break;
                    case EM_Fight_Enum.eFight_LegionDungeon:
                        break;
                    case EM_Fight_Enum.eFight_GrabTreasureSweep:
                    case EM_Fight_Enum.eFight_GrabTreasure:
                        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.GrabTreasureLevel))//--------------------------待修改（满足两个条件中其一即可）
                        {
                            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1937), UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel)));
                            return false;
                        }
                        break;
                    case EM_Fight_Enum.eFight_RunningManElite:
                    case EM_Fight_Enum.eFight_RunningMan:
                        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.RunningManLvel))
                        {
                            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1938), UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel)));
                            return false;
                        }
                        break;

                }
                if (null != uiThis)
                    ccUIHoldPool.GetInstance().f_Hold(uiThis);
                EM_Fight_Enum fightType = EM_Fight_Enum.eFight_DungeonMain;
                if (param > 0)
                    fightType = (EM_Fight_Enum)param;
                ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonChapterPageNew, UIMessageDef.UI_OPEN, fightType);
                return true;
            case UINameConst.RunningManPage://过关斩将
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.RunningManLvel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1938), UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel)));
                    return false;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_OPEN);

                return true;
            case UINameConst.GrabTreasurePage://夺宝
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.GrabTreasureLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1937), UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel)));
                    return false;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasurePage, UIMessageDef.UI_OPEN, param);
                return true;
            case UINameConst.ShopCommonPage://随机商店

                EM_ShopType emShopType = EM_ShopType.Card;
                if (param > 0)
                    emShopType = (EM_ShopType)param;
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopCommonPage, UIMessageDef.UI_OPEN, emShopType);
                return true;
            case UINameConst.FriendPage:  //好友界面
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.FriendPage, UIMessageDef.UI_OPEN);
                return true;
            case UINameConst.EquipBagPage: //装备背包界面
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.EquipBagPage, UIMessageDef.UI_OPEN);
                return true;
            case UINameConst.TreasureBagPage: //法宝装备界面
                if (!f_GetIsOpensystem(EM_NeedLevel.OpenTreasureLevel))
                    return false;
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureBagPage, UIMessageDef.UI_OPEN, param);
                return true;
            case UINameConst.RebelArmy: //叛军入侵
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.RebelArmyLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1936), UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel)));
                    return false;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelArmy, UIMessageDef.UI_OPEN);
                return true;
            case UINameConst.ArenaPageNew: //竞技场界面
                if (!f_GetIsOpensystem(EM_NeedLevel.ArenaLevel))
                    return false;
                System.DateTime start = System.DateTime.Now;
                //请求竞技场服务器数据
                bool bIsServerCallback = false;
                ccCallback getArenaServerData = (object obj) =>
                {
                    SocketCallbackDT ArenasocketCallbackDt = new SocketCallbackDT();
                    ArenasocketCallbackDt.m_ccCallbackSuc = (object result) =>
                    {
                        if (bIsServerCallback) return;
                        bIsServerCallback = true;
                        MessageBox.ASSERT("arena:f_CallbackSuc_OpenArena result:" + result);
                        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
                        {
MessageBox.ASSERT("Get time data from arena:" + (System.DateTime.Now - start).TotalSeconds);
                            ccUIHoldPool.GetInstance().f_Hold(uiThis);
                            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaPageNew, UIMessageDef.UI_OPEN);
MessageBox.ASSERT("Get open time data from arena:" + (System.DateTime.Now - start).TotalSeconds);
                        }
                        else
                        {
                            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1945) + result);
                        }
                        UITool.f_OpenOrCloseWaitTip(false);
                    };
                    ArenasocketCallbackDt.m_ccCallbackFail = (object result) =>
                    {
                        if (bIsServerCallback) return;
                        bIsServerCallback = true;
                        UITool.f_OpenOrCloseWaitTip(false);
                        MessageBox.ASSERT("arena:f_CallbackFail_OpenArena result:" + result);
                        UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1945) + result);
                    };
                    UITool.f_OpenOrCloseWaitTip(true);
                    Data_Pool.m_ArenaPool.f_ArenaList(ArenasocketCallbackDt);
                };

                //如果服务器没有返回数据，则定时请求（有可能请求的时候掉线了，然后重连上服务器不会再回调数据，避免新手引导卡死）
                int nTimeId = 0;
                nTimeId = ccTimeEvent.GetInstance().f_RegEvent(1, true, null, (object obj) =>
                {
                    if (bIsServerCallback)
                    {
                        ccTimeEvent.GetInstance().f_UnRegEvent(nTimeId);
                        return;
                    }
                    getArenaServerData(null);
                });

                getArenaServerData(null);
                return true;
            case UINameConst.LineUpPage: //阵容界面    传EM_FormationPos.eFormationPos_Reinforce进入援军
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LineUpPage, UIMessageDef.UI_OPEN, (EM_FormationPos)param);
                return true;
            case UINameConst.ShowVip: //充值界面
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, (ShowVip.EM_PageIndex)param);
                return true;
            case UINameConst.CardProperty:
                CardBox tCardBox = (CardBox)tBox;
                switch (tCardBox.m_bType)
                {
                    case CardBox.BoxType.Awaken:
                        if (!f_GetIsOpensystem(EM_NeedLevel.CardAwaken))
                            return false;
                        break;
                    case CardBox.BoxType.Sky:
                        if (!f_GetIsOpensystem(EM_NeedLevel.CardSky))
                            return false;
                        break;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CardProperty, UIMessageDef.UI_OPEN, tBox);
                return true;
            case UINameConst.EquipManage:
                EquipBox tEquipBox = (EquipBox)tBox;
                switch (tEquipBox.tType)
                {
                    case EquipBox.BoxTye.UpStar:
                        if (!f_GetIsOpensystem(EM_NeedLevel.EquipUpStar))
                            return false;
                        break;
                    case EquipBox.BoxTye.Refine:
                        if (!f_GetIsOpensystem(EM_NeedLevel.EquipRefine))
                            return false;
                        break;
                }

                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.EquipManage, UIMessageDef.UI_OPEN, tBox);
                return true;
            case UINameConst.TreasureManage:
                if (!f_GetIsOpensystem(EM_NeedLevel.OpenTreasureLevel))
                    return false;
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureManage, UIMessageDef.UI_OPEN, tBox);
                return true;
            case UINameConst.CardBagPage:
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBagPage, UIMessageDef.UI_OPEN);
                return true;
            case UINameConst.PatrolPage:
                if (!f_GetIsOpensystem(EM_NeedLevel.PatrolLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1939), UITool.f_GetSysOpenLevel(EM_NeedLevel.PatrolLevel)));
                    return false;
                }
                UITool.f_OpenOrCloseWaitTip(true);
                m_getWayUI = uiThis;
                m_getWayUIParam = param;
                SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
                socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolInit;
                socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolInit;
                Data_Pool.m_PatrolPool.f_PatrolInit(0, socketCallbackDt);
                return true;
            case UINameConst.ActivityPage:
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                if (param >= (int)EM_ActivityType.DaySignIn && param < (int)EM_ActivityType.End)
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.ActivityPage, UIMessageDef.UI_OPEN, (EM_ActivityType)param);
                else
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.ActivityPage, UIMessageDef.UI_OPEN);
                return true;
            case UINameConst.BattleFormPage: //阵图
                if (!f_GetIsOpensystem(EM_NeedLevel.BattleFormLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1940), UITool.f_GetSysOpenLevel(EM_NeedLevel.BattleFormLevel)));
                    return false;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFormPage, UIMessageDef.UI_OPEN);
                return true;
            case UINameConst.ShopMutiCommonPage://商店
                if (param == 6)
                {
                    if (!UITool.f_GetIsOpensystem(EM_NeedLevel.RebelArmyLevel))
                    {
                        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1941), UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel)));
                        return false;
                    }
                }
                if (param == 3)
                {
                    if (!UITool.f_GetIsOpensystem(EM_NeedLevel.ArenaLevel))
                    {
                        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1942), UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel)));
                        return false;
                    }
                }
                if (param == 5)
                {
                    if (!UITool.f_GetIsOpensystem(EM_NeedLevel.RunningManLvel))
                    {
                        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1943), UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel)));
                        return false;
                    }
                }

                EM_ShopMutiType emShopMutiType = EM_ShopMutiType.BattleFeatShop;
                if (param > 0)
                    emShopMutiType = (EM_ShopMutiType)param;
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, emShopMutiType);
                return true;
            case UINameConst.LegionCreatePage:
            case UINameConst.LegionListPage:
            case UINameConst.LegionMenuPage: //军团
                if (!f_GetIsOpensystem(EM_NeedLevel.LegionLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(16), UITool.f_GetSysOpenLevel(EM_NeedLevel.LegionLevel)));
                    return false;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
                {
                    //没有军团
                    f_OpenOrCloseWaitTip(true);
                    LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(false, true, delegate (object o)
                    {
                        UITool.f_OpenOrCloseWaitTip(false);
                        if ((int)o == (int)eMsgOperateResult.OR_Succeed)
                        {
                            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionListPage, UIMessageDef.UI_OPEN);
                        }
                        else
                        {
                            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(17) + o);
                        }
                    });
                }
                else
                {
                    LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(false, true, delegate (object o)
                    {
                        UITool.f_OpenOrCloseWaitTip(false);
                        if ((int)o == (int)eMsgOperateResult.OR_Succeed)
                        {
                            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionMenuPage, UIMessageDef.UI_OPEN);
                        }
                        else
                        {
                            UI_ShowFailContent(CommonTools.f_GetTransLanguage(18) + o);
                        }
                    });

                }
                return true;
            case UINameConst.Recycle:
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.Recycle, UIMessageDef.UI_OPEN);
                return true;
            case UINameConst.CrossServerBattlePage:
                if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1011), UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle)));
                    return false;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossServerBattlePage, UIMessageDef.UI_OPEN);
                return true;
            //TsuCode - ChaosBattle
            case UINameConst.ChaosBattlePage:
                if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.ChaosBattle))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1011), UITool.f_GetSysOpenLevel(EM_NeedLevel.ChaosBattle)));
                    return false;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChaosBattlePage, UIMessageDef.UI_OPEN);
                return true;
            //---------------------------
            case UINameConst.LegionChapterPage:
                if (!f_GetIsOpensystem(EM_NeedLevel.LegionLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(16), UITool.f_GetSysOpenLevel(EM_NeedLevel.LegionLevel)));
                    return false;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
                {
                    //没有军团
                    f_OpenOrCloseWaitTip(true);
                    LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(false, true, delegate (object o)
                    {
                        UITool.f_OpenOrCloseWaitTip(false);
                        if ((int)o == (int)eMsgOperateResult.OR_Succeed)
                        {
                            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionListPage, UIMessageDef.UI_OPEN);
                        }
                        else
                        {
                            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(17) + o);
                        }
                    });
                }
                else
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    LegionMain.GetInstance().m_LegionPlayerPool.f_ExecuteAfterLegionMemInit((object result)=> 
                    {                       
                        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
                        {
                            LegionMain.GetInstance().m_LegionDungeonPool.f_ExecuteAfterInitFiniChapterAndInitCurChapter((object objResult)=> 
                            {
                                UITool.f_OpenOrCloseWaitTip(false);
                                if ((int)result == (int)eMsgOperateResult.OR_Succeed)
                                {                                   
                                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterPage, UIMessageDef.UI_OPEN);
                                }

                            });
                        }
                        else
                        {
                            UITool.f_OpenOrCloseWaitTip(false);
                            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(477) + result);
                        }
                    });
                }
                return true;
            case UINameConst.LegionBattlePage:
                if (!f_GetIsOpensystem(EM_NeedLevel.LegionLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(16), UITool.f_GetSysOpenLevel(EM_NeedLevel.LegionLevel)));
                    return false;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
                {
                    //没有军团
                    f_OpenOrCloseWaitTip(true);
                    LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(false, true, delegate (object o)
                    {
                        UITool.f_OpenOrCloseWaitTip(false);
                        if ((int)o == (int)eMsgOperateResult.OR_Succeed)
                        {
                            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionListPage, UIMessageDef.UI_OPEN);
                        }
                        else
                        {
                            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(17) + o);
                        }
                    });
                }
                else
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    LegionMain.GetInstance().m_LegionPlayerPool.f_ExecuteAfterLegionMemInit((object result) =>
                    {
                        UITool.f_OpenOrCloseWaitTip(false);
                        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
                        {
                            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattlePage, UIMessageDef.UI_OPEN);
                        }
                        else
                        {
                            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(477) + result);
                        }
                    });
                }
                return true;
            case UINameConst.TurntablePage:
                //轮盘是弹窗，，所以它跟其他处理不太一样，不能关上个面板，要不就没有背景
                //所以如果上个界面是获取途径也是弹窗则直接把弹窗关掉而不是hold住
                GameParamDT _turntableParam = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.TurntableLottery);
                GameParamDT _turntableLvParam = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.TurntableLimitLv);
                if (!(Data_Pool.m_TurntablePool.f_GetIsOpen() && (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= _turntableLvParam.iParam1)))
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(2251));
                    return false;
                }
                if (null != uiThis && uiThis.name == UINameConst.GetWayPage)
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_CLOSE);
                UITool.f_OpenOrCloseWaitTip(true);
                SocketCallbackDT QueryCallback = new SocketCallbackDT();
                QueryCallback.m_ccCallbackFail = (object obj)=> 
                {
                    UITool.f_OpenOrCloseWaitTip(false);
                    eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(15) + CommonTools.f_GetTransLanguage((int)obj));
                };
                QueryCallback.m_ccCallbackSuc = (object obj)=> 
                {
                    UITool.f_OpenOrCloseWaitTip(false);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.TurntablePage, UIMessageDef.UI_OPEN);
                };
                Data_Pool.m_TurntablePool.f_QueryTurntableInfo(QueryCallback);
                return true;
            case UINameConst.LimitGodEquipActPage:
                if (!Data_Pool.m_GodDressPool.f_CheckLimitGodOpen())
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(2251));
                    return false;
                }
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                SocketCallbackDT queryCallback = new SocketCallbackDT();
                queryCallback.m_ccCallbackFail = (object obj)=> 
                {
                    eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(15) + CommonTools.f_GetTransLanguage((int)obj));
                };
                queryCallback.m_ccCallbackSuc = (object obj)=> 
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LimitGodEquipActPage, UIMessageDef.UI_OPEN);
                };
                Data_Pool.m_GodDressPool.f_QueryGodDressInfo(queryCallback);
                return true;
            case UINameConst.ShopPropsPage://tiệm đạo cụ
                ccUIHoldPool.GetInstance().f_Hold(uiThis);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopPropsPage, UIMessageDef.UI_OPEN);
                return true;
        }

        return false;
    }
    private static ccUIBase m_getWayUI;
    private static int m_getWayUIParam;
    private static void f_Callback_PatrolInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIHoldPool.GetInstance().f_Hold(m_getWayUI);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_PatrolPool.m_SelfPatrolDt, m_getWayUIParam });
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1944) + result);
        }
    }
    #endregion


    /// <summary>
    /// 打开或者关闭等待提示界面
    /// </summary>
    /// <param name="isOpen">true:打开</param>
    /// <param name="isForce">强制显示LoadTip</param>
    public static void f_OpenOrCloseWaitTip(bool isOpen, bool isForce = false)
    {
        if (isOpen)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.WaitTipPage, UIMessageDef.UI_OPEN, isForce);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.WaitTipPage, UIMessageDef.UI_CLOSE, isForce);
        }
    }

    /// <summary>
    /// 打开或者关闭充值Mask
    /// </summary>
    /// <param name="isOpen">ture：打开 false：关闭</param>
    /// <param name="tipLabel">展示label</param>
    /// <param name="showTime">展示时间，0：代表需要手动关闭</param>
    public static void f_OpenOrClosePccaccyMask(bool isOpen, string tipLabel, float showTime)
    {
        if (isOpen)
        {
            PccaccyMaskParams tParam = new PccaccyMaskParams(tipLabel, showTime);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PayMaskPage, UIMessageDef.UI_OPEN, tParam);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PayMaskPage, UIMessageDef.UI_CLOSE);
        }
    }

    /// <summary>
    /// 控制红点显示更新
    /// </summary>
    /// <param name="Obj">红点显示父级对象</param>
    /// <param name="iNum">条件数量</param>
    /// <param name="v3Pos">显示偏移坐标</param>
    /// <param name="iDepth">显示深度</param>
    public static void f_UpdateReddot(GameObject Obj, int iNum, Vector3 v3Pos, int iDepth,float from = 1, float to = 1.18f)
    {       
        if (Obj == null)
            return;
        Transform ReddotObj = Obj.transform.Find("Reddot");
        if (iNum > 0)
        {
            if (ReddotObj == null)
            {
                ReddotObj = glo_Main.GetInstance().m_ResourceManager.f_CreateReddot().transform;
                ReddotObj.parent = Obj.transform;
                ReddotObj.transform.localPosition = v3Pos;
                ReddotObj.name = "Reddot";
                UISprite tUISprite = ReddotObj.GetComponent<UISprite>();
                tUISprite.depth = iDepth;
            }
            ReddotObj.gameObject.SetActive(true);
        }
        else
        {
            if (ReddotObj != null)
            {
                ReddotObj.gameObject.SetActive(false);
            }
        }
        if(ReddotObj != null)
        {
            ReddotObj.GetComponent<TweenScale>().from = Vector3.one * from;
            ReddotObj.GetComponent<TweenScale>().to = Vector3.one * to;
        }
    }


    /// <summary>
    /// 检查是货币是否足够
    /// </summary>
    /// <param name="costType">需要花费的类型</param>
    /// <param name="costNum">需要花费的数量,0表示无条件打开使用界面和购买界面</param>
    /// <param name="needTip">是否需要弹窗提示</param>
    /// <returns></returns>
    public static bool f_IsEnoughMoney(EM_MoneyType costType, int costNum, bool needTip = true, bool needPage = true,ccUIBase uiBase = null)
    {
        int haveNum = 0;
        int vipLv = UITool.f_GetNowVipLv();
        if (costType == EM_MoneyType.eUserAttr_Sycee)
        {
            haveNum = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
            if (needTip && haveNum < costNum)
            {
                string tipContent = string.Format(CommonTools.f_GetTransLanguage(1946));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
            }
        }
        else if (costType == EM_MoneyType.eUserAttr_Vigor)
        {
            haveNum = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor);
            if (needTip && (haveNum < costNum || costNum == 0))
            {
                int vigorNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GameParamConst.VigorGoodId);
                ShopResourcePoolDT tShopPoolDt = (ShopResourcePoolDT)Data_Pool.m_ShopResourcePool.f_GetShopPoolDtByTypeAndId((int)EM_ResourceType.Good, GameParamConst.VigorGoodId);
                int[] timesArr = ccMath.f_String2ArrayInt(tShopPoolDt.m_ShopResourceDT.szVipLimitTimes, ";");
                int timesLimit = vipLv < timesArr.Length ? timesArr[vipLv] : timesArr[timesArr.Length - 1];
                int nextTimesLimit = vipLv + 1 < timesArr.Length ? timesArr[vipLv + 1] : timesArr[timesArr.Length - 1];
                BaseGoodsDT vigorTemplate = (BaseGoodsDT)glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(GameParamConst.VigorGoodId);
                if (needPage && costNum == 0 && haveNum + vigorTemplate.iEffectData > GameParamConst.VigorMax)
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1947));
                else if (vigorNum > 0)
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.UseGoodOnekeyPage, UIMessageDef.UI_OPEN, GameParamConst.VigorGoodId);
                else if (tShopPoolDt.m_iBuyTimes < timesLimit || timesLimit == 0)
                {
                    //ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyGoodOnekeyPage, UIMessageDef.UI_OPEN, tShopPoolDt);
                    GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, GameParamConst.VigorGoodId, uiBase);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                }
                else if (timesLimit != nextTimesLimit)
                {
                    string tipContent = string.Format(CommonTools.f_GetTransLanguage(1948), vipLv + 1, nextTimesLimit);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
                }
                else
                {
                    if (costNum > 0)
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1949));
                    else
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1950));
                }
            }
        }
        else if (costType == EM_MoneyType.eUserAttr_Energy)
        {
            haveNum = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy);
            if (needTip && (haveNum < costNum || costNum == 0))
            {
                int energyNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GameParamConst.EnergyGoodId);
                ShopResourcePoolDT tShopPoolDt = (ShopResourcePoolDT)Data_Pool.m_ShopResourcePool.f_GetShopPoolDtByTypeAndId((int)EM_ResourceType.Good, GameParamConst.EnergyGoodId);
                int[] timesArr = ccMath.f_String2ArrayInt(tShopPoolDt.m_ShopResourceDT.szVipLimitTimes, ";");
                int timesLimit = vipLv < timesArr.Length ? timesArr[vipLv] : timesArr[timesArr.Length - 1];
                int nextTimesLimit = vipLv + 1 < timesArr.Length ? timesArr[vipLv + 1] : timesArr[timesArr.Length - 1];
                BaseGoodsDT energyTemplate = (BaseGoodsDT)glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(GameParamConst.EnergyGoodId);
                int mEnergyLimit = f_GetNowVipPrivilege((int)EM_VipPrivilege.eVip_EnergyLimit); //GameParamConst.EnergyMax
                if (costNum == 0 && haveNum + energyTemplate.iEffectData > mEnergyLimit)
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1951));
                else if (energyNum > 0)
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.UseGoodOnekeyPage, UIMessageDef.UI_OPEN, GameParamConst.EnergyGoodId);
                else if (tShopPoolDt.m_iBuyTimes < timesLimit || timesLimit == 0)
                {
                    GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, GameParamConst.EnergyGoodId, uiBase);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                }
                else if (timesLimit != nextTimesLimit)
                {
                    string tipContent = string.Format(CommonTools.f_GetTransLanguage(1952), vipLv + 1, nextTimesLimit);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
                }
                else
                {
                    if (costNum > 0)
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1953));
                    else
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1954));
                }
            }
        }
        else if (costType == EM_MoneyType.eUserAttr_CrusadeToken)
        {
            haveNum = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_CrusadeToken);
            if (needTip && (haveNum < costNum || costNum == 0))
            {
                int tokenNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GameParamConst.CrusadeToken);
                ShopResourcePoolDT tShopPoolDt = (ShopResourcePoolDT)Data_Pool.m_ShopResourcePool.f_GetShopPoolDtByTypeAndId((int)EM_ResourceType.Good, GameParamConst.CrusadeToken);
                int[] timesArr = ccMath.f_String2ArrayInt(tShopPoolDt.m_ShopResourceDT.szVipLimitTimes, ";");
                int timesLimit = vipLv < timesArr.Length ? timesArr[vipLv] : timesArr[timesArr.Length - 1];
                int nextTimesLimit = vipLv + 1 < timesArr.Length ? timesArr[vipLv + 1] : timesArr[timesArr.Length - 1];
                if (tokenNum > 0)
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.UseGoodOnekeyPage, UIMessageDef.UI_OPEN, GameParamConst.CrusadeToken);
                else if (tShopPoolDt.m_iBuyTimes < timesLimit || timesLimit == 0)
                {
                    //ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyGoodOnekeyPage, UIMessageDef.UI_OPEN, tShopPoolDt);
                    GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, GameParamConst.CrusadeToken, uiBase);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                }
                else if (timesLimit != nextTimesLimit)
                {
                    string tipContent = string.Format(CommonTools.f_GetTransLanguage(1955), vipLv + 1, nextTimesLimit);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
                }
                else
                {
                    if (costNum > 0)
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1956));
                    else
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1957));
                }
            }
        }
        else if (costType == EM_MoneyType.eFreshToken)
        {
            ShopResourcePoolDT tShopPoolDt = (ShopResourcePoolDT)Data_Pool.m_ShopResourcePool.f_GetShopPoolDtByTypeAndId((int)EM_ResourceType.Good, (int)EM_MoneyType.eFreshToken);
			//MessageBox.ASSERT("Vip times: " + (int)EM_ResourceType.Good + " " + (int)EM_MoneyType.eFreshToken);
            int[] timesArr = ccMath.f_String2ArrayInt(tShopPoolDt.m_ShopResourceDT.szVipLimitTimes, ";");
            int timesLimit = vipLv < timesArr.Length ? timesArr[vipLv] : timesArr[timesArr.Length - 1];
            int nextTimesLimit = vipLv + 1 < timesArr.Length ? timesArr[vipLv + 1] : timesArr[timesArr.Length - 1];

            if (tShopPoolDt.m_iBuyTimes < timesLimit || timesLimit == 0)
            {
                //ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyGoodOnekeyPage, UIMessageDef.UI_OPEN, tShopPoolDt);
                GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, (int)EM_MoneyType.eFreshToken, uiBase);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            }
            else if (timesLimit != nextTimesLimit)
            {
                string tipContent = string.Format(CommonTools.f_GetTransLanguage(1958), vipLv + 1, nextTimesLimit);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
            }
            else
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1959));
            }
        }
        else if (costType == EM_MoneyType.eUserAttr_Money)
        {
            haveNum = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money);
            if (needTip && (haveNum < costNum || costNum == 0))
            {
                ShopResourcePoolDT tShopPoolDt = (ShopResourcePoolDT)Data_Pool.m_ShopResourcePool.f_GetShopPoolDtByTypeAndId((int)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money);
                int[] timesArr = ccMath.f_String2ArrayInt(tShopPoolDt.m_ShopResourceDT.szVipLimitTimes, ";");
                int timesLimit = vipLv < timesArr.Length ? timesArr[vipLv] : timesArr[timesArr.Length - 1];
                int nextTimesLimit = vipLv + 1 < timesArr.Length ? timesArr[vipLv + 1] : timesArr[timesArr.Length - 1];
                if (tShopPoolDt.m_iBuyTimes < timesLimit || timesLimit == 0)
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyGoodOnekeyPage, UIMessageDef.UI_OPEN, tShopPoolDt);
                else if (timesLimit != nextTimesLimit)
                {
                    string tipContent = string.Format(CommonTools.f_GetTransLanguage(1960), vipLv + 1, nextTimesLimit);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
                }
                else
                {
                    if (costNum > 0)
                    {
                        //银币不足
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1961));
                        GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, uiBase);
                        ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                    }
                    else
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1962));
                    
                }
            }
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1963));
        }
        return haveNum >= costNum;
    }
    public static void f_GetCurLevelOpenSystemDes(out string hasOpen, out string nextOpen, out int nextOpenLevel, out string OpenSprite, out string NextSprite)
    {
        int Lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        EM_NeedLevel hasOpenLevelMax = EM_NeedLevel.CardCultivate;
        int nearLevel = 0;
        EM_NeedLevel nextOpenLevelMin = EM_NeedLevel.CardCultivate;
        int nextLevelMin = 0;
        hasOpen = null;
        nextOpen = null;
        OpenSprite = null;
        NextSprite = null;
        for (int i = (int)EM_NeedLevel.CardCultivate; i < (int)EM_NeedLevel.End; i++)
        {
            //EM_NeedLevel em_NeedLevel = (EM_NeedLevel)i;
            int openLevel = f_GetSysOpenLevel((EM_NeedLevel)i);
            if (openLevel <= Lv)
            {
                if (openLevel >= nearLevel)
                {
                    hasOpenLevelMax = (EM_NeedLevel)i;
                    string temp = f_GetOpenSystemDes(hasOpenLevelMax, ref OpenSprite);
                    if (temp != "")
                    {
                        nearLevel = openLevel;
                        hasOpen = temp;
                    }
                }
            }
            else
            {
                if (nextLevelMin == 0 || openLevel <= nextLevelMin)
                {
                    nextOpenLevelMin = (EM_NeedLevel)i;
                    string temp = f_GetOpenSystemDes(nextOpenLevelMin, ref NextSprite);
                    if (temp != "")
                    {
                        nextLevelMin = openLevel;
                        nextOpen = temp;
                    }
                }
            }
        }

        nextOpenLevel = nextLevelMin;
    }
    /// <summary>
    /// 等级开放描述
    /// </summary>
    /// <param name="tmpNeedLevel">需要的等级</param>
    /// <returns></returns>
    public static string f_GetOpenSystemDes(EM_NeedLevel tmpNeedLevel, ref string SpriteName)
    {
        switch (tmpNeedLevel)
        {
            case EM_NeedLevel.ArenaLevel:
                SpriteName = "ArenaLevel";
                return CommonTools.f_GetTransLanguage(1964);
            case EM_NeedLevel.GrabTreasureLevel:
                SpriteName = "GrabTreasurePage";
                return CommonTools.f_GetTransLanguage(1965);
            case EM_NeedLevel.RunningManLvel:
                SpriteName = "RunningManPage-1";
                return CommonTools.f_GetTransLanguage(1966);
            case EM_NeedLevel.PatrolLevel:
                SpriteName = "PatrolPage";
                return CommonTools.f_GetTransLanguage(1967);
            case EM_NeedLevel.RebelArmyLevel:
                SpriteName = "RebelArmy";
                return CommonTools.f_GetTransLanguage(1968);
            case EM_NeedLevel.GrabTreasureFiveLevel:
                SpriteName = "GrabTreasurePage";
                return CommonTools.f_GetTransLanguage(1969);
            case EM_NeedLevel.DungeonEliteLevel:
                SpriteName = "DungeonChapterPageNew1";
                return CommonTools.f_GetTransLanguage(1970);
            case EM_NeedLevel.LegendLevel:
                SpriteName = "DungeonChapterPageNew3";
                return CommonTools.f_GetTransLanguage(1971);
            case EM_NeedLevel.BattleNumLevel:
                SpriteName = "Fast2";
                return CommonTools.f_GetTransLanguage(1972);
            case EM_NeedLevel.SweepLevel:
                SpriteName = "Sweep";
                return CommonTools.f_GetTransLanguage(1973);
            case EM_NeedLevel.OpenTwoCardLevel:
                SpriteName = "LineUpPage2";
                return CommonTools.f_GetTransLanguage(1974);
            case EM_NeedLevel.OpenThereCardLevel:
                SpriteName = "LineUpPage3";
                return CommonTools.f_GetTransLanguage(1975);
            case EM_NeedLevel.OpenFourCardLevel:
                SpriteName = "LineUpPage4";
                return CommonTools.f_GetTransLanguage(1976);
            case EM_NeedLevel.OpenFiveCardLevel:
                SpriteName = "LineUpPage5";
                return CommonTools.f_GetTransLanguage(1977);
            case EM_NeedLevel.OpenSixCardLevel:
                SpriteName = "LineUpPage6";
                return CommonTools.f_GetTransLanguage(1978);
            case EM_NeedLevel.OpenReinforceLevel:
                SpriteName = "Icon_Reinforce";
                return CommonTools.f_GetTransLanguage(1979);
            //case EM_NeedLevel.OpenReinforceBuffLevel:
            //return "援军助威：开启援军助威";
            //case EM_NeedLevel.EquipIntenFiveLevel:
            //    SpriteName = "DungeonChapterPageNew3";
            //return CommonTools.f_GetTransLanguage(1980);
            case EM_NeedLevel.EquipRefine:
                SpriteName = "EquipRefine";
                return CommonTools.f_GetTransLanguage(1981);
            case EM_NeedLevel.EquipUpStar:
                SpriteName = "UpStar";
                return CommonTools.f_GetTransLanguage(1982);
            //case EM_NeedLevel.CardCultivate:
            //return "卡牌培养：开启卡牌培养功能";
            case EM_NeedLevel.CardSky:
                SpriteName = "fate";
                return CommonTools.f_GetTransLanguage(1983);
            case EM_NeedLevel.CardAwaken:
                SpriteName = "Awaken";
                return CommonTools.f_GetTransLanguage(1984);
            case EM_NeedLevel.OpenTreasureLevel:
                SpriteName = "Icon_TreasureBag";
                return CommonTools.f_GetTransLanguage(1985);
            case EM_NeedLevel.DailyPveLevel:
                SpriteName = "DungeonChapterPageNew1";
                return CommonTools.f_GetTransLanguage(1986);
            case EM_NeedLevel.DungeonJumpLevel:
                SpriteName = "DungeonChapterPageNew1";
                return CommonTools.f_GetTransLanguage(1987);
            case EM_NeedLevel.BattleFormLevel:
                SpriteName = "BattleFormPage";
                return CommonTools.f_GetTransLanguage(1988);
            // case EM_NeedLevel.AwakeShopLevel:
               // SpriteName = "DungeonChapterPageNew3";
               // return CommonTools.f_GetTransLanguage(1989);
            case EM_NeedLevel.LegionLevel:
               SpriteName = "Icon_Legion";
               return CommonTools.f_GetTransLanguage(1990);
            case EM_NeedLevel.BattleNum3Level:
                SpriteName = "Fast3";
                return CommonTools.f_GetTransLanguage(1991);
			case EM_NeedLevel.TariningLevel: //Mới
                SpriteName = "Icon_Tactical";
                return CommonTools.f_GetTransLanguage(1991);
			case EM_NeedLevel.CrossServerBattle:
                SpriteName = "Icon_CrossServer";
                return CommonTools.f_GetTransLanguage(1991);
			case EM_NeedLevel.TransmigrationCard:
                SpriteName = "Icon_ChangeCard";
                return CommonTools.f_GetTransLanguage(1991);
			case EM_NeedLevel.TritalTowerOpenLV:
                SpriteName = "Tex_TrialTowerIcon";
                return CommonTools.f_GetTransLanguage(1991);
			case EM_NeedLevel.SevenStarOpenLv:
                SpriteName = "Icon_qxmd";
                return CommonTools.f_GetTransLanguage(1991);
			case EM_NeedLevel.CardBattle:
                SpriteName = "Icon_CardBattle";
                return CommonTools.f_GetTransLanguage(1991);
        }
        return "";
    }
    public static int f_GetSysOpenLevel(EM_NeedLevel tmpNeedLevel)
    {
        if (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)tmpNeedLevel) == null)
        {
            return 0;
        }
        int Lv = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)tmpNeedLevel) as GameParamDT).iParam1;
        #region 原来的匹配
        //switch (tmpNeedLevel)
        //{
        //    case EM_NeedLevel.ArenaLevel:
        //        Lv = GameParamConst.ArenaLevel;
        //        break;
        //    case EM_NeedLevel.GrabTreasureLevel:
        //        Lv = GameParamConst.GrabTreasureLevel;
        //        break;
        //    case EM_NeedLevel.RunningManLvel:
        //        Lv = GameParamConst.RunningManLvel;
        //        break;
        //    case EM_NeedLevel.PatrolLevel:
        //        Lv = GameParamConst.PatrolLevel;
        //        break;
        //    case EM_NeedLevel.RebelArmyLevel:
        //        Lv = GameParamConst.RebelArmyLevel;
        //        break;
        //    case EM_NeedLevel.GrabTreasureFiveLevel:
        //        Lv = GameParamConst.GrabTreasureFiveLevel;
        //        break;
        //    case EM_NeedLevel.DungeonEliteLevel:
        //        Lv = GameParamConst.DungeonEliteLevel;
        //        break;
        //    case EM_NeedLevel.LegendLevel:
        //        Lv = GameParamConst.LegendLevel;
        //        break;
        //    case EM_NeedLevel.DailyPveLevel:
        //        Lv = GameParamConst.DailyPveLevel;
        //        break;
        //    case EM_NeedLevel.BattleNumLevel:
        //        Lv = GameParamConst.BattleNumLevel;
        //        break;
        //    case EM_NeedLevel.SweepLevel:
        //        Lv = GameParamConst.SweepLevel;
        //        break;
        //    case EM_NeedLevel.OpenTwoCardLevel:
        //        Lv = GameParamConst.OpenTwoCardLevel;
        //        break;
        //    case EM_NeedLevel.OpenThereCardLevel:
        //        Lv = GameParamConst.OpenThereCardLevel;
        //        break;
        //    case EM_NeedLevel.OpenFourCardLevel:
        //        Lv = GameParamConst.OpenFourCardLevel;
        //        break;
        //    case EM_NeedLevel.OpenFiveCardLevel:
        //        Lv = GameParamConst.OpenFiveCardLevel;
        //        break;
        //    case EM_NeedLevel.OpenSixCardLevel:
        //        Lv = GameParamConst.OpenSixCardLevel;
        //        break;
        //    case EM_NeedLevel.OpenReinforceLevel:
        //        Lv = GameParamConst.OpenReinforceLevel;
        //        break;
        //    //case EM_NeedLevel.OpenReinforceBuffLevel:
        //    //Lv = GameParamConst.OpenReinforceBuffLevel;
        //    //break;
        //    case EM_NeedLevel.EquipIntenFiveLevel:
        //        Lv = GameParamConst.EquipIntenFiveLevel;
        //        break;
        //    case EM_NeedLevel.EquipRefine:
        //        Lv = GameParamConst.EquipRefine;
        //        break;
        //    case EM_NeedLevel.EquipUpStar:
        //        Lv = GameParamConst.EquipUpStar;
        //        break;
        //    //case EM_NeedLevel.CardCultivate:
        //    //Lv = GameParamConst.CardCultivate;
        //    //break;
        //    case EM_NeedLevel.CardSky:
        //        Lv = GameParamConst.CardSky;
        //        break;
        //    case EM_NeedLevel.CardAwaken:
        //        Lv = GameParamConst.CardAwaken;
        //        break;
        //    case EM_NeedLevel.OpenTreasureLevel:
        //        Lv = GameParamConst.OpenTreasureLevel;
        //        break;
        //    case EM_NeedLevel.BattleFormLevel:
        //        Lv = GameParamConst.BattleFormLevel;
        //        break;
        //}
        #endregion

        return Lv;
    }
    public static bool f_GetIsOpensystem(EM_NeedLevel tmpNeedLevel)
    {
        int Lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        return Lv >= f_GetSysOpenLevel(tmpNeedLevel);
    }

    public static GameParamDT f_GetGameParamDT(int GameParamId)
    {
        return glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(GameParamId) as GameParamDT;
    }
    /// <summary>
    /// 获取阵容位置开放等级
    /// </summary>
    /// <param name="pos">位置</param>
    /// <returns></returns>
    public static int f_GetTeamPosOpenLevel(EM_FormationPos pos)
    {
        switch (pos)
        {
            case EM_FormationPos.eFormationPos_Assist1:
                return UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenTwoCardLevel);
            case EM_FormationPos.eFormationPos_Assist2:
                return UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenThereCardLevel);
            case EM_FormationPos.eFormationPos_Assist3:
                return UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenFourCardLevel);
            case EM_FormationPos.eFormationPos_Assist4:
                return UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenFiveCardLevel);
            case EM_FormationPos.eFormationPos_Assist5:
                return UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenSixCardLevel);
            case EM_FormationPos.eFormationPos_Assist6:
                return UITool.f_GetSysOpenLevel(EM_NeedLevel.Open7CardLevel);
            case EM_FormationPos.eFormationPos_Reinforce:
                return UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenReinforceLevel);
        }
        return 0;
    }
    /// <summary>
    /// 战斗失败处理 点击不同按钮跳转不同界面
    /// </summary>
    /// <param name="processType"></param>
    public static void f_ProcessBattleLose(EM_BattleLoseProcess processType)
    {
        StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.BattleLose, processType);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }

    public static void f_SetSliderTween(UISlider tSlider, float starFloat, float endFloat)
    {

    }

    public static string f_GetPayName(PccaccyDT payDt)
    {
        string payName = string.Empty;
        if (payDt == null)
            return payName;
        if (payDt.iId == 1 || payDt.iId == 2)
            payName = string.Format(CommonTools.f_GetTransLanguage(1992), payDt.iPccaccyNum);
        else
            payName = string.Format(CommonTools.f_GetTransLanguage(1993), payDt.iPccaccyNum);
        return payName;
    }
    /// <summary>
    /// 动态加载图片
    /// </summary>
    /// <param name="texture">图片texture组件</param>
    /// <param name="strTexBgPathRoot">需要加载图片的路径</param>
    public static void f_LoadTexture(UITexture texture, string strTexBgPathRoot)
    {
        if (texture.mainTexture == null)
        {
            Texture2D tTexBg = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgPathRoot);
            texture.mainTexture = tTexBg;
        }
    }

    /// <summary>
    /// 点击物品图标，打开详细信息界面
    /// </summary>
    public static void f_OnItemIconClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, (ResourceCommonDT)obj1);
    }
    /// <summary>
    /// 检测物品是否足够，如果不够则返回false并弹出不足的提示
    /// </summary>
    /// <returns></returns>
    public static bool f_CheckEnoughWaste(byte resourceType1, int resourceId1, int resourceNum1, byte resourceType2, int resourceId2, int resourceNum2,ccUIBase shopUiBase)
    {
        if (resourceType1 > 0)
        {
            ResourceCommonDT waster1 = new ResourceCommonDT();
            waster1.f_UpdateInfo((byte)resourceType1, resourceId1, resourceNum1);
            if (UITool.f_GetGoodNum((EM_ResourceType)waster1.mResourceType, waster1.mResourceId) < waster1.mResourceNum)
            {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, waster1.mName + " không đủ!");
                ccTimeEvent.GetInstance().f_RegEvent(0.6f, false, waster1, (object obj1) => {
                    GetWayPageParam tGetWayParm = new GetWayPageParam((EM_ResourceType)waster1.mResourceType, waster1.mResourceId, shopUiBase);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                });
                return false;
            }
        }
        if (resourceType2 > 0)
        {
            ResourceCommonDT waster2 = new ResourceCommonDT();
            waster2.f_UpdateInfo((byte)resourceType2, resourceId2, resourceNum2);
            if (UITool.f_GetGoodNum((EM_ResourceType)waster2.mResourceType, waster2.mResourceId) < waster2.mResourceNum)
            {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, waster2.mName + " không đủ!");
                ccTimeEvent.GetInstance().f_RegEvent(0.6f, false, waster2, (object obj1) => {
                    GetWayPageParam tGetWayParm = new GetWayPageParam((EM_ResourceType)waster2.mResourceType, waster2.mResourceId, shopUiBase);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                });
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 检测是否达到开启条件
    /// </summary>
    /// <param name="OpenType">开启类型</param>
    /// <param name="OpenValue">开启值</param>
    /// <param name="HintStr">需要提示的字符串</param>
    /// <returns></returns>
    public static bool f_CheckOpen(int OpenType, int OpenValue, ref string HintStr)
    {
        HintStr = "";
        bool isOpen = false;
        switch (OpenType)
        {
            case 1://无条件开启
                isOpen = true;
                break;
            case 2://主角等级
                int Level = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
                isOpen = Level >= OpenValue ? true : false;
                HintStr += string.Format(CommonTools.f_GetTransLanguage(1994), OpenValue);
                break;
            case 3://三国无双通关星星数
                int starNum = Data_Pool.m_RunningManPool.m_iHistoryStarNum;
                isOpen = starNum >= OpenValue ? true : false;
                HintStr += string.Format(CommonTools.f_GetTransLanguage(1995), OpenValue);
                break;
            case 4://竞技场排名
                int rankLevel = Data_Pool.m_ArenaPool.m_iHighstRank;
                isOpen = rankLevel <= OpenValue ? true : false;
                HintStr += string.Format(CommonTools.f_GetTransLanguage(1996), OpenValue);
                break;
            case 5://军团等级
                int LegionLevel = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().f_GetProperty((int)EM_LegionProperty.Lv);
                isOpen = LegionLevel >= OpenValue ? true : false;
                HintStr += string.Format(CommonTools.f_GetTransLanguage(1997), OpenValue);
                break;
        }
        return isOpen;
    }

    /// <summary>
    /// 检测装备/法宝是否已经穿戴
    /// </summary>
    /// <param name="equipId"></param>
    public static bool f_CheckIsWear(long equipId)
    {
        bool isInWear = false;
        List<BasePoolDT<long>> allTeamPoosData = Data_Pool.m_TeamPool.f_GetAll();
        for (int i = 0; i < allTeamPoosData.Count; i++)
        {
            long[] equipArrayId = (allTeamPoosData[i] as TeamPoolDT).m_aEqupId;
            for (int j = 0; j < equipArrayId.Length; j++)
            {
                if (equipArrayId[j] == equipId)
                    isInWear = true;
            }
        }
        return isInWear;
    }
    /// <summary>
    /// 检测是否还有可装备的剩余（装备或法宝共用）
    /// </summary>
    public static bool f_CheckHasEquipLeft(EM_EquipPart equipPart)
    {
        //如果是左右法宝跟普通装备表不一样，表不一样得换
        if (equipPart == EM_EquipPart.eEquipPare_MagicLeft || equipPart == EM_EquipPart.eEquipPare_MagicRight)
        {
            List<BasePoolDT<long>> allUserEquip = Data_Pool.m_TreasurePool.f_GetAll();
            for (int i = 0; i < allUserEquip.Count; i++)
            {
                TreasurePoolDT equipPoolDT = allUserEquip[i] as TreasurePoolDT;
                //屏蔽非同类型的装备（如衣服不能穿在武器栏上）
                if (equipPoolDT.m_TreasureDT.iSite == (int)equipPart && !f_CheckIsWear(equipPoolDT.iId))
                    return true;
            }
        }
        else if (equipPart == EM_EquipPart.eEquipPart_GodWeapon)
        {
            List<BasePoolDT<long>> allUserEquip = Data_Pool.m_GodEquipPool.f_GetAll();
            for (int i = 0; i < allUserEquip.Count; i++)
            {
                GodEquipPoolDT equipPoolDT = allUserEquip[i] as GodEquipPoolDT;
                if (equipPoolDT.m_EquipDT.iSite == (int)equipPart && !f_CheckIsWear(equipPoolDT.iId))
                    return true;
            }
        }
        else
        {
            List<BasePoolDT<long>> allUserEquip = Data_Pool.m_EquipPool.f_GetAll();
            for (int i = 0; i < allUserEquip.Count; i++)
            {
                EquipPoolDT equipPoolDT = allUserEquip[i] as EquipPoolDT;
                //屏蔽非同类型的装备（如衣服不能穿在武器栏上）
                if (equipPoolDT.m_EquipDT.iSite == (int)equipPart && !f_CheckIsWear(equipPoolDT.iId))
                    return true;
            }
        }

        return false;
    }
    /// <summary>
    /// 检测是否还有更高品质装备的剩余（装备或法宝共用）
    /// </summary>
    public static bool f_CheckHasEquipHighColorLeft(EM_EquipPart equipPart, int curColor)
    {
        //如果是左右法宝跟普通装备表不一样，表不一样得换
        if (equipPart == EM_EquipPart.eEquipPare_MagicLeft || equipPart == EM_EquipPart.eEquipPare_MagicRight)
        {
            List<BasePoolDT<long>> allUserEquip = Data_Pool.m_TreasurePool.f_GetAll();
            for (int i = 0; i < allUserEquip.Count; i++)
            {
                TreasurePoolDT equipPoolDT = allUserEquip[i] as TreasurePoolDT;
                //屏蔽非同类型的装备（如衣服不能穿在武器栏上）
                if (equipPoolDT.m_TreasureDT.iSite == (int)equipPart && !f_CheckIsWear(equipPoolDT.iId) && equipPoolDT.m_TreasureDT.iImportant > curColor)
                    return true;
            }
        }
        else
        {
            List<BasePoolDT<long>> allUserEquip = Data_Pool.m_EquipPool.f_GetAll();
            for (int i = 0; i < allUserEquip.Count; i++)
            {
                EquipPoolDT equipPoolDT = allUserEquip[i] as EquipPoolDT;
                //屏蔽非同类型的装备（如衣服不能穿在武器栏上）
                if (equipPoolDT.m_EquipDT.iSite == (int)equipPart && !f_CheckIsWear(equipPoolDT.iId) && equipPoolDT.m_EquipDT.iColour > curColor)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// DayOfWeek转成中文
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    public static string f_DayOfWeek2String(DayOfWeek day)
    {
        string result = string.Empty;
        switch (day)
        {
            case DayOfWeek.Sunday:
                result = CommonTools.f_GetTransLanguage(1998);
                break;
            case DayOfWeek.Monday:
                result = CommonTools.f_GetTransLanguage(1999);
                break;
            case DayOfWeek.Tuesday:
                result = CommonTools.f_GetTransLanguage(2000);
                break;
            case DayOfWeek.Wednesday:
                result = CommonTools.f_GetTransLanguage(2001);
                break;
            case DayOfWeek.Thursday:
                result = CommonTools.f_GetTransLanguage(2002);
                break;
            case DayOfWeek.Friday:
                result = CommonTools.f_GetTransLanguage(2003);
                break;
            case DayOfWeek.Saturday:
                result = CommonTools.f_GetTransLanguage(2004);
                break;
        }
        return result;
    }

    public static string f_GetFriendServerResultDesc(string playerName, int result)
    {
        string tDesc = string.Empty;
        //服务器返回的
        if (result == (int)eMsgOperateResult.eOR_UserNotFound)
        {
            tDesc = CommonTools.f_GetTransLanguage(2005);
        }
        else if (result == (int)eMsgOperateResult.eOR_UserOffline)
        {
            tDesc = string.Format(CommonTools.f_GetTransLanguage(2006), playerName);
        }
        else if (result == (int)eMsgOperateResult.eOR_InPeerBlack)
        {
            tDesc = string.Format(CommonTools.f_GetTransLanguage(2007), playerName);
        }
        else if (result == (int)eMsgOperateResult.eOR_PeerInBlack)
        {
            tDesc = string.Format(CommonTools.f_GetTransLanguage(2008), playerName);
        }
        else if (result == (int)eMsgOperateResult.eOR_FriendListIsFull)
        {
            tDesc = string.Format(CommonTools.f_GetTransLanguage(2009), playerName);
        }
        return tDesc;
    }
    /// <summary>
    /// 获取服务器状态信息
    /// </summary>
    /// <param name="serverInforDT">dt</param>
    /// <returns></returns>
    public static EM_ServerState f_GetServerState(ServerInforDT serverInforDT)
    {
        EM_ServerState emServerState = EM_ServerState.Hot;
        if (serverInforDT.szAutoOpenTime != null && serverInforDT.szAutoOpenTime != "")
        {
            DateTime datetime1970 = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - datetime1970;
            DateTime dataOpenTime = CommonTools.f_GetDateTimeByTimeStr2(serverInforDT.szAutoOpenTime);
            long dateOT = ccMath.DateTime2time_t(dataOpenTime);
            if (timeSpan.TotalSeconds <= (dateOT + 2 * 24 * 60 * 60))//超过两天变成hot
                emServerState = EM_ServerState.New;
        }
        if (serverInforDT.iServState != (int)EM_ServerState.Hot && serverInforDT.iServState != (int)EM_ServerState.New)
        {
            emServerState = (EM_ServerState)serverInforDT.iServState;
        }
        return emServerState;
    }

    /// <summary>
    /// color 转换hex
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255.0f);
        int g = Mathf.RoundToInt(color.g * 255.0f);
        int b = Mathf.RoundToInt(color.b * 255.0f);
        int a = Mathf.RoundToInt(color.a * 255.0f);
        string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
        return hex;
    }

    /// <summary>
    /// hex转换到color
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color HexToColor(string hex)
    {
        byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        float r = br / 255f;
        float g = bg / 255f;
        float b = bb / 255f;
        float a = cc / 255f;
        return new Color(r, g, b, a);
    }

    public static string GetEnumName(Type enumType, object value)
    {
        return Enum.GetName(enumType, value);
    }

    public static string f_GetCardFightType(EM_CardFightType type)
    {
        string Desc = string.Empty;
        switch (type)
        {
            case EM_CardFightType.eCardPhyAtt:
                return CommonTools.f_GetTransLanguage(2232);
            case EM_CardFightType.eCardMagAtt:
                return CommonTools.f_GetTransLanguage(2233);
            case EM_CardFightType.eCardSup:
                return CommonTools.f_GetTransLanguage(2234);
            case EM_CardFightType.eCardTank:
                return CommonTools.f_GetTransLanguage(2235);
            default:
                break;
        }
        return Desc;
    }

    //征战界面名
    //private static string[] challengePageName = new string[8] { UINameConst.ArenaPageNew, UINameConst.GrabTreasurePage ,UINameConst.RunningManPage ,
    //    UINameConst.PatrolPage ,UINameConst.RebelArmy,UINameConst.CrossServerBattlePage,UINameConst.CardBattlePage,UINameConst.TrialTowerPage};
    //TsuCode - ChaosBattle
    private static string[] challengePageName = new string[9] { UINameConst.ArenaPageNew, UINameConst.GrabTreasurePage ,UINameConst.RunningManPage ,
        UINameConst.PatrolPage ,UINameConst.RebelArmy,UINameConst.CrossServerBattlePage,UINameConst.CardBattlePage,UINameConst.TrialTowerPage,UINameConst.ChaosBattlePage};

    //

    /// <summary>
    /// 判断是否可以进某个征战界面，解决因底层界面管理器没有界面堆栈管理，断线重连界面重叠或者白屏问题
    /// 1、如果断线的时候点了某个界面，重连后又点另一个征战界面，服务器消息回来就两个界面重叠冲突了
    /// 2、切回主界面，可能断线重连又返回打开征战界面的消息，此时打开则也会界面重叠
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public static bool f_IsCanOpenChallengePage(string uiName)
    {
        //先判断有没打开其他征战界面
        for (int i = 0; i < challengePageName.Length; i++)
        {
            //过滤掉自己
            if (challengePageName[i] == uiName)
            {
                continue;
            }

            //判断其他征战界面有没打开，打开则不能再打开,,,ccUIManage.GetInstance().f_CheckUIIsOpen好像有bug，原始判断
            ccUIBase uibase = ccUIManage.GetInstance().f_GetUIHandler(challengePageName[i]);
            if (null != uibase && uibase.m_Panel.gameObject.activeInHierarchy)
            {
                return false;
            }
        }

        //再判断有没打开主界面,切回主界面，可能断线重连又返回打开界面的消息，此时打开则也会界面重叠
        ccUIBase uiMain = ccUIManage.GetInstance().f_GetUIHandler(UINameConst.MainMenu);
        if (null != uiMain && uiMain.m_Panel.gameObject.activeInHierarchy)
        {
            return false;
        }

        return true;
    }

    public static Dictionary<int, bool> ConverStringToDictionrary(string array)
    {
        string[] data = array.Split('#');
        Dictionary<int, bool> dict_data = new Dictionary<int, bool>();
        for (int i = 0; i < data.Length; i++)
        {
            string StrCommonItem = data[i];
            if (StrCommonItem.Contains(";"))
            {
                int id = int.Parse(StrCommonItem.Split(';')[0]);
                bool check = int.Parse(StrCommonItem.Split(';')[1]) == 0 ? true : false;
                if (dict_data.ContainsKey(id))
                {
                    dict_data[id] = check;
                }
                else
                {
                    dict_data.Add(id, check);
                }
            }
        }
        return dict_data;
    }

    public static string ConverDictionraryToString(Dictionary<int, bool> array)
    {
        string data = "";
        for (int i = 0; i < array.Count; i++)
        {
            var item = array.ElementAt(i);
            string id = item.Key.ToString();
            string check = (item.Value == true ? 0 : 1).ToString();
            data += id + ";" + check;
            if(i < array.Count -1)
            {
                data += "#";
            }
        }

        return data;
    }

    public static void CreatLocalDataLevelGift(int id, bool check)
    {
        StaticValue.m_LocalLevelGift = LocalDataManager.f_GetLocalData<string>(LocalDataType.LevelGift, StaticValue.m_LoginName);
        Dictionary<int, bool> dict_data = UITool.ConverStringToDictionrary(StaticValue.m_LocalLevelGift == null ? "" : StaticValue.m_LocalLevelGift);
        if (dict_data.ContainsKey(id)){
            if (!check)
            {
                dict_data[id] = check;
            }
        }
        else
        {
            dict_data.Add(id, check);
        }

        string  s = UITool.ConverDictionraryToString(dict_data);
        LocalDataManager.f_SetLocalData<string>(LocalDataType.LevelGift, s, StaticValue.m_LoginName);
    }

    public static bool CheckInMainMenu()
    {
        GameObject Mainmenu = GameObject.Find("MainMenu/Panel");
        if (Mainmenu != null)
        {
            return Mainmenu.activeInHierarchy;
        }
        return false;
    }

    public static int f_GetGodEquipPro(GodEquipPoolDT dt)
    {
        RoleProperty ttt = new RoleProperty();
        RolePropertyTools.f_DispGodEquip(ref ttt, dt);
        int num = 0;
        for (int i = (int)EM_RoleProperty.Atk; i <= (int)EM_RoleProperty.MDef; i++)
        {
            if (ttt.f_GetProperty(i) > num)
                num = ttt.f_GetProperty(i);
        }
        return num;
    }

    public static CardPoolDT f_GetWhoEquip(long EquipId)
    {
        List<BasePoolDT<long>> tmpList = Data_Pool.m_TeamPool.f_GetAll();
        CardPoolDT tmpStr = null;
        TeamPoolDT item;     // TeamPoolDT item = new TeamPoolDT();
        for (int j = 0; j < tmpList.Count; j++)
        {
            item = tmpList[j] as TeamPoolDT;
            for (int i = 0; i < item.m_aEqupId.Length; i++)
            {
                if (item.m_aEqupId[i] == EquipId)
                {
                    tmpStr = item.m_CardPoolDT;
                }
            }
        }
        return tmpStr;
    }

    public static int f_GetGodEquipIntenMax()
    {
        //以前跟主角等级挂钩，，但是现在的表只填了150级的消耗数据（超过150级就异常了），，为了以后能扩展等级，就修改为填多少上限就是多少吧！
        int maxByLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) * 2;
        int maxByTable = glo_Main.GetInstance().m_SC_Pool.m_GodEquipIntensifyConsumeSC.f_GetAll().Count;
        return maxByLv > maxByTable ? maxByTable : maxByLv;
    }

    public static int f_GetGodEquipIntenCon(GodEquipPoolDT tmp, int Times)
    {
        int lvIntensify = tmp.m_lvIntensify;
        int Con = 0;
        for (int times = 1; times <= Times; times++)
        {
            GodEquipIntensifyConsumeDT tConsume = glo_Main.GetInstance().m_SC_Pool.m_GodEquipIntensifyConsumeSC.f_GetSC(lvIntensify + times) as GodEquipIntensifyConsumeDT;
            if (tConsume == null)
                return 0;
            switch ((EM_Important)tmp.m_EquipDT.iColour)
            {
                case EM_Important.Green:
                    Con += tConsume.iGreen;
                    break;
                case EM_Important.Blue:
                    Con += tConsume.iBule;
                    break;
                case EM_Important.Purple:
                    Con += tConsume.iViolet;
                    break;
                case EM_Important.Oragen:
                    Con += tConsume.iOrange;
                    break;
                case EM_Important.Red:
                    Con += tConsume.iRed;
                    break;
                case EM_Important.Gold:
                    Con += tConsume.iGold;
                    break;
                default:
                    break;
            }
        }
        return Con;
    }

    public static int[] f_GetGodEquipRefinePro(GodEquipPoolDT dt)
    {
        int[] tmp = new int[2];
        tmp[0] = dt.m_EquipDT.iRefinPro1 * dt.m_lvRefine;
        tmp[1] = dt.m_EquipDT.iRefinPro2 * dt.m_lvRefine;
        return tmp;
    }

    public static string f_GetGodStarSucRate(GodEquipPoolDT dt)
    {
        GodEquipUpStarDT tmp = f_GetGodEquipUpStar(dt);

        int num = tmp.iInitial + (dt.m_slucky / 150) * 500;

        if (num <= 3000)
        {
            return CommonTools.f_GetTransLanguage(1901);
        }
        if (num > 3100 && num <= 5000)
        {
            return CommonTools.f_GetTransLanguage(1902);
        }
        if (num > 5100 && num <= 7000)
        {
            return CommonTools.f_GetTransLanguage(1903);
        }
        if (num > 7100 && num <= 9000)
        {
            return CommonTools.f_GetTransLanguage(1904);
        }
        if (num > 9100)
        {
            return CommonTools.f_GetTransLanguage(1905);
        }
        return null;
    }

    public static GodEquipUpStarDT f_GetGodEquipUpStar(GodEquipPoolDT Equip)
    {
        if (Equip.m_sstars == 5 && Equip.m_EquipDT.iColour >= (int)EM_Important.Red || Equip.m_sstars == 3 && Equip.m_EquipDT.iColour == (int)EM_Important.Oragen)
            return (GodEquipUpStarDT)glo_Main.GetInstance().m_SC_Pool.m_GodEquipUpStarSC.f_GetSC(Equip.m_EquipDT.iId * 100 + Equip.m_sstars);
        return (GodEquipUpStarDT)glo_Main.GetInstance().m_SC_Pool.m_GodEquipUpStarSC.f_GetSC(Equip.m_EquipDT.iId * 100 + Equip.m_sstars + 1);
    }

    public static int f_GetGodEquipRefineExp(GodEquipPoolDT dt)
    {
        int tmp = 0;
        tmp = GetEquipRefineId(dt.m_EquipDT.iColour) + dt.m_lvRefine;
        if (glo_Main.GetInstance().m_SC_Pool.m_GodEquipConsumeSC.f_GetSC(tmp + 1) != null)
            return ((GodEquipConsumeDT)glo_Main.GetInstance().m_SC_Pool.m_GodEquipConsumeSC.f_GetSC(tmp + 1)).iRefineExp;
        else
            return 0;
    }

    public static int f_GetGodEquipFragmentNum(int iId)
    {
        int tNum = 0;
        GodEquipFragmentPoolDT tGodEquipFragmentPoolDT;
        for (int i = 0; i < Data_Pool.m_GodEquipFragmentPool.f_GetAllForData1(iId).Count; i++)
        {
            tGodEquipFragmentPoolDT = Data_Pool.m_GodEquipFragmentPool.f_GetAllForData1(iId)[i] as GodEquipFragmentPoolDT;

            tNum += tGodEquipFragmentPoolDT.m_iNum;
        }


        return tNum;
    }

    public static int[] f_GetGodEquipStarPro(GodEquipPoolDT Edt)
    {
        int[] tmpInt = new int[3];
        BetterList<GodEquipUpStarDT> tmpList = new BetterList<GodEquipUpStarDT>();
        foreach (GodEquipUpStarDT item in glo_Main.GetInstance().m_SC_Pool.m_GodEquipUpStarSC.f_GetAll())
        {
            if (Edt.m_EquipDT.iColour == (int)EM_Important.Oragen)
            {
                if (tmpList.size == 3)
                    break;
            }
            if (Edt.m_EquipDT.iColour == (int)EM_Important.Red)
            {
                if (tmpList.size == 5)
                    break;
            }
            if (item.iEquipId == Edt.m_EquipDT.iId)
            {
                tmpList.Add(item);
            }
        }
        for (int i = 0; i < Edt.m_sstars; i++)
        {
            if (Edt.m_sstars > i)
            {
                tmpInt[0] += tmpList[i].iAddPro + tmpList[i].iAddNum;
            }
            else if (Edt.m_sstars == i)
                tmpInt[0] += (int)((float)tmpList[i].iAddPro * (float)((float)((float)Edt.m_sexpStars / (float)tmpList[i].iUpExp) * (float)100) / (float)100);
        }
        try
        {
            tmpInt[1] = f_GetGodEquipUpStar(Edt).iAddPro;
            tmpInt[2] = f_GetGodEquipUpStar(Edt).iAddNum;
        }
        catch
        {
            tmpInt[1] = 0;
            tmpInt[2] = 0;
        }
        return tmpInt;
    }

    public static int GetMainCardId(EM_RoleSex sex, EM_CardFightType job)
    {
        int cardId = 0;
        if (sex == EM_RoleSex.Man)
        {
            switch (job)
            {
                case EM_CardFightType.eCardPhyAtt:
                    cardId = 1000;
                    break;
                case EM_CardFightType.eCardMagAtt:
                    cardId = 1010;
                    break;
                case EM_CardFightType.eCardSup:
                    cardId = 1020;
                    break;
                case EM_CardFightType.eCardTank:
                    cardId = 1030;
                    break;
                case EM_CardFightType.eCardKiller:
                    cardId = 1040;
                    break;
                case EM_CardFightType.eCardPhysician:
                    cardId = 1050;
                    break;
                case EM_CardFightType.eCardLogistics:
                    cardId = 1060;
                    break;
                default:
                    cardId = 1000;
                    break;
            }
        }
        else
        {
            switch (job)
            {
                case EM_CardFightType.eCardPhyAtt:
                    cardId = 1001;
                    break;
                case EM_CardFightType.eCardMagAtt:
                    cardId = 1011;
                    break;
                case EM_CardFightType.eCardSup:
                    cardId = 1021;
                    break;
                case EM_CardFightType.eCardTank:
                    cardId = 1031;
                    break;
                case EM_CardFightType.eCardKiller:
                    cardId = 1041;
                    break;
                case EM_CardFightType.eCardPhysician:
                    cardId = 1051;
                    break;
                case EM_CardFightType.eCardLogistics:
                    cardId = 1061;
                    break;
                default:
                    cardId = 1001;
                    break;
            }
        }
        return cardId;
    }
}

