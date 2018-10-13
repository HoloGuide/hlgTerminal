using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting_Load : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
    public void ChangeScene()
    {
        GameObject.Find("UIManager").GetComponent<UIManeger>().PrevSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Setting");
    }
}
