using UnityEngine;
using System.Collections;

/// <summary>
/// 无效代码入口01
/// </summary>
public class InvalidScriptEntry01
{
    InvalidGameWorld m_World;
    public InvalidScriptEntry01()
    {
        m_World = new InvalidGameWorld();
    }

    public void Init()
    {
        m_World.Init();
    }

    public void Update()
    {
        m_World.Update();
    } 
}
