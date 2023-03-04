using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    [SerializeField] private GameObject[] buildingPrefabs;

    [Header("Placing Color Settings")]
    [SerializeField] private Color canPlaceColor;
    [SerializeField] private Color cannotPlaceColor;

    private bool _isPlacing;
    private GameObject _instBuilding;
    private SpriteRenderer _instRenderer;
    private Color _originColor;
    private Vector3 _worldPosition;
    private ICanPlacable _placableObject;



    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlacing)
        {
            _worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _instBuilding.transform.position = new Vector3(_worldPosition.x, _worldPosition.y, 0);
            SetPlacingColor();
            if (Input.GetMouseButtonDown(0))
            {
                FinishPlace();
            }
            else if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                CancelPlace();
            }
        }
    }


    public void CreateBuilding(int buildingType)
    {
        SelectionManager.Instance.Deselect();
        if(buildingType == 0)
        {
            _instBuilding = PoolManager.Instance.GetBarrackPool().Get();
            _instBuilding.transform.parent = null;
        }
        else
        {
            _instBuilding = PoolManager.Instance.GetPowerPlantPool().Get();
            _instBuilding.transform.parent = null;
        }
        _instRenderer = _instBuilding.GetComponent<SpriteRenderer>();
        _placableObject = _instBuilding.GetComponent<ICanPlacable>();
        _originColor = _instRenderer.color;
        _isPlacing = true;
    }

    public bool CheckPlacing()
    {
        return _isPlacing;
    }

    private void SetPlacingColor()
    {
        if (_placableObject.CheckPlacableWithOverUICheck())
        {
            _instRenderer.color = canPlaceColor;
        }
        else
        {
            _instRenderer.color = cannotPlaceColor;
        }
    }

    private void FinishPlace()
    {
        if (_placableObject.CheckPlacableWithOverUICheck())
        {
            _instRenderer.color = _originColor;
            _instRenderer.rendererPriority = 1;
            float roundX = _instBuilding.transform.lossyScale.x % 2 == 1 ? (float)Math.Round(_worldPosition.x, MidpointRounding.AwayFromZero) : Mathf.Sign(_worldPosition.x) * (Mathf.Abs((int)_worldPosition.x) + 0.5f);
            float roundY = _instBuilding.transform.lossyScale.y % 2 == 1 ? (float)Math.Round(_worldPosition.y, MidpointRounding.AwayFromZero) : Mathf.Sign(_worldPosition.y) * (Mathf.Abs((int)_worldPosition.y) + 0.5f);
            _worldPosition = new Vector2(roundX, roundY);  
            _instBuilding.transform.position = _worldPosition;
        }
        else
        {
            CancelPlace();
        }
        _isPlacing = false;
        _instBuilding = null;
    }


    private void CancelPlace()
    {
        Destroy(_instBuilding);
        _isPlacing = false;
        _instBuilding = null;
    }
}


[SerializeField] public enum BuildingType { Barrack, PowerPlant}
