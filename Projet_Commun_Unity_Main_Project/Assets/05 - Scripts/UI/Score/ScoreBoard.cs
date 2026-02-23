using System.Collections;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private ScoreSlot[] scoreSlots;
    [SerializeField] private ScoreSlot totalScoreSlot;

    [Header("Parameters")] 
    [SerializeField] private float timeBetweenPlayerScores = 1f;

    private PlayerManager _playerManager;
    private ScoreManager _scoreManager;

    public Canvas ScoreBoardCanvas  { get; private set; }

    private void Start()
    {
        _playerManager = PlayerManager.Instance;
        _scoreManager = GameManager.Instance.Scores;
        
        ScoreBoardCanvas = gameObject.GetComponent<Canvas>();
        ScoreBoardCanvas.enabled = false;
    }

    public void SetScores()
    {
        for (int i = 0; i < _playerManager.Players.Count; i++)
        {
            scoreSlots[i].gameObject.SetActive(true);
            scoreSlots[i].SetMissionScore(_scoreManager.MissionScores[i]);
            scoreSlots[i].SetTotalScore(_scoreManager.TotalScores[i]);
        }

        for (int x = _playerManager.Players.Count; x < 4; x++)
        {
            scoreSlots[x].DisabledAppearance();
            scoreSlots[x].gameObject.SetActive(true);
        }
        totalScoreSlot.SetMissionScore(_scoreManager.TotalMissionScore());
    }

    public void Show() => StartCoroutine(ScoreRoutine());

    private IEnumerator ScoreRoutine()
    {
        ScoreBoardCanvas.enabled = true;
        
        for (int i = 0; i < _playerManager.Players.Count; i++)
        {
            yield return new WaitForSeconds(timeBetweenPlayerScores);
            yield return StartCoroutine(scoreSlots[i].ScoreFillRoutine());
        }
        
        yield return new WaitForSeconds(timeBetweenPlayerScores);
        
        ScoreBoardCanvas.enabled = false;
        
        GameManager gm = GameManager.Instance;
        gm.Scores.FillTotalScores();
        gm.SwitchState(GameState.Playing);
    }
}
