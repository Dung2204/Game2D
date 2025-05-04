using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using System;

public class SevenStarPage : UIFramwork
{

    private UILabel mGoods1Num;
    private UILabel mGoods2Num;

    private UILabel mGoods2Consume;
    private UILabel mGoods1Consume;

    private UILabel mBlessNum;

    private UILabel mActResTime;
    private UILabel mConverResTime;

    private int mGoods1ID = 210;
    private int mGoods2ID = 211;

    private int mGoods2ConsumeNum = 10;
    private int mGoods1ConsumeNum = 1;

    private int mActEndTime;
    private DateTime mResEndTime;

    private int Time_UpdateTimeLable;
    private bool CatLotty;

    private Transform mItemParent;

    private SocketCallbackDT mInfo = new SocketCallbackDT();
    private SocketCallbackDT mLottery = new SocketCallbackDT();
    private SocketCallbackDT mBlessUp = new SocketCallbackDT();

    private bool PlayAni;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        CatLotty = true;
        PlayAni = false;
        mInfo.m_ccCallbackSuc = InfoSuc;
        mInfo.m_ccCallbackFail = InfoFail;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_SevenStarPool.f_InitInfo(mInfo);

    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_UpdateTimeLable);
        PlayAni = false;
        StopCoroutine(f_Circle());
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Lottery", f_Lottery);
        f_RegClickEvent("TSGL", f_BlessLevelUp);
        f_RegClickEvent("MaskBg", f_CloseThis);
        f_RegClickEvent("CJJL", f_OpenList);
        f_RegClickEvent("GWKJL", f_OpenShopCardList);
        f_RegClickEvent("btnHelp", f_Help);
    }
    private string bgpath = "UI/TextureRemove/TrialTower/qxmd_bg_c";
    private string zhuanpath = "UI/TextureRemove/TrialTower/qxmd_qx_shu";
    protected override void InitGUI()
    {
        base.InitGUI();
        mGoods1Num = f_GetObject("Goods1Num").GetComponent<UILabel>();
        mGoods2Num = f_GetObject("Goods2Num").GetComponent<UILabel>();

        mActResTime = f_GetObject("ActResTime").GetComponent<UILabel>();
        mConverResTime = f_GetObject("ConverResTime").GetComponent<UILabel>();
        mBlessNum = f_GetObject("WisNum").GetComponent<UILabel>();
        mGoods2Consume = f_GetObject("Goods2Consume").GetComponent<UILabel>();
        mGoods1Consume = f_GetObject("Goods1Consume").GetComponent<UILabel>();
        mItemParent = f_GetObject("ItemParent").transform;

        mActEndTime = Data_Pool.m_TrialTowerPool.mNowEndTime;

        f_GetObject("AwardList").SetActive(false);
        Time_UpdateTimeLable = ccTimeEvent.GetInstance().f_RegEvent(1f, true, null, UpdateResAndConverTime);
        UpdateItemGoods();

        if (f_GetObject("Bg").GetComponent<UITexture>().mainTexture==null) {
            f_GetObject("Bg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(bgpath);
            f_GetObject("zhuanpan").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(zhuanpath);
        }
    }

    protected override void UpdateGUI()
    {
        base.UpdateGUI();
        mGoods1Num.text = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(mGoods1ID).ToString();
        mGoods2Num.text = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(mGoods2ID).ToString();
        if (Data_Pool.m_SevenStarPool.mBless > 0)
        {
            mBlessNum.text = "[3b992c]" + Data_Pool.m_SevenStarPool.mBless;
        }
        else
        {
            mBlessNum.text = "[ff0000]" + Data_Pool.m_SevenStarPool.mBless.ToString();
        }
        if (Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(mGoods2ID) < mGoods2ConsumeNum)
        {
            mGoods2Consume.text = "[ff0000]x" + mGoods2ConsumeNum;
        }
        else
        {
            mGoods2Consume.text = "[3b992c]x" + mGoods2ConsumeNum;
        }

        if (Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(mGoods1ID) < mGoods1ConsumeNum)
        {
            mGoods1Consume.text = "[ff0000]x" + mGoods1ConsumeNum;
        }
        else
        {
            mGoods1Consume.text = "[3b992c]x" + mGoods1ConsumeNum;
        }

        UpdateItem(0);
    }



    private void UpdateResAndConverTime(object obj)
    {
        int ResTime = mActEndTime - GameSocket.GetInstance().f_GetServerTime();
        int ConverTime = ResTime + 86400;
        if (ResTime<=0) {
            CatLotty = false;
            ResTime = 0;
        }
        int Day = ResTime / 86400;
        ResTime -= Day * 86400;
        int Hours = ResTime / 3600;
        ResTime -= Hours * 3600;
        int Minute = ResTime / 60;
        ResTime -= Minute * 60;
mActResTime.text = string.Format("{0} date {1}:{2}:{3}", Day, Hours, Minute, ResTime);
        Day = ConverTime / 86400;
        ConverTime -= Day * 86400;
        Hours = ConverTime / 3600;
        ConverTime -= Hours * 3600;
        Minute = ConverTime / 60;
        ConverTime -= Minute * 60;
mConverResTime.text = string.Format("{0} date {1}:{2}:{3}", Day, Hours, Minute, ConverTime);
    }


    private void UpdateItem(int ShowIndex)
    {
        for (int i = 0; i <= mItemParent.childCount; i++)
        {
            SetItemActive(mItemParent.Find("Item" + i), ShowIndex == i);
        }
    }

    private void UpdateItemGoods()
    {
        SevenStarLotterySC t = glo_Main.GetInstance().m_SC_Pool.m_SevenStarLotterySC;
        SevenStarLotteryDT tDT;
        ResourceCommonDT ResDt;

        for (int i = 1; i <= mItemParent.childCount; i++)
        {
            tDT = t.f_GetSC(i) as SevenStarLotteryDT;
            ResDt = new ResourceCommonDT();
            ResDt.f_UpdateInfo((byte)tDT.iAwardType, tDT.iAwardId, tDT.iAwardNum);
            UISprite Case = mItemParent.Find("Item" + i + "/" + "Case").GetComponent<UISprite>();
            UI2DSprite Icon = mItemParent.Find("Item" + i + "/" + "Icon").GetComponent<UI2DSprite>();
            UILabel num = mItemParent.Find("Item" + i + "/" + "Num").GetComponent<UILabel>();
            if (i == 1)
            {
                Case.gameObject.SetActive(Data_Pool.m_SevenStarPool.mBless > 0);
                Icon.gameObject.SetActive(Data_Pool.m_SevenStarPool.mBless > 0);
                num.gameObject.SetActive(Data_Pool.m_SevenStarPool.mBless > 0);
                mItemParent.Find("Item" + i + "/" + "lock").gameObject.SetActive(Data_Pool.m_SevenStarPool.mBless <= 0);
                num.text = " ";
                bool isGray = Data_Pool.m_SevenStarPool.mAwardRemain <= 0;
                UITool.f_SetSpriteGray(Case, isGray);
                UITool.f_Set2DSpriteGray(Icon, isGray);
            }
            num.text = ResDt.mResourceNum.ToString();
            Case.spriteName = UITool.f_GetImporentCase(ResDt.mImportant);
            Icon.sprite2D = UITool.f_GetIconSprite(ResDt.mIcon);
            f_RegClickEvent(mItemParent.Find("Item" + i).gameObject, OnClickItem, ResDt);



        }
    }

    private void OnClickItem(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT data = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, data);
    }

    private void SetItemActive(Transform tran, bool isActive)
    {
        if (tran == null)
        {
            return;
        }
        tran.Find("bg").gameObject.SetActive(isActive);
    }


    #region 按钮事件

    private void f_CloseThis(GameObject go, object obj1, object obj2)
    {
        if (PlayAni)
        {
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SevenStarPage, UIMessageDef.UI_CLOSE);
    }

    private void f_Help(GameObject go, object obj1, object obj2) {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage,UIMessageDef.UI_OPEN,12);
    }

    private void f_Lottery(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(mGoods1ID) < 1)
        {
UITool.Ui_Trip("Không đủ vật phẩm");
            return;
        }
        if (!CatLotty)
        {
UITool.Ui_Trip("Hết hạn");
            return;
        }

        if (PlayAni)
        {
            return;
        }

        //mLottery.m_ccCallbackSuc = LotterySuc;
        mLottery.m_ccCallbackFail = LotteryFail;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_SevenStarPool.f_Lottery(mLottery, LotterySuc);
    }

    private void f_BlessLevelUp(GameObject go, object obj1, object obj2)
    {
        if (PlayAni)
        {
            return;
        }
        if (Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(mGoods2ID) < 10)
        {
UITool.Ui_Trip("Không đủ vật phẩm");
            return;
        }

        if (!LocalDataManager.f_GetLocalData<bool>(LocalDataType.SevenStarBlessUp))
        {
PopupMenuParams tParams = new PopupMenuParams("Nhắc nhở", "Sử dụng 10 Linh hồn ngọn lửa vĩnh cửu để mở rộng quy mô?",
           "Xác nhận", PopSuc, "Hủy", PopFail, null, null, PopupMenuParams.PopSaveParam.SevenStarBless);

            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParams);
        }
        else
        {
            PopSuc(null);
        }
    }

    private void f_OpenList(GameObject go, object obj1, object obj2)
    {
        if (PlayAni)
        {
            return;
        }
        f_GetObject("AwardList").GetComponent<SevenStarAwardList>().OpenUI();
    }
    private void f_OpenShopCardList(GameObject go, object obj1, object obj2)
    {
        if (PlayAni)
        {
            return;
        }
        f_GetObject("ShopCardList").GetComponent<ShopCardList>().OpenUI();
    }

    #endregion

    #region 回调事件

    private void LotterySuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UpdateGUI();
        PlayAni = true;
        StartCoroutine(f_Circle());
    }
    private void LotteryFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
UITool.Ui_Trip("Không thành công "+obj.ToString());
        //mLottry = 8;
        //StartCoroutine(f_Circle());
    }

    private void BlessLevelUpSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UpdateGUI();
        UpdateItemGoods();
    }
    private void BlessLevelUpFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
UITool.Ui_Trip("Nâng cấp không thành công" + obj.ToString());
    }

    private void InfoSuc(object obj)
    {

        UITool.f_OpenOrCloseWaitTip(false);
        InitGUI();
        UpdateGUI();
    }

    private void InfoFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
UITool.Ui_Trip("Không thành công" + obj.ToString());
    }

    private void PopSuc(object obj)
    {
        mBlessUp.m_ccCallbackFail = BlessLevelUpFail;
        mBlessUp.m_ccCallbackSuc = BlessLevelUpSuc;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_SevenStarPool.f_BlessLevelUp(mBlessUp);
    }

    private void PopFail(object obj)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_CLOSE);
    }
    #endregion

    #region 转盘
    private int MaxNum = 9;
    private int NowIndex = 0;
    private float Speed = 0;
    private int NowCircle = 0;
    private int MaxCircle = 0;
    private int mLottry = 0;
    private bool StarAddSpeed;
    private IEnumerator f_Circle()
    {
        StarCircle();
        while (PlayAni)
        {
            yield return new WaitForSeconds(Speed);
            if (NowCircle >= 2)
            {
                if (NowIndex == mLottry)
                {
                    StarAddSpeed = true;
                }
                if (StarAddSpeed)
                {
                    Speed += 0.1f;
                }
            }
            CountNowIndex();
            UpdateItem(NowIndex);
            if (StarAddSpeed && NowIndex == mLottry)
            {
                yield return new WaitForSeconds(0.5f);
                PlayAni = false;
                if (mLottry == 1)
                {
                    GetShopCard();
                }
                else
                {
                    OpenGetUI();
                }
                StopCoroutine(f_Circle());
                break;
            }
        }
    }

    private void StarCircle()
    {
        Speed = 0.05f;
        NowIndex = 0;
        NowCircle = 0;
        mLottry = Data_Pool.m_SevenStarPool.mLottry;
        if (mLottry == 0)
        {
            return;
        }
        //mLottry = UnityEngine.Random.Range(1,9);
        StarAddSpeed = false;
        CountNowIndex();
    }

    private void CountNowIndex()
    {
        NowIndex++;
        if (NowIndex >= MaxNum)
        {
            if (Data_Pool.m_SevenStarPool.mBless == 0 || Data_Pool.m_SevenStarPool.mAwardRemain == 0)
            {
                NowIndex = 2;
            }
            else
            {
                NowIndex = 1;
            }
            NowCircle++;
        }
    }



    private void OpenGetUI()
    {
        SevenStarLotteryDT t = glo_Main.GetInstance().m_SC_Pool.m_SevenStarLotterySC.f_GetSC(mLottry) as SevenStarLotteryDT;
        List<AwardPoolDT> awardlist = new List<AwardPoolDT>();
        AwardPoolDT award = new AwardPoolDT();
        award.f_UpdateByInfo((byte)t.iAwardType, t.iAwardId, t.iAwardNum);
        awardlist.Add(award);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { awardlist });

    }

    private void GetShopCard()
    {
UITool.Ui_Trip("Xin chúc mừng bạn đã nhận được thẻ khuyến mại, vui lòng kiểm tra mã thẻ trong Nhật ký");
    }

    #endregion

}
