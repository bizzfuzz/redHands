using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rng : MonoBehaviour 
{
	int len=100;
	List<int> lootnum;
	int lootindex;
	int lootn;
	List<int> collectnum;
	int colindex;
	int collectmax=4;
	[SerializeField] Transform loot;
	Transform npcs;
	List<int> npcnum;
	int npcindex;
	List<int> chancenum;
	int chanceindex;
	List<int> locknum;
	int lockindex;
	List<int> activnum;
	public int activlen;

	void Awake()
	{
		if(loot!=null)
		{
			lootn = loot.childCount;
			lootnum = new List<int> ();
		}
		collectnum = new List<int> ();
		chancenum = new List<int> ();
		npcs = transform.parent;
		npcnum = new List<int> ();
		locknum = new List<int> ();
		activnum = new List<int> ();
	}

	void refreshactiv()
	{
		for (int i = 0; i < len; i++)
			activnum.Add (Random.Range (0, activlen));
		shuffle (activnum);
	}
	public int getactiv()
	{
		if (activnum.Count <= 0)
			refreshactiv ();
		int ret = activnum [activnum.Count-1];
		activnum.RemoveAt (activnum.Count-1);
		return ret;
	}
	public int getactiv(int nactiv)
	{
		activlen = nactiv;
		return getactiv ();
	}
	void refreshnpc()
	{
		npcindex=0;
		//Debug.Log ("npc: "+npcnum);
		for (int i = 0; i < len; i++)
			npcnum.Add (Random.Range (0, npcs.childCount));
		shuffle (npcnum);
	}
	public int getnpc()
	{
		//Debug.Log ("npc: "+npcnum);
		if (npcnum.Count <= 0)
			refreshnpc ();
		int ret = npcnum [npcindex];
		npcnum.RemoveAt (npcindex);
		npcindex++;
		return ret;
	}

	void refreshlock()
	{
		lockindex=0;
		//Debug.Log ("npc: "+npcnum);
		for (int i = 0; i < len; i++)
			locknum.Add (Random.Range (0, 181));
		shuffle (locknum);
	}
	public int getlock()
	{
		//Debug.Log ("npc: "+npcnum);
		if (locknum.Count <= 0)
			refreshlock ();
		int ret = locknum [lockindex];
		locknum.RemoveAt (lockindex);
		lockindex++;
		return ret;
	}

	void refreshchance()
	{
		chanceindex=0;
		for (int i = 0; i < len; i++)
			chancenum.Add (Random.Range (0, 101));
		shuffle (chancenum);
	}
	public int getchance()
	{
		if (chancenum.Count <= 0)
			refreshchance ();
		int ret = chancenum [chanceindex];
		chancenum.RemoveAt (chanceindex);
		chanceindex++;
		return ret;
	}
	void refreshcollect()
	{
		colindex=0;
		for (int i = 0; i < len; i++)
			collectnum.Add (Random.Range (1, collectmax+1));//ofset upper exclusive
		shuffle (collectnum);
		/*for (int i = 0; i < len; i++)
			Debug.Log (collectnum [i]);*/
	}
	public int getcollect()
	{
		if (collectnum.Count <= 0)
			refreshcollect ();
		int ret = collectnum [colindex];
		collectnum.RemoveAt (colindex);
		colindex++;
		return ret;
	}
	void refreshloot()
	{
		lootindex=0;
		for (int i = 0; i < len; i++)
			lootnum.Add (Random.Range (0, lootn));
		shuffle (lootnum);
	}
	public int getloot()
	{
		if (lootnum.Count <= 0)
			refreshloot ();
		int ret = lootnum [lootindex];
		lootnum.RemoveAt (lootindex);
		lootindex++;
		return ret;
	}

	void shuffle(List<int> alpha)
	{
		for (int i = 0; i < alpha.Count; i++) 
		{
			int temp = alpha[i];
			int randomIndex = Random.Range(i, alpha.Count);
			alpha[i] = alpha[randomIndex];
			alpha[randomIndex] = temp;
		}
	}
}
