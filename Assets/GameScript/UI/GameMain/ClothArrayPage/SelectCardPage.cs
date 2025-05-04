using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 选择卡牌界面（点阵容界面选择上阵卡牌，或者卡牌属性点更换卡牌）
/// </summary>
public class SelectCardPage : UIFramwork
{
    private UIWrapComponent CardWrapComponent = null;//拖动组件 
    private List<BasePoolDT<long>> _CardList;//卡牌列表
    private EM_FormationPos needReplacePos;//需要替换的上阵序号
    private SocketCallbackDT OnChangeTeamCardCallback = new SocketCallbackDT();//更换卡牌回调  
    private SocketCallbackDT OnChangeReinforceCallback = new SocketCallbackDT();//更换援军回调
    /// <summary>
    /// 检查是否有可上阵的卡牌
    /// </summary>
    /// <returns></returns>
    public bool CheckHasCardLeft()
    {
        Data_Pool.m_CardPool.f_SortList();//排序
        _CardList = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_CardPool.f_GetAll());
        List<BasePoolDT<long>> temp = Data_Pool.m_TeamPool.f_GetAll();
        CardPoolDT needRemoveCard = null;
        //去除已上阵卡牌位
        for (int i = 0; i < temp.Count; i++)
        {
            TeamPoolDT dt = temp[i] as TeamPoolDT;
            //上阵可以本位置英雄类型，但去除同一张卡牌
            if (LineUpPage.CurrentSelectCardPos == dt.m_eFormationPos)
            {
                needRemoveCard = dt.m_CardPoolDT;
                continue;
            }
            if (dt.m_eFormationPos == EM_FormationPos.eFormationPos_Main || dt.m_eFormationPos == EM_FormationPos.eFormationPos_Assist1 ||
                dt.m_eFormationPos == EM_FormationPos.eFormationPos_Assist2 || dt.m_eFormationPos == EM_FormationPos.eFormationPos_Assist3 ||
                dt.m_eFormationPos == EM_FormationPos.eFormationPos_Assist4 || dt.m_eFormationPos == EM_FormationPos.eFormationPos_Assist5 || dt.m_eFormationPos == EM_FormationPos.eFormationPos_Assist6)
            {
                //去除模板id,同模板id不能同时上阵
                RemoveCardByTempId(_CardList, dt.m_CardPoolDT.m_CardDT.iId);
            }
        }
        if (needRemoveCard != null)
        {
            RemoveCardById(_CardList, needRemoveCard.iId);
        }
        //去除已上阵的援军位
        CardPoolDT needRemoveReinforcePosCard = null;
        IEnumerator<EM_ReinforcePos> listData = Data_Pool.m_TeamPool.dicReinforceCardId.Keys.GetEnumerator();
        while (listData.MoveNext())
        {
            CardPoolDT CurrentCardPoolDT = Data_Pool.m_TeamPool.dicReinforceCardId[listData.Current];
            if (CurrentCardPoolDT != null)
            {
                if (LineUpPage.CurrentSelectCardPos == EM_FormationPos.eFormationPos_Reinforce && listData.Current == LineUpPage.CurrentSelectReinforcePos)
                {
                    needRemoveReinforcePosCard = CurrentCardPoolDT;
                    continue;
                }
                //去除模板id,同模板id不能同时上阵
                RemoveCardByTempId(_CardList, CurrentCardPoolDT.m_CardDT.iId);
            }
        }
        if (needRemoveReinforcePosCard != null)
        {
            RemoveCardById(_CardList, needRemoveReinforcePosCard.iId);
        }

        //排序，合击排前面
        //排序2（可增加缘分的往前，缘分相同的看品质）
        int ChangeIndex = 0;
        //如果阵容选择位是战宠或者援军，则不考虑合击排序。援军只看缘分
        if (LineUpPage.CurrentSelectCardPos != EM_FormationPos.eFormationPos_Reinforce &&
            LineUpPage.CurrentSelectCardPos != EM_FormationPos.eFormationPos_Pet)
        {
            string tempStr = "";
            for (int i = 0; i < _CardList.Count; i++)
            {
                CardPoolDT poolDT = _CardList[i] as CardPoolDT;
                if (CheckJointAttack(poolDT.m_CardDT, ref tempStr))
                {
                    _CardList.Remove(poolDT);
                    _CardList.Insert(ChangeIndex, poolDT);
                    ChangeIndex++;
                }
            }
        }
        int ChangeIndex2 = ChangeIndex;
        for (int i = ChangeIndex; i < _CardList.Count; i++)
        {
            CardPoolDT poolDT = _CardList[i] as CardPoolDT;
            int fateCount = CheckFateCount(poolDT.m_CardDT.iId);
            if (fateCount > 0)
            {
                _CardList.Remove(poolDT);
                _CardList.Insert(ChangeIndex2, poolDT);
                ChangeIndex2++;
            }
        }
        if (_CardList.Count <= 0)
        {
            return false;
        }
        return true;
    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        CheckHasCardLeft();
        if (_CardList.Count <= 0)
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "No champions！");
        }
        needReplacePos = LineUpPage.CurrentSelectCardPos;
        if (CardWrapComponent == null)
        {
            CardWrapComponent = new UIWrapComponent(195, 2, 805, 5, f_GetObject("CardItemParent"), f_GetObject("CardItem"), _CardList, CardItemUpdateByInfo, null);
        }
        CardWrapComponent.f_UpdateList(_CardList);
        CardWrapComponent.f_ResetView();
    }
    #region 更新卡牌item
    /// <summary>
    /// 更新卡牌（代码部分来源于卡牌背包）
    /// </summary>
    private void CardItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        CardPoolDT cardDT = dt as CardPoolDT;
        if (cardDT.m_iTempleteId == (int)EM_CardType.CaiWenji)
        {
            item.name = "Caiwenji";
        }
        else if (cardDT.m_iTempleteId == (int)EM_CardType.HuangYueYing)
        {
            item.name = "Huangyueying";
        }
        else if (cardDT.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
        {
            item.name = "Zhu";
        }
        else
            item.name = "0";
        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UILabel tmpQuality = item.Find("QualityTitle/Quality").GetComponent<UILabel>();
        UILabel tmpLevel = item.Find("LevelTitle/Level").GetComponent<UILabel>();
        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();
        Transform tStarParent = item.Find("star");
        GameObject tInTeam = item.Find("InTeam").gameObject;
        int evolveLv = UITool.f_GetEvolveLv(cardDT.m_iEvolveId);
        string name = string.Empty;
        if (evolveLv > 0)
            name = string.Format("{0}+{1}", UITool.f_GetCardName(cardDT.m_CardDT), evolveLv);
        else
            name = UITool.f_GetCardName(cardDT.m_CardDT);
        tmpCase.spriteName = UITool.f_GetImporentColorName(cardDT.m_CardDT.iImportant, ref name);
        tmpName.text = name;
        tmpQuality.text = cardDT.m_CardDT.iImportant.ToString();
        tmpLevel.text = cardDT.m_iLv.ToString();
        tmpIcon.sprite2D = UITool.f_GetIconSpriteByCardId(cardDT);
        tInTeam.SetActive(Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(dt.iId));
        //if (cardDT.m_iLv >= 50)
        //{
        //    tStarParent.gameObject.SetActive(true);
        //    UISprite[] tStar = new UISprite[6];
        //    for (int i = 0; i < item.Find("star").childCount; i++)
        //        tStar[i] = tStarParent.GetChild(i).GetComponent<UISprite>();
        //    UITool.f_UpdateStarNum(tStar, cardDT.m_iLvAwaken / 10);
        //}
        //else
        tStarParent.gameObject.SetActive(false);

        f_RegClickEvent(item.Find("BtnGoBattle").gameObject, OnBtnGoBattleClick, dt);//此行代码非来源卡牌背包

        string FateHintText = "";
        int count = CheckFateCount(cardDT.m_CardDT.iId);
        if (count > 0)
        {
FateHintText += "Hiệu ứng + " + count.ToString() + "\n";//Active charm
        }

        string atackeStr = "";

        //如果阵容选择位是战宠或者援军，则不考虑合击排序。援军只看缘分
        if (LineUpPage.CurrentSelectCardPos != EM_FormationPos.eFormationPos_Reinforce &&
            LineUpPage.CurrentSelectCardPos != EM_FormationPos.eFormationPos_Pet)
        {
            if (CheckJointAttack(cardDT.m_CardDT, ref atackeStr))
            {
                FateHintText += atackeStr + "\n";
            }
        }
        bool check = RolePropertyTools.f_CheckNextLevelAuraCamp(needReplacePos, cardDT);
        if (check)
        {
FateHintText += "Vòng sáng phe + 1" + "\n";
        }

        bool check1 = RolePropertyTools.f_CheckNextLevelAuraType(needReplacePos, cardDT);
        if (check1)
        {
            FateHintText += CommonTools.f_GetTransLanguage(2314) + "\n";//"Light Circle1 + 1" + "\n";
        }
        item.Find("FateHint").GetComponent<UILabel>().text = FateHintText;
    }
    #endregion
    /// <summary>
    /// 检测是否有合体技能（1.如果有合体技能，则设置字符串并返回true）
    /// </summary>
    /// <param name="cardTempId"></param>
    /// <param name="strJoinAttack"></param>
    /// <returns></returns>
    private static bool CheckJointAttack(CardDT cardDT, ref string strJoinAttack)
    {
        EM_FormationPos needReplaceCardPos = LineUpPage.CurrentSelectCardPos;
        List<MagicDT> tmpMagic = new List<MagicDT>(UITool.f_GetCardMagic(cardDT));
        //加上已上阵的
        List<BasePoolDT<long>> aData = Data_Pool.m_TeamPool.f_GetAll();
        for (int i = 0; i < aData.Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)aData[i];
            if (tTeamPoolDT.m_eFormationPos == LineUpPage.CurrentSelectCardPos)//去除当前选中的位置
                continue;
            tmpMagic.AddRange(UITool.f_GetCardMagic(tTeamPoolDT.m_CardPoolDT.m_CardDT));
        }
        for (int i = 0; i < tmpMagic.Count; i++)
        {
            if (tmpMagic[i] != null && tmpMagic[i].iClass == 3)//合击
            {
                int iGroup1 = tmpMagic[i].iGroupHero1;
                int iGroup2 = tmpMagic[i].iGroupHero2;
                int iGroup3 = tmpMagic[i].iGroupHero3;
                if (iGroup1 <= 0 && iGroup2 <= 0 && iGroup3 <= 0)//都为0表示没有合击
                {
                    continue;
                }
                if (iGroup1 != cardDT.iId && iGroup2 != cardDT.iId && iGroup3 != cardDT.iId)//当前卡牌必须要在里面
                {
                    continue;
                }
                if (iGroup1 > 0 && iGroup1 != cardDT.iId && !checkCardTempIdIsLineUp(iGroup1))
                {
                    continue;
                }
                if (iGroup2 > 0 && iGroup2 != cardDT.iId && !checkCardTempIdIsLineUp(iGroup2))
                {
                    continue;
                }
                if (iGroup3 > 0 && iGroup3 != cardDT.iId && !checkCardTempIdIsLineUp(iGroup3))
                {
                    continue;
                }
                string Str1 = (iGroup1 > 0 && iGroup1 != cardDT.iId) ? (glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(iGroup1) as CardDT).szName : "";
                string Str2 = (iGroup2 > 0 && iGroup2 != cardDT.iId) ? (glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(iGroup2) as CardDT).szName : "";
                string Str3 = (iGroup3 > 0 && iGroup3 != cardDT.iId) ? (glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(iGroup3) as CardDT).szName : "";
                if (Str3 != "" && Str2 != "")
                    Str2 += "、";
                if (Str2 != "" && Str1 != "")
                    Str1 += "、";
                if (Str3 != "" && Str2 == "" && Str1 != "")
                    Str1 += "、";
                strJoinAttack = "" + Str1 + Str2 + Str3 + "kết hợp";//"combination";
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 获取阵容里跟卡牌相关的缘分，且去除被更换的位置的缘分id列表
    /// </summary>
    /// <returns></returns>
    private static List<int> getCardFateIdArray()
    {
        List<int> listFateDataId = new List<int>();
        List<BasePoolDT<long>> aData = Data_Pool.m_TeamPool.f_GetAll();
        for (int i = 0; i < aData.Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)aData[i];
            if (tTeamPoolDT.m_eFormationPos == LineUpPage.CurrentSelectCardPos)//去除当前选中的位置
                continue;
            for (int j = 0; j < tTeamPoolDT.m_CardPoolDT.m_CardFatePoolDT.m_aFateList.Count; j++)
            {
                int fateId = tTeamPoolDT.m_CardPoolDT.m_CardFatePoolDT.m_aFateList[j] == null ? -1 : tTeamPoolDT.m_CardPoolDT.m_CardFatePoolDT.m_aFateList[j].iId;
                if (fateId != -1 && tTeamPoolDT.m_CardPoolDT.m_CardFatePoolDT.m_aFateList[j].iGoodsType == (int)EM_ResourceType.Card &&
                    !listFateDataId.Contains(fateId))
                {
                    listFateDataId.Add(fateId);
                }
            }
        }
        return listFateDataId;
    }
    /// <summary>
    /// 检测该卡牌id是否已经上阵
    /// </summary>
    private static bool checkCardTempIdIsLineUp(int cardTempId)
    {
        List<BasePoolDT<long>> aData = Data_Pool.m_TeamPool.f_GetAll();
        for (int i = 0; i < aData.Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)aData[i];
            if (tTeamPoolDT.m_eFormationPos == LineUpPage.CurrentSelectCardPos)//去除当前选中的位置
                continue;
            if (tTeamPoolDT.m_CardPoolDT.m_CardDT.iId == cardTempId)
                return true;
        }
        IEnumerator<EM_ReinforcePos> listData = Data_Pool.m_TeamPool.dicReinforceCardId.Keys.GetEnumerator();
        while (listData.MoveNext())
        {
            CardPoolDT CurrentCardPoolDT = Data_Pool.m_TeamPool.dicReinforceCardId[listData.Current];
            if (CurrentCardPoolDT != null)
            {
                if (LineUpPage.CurrentSelectCardPos == EM_FormationPos.eFormationPos_Reinforce && listData.Current == LineUpPage.CurrentSelectReinforcePos)
                {
                    continue;
                }
                if (CurrentCardPoolDT.m_CardDT.iId == cardTempId)
                    return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 检查装备此卡牌后可激活的缘分数量
    /// </summary>
    /// <returns></returns>
    private static int CheckFateCount(int cardTempId)
    {
        int count = 0;
        EM_FormationPos needReplaceCardPos = LineUpPage.CurrentSelectCardPos;
        List<int> listFateDataId = getCardFateIdArray();

        for (int i = 0; i < listFateDataId.Count; i++)
        {
            int fateDataId = listFateDataId[i];
            CardFateDataDT cardFateDataDT = glo_Main.GetInstance().m_SC_Pool.m_CardFateDataSC.f_GetSC(fateDataId) as CardFateDataDT;
            string[] cardIdArray = cardFateDataDT.szGoodsId.Split(';');
            bool isAllLineUp = true;
            bool isInFate = false;
            for (int a = 0; a < cardIdArray.Length; a++)//检测该缘分是否所有卡牌已上阵
            {
                int cardId = int.Parse(cardIdArray[a]);
                if (cardId > 0 && !checkCardTempIdIsLineUp(cardId) && cardTempId != cardId)
                {
                    isAllLineUp = false;
                    break;
                }
                if (cardId == cardTempId)
                    isInFate = true;
            }
            if (isAllLineUp && isInFate)
                count++;
        }
        return count;
    }
    /// <summary>
    /// 移除卡牌列表中的指定卡牌id(模板id)的卡牌
    /// </summary>
    /// <param name="listCard"></param>
    /// <param name="needRemoveCardTempId"></param>
    private static void RemoveCardByTempId(List<BasePoolDT<long>> listCard, int needRemoveCardTempId)
    {
        for (int index = listCard.Count - 1; index >= 0; index--)
        {
            CardPoolDT cardPoolDT = listCard[index] as CardPoolDT;
            if (cardPoolDT.m_CardDT.iId == needRemoveCardTempId)
            {
                listCard.RemoveAt(index);
            }
        }
    }
    /// <summary>
    /// 移除卡牌列表中的指定卡牌id(非模板id)的卡牌
    /// </summary>
    /// <param name="listCard"></param>
    /// <param name="needRemoveCardId"></param>
    private static void RemoveCardById(List<BasePoolDT<long>> listCard, long needRemoveCardId)
    {
        for (int index = listCard.Count - 1; index >= 0; index--)
        {
            CardPoolDT cardPoolDT = listCard[index] as CardPoolDT;
            if (cardPoolDT.iId == needRemoveCardId)
            {
                listCard.RemoveAt(index);
                break;
            }
        }
    }
    /// <summary>
    /// 初始化事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlack", OnBtnBlackClick);
        OnChangeTeamCardCallback.m_ccCallbackSuc = OnChangeTeamCardSuccess;
        OnChangeTeamCardCallback.m_ccCallbackFail = OnChangeTeamCardFail;
        OnChangeReinforceCallback.m_ccCallbackSuc = OnChangeReinforceSuccess;
        OnChangeReinforceCallback.m_ccCallbackFail = OnChangeReinforceFail;
    }
    #region 按钮事件
    /// <summary>
    /// 点击界面黑色背景关闭界面
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectCardPage, UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
    }
    /// <summary>
    /// 点击item里面上阵按钮
    /// </summary>
    private void OnBtnGoBattleClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        CardPoolDT cardPoolDT = obj1 as CardPoolDT;
        UITool.f_OpenOrCloseWaitTip(true);
        //如果阵容选择位是战宠或者援军，则不考虑合击排序。援军只看缘分
        if (LineUpPage.CurrentSelectCardPos == EM_FormationPos.eFormationPos_Pet)//宠物不做处理
        {
        }
        else if (LineUpPage.CurrentSelectCardPos == EM_FormationPos.eFormationPos_Reinforce)//援军上阵
        {
            Data_Pool.m_TeamPool.f_ChangeReinforce(LineUpPage.CurrentSelectReinforcePos, cardPoolDT.iId, OnChangeReinforceCallback);
        }
        else//阵容上阵
        {
            CardPoolDT cardPoolDT_old = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(needReplacePos);
            // check vị trí nó đang đứng (0-19)
            EM_CloseArrayPos posIndex = Data_Pool.m_ClosethArrayData.GetPosIndex(needReplacePos);
            
            if ((posIndex >= EM_CloseArrayPos.eCloseArray_PosOne && posIndex <= EM_CloseArrayPos.eCloseArray_PosFive) && cardPoolDT.m_CardDT.iCardFightType != (int)EM_CardFightType.eCardKiller)
            {
                UITool.f_OpenOrCloseWaitTip(false);
                // show thông báo
                PopupMenuParams tParams = new PopupMenuParams(CommonTools.f_GetTransLanguage(2119), CommonTools.f_GetTransLanguage(2343), CommonTools.f_GetTransLanguage(1400), f_OnGo, CommonTools.f_GetTransLanguage(2124));
                //PopupMenuParams tParams = new PopupMenuParams("Thông báo", "Đây là kích tướng cần đổi đội hình trước.", "Đến", f_OnGo, "Bỏ qua");
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParams);
                return;
            }
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_TeamPool.f_ChangeTeamCard((byte)needReplacePos, cardPoolDT.iId, OnChangeTeamCardCallback);
        }
    }
    private void f_OnGo(object value)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }
    
    #endregion
    private void OnChangeReinforceSuccess(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Đã chọn thành công!");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectCardPage, UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ClothArrayChanged);

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    private void OnChangeReinforceFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Chọn không thành công，" + ((eMsgOperateResult)obj).ToString());
    }
    private void OnChangeTeamCardSuccess(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        Data_Pool.m_GuidancePool.m_OtherSave = true;
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Đã chọn thành công!");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectCardPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LineUpPage, UIMessageDef.UI_OPEN);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FATE_TRIP);
        ClothArrayPage.CheckAndCommitClothArray();

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    private void OnChangeTeamCardFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Chọn không thành công，" + ((eMsgOperateResult)obj).ToString());
    }

}
