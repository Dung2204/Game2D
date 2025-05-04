using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;

public class AwardPool : BasePool
{
    private Dictionary<int, List<AwardPoolDT>> mDataDic;
    public List<AwardPoolDT> m_GetLoginAward;//卡牌   ,卡牌碎片
    public AwardPool() : base("AwardPoolDT")
    {
        m_GetLoginAward = new List<AwardPoolDT>();
    }

    protected override void f_Init()
    {
        mDataDic = new Dictionary<int, List<AwardPoolDT>>();
    }

    protected override void RegSocketMessage()
    {
        SC_Award tSC_Award = new SC_Award();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_RandAward, tSC_Award, Callback_SocketData_AwardUpdate);
        SC_CenterAward tCenter = new SC_CenterAward();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_AwardCenter, tCenter, _addData);

        stDelData tstPoolDelData = new stDelData();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_AwardCenterRemove, tstPoolDelData, _Dele);

        SC_Award tSC_OneKeyAward = new SC_Award();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_AwardCenterRcvAll, tSC_OneKeyAward, Callback_SocketData_OneKeyAward);
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }

    void _Dele(object obj)
    {
        stDelData tmpDeta = (stDelData)obj;
        f_Delete((long)tmpDeta.iId);
    }

    ccCallback ShowAward;
    void _addData(object obj)
    {
        SC_CenterAward tCenter = (SC_CenterAward)obj;
        AwardPoolDT tAward = new AwardPoolDT();
        tAward.iId = tCenter.id;
        tAward.m_Id = tCenter.id;
        tAward.m_Param = tCenter.Param;
        tAward.m_CenterAwardType = tCenter.CenterAwardType;
        tAward.m_Goods = new SC_CenterAwardNode[tCenter.nodes.Length];
        for (int i = 0; i < tCenter.nodes.Length; i++)
        {
            tAward.m_Goods[i] = tCenter.nodes[i];
        }
        if (f_GetForId(tAward.iId) == null)
            f_Save(tAward);
        _RunDelg();
    }

    public void _RunDelg()
    {
        if (ShowAward != null)
            ShowAward(true);
    }
    public void f_AwardCenter(ccCallback back)
    {
        ShowAward = back;
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(1);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_AwardCenter, bBuf);
    }

    public void f_AddAward(EM_ResourceType ResourceType, int ResourseTempId, int num)
    {
        if (this == null)
        {
            return;
        }
        if (StaticValue.m_curScene != EM_Scene.GameMain)
            return;
        if (m_GetLoginAward == null || num <= 0)
            return;

        AwardPoolDT tResourse = m_GetLoginAward.Find((AwardPoolDT tresoures) =>
        {
            return tresoures.mTemplate.mResourceId == ResourseTempId;
        });

        if (tResourse == null)
        {
            tResourse = new AwardPoolDT();
            tResourse.f_UpdateByInfo((byte)ResourceType, ResourseTempId, num);
            m_GetLoginAward.Add(tResourse);
        }
        else
        {
            tResourse.mTemplate.f_AddNum(num);
        }
        switch (ResourceType)
        {
            case EM_ResourceType.Good:
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.GoodsBagNewGoods);
                break;
            case EM_ResourceType.AwakenEquip:
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.AwakenBagNewGoods);
                break;
            case EM_ResourceType.Card:
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.CardBagNewGoods);
                break;
            case EM_ResourceType.CardFragment:
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.CardFragmentBagNewGoods);
                break;
            case EM_ResourceType.Equip:
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.EquipBagNewGoods);
                break;
            case EM_ResourceType.EquipFragment:
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.EquipFragmentBagNewGoods);
                break;
            case EM_ResourceType.Treasure:
                if (UITool.f_GetIsOpensystem(EM_NeedLevel.OpenTreasureLevel))
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TreasureBagNewGoods);
                break;
            case EM_ResourceType.TreasureFragment:
                if (UITool.f_GetIsOpensystem(EM_NeedLevel.OpenTreasureLevel))
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TreasureFragmentBagNewGoods);
                break;
            case EM_ResourceType.GodEquip:
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.GodEquipBagNewGoods);
                break;
            case EM_ResourceType.GodEquipFragment:
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.GodEquipFragmentBagNewGoods);
                break;
        }

    }
    /// <summary>
    /// 处理奖励数据
    /// </summary>
    /// <param name="value1">awardType</param>
    /// <param name="value2">无效</param>
    /// <param name="iNUm">数目</param>
    /// <param name="aData">数据</param>
    private void Callback_SocketData_AwardUpdate(int value1, int value2, int iNUm, ArrayList aData)
    {
        if (mDataDic.ContainsKey(value1))
        {
            mDataDic[value1].Clear();
        }
        else
        {
            List<AwardPoolDT> tList = new List<AwardPoolDT>();
            mDataDic.Add(value1, tList);
        }
        if (value1 == (int)EM_AwardSource.eAward_Sweep)
        {
            List<AwardPoolDT> awardList = new List<AwardPoolDT>();
            for (int i = 0; i < aData.Count; i++)
            {
                SC_Award node = (SC_Award)aData[i];
                bool isExitSame = false;
                for (int j = 0; j < awardList.Count; j++)
                {
                    if (awardList[j].mTemplate.mResourceType == node.resourceType
                        && awardList[j].mTemplate.mResourceId == node.resourceId)
                    {
                        awardList[j].mTemplate.f_AddNum(node.resourceNum);
                        isExitSame = true;
                    }
                }
                if (isExitSame)
                    continue;
                AwardPoolDT item = new AwardPoolDT();
                item.f_UpdateByInfo(node.resourceType, node.resourceId, node.resourceNum);
                awardList.Add(item);
            }
            Data_Pool.m_DungeonPool.f_AddSweepResult(awardList);
        }
        else
        {
            for (int i = 0; i < aData.Count; i++)
            {
                SC_Award node = (SC_Award)aData[i];
                AwardPoolDT item = new AwardPoolDT();
                item.f_UpdateByInfo(node.resourceType, node.resourceId, node.resourceNum);
                mDataDic[value1].Add(item);
            }
        }
    }

    /// <summary>
    /// 一键领取
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <param name="iNUm"></param>
    /// <param name="aData"></param>
    private void Callback_SocketData_OneKeyAward(int value1, int value2, int iNUm, ArrayList aData)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        for (int i = 0; i < aData.Count; i++)
        {
            SC_Award node = (SC_Award)aData[i];
            AwardPoolDT poolDT = new AwardPoolDT();
            poolDT.f_UpdateByInfo(node.resourceType, node.resourceId, node.resourceNum);
            awardList.Add(poolDT);
        }
        if (awardList.Count >= 1)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        }

        ccUIManage.GetInstance().f_SendMsg(UINameConst.Award, UIMessageDef.UI_CLOSE);
        f_Clear();
        _RunDelg();
    }

    public List<AwardPoolDT> f_GetAwardPoolDTByType(int starNum, EM_AwardSource awardSource)
    {
        if (starNum > 0 && mDataDic.ContainsKey((int)awardSource))
            return mDataDic[(int)awardSource];
        else
        {
            if (starNum > 0)
MessageBox.ASSERT(string.Format("AwardSource:{0} Reward does not exist", awardSource.ToString()));
            return new List<AwardPoolDT>();
        }
    }

    public List<AwardPoolDT> f_GetAwardPoolDTByType(EM_AwardSource awardSource)
    {
        if (mDataDic.ContainsKey((int)awardSource))
            return mDataDic[(int)awardSource];
        else
            return new List<AwardPoolDT>();
    }

    //根据奖励ID获取奖励列表的链表
    private List<AwardPoolDT> _awardList = new List<AwardPoolDT>();
    /// <summary>
    /// 根据奖励ID 获取奖励列表
    /// </summary>
    /// <param name="awardId"></param>
    /// <returns></returns>
    public List<AwardPoolDT> f_GetAwardPoolDTByAwardId(int awardId, bool newList = false, int num = 1)
    {
        _awardList.Clear();
        AwardDT tAwardDT = (AwardDT)glo_Main.GetInstance().m_SC_Pool.m_AwardSC.f_GetSC(awardId);
        string parseSource = tAwardDT == null ? "" : tAwardDT.szAward;
        string[] awardGroup = parseSource.Split('A');
        for (int i = 0; i < awardGroup.Length; i++)
        {
            string[] awardSubGroup = awardGroup[i].Split('#');
            for (int j = 0; j < awardSubGroup.Length; j++)
            {
                string[] awardItem = awardSubGroup[j].Split(';');
                if (awardItem.Length == 3)
                {
                    int tType = int.Parse(awardItem[0]);
                    if (tType == 0)
                        continue;
                    int tId = int.Parse(awardItem[1]);
                    int tNum = int.Parse(awardItem[2]) * num;
                    //如果找到相同的就忽略不加入
                    AwardPoolDT lastItem = _awardList.Find(delegate (AwardPoolDT dt) { return dt.mTemplate.mResourceType == tType && dt.mTemplate.mResourceId == tId; });
                    if (lastItem != null)
                        continue;
                    AwardPoolDT item = new AwardPoolDT();
                    item.f_UpdateByInfo((byte)tType, tId, tNum);
                    _awardList.Add(item);
                }
                else
                {
MessageBox.ASSERT("Error converting type of reward string, Id = " + awardId);
                }
            }
        }
        if (newList)
        {
            AwardPoolDT[] tNewArr = new AwardPoolDT[_awardList.Count];
            _awardList.CopyTo(tNewArr);
            List<AwardPoolDT> tNewList = new List<AwardPoolDT>(tNewArr);
            return tNewList;
        }
        else
            return _awardList;
    }

    //根据奖励ID组获取奖励列表的链表
    private List<AwardPoolDT> _awardList1 = new List<AwardPoolDT>();
    public List<AwardPoolDT> f_GetAwardPoolDTByAwardIdArray(List<int> awardId, bool newList = false, int num = 1)
    {
        _awardList1.Clear();
        for (int z = 0; z < awardId.Count; z++)
        {
            AwardDT tAwardDT = (AwardDT)glo_Main.GetInstance().m_SC_Pool.m_AwardSC.f_GetSC(awardId[z]);
            string parseSource = tAwardDT == null ? "" : tAwardDT.szAward;
            string[] awardGroup = parseSource.Split('A');
            for (int i = 0; i < awardGroup.Length; i++)
            {
                string[] awardSubGroup = awardGroup[i].Split('#');
                for (int j = 0; j < awardSubGroup.Length; j++)
                {
                    string[] awardItem = awardSubGroup[j].Split(';');
                    if (awardItem.Length == 3)
                    {
                        int tType = int.Parse(awardItem[0]);
                        if (tType == 0)
                            continue;
                        int tId = int.Parse(awardItem[1]);
                        int tNum = int.Parse(awardItem[2]) * num;
                        //如果找到相同的就忽略不加入
                        AwardPoolDT lastItem = _awardList1.Find(delegate (AwardPoolDT dt) { return dt.mTemplate.mResourceType == tType && dt.mTemplate.mResourceId == tId; });
                        if (lastItem != null)
                        {
                            lastItem.mTemplate.f_AddNum(tNum);
                            continue;
                        }
                        AwardPoolDT item = new AwardPoolDT();
                        item.f_UpdateByInfo((byte)tType, tId, tNum);
                        _awardList1.Add(item);
                    }
                    else
                    {
MessageBox.ASSERT("Award string type conversion error, Id = " + awardId[z]);
                    }
                }
            }
        }
        if (newList)
        {
            AwardPoolDT[] tNewArr = new AwardPoolDT[_awardList1.Count];
            _awardList1.CopyTo(tNewArr);
            List<AwardPoolDT> tNewList = new List<AwardPoolDT>(tNewArr);
            return tNewList;
        }
        else
            return _awardList1;
    }

    /// <summary>
    /// 通过奖励id,及一个资源，获取该资源在奖励列表中的序号，没有则返回-1
    /// </summary>
    /// <param name="awardId"></param>
    /// <param name="resourceType"></param>
    /// <param name="resourceId"></param>
    /// <param name="resourceNum"></param>
    /// <returns></returns>
    public int f_GetIndexOfAwardPoolDTByAwardId(int awardId, byte resourceType, int resourceId, int resourceNum)
    {
        List<AwardPoolDT> listAwardPoolDT = f_GetAwardPoolDTByAwardId(awardId);
        return f_GetIndexOfAwardPoolDTByAwardId(listAwardPoolDT, resourceType, resourceId, resourceNum);
    }
    /// <summary>
    /// 通过奖励id,及一个资源，获取该资源在奖励列表中的序号，没有则返回-1
    /// </summary>
    /// <param name="awardId"></param>
    /// <param name="resourceType"></param>
    /// <param name="resourceId"></param>
    /// <param name="resourceNum"></param>
    /// <returns></returns>
    public int f_GetIndexOfAwardPoolDTByAwardId(List<AwardPoolDT> listAwardPoolDT, byte resourceType, int resourceId, int resourceNum)
    {
        for (int i = 0; i < listAwardPoolDT.Count; i++)
        {
            AwardPoolDT poolDT = listAwardPoolDT[i] as AwardPoolDT;
            if (poolDT.mTemplate.mResourceType == resourceType &&
                poolDT.mTemplate.mResourceId == resourceId &&
                poolDT.mTemplate.mResourceNum == resourceNum)
                return i;
        }
        return -1;
    }
    /// <summary>
    /// 根据奖励ID 获取奖励列表 (在奖励列表一个奖励)
    /// </summary>
    /// <param name="awardId"></param>
    /// <returns></returns>
    public List<AwardPoolDT> f_GetAwardPoolDTByAwardId(int type, int id, int num, int awardId)
    {
        _awardList.Clear();
        AwardPoolDT addItem = new AwardPoolDT();
        addItem.f_UpdateByInfo((byte)type, id, num);
        _awardList.Add(addItem);
        AwardDT tAwardDT = (AwardDT)glo_Main.GetInstance().m_SC_Pool.m_AwardSC.f_GetSC(awardId);
        string parseSource = tAwardDT == null ? "" : tAwardDT.szAward;
        string[] awardGroup = parseSource.Split('A');
        for (int i = 0; i < awardGroup.Length; i++)
        {
            string[] awardSubGroup = awardGroup[i].Split('#');
            for (int j = 0; j < awardSubGroup.Length; j++)
            {
                string[] awardItem = awardSubGroup[j].Split(';');
                if (awardItem.Length == 3)
                {
                    int tType = int.Parse(awardItem[0]);
                    if (tType == 0)
                        continue;
                    int tId = int.Parse(awardItem[1]);
                    int tNum = int.Parse(awardItem[2]);
                    //如果找到相同的就忽略不加入
                    AwardPoolDT lastItem = _awardList.Find(delegate (AwardPoolDT dt) { return dt.mTemplate.mResourceType == tType && dt.mTemplate.mResourceId == tId; });
                    if (lastItem != null)
                        continue;
                    AwardPoolDT item = new AwardPoolDT();
                    item.f_UpdateByInfo((byte)tType, tId, tNum);
                    _awardList.Add(item);
                }
                else
                {
MessageBox.ASSERT("Error converting type of reward string, Id = " + awardId);
                }
            }
        }
        return _awardList;
    }

    /// <summary>
    /// 获取竞技场固定奖励
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public List<AwardPoolDT> f_GetArenaAwardByResult(int result)
    {
        _awardList.Clear();
        int lv = StaticValue.m_sLvInfo.m_iAddLv;
        //银币
        int moneyNum = GameFormula.f_VigorCost2Money(lv, GameParamConst.ArenaVigorCost);
        AwardPoolDT addItem = new AwardPoolDT();
        addItem.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_UserAttr.eUserAttr_Money, moneyNum);
        _awardList.Add(addItem);
        int addExp;
        //经验
        int expNum = GameFormula.f_VigorCost2Exp(lv, GameParamConst.ArenaVigorCost, out addExp);
        AwardPoolDT expItem = new AwardPoolDT();
        expItem.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_UserAttr.eUserAttr_Exp, expNum);
        _awardList.Add(expItem);
        //声望
        if (result != (int)EM_ArenaResult.Lose)
        {
            AwardPoolDT fameItem = new AwardPoolDT();
            fameItem.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_UserAttr.eUserAttr_Fame, GameParamConst.ArenaWinFame);
            _awardList.Add(fameItem);
        }
        else
        {
            AwardPoolDT fameItem = new AwardPoolDT();
            fameItem.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_UserAttr.eUserAttr_Fame, GameParamConst.ArenaLoseFame);
            _awardList.Add(fameItem);
        }
        return _awardList;
    }

    /// <summary>
    /// 领取奖励
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tback"></param>
    public void f_Get(long id, SocketCallbackDT tback)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_AwardCenterRecv, tback.m_ccCallbackSuc, tback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_AwardCenterRecv, bBuf);
    }

    /// <summary>
    /// 一键领取
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tback"></param>
    public void f_OneKeyRewardGet(SocketCallbackDT tback)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_AwardCenterRecvAll, tback.m_ccCallbackSuc, tback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_AwardCenterRecvAll, bBuf);
    }

    public List<AwardPoolDT> f_GetArenaRankAward(int rank)
    {
        _awardList.Clear();
        List<NBaseSCDT> rankAwards = glo_Main.GetInstance().m_SC_Pool.m_ArenaRankAwardSC.f_GetAll();
        for (int i = 0; i < rankAwards.Count; i++)
        {
            ArenaRankAwardDT node = (ArenaRankAwardDT)rankAwards[i];
            if (rank >= node.iMin && rank <= node.iMax)
            {
                string[] awardSubGroup = node.szAward.Split('#');
                for (int j = 0; j < awardSubGroup.Length; j++)
                {
                    string[] awardItem = awardSubGroup[j].Split(';');
                    if (awardItem.Length == 3)
                    {
                        int tType = int.Parse(awardItem[0]);
                        if (tType == 0)
                            continue;
                        int tId = int.Parse(awardItem[1]);
                        int tNum = int.Parse(awardItem[2]);
                        //如果找到相同的就忽略不加入
                        AwardPoolDT lastItem = _awardList.Find(delegate (AwardPoolDT dt) { return dt.mTemplate.mResourceType == tType && dt.mTemplate.mResourceId == tId; });
                        if (lastItem != null)
                            continue;
                        AwardPoolDT item = new AwardPoolDT();
                        item.f_UpdateByInfo((byte)tType, tId, tNum);
                        _awardList.Add(item);
                    }
                    else
                    {
MessageBox.ASSERT("Error converting the type of reward string rank, rank = " + rank);
                    }
                }
            }
        }
        return _awardList;
    }

    public List<AwardPoolDT> f_GetArenaCrossRankAward(int rank)
    {
        _awardList.Clear();
        List<NBaseSCDT> rankAwards = glo_Main.GetInstance().m_SC_Pool.m_CrossArenaRankAwardSC.f_GetAll();
        for (int i = 0; i < rankAwards.Count; i++)
        {
            ArenaRankAwardDT node = (ArenaRankAwardDT)rankAwards[i];
            if (rank >= node.iMin && rank <= node.iMax)
            {
                string[] awardSubGroup = node.szAward.Split('#');
                for (int j = 0; j < awardSubGroup.Length; j++)
                {
                    string[] awardItem = awardSubGroup[j].Split(';');
                    if (awardItem.Length == 3)
                    {
                        int tType = int.Parse(awardItem[0]);
                        if (tType == 0)
                            continue;
                        int tId = int.Parse(awardItem[1]);
                        int tNum = int.Parse(awardItem[2]);
                        //如果找到相同的就忽略不加入
                        AwardPoolDT lastItem = _awardList.Find(delegate (AwardPoolDT dt) { return dt.mTemplate.mResourceType == tType && dt.mTemplate.mResourceId == tId; });
                        if (lastItem != null)
                            continue;
                        AwardPoolDT item = new AwardPoolDT();
                        item.f_UpdateByInfo((byte)tType, tId, tNum);
                        _awardList.Add(item);
                    }
                    else
                    {
                        MessageBox.ASSERT("Lỗi chuyển đổi kiểu chuỗi phần thưởng hạng, hạng = " + rank);
                    }
                }
            }
        }
        return _awardList;
    }

    public List<AwardPoolDT> f_GetAwardByString(string source)
    {
        _awardList.Clear();
        string[] awardSubGroup = source.Split('#');
        for (int j = 0; j < awardSubGroup.Length; j++)
        {
            string[] awardItem = awardSubGroup[j].Split(';');
            if (awardItem.Length == 3)
            {
                int tType = int.Parse(awardItem[0]);
                if (tType == 0)
                    continue;
                int tId = int.Parse(awardItem[1]);
                int tNum = int.Parse(awardItem[2]);
                //如果找到相同的就忽略不加入
                AwardPoolDT lastItem = _awardList.Find(delegate (AwardPoolDT dt) { return dt.mTemplate.mResourceType == tType && dt.mTemplate.mResourceId == tId; });
                if (lastItem != null)
                    continue;
                AwardPoolDT item = new AwardPoolDT();
                item.f_UpdateByInfo((byte)tType, tId, tNum);
                _awardList.Add(item);
            }
            else
            {
MessageBox.ASSERT("Bonus string conversion error ");
            }
        }
        return _awardList;
    }


    public void f_ExchangeCode(string Code, SocketCallbackDT socketCallBack)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CdkeyAward, socketCallBack.m_ccCallbackSuc, socketCallBack.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(Code, 65);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CdkeyAward, bBuf);
    }
}
