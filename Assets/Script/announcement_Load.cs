using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class announcement_Load : MonoBehaviour {

    public GameObject Announcement;
    public GameObject Option;
    public bool A = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Power()
    {
        A = true;
        if(A == true)
        {
            Option.SetActive(false);
            Announcement.SetActive(true);
        }

    }
}
