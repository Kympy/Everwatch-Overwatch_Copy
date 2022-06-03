using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // ���� ��ȭ ���� ���� ��ũ��Ʈ
    public AimControl aimControl;
    //public GameObject aim;
    // źâ ��
    public int currentBullet;
    public int maxBullet; // �ִ� źâ
    public GameObject bulletEffect;

    // ����ӵ�
    public float fireRate;
    private float fireTime; // ���� ����� ���� �߻�ð�

    //�����ӵ�
    public float reloadingTime;

    //public Transform shootPoint; // �߻�����
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
                //Debug.Log(Random.Range(-aimControl.accuracy, aimControl.accuracy));
                /*Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward +
                new Vector3(Random.Range(-aimControl.accuracy, aimControl.accuracy), Random.Range(-aimControl.accuracy, aimControl.accuracy), 0f),Color.red, 2f);*/
            }
            GameObject effect = Instantiate(bulletEffect, hit.point, Quaternion.LookRotation(hit.normal)); // �� ���� ��ġ ǥ��
            Destroy(effect, 1f);
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
