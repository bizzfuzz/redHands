using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour {
	[SerializeField]int maxhp=100;
	public int hp;
	[SerializeField]Transform bar;
	[SerializeField]bool draw=false;
	public Objective killwarrant;

	void Start()
	{
		hp = maxhp;
	}

	public void change(int amount)
	{
		hp += amount;
		hp = Mathf.Clamp (hp, 0, maxhp);
		//Debug.Log (((float)hp / (float)maxhp));
		if(draw)
			bar.localScale = new Vector3 (((float)hp / (float)maxhp), 1, 1);
		if(hp<=0 && tag!="Player")
		{
			//Destroy (gameObject);
			if (killwarrant!=null)
				killwarrant.check ();
			gameObject.SetActive (false);
		}
	}
}
