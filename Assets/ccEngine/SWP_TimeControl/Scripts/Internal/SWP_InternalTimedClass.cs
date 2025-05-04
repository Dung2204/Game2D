// --------------ABOUT AND COPYRIGHT----------------------
//  Copyright © 2013 SketchWork Productions Limited
//        support@sketchworkproductions.com
// -------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
///This class is used when passing additional information to the TimedGameObjects.
/// </summary>
public class SWP_InternalTimedClass
{
	/// <summary>
	///The TimeController group ID.
	/// </summary>
	public int GroupID;
	
	/// <summary>
	///The New speed as a percentage.
	/// </summary>
	public float NewSpeed; 
	
	/// <summary>
	///Constructor which sets the Group ID and new speed as a percentage.
	/// </summary>
	public SWP_InternalTimedClass (int _GroupID, float _NewSpeed)
	{
		GroupID = _GroupID;
		NewSpeed = _NewSpeed;
	}
}