using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventFunc : MonoBehaviour {

    SceneBattle _SceneBattle;

    public void SetSceneBattle(SceneBattle com)
    {
        _SceneBattle = com;
    }

    void AnimationEvnetFunction(string str)
    {
        if (_SceneBattle != null)
            _SceneBattle.AnimationEvnetFunction(str);
    }
}
