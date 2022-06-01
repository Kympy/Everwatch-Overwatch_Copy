using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // �̵��� ���� ��������
    public AimControl aimControl;
    private CameraControl cameraControl; // �̵��� ���� ī�޶� ���� ��Ʈ��

    private Animator m_animator;
    private Rigidbody m_rigidbody;

    private Vector3 movement;

    private bool isJump; // ���� ���� ����
    private bool isJumping; // ���� ���� �� ���� Ȯ�� ����

    // �̵����� ����
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
        // �̵� Ű �Է� ����
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector3(horizontal, 0f, vertical);
        movement = Camera.main.transform.TransformDirection(movement); // ī�޶��� �������� ����
        movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0);

        bool isRun = hasHorizontalInput || hasVerticalInput;

        // ���� ����
        if (Input.GetKeyDown(KeyCode.Space)) // Ű�Է��� update���� ó��
        {
            if(isJumping == false)
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
            m_animator.SetBool("IsCrouch", true);
            moveSpeed = 3f; // �ɱ� �̵� �ӵ� ����
        }
        else
        {
            cameraControl.Crouch(false);
            m_animator.SetBool("IsCrouch", false);
            moveSpeed = 6f; // �ɱ� �̵� �ӵ� ����
        }
        // �̵� ����
        m_animator.SetBool("IsRun", isRun);
        transform.position += movement * moveSpeed * Time.deltaTime;
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
