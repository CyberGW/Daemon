using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayChosen : MonoBehaviour {
	public Text displayText;
	public Button quest1;
	public Button quest2;
	public Button quest3;

	// Use this for initialization
	void Start () {
		quest1 = quest1.GetComponent<Button> ();
		quest2 = quest2.GetComponent<Button> ();
		quest3 = quest3.GetComponent<Button> ();

		
	}
	
	public void Quest1(){
		displayText.text = "Quest Description1";
	}

	public void Quest2(){
		displayText.text = "Quest Description2";

	}
	public void Quest3(){
		displayText.text = "Quest Description3";

	}
}
