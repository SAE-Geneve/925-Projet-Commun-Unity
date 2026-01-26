using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    //Persists through the days
    public static int TotalMoneyScore = 0;
    public static int TotalTimeCollected = 0;

    //Resets after every mini-game
    public static int TotalMinigameScore = 0;
    //Positive scores 1-2
    public static int Subscore1 = 0;
    public static int Subscore2 = 0;
    //Negative scores 3-4
    public static int Subscore3 = 0;
    public static int Subscore4 = 0;

    void Start()
    {
        //Insures reset occurs when game restarts
        TotalMoneyScore = 0;
        TotalTimeCollected = 0;
        TotalMinigameScore = 0;
        Subscore1 = 0;
        Subscore2 = 0;
        Subscore3 = 0;
        Subscore4 = 0;
    }
    
    public static void MiniGameEnd()
    {
        //Converts score gained to general score
        TotalMoneyScore = TotalMinigameScore/20;
        TotalTimeCollected = TotalMinigameScore/10;
        
        //Reset scores
        Subscore1 = 0;
        Subscore2 = 0;
        Subscore3 = 0;
        Subscore4 = 0;
        TotalMinigameScore = 0;
    }
    
    public static void IncreaseScore(int category)
    {
        switch (category)
        {
            case 1:
                Subscore1++;
                TotalMinigameScore += 150;
                break;
            case 2:
                Subscore2++;
                TotalMinigameScore += 150;
                break;
            case 3:
                Subscore3++;
                break;
            case 4:
                Subscore4++;
                break;
            default:
                break;
        }
    }
}