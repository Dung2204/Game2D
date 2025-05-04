using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class ArenaSweepItem : MonoBehaviour
{
    public GameObject mItemObj;

    public GameObject mLoseObj;
    public UILabel mTitle;
    public UILabel mMoneyLabel;
    public UILabel mExpLabel;
    public UILabel mFameLabel;

    public GameObject mChooseAwardParent;
    public GameObject mChooseAwardObj;
    private ResourceCommonItem _chooseAwardItem;
    private ResourceCommonItem mChooseAwardItem
    {
        get
        {
            if (_chooseAwardItem == null)
                _chooseAwardItem = ResourceCommonItem.f_Create(mChooseAwardParent, mChooseAwardObj);
            return _chooseAwardItem;
        }
    }
    
    CMsg_SC_ArenaSweepRetNode mData;
    public void f_UpdateByInfo(int idx,CMsg_SC_ArenaSweepRetNode node,ccUIBase holdUI)
    {
        mItemObj.SetActive(true);
        f_UpdateView(idx,node,holdUI);
    }

    public void f_Disable()
    {
        mItemObj.SetActive(false);
    }

    private void f_UpdateView(int idx,CMsg_SC_ArenaSweepRetNode node,ccUIBase holdUI)
    {
        mData = node;
        if (node.isWin != (byte)EM_ArenaResult.Lose)
        {
            mChooseAwardItem.f_UpdateByInfo(node.award.resourceType, node.award.resourceId, node.award.resourceNum, EM_CommonItemShowType.All, EM_CommonItemClickType.AllTip, holdUI);
        }
        else
        {
            mChooseAwardItem.f_Disable();
        }
mTitle.text = string.Format("Battle {0}", idx + 1);
        mLoseObj.SetActive(node.isWin == (byte)EM_ArenaResult.Lose);
        mChooseAwardParent.SetActive(node.isWin != (byte)EM_ArenaResult.Lose);

        //奖励
        int tLv = StaticValue.m_sLvInfo.m_iAddLv;
        int addExp;
        int tExp = GameFormula.f_VigorCost2Exp(tLv, GameParamConst.ArenaVigorCost,out addExp);
        string strAddExp = addExp > 0 ? "[FFF700FF]（+" + addExp + "）" : "";
        int tMoney = GameFormula.f_EnergyCost2Money(tLv, GameParamConst.ArenaVigorCost);
        StaticValue.m_sLvInfo.f_AddExp(tExp + addExp);
        int tFame = node.isWin == (byte)EM_ArenaResult.Lose ? GameParamConst.ArenaLoseFame : GameParamConst.ArenaWinFame;
        mMoneyLabel.text = tMoney.ToString();
        mExpLabel.text = tExp.ToString() + strAddExp;
        mFameLabel.text = tFame.ToString();
    }
}

