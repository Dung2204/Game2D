using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class TipAniTemplateDT
{
    public TipAniTemplateDT()
    {

    }

    /// <summary>
    /// 根据tweener 来初始化数据
    /// </summary>
    /// <param 模板Id ="id"></param>
    /// <param 动画的步骤="idx"></param>
    /// <param 具体Tweener="tweener"></param>
    /// <returns></returns>
    public bool f_InitByTweener(byte id, byte idx, UITweener tweener)
    {
        if (tweener == null)
            return false;
        Id = id;
        Idx = idx;
        if (tweener is TweenPosition)
        {
            TweenType = (byte)UITweenerType.Position;
            TweenPosition tmpTween = tweener as TweenPosition;
            if (tmpTween == null)
            {
                Debug.LogError(string.Format("Tweener as {0} Error", UITweenerType.Position.ToString()));
                return false;
            }
            UpdateFromValue(tmpTween.from.x, tmpTween.from.y, tmpTween.from.z, 0);
            UpdateToValue(tmpTween.to.x, tmpTween.to.y, tmpTween.to.z, 0);

        }
        else if (tweener is TweenScale)
        {
            TweenType = (byte)UITweenerType.Scale;
            TweenScale tmpTween = tweener as TweenScale;
            if (tmpTween == null)
            {
                Debug.LogError(string.Format("Tweener as {0} Error", UITweenerType.Scale.ToString()));
                return false;
            }
            UpdateFromValue(tmpTween.from.x, tmpTween.from.y, tmpTween.from.z, 0);
            UpdateToValue(tmpTween.to.x, tmpTween.to.y, tmpTween.to.z, 0);
        }
        else if (tweener is TweenColor)
        {
            TweenType = (byte)UITweenerType.Color;
            TweenColor tmpTween = tweener as TweenColor;
            if (tmpTween == null)
            {
                Debug.LogError(string.Format("Tweener as {0} Error", UITweenerType.Color.ToString()));
                return false;
            }
            UpdateFromValue(tmpTween.from.r, tmpTween.from.g, tmpTween.from.b, tmpTween.from.a);
            UpdateToValue(tmpTween.from.r, tmpTween.from.g, tmpTween.from.b, tmpTween.from.a);
        }
        else
        {
            Debug.LogError("UITweenerType not Find! Please Add Type");
            return false;
        }
        UpdateTweenerValue(tweener.style, tweener.duration, tweener.delay, tweener.tweenGroup, tweener.animationCurve);
        return true;
    }

    /// <summary>
    /// 设置tweener数据
    /// </summary>
    /// <param name="tweener"></param>
    /// <returns></returns>
    public bool f_SetTweener(ref UITweener tweener)
    {
        if (tweener is TweenPosition)
        {
            TweenPosition tmpTween = tweener as TweenPosition;
            if (tmpTween == null)
            {
                if (tmpTween == null)
                {
                    Debug.LogError(string.Format("Tweener as {0} Error", UITweenerType.Position.ToString()));
                    return false;
                }
            }
            tmpTween.from = new Vector3(FromValue1, FromValue2, FromValue3);
            tmpTween.to = new Vector3(ToValue1, ToValue2, ToValue3);
        }
        else if (tweener is TweenScale)
        {
            TweenScale tmpTween = tweener as TweenScale;
            if (tmpTween == null)
            {
                if (tmpTween == null)
                {
                    Debug.LogError(string.Format("Tweener as {0} Error", UITweenerType.Scale.ToString()));
                    return false;
                }
            }
            tmpTween.from = new Vector3(FromValue1, FromValue2, FromValue3);
            tmpTween.to = new Vector3(ToValue1, ToValue2, ToValue3);
        }
        else if (tweener is TweenScale)
        {
            TweenColor tmpTween = tweener as TweenColor;
            if (tmpTween == null)
            {
                if (tmpTween == null)
                {
                    Debug.LogError(string.Format("Tweener as {0} Error", UITweenerType.Color.ToString()));
                    return false;
                }
            }
            tmpTween.from = new Color(FromValue1, FromValue2, FromValue3,FromValue4);
            tmpTween.to = new Color(ToValue1, ToValue2, ToValue3, FromValue4);
        }
        else
        {
            Debug.LogError("UITweenerType not Find! Please Add Type");
            return false;
        }
        tweener.style = (UITweener.Style)PlayStyle;
        tweener.duration = Duration;
        tweener.delay = StartDelay;
        tweener.tweenGroup = TweenGroup;
        tweener.animationCurve = mAnimationCurve;
        return true;
    }

    public byte Id
    {
        get;
        private set;
    }

    public byte Idx
    {
        get;
        private set;
    }

    public byte TweenType
    {
        get;
        private set;
    }

    public float FromValue1
    {
        get;
        private set;
    }

    public float FromValue2
    {
        get;
        private set;
    }
    public float FromValue3
    {
        get;
        private set;
    }
    public float FromValue4
    {
        get;
        private set;
    }

    public float ToValue1
    {
        get;
        private set;
    }

    public float ToValue2
    {
        get;
        private set;
    }

    public float ToValue3
    {
        get;
        private set;
    }

    public float ToValue4
    {
        get;
        private set;
    }

    #region Tweener属性

    public byte PlayStyle
    {
        get;
        private set;
    }

    public float Duration
    {
        get;
        private set;
    }

    public float StartDelay
    {
        get;
        private set;
    }

    public byte TweenGroup
    {
        get;
        private set;
    }

    public float[] CurveTime
    {
        get;
        private set;
    }
    public float[] CurveValue
    {
        get;
        private set;
    }
    public float[] CurveInTangent
    {
        get;
        private set;
    }
    public float[] CurveOutTangent
    {
        get;
        private set;
    }

    public AnimationCurve mAnimationCurve
    {
        get
        {
            if (_animationCuve == null)
            {
                _animationCuve = new AnimationCurve();
                for (int i = 0; i < CurveTime.Length; i++)
                {
                    Keyframe tmpKey = new Keyframe(CurveTime[i],CurveValue[i], CurveInTangent[i], CurveOutTangent[i]);
                    mAnimationCurve.AddKey(tmpKey);
                }
            }
            return _animationCuve;
        }
    }

    [NonSerialized]
    private AnimationCurve _animationCuve;

    #endregion

    private void UpdateFromValue(float value1,float value2,float value3,float value4)
    {
        FromValue1 = value1;
        FromValue2 = value2;
        FromValue3 = value3;
        FromValue4 = value4;
    }

    private void UpdateToValue(float value1, float value2, float value3, float value4)
    {
        ToValue1 = value1;
        ToValue2 = value2;
        ToValue3 = value3;
        ToValue4 = value4;
    }

    private void UpdateTweenerValue(UITweener.Style playStyle, float duration, float startDelay, int tweenGroup, AnimationCurve animationCurve)
    {
        PlayStyle = (byte)playStyle;
        Duration = duration;
        StartDelay = startDelay;
        TweenGroup = (byte)tweenGroup;
        CurveTime = new float[animationCurve.keys.Length];
        CurveValue = new float[animationCurve.keys.Length];
        CurveInTangent = new float[animationCurve.keys.Length];
        CurveOutTangent = new float[animationCurve.keys.Length];
        for (int i = 0; i < animationCurve.keys.Length; i++)
        {
            CurveTime[i] = animationCurve.keys[i].time;
            CurveValue[i] = animationCurve.keys[i].value;
            CurveInTangent[i] = animationCurve.keys[i].inTangent;
            CurveOutTangent[i] = animationCurve.keys[i].outTangent;            
        }
    } 

    public override string ToString()
    {
        return string.Format("ID:{0:d3} Idx:{1:d3} TweenType:{2:d3} from:[{3},{4},{5},{6}] to:[{7},{8},{9},{10}] PlayStyle:{11} Duration:{12} StartDelay:{13} TweenGroup:{14}",
            Id,Idx,((UITweenerType)TweenType).ToString(),FromValue1,FromValue2,FromValue3,FromValue4,ToValue1,ToValue2,ToValue3,ToValue4,((UITweener.Style)PlayStyle).ToString(),Duration,StartDelay,TweenGroup);
    }
}
