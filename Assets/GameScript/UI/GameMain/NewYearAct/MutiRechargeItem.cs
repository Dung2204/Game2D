using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class MutiRechargeItem : UIFramwork {
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
    public void f_SetData(int progress,MutiRechargePoolDT poolDT, List<ResourceCommonDT> listAward)
    {
        mRechargeCount.text = poolDT.m_NewYearMultiRechargeAwardDT.iCondition + CommonTools.f_GetTransLanguage(2058) + "";
        //TsuComment //mLabelProgress.text = "[b9dbf4]Tiến độ：[ffffff]" + progress + "0" + "/" + poolDT.m_NewYearMultiRechargeAwardDT.iCondition + "0";
        mLabelProgress.text = "[b9dbf4]Progress：[ffffff]" + progress + "/" + poolDT.m_NewYearMultiRechargeAwardDT.iCondition; //TsuCode

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
