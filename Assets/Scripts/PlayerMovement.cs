using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 이동에 따른 에임제어
    public AimControl aimControl;
    private CameraControl cameraControl; // 이동에 따른 카메라 시점 컨트롤

    private Rigidbody m_rigidbody;

    private Vector3 movement;

    private bool isJumping; // 점프 실행 중 여부 확인 변수
    bool isJump;
    bool isRun;
    bool isCrouch;

    // 이동관련 변수
    public float moveSpeed;
    public float jumpPower;

    void Start()
    {
        GameManager.Input.KeyAction -= OnKeyboard;
        GameManager.Input.KeyAction += OnKeyboard;
        m_rigidbody = GetComponent<Rigidbody>();
        cameraControl = GetComponentInChildren<CameraControl>();
        isJumping = false; isRun = false; isCrouch = false;
    }
    void OnKeyboard()
    {
        // 이동 키 입력 관련
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0);

        isCrouch = Input.GetKey(KeyCode.LeftControl);
        isRun = hasHorizontalInput || hasVerticalInput;
        isJump = Input.GetKeyDown(KeyCode.Space);

        // 이동 관련
        if (isRun)
        {
            Vector3 _moveHorizontal = transform.right * horizontal;
            Vector3 _moveVertical = transform.forward * vertical;
            Vector3 dir = (_moveHorizontal + _moveVertical).normalized;
            if(!isCrouch) moveSpeed = 6f; // 이동 속도 복구
            transform.position = transform.position + dir * moveSpeed * Time.deltaTime;
        }
        // 점프 관련
        if (isJump) // 키입력은 update에서 처리
        {
            if (isJumping == false)
            {
                isJumping = true;
                isJump = true;
            }
        }
        // 앉기 관련
        if (isCrouch)
        {
            isJump = false; // 앉기 중 점프 불가능
            cameraControl.Crouch(true); // 앉을 때 카메라 시점 조작
            moveSpeed = 3f; // 앉기 이동 속도 저하
        }
    }
    void Update()
    {
        cameraControl.Crouch(false); // 평상 시 카메라 시점 조작
    }
    private void FixedUpdate() // 물리만 fixed에서 처리
    {
        if (isJumping)
        {
            aimControl.Jump();
            Debug.Log("Jump");
            m_rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJumping = false;
        }
        else aimControl.Idle();
    }
    /*
    private void CharacterRotation()  // 좌우 캐릭터 회전
    {
        float _yRotation = Camera.main.transform.rotation.y;
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * cameraControl.mouseSpeed;
        m_rigidbody.MoveRotation(m_rigidbody.rotation * Quaternion.Euler(_characterRotationY)); // 쿼터니언 * 쿼터니언

    }*/

    // 바닥에 있는지 체크하는 함수
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }
}
