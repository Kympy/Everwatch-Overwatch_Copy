using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // �̵��� ���� ��������
    public AimControl aimControl;
    private CameraControl cameraControl; // �̵��� ���� ī�޶� ���� ��Ʈ��

    private Rigidbody m_rigidbody;

    private bool jump; // ���� ���� �� ���� Ȯ�� ����
    private bool isGround; // ������ Ȯ��
    bool isJump;
    bool isRun;
    bool isCrouch;

    // �̵����� ����
    public float moveSpeed;
    public float jumpPower;
    public float rayRange; // ���� ���� �� �Ÿ�

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
        // �̵� Ű �Է� ����
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0);

        isCrouch = Input.GetKey(KeyCode.LeftControl);
        isRun = hasHorizontalInput || hasVerticalInput;
        isJump = Input.GetKeyDown(KeyCode.Space);

        // �̵� ����
        if (isRun)
        {
            Vector3 _moveHorizontal = transform.right * horizontal;
            Vector3 _moveVertical = transform.forward * vertical;
            Vector3 dir = (_moveHorizontal + _moveVertical).normalized;
            if(!isCrouch) moveSpeed = 6f; // �̵� �ӵ� ����
            transform.position = transform.position + dir * moveSpeed * Time.deltaTime;
        }
        // ���� ����
        if (isJump && isGround) // Ű�Է� & ���̸� ����
        {
            jump = true; // ���� ����
        }
        // �ɱ� ����
        if (isCrouch)
        {
            isJump = false; // �ɱ� �� ���� �Ұ���
            cameraControl.Crouch(true); // ���� �� ī�޶� ���� ����
            moveSpeed = 3f; // �ɱ� �̵� �ӵ� ����
        }
    }
    void Update()
    {
        cameraControl.Crouch(false); // ��� �� ī�޶� ���� ����
        CheckGround(); // ������ Ȯ��
    }
    private void FixedUpdate() // ������ fixed���� ó��
    {
        if (jump)
        {
            aimControl.Jump(); // ���� �� �������� ����
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
            if (hit.transform.tag == "Ground") // ���̸� isGround true
            {
                isGround = true;
                return;
            }
        }
        isGround = false; // ���̰� ���� ��ü�� ���� �ƴ� ��
    }
    /*
    private void CharacterRotation()  // �¿� ĳ���� ȸ��
    {
        float _yRotation = Camera.main.transform.rotation.y;
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * cameraControl.mouseSpeed;
        m_rigidbody.MoveRotation(m_rigidbody.rotation * Quaternion.Euler(_characterRotationY)); // ���ʹϾ� * ���ʹϾ�

    }*/
}
