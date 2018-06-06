using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Npc : Ai
{
	[SerializeField] bool stationary;
	[SerializeField] Transform levelwrap;
	Level level;

	public override void Start () 
	{
		base.Start ();
		transitions = new Dictionary<Focusswitch,estate> 
		{
			{ new Focusswitch (estate.wander, eaction.spot), estate.wander },
			{ new Focusswitch (estate.wander, eaction.lose), estate.wander },
			{ new Focusswitch (estate.wander, eaction.witness), estate.attack },
			{ new Focusswitch (estate.wander, eaction.earshot), estate.attack },

			/*{ new Focusswitch (estate.chase, eaction.inrange), estate.attack },
			{ new Focusswitch (estate.chase, eaction.lose), estate.wander },
			{ new Focusswitch (estate.chase, eaction.spot), estate.chase },
			{ new Focusswitch (estate.chase, eaction.earshot), estate.chase },*/

			{ new Focusswitch (estate.attack, eaction.spot), estate.attack },
			{ new Focusswitch (estate.attack, eaction.witness), estate.attack },
			{ new Focusswitch (estate.attack, eaction.lose), estate.wander },
			{ new Focusswitch (estate.attack, eaction.earshot), estate.attack },
		};
		funcs = new Dictionary<estate,Action> 
		{
			{ estate.attack,new Action (attack) },
			{ estate.wander,new Action (wander) },
			//{ estate.watch,new Action (watch) },
		};

		if(stationary)
		{
			nmagent.enabled = false;
			anim.SetBool ("move", false);
		}

		level = levelwrap.GetComponent<Level> ();
		Transform playerwrap = GameObject.FindGameObjectWithTag ("Player").transform;
		gun.parent = transform;
	}

	public override void attack () 
	{
		
	}
	public override void wander () 
	{
		if(stationary)
		{
			
		}
	}

	public override void spotplayer () 
	{
		//print ("npspot");
		seeplayer = true;
		level.spotted (id);//register player visible
	}
	public override void loseplayer () 
	{
		seeplayer = false;
		level.unseen (id);//player out of view
	}

	public override IEnumerator hunt(float delay)
	{
		while (true) 
		{
			yield return new WaitForSeconds (delay);

			if(seeplayer && player.incrime)
				level.alert (target.position);
			
			if(nmagent.enabled && stopped ()) 
			{
				anim.SetBool ("move", false);
				if (state == estate.wander) 
				{
					activedest = true;
					enteractivity ();
				}
			}
		}
	}
}
