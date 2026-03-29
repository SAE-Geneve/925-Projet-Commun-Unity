using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HoldInteractableTask : GameTask, IInteractable
{
    [Header("Hold Settings")]
    [SerializeField] private float holdDuration = 2f;
    [SerializeField] private float releaseDuration = 1f;

    [Header("UI References")]
    [SerializeField] private Canvas donutCanvas;
    [SerializeField] private Canvas repairCanvas;
    [SerializeField] private Image donutFill;
    [SerializeField] private GameObject donutBackground;

    private PlayerController _currentPlayer;
    private float _progress;
    private bool _isHolding;
    private bool _playerInRange;
    private bool _isActive;

    private ObjectOutline _outline;

    [NonSerialized] public PlayerController PlayerController;

    protected override void Start()
    {
        base.Start();
        _outline = GetComponent<ObjectOutline>();
        SetDonutCanvasVisible(false);
        SetRepairCanvas(false);
        if (donutFill) donutFill.fillAmount = 0f;
        _isActive = false;
    }

    private void Update()
    {
        if (Done || !_isActive) return;

        if (_isHolding)
            _progress += Time.deltaTime / holdDuration;
        else if (_progress > 0f)
            _progress -= Time.deltaTime / releaseDuration;

        _progress = Mathf.Clamp01(_progress);

        if (donutFill) donutFill.fillAmount = _progress;

        if (_progress <= 0f && !_playerInRange)
        {
            SetDonutCanvasVisible(false);
            SetRepairCanvas(true);
        }

        if (_progress >= 1f)
            CompleteHold();
    }

    public string GetPromptText()
    {
        throw new NotImplementedException();
    }

    public void Interact(PlayerController playerController)
    {
        if (Done || !_isActive) return;
        if (_isHolding && _currentPlayer != playerController) return;

        _currentPlayer = playerController;
        _isHolding = true;
        playerController.InteractableGrabbed = this;
        AudioManager.Instance.PlayContinousSfx(AudioManager.Instance.RepairSFX);
    }

    public void InteractEnd()
    {
        _isHolding = false;
        if (_currentPlayer != null)
            _currentPlayer.InteractableGrabbed = null;

        _currentPlayer = null;
        AudioManager.Instance.StopContinousSfx();
    }

    public void AreaEnter()
    {
        _playerInRange = true;
        if (!_isActive) return;
        if (_outline) _outline.EnableOutline();
        SetDonutCanvasVisible(true);
        SetRepairCanvas(false);
    }

    public void AreaExit()
    {
        _playerInRange = false;
        if (_outline) _outline.DisableOutline();
        if (_currentPlayer != null)
            _currentPlayer.InteractableGrabbed = null;

        _isHolding = false;
        _currentPlayer = null;
    }

    public void Activate()
    {
        _isActive = true;
        if (_playerInRange)
        {
            if (_outline) _outline.EnableOutline();
            SetDonutCanvasVisible(true);
        }
    }

    public void Deactivate()
    {
        _isActive = false;
        _isHolding = false;

        if (_currentPlayer != null)
            _currentPlayer.InteractableGrabbed = null;

        _currentPlayer = null;
        if (_outline) _outline.DisableOutline();
        SetDonutCanvasVisible(false);
        SetRepairCanvas(false);
        AudioManager.Instance.StopContinousSfx();
    }

    private void CompleteHold()
    {
        _isHolding = false;
        _progress  = 0f;
        PlayerController = _currentPlayer;

        if (_currentPlayer != null)
            _currentPlayer.InteractableGrabbed = null;

        _currentPlayer = null;

        if (donutFill) donutFill.fillAmount = 0f;
        SetDonutCanvasVisible(false);
        SetRepairCanvas(false);

        Succeed(PlayerController);
    }

    private void SetDonutCanvasVisible(bool visible)
    {
        if (donutCanvas)      donutCanvas.enabled = visible;
        if (donutBackground) donutBackground.SetActive(visible);
    }
    
    private void SetRepairCanvas(bool visible)
    {
        if (repairCanvas)      repairCanvas.enabled = visible;
    }

    public override void ResetTask()
    {
        base.ResetTask();
        _isHolding = false;
        _progress  = 0f;

        if (_currentPlayer != null)
            _currentPlayer.InteractableGrabbed = null;

        _currentPlayer = null;
        if (donutFill) donutFill.fillAmount = 0f;
        if (!_playerInRange || !_isActive)
        {
            SetDonutCanvasVisible(false);
            SetRepairCanvas(false);
        }
    }
}