using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpHeight = 3f;

    private float h;
    private float v;
    private CharacterController _controller;
    private Animator _animator;
    private Vector3 _velocity;


    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        _animator.SetFloat("h", h);
        _animator.SetFloat("v", v);

        Movement();
        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            _animator.SetTrigger("JumpStart");
            _animator.SetBool("IsJumping", true);
            _animator.SetBool("IsFalling", false);
        }
        HandleJumpState();
    }

    public void Attack()
    {

    }

    public void HandleJumpState()
    {

        // 공중 상태 처리
        if (!_controller.isGrounded)
        {
            // 상승 중
            if (_velocity.y > 0.1f)
            {
                _animator.SetBool("IsJumping", true);
                _animator.SetBool("IsFalling", false);
            }
            // 하강 중
            else if (_velocity.y < -0.1f)
            {
                _animator.SetBool("IsFalling", true);
            }
        }

        // 착지 처리
        if (_controller.isGrounded && (_animator.GetBool("IsJumping") || _animator.GetBool("IsFalling")))
        {
            _animator.SetBool("IsJumping", false);
            _animator.SetBool("IsFalling", false);
        }
    }


    public void Movement()
    {
        Vector3 moveDirection = new Vector3(h, 0, v).normalized;
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        Vector3 move = moveDirection * moveSpeed;

        _velocity.y -= gravity * Time.deltaTime;

        move.y = _velocity.y;
        _controller.Move(move * Time.deltaTime);
    }
}
