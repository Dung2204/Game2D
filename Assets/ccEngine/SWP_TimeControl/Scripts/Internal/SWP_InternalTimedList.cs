// --------------ABOUT AND COPYRIGHT----------------------
//  Copyright © 2013 SketchWork Productions Limited
//        support@sketchworkproductions.com
// -------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
///This class is used for holding a list of Internal Timed Objects.
/// </summary>
public class SWP_InternalTimedList    //DOIT:  Awaiting implementation - maybe version 2.1??
{
	/// <summary>
	///The TimeController group ID.
	/// </summary>
	public int GroupID;

	/// <summary>
	///Constructor
	/// </summary>
	public SWP_InternalTimedList (int _GroupID)
	{
		GroupID = _GroupID;
	}
}