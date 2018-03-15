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
	private int Count;
	public bool rec;
	public bool romb;

	#endregion // PRIVATE_MEMBER_VARIABLES

	#region UNTIY_MONOBEHAVIOUR_METHODS

	protected virtual void Start()
	{
		rec = false;
		romb = false;
		Count = 20;
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
			if (mTrackableBehaviour.TrackableName == "Rectangle") {
				rec = true;
				romb = false;
			} else if(mTrackableBehaviour.TrackableName == "Circle") {
				romb = true;
				if (rec == true) {
					romb = false;
				}
			}
			OnTrackingFound();
		}
		else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
			newStatus == TrackableBehaviour.Status.NOT_FOUND)
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
			if (mTrackableBehaviour.TrackableName == "Rectangle") {
				rec = false;
			}
			if (mTrackableBehaviour.TrackableName == "Circle") {
				romb = false;
			}
			OnTrackingLost();
		}
		else
		{
			// For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
			// Vuforia is starting, but tracking has not been lost or found yet
			// Call OnTrackingLost() to hide the augmentations
			OnTrackingLost();
		}
		//Update ();
	}

	#endregion // PUBLIC_METHODS

	#region PRIVATE_METHODS

	protected virtual void OnTrackingFound()
	{


		GameObject rectangle = GameObject.FindGameObjectWithTag ("Rectangle");
		GameObject rombus = GameObject.FindGameObjectWithTag ("Circle");
		//	GameObject circle = GameObject.FindGameObjectWithTag ("Circle");

		if (mTrackableBehaviour.TrackableName == "Rectangle" && rec == true) {
			cube.transform.position = new Vector3 (rectangle.transform.position.x, rectangle.transform.position.y, rectangle.transform.position.z);
		} else if (mTrackableBehaviour.TrackableName == "Circle" && romb == true) {
			cube.SetActive (true);
			cube.transform.position = new Vector3 (rombus.transform.position.x + 5, rombus.transform.position.y, rombus.transform.position.z);
		} else {
		}



		//GameObject rombus = GameObject.FindGameObjectWithTag ("Rombus");
		//Debug.Log (rombus);
		//cube.transform.position = new Vector3 (rombus.transform.position.x + 5, rombus.transform.position.y, rombus.transform.position.z);
		/*var rendererComponents = GetComponentsInChildren<Renderer>(true);
		/*var colliderComponents = GetComponentsInChildren<Collider>(true);
		var canvasComponents = GetComponentsInChildren<Canvas>(true);

		Vector3 temp = new Vector3 ();

		// Enable rendering:
		foreach (var component in rendererComponents)
			component.enabled = true;

		// Enable colliders:
		foreach (var component in colliderComponents)
			component.enabled = true;

		// Enable canvas':
		foreach (var component in canvasComponents)
			component.enabled = true;
*/
	}

	protected void Update() {


		/*Debug.Log ("Start of Update Function");
		StateManager sm = TrackerManager.Instance.GetStateManager ();
		activeTrackables = sm.GetActiveTrackableBehaviours ();
		Debug.Log ("List of trackables currently active tracked");
		foreach (TrackableBehaviour tb in activeTrackables) {
			Debug.Log ("Trackable: " + tb.TrackableName);*/
		
		if (Count >= 0) {
			Count = Count - 1;
			if (Count == 0) {
				Count = 20;
				OnTrackingFound ();
			}
		}

	}


	protected  void OnTrackingLost()
	{
		//Debug.Log ("Tracking lost of " + mTrackableBehaviour.TrackableName);

		var rendererComponents = GetComponentsInChildren<Renderer>(true);
		var colliderComponents = GetComponentsInChildren<Collider>(true);
		var canvasComponents = GetComponentsInChildren<Canvas>(true);

		// Disable rendering:
		foreach (var component in rendererComponents)
			component.enabled = false;

		// Disable colliders:
		foreach (var component in colliderComponents)
			component.enabled = false;

		// Disable canvas':
		foreach (var component in canvasComponents)
			component.enabled = false;

		if (mTrackableBehaviour.TrackableName == "Circle") {
			romb = false;
		}
		if (mTrackableBehaviour.TrackableName == "Rectangle") {
			rec = false;
		}
		cube.SetActive (false);
	}

	#endregion // PRIVATE_METHODS
}
