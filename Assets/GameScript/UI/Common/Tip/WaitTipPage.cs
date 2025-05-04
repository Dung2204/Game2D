using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class WaitTipPage : UIFramwork
{
    private GameObject mDelayShowItem;

    private int  delayEventId = EventInitId;
    private const int EventInitId = -99;
    private const float DelayTime = 1f;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mDelayShowItem = f_GetObject("DelayShowItem");
        mDelayShowItem.SetActive(false);    
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (delayEventId != EventInitId)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(delayEventId);
            delayEventId = EventInitId;
        }
        bool isFroceShow = false;
        if (e != null && e is bool)
            isFroceShow = (bool)e;
        if (isFroceShow)
        {
            mDelayShowItem.SetActive(true);
            //m_EventId = ccTimeEvent.GetInstance().f_RegEvent(5f, false, null, timeOut, false);
        }
        else
        {
            delayEventId = ccTimeEvent.GetInstance().f_RegEvent(DelayTime, false, null, f_DelayShow);
            //m_EventId = ccTimeEvent.GetInstance().f_RegEvent(DelayTime+5f, false, null, timeOut, false);
        }
    }

    int m_EventId = 0;
    private void timeOut(object value)
    {
        
        ccTimeEvent.GetInstance().f_UnRegEvent(m_EventId);
        _NeedCloseSound = false;
        if (delayEventId != EventInitId)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(delayEventId);
            delayEventId = EventInitId;
        }
        //mDelayShowItem.SetActive(false);
        if (mDelayShowItem)
        {
            mDelayShowItem.SetActive(false);
            //UITool.Ui_Trip("请求超时，请重试");
            UITool.f_OpenOrCloseWaitTip(false);
        }
        //mDelayShowItem.SetActive(false);
    }

    protected override void UI_CLOSE(object e)
    {
        _NeedCloseSound = false;
        base.UI_CLOSE(e);
        if (delayEventId != EventInitId)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(delayEventId);
            delayEventId = EventInitId;
        }
        mDelayShowItem.SetActive(false);
        ccTimeEvent.GetInstance().f_UnRegEvent(m_EventId);
    }

    protected override void On_Destory()
    {
        base.On_Destory();
        _NeedCloseSound = false;
        if (delayEventId != EventInitId)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(delayEventId);
            delayEventId = EventInitId;
        }
        mDelayShowItem.SetActive(false);
        ccTimeEvent.GetInstance().f_UnRegEvent(m_EventId);
    }

    private void f_DelayShow(object value)
    {
        mDelayShowItem.SetActive(true);
        delayEventId = EventInitId;
    }

}
