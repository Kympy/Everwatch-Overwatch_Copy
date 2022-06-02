using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // �̵��� ���� ��������
    public AimControl aimControl;
    private CameraControl cameraControl; // �̵��� ���� ī�޶� ���� ��Ʈ��

    private Rigidbody m_rigidbody;

    private Vector3 movement;

    private bool isJumping; // ���� ���� �� ���� Ȯ�� ����
    bool isJump;
    bool isRun;
    bool isCrouch;

    // �̵����� ����
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
        if (isJump) // Ű�Է��� update���� ó��
        {
            if (isJumping == false)
            {
                isJumping = true;
                isJump = true;
            }
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
    }
    private void FixedUpdate() // ������ fixed���� ó��
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
    private void CharacterRotation()  // �¿� ĳ���� ȸ��
    {
        float _yRotation = Camera.main.transform.rotation.y;
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * cameraControl.mouseSpeed;
        m_rigidbody.MoveRotation(m_rigidbody.rotation * Quaternion.Euler(_characterRotationY)); // ���ʹϾ� * ���ʹϾ�

    }*/

    // �ٴڿ� �ִ��� üũ�ϴ� �Լ�
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }
}
