using UnityEngine;

public class HingeSync : MonoBehaviour
{
  #region Connstants

  private static readonly int ANGLE = Shader.PropertyToID("_Angle");

  #endregion


  #region Inspector

  [SerializeField]
  private HingeJoint hinge = null;

  [SerializeField]
  private SpriteRenderer spriteRenderer = null;

  #endregion


  #region MonoBehaviour

  private void Update ()
  {
    float angle = -hinge.angle;
    spriteRenderer.material.SetFloat(ANGLE, angle);
  }

  #endregion
}