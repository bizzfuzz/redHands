using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
	public string title;
	public string description;
	NPC source;
	string dialog;
	public  int id;
	public List<Objective> goals;
	Transform collectables;
	[SerializeField]bool complete=false;
	Rng rng;

	public Quest (NPC host) 
	{
		source = host;
		collectables = host.collectables;
		rng = host.rng;
		//Debug.Log (collectables.childCount);
		goals=new List<Objective>();
		description = "";
		Objective obj = killquest ();
		//Objective obj = collectquest ();
		title="quest";
		goals.Add(obj);
	}

	Objective collectquest()
	{
		int choice = rng.getloot();
		Objective obj = new Objective (source.player, collectables.GetChild (choice), objectivetype.collect, rng);
		description += "collect "+obj.amount+" "+collectables.GetChild (choice).name+"\n";
		return obj;
	}

	Objective killquest()
	{
		Transform mark = source.transform.parent.GetChild (rng.getnpc ());
		Objective obj = new Objective (source.player, mark, objectivetype.kill, rng);
		description += "kill "+mark.name+"\n";
		return obj;
	}
	
	public void check () 
	{
		for (int i = 0; i <goals.Count; i++)
		{
			if (!goals [i].bonus && !goals [i].complete)//any obj not a bonus & not complete
			{
				complete = false;
				return;
			}
		}
		complete = true;
	}
}
