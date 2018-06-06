using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
drag drop:
canvas
add ammohuds & link(text component)
*/


public class FPSCtrl : MonoBehaviour
{
	public KeyCode up;
	public KeyCode down;
	public KeyCode left;
	public KeyCode right;
	public KeyCode run;
	KeyCode jump;
	KeyCode interact;
	KeyCode book;
	KeyCode crouch;

	public float spd=25f;
	[SerializeField]float runspd=50f;
	public float defspd;
	[SerializeField]float jmpmod=5f;//2000 for superjump(use as power later?),200 normal
	Transform loadout;
	bool climbing=false;
	[SerializeField]float climbrange;//how close have to be to wall
	[SerializeField]float floordist=.2f;//how close have to be to wall
	Transform walltop;

	float wallheight;
	Hands hands;
	public Rigidbody rb;
	public bool canjump;
	int newwep;
	Transform fpscam;
	//[SerializeField]float waterlevel=190f;
	//Underwater uwater;
	public shot rshot;
	public shot lshot;
	GameObject questbook;

	public bool inbook;
	FPSCam camobj;
	Interactable last;
	[SerializeField] float interactdist = 8f;
	bool ininteraction=false;
	public bool mounted;
	public Horse mount;
	GameObject rfist;
	GameObject lfist;
	public bool running=false;
	public bool flying=false;

	RaycastHit interhit;
	Interactable obj;
	bool crouching=false;
	Transform capsule;
	float standheight;
	float crouchheight=1.5f;

	public bool moving = false;
	bool tmoving;
	Vector3 voff = new Vector3(0,1,0);

	void Awake () 
	{
		up = KeyCode.W;
		down = KeyCode.S;
		left = KeyCode.A;
		right = KeyCode.D;
		jump = KeyCode.Space;
		interact = KeyCode.F;
		//book = KeyCode.Tab;
		run = KeyCode.LeftShift;
		crouch = KeyCode.C;

		rb = GetComponent<Rigidbody> ();
		fpscam= transform.FindChild ("Camera");
		canjump = true;
		hands = GetComponent<Hands> ();
		hands.ctrl = this;
		//uwater = fpscam.GetComponent<Underwater> ();
		questbook = GameObject.Find ("questbook");
		if(questbook)
			questbook.SetActive (false);
		inbook = false;
		camobj = fpscam.GetComponent<FPSCam> ();
		loadout = transform.FindChild ("loadout");
		//equip fists
		rfist = loadout.FindChild ("fist").gameObject;
		lfist = loadout.FindChild ("fistalt").gameObject;
		hands.equip (true, rfist.transform);
		hands.equip (false, lfist.transform);
		defspd = spd;
		capsule = transform.FindChild ("Capsule");
	}

	public void stoprun()
	{
		running = false;
		hands.run (running);
	}

	void Update()
	{
		/*if (Input.GetKeyDown (book)) 
		{
			if(inbook)
				questbook.SetActive (false);
			else
				questbook.SetActive (true);
			inbook = !inbook;
		}*/
		if (Input.GetKeyDown (interact)) 
		{
			//Interactable obj;
			RaycastHit hit;
			//standing in front of interactable object
			if(Physics.Raycast (fpscam.position, fpscam.forward,out hit,interactdist) && obj)//(obj=hit.transform.GetComponent<Interactable> ()))
			{
				if(!ininteraction) 
				{
					ininteraction = true;
					//Debug.Log (obj);
					obj.interact ();
					ininteraction = false;
				}
			}
		}
		if (Input.GetKeyDown (crouch)) 
		{
			if (crouching)
				crouchstop ();
			else
				crouchstart ();
		}
		if(!flying) {
			if (Input.GetKeyDown (run) && !running) {
				spd = runspd;
				running = true;
				//hands.run (running);
			}
			if (Input.GetKeyUp (run) && running) {
				spd = defspd;
				running = false;
				//hands.run (running);
			}
		}
		camobj.running = !inbook;
	}

	public void crouchstart()
	{
		standheight = capsule.localScale.y;
		//print (standheight);
		Vector3 prop = capsule.localScale;
		prop.y = crouchheight;
		capsule.localScale = prop;
		crouching = true;
	}
	public void crouchstop()
	{
		//standheight = capsule.localScale.y;
		//print (standheight);
		Vector3 prop = capsule.localScale;
		prop.y = standheight;
		capsule.localScale = prop;
		crouching = false;
	}

	void LateUpdate()
	{
		if (!inbook && Physics.Raycast (fpscam.position, fpscam.forward, out interhit, interactdist) && (obj = interhit.transform.GetComponent<Interactable> ())) 
		{
			obj.popup ();
			last = obj;
		}
		else if(last!=null) 
		{
			last.closepopup ();
			last = null;
		}
	}

	void FixedUpdate () 
	{
		if (inbook || mounted)//on horse
		{
			return;
		}

		if(climbing)
		{
			transform.Translate (Vector3.up*spd*5*Time.deltaTime);
			//Debug.Log (transform.position.y+" - "+ wallheight+" = "+ Mathf.Abs (transform.position.y - wallheight));
			if (Mathf.Abs (transform.position.y - wallheight) < 0.3f) 
			{
				climbing = false;
				rb.useGravity = true;
			}
		}
		else//player grounded
		{
			tmoving = false;
			if (Input.GetKey (up)) 
			{
				//Debug.Log ("in");
				transform.Translate (Vector3.forward * spd * Time.deltaTime);
				tmoving = true;
			}
			if (Input.GetKey (down)) 
			{
				transform.Translate (Vector3.back * spd * Time.deltaTime);
				tmoving = true;
			}
			if (Input.GetKey (left)) 
			{
				transform.Translate (Vector3.left * spd * Time.deltaTime);
				tmoving = true;
			}
			if (Input.GetKey (right)) 
			{
				transform.Translate (Vector3.right * spd * Time.deltaTime);
				tmoving = true;
			}
			moving = tmoving;

			//jump ctrl
			RaycastHit hit;
			if (Physics.Raycast (transform.position+voff, Vector3.down, out hit)) 
			{
				//Debug.DrawLine (transform.position, hit.point,Color.white, 10);
				//print (Vector3.Distance (transform.position, hit.point));
				//print (hit.transform);
				canjump = (Vector3.Distance (transform.position, hit.point) < floordist);
			}
			//Debug.Log (Vector3.Distance (transform.position,hit.point));
			if (Input.GetKey (jump) && canjump) 
			{
				//Debug.Log ("in");
				if (Physics.Raycast (transform.position, transform.forward, out hit, climbrange) && hit.transform.tag=="wall")
				{
					//Debug.Log (hit.transform);
					climbing = true;
					rb.useGravity = false;
					walltop = hit.transform.FindChild ("wall").GetChild (0);
					wallheight = walltop.position.y;
				}
				else 
				{
					rb.velocity = Vector3.up * jmpmod * Time.deltaTime;
					canjump = false;
				}
			}
		}
	}
}
