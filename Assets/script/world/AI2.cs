using System.Collections;
using UnityEngine;

public class AI2 : MonoBehaviour {
	public Transform target;
	[SerializeField]float lookrad=200f;
	float lookdamp=5f;
	bool inview;

	void Awake ()
	{
		target= GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	public void run ()
	{
		//fov ();  //done by segment
		if(inview)
		{
			Vector3 dir = target.position - transform.position;
			dir.y = 0;
			if (dir == Vector3.zero)//object directlt in front
				dir = transform.forward;
			float angle = Vector3.Angle (transform.forward, dir);
			if(angle<45)
			{
				Quaternion targetrot = Quaternion.LookRotation (dir);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetrot, Time.deltaTime * lookdamp);
			}
		}
	}
	void fov()
	{
		float dist = Vector3.Distance (target.position, transform.position);
		inview = dist < lookrad;//object in front of agent
	}
}
