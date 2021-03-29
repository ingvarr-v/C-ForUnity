using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class detectEdge : MonoBehaviour
{
    public List<GameObject> terrains = new List<GameObject>();

    public GameObject Selec;

    public Vector3[] Vert = new Vector3[100];

    [ExecuteInEditMode]
    public void Update()
    {

    }


    private void OnDrawGizmos()
    {
        int k = 0;
        bool ok = false;
        Selec = null;

        foreach (GameObject selection in terrains)
        {
            if (Selection.Contains(selection))
            {
                ok = true;
            }
        }
        if (ok == true)
        {
            GameObject SelectedObj = Selection.activeGameObject;

            Selec = SelectedObj;

            for (int i = 0; i < SelectedObj.GetComponent<MeshFilter>().sharedMesh.vertices.Length; i++)
            {
                if (SelectedObj.GetComponent<MeshFilter>().sharedMesh.vertices[i].y < 0.01)
                {
                    Vert[i] = SelectedObj.GetComponent<MeshFilter>().sharedMesh.vertices[i];
                    //Gizmos.color = Color.blue;
                    //Gizmos.DrawWireSphere(SelectedObj.transform.position + SelectedObj.GetComponent<MeshFilter>().sharedMesh.vertices[i], 0.11f);
                }
            }

            if (SelectedObj.name.Contains("StraightTransition"))
            {
                foreach (GameObject cell in terrains)
                {

                    var vertices1 = SelectedObj.GetComponent<MeshFilter>().sharedMesh.vertices;
                    var vertices2 = cell.GetComponent<MeshFilter>().sharedMesh.vertices;

                    for (int i = 0; i < vertices1.Length; i++)
                    {
                        for (int j = 0; j < vertices2.Length; j++)
                        {
                            if (vertices1[i].x == 1 && 1 - vertices2[j].x == 1 && SelectedObj != cell)
                            {
                                if (vertices1[i] == new Vector3(1 - vertices2[j].x, vertices2[j].y, vertices2[j].z))
                                {
                                    Gizmos.color = Color.green;
                                    Gizmos.DrawCube(vertices1[i] + SelectedObj.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                                    Gizmos.color = Color.red;
                                    Gizmos.DrawCube(new Vector3(vertices2[j].x, vertices2[j].y, vertices2[j].z) + cell.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                                }
                            }
                        }
                    }
                }
            }
            if (SelectedObj.name.Contains("OutAngleTransition"))
            {
                foreach (GameObject cell in terrains)
                {
                    

                    var vertices1 = SelectedObj.GetComponent<MeshFilter>().sharedMesh.vertices;
                    var vertices2 = cell.GetComponent<MeshFilter>().sharedMesh.vertices;

                    for (int i = 0; i < vertices1.Length; i++)
                    {
                        k = 0;
                        for (int j = 0; j < vertices2.Length; j++)
                        {
                            //if (cell.name.Contains("StraightTransition"))
                            //{
                            //    if (vertices1[i].x == 0 && 1 - vertices2[j].x == 0 && SelectedObj != cell)
                            //    {
                            //        if (vertices1[i] == new Vector3(1 - vertices2[j].x, vertices2[j].y, vertices2[j].z))
                            //        {
                            //            Gizmos.color = Color.yellow;
                            //            Gizmos.DrawCube(vertices1[i] + SelectedObj.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                            //            Gizmos.color = Color.red;
                            //            Gizmos.DrawCube(new Vector3(vertices2[j].x, vertices2[j].y, vertices2[j].z) + cell.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                            //        }
                            //    }
                            //    if (SelectedObj != cell)
                            //    {

                            //        if (vertices1[i].z == vertices2[j].x)
                            //        {
                            //            if (vertices1[i].y < vertices2[j].y + 0.01 && vertices1[i].y > vertices2[j].y - 0.01)
                            //            {
                            //                Gizmos.color = Color.green;
                            //                Gizmos.DrawCube(vertices1[i] + SelectedObj.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                            //                Gizmos.color = Color.cyan;
                            //                Gizmos.DrawCube(new Vector3(vertices2[j].x, vertices2[j].y, vertices2[j].z) + cell.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                            //            }
                            //        }
                            //    }
                            //}
                            if (cell.name.Contains("InAngleTransition"))
                            {
                                if (vertices1[i].x < 0 + 0.1 && vertices1[i].x > 0 - 0.1 && vertices2[j].x < 1 + 0.1 && vertices2[j].x > 1 - 0.1 && SelectedObj != cell)
                                {
                                    if (vertices1[i].y < vertices2[j].y + 0.01 && vertices1[i].y > vertices2[j].y - 0.01)
                                    {
                                        Gizmos.color = Color.yellow;
                                        Gizmos.DrawCube(vertices1[i] + SelectedObj.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                                        Gizmos.color = Color.red;
                                        Gizmos.DrawCube(new Vector3(vertices2[j].x, vertices2[j].y, vertices2[j].z) + cell.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                                    }
                                }
                                if (SelectedObj != cell)
                                {

                                    if (vertices1[i].z < 0 + 0.1 && vertices1[i].z > 0 - 0.1 && vertices2[j].z < -1 + 0.1 && vertices2[j].z > -1 - 0.1 && SelectedObj != cell)
                                    {
                                        if (vertices1[i].y < vertices2[j].y + 0.01 && vertices1[i].y > vertices2[j].y - 0.01)
                                        {
                                            Gizmos.color = Color.green;
                                            Gizmos.DrawCube(vertices1[i] + SelectedObj.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                                            Gizmos.color = Color.cyan;
                                            Gizmos.DrawCube(new Vector3(vertices2[j].x, vertices2[j].y, vertices2[j].z) + cell.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //if (k != 0) { Debug.Log(cell.name); }
                }
                
            }
            
            if (SelectedObj.name.Contains("InAngleTransition"))
            {
                foreach (GameObject cell in terrains)
                {

                    var vertices1 = SelectedObj.GetComponent<MeshFilter>().sharedMesh.vertices;
                    var vertices2 = cell.GetComponent<MeshFilter>().sharedMesh.vertices;

                    for (int i = 0; i < vertices1.Length; i++)
                    {
                        for (int j = 0; j < vertices2.Length; j++)
                        {
                            if (cell.name.Contains("StraightTransition"))
                            {
                                if (vertices1[i].z < -1 + 0.1 && vertices1[i].z > -1 - 0.1 && vertices2[j].x == 1 && cell.name.Contains("InAngleTransition") == false)
                                {
                                    if (vertices1[i].y < vertices2[j].y + 0.01 && vertices1[i].y > vertices2[j].y - 0.01)
                                    {
                                        Gizmos.color = Color.yellow;
                                        Gizmos.DrawCube(vertices1[i] + SelectedObj.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                                        Gizmos.color = Color.red;
                                        Gizmos.DrawCube(new Vector3(vertices2[j].x, vertices2[j].y, vertices2[j].z) + cell.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                                    }
                                }
                                if (cell.name.Contains("InAngleTransition") == false)
                                {

                                    if (vertices1[i].x < 1 + 0.1 && vertices1[i].x > 1 - 0.1 && vertices2[j].x == 0)
                                    {
                                        if (vertices1[i].y < vertices2[j].y + 0.01 && vertices1[i].y > vertices2[j].y - 0.01)
                                        {
                                            Gizmos.color = Color.green;
                                            Gizmos.DrawCube(vertices1[i] + SelectedObj.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                                            Gizmos.color = Color.cyan;
                                            Gizmos.DrawCube(new Vector3(vertices2[j].x, vertices2[j].y, vertices2[j].z) + cell.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < Vert.Length; i++)
            {
                Vert[i] = new Vector3(0,0,0);
            }
            k = 0;
        }

    }

}
