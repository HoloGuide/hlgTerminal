using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void BtnBack_OnClicked()
    {
        SceneManager.LoadScene("Search");
    }

    public void ListContent_OnClicked()
    {
        // TODO: 次シーンへのルート情報の共有
        SceneManager.LoadScene("Route");
    }

    public void BtnSetting_OnClicked()
    {
        AppManager.Instance.MoveToSetting();
    }
}
