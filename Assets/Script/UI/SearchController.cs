using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SearchController : MonoBehaviour
{
    public InputField InputField_Destination;

    public void BtnSerach_OnClicked()
    {
        if (InputField_Destination.text == "")
        {
            return;
        }

        AppManager.Instance.Destination = InputField_Destination.text;
    }

    public void BtnSetting_OnClicked()
    {
        AppManager.Instance.MoveToSetting();
    }
}
