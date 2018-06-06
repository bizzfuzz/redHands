using System.Collections;
using UnityEngine;

public enum objectivetype
{
	collect,travel,kill
}
[System.Serializable]
public class Objective
{
	public string title;
	public string description;
	public int amount;//for collect
	public int current=0;
	public Transform target;//person/thing to go to
	public bool complete;
	public bool bonus;
	public objectivetype type;
	Transform player;
	public float range=50f;
	public int itemid;
	Health targethp;
	Rng rng;

	public Objective(Transform p)
	{
		player = p;
	}
	public Objective(Transform p, Transform t, objectivetype otype, Rng r)
	{
		player = p;
		target = t;
		type = otype;
		rng = r;
		switch(otype)
		{
		case objectivetype.kill:
			targethp = target.GetComponent<Health> ();
				targethp.killwarrant = this;
				break;
			case objectivetype.collect:
				amount = rng.getcollect ();//increase later
				itemid = target.GetComponent<Collectable> ().id;
				break;
		}
	}

	public void check()
	{
		switch (type) 
		{
			case objectivetype.collect:
				if (current >= amount) {//collected required stuff
					complete = true;
				}
				break;
			case objectivetype.travel:
				if ((target.position - player.position).sqrMagnitude < range)//reached dest
					complete = true;
				break;
			case objectivetype.kill:
				if (targethp.hp <= 0)//dead
					complete = true;
				break;
		}
	}
}
