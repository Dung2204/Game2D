using UnityEngine;
using System.Collections;

public class InvalidGameRole
{
    public enum RoleCamp
    {
        Hero = 0,
        Enemy = 1,
    }

    public InvalidGameRole(RoleCamp camp,int initHp,int initAttack,int initDefine)
    {
        Camp = camp;
        m_Hp = initHp;
        m_HpMax = initHp;
        m_Attack = initAttack;
        m_Define = initDefine;
    }

    public RoleCamp Camp
    {
        get;
        private set;
    }

    private int m_Hp;
    public int Hp
    {
        get
        {
            return m_Hp;
        }
    }

    private int m_HpMax;
    public int HpMax
    {
        get
        {
            return m_HpMax;
        }
    }

    private int m_Attack;
    public int Attack
    {
        get
        {
            return m_Attack;
        }
    }

    private int m_Define;
    public int Define
    {
        get
        {
            return m_Define;
        }
    }

    public bool IsDeath
    {
        get
        {
            return m_Hp <= 0;
        }
    }

    public void BeHit(int attack)
    {
        if (IsDeath)
            return;
        int hitValue = attack - m_Define;
        if (hitValue <= 0)
        {
            hitValue = 1;
        }
        m_Hp -= hitValue;
        if (m_Hp < 0)
            m_Hp = 0;
        //Debug.LogWarning(string.Format("{0} BeHit {1} hp,isDeath{2}", Camp.ToString(), hitValue, IsDeath));
    }
}
