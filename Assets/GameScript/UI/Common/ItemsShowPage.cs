using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
public class ItemShowPageParam
{
    /// <summary>
    /// 标题文本
    /// </summary>
    public string m_title;
    /// <summary>
    /// 内容列表
    /// </summary>
    public List<ItemShowGoodItem> m_listItem;
}
public class ItemShowGoodItem
{
    public ItemShowGoodItem(EM_ResourceType resourceType, int resourId, int count)
    {
        m_resourceType = resourceType;
        m_resourceId = resourId;
        m_count = count;
    }
    public EM_ResourceType m_resourceType;//资源类型
    public int m_resourceId;//资源id
    public int m_count;//资源数量
}
/// <summary>
/// 显示物体内容
/// </summary>
public class ItemsShowPage : UIFramwork {
    private UIWrapComponent m_contentWrapComponent = null;
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();
    private Dictionary<int, ItemShowGoodItem> dicContent = new Dictionary<int, ItemShowGoodItem>();
    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        ItemShowPageParam param = (ItemShowPageParam)e;
        //f_GetObject("LabelTitle").GetComponent<UILabel>().text = param.m_title;
        listContent.Clear();
        dicContent.Clear();
        for (int i = 0; i < param.m_listItem.Count; i++)
        {
            BasePoolDT<long> item = new BasePoolDT<long>();
            item.iId = i;
            listContent.Add(item);
            dicContent.Add(i, param.m_listItem[i]);
        }
        if (m_contentWrapComponent == null)
        {
            m_contentWrapComponent = new UIWrapComponent(186, 1, 800, 6, f_GetObject("ItemParent"), f_GetObject("GoodItem"), listContent, OnUpdateItem, null);
        }
        m_contentWrapComponent.f_ResetView();
    }
    /// <summary>
    /// item更新
    /// </summary>
    /// <param name="item"></param>
    /// <param name="dt"></param>
    private void OnUpdateItem(Transform item, BasePoolDT<long> dt)
    {
        ItemShowGoodItem data = dicContent[(int)dt.iId];
        ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
        resourceCommonDT.f_UpdateInfo((byte)data.m_resourceType, data.m_resourceId, 0);
        string Name = UITool.f_GetGoodName(data.m_resourceType, data.m_resourceId);
        string BorderName = UITool.f_GetImporentColorName(resourceCommonDT.mImportant, ref Name);
        UITool.f_SetIconSprite(item.Find("SprHead").GetComponent<UI2DSprite>(), data.m_resourceType, data.m_resourceId);
        item.Find("IconBorder").GetComponent<UISprite>().spriteName = BorderName;
        item.Find("GoodName").GetComponent<UILabel>().text = Name;
        item.Find("GoodNum").GetComponent<UILabel>().text = data.m_count.ToString();
        item.Find("GoodDes").GetComponent<UILabel>().text = UITool.f_GetGoodDescribe(data.m_resourceType, data.m_resourceId); 
        f_RegClickEvent(item.Find("SprHead").gameObject, OnGoodItemClick, data);
    }
    /// <summary>
    /// 点击内容中的物品头像
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void OnGoodItemClick(GameObject go, object obj1, object obj2)
    {
        ItemShowGoodItem data =(ItemShowGoodItem)obj1;
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)data.m_resourceType, data.m_resourceId, data.m_count);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
}
    /// <summary>
    /// 初始化注册事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("MaskClose", OnCloseBlackClick);
    }
    /// <summary>
    /// 点击界面黑色背景
    /// </summary>
    private void OnCloseBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ItemsShowPage, UIMessageDef.UI_CLOSE);
    }
}
