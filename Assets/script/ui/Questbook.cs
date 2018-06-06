using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum bookstate
{
	quest,inventory,def
}

public class Questbook : MonoBehaviour 
{
	List<Transform> buttons;
	Inventory invent;
	Questmaster qmaster;
	//labels
	Text title;
	Text info;
	public bookstate state;

	void Start () 
	{
		buttons = new List<Transform> ();
		state = bookstate.def;
		Transform wrapper = transform.FindChild ("context");
		for(int i=0; i<wrapper.childCount;i++)
		{
			buttons.Add (wrapper.GetChild (i));
		}

		Transform player = GameObject.FindGameObjectWithTag ("Player").transform;
		invent = player.GetComponent <Inventory> ();
		title = transform.FindChild ("title").GetComponent<Text> ();
		qmaster = player.GetComponent<Questmaster> ();
		info = transform.FindChild ("info").GetComponent<Text> ();
	}

	void OnEnable()
	{
		if (state == bookstate.inventory)
			showinventory ();
		if (state == bookstate.quest)
			showquests ();
	}
	
	public void showquests () 
	{
		state = bookstate.quest;
		title.text="To Do";
		for(int i=0; i<qmaster.quests.Count;i++)
		{
			buttons[i].GetComponentInChildren<Text> ().text = qmaster.quests[i].title;
			buttons[i].gameObject.SetActive (true);
		}
	}
	public void showquests (int start) 
	{
		state = bookstate.quest;
		refreshcontext ();
		title.text="To Do";
		int bindex = 0;
		for(int i=start; i<qmaster.quests.Count;i++)
		{
			buttons[bindex].GetComponentInChildren<Text> ().text = qmaster.quests[i].title;
			buttons[bindex].gameObject.SetActive (true);
			bindex++;
		}
	}
	public void showinventory () 
	{
		state = bookstate.inventory;
		refreshcontext ();
		title.text="Carry";
		for(int i=0; i<invent.objects.Count;i++)
		{
			buttons[i].GetComponentInChildren<Text> ().text = invstring (invent.objects[i]);
			buttons[i].gameObject.SetActive (true);
		}
	}
	string invstring(Collectable c)
	{
		return string.Format ("{0}x {1} - {2}lbs", c.amount, c.objname, c.weight);
	}
	void refreshcontext()
	{
		for (int i = 0; i < buttons.Count; i++) 
		{
			if (!buttons [i].gameObject.activeSelf)
				break;
			buttons [i].gameObject.SetActive (false);
		}
		info.text="...";
	}
	public void rclick(int index)
	{
		if(state==bookstate.inventory) 
		{
			invent.dropobj (index);
			showinventory ();
		}
	}
	/*public void showinfo(int i)
	{
		if(state==bookstate.quest)
			info.text=qmaster.quests[i].description;
		else if(state==bookstate.inventory)
			info.text=invent.objects[i].description;
	}*/
}
