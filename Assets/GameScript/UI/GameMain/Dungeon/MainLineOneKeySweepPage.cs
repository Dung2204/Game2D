using System;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using UnityEngine;

public class MainLineOneKeySweepPage : UIFramwork
{

    private GameObject mBtnMinusTen;
    private GameObject mBtnMinusOne;
    private GameObject mBtnAddOne;
    private GameObject mBtnAddTen;
    private GameObject mBtnSure;
    private GameObject mBtnClose;
    private GameObject mBtnCancel;

    private UILabel mSweepNum;
    private UILabel mGoodName;
    private UILabel mCanSweepNum;
    private UI2DSprite mIcon;
    private UISprite mBorder;
    private UIToggle mToggle;

    private int mMaxSweepNum = 0;
    private int mMaxSweepNumByEnergy = 0;

    private int mInputNum = 0;

    private bool isRequestingDungeon = false;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mBtnMinusTen = f_GetObject("BtnMinusTen");
        mBtnMinusOne = f_GetObject("BtnMinusOne");
        mBtnAddOne = f_GetObject("BtnAddOne");
        mBtnAddTen = f_GetObject("BtnAddTen");
        mBtnSure = f_GetObject("BtnSure");
        mBtnClose = f_GetObject("BtnClose");
        mBtnCancel = f_GetObject("BtnCancel");

        mSweepNum = f_GetObject("UseNum").GetComponent<UILabel>();
        mGoodName = f_GetObject("GoodName").GetComponent<UILabel>();
        mCanSweepNum = f_GetObject("HaveNum").GetComponent<UILabel>();
        mIcon = f_GetObject("Icon").GetComponent<UI2DSprite>();
        mBorder = f_GetObject("Frame").GetComponent<UISprite>();
        mToggle = f_GetObject("ToggleConsume").GetComponent<UIToggle>();

        f_RegClickEvent(mBtnClose, OnBlackBGClick);
        f_RegClickEvent(mBtnCancel, OnBlackBGClick);
        f_RegClickEvent(mBtnAddOne, f_ClickAddOrMinus, 1);
        f_RegClickEvent(mBtnAddTen, f_ClickAddOrMinus, 10);
        f_RegClickEvent(mBtnMinusTen, f_ClickAddOrMinus, -10);
        f_RegClickEvent(mBtnMinusOne, f_ClickAddOrMinus, -1);
        f_RegClickEvent("MaskClose", OnBlackBGClick);

        f_RegClickEvent(mBtnSure, f_OnClickSweep);
    }
    /// <summary>
    /// 点击一键扫荡
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_OnClickSweep(GameObject go, object value1, object value2)
    {
        if (isRequestingDungeon) return;
        string id = cardFragmentDt.szStage.Split(new string[] { ";" }, StringSplitOptions.None)[0];
        DungeonTollgatePoolDT tollgatePoolDt = Data_Pool.m_DungeonPool.f_GetTollgatePoolDTByType(EM_Fight_Enum.eFight_DungeonMain);

        //ShopResourcePoolDT tShopPoolDt = (ShopResourcePoolDT)Data_Pool.m_ShopResourcePool.f_GetShopPoolDtByTypeAndId((int)EM_ResourceType.Good, GameParamConst.EnergyGoodId);
        int energyGoodNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GameParamConst.EnergyGoodId);
        if (!mToggle.value || energyGoodNum <= 0)
        {
            if (!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Energy, tollgatePoolDt.mTollgateTemplate.iEnergyCost, true, true, this))
            {
                return;
            }
        }
        

        //发送扫荡请求
        int isUseProp = mToggle.value ? 1 : 0;
        SocketCallbackDT tCallBackDT = new SocketCallbackDT();
        tCallBackDT.m_ccCallbackSuc = CallBack_DungeonSweep_Suc;
        tCallBackDT.m_ccCallbackFail = CallBack_DungeonSweep_Fail;
        
        Data_Pool.m_DungeonPool.f_MainLineOneKeySweep(int.Parse(id), (int)EM_Fight_Enum.eFight_DungeonMain, mInputNum, cardFragmentDt.iId, isUseProp, tCallBackDT);
        UITool.f_OpenOrCloseWaitTip(true);
        isRequestingDungeon = true;
    }
    /// <summary>
    /// 扫荡完成回调
    /// </summary>
    /// <param name="result"></param>
    private void CallBack_DungeonSweep_Suc(object result)
    {
        isRequestingDungeon = false;
        UITool.f_OpenOrCloseWaitTip(false);
        MessageBox.DEBUG("DungeonSweep Suc! code:" + result);
        //打开奖励显示界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonSweepPage, UIMessageDef.UI_OPEN, Data_Pool.m_DungeonPool.f_GetSweepResult());
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MainLineOneKeySweepPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 扫荡失败回调
    /// </summary>
    /// <param name="result"></param>
    private void CallBack_DungeonSweep_Fail(object result)
    {
        isRequestingDungeon = false;
        UITool.f_OpenOrCloseWaitTip(false);
        MessageBox.DEBUG("DungeonSweep Fail! code:" + result);
    }
    /// <summary>
    /// 设置panel
    /// </summary>
    /// <param name="dt"></param>
    private void f_SetPanel(CardFragmentDT dt)
    {
        mIcon.sprite2D = UITool.f_GetIconSpriteByCardId(dt.iNewCardId);
        mIcon.MakePixelPerfect();
        string name = dt.szName;
        mBorder.spriteName = UITool.f_GetImporentColorName(dt.iImportant, ref name);
        mGoodName.text = name;
        mCanSweepNum.text = string.Format(CommonTools.f_GetTransLanguage(2168), mMaxSweepNum);//"可扫荡次数：" +mMaxSweepNum.ToString();
        mInputNum = Math.Min(mMaxSweepNumByEnergy, mMaxSweepNum);
        mInputNum = Math.Max(1, mInputNum);
        mSweepNum.text = mInputNum.ToString();
        
    }
    /// <summary>
    /// 点击+号-号
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_ClickAddOrMinus(GameObject go, object value1, object value2)
    {
        int num = int.Parse(value1.ToString());
        
        mInputNum = mInputNum + num;
        mInputNum = Math.Max(1, mInputNum);
        mInputNum = Math.Min(mInputNum, mMaxSweepNum);
        mSweepNum.text = mInputNum.ToString();
    }
    /// <summary>
    /// 设置最大可扫荡次数
    /// </summary>
    /// <param name="dt"></param>
    private void f_SetMaxSweepNum(CardFragmentDT dt)
    {
        string[] ids = dt.szStage.Split(new string[] { ";" }, StringSplitOptions.None);
        List<DungeonTollgatePoolDT> poolDt = new List<DungeonTollgatePoolDT>();//Data_Pool.m_DungeonPool.f_GetAllNoMaxStar((int)EM_Fight_Enum.eFight_DungeonMain);

        for (int i = 0; i < ids.Length - 1; i++)
        {
            int id = int.Parse(ids[i]);
            DungeonTollgateDT dungeonTollgateDt = glo_Main.GetInstance().m_SC_Pool.m_DungeonTollgateSC.f_GetSC(id) as DungeonTollgateDT;
            string chapterId = dungeonTollgateDt.szTollgateDesc.Split(new string[] { "-" }, StringSplitOptions.None)[0];
            DungeonPoolDT dungeonPoolDt = (DungeonPoolDT)Data_Pool.m_DungeonPool.f_GetForId(long.Parse(chapterId));
            DungeonTollgatePoolDT dungeonTollgatePoolDt = dungeonPoolDt.f_GetTollgateData(id);
            if (dungeonTollgatePoolDt.mStarNum >= 3)
            {
                poolDt.Add(dungeonTollgatePoolDt);
                mMaxSweepNum = dungeonTollgatePoolDt.mTollgateTemplate.iCountLimit - dungeonTollgatePoolDt.mTimes + mMaxSweepNum;

                int tEnergyValue = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy);
                int limitEnergy = tEnergyValue -
                                  dungeonTollgatePoolDt.mTollgateTemplate.iEnergyCost * mMaxSweepNumByEnergy;
                int num = Math.Min(mMaxSweepNum, limitEnergy / dungeonTollgatePoolDt.mTollgateTemplate.iEnergyCost);
                mMaxSweepNumByEnergy = mMaxSweepNumByEnergy + num;
            } 
        }
    }

    private CardFragmentDT cardFragmentDt = new CardFragmentDT();
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        CardFragmentDT dt = e as CardFragmentDT;
        cardFragmentDt = dt;
        mMaxSweepNum = 0;
        mMaxSweepNumByEnergy = 0;
        mInputNum = 0;
        f_SetMaxSweepNum(dt);
        f_SetPanel(dt);
    }

    private void OnBlackBGClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MainLineOneKeySweepPage, UIMessageDef.UI_CLOSE);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.MainMenu, UIMessageDef.UI_OPEN);
    }
}
