using UnityEngine;
using System.Collections;

public class InvalidGameWorld
{
    private InvalidGameRole m_HeroRole;
    private InvalidGameRole m_EnemyRole;
    
    private const float m_timecd = 1.0f;
    private float m_time = 0;

    public void Init()
    {
        m_HeroRole = new InvalidGameRole(InvalidGameRole.RoleCamp.Hero, Random.Range(300, 600), Random.Range(30, 60), Random.Range(10, 30));
        m_EnemyRole = new InvalidGameRole(InvalidGameRole.RoleCamp.Enemy, Random.Range(300, 600), Random.Range(30, 60), Random.Range(10, 30));
        //Debug.LogWarning(string.Format("Create Hero Hp:{0} Att:{1} def:{2}",m_HeroRole.Hp,m_HeroRole.Attack,m_HeroRole.Define));
        //Debug.LogWarning(string.Format("Create Enemy Hp:{0} Att:{1} def:{2}", m_EnemyRole.Hp, m_EnemyRole.Attack, m_EnemyRole.Define));
    }

    public void Update()
    {
        if (m_HeroRole == null || m_EnemyRole == null)
            return;
        if (m_time < m_timecd)
        {
            m_time += Time.fixedDeltaTime;
            return;
        }
        m_time = 0;

        m_EnemyRole.BeHit(m_HeroRole.Attack);
        m_HeroRole.BeHit(m_EnemyRole.Attack);
        if (m_HeroRole.IsDeath || m_EnemyRole.IsDeath)
        {
            m_HeroRole = null;
            m_EnemyRole = null;
        }

    }
}
