using UnityEngine;

// Require components in the attached gameobject if not create them
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CurvedWall : MonoBehaviour
{
    // material
    public Material material;

    // Serialize parameters
    [SerializeField] private float innerRadius = 4.5f; // radius of inner wall (distance from center)
    [SerializeField] private float outerRadius = 5f; // radius of outer wall (distance from center)
    [SerializeField] private float angle = 180f; // angle of the curve
    [SerializeField] private float wallHeight = 3f; // height of the wall
    [SerializeField] private int segments = 20; // how many polygones (LOD)

    // Properties
    public float InnerRadius
    {
        get { return innerRadius; }
        set { innerRadius = value; }
    }
    public float OuterRadius
    {
        get { return outerRadius; }
        set { outerRadius = value; }
    }
    public float Angle
    {
        get { return angle; }
        set { angle = value; }
    }
    public float WallHeight
    {
        get { return wallHeight; }
        set { wallHeight = value; }
    }
    public int Segments
    {
        get { return segments; }
        set { segments = value; }
    }




    public Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh(); // create new mesh
        int verticesCount = (segments + 1) * 4; 
        Vector3[] vertices = new Vector3[verticesCount];
        int[] triangles = new int[segments * 12 * 3 + 12]; // 12 triangles per segment + 12 triangles for the side
        Vector3[] normals = new Vector3[verticesCount]; // normals array one normal per vertex

        float angleStep = angle / segments;
        float radianStep = angleStep * Mathf.Deg2Rad;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = radianStep * i;

            float cosAngle = Mathf.Cos(currentAngle);
            float sinAngle = Mathf.Sin(currentAngle);

            float xInner = sinAngle * innerRadius;
            float zInner = cosAngle * innerRadius;
            float xOuter = sinAngle * outerRadius;
            float zOuter = cosAngle * outerRadius;

            // calculate vertices
            vertices[i * 4] = new Vector3(xInner, 0, zInner);
            vertices[i * 4 + 1] = new Vector3(xInner, WallHeight, zInner);
            vertices[i * 4 + 2] = new Vector3(xOuter, 0, zOuter);
            vertices[i * 4 + 3] = new Vector3(xOuter, WallHeight, zOuter);

            // calculate normals
            Vector3 innerNormal = new Vector3(xInner, 0, zInner).normalized;
            Vector3 outerNormal = new Vector3(xOuter, 0, zOuter).normalized;

            normals[i * 4] = -innerNormal;
            normals[i * 4 + 1] = -innerNormal;
            normals[i * 4 + 2] = outerNormal;
            normals[i * 4 + 3] = outerNormal;

            if (i < segments)
            {
                int baseIndex = i * 12 * 3;
                CreateQuadTriangles(triangles, baseIndex, i * 4, i * 4 + 1, i * 4 + 5, i * 4 + 4); // Inner wall
                CreateQuadTriangles(triangles, baseIndex + 6, i * 4 + 2, i * 4 + 6, i * 4 + 7, i * 4 + 3); // Outer wall
                CreateQuadTriangles(triangles, baseIndex + 12, i * 4 + 1, i * 4 + 3, i * 4 + 7, i * 4 + 5); // Top
                CreateQuadTriangles(triangles, baseIndex + 18, i * 4, i * 4 + 4, i * 4 + 6, i * 4 + 2); // Bottom
            }
        }

        // Fill gaps on the sides of wall
        CreateQuadTriangles(triangles, segments * 12 * 3, 1, 0, 2, 3); // Side 1
        CreateQuadTriangles(triangles, segments * 12 * 3 + 6, verticesCount - 4, verticesCount - 3, verticesCount - 1, verticesCount - 2); // Side 2

        // Assign vertices and triangles to mesh and recalculate normals
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        // Assign the normals to the mesh
        mesh.normals = normals;

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>(); // get the meshfilter componenet of our GO
        meshFilter.mesh = mesh; // set the mesh of the GO 

        // Generate mesh collider 
        MeshCollider collider = gameObject.AddComponent<MeshCollider>(); // add mesh collider to our custom made curved wall
        collider.sharedMesh = mesh;

        return mesh;
    }

    // method for creating triangles for each quad
    private void CreateQuadTriangles(int[] triangles, int index, int v0, int v1, int v2, int v3)
    {
        triangles[index] = v0;
        triangles[index + 1] = v1;
        triangles[index + 2] = v2;
        triangles[index + 3] = v0;
        triangles[index + 4] = v2;
        triangles[index + 5] = v3;
    }
}
