using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
	Text text;
	GameObject accept;//button used to take quests
	public NPC speaker;
	Questmaster player;
	void Start () 
	{
		text = GetComponentInChildren<Text> ();
		accept = transform.FindChild ("accept").gameObject;
		accept.SetActive (false);
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Questmaster> ();
		hide ();
	}
	public void showtext(string line)
	{
		text.text = line;
	}
	public void hide () 
	{
		accept.SetActive (false);
		gameObject.SetActive (false);
	}
	public void show () 
	{
		gameObject.SetActive (true);
	}
	public void showaccept()
	{
		accept.SetActive (true);
	}
	public void givequest()
	{
		player.quests.Add (speaker.job);
	}
}
