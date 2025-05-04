using UnityEngine;
using System.Collections;

public class LegionBattleGateItem : MonoBehaviour
{
    public UILabel mPlayerName;
    public UILabel mPlayerPower;
    public Transform mRoleParent;
    public UISprite[] mStar;
    public GameObject mBreakTip;

    public GameObject mClickItem;

    private GameObject role = null;

    public void f_ResetItem()
    {
        gameObject.SetActive(false);
        f_ClearRole();
    }

    public void f_UpdateByInfo(LegionBattleGateDefenderNode defenderInfo)
    {
        if (defenderInfo.m_PlayerInfo == null)
        {
            gameObject.SetActive(false);
            f_ClearRole();
            return;
        }
        gameObject.SetActive(true);
        mPlayerName.text = defenderInfo.m_PlayerInfo.m_szName;
        mPlayerPower.text = string.Format("{0}", UITool.f_CountToChineseStr(defenderInfo.m_PlayerInfo.m_iBattlePower));
        UITool.f_CreateRoleByCardId(defenderInfo.m_PlayerInfo.m_CardId, ref role, mRoleParent, 7);
        UITool.f_UpdateStarNum(mStar, defenderInfo.m_iStarNum);
        mBreakTip.SetActive(defenderInfo.m_iStarNum >= defenderInfo.m_iStarMaxNum);
    }

    private void f_ClearRole()
    {
        if (role != null)
        {
            UITool.f_DestoryStatelObject(role);
            role = null;
        }
    }
}
