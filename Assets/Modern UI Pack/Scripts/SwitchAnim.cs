using UnityEngine;
using UnityEngine.UI;

public class SwitchAnim : MonoBehaviour {

	[Header("SWITCH")]
	public bool isOn;
    public SwitchAnim switchAnim;
    public Animator switchAnimator;

    private string onTransition = "Switch On";
    private string offTransition = "Switch Off";

    void Start ()
	{
        if (switchAnim.isOn == true)
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
		if (switchAnim.isOn == true) 
		{
            switchAnimator.Play(offTransition);
            switchAnim.isOn = false;
            isOn = false;
        } 

		else
		{
            switchAnimator.Play(onTransition);
            switchAnim.isOn = true;
            isOn = true;
        }
	}
}