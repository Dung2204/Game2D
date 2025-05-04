using ccU3DEngine;

/// <summary>
/// 军团玩家信息，暂时只有基础信息
/// </summary>
public class LegionPlayerPoolDT : BasePoolDT<long>
{
    public LegionPlayerPoolDT()
    {

    }

    private BasePlayerPoolDT _playerInfo;
    public BasePlayerPoolDT PlayerInfo
    {
        get
        {
            return _playerInfo;
        }
    }

    public void f_UpdatePlayerInfo(BasePlayerPoolDT playerInfo)
    {
        _playerInfo = playerInfo;
    }

}
