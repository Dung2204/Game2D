using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LabelTipPage : UIFramwork
{
    private GameObject _itemParent;
    private GameObject _item;

    //单位为毫秒  1000毫秒 = 1秒 
    private int IgnoreTime = 1000;

    //item表现
    List<LabelTipItem> mLabelList = new List<LabelTipItem>();

    //展示的Idx +=2，展示label会根据这个设置深度
    private int mShowIdx = 0;
    //用来重置mShowIdx 使用的计数
    private int mShowIdxCount = 0;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _itemParent = f_GetObject("ItemParent");
        _item = f_GetObject("LabelTipItem");
    }


    /// <summary>
    /// 通过字符串传入显示
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if(e == null || !(e is string))
        {
MessageBox.ASSERT("LabelTipPage the input parameter must be of type string");
            return;
        }
        string tContent = (string)e;
        if (!f_IgnoreCheck(tContent))
            f_ShowLabel(tContent);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }


    /// <summary>
    /// 忽视检查（相同的字符串且时间少于忽视时间 就 忽略）
    /// </summary>
    /// <returns></returns>
    private bool f_IgnoreCheck(string content)
    {
        List<LabelTipItem> tSameData = mLabelList.FindAll(delegate (LabelTipItem item) { return item.mContent == content; });
        if (tSameData.Count == 0)
            return false;
        long tLastTime = 0;
        for (int i = 0; i < tSameData.Count; i++)
        {
            if (tLastTime < tSameData[i].mShowTime)
            {
                tLastTime = tSameData[i].mShowTime;
            }
        }
        if ((System.DateTime.Now.Ticks - tLastTime) / 10000 > IgnoreTime)
            return false;
        else
            return true;
    }

    private void f_ShowLabel(string content)
    {
        //先查找有没有未使用的，如果没有就创建，如果有就直接使用
        mShowIdx += 2;
        mShowIdxCount += 2;
        LabelTipItem tItem = mLabelList.Find(delegate (LabelTipItem item) { return item.mIsInUse == false; });
        if (tItem != null)
            tItem.f_Show(content,mShowIdx,f_CallFinish);
        else
        {
            tItem = LabelTipItem.f_Create(_itemParent, _item); 
            mLabelList.Add(tItem);
            tItem.f_Show(content, mShowIdx,f_CallFinish);
        }
    }

    //展示Idx 完成后减去
    private void f_CallFinish(object value1)
    {
        mShowIdxCount -= 2;
        //重置ShowIdx
        if (mShowIdxCount <= 0)
        {
            mShowIdxCount = 0;
            mShowIdx = 0;
        }
    }
}
