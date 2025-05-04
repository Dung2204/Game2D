using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class SellPage : UIFramwork
{
    SellPageParam param;
    private int buyCount;//出售数量 


    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null && !(e is SellPage))
        {
MessageBox.ASSERT("Error entering parameters");
        }

        param = (SellPageParam)e;
        buyCount = 1;
        InitUI();

    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnAddOne", OnSellAddBtn, 1);
        f_RegClickEvent("BtnAddTen", OnSellAddBtn, 10);
        f_RegClickEvent("BtnReduceOne", OnSellAddBtn, -1);
        f_RegClickEvent("BtnReduceTen", OnSellAddBtn, -10);
        f_RegClickEvent("BtnBlack", OnBtnBlackClick);
        f_RegClickEvent("BtnConfirmBuy", OnConfirmClick);
        f_RegClickEvent("BtnCancelBuy", OnCancelClick);
    }


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

        f_RegClickEvent("SpriteIcon", OnIconClick);
        UpdateNumUI();
    }

    private void UpdateNumUI()
    {
        f_GetObject("InputBuyCount").GetComponent<UILabel>().text = buyCount.ToString();
        f_GetObject("LabelPrice").GetComponent<UILabel>().text = (buyCount * param.SellNum).ToString();
    }

    #region  按钮事件

    private void OnSellAddBtn(GameObject go, object obj1, object obj2)
    {
        int num = (int)obj1;

        if (buyCount + num <= 0)
        {
            buyCount = 0;
            UpdateNumUI();
UITool.Ui_Trip("Cần chọn số lượng bán lớn hơn 0");
            return;
        }
        if (buyCount + num > param.resourceCount)
        {
            buyCount = param.resourceCount;
            UpdateNumUI();
UITool.Ui_Trip("Không thể bán nhiều hơn số lượng đang có");
            return;
        }
        buyCount += num;


        UpdateNumUI();
    }
    /// <summary>
    /// 点击黑色背景关闭按钮
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SellPage, UIMessageDef.UI_CLOSE);
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
    /// 点击确定按钮
    /// </summary>
    private void OnConfirmClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SellPage, UIMessageDef.UI_CLOSE);
        if (param.onConfirmBuyCallback != null)
        {
            param.onConfirmBuyCallback(param.iId, param.resourceType, param.resourceID, param.resourceCount, buyCount, param.KeyId);
        }
    }
    /// <summary>
    /// 点击取消按钮
    /// </summary>
    private void OnCancelClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SellPage, UIMessageDef.UI_CLOSE);
    }
    #endregion

}


public class SellPageParam
{
    public int iId;
    /// <summary>
    /// KeyId
    /// </summary>
    public long KeyId;
    /// <summary>
    /// 标题
    /// </summary>
public string title = "Bán";
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
    /// 购买价格类型
    /// </summary>
    public EM_MoneyType moneyType;
    /// <summary>
    /// 出售价格
    /// </summary>
    public int SellNum;
    /// <summary>
    /// 确认出售回调
    /// </summary>
    public ConfirmSellCallback onConfirmBuyCallback;
}

public delegate void ConfirmSellCallback(int iId, EM_ResourceType type, int resourceId, int resourceCount, int buyCount, long KeyId);
