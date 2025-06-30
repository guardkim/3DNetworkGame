using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float YDistance = 20f;
    private Vector3 _initialEulerAngles;

    private void Start()
    {
        _initialEulerAngles = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        if (Target == null)
        {
            // Debug.Log("타겟이 없음 ㅇㅇㅇㅇㅇㅇㅇㅇㅇㅇㅇㅇ");
            // Player player = FindAnyObjectByType<Player>();
            // player.GetAbility<PlayerAbility>()
            // Target = player.transform;
            return;
        }

        Vector3 targetPosition = Target.position;
        targetPosition.y += YDistance;

        transform.position = targetPosition;

        Vector3 targetEulerAngles = Target.eulerAngles;
        targetEulerAngles.x = _initialEulerAngles.x;
        targetEulerAngles.z = _initialEulerAngles.z;
        transform.eulerAngles = targetEulerAngles;
    }
}
