using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleRechargeItem : UIFramwork
{
    public UILabel mRechargeCount;//充值金额
    public GameObject mBtnGet;
    public GameObject mBtnGoRecharge;
    public GameObject mBtnHasGet;
    public GameObject mAwardItemParent;
    public GameObject mAwardItem;
    /// <summary>
    /// 设置数据
    /// </summary>
    public void f_SetData(SingleRechargePoolDT poolDT, List<ResourceCommonDT> listAward)
    {
        mRechargeCount.text = poolDT.m_NewYearSingleRechargeAwardDT.iCondition + CommonTools.f_GetTransLanguage(2058);

        GridUtil.f_SetGridView<ResourceCommonDT>(mAwardItemParent, mAwardItem, listAward, OnItemUpdate);
        mAwardItemParent.transform.GetComponent<UIGrid>().Reposition();
        mAwardItemParent.transform.GetComponentInParent<UIScrollView>().ResetPosition();
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
    }
    public void f_SetData(TotalConsumptionPoolDT poolDT, List<ResourceCommonDT> listAward)
    {
        mRechargeCount.text = string.Format(CommonTools.f_GetTransLanguage(2057), poolDT.m_NewYearSyceeConsume.iCondition);

        GridUtil.f_SetGridView<ResourceCommonDT>(mAwardItemParent, mAwardItem, listAward, OnItemUpdate);
        mAwardItemParent.transform.GetComponent<UIGrid>().Reposition();
        mAwardItemParent.transform.GetComponentInParent<UIScrollView>().ResetPosition();
    }

}
