using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimControl : MonoBehaviour
{
    private Animator m_animator;
    public float accuracy; // 0 에 가까울수록 정확

    private void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
    }
    public void Fire()
    {
        m_animator.SetBool("IsFire", true);
    }
    public void Crouch()
    {
        m_animator.SetBool("IsCrouch", true);
        accuracy = 0.09f;
    }
    public void CrouchFire()
    {
        m_animator.SetBool("IsCrouchFire", true);
        accuracy = 0.1f;
    }
    public void Jump()
    {
        m_animator.SetBool("IsFire", true);
        accuracy = 0.18f;
    }
    public void Walk()
    {
        accuracy = 0.13f;
    }
    public void Idle()
    {
        m_animator.SetBool("IsFire", false);
        m_animator.SetBool("IsCrouchFire", false);
        m_animator.SetBool("IsCrouch", false);
        accuracy = 0.1f;
    }
}
