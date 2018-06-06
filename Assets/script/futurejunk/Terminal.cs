using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : Interactable
{
	//GameObject ui;
	Thief player;
	[SerializeField] GameObject numslot;
	[SerializeField] difficulty diff = difficulty.easy;
	[SerializeField] int[] code;
	[SerializeField] int[] inpnum;
	[SerializeField] Text[] screennum;
	int tries =5;
	Rng rng;
	Transform hackwrap;
	bool toggled = false;
	[SerializeField] int current=0;

	public KeyCode up;
	public KeyCode down;
	public KeyCode left;
	public KeyCode right;
	public KeyCode setcode;
	[SerializeField] bool unlocked = false;

	Text screentries;
	Transform trywrap;
	Text screenright;
	Transform rightwrap;

	void Awake ()
	{
		//ui = GameObject.Find ("termui");
		int codelen;
		if (diff == difficulty.easy)
			codelen = 3;
		else if (diff == difficulty.medium)
			codelen = 4;
		else
			codelen = 5;

		code = new int[codelen];
		screennum = new Text[codelen];
		inpnum = new int[codelen];

		rng = transform.GetComponent<Rng> ();
		rng.getchance ();//first was always same;
		int chance = 50;
		hackwrap = transform.FindChild ("hack");
		float xoff = -1f;
		GameObject temp;
		Vector3 pos;

		for(int i=0; i<code.Length; i++)
		{
			if (rng.getchance () < chance)
				code [i] = 0;
			else
				code [i] = 1;
			temp = Instantiate (numslot, hackwrap);
			pos = temp.transform.localPosition;
			pos.x += xoff;
			xoff -= 0.4f;
			temp.transform.localPosition = pos;

			screennum [i] = temp.transform.GetChild (0).GetChild (0).GetComponent<Text> ();
			inpnum [i] = 0;
		}

		up = KeyCode.W;
		down = KeyCode.S;
		left = KeyCode.A;
		right = KeyCode.D;
		setcode = KeyCode.E;

		trywrap = transform.FindChild ("tries");
		screentries = trywrap.GetChild (0).GetChild (0).GetComponent<Text> ();
		screentries.text = ""+tries;

		rightwrap = transform.FindChild ("correct");
		screenright = rightwrap.GetChild (0).GetChild (0).GetComponent<Text> ();
		//screentries.text = ""+tries;
	}

	public override void Start()
	{
		base.Start ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Thief> ();
		trywrap.gameObject.SetActive (false);
		hackwrap.gameObject.SetActive (false);
		rightwrap.gameObject.SetActive (false);

		/*up = player.ctrl.up;
		down = player.ctrl.down;
		left = player.ctrl.left;
		right = player.ctrl.right;*/
	}

	public override void interact ()
	{
		if (tries <= 0)
			return;
		player.noinp ();
		closepopup ();
		trywrap.gameObject.SetActive (true);
		hackwrap.gameObject.SetActive (true);
		rightwrap.gameObject.SetActive (true);
		toggled = true;
		//ui.SetActive (true);
	}

	void Update()
	{
		if(toggled) 
		{
			if (Input.GetKeyDown (up) || Input.GetKeyDown (down)) 
			{
				//Debug.Log ("in");
				inpnum [current] = 1 - inpnum [current];
				screennum[current].text = ""+inpnum [current];
			}
			if (Input.GetKeyDown (left) && current>0) 
			{
				current--;
			}
			if (Input.GetKeyDown (right) && current<inpnum.Length-1) 
			{
				current++;
			}

			if (Input.GetKeyDown (player.exitint)) 
			{
				//print ("exit");
				player.inp ();
				toggled = false;
			}
			if (Input.GetKeyDown (setcode)) 
			{
				//print ("exit");
				unlocked = true;
				int right = 0;
				for(int i=0; i<code.Length; i++)
				{
					if (code [i] != inpnum [i]) 
					{
						unlocked = false;
						//break;
					}
					else
					{
						print ("partright");
						right++;
					}
				}
				//print ("hacked: "+unlocked);
				if(unlocked) 
				{
					reward ();
					tries = 0;
					off ();
				}
				if(tries<2)
				{
					off ();
					print ("failed");
				}
				tries--;
				screentries.text = ""+tries;
				screenright.text = ""+right;
			}
		}
	}

	void off()
	{
		player.inp ();
		toggled = false;
		trywrap.gameObject.SetActive (false);
		hackwrap.gameObject.SetActive (false);
		rightwrap.gameObject.SetActive (false);
	}
	void reward()
	{
		
	}
}

public enum difficulty
{
	easy, medium, hard
}