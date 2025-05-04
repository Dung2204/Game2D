 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using MiGameGeneral;

public class GameMain : UIFramwork
{
    private static GameMain _Instance = null;
    private static int nLastFightPower;
    public static GameMain GetInstance()
    {
        if (!_Instance)
        {
            _Instance = (GameMain)FindObjectOfType(typeof(GameMain));
            if (!_Instance)
            {
                Debug.LogError("init BattleMain Fail");
            }
        }


        return _Instance;
    }
    
    void LinkFunction()
    {
        var controll = new linkerController();
        controll.linkFunction();
    }
    void Start()
    {
      
        //StaticValue.m_EM_GameStatic = EM_GameStatic.BATTLE;
        InitEvent();
        InitObj();
        InitPool();
        if (nLastFightPower <= 0)
            ccTimeEvent.GetInstance().f_RegEvent(0.8f, false, null, OnFightPowerChange);
    }

   
    private void InitEvent()
    {
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.PAUSEGAME, OnPauseGame, this);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.RESUMEGAME, OnResumeGame, this);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.PlayerFightPowerChange, OnFightPowerChange, this);
    }
    
    private void InitObj()
    {

    }
    private void InitPool()
    {
        
    }

    private void OnDestroy()
    {
        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.PlayerFightPowerChange);
    }
    //private void InitMessage()
    //{
    //    glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.PAUSEGAME, OnPauseGame, this);
    //    glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.RESUMEGAME, OnResumeGame, this);
    //    glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.SETWALKPOS, OnSetWalkPos, this);
    //    glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.MOVECAMERA, OnMoveCamera, this);
    //    glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.SETWALKPOSSUC, OnSetWalkPosSuc, this);

    //    //glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.UPDATEHEROMAGIC, OnUpdateHeroMagic, this);
    //    //glo_Main.GetInstance().m_UIMessagePool.f_AddListener(MessageDef.UPDATERELIVE, UpdateRelive, this);
    //}


    //private void UnInitMessage()
    //{
    //    glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.PAUSEGAME, OnPauseGame, this);
    //    glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.RESUMEGAME, OnResumeGame, this);
    //    glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.SETWALKPOS, OnSetWalkPos, this);
    //    glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.MOVECAMERA, OnMoveCamera, this);
    //    glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.SETWALKPOSSUC, OnSetWalkPosSuc, this);

    //    glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(MessageDef.NOOB_MOVETOPOSTIONSUCC, CameraLookAtListener, null);

    //    //glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.UPDATEHEROMAGIC, OnUpdateHeroMagic, this);
    //    //glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(MessageDef.UPDATERELIVE, UpdateRelive, this);

    //    m_BattleAI.f_UnInitMessage();

    //}

    public void f_Close()
    {
    }
    
    /// <summary>
    /// 正式开始战斗
    /// </summary>
    private void StartGame()
    {
        MessageBox.DEBUG("StartGame");
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.RESUMEGAME);

        //TestBuild();


    }


    //void Update()
    //{
    //    if (IsPaused())
    //    {
    //        return;
    //    }

    //}

    void FixedUpdate()
    {
        if (IsPaused())
        {
            return;
        }
        
    }
    
    //void LateUpdate()
    //{
        
    //}

    public void f_UpdateMap()
    {
        //_MapMain.f_UpdateMap();
    }

    /// <summary>
    /// 战斗力改变
    /// </summary>
    /// <param name="obj"></param>
    private void OnFightPowerChange(object obj)
    {
        //计算战斗力,direction > 0 表示这次战力明确上升，direction < 0表示这次战力明确下降
        int direction = null == obj ? 0 : (int)obj;
        int fightPower = Data_Pool.m_TeamPool.f_GetTotalBattlePower();
        if ((fightPower > nLastFightPower && direction < 0) || (fightPower < nLastFightPower) && direction > 0) return;

        //和上次一样则返回
        if (nLastFightPower <= 0 || nLastFightPower == fightPower)
        {
            nLastFightPower = fightPower;
            return;
        }

        //打开战斗力改变界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FightPowerChangePage, UIMessageDef.UI_OPEN, new FightPowerChangeParam(fightPower,nLastFightPower));

        //保存战斗力
        nLastFightPower = fightPower;
    }

}
