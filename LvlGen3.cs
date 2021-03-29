using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class LvlGen3 : MonoBehaviour
{

    public List<GameObject> CellTypes = new List<GameObject>();

    public int MapSizeX = 5;
    public int MapSizeZ = 5;

    private string[,] MapArray = new string[0, 0];
    private GameObject[,] CellsArray = new GameObject[0,0];

    public GameObject ground;

    private List<GameObject> Cells = new List<GameObject>();


    [ExecuteInEditMode]
    public void MapGen()
    {
        ClearLog();


        MapArray = new string[MapSizeZ+1, MapSizeX+1];
        CellsArray = new GameObject[MapSizeZ + 1, MapSizeX + 1];

        foreach (GameObject cell in Cells)
        {
            DestroyImmediate(cell);
        }

        mapArrayOnStart();

        MapArrayGen();

        BuildMap();
    }

    private void mapArrayOnStart()
        {
            for (int i = 1; i <= MapSizeZ; i++)
            {
                for (int j = 1; j <= MapSizeX; j++)
                {
                    MapArray[i, j] = "0";
                }
            }
        }

    private void MapArrayGen()
    {
        for (int i = 1; i <= MapSizeZ; i++)
        {
            for (int j = 1; j <= MapSizeX; j++)
            {
                int y = 0;
                Height1(y, i, j);
                //Height2(y, i, j);
                if (MapArray[i, j] == "0")
                {
                    MapArray[i, j] = i.ToString() + '_' + j.ToString() + '_' + y.ToString();
                }
            }
        }
    }

    private void BuildMap()
    {
        for (int i = 1; i <= MapSizeZ; i++)
        {
            for (int j = 1; j <= MapSizeX; j++)
            {
                if (int.Parse(MapArray[i, j].Split('_')[2]) == 0)
                {
                    GameObject Cell = Instantiate(CellTypes[0], new Vector3(j, 0.5f * int.Parse(MapArray[i, j].Split('_')[2]), i), Quaternion.identity, this.gameObject.transform);
                    CellsArray[i, j] = Cell;
                    Cells.Add(Cell);
                    Cell.name = MapArray[i, j];
                }
                if (int.Parse(MapArray[i, j].Split('_')[2]) == 1)
                {
                    GameObject Cell = Instantiate(CellTypes[0], new Vector3(j, 0.5f * int.Parse(MapArray[i, j].Split('_')[2]), i), Quaternion.identity, this.gameObject.transform);
                    CellsArray[i, j] = Cell;
                    Cells.Add(Cell);
                    Cell.name = MapArray[i, j];
                }
            }
        }
    }

    private void Height1(int heightY, int i, int j)
    {
        int heightChance = Random.Range(1, 20);
        heightY = 0;
        if (heightChance == 1)
        {          

            heightY = 1;
            int hillSizeX = Random.Range(2, 4);
            if (j + hillSizeX - 1 >= MapSizeX) { hillSizeX = MapSizeX+1-j; }
            int hillSizeZ = Random.Range(2, 4);
            if (i + hillSizeZ - 1 >= MapSizeZ) { hillSizeZ = MapSizeZ+1-i; }
            for (int a = i; a <= i+hillSizeZ-1; a++)
            {
                for (int b = j; b <= j+hillSizeX-1; b++)
                {
                    Debug.Log(a);
                    Debug.Log(b);
                    MapArray[a, b] = a.ToString() + '_' + b.ToString() + '_' + heightY.ToString();                   
                }
                
            }

            //Debug.Log(MapArray[i, j]);
        }
    }

    private void Height2(int heightY, int i, int j)
    {
        int heightChance = Random.Range(1, 3);
        heightY = 0;
        if (heightChance == 1)
        {          

            heightY = 0;
            int hillSizeX = Random.Range(1, 3);
            if (j + hillSizeX - 1 >= MapSizeX) { hillSizeX = MapSizeX+1-j; }
            int hillSizeZ = Random.Range(1, 3);
            if (i + hillSizeZ - 1 >= MapSizeZ) { hillSizeZ = MapSizeZ+1-i; }
            for (int a = i; a <= i+hillSizeZ-1; a++)
            {
                for (int b = j; b <= j+hillSizeX-1; b++)
                {
                    Debug.Log(a);
                    Debug.Log(b);
                    MapArray[a, b] = a.ToString() + '_' + b.ToString() + '_' + heightY.ToString();                   
                }
                
            }

            //Debug.Log(MapArray[i, j]);
        }
    }

    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
