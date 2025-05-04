using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;
using System.Collections.Generic;

public class CardPool : BasePool
{

    //private static CardPool s_CardManage = null;
    //private BetterList<CardDT> g_CardPond;//卡牌对象池
    private List<int> listLastAddCardTempID = new List<int>();//保存添加的卡牌模板id(抽牌使用)
    /// <summary>
    /// 主角卡pooldt
    /// </summary>
    public CardPoolDT mRolePoolDt
    {
        get;
        private set;
    }

    public CardPool() : base("CardPoolDT", true)
    {
        //g_CardPond = new BetterList<CardDT>();
    }

    #region Pool数据管理

    protected override void f_Init()
    {
    }

    protected override void RegSocketMessage()
    {
        SC_CardInit tSC_CardInit = new SC_CardInit();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_CardInit, tSC_CardInit, Callback_SocketData_Update);
        stPoolDelData tstPoolDelData = new stPoolDelData();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CardRemove, tstPoolDelData, Callback_Del);
        CMsc_SC_CardSkyDestiny tSC_CardSkyDestiny = new CMsc_SC_CardSkyDestiny();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CardSkyDestiny, tSC_CardSkyDestiny, _GetSkyDestiny);
        SC_CardCostHistory tSC_CardCostHistory = new SC_CardCostHistory();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CardCostHistory, tSC_CardCostHistory, _GetSkyCostHistory);
    }
    /// <summary>
    /// 获取天命区间段经验
    /// </summary>
    /// <param name="obj"></param>
    private void _GetSkyDestiny(object obj)
    {
        if (_SkyUp != null)
            _SkyUp(obj);
    }
    private void _GetSkyCostHistory(object obj)
    {
        if (_SkyNum != null)
            _SkyNum(obj);
    }
    /// <summary>
    /// 获取增加的卡牌模板id信息
    /// </summary>
    /// <returns></returns>
    public List<int> f_GetlistLastAddCardTempID()
    {
        return listLastAddCardTempID;
    }
    /// <summary>
    /// 清楚保存添加的卡牌模板id
    /// </summary>
    public void f_ClearlistLastAddCardTempID()
    {
        listLastAddCardTempID.Clear();
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_CardInit tServerData = (SC_CardInit)Obj;
        CardPoolDT tPoolDataDT = new CardPoolDT();

        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_iLv = tServerData.lv;
        tPoolDataDT.m_iExp = tServerData.uExp;
        tPoolDataDT.m_iEvolveId = tServerData.evolveId;
        tPoolDataDT.m_iLvAwaken = tServerData.lvAwaken;
        tPoolDataDT.m_iFlagAwaken = tServerData.flagAwaken;
        tPoolDataDT.uSkyDestinyExp = tServerData.uSkyDestinyExp;
        tPoolDataDT.uSkyDestinyLv = tServerData.uSkyDestinyLv;
        tPoolDataDT.m_ArtifactPoolDT.f_UpdateValue(tServerData.lvArtifact);
        f_Save(tPoolDataDT);
        listLastAddCardTempID.Add(tServerData.tempId);
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.Card, tPoolDataDT.m_iTempleteId, 1);
        //发送玩家等级变更事件
        if (tPoolDataDT.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
        {
            //设置主角卡数据的引用
            _BeforeRoleLv = tPoolDataDT.m_iLv;
            //主角等级提升 神器数据刷新
            f_ArtifactUpdateByRoleLv(tPoolDataDT.m_iLv);
            mRolePoolDt = tPoolDataDT;
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerLvUpdate, tPoolDataDT.m_iLv);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress, new int[] { (int)EM_AchievementTaskCondition.eAchv_Level, tPoolDataDT.m_iLv });
        }
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_CardInit tServerData = (SC_CardInit)Obj;
        CardPoolDT tPoolDataDT = (CardPoolDT)f_GetForId(tServerData.id);
        if (tPoolDataDT == null)
        {
MessageBox.ASSERT("No character data，update failed");
        }
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_iLv = tServerData.lv;
        tPoolDataDT.m_iExp = tServerData.uExp;
        tPoolDataDT.m_iEvolveId = tServerData.evolveId;
        tPoolDataDT.m_iLvAwaken = tServerData.lvAwaken;
        tPoolDataDT.m_iFlagAwaken = tServerData.flagAwaken;
        tPoolDataDT.uSkyDestinyExp = tServerData.uSkyDestinyExp;
        tPoolDataDT.uSkyDestinyLv = tServerData.uSkyDestinyLv;
        tPoolDataDT.m_ArtifactPoolDT.f_UpdateValue(tServerData.lvArtifact);
        //发送玩家等级变更事件
        if (tPoolDataDT.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
        {
            if (_BeforeRoleLv != tPoolDataDT.m_iLv)
            {
                //UITool.Ui_Trip("升级了");
                int curEnergy = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy);
                int curVigor = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor);
                LevelUpPageParam param = new LevelUpPageParam(_BeforeRoleLv, tPoolDataDT.m_iLv, StaticValue.mOldEnergyValue, curEnergy, StaticValue.mOldVigorValue, curVigor);
                if (StaticValue.m_curScene == EM_Scene.GameMain)
                {
                    glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_USERINFOR);
                    //glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
                    Data_Pool.m_GuidancePool.f_ChangeGuidanceType(EM_GuidanceType.UpLevel);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LevelUpPage, UIMessageDef.UI_OPEN, param);
                    glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_TASKMAINUPDATE);
                }
                else
                {
                    StaticValue.mIsNeedShowLevelPage = true;
                    StaticValue.mLevelUpPageParam = param;
                }
                _BeforeRoleLv = tPoolDataDT.m_iLv;
                //主角等级提升 神器数据刷新
                f_ArtifactUpdateByRoleLv(tPoolDataDT.m_iLv);
                //角色升级 通知SDK
                glo_Main.GetInstance().m_SDKCmponent.f_SetRoleInfo(EM_SetRoleInfoType.LvUpRole, Data_Pool.m_UserData.m_iServerId, Data_Pool.m_UserData.m_strServerName, Data_Pool.m_UserData.m_szRoleName, Data_Pool.m_UserData.m_iUserId,
                Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee), UITool.f_GetNowVipLv(), Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level), LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().LegionName,
                Data_Pool.m_UserData.m_CreateTime.ToString(), Data_Pool.m_UserData.m_szSexDesc, Data_Pool.m_TeamPool.f_GetTotalBattlePower(), LegionMain.GetInstance().m_LegionInfor.m_iLegionId, Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard), Data_Pool.m_CardPool.f_GetRoleJob(), LegionMain.GetInstance().m_LegionPlayerPool.m_iSelfPos, LegionTool.f_GetPosDesc(LegionMain.GetInstance().m_LegionPlayerPool.m_iSelfPos),
                "Không");
            }
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerLvUpdate, tPoolDataDT.m_iLv);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress, new int[] { (int)EM_AchievementTaskCondition.eAchv_Level, tPoolDataDT.m_iLv });
        }
        //如果是阵上英雄数据变更
        if (Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(tPoolDataDT.iId))
        {
            //等级信息
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
                new int[] { (int)EM_AchievementTaskCondition.eAchv_Formation6CardLv, Data_Pool.m_TeamPool.f_GetTeamMinLv(GameParamConst.TaskTeamCheckNum) });
            //进化信息
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
                new int[] { (int)EM_AchievementTaskCondition.eAchv_Formation6CardEvolve, Data_Pool.m_TeamPool.f_GetTeamMinEvolveLv(GameParamConst.TaskTeamCheckNum) });
            //领悟信息
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
                new int[] { (int)EM_AchievementTaskCondition.eAchv_Formation6CardAwaken, Data_Pool.m_TeamPool.f_GetTeamMinAwakenStar(GameParamConst.TaskTeamCheckNum) });
        }
        Data_Pool.m_TeamPool.f_CheckCardFataUpdate(tServerData.id);
    }

    protected void Callback_Del(object Obj)
    {
        stPoolDelData tServerData = (stPoolDelData)Obj;
        f_Delete(tServerData.iId);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //以下为外部调用接口

    private int _BeforeRoleLv;
    /// <summary>
    /// 卡牌升级接口
    /// </summary>
    /// <param name="Id">指定Id</param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_LevelUp(CardPoolDT tCardPoolDT,
        long iItemKeyId0, int iItemNum0,
        long iItemKeyId1, int iItemNum1,
        long iItemKeyId2, int iItemNum2,
        long iItemKeyId3, int iItemNum3,
        SocketCallbackDT tSocketCallbackDT)
    {

        //I64u cardId;
        //CommItem item[4];

        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CardLvUp, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tCardPoolDT.iId);
        tCreateSocketBuf.f_Add(iItemKeyId0);
        tCreateSocketBuf.f_Add(iItemNum0);
        tCreateSocketBuf.f_Add(iItemKeyId1);
        tCreateSocketBuf.f_Add(iItemNum1);
        tCreateSocketBuf.f_Add(iItemKeyId2);
        tCreateSocketBuf.f_Add(iItemNum2);
        tCreateSocketBuf.f_Add(iItemKeyId3);
        tCreateSocketBuf.f_Add(iItemNum3);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CardLvUp, bBuf);

    }
    /// <summary>
    /// 强化五次
    /// </summary>
    public void f_LeveUp5(CardPoolDT tCardPoolDT,
        long iItemKeyId0, int iItemNum0,
        long iItemKeyId1, int iItemNum1,
        long iItemKeyId2, int iItemNum2,
        long iItemKeyId3, int iItemNum3,
        SocketCallbackDT tSocketCallbackDT)
    {

    }

    /// <summary>
    /// 卡牌卖出接口
    /// </summary>
    /// <param name="Id">指定Id</param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Sell(CardPoolDT tCardPoolDT, SocketCallbackDT tSocketCallbackDT)
    {

        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SellCard, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tCardPoolDT.iId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SellCard, bBuf);

    }

    /// <summary>
    /// 卡牌进阶接口
    /// </summary>
    /// <param name="tCardPoolDT"></param>
    /// <param name="iItemKeyId"></param>
    /// <param name="iItemNum"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Evolve(CardPoolDT tCardPoolDT, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CardEvolve, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tCardPoolDT.iId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CardEvolve, bBuf);
    }

    ccCallback _SkyUp;
    ccCallback _SkyNum;
    /// <summary>
    /// 卡牌请求天命接口
    /// </summary>
    public void f_CardSky(long id, int num, ccCallback ccCallBack,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CardSkyDestiny, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        _SkyUp = ccCallBack;
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        tCreateSocketBuf.f_Add(num);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CardSkyDestiny, bBuf);
    }
    /// <summary>
    /// 根据模板Id获取已拥有的数目
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    public int f_GetHaveNumByTemplate(int templateId)
    {
        return Data_Pool.m_CardPool.f_GetAllForData1(templateId).Count;
    }

    /// <summary>
    /// 卡牌领悟
    /// </summary>
    public void f_CardAwaken(long cardId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CardAwaken, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(cardId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CardAwaken, bBuf);
    }
    /// <summary>
    /// 卡牌分解
    /// </summary>
    public void f_CardRecycle(BasePoolDT<long> id1, BasePoolDT<long> id2, BasePoolDT<long> id3, BasePoolDT<long> id4,
        BasePoolDT<long> id5, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_Recycle, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)EM_ResourceType.Card);
        long[] tid = new long[5];
        BasePoolDT<long>[] tArr = { id1, id2, id3, id4, id5 };

        for (int i = 0; i < tArr.Length; i++)
        {
            if (tArr[i] != null)
            {
                tCreateSocketBuf.f_Add(tArr[i].iId);
                tid[i] = tArr[i].iId;
            }
            else
            {
                tCreateSocketBuf.f_Add(0L);
                tid[i] = 0;
            }
        }
        MessageBox.DEBUG("Thu hồi thẻ id1   " + tid[0] + "   Id2   " + tid[1] + "   id3   " + tid[2] + "   id4   " + tid[3] + "   id5   " + tid[4]);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_Recycle, bBuf);
    }
    //public void f_CheckCardRecycleRedPoint()
    //{
    //    Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.CardRecycle);
    //    List<BasePoolDT<long>> listData = f_GetAll();
    //    for (int i = 0; i < listData.Count; i++)
    //    {
    //        CardPoolDT poolDT = listData[i] as CardPoolDT;
    //        if ( (poolDT.m_CardDT.iImportant == (int)EM_Important.Blue || poolDT.m_CardDT.iImportant == (int)EM_Important.Green) 
    //            && !Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(poolDT.iId))
    //        {
    //            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.CardRecycle);
    //            return;
    //        }
    //    }
    //}
    /// <summary>
    /// 卡牌重生
    /// </summary>
    public void f_CardRebirth(long id, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_Rebirth, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)EM_ResourceType.Card);
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_Rebirth, bBuf);
    }

    public void f_GetCardSkyNum(long id, ccCallback SkyNum)
    {
        _SkyNum = SkyNum;
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CardCostHistory, bBuf);
    }
    /// <summary>
    /// 检测是否还有卡牌可上阵
    /// </summary>
    public bool f_CheckHasCardLeft()
    {
        long curCardId = 0;
        long curReinforceId = 0;
        List<int> listTempCardId = new List<int>();

        List<BasePoolDT<long>> temp = Data_Pool.m_TeamPool.f_GetAll();
        //去除已上阵卡牌位
        for (int i = 0; i < temp.Count; i++)
        {
            TeamPoolDT dt = temp[i] as TeamPoolDT;
            //上阵可以本位置英雄类型，但去除同一张卡牌
            if (LineUpPage.CurrentSelectCardPos == dt.m_eFormationPos)
            {
                curCardId = dt.m_CardPoolDT.iId;
                continue;
            }
            //去除模板id,同模板id不能同时上阵
            listTempCardId.Add(dt.m_CardPoolDT.m_CardDT.iId);
        }
        //去除已上阵的援军位
        IEnumerator<EM_ReinforcePos> listData = Data_Pool.m_TeamPool.dicReinforceCardId.Keys.GetEnumerator();
        while (listData.MoveNext())
        {
            CardPoolDT CurrentCardPoolDT = Data_Pool.m_TeamPool.dicReinforceCardId[listData.Current];
            if (CurrentCardPoolDT != null)
            {
                if (LineUpPage.CurrentSelectCardPos == EM_FormationPos.eFormationPos_Reinforce && listData.Current == LineUpPage.CurrentSelectReinforcePos)
                {
                    curReinforceId = CurrentCardPoolDT.iId;
                    continue;
                }
                //去除模板id,同模板id不能同时上阵
                listTempCardId.Add(CurrentCardPoolDT.m_CardDT.iId);
            }
        }
        List<BasePoolDT<long>> _CardList = Data_Pool.m_CardPool.f_GetAll();
        for (int i = 0; i < _CardList.Count; i++)
        {
            CardPoolDT cardPoolDT = _CardList[i] as CardPoolDT;
            if(cardPoolDT.iId == curCardId || cardPoolDT.iId == curReinforceId)
                continue;
            bool isSameTempId = false;
            for (int j = 0; j < listTempCardId.Count; j++)
            {
                if (cardPoolDT.m_CardDT.iId == listTempCardId[j])
                {
                    isSameTempId = true;
                    break;
                }
            }
            if (isSameTempId)
                continue;
            return true;
        }
        return false;
    }
    /// <summary>
    /// 排序规则 主角卡>已上阵> 援军> 品质（高在前）>战斗力（高在前，暂无战斗力）>id（高在前）
    /// </summary>
    public void f_SortList()
    {
        f_GetAll().Sort(delegate (BasePoolDT<long> node1, BasePoolDT<long> node2)
        {
            CardPoolDT item1 = (CardPoolDT)node1;
            CardPoolDT item2 = (CardPoolDT)node2;
			if (item1.m_iLv > item2.m_iLv)
			{
                return -1;
            }
            else if (item1.m_iLv < item2.m_iLv)
            {
                return 1;
            }
            else
            {
				if (item1.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
					return -1;
				else if (item2.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
					return 1;
				else
				{
					bool tInTeam1 = Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(item1.iId);
					bool tInTeam2 = Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(item2.iId);
					if (tInTeam1 && !tInTeam2)
						return -1;
					else if (!tInTeam1 && tInTeam2)
						return 1;
					else
					{
						bool tIsReinforce1 = Data_Pool.m_TeamPool.dicReinforceCardId.ContainsValue(item1);
						bool tIsReinforce2 = Data_Pool.m_TeamPool.dicReinforceCardId.ContainsValue(item2);
						if (tIsReinforce1 && !tIsReinforce2)
						{
							return -1;
						}
						else if (!tIsReinforce1 && tIsReinforce2)
						{
							return 1;
						}
						else
						{
							if (item1.m_CardDT.iImportant > item2.m_CardDT.iImportant)
								return -1;
							else if (item1.m_CardDT.iImportant < item2.m_CardDT.iImportant)
								return 1;
							else
							{
								if (item1.m_CardDT.iId < item2.m_CardDT.iId)
									return -1;
								else if (item1.m_CardDT.iId > item2.m_CardDT.iId)
									return 1;
							}
						}
					}
				}
			}
            return 0;
        });
    }

    public void f_SortList(List<BasePoolDT<long>> obj)
    {
        obj.Sort(delegate (BasePoolDT<long> node1, BasePoolDT<long> node2)
        {
            CardPoolDT item1 = (CardPoolDT)node1;
            CardPoolDT item2 = (CardPoolDT)node2;
            if (item1.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
                return -1;
            else if (item2.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
                return 1;
            else
            {
                bool tInTeam1 = Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(item1.iId);
                bool tInTeam2 = Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(item2.iId);
                if (tInTeam1 && !tInTeam2)
                    return -1;
                else if (!tInTeam1 && tInTeam2)
                    return 1;
                else
                {
                    if (item1.m_CardDT.iImportant > item2.m_CardDT.iImportant)
                        return -1;
                    else if (item1.m_CardDT.iImportant < item2.m_CardDT.iImportant)
                        return 1;
                    else
                    {
                        if (item1.m_CardDT.iId > item2.m_CardDT.iId)
                            return -1;
                        else if (item1.m_CardDT.iId < item2.m_CardDT.iId)
                            return 1;
                    }
                }
            }
            return 0;
        });
    }

    public CardPoolDT f_GetPlayerRolePoolDT()
    {
        foreach (CardPoolDT item in f_GetAll())
        {
            if (item.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
            {
                return item;
            }
        }
        return null;
    }


    /// <summary>
    /// 获取主角卡等级
    /// </summary>
    /// <returns></returns>
    public int f_GetRoleLevel()
    {
        if (mRolePoolDt == null)
        {
            MessageBox.DEBUG("RolePoolDt Not Init By Server");
            return 0;
        }
        return mRolePoolDt.m_iLv;
    }

    /// <summary>
    /// 获取主角卡经验
    /// </summary>
    /// <returns></returns>
    public int f_GetRoleExp()
    {
        if (mRolePoolDt == null)
        {
            MessageBox.DEBUG("RolePoolDt Not Init By Server");
            return 0;
        }
        return mRolePoolDt.m_iExp;
    }

    /// <summary>
    /// 获取角色职业名字（卡牌名字）
    /// </summary>
    /// <returns></returns>
    public string f_GetRoleJob()
    {
        if (mRolePoolDt == null)
        {
            MessageBox.DEBUG("RolePoolDt Not Init By Server");
return "Not found";
        }
        return mRolePoolDt.m_CardDT.szName;

    }

    #region 神器 Artifact

    private class ArtifactLvLimitData
    {
        public ArtifactLvLimitData(int roleLv, int artifactLvMax)
        {
            RoleLv = roleLv;
            ArtifactLvMax = artifactLvMax;
        }
        public int RoleLv;
        public int ArtifactLvMax;
    }

    private readonly ArtifactLvLimitData[] m_ArtifactLvLimit = new ArtifactLvLimitData[] {
                                                            new ArtifactLvLimitData(60,30),
                                                            new ArtifactLvLimitData(65,40),
                                                            new ArtifactLvLimitData(70,50)
                                                            };

    private int m_ArtifactLvMax = 0;
    /// <summary>
    /// 神器最大等级 0：未开放  !=0：代表目前能升级的最大等级
    /// </summary>
    public int ArtifactLvMax
    {
        get
        {
            return m_ArtifactLvMax;
        }
    }

    private void f_ArtifactUpdateByRoleLv(int roleLv)
    {
        for (int i = m_ArtifactLvLimit.Length - 1; i >= 0; i--)
        {
            if (roleLv >= m_ArtifactLvLimit[i].RoleLv)
            {
                m_ArtifactLvMax = m_ArtifactLvLimit[i].ArtifactLvMax;
                return;
            }
        }
        m_ArtifactLvMax = 0;
    }

    /// <summary>
    /// 卡牌神器升级
    /// </summary>
    /// <param name="cardServerId">卡牌服务器Id</param>
    /// <param name="socketCallbackDt"></param>
    public void f_CardArtifactUpgrade(long cardServerId,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CardArtifactUpgrade, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(cardServerId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CardArtifactUpgrade, bBuf);
    }

    /// <summary>
    /// 获取神器开启等级
    /// </summary>
    /// <returns></returns>
    public int f_GetArtifactOpenLv()
    {
        if (m_ArtifactLvLimit != null && m_ArtifactLvLimit.Length > 0)
        {
            return m_ArtifactLvLimit[0].RoleLv;
        }
        //数据有错 不开启
        return 99999;
    }

    /// <summary>
    /// 是否真的满级了
    /// </summary>
    /// <param name="roleLv">玩家等级</param>
    /// <param name="artifactLv">神器等级</param>
    /// <returns></returns>
    public bool f_IsTrueArtifactMaxLv(int roleLv, int artifactLv)
    {
        
        int tRoleLvMax = m_ArtifactLvLimit[m_ArtifactLvLimit.Length - 1].RoleLv;
        int tArtifactLvMax = m_ArtifactLvLimit[m_ArtifactLvLimit.Length - 1].ArtifactLvMax;
        return roleLv >= tRoleLvMax && artifactLv >= tArtifactLvMax;
    }

    /// <summary>
    /// 神器升级所需的玩家
    /// </summary>
    /// <param name="roleLv"></param>
    /// <returns></returns>
    public int f_GetArtifactUpgradeNeedRoleLv(int roleLv)
    {
        int needLv = 0;
        for (int i = 0; i < m_ArtifactLvLimit.Length; i++)
        {
            if (roleLv < m_ArtifactLvLimit[i].RoleLv)
            {
                needLv = m_ArtifactLvLimit[i].RoleLv;
                break;
            }
                
        }
        return needLv;
    }

    #endregion

}



