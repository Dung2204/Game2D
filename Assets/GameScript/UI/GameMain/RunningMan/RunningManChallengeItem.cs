using UnityEngine;
using System.Collections;

public class RunningManChallengeItem : MonoBehaviour 
{
    public UILabel mMoneyLabel;
    public UILabel mPrestigeLabel;
    public UILabel mPassConditon;

    public GameObject mBtnChallenge;

    public void f_UpdateByInfo(int money,int prestige,string passConditon)
    {
        mMoneyLabel.text = money.ToString();
        mPrestigeLabel.text = prestige.ToString();
        mPassConditon.text = passConditon;
    }
}
