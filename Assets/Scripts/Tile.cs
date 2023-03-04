using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{    
    public void SetTileColor(Color color)
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = color;
    }
}
