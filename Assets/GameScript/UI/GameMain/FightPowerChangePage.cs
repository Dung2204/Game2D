using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//战斗力改变参数
public class FightPowerChangeParam
{
    public int nCurFightPower;    //当前战斗力
    public int nLastFightPower;   //上一次战斗力

    public FightPowerChangeParam(int curFightPower,int lastFightPower)
    {
        nCurFightPower = curFightPower;
        nLastFightPower = lastFightPower;
    }
}

public class FightPowerChangePage : UIFramwork
{
    private UILabel txtUpFightPower;
    private UILabel txtDownFightPower;
    private UILabel txtFightPower;
    private const float fNumChangeTotalTime = 1.2f;       //数字滚动总时间
    private const float fNumChangeTime = 0.02f;           //数字滚动时间
    private const float fOpenUITime = 2f;                 //界面打开时间
    private int   mNumAddPerTime;                         //每一次滚动添加的数值
    private int   mChangePower;                           //改变的战斗力
    private float mUpdateTime;                            //刷新的时间
    private float mTotalUpdateTime;                       //刷新的总时间
    private FightPowerChangeParam mFightPowerChangeParam; //战斗力改变界面参数

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        txtUpFightPower = f_GetObject("Label_Up").GetComponent<UILabel>();
        txtDownFightPower = f_GetObject("Label_Down").GetComponent<UILabel>();
        txtFightPower = f_GetObject("Label_FightPower").GetComponent<UILabel>();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e">EquipSythesis类似</param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (null == e)
        {
MessageBox.ASSERT("FightPowerChangePage， parameter null！");
            return;
        }
        FightPowerChangeParam fightPowerChangeParam = e as FightPowerChangeParam;
        if (null == fightPowerChangeParam) return;
MessageBox.DEBUG(string.Format("Change combat strength, before： {0}, after： {1}", fightPowerChangeParam.nLastFightPower, fightPowerChangeParam.nCurFightPower));
        mFightPowerChangeParam = fightPowerChangeParam;
        mUpdateTime = 0;
        mTotalUpdateTime = 0;
        enabled = true;

        //设置改变的数值//UITool.f_CountToChineseStr(fightPower);
        mChangePower = fightPowerChangeParam.nCurFightPower - fightPowerChangeParam.nLastFightPower;
        if (mChangePower == 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.FightPowerChangePage, UIMessageDef.UI_CLOSE);
            return;
        }
        txtDownFightPower.transform.parent.gameObject.SetActive(mChangePower < 0);
        txtDownFightPower.text = mChangePower.ToString();
        txtUpFightPower.transform.parent.gameObject.SetActive(mChangePower > 0);
        txtUpFightPower.text = mChangePower.ToString();
        txtFightPower.text = fightPowerChangeParam.nLastFightPower.ToString();
        
        //每次改变的差值
        mNumAddPerTime = (int)(fNumChangeTime * mChangePower / fNumChangeTotalTime);
        if (mNumAddPerTime == 0)
        {
            mNumAddPerTime = mChangePower > 0 ? 1 : -1;
        }
    }

    void FixedUpdate()
    {
        mUpdateTime += Time.fixedDeltaTime;
        mTotalUpdateTime += Time.fixedDeltaTime;
        if (mTotalUpdateTime >= fOpenUITime)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.FightPowerChangePage, UIMessageDef.UI_CLOSE);
            enabled = false;
            return;
        }

        if (!m_Panel.gameObject.activeInHierarchy || mUpdateTime < fNumChangeTime ||
            mFightPowerChangeParam.nLastFightPower == mFightPowerChangeParam.nCurFightPower)
        {
            return;
        }

        //累加差值
        mUpdateTime = 0;
        mFightPowerChangeParam.nLastFightPower += mNumAddPerTime;
        if ((mChangePower > 0 && mFightPowerChangeParam.nLastFightPower >= mFightPowerChangeParam.nCurFightPower) ||
        (mChangePower < 0) && mFightPowerChangeParam.nLastFightPower <= mFightPowerChangeParam.nCurFightPower)
        {
            mFightPowerChangeParam.nLastFightPower = mFightPowerChangeParam.nCurFightPower;
        }

        //定时累加数值达到数字滚动效果
        txtFightPower.text = mFightPowerChangeParam.nLastFightPower.ToString();
    }
}
