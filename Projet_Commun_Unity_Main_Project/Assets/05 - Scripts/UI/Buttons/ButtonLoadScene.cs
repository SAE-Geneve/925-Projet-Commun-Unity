using UnityEngine;
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
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        SceneLoader.Instance.LoadScene(sceneName);
    }
}
