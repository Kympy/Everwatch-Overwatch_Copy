using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator m_animator;
    private Rigidbody m_rigidbody;

    private Vector3 movement;

    public float moveSpeed;
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector3(-vertical, 0f, horizontal);
        movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0);

        bool isRun = hasHorizontalInput || hasVerticalInput;

        m_animator.SetBool("IsRun", isRun);
        m_rigidbody.MovePosition(m_rigidbody.position + movement * moveSpeed *Time.deltaTime);
    }
}
