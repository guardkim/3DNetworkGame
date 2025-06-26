using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpHeight = 3f;

    private CharacterController controller;
    private Vector3 velocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Movement();

        // 스페이스바로 점프
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }
    }

    public void Movement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(h, 0, v).normalized;
        Vector3 move = moveDirection * moveSpeed;

        // 중력 적용
        if (controller.isGrounded)
        {
            velocity.y = -2f; // 살짝 음수로 설정해서 바닥에 붙어있도록
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        move.y = velocity.y;
        controller.Move(move * Time.deltaTime);
    }
}
