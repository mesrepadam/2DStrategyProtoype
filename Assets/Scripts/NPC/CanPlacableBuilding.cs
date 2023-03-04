using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanPlacableBuilding : MonoBehaviour, ICanPlacable
{
    private bool _isPlacable = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.CompareTag("Building"))
        {
            _isPlacable = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isPlacable = true;
    }

    public bool CheckPlacableWithOverUICheck()
    {
        bool isMouseOverUI = EventSystem.current.IsPointerOverGameObject();
        return !isMouseOverUI && _isPlacable;
    }

}
