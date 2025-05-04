using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using Spine.Unity;

public class FightElementPool
{
    private List<FightElementItem> m_aElementList = new List<FightElementItem>();
    public void Init()
    {
    }
	
	public List<FightElementItem> GetAllControl() { return m_aElementList; }

    public FightElementItem f_GetElementControl(int iSide,int element)
    {
        for (int i = 0; i < m_aElementList.Count; i++)
        {
            if (m_aElementList[i].isElement(iSide, element))
            {
                return m_aElementList[i];
            }
        }
        return null;
    }

    public FightElementItem f_CreateFightElement(stFightElementInfor tData)
    {
        if (tData.m_iId == 0) return null;
        return CreateFightElement(tData);
    }

    private FightElementItem CreateFightElement(stFightElementInfor tData)
    {
        FightElementItem tControl = glo_Main.GetInstance().m_ResourceManager.f_CreateFightElementItem();
        tControl.f_InIt(tData);
        tControl.SetParent((EM_Factions)tData.m_iSide);
        tControl.f_Mp(tData.m_iAnger);
        m_aElementList.Add(tControl);
		return tControl;
    }

	public void f_Destory()
    {
        for (int i = 0; i < m_aElementList.Count; i++)
        {
            if (m_aElementList[i] != null)
            {
                continue;
            }
            m_aElementList[i].f_Destory();
			
        }
    }
}
