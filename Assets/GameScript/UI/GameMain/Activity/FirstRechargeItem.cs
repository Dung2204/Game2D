using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class FirstRechargeItem : UIFramwork
{
    public UILabel mRechargeCount;//充值金额
    public GameObject mBtnGet;
    public GameObject mBtnGoRecharge;
    public GameObject mBtnHasGet;
    public GameObject mAwardItemParent;
    public GameObject mAwardItem;
    public UILabel mLabelProgress;//进度
    /// <summary>
    /// 设置数据
    /// </summary>
    public void f_SetData(int progress, FirstRechargePoolDT poolDT, List<ResourceCommonDT> listAward)
    {
        mRechargeCount.text = string.Format(CommonTools.f_GetTransLanguage(1383), poolDT.m_FirstRechargeDT.iCondition);
        mLabelProgress.text = CommonTools.f_GetTransLanguage(1384) + progress + "/" + poolDT.m_FirstRechargeDT.iCondition;

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
}
