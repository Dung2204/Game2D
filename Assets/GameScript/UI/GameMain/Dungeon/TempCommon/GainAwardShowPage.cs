using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GainAwardShowPage : UIFramwork
{
    private UIGrid awardGrid;
    private GameObject awardItem;
    private ResourceCommonItemComponent _awardShowComponent;
    private ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(awardGrid, awardItem);
            return _awardShowComponent;
        }
    }
    private GameObject mMaskClose;
    private ccUIBase mNeedHoldUI;
    private List<AwardPoolDT> _awardList;


    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        awardGrid = f_GetObject("AwardGrid").GetComponent<UIGrid>();
        awardItem = f_GetObject("ResourceCommonItem");
        mMaskClose = f_GetObject("MaskClose");
        f_RegClickEvent(mMaskClose, f_MaskClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is object[]))
        {
MessageBox.ASSERT("GainAwardShowPage needs to be passed object[2] idx:0 in the rewards list，1: interface jumps over to another interface needs to be kept");
        }
        object[] tdata = (object[])e;
        if (tdata.Length == 1)
        {
            _awardList = (List<AwardPoolDT>)tdata[0];
            mNeedHoldUI = null;
        }
        else
        {
            _awardList = (List<AwardPoolDT>)tdata[0];
            mNeedHoldUI = (ccUIBase)tdata[1];
        }

        AwardPoolDT money= _awardList.Find((AwardPoolDT a)=>{
            if (a.mTemplate.mResourceType == (int)EM_ResourceType.Money) {
                if (a.mTemplate.mResourceId== (int)EM_UserAttr.eUserAttr_TaskScore) {
                    return true;
                }
            }
            return false;
        } );

        _awardList.Remove(money);

        //根据类型跳转界面暂时注释
        //mAwardShowComponent.f_Show(_awardList[i].mTemplate,EM_CommonItemShowType.All,EM_CommonItemClickType.Normal,this);
        //现在全部展示Tip
        mAwardShowComponent.f_Show(_awardList, EM_CommonItemShowType.All, EM_CommonItemClickType.AllTip, this);
        awardGrid.repositionNow = true;
        GameObject TexTitle = f_GetObject("TexTitle");
        if (TexTitle.transform.GetComponent<TweenScale>() != null)
        {
            Destroy(TexTitle.transform.GetComponent<TweenScale>());
        }
        TweenScale ts = TexTitle.AddComponent<TweenScale>();
        ts.from = new Vector3(0.3f, 0.3f, 0.3f);
        ts.to = new Vector3(1, 1, 1);
        ts.animationCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        if (mNeedHoldUI != null)
        {
            ccUIHoldPool.GetInstance().f_Hold(mNeedHoldUI);
        }
    }

    /// <summary>
    /// unhold 目前是同步  先用
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        if (mNeedHoldUI != null)
        {
            ccUIHoldPool.GetInstance().f_UnHold();
        }
    }

    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        Data_Pool.m_GuidancePool.f_ChangeGuidanceType(EM_GuidanceType.GetAward);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_CLOSE);
    }
}
