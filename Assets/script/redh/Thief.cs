using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thief : MonoBehaviour 
{
	Transform noisebart;
	Image noisebar;
	Color noisecol;
	public int noise=0;
	public FPSCtrl ctrl;
	Transform lockpick;
	public KeyCode exitint = KeyCode.Tab;
	public int hp= 3;
	[SerializeField] int maxhp;
	public int walknoise=5;
	float steptime=0f;
	float stepwait=1f;
	float reducetime=1f;
	float reducewait=1f;
	public bool lit=false;

	int heartstartx=20;
	int heartspace=40;
	Transform healthui;
	[SerializeField] GameObject heart;
	List<GameObject> hearts;
	int heartindex;

	[SerializeField] int yelthreshold = 70;
	[SerializeField] int redthreshold = 90;
	[SerializeField] int maxnoise=100;
	int runnoise;
	int movenoise;
	int reduceamount;
	[SerializeField] int reducedef=2;
	[SerializeField] int reducequiet=10;
	Text cashtext;

	public Transform hold;
	public Transform holdfar;
	public Transform cam;
	public bool canthrow;

	public KeyCode grabkey = KeyCode.E;
	Holdable grabobj;
	public float throwforce=2000f;
	public bool holding=false;
	RaycastHit grabhit;
	float grabrange = 8f;
	public float holdrange = 5f;

	Inventory inventory;
	[SerializeField] Transform levelwrap;
	Level level;
	public bool incrime=false;

	void Awake()
	{
		cam = transform.FindChild ("fpcontrol").FindChild ("Camera");
		hold = cam.FindChild ("drop");
		holdfar = cam.FindChild ("dropfar");
	}

	void Start ()
	{
		noisebart = GameObject.Find ("noise").transform;
		noisebar = noisebart.GetChild (0).GetComponent<Image> ();
		ctrl = transform.FindChild ("fpcontrol").GetComponent<FPSCtrl> ();
		lockpick = transform.FindChild ("tools").FindChild ("lockpick");
		lockpick.gameObject.SetActive (false);
		healthui = GameObject.Find ("healthui").transform;
		hearts = new List<GameObject> ();
		GameObject temp;
		Vector3 pos;
		RectTransform rt;
		int x=heartstartx;
		//maxhp = hp;
		for(int i=0; i<maxhp; i++)
		{
			temp = Instantiate (heart, healthui);
			rt = temp.GetComponent<RectTransform> ();
			pos = rt.localPosition;
			pos.x = x;
			rt.localPosition = pos;
			x += heartspace;
			if (i >= hp)
				temp.SetActive (false);
			hearts.Add (temp);
		}
		heartindex = hp - 1;
		runnoise = walknoise * 2;
		reduceamount = reducedef;
		cashtext = GameObject.Find ("cashtext").GetComponent<Text> ();
		inventory = GetComponent<Inventory> ();
		level = levelwrap.GetComponent<Level> ();
	}
	
	void Update () 
	{
		updatenoise ();
		if (Input.GetKeyDown (grabkey)) 
			grab ();
	}

	void LateUpdate()
	{
		updatenbar ();
		colornoise ();
	}

	public void steal(Collectable obj)//add obj to inv
	{
		if(!obj.free && level.seen)
		{
			print ("seen steal");
			level.alert (ctrl.transform.position);
		}
		inventory.addobj (obj);
	}

	void grab()//hold obj
	{
		if (canthrow) 
		{
			canthrow = false;
			holding = false;
			//hands.tele (inuse);
			grabobj.toss ();
		}
		else 
		{
			if (Physics.Raycast (cam.position, cam.forward, out grabhit, grabrange) && (grabobj = grabhit.transform.GetComponent<Holdable> ())) 
			{
				Debug.Log ("rgab");
				holding = true;
				//hands.tele (inuse);
				grabobj.grab ();
			}
		}
	}

	public void updatecash(int amount)
	{
		cashtext.text = "$"+amount;
	}
	public void damage(int dmg)
	{
		for(int i=0; i<dmg; i++)
		{
			if (hp == 0)
				return;
			hearts [heartindex].SetActive (false);
			hp--;
			heartindex--;
		}
	}
	public void heal(int amount)
	{
		for(int i=0; i<amount; i++)
		{
			//print (hp +" - "+ heartindex);
			if (hp == maxhp)
				continue;
			hp++;
		}
		refreshhearts ();
	}

	void refreshhearts()
	{
		for(int i=0; i<hp; i++)
		{
			//print (hp +" - "+ heartindex);
			hearts [i].SetActive (true);
			heartindex=i;
		}
	}

	public void noinp()
	{
		/*ctrl.camobj.mouseDelta = Vector2.zero;
		ctrl.camobj._smoothMouse = Vector2.zero;
		ctrl.camobj._mouseAbsolute = Vector2.zero;*/
		ctrl.inbook = true;
	}

	public void inp()
	{
		ctrl.inbook = false;
	}

	void updatenoise()
	{
		if (ctrl.running)
			movenoise = runnoise;
		else
			movenoise = walknoise;
			
		//walking
		if (ctrl.moving) 
		{
			reduceamount = reducedef;
			//print ("thief mov");
			if (steptime < 0f) 
			{
				noise += movenoise;
				steptime = stepwait;
			} 
			else
				steptime -= Time.deltaTime;
		}
		else
		{
			reduceamount = reducequiet;
			steptime = 0f;
		}

		//reduce over time
		if(reducetime<0) 
		{
			noise -= reduceamount;
			if (noise < 0)
				noise = 0;
			reducetime = reducewait;
		}
		else
		{
			reducetime -= Time.deltaTime;
		}

		if (noise > maxnoise)
			noise = maxnoise;
	}

	void updatenbar()
	{
		noisebart.localScale = new Vector3 (((float)noise / (float)maxnoise), 1, 1);
	}

	void colornoise()
	{
		if(noise > redthreshold)
			noisebar.color = Color.red;
		else if(noise > yelthreshold)
			noisebar.color = Color.yellow;
		else
			noisebar.color = Color.green;
	}
}
