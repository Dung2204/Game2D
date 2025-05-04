using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneBattle : MonoBehaviour {
    //单方阵营出战数量
    int MINENUM = 6;

    enum GameSteat
    {
        CREAT,//创角场景
        READY,//战斗准备
        START,//战斗开始
        END,//回合结束
        Over,//整场战斗结束
    }

    public List<Transform> positions;
    public List<Transform> mineAttackPoint;
    public List<Transform> enemyAttackPoint;
    public Camera camera;

    //我方对象
    Dictionary<int, SceneObj> _mineTrans;

    //地方对象
    Dictionary<int, SceneObj> _enemyTrans;

    //场景对象
    class SceneObj
    {
        public Transform _trans;
        public Animator _anim;
        public SceneObj(Transform transform, Animator animator)
        {
            _trans = transform;
            _anim = animator;
        }
    }

    //战斗信息
    class BattleInfo
    {
        public int camp;//阵营
        public int index;//序号
        public int attackpoint;//前往攻击点
        public List<int> targ;//目标列表
        public int skill;//技能ID

        public BattleInfo()
        {
            camp = 0;
            index = 0;
            attackpoint = 0;
            targ = new List<int>();
            skill = 0;
        }
    }
    //一回合战斗信息
    BattleInfo _battleInfo;

    //记录添加的动画事件
    Dictionary<int, List<string>> _recordAnimtorEvent;

    //战斗阶段
    GameSteat gameSteat = GameSteat.CREAT;

    public void Awake()
    {
        _mineTrans = new Dictionary<int, SceneObj>();
        _enemyTrans = new Dictionary<int, SceneObj>();
        _recordAnimtorEvent = new Dictionary<int, List<string>>();
        _battleInfo = new BattleInfo();
        SetBattleScene(1, "2004001;0;2004001;0;2004001;2004001;2004001;2004001;2004001;2004001;2004001;2004001");
        gameSteat = GameSteat.READY;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            List<int> list = new List<int>() {0,1,2,3,4,5};
            SetAttck(1, 1, 1, list, 1);
        }
    }

    #region 初始化战斗场景
    /// <summary>
    /// 初始化场景和角色
    /// </summary>
    /// <param name="_scene">场景ID</param>
    /// <param name="_data">角色数据  用分号隔开</param>
    public void SetBattleScene(int _scene, string _data)
    {
        #region 根据场景ID读取表格设定模型坐标

        #endregion

        #region 加载角色模型
        string[] _modelID = _data.Split(';');
        if (_data.Length < MINENUM * 2)
        {
Debug.LogError("Data Error");
            return;
        }
        int modelID;
        Transform loadtrans;
        Animator loadAnim;
        //加载我方角色
        for (int i = 0; i < MINENUM; i++)
        {
            modelID = int.Parse(_modelID[i]);
            if (modelID > 0)
            {
                loadtrans = LoadModel(modelID);
                if (loadtrans != null)
                {
                    loadAnim = loadtrans.GetComponent<Animator>();
                    SceneObj cenceObj = new SceneObj(loadtrans, loadAnim);
                    _mineTrans[i] = cenceObj;
                    loadtrans.parent = positions[i];
                    loadtrans.localPosition = Vector3.zero;
                    loadtrans.localEulerAngles = Vector3.zero;
                    SetAnimatorEvent(loadAnim, modelID);
                }
            }
        }

        //加载敌方角色
        for(int i = 0; i < MINENUM; i++)
        {
            modelID = int.Parse(_modelID[i + MINENUM]);
            if (modelID > 0)
            {
                loadtrans = LoadModel(modelID);
                if (loadtrans != null)
                {
                    loadAnim = loadtrans.GetComponent<Animator>();
                    SceneObj cenceObj = new SceneObj(loadtrans, loadAnim);
                    _enemyTrans[i] = cenceObj;
                    loadtrans.parent = positions[i + MINENUM];
                    loadtrans.localPosition = Vector3.zero;
                    loadtrans.localEulerAngles = Vector3.zero;
                    SetAnimatorEvent(loadAnim, modelID);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 添加动画事件
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="id"></param>
    void SetAnimatorEvent(Animator anim, int id)
    {
        if (_recordAnimtorEvent.ContainsKey(id))
            return;

        _recordAnimtorEvent[id] = new List<string>();

        //通过表格获取需要添加的动画事件  
        Dictionary<string, float> eventList = new Dictionary<string, float>();

        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            if (string.Equals(clips[i].name, "attack1"))
            {
                //创建动画事件
                AnimationEvent endevent = new AnimationEvent();
                //设置事件回掉函数名字
                endevent.functionName = "AnimationEvnetFunction";
                //设置参数
                endevent.stringParameter = "end";
                //设置触发时间
                endevent.time = clips[i].length * 0.9f;
                
                clips[i].AddEvent(endevent);

                //创建动画事件
                AnimationEvent attackevent = new AnimationEvent();
                //设置事件回掉函数名字
                attackevent.functionName = "AnimationEvnetFunction";
                //设置参数
                attackevent.stringParameter = "attack";
                //设置触发时间
                attackevent.time = clips[i].length * 0.5f;
                
                clips[i].AddEvent(attackevent);
            }
        }
    }

    /// <summary>
    /// 加载模型
    /// </summary>
    /// <param name="modelID"></param>
    /// <returns></returns>
    Transform LoadModel(int modelID)
    {
        Transform trans;
        GameObject obj = Resources.Load(string.Format("Model/{0}/{1}", modelID, modelID)) as GameObject;
        if (obj != null)
        {
            AnimationEventFunc animation;
            trans = GameObject.Instantiate<GameObject>(obj).transform;
            animation = trans.gameObject.AddComponent<AnimationEventFunc>();
            animation.SetSceneBattle(this);
            return trans;
        }
        else
            return null;
    }
    #endregion

    #region 战斗流程
    /// <summary>
    /// 战斗流程
    /// </summary>
    /// <param name="camp">0.我方,1.敌方</param>
    /// <param name="index">序号</param>
    /// <param name="attackpoint">攻击坐标</param>
    /// <param name="targ">目标列表</param>
    /// <param name="skill">技能ID</param>
    void SetAttck(int camp, int index, int attackpoint, List<int> targ, int skill)
    {
        //切换到战斗阶段
        gameSteat = GameSteat.START;

        _battleInfo.camp = camp;
        _battleInfo.index = index;
        _battleInfo.attackpoint = attackpoint;
        _battleInfo.targ = targ;
        _battleInfo.skill = skill;

        //行动模型
        Transform trans = null;
        Animator anim = null;
        Vector3 pos = Vector3.zero;

        if (camp == 0)
        {
            if (_mineTrans.ContainsKey(index))
            {
                trans = _mineTrans[index]._trans;
                anim = _mineTrans[index]._anim;
                pos = mineAttackPoint[index].localPosition;
            }
        }
        else
        {
            if (_enemyTrans.ContainsKey(index))
            {
                trans = _enemyTrans[index]._trans;
                anim = _enemyTrans[index]._anim;
                pos = enemyAttackPoint[index].localPosition;
            }
        }

        if (trans != null)
        {
            if (attackpoint >= 0)
            {
                //前往攻击点
                anim.SetTrigger("move");
                trans.DOMove(pos, 0.3f).OnComplete(() => { PlaySkill(anim); });
            }
            else
            {
                PlaySkill(anim);
            }
        }
        else
        {
            EndAttack(anim);
        }
    }

    void PlaySkill(Animator anim)
    {
        anim.SetTrigger("attack");
    }
    
    void GoBack()
    {
        SceneObj obj;
        if (_battleInfo.camp == 0)
            obj = _mineTrans[_battleInfo.index];
        else
            obj = _enemyTrans[_battleInfo.index];
        if (obj != null)
        {
            obj._anim.SetTrigger("back");
            obj._trans.DOMove(positions[_battleInfo.index + _battleInfo.camp * 6].localPosition, 0.3f).OnComplete(() => { EndAttack(obj._anim); });
        }
    }

    void EndAttack(Animator anim)
    {
        anim.SetTrigger("idle");
Debug.Log("End of Round");
        gameSteat = GameSteat.END;
    }

    /// <summary>
    /// 动画事件回调
    /// </summary>
    /// <param name="str"></param>
    public void AnimationEvnetFunction(string str)
    {
        if(string.Equals(str,"attack"))
        {
            for (int i = 0; i < _battleInfo.targ.Count; i++)
            {
                if (_battleInfo.camp == 1)
                {
                    if (_mineTrans.ContainsKey(_battleInfo.targ[i]))
                        _mineTrans[_battleInfo.targ[i]]._anim.SetTrigger("hurt");
                }
                else
                {
                    if (_enemyTrans.ContainsKey(_battleInfo.targ[i]))
                        _enemyTrans[_battleInfo.targ[i]]._anim.SetTrigger("hurt");
                }
            }
        }
        else if(string.Equals(str, "end"))
        {
            GoBack();
        }
        Debug.Log("name = " + str);
    }
    #endregion
}
