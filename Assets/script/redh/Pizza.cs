using System.Collections;
using UnityEngine;

public class Pizza : Interactable
{
	Transform lid;
	[SerializeField] bool open = false;
	bool eaten=false;
	Thief player;
	Vector3 lideuler;
	Vector3 closerot;
	Vector3 openrot;

	[SerializeField] Vector3 openpos;
	Vector3 closepos;
	[SerializeField] float opendist;
	Vector3 currpos;
	int heal=3;
	bool toggled=false;
	GameObject pie;

	public override void Start () 
	{
		base.Start ();
		lid = transform.FindChild ("lid");
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Thief> ();
		closerot = lid.localEulerAngles;
		openrot = closerot;
		//print (openrot.x);
		openrot.x -= 90f;
		//print (openrot.x);
		closepos = lid.localPosition;
		openpos = closepos;
		openpos.z = opendist;
		pie = transform.FindChild ("pie").gameObject;
	}
	
	public override void interact ()
	{
		if(toggled)
		{
			eat ();
		}
		open = !open;
		if(open && !eaten)
		{
			print ("in");
			setpopup ("eat");
			toggled = true;
		}
		else if(open)
		{
			setpopup ("close");
		}
		else
		{
			setpopup ("open");
		}

	}

	void eat()
	{
		player.heal (heal);
		eaten = true;
		toggled = false;
		pie.SetActive (false);
	}

	void Update()
	{
		if(open)
		{
			lideuler = lid.eulerAngles;
			lideuler = new Vector3 (Mathf.LerpAngle (lideuler.x, openrot.x, 0.1f), lideuler.y, lideuler.z);
			lid.eulerAngles = lideuler;
			//print (lideuler.x);
			currpos = lid.localPosition;
			currpos = new Vector3 (currpos.x, currpos.y, Mathf.Lerp (currpos.z, openpos.z, 0.1f));
			lid.localPosition = currpos;
		}
		else
		{
			lideuler = lid.eulerAngles;
			lideuler = new Vector3 (Mathf.LerpAngle (lideuler.x, closerot.x, 0.1f), lideuler.y, lideuler.z);
			lid.eulerAngles = lideuler;

			currpos = lid.localPosition;
			currpos = new Vector3 (currpos.x, currpos.y, Mathf.Lerp (currpos.z, closepos.z, 0.1f));
			lid.localPosition = currpos;
		}
	}
}
