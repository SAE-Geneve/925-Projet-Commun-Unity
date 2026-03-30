using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class FloorPathIndicator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float heightOffset = 0.05f;
    [SerializeField] private float recalculateInterval = 0.3f;

    [Header("Line Appearance")]
    [SerializeField] private float startWidth = 0.25f;
    [SerializeField] private float endWidth = 0.08f;
    [SerializeField] private float scrollSpeed = 1.5f;

    private LineRenderer _line;
    private PlayerController _player;
    private MissionManager _missionManager;

    private NavMeshPath _path;
    private float _recalculateTimer;
    private float _scrollOffset;
    private Material _lineMaterialInstance;

    public void Init(PlayerController player, MissionManager missionManager, Color color, Material material)
    {
        _player = player;
        _missionManager = missionManager;
        _path = new NavMeshPath();

        _line = GetComponent<LineRenderer>();
        _line.startWidth = startWidth;
        _line.endWidth = endWidth;
        _line.useWorldSpace = true;
        _line.positionCount = 0;
        _line.numCapVertices = 4;

        if (material != null)
        {
            _lineMaterialInstance = new Material(material);
            if (_lineMaterialInstance.HasProperty("_BaseColor"))
                _lineMaterialInstance.SetColor("_BaseColor", color);
            else if (_lineMaterialInstance.HasProperty("_Color"))
                _lineMaterialInstance.SetColor("_Color", color);
            _line.material = _lineMaterialInstance;
        }

        _recalculateTimer = recalculateInterval;
    }

    private void Update()
    {
        if (_player == null || _missionManager == null) return;

        bool inHub = GameManager.Instance != null && GameManager.Instance.Context == GameContext.Hub;
        Mission current = _missionManager.CurrentMission;
        bool hasMission = current != null && !current.IsLocked;

        if (!inHub || !hasMission)
        {
            _line.positionCount = 0;
            return;
        }

        _scrollOffset -= scrollSpeed * Time.deltaTime;
        if (_lineMaterialInstance != null)
            _lineMaterialInstance.mainTextureOffset = new Vector2(_scrollOffset, 0f);

        _recalculateTimer += Time.deltaTime;
        if (_recalculateTimer >= recalculateInterval)
        {
            _recalculateTimer = 0f;
            RecalculatePath(current.ArrowTarget);
        }
    }

    private void RecalculatePath(Transform target)
    {
        if (target == null) return;

        if (!NavMesh.SamplePosition(_player.transform.position, out NavMeshHit startHit, 2f, NavMesh.AllAreas)) return;
        if (!NavMesh.SamplePosition(target.position, out NavMeshHit endHit, 2f, NavMesh.AllAreas)) return;
        if (!NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, _path)) return;
        if (_path.corners.Length < 2) return;

        _line.positionCount = _path.corners.Length;
        for (int i = 0; i < _path.corners.Length; i++)
            _line.SetPosition(i, _path.corners[i] + Vector3.up * heightOffset);
    }

    private void OnDestroy()
    {
        if (_lineMaterialInstance != null)
            Destroy(_lineMaterialInstance);
    }
}