using Photon.Pun;
using UnityEngine;

public class PlayerMove : PlayerAbility, IPunObservable
{
    [SerializeField] private float gravity = 9.81f;

    private float h;
    private float v;
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isRunning;

    private Vector3 _receivedPosition = Vector3.zero;
    private Quaternion _receivedRotation = Quaternion.identity;
    private const float DAMPING = 20f;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터 동기화를 위한 데이터 전송 및 수신 기능
        // stream : 서버에서 주고받을 데이터가 담겨있는 변수
        // info   : 송수신 성공/실패 여부에 대한 로그
        if (stream.IsWriting )
        {
            Debug.Log("전송중");
            // 내꺼의 데이터만 보내준다...
            // 데이터를 전송하는 상황 -> 데이터를 보내주면 되고,
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading )
        {
            Debug.Log("수신중");
            // 데이터를 수신하는 상황 -> 받은 데이터를 세팅하면 됩니다.
            // 보내준 순서대로 받는다.
            _receivedPosition = (Vector3)stream.ReceiveNext();
            _receivedRotation = (Quaternion)stream.ReceiveNext();
        }
        if (!_photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, _receivedPosition, Time.deltaTime * DAMPING);
            transform.rotation = Quaternion.Slerp(transform.rotation, _receivedRotation, Time.deltaTime * DAMPING);
            return;
        }
    }
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        _animator.SetFloat("h", h);
        _animator.SetFloat("v", v);

        Movement();
        if (Input.GetButtonDown("Jump") && _controller.isGrounded && TryUseStamina(15f))
        {
            _velocity.y = Mathf.Sqrt(_owner.Stat.JumpForce * 2f * gravity);
            _animator.SetTrigger("JumpStart");
            _animator.SetBool("IsJumping", true);
            _animator.SetBool("IsFalling", false);
            // _owner.Stat.Stamina -= 15f;
        }
        HandleJumpState();
        _isRunning = Input.GetKey(KeyCode.LeftShift);
        CanRegenStamina();
    }

    protected override bool CanRegenStamina()
    {
        return !_isRunning && !_controller.isGrounded;
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
        Vector3 move = moveDirection * _owner.Stat.MoveSpeed;

        if (_isRunning)
        {
            _animator.SetFloat("v", 2);
            TryUseStamina(20f * Time.deltaTime);
            // _owner.Stat.Stamina -= 10f * Time.deltaTime;
            move = moveDirection * _owner.Stat.RunSpeed;
        }

        _velocity.y -= gravity * Time.deltaTime;

        move.y = _velocity.y;
        _controller.Move(move * Time.deltaTime);
    }
}
