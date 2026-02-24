using System.Collections.Generic;
using UnityEngine;

public class EventManagerBorder : EventManager
{
    [Header("AI Manager Link")]
    [Tooltip("Glisse ici l'objet qui gère le spawn des IA pour le stopper pendant la panne")]
    [SerializeField] private AIManagerBorder _aiManagerBorder;

    [Header("Probability Settings")]
    [Tooltip("1 chance sur X que l'écran tombe en panne (ex: 3 = 1 chance sur 3)")]
    [SerializeField] private int _oneInXChanceToBreak = 3;

    [Header("Screen Event Settings")]
    [SerializeField] private GameObject _scrambledScreenVisual;
    [SerializeField] private GameObject _warningLogoVisual;
    [SerializeField] private GameTask _screenRepairTask;

    [Header("Conveyor Settings")]
    [SerializeField] private List<GameTask> _restartButtonTasks; 
    [SerializeField] private List<ConveyorBelt> _conveyorBelts;

    private bool _isScreenBroken = false;
    private bool _isConveyorStopped = false;

    private void Start()
    {
        if (_screenRepairTask != null)
        {
            _screenRepairTask.OnSucceedAction += HandleScreenRepaired;
        }

        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedAction += HandleButtonPressed;
        }

        if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(false);
        if (_warningLogoVisual != null) _warningLogoVisual.SetActive(false);
    }

    protected override void TriggerRandomEvent()
    {
        if (_isScreenBroken || _isConveyorStopped) return;

        int diceRoll = Random.Range(0, _oneInXChanceToBreak);

        if (diceRoll == 0)
        {
            BreakScreenAndConveyor();
        }
        else
        {
            Debug.Log("Lancer de dé réussi, l'écran tient le coup cette fois !");
        }
    }

    private void BreakScreenAndConveyor()
    {
        Debug.Log("EVENT: L'écran du scanner est brouillé ! Le tapis s'arrête.");
        _isScreenBroken = true;
        _isConveyorStopped = true;
        
        if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = true;

        foreach (var belt in _conveyorBelts)
        {
            if (belt != null) belt.StopBelt();
        }

        if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(true);
        if (_warningLogoVisual != null) _warningLogoVisual.SetActive(true);

        if (_screenRepairTask != null) 
        {
            _screenRepairTask.ResetTask();
            if (_screenRepairTask.TryGetComponent<Animator>(out Animator anim))
            {
                anim.SetBool("IsBroken", true);
            }
        }

        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) 
            {
                buttonTask.ResetTask();
                if (buttonTask.TryGetComponent<Animator>(out Animator anim))
                {
                    anim.SetBool("IsBroken", false);
                }
            }
        }
    }

    private void HandleScreenRepaired()
    {
        if (_isScreenBroken)
        {
            Debug.Log("ÉTAPE 1 RÉSOLUE: L'écran est réparé ! Il faut relancer le tapis.");
            _isScreenBroken = false;

            if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(false);
            if (_warningLogoVisual != null) _warningLogoVisual.SetActive(false);

            if (_screenRepairTask != null && _screenRepairTask.TryGetComponent<Animator>(out Animator screenAnim))
            {
                screenAnim.SetBool("IsBroken", false);
            }

            foreach (var buttonTask in _restartButtonTasks)
            {
                if (buttonTask != null && buttonTask.TryGetComponent<Animator>(out Animator beltAnim))
                {
                    beltAnim.SetBool("IsBroken", true);
                }
            }
        }
    }

    private void HandleButtonPressed()
    {
        if (_isConveyorStopped)
        {
            if (_isScreenBroken)
            {
                Debug.Log("ERREUR: Impossible de relancer le tapis, l'écran est toujours en panne !");
                foreach (var buttonTask in _restartButtonTasks)
                {
                    if (buttonTask != null) buttonTask.ResetTask();
                }
                return; 
            }

            Debug.Log("ÉTAPE 2 RÉSOLUE: Le tapis redémarre !");
            _isConveyorStopped = false;
            
            if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = false;

            foreach (var belt in _conveyorBelts)
            {
                if (belt != null) belt.StartBelt();
            }

            foreach (var buttonTask in _restartButtonTasks)
            {
                if (buttonTask != null && buttonTask.TryGetComponent<Animator>(out Animator beltAnim))
                {
                    beltAnim.SetBool("IsBroken", false);
                }
            }
        }
    }

    public override void ResetManager()
    {
        base.ResetManager();

        _isScreenBroken = false;
        _isConveyorStopped = false;
        
        if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = false;

        if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(false);
        if (_warningLogoVisual != null) _warningLogoVisual.SetActive(false);

        foreach (var belt in _conveyorBelts)
        {
            if (belt != null) belt.StartBelt();
        }

        if (_screenRepairTask != null) 
        {
            _screenRepairTask.ResetTask();
            if (_screenRepairTask.TryGetComponent<Animator>(out Animator anim))
            {
                anim.SetBool("IsBroken", false);
            }
        }
        
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) 
            {
                buttonTask.ResetTask();
                if (buttonTask.TryGetComponent<Animator>(out Animator anim))
                {
                    anim.SetBool("IsBroken", false);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (_screenRepairTask != null) _screenRepairTask.OnSucceedAction -= HandleScreenRepaired;

        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedAction -= HandleButtonPressed;
        }
    }
}