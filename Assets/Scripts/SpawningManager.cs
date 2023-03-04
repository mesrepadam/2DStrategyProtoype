using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    public static SpawningManager Instance { get; private set; }

    [SerializeField] [NamedSoldier] private GameObject[] soldierPrefabs;
    [SerializeField] private GameObject buttonsPart;

    private Vector2 _spawnPos;
    private Transform _spawnTargetTransform;
    private GameObject _instObject;
    private Soldier _instSoldier;

    private void Awake()
    {
        Instance = this;
    }


    public void SetSpawnPos(GameObject spawnObj)
    {
        var spawner = spawnObj.GetComponent<ICanSpawn>();
        _spawnPos = spawnObj.transform.position;
        _spawnTargetTransform = spawner.GetSpawnTransform();
    }


    public void SpawnSoldiers(int index)
    {
        _instObject = Instantiate(soldierPrefabs[index], _spawnPos, Quaternion.identity, null);
        _instSoldier = _instObject.GetComponent<Soldier>();
        _instSoldier.Move(_spawnTargetTransform.position);
    }

    public void TurnSpawnMenu()
    {
        buttonsPart.SetActive(!buttonsPart.activeInHierarchy);
    }
}
