using UnityEngine;
using System.Collections.Generic;

public class CmdController
{
    public delegate void Callback();

    private Dictionary<string, Callback> _cmdDic;

    public CmdController()
    {
        _cmdDic = new Dictionary<string, Callback>();
        f_RegCmd(CMD_DIALOG_LIMIT_ON, f_Cmd_DialogLimitOn);
        f_RegCmd(CMD_DIALOG_LIMIT_OFF, f_Cmd_DialogLimitOff);
    }

    private void f_RegCmd(string cmd,Callback cmdProcess)
    {
        _cmdDic.Add(cmd, cmdProcess);
    }

    public bool f_CheckAndExecuteCmd(string cmd)
    {
        if (_cmdDic.ContainsKey(cmd))
        {
            _cmdDic[cmd]();
            return true;
        }
        return false;
    }

    #region 命令

    /// <summary>
    /// 剧情限制打开
    /// </summary>
    private const string CMD_DIALOG_LIMIT_ON = "DialogLimitOn";

    /// <summary>
    /// 剧情限制关闭
    /// </summary>
    private const string CMD_DIALOG_LIMIT_OFF = "DialogLimitOff";

    #endregion


    #region 命令处理函数

    private void f_Cmd_DialogLimitOn()
    {
        CmdParam.m_IgnoreDialogLimit = false;
    }
    
    private void f_Cmd_DialogLimitOff()
    {
        CmdParam.m_IgnoreDialogLimit = true;
    }
    
    #endregion



}
