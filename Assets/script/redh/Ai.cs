using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Ai : MonoBehaviour 
{
	[SerializeField] protected Dictionary<Focusswitch,estate> transitions;
	protected Dictionary<estate, Action > funcs;
	public Transform target;
	public estate state=estate.wander;
	public float chaserange= 500f;
	[SerializeField]float speed= 5.0f;
	[SerializeField]float stoprange=20;
	[SerializeField]float attrange=100f;
	[SerializeField]float cooldown=0.7f;
	[SerializeField]float margin=1f;
	[SerializeField]bool melee=false;
	public float earrange=9000f;
	bool cooling=false;
	float timer;
	public float dist;
	Fov fov;
	protected NavMeshAgent nmagent;
	[SerializeField] protected bool seeplayer = false;

	protected Animator anim;
	protected Transform gun;
	public Transform gunhold;

	protected Thief player;
	Transform gunpoint;
	int playerlayer;
	[SerializeField] int dmg=1;
	Vector3 voff = new Vector3(0,1,0);

	[SerializeField] List<Activity> activities;
	[SerializeField] float activitywait = 10;
	[SerializeField] float activitytimer;
	Rng rng;
	int curractiv;
	[SerializeField] bool patrol=false;
	Vector3 activpos;
	[SerializeField] protected bool activedest;
	[SerializeField] bool destset=false;

	GameObject viewbox;
	GameObject viewboxlit;
	[SerializeField] List<Transform> route;
	int routepoint;
	[SerializeField] Transform routewrap;
	[SerializeField] int noisethreshold;
	Transform stage;
	bool sitting=false;

	public int id;
	public bool hostile = false;
	[SerializeField] float runspd=10f;
	bool running=false;

	public virtual void Start () 
	{
		transitions = new Dictionary<Focusswitch,estate> 
		{
			{ new Focusswitch (estate.wander, eaction.spot), estate.chase },
			{ new Focusswitch (estate.wander, eaction.lose), estate.wander },
			{ new Focusswitch (estate.wander, eaction.inrange), estate.attack },
			{ new Focusswitch (estate.wander, eaction.earshot), estate.chase },

			{ new Focusswitch (estate.chase, eaction.inrange), estate.attack },
			{ new Focusswitch (estate.chase, eaction.lose), estate.wander },
			{ new Focusswitch (estate.chase, eaction.spot), estate.chase },
			{ new Focusswitch (estate.chase, eaction.earshot), estate.chase },

			{ new Focusswitch (estate.attack, eaction.spot), estate.chase },
			{ new Focusswitch (estate.attack, eaction.inrange), estate.attack },
			{ new Focusswitch (estate.attack, eaction.lose), estate.wander },
			{ new Focusswitch (estate.attack, eaction.earshot), estate.attack },
		};
		funcs = new Dictionary<estate,Action> 
		{
			{ estate.attack,new Action (attack) },
			{ estate.chase,new Action (chase) },
			{ estate.wander,new Action (wander) },
		};

		Transform playerwrap = GameObject.FindGameObjectWithTag ("Player").transform;
		player = playerwrap.GetComponent < Thief> ();
		target = playerwrap.transform.FindChild ("fpcontrol");
		fov = transform.GetComponent<Fov> ();
		fov.speed = speed*2;
		fov.enabled = false;
		nmagent = GetComponent<NavMeshAgent> ();
		nmagent.speed = speed;
		nmagent.stoppingDistance = stoprange;
		anim = transform.GetChild (0).GetComponent<Animator> ();
		gun = transform.FindChild ("gun");
		gunpoint = gun.FindChild ("point");
		//gunhold = transform.FindChild ("ghold");
		gun.parent = gunhold;
		gun.localPosition = Vector3.zero;
		gun.localRotation = Quaternion.identity;
		gun.gameObject.SetActive (false);
		playerlayer = LayerMask.NameToLayer ("player");
		//Transform activwrap = GameObject.Find ("activities").transform;
		stage = transform.parent.parent;
		Transform activwrap = stage.FindChild ("activities");

		if(activwrap) 
		{
			activities = new List<Activity> ();
			for (int i = 0; i < activwrap.childCount; i++)
				activities.Add (activwrap.GetChild (i).GetComponent<Activity> ());
			rng = transform.GetComponent<Rng> ();
			rng.activlen = activities.Count;
			//pickactivity ();
		}
		/*rng = transform.GetComponent<Rng> ();
		rng.activlen = activities.Count;*/
		viewbox = transform.FindChild ("viewbox").gameObject;
		viewboxlit = transform.FindChild ("viewboxlit").gameObject;

		if(patrol)
		{
			//Transform routewrap = transform.FindChild ("patrol");
			routepoint = 0;
			route = new List<Transform> ();
			//print ("rlen - "+routewrap.childCount);
			for (int i = 0; i < routewrap.childCount; i++)
				route.Add (routewrap.GetChild (i));
		}

		pickactivity ();
		StartCoroutine ("hunt", .1f);
	}

	void pickactivity()
	{
		if (destset || !nmagent.enabled)
			return;
		nmagent.stoppingDistance = 1;
		//activitywait = 10;
		//activitytimer = activitywait;
		activedest = false;
		destset = true;
		//print (activities[curractiv]);

		if(patrol)
		{
			//print (route+" - "+routepoint);
			activpos = route [routepoint].position;
			routepoint++;
			if (routepoint >= route.Count)
				routepoint = 0;
			activitytimer = activitywait;
		}
		else 
		{
			curractiv = rng.getactiv ();
			activpos = activities [curractiv].transform.position;
			if (activities [curractiv].activetime > 0)
				activitytimer = activities [curractiv].activetime;
			else
				activitytimer = activitywait;
		}
		//print (activities[curractiv]);
		nmagent.destination = activpos;
		anim.SetBool ("move", true);
	}

	void exitwander()
	{
		if (sitting)
			stand ();
		nmagent.stoppingDistance = stoprange;
		activedest = false;
		destset = false;
	}

	public void investigate(Vector3 pos)
	{
		exitwander ();
		anim.SetBool ("move", true);
		hostile = true;
		nmagent.destination = pos;
		destset = true;
		nmagent.stoppingDistance = 1;
	}

	void Update () 
	{
		funcs [state].DynamicInvoke ();
		if(cooling)
		{
			timer -= Time.deltaTime;
			if (timer < 0f)
				cooling = false;
		}
		if(activedest)
		{
			activitytimer -= Time.deltaTime;
			if (activitytimer < 0f) 
			{
				destset = false;
				//print ("updatestand");
				if (sitting)
					stand ();
				pickactivity ();
			}
		}
		/*if (player.noise > noisethreshold)
			nmagent.destination = player.transform.position;*/
	}

	public virtual IEnumerator hunt(float delay)//start
	{
		while(true)
		{
			yield return new WaitForSeconds (delay);

			dist = (transform.position-target.position).sqrMagnitude;
			//print (dist);
			if (hostile) 
			{
				if (seeplayer && dist < attrange) 
				{
					cooling = true;
					anim.SetBool ("att", true);
					gun.gameObject.SetActive (true);
					fov.target = target;
					fov.enabled = true;
					exitwander ();
					switchfocus (eaction.inrange);
				}
				else if (seeplayer && dist < chaserange) 
				{
					anim.SetBool ("att", false);
					spotplayer ();
				}
				else 
				{
					if(running)
					{
						nmagent.speed = speed;
						running = false;
					}
					fov.target = null;
					fov.enabled = false;
					seeplayer = false;
					timer = cooldown;
					cooling = false;
					switchfocus (eaction.lose);
				}
			}
			if(nmagent.enabled && stopped ()) 
			{
				anim.SetBool ("move", false);
				if (state == estate.wander) 
				{
					activedest = true;
					enteractivity ();
				}
			}
			if(player.lit)
			{
				viewbox.SetActive (false);
				viewboxlit.SetActive (true);
			}
			else
			{
				viewbox.SetActive (true);
				viewboxlit.SetActive (false);
			}
		}
	}

	public void enteractivity()
	{
		if (patrol)
			return;
		if(activities[curractiv].sit)
		{
			transform.position = activities [curractiv].aiparent.position;
			//print ("rout sit: "+nmagent.remainingDistance);
			sit ();
		}
		else if(activities[curractiv].sleep)
		{
			transform.position = activities [curractiv].aiparent.position;
			//print ("rout sit: "+nmagent.remainingDistance);
			sleep ();
		}
	}

	void sleep()
	{
		nmagent.enabled = false;
		sitting = true;
		transform.localEulerAngles = activities [curractiv].rot;
	}

	void sit()
	{
		//print ("sit");
		nmagent.enabled = false;
		sitting = true;
		transform.localEulerAngles = activities [curractiv].rot;
		anim.SetBool ("sitting", sitting);
	}

	void stand()
	{
		//print ("stand");
		sitting = false;
		anim.SetBool ("sitting", false);
		transform.position = activities [curractiv].transform.position;
		anim.SetBool ("move", true);
		nmagent.enabled = true;
	}

	public bool stopped()
	{
		if (nmagent.pathPending)
			return false;
		return (nmagent.remainingDistance < nmagent.stoppingDistance);
	}

	public virtual void spotplayer()
	{
		if (player.incrime)
			hostile = true;
		if(hostile) 
		{
			exitwander ();
			nmagent.speed = runspd;
			running = true;
			anim.SetBool ("move", true);
			fov.target = null;
			fov.enabled = false;
			seeplayer = true;
			timer = cooldown;
			cooling = false;
			//exitwander ();
			switchfocus (eaction.spot);
		}
	}

	public virtual void loseplayer () 
	{
		//seeplayer = false;
	}

	public virtual void wander()
	{
		if (!destset)
			pickactivity ();
		if (player.noise > noisethreshold) 
		{
			print ("ai noise");
			if (sitting)
				stand ();
			nmagent.destination = target.position;
		}
	}
	void chase()
	{
		if (sitting)
			stand ();
		nmagent.destination = target.position;
	}

	public virtual void attack()
	{
		if (cooling)
			return;
		timer = cooldown;
		cooling = true;
		//anim.SetTrigger (this.shot);
		GameObject temp; 

		if (!melee && (temp = Pool.shared.getobj(pooltype.muzzle))) //move to separate func, no need to repeat
		{
			temp.transform.position = gunpoint.position;
			temp.transform.rotation = transform.rotation;
			temp.SetActive(true);
		}
		//anim.SetTrigger (back2idle);
		Vector3 shot = (target.position + voff) + UnityEngine.Random.insideUnitSphere * margin;//cache vars
		shot -= gunpoint.position;
		RaycastHit hit;

		if(Physics.Raycast (gunpoint.position,shot,out hit,attrange))
		{
			if(hit.transform.gameObject.layer==playerlayer)
			{
				//print ("shot");
				player.damage (dmg);
				if (temp = Pool.shared.getobj(pooltype.blood)) 
				{
					temp.transform.position = hit.point;
					temp.transform.rotation = Quaternion.LookRotation (hit.normal);
					temp.SetActive(true);
				}
			}
			else if(!melee)
			{
				if (temp = Pool.shared.getobj(pooltype.spark)) 
				{
					temp.transform.position = hit.point;
					temp.transform.rotation = Quaternion.LookRotation (hit.normal);
					temp.SetActive(true);
				}
				if (temp = Pool.shared.getobj(pooltype.hole)) 
				{
					temp.transform.position = hit.point;
					temp.transform.rotation = Quaternion.FromToRotation (Vector3.forward, hit.normal);
					temp.SetActive(true);
				}
			}
		}
	}

	public bool inrange(Transform thing, float range)
	{
		float res = (transform.position-thing.position).sqrMagnitude;
		return res < range;
	}
	public bool inrange(float range)
	{
		float res = (transform.position-target.position).sqrMagnitude;
		//Debug.Log ("inrange "+res);
		return res < range;
	}
	public void switchfocus(eaction act)
	{
		Focusswitch focus = new Focusswitch (state, act);
		estate next=estate.wander;
		//Debug.Log (state+" : "+act+" | "+transitions);
		if(!(transitions==null || transitions.TryGetValue(focus, out next))) 
		{
			Debug.Log("Invalid transition: " + state + " -> " + act);
			return;
		}
		if (next == state)//too many prints, combine above done testing
			return;
		//Debug.Log (transform.name+" sw: "+state+" ("+act+") -> "+next);
		state = next;
	}
}

public class Focusswitch
{
	readonly estate state;
	readonly eaction action;
	public Focusswitch(estate s,eaction a)
	{
		state=s;
		action=a;
	}

	public override int GetHashCode()
	{
		return 17 + 31 * state.GetHashCode() + 31 *action.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		Focusswitch other = obj as Focusswitch;
		return other != null && this.state == other.state && this.action == other.action;
	}
}

public enum estate
{
	chase,attack,wander,watch,flee
};
public enum eaction
{
	spot,lose,inrange,earshot,witness
};