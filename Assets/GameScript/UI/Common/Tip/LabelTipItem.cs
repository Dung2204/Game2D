using ccU3DEngine;
using UnityEngine;
using System;
using System.Collections;

public class LabelTipItem : MonoBehaviour
{
    public static LabelTipItem f_Create(GameObject parent, GameObject item)
    {
        GameObject go = NGUITools.AddChild(parent, item);
        LabelTipItem result = go.GetComponent<LabelTipItem>();
        if (result == null)
MessageBox.ASSERT("LabelTipItem f_Create Item parameter must contain LabelTipItem code");
        else
            result.f_Init();
        return result;
    }

    public GameObject _go;
    public TweenScale _tweenScale;
    public TweenPosition _tweenPosition;
    public TweenAlpha _tweenAlpha;
    public UILabel _contentLabel;
    //public UISprite _contentBG1;
    //public UISprite _contentBG2;

    private ccCallback _finishCallBack;

    /// <summary>
    /// 是否正在使用中
    /// </summary>
    public bool mIsInUse
    {
        get;
        private set;
    }

    /// <summary>
    /// 内容
    /// </summary>
    public string mContent
    {
        get;
        private set;
    }

    //展示的时间(当前时间  单位为毫秒)
    public long mShowTime
    {
        get;
        private set;
    }

    private void f_Init()
    {
        EventDelegate.Add(_tweenScale.onFinished,f_ScaleFinish);
        EventDelegate.Add(_tweenPosition.onFinished, f_PositionFinish);
        EventDelegate.Add(_tweenAlpha.onFinished, f_AlphaFinish);
        mIsInUse = false;
        mContent = string.Empty;
        mShowTime = 0;
    }

    public void f_Show(string content,int idx,ccCallback finishCallBack)
    {
        if (mIsInUse)
        {
            return;
        }
        _finishCallBack = finishCallBack;
        mContent = content;
        mIsInUse = true;
        _go.SetActive(true);
        mShowTime = DateTime.Now.Ticks;
        _contentLabel.text = mContent;
        _contentLabel.depth = idx + 1;
        //_contentBG1.depth = idx;
        //_contentBG2.depth = idx;
        //_contentBG1.height = 120 + _contentLabel.height;
        //_contentBG2.height = 120 + _contentLabel.height;
        //_contentBG1.width = _contentLabel.width / 2 + 128;
        //_contentBG2.width = _contentLabel.width / 2 + 128;
        _tweenScale.ResetToBeginning();
        _tweenPosition.ResetToBeginning();
        _tweenAlpha.ResetToBeginning();
        _tweenScale.PlayForward();
    }


    private void f_ScaleFinish()
    {
        f_WaitForOneFrame(_tweenPosition.PlayForward);
    }

    private void f_PositionFinish()
    {
        f_WaitForOneFrame(_tweenAlpha.PlayForward);
    }
    private void f_AlphaFinish()
    {
        mIsInUse = false;
        mContent = string.Empty;
        _go.SetActive(false);
        if (_finishCallBack != null)
        {
            _finishCallBack(null);
            _finishCallBack = null;
        }
    }

    /// <summary>
    /// 等待一帧执行
    /// </summary>
    /// <param name="callback"></param>
    private void f_WaitForOneFrame(EventDelegate.Callback callback)
    {
        StartCoroutine(WaitOneFrameCallback(callback));
    }

    IEnumerator WaitOneFrameCallback(EventDelegate.Callback callback)
    {
        yield return 0;
        if (callback != null)
        {
            callback();
        }
    }

}
