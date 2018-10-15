using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SearchController : MonoBehaviour
{
    public Text Label;
    public InputField InputField_Destination;

    private int clickedCount = 0;

    private void Start()
    {
        if (AppManager.Instance.NavigationState == NavState.Completed)
        {
            // 案内終了と表示
            Label.text = "案内が終了しました。";
            AppManager.Instance.NavigationState = NavState.Ready;
        }
    }

    public void Logo_OnClicked()
    {
        clickedCount++;

        if (clickedCount >= 5)
        {
            AppManager.Instance.OutputDebugLog = true;
        }
    }

    public void BtnSerach_OnClicked()
    {
        var station = InputField_Destination.text;
        if (station == "")
        {
            Label.text = "駅名を入力してください。";
            return;
        }

        AppManager.Instance.Destination = InputField_Destination.text;
    }

    public void BtnSetting_OnClicked()
    {
        AppManager.Instance.MoveToSetting();
    }
}
