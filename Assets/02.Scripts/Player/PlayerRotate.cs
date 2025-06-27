using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerRotate : PlayerAbility
{
    // 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전 시키고 싶다.

    public Transform CameraRoot;

    private float _mx;
    private float _my;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!_photonView.IsMine) return;

        CinemachineCamera cinemachineCamera = GameObject.FindWithTag("FollowCamera").GetComponent<CinemachineCamera>();
        cinemachineCamera.Follow = CameraRoot;
    }
    private void Update()
    {
        // 1. 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 회전 방향 결정하기.
        _mx += mouseX * _owner.Stat.RotationSpeed * Time.deltaTime;
        _my += mouseY * _owner.Stat.RotationSpeed * Time.deltaTime;

        _my = Mathf.Clamp(_my, -90f, 90f);
        // y축 회전은 캐릭터만 한다.
        transform.eulerAngles = new Vector3(0, _mx, 0);

        // x 축 회전은 캐릭터는 하지 않는다.(즉, 카메라 루트만 x축 회전하면 된다.)
        CameraRoot.localEulerAngles = new Vector3(-_my, 0, 0);


        // 3. 회전 하기
    }
}
