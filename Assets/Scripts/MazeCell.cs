using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{

    [SerializeField] private GameObject frontGO;
    [SerializeField] private GameObject leftGO;

    private float _x;
    private float _z;
    private struct CellData
    {
        public bool front; // front wall
        public bool left; // left wall
    }

    private CellData _data;

    public void Init(GameObject parent, float x = 0.0f, float z = 0.0f)
    {
        transform.parent = parent.transform;
        transform.position = new Vector3(x, 4.0f, z);


    }
    public void SetWallsGO(GameObject frontWall, GameObject leftWall)
    {
        frontGO = frontWall;
        leftGO = leftWall;
    }

    public void SetFrontColor(Color color)
    {
        if (frontGO != null)
        {
            frontGO.GetComponent<Renderer>().material.color = color;
            return;
        }
        print("Exception: null front wall GO");
    }

    public void SetLeftColor(Color color)
    {
        if (leftGO != null)
        {
            leftGO.GetComponentInChildren<Renderer>().material.color = color;
            return;
        }
        print("Exception: null left wall GO");
    }

    // Methods to set individual fields
    public void SetFront(bool value)
    {
        if(frontGO != null)
        {
            _data.front = value;
            frontGO.SetActive(value);
            return;
        }
        print("Exception: null front wall GO");
    }


    public void SetLeft(bool value)
    {
        if (leftGO != null)
        {
            _data.left = value;
            leftGO.SetActive(value);
            return;
        }
        print("Exception: null left wall GO");
    }



}
