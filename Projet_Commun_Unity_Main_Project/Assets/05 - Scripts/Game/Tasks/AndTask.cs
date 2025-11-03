using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AndTask : GameTask
{
    [SerializeField] private List<GameTask> tasks = new List<GameTask>();
    
    protected override void Start()
    {
        foreach (var task in tasks)
        {
            task.OnSucceedAction += CheckTasks;
        }
    }

    private void CheckTasks()
    {
        if (tasks.Any(task => !task.Done))
            return;
        ResetAllPlayers();
        // ✅ Toutes les tasks sont terminées
        Succeed();

        // Nettoyage
        foreach (var task in tasks)
        {
            task.OnSucceedAction -= CheckTasks;
        }

        // ✅ On fait un Reset() sur tous les Player de la scène
     
    }

    private void ResetAllPlayers()
    {
        // On cherche tous les objets appelés "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            Controller controller = player.GetComponent<Controller>();
            if (controller != null)
            {
                Debug.LogWarning("DROP ALL PLAYERS");
                controller.Dropped();
                controller.Reset();
            }
        }
    }
}