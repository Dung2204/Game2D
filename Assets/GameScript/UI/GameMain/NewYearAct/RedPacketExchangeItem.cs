using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 红包兑换item
/// </summary>
public class RedPacketExchangeItem : UIFramwork {
    public UI2DSprite mExchargeIcon;//兑换物品icon
    public UILabel mExchargeName;//兑换物品名字
    public UILabel mExchargeCount;//兑换物品数量
    public UISprite mExchargeBorder;//兑换物品品质框
    public GameObject mObjWastItemParent;
    public GameObject mObjWastItem;
    public GameObject mBtnExcharge;//兑换按钮
    public GameObject mBtnExchargeGay;//兑换灰色按钮(不可兑换)
    public UILabel mLabelTimes;//剩余次数
    /// <summary>
    /// 设置数据
    /// </summary>
    public void f_SetData(ResourceCommonDT SailResourceItem,int hasExchargeCount,int maxExchargeCount,List<ResourceCommonDT> listWastItem)
    {
        string sailResName = SailResourceItem.mName;
        mExchargeBorder.spriteName = UITool.f_GetImporentColorName(SailResourceItem.mImportant, ref sailResName);
        mExchargeName.text = sailResName;
        mExchargeCount.text = SailResourceItem.mResourceNum.ToString();
        mExchargeIcon.sprite2D = UITool.f_GetIconSprite(SailResourceItem.mIcon);
mLabelTimes.text = "[b9dbf4]Also：[ffffff]" + (maxExchargeCount - hasExchargeCount) + "/" + maxExchargeCount;

        GridUtil.f_SetGridView<ResourceCommonDT>(mObjWastItemParent, mObjWastItem, listWastItem, OnItemUpdate);
        mObjWastItemParent.transform.GetComponent<UIGrid>().Reposition();
        mObjWastItemParent.transform.GetComponentInParent<UIScrollView>().ResetPosition();
    }
    /// <summary>
    /// item更新
    /// </summary>
    /// <param name="item"></param>
    /// <param name="data"></param>
    private void OnItemUpdate(GameObject item, ResourceCommonDT data)
    {
        string sailResName = data.mName;
        item.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(data.mImportant, ref sailResName);
        item.transform.Find("LabelCount").GetComponent<UILabel>().text = data.mResourceNum.ToString();
        item.transform.GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(data.mIcon);
        f_RegClickEvent(item, UITool.f_OnItemIconClick, data);
        int hasNum = UITool.f_GetGoodNum((EM_ResourceType)data.mResourceType, data.mResourceId);
        //不够数量的灰显
        UITool.f_Set2DSpriteGray(item.transform.GetComponent<UI2DSprite>(), hasNum < data.mResourceNum);
        UITool.f_SetSpriteGray(item.transform.Find("IconBorder").GetComponent<UISprite>(), hasNum < data.mResourceNum);
    }
}
