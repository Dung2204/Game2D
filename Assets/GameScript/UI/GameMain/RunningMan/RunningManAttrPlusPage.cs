using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunningManAttrPlusPage : UIFramwork
{
    private GameObject _attrDragObj;
    private UIGrid _attrGrid;
    private UIScrollView _attrScrollView;
    private List<GameObject> _listObjAttrItem;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _attrDragObj = f_GetObject("Label_Attr");
        _attrGrid = f_GetObject("Grid").GetComponent<UIGrid>();
        _listObjAttrItem = new List<GameObject>();
        f_RegClickEvent("CloseMask", f_CloseMask); 
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        //过滤掉属性为0的部分
        List<RunningManBuffProperty> buffPropertyTempList = Data_Pool.m_RunningManPool.m_BuffPropertyList;
        List<RunningManBuffProperty> buffPropertyList = new List<RunningManBuffProperty>();
        for (int i = 0; i < buffPropertyTempList.Count; i++)
        {
            RunningManBuffProperty property = buffPropertyTempList[i];
            if (null == property || property.m_iPropertyValue <= 0)
                continue;
            buffPropertyList.Add(property);
        }

        //不够预制的话克隆
        for (int i = _listObjAttrItem.Count; i < buffPropertyList.Count; i++)
        {
            GameObject attrItem = NGUITools.AddChild(_attrGrid.gameObject, _attrDragObj);
            _listObjAttrItem.Add(attrItem);
        }

        //设置属性对象
        for (int i = 0; i < buffPropertyList.Count; i++)
        {
            RunningManBuffProperty property = buffPropertyList[i];
            GameObject attrItem = _listObjAttrItem[i];
            if (null == property || null == attrItem)
                continue;
            attrItem.SetActive(true);
            attrItem.GetComponent<UILabel>().text = string.Format("{0}[00FF00FF]+{1}%", UITool.f_GetProName((EM_RoleProperty)property.m_iPropertyType), property.m_iPropertyValue / 10000.0f * 100);
        }

        //隐藏多余的预设
        for (int i = buffPropertyList.Count; i < _listObjAttrItem.Count; i++)
        {
            _listObjAttrItem[i].SetActive(false);
        }

        _attrGrid.repositionNow = true;
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_CloseMask(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManAttrPlusPage, UIMessageDef.UI_CLOSE);   
    }

}
