using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using Spine.Unity;
/// <summary>
/// 抽牌界面（通用）
/// </summary>
public class ChooseAwardPage : UIFramwork {
    /// <summary>
    /// 旋转时间
    /// </summary>
    private float timeRotate = 0.33f;
    /// <summary>
    /// 传递参数
    /// </summary>
    private ChooseAwardParam chooseAwardParam;
    /// <summary>
    /// 是否已经点击了牌
    /// </summary>
    private bool mAlreadyClick;
    /// <summary>
    /// 奖励动画有没有播放完成
    /// </summary>
    private bool mOverFlag;
    /// <summary>
    /// 点击的卡牌序号
    /// </summary>
    private int TouchIndex;

    /// <summary>
    /// 获得内容
    /// </summary>
    private UILabel mContent;

    /// <summary>
    /// 是否自动切换页面
    /// </summary>
    private bool isAutoChangePageFlag;
    private List<int> listHasShowData = new List<int>();
    /// <summary>
    /// Awake
    /// </summary>
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }
    /// <summary>
    /// 初始化UI
    /// </summary>
    protected override void InitGUI()
    {
        base.InitGUI();
        f_RegClickEvent("Choose1", f_ChoseAwardClick, 1);
        f_RegClickEvent("Choose2", f_ChoseAwardClick, 2);
        f_RegClickEvent("Choose3", f_ChoseAwardClick, 3);
        f_RegClickEvent("BlackBG", f_BlackBGHandle);
        mContent = f_GetObject("Content").GetComponent<UILabel>();
    }
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        chooseAwardParam = e as ChooseAwardParam;
        if (e == null || chooseAwardParam.mListData.Count < 3)
        {
MessageBox.ASSERT("Invalid parameter，cannot create card interface");
            return;
        }
        if (chooseAwardParam.mChooseIndex < 0)
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, "Error winning，reward is not in the list!");
            chooseAwardParam.mChooseIndex = 0;
        }
        listHasShowData.Clear();
        listHasShowData.Add(chooseAwardParam.mChooseIndex);
        mAlreadyClick = false;
        mOverFlag = false;
        isAutoChangePageFlag = false;
        ResetData(f_GetObject("Choose1"));
        ResetData(f_GetObject("Choose2"));
        ResetData(f_GetObject("Choose3"));
        mContent.text = string.Empty;
    }
    /// <summary>
    /// 重设数据
    /// </summary>
    /// <param name="go"></param>
    private void ResetData(GameObject go)
    {
        go.transform.Find("AwardItem").gameObject.transform.localScale = new Vector3(0, 1, 1);
        go.transform.Find("AwardItem").gameObject.SetActive(false);
        if(go.transform.Find("Animator") != null)
            go.transform.Find("Animator").GetComponent<SkeletonAnimation>().AnimationName = null;
    }
    /// <summary>
    /// 点击卡片牌事件
    /// </summary>
    private void f_ChoseAwardClick(GameObject go, object value1, object value2)
    {
        if (mAlreadyClick)
        {
            return;
        }
        mAlreadyClick = true;
        TouchIndex = (int)value1;
        if (go.transform.Find("Animator") != null)
            go.transform.Find("Animator").GetComponent<SkeletonAnimation>().AnimationName = "animation1";
        ChooseAwardItem chooseAwardItem = go.transform.Find("AwardItem").GetComponent<ChooseAwardItem>();
        chooseAwardItem.SetData(chooseAwardParam.mListData[chooseAwardParam.mChooseIndex]);
        f_RegClickEvent(chooseAwardItem.mBtnBox, OnAwardIconClick, chooseAwardParam.mListData[chooseAwardParam.mChooseIndex],null);
        ccTimeEvent.GetInstance().f_RegEvent(timeRotate, false, go, OnEnd);
        ccTimeEvent.GetInstance().f_RegEvent(timeRotate * 2, false, chooseAwardParam.mChooseIndex, ShowOtherCard);
    }
    /// <summary>
    /// 结束之后翻转牌
    /// </summary>
    /// <param name="data"></param>
    private void OnEnd(object data)
    {
        GameObject go = (GameObject)data;
        go.transform.Find("AwardItem").gameObject.SetActive(true);
        TweenScale ts = go.transform.Find("AwardItem").GetComponent<TweenScale>();
        ts.ResetToBeginning();
        ts.PlayForward();
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT commonData = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 展示其他奖励
    /// </summary>
    /// <param name="slectIndex"></param>
    private void ShowOtherCard(object slectIndex)
    {
        f_ShowAward(null);
        int curIndex = (int)slectIndex;
        if(TouchIndex != 1)
            ActCard(f_GetObject("Choose1"));
        if (TouchIndex != 2)
            ActCard(f_GetObject("Choose2"));
        if (TouchIndex != 3)
            ActCard(f_GetObject("Choose3"));
    }
    /// <summary>
    /// 激活其他未选中的卡牌
    /// </summary>
    /// <param name="go"></param>
    private void ActCard(GameObject go)
    {
        int index = Random.Range(0, chooseAwardParam.mListData.Count);
        while (listHasShowData.Contains(index))
        {
            index = Random.Range(0, chooseAwardParam.mListData.Count);
        }
        listHasShowData.Add(index);
        ChooseAwardItem chooseAwardItem = go.transform.Find("AwardItem").GetComponent<ChooseAwardItem>();
        chooseAwardItem.SetData(chooseAwardParam.mListData[index]);
        f_RegClickEvent(chooseAwardItem.mBtnBox, OnAwardIconClick, chooseAwardParam.mListData[index], null);
        ccTimeEvent.GetInstance().f_RegEvent(timeRotate, false, go, OnEndOther);
        ccTimeEvent.GetInstance().f_RegEvent(timeRotate * 2, false, null, SetOVerFlag);
        ccTimeEvent.GetInstance().f_RegEvent(timeRotate * 2 + 2.0f, false, null, SetAutoChangePageFlag);
        if(go.transform.Find("Animator") != null)
            go.transform.Find("Animator").GetComponent<SkeletonAnimation>().AnimationName = "animation1";
    }
    /// <summary>
    /// 设置可以关闭页面
    /// </summary>
    /// <param name="data"></param>
    private void SetOVerFlag(object data)
    {
        mOverFlag = true;
    }
    /// <summary>
    /// 设置自动切换页面
    /// </summary>
    /// <param name="data"></param>
    private void SetAutoChangePageFlag(object data)
    {
        isAutoChangePageFlag = true;
    }
    /// <summary>
    /// 展示其他的奖励
    /// </summary>
    /// <param name="data"></param>
    private void OnEndOther(object data)
    {
        GameObject go = (GameObject)data;
        go.transform.Find("AwardItem").gameObject.SetActive(true);
        TweenScale ts = go.transform.Find("AwardItem").GetComponent<TweenScale>();
        ts.ResetToBeginning();
        ts.PlayForward();
    }
    /// <summary>
    /// 点击黑色背景返回
    /// </summary>
    private void f_BlackBGHandle(GameObject go, object value1, object value2)
    {
        if (!mOverFlag)
            return;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChooseAwardPage, UIMessageDef.UI_CLOSE);
        if (chooseAwardParam.mOnReturnCallback != null)
        {
            chooseAwardParam.mOnReturnCallback(chooseAwardParam.mObj);
        }
    }
    protected override void f_Update()
    {
        base.f_Update();
        if (isAutoChangePageFlag)
        {
            isAutoChangePageFlag = false;
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ChooseAwardPage, UIMessageDef.UI_CLOSE);
            if (chooseAwardParam.mOnReturnCallback != null)
            {
                chooseAwardParam.mOnReturnCallback(chooseAwardParam.mObj);
            }
        }
    }
    /// <summary>
    /// 展示获得奖励
    /// </summary>
    /// <param name="result"></param>
    private void f_ShowAward(object result)
    {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage,UIMessageDef.UI_OPEN, string.Format("Chúc mừng bạn đã nhận được {0}", chooseAwardParam.mListData[chooseAwardParam.mChooseIndex].mName));
        string name = chooseAwardParam.mListData[chooseAwardParam.mChooseIndex].mName;
        UITool.f_GetImporentColorName(chooseAwardParam.mListData[chooseAwardParam.mChooseIndex].mImportant, ref name);
mContent.text = string.Format("[FAEF45FF]Chúc mừng bạn đã nhận được [-]{0}", name);
    }
}
/// <summary>
/// 界面参数
/// </summary>
public class ChooseAwardParam
{
    /// <summary>
    /// 随机列表（待抽取）
    /// </summary>
    public List<ResourceCommonDT> mListData = new List<ResourceCommonDT>();
    /// <summary>
    /// 已经抽取的奖励序号(随机列表里的序号)
    /// </summary>
    public int mChooseIndex;
    /// <summary>
    /// 传递参数
    /// </summary>
    public object mObj;
    /// <summary>
    /// 点击返回回调
    /// </summary>
    public ccCallback mOnReturnCallback;
}
