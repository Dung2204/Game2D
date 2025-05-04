using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 根据奖励列表展示奖励ResourceCommonItem
/// </summary>
public class ResourceCommonItemComponent
{
    private UIGrid _awardGrid;
    private GameObject _awardItem;

    private List<AwardPoolDT> _awardList;
    private List<ResourceCommonItem> _itemList = new List<ResourceCommonItem>();

    private EM_CommonItemShowType _curShowType;
    private EM_CommonItemClickType _curClickType;
    private ccUIBase _curNeedHoldUI;

    //初始化
    public ResourceCommonItemComponent(UIGrid awardGrid, GameObject awardItem)
    {
        _awardGrid = awardGrid;
        _awardItem = awardItem;
    }

    public void f_ShowEffect()
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (_itemList[i].gameObject.activeSelf)
            {
                UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinGoodEffect, _itemList[i].transform, 0, 0, false, 1f, 2f);
            }
        }
    }

    //展示
    public void f_Show(List<AwardPoolDT> awardList, EM_CommonItemShowType showType = EM_CommonItemShowType.All, EM_CommonItemClickType clickType = EM_CommonItemClickType.AllTip, ccUIBase needHoldUI = null)
    {
        _awardList = awardList;
        f_ShowAwardByList(showType, clickType, needHoldUI);
    }
    private void f_ShowAwardByList(EM_CommonItemShowType showType, EM_CommonItemClickType clickType, ccUIBase needHoldUI)
    {
        int tmp = 0;

        if (_itemList.Count < _awardList.Count)
        {
            int tAddNum = _awardList.Count - _itemList.Count;
            for (int i = 0; i < tAddNum; i++)
            {
                ResourceCommonItem tItem = ResourceCommonItem.f_Create(_awardGrid.gameObject, _awardItem);
                _itemList.Add(tItem);
            }
        }
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (i < _awardList.Count)
            {
                _itemList[i].f_UpdateByInfo(_awardList[i].mTemplate, showType, clickType, needHoldUI);
                tmp++;
            }
            else
            {
                _itemList[i].f_Disable();
            }
        } 
        _awardGrid.repositionNow = true;
        _awardGrid.Reposition();
        UIScrollView tScrollView = _awardGrid.transform.parent.GetComponent<UIScrollView>();
        if (tScrollView != null)
        {
            if (_awardList.Count <= 4)
                tScrollView.contentPivot = UIWidget.Pivot.Center;
            else
                tScrollView.contentPivot = UIWidget.Pivot.Left;
            tScrollView.ResetPosition();
        }
    }
}
