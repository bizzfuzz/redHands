using System.Collections;
using UnityEngine;

public class Activity : MonoBehaviour
{
	public bool sleep;
	public bool sit;
	public Transform aiparent;
	public Vector3 rot;
	public int activetime = 0;

	void Start ()
	{
		if(sit || sleep)
			aiparent = transform.GetChild (0);
	}
	
	/*void Update () {
		
	}*/
}
