using System.Linq;
using Common.Scripts;
using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
  [CustomEditor(typeof(EditableColliderTool))]
  public sealed class ColliderToolEditor : ShapeToolEditor
  {
    #region Properties

    private EditableColliderTool ColliderShape => Shape as EditableColliderTool;

    private MeshCollider MeshCollider => Shape.GetComponent<MeshCollider>();

    #endregion


    #region ShapeToolEditor

    public override void Bake ()
    {
      Vector3 topOffset = new Vector3(0, ColliderShape.height, 0);
      int pointsCount = Shape.points.Count;

      Vector3[] orderedPoints = Shape.points.ToArray();
      if ( ColliderShape.reverseTriangles )
        orderedPoints = orderedPoints.Reverse().ToArray();

      Vector3[] vertices = new Vector3[pointsCount * 2];
      int[] tris = new int[pointsCount * 6];
      for ( int i = 0; i < pointsCount; i++ )
      {
        int currentIndex = i * 2;
        int currentIndexTop = currentIndex + 1;

        Vector3 currentPoint = orderedPoints[i];
        Vector3 currentPointTop = currentPoint + topOffset;
        vertices[currentIndex] = currentPoint;
        vertices[currentIndexTop] = currentPointTop;


        if ( Shape.isClosedShape || i < pointsCount - 1 )
        {
          int nextIndex = (i + 1) % pointsCount * 2;
          int nextIndexTop = nextIndex + 1;

          int baseTri = i * 6;
          tris[baseTri] = currentIndexTop;
          tris[baseTri + 1] = nextIndex;
          tris[baseTri + 2] = currentIndex;
          tris[baseTri + 3] = currentIndexTop;
          tris[baseTri + 4] = nextIndexTop;
          tris[baseTri + 5] = nextIndex;
        }
      }

      Mesh mesh = new Mesh
      {
        vertices = vertices,
        triangles = tris,
        name = "Baked Collider"
      };
      mesh.RecalculateNormals();
      mesh.RecalculateBounds();

      Shape.gameObject.isStatic = true;
      MeshCollider.sharedMesh = mesh;
    }

    public override void Clear ()
    {
      base.Clear();

      MeshCollider.sharedMesh = null;
    }

    #endregion
  }
}