using System.Collections;
using UnityEngine;

public class Lock : Interactable
{
	Transform toolsspawn;
	Transform player;
	Transform lockpick;
	Transform pick;
	Transform lockmesh;
	Transform screw;
	Transform cam;
	Transform fpctrl;
	Thief thief;
	Transform playerspawn;
	bool toggled;
	int rotspeed = 3;
	[SerializeField] public int unlockang;
	public int threshold;
	[SerializeField] int absang;
	float vertinp;
	Vector3 lockeuler;
	Vector3 screweuler;
	[SerializeField] bool inrange = false;
	float adjust;
	string pickaxis = "Horizontal";
	string lockaxis = "Vertical";
	[SerializeField] bool unlocked = false;

	//Animator anim;
	[SerializeField] bool open = false;
	Rng rng;
	Vector3 dooreuler;
	float ogrot;

	int ailayer;
	[SerializeField] bool slide=false;
	[SerializeField] float slidedist;
	Vector3 doorpos;
	Vector3 openpos;
	Vector3 closepos;
	[SerializeField] bool left;
	[SerializeField] bool auto=false;

	public override void Start () 
	{
		base.Start ();
		playerspawn = transform.FindChild ("spawn");
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		thief = player.GetComponent<Thief> ();
		fpctrl = player.FindChild ("fpcontrol");
		cam = fpctrl.FindChild ("Camera");
		toolsspawn = cam.FindChild ("pick");
		Transform tools = player.FindChild ("tools");
		lockpick = tools.FindChild ("lockpick");
		pick = lockpick.FindChild ("pick");
		lockmesh = lockpick.FindChild ("lock");
		screw = lockpick.FindChild ("screw");
		//anim = GetComponent<Animator> ();
		if (!auto && unlocked)
			setpopup ("open");
		rng = transform.GetComponent<Rng> ();
		rng.getlock ();
		rng.getchance ();
		unlockang = rng.getlock ();
		if (rng.getchance () < 50)
			unlockang = -unlockang;
		ogrot = transform.eulerAngles.y;
		ailayer = LayerMask.NameToLayer("ai");

		if(slide)
		{
			doorpos = transform.localPosition;
			openpos = doorpos;
			closepos = doorpos;
			if(left)
				openpos.x += slidedist;
			else
				openpos.x -= slidedist;
			//print (left+" - "+openpos.x);
		}
	}

	public override void interact ()
	{
		if (auto)
			return;
		if (unlocked) 
		{
			if (open)
				closedoor ();
			else
				opendoor ();
			return;
		}
		fpctrl.position = playerspawn.position;
		lockpick.gameObject.SetActive (true);
		lockpick.position = toolsspawn.position;
		lockpick.LookAt (cam);
		thief.noinp ();
		toggled = true;
		thief.incrime = true;
	}
	
	void Update () 
	{
		if (toggled) 
		{
			twisttools ();

			if(Input.GetKeyDown (thief.exitint)) 
			{
				//print ("exit");
				thief.inp ();
				toggled = false;
				lockpick.gameObject.SetActive (false);
				thief.incrime = false;
			}
		}
		//set below not run when at dest--eats cpu
		movedoor ();
	}

	void twisttools()
	{
		pick.Rotate(0.0f, 0.0f, -Input.GetAxis (pickaxis) * rotspeed);//add noise to total based on input
		absang = (int) Mathf.Abs (pick.eulerAngles.z);

		//pick in right area
		if (absang > 180)
			absang = -(360 - absang);
		inrange = (absang < unlockang + threshold) && (absang > unlockang - threshold);

		//torque lock
		vertinp = Input.GetAxisRaw (lockaxis);
		if(vertinp>0) 
		{
			lockmesh.Rotate (0.0f, 0.0f, vertinp * rotspeed * 0.5f);
			lockeuler = lockmesh.eulerAngles;
			//print (unlockang/absang);
			if(inrange)
				lockeuler.z = Mathf.Clamp (lockeuler.z, 0, 90);
			else
			{
				//adjust = (360f - Mathf.Max (1f, absang)) / 360f;
				adjust = (float) absang/unlockang;
				//print (absang + " - " + adjust);
				lockeuler.z = Mathf.Clamp (lockeuler.z, 0f, -adjust * 90f);
			}
			lockmesh.eulerAngles = lockeuler;
			screw.Rotate(0.0f, vertinp * rotspeed * 0.5f, 0.0f);
			screweuler = screw.localEulerAngles;
			screweuler.y = Mathf.Clamp (screweuler.y, 0, 40);
			screweuler.x = 0f;
			//print (screweuler.y);
			screw.localEulerAngles = screweuler;

			if (inrange && lockmesh.eulerAngles.z >= 90)
			{
				unlock ();
				thief.inp ();
				lockpick.gameObject.SetActive (false);
				opendoor ();
				toggled = false;
			}
			//tut tips: start top check slight left,right - pick side tht goes clockwise. if stops clockwise after,
			//gone too far, start back at top
		}
		else//rotate lock back to origin
		{
			lockeuler = lockmesh.eulerAngles;
			lockeuler = new Vector3 (lockeuler.x, lockeuler.y, Mathf.LerpAngle (lockeuler.z, 0f, 0.1f));
			lockmesh.eulerAngles = lockeuler;
			screweuler = screw.localEulerAngles;
			screweuler = new Vector3 (screweuler.x, Mathf.LerpAngle (screweuler.y, 0f, 0.1f), screweuler.z);
			screweuler.x = 0f;
			screw.localEulerAngles = screweuler;
		}
	}

	void movedoor()
	{
		if(slide)
		{
			if (open) 
			{
				doorpos = transform.localPosition;
				doorpos = new Vector3 (Mathf.Lerp (doorpos.x, openpos.x, 0.1f), doorpos.y, doorpos.z);
				transform.localPosition = doorpos;
			}
			else 
			{
				doorpos = transform.localPosition;
				doorpos = new Vector3 (Mathf.Lerp (doorpos.x, closepos.x, 0.1f), doorpos.y, doorpos.z);
				transform.localPosition = doorpos;
			}
		}
		else 
		{
			if (open) 
			{
				dooreuler = transform.eulerAngles;
				dooreuler = new Vector3 (dooreuler.x, Mathf.LerpAngle (dooreuler.y, ogrot - 90f, 0.1f), dooreuler.z);
				transform.eulerAngles = dooreuler;
			}
			else 
			{
				dooreuler = transform.eulerAngles;
				dooreuler = new Vector3 (dooreuler.x, Mathf.LerpAngle (dooreuler.y, ogrot, 0.1f), dooreuler.z);
				transform.eulerAngles = dooreuler;
			}
		}
	}

	public void opendoor()
	{
		//anim.SetTrigger ("open");
		open = true;
		if(!auto)
			setpopup ("close");
	}

	public void closedoor()
	{
		//anim.SetTrigger ("close");
		open = false;
		if(!auto)
			setpopup ("open");
	}

	void unlock()
	{
		unlocked = true;
	}

	void OnTriggerEnter(Collider other)
	{
		//print(name+ " - " + col.contacts[0].normal);
		//print (rb);
		//print ("door trig");
		if(other.gameObject.layer==ailayer) 
		{
			//print ("door ai");
			opendoor ();
		}
	}
}
