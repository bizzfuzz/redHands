using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum pooltype
{
	hole,blood,spark,muzzle,magfire
}

public class Pool : MonoBehaviour 
{
	public static Pool shared;
	[SerializeField]Dictionary<pooltype,List<GameObject>> objects;
	[SerializeField] GameObject hole;
	[SerializeField] GameObject blood;
	[SerializeField] GameObject spark;
	[SerializeField] GameObject muzzle;
	[SerializeField] GameObject magfire;
	[SerializeField] bool expand=true;
	//Transform trash;
	List<Poolpref> prefs;
	int basen=50;

	void Awake()
	{
		shared = this;
	}

	void Start () 
	{
		//trash = GameObject.Find ("trash").transform;
		objects = new Dictionary<pooltype,List<GameObject>> {};
		//set init state
		prefs = new List<Poolpref> ();
		prefs.Add (new Poolpref (pooltype.hole, hole, basen));
		prefs.Add (new Poolpref (pooltype.blood, blood, basen));
		prefs.Add (new Poolpref (pooltype.spark, spark, basen));
		prefs.Add (new Poolpref (pooltype.muzzle, muzzle, basen));
		//prefs.Add (new Poolpref (pooltype.magfire, magfire, basen));
		//instantiate prefabs
		for (int i = 0; i < prefs.Count; i++)
		{
			create (prefs[i]);
		}
	}
	void create(Poolpref pref)//take pref and create specified prefab n times
	{
		GameObject obj;
		List<GameObject> ls= getpool (pref.type);
		if(ls==null)
			ls= new List<GameObject>();
		for (int i = 0; i < pref.num; i++) 
		{
			obj = (GameObject)Instantiate(pref.prefab,transform);
			obj.SetActive(false); 
			ls.Add(obj);
		}
		objects [pref.type] = ls;
	}

	List<GameObject> getpool(pooltype type)//get pool for a specific item
	{
		List<GameObject> ls;
		objects.TryGetValue (type, out ls);
		return ls;
	}

	public GameObject getobj(pooltype type) //get instance of an object from pool
	{
		if(!(objects==null)) 
		{
			List<GameObject> ls = getpool (type);
			for (int i = 0; i < ls.Count; i++)
				if (!ls [i].activeInHierarchy)
					return ls [i];
		}
		if(expand)
		{
			for (int i = 0; i < prefs.Count; i++) 
			{
				if(prefs[i].type==type) 
				{
					GameObject obj = Instantiate (prefs [i].prefab, transform);
					obj.SetActive (false); 
					getpool (type).Add (obj);
					return obj;
				}
			}
		}
		return null;
	}
}

public class Poolpref 
{
	public pooltype type;
	public GameObject prefab;
	public int num;

	public Poolpref(pooltype t,GameObject p,int n)
	{
		type = t;
		prefab = p;
		num = n;
	}
}