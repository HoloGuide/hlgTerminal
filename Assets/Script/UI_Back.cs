using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class UI_Back : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BackClicked()
    {
        var prevScene = GameObject.Find("UIManager").GetComponent<UIManeger>().PrevSceneName;
        Debug.Log(prevScene);
        SceneManager.LoadScene(prevScene);

    }
}
