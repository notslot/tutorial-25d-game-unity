using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts
{
  public abstract class EditableShape : MonoBehaviour
  {
    #region Inspector

    [SerializeField]
    public bool isClosedShape = true;

    #endregion


    #region Fields

    [HideInInspector]
    public List<Vector3> points = new List<Vector3>();

    #endregion


    #region Methods

    public void AddPoint (Vector3 position)
    {
      points.Add(position);
    }

    public void InsertPoint (int index, Vector3 position)
    {
      points.Insert(index, position);
    }

    public void RemovePoint (int selected)
    {
      points.RemoveAt(selected);
    }

    #endregion
  }
}