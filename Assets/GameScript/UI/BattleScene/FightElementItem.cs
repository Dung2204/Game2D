using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightElementItem : MonoBehaviour
{
    public UISlider m_MPPanel;
    //public GameObject[] m_aMPObj;
    public UISprite Icon;
    public UILabel Name;
    public GameObject Effect;
    public EM_Factions curFactions;
    private stFightElementInfor tData;
    private int __iMp;
    private int _iMp
    {
        set
        {
            __iMp = value;
            UpdateMP();
        }
    }
    public void f_InIt(stFightElementInfor data)
    {
        f_Reset();
        tData = data;
        SetIcon((int)tData.m_iId);
        string name = "";
        MagicDT tMagicDT = (MagicDT)glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(tData.m_iMagicId);
        if (tMagicDT!= null)
        {
            name = tMagicDT.szName;
        }
        //switch (tData.m_iId)
        //{
        //    case 1:
        //        name = "Nộ Kim";
        //        break;
        //    case 2:
        //        name = "Nộ Mộc";
        //        break;
        //    case 3:
        //        name = "Nộ Thủy";
        //        break;
        //    case 4:
        //        name = "Nộ Hỏa";
        //        break;
        //    case 5:
        //        name = "Nộ Thổ";
        //        break;
        //}
        Name.text = name;
        m_MPPanel.transform.Find("ExBg").Find("ExFg").GetComponent<UISprite>().spriteName = "ele_" + tData.m_iId;
    }

    public bool isElement(int iSide,int element)
    {
        if (tData.m_iId == 0) return false;
        if (iSide == tData.m_iSide && element == tData.m_iId) return true;
        return false;
    }
    public void SetParent(EM_Factions m_EM_Factions)
    {
        curFactions = m_EM_Factions;
        Transform parent = null;
        if (m_EM_Factions == EM_Factions.ePlayer_A)
        {
            if (GameObject.Find("GridElementA") != null)
            {
                parent = GameObject.Find("GridElementA").transform;
            }
        }
        else
        {
            if (GameObject.Find("GridElementB") != null)
            {
                parent = GameObject.Find("GridElementB").transform;
            }
        }

        transform.parent = parent;
        transform.localScale = new Vector3(1, 1, 1);
        if (parent != null)
            parent.gameObject.GetComponent<UIGrid>().enabled = true;
    }
    public void f_Mp(int iDefaultMp)
    {
        _iMp = iDefaultMp;
    }
    private void UpdateMP()
    {
        int iA = __iMp;
        if (iA > 8)
        {
            iA = 8;
        }
        float fHpPerCent = iA / 8f;
        m_MPPanel.value = fHpPerCent;
        //for (int i = 0; i < iA; i++)
        //{
        //    m_aMPObj[i].SetActive(true);
        //}
        //for (int i = iA; i < m_aMPObj.Length; i++)
        //{
        //    m_aMPObj[i].SetActive(false);
        //}
    }

    public void SetIcon(int element)
    {
        Icon.spriteName = "IconEle_" + element;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayEffect();
        }
    }
   
    public void PlayEffect()
    {
        Effect.SetActive(false);
        Effect.SetActive(true);
    }

    public void f_Reset()
    {
        Icon.spriteName = "";
        Name.text = "";
        //for (int i = 0; i < m_aMPObj.Length; i++)
        //{
        //    m_aMPObj[i].SetActive(false);
        //}
        tData = new stFightElementInfor();
        m_MPPanel.value = 0;
    }
    public void f_Destory()
    {
        transform.gameObject.SetActive(false);
    }
   
}
