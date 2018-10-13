using UnityEngine;
using UnityEngine.UI;

public class SwitchAnim : MonoBehaviour {

	[Header("SWITCH")]
	public bool isOn;
    public SwitchAnim Switch;
    public Animator switchAnimator;

    private string onTransition = "Switch On";
    private string offTransition = "Switch Off";

    void Start ()
	{
        if (isOn == true)
        {
            switchAnimator.Play(onTransition);
        }

        else
        {
            switchAnimator.Play(offTransition);
        }
    }

	public void AnimateSwitch()
	{
		if (Switch.isOn == true) 
		{
            switchAnimator.Play(offTransition);
            Switch.isOn = false;
        } 

		else if(Switch.isOn == false)
		{
            switchAnimator.Play(onTransition);
            Switch.isOn = true;
        }
	}
}