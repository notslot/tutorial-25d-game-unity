using System;
using UnityEngine;

namespace Common.Scripts
{
  public class BasicCharacter : MonoBehaviour
  {
    #region Constants

    private static readonly int WALK_PROPERTY = Animator.StringToHash("Walk");

    #endregion


    #region Inspector

    [SerializeField]
    private float speed = 2f;

    [Header("Relations")]
    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private Rigidbody physicsBody = null;

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    #endregion


    #region Fields

    private Vector3 _movement;

    #endregion


    #region MonoBehaviour

    private void Update ()
    {
      // Vertical
      float inputY = 0;
      if ( Input.GetKey(KeyCode.UpArrow) )
        inputY = 1;
      else if ( Input.GetKey(KeyCode.DownArrow) )
        inputY = -1;

      // Horizontal
      float inputX = 0;
      if ( Input.GetKey(KeyCode.RightArrow) )
      {
        inputX = 1;
        spriteRenderer.flipX = false;
      }
      else if ( Input.GetKey(KeyCode.LeftArrow) )
      {
        inputX = -1;
        spriteRenderer.flipX = true;
      }

      // Normalize
      _movement = new Vector3(inputX, 0, inputY).normalized;

      animator.SetBool(WALK_PROPERTY,
                       Math.Abs(_movement.sqrMagnitude) > Mathf.Epsilon);
    }

    private void FixedUpdate ()
    {
      physicsBody.velocity = _movement * speed;
    }

    #endregion
  }
}