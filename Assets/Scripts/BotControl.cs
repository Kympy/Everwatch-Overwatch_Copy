using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotControl : MonoBehaviour
{
    [SerializeField]
    private int currentHP; // 현재 체력
    private int maxHp; // 전체 체력
    private int attackPower; // 봇의 공격력

    private Animator m_animator;
    void Start()
    {
        maxHp = 100;
        currentHP = maxHp; // 시작 시 체력 회복
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
