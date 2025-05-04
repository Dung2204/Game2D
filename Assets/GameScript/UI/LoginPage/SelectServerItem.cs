using UnityEngine;
using System.Collections;
/// <summary>
/// 选择服务器界面服务器item控制
/// </summary>
public class SelectServerItem : MonoBehaviour {
    public UISprite sprSerState;//服务器状态
    public UILabel labelSerName;//服务器名称
    public GameObject objNew;//新
    public GameObject objHot;//热
    public GameObject objMaintain;//维护
    public GameObject objUnhindered;//顺畅
    public GameObject objPreheat;//预热
    /// <summary>
    /// 设置服务器item数据
    /// </summary>
    /// <param name="serverStateSpriteName">服务器状态</param>
    /// <param name="serName">服务器名称</param>
    public void f_SetData(EM_ServerState emServerState, string serName)
    {
        labelSerName.text = serName;
        objNew.SetActive(emServerState == EM_ServerState.New);
        objHot.SetActive(emServerState == EM_ServerState.Hot);
        objMaintain.SetActive(emServerState == EM_ServerState.Maintain);
        objUnhindered.SetActive(emServerState == EM_ServerState.Unhindered);
        objPreheat.SetActive(emServerState == EM_ServerState.Preheat);
    }
}