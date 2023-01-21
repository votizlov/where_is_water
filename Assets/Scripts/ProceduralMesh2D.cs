using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMesh2D : MonoBehaviour
{
    public MeshFilter meshFilter;
    public float holeRadius;

    public Vector2 boxSize;
    public int verts = 50;

    void Start()
    {
        // Create a new mesh
        Mesh mesh = new Mesh();

        // Define the vertices of the mesh
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(boxSize.x, 0, 0),
            new Vector3(boxSize.x, boxSize.y, 0),
            new Vector3(0, boxSize.y, 0)
        };

        // Define the triangles of the mesh
        int[] triangles = new int[6]
        {
            0, 1, 2,
            0, 2, 3
        };

        // Assign the vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Create a new mesh filter and set the mesh
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // Create a new mesh renderer and set the material
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));

        // Set the position of the box to fill the screen
        gameObject.transform.position = new Vector3(-boxSize.x / 2, -boxSize.y / 2, 0);
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;

            // Convert the touch position from screen space to world space
            Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 10));

            // Remove the vertices inside the hole radius
            RemoveVerticesInsideRadius(meshFilter.mesh, worldTouchPos, holeRadius);
        }
    }

    void RemoveVerticesInsideRadius(Mesh mesh, Vector3 holePosition, float holeRadius)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector2[] uvs = mesh.uv;

        // Create lists to store the new vertex, triangle and uv data
        List<Vector3> newVertices = new List<Vector3>();
        List<int> newTriangles = new List<int>();
        List<Vector2> newUVs = new List<Vector2>();

        // Iterate through the vertices and remove the ones inside the hole radius
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            Vector2 uv = uvs[i];
            float distance = Vector3.Distance(vertex, holePosition);

            if (distance > holeRadius)
            {
                newVertices.Add(vertex);
                newUVs.Add(uv);
            }
        }

        // Iterate through the triangles and remove the ones inside the hole radius
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int triangleIndex1 = triangles[i];
            int triangleIndex2 = triangles[i + 1];
            int triangleIndex3 = triangles[i + 2];

            if (triangleIndex1 < newVertices.Count && triangleIndex2 < newVertices.Count && triangleIndex3 < newVertices.Count)
            {
                newTriangles.Add(triangleIndex1);
                newTriangles.Add(triangleIndex2);
                newTriangles.Add(triangleIndex3);
            }
        }

        // Assign the new vertex, triangle and uv data to the mesh
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUVs.ToArray();

        // Recalculate the normals and bounds of the mesh
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
