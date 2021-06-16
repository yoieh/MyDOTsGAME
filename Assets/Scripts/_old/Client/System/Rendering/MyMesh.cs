using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyMesh
{

    public static Mesh Quad()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(0, 100);
        vertices[2] = new Vector3(100, 100);
        vertices[3] = new Vector3(100, 0);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    public static Mesh QuadGrid(int xSize, int ySize)
    {

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4 * (xSize * ySize)];
        Vector2[] uv = new Vector2[4 * (xSize * ySize)];
        int[] triangles = new int[6 * (xSize * ySize)];

        // WTF is tangents???
        // Vector4[] tangents = new Vector4[vertices.Length];
        // Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                int index = i * ySize + j;
                vertices[index * 4 + 0] = new Vector3(i, j);
                vertices[index * 4 + 1] = new Vector3(i, j + 1);
                vertices[index * 4 + 2] = new Vector3(i + 1, j + 1);
                vertices[index * 4 + 3] = new Vector3(i + 1, j);

                // tangents[index * 4 + 0] = tangent;
                // tangents[index * 4 + 1] = tangent;
                // tangents[index * 4 + 2] = tangent;
                // tangents[index * 4 + 3] = tangent;

                // uv[index * 4 + 0] = new Vector2(0, 0);
                // uv[index * 4 + 1] = new Vector2(0, 1);
                // uv[index * 4 + 2] = new Vector2(1, 1);
                // uv[index * 4 + 3] = new Vector2(1, 0);


                // uv[index * 4 + 0] = new Vector2(uv00.x, uv11.y);
                // uv[index * 4 + 1] = new Vector2(uv00.x, uv00.y);
                // uv[index * 4 + 2] = new Vector2(uv11.x, uv00.y);
                // uv[index * 4 + 3] = new Vector2(uv11.x, uv11.y);

                triangles[index * 6 + 0] = index * 4 + 0;
                triangles[index * 6 + 1] = index * 4 + 1;
                triangles[index * 6 + 2] = index * 4 + 2;

                triangles[index * 6 + 3] = index * 4 + 0;
                triangles[index * 6 + 4] = index * 4 + 2;
                triangles[index * 6 + 5] = index * 4 + 3;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    public static Mesh UpdateQuadGridTexture(Mesh mesh, int xSize, int ySize, Texture texture)
    {

        int tileResolution = 32;

        int numTilesPerRow = texture.width / tileResolution;
        int nomRows = texture.height / tileResolution;

        int texWidth = xSize * tileResolution;
        int texHeigth = ySize * tileResolution;



        float amountOfTiles = 8f;
        Vector2 texturePosition = new Vector2(4, 7);

        Vector2 tiling = new Vector2((1f / amountOfTiles), (1f / amountOfTiles));
        Vector2 offset = texturePosition * amountOfTiles;

        Vector2 uv00Pixels = new Vector2(32 * 3, 32 * 7);
        Vector2 uv11Pixels = new Vector2(32 * 4, 32 * 8);

        float textureWidth = texture.width;
        float textureHeight = texture.height;

        Vector2 uv00 = new Vector2(uv00Pixels.x / textureWidth, uv00Pixels.y / textureHeight);
        Vector2 uv11 = new Vector2(uv11Pixels.x / textureWidth, uv11Pixels.y / textureHeight);


        Vector2[] uv = new Vector2[4 * (xSize * ySize)];

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                int index = i * ySize + j;

                uv[index * 4 + 0] = new Vector2(uv00.x, uv11.y);
                uv[index * 4 + 1] = new Vector2(uv00.x, uv00.y);
                uv[index * 4 + 2] = new Vector2(uv11.x, uv00.y);
                uv[index * 4 + 3] = new Vector2(uv11.x, uv11.y);
            }
        }

        mesh.uv = uv;

        return mesh;
    }


}




// OLD not repeating usvs QuadGrid

// Vector3[] vertices = new Vector3[(xSize + 1) * (ySize + 1)];
// Vector2[] uv = new Vector2[vertices.Length];
// int[] triangles = new int[xSize * ySize * 6];

// Vector4[] tangents = new Vector4[vertices.Length];
// Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

// for (int i = 0, y = 0; y <= ySize; y++)
// {
//     for (int x = 0; x <= xSize; x++, i++)
//     {
//         vertices[i] = new Vector3(x, y);
//         uv[i] = new Vector2(x / xSize, y / ySize);
//         tangents[i] = tangent;
//     }
// }

// mesh.vertices = vertices;
// mesh.uv = uv;
// mesh.tangents = tangents;

// for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
// {
//     for (int x = 0; x < xSize; x++, ti += 6, vi++)
//     {
//         triangles[ti] = vi;
//         triangles[ti + 3] = triangles[ti + 2] = vi + 1;
//         triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
//         triangles[ti + 5] = vi + xSize + 2;
//     }
// }

// mesh.triangles = triangles;

// mesh.RecalculateNormals();