using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpHeight = 3f;

    private float h;
    private float v;
    private CharacterController _controller;
    private Animator _animator;
    private Vector3 _velocity;
    private bool _isMove;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        _isMove = h != 0 || v != 0;
        _animator.SetFloat("h", h);
        _animator.SetFloat("v", v);
        _animator.SetBool("isMove", _isMove);
        Movement();
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    public void Jump()
    {
        if (_controller.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }
    }

    public void Movement()
    {
        _isMove = true;


        Vector3 moveDirection = new Vector3(h, 0, v).normalized;
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        Vector3 move = moveDirection * moveSpeed;

        _velocity.y -= gravity * Time.deltaTime;

        move.y = _velocity.y;
        _controller.Move(move * Time.deltaTime);
    }
}
