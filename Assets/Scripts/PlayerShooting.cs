using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public AimControl aimControl; // ���� ��ȭ ���� ���� ��ũ��Ʈ
    // źâ ��
    public int currentBullet;
    public int maxBullet; // �ִ� źâ

    // ����ӵ�
    public float fireRate;
    private float fireTime; // ���� ����� ���� �߻�ð�

    //�����ӵ�
    private float timer;
    public float reloadingTime;

    public Transform shootPoint; // �߻�����
    public float range; // �����Ÿ�
    void Start()
    {
        GameManager.Input.KeyAction -= OnMouse;
        GameManager.Input.KeyAction += OnMouse;
        currentBullet = maxBullet;
    }
    private void OnMouse()
    {
        if (Input.GetMouseButton(0))
        {
            if (currentBullet > 0)
            {
                Fire();
            }
            else Reload();

            if (fireTime < fireRate)
            {
                fireTime += Time.deltaTime;
            }
        }
        else aimControl.Idle();
    }
    
    void Fire()
    {
        aimControl.Fire();
        if (fireTime >= fireRate)
        {
            RaycastHit hit;
            if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
            }
            currentBullet--;
            fireTime = 0f;
        }
    }
    void Reload()
    {
        Debug.Log("Reloading..");
        timer += Time.deltaTime;
        if(timer >= reloadingTime) // �����ð��� �ѱ�� ����
        {
            currentBullet = maxBullet;
            timer = 0f;
        }
    }
}
