using UnityEngine;
using UnityEngine.SceneManagement;

public class SearchController : MonoBehaviour
{
    public void BtnSerach_OnClicked()
    {
        // TODO: 目的地の次シーンへの共有
        SceneManager.LoadScene("Time");
    }

    public void BtnSetting_OnClicked()
    {
        AppManager.Instance.MoveToSetting();
    }
}
