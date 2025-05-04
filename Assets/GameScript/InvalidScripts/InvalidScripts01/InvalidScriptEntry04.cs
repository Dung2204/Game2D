using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

/// <summary>
/// 无效代码入口02
/// </summary>
public class InvalidScriptEntry04
{
    public InvalidScriptEntry04()
    {

    }

    public void Init()
    {
        Articles articles = new Articles();
        Books books = new Books();
        int x = 1;
        if(x==1)
        {
            books.Cate();
            books.Boo();
        }
        else
        {
            articles.Cate();
            articles.Art();
        }
    }

    public void Update()
    {

    }
}

public class Catalogue
{
    private string title;
    public string Title
    {
        get { return title; }

        set { title = value; }
    }

    private string author;
    public string Author
    {
        get { return author; }

        set { author = value; }
    }

    private string date;
    public string Date
    {
        get { return date; }

        set { date = value; }
    }

    public void Cate()
    {
        title = "x";
        author = "z";
        date = "2017.12.12";
    }
}


public class Articles : Catalogue
{
    private string magazins;
    public string Magazines
    {
        get { return magazins; }
        set { magazins = value; }
    }

    private string issn;
    public string ISSN
    {
        get { return issn; }
        set { issn = value; }
    }

    public void Art()
    {
        magazins = "h";
        issn = "1";
    }

}

public class Books:Catalogue
{
    private string pressname;
    public string Pressname
    {
        get { return pressname; }
        set { pressname = value; }
    }

    private string pressplace;
    public string PressPlace
    {
        get { return pressplace; }
        set { pressplace = value; }
    }

    public void Boo()
    {
        pressname = "z";
        pressplace = "h";
    }
}
