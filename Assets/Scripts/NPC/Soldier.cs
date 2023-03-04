using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour, ICanAttack, ICanMove, ICanTakeDamage, ICanSelectable
{
    public int HP => hp;

    [SerializeField] private int damage;
    [SerializeField] private int hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private HealthSlider healthSlider;

    private Color _originalColor;
    private bool _hasDiedAlready;
    private int _originalHP;




    private void Awake()
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        _originalColor = renderer.color;
        _originalHP = hp;
        healthSlider.SetHealthSlider(hp);

    }


    private void OnEnable()
    {
        _hasDiedAlready = false;
        hp = _originalHP;
        healthSlider.SetHealthSlider(hp);
    }


    public void Move(Vector2 targetPos)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine(targetPos));
    }

    public void Attack(ICanTakeDamage target, Vector2 targetPos)
    {
        StopAllCoroutines();
        StartCoroutine(AttackCoroutine(target, targetPos));

    }

    private IEnumerator MoveCoroutine(Vector2 targetPos)
    {
        float randomX = Random.Range(-0.5f, 0.5f);
        float randomY = Random.Range(-0.5f, 0.5f);
        Vector2 targetPosWithOffset = new Vector2(targetPos.x + randomX, targetPos.y + randomY);
        Vector2 direction;

        while(Vector2.Distance(gameObject.transform.position, targetPosWithOffset) > 0.25f)
        {
            direction = (targetPosWithOffset - (Vector2)gameObject.transform.position).normalized;
            gameObject.transform.Translate(moveSpeed * Time.deltaTime * direction, Space.World);
            yield return null;
        }
    }

    private IEnumerator AttackCoroutine(ICanTakeDamage target, Vector2 targetPos)
    {
        float randomX = Random.Range(-0.5f, 0.5f);
        float randomY = Random.Range(-0.5f, 0.5f);
        Vector2 targetPosWithOffset = new Vector2(targetPos.x + randomX, targetPos.y + randomY);
        Vector2 direction;

        while (Vector2.Distance(gameObject.transform.position, targetPosWithOffset) > 0.25f)
        {
            direction = (targetPosWithOffset - (Vector2)gameObject.transform.position).normalized;
            gameObject.transform.Translate(moveSpeed * Time.deltaTime * direction, Space.World);
            yield return null;
        }

        if(target != null)
        {
            target.TakeDamage(damage);
        }
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
        Destroy(gameObject);
    }

    public void Select()
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = Color.green;
    }

    public void Deselect()
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = _originalColor;
    }
}


public enum SoldierType { Soldier1, Soldier2, Soldier3}
