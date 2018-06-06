using UnityEngine; 
using System.Collections; 
using System.Collections.Generic; 
using System; 

public class AI:MonoBehaviour
{
	[SerializeField]Transform target;
	public Transform player;
	[SerializeField]GameObject muzzleflash;
	[SerializeField]GameObject blood;
	[SerializeField]GameObject spark;
	[SerializeField]GameObject hole;
	[SerializeField]Transform point;
	[SerializeField]estate state=estate.wander;
	[SerializeField]int dmg;
	public float chaserange= 10.0f;
	[SerializeField]float speed= 5.0f;
	[SerializeField]float attrange=2f;
	[SerializeField]float cooldown=0.7f;
	[SerializeField]float margin=1f;
	[SerializeField]bool melee=false;
	public float earrange=9000f;
	CharacterController cont;
	[SerializeField]Dictionary<Focusswitch,estate> transitions;
	Dictionary<estate, Action > funcs;
	Animator anim;
	Vector3 dir=Vector3.zero;
	float timer;
	float dist;
	bool cooling=false;
	string moving = "moving";
	string shot = "shot";
	string back2idle = "back2idle";

	void Start()
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
		anim = transform.GetChild (0).GetComponent<Animator> ();
		cont = GetComponent<CharacterController> ();
		state = estate.wander;
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		point = transform.FindChild ("point");
		//trash = GameObject.Find ("trash").transform;
		StartCoroutine ("hunt", .05f);
	}
	void Update()
	{
		funcs [state].DynamicInvoke ();
		if(cooling)
		{
			timer -= Time.deltaTime;
			if (timer <= 0f)
				cooling = false;
		}
	}
	IEnumerator hunt(float delay)//start
	{
		while(true)
		{
			yield return new WaitForSeconds (delay);

			dist = (transform.position-player.position).sqrMagnitude;
			if (dist < attrange) 
			{
				anim.SetBool (moving, false);
				target = player;
				switchfocus (eaction.inrange);
			}
			else if(dist<chaserange) 
			{
				anim.SetBool (moving, true);
				target = player;
				switchfocus (eaction.spot);
			}
			else
			{
				anim.SetBool (moving, false);
				switchfocus (eaction.lose);
			}

			//funcs [state].DynamicInvoke ();
		}
	}
	public bool inrange(Transform thing, float range)
	{
		float res = (transform.position-thing.position).sqrMagnitude;
		return res < range;
	}
	public bool inrange(float range)
	{
		float res = (transform.position-player.position).sqrMagnitude;
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
	void wander()
	{
		
	}
	void chase()
	{
		//transform.Translate (Vector3.forward * move_speed * Time.deltaTime);
		transform.LookAt(target);
		dir = transform.forward * speed;
		dir.y=0;
		cont.Move (dir*Time.deltaTime);
	}
	void attack()
	{
		if (cooling)
			return;
		transform.LookAt(target);
		timer = cooldown;
		cooling = true;
		anim.SetTrigger (this.shot);
		GameObject temp; 

		if (!melee && (temp = Pool.shared.getobj(pooltype.muzzle))) 
		{
			temp.transform.position = point.position;
			temp.transform.rotation = Quaternion.identity;
			temp.SetActive(true);
		}
		anim.SetTrigger (back2idle);
		Vector3 shot = target.position +UnityEngine.Random.insideUnitSphere * margin;
		Vector3 shotdir = shot - point.position;
		RaycastHit hit;

		if(Physics.Raycast (point.position,shotdir,out hit,attrange))
		{
			Health hp;
			if(hp=hit.transform.GetComponent<Health> ())
			{
				hp.change (-dmg);
				GameObject temp2; 
				if (temp2 = Pool.shared.getobj(pooltype.blood)) 
				{
					temp2.transform.position = hit.point;
					temp2.transform.rotation = Quaternion.LookRotation (hit.normal);
					temp2.SetActive(true);
				}
			}
			else if(!melee)
			{
				GameObject spark; 
				if (spark = Pool.shared.getobj(pooltype.spark)) 
				{
					spark.transform.position = hit.point;
					spark.transform.rotation = Quaternion.LookRotation (hit.normal);
					spark.SetActive(true);
				}
				GameObject temp2; 
				if (temp2 = Pool.shared.getobj(pooltype.hole)) 
				{
					temp2.transform.position = hit.point;
					temp2.transform.rotation = Quaternion.FromToRotation (Vector3.forward, hit.normal);
					temp2.SetActive(true);
				}
			}
		}
	}
}


