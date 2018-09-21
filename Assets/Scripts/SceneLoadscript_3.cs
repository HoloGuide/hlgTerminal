using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadscript_3 : MonoBehaviour
{
    public GameObject Panel;
    public bool A = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Panel_On()
    {
        A = true;
        Panel.SetActive(true);
    }

    public void Panel_Off()
    {
        A = false;
        Panel.SetActive(false);
    }
}
