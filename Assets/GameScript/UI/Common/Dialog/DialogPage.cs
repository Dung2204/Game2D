using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class DialogPage : UIFramwork
{
    private DialogItem _leftDialogItem;
    private DialogItem _rightDialogItem;
    private GameObject _skipBtn;

    private bool mIsPlaying = false;
    private int mDialogIdx = 0;
    private List<DungeonDialogDT> mData;
    private EM_DialogcCondition mCondition;
    private ccCallback mCallBack_AllFinish;
    private int DialogCondition;

    private int Time_PlayDialog;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _leftDialogItem = f_GetObject("LeftDialogItem").GetComponent<DialogItem>();
        _rightDialogItem = f_GetObject("RightDialogItem").GetComponent<DialogItem>();
        _skipBtn = f_GetObject("SkipBtn");
        UIEventListener.Get(_skipBtn).onClick = f_SkipBtnHandle;
        UIEventListener.Get(f_GetObject("MaskClose")).onClick = f_MaskClose;
        //f_RegClickEvent("MaskClose", f_MaskClose);
    }

    protected override void UI_OPEN(object e)
    {
        _NeedCloseSound = false;
        base.UI_OPEN(e);
        if (e == null || !(e is DialogPageParams))
        {
MessageBox.ASSERT("DialogPage must be passed the DialogPageParams parameter");
        }
        DialogPageParams tValue = (DialogPageParams)e;
        mData = tValue.mDialogData;
        mCondition = tValue.mCondition;
        mCallBack_AllFinish = tValue.mDialogFinishCallBack;
        _leftDialogItem.f_ShowOver();
        _rightDialogItem.f_ShowOver();
        mDialogIdx = -1;
        _skipBtn.SetActive(true);
        f_PlayNextDialog(null);

        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.FIGHTPAUSE);
		ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_CLOSE);
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_PlayDialog);
        switch (mCondition)
        {
            case EM_DialogcCondition.BattleStart:
                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.DIALOGSTARBATTLE);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 一个对话Item 播放剧情完成
    /// </summary>
    /// <param name="value"></param>
    private void f_PlayDialogItemFinish(object value)
    {
        mIsPlaying = false;
        float time = (float) value;
        Time_PlayDialog = ccTimeEvent.GetInstance().f_RegEvent(time, false, null, f_PlayNextDialog, true);
    }

    /// <summary>
    /// 所有对话 播放完成
    /// </summary>
    /// <param name="value"></param>
    private void f_PlayDialogFinish()
    {
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.FIGHTRESUME);
        if (mCallBack_AllFinish != null)
        {
            mCallBack_AllFinish(mCondition);
        }
        mData = null;
        mCallBack_AllFinish = null;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DialogPage, UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_AdudioManager.f_StopAudioButtle();
    }

    //点击背景就播放下一个
    private void f_MaskClose(GameObject go)
    {
        if (!mIsPlaying)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_PlayDialog);
            f_PlayNextDialog(null);
        }
    }

    //播放下一个对话
    private void f_PlayNextDialog(object obj)
    {
        if (null == _skipBtn|| mData==null)
        {
            return;
        }

        mDialogIdx++;
        
        _skipBtn.SetActive(mDialogIdx > 0);
        if (mDialogIdx >= mData.Count)
            f_PlayDialogFinish();
        else
        {
            if (mData[mDialogIdx].iAnchor == (int)EM_DialogAnchor.Dialog_Left)
            {
                mIsPlaying = true;
                _rightDialogItem.f_ShowOver();
                _leftDialogItem.f_ShowDialog(mData[mDialogIdx], f_PlayDialogItemFinish);
            }
            else if (mData[mDialogIdx].iAnchor == (int)EM_DialogAnchor.Dialog_Right)
            {
                mIsPlaying = true;
                _leftDialogItem.f_ShowOver();
                _rightDialogItem.f_ShowDialog(mData[mDialogIdx], f_PlayDialogItemFinish);
            }
        }
    }

    private void f_SkipBtnHandle(GameObject go)
    {
        f_PlayDialogFinish();
    }
}

public class DialogPageParams
{
    public DialogPageParams(List<DungeonDialogDT> data, EM_DialogcCondition condition, ccCallback finishCallBack)
    {
        mDialogData = data;
        mCondition = condition;
        mDialogFinishCallBack = finishCallBack;
    }

    public List<DungeonDialogDT> mDialogData
    {
        get;
        private set;
    }

    public EM_DialogcCondition mCondition
    {
        get;
        private set;
    }

    public ccCallback mDialogFinishCallBack
    {
        get;
        private set;
    }
}
