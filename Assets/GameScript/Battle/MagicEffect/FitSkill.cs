using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using Spine;
using Spine.Unity;

public class FitSkill : UIFramwork
{
    private MagicEffect _MagicEffectS;
    private MagicDT _MagicDT;
    private FitSkillParam _FitSkillParam;
    private Skill2MvParam _Skill2MvParam;
    private UILabel skillName = null;

    private GameObject skill2RolePoint = null;
    private UISprite skill2SkillName = null;

    Dictionary<long, GameObject> dicSpine = new Dictionary<long, GameObject>();

    private Transform[] boneRoles = new Transform[3];
    private Transform boneSkillName = null;

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e != null && e is FitSkillParam)
        {
            _FitSkillParam = (FitSkillParam)e;
            _MagicDT = _FitSkillParam.m_MagicDT;
			CreateEffect();
        }
        else if (e != null && e is Skill2MvParam)
        {
            _Skill2MvParam = (Skill2MvParam)e;
            _MagicDT = _Skill2MvParam.m_MagicDT;
            CreateSkill2Mv();
        }
        else
        {
MessageBox.ASSERT("Error DT skill input parameter");
        }
    }

    private SkeletonAnimation SkelSkill2Ani = null;
    public void CreateSkill2Mv()
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioDialog("skill_" + _MagicDT.iId, 1);
        if (dicSpine.ContainsKey((int)EM_MagicId.eFitSkill))
        {
            dicSpine[(int)EM_MagicId.eFitSkill].gameObject.SetActive(false);
        }

        GameObject parent = f_GetObject("EffectParent");

        float rotation = Data_Pool.m_CardPool.f_GetForId(_Skill2MvParam.m_Spine.m_iId) == null ? 1 : -1;
        if (dicSpine.ContainsKey((int)EM_MagicId.eSkill2Mv))
        {
            dicSpine[(int)EM_MagicId.eSkill2Mv].gameObject.SetActive(true);
            SkelSkill2Ani.state.SetAnimation(0, "animation", false);
        }
        else
        {
            GameObject oGo = null;
            SkelSkill2Ani = UITool.f_CreateMagicById((int)EM_MagicId.eSkill2Mv, ref oGo, parent.transform, 100, "animation", spineSkill2Complete);
            if (oGo != null)
                dicSpine[(int)EM_MagicId.eSkill2Mv] = oGo;
        }
        if (SkelSkill2Ani == null)
        {
            if (_Skill2MvParam.m_Change != null)
            {
                _Skill2MvParam.m_Change(null);
            }
            return;
        }
        int iSpeed = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1);
        //SkelSkill2Ani.state.TimeScale = 1;//iSpeed * (float)BattleManager.speedRate;
        SkelSkill2Ani.state.TimeScale = 1*iSpeed * (float)BattleManager.speedRate;
        dicSpine[(int)EM_MagicId.eSkill2Mv].transform.localScale = new Vector3(100 * rotation, 100, 100);

        if (skill2RolePoint == null)
        {
            GameObject oGo = NGUITools.AddChild(dicSpine[(int)EM_MagicId.eSkill2Mv], f_GetObject("boneFollowerTemp"));
            BoneFollower bf = oGo.GetComponent<BoneFollower>();
            bf.skeletonRenderer = SkelSkill2Ani;
            bf.SetBone("jiaose1");
            oGo.SetActive(true);
            skill2RolePoint = NGUITools.AddChild(oGo, f_GetObject("RolePoint"));
            skill2RolePoint.SetActive(true);
			skill2RolePoint.transform.localPosition = new Vector3(-1.2f, 2.5f, 0);
            skill2RolePoint.transform.localScale = Vector3.one * 0.01f;
        }
        if (skill2SkillName == null)
        {
            GameObject oGo = NGUITools.AddChild(dicSpine[(int)EM_MagicId.eSkill2Mv], f_GetObject("boneFollowerTemp"));
            BoneFollower bf = oGo.GetComponent<BoneFollower>();
            bf.skeletonRenderer = SkelSkill2Ani;
            bf.SetBone("ziti");
            oGo.SetActive(true);
            UIPanel oPanel = NGUITools.AddChild<UIPanel>(oGo);
            oPanel.sortingOrder = 0;
            skill2SkillName = NGUITools.AddChild(oPanel.gameObject, f_GetObject("skillLabelName")).GetComponent<UISprite>();
            skill2SkillName.gameObject.SetActive(true);
        }

        skill2SkillName.spriteName = _MagicDT.iId.ToString();
        skill2SkillName.MakePixelPerfect();
        skill2SkillName.transform.localScale = new Vector3(0.01f*rotation, 0.01f, 0.01f);

        int id = _Skill2MvParam.m_Spine._mRoleControlDT.m_RoleModelDT.iModel;
        for (int i = 0; i < skill2RolePoint.transform.childCount; i++)
        {
            Transform role = skill2RolePoint.transform.GetChild(i);
            role.gameObject.SetActive(false);
        }

        if (dicSpine.ContainsKey(id))
        {
            dicSpine[id].transform.parent = skill2RolePoint.transform;
            dicSpine[id].transform.localPosition = Vector3.zero;
            dicSpine[id].SetActive(true);
            dicSpine[id].transform.localScale = Vector3.one * 400;
        }
        else
        {
            GameObject go = null;
			//Fix Code
			CardPoolDT tcardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(0);
			if( id != 10001 && id != 10011 )
			{
				UITool.f_CreateRoleByModeId(id, ref go, skill2RolePoint.transform, 503, false);
			}
			else
			{
				if (tcardPoolDT.m_FanshionableDressPoolDT != null)
				{
					int FashId = tcardPoolDT.m_FanshionableDressPoolDT.m_FashionableDressDT.iModel;
					UITool.f_CreateFashRoleByModeId(FashId, ref go, skill2RolePoint.transform, 503, false);
				}
				else
				{
					UITool.f_CreateRoleByModeId(id, ref go, skill2RolePoint.transform, 503, false);
				}
			}
            go.SetActive(true);
            dicSpine[id] = go;
            go.transform.localScale = Vector3.one * 400;
        }
    }

    void spineSkill2Complete(TrackEntry trackEntry)
    {
        if (dicSpine.ContainsKey((int)EM_MagicId.eSkill2Mv))
        {
            dicSpine[(int)EM_MagicId.eSkill2Mv].gameObject.SetActive(false);
            //skillName.text = "";
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FitSkillPage, UIMessageDef.UI_CLOSE);
        if (_Skill2MvParam.m_Change != null)
        {
            _Skill2MvParam.m_Change(null);
        }
    }

    void spineComplete(TrackEntry trackEntry)
    {
        if (dicSpine.ContainsKey((int)EM_MagicId.eFitSkill))
        {
            dicSpine[(int)EM_MagicId.eFitSkill].gameObject.SetActive(false);
            skillName.text = "";
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FitSkillPage, UIMessageDef.UI_CLOSE);
        if (_FitSkillParam.m_Change != null)
        {
            _FitSkillParam.m_Change(null);
        }
    }
    SkeletonAnimation SkeAni = null;
	SkeletonAnimation SkeAni2 = null;
    private void CreateEffect()
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioDialog("skill_" + _MagicDT.iId, 1);
		// MessageBox.ASSERT("id" + _MagicDT.iId);
        //glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_ACTIVE, false);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.FitMagic, 1, 0.3f);
        GameObject parent = f_GetObject("EffectParent");
        //My Code
		// GameObject Ani = GameObject.Find("Texture");
		// if(_MagicDT.iId == 130013 || _MagicDT.iId == 110013 || _MagicDT.iId == 110113 || _MagicDT.iId == 120013 || _MagicDT.iId == 140013 || _MagicDT.iId == 140113)
		// {
			// int iSpeed2 = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1);
			// int AniID = int.Parse(_MagicDT.iId + "14");
			// GameObject charMagic = null;
			// Transform magicPoint = parent.transform.Find("Texture");
			// if(_MagicDT.iId == 110113 || _MagicDT.iId == 140013 || _MagicDT.iId == 140113 || _MagicDT.iId == 110013)
			// {
				// SkeAni2 = UITool.f_CreateMagicById(AniID, ref charMagic, magicPoint, 106, "skill", null, false, 0.37f);
			// }
			// else
			// {
				// SkeAni2 = UITool.f_CreateMagicById(AniID, ref charMagic, magicPoint, 106, "skill", null, false, 0.18f);
			// }
			// SkeAni2.state.TimeScale = 1*iSpeed2 * (float)BattleManager.speedRate;
		// }
		// else
		// {
			
		// }
		//
        float rotation = Data_Pool.m_CardPool.f_GetForId(_FitSkillParam.m_Spine.m_iId) == null ? 1 : -1;
        if (dicSpine.ContainsKey((int)EM_MagicId.eFitSkill))
        {
            dicSpine[(int)EM_MagicId.eFitSkill].gameObject.SetActive(true);
            SkeAni.state.SetAnimation(0, "animation", false);
        }
        else
        {
            GameObject oGo = null;
            SkeAni = UITool.f_CreateMagicById((int)EM_MagicId.eFitSkill, ref oGo, parent.transform, 100, "animation", spineComplete);
            dicSpine[(int) EM_MagicId.eFitSkill] = oGo;
        }

        if (SkeAni == null)
        {
            _FitSkillParam.m_Change(null);
            return;
        }
        int iSpeed = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1);
        //SkeAni.state.TimeScale = 1;//iSpeed * (float)BattleManager.speedRate;
        SkeAni.state.TimeScale = 1*iSpeed * (float)BattleManager.speedRate;
        dicSpine[(int)EM_MagicId.eFitSkill].transform.localScale = new Vector3(100 * rotation, 100, 100);

        for (int i = 0; i < boneRoles.Length; i++)
        {
            if (!boneRoles[i])
            {
                boneRoles[i] = NGUITools.AddChild(dicSpine[(int) EM_MagicId.eFitSkill], f_GetObject("boneFollowerTemp")).transform;
                BoneFollower bf = boneRoles[i].GetComponent<BoneFollower>();
                bf.skeletonRenderer = SkeAni;
                bf.SetBone("jiaose_a" + (i+1));
                boneRoles[i].gameObject.SetActive(true);
            }
        }

        if (!boneSkillName)
        {
            boneSkillName = NGUITools.AddChild(dicSpine[(int)EM_MagicId.eFitSkill], f_GetObject("boneFollowerTemp")).transform;
            BoneFollower bf = boneSkillName.GetComponent<BoneFollower>();
            bf.skeletonRenderer = SkeAni;
            bf.SetBone("ziti");
            boneSkillName.gameObject.SetActive(true);
        }

        if (!skillName)
        {
            skillName = NGUITools.AddChild(boneSkillName.gameObject, f_GetObject("skillName")).GetComponent<UILabel>();
            skillName.gameObject.SetActive(true);
        }
        skillName.transform.localScale = new Vector3(0.01f * rotation, 0.01f, 0.01f);
        skillName.text = _MagicDT.szName;

        int[] order = new[] {103, 102, 101};
		int[] ids;
		//My Code
		if(_MagicDT.iGroupHero3 != 0)
			ids = new[] {_MagicDT.iGroupHero3, _MagicDT.iGroupHero2, _MagicDT.iGroupHero1};
		else
			ids = new[] {_MagicDT.iGroupHero1, _MagicDT.iGroupHero2, _MagicDT.iGroupHero3};
		//
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < boneRoles[i].childCount; j++)
            {
                boneRoles[i].GetChild(j).gameObject.SetActive(false);
            }
            if (ids[i] != 0)
            {
                if (dicSpine.ContainsKey(ids[i]))
                {
                    dicSpine[ids[i]].transform.parent = boneRoles[i];
					if(ids[i] == 1201)
					{
						dicSpine[ids[i]].transform.localPosition = new Vector3(-1.35f, -1.8f, 0);
					}
					else
					{
						dicSpine[ids[i]].transform.localPosition = Vector3.zero;
					}
                    dicSpine[ids[i]].SetActive(true);
                    dicSpine[ids[i]].transform.localScale = Vector3.one * 1f;
                }
                else
                {
					GameObject go = null;
					UITool.f_CreateRoleByCardId(ids[i], ref go, boneRoles[i], order[i], 1f, true);
					// MessageBox.ASSERT("Run2: " + ids[i]);
					go.SetActive(true);
					dicSpine[ids[i]] = go;
                }
            }
        }
		//
        //_FitSkillParam.m_Change(null);

        //ccTimeEvent.GetInstance().f_RegEvent(2.1f, false, null, _FitSkillParam.m_Change);

        //        GameObject go = f_GetObject("FX_skill_Item");
        //        _MagicEffectS = go.GetComponent<MagicEffect>();
        //        _MagicEffectS.f_SetCardHead(_MagicDT);
        //        _MagicEffectS.f_SetCardPoint(_FitSkillParam.m_Spine.m_CharActionController.m_oRoleSpine);
        //        _MagicEffectS.f_SetEffectBgColor(_FitSkillParam.m_Spine.f_GetRoleAttack().m_iFitMagic);
        //        _MagicEffectS.f_SetMagicName(_MagicDT.szName);
        //        _MagicEffectS.f_SetChangeAttack(_FitSkillParam.m_Change);
        //        _MagicEffectS.f_SetRotation(Data_Pool.m_CardPool.f_GetForId(_FitSkillParam.m_Spine.m_iId) == null ? 0 : 180);
        //        go.SetActive(true);
    }
    protected override void UI_CLOSE(object e)
    {
        _NeedCloseSound = false;
        base.UI_CLOSE(e);
        //f_GetObject("FX_skill_Item").SetActive(false);
    }
}

public class FitSkillParam
{
    public MagicDT m_MagicDT;
    public RoleControl m_Spine;
    public ccCallback m_Change;
    public FitSkillParam(MagicDT tMagicDT, RoleControl tSpine, ccCallback Change)
    {
        m_MagicDT = tMagicDT;
        m_Spine = tSpine;
        m_Change = Change;
    }
}

public class Skill2MvParam
{
    public MagicDT m_MagicDT;
    public RoleControl m_Spine;
    public ccCallback m_Change;
    public Skill2MvParam(MagicDT tMagicDT, RoleControl tSpine, ccCallback Change)
    {
        m_MagicDT = tMagicDT;
        m_Spine = tSpine;
        m_Change = Change;
    }
}
