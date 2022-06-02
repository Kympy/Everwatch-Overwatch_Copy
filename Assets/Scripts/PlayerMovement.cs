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

    private bool isJump; // ���� ���� ����
    private bool isJumping; // ���� ���� �� ���� Ȯ�� ����

    // �̵����� ����
    public float moveSpeed;
    public float jumpPower;
    void Start()
    {
        GameManager.Input.KeyAction -= OnKeyboard;
        GameManager.Input.KeyAction += OnKeyboard;
        m_rigidbody = GetComponent<Rigidbody>();
        cameraControl = GetComponentInChildren<CameraControl>();
        isJump = false; isJumping = false;
    }
    void OnKeyboard()
    {
        // �̵� Ű �Է� ����
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0);

        bool isRun = hasHorizontalInput || hasVerticalInput;

        if (isRun)
        {
            movement = new Vector3(horizontal, 0f, vertical);
            movement = Camera.main.transform.TransformDirection(movement); // ī�޶��� �������� ����
            movement.Normalize();
            transform.position += movement * moveSpeed * Time.deltaTime;
        }
        // ���� ����
        if (Input.GetKeyDown(KeyCode.Space)) // Ű�Է��� update���� ó��
        {
            if (isJumping == false)
            {
                isJumping = true;
                isJump = true;
            }
        }
        // �ɱ� ����
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isJump = false; // �ɱ� �� ���� �Ұ���
            cameraControl.Crouch(true); // ���� �� ī�޶� ���� ����
            moveSpeed = 3f; // �ɱ� �̵� �ӵ� ����
        }
        else
        {
            cameraControl.Crouch(false);
            moveSpeed = 6f; // �ɱ� �̵� �ӵ� ����
        }  
    }
    void Update()
    {
        

        
    }
    private void FixedUpdate() // ������ fixed���� ó��
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

    // �ٴڿ� �ִ��� üũ�ϴ� �Լ�
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }
}
