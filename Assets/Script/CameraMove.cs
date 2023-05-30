using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject Target;               // 카메라가 따라다닐 타겟

    private float offsetX = 0f;            // 카메라의 x좌표
    private float offsetY = 30.0f;           // 카메라의 y좌표
    private float offsetZ = -10.0f;          // 카메라의 z좌표
    private float CameraSpeed = 10.0f;       // 카메라의 속도

    Vector3 TargetPos;                      // 타겟의 위치

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameObject.FindWithTag("Player"))
        {
            return;
        }

        // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }
}
