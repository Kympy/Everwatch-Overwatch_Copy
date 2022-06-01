using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimControl : MonoBehaviour
{
    private Animator m_animator;

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
    }
    public void CrouchFire()
    {
        m_animator.SetBool("IsCrouchFire", true);
    }
    public void Jump()
    {
        m_animator.SetBool("IsFire", true);
    }
    public void Idle()
    {
        m_animator.SetBool("IsFire", false);
        m_animator.SetBool("IsCrouchFire", false);
        m_animator.SetBool("IsCrouch", false);
    }
}
