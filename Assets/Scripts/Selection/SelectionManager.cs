using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    [SerializeField] private Transform selectionAreaTransform;

    private List<GameObject> _selectedList;
    private ICanSelectable _target;
    private Vector2 _targetPosition;
    private Vector2 _startMousePos;
    private Vector2 _endMousePos;
    private Vector2 _currentMousePos;
    private Vector2 _selectionLowerLeft;
    private Vector2 _selectionUpperRight;
    private bool _isSelectionAreaStart;




    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _selectedList = new List<GameObject>();
    }


    private void Update()
    {
        if (!BuildingManager.Instance.CheckPlacing())
        {
            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            {
                _isSelectionAreaStart = true;
                selectionAreaTransform.gameObject.SetActive(true);
                _startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (_isSelectionAreaStart && Input.GetMouseButton(0))
            {
                _currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _selectionLowerLeft = new Vector2(Mathf.Min(_startMousePos.x, _currentMousePos.x), Mathf.Min(_startMousePos.y, _currentMousePos.y));
                _selectionUpperRight = new Vector2(Mathf.Max(_startMousePos.x, _currentMousePos.x), Mathf.Max(_startMousePos.y, _currentMousePos.y));
                selectionAreaTransform.position = _selectionLowerLeft;
                selectionAreaTransform.localScale = _selectionUpperRight - _selectionLowerLeft;
            }

            if (_isSelectionAreaStart && Input.GetMouseButtonUp(0))
            {
                selectionAreaTransform.gameObject.SetActive(false);
                _endMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Select();
            }

            if (Input.GetMouseButtonDown(1))
            {
                TargetSelect();
            }
        }
    }



    private void Select()
    {
        Deselect();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        List<Collider2D> hits = new List<Collider2D>();
        hits.AddRange(Physics2D.OverlapAreaAll(_startMousePos, _endMousePos));
        if(hits.Count > 0 && hits.Exists(member => member.gameObject.TryGetComponent(out ICanMove moveable)))
        {
            hits.RemoveAll(member => member.gameObject.TryGetComponent(out ICanPlacable canPlacable));
            hits.ForEach(SelectFromHit);            
        }
        else if(hits.Count == 1 && hits[0].gameObject.TryGetComponent(out ICanSpawn spawner))
        {
            _selectedList.Add(hits[0].transform.gameObject);
            ICanSelectable canSelectable = hits[0].GetComponent<ICanSelectable>();
            canSelectable.Select();
        }
    }

    private void SelectFromHit(Collider2D hit)
    {
        _selectedList.Add(hit.gameObject);
        ICanSelectable canSelectable = hit.gameObject.GetComponent<ICanSelectable>();
        canSelectable.Select();       
    }

    public void Deselect()
    {
        _isSelectionAreaStart = false;
        selectionAreaTransform.gameObject.SetActive(false);
        ICanSelectable selection;
        foreach(GameObject selectableObject in _selectedList)
        {
            selection = selectableObject.GetComponent<ICanSelectable>();
            selection.Deselect();
        }
        _selectedList.Clear();
    }

    private void TargetSelect()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapBox(worldPos, new Vector2(0.1f, 0.1f), 0);
        if (_selectedList.Count > 0)
        {
            if (_selectedList.TrueForAll(member => member.TryGetComponent(out ICanMove canMovable)))
            {
                if (hit && hit.transform.gameObject.TryGetComponent(out ICanSelectable canSelectable))
                {
                    AttackAllSelectedToTarget(hit.transform.gameObject.GetComponent<ICanTakeDamage>(), hit.transform.position);
                }
                else
                {
                    MoveAllSelectedToTarget(worldPos);
                }
            }
            else if(_selectedList[0].TryGetComponent(out ICanSpawn canSpawn))
            {
                canSpawn.UpdateSpawnTransform(worldPos);
            }
        }
    }


    private void AttackAllSelectedToTarget(ICanTakeDamage target, Vector2 targetPos)
    {
        if (_selectedList.Exists(member => member.GetComponent<ICanTakeDamage>() == target))
        {
            GameObject selfAttacker = _selectedList.Find(member => member.GetComponent<ICanTakeDamage>() == target);
            ICanSelectable selfAttackerSelectable = selfAttacker.GetComponent<ICanSelectable>();
            _selectedList.Remove(selfAttacker);
            selfAttackerSelectable.Deselect();
        }

        foreach (GameObject selectedObject in _selectedList)
        {
            if (selectedObject.TryGetComponent(out ICanAttack attackingNPC))
            {
                attackingNPC.Attack(target, targetPos);
            }
            else
            {
                selectedObject.TryGetComponent(out ICanMove onlyMovingNPC);
                onlyMovingNPC.Move(targetPos);
            }
        }
    }


    private void MoveAllSelectedToTarget(Vector2 targetPos)
    {
        ICanMove movingNPC;
        foreach (GameObject selectedObject in _selectedList)
        {
            movingNPC = selectedObject.GetComponent<ICanMove>();
            movingNPC.Move(targetPos);
        }

    }
}
