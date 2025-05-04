using UnityEngine;
using System.Collections;

public class BattleMaskBG : MonoBehaviour
{
    public SpriteRenderer m_MaskBg;


    //void Start()
    //{

    //}
    

    public void f_Open()
    {
        m_MaskBg.gameObject.SetActive(true);

        Hashtable args = new Hashtable();
        args.Add("a", 0.78f);      
        args.Add("time", 0.167f); 
        args.Add("easeType", iTween.EaseType.linear);        
        args.Add("oncomplete", "AnimationEnd");
        args.Add("oncompleteparams", "OpenEnd");
        args.Add("oncompletetarget", gameObject);
        
        iTween.ColorTo(m_MaskBg.gameObject, args);
    }

    public void f_Close()
    {
        Hashtable args = new Hashtable();
        args.Add("a", 0);
        args.Add("time", 0.167f);
        args.Add("easeType", iTween.EaseType.linear);
        args.Add("oncomplete", "AnimationEnd");
        args.Add("oncompleteparams", "CloseEnd");
        args.Add("oncompletetarget", gameObject);

        iTween.ColorTo(m_MaskBg.gameObject, args);
    }


    void AnimationEnd(string f)
    {
        if (f == "CloseEnd")
        {
            m_MaskBg.gameObject.SetActive(false);
        }

    }


}
