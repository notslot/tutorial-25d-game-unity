using UnityEngine;

namespace Common.Scripts
{
  [RequireComponent(typeof(MeshCollider))]
  public sealed class EditableColliderTool : EditableShape
  {
    #region Inspector

    [SerializeField]
    public float height = 2;

    [SerializeField]
    public bool reverseTriangles = false;

    #endregion
  }
}