using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LvlGenerator1 : MonoBehaviour
{

    public GameObject FloorCell1, FloorCell2, FloorCell3, FloorCell4, FloorCell5, FloorCell6, FloorCell7, FloorCell8, WallCell1, TransitionCell1, DoorFrameCell1, ColumnCell1;
    public NavMeshSurface NavMesh;

    public float NetworkPower=15;

    private float LvlComplexity;

    private float x, z;

    private string[,] MapArray = new string[30, 30];

    private int MapSizeX;
    private int MapSizeZ;

    private int NextRoomSide;

    private int Entrance;
    int[] RealEntrances = new int[10];
    int CellsAdjoin = 0;

    private string[] AllModules = new string[12];
    private string[] Modules = new string[12];
    private string[] ModulesExist = new string[12];
    private string[] ModulesRandomize = new string[12];
    private int ModulesQuantity;

    private int[] K = new int[11];

    void Start()
    {
        ModulesBuild();

        MapBuild();

        NavMesh.BuildNavMesh();
    }

    public void MapBuild()
    {
        MapSizeX = Random.Range(7, 10);
        MapSizeZ = Random.Range(7, 10);

        //Debug.Log(MapSizeX);
        //Debug.Log(MapSizeZ);

        for (int i = 1; i < 30; i++)
        {
            for (int j = 1; j < 30; j++)
            {
                MapArray[i, j] = "0";
            }
        }

        int k = 0;
        int count = 0;

        for (int i = 1; i < 30; i++)
        {
            x = 3;

            for (int j = 1; j < 30; j++)
            {

                if (j >= 0 && j < MapSizeX && i >= 0 && i < MapSizeZ)
                {
                    if (MapArray[i, j] == "0")
                    {
                        

                        int RoomSizeZ = Random.Range(3, 5);
                        int RoomSizeX = Random.Range(3, 5);

                        //if (i + RoomSizeZ > MapSizeZ) { RoomSizeZ = MapSizeZ - i; }
                        //if (j + RoomSizeX > MapSizeX) { RoomSizeX = MapSizeX - j; }

                        for (int i1 = i; i1 < i + RoomSizeZ; i1++)
                        {
                            for (int j1 = j; j1 < j + RoomSizeX; j1++)
                            {
                                MapArray[i1, j1] = $"{ModulesExist[k].Split(';')[0]};{i}.{j};{x};{z}";
                            }

                        }
                        
                        k++;

                        if (k > 11) { break; }
                        
                    }

                    if (k > 11) { break; }

                }

                //Debug.Log(MapArray[i, j]);
                
                    x = x + 3;
                
            }
            
                z = z + 3;
 
        }

        x = 0;
        z = 0;

        for (int i = 1; i < 30; i++)
        {
            x = 3;
            for (int j = 1; j < 30; j++)
            {
                if (MapArray[i, j] != "0")
                {
                    if (MapArray[i, j].Contains("DM"))
                    {
                        Instantiate(FloorCell1, new Vector3(x, 0, z), Quaternion.identity);
                        GameObject.Find("FloorCell1(Clone)").tag = "ground";
                        GameObject.Find("FloorCell1(Clone)").layer = 9;
                        GameObject.Find("FloorCell1(Clone)").name = MapArray[i, j]; //$"{i}.{j}";
                    }
                    if (MapArray[i, j].Contains("SM"))
                    {
                        Instantiate(FloorCell2, new Vector3(x, 0, z), Quaternion.identity);
                        GameObject.Find("FloorCell2(Clone)").tag = "ground";
                        GameObject.Find("FloorCell2(Clone)").layer = 9;
                        GameObject.Find("FloorCell2(Clone)").name = MapArray[i, j]; //$"{i}.{j}";
                    }
                    if (MapArray[i, j].Contains("GM"))
                    {
                        Instantiate(FloorCell3, new Vector3(x, 0, z), Quaternion.identity);
                        GameObject.Find("FloorCell3(Clone)").tag = "ground";
                        GameObject.Find("FloorCell3(Clone)").layer = 9;
                        GameObject.Find("FloorCell3(Clone)").name = MapArray[i, j]; //$"{i}.{j}";
                    }
                    if (MapArray[i, j].Contains("EM"))
                    {
                        Instantiate(FloorCell4, new Vector3(x, 0, z), Quaternion.identity);
                        GameObject.Find("FloorCell4(Clone)").tag = "ground";
                        GameObject.Find("FloorCell4(Clone)").layer = 9;
                        GameObject.Find("FloorCell4(Clone)").name = MapArray[i, j]; //$"{i}.{j}";
                    }
                    if (MapArray[i, j].Contains("CRM"))
                    {
                        Instantiate(FloorCell5, new Vector3(x, 0, z), Quaternion.identity);
                        GameObject.Find("FloorCell5(Clone)").tag = "ground";
                        GameObject.Find("FloorCell5(Clone)").layer = 9;
                        GameObject.Find("FloorCell5(Clone)").name = MapArray[i, j]; //$"{i}.{j}";
                    }
                }
                x = x + 3;
            }
            z = z + 3;
        }

    }


    //public void ComplexityBuild()
    public void ModulesBuild()
    {
        AllModules[0] = "NM";
        AllModules[1] = "DM;Primary";
        AllModules[2] = "SM;Primary";
        AllModules[3] = "CRM;Primary";
        AllModules[4] = "GM;Primary";
        AllModules[5] = "EM;Primary";
        AllModules[6] = "RM"; 
        AllModules[7] = "OM";
        AllModules[8] = "HM";
        AllModules[9] = "CM";
        AllModules[10] = "TM";
        AllModules[11] = "-";

        ModulesQuantity = 0;



        //SM QUANTITY (NETWORK POWER)
        int ServersQ = (int)Mathf.Ceil(NetworkPower / 20);
        //Modules[i] = $"SM;{ServersQ}";

        Debug.Log("Q  " + ServersQ);

        for (int i = 0; i < 11; i++)
        {
            if (AllModules[i].Contains("Primary"))
            {
                if (AllModules[i].Contains("SM"))
                {
                    for (int smCount = i; smCount < i + ServersQ; smCount++)
                    {
                        Modules[smCount] = "SM";
                    }
                    //i = i + ServersQ;
                }
                else if(i>1)
                {
                    Modules[i+ServersQ-1] = AllModules[i].Split(';')[0];
                }
                else
                {
                    Modules[i] = AllModules[i].Split(';')[0];
                }

            }
        }

        //for (int i = 0; i < 11; i++)
        //{
        //    Debug.Log(Modules[i]);
        //}
        

        for (int i = 0; i < 11; i++)
        {
            ModulesRandomize[i] = Modules[i];
            if (ModulesRandomize[i] == null)
            {
                ModulesRandomize[i] = "-";
            }
        }

        for (int t = 0; t < 11; t++)
        {
            int r = Random.Range(t, AllModules.Length);
            string tmp = ModulesRandomize[t];
            ModulesRandomize[t] = ModulesRandomize[r];
            ModulesRandomize[r] = tmp;

            if (ModulesRandomize[t] == null) ModulesRandomize[t] = "-";

            //Debug.Log(ModulesRandomize[t]);
        }

        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                if (ModulesRandomize[j] != "-")
                {
                    ModulesExist[i] = ModulesRandomize[j];
                    ModulesRandomize[j] = "-";
                    break;
                }
            }
            if (ModulesExist[i] == null)
            {
                ModulesExist[i] = "-";
            }
            Debug.Log(ModulesExist[i]);

        }

    }
    
    public void ModulesConditions()
    {
        //for (int i = 0; i < length; i++)
        //{

        //}
    }
}
