using UnityEngine;

public class TriggerScene : MonoBehaviour
{
    
    [SerializeField] string sceneToLoad;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("loading scene");
        if(other.CompareTag("Player")) SceneLoader.Instance.LoadScene(sceneToLoad);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")) ;
    }
}
