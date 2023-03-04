
using UnityEngine;

public interface ICanSpawn
{
    void ShowSpawnTransform();
    void HideSpawnTransform();
    Transform GetSpawnTransform();
    void SetSpawnTransform();
    void UpdateSpawnTransform(Vector2 newPosition);
}
