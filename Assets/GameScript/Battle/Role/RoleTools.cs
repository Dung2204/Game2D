using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoleTools
{


    public static RoleControl f_CreateRole(int iModelId, Vector3 Pos, EM_Factions tEM_Factions, bool needShowRedCard, bool needShowShadow, int notRoleId = 0, int iStatelId1 = 0, int iImportant = 0, int m_iGodEquipSkillId = 0)
    {
       
        RoleControl tRoleControl = glo_Main.GetInstance().m_ResourceManager.f_CreateRoleModel();
        GameObject tSpine = glo_Main.GetInstance().m_ResourceManager.f_CreateRole(iModelId, needShowRedCard, needShowShadow, notRoleId);
        tSpine.layer = 5;
        tRoleControl.f_Create(Pos, tSpine, iStatelId1, iImportant, m_iGodEquipSkillId);
        //if (tEM_Factions == EM_Factions.eEnemy_B)
        //{
        //    tRoleControl.f_Face2Left();
        //}
        //else
        //{
        //    tRoleControl.f_Face2Right();
        //}
        return tRoleControl;
    }

    public static bool f_CheckIsDie(RoleControl tRoleControl)
    {
        if (tRoleControl == null)
        {
            return true;
        }
        return tRoleControl.f_CheckIsDie();
    }

    public static int f_GetFormationPosDepth(EM_CloseArrayPos tEM_FormationPos)
    {
        if(tEM_FormationPos == EM_CloseArrayPos.eCloseArray_PosOne || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_PosSix || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos11 || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos16)
        {
            return 0;
        }else if (tEM_FormationPos == EM_CloseArrayPos.eCloseArray_PosTwo || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos7 || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos12 || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos17)
        {
            return 1;
        }
        else if (tEM_FormationPos == EM_CloseArrayPos.eCloseArray_PosThree || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos8 || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos13 || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos18)
        {
            return 2;
        }
        else if (tEM_FormationPos == EM_CloseArrayPos.eCloseArray_PosFour || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos9 || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos14 || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos19)
        {
            return 3;
        }
        else if (tEM_FormationPos == EM_CloseArrayPos.eCloseArray_PosFive || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos10 || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos15 || tEM_FormationPos == EM_CloseArrayPos.eCloseArray_Pos20)
        {
            return 4;
        }
        
        //if (tEM_FormationPos == EM_FormationPos.eFormationPos_Main || tEM_FormationPos == EM_FormationPos.eFormationPos_Assist3)
        //{
        //    return 0;
        //}
        //else if (tEM_FormationPos == EM_FormationPos.eFormationPos_Assist1 || tEM_FormationPos == EM_FormationPos.eFormationPos_Assist4)
        //{
        //    return 1;
        //}
        //else if (tEM_FormationPos == EM_FormationPos.eFormationPos_Assist2 || tEM_FormationPos == EM_FormationPos.eFormationPos_Assist5)
        //{
        //    return 2;
        //}

        return 0;

    }

    /// <summary>
    /// 根据装备ID来取对应的套装DT
    /// </summary>
    public static SetEquipDT[] f_GetSetEquip(EquipPoolDT tEquipDT1 = null, EquipPoolDT tEquipDT2 = null, EquipPoolDT tEquipDT3 = null, EquipPoolDT tEquipDT4 = null)
    {
        int id1 = 0, id2 = 0, id3 = 0, id4 = 0;
        if (tEquipDT1 != null && tEquipDT1.m_EquipDT != null)
        {
            id1 = tEquipDT1.m_EquipDT.iId;
        }
        if (tEquipDT2 != null && tEquipDT2.m_EquipDT != null)
        {
            id2 = tEquipDT2.m_EquipDT.iId;
        }
        if (tEquipDT3 != null && tEquipDT3.m_EquipDT != null)
        {
            id3 = tEquipDT3.m_EquipDT.iId;
        }
        if (tEquipDT4 != null && tEquipDT4.m_EquipDT != null)
        {
            id4 = tEquipDT4.m_EquipDT.iId;
        }
        SetEquipDT tsetEquipDT1 = f_GetSetEquipDT(id1);
        SetEquipDT tsetEquipDT2 = f_GetSetEquipDT(id2);
        SetEquipDT tsetEquipDT3 = f_GetSetEquipDT(id3);
        SetEquipDT tsetEquipDT4 = f_GetSetEquipDT(id4);
        SetEquipDT[] tsetEquip = { tsetEquipDT1, tsetEquipDT2, tsetEquipDT3, tsetEquipDT4 };
        List<SetEquipDT> ListEquip = new List<SetEquipDT>();
        for (int i = 0; i < tsetEquip.Length; i++)
        {
            if (tsetEquip[i] != null)
            {
                if (!ListEquip.Contains(tsetEquip[i]))
                    ListEquip.Add(tsetEquip[i]);
            }
        }

        return ListEquip.ToArray();
    }

    public static SetEquipDT f_GetSetEquipDT(int id)
    {
        if (id == 0)
            return null;
        if (glo_Main.GetInstance().m_SC_Pool.m_SetEquipSC.M_SetEquipSC.ContainsKey(id))
            return glo_Main.GetInstance().m_SC_Pool.m_SetEquipSC.M_SetEquipSC[id];
        else
            return null;
    }

    public static bool f_IsHaveSetEquip(SetEquipDT tequipdt, int id)
    {
        if (tequipdt.iEquipId1 == id)
            return true;
        else if (tequipdt.iEquipId2 == id)
            return true;
        else if (tequipdt.iEquipId3 == id)
            return true;
        else if (tequipdt.iEquipId4 == id)
            return true;
        return false;
    }

    /// <summary>
    /// 检测tRoleControl1方向 bool右 false左
    /// </summary>
    /// <returns></returns>
    public static bool f_CheckFace(RoleControl tRoleControl1, RoleControl RoleControl2)
    {
        if (tRoleControl1.transform.position.x > RoleControl2.transform.position.x)
        {
            return true;
        }

        return false;

    }


}