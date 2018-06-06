using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questmaster : MonoBehaviour 
{
	public List<Quest> quests; //add func info(int i) builds string main objective+others, list progress if collect
	void Start () 
	{
		quests = new List<Quest> ();
	}
}
