using UnityEngine;
using System.Collections.Generic;
using ccU3DEngine;

public class LegionSacrifice : UIFramwork
{
    //LegionInfo
    private UILabel mLegionName;
    private UILabel mLegionLv;
    private UILabel mLegionContri;
    private UILabel mLegionExp;
    private UISlider mLegionExpSlider;    //军团经验

    //Role
    private Transform NpcPos;
    private GameObject NpcMode;
    private UILabel NpcText;
    private UILabel NpcName;

    //topRight  宝箱
    private UILabel _SacrificeNum;
    private UILabel _SacrificePeople;
    private UISlider SacrificeSlider;   //当天祭天进度
    private Transform divide1;
    private Transform divide2;
    private Transform divide3;
    private Transform divide4;

    //SacrificeType
    private LegionSacrificeTypeItem[] mTypeItems;

    private GameObject mSureBtn;

    //单前祭天类型
    private int curSacrificeType = 0;
    //单前祭天类型模板数据
    private LegionSacrificeDT curSacrificeTypeDt = null;


    private const string szCenterBgFile = "UI/TextureRemove/Legion/Tex_LegionSacrificeBg";
    private const string szRoleBgFile = "UI/TextureRemove/Legion/Tex_LegionSacrificeBoxBg";

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitUI();
    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        UpdateMain(null);
        f_OpenOrCloseMonenyPage(true);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        f_LoadTexture();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        f_OpenOrCloseMonenyPage(false);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
    }

    private void f_LoadTexture()
    {
        f_GetObject("TextureBg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szCenterBgFile);
        f_GetObject("RoleBg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szRoleBgFile);
    }

    private void f_OnSacrificeSelectBtn(GameObject go, object value1, object value2)
    {
        if (LegionMain.GetInstance().m_LegionInfor.m_iSacrificeType > 0)
        {
            if (curSacrificeType == LegionMain.GetInstance().m_LegionInfor.m_iSacrificeType)
                return;
            curSacrificeType = LegionMain.GetInstance().m_LegionInfor.m_iSacrificeType;
        }
        else
        {
            if (curSacrificeType == (int)value1)
                return;
            curSacrificeType = (int)value1;
        }
        curSacrificeTypeDt = glo_Main.GetInstance().m_SC_Pool.m_LegionSacrificeSC.f_GetSC(curSacrificeType) as LegionSacrificeDT;
        for (int i = 0; i < mTypeItems.Length; i++)
        {
            mTypeItems[i].f_UpdatebyInfo(LegionMain.GetInstance().m_LegionInfor.m_iSacrificeType, curSacrificeType, glo_Main.GetInstance().m_SC_Pool.m_LegionSacrificeSC.f_GetSC(i + 1) as LegionSacrificeDT);
        }
        UITool.f_CreateRoleByModeId(curSacrificeTypeDt.iNpc, ref NpcMode, NpcPos, 1);
        if (LegionMain.GetInstance().m_LegionInfor.m_iSacrificeType > 0)
        {
            NpcText.text = curSacrificeTypeDt.szAlreadyText;
        }
        else
        {
            NpcText.text = curSacrificeTypeDt.szNotText;
        }
        switch (curSacrificeType)
        {
            case 1:
                NpcName.text = CommonTools.f_GetTransLanguage(704);//初级祭天管
                break;
            case 2:
                NpcName.text = CommonTools.f_GetTransLanguage(705);//中级祭天管
                break;
            case 3:
                NpcName.text = CommonTools.f_GetTransLanguage(706);//高级祭天官
                break;
        }
    }

    #region 按钮信息
    void f_BackBtn(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionSacrifice, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    void OpenBox(GameObject gp, object obj1, object obj2)
    {
        int t = (int)obj1;
        byte tByte = (byte)t;
        f_GetObject("Award").SetActive(true);
        UpdateBox((LegionAwardDT)obj2, tByte);
    }
    void UI_Suc(GameObject gp, object obj1, object obj2)
    {

        if (curSacrificeTypeDt == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(707));//请选择一个祭天类型
            return;
        }
        else if (LegionMain.GetInstance().m_LegionInfor.m_iSacrificeType != 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(708));//今天已经祭天
            return;
        }
        else if (!UITool.f_IsEnoughMoney((EM_MoneyType)curSacrificeTypeDt.iCostType, curSacrificeTypeDt.iCostCount, true, this))
        {
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tSocketCallBackDT = new SocketCallbackDT();
        tSocketCallBackDT.m_ccCallbackSuc = f_Callback_SacrificeResult;
        tSocketCallBackDT.m_ccCallbackFail = f_Callback_SacrificeResult;
        LegionMain.GetInstance().m_LegionInfor.f_SendSacrifice((byte)curSacrificeType, tSocketCallBackDT);
    }

    void f_Callback_SacrificeResult(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(709));//祭天成功
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(710));//祭天出错
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(711) + result);
        }
        LegionMain.GetInstance().m_LegionInfor.f_SendSacrificeInfo(UpdateMain);
    }

    #endregion

    void InitUI()
    {
        mLegionName = f_GetObject("LegionName").GetComponent<UILabel>();
        mLegionLv = f_GetObject("LegionLv").GetComponent<UILabel>();
        mLegionContri = f_GetObject("LegionContril").GetComponent<UILabel>();
        mLegionExpSlider = f_GetObject("LegionExpSlider").GetComponent<UISlider>();
        mLegionExp = f_GetObject("LegionExp").GetComponent<UILabel>();

        NpcPos = f_GetObject("NpcPos").transform;
        NpcText = f_GetObject("NpcText").GetComponent<UILabel>();
        NpcName = f_GetObject("NpcName").GetComponent<UILabel>();

        _SacrificeNum = f_GetObject("sacrificeNum").GetComponent<UILabel>();
        _SacrificePeople = f_GetObject("sacrificePeople").GetComponent<UILabel>();
        SacrificeSlider = f_GetObject("SacrificeSlider").GetComponent<UISlider>();
        divide1 = f_GetObject("divide1").transform;
        divide2 = f_GetObject("divide2").transform;
        divide3 = f_GetObject("divide3").transform;
        divide4 = f_GetObject("divide4").transform;

        mTypeItems = new LegionSacrificeTypeItem[3];
        for (int i = 0; i < mTypeItems.Length; i++)
        {
            mTypeItems[i] = f_GetObject(string.Format("SacrificeTypeItem{0}", i + 1)).GetComponent<LegionSacrificeTypeItem>();
            f_RegClickEvent(mTypeItems[i].m_SelectBtn, f_OnSacrificeSelectBtn, i + 1);
        }
        mSureBtn = f_GetObject("SureBtn");
        f_RegClickEvent(mSureBtn, UI_Suc);
        f_RegClickEvent("BackBtn", f_BackBtn);
    }

    #region  主界面

    void UpdateMain(object obj)
    {
        LegionInfoPoolDT tLegion = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion();

        //update LegionInfo
        int tLeigonLv = tLegion.f_GetProperty((int)EM_LegionProperty.Lv);
        mLegionName.text = tLegion.LegionName;
        mLegionLv.text = tLeigonLv.ToString();
        int tExpMax = LegionTool.f_GetLvUpExpValue(tLeigonLv);
        int tExpCur = tLegion.f_GetProperty((int)EM_LegionProperty.Exp);
        mLegionExpSlider.value = tExpMax == 0 ? 1.0f : tExpCur / (float)tExpMax;
        mLegionExp.text = string.Format("{0}/{1}", tExpCur, tExpMax);
        mLegionContri.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_LegionContribution).ToString();

        //宝箱
        LegionAwardDT tLegionAward = glo_Main.GetInstance().m_SC_Pool.m_LegionAwardSC.f_GetSC(tLeigonLv) as LegionAwardDT;
        SacrificeSlider.value = (float)LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice / tLegionAward.iMax;
        float tSliderLength = 990;
        float tMax = tLegionAward.iMax;
        divide1.localPosition = new Vector2(((float)tLegionAward.iNeedNum1 / tMax) * tSliderLength+60, 0);
        divide2.localPosition = new Vector2(((float)tLegionAward.iNeedNum2 / tMax) * tSliderLength, 0);
        divide3.localPosition = new Vector2(((float)tLegionAward.iNeedNum3 / tMax) * tSliderLength, 0);
        divide4.localPosition = new Vector2(((float)tLegionAward.iNeedNum4 / tMax) * tSliderLength, 0);
        divide1.Find("Num").GetComponent<UILabel>().text = tLegionAward.iNeedNum1.ToString();
        divide2.Find("Num").GetComponent<UILabel>().text = tLegionAward.iNeedNum2.ToString();
        divide3.Find("Num").GetComponent<UILabel>().text = tLegionAward.iNeedNum3.ToString();
        divide4.Find("Num").GetComponent<UILabel>().text = tLegionAward.iNeedNum4.ToString();
        f_RegClickEvent(divide1.transform.Find("Box").gameObject, OpenBox, 1, tLegionAward);
        f_RegClickEvent(divide2.transform.Find("Box").gameObject, OpenBox, 2, tLegionAward);
        f_RegClickEvent(divide3.transform.Find("Box").gameObject, OpenBox, 3, tLegionAward);
        f_RegClickEvent(divide4.transform.Find("Box").gameObject, OpenBox, 4, tLegionAward);
        _SacrificeNum.text = string.Format(CommonTools.f_GetTransLanguage(712), LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice);//当前祭天进度
        _SacrificePeople.text = string.Format(CommonTools.f_GetTransLanguage(713), LegionMain.GetInstance().m_LegionPlayerPool.m_iTodayContriMemberNum, LegionMain.GetInstance().m_LegionPlayerPool.m_iMemberNum);
        Box(tLegionAward);

        //选择
        curSacrificeType = 0;
        f_OnSacrificeSelectBtn(null, 1, null);
        //祭天按钮
        mSureBtn.SetActive(LegionMain.GetInstance().m_LegionInfor.m_iSacrificeType == 0);
    }

    private string curBoxFlag = string.Empty;

    void Box(LegionAwardDT taward)
    {
        curBoxFlag = LegionMain.GetInstance().m_LegionInfor.mSacrificeAward.ToString();
        if (curBoxFlag.Length != 4)
        {
            for (int i = curBoxFlag.Length; i <= 4; i++)
            {
                curBoxFlag = "0" + curBoxFlag;
            }
        }
        if (curBoxFlag[0] != '0')
            divide1.Find("Box").GetComponent<UIButton>().normalSprite = divide1.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_c";
        else
        {
            if (LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice >= taward.iNeedNum1)
                divide1.Find("Box").GetComponent<UIButton>().normalSprite = divide1.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_cc";// "Icon_BoxOpen";
            else
                divide1.Find("Box").GetComponent<UIButton>().normalSprite = divide1.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_cc";// "Icon_Box";
        }

        if (curBoxFlag[1] != '0')
            divide2.Find("Box").GetComponent<UIButton>().normalSprite = divide2.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_c";// "Icon_BoxGet";
        else
        {
            if (LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice >= taward.iNeedNum2)
                divide2.Find("Box").GetComponent<UIButton>().normalSprite = divide2.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_cc";//"Icon_BoxOpen";
            else
                divide2.Find("Box").GetComponent<UIButton>().normalSprite = divide2.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_cc";//"Icon_Box";
        }

        if (curBoxFlag[2] != '0')
            divide3.Find("Box").GetComponent<UIButton>().normalSprite = divide3.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_c";// "Icon_BoxGet";
        else
        {
            if (LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice >= taward.iNeedNum3)
                divide3.Find("Box").GetComponent<UIButton>().normalSprite = divide3.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_cc"; //"Icon_BoxOpen";
            else
                divide3.Find("Box").GetComponent<UIButton>().normalSprite = divide3.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_cc"; // "Icon_Box";
        }

        if (curBoxFlag[3] != '0')
            divide4.Find("Box").GetComponent<UIButton>().normalSprite = divide4.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_c";// "Icon_BoxGet";
        else
        {
            if (LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice >= taward.iNeedNum4)
                divide4.Find("Box").GetComponent<UIButton>().normalSprite = divide4.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_cc"; // "Icon_BoxOpen";
            else
                divide4.Find("Box").GetComponent<UIButton>().normalSprite = divide4.Find("Box/Background").GetComponent<UISprite>().spriteName = "ptfb_get_cc"; //"Icon_Box";
        }
    }

    #endregion

    #region 宝箱界面

    private ResourceCommonItem m_ShowAwardItem;

    void UpdateBox(LegionAwardDT taward, byte id)
    {
        UISprite BoxName = f_GetObject("BoxName").GetComponent<UISprite>();
        f_RegClickEvent("Btn_GetBox", GetBox, id, taward);
        f_RegClickEvent("Bg", CloseBox);
        if (m_ShowAwardItem == null)
            m_ShowAwardItem = ResourceCommonItem.f_Create(f_GetObject("AwardItemParent"), f_GetObject("AwardTipShowItem"));
        switch (id)
        {
            case 1:
                f_GetObject("Btn_GetBox").SetActive(curBoxFlag[id - 1] == '0' && LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice >= taward.iNeedNum1);
                f_GetObject("LockTip").SetActive(curBoxFlag[id - 1] == '0' && LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice < taward.iNeedNum1);
                f_GetObject("GetTip").SetActive(curBoxFlag[id - 1] != '0');
                BoxName.spriteName = "title_tibx";
                m_ShowAwardItem.f_UpdateByInfo(taward.iGoodsType1, taward.iGoodsId1, taward.iGoodsNum1);
                break;
            case 2:
                f_GetObject("Btn_GetBox").SetActive(curBoxFlag[id - 1] == '0' && LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice >= taward.iNeedNum2);
                f_GetObject("LockTip").SetActive(curBoxFlag[id - 1] == '0' && LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice < taward.iNeedNum2);
                f_GetObject("GetTip").SetActive(curBoxFlag[id - 1] != '0');
                BoxName.spriteName = "title_tobx";
                m_ShowAwardItem.f_UpdateByInfo(taward.iGoodsType2, taward.iGoodsId2, taward.iGoodsNum2);
                break;
            case 3:
                f_GetObject("Btn_GetBox").SetActive(curBoxFlag[id - 1] == '0' && LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice >= taward.iNeedNum3);
                f_GetObject("LockTip").SetActive(curBoxFlag[id - 1] == '0' && LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice < taward.iNeedNum3);
                f_GetObject("GetTip").SetActive(curBoxFlag[id - 1] != '0');
                BoxName.spriteName = "title_jibx";
                m_ShowAwardItem.f_UpdateByInfo(taward.iGoodsType3, taward.iGoodsId3, taward.iGoodsNum3);
                break;
            case 4:
                f_GetObject("Btn_GetBox").SetActive(curBoxFlag[id - 1] == '0' && LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice >= taward.iNeedNum4);
                f_GetObject("LockTip").SetActive(curBoxFlag[id - 1] == '0' && LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice < taward.iNeedNum4);
                f_GetObject("GetTip").SetActive(curBoxFlag[id - 1] != '0');
                BoxName.spriteName = "title_yubx";
                m_ShowAwardItem.f_UpdateByInfo(taward.iGoodsType4, taward.iGoodsId4, taward.iGoodsNum4);
                break;
        }
    }

    void GetBox(GameObject gp, object obj1, object obj2)
    {
        //int t = (int)obj1;
        byte tByte = (byte)obj1;
        LegionAwardDT tAwardDT = (LegionAwardDT)obj2;
        switch (tByte)
        {
            case 1:
                if (LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice < tAwardDT.iNeedNum1)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(714));//进度不足 
                    return;
                }
                break;
            case 2:
                if (LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice < tAwardDT.iNeedNum2)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(714));//进度不足
                    return;
                }
                break;
            case 3:
                if (LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice < tAwardDT.iNeedNum3)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(714));
                    return;
                }
                break;
            case 4:
                if (LegionMain.GetInstance().m_LegionInfor.m_LegionSacrifice.uSacrifice < tAwardDT.iNeedNum4)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(714));
                    return;
                }
                break;
        }
        SocketCallbackDT tSocketCallBackDT = new SocketCallbackDT();
        tSocketCallBackDT.m_ccCallbackSuc = (object obj) =>
        {
            LegionMain.GetInstance().m_LegionInfor.f_SendSacrificeInfo(UpdateMain);
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(715));//领取成功
            f_GetObject("Award").SetActive(false);
        };
        LegionMain.GetInstance().m_LegionInfor.f_SendGetBox(tByte, tSocketCallBackDT);
    }

    void CloseBox(GameObject gp, object obj1, object obj2)
    {
        f_GetObject("Award").SetActive(false);
    }
    #endregion

    private void f_OpenOrCloseMonenyPage(bool isOpen)
    {
        if (isOpen)
        {
            List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
            listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
            listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
            listMoneyType.Add(EM_MoneyType.eUserAttr_LegionContribution);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        }
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionSacrifice, UIMessageDef.UI_CLOSE);
    }
}
