using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SearchController : MonoBehaviour
{
    public InputField InputField_Destination;

    public void BtnSerach_OnClicked()
    {
        AppManager.Instance.Destination = InputField_Destination.text;
        SceneManager.LoadScene("Time");
    }

    public void BtnSetting_OnClicked()
    {
        AppManager.Instance.MoveToSetting();
    }
}
