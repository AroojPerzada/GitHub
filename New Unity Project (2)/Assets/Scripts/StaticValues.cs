using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class StaticValues : MonoBehaviour {

	public GameObject Rectangle;
	public GameObject Circle;
	public GameObject Rombus;
	public GameObject cube;

	// Use this for initialization
	void Start () {
		Rectangle.transform.position = new Vector3 (0f, 0f, 0f);

		Rombus.transform.position = new Vector3 (-10f, 0f, 0f);

		Circle.transform.position = new Vector3 (10f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		StateManager sm = TrackerManager.Instance.GetStateManager ();
		IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours ();
		Debug.Log ("List of trackables currently active tracked");
		foreach (TrackableBehaviour tb in activeTrackables) {
			Debug.Log ("Trackable: " + tb.TrackableName);
			if (tb.TrackableName == "Rectangle") {
				Rectangle.transform.position = new Vector3 (0f, 0f, 0f);
				cube.transform.position = Rectangle.transform.position;
			}
			else if (tb.TrackableName == "Rombus") {
				Rombus.transform.position = new Vector3 (-10f, 0f, 0f);
				cube.transform.position = Rombus.transform.position;	
			} else if (tb.TrackableName == "Circle") {
				Circle.transform.position = new Vector3 (10f, 0f, 0f);
				cube.transform.position = Circle.transform.position;
			} else {
				Debug.Log ("Nothing is active");	
			}
		}
	}
}
