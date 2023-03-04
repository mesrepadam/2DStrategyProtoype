using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int column;
    [SerializeField] private int row;
    [Header("Tile Settings")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Color[] tileColors;


    private void Start()
    {
        CreateGrid();
    }


    private void CreateGrid()
    {
        GameObject tileObj;
        Tile tile;
        Vector2 instPos;
        Vector2 instOffset = -new Vector2(column / 2, row / 2);
        int tileColorCounter = 0;

        for(int j = 0; j < row; j++)
        {
            for(int i = 0; i < column; i++)
            {
                instPos = instOffset + new Vector2(i, j);
                tileObj = Instantiate(tilePrefab, instPos, Quaternion.identity, gameObject.transform);
                tile = tileObj.GetComponent<Tile>();
                tile.SetTileColor(tileColors[tileColorCounter % 2]);
                tileColorCounter += 1;
            }
            if(column % 2 == 0)
            {
                tileColorCounter += 1;
            }
        }
    }
}
