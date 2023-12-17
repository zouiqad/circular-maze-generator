using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Maze : MonoBehaviour
{
    // Reference of game manager and cell data structure
    private GameManager gameManager;
    private List<List<MazeCell>> cells;

    // Additional variables for maze generation
    private int ringsNB; 
    private bool[][] visited;
    private System.Random rng = new System.Random();

    private int exit;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }


    public void GenerateMazePaths(List<List<MazeCell>> cellsRef)
    {
        cells = cellsRef;
        ringsNB = cells.Count;

        // Initialize visited array
        visited = new bool[cells.Count][];
        for (int i = 0; i < cells.Count; i++)
        {
            visited[i] = new bool[cells[i].Count];
        }

        // Randomly choose an exit in the outermost ring
        int exitSegment = rng.Next(cells[^1].Count);
        exit = exitSegment;
        print(" EXIT : " + (ringsNB - 1) + ", " + exitSegment);
        ColorCell((ringsNB - 1, exitSegment), Color.red);

        // Start the maze generation from the exit
        StartCoroutine(VisitCell(ringsNB - 1, exitSegment));



    }

    private int activeCoroutines = 0;
    private IEnumerator VisitCell(int ring, int segment)
    {
        activeCoroutines++;

        visited[ring][segment] = true;

        yield return new WaitForSeconds(0);
        // Get a list of all neighbors, shuffle it to ensure random order
        List<(int, int)> neighbors = GetUnvisitedNeighbors(ring, segment);
        Shuffle(neighbors);  // Shuffle the list

        foreach(var neighbor in neighbors)
        {
            ColorCell(neighbor, Color.yellow);

        }

        foreach (var neighbor in neighbors)
        {
            // Get neighbor ring and segment
            (int neighborRing, int neighborSegment) = neighbor;

            ColorCell(neighbor, Color.white);

            // If the neighbor has not been visited
            if (!visited[neighborRing][neighborSegment])
            {
                // Remove the wall between the current cell and the chosen cell
                StartCoroutine(RemoveWallBetween(ring, segment, neighborRing, neighborSegment));

                // Recursively visit the chosen cell
                yield return StartCoroutine(VisitCell(neighborRing, neighborSegment));
            }
        }
        activeCoroutines--;
        // If this was the last active coroutine, call the completion method
        if (activeCoroutines == 0)
        {
            CoroutineComplete();
        }
    }

    private void CoroutineComplete()
    {
        // This method is called when all coroutines finish execution
        Debug.Log("All instances of the recursive coroutine have completed.");
        
    }

    private void ColorCell((int, int) cell, Color color)
    {
        cells[cell.Item1][cell.Item2].SetFrontColor(color);
        cells[cell.Item1][cell.Item2].SetLeftColor(color);

    }

    private List<(int, int)> GetUnvisitedNeighbors(int ring, int segment)
    {
        List<(int, int)> neighbors = new List<(int, int)>();
        int currentRing_segmentsNB = GetSegmentCount(ring);

        // Add the inner and outer ring neighbors if they haven't been visited

        // For outer ring neighbors
        if (ring < ringsNB - 2)
        {
            int outerSegmentsNB = GetSegmentCount(ring + 1);

            if (currentRing_segmentsNB != outerSegmentsNB )
            {
                int childSegment = segment * 2;

                if (!visited[ring + 1][childSegment])
                    neighbors.Add((ring + 1, childSegment));
                if (!visited[ring + 1][(childSegment + 1)])
                    neighbors.Add((ring + 1, childSegment + 1));
            } else
            {
                int childSegment = segment;
                if (!visited[ring + 1][childSegment])
                    neighbors.Add((ring + 1, childSegment));
            }

        }

        // For inner ring neighbors
        if (ring > 1)
        {
            int innerSegmentsNB = GetSegmentCount(ring - 1);

            int parentSegment = (currentRing_segmentsNB == innerSegmentsNB) ? segment : segment / 2; // TO FIX

            if (!visited[ring - 1][parentSegment])
                neighbors.Add((ring - 1, parentSegment));
        } else if (ring > 0)
        {
            int parentSegment = 0;
            if (!visited[ring - 1][parentSegment])
                neighbors.Add((ring - 1, parentSegment));
        }

        // Add the left and right segment neighbors in the same ring if they haven't been visited
        if(ring != (ringsNB - 1))
        {
            int leftSegment = (segment - 1 + currentRing_segmentsNB) % currentRing_segmentsNB;
            if (!visited[ring][leftSegment])
                neighbors.Add((ring, leftSegment));

            int rightSegment = (segment + 1) % currentRing_segmentsNB;
            if (!visited[ring][rightSegment])
                neighbors.Add((ring, rightSegment));
        }

        return neighbors;
    }

    public bool FindPathToCenter(int exitSegment)
    {
        // Assuming exit is at the outermost ring
        var start = (ringsNB - 1, exitSegment);

        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue(start);

        visited = new bool[ringsNB][];
        for (int i = 0; i < ringsNB; i++)
        {
            visited[i] = new bool[GetSegmentCount(i)];
        }
        visited[start.Item1][start.Item2] = true;

        while (queue.Count > 0)
        {
            (int ring, int segment) = queue.Dequeue();

            // If we have reached the center ring
            if (ring == 0)
            {
                return true; // Path to the center has been found
            }


            List<(int, int)> neighbors = GetUnvisitedNeighbors(ring, segment);

            foreach (var neighbor in neighbors)
            {

                if (IsPathOpen(ring, segment, neighbor.Item1, neighbor.Item2))
                {
                    visited[neighbor.Item1][neighbor.Item2] = true;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return false; // No path to the center found
    }

    private bool IsPathOpen(int ring1, int segment1, int ring2, int segment2)
    {
        int segmentsNB = GetSegmentCount(ring1);
        // Get the cells at the given positions
        MazeCell cell1 = cells[ring1][segment1].GetComponent<MazeCell>();
        MazeCell cell2 = cells[ring2][segment2].GetComponent<MazeCell>();

        // Determine the position and remove the appropriate walls
        // If they are in the same ring:
        if (ring1 == ring2 && ring1 != ringsNB - 1)
        {
            if (segment1 == (segment2 + 1) % segmentsNB) // cell2 is to the left of cell1
            {
                return cell1.leftGO.activeSelf;

            }
            else if (segment2 == (segment1 + 1) % segmentsNB) // cell1 is to the left of cell2
            {
                return cell2.leftGO.activeSelf;

            }
        }
        // If they are in adjacent rings:
        else
        {
            if (ring1 < ring2) // cell2 is outside cell1
            {
                return cell2.frontGO.activeSelf;

            }
            else // cell1 is outside cell2
            {
                return cell1.frontGO.activeSelf;
            }
        }
        return false;
    }



    private IEnumerator RemoveWallBetween(int ring1, int segment1, int ring2, int segment2)
    {
        int segmentsNB = GetSegmentCount(ring1);
        // Get the cells at the given positions
        MazeCell cell1 = cells[ring1][segment1].GetComponent<MazeCell>();
        MazeCell cell2 = cells[ring2][segment2].GetComponent<MazeCell>();

        // Determine the position and remove the appropriate walls
        // If they are in the same ring:
        if (ring1 == ring2 && ring1 != ringsNB - 1)
        {
            if (segment1 == (segment2 + 1) % segmentsNB) // cell2 is to the left of cell1
            {
                cell1.SetLeftColor(Color.red);
                yield return new WaitForSeconds(0);
                cell1.SetLeft(false);
            }
            else if (segment2 == (segment1 + 1) % segmentsNB) // cell1 is to the left of cell2
            {
                cell2.SetLeftColor(Color.red);
                yield return new WaitForSeconds(0);
                cell2.SetLeft(false);
            }
        }
        // If they are in adjacent rings:
        else
        {
            if (ring1 < ring2) // cell2 is outside cell1
            {
                cell2.SetFrontColor(Color.red);
                yield return new WaitForSeconds(0);
                cell2.SetFront(false);
            }
            else // cell1 is outside cell2
            {
                cell1.SetFrontColor(Color.red);
                yield return new WaitForSeconds(0);
                cell1.SetFront(false);
            }
        }
    }

    // Shuffle method using Fisher-Yates shuffle
    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private int GetSegmentCount(int ringIndex)
    {
        return cells[ringIndex].Count;
    }


}
