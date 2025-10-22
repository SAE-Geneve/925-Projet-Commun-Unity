using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class ButtonLoadScene : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void OpenScene(string sceneName)
    {
        SceneLoader.Instance.LoadScene(sceneName);
    }
}
