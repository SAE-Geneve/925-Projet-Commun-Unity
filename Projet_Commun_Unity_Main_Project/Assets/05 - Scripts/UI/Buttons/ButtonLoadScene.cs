using UnityEngine;
using UnityEngine.UI;

public class ButtonLoadScene : MonoBehaviour
{
    public void OpenScene(string sceneName)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        SceneLoader.Instance.LoadScene(sceneName);
    }
}
