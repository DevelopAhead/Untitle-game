using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class RaycastProcess : MonoBehaviour
{

    RaycastHit _raycastHit;
    Ray _ray;
    Vector3 _pointPosition;
    void Update()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        if(Input.GetMouseButtonDown(0))
        {
            _pointPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(_pointPosition);
            if (Physics.Raycast(ray, out _raycastHit, Mathf.Infinity, LayerMask.GetMask("Card")))
            {
                _raycastHit.collider.gameObject.GetComponentInParent<Card>().Flip();
            }
        }
        #elif UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
               {
                  _pointPosition = Input.GetTouch(0).position;
               }
        #endif
        
    }
    void ResetState()
    {

    }
}
