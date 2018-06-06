using System.Collections;
using UnityEngine;

public class Fov : MonoBehaviour {

	public float angle;
	[Range(0,360)]
	public float radius;
	public Transform target;
	Vector3 dir;
	public float speed;
	float step;
	Quaternion rot;
	//public Vector3 adjust;

	void Start () 
	{
		target = null;
		StartCoroutine ("look", .05f);
	}

	void looktarget()
	{
		dir = (target.position - transform.position).normalized;//direction to target from transform
		//dir += adjust;

		if (Vector3.Angle (transform.forward, dir) > angle / 2) //ball not in view
		{
			step = speed * Time.deltaTime; //switch to agility if added
			dir.y = 0;
			rot = Quaternion.LookRotation (dir);
			transform.rotation = Quaternion.Slerp (transform.rotation, rot, step);
		}
	}
	IEnumerator look(float delay)
	{
		while(true)
		{
			yield return new WaitForSeconds (delay);
			if(target)
				looktarget ();
		}
	}
	public Vector3 direction(float angledegrees,bool global)//direction to angle
	{
		if (!global)
			angledegrees += transform.eulerAngles.y;
		return new Vector3 (Mathf.Sin (angledegrees * Mathf.Deg2Rad), 0, Mathf.Cos (angledegrees * Mathf.Deg2Rad));
	}
}