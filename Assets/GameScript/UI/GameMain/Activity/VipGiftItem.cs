using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// vip礼包
/// </summary>
public class VipGiftItem : UIFramwork
{
    public UISprite mSprVip;//vip等级
    public GameObject mBtnGetGay;//灰色领取
    public GameObject mBtnGet;//可领取
    public GameObject mBtnRecharge;//充值按钮
    public GameObject mBtnHasGet;//已领取
    public GameObject AwardParent;//奖励父物体
    public GameObject AwardItem;//奖励item
    public UILabel mLabelSprVip;
    /// <summary>
    /// 设置数据
    /// </summary>
    public void InitData(VipGiftPoolDT vipGiftPoolDT)
    {
        List<AwardPoolDT> listAwardData = CommonTools.f_GetListAwardPoolDT(vipGiftPoolDT.mVipGiftDT.szAward);
        GridUtil.f_SetGridView<AwardPoolDT>(AwardParent, AwardItem, listAwardData, OnShowItemCallback);
        AwardParent.transform.parent.GetComponent<UIScrollView>().ResetPosition();
        if (mSprVip != null)
        {
            mSprVip.spriteName = "Vip_ (" + vipGiftPoolDT.mVipGiftDT.iId + ")";
            mSprVip.MakePixelPerfect();
        }
        else {
            mLabelSprVip.text = string.Format("VIP  {0}", vipGiftPoolDT.mVipGiftDT.iId);
        }
    }
    /// <summary>
    /// 显示内容
    /// </summary>
    /// <param name="item"></param>
    /// <param name="awardPoolDt"></param>
    private void OnShowItemCallback(GameObject item, AwardPoolDT awardPoolDt)
    {
        item.GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(awardPoolDt.mTemplate.mIcon);
        item.transform.Find("LabelCount").GetComponent<UILabel>().text = UITool.f_CountToChineseStr(awardPoolDt.mTemplate.mResourceNum);
        string name = awardPoolDt.mTemplate.mName;
        item.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(awardPoolDt.mTemplate.mImportant, ref name);
        f_RegClickEvent(item, OnAwardIconClick, awardPoolDt);
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        AwardPoolDT awardPoolDt = (AwardPoolDT)obj1;
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)awardPoolDt.mTemplate.mResourceType, awardPoolDt.mTemplate.mResourceId, awardPoolDt.mTemplate.mResourceNum);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }

}
