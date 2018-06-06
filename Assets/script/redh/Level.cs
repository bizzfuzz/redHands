using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour 
{
	[SerializeField] List<Transform> aiwraps;
	[SerializeField] List<Npc> npcs;
	[SerializeField] List<Ai> guards;

	int lastnpcseen;
	public bool seen=false;
	/*Thief player;
	Transform target;*/

	void Start () 
	{
		int wrap, agent;
		Transform curr;
		Ai ai;
		Npc npc;
		for(wrap=0; wrap<aiwraps.Count; wrap++)
		{
			curr = aiwraps [wrap];
			npcs = new List<Npc> ();
			guards = new List<Ai> ();
			int guardid = 0;
			int npcid = guardid;

			for(agent=0; agent<curr.childCount; agent++)
			{
				ai = curr.GetChild (agent).GetComponent<Ai> ();
				npc = curr.GetChild (agent).GetComponent<Npc> ();

				if(npc)
				{
					//print ("civ");
					npc.id = npcid++;
					npcs.Add (npc);
				}
				else if(ai)
				{
					//print ("guard");
					ai.id = guardid++;
					guards.Add (ai);
				}
			}
		}
		Transform playerwrap = GameObject.FindGameObjectWithTag ("Player").transform;
		/*player = playerwrap.GetComponent < Thief> ();
		target = playerwrap.transform.FindChild ("fpcontrol");*/
	}
	

	public void spotted (int npc) 
	{
		lastnpcseen = npc;
		seen = true;
	}
	public void unseen(int npc)
	{
		if(npc==lastnpcseen) //prevent mark unseen if another can see
		{
			lastnpcseen = -1;
			seen = false;
		}
	}

	public void alert(Vector3 pos)
	{
		Ai closest = nearest (pos);
		if(closest)
			closest.investigate (pos);
	}

	Ai nearest(Vector3 pos)
	{
		Ai closest = null;
		float dist = Mathf.Infinity;
		for(int i = 0; i<guards.Count; i++)
		{
			if(guards[i].dist<dist)
			{
				closest = guards [i];
				dist = closest.dist;
			}
		}
		return closest;
	}
}
