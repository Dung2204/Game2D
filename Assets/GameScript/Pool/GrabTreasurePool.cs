using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 夺宝Pool
/// </summary>
public class GrabTreasurePool : BasePool
{
    public SC_GrabTreasureInfoNode [] robotInfoArray;//6个固定机器人信息
    public GrabTreasurePoolDT m_GrabTreasurePoolDT;//单次夺宝的结算保存
    public bool m_IsNeedChangeToRobotPage = false;//是否需要切换至机器人页面
    public bool m_GrabIsFinish = false;//夺宝是否结算
    public bool m_GrabSweepIsFinish = false;//夺宝扫荡是否结算
    public int m_OnKeyGrabTreasureOpenLv = 80;

    public List<OneKeyFragmentInfo> m_GrabTreasureOneKeyResult = new List<OneKeyFragmentInfo>();
    private ccCallback m_SweepCompCallback = null;//夺宝扫荡结果回调
    /// <summary>
    /// 构造
    /// </summary>
    public GrabTreasurePool()
        : base("GrabTreasurePoolDT" , true)
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        Init();
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void Init()
    {

    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void RegSocketMessage()
    {
        //机器人信息
        SC_GrabTreasureInfo scGrabTreasureInfo = new SC_GrabTreasureInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_GrabTreasureInfo , scGrabTreasureInfo , OnGrabTreasureInfoCallback);
        //夺宝结算
        SC_GrabTreasure scGrabTreasure = new SC_GrabTreasure();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_GrabTreasure , scGrabTreasure , OnGrabTreasureResultCallback);
        SC_GrabTreasure scGrabTreasureSweep = new SC_GrabTreasure();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_GrabTreasureSweep , scGrabTreasureSweep , OnGrabTreasureSweepResultCallback);

        OneKeyFragmentInfo scGrabTreasureOneKey = new OneKeyFragmentInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_GrabTreasureOneKey , scGrabTreasureOneKey , GrabTreasureOnKey);
    }
    /// <summary>
    /// 夺宝机器人信息回调
    /// </summary>
    private void OnGrabTreasureInfoCallback(object obj)
    {
        SC_GrabTreasureInfo scGrabTreasureInfo = (SC_GrabTreasureInfo)obj;
        robotInfoArray = scGrabTreasureInfo.grabTreasureInfoNodeInfo;
    }
    /// <summary>
    /// 夺宝结算回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGrabTreasureResultCallback(object obj)
    {
        SC_GrabTreasure scGrabTreasure = (SC_GrabTreasure)obj;
        m_GrabTreasurePoolDT = new GrabTreasurePoolDT();
        m_GrabTreasurePoolDT.iId = 0;
        m_GrabTreasurePoolDT.star = scGrabTreasure.star;
        m_GrabTreasurePoolDT.awardID = scGrabTreasure.awardId;
        m_GrabTreasurePoolDT.resourceType = scGrabTreasure.awardNode.resourceType;
        m_GrabTreasurePoolDT.resourceId = scGrabTreasure.awardNode.resourceId;
        m_GrabTreasurePoolDT.resourceNum = scGrabTreasure.awardNode.resourceNum;
        m_GrabTreasurePoolDT.treaFragID = scGrabTreasure.treaFragId;
        m_GrabIsFinish = true;
    }
    /// <summary>
    /// 夺宝扫荡结算回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGrabTreasureSweepResultCallback(int iData1 , int iData2 , int iNum , ArrayList aData)
    {
        int index = 0;
        foreach (SockBaseDT obj in aData)
        {
            SC_GrabTreasure scGrabTreasure = (SC_GrabTreasure)obj;
            GrabTreasurePoolDT poolDT = new GrabTreasurePoolDT();
            poolDT.iId = index;
            poolDT.star = scGrabTreasure.star;
            poolDT.awardID = scGrabTreasure.awardId;
            poolDT.resourceType = scGrabTreasure.awardNode.resourceType;
            poolDT.resourceId = scGrabTreasure.awardNode.resourceId;
            poolDT.resourceNum = scGrabTreasure.awardNode.resourceNum;
            poolDT.treaFragID = scGrabTreasure.treaFragId;
            f_Save(poolDT);
            index++;
        }
        m_GrabSweepIsFinish = true;
        if (m_SweepCompCallback != null)
        {
            m_SweepCompCallback(null);
            m_SweepCompCallback = null;
        }
    }
    /// <summary>
    /// 一键夺宝返回   (最多5次)
    /// </summary>
    /// <param name="iData1"></param>
    /// <param name="iData2"></param>
    /// <param name="iNum"></param>
    /// <param name="aData"></param>
    private void GrabTreasureOnKey(int iData1 , int iData2 , int iNum , ArrayList aData)
    {

        foreach (SockBaseDT date in aData)
        {
            OneKeyFragmentInfo scGrabTreasure = (OneKeyFragmentInfo)date;
            m_GrabTreasureOneKeyResult.Add(scGrabTreasure);
        }

    }

    protected override void f_Socket_AddData(SockBaseDT Obj , bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #region 外部接口

    /// <summary>
    /// 查询机器人信息
    /// treaFragId碎片id
    /// </summary>
    public void f_RequestRobotInfo(short treaFragId , SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GrabTreasureInfo , tSocketCallbackDT.m_ccCallbackSuc , tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(treaFragId);
        byte [] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GrabTreasureInfo , bBuf);
    }
    /// <summary>
    /// 夺宝请求
    /// </summary>
    /// <param name="treasureFragId">碎片id</param>
    /// <param name="robotIndex">目标合成法宝id</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_GrabTreasure(short treasureFragId , byte robotIndex , SocketCallbackDT tSocketCallbackDT)
    {
        m_GrabIsFinish = false;
        //更新战斗数据
        StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_GrabTreasure , 0 , 0 , 0);
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GrabTreasure , tSocketCallbackDT.m_ccCallbackSuc , tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(treasureFragId);
        tCreateSocketBuf.f_Add(robotIndex);
        byte [] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GrabTreasure , bBuf);
    }
    /// <summary>
    /// 夺宝扫荡请求
    /// </summary>
    public void f_GrabTreasureSweep(short treasureFragId , byte robotIndex , SocketCallbackDT tSocketCallbackDT , ccCallback onSweepComCallback)
    {
        f_Clear();
        m_GrabSweepIsFinish = false;
        this.m_SweepCompCallback = onSweepComCallback;
        //更新战斗数据
        StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_GrabTreasureSweep , 0 , 0 , 0);
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GrabTreasureSweep , tSocketCallbackDT.m_ccCallbackSuc , tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(treasureFragId);
        tCreateSocketBuf.f_Add(robotIndex);
        byte [] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GrabTreasureSweep , bBuf);
    }
    /// <summary>
    /// 一键合成法宝
    /// </summary>
    /// <param name="destTreasureID">目标合成法宝id</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_TreasureSynthes(int destTreasureID , int num , SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TreasureSynthesisOnKey , tSocketCallbackDT.m_ccCallbackSuc , tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(destTreasureID);
        tCreateSocketBuf.f_Add(num);
        byte [] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TreasureSynthesisOnKey , bBuf);
    }
    /// <summary>
    /// 一键夺宝
    /// </summary>
    public void f_GrabTreasureOneKey(int tTreasureId , SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GrabTreasureOneKey , tSocketCallbackDT.m_ccCallbackSuc , tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tTreasureId);
        byte [] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GrabTreasureOneKey , bBuf);
    }

    #endregion
}
