using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // ���� ��ȭ ���� ���� ��ũ��Ʈ
    public AimControl aimControl;

    // źâ ��
    public int currentBullet;
    public int maxBullet; // �ִ� źâ
    public GameObject bulletEffect;
    public GameObject shootEffect;

    // ����ӵ�
    public float fireRate;
    private float fireTime; // ���� ����� ���� �߻�ð�

    //�����ӵ�
    public float reloadingTime;

    public Transform shootPoint_L; // �߻�����
    public Transform shootPoint_R; // �߻�����
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
            else StartCoroutine(Reload()); // �Ѿ��� 0 ���ϸ� ������ �ڷ�ƾ ����

            if (fireTime < fireRate) // �߻�ð��� ����ӵ����� ������
            {
                fireTime += Time.deltaTime; // �߻�ð� ����
            }
        }
        else
        {
            aimControl.Idle();
        }
    }
    void Fire()
    {
        aimControl.Fire();
        if (fireTime >= fireRate)
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.transform.position,  // ���� ��Ȯ���� ���� ���̸� ��
                Camera.main.transform.forward +
                new Vector3(Random.Range(-aimControl.accuracy, aimControl.accuracy), Random.Range(-aimControl.accuracy, aimControl.accuracy),0f),
                out hit, range))
            {
                if(hit.transform.tag == "Head")
                {
                    BotControl bot = hit.transform.gameObject.GetComponent<BotControl>();
                    bot.Damaged(10);
                }
                else if(hit.transform.gameObject.tag == "Bot")
                {
                    BotControl bot = hit.transform.gameObject.GetComponent<BotControl>();
                    bot.Damaged(5);
                }
            }
            Instantiate(bulletEffect, hit.point * 1.001f, Quaternion.LookRotation(hit.normal)); // �� ���� ��ġ ǥ��
            GameObject effect_L = Instantiate(shootEffect, shootPoint_L);
            GameObject effect_R = Instantiate(shootEffect, shootPoint_R);

            Destroy(effect_L, 0.1f); Destroy(effect_R, 0.1f);
            currentBullet--;
            fireTime = 0f;
        }
    }
    IEnumerator Reload() // �ڷ�ƾ���� ������ ����
    {
        Debug.Log("Reloading..");
        yield return new WaitForSeconds(reloadingTime); // ������ �ð���ŭ �Ŀ� ����
        currentBullet = maxBullet;
        Debug.Log("Reload finish");
    }
}
