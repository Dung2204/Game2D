using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;

public class ResourceCommonItemDetailPage : UIFramwork
{
    private UILabel mTitle;
    private UI2DSprite mIcon;
    private UILabel mName;
    private UILabel mHaveNum;
    private UILabel mDesc;
    private GameObject mMaskClose;

    private ResourceCommonDT mData;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mTitle = f_GetObject("Title").GetComponent<UILabel>();
        mIcon = f_GetObject("Icon").GetComponent<UI2DSprite>();
        mName = f_GetObject("Name").GetComponent<UILabel>();
        mHaveNum = f_GetObject("HaveNum").GetComponent<UILabel>();
        mDesc = f_GetObject("Desc").GetComponent<UILabel>();
        mMaskClose = f_GetObject("MaskClose");
        f_RegClickEvent(mMaskClose, f_MaskClose);

    }
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is ResourceCommonDT))
        {
MessageBox.ASSERT("ResourceCommonItemDetailPage must be passed to ResourceCommonDT");
            return;
        }
        mData = (ResourceCommonDT)e;
        f_GetObject("SingeonItemContent").SetActive(true);
        f_GetObject("MutiItemContent").SetActive(false);
        if (mData.mResourceType == (int)EM_ResourceType.Good)
        {
            BaseGoodsDT baseGoodDT = glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(mData.mResourceId) as BaseGoodsDT;
            if (baseGoodDT.iEffect == (int)EM_GoodsEffect.OptionalReward)
            {
                f_GetObject("SingeonItemContent").SetActive(false);
                f_GetObject("MutiItemContent").SetActive(true);
                ShowMutiItem(baseGoodDT);
            }
            else
            {
                ShowSingleItem();
            }
        }
        else
        {
            ShowSingleItem();
        }
    }
    /// <summary>
    /// 显示多个物体信息
    /// </summary>
    private void ShowMutiItem(BaseGoodsDT baseGoodDT)
    {
        List<AwardPoolDT> tAwardDT = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(baseGoodDT.iEffectData);
        GridUtil.f_SetGridView<AwardPoolDT>(f_GetObject("MutiItemParent"), f_GetObject("MutiItem"), tAwardDT, OnMutiItemUpdate);
        f_GetObject("MutiItemParent").GetComponent<UIGrid>().Reposition();
        f_GetObject("MutiItemScrollView").GetComponent<UIScrollView>().ResetPosition();
    }
    /// <summary>
    /// 物体信息更新
    /// </summary>
    /// <param name="item"></param>
    /// <param name="awardPoolDT"></param>
    private void OnMutiItemUpdate(GameObject item, AwardPoolDT awardPoolDT)
    {
        item.GetComponent<ResourceCommonItem>().f_UpdateByInfo((byte)awardPoolDT.mTemplate.mResourceType, awardPoolDT.mTemplate.mResourceId, awardPoolDT.mTemplate.mResourceNum);
        f_RegClickEvent(item, UI_ShowAward, awardPoolDT.mTemplate);
    }
    /// <summary>
    /// 点击icon显示详细信息
    /// </summary>
    void UI_ShowAward(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetail2Page, UIMessageDef.UI_OPEN, obj1);
    }
    /// <summary>
    /// 显示单个物体信息
    /// </summary>
    private void ShowSingleItem()
    {
        //mTitle.text = mData.mResourceType == (int)EM_ResourceType.Good ? CommonTools.f_GetTransLanguage(2047) : CommonTools.f_GetTransLanguage(2048);
        UITool.f_SetIconSprite(mIcon, (EM_ResourceType)mData.mResourceType, mData.mResourceId);
        string name = mData.mName;
        string spriteBorderName = UITool.f_GetImporentColorName(mData.mImportant, ref name);
        mName.text = name;
        mDesc.text = mData.mDesc;
        f_GetObject("IconBorder").GetComponent<UISprite>().spriteName = spriteBorderName;
        if (mData.mResourceNum > 0)
        {
            f_GetObject("GoodNum").SetActive(true);
            f_GetObject("GoodNum").GetComponent<UILabel>().text = UITool.f_GetMoney(mData.mResourceNum);
        }
        else
        {
            f_GetObject("GoodNum").SetActive(false);
        }
        mHaveNum.text = UITool.f_GetMoney(UITool.f_GetResourceHaveNum(mData.mResourceType, mData.mResourceId));
    }
    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        ccU3DEngine.ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_CLOSE);
    }
}
