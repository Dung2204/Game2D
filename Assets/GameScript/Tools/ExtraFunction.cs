using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtraFunction {
    /// <summary>
    /// 重置位置、旋转、缩放
    /// </summary>
    /// <param name="trans"></param>
    public static void ResetTransform(this Transform trans)
    {
        trans.localPosition = Vector3.zero;
        trans.localScale = Vector3.one;
        trans.localEulerAngles = Vector3.zero;
    }
}
