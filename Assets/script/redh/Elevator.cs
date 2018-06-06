using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour 
{
	Transform fpplayer;
	//public Transform dest;
	Lock leftdoor;
	Lock rightdoor;
	[SerializeField] float closewait=1f;
	[SerializeField] float closetimer;
	//[SerializeField] bool open=false;

	void Start ()
	{
		fpplayer = GameObject.FindGameObjectWithTag ("Player").transform.FindChild ("fpcontrol");
		//print (transform.FindChild ("doorl")+" - "+transform);
		leftdoor = transform.FindChild ("doorl").GetComponent<Lock> ();
		rightdoor = transform.FindChild ("doorr").GetComponent<Lock> ();
	}

	void Update()
	{
		if (closetimer > 0f) 
		{
			closetimer -= Time.deltaTime;
			if (closetimer < 0f)
				closedoors ();
		}
	}

	public void go (Transform dest)
	{
		Vector3 pos = fpplayer.position;
		pos.y = dest.position.y;
		fpplayer.position = pos;
	}

	public void opendoors()
	{
		leftdoor.opendoor ();
		rightdoor.opendoor ();
		closetimer = closewait;
	}

	public void closedoors()
	{
		leftdoor.closedoor ();
		rightdoor.closedoor ();
	}
}
