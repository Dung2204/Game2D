// --------------ABOUT AND COPYRIGHT----------------------
//  Copyright © 2013 SketchWork Productions Limited
//        support@sketchworkproductions.com
// -------------------------------------------------------

using UnityEngine;  
using System.Collections;

/// <summary>
/// This is a special internal class used by the SWP_TimedGameObject and 3rd party SWP_Timed???? classes and should never not be used on its own.
/// </summary>
public class SWP_InternalTimedGameObject: MonoBehaviour
{
	/// <summary>
	///You can use this to attach one of your scene TimeControllers to. It is only used to set the starting speed of the time controlled components. This is mainly for objects that are instantiated so they are set to the correct speed of the current TimeController.
	/// </summary>
	[SerializeField] public SWP_TimeGroupController TimeGroupController;

	/// <summary>
	///This is what links your TimedGameObject script to the correct TimeController GroupID.
	/// </summary>
	[SerializeField] public int ControllerGroupID = 1;

	/// <summary>
	///If true, this will follow point number 2 above and ignores the “Assigned Objects” array.
	///If false, this will follow point number 3 above and uses the “Assigned Objects” array.
	/// </summary>
	[SerializeField] public bool SearchObjects = true;

	/// <summary>
	///Only used when SearchObjects is true. Put any objects you want to time control in here.
	/// </summary>
	[SerializeField] public Object[] AssignedObjects;

	/// <summary>
	///You should not directly change this value, but you can read this to see the current speed as a percentage. 
	/// </summary>
	internal float fCurrentSpeedPercent = 100f;

	/// <summary>
	///You should not directly change this value, but you can read this to see the current speed as a zero to one value.
	/// </summary>
	internal float fCurrentSpeedZeroToOne = 1f;

	/// <summary>
	///Internal value of the last speed as a percentage.
	/// </summary>
	protected float fPreviousSpeedPercentage = 100f;
	
	/// <summary>
	///The Start function is used to assign the current speed to the objects when instantiated.
	/// </summary>
	void Start()
	{
		if (TimeGroupController != null)
			SetSpeed(TimeGroupController.ControllerSpeedPercent);
	}
	
	/// <summary>
	///This function gets the speed from the new percentage.
	/// </summary>
	protected float GetNewSpeedFromPercentage(float _fInSpeed)
	{
		float thisOriginalSpeed;
		
		if (fPreviousSpeedPercentage != 0f)
			thisOriginalSpeed = (100f / fPreviousSpeedPercentage) * _fInSpeed;
		else
			thisOriginalSpeed = _fInSpeed;
		
		return (thisOriginalSpeed / 100f) * fCurrentSpeedPercent;
	}
	
	/// <summary>
	///This function gets the speed from the new percentage.
	/// </summary>
	protected float GetNewSpeedFromPercentage(float _fOriginalSpeed, float _fInSpeed, bool _bReverse)
	{
		if (_bReverse && fCurrentSpeedPercent != 0f)
			return _fOriginalSpeed * (100f / fCurrentSpeedPercent);
		else if (_bReverse)
			return _fOriginalSpeed * (100f / 0.001f);
		else
			return (_fOriginalSpeed / 100f) * fCurrentSpeedPercent;
	}
	
	/// <summary>
	///When not using SeachObjects the caches need to be cleaned.
	/// </summary>
	protected virtual void ClearAssignedObjects()
	{
	}

	/// <summary>
	///When not using SeachObject this function loops through the object array and sets the speed for the ohjects.
	/// </summary>
	protected virtual void SetSpeedLooping(float _fNewSpeed, float _fCurrentSpeedPercent, float _fCurrentSpeedZeroToOne)
	{
	}

	/// <summary>
	///This function actually sets the new speed of the objects.
	/// </summary>
	protected virtual void SetSpeedAssigned(float _fNewSpeed, float _fCurrentSpeedPercent, float _fCurrentSpeedZeroToOne)
	{
	}

	/// <summary>
	///This function sets the current speed (percentage and ZeroToOne) parameters, calls the main set new speed routine and finally sets the previous speed parameter.
	/// </summary>
	void SetSpeed(float _fNewSpeed)
	{
		fCurrentSpeedPercent = Mathf.Clamp(_fNewSpeed, 0f, 1000f);
		fCurrentSpeedZeroToOne = (fCurrentSpeedPercent == 0f ? 0f : fCurrentSpeedPercent / 100f);
		
		if (SearchObjects)
			SetSpeedAssigned(_fNewSpeed, fCurrentSpeedPercent, fCurrentSpeedZeroToOne);
		else
			SetSpeedLooping(_fNewSpeed, fCurrentSpeedPercent, fCurrentSpeedZeroToOne);
		
		if (fCurrentSpeedPercent != 0)
			fPreviousSpeedPercentage = fCurrentSpeedPercent;
	}

	/// <summary>
	///This method is used where you want to time control DeltaTime.  Just replace your Time.DeltaTime calls with a call to this function.
	/// </summary>
	public float TimedDeltaTime()
	{
		if (fCurrentSpeedPercent != 0)
			return Time.deltaTime / (100f / fCurrentSpeedPercent);
		else
			return 0f;
	}  

	/// <summary>
	///This is called when you recieve a Pause Request broadcast fron the main TimeController component for this GroupID.
	/// </summary>
	void OnGroupPauseBroadcast(SWP_InternalTimedClass _swpTimedClass)
	{
		if (_swpTimedClass.GroupID != ControllerGroupID)
			return;			
		
		SetSpeed(_swpTimedClass.NewSpeed);
	}

	/// <summary>
	///This is called when you recieve a SlowDown Request broadcast fron the main TimeController component for this GroupID.
	/// </summary>
	void OnGroupSlowDownBroadcast(SWP_InternalTimedClass _swpTimedClass)
	{
		if (_swpTimedClass.GroupID != ControllerGroupID)
			return;
		
		SetSpeed(_swpTimedClass.NewSpeed);
	}

	/// <summary>
	///This is called when you recieve a SpeedUp Request broadcast fron the main TimeController component for this GroupID.
	/// </summary>
	void OnGroupSpeedUpBroadcast(SWP_InternalTimedClass _swpTimedClass)
	{
		if (_swpTimedClass.GroupID != ControllerGroupID)
			return;
		
		SetSpeed(_swpTimedClass.NewSpeed);
	}
	
	/// <summary>
	///This is called when you recieve a BackToNormal time Request broadcast fron the main TimeController component for this GroupID.
	/// </summary>
	void OnGroupResumeBroadcast(SWP_InternalTimedClass _swpTimedClass)
	{
		if (_swpTimedClass.GroupID != ControllerGroupID)
			return;
		
		SetSpeed(_swpTimedClass.NewSpeed);
	}
	
	/// <summary>
	///This is called when you recieve a Global Pause time Request broadcast fron the Time Manager.
	/// </summary>
	void OnGlobalPauseBroadcast()
	{
	}
	
	/// <summary>
	///This is called when you recieve a Global Resume time Request broadcast fron the Time Manager.
	/// </summary>
	void OnGlobalResumeBroadcast()
	{
	}
}