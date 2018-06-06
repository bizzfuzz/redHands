using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{
	public List<Collectable> objects;
	int maxweight=150;
	[SerializeField]int weight=0;
	[SerializeField]int value=0;
	//Questmaster player;
	Thief player;

	void Start () 
	{
		objects = new List<Collectable> ();
		player = transform.GetComponent<Thief> ();
	}
	
	public void addobj(Collectable obj)
	{
		if (weight + obj.weight > maxweight) 
		{
			print ("inv full");
			return;
		}
		bool added = false;
		for(int i=0; i< objects.Count; i++)
		{
			if(objects[i].id==obj.id)
			{
				objects [i].amount++;
				objects [i].weight += obj.weight;
				added = true;
				break;
			}
		}
		if(!added)
			objects.Add (obj);
		weight += obj.weight;
		value += obj.value;
		player.updatecash (value);
		/*for(int i=0; i<player.quests.Count; i++)
		{
			for(int x=0; x<player.quests[i].goals.Count; x++)
			{
				if (player.quests [i].goals [x].itemid == obj.id) 
				{
					player.quests [i].goals [x].current++;
					player.quests [i].goals [x].check ();
					player.quests [i].check ();
				}
			}
		}*/
		obj.gameObject.SetActive (false);
	}
	public void dropobj(int i)
	{
		if (objects [i].amount > 1) 
		{
			objects [i].amount--;
			objects [i].weight -= objects [i].baseweight;
			Collectable temp = Instantiate (objects [i]) as Collectable;
			temp.amount = 1;
			temp.droppos = objects [i].droppos;
			temp.drop ();
		}
		else
		{
			objects [i].drop ();
			objects.Remove (objects [i]);
		}
	}
}
