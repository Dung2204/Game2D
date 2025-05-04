using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;


public class GodEquipBoxInfo
{

    public GodEquipBoxInfo(int idx)
    {
        mIdx = idx;
    }

    public void f_UpdateInfo(int boxId, int boxScore)
    {
        mBoxId = boxId;
        if (mBoxId == 0)
            mBoxState = EM_BoxGetState.Invalid;
        mBoxScore = boxScore;
        //Debug.LogError("mBoxId:" + mBoxId+">>>>:"+ mBoxState);
    }

    public void f_UpdateInfo(EM_BoxGetState boxState)
    {
        mBoxState = boxState;
    }

    public EM_BoxGetState mBoxState
    {
        get;
        private set;
    }
    public int mBoxScore
    {
        get;
        private set;
    }

    public int mBoxId
    {
        get;
        private set;
    }

    public int mIdx
    {
        get;
        private set;
    }
}

public class GodDressPool : BasePool
{
    public List<AwardPoolDT> m_GodAwardList = new List<AwardPoolDT>();
    //当前活动id
    private int curGodDressId = 1;
    //最大积分
    public int maxIntegral;
    //排行数据
    private SC_GodDressRankInfo _GodDressRankInfo;
    //当前宝箱数据
    public GodDressBoxDT mCurGodDressBoxDT
    {
        get;
        private set;
    }
    private GodEquipBoxInfo[] mBoxInfo;


    public GodDressPool() : base("GodDressPoolDT", true)
    {
        _GodDressRankInfo = new SC_GodDressRankInfo();
    }

    protected override void f_Init()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_GodDressSC.f_GetAll();
        for(int i =0;i< listDT.Count;i++)
        {
            GodDressDT dt = listDT[i] as GodDressDT;
            GodDressPoolDT poolDT = new GodDressPoolDT();
            poolDT.iId = dt.iId;
            poolDT.m_GodDressDT = dt;
            poolDT.m_CurAccumIntegral = 0;
            poolDT.m_TodayCurCount = 0;
            poolDT.m_MyRank = 0;
            poolDT.m_Integral = 0;
            f_Save(poolDT);
        }
        curGodDressId = f_GetActId(); //TsuUnlock Comment
        mBoxInfo = new GodEquipBoxInfo[4];
        for (int i = 0; i < mBoxInfo.Length; i++)
        {
            mBoxInfo[i] = new GodEquipBoxInfo(i);
            mBoxInfo[i].f_UpdateInfo(0, 0);
        }
        //初始宝箱数据
        f_UpdateCurTaskBoxDT();
    }


    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }

    protected override void RegSocketMessage()
    {
        //限时神装基本信息
        SC_GodDressInfo scGodDressInfo = new SC_GodDressInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_GodDressInfo, scGodDressInfo, CallbackGodDressInfo);

        //宝箱信息
        SC_GodDressBox scGodDressBoxInfo = new SC_GodDressBox();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_GodDressBox, scGodDressBoxInfo, CallbackGodDressBoxInfo);

        //积分排行信息
        SC_GodDressRankInfo scGodDressRankInfo = new SC_GodDressRankInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_GodDressRankInfo, scGodDressRankInfo, CallbackGodDressRankInfo);

        //购买返还信息
        SC_CenterAwardNode scAward = new SC_CenterAwardNode();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_GodDressBuy, scAward, _AddAward);

    }
    //限时神装基本信息回调
    private void CallbackGodDressInfo(object data)
    {
        SC_GodDressInfo scGodDressData = (SC_GodDressInfo)data;
        if (curGodDressId == 0 )
        {
            return;
        }
        GodDressPoolDT godDressPoolDT = (GodDressPoolDT)f_GetForId((long)curGodDressId);
        godDressPoolDT.m_TodayCurCount = scGodDressData.uTodayScore;
        godDressPoolDT.m_CurAccumIntegral = scGodDressData.uTotalScore;
    }

    //限时神装宝箱回调
    private void CallbackGodDressBoxInfo(object data)
    {
        SC_GodDressBox scGodDressBoxData = (SC_GodDressBox)data;
        byte[] boxArrayState = scGodDressBoxData.uBoxState;
        for (int i= 0;i< boxArrayState.Length;i++)
        {
            //Debug.LogError("状态："+ boxArrayState[i]);
            if(boxArrayState[i] == 0)//不可领取
            {
                mBoxInfo[i].f_UpdateInfo(EM_BoxGetState.Lock);
            }
            else if(boxArrayState[i] == 1)//已领取
            {
                mBoxInfo[i].f_UpdateInfo(EM_BoxGetState.AlreadyGet);
            }
            else //可领取
            {
                mBoxInfo[i].f_UpdateInfo(EM_BoxGetState.CanGet);
            }
        }
    }

    //限时神装积分排名回调
    private void CallbackGodDressRankInfo(object data)
    {
        _GodDressRankInfo = (SC_GodDressRankInfo)data;
        GodDressPoolDT godDressPoolDT = (GodDressPoolDT)f_GetForId((long)curGodDressId);
        if (godDressPoolDT == null) return;
        godDressPoolDT.m_MyRank = _GodDressRankInfo.idx;
        godDressPoolDT.m_Integral = _GodDressRankInfo.uScore;

    }

    private void _AddAward(int iData1, int iData2, int iNum, ArrayList aData)
    {
        m_GodAwardList.Clear();
        foreach (SC_CenterAwardNode item in aData)
        {
            AwardPoolDT tSC_CenterAwardNode = new AwardPoolDT();
            tSC_CenterAwardNode.f_UpdateByInfo(item.uType, item.uId, item.uNum);
            m_GodAwardList.Add(tSC_CenterAwardNode);
        }
    }

    //查询限时神装
    public void f_QueryGodDressInfo(SocketCallbackDT tSocketCallbackDT)
    {
        curGodDressId = f_GetActId();
        f_UpdateCurTaskBoxDT();
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GodDressInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GodDressInfo, bBuf);
    }

    //请求购买神器 1 / 10
    public void f_QueryGodDressBuy(short id,byte buyCount, SocketCallbackDT tSocketCallbackDT)
    {
       
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GodDressBuy, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        tCreateSocketBuf.f_Add(buyCount);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GodDressBuy, bBuf);
      
    }

    //请求领取宝箱
    public void f_QueryGodDressBox(byte index, SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GodDressBox, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(index);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GodDressBox, bBuf);
    }

    //根据服务器时间获取限时活动id
    private int f_GetActId()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_GodDressSC.f_GetAll();
        for(int i =0;i< listDT.Count;i++)
        {
            GodDressDT dt = listDT[i] as GodDressDT;
            int timeNow = GameSocket.GetInstance().f_GetServerTime();
            //TsuCode
            //long timeNow = System.DateTime.Now.Ticks;
            //long timeStart = GetTimeByTimeStr(dt.iBeginTime).Ticks;
            //long timeEnd = GetTimeByTimeStr(dt.iEndTime).Ticks;
            //
            long timeStart = ccMath.DateTime2time_t(GetTimeByTimeStr(dt.iBeginTime));
            long timeEnd = ccMath.DateTime2time_t(GetTimeByTimeStr(dt.iEndTime));
            
            if (timeStart < timeNow && timeNow < timeEnd)
            {
                return dt.iId;
            }
        }
        return 0;
    }

    public DateTime GetTimeByTimeStr(int StrTime)
    {
        //int TimeTike = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + (StrTime-1) * 86400;


        //int bYear = int.Parse(a.Substring(0, 4)); //TsuCode - OpenComment
        //int bMonth = int.Parse(a.Substring(4, 2)); //TsuCode - OpenComment
        //int bDay = int.Parse(a.Substring(6, 2)); //TsuCode - OpenComment
        //DateTime time = ccMath.time_t2DateTime(TimeTike); //OldCode
        //Debug.LogError("time:"+ time);
        //return time; //OldCode
        //TsuCode

        String a = StrTime.ToString(); //TsuCode
        int bYear = int.Parse(a.Substring(0, 4)); //TsuCode - OpenComment
        int bMonth = int.Parse(a.Substring(4, 2)); //TsuCode - OpenComment
        int bDay = int.Parse(a.Substring(6, 2)); //TsuCode - OpenComment
        DateTime start = new DateTime(bYear, bMonth, bDay,0,0,0,0); //TsuCode
        return start; ///TsuCode
        /////
    }

    //根据宝箱表id初始宝箱数据
    private void f_UpdateCurTaskBoxDT()
    {
        GodDressPoolDT godDressPoolDT = (GodDressPoolDT)f_GetForId((long)curGodDressId);
        if (godDressPoolDT == null)
        {
            return;
        }
        List<NBaseSCDT> tmp = glo_Main.GetInstance().m_SC_Pool.m_GodDressBoxSC.f_GetAll();
        GodDressBoxDT tmpItem;
        for (int i = 0; i < tmp.Count; i++)
        {
            tmpItem = (GodDressBoxDT)tmp[i];
            if (godDressPoolDT.m_GodDressDT.iBoxAwardID == tmpItem.iId)
            {
                mCurGodDressBoxDT = tmpItem;
                mBoxInfo[0].f_UpdateInfo(mCurGodDressBoxDT.iAwardBox1,mCurGodDressBoxDT.iParam1);
                mBoxInfo[1].f_UpdateInfo(mCurGodDressBoxDT.iAwardBox2, mCurGodDressBoxDT.iParam2);
                mBoxInfo[2].f_UpdateInfo(mCurGodDressBoxDT.iAwardBox3, mCurGodDressBoxDT.iParam3);
                mBoxInfo[3].f_UpdateInfo(mCurGodDressBoxDT.iAwardBox4, mCurGodDressBoxDT.iParam4);
                maxIntegral = mCurGodDressBoxDT.iParam4;
                f_UpdateBoxInfo();
            }
        }
    }

    //更新宝箱状态
    private void f_UpdateBoxInfo()
    {
        for (int i = 0; i < mBoxInfo.Length; i++)
        {
            mBoxInfo[i].f_UpdateInfo(EM_BoxGetState.Lock);
        }
    }

    //检测活动是否开启
    public bool f_CheckLimitGodOpen()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_GodDressSC.f_GetAll();
        for (int i = 0; i < listDT.Count; i++)
        { 
            
            GodDressDT dt = listDT[i] as GodDressDT;
            int timeNow = GameSocket.GetInstance().f_GetServerTime();
            long timeStart = ccMath.DateTime2time_t(GetTimeByTimeStr(dt.iBeginTime));
            long timeEnd = ccMath.DateTime2time_t(GetTimeByTimeStr(dt.iEndTime));
            //MessageBox.ASSERT("TimeLimitGod: "+  ccMath.time_t2DateTime(timeStart) + " : " + ccMath.time_t2DateTime(timeEnd)); //TsuCode
            //Debug.LogError("timeNow:" + timeNow+ ">>timeStart:"+ timeStart+ ">>timeEnd:"+ timeEnd);
            if (timeStart < timeNow && timeNow < timeEnd)
            {
                return true;
            }
        }
        return false;
    }

    //获取当前显示活动数据
    public GodDressPoolDT f_GetCurPoolDt()
    {
        return (GodDressPoolDT)f_GetForId(curGodDressId);
    }

    //获取排行积分信息
    public SC_GodDressRankInfo m_GodDressRankInfo
    {
        get
        {
            return _GodDressRankInfo;
        }
    }

    public EM_BoxGetState f_GetBoxStateByIdx(int idx)
    {
        if (idx < 0 || idx >= mBoxInfo.Length)
        {
            return EM_BoxGetState.Invalid;
        }
        else
        {
            return mBoxInfo[idx].mBoxState;
        }
    }

    public string f_GetBoxExtraInfo(int idx)
    {
        if (idx < 0 || idx >= mBoxInfo.Length)
            return string.Empty;
        else
return string.Format("{0} score", mBoxInfo[idx].mBoxScore);
    }

    public GodEquipBoxInfo f_GetBoxInfo(int idx)
    {
        if (idx < 0 || idx >= mBoxInfo.Length)
            return null;
        else
            return mBoxInfo[idx];
    }

}
