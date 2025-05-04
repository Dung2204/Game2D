using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 红包兑换item
/// </summary>
public class RedPacketTaskItem : UIFramwork
{
    public UILabel mLabelTitle;
    public GameObject mObjWastItemParent;
    public GameObject mObjWastItem;
    public GameObject mBtnGet;//兑换按钮
    public GameObject mBtnGoto;//前往按钮
    public GameObject mBtnHasGet;//已领取按钮
    public UILabel mLabelTimes;//剩余次数
    /// <summary>
    /// 设置数据
    /// </summary>
    public void f_SetData(RedPacketTaskPoolDT poolDT)
    {
        mLabelTitle.text = poolDT.mRedPacketTaskDT.szDesc.Replace(GameParamConst.ReplaceFlag, poolDT.mRedPacketTaskDT.iConditonParam.ToString());
        List<ResourceCommonDT> listCommonDT = new List<ResourceCommonDT>();
        if (poolDT.mRedPacketTaskDT.szAward1.Contains(";"))
            listCommonDT.Add(CommonTools.f_GetCommonResourceByResourceStr(poolDT.mRedPacketTaskDT.szAward1));
        if (poolDT.mRedPacketTaskDT.szAward2.Contains(";"))
            listCommonDT.Add(CommonTools.f_GetCommonResourceByResourceStr(poolDT.mRedPacketTaskDT.szAward2));
        if (poolDT.mRedPacketTaskDT.szAward3.Contains(";"))
            listCommonDT.Add(CommonTools.f_GetCommonResourceByResourceStr(poolDT.mRedPacketTaskDT.szAward3));

mLabelTimes.text = "[b9dbf4]Progress：[ffffff]" + poolDT.mProgress + "/" + poolDT.mRedPacketTaskDT.iConditonParam;
        EM_BoxGetState state = EM_BoxGetState.Lock;
        if (poolDT.mProgress >= poolDT.mRedPacketTaskDT.iConditonParam)//未达到领取条件
        {
            if (poolDT.mHasGetCount <= 0)
                state = EM_BoxGetState.CanGet;
            else
                state = EM_BoxGetState.AlreadyGet;
        }
        string uiName = poolDT.mRedPacketTaskDT.szUIName;
        mBtnGet.SetActive(state == EM_BoxGetState.CanGet);
        mBtnGoto.SetActive(state == EM_BoxGetState.Lock && uiName != null && uiName != "");
        mBtnHasGet.SetActive(state == EM_BoxGetState.AlreadyGet);

        GridUtil.f_SetGridView<ResourceCommonDT>(mObjWastItemParent, mObjWastItem, listCommonDT, OnItemUpdate);
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
    }
}
