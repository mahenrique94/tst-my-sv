using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelTilemap : MonoBehaviour
{
    public static LevelTilemap Instance { private set; get; }

    private Tilemap tilemap;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        tilemap = GetComponent<Tilemap>();
    }

    public Vector2 GetMinBounds()
    {
        Bounds localBounds = tilemap.localBounds;
        Vector3 worldMin = tilemap.transform.TransformPoint(localBounds.min);
        return worldMin;
    }

    public Vector2 GetMaxBounds()
    {
        Bounds localBounds = tilemap.localBounds;
        Vector3 worldMax = tilemap.transform.TransformPoint(localBounds.max);
        return worldMax;
    }
}
