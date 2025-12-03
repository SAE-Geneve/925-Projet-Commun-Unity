using UnityEngine;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{
    private void Start() => GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.MenuReset());
}
