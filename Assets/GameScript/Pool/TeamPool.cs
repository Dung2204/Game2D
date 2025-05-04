using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;
using System.Collections.Generic;

public class TeamPool : BasePool
{
    //援军上阵卡牌和其位置对于关系,如果没有卡牌则为null
    public Dictionary<EM_ReinforcePos, CardPoolDT> dicReinforceCardId = new Dictionary<EM_ReinforcePos, CardPoolDT>();
    public TeamPool() : base("TeamPoolDT", true)
    {

    }


    #region Pool数据管理

    protected override void f_Init()
    {
        dicReinforceCardId.Clear();
        dicReinforceCardId.Add(EM_ReinforcePos.eReinforcePos_B1, null);
        dicReinforceCardId.Add(EM_ReinforcePos.eReinforcePos_B2, null);
        dicReinforceCardId.Add(EM_ReinforcePos.eReinforcePos_B3, null);
        dicReinforceCardId.Add(EM_ReinforcePos.eReinforcePos_B4, null);
        dicReinforceCardId.Add(EM_ReinforcePos.eReinforcePos_B5, null);
        dicReinforceCardId.Add(EM_ReinforcePos.eReinforcePos_B6, null);
    }

    protected override void RegSocketMessage()
    {
        SC_FormationInfo tSC_FormationInfo = new SC_FormationInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_FormationInfo, tSC_FormationInfo, Callback_SocketData_Update);
        SC_ReinforceInfo scReinforceInfo = new SC_ReinforceInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ReinforceInfo, scReinforceInfo, Callback_ReinforceInfo);
        //stPoolDelData tstPoolDelData = new stPoolDelData();
        //GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CardRemove, tstPoolDelData, Callback_Del);
    }
    /// <summary>
    /// 援军信息
    /// </summary>
    /// <param name="data"></param>
    public void Callback_ReinforceInfo(object data)
    {
        //MessageBox.ASSERT("TsuLog TeamPool Callback_ReinforceInfo");
        dicReinforceCardId.Clear();
        SC_ReinforceInfo scReinforceInfo = (SC_ReinforceInfo)data;
        for(int i = 0; i < scReinforceInfo.cardId.Length; i++)
        {

            CardPoolDT tCardPoolDT = Data_Pool.m_CardPool.f_GetForId(scReinforceInfo.cardId[i]) as CardPoolDT;
            dicReinforceCardId.Add((EM_ReinforcePos)i, tCardPoolDT);
        }
        //更新缘分
        List<BasePoolDT<long>> aData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < aData.Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)aData[i];
            f_UpdateCardFate(tTeamPoolDT);
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
       // MessageBox.ASSERT("TsuLog TeamPool f_Socket_AddData");
        SC_FormationInfo tServerData = (SC_FormationInfo)Obj;
        TeamPoolDT tPoolDataDT = new TeamPoolDT();

        tPoolDataDT.iId = (int)tServerData.eFormationPos;
        tPoolDataDT.m_iCardId = tServerData.cardId;
        tPoolDataDT.m_eFormationPos = (EM_FormationPos)tServerData.eFormationPos;
        tPoolDataDT.m_eFormationSlot = (EM_FormationSlot)tServerData.eFormationSlot;
        Array.Copy(tServerData.equipId, tPoolDataDT.m_aEqupId, tServerData.equipId.Length);
        for(int i = 0; i < tServerData.equipId.Length; i++)
        {
            EM_EquipPart equipPart = (EM_EquipPart)(i + 1);
            if (equipPart == EM_EquipPart.eEquipPare_MagicLeft || equipPart == EM_EquipPart.eEquipPare_MagicRight)
            {
                tPoolDataDT.m_aTreamPoolDT[(int)equipPart - 5] = Data_Pool.m_TreasurePool.f_GetForId(tServerData.equipId[i]) as TreasurePoolDT;
            }
            else if (equipPart == EM_EquipPart.eEquipPart_GodWeapon)//thêm vào pool card đã mặc
            {
                tPoolDataDT.m_aGodEquipPoolDT[(int)equipPart - 7] = Data_Pool.m_GodEquipPool.f_GetForId(tServerData.equipId[i]) as GodEquipPoolDT;
            }
            else
            {
                tPoolDataDT.m_aEquipPoolDT[(int)equipPart - 1] = Data_Pool.m_EquipPool.f_GetForId(tServerData.equipId[i]) as EquipPoolDT;
            }
        }
        f_UpdateCardFate(tPoolDataDT);
        f_Save(tPoolDataDT);
        f_AchievementTaskUpdateProgressEvent();
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
       // MessageBox.ASSERT("TsuLog TeamPool f_Socket_AddData");
        SC_FormationInfo tServerData = (SC_FormationInfo)Obj;
        TeamPoolDT tPoolDataDT = (TeamPoolDT)f_GetForId((int)tServerData.eFormationPos);
        if(tPoolDataDT == null)
        {
MessageBox.DEBUG("No BaseGoods data，add");
            f_Socket_AddData(Obj, true);
            return;
        }
        tPoolDataDT.m_iCardId = tServerData.cardId;
        tPoolDataDT.m_eFormationPos = (EM_FormationPos)tServerData.eFormationPos;
        tPoolDataDT.m_eFormationSlot = (EM_FormationSlot)tServerData.eFormationSlot;
        Array.Copy(tServerData.equipId, tPoolDataDT.m_aEqupId, tServerData.equipId.Length);
        for(int i = 0; i < tServerData.equipId.Length; i++)
        {
            EM_EquipPart equipPart = (EM_EquipPart)(i + 1);
            if(equipPart == EM_EquipPart.eEquipPare_MagicLeft || equipPart == EM_EquipPart.eEquipPare_MagicRight)
            {
                tPoolDataDT.m_aTreamPoolDT[(int)equipPart - 5] = Data_Pool.m_TreasurePool.f_GetForId(tServerData.equipId[i]) as TreasurePoolDT;
            }
            else if (equipPart == EM_EquipPart.eEquipPart_GodWeapon)//thêm vào pool card đã mặc
            {
                tPoolDataDT.m_aGodEquipPoolDT[(int)equipPart - 7] = Data_Pool.m_GodEquipPool.f_GetForId(tServerData.equipId[i]) as GodEquipPoolDT;
            }
            else
            {
                tPoolDataDT.m_aEquipPoolDT[(int)equipPart - 1] = Data_Pool.m_EquipPool.f_GetForId(tServerData.equipId[i]) as EquipPoolDT;
            }
        }
        f_UpdateCardFate(tPoolDataDT);
        f_AchievementTaskUpdateProgressEvent();
    }

    protected void Callback_Del(object Obj)
    {
        //stPoolDelData tServerData = (stPoolDelData)Obj;
        //f_Delete(tServerData.iId);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //以下为外部调用接口

    #region 红点提示
    /// <summary>
    /// 检查主界面阵容按钮是否有红点
    /// </summary>
    /// <returns></returns>
    public bool f_CheckRedPoint()
    {
        if(f_CheckForceRedPoint())
            return true;
        for(int i = 0; i <= (int)EM_FormationPos.eFormationPos_Assist6; i++)
        {
            bool redPoint = f_CheckTeamPoolDTRedPoint((TeamPoolDT)f_GetForId(i), (EM_FormationPos)i);
            if(redPoint)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 检查该援军位置是否开启
    /// </summary>
    /// <param name="pos">援军位置</param>
    /// <returns></returns>
    private bool CheckForceItemIsOpen(EM_ReinforcePos pos)
    {
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        int openLevel = 30;
        switch(pos)
        {
            case EM_ReinforcePos.eReinforcePos_B1: openLevel = 30; break;
            case EM_ReinforcePos.eReinforcePos_B2: openLevel = 35; break;
            case EM_ReinforcePos.eReinforcePos_B3: openLevel = 40; break;
            case EM_ReinforcePos.eReinforcePos_B4: openLevel = 45; break;
            case EM_ReinforcePos.eReinforcePos_B5: openLevel = 50; break;
            case EM_ReinforcePos.eReinforcePos_B6: openLevel = 55; break;
        }
        return playerLevel >= openLevel;
    }
    /// <summary>
    /// 检查是否有援军位已经开启且可上阵英雄
    /// </summary>
    /// <returns></returns>
    public bool f_CheckForceRedPoint()
    {
        bool HasCardLeft = Data_Pool.m_CardPool.f_CheckHasCardLeft();
        if(CheckForceItemIsOpen(EM_ReinforcePos.eReinforcePos_B1) && Data_Pool.m_TeamPool.dicReinforceCardId[EM_ReinforcePos.eReinforcePos_B1] == null && HasCardLeft)
            return true;
        if(CheckForceItemIsOpen(EM_ReinforcePos.eReinforcePos_B2) && Data_Pool.m_TeamPool.dicReinforceCardId[EM_ReinforcePos.eReinforcePos_B2] == null && HasCardLeft)
            return true;
        if(CheckForceItemIsOpen(EM_ReinforcePos.eReinforcePos_B3) && Data_Pool.m_TeamPool.dicReinforceCardId[EM_ReinforcePos.eReinforcePos_B3] == null && HasCardLeft)
            return true;
        if(CheckForceItemIsOpen(EM_ReinforcePos.eReinforcePos_B4) && Data_Pool.m_TeamPool.dicReinforceCardId[EM_ReinforcePos.eReinforcePos_B4] == null && HasCardLeft)
            return true;
        if(CheckForceItemIsOpen(EM_ReinforcePos.eReinforcePos_B5) && Data_Pool.m_TeamPool.dicReinforceCardId[EM_ReinforcePos.eReinforcePos_B5] == null && HasCardLeft)
            return true;
        if(CheckForceItemIsOpen(EM_ReinforcePos.eReinforcePos_B6) && Data_Pool.m_TeamPool.dicReinforceCardId[EM_ReinforcePos.eReinforcePos_B6] == null && HasCardLeft)
            return true;
        return false;
    }
    /// <summary>
    /// 检查阵容红点
    /// </summary>
    /// <param name="teamPoolDT"></param>
    /// <returns></returns>
    public bool f_CheckTeamPoolDTRedPoint(TeamPoolDT teamPoolDT, EM_FormationPos pos)
    {
        TeamPoolDT tTeamPoolDT = teamPoolDT;
        if(teamPoolDT == null)//空位置，可上阵
        {
            bool HasHero = Data_Pool.m_CardPool.f_CheckHasCardLeft();
            int openLevel = UITool.f_GetTeamPosOpenLevel(pos);
            int Lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            bool isOpen = Lv >= openLevel;
            return HasHero && isOpen;
        }
        bool CardCanLvUp = false;
        bool CardCanEnvolve = false;
        f_CheckTeamCardRedPoint(tTeamPoolDT.m_CardPoolDT, ref CardCanLvUp, ref CardCanEnvolve);
        if(CardCanLvUp || CardCanEnvolve)
        {
            return true;
        }
        bool EquipCanLvUp = false;
        bool EquipCanEquip = false;
        f_CheckTeamEquipRedPoint(tTeamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Weapon), EM_EquipPart.eEquipPart_Weapon, ref EquipCanLvUp, ref EquipCanEquip);
        if(EquipCanLvUp || EquipCanEquip)
            return true;
        f_CheckTeamEquipRedPoint(tTeamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Armour), EM_EquipPart.eEquipPart_Armour, ref EquipCanLvUp, ref EquipCanEquip);
        if(EquipCanLvUp || EquipCanEquip)
            return true;
        f_CheckTeamEquipRedPoint(tTeamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Helmet), EM_EquipPart.eEquipPart_Helmet, ref EquipCanLvUp, ref EquipCanEquip);
        if(EquipCanLvUp || EquipCanEquip)
            return true;
        f_CheckTeamEquipRedPoint(tTeamPoolDT.f_GetEquipPoolDT(EM_Equip.eEquipPart_Belt), EM_EquipPart.eEquipPart_Belt, ref EquipCanLvUp, ref EquipCanEquip);
        if(EquipCanLvUp || EquipCanEquip)
            return true;
        bool TreasureCanLvUp = false;
        bool TreasureCanEquip = false;
        bool TreasureCanRefine = false;
        f_CheckTeamTreasureRedPoint(tTeamPoolDT.f_GetTreasurePoolDT(EM_Treasure.eEquipPare_MagicLeft), EM_EquipPart.eEquipPare_MagicLeft, ref TreasureCanLvUp, ref TreasureCanEquip,ref TreasureCanRefine);
        if(TreasureCanLvUp || TreasureCanEquip)
            return true;
        f_CheckTeamTreasureRedPoint(tTeamPoolDT.f_GetTreasurePoolDT(EM_Treasure.eEquipPare_MagicRight), EM_EquipPart.eEquipPare_MagicRight, ref TreasureCanLvUp, ref TreasureCanEquip,ref TreasureCanRefine);
        if(TreasureCanLvUp || TreasureCanEquip)
            return true;
        return false;
    }
    /// <summary>
    /// 检查阵容装备是否可升级(有新装备可装备或有高品质可更换)
    /// </summary>
    /// <param name="equipPoolDt">装备</param>
    /// <param name="CanLvUp">有装备且可以升级</param>
    /// <param name="CanEquip">无装备有新装备或更高品质</param>
    public void f_CheckTeamEquipRedPoint(EquipPoolDT equipPoolDt, EM_EquipPart equipPart, ref bool CanLvUp, ref bool CanEquip)
    {
        if(equipPoolDt == null)
        {
            CanLvUp = false;
            CanEquip = UITool.f_CheckHasEquipLeft(equipPart);
            return;
        }
        if(equipPoolDt.m_lvIntensify < UITool.f_GetEquipIntenMax())
        {
            CanLvUp = true;
            if(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money) < UITool.f_GetEquipIntenCon(equipPoolDt, 1))
            {
                CanLvUp = false;
            }
        }
        else
        {
            CanLvUp = false;
        }
        CanEquip = UITool.f_CheckHasEquipHighColorLeft(equipPart, equipPoolDt.m_EquipDT.iColour);
    }
    /// <summary>
    /// 检查阵容法宝是否可升级(有新装备可装备或有高品质可更换)
    /// </summary>
    /// <param name="treasurePoolDT">法宝pool</param>
    /// <param name="treasurePart"></param>
    /// <param name="CanLvUp"></param>
    /// <param name="CanEquip">无装备有新装备或更高品质</param>
    public void f_CheckTeamTreasureRedPoint(TreasurePoolDT treasurePoolDT, EM_EquipPart treasurePart, ref bool CanLvUp, ref bool CanEquip,ref bool CanRefine)
    {
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if(playerLevel < UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenTreasureLevel))
        {
            CanLvUp = false;
            CanEquip = false;
            return;
        }
        if (treasurePoolDT == null)
        {
            CanLvUp = false;
            CanEquip = UITool.f_CheckHasEquipLeft(treasurePart);
            return;
        }

        if (Data_Pool.m_TreasurePool.f_CheckTreasureRefin(treasurePoolDT)) {
            CanRefine = true;
        }
        
        if (treasurePoolDT.m_lvIntensify <= UITool.f_GetTreasureIntenManx(treasurePoolDT))
        {
            if (Data_Pool.m_TreasurePool.f_CheckTreasureUpLevel(treasurePoolDT))
            {
                CanLvUp = true;
            }
        }
        CanEquip = UITool.f_CheckHasEquipHighColorLeft(treasurePart, treasurePoolDT.m_TreasureDT.iImportant);
    }
    /// <summary>
    /// 检测阵容卡牌红点，是否可升级/可进阶
    /// </summary>
    /// <param name="cardPoolDT">卡牌</param>
    /// <param name="CanLvUp">是否可升级</param>
    /// <param name="CanLvUp">是否可进阶</param>
    public void f_CheckTeamCardRedPoint(CardPoolDT cardPoolDT, ref bool CanLvUp, ref bool CanEnvolve)
    {
        CanLvUp = f_CheckTeamCardIsCanLvUp(cardPoolDT);
        CanEnvolve = f_CheckTeamCardIsCanEnvolve(cardPoolDT);
    }
    #region 检测卡牌是否可升级/可进阶
    /// <summary>
    /// 获取卡牌下一级所需要的经验值
    /// </summary>
    /// <param name="_Card"></param>
    /// <returns></returns>
    private int CountNeedExp(CardPoolDT _Card)
    {
        int Exp = 0;
        CarLvUpDT CardLv = (CarLvUpDT)glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(_Card.m_iLv + 1);
        switch(_Card.m_CardDT.iImportant)
        {
            case 1:
                Exp = CardLv.iWhiteCard - _Card.m_iExp;
                break;
            case 2:
                Exp = CardLv.iGreenCard - _Card.m_iExp;
                break;
            case 3:
                Exp = CardLv.iBlueCard - _Card.m_iExp;
                break;
            case 4:
                Exp = CardLv.iPurpleCard - _Card.m_iExp;
                break;
            case 5:
                Exp = CardLv.iOragenCard - _Card.m_iExp;
                break;
            case 6:
                Exp = CardLv.iRedCard - _Card.m_iExp;
                break;
            //Tsucode - tuong kim
            case 7:
                Exp = CardLv.iGoldCard - _Card.m_iExp;
                break;
            //------------------------------
            default:
                break;
        }
        return Exp;
    }
    /// <summary>
    /// 检查卡牌是否可升级
    /// </summary>
    /// <param name="cardPoolDT"></param>
    /// <param name="CanLvUp"></param>
    /// <returns></returns>
    private bool f_CheckTeamCardIsCanLvUp(CardPoolDT cardPoolDT)
    {
        if(cardPoolDT == null)
        {
            return false;
        }
        CardPoolDT MainCardPoolDT = f_GetFormationPosCardPoolDT(EM_FormationPos.eFormationPos_Main);//主卡
        if(MainCardPoolDT.iId == cardPoolDT.iId)
        {
            return false;
        }
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if(cardPoolDT.m_iLv >= playerLevel)//1.等级小于主卡
        {
            return false;
        }
        int NeedGold = 0;
        int NeedExp = CountNeedExp(cardPoolDT);
        List<BasePoolDT<long>> GoodsPool = Data_Pool.m_BaseGoodsPool.f_GetAll();
        GoodsPool.Sort((BasePoolDT<long> a, BasePoolDT<long> b) => {
            return ((BaseGoodsPoolDT)a).m_BaseGoodsDT.iPriority >= ((BaseGoodsPoolDT)b).m_BaseGoodsDT.iPriority ? -1 : 1;
        });
        BaseGoodsPoolDT tGoods = new BaseGoodsPoolDT();
        bool isEnough = false;
        for(int count = GoodsPool.Count - 1; count >= 0; count--)
        {
            tGoods = (BaseGoodsPoolDT)GoodsPool[count];
            //判断道具是否为经验石
            if((tGoods.m_BaseGoodsDT.iEffect) == (int)EM_GoodsEffect.CardExp)
            {
                if(tGoods.m_BaseGoodsDT.iEffectData <= 0)
                {
                    continue;
                }
                int needNum = NeedExp / tGoods.m_BaseGoodsDT.iEffectData;
                int leftNum = NeedExp % tGoods.m_BaseGoodsDT.iEffectData;
                needNum = leftNum == 0 ? needNum : (needNum + 1);
                if(tGoods.m_iNum >= needNum)//2经验石足够
                {
                    NeedGold += (tGoods).m_BaseGoodsDT.iEffectData * needNum;
                    isEnough = true;
                    break;
                }
                else
                {
                    NeedExp -= (tGoods).m_BaseGoodsDT.iEffectData * tGoods.m_iNum;
                    NeedGold += (tGoods).m_BaseGoodsDT.iEffectData;
                }
            }
        }
        if(isEnough && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money) > NeedGold) //3.银币足够，
        {
            return true;
        }
        else
            return false;
    }
    /// <summary>
    /// 检查卡牌是否可进阶
    /// </summary>
    /// <param name="cardPoolDT"></param>
    /// <returns></returns>
    private bool f_CheckTeamCardIsCanEnvolve(CardPoolDT cardPoolDT)
    {
        if(cardPoolDT == null)
        {
            return false;
        }
        CardEvolveDT tCardEvoDT;
        tCardEvoDT = glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.f_GetSC(cardPoolDT.m_CardDT.iId * 100 + (cardPoolDT.m_iEvolveLv + 1)) as CardEvolveDT;

        if(tCardEvoDT == null)
        {
            return false;
        }
        //银币不足
        if(Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Money) < tCardEvoDT.iMoney)
        {
            return false;
        }//进阶丹不足
        else if(Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(100) < tCardEvoDT.iEvolvePill)
        {
            return false;
        }//卡牌数量不足
        else if(Data_Pool.m_CardPool.f_GetHaveNumByTemplate(cardPoolDT.m_CardDT.iId) - 1 < tCardEvoDT.iNeedCardNum)
        {
            return false;
        }//等级不足
        else if(cardPoolDT.m_iLv < tCardEvoDT.iNeedLv)
        {
            return false;
        }
        return true;
    }
    #endregion
    #endregion

    /// <summary>
    /// 获取相应位置对应的卡牌CardPoolDT
    /// </summary>
    /// <param name="tEM_FormationPos"></param>
    /// <returns></returns>
    public CardPoolDT f_GetFormationPosCardPoolDT(EM_FormationPos tEM_FormationPos)
    {
        List<BasePoolDT<long>> aData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < aData.Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)aData[i];
            if(tTeamPoolDT.m_eFormationPos == tEM_FormationPos)
            {
                return tTeamPoolDT.m_CardPoolDT;
            }
        }
        return null;
    }

    /// <summary>
    /// 阵型更换卡牌
    /// </summary>
    /// <param name="cardPos">位置（从0开始，但只能1-7能换）</param>
    /// <param name="tSocketCallbackDT">卡牌位置</param>
    public void f_ChangeTeamCard(byte cardPos, long iNewCardId, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FormationChangeCard, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(cardPos);
        tCreateSocketBuf.f_Add(iNewCardId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FormationChangeCard, bBuf);
    }

    /// <summary>
    /// 阵型更换装备
    /// </summary>
    /// <param name="cardPos">装备主卡位置（从0开始，但只能1-7能换）</param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_ChangeTeamEquip(long cardPos, long iEquipId, byte equipPos, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FormationChangeEquip, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)cardPos);
        tCreateSocketBuf.f_Add(iEquipId);
        tCreateSocketBuf.f_Add(equipPos);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FormationChangeEquip, bBuf);
    }

    /// <summary>
    /// 阵型更换法宝
    /// </summary>
    public void f_ChangeTeamTreasure(long cardPos, long iTreamsureId, byte TreamsurePos, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FormationChangeTreasure, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)cardPos);
        tCreateSocketBuf.f_Add(iTreamsureId);
        tCreateSocketBuf.f_Add(TreamsurePos);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FormationChangeTreasure, bBuf);
    }

    /// <summary>
    /// 一键穿戴装备
    /// </summary>
    /// <param name="eFormationPos"></param>
    /// <param name="equipId"></param>
    /// <param name="equipPos"></param>
    /// <param name="treasureId"></param>
    /// <param name="treasurePos"></param>
    public void f_ChangeEquipOneKey(byte eFormationPos,long[] equipId,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FormationChangeEquipOneKey, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(eFormationPos);
        for (int i = 0; i < equipId.Length; i++) {
            tCreateSocketBuf.f_Add(equipId[i]);
        }        
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        UITool.f_OpenOrCloseWaitTip(true);
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FormationChangeEquipOneKey, bBuf);
    }

    /// <summary>
    /// 援军上阵卡牌
    /// </summary>
    public void f_ChangeReinforce(EM_ReinforcePos reinforcePos, long iNewCardId, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ReinforceFight, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)reinforcePos);
        tCreateSocketBuf.f_Add(iNewCardId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ReinforceFight, bBuf);
    }

    /// <summary>
    /// 获取总战斗力
    /// </summary>
    public int f_GetTotalBattlePower()
    {
       // MessageBox.ASSERT("TsuLog TeamPool f_GetToTalBattlePower");
        List<BasePoolDT<long>> TeamBasePoolDTArray = f_GetAll();
        int tBattleValue = 0;
        for(int i = 0; i < TeamBasePoolDTArray.Count; i++)
        {
            TeamPoolDT teamPoolDT = TeamBasePoolDTArray[i] as TeamPoolDT;
            //TsuCode
            Data_Pool.m_TeamPool.f_UpdateCardFate(teamPoolDT);
            //
            List<EquipPoolDT> listEquipPoolDT = new List<EquipPoolDT>();
            List<TreasurePoolDT> listTreasurePoolDT = new List<TreasurePoolDT>();
            List<GodEquipPoolDT> listGodEquipPoolDT = new List<GodEquipPoolDT>();

            for (int j = 0; j < teamPoolDT.m_aEquipPoolDT.Length; j++)
            {
                if(teamPoolDT.m_aEquipPoolDT[j] != null)
                    listEquipPoolDT.Add(teamPoolDT.m_aEquipPoolDT[j]);
            }
            for(int k = 0; k < teamPoolDT.m_aTreamPoolDT.Length; k++)
            {
                if(teamPoolDT.m_aTreamPoolDT[k] != null)
                    listTreasurePoolDT.Add(teamPoolDT.m_aTreamPoolDT[k]);
            }
            for (int k = 0; k < teamPoolDT.m_aGodEquipPoolDT.Length; k++)
            {
                if (teamPoolDT.m_aGodEquipPoolDT[k] != null)
                    listGodEquipPoolDT.Add(teamPoolDT.m_aGodEquipPoolDT[k]);
            }
            RoleProperty roleProperty = RolePropertyTools.f_Disp(teamPoolDT.m_CardPoolDT, listEquipPoolDT, Data_Pool.m_BattleFormPool.iDestinyProgress, listTreasurePoolDT, listGodEquipPoolDT);
            //tBattleValue += RolePropertyTools.f_GetBattlePower(roleProperty); //TsuComment
            //TsuCode
          
            int battlePowerOfCard = RolePropertyTools.f_GetBattlePower(roleProperty);
            //MessageBox.ASSERT("TsuLog TeamPool f_GetTotal... " + teamPoolDT.m_CardPoolDT.m_iTempleteId + "-LC:" + battlePowerOfCard);
            tBattleValue += battlePowerOfCard;
            
            //
        }
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress, new int[] { (int)EM_AchievementTaskCondition.eAchv_Power, tBattleValue });
        //MessageBox.ASSERT("TsuLog TeamPool f_GetToTal.... TongLC: " + tBattleValue);
        return tBattleValue;
    }
    /// <summary>
    /// 计算阵容某个位置的全属性，如果该位置没有装备卡牌，则返回null
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public RoleProperty f_GetCardAllProperty(EM_FormationPos pos)
    {
        CardPoolDT cardPool = f_GetFormationPosCardPoolDT(pos);
        if(cardPool != null)
            return f_GetCardAllProperty(cardPool.iId);
        else
            return null;
    }
    public RoleProperty f_GetCardAllProperty(CardPoolDT cardPoolDT)
    {
        List<BasePoolDT<long>> TeamBasePoolDTArray = f_GetAll();
        //RoleProperty allRoleProperty = new RoleProperty();
        //allRoleProperty.f_Reset();
        List<EquipPoolDT> listEquipPoolDT = new List<EquipPoolDT>();
        List<TreasurePoolDT> listTreasurePoolDT = new List<TreasurePoolDT>();
        for(int i = 0; i < TeamBasePoolDTArray.Count; i++)
        {
            TeamPoolDT teamPoolDT = TeamBasePoolDTArray[i] as TeamPoolDT;
            if(cardPoolDT.iId == teamPoolDT.m_iCardId)
            {
                for(int j = 0; j < teamPoolDT.m_aEquipPoolDT.Length; j++)
                {
                    if(teamPoolDT.m_aEquipPoolDT[j] != null)
                        listEquipPoolDT.Add(teamPoolDT.m_aEquipPoolDT[j]);
                }
                for(int k = 0; k < teamPoolDT.m_aTreamPoolDT.Length; k++)
                {
                    if(teamPoolDT.m_aTreamPoolDT[k] != null)
                        listTreasurePoolDT.Add(teamPoolDT.m_aTreamPoolDT[k]);
                }
                break;
            }
        }
        return RolePropertyTools.f_Disp(cardPoolDT, listEquipPoolDT, Data_Pool.m_BattleFormPool.iDestinyProgress, listTreasurePoolDT);
    }
    /// <summary>
    /// 计算某个阵容位置的全属性(包含装备的的属性，如果卡牌不在上阵位则返回该卡牌的属性)
    /// </summary>
    /// <returns></returns>
    public RoleProperty f_GetCardAllProperty(long cardID)
    {
        return f_GetCardAllProperty(Data_Pool.m_CardPool.f_GetForId(cardID) as CardPoolDT);
    }
    /// <summary>
    /// 检查某个卡牌是否上阵
    /// </summary>
    public bool f_CheckInTeamByKeyId(long keyId)
    {
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)tData[i];
            if(tTeamPoolDT.m_iCardId == keyId)
                return true;
        }
        return false;
    }

    public bool f_CheckInTeamByKeyId(int iTempId)
    {
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)tData[i];
            if(tTeamPoolDT.m_CardPoolDT.m_iTempleteId == iTempId)
                return true;
        }
        return false;
    }

    public bool f_CheckReinforceCardForTempId(int iTempId)
    {
        IEnumerator<CardPoolDT> listData = dicReinforceCardId.Values.GetEnumerator();
        while(listData.MoveNext())
        {
            if(listData.Current != null && listData.Current.m_CardDT.iId == iTempId)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 检查某个角色某种法宝是否已被穿戴
    /// </summary>
    public bool f_CheckInTreamByKeyId(int cardId, int keyId)
    {
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for (int i = 0; i < tData.Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)tData[i];
            if (tTeamPoolDT.m_CardPoolDT.m_iTempleteId != cardId) continue;
            for (int a = 0; a < tTeamPoolDT.m_aTreamPoolDT.Length; a++)
            {
                if (tTeamPoolDT.m_aTreamPoolDT[a] != null && tTeamPoolDT.m_aTreamPoolDT[a].m_iTempleteId == keyId)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 检查某个角色某种装备是否已被穿戴
    /// </summary>
    public bool f_CheckInEquipByKeyId(int cardId, int keyId)
    {
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)tData[i];
            if(tTeamPoolDT.m_CardPoolDT.m_iTempleteId != cardId) continue;
            for(int a = 0; a < tTeamPoolDT.m_aEquipPoolDT.Length; a++)
            {
                if(tTeamPoolDT.m_aEquipPoolDT[a] != null && tTeamPoolDT.m_aEquipPoolDT[a].m_iTempleteId == keyId)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 成就任务次数更新事件
    /// </summary>
    private void f_AchievementTaskUpdateProgressEvent()
    {
        //等级信息
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
            new int[] { (int)EM_AchievementTaskCondition.eAchv_Formation6CardLv, f_GetTeamMinLv(GameParamConst.TaskTeamCheckNum) });
        //进化信息
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
            new int[] { (int)EM_AchievementTaskCondition.eAchv_Formation6CardEvolve, f_GetTeamMinEvolveLv(GameParamConst.TaskTeamCheckNum) });
        //领悟信息
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
            new int[] { (int)EM_AchievementTaskCondition.eAchv_Formation6CardAwaken, f_GetTeamMinAwakenStar(GameParamConst.TaskTeamCheckNum) });
        //装备信息
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
            new int[] { (int)EM_AchievementTaskCondition.eAchv_4EquipFormationCards, f_GetTeamMinAllEquipHeroNum(GameParamConst.TaskEquipCheckNum) });
        //法宝信息
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
            new int[] { (int)EM_AchievementTaskCondition.eAchv_2TreasureFormationCards, f_GetTeamMinAllTreasureHeroNum(GameParamConst.TaskTreasureCheckNum) });
    }

    /// <summary>
    /// 获取N个上阵卡牌最小等级
    /// </summary>
    public int f_GetTeamMinLv(int checkNum)
    {
        int result = 0;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        TeamPoolDT tItem;
        if(tData.Count < checkNum)
        {
            return result;
        }
        else
        {
            result = int.MaxValue;
            for(int i = 0; i < checkNum; i++)
            {
                tItem = (TeamPoolDT)tData[i];
                if(result > tItem.m_CardPoolDT.m_iLv)
                    result = tItem.m_CardPoolDT.m_iLv;
            }
        }
        return result;
    }

    /// <summary>
    /// 获取N个上阵卡牌最小进化等级
    /// </summary>
    public int f_GetTeamMinEvolveLv(int checkNum)
    {
        int result = 0;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        TeamPoolDT tItem;
        if(tData.Count < checkNum)
        {
            return result;
        }
        else
        {
            result = int.MaxValue;
            for(int i = 0; i < checkNum; i++)
            {
                tItem = (TeamPoolDT)tData[i];
                if(result > tItem.m_CardPoolDT.m_iEvolveLv)
                    result = tItem.m_CardPoolDT.m_iEvolveLv;
            }
        }
        return result;
    }

    /// <summary>
    /// 获取N个上阵卡牌最小领悟等级
    /// </summary>
    public int f_GetTeamMinAwakenStar(int checkNum)
    {
        int result = 0;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        TeamPoolDT tItem;
        if(tData.Count < checkNum)
        {
            return result;
        }
        else
        {
            result = int.MaxValue;
            for(int i = 0; i < checkNum; i++)
            {
                tItem = (TeamPoolDT)tData[i];
                if(result > tItem.m_CardPoolDT.m_iLvAwaken)
                    result = tItem.m_CardPoolDT.m_iLvAwaken;
            }
        }
        return result;
    }

    /// <summary>
    /// 获取队伍装备了N件装备的英雄数
    /// </summary>
    public int f_GetTeamMinAllEquipHeroNum(int checkNum)
    {
        int result = 0;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        TeamPoolDT tItem;
        for(int i = 0; i < tData.Count; i++)
        {
            int tEquipNum = 0;
            tItem = (TeamPoolDT)tData[i];
            for(int j = 0; j < tItem.m_aEqupId.Length - 2; j++)
            {
                if(tItem.m_aEqupId[j] != 0)
                    tEquipNum++;
            }
            if(tEquipNum >= checkNum)
                result++;
        }
        return result;
    }

    /// <summary>
    /// 获取队伍装备了N件法宝的英雄数
    /// </summary>
    public int f_GetTeamMinAllTreasureHeroNum(int checkNum)
    {
        int result = 0;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        TeamPoolDT tItem;
        for(int i = 0; i < tData.Count; i++)
        {
            int tTreasureNum = 0;
            tItem = (TeamPoolDT)tData[i];
            for(int j = tItem.m_aEqupId.Length - 2; j < tItem.m_aEqupId.Length; j++)
            {
                if(tItem.m_aEqupId[j] != 0)
                    tTreasureNum++;
            }
            if(tTreasureNum >= checkNum)
                result++;
        }
        return result;
    }


    /// <summary>
    /// 检查队伍或者援军上阵位里是否包含有此模板的卡牌
    /// </summary>
    /// <param name="iTempId"></param>
    /// <returns></returns>
    public bool f_CheckHaveCardForTemp(int iTempId)
    {
        if(f_CheckReinforceCardForTempId(iTempId) == true)
            return true;

        return f_CheckInTeamByKeyId(iTempId);

        //List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        //for (int i = 0; i < tData.Count; i++)
        //{
        //    TeamPoolDT tTeamPoolDT = (TeamPoolDT)tData[i];
        //    if (tTeamPoolDT.m_CardPoolDT.m_CardDT.iId == iTempId)
        //        return true;
        //}
        //return false;
    }
    /// <summary>
    /// 刷新相应队伍的缘分信息
    /// </summary>
    /// <param name="tCardPoolDT"></param>
    public void f_UpdateCardFate(CardPoolDT tCardPoolDT)
    {
       // MessageBox.ASSERT("TsuLog TeamPool f_UpdateCardFate");
        if (tCardPoolDT.m_CardFatePoolDT == null)
        {
            CardFatePoolDT tCardFatePoolDT = new CardFatePoolDT();
            tCardFatePoolDT.iId = tCardPoolDT.m_CardDT.iId;
            tCardFatePoolDT.m_CardFateDT = (CardFateDT)glo_Main.GetInstance().m_SC_Pool.m_CardFateSC.f_GetSC(tCardPoolDT.m_CardDT.iId);
            int[] aFateId = ccMath.f_String2ArrayInt(tCardFatePoolDT.m_CardFateDT.szFateId, ";");
            for(int i = 0; i < aFateId.Length; i++)
            {
                if(aFateId[i] > 0)
                {
                    CardFateDataDT tCardFateDataDT = (CardFateDataDT)glo_Main.GetInstance().m_SC_Pool.m_CardFateDataSC.f_GetSC(aFateId[i]);
                    tCardFatePoolDT.m_aFateList.Add(tCardFateDataDT);
                    tCardFatePoolDT.m_aFateIsOk.Add(false);
                }
            }
            tCardPoolDT.m_CardFatePoolDT = tCardFatePoolDT;
        }
    }
    /// <summary>
    /// 检测是否需要更新缘分信息
    /// 如果该卡牌位已经上阵，则更新其缘分信息
    /// </summary>
    /// <param name="iIdId">卡牌唯一id</param>
    public void f_CheckCardFataUpdate(long iIdId)
    {
       // MessageBox.ASSERT("TsuLog TeamPool f_CheckCardFataUpdate");
        List<BasePoolDT<long>> listData = f_GetAll();
        for(int i = 0; i < listData.Count; i++)
        {
            TeamPoolDT poolDT = listData[i] as TeamPoolDT;
            if(poolDT.m_iCardId == iIdId)
            {
                poolDT.m_CardPoolDT.m_CardFatePoolDT = null;
                f_UpdateCardFate(poolDT);
                break;
            }
        }
    }
    /// <summary>
    /// 刷新相应队伍的缘分信息
    /// </summary>
    /// <param name="tTeamPoolDT"></param>
    /// <returns></returns>
    public void f_UpdateCardFate(TeamPoolDT tTeamPoolDT)
    {
       // MessageBox.ASSERT("TsuLog TeamPool f_UpdateCardFate");
        if(tTeamPoolDT.m_CardPoolDT.m_CardFatePoolDT == null)
        {
            CardFatePoolDT tCardFatePoolDT = new CardFatePoolDT();
            tCardFatePoolDT.iId = tTeamPoolDT.m_CardPoolDT.m_CardDT.iId;
            tCardFatePoolDT.m_CardFateDT = (CardFateDT)glo_Main.GetInstance().m_SC_Pool.m_CardFateSC.f_GetSC(tTeamPoolDT.m_CardPoolDT.m_CardDT.iId);
            int[] aFateId = ccMath.f_String2ArrayInt(tCardFatePoolDT.m_CardFateDT.szFateId, ";");
            for(int i = 0; i < aFateId.Length; i++)
            {
                if(aFateId[i] > 0)
                {
                    CardFateDataDT tCardFateDataDT = (CardFateDataDT)glo_Main.GetInstance().m_SC_Pool.m_CardFateDataSC.f_GetSC(aFateId[i]);
                    tCardFatePoolDT.m_aFateList.Add(tCardFateDataDT);
                    tCardFatePoolDT.m_aFateIsOk.Add(false);
                }
            }
            tTeamPoolDT.m_CardPoolDT.m_CardFatePoolDT = tCardFatePoolDT;
        }
        //检测适合条件的缘分           2018/4/27  17:12
        for(int i = 0; i < tTeamPoolDT.m_CardPoolDT.m_CardFatePoolDT.m_aFateList.Count; i++)
        {
            int[] aGoodsId = ccMath.f_String2ArrayInt(tTeamPoolDT.m_CardPoolDT.m_CardFatePoolDT.m_aFateList[i].szGoodsId, ";");
            tTeamPoolDT.m_CardPoolDT.m_CardFatePoolDT.m_aFateIsOk[i] = CheckCardHaveThisFate((EM_ResourceType)tTeamPoolDT.m_CardPoolDT.m_CardFatePoolDT.m_aFateList[i].iGoodsType, aGoodsId, tTeamPoolDT.m_aEquipPoolDT, tTeamPoolDT.m_aTreamPoolDT);
        }

    }

    /// <summary>
    /// 获取全部装备等级强化等级达到X级   IntenOrRefine  1为强化
    /// </summary>
    /// <returns></returns>
    public int f_GetTeamEquipInten(int Level, int IntenOrRefine)
    {
        int num = 0;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)tData[i];
            for(int j = 0; j < tTeamPoolDT.m_aEquipPoolDT.Length; j++)
            {
                bool isContent = true;
                if(tTeamPoolDT.m_aEquipPoolDT[j] == null)
                {
                    continue;
                }
                if(IntenOrRefine == 1)
                {
                    if(tTeamPoolDT.m_aEquipPoolDT[j].m_lvIntensify < Level)
                    {
                        isContent = false;
                    }
                }
                else
                {
                    if(tTeamPoolDT.m_aEquipPoolDT[j].m_lvRefine < Level)
                    {
                        isContent = false;
                    }
                }
                if(isContent)
                {
                    num++;
                }
            }
        }

        return num;
    }

    /// <summary>
    /// 获取全部法宝等级的个数
    /// </summary>
    /// <param name="Level"></param>
    /// <param name="IntenOrRefine"></param>
    /// <returns></returns>
    public int f_GetTeamTreasureIntenNum(int Level, int IntenOrRefine)
    {
        int num = 0;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = tData[i] as TeamPoolDT;
            for(int j = 0; j < tTeamPoolDT.m_aTreamPoolDT.Length; j++)
            {
                bool isContent = false;
                if(tTeamPoolDT.m_aTreamPoolDT[j] != null)
                {
                    if(IntenOrRefine == 1)
                    {
                        isContent = tTeamPoolDT.m_aTreamPoolDT[j].m_lvIntensify >= Level;
                    }
                    else
                    {
                        isContent = tTeamPoolDT.m_aTreamPoolDT[j].m_lvRefine >= Level;
                    }
                    if(isContent)
                    {
                        num++;
                    }
                }
            }

        }

        return num;
    }
    public int f_GetTeamTreasureMaxLevel()
    {
        int Level = 0;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)tData[i];
            for(int j = 0; j < tTeamPoolDT.m_aTreamPoolDT.Length; j++)
            {
                if(tTeamPoolDT.m_aTreamPoolDT[j] == null)
                    continue;
                if(tTeamPoolDT.m_aTreamPoolDT[j].m_lvRefine > Level)
                {
                    Level = tTeamPoolDT.m_aTreamPoolDT[j].m_lvRefine;
                }
            }
        }
        return Level;
    }

    /// <summary>
    /// 获取全部装备数量
    /// </summary>
    /// <returns></returns>
    public int f_GetAllEquipNum()
    {
        int num = 0;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)tData[i];
            for(int j = 0; j < tTeamPoolDT.m_aEquipPoolDT.Length; j++)
            {
                if(tTeamPoolDT.m_aEquipPoolDT[j] == null)
                    continue;
                num++;
            }
        }
        return num;
    }

    /// <summary>
    /// 获取装备最低等级  IntenOrRefine  1为强化
    /// </summary>
    /// <returns></returns>
    public int f_GetEquipMinLevel(int IntenOrRefine)
    {
        int Lv = 200;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)tData[i];
            for(int j = 0; j < tTeamPoolDT.m_aEquipPoolDT.Length; j++)
            {
                if(tTeamPoolDT.m_aEquipPoolDT[j] == null) continue;
                if(IntenOrRefine == 1)
                {
                    if(Lv > tTeamPoolDT.m_aEquipPoolDT[j].m_lvIntensify)
                    {
                        Lv = tTeamPoolDT.m_aEquipPoolDT[j].m_lvIntensify;
                    }
                }
                else
                {
                    if(Lv > tTeamPoolDT.m_aEquipPoolDT[j].m_lvRefine)
                    {
                        Lv = tTeamPoolDT.m_aEquipPoolDT[j].m_lvRefine;
                    }
                }
            }

        }
        return Lv;
    }

    /// <summary>
    /// 刷新装备套装信息
    /// </summary>
    public void f_UpdateSetEquip(TeamPoolDT tTeamPoolDT)
    {
        for(int i = 0; i < tTeamPoolDT.m_aEquipPoolDT.Length; i++)
        {
            if(tTeamPoolDT.m_aEquipPoolDT[i] == null)
                continue;
            tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip = new SetEquipPoolDT();
            tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_SetEquipDT = RoleTools.f_GetSetEquipDT(tTeamPoolDT.m_aEquipPoolDT[i].m_EquipDT.iId);
            if(tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_SetEquipDT.iEquipId1 == tTeamPoolDT.f_GetEquipPoolDTId(EM_Equip.eEquipPart_Weapon))
                tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_aSetIsOk[0] = true;
            else
                tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_aSetIsOk[0] = false;

            if(tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_SetEquipDT.iEquipId2 == tTeamPoolDT.f_GetEquipPoolDTId(EM_Equip.eEquipPart_Armour))
                tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_aSetIsOk[1] = true;
            else
                tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_aSetIsOk[1] = false;

            if(tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_SetEquipDT.iEquipId3 == tTeamPoolDT.f_GetEquipPoolDTId(EM_Equip.eEquipPart_Helmet))
                tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_aSetIsOk[2] = true;
            else
                tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_aSetIsOk[2] = false;

            if(tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_SetEquipDT.iEquipId4 == tTeamPoolDT.f_GetEquipPoolDTId(EM_Equip.eEquipPart_Belt))
                tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_aSetIsOk[3] = true;
            else
                tTeamPoolDT.m_aEquipPoolDT[i].m_SetEquip.m_aSetIsOk[3] = false;
        }

    }


    private bool CheckCardHaveThisFate(EM_ResourceType tEM_ResourceType, int[] aGoodsId, EquipPoolDT[] aEquipPoolDT, TreasurePoolDT[] aTreasurePoolDT)
    {
        //MessageBox.ASSERT("Tsulog TeamPool CheckCardHaveThisFate");
        bool isOk = tEM_ResourceType == EM_ResourceType.Card;
        if(tEM_ResourceType == EM_ResourceType.Equip)
        {
            for(int i = 0; i < aGoodsId.Length; i++)
            {
                if(aGoodsId[i] > 0)
                {
                    if(CheckEquipmentIsOk(aGoodsId[i], aEquipPoolDT))
                    {
                        isOk = true;
                        //return true;
                    }
                }
            }
            //return false;
        }
        else if(tEM_ResourceType == EM_ResourceType.Treasure)
        {
            for(int i = 0; i < aGoodsId.Length; i++)
            {
                if(aGoodsId[i] > 0)
                {
                    if(CheckEquipmentIsOk(aGoodsId[i], aTreasurePoolDT))
                    {
                        isOk = true;
                    }
                }
            }
        }
        else if(tEM_ResourceType == EM_ResourceType.Card)
        {
            for(int i = 0; i < aGoodsId.Length; i++)
            {
                if(aGoodsId[i] > 0)
                {
                    if(!f_CheckHaveCardForTemp(aGoodsId[i]))
                    {
                        isOk = false;
                        //return false;
                    }
                }
            }
        }
        else
        {
MessageBox.ASSERT("Invalid charm type," + "type" + tEM_ResourceType.ToString());
            return false;
        }

        return isOk;
        //return true;
    }

    private bool CheckEquipmentIsOk(int iGoodsId, EquipPoolDT[] aEquipPoolDT)
    {
        EquipPoolDT tEquipPoolDT = Array.Find<EquipPoolDT>(aEquipPoolDT, delegate (EquipPoolDT tItem)
        {
            if(tItem == null)
            {
                return false;
            }
            return tItem.m_EquipDT.iId == iGoodsId;
        });
        if(tEquipPoolDT != null)
        {
            return true;
        }
        return false;
    }

    private bool CheckEquipmentIsOk(int iGoodsId, TreasurePoolDT[] aTreasurePoolDT)
    {
        TreasurePoolDT tTreasurePoolDT = Array.Find<TreasurePoolDT>(aTreasurePoolDT, delegate (TreasurePoolDT tItem)
         {
             if(tItem == null)
             {
                 return false;
             }
             return tItem.m_TreasureDT.iId == iGoodsId;
         });
        if(tTreasurePoolDT != null)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 获取装备或法宝 的 强化或精炼大师等级
    /// 如果没有开启强化大师则返回-1
    /// </summary>
    /// <returns></returns>
    public int f_GetMasterLevel(EM_Master eMaster, EM_FormationPos tEM_FormationPos)
    {
        TeamPoolDT teamPoolDT = f_GetForId((int)tEM_FormationPos) as TeamPoolDT;
        if(teamPoolDT == null)
        {
MessageBox.ASSERT("Error，Unable to find equipment data at this location");
            return -1;
        }
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_MasterSC.f_GetAll();
        int masterLevelTemp = 0;
        for(int i = 0; i < listData.Count; i++)
        {
            MasterDT masterDT = listData[i] as MasterDT;
            if(masterDT.iType == (int)eMaster)
            {
                if (eMaster == EM_Master.EquipIntensify || eMaster == EM_Master.EquipRefine)
                {
                    for (int j = 0; j < teamPoolDT.m_aEquipPoolDT.Length; j++)
                    {
                        if (teamPoolDT.m_aEquipPoolDT[j] == null)
                            return -1;
                        int lv = eMaster == EM_Master.EquipIntensify
                            ? teamPoolDT.m_aEquipPoolDT[j].m_lvIntensify
                            : teamPoolDT.m_aEquipPoolDT[j].m_lvRefine;
                        if (lv < masterDT.iLv)
                            return masterLevelTemp;
                    }
                    masterLevelTemp++;
                }
                else if (eMaster == EM_Master.TreasureIntensify || eMaster == EM_Master.TreasureRefine)
                {
                    for (int j = 0; j < teamPoolDT.m_aTreamPoolDT.Length; j++)
                    {
                        if (teamPoolDT.m_aTreamPoolDT[j] == null)
                            return -1;
                        int lv = eMaster == EM_Master.TreasureIntensify
                            ? teamPoolDT.m_aTreamPoolDT[j].m_lvIntensify
                            : teamPoolDT.m_aTreamPoolDT[j].m_lvRefine;
                        if (lv < masterDT.iLv)
                            return masterLevelTemp;
                    }
                    masterLevelTemp++;
                }
            }
        }
        return masterLevelTemp;
MessageBox.ASSERT("Error，master level not found");
        return -1;
    }
    /// <summary>
    /// 判断该部位是否装备的有
    /// </summary>
    /// <param name="tPart"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool f_GetIsEquip(EM_EquipPart tPart, long id)
    {
        TeamPoolDT t;
        if(tPart == EM_EquipPart.eEquipPart_INVALID || tPart == EM_EquipPart.eEquipPart_NONE)
            return false;
        for(int i = 0; i < Data_Pool.m_TeamPool.f_GetAll().Count; i++)
        {
            if(Data_Pool.m_TeamPool.f_GetAll()[i] == null)
                continue;
            t = Data_Pool.m_TeamPool.f_GetAll()[i] as TeamPoolDT;

            if(t.m_aEqupId[(int)tPart - 1] == id)
                return true;
        }
        return false;
    }


    #region 七日活动用
    /// <summary>
    /// 获取法宝最低等级  IntenOrRefine  1为强化
    /// </summary>
    /// <returns></returns>
    public int f_GetTreasureMinLevel(int IntenOrRefine)
    {
        int Lv = 200;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)tData[i];
            for(int j = 0; j < tTeamPoolDT.m_aTreamPoolDT.Length; j++)
            {
                if(tTeamPoolDT.m_aTreamPoolDT[j] == null) continue;
                if(IntenOrRefine == 1)
                {
                    if(Lv > tTeamPoolDT.m_aTreamPoolDT[j].m_lvIntensify)
                    {
                        Lv = tTeamPoolDT.m_aTreamPoolDT[j].m_lvIntensify;
                    }
                }
                else
                {
                    if(Lv > tTeamPoolDT.m_aTreamPoolDT[j].m_lvRefine)
                    {
                        Lv = tTeamPoolDT.m_aTreamPoolDT[j].m_lvRefine;
                    }
                }
            }

        }
        return Lv;
    }

    public int f_GetAllTreasureNum()
    {
        int num = 0;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)tData[i];
            for(int j = 0; j < tTeamPoolDT.m_aTreamPoolDT.Length; j++)
            {
                if(tTeamPoolDT.m_aTreamPoolDT[j] == null)
                    continue;
                num++;
            }
        }
        return num;
    }
    public int f_GetTeamEquipMaxRefine()
    {
        int MaxLv = 0;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = tData[i] as TeamPoolDT;
            for(int j = 0; j < tTeamPoolDT.m_aEquipPoolDT.Length; j++)
            {
                if(tTeamPoolDT.m_aEquipPoolDT[j] == null)
                    continue;
                if(tTeamPoolDT.m_aEquipPoolDT[j].m_lvRefine > MaxLv)
                {
                    MaxLv = tTeamPoolDT.m_aEquipPoolDT[j].m_lvRefine;
                }
            }
        }
        return MaxLv;

    }

    /// <summary>
    /// 获取卡牌命星等级   1为命星  2为进阶
    /// </summary>
    /// <returns></returns>
    public int f_GetTeamCardTypeLv(int Level, int tType)
    {
        int num = 0;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = tData[i] as TeamPoolDT;
            if(tTeamPoolDT != null)
            {
                if(tTeamPoolDT.m_CardPoolDT.uSkyDestinyLv >= Level)
                {
                    num++;
                }
            }

        }
        return num;
    }
    /// <summary>
    /// 获取阵形卡牌的最大参数     1为命星  2为进阶
    /// </summary>
    /// <returns></returns>
    public int f_GetTeamCardMax(int Type)
    {
        int MaxLv = 0;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = tData[i] as TeamPoolDT;
            if(tTeamPoolDT != null)
            {
                if(tTeamPoolDT.m_CardPoolDT.uSkyDestinyLv > MaxLv)
                {
                    MaxLv = tTeamPoolDT.m_CardPoolDT.uSkyDestinyLv;
                }
            }
        }
        return MaxLv;
    }
    /// <summary>
    /// 获取上阵了几名X品质的角色
    /// </summary>
    /// <param name="Imporent"></param>
    public int f_GetTeamImporent(int Imporent)
    {
        int Num = 0;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)tData[i];
            if(tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
                continue;
            if(tTeamPoolDT.m_CardPoolDT.m_CardDT.iImportant >= Imporent)
            {
                Num++;
            }
        }
        return Num;
    }

    public int f_GetTeamEquipImporentNum(int Imporent)
    {
        int num = 0;
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)tData[i];
            if(tTeamPoolDT == null)
                continue;
            for(int j = 0; j < tTeamPoolDT.m_aEquipPoolDT.Length; j++)
            {
                if(tTeamPoolDT.m_aEquipPoolDT[j] == null)
                    continue;
                if(tTeamPoolDT.m_aEquipPoolDT[j].m_EquipDT.iColour >= Imporent)
                    num++;
            }
        }
        return num;
    }


    public bool f_CheckTeamTreasure(long KeyId)
    {
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for(int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)tData[i];

            for(int j = 0; j < tTeamPoolDT.m_aTreamPoolDT.Length; j++)
            {
                if(tTeamPoolDT.m_aTreamPoolDT[j] != null)
                {
                    if(tTeamPoolDT.m_aTreamPoolDT[j].iId == KeyId)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    #endregion

    public void f_CheckTeamGodEquipRedPoint(GodEquipPoolDT equipPoolDt, EM_EquipPart equipPart, ref bool CanLvUp, ref bool CanEquip, ref bool CanRefine)
    {
        if (equipPoolDt == null)
        {
            CanLvUp = false;
            CanEquip = UITool.f_CheckHasEquipLeft(equipPart);
            return;
        }
        if (equipPoolDt.m_lvIntensify < UITool.f_GetGodEquipIntenMax())// check xem có phải up ở lv cao nhất chưa
        {
            CanLvUp = true;
            if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money) < UITool.f_GetGodEquipIntenCon(equipPoolDt, 1))// kiểm tra đủ tiền nâng cấp ko
            {
                CanLvUp = false;
            }
        }
        else
        {
            CanLvUp = false;
        }
        if (Data_Pool.m_GodEquipPool.f_CheckGodEquipRefin(equipPoolDt))
        {
            CanRefine = true;
        }
        CanEquip = UITool.f_CheckHasEquipHighColorLeft(equipPart, equipPoolDt.m_EquipDT.iColour);// kiểm tra có phẩm nào cao hơn ko
    }

    public void f_ChangeTeamGodEquip(long cardPos, long iEquipId, byte equipPos, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FormationChangeGodEquip, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)cardPos);
        tCreateSocketBuf.f_Add(iEquipId);
        tCreateSocketBuf.f_Add(equipPos);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FormationChangeGodEquip, bBuf);
    }

    public bool f_CheckTeamGodEquip(long KeyId)
    {
        TeamPoolDT tTeamPoolDT;
        List<BasePoolDT<long>> tData = Data_Pool.m_TeamPool.f_GetAll();
        for (int i = 0; i < tData.Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)tData[i];

            for (int j = 0; j < tTeamPoolDT.m_aGodEquipPoolDT.Length; j++)
            {
                if (tTeamPoolDT.m_aGodEquipPoolDT[j] != null)
                {
                    if (tTeamPoolDT.m_aGodEquipPoolDT[j].iId == KeyId)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
