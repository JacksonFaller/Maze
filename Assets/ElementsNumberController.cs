using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementsNumberController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddElement(Text textElement)
    {
        int number = int.Parse(textElement.text);
        textElement.text = (number + 1).ToString();
    }

    public void SubstractElement(Text textElement)
    {
        int number = int.Parse(textElement.text);
        if(number == 0) return;
        textElement.text = (number - 1).ToString();
    }
}
