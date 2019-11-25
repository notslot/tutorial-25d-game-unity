using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
  [InitializeOnLoad]
  public class ResetScene
  {
    #region Class

    static ResetScene ()
    {
      SceneView.duringSceneGui -= DrawEditor;
      SceneView.duringSceneGui += DrawEditor;
    }

    #endregion


    #region Methods

    private static void DrawEditor (SceneView sceneView)
    {
      Rect sceneRect = sceneView.position;

      Handles.BeginGUI();
      GUILayout.BeginArea(new Rect(6, sceneRect.height - 47,
                                   sceneRect.width - 12, 19));
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();

      ResetCamera();

      GUILayout.EndHorizontal();
      GUILayout.EndArea();
      Handles.EndGUI();
    }

    private static void ResetCamera ()
    {
      GUIContent content = EditorGUIUtility.IconContent("SceneViewCamera");
      content.text = " Rest Camera";
      content.tooltip = "Set the camera’s to the game’s angle";
      if ( !GUILayout.Button(content) )
        return;

      SceneView view = SceneView.currentDrawingSceneView;
      if ( view == null )
        return;
      view.rotation = Quaternion.Euler(45, 0, 0);
      view.orthographic = false;
    }

    #endregion
  }
}