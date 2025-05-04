using UnityEngine;
using System.Collections;


public class SWP_TimedSpine : SWP_InternalTimedGameObject
{
    private CharActionController _CharActionController = null;

    //void Start()
    //{
       

    //}

    protected override void SetSpeedAssigned(float _fNewSpeed, float _fCurrentSpeedPercent, float _fCurrentSpeedZeroToOne)
    {
        if (_CharActionController == null)
        {
             _CharActionController = gameObject.GetComponent<CharActionController>();
        }
        if (_CharActionController != null)
        {
            _CharActionController.timeScale = _fCurrentSpeedZeroToOne;
        }
    }



}
