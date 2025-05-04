using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// 无效代码入口01
/// </summary>
public class InvalidScriptEntry02
{
    RandomUtils m_Data;
    public InvalidScriptEntry02()
    {
        
    }

    public void Init()
    {
        m_Data = new RandomUtils();
        int a = 1;
        int b = 2;
        m_Data.Swap(ref a, ref b);
        m_Data.First(2,3,6);
        m_Data.DateDay();
        m_Data.Biao();
        float num = m_Data.FindNum("1.2", "5.2", "6.2");
        long timeStart = ccMath.DateTime2time_t(m_Data.GetTimeByTimeStr("20180605"));
        int total = 0;//计时
        int gk = Int32.Parse("10");
        int count = Int32.Parse("5");
        int size = Int32.Parse("4");
        for (int i = 0; i < gk; i++)
        {

            for (int j = 0; j < count; j++)
            {
                //Console.WriteLine("这是第" + (i + 1) + "关" + "第" + (j + 1) + "次");
                //产生随机字母
                string str = new RandomUtils().CreateRandomWord(size);
                //Console.WriteLine("你要输入的内容为：" + str);
                //时间计算
                DateTime start = DateTime.Now;
                //等待用户输入
                string userinput = Console.ReadLine();
                DateTime end = DateTime.Now;

                int t = (int)(end.Ticks - start.Ticks) / 10000000;//单次计时
                total += t;//总计时

            }
        }

    }

    //数字排序
    public void PopSort(int[] list)
    {
        int i, j, temp;  //先定义一下要用的变量
        for (i = 0; i < list.Length - 1; i++)
        {
            for (j = i + 1; j < list.Length; j++)
            {
                if (list[i] > list[j]) //如果第二个小于第一个数
                {
                    //交换两个数的位置，在这里你也可以单独写一个交换方法，在此调用就行了
                    temp = list[i]; //把大的数放在一个临时存储位置
                    list[i] = list[j]; //然后把小的数赋给前一个，保证每趟排序前面的最小
                    list[j] = temp; //然后把临时位置的那个大数赋给后一个
                }
            }
        }
    }

    public void Update()
    {

    }
}


public class RandomUtils
{
    /// <summary>
    /// 用来装载字符的数组
    /// </summary>
    private char[] chars = new char[50];
    /// <summary>
    /// 初始化数组数据
    /// </summary>
    public RandomUtils()
    {

        InvalidScriptEntry02 data = new InvalidScriptEntry02();
        int[] arrayNum = {2,8,3,9,1,5};
        data.PopSort(arrayNum);
        //得到a-z的字符
        int idx = 0;
        for (int i = 'a'; i < 'z' + 1; i++)
        {
            if (i == 'o')
            {//去掉o字母
                continue;
            }
            chars[idx] += (char)i;
            idx++;

        }

        //得到1-9的字符
        int idx2 = idx;
        for (int j = '0'; j < '9' + 1; j++)
        {
            chars[idx2++] = (char)j;

        }

        //重新组装数据
        char[] newchars = new char[idx2];
        for (int m = 0; m < idx2; m++)
        {
            if (chars[m] == 'l')
            {//将小写的l换成L


                chars[m] = 'L';

            }
            newchars[m] = chars[m];

        }
        //将重组后的新数组赋值给原来的数组便于给其他方法访问数组数据
        chars = newchars;

    }

    /// <summary>
    /// 随机产生字符串
    /// </summary>
    /// <param name="size">产生的字符串个数</param>
    /// <returns></returns>
    public string CreateRandomWord(int size)
    {

        StringBuilder builder = new StringBuilder();
        System.Random r = new System.Random();
        for (int i = 0; i < size; i++)
        {

            char c = chars[r.Next(chars.Length)];

            if (builder.ToString().Contains(c))//处理字符串重复出现
            {
                i--;
                continue;
            }
            builder.Append(c);

        }
        return builder.ToString();
    }

    //交换变量
    public void Swap(ref int n1, ref int n2)
    {
        int temp = n1;
        n1 = n2;
        n2 = temp;
    }


    public void First(int a, int b, int q)
    {
        System.Random rd = new System.Random();
        a = rd.Next(0, 51);
        if (a >= 0 && a <= 3)
        {
            b = 2;
        }
        else if (a >= 4 && a <= 7)
        {
            b = 3;
        }
        else if (a >= 8 && a <= 11)
        {
            b = 4;
        }
        else if (a >= 12 && a <= 15)
        {
            b = 5;
        }
        else if (a >= 16 && a <= 19)
        {
            b = 6;
        }
        else if (a >= 20 && a <= 23)
        {
            b = 7;
        }
        else if (a >= 24 && a <= 27)
        {
            b = 8;
        }
        else if (a >= 28 && a <= 31)
        {
            b = 9;
        }
        else if (a >= 32 && a <= 47)
        {
            b = 10;
        }
        else if (a >= 48 && a <= 51)
        {
            if (q >= 0 && q <= 21)
            {
                b = 10;
            }
            else
                b = 1;
        }
        else
        {
            b = 0;
        }
    }

    public DateTime GetTimeByTimeStr(string StrTime)
    {
        int bYear = int.Parse(StrTime.Substring(0, 4));
        int bMonth = int.Parse(StrTime.Substring(4, 2));
        int bDay = int.Parse(StrTime.Substring(6, 2));
        DateTime time = new DateTime(bYear, bMonth, bDay, 0,
            0, 0);
        return time;
    }

    public void DateDay()
    {
        bool isActive = true;
        while (isActive)
        {
            int year, month;
            while (true)
            {
                year = int.Parse("1901");
                if (year < 1900 || year > 2100)
                {
                    month = int.Parse("5");
                    break;
                }
                else
                {
                    month = int.Parse("5");
                    if (month < 1 || month > 12)
                    {
                        break;
                    }
                    else
                        break;
                }
            }
            List<string> dataes = new List<string>();
            
            int crossDayToYear = 0, crossDayToMonth = 0;
             
            for (int i = 1900; i < year; i++)
            {
                if (i % 4 == 0 && i % 100 != 0 || i % 400 == 0)
                    crossDayToYear += 366;
                else
                    crossDayToYear += 365;
            }

            for (int i = 1; i < month; i++)
            {
                if (i == 2)
                {
                    if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
                        crossDayToMonth += 29;
                    else
                        crossDayToMonth += 28;
                }
                else if (i <= 7 && i % 2 != 0 || i > 7 && i % 2 == 0)
                    crossDayToMonth += 31;
                else
                    crossDayToMonth += 30;
            }

            int crossDay = crossDayToYear + crossDayToMonth;
            int dayOfWeek = crossDay % 7 + 1;
            int space = dayOfWeek - 1;
            for (int i = 0; i < space; i++)
            {
                dataes.Add("");
            }

            int days;
            if (month == 2)
            {
                if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
                    days = 29;
                else
                    days = 28;
            }
            else if (month <= 7 && month % 2 != 0 || month > 7 && month % 2 == 0)
                days = 31;
            else
                days = 30;
            for (int i = 1; i <= days; i++)
            {
                dataes.Add(i.ToString());
            }
            for (int i = 0; i < dataes.Count; i++)
            {
                if (i % 7 == 0)
                    break;
            }
            isActive = false;
        }
    }

    public void Biao()
    {
        string st1 = "2008-11-4";
        DateTime dt2 = System.DateTime.Now;
        DateTime dt1 = Convert.ToDateTime(st1);

        string dateTerm = null;//项目剩余时间  
        try
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();

            dateTerm =  ts.Days.ToString();
        }
        catch
        {

        }
    }


    public float FindNum(string a,string b,string c)
    {
        float x, y, z, min = 0;
        x = float.Parse(a);
        y = float.Parse(b);
        z = float.Parse(c);
        if (x < y && x < z)
        {
            min = x;
        }    
        else if (y < x && y < z)
        {
            min = y;
        }
        else
        {
            min = z;
        }
        return min;
    }

}