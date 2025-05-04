using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCashSkill : UIFramwork
{
    public UISlider m_HPPanel;
	public UISlider m_HPPanel_B;
    public UILabel m_MPText;
    public GameObject[] m_aMPObj;
	public UILabel m_MPText_B;
    public GameObject[] m_aMPObj_B;
    public UI2DSprite Icon;
    public UISprite Border;
	public UI2DSprite Icon_B;
    public UISprite Border_B;
	public GameObject TeamA;
	public GameObject TeamB;

    public GameObject Effect;
	public GameObject Effect_B;
    private int __iMp;
    private int _iMp
    {
        set
        {
            __iMp = value;
            UpdateMP();
        }
    }
    public Transform _ListIconBuff;
	public Transform _ListIconBuff_B;
    public BuffIcon BuffIconItem;
    public BuffIcon BuffIconItem_B;
	public EM_Factions curFactions;
    private List<int> m_ListBuffId = new List<int>();
    public void SetParent(EM_Factions m_EM_Factions , int iStatelId1, int iImportant)
    {
        //string Path = "UI Root/BattleMain/BattlePopW/BattlePopH/StatusView/Panel/Anchor-Left/";
        string Path = "UI Root/BattleMain/BattlePopW/BattlePopH/StatusView/";
        curFactions = m_EM_Factions;
        Transform parent = null;
        if (m_EM_Factions == EM_Factions.ePlayer_A)
        {
			if(GameObject.Find(Path+ "ScrollA/ScrollView/GridCardCashSkillePlayer_APanel") != null)
			{
				parent = GameObject.Find(Path+ "ScrollA/ScrollView/GridCardCashSkillePlayer_APanel").transform;
				//TeamA.SetActive(true);
				//TeamB.SetActive(false);
			}
        }
        else
        {
			if(GameObject.Find(Path + "ScrollB/ScrollView/GridCardCashSkillePlayer_BPanel") != null)
			{
				parent = GameObject.Find(Path + "ScrollB/ScrollView/GridCardCashSkillePlayer_BPanel").transform;
                //TeamA.SetActive(false);
                //TeamB.SetActive(true);
            }
        }
        TeamA.SetActive(true);
        TeamB.SetActive(false);
        transform.parent = parent;
        transform.localScale = new Vector3(1, 1, 1);
		if(parent != null)
        {
            parent.gameObject.GetComponent<UIGrid>().enabled = true;
            parent.gameObject.GetComponent<UIGrid>().Reposition();
        }
			
        f_Reset();
        SetIcon(iStatelId1, iImportant);
        f_RegClickEvent(gameObject, GOClickHandle);
        //ccUIEventListener.Get(gameObject).f_RegClick(GOClickHandle, null, null);
    }
    public void f_Reset()
    {
        m_HPPanel.value = 0;
		m_HPPanel_B.value = 0;
        m_MPText.text = "";
		m_MPText_B.text = "";
        m_ListBuffId.Clear();
    }

    public void f_InitHp(float fHpPerCent)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        m_HPPanel.value = fHpPerCent;
		m_HPPanel_B.value = fHpPerCent;
    }

    public void f_Mp(int iDefaultMp, int iChangMp, EM_BattleMpType tEM_BattleMpType)
    {
        _iMp = iDefaultMp;
        //m_MPText.gameObject.SetActive(iDefaultMp > 4);
        //m_MPText.text = "X" + iDefaultMp;
		//m_MPText_B.gameObject.SetActive(iDefaultMp > 4);
        //m_MPText_B.text = "X" + iDefaultMp;
    }

    private void UpdateMP()
    {
        int iA = __iMp;
        if (iA > 5)
        {
            iA = 5;
        }
        for (int i = 0; i < iA; i++)
        {
            m_aMPObj[i].SetActive(true);
        }
        for (int i = iA; i < m_aMPObj.Length; i++)
        {
            m_aMPObj[i].SetActive(false);
        }
		//for (int i = 0; i < iA; i++)
  //      {
  //          m_aMPObj_B[i].SetActive(true);
  //      }
  //      for (int i = iA; i < m_aMPObj_B.Length; i++)
  //      {
  //          m_aMPObj_B[i].SetActive(false);
  //      }
    }

    private void PlayEffect()
    {
        Effect.SetActive(false);
        Effect.SetActive(true);
        //Effect_B.SetActive(false);
        //      Effect_B.SetActive(true);
        
    }

    public void GOClickHandle(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BuffDetailPage, UIMessageDef.UI_OPEN, m_ListBuffId);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayEffect();
        }
    }

    public void f_PlaySkill(int iMagicIndex)
    {
        if (iMagicIndex == 0)
        {
           
        }
        else if (iMagicIndex == 1)
        {
            PlayEffect();
        }
        else if (iMagicIndex == 2)
        {
            PlayEffect();
        }
        else if (iMagicIndex == 3)
        {
            PlayEffect();
        }
        else
        {
MessageBox.ASSERT("Skill code does not exist" + iMagicIndex);
        }
    }

    public void SetIcon(int iStatelId1 = 1001, int iImportant = 0)
    {
        RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(iStatelId1);
		//Tạm ẩn
        Icon.sprite2D = UITool.f_GetCardIcon(roleModle.iIcon, "L1_");
        Border.spriteName = UITool.f_GetImporentCase(iImportant);
        //Icon_B.sprite2D = UITool.f_GetCardIcon(roleModle.iIcon, "L1_"); 
        // Border_B.spriteName = UITool.f_GetImporentCase(iImportant);
    }

    private List<GameObject> listBuffIcon = new List<GameObject>();
    public void f_UpdateListBuffIcon(List<stBeAttackInfor> list_stBeAttackInfors)
    {
		f_ResetListBuffIcon();
		for (int i = 0; i < list_stBeAttackInfors.Count; i++)
		{
			GameObject go = f_GetBuffIconGO();
			if (go == null)
			{
                go = Instantiate(BuffIconItem.gameObject);
    //            if (curFactions == EM_Factions.ePlayer_A)
				//{
				//	go = Instantiate(BuffIconItem.gameObject);
				//}
				//else
				//{
				//	go = Instantiate(BuffIconItem_B.gameObject);
				//}
				go.SetActive(true);
				go.GetComponent<BuffIcon>().SetUI(list_stBeAttackInfors[i].m_iBuf1, list_stBeAttackInfors[i].m_iAnger);
                go.transform.parent = _ListIconBuff;
    //            if (curFactions == EM_Factions.ePlayer_A)
				//{
				//	go.transform.parent = _ListIconBuff;
				//}
				//else
				//{
				//	go.transform.parent = _ListIconBuff_B;
				//}
				listBuffIcon.Add(go);
				go.transform.localScale = new Vector3(1, 1, 1);
			}
			else
			{
				go.SetActive(true);
				go.GetComponent<BuffIcon>().SetUI(list_stBeAttackInfors[i].m_iBuf1, list_stBeAttackInfors[i].m_iAnger);
			}
            m_ListBuffId.Add(list_stBeAttackInfors[i].m_iBuf1);
		}
        _ListIconBuff.gameObject.GetComponent<UIGrid>().enabled = true;
  //      if (curFactions == EM_Factions.ePlayer_A)
		//{
		//	_ListIconBuff.gameObject.GetComponent<UIGrid>().enabled = true;
		//}
		//else
		//{
		//	_ListIconBuff_B.gameObject.GetComponent<UIGrid>().enabled = true;
		//}
    }

    private void f_ResetListBuffIcon()
    {
        for (int i = 0; i < listBuffIcon.Count; i++)
        {
            listBuffIcon[i].SetActive(false);
        }
        m_ListBuffId.Clear();
    }

    private GameObject f_GetBuffIconGO()
    {
        GameObject go = null;
        go = listBuffIcon.Find(GO => GO.activeSelf == false);
        return go;
    }
}
