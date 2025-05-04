using ccU3DEngine;
using UnityEngine;

public class PlotTextTyping : ccMachineStateBase
{
    protected PlotSysManager _PlotSysManager;
    private string[] mArrTypingTxt;
    private int mPageIndex;
    private int mTxtIndex;
    private const float fTypingFrequency = 0.02f;
    private float fTotalTime = 0;
    public PlotTextTyping(PlotSysManager plotSysManager) : base((int)EM_PlotState.EM_PlotState_TextTyping)
    {
        _PlotSysManager = plotSysManager;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        if (null == Obj)
        {
MessageBox.ASSERT("The plot text input parameter is empty！！！");
            f_SetComplete((int)EM_PlotState.EM_PlotState_Wait);
            return;
        }
        PlotDT plotDt = (PlotDT)Obj;
        mArrTypingTxt = plotDt.szEffectParams.Replace("\\n","\n").Split('^');
        _PlotSysManager.mlabelTypingTxt.transform.parent.gameObject.SetActive(true);
        mTxtIndex = 0;
        mPageIndex = 0;
        fTotalTime = 0;
    }

    public override void f_Execute()
    {
        base.f_Execute();
        fTotalTime += Time.deltaTime;
        if (fTotalTime < fTypingFrequency)
            return;
        fTotalTime = 0;       

        if (mPageIndex >= mArrTypingTxt.Length)
        {
            f_SetComplete((int)EM_PlotState.EM_PlotState_Wait);
            return;
        }

        //每页字一个个显示
        string pageText = mArrTypingTxt[mPageIndex];
        if (pageText == "")
        {
            mPageIndex++;
            mTxtIndex = 0;
            return;
        }
            
        if (pageText[mTxtIndex] == '\\') {
            //遇到转义字符则下延一位
            mTxtIndex++;
        }

        //字“一个个”出现
        _PlotSysManager.mlabelTypingTxt.text = pageText.Substring(0,mTxtIndex + 1);
        mTxtIndex++;
        if (mTxtIndex >= pageText.Length) {
            mPageIndex++;
            mTxtIndex = 0;
        }
    }

    public override void f_Exit()
    {
        _PlotSysManager.mlabelTypingTxt.transform.parent.gameObject.SetActive(false);
        base.f_Exit();
    }

    
}
