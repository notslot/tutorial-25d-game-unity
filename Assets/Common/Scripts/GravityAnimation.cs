using System;
using UnityEngine;

namespace Common.Scripts
{
  public class GravityAnimation : MonoBehaviour
  {
    #region Inspector

    [SerializeField]
    private bool yZCorrelation = false;

    #endregion


    #region Fields

    private Vector3 _origin;

    #endregion


    #region MonoBehaviour

    private void Start ()
    {
      _origin = transform.position;
    }

    private void Update ()
    {
      float amount = Mathf.Sin(Time.time) + 1;

      Vector3 offset = Vector3.zero;
      offset.y = amount;
      if ( yZCorrelation )
        offset.z = amount;

      transform.position = _origin + offset;
    }

    #endregion
  }
}