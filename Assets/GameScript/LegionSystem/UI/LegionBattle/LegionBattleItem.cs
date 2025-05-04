using UnityEngine;
using System.Collections;

public class LegionBattleItem : MonoBehaviour
{
    public UILabel mGateName;
    public UILabel mMemberNum;
    public UILabel mStarNum;

    public GameObject mClickItem;
    public void f_UpdateByInfo(EM_LegionBattleState state, long legionId, bool isBattling,LegionBattleGateNode gateInfo)
    {
        mGateName.text = gateInfo.m_szGateName;
        mMemberNum.gameObject.SetActive(isBattling && legionId != 0 && state != EM_LegionBattleState.eLegionBattle_Init && state != EM_LegionBattleState.eLegionBattle_Matching);
        mStarNum.gameObject.SetActive(isBattling && legionId != 0 && state != EM_LegionBattleState.eLegionBattle_Init && state != EM_LegionBattleState.eLegionBattle_Matching);
        mMemberNum.text = string.Format(CommonTools.f_GetTransLanguage(509), gateInfo.m_iMemberNum);
        mStarNum.text= string.Format(CommonTools.f_GetTransLanguage(510), gateInfo.m_iStarNum);
        mMemberNum.text = isBattling && legionId != 0 && state != EM_LegionBattleState.eLegionBattle_Init && state != EM_LegionBattleState.eLegionBattle_Matching ? string.Format(CommonTools.f_GetTransLanguage(509), gateInfo.m_iMemberNum):string.Empty;
        mStarNum.text = isBattling && legionId != 0 && state != EM_LegionBattleState.eLegionBattle_Init && state != EM_LegionBattleState.eLegionBattle_Matching ? string.Format(CommonTools.f_GetTransLanguage(510), gateInfo.m_iStarNum):string.Empty;
    }
}
