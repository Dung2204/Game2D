using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using System;

public class SevenStarBlessPool : BasePool
{

    public List<SC_SevenStarInfoNode> mRecord;
    public List<ShopCardParam> mShopCardList;

    public int mAwardRemain;

    public int mBless;

    public int mLottry;

    private ccCallback LottoryUI;
    public SevenStarBlessPool() : base("SevenStarBlessPoolDT", false)
    {
        mShopCardList = new List<ShopCardParam>();
        mRecord = new List<SC_SevenStarInfoNode>();
        mAwardRemain = 1;
    }

    protected override void f_Init()
    {

    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }

    private void f_Socket_DelData(SockBaseDT Obj)
    {

    }

    protected override void RegSocketMessage()
    {
        SC_SevenStarInfo tSC_SevenStarInfo = new SC_SevenStarInfo();
        SC_SevenStarInfoNode tSC_SevenStarInfoNode = new SC_SevenStarInfoNode();
        SC_SevenStarBlessLevelUp tSC_SevenStarBlessLevelUp = new SC_SevenStarBlessLevelUp();
        SC_SevenStarLottery tSC_SevenStarLottery = new SC_SevenStarLottery();
        SC_SevenStarGiftAwardKey tSC_SevenStarGiftAwardKey = new SC_SevenStarGiftAwardKey();
        SC_SevenStarGiftAwardList tSC_SevenStarGiftAwardList = new SC_SevenStarGiftAwardList();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_SevenStarLotteryAwardList, tSC_SevenStarInfoNode, UpdateInfo);
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_SevenStarBlessLevelUp, tSC_SevenStarBlessLevelUp, UpdateBless);
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_SevenStarLottery, tSC_SevenStarLottery, UpdateLottery);
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_SevenStarInfo, tSC_SevenStarInfo, UpdateInfo);
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_SevenStarGiftAwardList, tSC_SevenStarGiftAwardList, InfoShopCard);
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_SevenStarGiftAwardKey, tSC_SevenStarGiftAwardKey, InfoShopCardKey);
    }
    protected void UpdateInfo(int iData1, int iData2, int iNum, ArrayList aData)
    {
        mRecord.Clear();
        // = iData2;
        foreach (SockBaseDT tData in aData)
        {
            mRecord.Add((SC_SevenStarInfoNode)tData);
        }
    }

    private void UpdateInfo(object obj)
    {
        SC_SevenStarInfo tSC_SevenStarInfo = (SC_SevenStarInfo)obj;
        mBless = tSC_SevenStarInfo.uBlessValue;
        mAwardRemain = tSC_SevenStarInfo.uAwardRemain;


    }

    private void UpdateBless(object obj)
    {
        mBless = ((SC_SevenStarBlessLevelUp)obj).uBlessValue;
    }

    private void UpdateLottery(object obj)
    {
        SC_SevenStarLottery tSC_SevenStarLottery = (SC_SevenStarLottery)obj;
        mLottry = tSC_SevenStarLottery.uId;
        mAwardRemain = tSC_SevenStarLottery.uAwardRemain;
        if (LottoryUI != null)
        {
            LottoryUI(obj);
        }
    }

    private void InfoShopCard(int iData1, int iData2, int iNum, ArrayList aData)
    {

        foreach (SC_SevenStarGiftAwardList tData in aData)
        {
            ShopCardParam tParam = mShopCardList.Find((ShopCardParam a) =>
            {
                if (tData.uId == a.ID)
                {
                    return true;
                }
                return false;
            });
            if (tParam == null)
            {
                tParam = new ShopCardParam();
                tParam.ID = tData.uId;
                tParam.Time = tData.iTime;
                mShopCardList.Add(tParam);
            }

        }
        mShopCardList.Sort(SortList);
    }

    private int SortList(ShopCardParam a, ShopCardParam b)
    {
        if (a.Time > b.Time) return -1;
        else if (a.Time < b.Time) return 1;
        else return 0;
    }

    private void InfoShopCardKey(object obj)
    {
        SC_SevenStarGiftAwardKey tSC_SevenStarGiftAwardKey = (SC_SevenStarGiftAwardKey)obj;

        ShopCardParam tParam = mShopCardList.Find((ShopCardParam a) =>
        {
            if (tSC_SevenStarGiftAwardKey.uid == a.ID)
            {
                return true;
            }
            return false;
        });
        if (tParam != null)
        {
            tParam.Key = tSC_SevenStarGiftAwardKey.szKey;
        }

    }


    #region 外部接口    
    public void f_InitInfo(SocketCallbackDT callback)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SevenStarLightInfo, callback.m_ccCallbackSuc, callback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SevenStarLightInfo, bBuf);
    }

    public void f_GetAwardList(SocketCallbackDT callback)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SevenStarLightAwardList, callback.m_ccCallbackSuc, callback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SevenStarLightAwardList, bBuf);
    }

    public void f_Lottery(SocketCallbackDT callback, ccCallback LotteryUI)
    {
        this.LottoryUI = LotteryUI;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SevenStarLightLottery, callback.m_ccCallbackSuc, callback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SevenStarLightLottery, bBuf);
    }

    public void f_BlessLevelUp(SocketCallbackDT callback)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SevenStarBlessLevelUp, callback.m_ccCallbackSuc, callback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SevenStarBlessLevelUp, bBuf);
    }

    public void f_AwardList(SocketCallbackDT callback)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SevenStarGiftAwardList, callback.m_ccCallbackSuc, callback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SevenStarGiftAwardList, bBuf);
    }

    public void f_GetShopCardKey(int id, SocketCallbackDT callback)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SevenStarGiftAwardKey, callback.m_ccCallbackSuc, callback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SevenStarGiftAwardKey, bBuf);
    }
    #endregion

}
