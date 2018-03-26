/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class OriginOriented : MonoBehaviour, ITrackableEventHandler
{
	#region PRIVATE_MEMBER_VARIABLES

	protected TrackableBehaviour mTrackableBehaviour;
	public GameObject cube;
	private IEnumerable<TrackableBehaviour> activeTrackables;
	private bool ifTracked = false;

	//For canvas
	public Text rect_distance;

	#endregion // PRIVATE_MEMBER_VARIABLES

	#region UNTIY_MONOBEHAVIOUR_METHODS

	protected virtual void Start()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
	}

	#endregion // UNTIY_MONOBEHAVIOUR_METHODS

	#region PUBLIC_METHODS

	/// <summary>
	///     Implementation of the ITrackableEventHandler function called when the
	///     tracking state changes.
	/// </summary>
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
			ifTracked = true;
		//	OnTrackingFound();
		}
		else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
			newStatus == TrackableBehaviour.Status.NOT_FOUND)
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
			ifTracked = false;
		//	OnTrackingLost();
		}
		else
		{
			ifTracked = false;
			// For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
			// Vuforia is starting, but tracking has not been lost or found yet
			// Call OnTrackingLost() to hide the augmentations
		//	OnTrackingLost();
		}
	//	Update ();
	}

	#endregion // PUBLIC_METHODS

	#region PRIVATE_METHODS
	protected void Update() {
		StateManager sm = TrackerManager.Instance.GetStateManager ();
		activeTrackables = sm.GetActiveTrackableBehaviours ();

	Debug.Log ("List of trackables currently active tracked");
	ifTracked = false;
	foreach (TrackableBehaviour tb in activeTrackables) {
		Debug.Log ("Trackable: " + tb.TrackableName);
		ifTracked = true;

		if (tb.TrackableName == "Rectangle") {
			cube.SetActive (true);	
			cube.transform.position = tb.transform.position;
				rect_distance.text = "Rectangle: " + tb.transform.position;
			Debug.Log ("Rectangle position: " + tb.transform.position + " " + cube.transform.position);	
		} else if (tb.TrackableName == "Circle") {
			cube.SetActive (true);
			cube.transform.position = new Vector3 (GameObject.FindGameObjectWithTag ("Circle").transform.position.x + 3,
				GameObject.FindGameObjectWithTag ("Circle").transform.position.y,
				GameObject.FindGameObjectWithTag ("Circle").transform.position.z);
			Debug.Log ("Circle position: " + tb.transform.position + " " + cube.transform.position);
				rect_distance.text = "Circle: "+tb.transform.position;
		} else if (tb.TrackableName == "Rectangle" && tb.TrackableName == "Circle") {
			cube.SetActive (true);	
			cube.transform.position = GameObject.FindGameObjectWithTag ("Rectangle").transform.position;
			Debug.Log ("Rectangle position: " + tb.transform.position + " " + cube.transform.position);	
		}

	} 
	if (!ifTracked) {
		cube.SetActive (false);
	}
	}
#endregion // PRIVATE_METHODS
}
