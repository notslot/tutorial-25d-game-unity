using Common.Scripts;
using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
  public abstract class ShapeToolEditor : UnityEditor.Editor
  {
    #region Fields

    private int _selectedPoint = -1;

    private bool _pendingPaint;

    private bool _isEditing;

    #endregion


    #region Properties

    protected EditableShape Shape { get; private set; }

    #endregion


    #region Editor

    protected void OnEnable ()
    {
      Shape = target as EditableShape;
    }

    public void OnSceneGUI ()
    {
      if ( !_isEditing )
        return;

      _pendingPaint = false;
      Event evt = Event.current;

      switch ( evt.type )
      {
        case EventType.Repaint:
          DrawShape();
          break;
        case EventType.Layout:
          HandleUtility.AddDefaultControl(
            GUIUtility.GetControlID(FocusType.Passive));
          break;
        case EventType.MouseDown:
          OnMouseDown(evt);
          break;
        case EventType.MouseDrag:
          OnMouseDrag(evt);
          break;
        case EventType.MouseUp:
          OnMouseUp(evt);
          break;
      }

      if ( !_pendingPaint )
        return;
      HandleUtility.Repaint();
      _pendingPaint = false;
    }

    public override void OnInspectorGUI ()
    {
      base.OnInspectorGUI();

      GUILayout.Space(10);

      if ( _isEditing )
      {
        GUILayout.Label("Tip: right click to remove a point.");

        if ( GUILayout.Button("Attach to ground") )
        {
          AttachToGround();
          HandleUtility.Repaint();
        }

        if ( GUILayout.Button("Bake") )
        {
          Bake();
          _isEditing = false;
        }

        GUILayout.Space(10);
        if ( GUILayout.Button("Clear Shape") )
          Clear();
      }
      else if ( GUILayout.Button("Edit") )
      {
        _isEditing = true;
      }
    }

    #endregion


    #region Methods

    public abstract void Bake ();

    public virtual void Clear ()
    {
      Shape.points.Clear();
    }

    private void OnMouseDown (Event evt)
    {
      if ( evt.modifiers != EventModifiers.None )
        return;

      Vector3 position = GetGroundPos(evt.mousePosition);
      if ( float.IsNegativeInfinity(position.x) )
        return;

      int selected = GetPointIndex(position);
      switch ( evt.button )
      {
        case 0 when selected >= 0:
          _selectedPoint = selected;
          _pendingPaint = true;
          evt.Use();
          break;
        case 0:
          Undo.RecordObject(Shape, "Add Point");

          int overLine = IsOverLine(position);
          if ( overLine >= 0 )
          {
            Shape.InsertPoint(overLine, ToRelative(position));
            _selectedPoint = overLine;
          }
          else
          {
            Shape.AddPoint(ToRelative(position));
            _selectedPoint = Shape.points.Count - 1;
          }

          _pendingPaint = true;
          evt.Use();
          break;
        case 1 when selected >= 0:
          Shape.RemovePoint(selected);
          evt.Use();
          break;
      }
    }

    private void OnMouseDrag (Event evt)
    {
      if ( _selectedPoint < 0 )
        return;

      Vector3 position = GetGroundPos(evt.mousePosition);
      if ( float.IsNegativeInfinity(position.x) )
        return;

      Shape.points[_selectedPoint] = ToRelative(position);
      _pendingPaint = true;
      evt.Use();
    }

    private void OnMouseUp (Event evt)
    {
      _selectedPoint = -1;
      _pendingPaint = true;
    }

    private void DrawShape ()
    {
      // Handles
      for ( int i = 0; i < Shape.points.Count; i++ )
      {
        Handles.color = _selectedPoint == i ? Color.yellow : Color.white;
        Vector3 point = FromRelative(Shape.points[i]);
        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive),
                                point, Quaternion.identity, HandleSize(point),
                                EventType.Repaint);
      }

      // Line
      int count = Shape.points.Count;
      Vector3[] points = new Vector3[Shape.isClosedShape ? count + 1 : count];
      for ( int i = 0; i < Shape.points.Count; i++ )
        points[i] = FromRelative(Shape.points[i]);

      if ( Shape.isClosedShape )
        points[count] = points[0];
      Handles.color = Color.white;
      Handles.DrawPolyLine(points);
    }

    private void AttachToGround ()
    {
      for ( int i = 0; i < Shape.points.Count; i++ )
      {
        Vector3 point = FromRelative(Shape.points[i]);

        // Vector3 ground = GroundUtil.GroundPos(point);
        point.y = 0;
        Shape.points[i] = ToRelative(point);
      }
    }

    private static Vector3 GetGroundPos (Vector2 mouse)
    {
      Ray mouseRay = HandleUtility.GUIPointToWorldRay(mouse);
      if ( !(HandleUtility.RaySnap(mouseRay) is RaycastHit hit) )
        return Vector3.negativeInfinity;

      return hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")
        ? hit.point
        : Vector3.negativeInfinity;
    }

    private int GetPointIndex (Vector3 position)
    {
      for ( int i = 0; i < Shape.points.Count; i++ )
      {
        Vector3 point = Shape.points[i];
        float distance = Vector3.Distance(FromRelative(point), position);
        if ( distance <= HandleSize(position) )
          return i;
      }

      return -1;
    }

    private int IsOverLine (Vector3 position)
    {
      Vector3 point = FromRelative(position);
      for ( int i = 0; i < Shape.points.Count; i++ )
      {
        int nextIndex = (i + 1) % Shape.points.Count;
        Vector3 current = FromRelative(Shape.points[i]);
        Vector3 next = FromRelative(Shape.points[nextIndex]);
        float distance = HandleUtility.DistancePointLine(point, current, next);
        if ( distance <= HandleSize(position) )
          return nextIndex;
      }

      return -1;
    }

    private Vector3 ToRelative (Vector3 src)
    {
      return src - Shape.transform.position;
    }

    private Vector3 FromRelative (Vector3 src)
    {
      return src + Shape.transform.position;
    }

    private static float HandleSize (Vector3 position)
    {
      return HandleUtility.GetHandleSize(position) / 5;
    }

    #endregion
  }
}