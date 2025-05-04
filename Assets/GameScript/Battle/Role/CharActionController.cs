using UnityEngine;
using System.Collections;
using Spine.Unity;
using System.Collections.Generic;
using ccU3DEngine;
using Spine;
using System;
//using DG.Tweening;

public class CharActionController : MonoPauseRun
{
    public GameObject m_oRoleSpine;

    //private Dictionary<string, Bone> _aBone = new Dictionary<string, Bone>();
    private ccCallback _Callback_Event = null;    
    SkeletonAnimation skeletonAnimation;
    SkeletonRenderer _SkeletonRenderer;
    MeshRenderer _MeshRenderer;
    AnimationStateData _AnimationStateData;
    private TrackEntry _TrackEntry;
    private int shakeScreenState = 0;  //震屏状态，0 没有震屏 1 横向震屏 2 纵向震屏，用以处理美术缺少震屏关闭事件以及报错用

    private int _iId;

    private float _fHeight;

    private float _timeScale;
    public float timeScale
    {
        set
        {
            _timeScale = value;
            if (skeletonAnimation == null)
            {
                skeletonAnimation = GetComponent<SkeletonAnimation>();
            }
            skeletonAnimation.timeScale = _timeScale;
        }
        get
        {
            return _timeScale;
        }


    }
        
    //=====================================================================================================================================
    public void f_Init(GameObject oRoleSpine, ccCallback Callback_Event)
    {
        m_oRoleSpine = oRoleSpine;
        _Callback_Event = Callback_Event;
        //aniChar = oRoleSpine.GetComponent<Animator>();
        skeletonAnimation = oRoleSpine.GetComponent<SkeletonAnimation>();
        _SkeletonRenderer = (SkeletonRenderer)oRoleSpine.transform.GetComponent<SkeletonRenderer>();
        _MeshRenderer = (MeshRenderer)oRoleSpine.transform.GetComponent<MeshRenderer>();
        skeletonAnimation.state.Event += onCompleteEvent;

        _fHeight = (float)skeletonAnimation.skeleton.Data.Height / 100;
        f_SetDepthForOther(0);
        f_SetAlpha(1);

        _iId = ccMath.f_CreateKeyId();
    }

    public float f_GetHeight()
    {
        return GloData.glo_fHpHeight;
    }
    
    private void Reset()
    {
        //skeletonAnimation.skeleton.SetToSetupPose();
        //return;

        _SkeletonRenderer.Initialize(true);
        skeletonAnimation.state.Event += onCompleteEvent;
    }


    private bool CheckIsPlayAnimation(string ppSQL)
    {
        if (skeletonAnimation.state.GetCurrent(0) == null)
        {
            return false;
        }
        if (skeletonAnimation.state.GetCurrent(0).Animation.Name == ppSQL)
        {
            return true;
        }
        return false;
    }

    public void f_PlayDead(int depth)
    {
        if (!CheckIsPlayAnimation("Dead"))	
        {	
            Reset();	
            _TrackEntry = skeletonAnimation.state.SetAnimation(0, "Dead", false);	
        }	
		// My Code	
		GameObject charMagic = null;	
		Transform magicPoint = m_oRoleSpine.transform;	
		int d = 50 + depth;	
		// UITool.f_CreateMagicById(20032, ref charMagic, magicPoint, d, "animation", null, false, 1f);	
		//	
    }

    public void f_PlayMagic()
    {
        Reset();
        _TrackEntry = skeletonAnimation.state.SetAnimation(0, "animation", false);
        int iSpeed = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1);
        skeletonAnimation.state.TimeScale = iSpeed * (float)BattleManager.speedRate;
    }

    public void f_PlaySkill(int iMagicIndex)
    {
        shakeScreenState = 0;
        if (iMagicIndex == 0)
        {
            PlaySkill_1();
        }
        else if (iMagicIndex == 1)
        {
            PlaySkill_2();
        }
        else if(iMagicIndex == 2)
        {
            PlaySkill_3();
        }
        else if(iMagicIndex == 3)
        {
            PlaySkill_4();
        }
        else
        {
MessageBox.ASSERT("Skill code does not exist" + iMagicIndex);
        }
    }
	
	public float GetAnimationTimeByMagicIndex(int magicIndex) {
        try
        {
            return skeletonAnimation.SkeletonDataAsset.GetSkeletonData(true).FindAnimation("Skill_" + (magicIndex + 1)).duration;
        }
        catch (Exception ex){}finally {  }
        return 1f;
    }

    public void f_PlayBeAttack()
    {
        Reset();
        _TrackEntry = skeletonAnimation.state.SetAnimation(0, "Behit", false);
        _TrackEntry = skeletonAnimation.state.AddAnimation(0, "Stand", true, 0);
        skeletonAnimation.state.TimeScale = 1 / (float)BattleManager.speedRate;
    }

    private void PlaySkill_1()
    {
        if (!CheckIsPlayAnimation("Skill_1"))
        {
            Reset();
            _TrackEntry = skeletonAnimation.state.SetAnimation(0, "Skill_1", false);
        }
    }
    private void PlaySkill_2()
    {
        if (!CheckIsPlayAnimation("Skill_2"))
        {
            Reset();
            _TrackEntry = skeletonAnimation.state.SetAnimation(0, "Skill_2", false);
            //skeletonAnimation.state.AddAnimation(0, "Stand", true, _TrackEntry.endTime);
        }
    }
    private void PlaySkill_3()
    {
        if (!CheckIsPlayAnimation("Skill_3"))
        {
            Reset();
            _TrackEntry = skeletonAnimation.state.SetAnimation(0, "Skill_3", false);
            //skeletonAnimation.state.AddAnimation(0, "Stand", true, _TrackEntry.endTime);
        }
    }
    private void PlaySkill_4()
    {
        if (!CheckIsPlayAnimation("Skill_4"))
        {
            Reset();
            _TrackEntry = skeletonAnimation.state.SetAnimation(0, "Skill_4", false);
        }
    }

    public void f_PlayStand()
    {

        if (!CheckIsPlayAnimation("Stand"))
        {
            Reset();
            _TrackEntry = skeletonAnimation.state.SetAnimation(0, "Stand", true);
            skeletonAnimation.state.TimeScale = 1 / (float)BattleManager.speedRate;
        }
    }

    public void f_PlayWalk(float fSpeed)
    {
        if (!CheckIsPlayAnimation("Run"))
        {
            Reset();
            var walk = skeletonAnimation.AnimationState.Data.skeletonData.Animations.Find(o => o.Name == "Run");
            _TrackEntry = skeletonAnimation.state.SetAnimation(0, walk!=null? "Run":"Stand", true);
            _TrackEntry.TimeScale = fSpeed/2;
			// MessageBox.ASSERT("Walk: " + walk);
        }
    }

    public void f_PlayWin()
    {
        if (!CheckIsPlayAnimation("Behit"))
        {
            Reset();
            _TrackEntry = skeletonAnimation.state.SetAnimation(0, "Behit", true);
        }
    }
	
	public void SetUnScaledTime(bool isUnscaled) {
        if (skeletonAnimation) { 
            skeletonAnimation.unscaledTime = isUnscaled;
        }
    }
    
    void onCompleteEvent(TrackEntry trackEntry, Spine.Event e)
    {

        //OnCreateMagicBall 产生弹道
        //OnCreateMagicHarm 产生伤害
        //OnPlaySound 播放声音
        //Skill_Complete 攻击结束
        //Dead_Complete 死亡结束
        //OnOpenBg 打开背景
        //OnCloseBg 关闭背景

        switch (e.Data.Name)
        {
            case "OnPlaySound":
                if (!string.IsNullOrEmpty(e.String))
                {
                    e.String = e.String.Replace("\n", "");
                    int speed = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1);
                    glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMagic(e.String, speed * (float)BattleManager.speedRate);
                }
                else
                {
                    _Callback_Event("OnPlaySound");
                }
                break;           
            case "OnCreateMagicBall":
                OnCreateMagicBall(e.Int);      //OnMagicAttack(e.Data.Int);
                break;
            case "OnCreateMagicHarm":
                OnCreateMagicHarm(e.Int);      //OnMagicAttack(e.Data.Int);
                break;
            case "Skill_Complete":
                if (shakeScreenState == 1)
                {
MessageBox.ASSERT("The horizontal screen vibration has not been turned off, is off，model name：" + m_oRoleSpine.name);
                    shakeScreenState = 0;
                    glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BattlePopCloseW, m_oRoleSpine);
                }
                else if (shakeScreenState == 2)
                {
MessageBox.ASSERT("Vertical screen vibration is not disabled, is off，model name:" + m_oRoleSpine.name);
                    shakeScreenState = 0;
                    glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BattlePopCloseH, m_oRoleSpine);
                }
                    OnPlayComplete(null);
                break;
            case "OnOpenBg":
                _Callback_Event("OnOpenBg");
                break;
            case "OnCloseBg":
                _Callback_Event("OnCloseBg");
                break;

            case "PopOpenW":
                shakeScreenState = 1;
                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BattlePopOpenW, m_oRoleSpine);
                break;
            case "PopCloseW":
                shakeScreenState = 0;
                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BattlePopCloseW, m_oRoleSpine);
                break;
            case "PopOpenH":
                shakeScreenState = 2;
                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BattlePopOpenH, m_oRoleSpine);
                break;
            case "PopCloseH":
                shakeScreenState = 0;
                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BattlePopCloseH, m_oRoleSpine);
                break;

                



                //case "Dead_Complete":
                //    OnDeadComplete(null);
                //    break;
        }
    }
    
    /// <summary>
    /// 产生弹道(1,2,3,4,5)
    /// </summary>
    /// <param name="tstEventData"></param>
    void OnCreateMagicHarm(int iTime)           //AnimationEvent tstEventData)
    {
        _Callback_Event("OnCreateMagicHarm");
    }
    
    /// <summary>
    /// 产生弹道(1,2,3,4,5)
    /// </summary>
    /// <param name="tstEventData"></param>
    void OnCreateMagicBall(int iTime)           //AnimationEvent tstEventData)
    {
        _Callback_Event("OnCreateMagicBall");
    }
    
    void OnPlayComplete(AnimationEvent tstEventData)
    {
        _Callback_Event("Skill_Complete");
    }

 
    ///// <summary>
    ///// 产生伤害
    ///// </summary>
    ///// <param name="iTime"></param>
    //void OnMagicAttack(int iTime)
    //{
    //    DispatchEvent(new FLEventBase("", MessageDef.MAGICATTACK, iTime));
    //}

    public void f_Pause()
    {
        OnPauseGame(null);
    }


    public void f_Resume()
    {
        OnResumeGame(null);
    }


    public override void OnPauseGame(object data)
    {
        //if (_Animator != null)
        //{
        //    _Animator.speed = 0;
        //}
        base.OnPauseGame(data);
    }


    public override void OnResumeGame(object data)
    {
        //if (_Animator != null)
        //{
        //    _Animator.speed = _fDefaultSpeed;
        //}
        base.OnResumeGame(data);
    }
    
    public void f_SetAlpha(float fAlpha)
    {
        skeletonAnimation.skeleton.A = fAlpha;

    }
	
	public void f_SetSkillDepth(bool isSkill)
    {
        _MeshRenderer.sortingLayerName = isSkill ? "SpecialSkill" : "Default";
    }

    public void f_SetDepthForAttack()
    {
        _MeshRenderer.sortingOrder = (int)EM_BattleDepth.Attack;
    }

    public void f_SetDepthForBeAttack(int iDepth)
    {
        _MeshRenderer.sortingOrder = (int)EM_BattleDepth.BeAttack + iDepth;
    }

    public void f_SetDepthForOther(int iDepth)
    {
        _MeshRenderer.sortingOrder = (int)EM_BattleDepth.Other + iDepth;
    }

    public int f_GetDepth()
    {
        return _MeshRenderer.sortingOrder;
    }
}
