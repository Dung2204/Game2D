using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class RolePropertyTools
{


    /// <summary>
    /// 卡牌天命增加的属性
    /// </summary>
    /// <param name="SkyDestintLv"></param>
    /// <returns></returns>
    public static RoleProperty f_GetSkyDestiny(int SkyDestintLv)
    {
        MessageBox.ASSERT("TsuLog SkyDestiny " + SkyDestintLv);
        RoleProperty _AddProperty = new RoleProperty();
        int Move = (int)EM_RoleProperty.AddAtk;
        for (; Move <= 8; Move++)
            _AddProperty.f_AddProperty(Move, SkyDestintLv * 600);
        return _AddProperty;
    }
    /// <summary>
    /// 获取天命增加的属性
    /// </summary>
    /// <param name="iDestinyProgress">天命系统进度</param>
    /// <returns></returns>
    public static RoleProperty f_GetDestinyAddProperty(int iDestinyProgress)
    {
        RoleProperty _AddProperty = new RoleProperty();
        for (int i = 1; i <= iDestinyProgress; i++)
        {
            BattleFormationsDT data = glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(i) as BattleFormationsDT;
            if (data.iAttrID != 0)
            {
                _AddProperty.f_AddProperty((int)data.iAttrID, data.iAttrValue);
            }
        }
        return _AddProperty;
    }
    /// <summary>
    /// 加上计算单件装备的增加属性
    /// </summary>
    /// <param name="tEquipPoolDT"></param>
    /// <returns></returns>
    public static RoleProperty f_DispEquip(ref RoleProperty _RoleProperty, EquipPoolDT tEquipPoolDT)
    {
        if (tEquipPoolDT != null && tEquipPoolDT.m_iTempleteId > 0)
        {
            //强化 (基础值 + 强化等级*(强化属性Id 强化初始值))
            _RoleProperty.f_AddProperty(tEquipPoolDT.m_EquipDT.iIntenProId, CalculatePropertyStartLv1(tEquipPoolDT.m_EquipDT.iStartPro, tEquipPoolDT.m_EquipDT.iAddPro, tEquipPoolDT.m_lvIntensify));
            //精炼 (精炼等级*(精炼属性Id1 每级属性值1)   精炼等级*(精炼属性Id2 每级属性值2)) iRefinProId1	iRefinPro1	iRefinProId2	iRefinPro2
            _RoleProperty.f_AddProperty(tEquipPoolDT.m_EquipDT.iRefinProId1, CalculatePropertyStartLv0(0, tEquipPoolDT.m_EquipDT.iRefinPro1, tEquipPoolDT.m_lvRefine));
            _RoleProperty.f_AddProperty(tEquipPoolDT.m_EquipDT.iRefinProId2, CalculatePropertyStartLv0(0, tEquipPoolDT.m_EquipDT.iRefinPro2, tEquipPoolDT.m_lvRefine));


            //装备升星
            EquipUpStarDT tUpstar;
            if (tEquipPoolDT.m_sexpStars > 0)
            {
                tUpstar = glo_Main.GetInstance().m_SC_Pool.m_EquipUpStarSC.f_GetSC(tEquipPoolDT.m_EquipDT.iId * 100 + tEquipPoolDT.m_sstars + 1) as EquipUpStarDT;
                if (tUpstar != null)
                    _RoleProperty.f_AddProperty((int)tUpstar.iProId, Convert.ToInt32(((float)tEquipPoolDT.m_sexpStars / (float)tUpstar.iUpExp) * tUpstar.iAddPro));
            }
            for (int j = 1; j <= tEquipPoolDT.m_sstars; j++)
            {
                tUpstar = glo_Main.GetInstance().m_SC_Pool.m_EquipUpStarSC.f_GetSC(tEquipPoolDT.m_EquipDT.iId * 100 + j) as EquipUpStarDT;
                if (tUpstar != null)
                    _RoleProperty.f_AddProperty((int)tUpstar.iProId, tUpstar.iAddNum + tUpstar.iAddPro);
            }
        }
        return _RoleProperty;
    }
    /// <summary>
    /// 加上计算单个法宝的增加属性
    /// </summary>
    /// <param name="tEquipPoolDT"></param>
    /// <returns></returns>
    public static RoleProperty f_DispTreasure(ref RoleProperty _RoleProperty, TreasurePoolDT tTreasurePoolDT)
    {
        if (tTreasurePoolDT != null && tTreasurePoolDT.m_iTempleteId > 0)
        {
            //强化 (基础值 + 强化等级*(强化属性Id 强化初始值)) 1 , 2
            _RoleProperty.f_AddProperty(tTreasurePoolDT.m_TreasureDT.iIntenProId1, CalculatePropertyStartLv1(tTreasurePoolDT.m_TreasureDT.iStartPro1, tTreasurePoolDT.m_TreasureDT.iAddPro1, tTreasurePoolDT.m_lvIntensify));
            _RoleProperty.f_AddProperty(tTreasurePoolDT.m_TreasureDT.iIntenProId2, CalculatePropertyStartLv1(tTreasurePoolDT.m_TreasureDT.iStartPro2, tTreasurePoolDT.m_TreasureDT.iAddPro2, tTreasurePoolDT.m_lvIntensify));
            //精炼 (精炼等级*(精炼属性Id1 每级属性值1)   精炼等级*(精炼属性Id2 每级属性值2)) iRefinProId1	iRefinPro1	iRefinProId2	iRefinPro2
            _RoleProperty.f_AddProperty(tTreasurePoolDT.m_TreasureDT.iRefinProId1, CalculatePropertyStartLv0(0, tTreasurePoolDT.m_TreasureDT.iRefinPro1, tTreasurePoolDT.m_lvRefine));
            _RoleProperty.f_AddProperty(tTreasurePoolDT.m_TreasureDT.iRefinProId2, CalculatePropertyStartLv0(0, tTreasurePoolDT.m_TreasureDT.iRefinPro2, tTreasurePoolDT.m_lvRefine));
        }
        return _RoleProperty;
    }

    public static void f_GetBaseEvolevProperty(CardPoolDT tCardPoolDT, ref RoleProperty tRoleProperty)
    {
        //升级、进阶、培养、领悟、缘分。主角多一个突破系统，改变颜色品质			
        //升级、进阶、培养、领悟、缘分	
        //

        //基础
        //--卡牌基础属性
        //基础值 + 升级值 * (等级)
        //攻击
        tRoleProperty.f_AddProperty((int)EM_RoleProperty.Atk, CalculatePropertyStartLv1(tCardPoolDT.m_CardDT.iInitAtk, tCardPoolDT.m_CardDT.iAddAtk, tCardPoolDT.m_iLv));
        //生命 
        tRoleProperty.f_AddHp((long)(CalculatePropertyStartLv1(tCardPoolDT.m_CardDT.iInitHP, tCardPoolDT.m_CardDT.iAddHP, tCardPoolDT.m_iLv)));
        //物防
        tRoleProperty.f_AddProperty((int)EM_RoleProperty.Def, CalculatePropertyStartLv1(tCardPoolDT.m_CardDT.iInitPhyDef, tCardPoolDT.m_CardDT.iAddDef, tCardPoolDT.m_iLv));
        //法防
        tRoleProperty.f_AddProperty((int)EM_RoleProperty.MDef, CalculatePropertyStartLv1(tCardPoolDT.m_CardDT.iInitMagDef, tCardPoolDT.m_CardDT.iAddMagDef, tCardPoolDT.m_iLv));

        //--卡牌进化(进阶)     
        //(进化Id) == 0：基础值 + 升级值 * (等级)
        //(进化ID > 0：基础值 + 升级值 * (等级)
        if (tCardPoolDT.m_iEvolveLv > 0)
        {//进化＞0：(基础值 + 升级值 * (等级 - 1)) * 1.15 ^ 进化次数   结果向下取整
            //攻击        

            tRoleProperty.f_UpdateProperty((int)EM_RoleProperty.Atk, Convert.ToInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.Atk) * Mathf.Pow(1.15f, tCardPoolDT.m_iEvolveLv) - 0.5f));
            //生命  
            tRoleProperty.f_UpdateHp((long)(Convert.ToInt32(tRoleProperty.f_GetHp() * Mathf.Pow(1.15f, tCardPoolDT.m_iEvolveLv) - 0.5f)));
            //物防
            tRoleProperty.f_UpdateProperty((int)EM_RoleProperty.Def, Convert.ToInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.Def) * Mathf.Pow(1.15f, tCardPoolDT.m_iEvolveLv) - 0.5f));
            //法防
            tRoleProperty.f_UpdateProperty((int)EM_RoleProperty.MDef, Convert.ToInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.MDef) * Mathf.Pow(1.15f, tCardPoolDT.m_iEvolveLv) - 0.5f));
        }
    }

    public static void f_GetAwakenProperty(CardPoolDT tCardPoolDT, ref RoleProperty tRoleProperty)
    {
        //--领悟
        if ((tCardPoolDT.m_iLvAwaken != 0 && tCardPoolDT.m_iLvAwaken <= 60))
        {
            AwakenCardDT tAwaken = glo_Main.GetInstance().m_SC_Pool.m_AwakenCardSC.f_GetSC(tCardPoolDT.m_iLvAwaken) as AwakenCardDT;
            tRoleProperty.f_AddProperty(tAwaken.iAddProId1, tAwaken.iAddPro1);
            tRoleProperty.f_AddProperty(tAwaken.iAddProId2, tAwaken.iAddPro2);
            tRoleProperty.f_AddProperty(tAwaken.iAddProId3, tAwaken.iAddPro3);
            tRoleProperty.f_AddProperty(tAwaken.iAddProId4, tAwaken.iAddPro4);
            tRoleProperty.f_AddProperty(tAwaken.iStarProId1, tAwaken.iStarPro1);
            tRoleProperty.f_AddProperty(tAwaken.iStarProId2, tAwaken.iStarPro2);
            tRoleProperty.f_AddProperty(tAwaken.iStarProId3, tAwaken.iStarPro3);
            tRoleProperty.f_AddProperty(tAwaken.iStarProId4, tAwaken.iStarPro4);

        }
    }

    /// <summary>
    /// 添加神器属性
    /// </summary>
    /// <param name="tCardPoolDT"></param>
    /// <param name="artifactProperty"></param>
    public static void f_GetArtifactProperty(CardPoolDT tCardPoolDT, ref RoleProperty artifactProperty)
    {
#if Game
        if (tCardPoolDT.m_ArtifactPoolDT.Template != null)
        {
            artifactProperty.f_AddProperty((int)EM_RoleProperty.Hp, tCardPoolDT.m_ArtifactPoolDT.Template.iHp);
            artifactProperty.f_AddProperty((int)EM_RoleProperty.Atk, tCardPoolDT.m_ArtifactPoolDT.Template.iAtk);
            artifactProperty.f_AddProperty((int)EM_RoleProperty.Def, tCardPoolDT.m_ArtifactPoolDT.Template.iDef);
            artifactProperty.f_AddProperty((int)EM_RoleProperty.MDef, tCardPoolDT.m_ArtifactPoolDT.Template.iMagDef);
        }
#else
        if (tCardPoolDT.m_ArtifactPoolDT != null)
        {
            artifactProperty.f_AddProperty((int)EM_RoleProperty.Hp, tCardPoolDT.m_ArtifactPoolDT.Template.iHp);
            artifactProperty.f_AddProperty((int)EM_RoleProperty.Atk, tCardPoolDT.m_ArtifactPoolDT.Template.iAtk);
            artifactProperty.f_AddProperty((int)EM_RoleProperty.Def, tCardPoolDT.m_ArtifactPoolDT.Template.iDef);
            artifactProperty.f_AddProperty((int)EM_RoleProperty.MDef, tCardPoolDT.m_ArtifactPoolDT.Template.iMagDef);
        }
#endif
    }

    public static void f_GetTalentProperty(CardPoolDT tCardPoolDT, ref RoleProperty tRoleProperty, ref OtherProperty tOtherProperty)
    {
        //--天赋属性库ID
        if (tCardPoolDT.m_iEvolveLv > 0 && tCardPoolDT.m_CardEvolveDT.iTalentId > 0)
        {
            CardTalentDT tCardTalentDT;
            for (int i = 0; i < tCardPoolDT._CardEvolveDT.Count; i++)
            {
                tCardTalentDT = (CardTalentDT)glo_Main.GetInstance().m_SC_Pool.m_CardTalentSC.f_GetSC(tCardPoolDT._CardEvolveDT[i].iTalentId);
                if (tCardTalentDT == null)
                    break;

                if (tCardTalentDT.iTarget != (int)EM_ProTarget.eMyself)
                {
                    tOtherProperty.f_AddOtherProperty(tCardTalentDT.iTarget, tCardTalentDT.iPropertyId1, tCardTalentDT.iPropertyNum1);

                }
                else
                {
                    tRoleProperty.f_AddProperty(tCardTalentDT.iPropertyId1, tCardTalentDT.iPropertyNum1);
                    tRoleProperty.f_AddProperty(tCardTalentDT.iPropertyId2, tCardTalentDT.iPropertyNum2);
                }
            }
        }
    }

    public static void f_GetAllEquipProperty(List<EquipPoolDT> aEquipPoolDT, ref RoleProperty tRoleProperty)
    {
        //---装备属性(a、强化 b、精炼 c、套装 d、升星 大师)
        //强化 (基础值 + 强化等级*(强化属性Id 强化初始值))
        //精炼 (精炼等级*(精炼属性Id1 每级属性值1)   精炼等级*(精炼属性Id2 每级属性值2))
        //大师 (装备等级附加 属性ID1 属性ID2 属性ID3 属性ID4 属性ID5)直接相加
        //套装
        if (aEquipPoolDT != null)
        {
            bool isInvalid = false;//是否无效（4件装备都不为空）
            int IntensifyLevel = -1;//4件装备中的最低强化等级
            int RefineLevel = -1;//4件装备中的最低精炼等级
            for (int i = 0; i < aEquipPoolDT.Count; i++)
            {
                if (aEquipPoolDT[i] != null)
                {
                    f_DispEquip(ref tRoleProperty, aEquipPoolDT[i]);
                    if (IntensifyLevel == -1 || aEquipPoolDT[i].m_lvIntensify < IntensifyLevel)
                        IntensifyLevel = aEquipPoolDT[i].m_lvIntensify;
                    if (RefineLevel == -1 || aEquipPoolDT[i].m_lvRefine < RefineLevel)
                        RefineLevel = aEquipPoolDT[i].m_lvRefine;
                }
                else
                {
                    isInvalid = true;
                }
            }
            //大师 (装备等级附加 属性ID1 属性ID2 属性ID3 属性ID4 属性ID5)直接相加
            //1、装备强化。2、装备精炼。
            if (aEquipPoolDT.Count == 4 && !isInvalid)
            {
                MasterDT tMasterDT = GetMasterDT((int)EM_Master.EquipIntensify, IntensifyLevel);
                if (tMasterDT != null)
                {   //装备强化
                    //iProID1	iPro1	iProId2	iPro	iProId3	iPro3	iProId4	iPro4	iProId5	iPro5
                    tRoleProperty.f_AddProperty(tMasterDT.iProID1, tMasterDT.iPro1);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId2, tMasterDT.iPro);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId3, tMasterDT.iPro3);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId4, tMasterDT.iPro4);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId5, tMasterDT.iPro5);
                }
                tMasterDT = GetMasterDT((int)EM_Master.EquipRefine, RefineLevel);
                if (tMasterDT != null)
                {//装备精炼
                    //iProID1	iPro1	iProId2	iPro	iProId3	iPro3	iProId4	iPro4	iProId5	iPro5
                    tRoleProperty.f_AddProperty(tMasterDT.iProID1, tMasterDT.iPro1);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId2, tMasterDT.iPro);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId3, tMasterDT.iPro3);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId4, tMasterDT.iPro4);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId5, tMasterDT.iPro5);
                }
            }



            //计算套装属性
            if (aEquipPoolDT.Count >= 2)  //当前装备两件以上装备
            {
                SetEquipDT[] teuqip = null;
                if (aEquipPoolDT.Count == 2)
                    teuqip = RoleTools.f_GetSetEquip(aEquipPoolDT[0], aEquipPoolDT[1]);
                else if (aEquipPoolDT.Count == 3)
                    teuqip = RoleTools.f_GetSetEquip(aEquipPoolDT[0], aEquipPoolDT[1], aEquipPoolDT[2]);
                else if (aEquipPoolDT.Count == 4)
                    teuqip = RoleTools.f_GetSetEquip(aEquipPoolDT[0], aEquipPoolDT[1], aEquipPoolDT[2], aEquipPoolDT[3]);
                int equipnum = 0;   //有效装备数量
                for (int i = 0; i < aEquipPoolDT.Count; i++)
                {
                    if (aEquipPoolDT[i] != null)
                        equipnum++; //So luong trang bi dang mac
                }
                if (teuqip.Length == 1)
                {
                    switch (equipnum)
                    {
                        case 2:
                            tRoleProperty.f_AddProperty(teuqip[0].iTwoEquipProId, teuqip[0].iTwoPro);
                            break;
                        case 3:
                            tRoleProperty.f_AddProperty(teuqip[0].iTwoEquipProId, teuqip[0].iTwoPro);
                            tRoleProperty.f_AddProperty(teuqip[0].iThreeEquipProId, teuqip[0].iThreePro);
                            break;
                        case 4:
                            tRoleProperty.f_AddProperty(teuqip[0].iTwoEquipProId, teuqip[0].iTwoPro);
                            tRoleProperty.f_AddProperty(teuqip[0].iThreeEquipProId, teuqip[0].iThreePro);
                            tRoleProperty.f_AddProperty(teuqip[0].iFourEquipProId1, teuqip[0].iFourPro1);
                            tRoleProperty.f_AddProperty(teuqip[0].iFourEquipProId2, teuqip[0].iFourPro2);
                            break;
                        default:
                            break;
                    }
                }
                else if (teuqip.Length == 2)
                {
                    int[] tmp = { 0, 0 };
                    int Equipindex = 0;
                    switch (equipnum)
                    {
                        case 3:
                            f_setEquip(ref tmp, equipnum, ref Equipindex, teuqip, aEquipPoolDT);
                            tRoleProperty.f_AddProperty(teuqip[Equipindex].iTwoEquipProId, teuqip[Equipindex].iTwoPro);
                            break;
                        case 4:
                            f_setEquip(ref tmp, equipnum, ref Equipindex, teuqip, aEquipPoolDT);
                            if (tmp.Contains(2) && !tmp.Contains(1))
                            {
                                tRoleProperty.f_AddProperty(teuqip[0].iTwoEquipProId, teuqip[0].iTwoPro);
                                tRoleProperty.f_AddProperty(teuqip[1].iTwoEquipProId, teuqip[1].iTwoPro);
                            }
                            else if (tmp.Contains(3))
                            {
                                tRoleProperty.f_AddProperty(teuqip[Equipindex].iTwoEquipProId, teuqip[Equipindex].iTwoPro);
                                tRoleProperty.f_AddProperty(teuqip[Equipindex].iThreeEquipProId, teuqip[Equipindex].iThreePro);
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if (teuqip.Length == 3)
                {
                    int[] tmp = { 0, 0, 0 };
                    int Equipindex = 0;
                    if (equipnum == 4)
                    {
                        f_setEquip(ref tmp, equipnum - 1, ref Equipindex, teuqip, aEquipPoolDT);
                        if (tmp.Contains(2))
                            tRoleProperty.f_AddProperty(teuqip[Equipindex].iTwoEquipProId, teuqip[Equipindex].iTwoPro);
                    }
                }
            }//end SetEquip 
        }

    }

    public static void f_UpdateAddProperty(RoleProperty tRoleProperty)
    {
        //TsuCode
        //MessageBox.ASSERT("TsuLog RolePropertyTools f_UpdateAddProperty + tempID:  ");
        int atkRoot = tRoleProperty.f_GetProperty((int)EM_RoleProperty.Atk); //
        int atkAdd = 0;
        atkAdd = tRoleProperty.f_GetProperty((int)EM_RoleProperty.AddAtk);
        
        int atk = CommonTools.f_GetPercentValueTenThousandInt32(atkRoot,atkAdd);
        //MessageBox.ASSERT("TsuLog RolePropertyTools f_UpdateAddProperty: LC: " + atk + "-Value1:-" + atkRoot + "-" + atkAdd);
        tRoleProperty.f_UpdateProperty((int)EM_RoleProperty.Atk, atk);
        //
        //TsuComment//tRoleProperty.f_UpdateProperty((int)EM_RoleProperty.Atk, CommonTools.f_GetPercentValueTenThousandInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.Atk), tRoleProperty.f_GetProperty((int)EM_RoleProperty.AddAtk)));

        tRoleProperty.f_UpdateProperty((int)EM_RoleProperty.Def, CommonTools.f_GetPercentValueTenThousandInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.Def), tRoleProperty.f_GetProperty((int)EM_RoleProperty.AddDef)));
        tRoleProperty.f_UpdateProperty((int)EM_RoleProperty.MDef, CommonTools.f_GetPercentValueTenThousandInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.MDef), tRoleProperty.f_GetProperty((int)EM_RoleProperty.AddMDef)));
        tRoleProperty.f_UpdateHp(CommonTools.f_GetPercentValueTenThousandInt64(tRoleProperty.f_GetHp(), tRoleProperty.f_GetProperty((int)EM_RoleProperty.AddHp)));
    }

    public static void f_GetAwakenEquipProperty(CardPoolDT tCardPoolDT, ref RoleProperty tRoleProperty)
    {
        //领悟装备
        if ((tCardPoolDT.m_iFlagAwaken != 0 || tCardPoolDT.m_iLvAwaken <= 60))
        {
            Dictionary<int, int> tDict = new Dictionary<int, int>();  //ID    数量
            AwakenCardDT tAwaken = null;
            for (int awakenLv = 0; awakenLv <= tCardPoolDT.m_iLvAwaken; awakenLv++)
            {
                if (awakenLv < 60)       //如果领悟等级等于0
                {
                    tAwaken = glo_Main.GetInstance().m_SC_Pool.m_AwakenCardSC.f_GetSC(awakenLv + 1) as AwakenCardDT;
                }
                else
                {
                    tAwaken = new AwakenCardDT();
                }
                if (tCardPoolDT.m_iLvAwaken == awakenLv)    //当前等级
                {
                    string flagAwaken = tCardPoolDT.m_iFlagAwaken.ToString();
                    if (flagAwaken.Length != 4)   //掩码转换字符串
                    {
                        for (int i = flagAwaken.Length; i <= 3; i++)
                        {
                            flagAwaken = "0" + flagAwaken;
                        }
                    }
                    if (flagAwaken[0] != '0')
                        AddAwaken(ref tDict, tAwaken.iEquipID1, 0, 0, 0);
                    if (flagAwaken[1] != '0')
                        AddAwaken(ref tDict, 0, tAwaken.iEquipID2, 0, 0);
                    if (flagAwaken[2] != '0')
                        AddAwaken(ref tDict, 0, 0, tAwaken.iEquipID3, 0);
                    if (flagAwaken[3] != '0')
                        AddAwaken(ref tDict, 0, 0, 0, tAwaken.iEquipID4);
                    break;
                }
                AddAwaken(ref tDict, tAwaken.iEquipID1, tAwaken.iEquipID2, tAwaken.iEquipID3, tAwaken.iEquipID4);
            }

            for (int i = 0; i < tDict.Count; i++)
            {
                AwakenEquipDT tEquip = glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(tDict.ElementAt(i).Key) as AwakenEquipDT;
                tRoleProperty.f_AddProperty(tEquip.iAddProId1, tEquip.iAddPro1 * tDict.ElementAt(i).Value);
                tRoleProperty.f_AddProperty(tEquip.iAddProId2, tEquip.iAddPro2 * tDict.ElementAt(i).Value);
                tRoleProperty.f_AddProperty(tEquip.iAddProId3, tEquip.iAddPro3 * tDict.ElementAt(i).Value);
                tRoleProperty.f_AddProperty(tEquip.iAddProId4, tEquip.iAddPro4 * tDict.ElementAt(i).Value);
            }
        }
    }

    public static void f_GetTreasureProperty(CardPoolDT tCardPoolDT, List<TreasurePoolDT> aTreasurePoolDT, ref RoleProperty tRoleProperty)
    {
        //宝物强化
        //宝物精炼
        if (aTreasurePoolDT != null)
        {
            bool isInvalid = false;//是否无效（2件法宝都不为空）
            int IntensifyLevel = -1;//2件法宝中的最低强化等级
            int RefineLevel = -1;//2件法宝中的最低精炼等级
            for (int j = 0; j < aTreasurePoolDT.Count; j++)
            {
                if (aTreasurePoolDT[j] != null)
                {
                 
                    tRoleProperty = f_DispTreasure(ref tRoleProperty, aTreasurePoolDT[j]);
                    if (IntensifyLevel == -1 || aTreasurePoolDT[j].m_lvIntensify < IntensifyLevel)
                        IntensifyLevel = aTreasurePoolDT[j].m_lvIntensify;
                    if (RefineLevel == -1 || aTreasurePoolDT[j].m_lvRefine < RefineLevel)
                        RefineLevel = aTreasurePoolDT[j].m_lvRefine;
                }
                else
                {
                    isInvalid = true;
                }
            }
            //大师 (装备等级附加 属性ID1 属性ID2 属性ID3 属性ID4 属性ID5)直接相加
            //1、法宝强化。2、法宝精炼。
            if (aTreasurePoolDT.Count == 2 && !isInvalid)
            {
                MasterDT tMasterDT = GetMasterDT((int)EM_Master.TreasureIntensify, IntensifyLevel);
                if (tMasterDT != null)
                {   //装备强化
                    //iProID1	iPro1	iProId2	iPro	iProId3	iPro3	iProId4	iPro4	iProId5	iPro5
                    tRoleProperty.f_AddProperty(tMasterDT.iProID1, tMasterDT.iPro1);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId2, tMasterDT.iPro);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId3, tMasterDT.iPro3);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId4, tMasterDT.iPro4);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId5, tMasterDT.iPro5);
                }
                tMasterDT = GetMasterDT((int)EM_Master.TreasureRefine, RefineLevel);
                if (tMasterDT != null)
                {//装备精炼
                    //iProID1	iPro1	iProId2	iPro	iProId3	iPro3	iProId4	iPro4	iProId5	iPro5
                    tRoleProperty.f_AddProperty(tMasterDT.iProID1, tMasterDT.iPro1);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId2, tMasterDT.iPro);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId3, tMasterDT.iPro3);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId4, tMasterDT.iPro4);
                    tRoleProperty.f_AddProperty(tMasterDT.iProId5, tMasterDT.iPro5);
                }
            }
        }

    }

    public static void f_GetFateProperty(CardPoolDT tCardPoolDT, ref RoleProperty tRoleProperty)
    {
#if Game
        //TsuCode
        //MessageBox.ASSERT("TsuLog RolePropertyTools f_GetFateProperty");
        //
        //缘分
        Data_Pool.m_TeamPool.f_UpdateCardFate(tCardPoolDT);
        for (int i = 0; i < tCardPoolDT.m_CardFatePoolDT.m_aFateList.Count; i++)
        {
            if (tCardPoolDT.m_CardFatePoolDT.m_aFateIsOk[i])
            {
                if (tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrID1 != 0)
                    tRoleProperty.f_AddProperty(tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrID1, tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrValue1);
                if (tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrID2 != 0)
                    tRoleProperty.f_AddProperty(tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrID2, tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrValue2);
                if (tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrID3 != 0)
                    tRoleProperty.f_AddProperty(tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrID3, tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrValue3);
                if (tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrID4 != 0)
                    tRoleProperty.f_AddProperty(tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrID4, tCardPoolDT.m_CardFatePoolDT.m_aFateList[i].iAttrValue4);
            }
        }
#endif
    }

    public static void f_GetDestinyProperty(CardPoolDT tCardPoolDT, int iDestinyProgress, ref RoleProperty tRoleProperty)
    {
        //计算阵法的附加属性_
        if (iDestinyProgress > 0)
        {
            if (iDestinyProgress > 300)
            {
MessageBox.ASSERT("Id exceeds array limit " + tCardPoolDT.iId + " " + iDestinyProgress);
                return;
            }
            for (int i = 1; i <= iDestinyProgress; i++)
            {
                BattleFormationsDT data = glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(i) as BattleFormationsDT;
                if (data != null && data.iAttrID != 0)
                {
                    tRoleProperty.f_AddProperty((int)data.iAttrID, data.iAttrValue);
                }
            }
        }
        //if (IsProperty[(int)EM_PropertyControl.isAddPro])
        //{
        //    if (_RoleProperty.f_GetProperty((int)EM_RoleProperty.AddHp) != 0)
        //        _RoleProperty.f_UpdateProperty((int)EM_RoleProperty.Hp, Convert.ToInt32(_RoleProperty.f_GetProperty((int)EM_RoleProperty.Hp) * _RoleProperty.f_GetProperty((int)EM_RoleProperty.AddHp) / 1000 - 0.5f));

        //    if (_RoleProperty.f_GetProperty((int)EM_RoleProperty.AddAtk) != 0)
        //        _RoleProperty.f_UpdateProperty((int)EM_RoleProperty.Atk, Convert.ToInt32(_RoleProperty.f_GetProperty((int)EM_RoleProperty.Atk) * _RoleProperty.f_GetProperty((int)EM_RoleProperty.AddAtk) / 1000 - 0.5f));

        //    if (_RoleProperty.f_GetProperty((int)EM_RoleProperty.AddDef) != 0)
        //        _RoleProperty.f_UpdateProperty((int)EM_RoleProperty.Def, Convert.ToInt32(_RoleProperty.f_GetProperty((int)EM_RoleProperty.Def) * _RoleProperty.f_GetProperty((int)EM_RoleProperty.AddDef) / 1000 - 0.5f));

        //    if (_RoleProperty.f_GetProperty((int)EM_RoleProperty.AddMDef) != 0)
        //        _RoleProperty.f_UpdateProperty((int)EM_RoleProperty.MDef, Convert.ToInt32(_RoleProperty.f_GetProperty((int)EM_RoleProperty.MDef) * _RoleProperty.f_GetProperty((int)EM_RoleProperty.MDef) / 1000 - 0.5f));
        //}
        //临时使用 ----------待修改
        //tRoleProperty.f_AddHp(_RoleProperty.f_GetProperty((int)EM_RoleProperty.Hp));
    }

    /// <summary>
    /// 计算CardPoolDT里的全属性，计算完成保存在CardPoolDT里面的m_RoleProperty (包含强化大师增加的属性)
    /// (包含阵法和领悟增加的属性)
    /// 
    /// IsProperty使用EM_PropertyControl
    /// </summary>
    /// <param name="tCardPoolDT">卡牌</param>
    /// <param name="aEquipPoolDT">有效装备列表，装备不存在则为null</param>
    /// <returns></returns>
    public static RoleProperty f_Disp(CardPoolDT tCardPoolDT, List<EquipPoolDT> aEquipPoolDT, int iDestinyProgress, List<TreasurePoolDT> aTreasurePoolDT = null, List<GodEquipPoolDT> aGodEquipPoolDT = null, bool[] IsProperty = null)
    {
        return tCardPoolDT.m_RolePropertyPool.f_Disp(aEquipPoolDT, iDestinyProgress, aTreasurePoolDT, tCardPoolDT.uSkyDestinyLv, aGodEquipPoolDT, IsProperty);
    }

    /// <summary>
    /// 等级从1开始
    /// </summary>
    /// <returns></returns>
    public static int CalculatePropertyStartLv1(int iBase, int iAdd, int iLv)
    {
        //基础值 + 升级值 * (等级 - 1)
        return iBase + iAdd * (iLv - 1);
    }
    /// <summary>
    /// 等级从0开始
    /// </summary>
    /// <returns></returns>
    private static int CalculatePropertyStartLv0(int iBase, int iAdd, int iLv)
    {
        //基础值 + 升级值 * (等级 - 1)
        return iBase + iAdd * iLv;
    }

    /// <summary>
    /// 等级从1开始
    /// </summary>
    /// <returns></returns>
    private static long CalculatePropertyStartLv1(long iBase, int iAdd, int iLv)
    {
        //基础值 + 升级值 * (等级 - 1)
        return iBase + iAdd * (iLv - 1);
    }
    /// <summary>
    /// 等级从0开始
    /// </summary>
    /// <returns></returns>
    private static long CalculatePropertyStartLv0(long iBase, int iAdd, int iLv)
    {
        //基础值 + 升级值 * (等级)
        return iBase + iAdd * iLv;
    }

    private static MasterDT GetMasterDT(int iType, int iLv)
    {
        List<NBaseSCDT> tmpNB = glo_Main.GetInstance().m_SC_Pool.m_MasterSC.f_GetAll();
        for (int i = 0; i < tmpNB.Count; i++)
        {
            MasterDT item = (MasterDT)tmpNB[i];
            MasterDT itemNext = (i >= tmpNB.Count - 1) ? null : (MasterDT)tmpNB[i + 1];
            if (item.iType == iType)
            {
                if (iLv >= item.iLv)
                {
                    if (itemNext == null)
                        return item;
                    if (itemNext.iType != iType)
                        return item;
                    else if (iLv < itemNext.iLv)
                        return item;
                }
            }
        }
        return null;
    }
    private static void AddAwaken(ref Dictionary<int, int> tdict, int id1, int id2, int id3, int id4)
    {
        if (id1 != 0)
        {
            if (tdict.ContainsKey(id1))
                tdict[id1]++;
            else
                tdict.Add(id1, 1);
        }
        if (id2 != 0)
        {
            if (tdict.ContainsKey(id2))
                tdict[id2]++;
            else
                tdict.Add(id2, 1);
        }
        if (id3 != 0)
        {
            if (tdict.ContainsKey(id3))
                tdict[id3]++;
            else
                tdict.Add(id3, 1);
        }
        if (id4 != 0)
        {
            if (tdict.ContainsKey(id4))
                tdict[id4]++;
            else
                tdict.Add(id4, 1);
        }
    }

    public static RoleProperty f_DispNpc(MonsterDT tMonsterDT)
    {
        RoleProperty _RoleProperty = new RoleProperty();
        _RoleProperty.f_Reset();
        //iAnger	iAtk	iHp	iDef	iMDef	iHitR	iDodgeR	iCritR	iAntiknockR
        _RoleProperty.f_AddProperty((int)EM_RoleProperty.Anger, tMonsterDT.iAnger);
        _RoleProperty.f_AddProperty((int)EM_RoleProperty.Atk, tMonsterDT.iAtk);
        _RoleProperty.f_AddHp(tMonsterDT.iHp);            //.f_AddProperty((int)EM_RoleProperty.Hp, tMonsterDT.iHp);
        _RoleProperty.f_AddProperty((int)EM_RoleProperty.Def, tMonsterDT.iDef);
        _RoleProperty.f_AddProperty((int)EM_RoleProperty.MDef, tMonsterDT.iMDef);
        _RoleProperty.f_AddProperty((int)EM_RoleProperty.HitR, tMonsterDT.iHitR);
        _RoleProperty.f_AddProperty((int)EM_RoleProperty.DodgeR, tMonsterDT.iDodgeR);
        _RoleProperty.f_AddProperty((int)EM_RoleProperty.CritR, tMonsterDT.iCritR);
        _RoleProperty.f_AddProperty((int)EM_RoleProperty.AntiknockR, tMonsterDT.iAntiknockR);

        return _RoleProperty;
    }

    /// <summary>
    /// 计算战斗力
    /// </summary>
    /// <param name="tRoleProperty"></param>
    /// <returns></returns>
    public static int f_GetBattlePower(RoleProperty tRoleProperty)
    {
        /*
                //int iAtk = CommonTools.f_GetPercentValueTenThousandInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.Atk), tRoleProperty.f_GetProperty((int)EM_RoleProperty.AddAtk));
                //long iHp = 0;
                ////iHp = (long)(tRoleProperty.f_GetHp() * ((tRoleProperty.f_GetProperty((int)EM_RoleProperty.AddHp) + 10000)  / 10000));
                //iHp = (long)(tRoleProperty.f_GetHp() * ((tRoleProperty.f_GetProperty((int)EM_RoleProperty.AddHp) + 10000) * 1.0f / 10000));
                //int iDef = CommonTools.f_GetPercentValueTenThousandInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.Def), tRoleProperty.f_GetProperty((int)EM_RoleProperty.AddDef));
                //int iMDef = CommonTools.f_GetPercentValueTenThousandInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.MDef), tRoleProperty.f_GetProperty((int)EM_RoleProperty.AddMDef));
                RolePropertyTools.f_UpdateAddProperty(tRoleProperty);
        */ 

        //TsuCode
        //MessageBox.ASSERT("TsuLog RolePropertyTools f_GetBattlePower");

       //MessageBox.ASSERT("Info Card Ban Dau:: Atk-" + tRoleProperty.f_GetProperty((int)EM_RoleProperty.Atk) + "-Hp-" + tRoleProperty.f_GetProperty((int)EM_RoleProperty.Hp) + "-Def-" + tRoleProperty.f_GetProperty((int)EM_RoleProperty.Def) + "-Mdef-" + tRoleProperty.f_GetProperty((int)EM_RoleProperty.MDef));
        //
        RolePropertyTools.f_UpdateAddProperty(tRoleProperty);
        int iAtk = tRoleProperty.f_GetProperty((int)EM_RoleProperty.Atk);
        long iHp = tRoleProperty.f_GetHp();
        int iDef = tRoleProperty.f_GetProperty((int)EM_RoleProperty.Def);
        int iMDef = tRoleProperty.f_GetProperty((int)EM_RoleProperty.MDef);
        //iHp = (long)(iHp / ((tRoleProperty.f_GetProperty((int)EM_RoleProperty.AddHp) * 1.0f + 10000) / 10000));
  

        float fCritR = tRoleProperty.f_GetProperty((int)EM_RoleProperty.CritR) * 1.0f / 10000;
        float fAntiknockR = tRoleProperty.f_GetProperty((int)EM_RoleProperty.AntiknockR) * 1.0f / 10000;
        float fHitR = tRoleProperty.f_GetProperty((int)EM_RoleProperty.HitR) * 1.0f / 10000;
        float fDodgeR = tRoleProperty.f_GetProperty((int)EM_RoleProperty.DodgeR) * 1.0f / 10000;
        float fDamageR = tRoleProperty.f_GetProperty((int)EM_RoleProperty.DamageR) * 1.0f / 10000;
        float fInjurySR = tRoleProperty.f_GetProperty((int)EM_RoleProperty.InjurySR) * 1.0f / 10000;

        //MessageBox.ASSERT("TsuLog Info Card:: Atk-" + iAtk + "-Hp-" + iHp + "-Def-" + iDef + "-Mdef-" + iMDef + "-Crit-" + fCritR + "-AntiKnock-" + fAntiknockR + "-fHitR" + fHitR + "-Dodge" + fDodgeR + "-Damage-" + fDamageR + "-Injury-" + fInjurySR);
        int iNum = (int)((iAtk * 3 + iHp * 0.25f + iDef * 5 + iMDef * 5) * (1 + fCritR + fAntiknockR + fHitR + fDodgeR + fDamageR + fInjurySR));
        
       // MessageBox.ASSERT("TsuLog Info Card:: " + iNum);
        return iNum;
    }
    private static void f_setEquip(ref int[] tmp, int num, ref int Equipindex, SetEquipDT[] teuqip, List<EquipPoolDT> aEquipPoolDT)
    {
        for (int i = 0; i < teuqip.Length; i++)
        {
            tmp[i] = 0;
            for (int j = 0; j < aEquipPoolDT.Count; j++)
            {
                if (aEquipPoolDT[j] != null)
                {
                    if (RoleTools.f_IsHaveSetEquip(teuqip[i], aEquipPoolDT[j].m_EquipDT.iId))
                    {
                        tmp[i]++;
                        if (tmp[i] == num - 1)
                            Equipindex = i;
                    }
                }
            }
        }
    }

    public static void f_AddOtherCardProperty(ref RoleProperty tRoleProperty1, OtherProperty tRoleProperty2, int iCardCamp)
    {
#if Game
        if (!tRoleProperty2.m_bIsAddProperty)
            return;
#endif
        for (int i = 0; i < tRoleProperty2.m_iIndex; i++)
        {
            switch ((EM_ProTarget)tRoleProperty2.f_GetTarget(i))
            {
                case EM_ProTarget.eAllCard:
                    _AddPropertyForIndex(ref tRoleProperty1, tRoleProperty2, i);
                    break;
                case EM_ProTarget.eAllWei:
                    if (iCardCamp == (int)EM_CardCamp.eCardWei)
                        _AddPropertyForIndex(ref tRoleProperty1, tRoleProperty2, i);
                    break;
                case EM_ProTarget.eAllShu:
                    if (iCardCamp == (int)EM_CardCamp.eCardShu)
                        _AddPropertyForIndex(ref tRoleProperty1, tRoleProperty2, i);
                    break;
                case EM_ProTarget.eAllWu:
                    if (iCardCamp == (int)EM_CardCamp.eCardWu)
                        _AddPropertyForIndex(ref tRoleProperty1, tRoleProperty2, i);
                    break;
                case EM_ProTarget.eAllQun:
                    if (iCardCamp == (int)EM_CardCamp.eCardGroupHero)
                        _AddPropertyForIndex(ref tRoleProperty1, tRoleProperty2, i);
                    break;
                default:
MessageBox.DEBUG("Additional properties " + tRoleProperty2.f_GetTarget(i).ToString());
                    break;
            }
        }
    }

    private static void _AddPropertyForIndex(ref RoleProperty tRoleProperty1, OtherProperty tRoleProperty2, int index)
    {
        switch ((EM_RoleProperty)tRoleProperty2.f_GetPropertyId(index))
        {
            case EM_RoleProperty.Def:
                tRoleProperty1.f_AddProperty((int)EM_RoleProperty.Def, tRoleProperty2.f_GetPropertyNum(index));
                tRoleProperty1.f_AddProperty((int)EM_RoleProperty.MDef, tRoleProperty2.f_GetPropertyNum(index));
                break;
            case EM_RoleProperty.AddDef:
                tRoleProperty1.f_AddProperty((int)EM_RoleProperty.AddDef, tRoleProperty2.f_GetPropertyNum(index));
                tRoleProperty1.f_AddProperty((int)EM_RoleProperty.AddMDef, tRoleProperty2.f_GetPropertyNum(index));
                break;
            default:
                tRoleProperty1.f_AddProperty(tRoleProperty2.f_GetPropertyId(index), tRoleProperty2.f_GetPropertyNum(index));
                break;
        }
    }

#if Game
    public static void f_UpdateLegionSkillProperty(LegionSkillPool tLegionSkill, ref RoleProperty tRoleProperty)
    {
        LegionSkillPoolDT poolDT;
        for (int i = 0; i < tLegionSkill.f_GetAll().Count; i++)
        {
            poolDT = tLegionSkill.f_GetAll()[i] as LegionSkillPoolDT;
            if (poolDT.m_LegionSkillDT != null)
            {
                if (poolDT.m_LegionSkillDT.iBuffID != 0)
                {
                    tRoleProperty.f_AddProperty((int)poolDT.m_LegionSkillDT.iBuffID, poolDT.m_LegionSkillDT.iBuffCount);
                }
            }
        }
    }
#endif



#if Game
    public static void f_UpdateFanshionableDress(FanshionableDressPoolDT tFanshionableDressPoolDT, ref RoleProperty tRoleProperty)
    {
        if (tFanshionableDressPoolDT == null)
        {
            return;
        }
        tRoleProperty.f_AddProperty(tFanshionableDressPoolDT.m_FashionableDressDT.iPropertyId1, tFanshionableDressPoolDT.m_FashionableDressDT.iPropertyNum1);
        tRoleProperty.f_AddProperty(tFanshionableDressPoolDT.m_FashionableDressDT.iPropertyId2, tFanshionableDressPoolDT.m_FashionableDressDT.iPropertyNum2);
    }
#else
    public static void f_UpdateFanshionableDress(FashionableDressDT tFashionableDressDT, ref RoleProperty tRoleProperty)
    {
        if (tFashionableDressDT == null)
        {
            return;
        }
        tRoleProperty.f_AddProperty(tFashionableDressDT.iPropertyId1, tFashionableDressDT.iPropertyNum1);
        tRoleProperty.f_AddProperty(tFashionableDressDT.iPropertyId2, tFashionableDressDT.iPropertyNum2);
    }
#endif

#if Game
    public static void f_UpdateTactical(CardPoolDT card, int MaxId, ref RoleProperty tRoleProperty)
    {
        int SkillId = card.m_TacticalId;
        TacticalDT tTacticalDT;
        for (int i = MaxId; i > MaxId - 10; i--)
        {
            if (i <= 0)
                continue;
            tTacticalDT = glo_Main.GetInstance().m_SC_Pool.m_TacticalSC.f_GetSC(i) as TacticalDT;
            if (tTacticalDT.iSkillType == SkillId)
            {
                tRoleProperty.f_AddProperty(tTacticalDT.iProId, tTacticalDT.iProNum);
                break;
            }
        }
    }
#else
    public static void f_UpdateTactical(CardPoolDT card, ref RoleProperty tRoleProperty)
    {
        if (card.m_TacticalId > 0)
        {
            TacticalDT tTacticalDT = glo_Main.GetInstance().m_SC_Pool.m_TacticalSC.f_GetSC(card.m_TacticalId) as TacticalDT;
            if (tTacticalDT == null)
            {
MessageBox.ASSERT("No squad information found" + card.m_TacticalId);
            }
            else
            {
                tRoleProperty.f_AddProperty(tTacticalDT.iProId, tTacticalDT.iProNum);
            }
        }
    }
#endif
    /// <summary>
    /// 根据天命等级来计算累计加成
    /// </summary>
    public static void f_CountSkyDesnityForSkyLevel(int level, ref RoleProperty tRoleProperty)
    {
        MessageBox.ASSERT("TsuLog SkyDestiny " + level);
        SkyDesnitySC SkyPro = glo_Main.GetInstance().m_SC_Pool.m_SkyDesnitySC;
        SkyDesnityDT SkyDT;
        if (level > 13) level = 13;   //最大等级为13  进行偏移
        for (int i = level; i > 0; i--)
        {
            SkyDT = SkyPro.f_GetSC(i) as SkyDesnityDT;
            tRoleProperty.f_AddProperty(SkyDT.iSkyDestinyProid1, SkyDT.iSkyDestinyPro1);
            tRoleProperty.f_AddProperty(SkyDT.iSkyDestinyProid2, SkyDT.iSkyDestinyPro2);
            tRoleProperty.f_AddProperty(SkyDT.iSkyDestinyProid3, SkyDT.iSkyDestinyPro3);
            tRoleProperty.f_AddProperty(SkyDT.iSkyDestinyProid4, SkyDT.iSkyDestinyPro4);
        }
    }

    public static void f_CountMonthCard(CardPoolDT card, ref RoleProperty tRoleProperty)
    {
        bool monthCard = Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy25, perpatualCard = Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy50;

        if (monthCard)
        {
            GameParamDT MonthCard = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameNameParamType.MonthCard) as GameParamDT;
            if (card.m_CardDT.iCardCamp == (int)EM_CardCamp.eCardMain)
                //for (int i = (int)EM_RoleProperty.AddAtk; i <= 8; i++) //TsuComment
                //    tRoleProperty.f_AddProperty(i, MonthCard.iParam1);
                for (int i = (int)EM_RoleProperty.PInjuryAR; i <= (int)EM_RoleProperty.PInjurySR; i++) //TsuCode
                       tRoleProperty.f_AddProperty(i, MonthCard.iParam1);
        }

        if (perpatualCard)
        {
            GameParamDT PerpatualCard = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameNameParamType.PerpetualCard) as GameParamDT;
            //for (int i = (int)EM_RoleProperty.AddAtk; i <= 8; i++)
            //    tRoleProperty.f_AddProperty(i, PerpatualCard.iParam1);
            for (int i = (int)EM_RoleProperty.PInjuryAR; i <= (int)EM_RoleProperty.PInjurySR; i++) //TsuCode
                tRoleProperty.f_AddProperty(i, PerpatualCard.iParam1);
        }
    }

    public static void f_CountAuraCamp(CardPoolDT card, ref RoleProperty tRoleProperty)
    {
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_AuraCampSC.f_GetAll();
        Dictionary<int, int> dict_AuraCamp = new Dictionary<int, int>();
        GetDictAuraCamp(ref dict_AuraCamp);
        Dictionary<EM_RoleProperty, int> dict_Property = new Dictionary<EM_RoleProperty, int>();
        // lấy ds config vòng sáng phe kích hoạt của trận hình
        List<AuraCampDT> auraCampDTs = new List<AuraCampDT>();
        for (int i = 0; i < dict_AuraCamp.Count; i++)
        {
            int type = dict_AuraCamp.ElementAt(i).Key;
            int level = dict_AuraCamp.ElementAt(i).Value;
            AuraCampDT auraCampDT = GetConfigAuraCamp(listData, level, type);
            if (auraCampDT != null)
            {
                auraCampDTs.Add(auraCampDT);
            }
        }
        // xử lý active thuộc tính từ config
        if (auraCampDTs.Count == 0) return;
        for (int i = 0; i < auraCampDTs.Count; i++)
        {
            AuraCampDT auraCampDT = auraCampDTs[i] as AuraCampDT;
            string[] propertyGroup = auraCampDT.szParam.Split('#');
            for (int j = 0; j < propertyGroup.Length; j++)
            {
                string[] propertyItem = propertyGroup[j].Split(';');
                int value;
                if (dict_Property.TryGetValue((EM_RoleProperty)int.Parse(propertyItem[0]), out value))
                {
                    value += int.Parse(propertyItem[1]);
                    dict_Property[(EM_RoleProperty)int.Parse(propertyItem[0])] = value;
                }
                else
                {
                    dict_Property.Add((EM_RoleProperty)int.Parse(propertyItem[0]), int.Parse(propertyItem[1]));
                }
            }
        }

        for (int i = 0; i < dict_Property.Count; i++)
        {
            int Key = (int)dict_Property.ElementAt(i).Key;
            int Value = dict_Property.ElementAt(i).Value;

            tRoleProperty.f_AddProperty(Key, Value);
        }
    }
    /// <summary>
    ///  lấy config với level bằng hoặc nằm giữa
    /// </summary>
    public static AuraCampDT GetConfigAuraCamp(List<NBaseSCDT> listData, int level, int camp)
    {
        AuraCampDT auraCampDT = null;
        for (int i = 0; i < listData.Count; i++)
        {
            AuraCampDT temp = listData[i] as AuraCampDT;
            AuraCampDT nextAuraCampDT = i + 1 < listData.Count ? listData[i + 1] as AuraCampDT : null;
            if (temp.iCamp == camp && temp.szParam != null &&
                (temp.iLevel == level || (nextAuraCampDT != null && level > temp.iLevel && level < nextAuraCampDT.iLevel)))
            {
                return temp;
            }
        }
        return auraCampDT;
    }

    public static List<AuraCampDT> GetConfigAuraCampByType(List<NBaseSCDT> listData, int type)
    {
        List<AuraCampDT> auraCampDTs = new List<AuraCampDT>();
        for (int i = 0; i < listData.Count; i++)
        {
            AuraCampDT auraCampDT = listData[i] as AuraCampDT;
            if (auraCampDT.iCamp == type)
            {
                auraCampDTs.Add(auraCampDT);
            }
        }
        return auraCampDTs;
    }

    public static void GetLevelAuraCamp(ref int Level, ref int Camp)
    {
        Dictionary<int, int> dict_AuraCamp = new Dictionary<int, int>();
        GetDictAuraCamp(ref dict_AuraCamp);
        for (int i = 0; i < dict_AuraCamp.Count; i++)
        {
            int Key = dict_AuraCamp.ElementAt(i).Key;
            int Value = dict_AuraCamp.ElementAt(i).Value;
            if (Key != 0 && Level <= Value)
            {
                Level = Value;
                Camp = Key;
            }
        }
    }
    public static void GetIconAndLevelAuraCamp(ref string icon, ref string lbLevel)
    {
        Dictionary<int, int> dict_AuraCamp = new Dictionary<int, int>();
        GetDictAuraCamp(ref dict_AuraCamp);
        for (int i = 0; i < dict_AuraCamp.Count; i++)
        {
            int type = dict_AuraCamp.ElementAt(i).Key;
            int level = dict_AuraCamp.ElementAt(i).Value;
            if (level <= 1) continue;
            if (icon == "")
            {
                icon += "IconCamp_" + type;
            }
            else
            {
                icon += "_" + type;
            }
            if (lbLevel == "")
            {
                lbLevel += "" + level;
            }
            else
            {
                lbLevel += "/" + level;
            }
        }
    }
    public static void GetDictAuraCamp(ref Dictionary<int, int> dict_AuraCamp)
    {
        for (int i = 0; i < Data_Pool.m_TeamPool.f_GetAll().Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[i];
            if (tTeamPoolDT == null) continue;
            int num = 0;
            if (dict_AuraCamp.TryGetValue(tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardCamp, out num))
            {
                num++;
                dict_AuraCamp[tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardCamp] = num;
            }
            else
            {
                dict_AuraCamp.Add(tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardCamp, 1);
            }
        }
        var temp = dict_AuraCamp.OrderBy(key => key.Key);
        var dic = temp.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        dict_AuraCamp = dic;
    }

    public static bool f_CheckNextLevelAuraCamp(EM_FormationPos needReplacePos, CardPoolDT cardin)
    {
        if (Data_Pool.m_TeamPool.f_GetAll().Count >= (int)needReplacePos + 1)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[(int)needReplacePos];
            if (tTeamPoolDT != null)
            {
                if (tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardCamp == cardin.m_CardDT.iCardCamp && tTeamPoolDT.m_CardPoolDT.m_CardDT.iImportant >= (int)EM_Important.Oragen) return false;
            }
        }
        if (cardin.m_CardDT.iImportant < (int)EM_Important.Oragen) return false;

        Dictionary<int, int> dict_AuraCamp = new Dictionary<int, int>();
        GetDictAuraCamp(ref dict_AuraCamp);
        int levelnew = 1;
        int campnew = cardin.m_CardDT.iCardCamp;
        if (dict_AuraCamp.TryGetValue(cardin.m_CardDT.iCardCamp, out levelnew))
        {
            return true;
        }
        return false;
    }
    // maco clone
    public static List<AuraTypeDT> GetConfigAuraTypeByType(List<NBaseSCDT> listData, int type)
    {
        List<AuraTypeDT> auraTypeDTs = new List<AuraTypeDT>();
        for (int i = 0; i < listData.Count; i++)
        {
            AuraTypeDT auraTypeDT = listData[i] as AuraTypeDT;
            if (auraTypeDT.iType == type)
            {
                auraTypeDTs.Add(auraTypeDT);
            }
        }
        return auraTypeDTs;
    }
    public static void GetIconAndLevelAuraType(ref string icon, ref string lbLevel)
    {
        Dictionary<int, int> dict_FightType = new Dictionary<int, int>();
        GetDictAuraType(ref dict_FightType);
        for (int i = 0; i < dict_FightType.Count; i++)
        {
            int type = dict_FightType.ElementAt(i).Key;
            int level = dict_FightType.ElementAt(i).Value;
            if (level <= 1) continue;
            if (icon == "")
            {
                icon += "IconType_" + type;
            }
            else
            {
                icon += "_" + type;
            }
            if (lbLevel == "")
            {
                lbLevel += "" + level;
            }
            else
            {
                lbLevel += "/" + level;
            }
        }
    }
    /// <summary>
    /// Lấy danh sách số lượng nghề kích hoạt được ở trận hình 
    /// </summary>
    public static void GetDictAuraType(ref Dictionary<int, int> dict_FightType)
    {
        for (int i = 0; i < Data_Pool.m_TeamPool.f_GetAll().Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[i];
            if (tTeamPoolDT == null) continue;
            int num = 0;
            if (dict_FightType.TryGetValue(tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardFightType, out num))
            {
                num++;
                dict_FightType[tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardFightType] = num;
            }
            else
            {
                dict_FightType.Add(tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardFightType, 1);
            }
        }
        if(dict_FightType.Count == Data_Pool.m_TeamPool.f_GetAll().Count)
        {
            dict_FightType.Clear();
            dict_FightType.Add(0, Data_Pool.m_TeamPool.f_GetAll().Count);
        }
        var temp = dict_FightType.OrderBy(key => key.Key);
        var dic = temp.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        dict_FightType = dic;
    }
    public static bool f_CheckNextLevelAuraType(EM_FormationPos needReplacePos, CardPoolDT cardin)
    {
        if (Data_Pool.m_TeamPool.f_GetAll().Count >= (int)needReplacePos + 1)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[(int)needReplacePos];
            if (tTeamPoolDT != null)
            {
                if (tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardFightType == cardin.m_CardDT.iCardFightType) return false;
            }
        }
        Dictionary<int, int> dict_AuraType = new Dictionary<int, int>();
        GetDictAuraType(ref dict_AuraType);
        int levelNew = 0;
        int typeNew = cardin.m_CardDT.iCardFightType;
        if (dict_AuraType.TryGetValue(cardin.m_CardDT.iCardFightType, out levelNew))
        {
            return true;
        }
        return false;
    }

    public static List<AuraFiveElementsDT> GetConfigAuraFiveEleByType(List<NBaseSCDT> listData, int type)
    {
        List<AuraFiveElementsDT> auraFiveElementDTs = new List<AuraFiveElementsDT>();
        for (int i = 0; i < listData.Count; i++)
        {
            AuraFiveElementsDT auraFiveElementDT = listData[i] as AuraFiveElementsDT;
            if (auraFiveElementDT.iType == type)
            {
                auraFiveElementDTs.Add(auraFiveElementDT);
            }
        }
        return auraFiveElementDTs;
    }
    public static void GetIconAndLevelAuraFiveEle(ref string icon, ref string lbLevel)
    {
        Dictionary<int, int> dict_AuraFiveEle = new Dictionary<int, int>();
        GetDictAuraFiveEle(ref dict_AuraFiveEle);
        for (int i = 0; i < dict_AuraFiveEle.Count; i++)
        {
            int type = dict_AuraFiveEle.ElementAt(i).Key;
            int level = dict_AuraFiveEle.ElementAt(i).Value;
            if (level <= 2) continue;
            if (icon == "")
            {
                icon += "IconEle_" + type;
            }
            else
            {
                icon += "_" + type;
            }
            if (lbLevel == "")
            {
                lbLevel += "" + level;
            }
            else
            {
                lbLevel += "/" + level;
            }
        }
    }
    /// <summary>
    /// Lấy danh sách số lượng nguyên tố kích hoạt được ở trận hình 
    /// </summary>
    public static void GetDictAuraFiveEle(ref Dictionary<int, int> dict_AuraFiveEle)
    {
        for (int i = 0; i < Data_Pool.m_TeamPool.f_GetAll().Count; i++)
        {
            TeamPoolDT tTeamPoolDT = (TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[i];
            if (tTeamPoolDT == null) continue;
            int num = 0;
            if (dict_AuraFiveEle.TryGetValue(tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardEle, out num))
            {
                num++;
                dict_AuraFiveEle[tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardEle] = num;
            }
            else
            {
                dict_AuraFiveEle.Add(tTeamPoolDT.m_CardPoolDT.m_CardDT.iCardEle, 1);
            }
        }
        
        var temp = dict_AuraFiveEle.OrderBy(key => key.Key);
        var dic = temp.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        dict_AuraFiveEle = dic;
    }
    //end maco
    public static RoleProperty f_DispGodEquip(ref RoleProperty _RoleProperty, GodEquipPoolDT tEquipPoolDT)
    {
        if (tEquipPoolDT != null && tEquipPoolDT.m_iTempleteId > 0)
        {
            //强化 (基础值 + 强化等级*(强化属性Id 强化初始值))
            _RoleProperty.f_AddProperty(tEquipPoolDT.m_EquipDT.iIntenProId, CalculatePropertyStartLv1(tEquipPoolDT.m_EquipDT.iStartPro, tEquipPoolDT.m_EquipDT.iAddPro, tEquipPoolDT.m_lvIntensify));
            //精炼 (精炼等级*(精炼属性Id1 每级属性值1)   精炼等级*(精炼属性Id2 每级属性值2)) iRefinProId1	iRefinPro1	iRefinProId2	iRefinPro2
            _RoleProperty.f_AddProperty(tEquipPoolDT.m_EquipDT.iRefinProId1, CalculatePropertyStartLv0(0, tEquipPoolDT.m_EquipDT.iRefinPro1, tEquipPoolDT.m_lvRefine));
            _RoleProperty.f_AddProperty(tEquipPoolDT.m_EquipDT.iRefinProId2, CalculatePropertyStartLv0(0, tEquipPoolDT.m_EquipDT.iRefinPro2, tEquipPoolDT.m_lvRefine));



            GodEquipUpStarDT tUpstar;
            if (tEquipPoolDT.m_sexpStars > 0)
            {
                tUpstar = glo_Main.GetInstance().m_SC_Pool.m_GodEquipUpStarSC.f_GetSC(tEquipPoolDT.m_EquipDT.iId * 100 + tEquipPoolDT.m_sstars + 1) as GodEquipUpStarDT;
                if (tUpstar != null)
                    _RoleProperty.f_AddProperty((int)tUpstar.iProId, Convert.ToInt32(((float)tEquipPoolDT.m_sexpStars / (float)tUpstar.iUpExp) * tUpstar.iAddPro));
            }
            for (int j = 1; j <= tEquipPoolDT.m_sstars; j++)
            {
                tUpstar = glo_Main.GetInstance().m_SC_Pool.m_GodEquipUpStarSC.f_GetSC(tEquipPoolDT.m_EquipDT.iId * 100 + j) as GodEquipUpStarDT;
                if (tUpstar != null)
                    _RoleProperty.f_AddProperty((int)tUpstar.iProId, tUpstar.iAddNum + tUpstar.iAddPro);
            }
        }
        return _RoleProperty;
    }

    public static void f_GetAllGodEquipProperty(List<GodEquipPoolDT> aEquipPoolDT, ref RoleProperty tRoleProperty)
    {
        // --- Thuộc tính trang bị (a, cường hóa b, tinh luyện c, phù đồ d, chủ nhân tăng sao)
        // Cường hóa (giá trị cơ bản + mức cường hóa * (tăng cường Id thuộc tính để tăng cường giá trị ban đầu))
        // Tinh chỉnh (tinh chỉnh cấp độ * (tinh chỉnh thuộc tính Id1 trên mỗi cấp độ giá trị thuộc tính 1) cấp độ tinh luyện * (tinh chỉnh thuộc tính Id2 mỗi cấp độ giá trị thuộc tính 2))
        // Master (cấp trang bị thuộc tính bổ sung ID1 thuộc tính ID2 thuộc tính ID3 thuộc tính ID4 thuộc tính ID5) thêm trực tiếp
        //Bộ
        if (aEquipPoolDT != null)
        {
            bool isInvalid = false; //Nó không hợp lệ(4 trang bị không có sản phẩm nào)
            int IntensifyLevel = -1;//Cấp độ cường hóa thấp nhất trong số 4 mảnh trang bị
            int RefineLevel = -1;//Mức độ tinh chỉnh thấp nhất trong số 4 thiết bị
            for (int i = 0; i < aEquipPoolDT.Count; i++)
            {
                if (aEquipPoolDT[i] != null)
                {
                    f_DispGodEquip(ref tRoleProperty, aEquipPoolDT[i]);
                    if (IntensifyLevel == -1 || aEquipPoolDT[i].m_lvIntensify < IntensifyLevel)
                        IntensifyLevel = aEquipPoolDT[i].m_lvIntensify;
                    if (RefineLevel == -1 || aEquipPoolDT[i].m_lvRefine < RefineLevel)
                        RefineLevel = aEquipPoolDT[i].m_lvRefine;
                }
                else
                {
                    isInvalid = true;
                }
            }
        }
    }
    /// <summary>
    /// Lấy nguyên tố của mùa
    /// </summary>
    public static int f_GetElementalSeason()
    {
        ElementalSeasonDT mData = glo_Main.GetInstance().m_SC_Pool.m_ElementalSeasonSC.f_GetSC(1) as ElementalSeasonDT;
        Int32 timeDefault = mData.iTimeDefault;//1695747599;
        Int32 unixTimestamp = GameSocket.GetInstance().f_GetServerTime();
        int tempTime = unixTimestamp - timeDefault;
        int runDay = Mathf.CeilToInt(tempTime / 86400f);
        int fiveEle = Mathf.CeilToInt(runDay / (float)mData.iDay) % 5;
        if (fiveEle == 0) fiveEle = 5;
        return fiveEle;

    }
    /// <summary>
    /// chỉ số mùa nguyên tố tăng
    /// </summary>
    public static void f_GetElementalSeasonProperty(CardPoolDT card, ref RoleProperty tElementalSeasonProperty)
    {

        int fiveEle = f_GetElementalSeason();
        if (card.m_CardDT.iCardEle != fiveEle) return;
        ElementalSeasonDT mData = glo_Main.GetInstance().m_SC_Pool.m_ElementalSeasonSC.f_GetSC(1) as ElementalSeasonDT;
        // tăng 20% 4 chỉ số cơ bản
        //tRoleProperty.f_UpdateProperty((int)EM_RoleProperty.Atk, Convert.ToInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.Atk) * 1.2f));
        //tRoleProperty.f_UpdateHp((long)(Convert.ToInt32(tRoleProperty.f_GetHp() * 1.2f)));
        //tRoleProperty.f_UpdateProperty((int)EM_RoleProperty.Def, Convert.ToInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.Def) * 1.2f));
        //tRoleProperty.f_UpdateProperty((int)EM_RoleProperty.MDef, Convert.ToInt32(tRoleProperty.f_GetProperty((int)EM_RoleProperty.MDef) * 1.2f));
        tElementalSeasonProperty.f_AddProperty((int)EM_RoleProperty.AddHp, mData.iAddHp);
        tElementalSeasonProperty.f_AddProperty((int)EM_RoleProperty.AddAtk, mData.iAddAtk);
        tElementalSeasonProperty.f_AddProperty((int)EM_RoleProperty.AddDef, mData.iAddDef);
        tElementalSeasonProperty.f_AddProperty((int)EM_RoleProperty.AddMDef, mData.iAddMDef);
    }


    public static void GetDictAuraCamp(List<RoleControl> m_aTeam,ref Dictionary<int, int> dict_AuraCamp)
    {
        for (int i = 0; i < m_aTeam.Count; i++)
        {
            RoleControl rs = (RoleControl)m_aTeam[i];
            if (rs == null) continue;
            int num = 0;
            if (rs._mRoleControlDT.m_CardDT.GetType() == typeof(CardDT))
            {
                CardDT m_CardDT = (CardDT)rs._mRoleControlDT.m_CardDT;
                if (dict_AuraCamp.TryGetValue(m_CardDT.iCardCamp, out num))
                {
                    num++;
                    dict_AuraCamp[m_CardDT.iCardCamp] = num;
                }
                else
                {
                    dict_AuraCamp.Add(m_CardDT.iCardCamp, 1);
                }
            }
            else
            {
                MonsterDT m_CardDT = (MonsterDT)rs._mRoleControlDT.m_CardDT;
                if (dict_AuraCamp.TryGetValue(m_CardDT.iCardCamp, out num))
                {
                    num++;
                    dict_AuraCamp[m_CardDT.iCardCamp] = num;
                }
                else
                {
                    dict_AuraCamp.Add(m_CardDT.iCardCamp, 1);
                }
            }
            
        }
        var temp = dict_AuraCamp.OrderBy(key => key.Key);
        var dic = temp.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        dict_AuraCamp = dic;
    }
    public static void GetDictAuraType(List<RoleControl> m_aTeam, ref Dictionary<int, int> dict_FightType)
    {
        for (int i = 0; i < m_aTeam.Count; i++)
        {
            RoleControl rs = (RoleControl)m_aTeam[i];
            if (rs == null) continue;
            int num = 0;
            if (rs._mRoleControlDT.m_CardDT.GetType() == typeof(CardDT))
            {
                CardDT m_CardDT = (CardDT)rs._mRoleControlDT.m_CardDT;
                if (dict_FightType.TryGetValue(m_CardDT.iCardFightType, out num))
                {
                    num++;
                    dict_FightType[m_CardDT.iCardFightType] = num;
                }
                else
                {
                    dict_FightType.Add(m_CardDT.iCardFightType, 1);
                }
            }
            else
            {
                MonsterDT m_CardDT = (MonsterDT)rs._mRoleControlDT.m_CardDT;
                if (dict_FightType.TryGetValue(m_CardDT.iCardFightType, out num))
                {
                    num++;
                    dict_FightType[m_CardDT.iCardFightType] = num;
                }
                else
                {
                    dict_FightType.Add(m_CardDT.iCardFightType, 1);
                }
            }
                
        }
        if (dict_FightType.Count == Data_Pool.m_TeamPool.f_GetAll().Count)
        {
            dict_FightType.Clear();
            dict_FightType.Add(0, Data_Pool.m_TeamPool.f_GetAll().Count);
        }
        var temp = dict_FightType.OrderBy(key => key.Key);
        var dic = temp.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        dict_FightType = dic;
    }
    public static void GetDictAuraFiveEle(List<RoleControl> m_aTeam, ref Dictionary<int, int> dict_AuraFiveEle)
    {
        for (int i = 0; i < m_aTeam.Count; i++)
        {
            RoleControl rs = (RoleControl)m_aTeam[i];
            if (rs == null) continue;
            int num = 0;
            if (rs._mRoleControlDT.m_CardDT.GetType() == typeof(CardDT))
            {
                CardDT m_CardDT = (CardDT)rs._mRoleControlDT.m_CardDT;
                if (dict_AuraFiveEle.TryGetValue(m_CardDT.iCardEle, out num))
                {
                    num++;
                    dict_AuraFiveEle[m_CardDT.iCardEle] = num;
                }
                else
                {
                    dict_AuraFiveEle.Add(m_CardDT.iCardEle, 1);
                }
            }
            else
            {
                MonsterDT m_CardDT = (MonsterDT)rs._mRoleControlDT.m_CardDT;
                if (dict_AuraFiveEle.TryGetValue(m_CardDT.iCardEle, out num))
                {
                    num++;
                    dict_AuraFiveEle[m_CardDT.iCardEle] = num;
                }
                else
                {
                    dict_AuraFiveEle.Add(m_CardDT.iCardEle, 1);
                }
            }
        }

        var temp = dict_AuraFiveEle.OrderBy(key => key.Key);
        var dic = temp.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        dict_AuraFiveEle = dic;
    }
}