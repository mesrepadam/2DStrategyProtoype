using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerPlant : CanPlacableBuilding, ICanTakeDamage, ICanSelectable
{
    public int HP => hp;

    [SerializeField] private int hp;
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
        PoolManager.Instance.GetPowerPlantPool().Release(gameObject);
    }

    public void Select()
    {
        //Not Implemented
    }

    public void Deselect()
    {
        //Not Implemented
    }
}
