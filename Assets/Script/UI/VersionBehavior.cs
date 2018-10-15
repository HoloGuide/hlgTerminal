using UnityEngine;

public class VersionBehavior : MonoBehaviour
{
    private void Start()
    {
        var version = this.GetComponent<UnityEngine.UI.Text>();
        version.text = AppManager.VERSION;
    }
}
