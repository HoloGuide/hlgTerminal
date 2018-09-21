using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sound : MonoBehaviour
{
    public Slider slider;
    public Text text;
    int Volume;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Volume = (int)(slider.value * 100);
        text.text = "" + Volume + "%";
    }
}
