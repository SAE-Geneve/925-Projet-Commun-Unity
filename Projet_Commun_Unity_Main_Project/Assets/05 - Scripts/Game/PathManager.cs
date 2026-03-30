using System.Collections.Generic;
using UnityEngine;

public enum MiniGameType
{
    LostLuggage,
    ConveyorBelt,
    BorderControl,
    Boarding
}

public enum PathStage
{
    Inactive    = 0,
    Approaching = 1,
    Active      = 2
}

[System.Serializable]
public class PathRoute
{
    public MiniGameType destination;

    [Tooltip("Les segments dans l'ordre FROM hub TO mini-jeu")]
    public PathSegment[] segments;
}

[System.Serializable]
public class PathSegment
{
    public Renderer renderer;

    [Tooltip("Direction du scroll (normalisée automatiquement)")]
    public Vector3 directionToDestination;
}

public class PathManager : MonoBehaviour
{
    [Header("Routes")]
    public PathRoute[] routes;

    [Header("Liaison Mission")]
    public MiniGameType[] missionIndexToType;

    [Header("Scroll Settings")]
    public float scrollSpeed = 1f;
    public float approachingScrollSpeed = 0.5f;

    private Dictionary<int, PathStage> playerStages = new Dictionary<int, PathStage>();
    private MissionManager _missionManager;
    private MiniGameType? _currentDestination = null;
    private PathStage _currentStage = PathStage.Inactive;

    void Start()
    {
        _missionManager = FindObjectOfType<MissionManager>();
        // Éteint tout au départ
        foreach (var route in routes)
            SetRouteVisible(route, false);
    }

    void Update()
    {
        if (_missionManager == null) return;

        int idx = _missionManager.MissionIndex;
        if (idx < 0 || idx >= missionIndexToType.Length) return;

        MiniGameType newDest = missionIndexToType[idx];

        if (_currentDestination != newDest)
        {
            _currentDestination = newDest;
            playerStages.Clear();
            Refresh();
        }

        // Scroll les segments de la route active
        if (_currentDestination != null && _currentStage != PathStage.Inactive)
        {
            foreach (var route in routes)
            {
                if (route.destination != _currentDestination.Value) continue;

                float speed = _currentStage == PathStage.Active
                    ? scrollSpeed
                    : approachingScrollSpeed;

                foreach (var seg in route.segments)
                {
                    if (seg.renderer == null) continue;

                    Material mat = seg.renderer.material;
                    Vector2 scrollDir = WorldDirToUV(seg.directionToDestination);
                    mat.mainTextureOffset += scrollDir * speed * Time.deltaTime;
                }
            }
        }
    }

    public void RegisterPlayer(int playerID, PathStage stage)
    {
        playerStages[playerID] = stage;
        Refresh();
    }

    public void UnregisterPlayer(int playerID)
    {
        playerStages.Remove(playerID);
        Refresh();
    }

    void Refresh()
    {
        if (_currentDestination == null)
        {
            _currentStage = PathStage.Inactive;
            foreach (var route in routes)
                SetRouteVisible(route, false);
            return;
        }

        PathStage highest = PathStage.Approaching;
        foreach (var stage in playerStages.Values)
            if (stage > highest) highest = stage;

        _currentStage = highest;

        foreach (var route in routes)
        {
            if (route.destination == _currentDestination.Value)
            {
                SetRouteVisible(route, true);
                ApplyColor(route, highest);
            }
            else
            {
                SetRouteVisible(route, false);
            }
        }
    }

    void SetRouteVisible(PathRoute route, bool visible)
    {
        foreach (var seg in route.segments)
        {
            if (seg.renderer == null) continue;
            seg.renderer.enabled = visible;
        }
    }

    void ApplyColor(PathRoute route, PathStage stage)
    {
        Color color = GetColor(route.destination, stage);
        foreach (var seg in route.segments)
        {
            if (seg.renderer == null) continue;
            seg.renderer.material.color = color;
        }
    }

    // Convertit une direction 3D world en direction UV pour le scroll
    Vector2 WorldDirToUV(Vector3 dir)
    {
        dir.Normalize();
        // X world → U, Z world → V
        return new Vector2(dir.x, dir.z);
    }

    Color GetColor(MiniGameType type, PathStage stage)
    {
        return (type, stage) switch
        {
            (MiniGameType.LostLuggage,   PathStage.Approaching) => new Color(1f, 0.5f, 0f),
            (MiniGameType.LostLuggage,   PathStage.Active)      => Color.red,
            (MiniGameType.ConveyorBelt,  PathStage.Approaching) => Color.cyan,
            (MiniGameType.ConveyorBelt,  PathStage.Active)      => Color.blue,
            (MiniGameType.BorderControl, PathStage.Approaching) => Color.yellow,
            (MiniGameType.BorderControl, PathStage.Active)      => new Color(1f, 0.6f, 0f),
            (MiniGameType.Boarding,      PathStage.Approaching) => new Color(0.5f, 1f, 0f),
            (MiniGameType.Boarding,      PathStage.Active)      => Color.green,
            _                                                    => Color.white
        };
    }
}