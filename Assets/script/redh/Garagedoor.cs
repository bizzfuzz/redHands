using System.Collections;
using UnityEngine;

public class Garagedoor : Interactable
{
	[SerializeField] bool open = false;
	Vector3 dooreuler;
	Vector3 ogrot;
	float openrot;
	[SerializeField] Vector3 openpos;
	Vector3 closepos;
	[SerializeField] int opendist;
	Vector3 currpos;

	public override void Start () 
	{
		base.Start ();
		ogrot = transform.eulerAngles;
		openrot = ogrot.x - 90f;
		closepos = transform.localPosition;
		openpos = closepos;
		openpos.y += opendist;
	}
	
	void Update ()
	{
		if(open)
		{
			dooreuler = transform.eulerAngles;
			dooreuler = new Vector3 (Mathf.LerpAngle (dooreuler.x, openrot, 0.1f), dooreuler.y, dooreuler.z);
			transform.eulerAngles = dooreuler;
			lift ();
		}
		else
		{
			dooreuler = transform.eulerAngles;
			dooreuler = new Vector3 (Mathf.LerpAngle (dooreuler.x, ogrot.x, 0.1f), dooreuler.y, dooreuler.z);
			transform.eulerAngles = dooreuler;
			lower ();
		}
	}

	public override void interact ()
	{
		if (open)
			closedoor ();
		else
			opendoor ();
	}

	void opendoor()
	{
		open = true;
		setpopup ("close");
	}

	void closedoor()
	{
		open = false;
		setpopup ("open");
	}

	void lift()
	{
		currpos = transform.localPosition;
		currpos = new Vector3 (currpos.x, Mathf.Lerp (currpos.y, openpos.y, 0.1f), currpos.z);
		transform.localPosition = currpos;
	}
	void lower()
	{
		currpos = transform.localPosition;
		currpos = new Vector3 (currpos.x, Mathf.Lerp (currpos.y, closepos.y, 0.1f), currpos.z);
		transform.localPosition = currpos;
	}

}
