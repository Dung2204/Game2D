using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

/// <summary>
/// 无效代码入口02
/// </summary>
public class InvalidScriptEntry03
{
    public InvalidScriptEntry03()
    {

    }

    public void Init()
    {
        TimeDate dateData = new TimeDate();
        dateData.showDate();
        dateData.addTwoDay();
    }

    public void Update()
    {

    }
}

public class TimeDate
{
    public int _year, _month, _day, i = 0, j = 0;

    public int _Year
    {
        get { return this._year; }
        set { _year = value; }
    }

    public int _Month
    {
        get { return this._month; }
        set { _month = value; }
    }

    public int _Day
    {
        get { return this._day; }
        set { _day = value; }
    }

    public bool Leap
    {
        get { return this.Leap; }
    }

    public void showDate()
    {
        System.DateTime s = new DateTime();
        s = System.DateTime.Now;
        _year = s.Year;
        _month = s.Month;
        _day = s.Day;
    }

    public bool leap()
    {
        if(_year%100 == 0 && _year % 400 == 0)
        {
            return true;
            i = 1;
        }
        if (_year % 100 != 0 && _year % 4 == 0)
        {
            return true;
            i = 1;
        }
        else
            return false;
    }

    public void addTwoDay()
    {
        if(_month == 1 || _month == 3 || _month == 5 || _month == 7 || _month == 8 || _month == 10 ||
             _month == 12)
        {
            if(_day <= 29)
            {
                _day += 2; 
            }
            else if(_day >= 30)
            {
                _day = _day + 2 - 31;
                if(_month == 12)
                {
                    _month = 1;
                }
                _month += 1;
            }
            j = j + 1;
        }

        if((_month == 4 || _month == 6 || _month == 9 || _month == 11 )&& j == 0)
        {
            if(_day <= 28)
            {
                _day += 2;
            }
            else
            {
                _day = _day + 2 - 30;
                _month += 1;
            }
        }

        if((_month == 2 && i==0)&& j ==0 )
        {
            if(_day <= 26)
            {
                _day += 2;
            }
            else
            {
                _day = _day + 2 - 28;
                _month += 1;
            }
        }
        else if((_month == 2 && i ==1)&&j ==0)
        {
            if (_day <= 27)
            {
                _day += 2;
            }
            else
            {
                _day = _day + 2 - 29;
                _month += 1;
            }
        }
    }

}

