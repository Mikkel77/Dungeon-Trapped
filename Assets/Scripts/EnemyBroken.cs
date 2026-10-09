/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float distance = 10;
    public float angle= 30;
    public float height = 1.0f;
    public Color meshcolor = ConsoleColor.Red;

    Mesh mesh;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Mesh CreateWedgeMesh() {
    mesh = new Mesh();

        int numTriangles = 8;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottemCenter = Vector3.zero;
        Vector3 bottemLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottemRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottemCenter + Vector3.up * height;
        Vector3 topRight = bottemRight + Vector3.up * height;
        Vector3 topLeft = bottemLeft + Vector3.up * height;

        int vert = 0;

		//left side
        vertices[vert++] = bottemCenter; 
        vertices[vert++] = bottemLeft; 
        vertices[vert++] = topLeft;

		vertices[vert++] = topLeft;
		vertices[vert++] = topCenter;
		vertices[vert++] = bottemCenter;

		//right side
        vertices[vert++] = bottemCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottemRight;
        vertices[vert++] = bottemCenter;

		// far side
		vertices[vert++] = bottemLeft;
		vertices[vert++] = bottemRight;
		vertices[vert++] = topRight;

		vertices[vert++] = topRight;
		vertices[vert++] = topLeft;
		vertices[vert++] = bottemLeft;

		// top
		vertices[vert++] = topCenter;
		vertices[vert++] = topLeft;
		vertices[vert++] = topRight;

		// bottom
		vertices[vert++] = bottemCenter;
		vertices[vert++] = bottemRight;
		vertices[vert++] = bottemLeft;

        for(int i = 0; i < numVertices; i++) {
            triangles[i] = i;
		}
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

		returne Mesh;
	}
    private void OnValidate() {
        mesh = CreateWedgeMesh();
	}
    private void OnDrawGizmos() {
        if (mesh) { 
            Gizmos.color = Color.red;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
		}
	}
}
*/
