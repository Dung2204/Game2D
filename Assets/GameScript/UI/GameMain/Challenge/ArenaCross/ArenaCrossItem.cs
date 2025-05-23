﻿using UnityEngine;
using ccU3DEngine;
using System.Collections;

public class ArenaCrossItem : MonoBehaviour
{
    public static ArenaCrossItem f_Create(GameObject parent, GameObject item)
    {
        ArenaCrossItem result = item.GetComponent<ArenaCrossItem>();
        if (result == null)
            MessageBox.ASSERT("ArenaItem.f_Create trong Item phải chứa ArenaItemNew");
        else
            result.f_Init(parent);
        return result;
    }

    [HideInInspector]
    public GameObject mParent;
    public GameObject mItem;
    public UILabel mRankLabel;
    public UILabel mNameLabel;
    public UILabel mPowerLabel;
    public UILabel mLevelLabel;
    public GameObject mSweepBtn;
    public GameObject mChallengeBtn;
    public GameObject mSelfTip;
    public GameObject mNormalRank;
    public UISprite mSpriteRankBg;
    public UISprite mSpriteTopThree;
    public UISprite mSpriteFrame;
    public UI2DSprite mSpriteIcon;
    private GameObject mRole;

    private int mCurSex = -99;
    private int mSelfIdx = 0;
    string RoleIConAdress = "UI/TextureRemove/Arena/";
    private const int NewGuideId = 2004; //竞技场新手引导id
    public ArenaPoolDT m_ArenaPoolDT;

    public void f_Init(GameObject parent)
    {
        mParent = parent;
        mItem.SetActive(false);       
    }
    public void f_UpdateByInfo(int idx, CMsg_ArenaCrossInfo info)
    {
        mItem.SetActive(info.uMainCard != 0);
        mItem.transform.name = idx.ToString();
        mChallengeBtn.transform.name = "RoleParent" + idx;
        mSelfIdx = idx;
        if (info.uMainCard == 0)
           return;

        int FashId = info.userView.uSex;
        //MessageBox.ASSERT("" + FashId);
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
        if (mCurSex != info.userView.uSex)
        {
            //mCurSex = info.m_PlayerInfo.m_iSex;
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
        mNormalRank.SetActive(info.uRank > 3);
        mSpriteTopThree.gameObject.SetActive(info.uRank <= 3);
        if (info.uRank > 3)
        {
            mRankLabel.text = info.uRank.ToString();
            //mSpriteRankBg.spriteName = info.m_iRank > 10 ? "Border_pam2" : "Border_pam1";
        }
        else
        {
            mSpriteTopThree.spriteName = "Rank" + info.uRank;
            mSpriteTopThree.MakePixelPerfect();
        }

        //处理我自己
        bool isSelf = info.userId == Data_Pool.m_UserData.m_iUserId;
        //mSpriteRankBg.spriteName = isSelf ? "Border_pam2" : "Border_pam1";
        mSpriteRankBg.gameObject.SetActive(info.uRank != 1);
        //mSpriteRankBg.MakePixelPerfect();
        //mSpriteFrame.spriteName = isSelf ? "Border_pam2_1" : "Border_pam1_1";
        mSelfTip.SetActive(isSelf);

        //设置战力和名字
        if (isSelf && Data_Pool.m_GuidancePool.IGuidanceID == NewGuideId)
        {
            //如果是新手引导，，则把我名字随机改名，战力上下浮动，伪装成一个玩家
            string randName = "ggdd";
            if (null != glo_Main.GetInstance() && null != glo_Main.GetInstance().m_SC_Pool && null != glo_Main.GetInstance().m_SC_Pool.m_RandNameSC)
            {
                RandNameSC randNameSc = glo_Main.GetInstance().m_SC_Pool.m_RandNameSC;
                int count = randNameSc.f_GetAll().Count;
                int surnameId = Random.Range(0, 694);
                RandNameDT surnameDt = (RandNameDT)randNameSc.f_GetSC(surnameId);
                randName = surnameDt.szName1;
                int tSex = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard) % 2;
                int nameId = Random.Range(0, count);
                RandNameDT nameDt = (RandNameDT)randNameSc.f_GetSC(nameId);
                if (tSex == 0)
                {
                    randName += nameDt.szNameM;
                }
                else
                {
                    randName += nameDt.szNameW;
                }
            }
            mNameLabel.text = randName;
            PlayerPrefs.SetString(ArenaPageNew.ArenaFakePlayerNameKey, randName);


            //战力比自己低1-1000
            int cutPower = Random.Range(1, 1001);
            int fakePower = info.userExtra.iBattlePower - cutPower;
            PlayerPrefs.SetInt(ArenaPageNew.ArenaFakePlayerPowerKey, fakePower);
            mPowerLabel.text = fakePower.ToString();

            mSelfTip.SetActive(false);
        }
        else
        {
            ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(info.userExtra.offlineTime);
            mNameLabel.text = "(" + serverInfo.szName+")"+info.userView.m_szName;
            mPowerLabel.text = info.userExtra.iBattlePower.ToString();
        }

        mSweepBtn.SetActive(info.uRank > Data_Pool.m_CrossArenaPool.m_iRank);
        StartCoroutine(loadModel(info.uMainCard));

    }

    IEnumerator loadModel(int uMainCard)
    {
        yield return null;
        UITool.f_CreateRoleByCardId(uMainCard, ref mRole, mChallengeBtn.transform, 3, 50);
    }
}
