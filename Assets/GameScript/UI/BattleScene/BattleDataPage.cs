using ccU3DEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDataPage : UIFramwork
{
    private int[] mBattleData;
    private stRoleInfor[] mLineUp;
    private List<Transform> Items;
    private int mSelfMaxDPS;
    private int mEnemyMaxDps;

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mSelfMaxDPS = 0;
        mEnemyMaxDps = 0;
        mBattleData = Data_Pool.m_BattleDataPool.f_GetDPS();
        mLineUp = Data_Pool.m_BattleDataPool.f_GetRoleList();
        if (Items == null)
        {
            Items = new List<Transform>();
            for (int i = 0; i < f_GetObject("SelfLineUp").transform.childCount; i++)
            {
                Items.Add(f_GetObject("SelfLineUp").transform.GetChild(i));
            }
            for (int i = 0; i < f_GetObject("EnemyLineUp").transform.childCount; i++)
            {
                Items.Add(f_GetObject("EnemyLineUp").transform.GetChild(i));
            }
        }
        if (mBattleData == null)
        {
Debug.Log("Wrong battle data");
        }
        f_RegClickEvent("BlackBg", f_Close);

        f_CountAllDPS();
        f_UpdateUI();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);

    }

    private void f_CountAllDPS()
    {
        for (int i = 0; i < mBattleData.Length; i++)
        {
            if (i < 6)
                mSelfMaxDPS += mBattleData[i];
            else
                mEnemyMaxDps += mBattleData[i];
        }
    }

    private void f_UpdateUI()
    {
        int itemindex = 0;
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < 40; i++)
        {
            if (i == 20)
            {
                itemindex = 6;
            }
            if (mLineUp[i].m_iId != 0)
            {
                Items[itemindex].gameObject.SetActive(true);
                f_UpdateItem(Items[itemindex], mLineUp[i].m_iTempId, mBattleData[itemindex]);
                itemindex++;
            }
        }
    }
    private void f_UpdateItem(Transform item, int id, int dps)
    {
        UISprite Case = item.Find("Case").GetComponent<UISprite>();
        UI2DSprite Icon = item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel num = item.Find("Num").GetComponent<UILabel>();
        UISlider Bar = item.Find("Bar").GetComponent<UISlider>();
        CardDT Card = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(id) as CardDT;
        int Important = 0;
        Sprite tsprite = null;
        int MaxDps = 0;
        if (Card == null)
        {
            MonsterDT tMonster = glo_Main.GetInstance().m_SC_Pool.m_MonsterSC.f_GetSC(id) as MonsterDT;
            Important = tMonster.iImportant;
            tsprite = UITool.f_GetIconSpriteByModelId(tMonster.iStatelId1);
            MaxDps = mEnemyMaxDps ==0 ?1:mEnemyMaxDps;
        }
        else
        {
            MaxDps = mSelfMaxDPS == 0 ? 1 : mSelfMaxDPS;
            Important = Card.iImportant;
            tsprite = UITool.f_GetIconSpriteByCardId(Card.iId);
        }
        string strCare = UITool.f_GetImporentCase(Important);
        Case.spriteName = strCare;
        Icon.sprite2D = tsprite;
        num.text = string.Format("{0}({1}%)", dps, Mathf.Round((float)dps / (float)MaxDps * 10000) / 100);
        Bar.value = (float)dps / (float)MaxDps;
    }

    private void f_Close(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleDataPag, UIMessageDef.UI_CLOSE);
    }
}
