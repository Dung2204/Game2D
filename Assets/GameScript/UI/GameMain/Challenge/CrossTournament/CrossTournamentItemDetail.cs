using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;

public class CrossTournamentItemDetail : UIFramwork
{
    public UILabel mLabelDesc;
    public GameObject AwardParent;//奖励父物体
    public GameObject AwardItem;//奖励item
    public void InitData(NBaseSCDT data)
    {
        if(data is CrossTournamentKnockDT)
        {
            CrossTournamentKnockDT pData = data as CrossTournamentKnockDT;
            mLabelDesc.text = pData.szName;
            List<AwardPoolDT> listAwardData = CommonTools.f_GetListAwardPoolDT(pData.szAward);
            GridUtil.f_SetGridView<AwardPoolDT>(AwardParent, AwardItem, listAwardData, OnShowItemCallback);
            AwardParent.transform.parent.GetComponent<UIScrollView>().ResetPosition();
        }
        else if (data is CrossTournamentQualifyingRoundDT)
        {
            CrossTournamentQualifyingRoundDT pData = data as CrossTournamentQualifyingRoundDT;
            mLabelDesc.text = pData.szName;
            List<AwardPoolDT> listAwardData = CommonTools.f_GetListAwardPoolDT(pData.szAward);
            GridUtil.f_SetGridView<AwardPoolDT>(AwardParent, AwardItem, listAwardData, OnShowItemCallback);
            AwardParent.transform.parent.GetComponent<UIScrollView>().ResetPosition();
        }
        else if (data is CrossTournamentDT)
        {
            CrossTournamentDT pData = data as CrossTournamentDT;
            mLabelDesc.text = pData.szName;
            List<AwardPoolDT> listAwardData = CommonTools.f_GetListAwardPoolDT(pData.szAward);
            GridUtil.f_SetGridView<AwardPoolDT>(AwardParent, AwardItem, listAwardData, OnShowItemCallback);
            AwardParent.transform.parent.GetComponent<UIScrollView>().ResetPosition();
        }
        else
        {
            mLabelDesc.text = "";
        }
    }
    private void OnShowItemCallback(GameObject item, AwardPoolDT awardPoolDt)
    {
        item.GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(awardPoolDt.mTemplate.mIcon);
        item.transform.Find("LabelCount").GetComponent<UILabel>().text = UITool.f_CountToChineseStr(awardPoolDt.mTemplate.mResourceNum);
        string name = awardPoolDt.mTemplate.mName;
        item.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(awardPoolDt.mTemplate.mImportant, ref name);
        f_RegClickEvent(item, OnAwardIconClick, awardPoolDt);
    }
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        AwardPoolDT awardPoolDt = (AwardPoolDT)obj1;
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)awardPoolDt.mTemplate.mResourceType, awardPoolDt.mTemplate.mResourceId, awardPoolDt.mTemplate.mResourceNum);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
}
