using UnityEngine;
using System.Collections;
/// <summary>
/// 等级礼包item控制
/// </summary>
public class ActRankGiftItemCtl : UIFramwork
{
    public UILabel LabelDiscount;//折扣标签
    public UI2DSprite SprIcon;//礼包图片
    public UISprite AwardBorder;//边框
    public UILabel LabelCount;//礼包数量
    public UILabel LabelName;//礼包名称
    public GameObject BtnGet;//领取按钮
    public GameObject BtnHasGet;//已经领取
    public GameObject BtnWaitGet;//等级未到达，等待领取
    public UILabel LableWait;
    public GameObject OutOfTime; //已过期
    public UILabel LabelPrice;//物品价格
    public UILabel LabelBuyHint;//购买次数/提示需要达到多少等级提示
    public UISprite Bg;
    public UILabel Label;
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="discount">折扣</param>
    /// <param name="giftIcon">礼包名称</param>
    /// <param name="giftCount">礼包数量</param>
    /// <param name="giftName">礼包名称</param>
    /// <param name="giftState">礼包状态</param>
    /// <param name="price">价格</param>
    /// <param name="buyTiemsLeft">剩余购买次数</param>
    public void SetData(int discount, EM_ResourceType resourceType,int resourceId ,string borderSprName,int giftCount,string giftName,
        GiftState giftState,int openLevel,int price,int buyTimesMax,int buyTimeNow)
    {
        SetGameObjectAction(true);
        UITool.f_SetSpriteGray(Bg, false);
        LabelDiscount.text = discount.ToString() + "%";
        LabelDiscount.transform.parent.gameObject.SetActive(discount <= 0 ? false : true);
        UITool.f_SetIconSprite(SprIcon, resourceType, resourceId);
        LabelCount.text = giftCount.ToString();
        LabelName.text = giftName;
        LabelPrice.text = price.ToString();
        LabelPrice.transform.parent.gameObject.SetActive(giftState != GiftState.HasGet);
        BtnGet.SetActive(giftState == GiftState.CanGet);
        BtnHasGet.SetActive(giftState == GiftState.HasGet);
        BtnWaitGet.SetActive(giftState == GiftState.WaitGet);
        OutOfTime.SetActive(giftState == GiftState.OutOfTime);
LabelBuyHint.text = "（Purchased: " + buyTimeNow + "/" + buyTimesMax + "times）";
LableWait.text = string.Format("Level {0} can buy", openLevel);
        AwardBorder.spriteName = borderSprName;
    }

    public void SetGray() {
        UITool.f_SetSpriteGray(Bg,true);
        SetGameObjectAction(false);
    }

    private void SetGameObjectAction(bool isAction) {
        LabelDiscount.transform.parent.gameObject.SetActive(isAction);
        SprIcon.gameObject.SetActive(isAction);
        AwardBorder.gameObject.SetActive(isAction);
        LabelCount.gameObject.SetActive(isAction);
        LabelName.gameObject.SetActive(isAction);
        BtnGet.gameObject.SetActive(isAction);
        BtnHasGet.gameObject.SetActive(isAction);
        BtnWaitGet.gameObject.SetActive(isAction);
        LableWait.gameObject.SetActive(isAction);
        OutOfTime.gameObject.SetActive(isAction);
        LabelPrice.transform.parent.gameObject.SetActive(isAction);
        LabelBuyHint.gameObject.SetActive(isAction);
        Label.gameObject.SetActive(!isAction);
    }
    /// <summary>
    /// 礼包状态
    /// </summary>
    public enum GiftState
    {
        None=0,
        /// <summary>
        /// 已经领取
        /// </summary>
        HasGet = 1,
        /// <summary>
        /// 可领取
        /// </summary>
        CanGet,
        /// <summary>
        /// 等待领取
        /// </summary>
        WaitGet,
        /// <summary>
        /// 已过期
        /// </summary>
        OutOfTime,
    }
}
