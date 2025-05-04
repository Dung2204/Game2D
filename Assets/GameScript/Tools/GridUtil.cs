using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GridUtil
{
    /// <summary>
    /// 每增加一个Item就会调用一次这个回调
    /// </summary>
    /// <typeparam name="T">传入数据的类型</typeparam>
    /// <param name="oneItem">新增加的物体</param>
    /// <param name="dataInfo">物体的信息</param>
    public delegate void OnAddItemCallback<T>(GameObject oneItem, T dataInfo);

    /// <summary>
    /// 读取一个列表里面的信息显示到一个Grid控件下面
    /// </summary>
    /// <typeparam name="T">传入数据的类型</typeparam>
    /// <param name="gridRoot">Grid节点</param>
    /// <param name="itemPrefab">每一个item的模板</param>
    /// <param name="dataInfoList">信息列表</param>
    /// <param name="addItemCallback">创建一个item时的回调,作显示或者保存信息处理</param>

    public static void f_SetGridView<T>(GameObject gridRoot, GameObject itemPrefab, List<T> dataInfoList, OnAddItemCallback<T> addItemCallback, bool index = false)
    {
        Transform[] childNodes = f_GetChild(gridRoot.transform);

        f_SetChildEnable(childNodes, false);
        UIGrid grid = gridRoot.GetComponent<UIGrid>();
        grid.enabled = false;
        if (null != dataInfoList)
        {
            for (int i = 0; i < dataInfoList.Count; i++)
            {
                GameObject oneChild = f_GetChildOfList(childNodes, i);
                if (null == oneChild)
                {
                    oneChild = NGUITools.AddChild(gridRoot, itemPrefab);
                }
                if (index)
                {
                    oneChild.name = i.ToString();
                }
                oneChild.SetActive(true);
                
                Vector3 pos = new Vector3();
                if (grid.arrangement == UIGrid.Arrangement.Horizontal)
                {
                    if (grid.maxPerLine == 0)
                    {
                        float offset = 0;
                        if (grid.pivot == UIWidget.Pivot.Center)
                        {
                            offset = (dataInfoList.Count - 1) * grid.cellWidth / 2;
                        }

                        pos.x = i * grid.cellWidth - offset;
                    }
                    else
                    {
                        pos.x = (i % grid.maxPerLine) * grid.cellWidth;
                        pos.y = -(i / grid.maxPerLine) * grid.cellHeight;
                    }
                }
                else
                {
                    if (grid.maxPerLine == 0)
                    {
                        pos.y = 0 - i * grid.cellHeight;
                    }
                    else
                    {
                        pos.x = (i / grid.maxPerLine) * grid.cellWidth;
                        pos.y = -(i % grid.maxPerLine) * grid.cellHeight;
                    }
                }
                oneChild.transform.localPosition = pos;

                if (addItemCallback != null)
                    addItemCallback(oneChild, dataInfoList[i]);
            }
        }
    }

    public static void f_SetGridView<T>(bool resetPos, GameObject gridRoot, GameObject itemPrefab, List<T> dataInfoList, OnAddItemCallback<T> addItemCallback)
    {
        Transform[] childNodes = f_GetChild(gridRoot.transform);
        f_SetChildEnable(childNodes, false);
        UIGrid grid = gridRoot.GetComponent<UIGrid>();
        grid.enabled = false;
        if (null != dataInfoList)
        {
            for (int i = 0; i < dataInfoList.Count; i++)
            {
                GameObject oneChild = f_GetChildOfList(childNodes, i);
                if (null == oneChild)
                {
                    oneChild = NGUITools.AddChild(gridRoot, itemPrefab);
                }

                oneChild.SetActive(true);
                if (addItemCallback != null)
                    addItemCallback(oneChild, dataInfoList[i]);
            }
            if (resetPos)
            {
                grid.repositionNow = true;
                grid.Reposition();
            }
        }
    }

    public static Transform[] f_GetChild(Transform tra)
    {
        Transform[] childs = new Transform[tra.childCount];

        for (int idx = 0; idx < tra.childCount; idx++)
        {
            childs[idx] = tra.GetChild(idx);
        }

        return childs;
    }

    /// <summary>
    /// 获得某个gameobject下面隐藏的子物体
    /// </summary>
    public static GameObject f_GetChildOfList(Transform tra)
    {
        for (int idx = 0; idx < tra.childCount; idx++)
        {
            if (!tra.GetChild(idx).gameObject.activeSelf)
                return tra.GetChild(idx).gameObject;
        }
        return null;
    }

    public static GameObject f_GetChildOfList(Transform[] tra, int index)
    {
        if (tra == null || tra.Length <= index)
            return null;

        return tra[index].gameObject;
    }

    /// <summary>
    /// 设置列表里面子物体的显示/隐藏
    /// </summary>
    public static void f_SetChildEnable(Transform t, bool isEnable)
    {
        for (int idx = 0; idx < t.childCount; idx++)
        {
            t.GetChild(idx).gameObject.SetActive(isEnable);
        }
    }

    public static void f_SetChildEnable(Transform[] t, bool isEnable)
    {
        if (t == null)
            return;

        for (int i = 0; i < t.Length; i++)
        {
            t[i].gameObject.SetActive(isEnable);
        }
    }
    /////TSuCode - CheckFunc


    public static void f_SetGridView_Tsu<T>(GameObject gridRoot, GameObject itemPrefab, List<T> dataInfoList, OnAddItemCallback<T> addItemCallback)
    {
        Transform[] childNodes = f_GetChild(gridRoot.transform);

        //f_SetChildEnable(childNodes, false);
        UIGrid grid = gridRoot.GetComponent<UIGrid>();
        grid.enabled = false;
        if (null != dataInfoList)
        {
            for (int i = 0; i < dataInfoList.Count; i++)
            {
                GameObject oneChild = f_GetChildOfList(childNodes, i+1); //i -> i +1, 0: Btn
                if (null == oneChild)
                {
                    oneChild = NGUITools.AddChild(gridRoot, itemPrefab);
                }

                oneChild.SetActive(true);

                Vector3 pos = new Vector3();
                if (grid.arrangement == UIGrid.Arrangement.Horizontal)
                {
                    if (grid.maxPerLine == 0)
                    {
                        float offset = 0;
                        if (grid.pivot == UIWidget.Pivot.Center)
                        {
                            offset = (dataInfoList.Count - 1) * grid.cellWidth / 2;
                        }

                        pos.x = i * grid.cellWidth - offset;
                    }
                    else
                    {
                        pos.x = (i % grid.maxPerLine) * grid.cellWidth;
                        pos.y = -(i / grid.maxPerLine) * grid.cellHeight;
                    }
                }
                else
                {
                    if (grid.maxPerLine == 0)
                    {
                        pos.y = 0 - i * grid.cellHeight;
                    }
                    else
                    {
                        pos.x = (i / grid.maxPerLine) * grid.cellWidth;
                        pos.y = -(i % grid.maxPerLine) * grid.cellHeight;
                    }
                }
                oneChild.transform.localPosition = pos;

                if (addItemCallback != null)
                    addItemCallback(oneChild, dataInfoList[i]);
            }
        }
    }
    ///
}
