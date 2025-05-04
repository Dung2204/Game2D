using UnityEngine;
using System.Collections;

public class ArenaItem : MonoBehaviour
{
    public static ArenaItem f_Create(GameObject parent, GameObject item)
    {
        GameObject go = NGUITools.AddChild(parent, item);
        NGUITools.MarkParentAsChanged(go);
        ArenaItem result = go.GetComponent<ArenaItem>();
        if(result == null)
MessageBox.ASSERT("ArenaItem.f_Create in Item must have ArenaItem");
        else
            result.f_Init();
        return result;
    }

    public void f_Init()
    {
        mItem.SetActive(false);
    }

    public GameObject mItem;
    public UILabel mRankLabel;
    public UILabel mNameLabel;
    public UILabel mPowerLabel;
    public GameObject mSweepBtn;
    public GameObject mChallengeBtn;
    public GameObject mSelectItem;
    public UITexture mRoleIcon;
    public BoxCollider mRoleIconBoxCollider;
    public UISprite mArenaBg;
    public UIPlayTween mPlayTween;
    public GameObject mSelfTip;

    private ArenaPoolDT mInfo;
    private int mCurSex = -99;

    private int mSelfIdx = 0;
    private int mSelectIdx = 0;
    string RoleIConAdress = "UI/TextureRemove/Arena/";
    public void f_UpdateByInfo(int selectIdx, int idx, ArenaPoolDT info)
    {
        mInfo = info;
        mItem.SetActive(mInfo != null);
        mItem.transform.name = idx.ToString();
        mSelfIdx = idx;
        mSelectIdx = selectIdx;
        if(mInfo == null)
            return;

        int FashId = info.m_PlayerInfo.m_iSex;
        if (FashId == (int)EM_RoleSex.Man)
        {
            FashId = 10001;
        }
        else
        {
            FashionableDressDT tFashionableDressDT = glo_Main.GetInstance().m_SC_Pool.m_FashionableDressSC.f_GetSC(FashId) as FashionableDressDT;
            if (tFashionableDressDT != null)
            {
                FashId = tFashionableDressDT.iIcon;
            }
        }
        mRoleIcon.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(RoleIConAdress + FashId);
        if(mCurSex != mInfo.m_PlayerInfo.m_iSex)
        {
            //mCurSex = mInfo.m_PlayerInfo.m_iSex;
            //if (mCurSex == (int)EM_RoleSex.Man)
            //{
            //    mRoleIcon.spriteName = "Icon_Man";
            //    mRoleIcon.MakePixelPerfect();
            //}
            //else if (mCurSex == (int)EM_RoleSex.Woman)
            //{
            //    mRoleIcon.spriteName = "Icon_Women";
            //    mRoleIcon.MakePixelPerfect();
            //}
            //mRoleIcon.spriteName
        }
        mRankLabel.text = mInfo.m_iRank.ToString();
        mArenaBg.spriteName = mInfo.m_iRank > 10 ? "Border_ArenaItemBg" : "Border_ArenaItemBg2";
        if(info.iId == Data_Pool.m_UserData.m_iUserId && !Data_Pool.m_ArenaPool.m_IsOnRank)
mRankLabel.text = "Not in the list";
        mNameLabel.text = mInfo.m_PlayerInfo.m_szName;
        mPowerLabel.text = mInfo.m_PlayerInfo.m_iBattlePower.ToString();
        mSweepBtn.SetActive(Data_Pool.m_ArenaPool.m_IsOnRank && mInfo.m_iRank > Data_Pool.m_ArenaPool.m_iRank);
        mChallengeBtn.SetActive(!mSweepBtn.activeSelf && info.iId != Data_Pool.m_UserData.m_iUserId);
        mPlayTween.Play(false);
        mRoleIconBoxCollider.enabled = false;
        mSelfTip.SetActive(info.iId == Data_Pool.m_UserData.m_iUserId);
        if(info.iId == Data_Pool.m_UserData.m_iUserId)
            mArenaBg.spriteName = "Border_SelfArenaItemBg";
    }

    public void f_UpdateSelectIdx(int selectIdx)
    {
        if(!mItem.activeSelf)
            return;
        mSelectIdx = selectIdx;
        mPlayTween.Play(mSelfIdx == mSelectIdx);
        mRoleIconBoxCollider.enabled = mSelfIdx == mSelectIdx;
    }

}
