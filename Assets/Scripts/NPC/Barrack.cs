using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : CanPlacableBuilding, ICanSpawn, ICanTakeDamage, ICanSelectable
{
    public int HP => hp;

    [SerializeField] private int hp;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private HealthSlider healthSlider;

    private bool _hasDiedAlready;
    private int _originalHP;


    private void Awake()
    {
        _originalHP = hp;
        healthSlider.SetHealthSlider(hp);
    }


    private void OnEnable()
    {
        _hasDiedAlready = false;
        hp = _originalHP;
        healthSlider.SetHealthSlider(hp);
    }





    public void ShowSpawnTransform()
    {
        spawnTransform.gameObject.SetActive(true);
    }

    public void HideSpawnTransform()
    {
        spawnTransform.gameObject.SetActive(false);
    }

    public Transform GetSpawnTransform()
    {
        return spawnTransform;
    }

    public void SetSpawnTransform()
    {
        //Auto correct spawn position   ---High Level Implement
    }

    public void UpdateSpawnTransform(Vector2 newPosition)
    {
        spawnTransform.position = newPosition;
    }

    public void TakeDamage(int damage)
    {
        if (!_hasDiedAlready)
        {
            hp -= damage;
            healthSlider.UpdateHealthSlider(hp);
            if (hp <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        _hasDiedAlready = true;
        PoolManager.Instance.GetBarrackPool().Release(gameObject);
    }


    public void Select()
    {
        ShowSpawnTransform();
        SpawningManager.Instance.SetSpawnPos(gameObject);
        SpawningManager.Instance.TurnSpawnMenu();
    }

    public void Deselect()
    {
        HideSpawnTransform();
        SpawningManager.Instance.TurnSpawnMenu();
    }
}