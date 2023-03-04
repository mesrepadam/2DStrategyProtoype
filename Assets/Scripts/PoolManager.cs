using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [SerializeField] private GameObject barrackPrefab;
    [SerializeField] private GameObject powerPlantPrefab;

    private ObjectPool<GameObject> barrackPool;
    private ObjectPool<GameObject> powerPlantPool;

    private void Awake()
    {
        Instance = this;
        barrackPool = new ObjectPool<GameObject>(CreateBarrack, TakeBarrackFromPool, ReturnBarrackToPool, null, false, 5, 100);
        powerPlantPool = new ObjectPool<GameObject>(CreatePowerPlant, TakePowerPlantFromPool, ReturnPowerPlantToPool, null, false, 5, 100);
        CreatePreAllocateBarracks();
        CreatePreAllocatePowerPlants();
    }


    private void Start()
    {

    }


    private void CreatePreAllocateBarracks()
    {        
        for(int i = 0; i < 5; i++)
        {
            barrackPool.Release(Instantiate(barrackPrefab));
        }
    }

    private GameObject CreateBarrack()
    {
        GameObject barrack = Instantiate(barrackPrefab);        
        return barrack;
    }


    private void TakeBarrackFromPool(GameObject barrack)
    {
        barrack.SetActive(true);
    }


    private void ReturnBarrackToPool(GameObject barrack)
    {
        barrack.SetActive(false);
        barrack.transform.parent = gameObject.transform.parent;
    }


    public ObjectPool<GameObject> GetBarrackPool()
    {
        return barrackPool;
    }

    private void CreatePreAllocatePowerPlants()
    {
        for (int i = 0; i < 5; i++)
        {
            powerPlantPool.Release(Instantiate(powerPlantPrefab));
        }
    }

    private GameObject CreatePowerPlant()
    {
        GameObject powerPlant = Instantiate(powerPlantPrefab);
        return powerPlant;
    }


    private void TakePowerPlantFromPool(GameObject powerPlant)
    {
        powerPlant.SetActive(true);
    }


    private void ReturnPowerPlantToPool(GameObject powerPlant)
    {
        powerPlant.SetActive(false);
        powerPlant.transform.parent = gameObject.transform.parent;
    }


    public ObjectPool<GameObject> GetPowerPlantPool()
    {
        return powerPlantPool;
    }
}
