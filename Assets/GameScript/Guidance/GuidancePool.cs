
using System;
using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuidancePool
{
    public bool m_isFirstEnter = true;
    public GuidancePool()
    {
        RegSocketMessage();
        WeakGuidance = new short[2];
        IGuidanceID = 1;
        IsSave = false;
        _GuidanceType = EM_GuidanceType.FirstLogin;
        tNewbieStepArr.Add((int)EM_GuidanceType.FirstLogin, new short[2] { 1, 1 });
        m_OtherSave = false;
    }
    #region Pool数据管理
    void RegSocketMessage()
    {
        CMsc_SC_NewbieStep tSC_NewbieStep = new CMsc_SC_NewbieStep();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_NewbieStep, tSC_NewbieStep, f_SaveNewbieStep);
    }

    void f_SaveNewbieStep(int iData1, int iData2, int iNum, ArrayList aData)
    {
//        Debug.LogError("==============收到引导推送======iData1============" + iData1);
//        Debug.LogError("==============收到引导推送==========iData2========" + iData2);
//        Debug.LogError("==============收到引导推送========iNum==========" + iNum);
//        Debug.LogError("==============收到引导推送========aData==========" + aData.Count);
//        foreach (var item in aData)
//        {
//            Debug.LogError("===========收到引导推送===aData=============="+item);
//        }
        foreach (CMsc_SC_NewbieStep item in aData)
        {
            //Debug.LogError("=============引导类型：" + (byte)item.idx + "id为：" + item.value[0]);
            if (!tNewbieStepArr.ContainsKey(item.idx))
                tNewbieStepArr.Add(item.idx, item.value);
            else
                tNewbieStepArr[item.idx] = item.value;

        }
        if (!IsSave)
        {
            foreach (KeyValuePair<byte, short[]> t in tNewbieStepArr)
            {
                if (t.Value[0] != 30000)
                {
                    IGuidanceID = t.Value[0];
                    _GuidanceType = (EM_GuidanceType)t.Key;
                    if (m_GuidanceDT != null)
                        break;
                }
            }

            switch (IGuidanceID)
            {
                case 6009:
                    IGuidanceID = 6900;
                    break;
                case 6000:
                    IGuidanceID = 6904;
                    break;
                case 2054:
                    IGuidanceID = 2900;
                    break;
                case 6060:
                    IGuidanceID = 6910;
                    break;
            }

            if (_GuidanceType == EM_GuidanceType.FirstLogin)//在新手引导阶段
            {
                switch (tNewbieStepArr[(int)EM_GuidanceType.FirstLogin][0])
                {
                    case 30000:
                        IGuidanceID = 30000;
                        break;
                    case 5:
                        IGuidanceID = 200;
                        break;
                    case 54:
                        IGuidanceID = 208;
                        break;
                    case 14:
                        IGuidanceID = 211;
                        break;
                    default:
                        IGuidanceID = tNewbieStepArr[(int)EM_GuidanceType.FirstLogin][0];
                        break;
                }
                if (tNewbieStepArr[(int)EM_GuidanceType.FirstLogin][1] != 1)
                {
                    IGuidanceID = 30000;
                }

                //切换账号保存
                LocalDataManager.f_SetLocalData<int>(LocalDataType.FirstGuidance, IGuidanceID);
            }
        }

        //if (iGuidanceID == 30000 || iGuidanceID > 65)//新手教程已完成
        //{
        //    if (m_isFirstEnter)
        //        glo_Main.GetInstance().m_ResourceManager.f_StartUpdateServerFile(true, CallBack_LoadGameResourceComplete);
        //}
        //else
        //{
        //    if (m_isFirstEnter)
        //        glo_Main.GetInstance().m_ResourceManager.f_StartUpdateServerFile(false, CallBack_LoadGameResourceComplete);
        //}
        //m_isFirstEnter = false;
    }
    /// <summary>
    /// 加载load界面
    /// </summary>
    //private void StartLoadRes()
    //{
    //    //if(StaticValue.m_curScene == EM_Scene.Login)
    //    ccUIManage.GetInstance().f_SendMsg(UINameConst.ForceResLoadPage, UIMessageDef.UI_OPEN);
    //    glo_Main.GetInstance().m_ResourceManager.f_StartUpdateServerFile(true, CallBack_LoadGameResourceComplete);
    //}
    /// <summary>
    /// 资源加载完成
    /// </summary>
    private void CallBack_LoadGameResourceComplete(object Obj)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ForceResLoadPage, UIMessageDef.UI_CLOSE);
    }
    public void f_SendSaveNewbieStep(CMsc_SC_NewbieStep tNewbieStep, SocketCallbackDT tSocketCallbackDT)
    {
        IsSave = true;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SetNewbieStep, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tNewbieStep);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SetNewbieStep, bBuf);
        Debug.LogError("====Lưu hướng dẫn tân thủ iTeam：============"+ tNewbieStep.idx + "    id bước： "+tNewbieStep.value[0]);
    }
    #endregion

    private int is_AllEscTime;

    int iGuidanceID;
    public short[] WeakGuidance;
    private GameObject _GuidanceArr;
    private List<Transform> _GuidanceArrBoxList = new List<Transform>();
    GuidanceManage _GuidanceManage;

    public bool IsSave;
    public bool m_OtherSave;
    public Dictionary<byte, short[]> tNewbieStepArr = new Dictionary<byte, short[]>();
    private EM_GuidanceType _GuidanceType;    //当前引导的类型

    public bool IsOpenChanllenge;//是否打开了副本挑战按钮
    public bool IsOpenSween;   //是否打开了扫荡界面
    public bool IsOpenArenaSween;  //是否打开了竞技场扫荡界面
    public bool IsUpdateArena;    //是否刷新了竞技场
    public bool IsOpponent;       //群雄夺宝的选择界面
    public bool IsOpponentSween;       //群雄夺宝的扫荡界面
    public bool IsOpenOneKeyGrabTreasure;  //一键夺宝界面
    public bool IsChange = false;

    public string m_NowOpenUIName = "";   //当前打开的界面 


    public int DungeonResult;
    public bool IsEnter = false;

    public string GuidanceBtnName;
    public ccCallback m_GuidanceCallback;

    public string OpenLevelPageUIName;

    public GuidanceDT m_GuidanceDT;

    private int Time_Save = -99;
    public EM_GuidanceType m_GuidanceType
    {
        get { return _GuidanceType; }
    }
    public int IGuidanceID
    {
        get { return iGuidanceID; }
        set
        {
            iGuidanceID = value;
            if (iGuidanceID == 207)
                iGuidanceID = 10;
            else if (iGuidanceID == 210)
                iGuidanceID = 54;
            else if (iGuidanceID == 6903)
                iGuidanceID = 6010;
            else if (iGuidanceID == 6906)
                iGuidanceID = 6000;
            else if (iGuidanceID == 212)
                iGuidanceID = 14;
            else if (iGuidanceID == 2903)
                iGuidanceID = 2055;
            else if (iGuidanceID == 6912)
                iGuidanceID = 6060;
            if (iGuidanceID == 2001)
                _ChangeUpGuidance(iGuidanceID);
            m_GuidanceDT = glo_Main.GetInstance().m_SC_Pool.m_GuidanceSC.f_GetSC(iGuidanceID) as GuidanceDT;
        }
    }

    private int _Condition;
    public int Condition
    {
        get
        { return _Condition; }

        set
        { _Condition = value; }
    }

    private int _DungeonID;
    public int DungeonID
    {
        get { return _DungeonID; }
        set { _DungeonID = value; }
    }

    public GuidanceManage m_GuidanceManage
    {
        get
        {
            if (_GuidanceManage == null)
            {
                GameObject tGuidance = Resources.Load<GameObject>("GamePrefab/Guidance");
                //GameObject ttGuidance = GameObject.Instantiate<GameObject>(tGuidance);
                GameObject GuidanceArr = (Resources.Load<GameObject>("GamePrefab/GuidanceArr"));
                GameObject ttguidance = NGUITools.AddChild(UICamera.mainCamera.transform.parent.gameObject, tGuidance);
                _GuidanceArr = NGUITools.AddChild(ttguidance, GuidanceArr);
                _GuidanceArr.SetActive(false);
                _GuidanceManage = ttguidance.GetComponent<GuidanceManage>();
                _GuidanceManage.f_Create();
            }
            return _GuidanceManage;
        }
    }

    public GameObject m_GuidanceArr
    {
        get
        {
            if (_GuidanceArr == null)
                _GuidanceArr = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("GamePrefab/GuidanceArr"));
            return _GuidanceArr;
        }
    }

    public int Is_AllEscTime
    {
        get
        {
            GameParamDT tGameParamDT = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.AllGuidanceEsc) as GameParamDT;
            if (tGameParamDT == null)
                return 99;
            else
                return tGameParamDT.iParam1;
        }
    }

    private void _ChangeUpGuidance(int GuidanceID)
    {
        List<NBaseSCDT> tList = glo_Main.GetInstance().m_SC_Pool.m_GuidanceTeamSC.f_GetAll();
        GuidanceTeamDT tGuidanceTeamDT;
        for (int i = 0; i < tList.Count; i++)
        {
            tGuidanceTeamDT = tList[i] as GuidanceTeamDT;
            if (tGuidanceTeamDT.iTrigger != (int)EM_GuidanceType.UpLevel)
                continue;

            if (tGuidanceTeamDT.iCondition == Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level))
            {
                IGuidanceID = 2004;
                iGuidanceID = tGuidanceTeamDT.iGuidanceId;
               if (tNewbieStepArr.ContainsKey((byte)EM_GuidanceType.UpLevel))
               {
                   switch (tGuidanceTeamDT.iCondition)
                   {
                       case 25:
                           iGuidanceID = 2022;
                           return;
                       default:
                           return;
                   }
               }
               else
                   IGuidanceID = 2004;
               return;
            }

        }

    }
    /// <summary>
    /// 开启引导检测
    /// </summary>
    public void f_Enter()
    {
        if (!GloData.glo_StarGuidance || Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) > UITool.f_GetSysOpenLevel(EM_NeedLevel.GuidanceEndLevel))
        {
            return;
        }
        if (_GuidanceManage == null)
        {
            _GuidanceManage = m_GuidanceManage;
            _GuidanceManage.f_Create();
        }
        if (IsEnter)
            return;
        IsEnter = true;
        _GuidanceManage.tManage.f_ChangeState((int)EM_Guidance.GuidanceRead);
    }

    /// <summary>
    /// 切换分支剧情
    /// </summary>
    /// <param name="tType"></param>
    public void f_ChangeGuidanceType(EM_GuidanceType tType)
    {
        if (!GloData.glo_StarGuidance)
            return;
        switch (IsEnter)
        {
            case true:
                return;
            case false:
                break;
        }
//        if (tType == EM_GuidanceType.DungeonResult)
//            return;

        _GuidanceType = tType;
        if (tNewbieStepArr.ContainsKey((byte)tType))
        {
            if (tNewbieStepArr[(byte)tType][0] == 30000)
                return;
        }
        List<NBaseSCDT> tList = glo_Main.GetInstance().m_SC_Pool.m_GuidanceTeamSC.f_GetAll();
        GuidanceTeamDT tGuidanceTeamDT;
        for (int i = 0; i < tList.Count; i++)
        {
            tGuidanceTeamDT = tList[i] as GuidanceTeamDT;
            if (tGuidanceTeamDT.iTrigger == (int)tType)
            {
                switch (tType)
                {
                    case EM_GuidanceType.UpLevel:
                        if (tGuidanceTeamDT.iCondition == Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level))
                        {
                            if (!tNewbieStepArr.ContainsKey((byte)EM_GuidanceType.UpLevel))
                            {
                                tNewbieStepArr.Add((byte)EM_GuidanceType.UpLevel, new short[2]);
                            }

                            IGuidanceID = tGuidanceTeamDT.iSave;
                            f_ChangeSave(m_GuidanceDT);
                            IGuidanceID = 2000;
                            f_Enter();
                            return;
                            #region
                            //switch (tGuidanceTeamDT.iCondition)
                            //{
                            //    case 10:
                            //        if (tNewbieStepArr[(byte)EM_GuidanceType.UpLevel][0] >= 2009)
                            //            continue;
                            //        isChange = true;
                            //        break;
                            //    case 15:
                            //        if (tNewbieStepArr[(byte)EM_GuidanceType.UpLevel][0] != 2009)
                            //        {
                            //            continue;
                            //        }
                            //        else
                            //        {
                            //            isChange = true;
                            //        }
                            //        break;
                            //    case 22:
                            //        if (tNewbieStepArr[(byte)EM_GuidanceType.UpLevel][0] != 2017)
                            //            continue;
                            //        isChange = true;
                            //        break;
                            //    case 28:
                            //        if (tNewbieStepArr[(byte)EM_GuidanceType.UpLevel][0] != 2026)
                            //            continue;
                            //        isChange = true;
                            //        break;
                            //    case 30:
                            //        if (tNewbieStepArr[(byte)EM_GuidanceType.UpLevel][0] != 2035)
                            //            continue;
                            //        isChange = true;
                            //        break;
                            //    case 32:
                            //        if (tNewbieStepArr[(byte)EM_GuidanceType.UpLevel][0] != 2042)
                            //            continue;
                            //        isChange = true;
                            //        break;
                            //    default:
                            //        break;
                            //}
                            #endregion
                        }

                        break;
                    case EM_GuidanceType.Dungeon:
                        break;
                    case EM_GuidanceType.GetAward:

                        if (tGuidanceTeamDT.iCondition == Condition)
                        {
                            IGuidanceID = tGuidanceTeamDT.iGuidanceId;
                            f_Enter();
                            Condition = 0;
                            return;
                        }
                        else
                            continue;
                    case EM_GuidanceType.OnDungeon:
                        break;
                    case EM_GuidanceType.OutDungeon:
                        if (tGuidanceTeamDT.iCondition == DungeonID)
                        {
                            if (tNewbieStepArr.ContainsKey((byte)EM_GuidanceType.OutDungeon))
                            {
                                if (tGuidanceTeamDT.iGuidanceId < tNewbieStepArr[(byte)EM_GuidanceType.OutDungeon][0])
                                {
                                    return;
                                }
                                else if (tNewbieStepArr[(byte)EM_GuidanceType.OutDungeon][0] == 30000)
                                    return;
                            }
                            //如果引导到开宝箱领装备的关卡或者穿上装备的引导而身上又已经装备了的话就不引导
                            if (tGuidanceTeamDT.iGuidanceId == 6000 || tGuidanceTeamDT.iGuidanceId == 6006)
                            {
                                TeamPoolDT tTeamPoolDT;
                                for (int j = 0; j < Data_Pool.m_TeamPool.f_GetAll().Count; j++)
                                {
                                    tTeamPoolDT = Data_Pool.m_TeamPool.f_GetAll()[j] as TeamPoolDT;

                                    if (tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
                                    {
                                        if (tTeamPoolDT.m_aEquipPoolDT[(int)EM_EquipPart.eEquipPart_Weapon - 1] != null)
                                            return;
                                    }
                                }
                            }
                            if (tGuidanceTeamDT.iGuidanceId == 6040)
                            {
                                if (Data_Pool.m_TeamPool.f_GetAll().Count >= 4)
                                {
                                    return;
                                }
                            }
                            IGuidanceID = tGuidanceTeamDT.iGuidanceId;
                            f_ChangeSave(m_GuidanceDT);
                            f_Enter();
                            Condition = 0;
                            return;
                        }
                        else
                            continue;
                    case EM_GuidanceType.DungeonResult:
                        if (tGuidanceTeamDT.iCondition == -1 && DungeonResult == 0)
                        {
                            if (tNewbieStepArr.ContainsKey((byte)EM_GuidanceType.DungeonResult))
                            {
                                if (tNewbieStepArr[(byte)EM_GuidanceType.DungeonResult][0] == 30000)
                                {
                                    return;
                                }
                            }
                            IGuidanceID = tGuidanceTeamDT.iGuidanceId;
                            f_Enter();
                            return;
                        }
                        break;
                    case EM_GuidanceType.RebelArmy:
                        if (tNewbieStepArr.ContainsKey((byte)EM_GuidanceType.RebelArmy))
                        {
                            if (tNewbieStepArr[(byte)EM_GuidanceType.RebelArmy][0] == 30000)
                            {
                                return;
                            }
                        }
                        IGuidanceID = tGuidanceTeamDT.iGuidanceId;
                        f_ChangeSave(m_GuidanceDT);
                        f_Enter();
                        return;
                }
            }
        }
    }


    public void f_SetCurClickButton(string BtnName, ccCallback ccCallback)
    {
        ccUIManage.GetInstance().f_SetCurClickButton(BtnName, ccCallback);
        GuidanceBtnName = BtnName;
        m_GuidanceCallback = ccCallback;
    }

    public void f_SetCurClickButtonNull()
    {
        ccUIManage.GetInstance().f_UnSetCurClickButton();
    }

    public void f_StopGuidance()
    {
        IsEnter = false;
    }

    public void f_SaveNewbie(GuidanceDT _GuidanceDt)
    {
        if (_GuidanceDt == null)
            return;
        if (_GuidanceDt.iSave != 0)
        {
            if (_GuidanceDt.iSave == _GuidanceDt.iId)
            {
                return;
            }
            GuidanceDT tGuidanceDT = glo_Main.GetInstance().m_SC_Pool.m_GuidanceSC.f_GetSC(_GuidanceDt.iSave) as GuidanceDT;
            m_OtherSave = tGuidanceDT == null;
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_Save);
            Time_Save = ccTimeEvent.GetInstance().f_RegEvent(0.02f, true, _GuidanceDt, f_SaveData);
        }
    }
    private void f_SaveData(object obj)
    {
        if (m_OtherSave)
        {
            GuidanceDT _GuidanceDt = (GuidanceDT)obj;
            CMsc_SC_NewbieStep tStep = new CMsc_SC_NewbieStep();
            tStep.idx = (byte)_GuidanceDt.iTeam;
            tStep.value = new short[2] { (short)_GuidanceDt.iSave, 1 };
            SocketCallbackDT tSocketCallbackDT = new SocketCallbackDT();
            tSocketCallbackDT.m_ccCallbackFail = SaveFail;
            tSocketCallbackDT.m_ccCallbackSuc = SaveSuc;
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_GuidancePool.f_SendSaveNewbieStep(tStep, tSocketCallbackDT);
        }
    }

    public void f_TestSave(int type, int saveId)
    {
        #if DEBUG
            CMsc_SC_NewbieStep tStep = new CMsc_SC_NewbieStep();
            tStep.idx = (byte)type;
            tStep.value = new short[2] { (short)saveId, 1 };
            CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
            tCreateSocketBuf.f_Add(tStep);
            byte[] bBuf = tCreateSocketBuf.f_GetBuf();
            GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SetNewbieStep, bBuf);
        #endif
    }

    public void f_Skip_GuidanceDtiTeam()
    {
        CMsc_SC_NewbieStep tStep = new CMsc_SC_NewbieStep();
        tStep.idx = (byte) 1;
        tStep.value = new short[2] { (short)30000, 1 };
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tStep);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SetNewbieStep, bBuf);
    }

    private void SaveSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_Save);
    }
    private void SaveFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
    }


    private void f_ChangeSave(GuidanceDT _GuidanceDt)
    {
        if (_GuidanceDt == null)
            return;
        if (_GuidanceDt.iSave != 0)
        {
            CMsc_SC_NewbieStep tStep = new CMsc_SC_NewbieStep();
            tStep.idx = (byte)_GuidanceDt.iTeam;
            tStep.value = new short[2] { (short)_GuidanceDt.iSave, 1 };
            SocketCallbackDT tSocketCallbackDT = new SocketCallbackDT();
            tSocketCallbackDT.m_ccCallbackFail = SaveFail;
            tSocketCallbackDT.m_ccCallbackSuc = SaveSuc;
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_GuidancePool.f_SendSaveNewbieStep(tStep, tSocketCallbackDT);
        }
    }

    public void f_SetArrNull()
    {
        GameObject.Destroy(_GuidanceManage.gameObject);
    }
}

