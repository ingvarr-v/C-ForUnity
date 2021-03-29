using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RTScontroller : MonoBehaviour
{

    private Vector2 StartPosition;
    public RectTransform SelectionBox;

    public List<GameObject> Units;
    public List<GameObject> SelectedUnits;
    private GameObject manager;


    private void Awake()
    {
        manager = GameObject.Find("manager");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }
        if (Input.GetMouseButton(0))
        {
            UpdateTheSelectionBox(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(1))
        {
            UnitsMove();
        }
    }

    void UpdateTheSelectionBox(Vector2 curMousePos)
    {
        if (!SelectionBox.gameObject.activeInHierarchy)
        {
            SelectionBox.gameObject.SetActive(true);
        }

        float width = curMousePos.x - StartPosition.x;
        float height = curMousePos.y - StartPosition.y;

        SelectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        SelectionBox.anchoredPosition = StartPosition + new Vector2(width / 2, height / 2);
    }

    void ReleaseSelectionBox()
    {
        SelectionBox.gameObject.SetActive(false);

        Vector2 min = SelectionBox.anchoredPosition - (SelectionBox.sizeDelta / 2);
        Vector2 max = SelectionBox.anchoredPosition + (SelectionBox.sizeDelta / 2);

        SelectedUnits.Clear();

        foreach (GameObject unit in Units)
        {
            Vector3 screenPosDown = Camera.main.WorldToScreenPoint(new Vector3(unit.transform.position.x, unit.transform.position.y, unit.transform.position.z));
            Vector3 screenPosUp = Camera.main.WorldToScreenPoint(new Vector3(unit.transform.position.x, unit.transform.position.y+2, unit.transform.position.z));

            int ScreenPosX2 = 0;
            int ScreenPosX1 = 0;

            if (screenPosUp.x >= screenPosDown.x)
            {
                ScreenPosX1 = (int)screenPosDown.x - 10;
                ScreenPosX2 = (int) screenPosUp.x + 10;

            }
            else if(screenPosUp.x < screenPosDown.x)
            {
                ScreenPosX1 = (int)screenPosUp.x - 10;
                ScreenPosX2 = (int)screenPosDown.x + 10;
            }

            bool UnitAdded = false;
            unit.GetComponent<UnitBase>().trig.SetActive(false);

            for (int screenPosY = (int)screenPosDown.y; screenPosY <= (int)screenPosUp.y; screenPosY++)
            {
                for (int screenPosX = ScreenPosX1; screenPosX <= ScreenPosX2; screenPosX++)
                {
                    if (min != max)
                    {
                        if (screenPosX >= min.x && screenPosX <= max.x && screenPosY >= min.y && screenPosY <= max.y)
                        {
                            SelectedUnits.Add(unit);
                            unit.GetComponent<UnitBase>().trig.SetActive(true);
                            UnitAdded = true;
                            break;
                        }
                    }
                    else if (min == max)
                    {                                     
                        if (screenPosX == min.x && screenPosY == min.y)
                        {
                            SelectedUnits.Add(unit);
                            unit.GetComponent<UnitBase>().trig.SetActive(true);
                            UnitAdded = true;
                            break;
                        }
                    }
                }
                if (UnitAdded)
                {
                    break;
                }
            }
        }
    }

    void UnitsMove()
    {

        int LayerMask = 1 << 9;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject RayObj;
            
        if (Physics.Raycast(ray, out hit, 5000, LayerMask))
        {
            RayObj = hit.collider.gameObject;
            if (RayObj.gameObject.tag == "ground")
            {

                int TargetPosListIndex = 0;

                List<Vector3> TargetPositionList = GetPositionList(hit);

                foreach (GameObject unit in SelectedUnits)
                {
                    NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();

                    NavMeshPath path = new NavMeshPath();
                    agent.CalculatePath(TargetPositionList[TargetPosListIndex], path);

                    //if (path.status == NavMeshPathStatus.PathComplete)
                    //{
                        agent.SetDestination(TargetPositionList[TargetPosListIndex]);
                    //}

                    TargetPosListIndex = TargetPosListIndex + 1;                    

                }
            }
        }
        
    }

    private List<Vector3> GetPositionList(RaycastHit hit)
    {
        //Debug.Log("###");

        List<Vector3> PositionList = new List<Vector3>();

        if (SelectedUnits.Count > 1)
        {
            //Vector3 FinalPosition = new Vector3(0, 0, 0);

            //for (int i = 1; i <= 3 ; i++)
            //{
            //    for (int j = 1; j <= 3; j++)
            //    {
            //        if (NavMesh.SamplePosition(hit.point, out NavMeshHit finalHit, 5f , NavMesh.AllAreas))
            //        {
            //            FinalPosition = new Vector3(hit.point.x + j, 0, hit.point.z + i);
            //        }                   
            //        PositionList.Add(FinalPosition);

            //    }

            //}

            PositionList.Add(new Vector3Int((int)hit.point.x, 0, (int)hit.point.z));
            PositionList.Add(new Vector3Int((int)hit.point.x-1, 0, (int)hit.point.z-1));
            PositionList.Add(new Vector3Int((int)hit.point.x, 0, (int)hit.point.z-1));
            PositionList.Add(new Vector3Int((int)hit.point.x+1, 0, (int)hit.point.z-1));
            PositionList.Add(new Vector3Int((int)hit.point.x-1, 0, (int)hit.point.z+1));
            PositionList.Add(new Vector3Int((int)hit.point.x, 0, (int)hit.point.z+1));
            PositionList.Add(new Vector3Int((int)hit.point.x+1, 0, (int)hit.point.z+1));
            PositionList.Add(new Vector3Int((int)hit.point.x-1, 0, (int)hit.point.z));
            PositionList.Add(new Vector3Int((int)hit.point.x+1, 0, (int)hit.point.z));
        }

        if (SelectedUnits.Count == 1)
        {
            Vector3 FinalPosition = new Vector3(0, 0, 0);

            int k = 2;

            for (int i = 0; i < SelectedUnits.Count; i++)
            {             
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit finalHit, k * 2, NavMesh.AllAreas))
                {
                    FinalPosition = finalHit.position;
                }
                PositionList.Add(FinalPosition);
            }
        }

        //for (int i = 0; i <= 6; i++)
        //{
        //    Debug.Log(PositionList[i]);
        //}

        return PositionList;

    }

}
