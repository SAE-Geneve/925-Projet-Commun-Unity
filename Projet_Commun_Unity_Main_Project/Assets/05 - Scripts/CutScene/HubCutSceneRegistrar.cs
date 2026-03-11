using UnityEngine;
using UnityEngine.Playables;

public class HubCutSceneRegistrar : MonoBehaviour
{
    [SerializeField] private PlayableDirector startDirector;
    [SerializeField] private PlayableDirector victoryDirector;
    [SerializeField] private PlayableDirector looseDirector;
    [SerializeField] private PlayableDirector endingDirector;

    private void Awake()
    {
        TimelineManager.Instance?.RegisterDirectors(
            startDirector,
            victoryDirector,
            looseDirector,
            endingDirector
        );
    }

}
