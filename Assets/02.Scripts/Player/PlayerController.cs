using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = 9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();
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
            velocity.y = -2f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        move.y = velocity.y;
        controller.Move(move * Time.deltaTime);
    }
}
