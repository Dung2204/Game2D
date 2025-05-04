using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

/// <summary>
/// 玩家已解锁的角色池
/// </summary>
public class CharacterPool  : BasePool
{

    public CharacterPool()
        : base("CharacterPoolDT", true)  
    {

    }


    protected override void f_Init()
    {
    }

    protected override void RegSocketMessage()
    {
        sCharacter tData = new sCharacter();
        //GameSocket.GetInstance().f_RegMessage_Int0(SocketCommand.DATA_GTC_UpdateCharacter, tData, Callback_SocketData_Update);
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        //sCharacter tServerData = (sCharacter)Obj;
        //CharacterPoolDT tPoolDataDT = new CharacterPoolDT();
        //tPoolDataDT.iId = tServerData.iId;
        //tPoolDataDT.m_iExp = tServerData.m_iExp;
        ////tPoolDataDT.m_iCharacterId = tServerData.m_iCharacterId;
        ////tPoolDataDT.m_iTempleteId = tServerData.m_iTempleteId;
        ////tPoolDataDT.m_iCityId = tServerData.m_iCityId;
        //f_Save(tPoolDataDT);
    }


    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        //sCharacter tServerData = (sCharacter)Obj;
        //CharacterPoolDT tPoolDataDT = (CharacterPoolDT)f_GetForId(tServerData.iId);

        //if (tPoolDataDT == null)
        //{
        //    MessageBox.ASSERT("无此角色的资料，更新失败");
        //}
        //tPoolDataDT.iId = tServerData.iId;
        //tPoolDataDT.m_iExp = tServerData.m_iExp;
        ////tPoolDataDT.m_iCharacterId = tServerData.m_iCharacterId;
        ////tPoolDataDT.m_iTempleteId = tServerData.m_iTempleteId;
        ////tPoolDataDT.m_iCityId = tServerData.m_iCityId;

    }

    //protected override void f_Socket_DelData(SockBaseDT Obj)
    //{
    //    sCharacter tServerData = (sCharacter)Obj;
    //    f_Delete(tServerData.iId);
    //}

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //以下为外部调用接口
    
    public CharacterPoolDT f_GetReadyCreateCharacterDT()
    {
       
        return null;
    }
    






}
