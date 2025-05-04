using UnityEngine;
using System.Collections;
using System;

public class RoleProperty : BaseProperty
{
    private long _lHp;

    public RoleProperty() : base((int)EM_RoleProperty.End)
    {
    }

    public override void f_Reset()
    {
        base.f_Reset();
        _lHp = 0;
    }

    public long f_GetHp()
    {
        return _lHp;
    }

    public void f_AddHp(int iHp)
    {
        _lHp += iHp;
        if (_lHp < 0)
        {
            _lHp = 0;
        }
        base.f_UpdateProperty((int)EM_RoleProperty.Hp, (int)_lHp);
    }

    public void f_AddHp(long lHp)
    {
        _lHp += lHp;
        base.f_UpdateProperty((int)EM_RoleProperty.Hp, (int)_lHp);
    }

    public void f_SubtractHp(int iHp)
    {
        _lHp -= iHp;
        base.f_UpdateProperty((int)EM_RoleProperty.Hp, (int)_lHp);
    }

    public void f_UpdateHp(long lHp)
    {
        _lHp = lHp;
        base.f_UpdateProperty((int)EM_RoleProperty.Hp, (int)lHp);
    }

    public override void f_AddProperty(int iIndex, int iData)
    {
        if (iIndex == (int)EM_RoleProperty.Hp)
        {
            f_AddHp(iData);
        }
        else
        {
            base.f_AddProperty(iIndex, iData);
        }
    }

    /// <summary>
    /// 运算符重载
    /// 重载加方法
    /// </summary>
    /// <param name="tRoleProperty1"></param>
    /// <param name="tRoleProperty2"></param>
    /// <returns></returns>
    public static RoleProperty operator +(RoleProperty tRoleProperty1, RoleProperty tRoleProperty2)
    {
        for (int i = 1; i < (int)EM_RoleProperty.End; i++)
        {
            tRoleProperty1.f_AddProperty(i, tRoleProperty2.f_GetProperty(i));
        }
        //tRoleProperty1.f_AddHp(tRoleProperty2.f_GetHp());
        return tRoleProperty1;
    }
    /// <summary>
    /// 运算符重载
    /// 重载减方法
    /// </summary>
    /// <param name="tRoleProperty1"></param>
    /// <param name="tRoleProperty2"></param>
    /// <returns></returns>
    public static RoleProperty operator -(RoleProperty tRoleProperty1, RoleProperty tRoleProperty2)
    {
        for (int i = 1; i < (int)EM_RoleProperty.End; i++)
        {
            tRoleProperty1.f_AddProperty(i, -1 * tRoleProperty2.f_GetProperty(i));
        }
        //tRoleProperty1.f_AddHp(-1 * tRoleProperty2.f_GetHp());
        return tRoleProperty1;
    }
}