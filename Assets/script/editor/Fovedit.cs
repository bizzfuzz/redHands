using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fov))]
public class Fovedit : Editor {

	void OnSceneGUI () 
	{
		Fov fov = (Fov)target;
		Handles.color = Color.white;
		Handles.DrawWireArc (fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);
		Vector3 anglea = fov.direction (-fov.angle / 2, false);//left edge
		Vector3 angleb = fov.direction (fov.angle / 2, false);//right edge
		Handles.DrawLine (fov.transform.position, fov.transform.position+anglea*fov.radius);//draw left
		Handles.DrawLine (fov.transform.position, fov.transform.position+angleb*fov.radius);//draw right
		if (fov.target != null)
			Handles.DrawLine (fov.transform.transform.position, fov.target.position);
	}
}
