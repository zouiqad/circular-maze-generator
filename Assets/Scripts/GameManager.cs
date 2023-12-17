using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public List<List<MazeCell>> cells;

    [SerializeField] private float curvedWallHeight = 5.0f;
    [SerializeField] private float thickness = 0.5f; // thickness of the wall
    [SerializeField] private float radius = 10.0f; // radius of first ring
    [SerializeField] private int segmentsNB = 8; // number of segments in a ring
    [SerializeField] private int ringsNB = 3;
    [SerializeField] private GameObject leftWallModel;
    [SerializeField] private Material mat;
    [SerializeField] private Maze maze;
    [SerializeField] private float segmentDistanceThreshold;


    private GameObject MazeGO;
    // Start is called before the first frame update
    void Start()
    {
        MazeGO = new GameObject("Maze");

        GenerateRings();
        maze.GenerateMazePaths(cells);

        MazeGO.transform.position += 4.0f * Vector3.down;
    }


    private GameObject CreateCurvedWall(float innerRadius, float outerRadius, float angle, int curvedWallSegments = 20)
    {
        GameObject wallObject = new GameObject("Curved Wall");
        CurvedWall curvedWall = wallObject.AddComponent<CurvedWall>();

        // Set your parameters here
        curvedWall.InnerRadius = innerRadius;
        curvedWall.OuterRadius = outerRadius;
        curvedWall.Angle = angle;
        curvedWall.WallHeight = curvedWallHeight;
        curvedWall.Segments = curvedWallSegments;

        // Generate mesh alongside mesh collider
        curvedWall.GenerateMesh();
        // Assign the material to the curved wall
        curvedWall.GetComponent<MeshRenderer>().material = mat;

        return wallObject;
    }

    private void DrawOuterRing()
    {
        GameObject ringGO = new GameObject($"Ring{ringsNB - 1}"); // parent ring GO
        ringGO.transform.parent = MazeGO.transform;
        List<MazeCell> segments = new List<MazeCell>();
        cells.Add(segments);

        // Curved wall parameters
        float innerRadius = radius * (ringsNB + 1);
        float outerRadius = innerRadius + thickness;


        // Create full curved wall (outer ring)
        segmentsNB /= 2;

        float teta = 360.0f / segmentsNB;
        float wallRotation = 0.0f;
        for (int j = 0; j < segmentsNB; j++)
        {
            // Create parent gameobject that holds cell data
            MazeCell current_cell = new GameObject($"{ringsNB - 1}, {j}").AddComponent<MazeCell>();

            // Create curved wall
            GameObject curvedWall = CreateCurvedWall(innerRadius, outerRadius, teta);

            curvedWall.transform.rotation = Quaternion.Euler(0.0f, wallRotation + curvedWall.transform.rotation.eulerAngles.y, 0.0f);
            curvedWall.transform.position = Vector3.zero;

            wallRotation += teta;
            curvedWall.transform.parent = current_cell.transform;
            current_cell.transform.parent = ringGO.transform;
            current_cell.SetWallsGO(curvedWall, null);
            

            segments.Add(current_cell);
        }
    }

    // Generate the cell where the player spawns (the goal cell to our backtracking algorithm)
    private void GenerateSpawnCell()
    {
        List<MazeCell> spawnRing = new List<MazeCell>();

        MazeCell spawnCell = new GameObject("Spawn cell").AddComponent<MazeCell>();


        spawnCell.SetWallsGO(null, null);
        spawnRing.Add(spawnCell);

        cells.Add(spawnRing);
    }

    private void GenerateRings()
    {
        cells = new List<List<MazeCell>>();

        GenerateSpawnCell();


        for (int i = 1; i < ringsNB; i++)
        {
            GameObject ringGO = new GameObject($"Ring{i - 1}"); // parent ring GO
            ringGO.transform.parent = MazeGO.transform;

            List<MazeCell> segments = new List<MazeCell>();
            cells.Add(segments);

            float teta = 360.0f / segmentsNB; // angle of every wall segment
            float wallRotation = 0.0f; // reset orientation of the wall

            for (int j = 0; j < segmentsNB; j++)
            {
                // Create parent gameobject that holds cell data
                MazeCell current_cell = new GameObject($"{i - 1}, {j}").AddComponent<MazeCell>();


                // Curved wall parameters
                float innerRadius = radius * (i + 1);
                float outerRadius = innerRadius + thickness;

                // Create curved wall
                GameObject curvedWall = CreateCurvedWall(innerRadius, outerRadius, teta);

                curvedWall.transform.rotation = Quaternion.Euler(0.0f, wallRotation + curvedWall.transform.rotation.eulerAngles.y, 0.0f);
                curvedWall.transform.position = Vector3.zero;


                // Create Left wall
                Quaternion orientation = Quaternion.Euler(new Vector3(0.0f, -90.0f + wallRotation, 0.0f));
                Vector3 pos = orientation * new Vector3(radius * (i + 1), 0, 0);
                GameObject leftWall = Instantiate(leftWallModel, pos, orientation);
                leftWall.transform.localScale = new Vector3(radius, curvedWallHeight, 0.5f); // scale left wall
                leftWall.name = "Left Wall";

                current_cell.Init(ringGO, x: pos.x, z: pos.z);

                // Set the parent transform and set wall data
                curvedWall.transform.parent = current_cell.transform;
                leftWall.transform.parent = current_cell.transform;
                current_cell.SetWallsGO(curvedWall, leftWall);

                wallRotation += teta;

                segments.Add(current_cell);
            }

            // Since our rings get bigger we need to double segments count, but first check if 
            // not segments not too close to each others
            if (Mathf.Abs(Mathf.Tan(360.0f / (segmentsNB * 2))) > segmentDistanceThreshold)
            {
                segmentsNB *= 2; 

            }
        }

        DrawOuterRing();
    }

}
