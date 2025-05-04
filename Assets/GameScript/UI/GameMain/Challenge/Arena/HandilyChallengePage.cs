using UnityEngine;
using ccU3DEngine;

//便捷挑战参数
public class HandilyChallengeParam {
    public EM_MoneyType             mCostType;                   //消耗类型
    public int                      mCostCountPerTime;           //每一次挑战消耗的数量
    public string                   mCheckDesc;                  //勾选框旁边的描述文字（如自动消耗精力丹补充精力）
    public object                   mcallbackParam;              //透传参数
    public CalcCanChallengeTimes    mCalcCanChallengeTimes;      //计算挑战次数，返回实际可挑战的次数
    public HandilyChallengeCallback mHandilyChallengeCallback;   //回调
}

/// <summary>
/// 便捷挑战回调委托
/// </summary>
/// <param name="callbackParam">透传参数</param>
/// <param name="count">挑战次数</param>
/// <param name="isAutoCost">是否自动消耗（精力丹）</param>
public delegate void HandilyChallengeCallback(object callbackParam,int count,bool isAutoCost);

/// <summary>
/// 计算挑战次数次数，返回实际可挑战的次数
/// </summary>
/// <param name="count">待判断的次数</param>
/// <param name="isAutoCost">是否自动消耗（精力丹）</param>
/// <returns></returns>
public delegate int CalcCanChallengeTimes(int count, bool isAutoCost);

/// <summary>
/// 便捷挑战界面
/// </summary>
public class HandilyChallengePage : UIFramwork
{
    private HandilyChallengeParam param;//参数
    private int count;//数量 
    private UIInput _InputCount;
    private UILabel _labelPrice;
    private UIToggle _toggleAutoCost;
    private UISprite _MoneyIcon;
    private UILabel _labelAutoCost;
    private UILabel _labelCostEnergy;

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
        _MoneyIcon = f_GetObject("MoneyIcon").GetComponent<UISprite>();
        _labelPrice = f_GetObject("LabelPrice").GetComponent<UILabel>();
        _InputCount = f_GetObject("InputBuyCountBg").GetComponent<UIInput>();
        _toggleAutoCost = f_GetObject("ToggleAutoCost").GetComponent<UIToggle>();
        _toggleAutoCost.value = false;
        _labelAutoCost = f_GetObject("Label_AutoCost").GetComponent<UILabel>();
        _labelCostEnergy = f_GetObject("LabelCostEnergy").GetComponent<UILabel>();
    }

    /// <summary>
    /// 初始化事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlack", OnClose);
        f_RegClickEvent("Sprite_Close", OnClose);
        f_RegClickEvent("BtnCancel", OnClose);
        f_RegClickEvent("BtnConfirm", OnConfirmClick);
        f_RegClickEvent("BtnAddOne", OnBuyAddOneClick);
        f_RegClickEvent("BtnAddTen", OnBuyAddTenClick);
        f_RegClickEvent("BtnReduceOne", OnBuyReduceOneClick);
        f_RegClickEvent("BtnReduceTen", OnBuyReduceTenClick);
        UIEventListener.Get(f_GetObject("InputBuyCountBg")).onSelect = Call_InputCount;
    }

    /// <summary>
    /// 页面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        count = 1;
        param = (HandilyChallengeParam)e;       
        _labelAutoCost.text = param.mCheckDesc;
        _MoneyIcon.spriteName = UITool.f_GetMoneySpriteName(param.mCostType);
        string costName = UITool.f_GetGoodName(EM_ResourceType.Money, (int)param.mCostType);
        _labelCostEnergy.text = string.Format(CommonTools.f_GetTransLanguage(2180), costName);
        UpdateUI();
    }

    /// <summary>
    /// 更新UI
    /// </summary>
    private void UpdateUI()
    {
        if (count > 99) count = 99;
        count = param.mCalcCanChallengeTimes(count, _toggleAutoCost.value);
        _InputCount.value = count.ToString();
        _labelPrice.text = (count * param.mCostCountPerTime).ToString();
    }
    #region 界面事件相关
    /// <summary>
    /// 点击确定按钮
    /// </summary>
    private void OnConfirmClick(GameObject go, object obj1, object obj2)
    {        
        if (param.mHandilyChallengeCallback != null)
        {
            param.mHandilyChallengeCallback(param.mcallbackParam,count, _toggleAutoCost.value);
        }
    }

    /// <summary>
    /// 关闭监听
    /// </summary>
    private void OnClose(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.HandilyChallengePage, UIMessageDef.UI_CLOSE);
    }

    /// <summary>
    /// 点击购买道具减10
    /// </summary>
    private void OnBuyReduceTenClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        count -= 10;
        count = count < 1 ? 1 : count;
        UpdateUI();
    }

    /// <summary>
    /// 点击购买道具减1
    /// </summary>
    private void OnBuyReduceOneClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        count -= 1;
        count = count < 1 ? 1 : count;
        UpdateUI();
    }

    /// <summary>
    /// 点击购买道具加10
    /// </summary>
    private void OnBuyAddTenClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        count += 10;
        UpdateUI();
    }

    /// <summary>
    /// 点击购买道具加1
    /// </summary>
    private void OnBuyAddOneClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        count += 1;
        UpdateUI();
    }

    /// <summary>
    /// 输入回调
    /// </summary>
    /// <param name="go"></param>
    /// <param name="isSelect"></param>
    private void Call_InputCount(GameObject go, bool isSelect)
    {
        if (!isSelect)
        {
            count = ccMath.atoi(_InputCount.value);
            count = count < 1 ? 1 : count;
            UpdateUI();
        }
    }
    #endregion


}