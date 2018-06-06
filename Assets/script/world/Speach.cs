using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speach
{
	public List<string> lines;
	public bool quest;
	public int queststatement;
	Dialog dialogui;
	int dialogind=0;

	public Speach(Dialog ui)
	{
		dialogui = ui;
	}

	public void shownext () 
	{
		dialogui.show ();
		if (dialogind == lines.Count)
			dialogind = 0;
		dialogui.showtext (lines[dialogind]);
		if (quest && dialogind == queststatement)
			dialogui.showaccept ();
		dialogind++;
	}
	
	public void end () 
	{
		dialogui.hide ();
	}
}
