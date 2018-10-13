using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Back : MonoBehaviour {

    public GameObject Announcement;
    public GameObject Option;
    public bool B = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Power()
    {
        B = true;
        if(B == true)
        {
            Option.SetActive(true);
            Announcement.SetActive(false);
        }
    }
}
