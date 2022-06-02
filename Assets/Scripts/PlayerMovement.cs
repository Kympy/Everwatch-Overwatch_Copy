using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 이동에 따른 에임제어
    public AimControl aimControl;
    private CameraControl cameraControl; // 이동에 따른 카메라 시점 컨트롤

    private Rigidbody m_rigidbody;

    private bool jump; // 점프 실행 중 여부 확인 변수
    private bool isGround; // 땅인지 확인
    bool isJump;
    bool isRun;
    bool isCrouch;

    // 이동관련 변수
    public float moveSpeed;
    public float jumpPower;
    public float rayRange; // 점프 레이 쏠 거리

    void Start()
    {
        GameManager.Input.KeyAction -= OnKeyboard;
        GameManager.Input.KeyAction += OnKeyboard;
        m_rigidbody = GetComponent<Rigidbody>();
        cameraControl = GetComponentInChildren<CameraControl>();
        jump = false; isRun = false; isCrouch = false; isGround = false;
        rayRange = 1.8f;
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
        if (isJump && isGround) // 키입력 & 땅이면 점프
        {
            jump = true; // 점프 실행
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
        CheckGround(); // 땅인지 확인
    }
    private void FixedUpdate() // 물리만 fixed에서 처리
    {
        if (jump)
        {
            aimControl.Jump(); // 점프 시 에임으로 변경
            Debug.Log("Jump");
            m_rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            jump = false;
        }
        else aimControl.Idle();
    }
    void CheckGround()
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position, Vector3.down * rayRange, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayRange))
        {
            if (hit.transform.tag == "Ground") // 땅이면 isGround true
            {
                isGround = true;
                return;
            }
        }
        isGround = false; // 레이가 맞은 물체가 땅이 아닐 때
    }
    /*
    private void CharacterRotation()  // 좌우 캐릭터 회전
    {
        float _yRotation = Camera.main.transform.rotation.y;
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * cameraControl.mouseSpeed;
        m_rigidbody.MoveRotation(m_rigidbody.rotation * Quaternion.Euler(_characterRotationY)); // 쿼터니언 * 쿼터니언

    }*/
}
