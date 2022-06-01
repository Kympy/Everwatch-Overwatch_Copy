using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 이동에 따른 에임제어
    public AimControl aimControl;
    private CameraControl cameraControl; // 이동에 따른 카메라 시점 컨트롤

    private Animator m_animator;
    private Rigidbody m_rigidbody;

    private Vector3 movement;

    private bool isJump; // 점프 실행 변수
    private bool isJumping; // 점프 실행 중 여부 확인 변수

    // 이동관련 변수
    public float moveSpeed;
    public float jumpPower;
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
        cameraControl = GetComponentInChildren<CameraControl>();
        isJump = false; isJumping = false;
    }
    void Update()
    {
        // 이동 키 입력 관련
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector3(horizontal, 0f, vertical);
        movement = Camera.main.transform.TransformDirection(movement); // 카메라의 방향으로 설정
        movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0);

        bool isRun = hasHorizontalInput || hasVerticalInput;

        // 점프 관련
        if (Input.GetKeyDown(KeyCode.Space)) // 키입력은 update에서 처리
        {
            if(isJumping == false)
            {
                isJumping = true;
                isJump = true;
            }
        }
        // 앉기 관련
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isJump = false; // 앉기 중 점프 불가능
            cameraControl.Crouch(true); // 앉을 때 카메라 시점 조작
            m_animator.SetBool("IsCrouch", true);
            moveSpeed = 3f; // 앉기 이동 속도 저하
        }
        else
        {
            cameraControl.Crouch(false);
            m_animator.SetBool("IsCrouch", false);
            moveSpeed = 6f; // 앉기 이동 속도 복구
        }
        // 이동 관련
        m_animator.SetBool("IsRun", isRun);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
    private void FixedUpdate() // 물리만 fixed에서 처리
    {
        if (isJump)
        {
            aimControl.Jump();
            Debug.Log("Jump");
            m_rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = false;
        }
        else aimControl.Idle();
    }

    // 바닥에 있는지 체크하는 함수
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }
}
