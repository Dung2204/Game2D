using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 招财符
/// </summary>
public class ActLuckySymbolCtl : UIFramwork {
    public UILabel m_LabelTimesHint;//招财次数提示
    public GameObject m_BtnLuckySymbol;//招财按钮
    public UILabel m_LabelHintGetMoney;//获取银币数量
    public UILabel m_LabelWasteSycee;//需要消耗元宝的数量
    public UILabel m_LabelFree;//本次免费
    public UILabel m_LabelGetMoneyHint;//提示本次最多获取多少银两
    private SocketCallbackDT RequestLuckySymbolCallback = new SocketCallbackDT();//招财符回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//回调
    private SocketCallbackDT RequestQueryCallback = new SocketCallbackDT();//回调

    private int currentBuyTImes;//当前购买次数
    private int maxTimes;//当日最大可购买次数
    private int moneyCountOri;//招财前的银两数量
    private int price;//当前购买所需的价格
    List<int> listMask;//保存服务器返回的掩码
    List<AwardPoolDT> awardList = new List<AwardPoolDT>();
    private string strTexBgRoot = "UI/TextureRemove/Activity/Tex_LuckyBg";

    private GameObject effect = null;
    
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    //public
    public void f_ShowView()
    {
        gameObject.SetActive(true);
        f_RegClickEvent(m_BtnLuckySymbol.gameObject, OnLuckySymbolClick);
        f_RegClickEvent("SprAwake0", OnGetClick, 0);
        f_RegClickEvent("SprAwake1", OnGetClick, 1);
        f_RegClickEvent("SprAwake2", OnGetClick, 2);
        f_RegClickEvent("SprAwake3", OnGetClick, 3);
        f_RegClickEvent("BtnHelp", OnBtnHelpClick);
        //f_RegClickEvent(m_BtnGet.gameObject, OnGetClick);

        RequestLuckySymbolCallback.m_ccCallbackSuc = OnLuckySymbolSucCallback;
        RequestLuckySymbolCallback.m_ccCallbackFail = OnLuckySymbolFailCallback;

        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;

        RequestQueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        RequestQueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        maxTimes = GetCanBuyTimesByVipLevel(UITool.f_GetNowVipLv());
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_QueryLuckySymbolInfo(RequestQueryCallback);
        f_LoadTexture();
        if (!effect)
        {
            UITool.f_CreateMagicById((int)EM_MagicId.eLuckySymbolBg, ref effect, f_GetObject("spineParent").transform, 1, 
                "animation", null, true, 100f);
        }
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;
        }
    }
    /// <summary>
    /// 点击招财
    /// </summary>
    /// <param name="go"></param>
    private void OnLuckySymbolClick(GameObject go, object obj1,object obj2)
    {
        moneyCountOri = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money);

        if (currentBuyTImes >= maxTimes)
        {
            int tVipLv = UITool.f_GetNowVipLv();
            if (tVipLv < 15)
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1310));
            else
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1311));
            return;
        }
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < price)
        { 
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1312));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_LuckySymbolInfo(RequestLuckySymbolCallback);
    }
    /// <summary>
    /// 点击帮助按钮
    /// </summary>
    private void OnBtnHelpClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 11);
    }
    /// <summary>
    /// 点击招财
    /// </summary>
    /// <param name="go"></param>
    private void OnGetClick(GameObject go, object obj1, object obj2)
    {
        Debug.Log("ID click："+ obj1);
        int index = (int)obj1;
        if (listMask == null)
            return;
        if (listMask[index] == 1)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1314));
            return;
        }
        bool isTimesGet = Data_Pool.m_ActivityCommonData.ActLuckySymbolBuyTimes >= (index + 1) * 5;

        ActLuckySymbolDT actLuckySymbolDT = glo_Main.GetInstance().m_SC_Pool.m_ActLuckySymbolSC.f_GetSC((index + 1) * 5) as ActLuckySymbolDT;
        awardList.Clear();
        AwardPoolDT tPool1 = new AwardPoolDT();
        tPool1.f_UpdateByInfo((byte)actLuckySymbolDT.iRewardType1, actLuckySymbolDT.iRewardId1, actLuckySymbolDT.iRewardCount1);
        AwardPoolDT tPool2 = new AwardPoolDT();
        tPool2.f_UpdateByInfo((byte)actLuckySymbolDT.iRewardType2, actLuckySymbolDT.iRewardId2, actLuckySymbolDT.iRewardCount2);
        awardList.Add(tPool1);
        awardList.Add(tPool2);
        AwardGetSubPageParam param = new AwardGetSubPageParam(isTimesGet ? EM_AwardGetState.CanGet : EM_AwardGetState.Lock, CommonTools.f_GetTransLanguage(1315), "", awardList, OnGetHandle, index, this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AwardGetSubPage, UIMessageDef.UI_OPEN, param);
    }
    /// <summary>
    /// 点击了领取按钮
    /// </summary>
    /// <param name="data"></param>
    private void OnGetHandle(object data)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_LuckySymbolGet((int)data + 1, RequestGetCallback);
    }
    #region 招财回调
    /// <summary>
    /// 招财回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnLuckySymbolSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        int moneyCountAfter = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money);
        int GetNum = moneyCountAfter - moneyCountOri;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(1316), GetNum));
        UpdateLuckySymbolUI();

        //List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        //AwardPoolDT item1 = new AwardPoolDT();
        //item1.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, GetNum);
        //awardList.Add(item1);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
        //    new object[] { awardList });
    }
    private void OnLuckySymbolFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1317) + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion

    #region 领取奖励回调
    /// <summary>
    /// 领取奖励回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AwardGetSubPage, UIMessageDef.UI_CLOSE);
        UpdateLuckySymbolUI();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
    }
    private void OnGetFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1318) + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion

    #region 招财符信息回调
    /// <summary>
    /// 招财符信息回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuerySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "招财符信息成功！");
        UpdateLuckySymbolUI();
    }
    private void OnQueryFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1319) + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion
    public void UpdateLuckySymbolUI()
    {
        currentBuyTImes = Data_Pool.m_ActivityCommonData.ActLuckySymbolBuyTimes;
        price = GetPriceByTimes(currentBuyTImes + 1);
        bool isFree = price <= 0;
        m_LabelWasteSycee.transform.parent.gameObject.SetActive(!isFree);
        m_LabelWasteSycee.text = string.Format("{0}[C81334FF]  （{1} / {2}）", price, currentBuyTImes, maxTimes);
        f_GetObject("SyceeIcon").SetActive(!isFree);
        m_LabelFree.gameObject.SetActive(isFree);

        int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        ActLuckyLevelConfigDT actLuckyLevelConfigDT = glo_Main.GetInstance().m_SC_Pool.m_ActLuckyLevelConfigSC.f_GetSC(userLevel) as ActLuckyLevelConfigDT;
        int GetMoneyCount = actLuckyLevelConfigDT.iCoinCost;
        m_LabelGetMoneyHint.text = string.Format(CommonTools.f_GetTransLanguage(1320), GetMoneyCount);

        int getMask = Data_Pool.m_ActivityCommonData.ActLuckySymbolIsGetMask;
        char[] maskArray = getMask.ToString().ToCharArray();
        listMask = new List<int>();
        for (int i = 0; i < 4 - maskArray.Length; i++)
        {
            listMask.Add(0);
        }
        for (int j = 0; j < maskArray.Length; j++)
        {
            if (maskArray[j] == '1')
                listMask.Add(1);
            else
                listMask.Add(0);
        }
        if (listMask.Count != 4)
        {
            Debug.LogError(CommonTools.f_GetTransLanguage(1321));
        }
        f_GetObject("SliderProgress").GetComponent<UISlider>().value = currentBuyTImes * 1.0f / 20;
        for (int i = 0; i < listMask.Count; i++)
        {
            UISprite box = f_GetObject("SprAwake" + i.ToString()).transform.Find("IconParent/Icon").GetComponent<UISprite>();
            Transform ParticleOpen = f_GetObject("SprAwake" + i.ToString()).transform.Find("ParticleOpen");
            Transform ParticleBox = f_GetObject("SprAwake" + i.ToString()).transform.Find("ParticleBox");
            box.gameObject.SetActive(true);
            if (currentBuyTImes < (i + 1) * 5)
            {
                //box.spriteName = "ptfb_get_dd";
                if (ParticleOpen != null)
                {
                    // GameObject.Destroy(ParticleOpen.gameObject);
                }
                if (ParticleBox == null)
                {
                    // GameObject Particle = UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.BoxLockEffect1, f_GetObject("SprAwake" + i.ToString()).transform, 0, 9.5f, false, 1.15f);
                    // Particle.name = "ParticleBox";
                }
            }
            else
            {
                if (listMask[i] == 1)
                {
                    //box.spriteName = "ptfb_get_d";
                    if (ParticleOpen != null)
                    {
                        // GameObject.Destroy(ParticleOpen.gameObject);
                    }
                    if (ParticleBox != null)
                    {
                        // GameObject.Destroy(ParticleBox.gameObject);
                    }
                }
                else
                {
                    box.gameObject.SetActive(false);
                    //box.spriteName = "ptfb_get_dd";
                    if (ParticleOpen == null)
                    {
                        // GameObject Particle = UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.BoxCanGetEffect1, f_GetObject("SprAwake" + i.ToString()).transform, 0, 10);
                        // Particle.name = "ParticleOpen";
                    }
                    if (ParticleBox != null)
                    {
                        // GameObject.Destroy(ParticleBox.gameObject);
                    }
                }
            }
            //box.MakePixelPerfect();
        }
    }
    private void CreateBoxEffect(UINameConst effectUIName)
    {

    }
    private int GetPriceByTimes(int buyTimes)
    {
        ActLuckySymbolDT actLuckySymbolDT = (ActLuckySymbolDT)glo_Main.GetInstance().m_SC_Pool.m_ActLuckySymbolSC.f_GetSC(buyTimes);
        if (actLuckySymbolDT == null)
        {
            List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_ActLuckySymbolSC.f_GetAll();
            return ((ActLuckySymbolDT)listData[listData.Count - 1]).iBuyPrice;
        }
        return actLuckySymbolDT.iBuyPrice;
    }
    private int GetCanBuyTimesByVipLevel(int vipLevel)
    {
        VipPrivilegeDT vipPrivilegeDT = (VipPrivilegeDT)glo_Main.GetInstance().m_SC_Pool.m_VipPrivilegeSC.f_GetSC(14);
        switch (vipLevel)
        {
            case 0: return vipPrivilegeDT.iLv0;
            case 1: return vipPrivilegeDT.iLv1;
            case 2: return vipPrivilegeDT.iLv2;
            case 3: return vipPrivilegeDT.iLv3;
            case 4: return vipPrivilegeDT.iLv4;
            case 5: return vipPrivilegeDT.iLv5;
            case 6: return vipPrivilegeDT.iLv6;
            case 7: return vipPrivilegeDT.iLv7;
            case 8: return vipPrivilegeDT.iLv8;
            case 9: return vipPrivilegeDT.iLv9;
            case 10: return vipPrivilegeDT.iLv10;
            case 11: return vipPrivilegeDT.iLv11;
            case 12: return vipPrivilegeDT.iLv12;
            case 13: return vipPrivilegeDT.iLv13;
            case 14: return vipPrivilegeDT.iLv14;
            case 15: return vipPrivilegeDT.iLv15;
            default: return vipPrivilegeDT.iLv15;
        }
    }
}
