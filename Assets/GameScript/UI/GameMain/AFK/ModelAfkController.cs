using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
using Spine;
using Spine.Unity;
using DG.Tweening;
/// <summary>
/// 活动首充界面
/// </summary>
public class ModelAfkController : MonoBehaviour
{
    public enum ModelState
    {
        Stand,
        Behit,
        Attack,
        Dead,
        Run,
        Revive
    }

    public enum Team
    {
        A,
        B
    }
    public int _Hp;
    public ModelState _modelState;
    private SkeletonAnimation _skeletonAnimation;
    public ModelAfkController Target;
    public EM_CloseArrayPos _EM_CloseArrayPos;
    public Team _team;
    public Vector3 PawnPoint;
    public Transform BatlePoint;

    AFKInfoPage _aFKInfoPage;
    private int _damage = 1;
    public void InitControl(int Hp, EM_CloseArrayPos eM_CloseArrayPos, AFKInfoPage aFKInfoPage , int team)
    {
        _skeletonAnimation = transform.GetComponent<SkeletonAnimation>();
        _Hp = Hp;
        _EM_CloseArrayPos = eM_CloseArrayPos;
        _aFKInfoPage = aFKInfoPage;
        if (_skeletonAnimation == null) return;
        _team = (Team)team;
        PawnPoint = transform.position;
    }


    public void UpdateState(ModelState modelState)
    {
        _modelState = modelState;
        switch (_modelState)
        {
            case ModelState.Stand:
                //todo đứng yên chờ trận tiếp theo
                Stand();
                break;
            case ModelState.Behit:
                _skeletonAnimation.AnimationState.SetAnimation(1, "Behit", true);
                break;
            case ModelState.Attack:
                Attack();
                break;
            case ModelState.Dead:
                Dead();                         
                break;
            case ModelState.Run:
                break;
            case ModelState.Revive:
                //todo đứng yên chờ trận tiếp theo
                Revive();
                break;
            default:
                break;
        }

    }

    public void ChangeTarget(ModelAfkController target)
    {
        Target = target;
        UpdateState(ModelState.Attack);
    }

    public void MoveTo(Transform _transform)
    {


    }

    public void GoBatle(Transform _transform)
    {
        BatlePoint = _transform;
        _skeletonAnimation.AnimationState.SetAnimation(1, "Stand", true);
        _modelState = ModelState.Stand;
        if (_Hp <= 0)
        {
            UpdateState(ModelState.Dead);
            return;
        }
        switch (ccMath.f_GetRand(1, 3))
        {
            case 1:
                transform.DOJump(_transform.position, 0.2f, 1, 1);
                break;
            case 2:
                transform.DOMove(_transform.position, 1);
                break;
            default:
                break;
        }

    }

    public void SetEM_CloseArrayPos(EM_CloseArrayPos eM_CloseArrayPos)
    {
        _EM_CloseArrayPos = eM_CloseArrayPos;
    }

    float duration = 0;
    public bool callback = false;
    public void Attack()
    {
        if (_modelState != ModelState.Attack) return;
        if (_Hp <= 0)
        {
            _modelState = ModelState.Dead;
            return;
        }
        int rand = ccMath.f_GetRand(1, 101);
        if (1 <= rand && rand <= 100)
        {
            if(_skeletonAnimation.skeletonDataAsset.GetSkeletonData(true).FindAnimation("Skill_1") != null)
            {
                Spine.Animation animation = _skeletonAnimation.skeletonDataAsset.GetSkeletonData(true).FindAnimation("Skill_1");
                duration = animation.duration;
                _damage = 1;
                _skeletonAnimation.state.SetAnimation(1, "Skill_1", false);
            }
            else
            {
                return;
            }
        }
        else if (101 <= rand && rand <= 102)
        {
            if (_skeletonAnimation.skeletonDataAsset.GetSkeletonData(true).FindAnimation("Skill_2") != null)
            {
                Spine.Animation animation = _skeletonAnimation.skeletonDataAsset.GetSkeletonData(true).FindAnimation("Skill_2");
                duration = animation.duration;
                _damage = 2;
                _skeletonAnimation.state.SetAnimation(1, "Skill_2", false);
            }
            else
            {
                return;
            }
        }
        else
        {
            if (_skeletonAnimation.skeletonDataAsset.GetSkeletonData(true).FindAnimation("Skill_3") != null)
            {
                Spine.Animation animation = _skeletonAnimation.skeletonDataAsset.GetSkeletonData(true).FindAnimation("Skill_3");
                duration = animation.duration;
                _damage = 3;
                _skeletonAnimation.state.SetAnimation(1, "Skill_3", false);
            }
            else
            {
                return;
            }
        }
        callback = true;
    }

    private void Dead()
    {
        if (_modelState != ModelState.Dead) return;
        _skeletonAnimation.state.SetAnimation(0, "Behit", false);
        DOTween.To(() => _skeletonAnimation.skeleton.a, x => _skeletonAnimation.skeleton.a = x, 0, 1);
        transform.position = new Vector3(9999,9999,9999);
        //_aFKInfoPage.CheckEndBattle((int)_team);
    }

    private void Stand()
    {

    }

    private void Revive()
    {
        if (_modelState != ModelState.Revive) return;
        transform.position = PawnPoint;
        DOTween.To(() => _skeletonAnimation.skeleton.a, x => _skeletonAnimation.skeleton.a = x, 1, 1);
        _skeletonAnimation.state.SetAnimation(0, "Stand", false);
        GoBatle(BatlePoint);
        _aFKInfoPage.ReviveEnemy();
    }

    private void _TrackEntry_End()
    {
        if (Target == null) return;
        switch (_modelState)
        {
            case ModelState.Stand:

                break;
            case ModelState.Behit:
                break;
            case ModelState.Attack:
                //todo gây dame cho target
                if(Target != null)
                {
                    Target.BeAttack(_damage);
                    if (Target._Hp > 0)
                    {
                        UpdateState(ModelState.Attack);
                    }                  
                }
                else
                {
                    callback = false;
                    _aFKInfoPage.ChangeTarget(this);
                }
                //tiếp tục đánh

                break;
            case ModelState.Dead:
                //todo làm mờ model 
                

                break;
            case ModelState.Run:
                //todo đi từ map vào trung tâm
           
                break;
            default:
                break;
        }
    }

    public void BeAttack(int damage)
    {
        if (_Hp > 0)
            _Hp -= damage;
        if (_Hp <= 0)
        {
            if (_modelState == ModelState.Dead) return;
            _modelState = ModelState.Dead;
            UpdateState(_modelState);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            UpdateState(ModelState.Attack);
        }

        if (callback)
        {
            duration -= Time.deltaTime;
            if(duration <= 0)
            {
                callback = false;
                _TrackEntry_End();
            }
        }       
    }

    public void ModelRevive(int hp)
    {
        _Hp = hp;
        _modelState = ModelState.Revive;
        UpdateState(_modelState);
    }


}




