using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
public class ViewPlayerLineUpPageParam
{
    /// <summary>
    /// 玩家id
    /// </summary>
    public long userId;
    /// <summary>
    /// 玩家姓名
    /// </summary>
    public string szName;
}
/// <summary>
/// 查看其他玩家阵容界面
/// </summary>
public class ViewPlayerLineUpPage : UIFramwork
{
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private ViewPlayerLineUpPageParam param = null;
    public static EM_FormationPos CurrentSelectCardPos = EM_FormationPos.eFormationPos_Main;//当前选中的卡牌位置
	private BasePlayerPoolDT playerInfo;
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        param = (ViewPlayerLineUpPageParam)e;
        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        Data_Pool.m_ViewPlayerLineUpPool.f_QueryPlayerTeam(param.userId, QueryCallback);
		playerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(param.userId);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    /// <summary>
    /// 查询成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuerySucCallback(object obj)
    {
        //MessageBox.DEBUG("查询成功");
        //设置数据
        CurrentSelectCardPos = EM_FormationPos.eFormationPos_Main;
        InitTeamData();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnQueryFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Truy vấn không thành công!");
    }
    /// <summary>
    /// 页面关闭
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }
    /// <summary>
    /// 设置左菜单卡牌按钮UI
    /// </summary>
    private void SetCardBtnContent(GameObject go, PlayerTeamItem data, EM_FormationPos index)
    {
        if(data == null || (data.mCardId == 0 && data.mFashId == 0))
        {
            go.gameObject.SetActive(false);
            return;
        }
		CardDT cardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(data.mCardId) as CardDT;
        go.gameObject.SetActive(true);
        go.transform.Find("SprHeadIcon").gameObject.SetActive(true);
		//My Code
		if(index == EM_FormationPos.eFormationPos_Main)//如果是主卡，则显示的是玩家的名字
        {
			if(playerInfo.m_iSex == 0 || playerInfo.m_iSex == 1)
			{
				//MessageBox.ASSERT("IconID: " + data.mCardId);
				go.transform.Find("SprHeadIcon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteByCardId(data.mCardId);
			}
			else
			{
				//MessageBox.ASSERT("IconID: " + playerInfo.m_iSex);
				go.transform.Find("SprHeadIcon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteBySexId(playerInfo.m_iSex);
			}
		}
		else
		{
			//MessageBox.ASSERT("IconID: " + data.mCardId);
			go.transform.Find("SprHeadIcon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteByCardId(data.mCardId);
		}
		//
        int iEvolveLv = data.miEvolveLv;
        string cardName = cardDT.szName;
        string borderName = UITool.f_GetImporentColorName(cardDT.iImportant, ref cardName);
        go.transform.Find("SprBorder").gameObject.SetActive(true);
        go.transform.Find("SprBorder").GetComponent<UISprite>().spriteName = borderName;
    }
    /// <summary>
    /// 根据选中的位置获取左侧卡牌按钮
    /// </summary>
    /// <param name="emFormationPos">卡牌位置</param>
    /// <returns></returns>
    private GameObject GetCardBtnByIndex(EM_FormationPos emFormationPos)
    {
        GameObject cardBtn = null;
        switch(emFormationPos)
        {
            case EM_FormationPos.eFormationPos_Main:
                cardBtn = f_GetObject("BtnCardMain");
                break;
            case EM_FormationPos.eFormationPos_Assist1:
                cardBtn = f_GetObject("BtnCardAssist1");
                break;
            case EM_FormationPos.eFormationPos_Assist2:
                cardBtn = f_GetObject("BtnCardAssist2");
                break;
            case EM_FormationPos.eFormationPos_Assist3:
                cardBtn = f_GetObject("BtnCardAssist3");
                break;
            case EM_FormationPos.eFormationPos_Assist4:
                cardBtn = f_GetObject("BtnCardAssist4");
                break;
            case EM_FormationPos.eFormationPos_Assist5:
                cardBtn = f_GetObject("BtnCardAssist5");
                break;
            case EM_FormationPos.eFormationPos_Assist6:
                cardBtn = f_GetObject("BtnCardAssist6");
                break;
            case EM_FormationPos.eFormationPos_Pet:
                cardBtn = f_GetObject("BtnCardPet");
                break;
        }
        return cardBtn;
    }
    /// <summary>
    /// 初始化阵容数据
    /// </summary>
    private void InitTeamData()
    {
        for(int i = 0; i < GameParamConst.MAX_FIGHT_POS; i++)
        {
            EM_FormationPos pos = (EM_FormationPos)i;
            SetCardBtnContent(GetCardBtnByIndex(pos), i > Data_Pool.m_ViewPlayerLineUpPool.mDicTeamPoolDT.Count - 1 ? null : Data_Pool.m_ViewPlayerLineUpPool.mDicTeamPoolDT[pos], pos);
        }
        f_GetObject("CardGrid").GetComponent<UIGrid>().Reposition();
        UpdateModelData(CurrentSelectCardPos);
        f_GetObject("CardProgressBar").GetComponent<UIProgressBar>().value = (int)CurrentSelectCardPos * 1.0f / 8;
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlackBg", OnBtnBlackClick);
        //卡牌按钮事件
        f_RegClickEvent("BtnCardMain", OnCardClick, EM_FormationPos.eFormationPos_Main);
        f_RegClickEvent("BtnCardAssist1", OnCardClick, EM_FormationPos.eFormationPos_Assist1);
        f_RegClickEvent("BtnCardAssist2", OnCardClick, EM_FormationPos.eFormationPos_Assist2);
        f_RegClickEvent("BtnCardAssist3", OnCardClick, EM_FormationPos.eFormationPos_Assist3);
        f_RegClickEvent("BtnCardAssist4", OnCardClick, EM_FormationPos.eFormationPos_Assist4);
        f_RegClickEvent("BtnCardAssist5", OnCardClick, EM_FormationPos.eFormationPos_Assist5);
        f_RegClickEvent("BtnCardAssist6", OnCardClick, EM_FormationPos.eFormationPos_Assist6);
    }
    /// <summary>
    /// 更新模型（上阵完卡牌，装备完装备或法宝更新UI事件）
    /// </summary>
    private void UpdateModelView(object data)
    {
        if(Data_Pool.m_ViewPlayerLineUpPool.mDicTeamPoolDT.ContainsKey(CurrentSelectCardPos))
        {
            UpdateModelData(CurrentSelectCardPos);
        }
    }
    #region 按钮事件
    /// <summary>
    /// 点击左侧栏卡片按钮事件
    /// </summary>
    private void OnCardClick(GameObject go, object obj1, object obj2)
    {
        CurrentSelectCardPos = (EM_FormationPos)obj1;//隐藏其他选中状态
        UpdateModelData(CurrentSelectCardPos);
    }
    /// <summary>
    /// 点击黑色背景关闭页面
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ViewPlayerLineUpPage, UIMessageDef.UI_CLOSE);
    }
    #endregion
    /// <summary>
    /// 设置卡牌按钮状态
    /// </summary>
    private void UpdateCardBtnState(EM_FormationPos emFormationPos)
    {
        for(int i = 0; i < (int)EM_FormationPos.eFormationPos_Pet; i++)
        {
            GetCardBtnByIndex((EM_FormationPos)i).transform.Find("SelectEffect").gameObject.SetActive(emFormationPos == (EM_FormationPos)i);
        }
    }
    /// <summary>
    /// 通过装备位获取对应装备按钮
    /// </summary>
    /// <param name="emEquipPart">装备部位</param>
    /// <returns></returns>
    private GameObject GetEquipBtnByIndex(EM_EquipPart emEquipPart)
    {
        GameObject EquipBtn = null;
        switch(emEquipPart)
        {
            case EM_EquipPart.eEquipPart_Weapon:
                EquipBtn = f_GetObject("BtnWeapon");
                break;
            case EM_EquipPart.eEquipPart_Armour:
                EquipBtn = f_GetObject("BtnArmour");
                break;
            case EM_EquipPart.eEquipPart_Helmet:
                EquipBtn = f_GetObject("BtnHelmet");
                break;
            case EM_EquipPart.eEquipPart_Belt:
                EquipBtn = f_GetObject("BtnBelt");
                break;
            case EM_EquipPart.eEquipPare_MagicLeft:
                EquipBtn = f_GetObject("BtnMagicLeft");
                break;
            case EM_EquipPart.eEquipPare_MagicRight:
                EquipBtn = f_GetObject("BtnMagicRight");
                break;
            case EM_EquipPart.eEquipPart_GodWeapon:
                EquipBtn = f_GetObject("BtnGodWeapon");
                break;
        }
        return EquipBtn;
    }
    /// <summary>
    /// 更新模型数据
    /// </summary>
    /// <param name="index">选中的卡牌部位</param>
    private void UpdateModelData(EM_FormationPos index)
    {
        bool isContainKey = Data_Pool.m_ViewPlayerLineUpPool.mDicTeamPoolDT.ContainsKey(index);//该位置是否有卡牌上阵
        UpdateCardBtnState(index);//设置左侧卡牌按钮选中状态
        for(int i = 1; i <= 7; i++)//设置装备位置显示状态（有上阵则显示否则不显示）
            GetEquipBtnByIndex((EM_EquipPart)i).SetActive(isContainKey);
        GameObject ModelPoint = f_GetObject("ModelPoint");
        ModelPoint.SetActive(isContainKey);
        if(ModelPoint.transform.Find("Model") != null)//删除旧的卡牌模型
            UITool.f_DestoryStatelObject(ModelPoint.transform.Find("Model").gameObject);
        RoleProperty tmpRole = new RoleProperty();
        if(isContainKey)
        {
            PlayerTeamItem data = Data_Pool.m_ViewPlayerLineUpPool.mDicTeamPoolDT[index];
            int iEvolveLv = data.miEvolveLv;
            CardDT cardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(data.mCardId) as CardDT;
            string cardName = cardDT.szName + (iEvolveLv > 0 ? ("+" + iEvolveLv) : "");
            if(index == EM_FormationPos.eFormationPos_Main)//如果是主卡，则显示的是玩家的名字
            {
				//My Code
                if (playerInfo.m_iSex == 0)
                    UITool.f_GetStatelObject(data.mCardId, ModelPoint.transform, Vector3.zero, new Vector3(0f, -50f, 0f), 30, "Model", 90);
                else
                    UITool.f_GetStatelObjectByFashId(playerInfo.m_iSex, ModelPoint.transform, Vector3.zero, new Vector3(0f, -50f, 0f), 30, "Model", 90);
				//
                cardName = param.szName + (iEvolveLv > 0 ? ("+" + iEvolveLv) : "");
            }
            else
            {
                UITool.f_GetStatelObject(cardDT.iId, ModelPoint.transform, Vector3.zero, new Vector3(0f, -50f, 0f), 30, "Model", 90);

            }
            UITool.f_GetImporentColorName(cardDT.iImportant, ref cardName);
            f_GetObject("LabelCardName").GetComponent<UILabel>().text = cardName;
            InitEquipAndTreasureData(data);
            f_GetObject("LabelGetCardType").GetComponent<UILabel>().text = UITool.f_GetCardFightType(cardDT.iCardFightType);
            f_GetObject("LabelCardCamp").GetComponent<UILabel>().text = UITool.f_GetCardCamp(cardDT.iCardCamp);

            f_UpdateStarNum(data.mAwakeLevel / 10);
        }

    }
    /// <summary>
    /// 根据数量来显示星星
    /// </summary>
    /// <param name="stars">星星显示数组</param>
    /// <param name="num">显示的星数</param>
    public void f_UpdateStarNum(int num)
    {
        for(int i = 0; i < 6; i++)
        {
            f_GetObject("AwakenStar").transform.Find("Sprite" + i).GetComponent<UISprite>().spriteName = i < num ? "Icon_DungeonStarGrey" : "Icon_Star";
        }
    }
    /// <summary>
    /// 设置装备和法宝的UI数据
    /// </summary>
    /// <param name="data">阵容PoolDT</param>
    private void InitEquipAndTreasureData(PlayerTeamItem data)
    {
        string name = "";
        string borderSprName = "";
        for(int i = 1; i <= 4; i++)//装备
        {
            if(data.dicEquipItem[(EM_EquipPart)i] != null)
            {
                PlayerTeamEquipItem item = data.dicEquipItem[(EM_EquipPart)i];
                EquipDT equipDT = glo_Main.GetInstance().m_SC_Pool.m_EquipSC.f_GetSC(item.EquipId) as EquipDT;
                name = equipDT.szName;
                borderSprName = "";
                borderSprName = UITool.f_GetImporentColorName(equipDT.iColour, ref name);
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), name, equipDT.iIcon, borderSprName);
            }
            else
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), "", -1, "");
        }
        for(int i = 5; i <= 6; i++)//法宝
        {
            if(data.dicEquipItem[(EM_EquipPart)i] != null)
            {
                PlayerTeamEquipItem item = data.dicEquipItem[(EM_EquipPart)i];
                TreasureDT treasureDT = glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(item.EquipId) as TreasureDT;
                name = treasureDT.szName;
                borderSprName = "";
                borderSprName = UITool.f_GetImporentColorName(treasureDT.iImportant, ref name);
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), name, treasureDT.iIcon, borderSprName);
            }
            else
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), "", -1, "");
        }
        for (int i = 7; i <= 7; i++)//法宝
        {
            if (data.dicEquipItem[(EM_EquipPart)i] != null)
            {
                PlayerTeamEquipItem item = data.dicEquipItem[(EM_EquipPart)i];
                GodEquipDT qodEquipDT = glo_Main.GetInstance().m_SC_Pool.m_GodEquipSC.f_GetSC(item.EquipId) as GodEquipDT;
                name = qodEquipDT.szName;
                borderSprName = "";
                borderSprName = UITool.f_GetImporentColorName(qodEquipDT.iColour, ref name);
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), name, qodEquipDT.iIcon, borderSprName);
            }
            else
                SetEquipPosData(GetEquipBtnByIndex((EM_EquipPart)i), "", -1, "");
        }
    }
    /// <summary>
    /// 设置装备位内容
    /// </summary>
    /// <param name="EquipObj">装备位按钮</param>
    /// <param name="Name">装备名称</param>
    /// <param name="iconID">装备图标id</param>
    /// <param name="sprBorderSpriteName">装备边框</param>
    private void SetEquipPosData(GameObject EquipObj, string Name, int iconID, string sprBorderSpriteName)
    {
        EquipObj.GetComponentInChildren<UILabel>().text = Name;
        UI2DSprite spriteIcon3 = EquipObj.transform.Find("Icon").GetComponent<UI2DSprite>();
        spriteIcon3.gameObject.SetActive(iconID == -1 ? false : true);
        spriteIcon3.sprite2D = UITool.f_GetIconSprite(iconID);
        EquipObj.transform.Find("IconBorder").gameObject.SetActive(sprBorderSpriteName == "" ? false : true);
        EquipObj.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = sprBorderSpriteName;
    }
}
