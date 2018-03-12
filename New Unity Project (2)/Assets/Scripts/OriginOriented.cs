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
			OnTrackingFound();
		}
		else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
			newStatus == TrackableBehaviour.Status.NOT_FOUND)
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
			OnTrackingLost();
		}
		else
		{
			// For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
			// Vuforia is starting, but tracking has not been lost or found yet
			// Call OnTrackingLost() to hide the augmentations
			OnTrackingLost();
		}
		Update ();
	}

	#endregion // PUBLIC_METHODS

	#region PRIVATE_METHODS

	protected virtual void OnTrackingFound()
	{
		var rendererComponents = GetComponentsInChildren<Renderer>(true);
		var colliderComponents = GetComponentsInChildren<Collider>(true);
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

		GameObject rectangle = GameObject.FindGameObjectWithTag ("Rectangle");
		GameObject rombus = GameObject.FindGameObjectWithTag ("Rombus");
	//	GameObject circle = GameObject.FindGameObjectWithTag ("Circle");

			if (!cube.activeSelf) {
				Debug.Log ("if cube is not active");
				cube.SetActive (true);
			}
			if (mTrackableBehaviour.TrackableName == "Rectangle") {
				cube.transform.position = rectangle.transform.position;
				Debug.Log ("Rectangle position: " + rectangle.transform.position + " " + cube.transform.position);
			} else if (mTrackableBehaviour.TrackableName == "Rombus") {
				temp.x = rombus.transform.position.x * 3.0f;
				temp.y = rombus.transform.position.y;
				temp.z = rombus.transform.position.z;
				cube.transform.position = temp;
				Debug.Log ("Rombus: " + rombus.transform.position + " " + cube.transform.position);
			} else if ((mTrackableBehaviour.TrackableName == "Rectangle") && (mTrackableBehaviour.TrackableName == "Rombus") == true) {
				cube.transform.position = rectangle.transform.position;
				Debug.Log ("Rectangle and Rombus found: " + rectangle.transform.position + " " + cube.transform.position);
			} else {
				Debug.Log ("All conditions are false");
			}
	} 

	protected void Update() {
		StateManager sm = TrackerManager.Instance.GetStateManager ();
		activeTrackables = sm.GetActiveTrackableBehaviours ();
		Debug.Log ("List of trackables currently active tracked");
		foreach (TrackableBehaviour tb in activeTrackables) {
			Debug.Log ("Trackable: " + tb.TrackableName);
		}
	}


	protected  void OnTrackingLost()
	{
		Debug.Log ("Tracking lost of " + mTrackableBehaviour.TrackableName);

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

		if (mTrackableBehaviour.TrackableName == "Rombus") {
			cube.SetActive (false);
		}
		 else if (mTrackableBehaviour.TrackableName == "Rectangle") {
			cube.SetActive (false);
		}
	}

	#endregion // PRIVATE_METHODS
}
