using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PolygonField : MonoBehaviour {

	public GameObject[] go_raw;
	public Text[] go_text_angle;
	public Text[] go_text_distance;
	public Text perimeter_text;
	public Text area_text;
	public Text zombie_text; 

	public GameObject OriginPlaceHolder;

	private Vector2[] go_points;
	private Text[] go_points_text_a;
	private Text[] go_points_text_d;
	private GameObject[] go_n;

	private LineRenderer lineRenderer;
	private MeshFilter filter;

	void Start () {
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		filter = gameObject.GetComponent<MeshFilter> ();
	}

	void Update () {
		getAllAvailablePoints ();
		draw ();
		drawLines ();
		calculation ();
	//	getOriginPoint ();
	}
	private void getOriginPoint() {
		// Get GameObject 1 cordinates
		// Loop all points
		//for (int i = 0; i < go_points.Length; i++) {
		//	midDistance.text = "Distance: " + go_points_text_a [0];
		//	Debug.Log ("Go Text Distance: " + go_text_distance[i]);
		//}

	}
	private void getAllAvailablePoints(){
		// Create new Vector2 and Text Lists
		List<Vector2> vertices2DList = new List<Vector2>();
		List<Text> textAList = new List<Text>();
		List<Text> textDList = new List<Text>();
		List<GameObject> oList = new List<GameObject>();

		// Fill lists if availble
		for(int i = 0; i < go_raw.Length; i++){
			if (go_raw [i] != null) {
				if (go_raw [i].GetComponent<MeshRenderer> ().enabled) {
					
					//OriginPlaceHolder.GetComponent<MeshRenderer> ().enabled = true;

					OriginPlaceHolder.SetActive (true);
					go_text_angle [i].enabled = true;
					go_text_distance [i].enabled = true;

					vertices2DList.Add (new Vector2 (go_raw [i].transform.position.x, go_raw [i].transform.position.y));

					textAList.Add (go_text_angle [i]);
					textDList.Add (go_text_distance [i]);

					oList.Add ( go_raw [i] );

					Debug.Log ("Go raw: " + go_raw [i].transform.position);

				} else {
					OriginPlaceHolder.SetActive (false);
					go_text_angle [i].enabled = false;
					go_text_distance [i].enabled = false;
					//OriginPlaceHolder.GetComponent<MeshRenderer> ().enabled = false;
				}
			}
		}

		// Convert to array
		go_points_text_a = textAList.ToArray ();
		go_points_text_d = textDList.ToArray ();
		go_points = vertices2DList.ToArray ();
		go_n = oList.ToArray ();
	}

	// Draw Mesh for Mesh Filter
	private void draw(){
		// Create Vector2 vertices
		Vector2[] vertices2D = go_points;

		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertices2D);
		int[] indices = tr.Triangulate();

		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[vertices2D.Length];
		for (int i = 0; i < vertices.Length; i++) {
			vertices[i] = new Vector3(go_n[i].transform.position.x, go_n[i].transform.position.y, go_n[i].transform.position.z);
		}

		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals();
		msh.RecalculateBounds();

		// Set up game object with mesh;
		filter.mesh = msh;
	}

	// Draw Lines for Line Renderer
	private void drawLines(){
		// Set positions size
		lineRenderer.positionCount = go_points.Length;

		// Set all line psitions
		for(int i = 0; i < go_points.Length; i++){
			lineRenderer.SetPosition(i, new Vector3(go_n[i].transform.position.x, go_n[i].transform.position.y, go_n[i].transform.position.z));
		}
	}

	private void calculation(){
		// Perimeter varible
		double p = 0;
		// Area varible
		double area = 0;
		// Type varible
		int n = 0;

		// For mid Values
		int s = 0;
		Vector2[] midPoints = new Vector2[2];

		GameObject rect = GameObject.FindGameObjectWithTag ("Rectangle");
		GameObject circ = GameObject.FindGameObjectWithTag ("Circle");

		// Loop all points

		for(int i = 0; i < go_points.Length; i++){
//			For test neighbors
//			int x0, x1, x2;

			// Point before
			Vector2 v0;
			if ((i - 1) >= 0) {
				v0 = go_points [i-1];						// x0 = i - 1;
			} else {
				v0 = go_points [go_points.Length - 1];		// x0 = go_points.Length - 1;
			}

			// Point now
			Vector2 v1 = go_points [i];						// x1 = i;

			// Point after
			Vector2 v2;
			if ((i + 1) < go_points.Length) {
				v2 = go_points [i+1];						// x2 = i + 1;
			} else {
				v2 = go_points [0];							// x2 = 0;
			}

			// triangular distances
			double dv0 = distance (v0.x, v0.y, v1.x, v1.y); // v0 & v1
			double dv1 = distance (v1.x, v1.y, v2.x, v2.y); // v1 & v2
			double dv2 = distance (v0.x, v0.y, v2.x, v2.y); // v0 & v2

			// Perimeter

			p += dv1;

			// Area

			double temp_area = (v1.x * v2.y) - (v1.y * v2.x);
			area += temp_area;

			// Type

			n++;

			// Angle

			// Set point angle ∠v0v1v2
			double a = angle (dv0, dv1, dv2, v0.x, v0.y, v1.x, v1.y, v2.x, v2.y);
			go_points_text_a [i].text = Math.Round (a) +"°";
			//	test neighbors 
			//	go_points_text_a [i].text = i + "# " + x0 + " " + x1 + " " + x2;

			// Distance

			// Set distance position
			Vector2 mp = midPoint (v1.x, v1.y, v2.x, v2.y);
			Debug.Log ("Distance of Marker 1: " + v1.x + "," + v1.y);
			Debug.Log ("Distance of Marker 2: " + v2.x + "," + v2.y);



			go_points_text_d [i].transform.parent.position = new Vector3(mp.x, mp.y, go_points_text_d[i].transform.parent.position.z );

			OriginPlaceHolder.SetActive (true);
			if ((v1.x == rect.transform.position.x && v1.y == rect.transform.position.y) && (v2.x == rect.transform.position.x && v2.y == rect.transform.position.y)) {
				Debug.Log ("Rectangle found");
				OriginPlaceHolder.transform.position  = new Vector3(v1.x, v1.y, go_points_text_d[i].transform.parent.position.z );
				perimeter_text.text = "Rect: " + v1;
				zombie_text.text = "Z pos: " + OriginPlaceHolder.transform.position;
				Debug.Log ("Zombie position when rect found: " + OriginPlaceHolder.transform.position);
			} else if ((v1.x == circ.transform.position.x && v1.y == circ.transform.position.y) && (v2.x == circ.transform.position.x && v2.y == circ.transform.position.y)) {
				Debug.Log ("Circle found");	
			
				OriginPlaceHolder.transform.position = new Vector3(v1.x + 3, v1.y, go_points_text_d[i].transform.parent.position.z );
				Debug.Log ("Zombie position when circle found: " + OriginPlaceHolder.transform.position +" "+
					v1);
				area_text.text = "Circ: " + v1;
				zombie_text.text = "Z pos: " + OriginPlaceHolder.transform.position;
			} else if ((v1.x == circ.transform.position.x && v1.y == circ.transform.position.y) && (v2.x == rect.transform.position.x && v2.y == rect.transform.position.y)) {
				Debug.Log ("Both found Circle and rect");	

				perimeter_text.text = "Rect: "+ v2;
				area_text.text = "Circ:" + v1;

				OriginPlaceHolder.transform.position = new Vector3(v2.x, v2.y, go_points_text_d[i].transform.parent.position.z );
				Debug.Log ("Zombie position: " + OriginPlaceHolder.transform.position);

				zombie_text.text = "Z pos: " + OriginPlaceHolder.transform.position;
			} else if ((v1.x == rect.transform.position.x && v1.y == rect.transform.position.y) && (v2.x == circ.transform.position.x && v2.y == circ.transform.position.y)) {
				Debug.Log ("Both found rect and circle");	

				perimeter_text.text = "Rect: "+ v1;
				area_text.text = "Circ:" + v2;

				OriginPlaceHolder.transform.position = new Vector3(v1.x, v1.y, go_points_text_d[i].transform.parent.position.z );
				Debug.Log ("Zombie position: " + OriginPlaceHolder.transform.position);

				zombie_text.text = "Z pos: " + OriginPlaceHolder.transform.position;
			}

			// Set distance angle
			double az = angle_zero (v1.x, v1.y, v2.x, v2.y);
			go_points_text_d [i].transform.parent.eulerAngles = new Vector3(go_points_text_d [i].transform.parent.eulerAngles.x, go_points_text_d [i].transform.parent.eulerAngles.y, (float)az);

			// Set "point" and "point after" distance
			go_points_text_d [i].text = Math.Round (dv1, 2) + "";

			midPoints [s] = mp;
			s = s + 1;

			//midDistance.text = "MidValue: "+go_points_text_d [i].transform.parent.position;
			//OriginPlaceHolder.transform.position = new Vector3(mp.x, mp.y, go_points_text_d[i].transform.parent.position.z );
			Debug.Log ("Zombie position: " + OriginPlaceHolder.transform.position);
		}
	}

	// Distance between two points (Pitagor theory)
	private double distance(float x1, float y1, float x2, float y2){
		float a = Math.Abs(x1 - x2);
		float b = Math.Abs(y1 - y2);
		double c = Math.Sqrt(a*a + b*b);

		return c;
	}

	// Angle between two lines(three points) anticlockwise
	private double angle(double i1, double i2, double i3, float p1x, float p1y, float p2x, float p2y, float p3x, float p3y){
		double k = ((i2 * i2) + (i1 * i1) - (i3 * i3)) / (2 * i1 * i2);
		double d = Math.Acos ( k ) * (180 / Math.PI);

		double dd = direction ( p1x, p1y, p2x, p2y, p3x, p3y );

		if (dd > 0) {
			d = 360 - d;
		}

		return d;
	}
	private double direction(float x1, float y1, float x2, float y2, float x3, float y3){
		double d = ((x2 - x1)*(y3 - y1)) - ((y2 - y1)*(x3 - x1));
		return d;
	}

	// Middle Point betwwen two points
	private Vector2 midPoint(float x1, float y1, float x2, float y2){
		float x = (x1 + x2) / 2;
		float y = (y1 + y2) / 2;
		return new Vector2 (x, y);
	}

	// Zero way angle betwwen two points
	private double angle_zero(float x1, float y1, float x2, float y2){
		double xDiff = x2 - x1;
		double yDiff = y2 - y1;
		double d = Math.Atan2(yDiff, xDiff) * (180 / Math.PI);
		return d;
	}
}