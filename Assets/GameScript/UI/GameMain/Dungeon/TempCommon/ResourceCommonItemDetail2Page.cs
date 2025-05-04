using UnityEngine;
using System.Collections;

public class ResourceCommonItemDetail2Page : UIFramwork
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

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is ResourceCommonDT))
        {
		MessageBox.ASSERT("ResourceCommonItemDetailPage must be passed to ResourceCommonDT");
            return;
        }
        mData = (ResourceCommonDT)e;
		mTitle.text = mData.mResourceType == (int)EM_ResourceType.Good ? "Info" : "Detail";
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
        ccU3DEngine.ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetail2Page, UIMessageDef.UI_CLOSE);
    }
}
