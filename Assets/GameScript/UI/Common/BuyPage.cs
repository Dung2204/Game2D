using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 购买页面
/// </summary>
public class BuyPage : UIFramwork
{
    private BuyParam param;//参数
    private int buyCount;//购买数量 
    private UIInput _InputCount;
    /// <summary>
    /// 页面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        param = (BuyParam)e;
        buyCount = 1;
        InitUI();
    }
    /// <summary>
    /// 初始化UI
    /// </summary>
    private void InitUI()
    {
        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)param.resourceType, param.resourceID, param.resourceCount);
        UITool.f_SetIconSprite(f_GetObject("SpriteIcon").GetComponent<UI2DSprite>(), param.resourceType, param.resourceID);
        string itemName = dt.mName;
        string borderName = UITool.f_GetImporentColorName(dt.mImportant, ref itemName);
        f_GetObject("IconBorder").GetComponent<UISprite>().spriteName = borderName;
        f_GetObject("GoodNum").GetComponent<UILabel>().text = param.resourceCount.ToString();
        f_GetObject("LabelName").GetComponent<UILabel>().text = itemName;
        f_GetObject("LabelHasCount").GetComponent<UILabel>().text = UITool.f_GetGoodNum(param.resourceType, param.resourceID).ToString();
        f_GetObject("MoneyIcon").GetComponent<UISprite>().spriteName = UITool.f_GetMoneySpriteName(param.moneyType);
        //f_GetObject("MoneyIcon").GetComponent<UISprite>().MakePixelPerfect();
        f_GetObject("LabelHintTimesLeft").GetComponent<UILabel>().text = param.buyHint;
        _InputCount = f_GetObject("InputBuyCountBg").GetComponent<UIInput>();
        UIEventListener.Get(f_GetObject("InputBuyCountBg")).onSelect = Call_InputCount;
        UpdateUI();
        f_RegClickEvent("SpriteIcon", OnIconClick);
    }
    /// <summary>
    /// 点击图标
    /// </summary>
    private void OnIconClick(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)param.resourceType, param.resourceID, param.resourceCount);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    private void UpdateUI()
    {
        f_GetObject("InputBuyCount").GetComponent<UILabel>().text = buyCount.ToString();
        f_GetObject("LabelPrice").GetComponent<UILabel>().text = getTotalPrice(buyCount).ToString();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlack", OnBtnBlackClick);
        f_RegClickEvent("BtnConfirmBuy", OnConfirmClick);
        f_RegClickEvent("BtnCancelBuy", OnCancelClick);
        f_RegClickEvent("BtnAddOne", OnBuyAddOneClick);
        f_RegClickEvent("BtnAddTen", OnBuyAddTenClick);
        f_RegClickEvent("BtnReduceOne", OnBuyReduceOneClick);
        f_RegClickEvent("BtnReduceTen", OnBuyReduceTenClick);
    }
    #region 按钮事件相关
    /// <summary>
    /// 点击黑色背景关闭按钮
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击确定按钮
    /// </summary>
    private void OnConfirmClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyPage, UIMessageDef.UI_CLOSE);
        if (param.onConfirmBuyCallback != null)
        {
            param.onConfirmBuyCallback(param.iId, param.resourceType, param.resourceID, param.resourceCount, buyCount);
        }
    }
    /// <summary>
    /// 点击取消按钮
    /// </summary>
    private void OnCancelClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击购买道具减10
    /// </summary>
    private void OnBuyReduceTenClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        buyCount -= 10;
        buyCount = buyCount < 1 ? 1 : buyCount;
        UpdateUI();
    }
    /// <summary>
    /// 点击购买道具减1
    /// </summary>
    private void OnBuyReduceOneClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        buyCount -= 1;
        buyCount = buyCount < 1 ? 1 : buyCount;
        UpdateUI();
    }
    /// <summary>
    /// 点击购买道具加10
    /// </summary>
    private void OnBuyAddTenClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        buyCount += 10;
        CheckBuyCount();
        UpdateUI();
    }
    /// <summary>
    /// 点击购买道具加1
    /// </summary>
    private void OnBuyAddOneClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        buyCount += 1;
        CheckBuyCount();
        UpdateUI();
    }
    /// <summary>
    /// 检测购买数量是否合法，不合法则纠正购买数量并显示提示
    /// </summary>
    private void CheckBuyCount()
    {
        bool isReduce = false;

        if (buyCount > param.canBuyTimes && param.canBuyTimes != 0)
        {
            buyCount = param.canBuyTimes + 1;
        }
        if (param.canBuyTimes != 0)
        {
            while (!CheckMoneyPrice(buyCount))
            {
                buyCount--;
                isReduce = true;
            }
        }
        else
        {
            int OneNum = GetPriceByBuyTimes(1);
            int NeedNum = OneNum * buyCount;
            int HasNum = UITool.f_GetGoodNum(EM_ResourceType.Money, (int)param.moneyType);
            if (NeedNum > HasNum)
            {
                buyCount = HasNum / OneNum;
UITool.Ui_Trip("Có thể mua tối đa " + buyCount);
            }
        }



        if (isReduce && buyCount <= param.canBuyTimes)
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không đủ tiền, đã mua: " + buyCount);
            return;
        }
        if (param.canBuyTimes > 0 && buyCount > param.canBuyTimes)
        {
            buyCount = param.canBuyTimes;
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Có thể mua tối đa" + param.canBuyTimes);
        }
        if (param.canBuyTimes == -1)
        {
            buyCount = 1;
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Đã hết thời gian！");
        }
        _InputCount.value = buyCount.ToString();
    }
    /// <summary>
    /// 检测金钱数量
    /// </summary>
    private bool CheckMoneyPrice(int buyCount)
    {
        int hasNum = UITool.f_GetGoodNum(EM_ResourceType.Money, (int)param.moneyType);
        int totalprice = getTotalPrice(buyCount);
        bool isEnough = hasNum >= totalprice ? true : false;
        return isEnough;
    }
    /// <summary>
    /// 根据数量获取总价
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private int getTotalPrice(int num)
    {
        int price = 0;
        for (int i = 0; i < num; i++)
        {
            price += GetPriceByBuyTimes(i);
        }
        return price;
    }

    /// <summary>
    /// 通过玩家已经购买次数获取道具价格
    /// </summary>
    /// <returns></returns>
    private int GetPriceByBuyTimes(int buyTimes)
    {
        buyTimes = buyTimes > (param.price.Count - 1) ? (param.price.Count - 1) : buyTimes;
        int price = param.price[buyTimes];
        return price;
    }
    #endregion

    private void Call_InputCount(GameObject go, bool isSelect)
    {
        if (!isSelect)
        {
            buyCount = ccMath.atoi(_InputCount.value);
            buyCount = buyCount < 1 ? 1 : buyCount;
            if (buyCount > 999)
            {
                buyCount = 999;
UITool.Ui_Trip("Tối đa 999 trong 1 lần");
            }
            CheckBuyCount();
            UpdateUI();
        }
    }
}
/// <summary>
/// 确认购买委托
/// </summary>
/// <param name="buyCount">购买数量</param>
public delegate void ConfirmBuyCallback(int iId, EM_ResourceType type, int resourceId, int resourceCount, int buyCount);
/// <summary>
/// 商店购买参数
/// </summary>
public class BuyParam
{
    public int iId;
    /// <summary>
    /// 标题
    /// </summary>
public string title = "Mua";
    /// <summary>
    /// 资源类型
    /// </summary>
    public EM_ResourceType resourceType;
    /// <summary>
    /// 资源id
    /// </summary>
    public int resourceID;
    /// <summary>
    /// 资源数量
    /// </summary>
    public int resourceCount;
    /// <summary>
    /// 今日可购买次数，小于-1表示没有限制，-1表示不可再买
    /// </summary>
    public int canBuyTimes = 0;
    /// <summary>
    /// 购买提示，如：今日可购买3次
    /// </summary>
    public string buyHint = "";
    /// <summary>
    /// 购买价格类型
    /// </summary>
    public EM_MoneyType moneyType;
    /// <summary>
    /// 物品单价
    /// </summary>
    public List<int> price;
    /// <summary>
    /// 确认购买回调
    /// </summary>
    public ConfirmBuyCallback onConfirmBuyCallback;
}
