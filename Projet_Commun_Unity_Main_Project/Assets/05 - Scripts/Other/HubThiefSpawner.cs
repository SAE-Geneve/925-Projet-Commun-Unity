using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubThiefSpawner : MonoBehaviour
{
    [Header("Thief Settings")]
    [Tooltip("Le Prefab de ton IA Voleur (qui doit tenir le MoneyBagProp)")]
    [SerializeField] private GameObject _thiefPrefab;
    [Tooltip("Les endroits où le voleur peut apparaître dans le Hub")]
    [SerializeField] private List<Transform> _spawnPoints;

    [Header("Spawn Timers")]
    [SerializeField] private float _minSpawnTime = 20f;
    [SerializeField] private float _maxSpawnTime = 40f;

    private GameObject _activeThief;

    private void Start()
    {
        StartCoroutine(ThiefSpawnRoutine());
    }

    private IEnumerator ThiefSpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(_minSpawnTime, _maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
            
            if (_spawnPoints.Count > 0)
            {
                int randomIndex = Random.Range(0, _spawnPoints.Count);
                Transform spawnPoint = _spawnPoints[randomIndex];

                _activeThief = Instantiate(_thiefPrefab, spawnPoint.position, spawnPoint.rotation);
                Debug.Log("Un voleur est apparu dans le Hub !");
            }
            yield return new WaitUntil(() => _activeThief == null);
        }
    }
}