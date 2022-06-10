using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotControl : MonoBehaviour
{
    [SerializeField]
    private int currentHP; // ���� ü��
    private int maxHp; // ��ü ü��
    private int attackPower; // ���� ���ݷ�

    private Animator m_animator;
    void Start()
    {
        maxHp = 100;
        currentHP = maxHp; // ���� �� ü�� ȸ��
        m_animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Damaged(5);
        }
    }
    public void Damaged(int power)
    {
        currentHP = currentHP - power;
        m_animator.SetTrigger("IsDamaged");
    }
    private void Dead()
    {
        if(currentHP <= 0)
        {
            m_animator.SetBool("IsDead", true);
            Destroy(this.gameObject, 2f);
        }
    }
    void Update()
    {
        if (currentHP <= 0) currentHP = 0;
        Dead();
    }
}
