using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandilyChallengeResultPage : UIFramwork
{
    private UILabel    _labelSweetCount;
    private UILabel    _labelSuccessCount;
    private UILabel    _labelEnergyCount;
    private UILabel    _labelCoinCount;
    private UILabel    _labelExpCount;
    private UILabel    _labelPrestigeCount;
    private GameObject _objRewardPrefab;
    private UIGrid     _rewardGrid;
    private UIScrollView _rewardScrollView;
    private GameObject _commonRewardObj;
    private List<HandilyChallengeReward> _handilyChallengeRewardList;

    public class HandilyChallengeReward
    {
        public GameObject _root;
        public ResourceCommonItem commonRewardItem;
        public GameObject _frameFlag;

        public HandilyChallengeReward(GameObject parent,GameObject itemPrefab,GameObject commonRewardObj)
        {
            _root = NGUITools.AddChild(parent, itemPrefab);
            _frameFlag = _root.transform.Find("Label_FrameFlag").gameObject;
            commonRewardItem = ResourceCommonItem.f_Create(_root, commonRewardObj);
            _root.transform.localScale = Vector3.one * 0.8f;
        }
    }

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _labelSweetCount    = f_GetObject("Label_SweetCount").GetComponent<UILabel>();
        _labelSuccessCount  = f_GetObject("Label_SuccessCount").GetComponent<UILabel>();
        _labelEnergyCount   = f_GetObject("Label_EnergyCount").GetComponent<UILabel>();
        _labelCoinCount     = f_GetObject("Label_CoinCount").GetComponent<UILabel>();
        _labelExpCount      = f_GetObject("Label_ExpCount").GetComponent<UILabel>();
        _labelPrestigeCount = f_GetObject("Label_PrestigeCount").GetComponent<UILabel>();
        _objRewardPrefab    = f_GetObject("Reward");
        _commonRewardObj    = f_GetObject("ResourceCommonItem");
        _rewardGrid         = f_GetObject("Grid").GetComponent<UIGrid>();
        _rewardScrollView = f_GetObject("PieceReward").GetComponent<UIScrollView>();
        _handilyChallengeRewardList = new List<HandilyChallengeReward>();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlack", OnClose);
        f_RegClickEvent("Sprite_Close", OnClose);
        f_RegClickEvent("BtnConfirm", OnClose);
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e">EquipSythesis类似</param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        HandilyChallengeResultParam handilyChallengeResultParam = (HandilyChallengeResultParam)e;
        _labelSweetCount.text = handilyChallengeResultParam.ChallengeTimes.ToString();
        _labelSuccessCount.text = handilyChallengeResultParam.ChallengeTimes.ToString();
        _labelEnergyCount.text = handilyChallengeResultParam.useItemNum.ToString();

        //通过次数计算获得的银币、经验以及声望
        int tLv = StaticValue.m_sLvInfo.m_iAddLv;
        int moneyNum = GameFormula.f_VigorCost2Money(tLv, GameParamConst.ArenaVigorCost);
        _labelCoinCount.text = (moneyNum * handilyChallengeResultParam.ChallengeTimes).ToString();
        int addExp;
        int expNum = GameFormula.f_VigorCost2Exp(tLv, GameParamConst.ArenaVigorCost, out addExp);
        string strAddExp = addExp > 0 ? " [8dd91b]+" + (addExp * handilyChallengeResultParam.ChallengeTimes) + CommonTools.f_GetTransLanguage(2186) : "";
        _labelExpCount.text = "[cbc4b1]" + expNum * handilyChallengeResultParam.ChallengeTimes + strAddExp;
        StaticValue.m_sLvInfo.f_AddExp((expNum + addExp) * handilyChallengeResultParam.ChallengeTimes);
        int tFame = GameParamConst.ArenaWinFame * handilyChallengeResultParam.ChallengeTimes;
        _labelPrestigeCount.text = tFame.ToString();

        //不够奖励预设则生成奖励
        for (int i = _handilyChallengeRewardList.Count; i < handilyChallengeResultParam.RewardList.Count; i++) {
            HandilyChallengeReward handilyChallengeReward = new HandilyChallengeReward(_rewardGrid.gameObject,_objRewardPrefab,_commonRewardObj);
            _handilyChallengeRewardList.Add(handilyChallengeReward);
        }

        //设置奖励
        _rewardScrollView.enabled = handilyChallengeResultParam.RewardList.Count > 6;
        for (int i = 0; i < handilyChallengeResultParam.RewardList.Count; i++) {
            HandilyChallengeReward handilyChallengeReward = _handilyChallengeRewardList[i];
            handilyChallengeReward._root.SetActive(true);
            SC_Award reward = handilyChallengeResultParam.RewardList[i];
            handilyChallengeReward.commonRewardItem.f_UpdateByInfo(reward.resourceType, reward.resourceId, 
                reward.resourceNum, EM_CommonItemShowType.Icon, EM_CommonItemClickType.AllTip, this);
            handilyChallengeReward.commonRewardItem.mNum.text = UITool.f_CountToChineseStr(reward.resourceNum);
            handilyChallengeReward._frameFlag.SetActive(UITool.f_JudgeIsFragement((EM_ResourceType)reward.resourceType));
        }
       
        //隐藏多余奖励
        for (int i = handilyChallengeResultParam.RewardList.Count; i < _handilyChallengeRewardList.Count; i++) {
            _handilyChallengeRewardList[i]._root.SetActive(false);
        }

        //重置位置
        _rewardGrid.Reposition();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    /// <summary>
    /// 关闭监听
    /// </summary>
    private void OnClose(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.HandilyChallengeResultPage, UIMessageDef.UI_CLOSE);
    }
}

public class HandilyChallengeResultParam {
    public int useItemNum;             //自动消耗精力丹个数
    public int ChallengeTimes;         //挑战次数
    public List<SC_Award> RewardList;  //奖励
}