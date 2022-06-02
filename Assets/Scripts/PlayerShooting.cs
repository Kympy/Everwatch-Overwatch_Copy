using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public AimControl aimControl; // 에임 변화 조절 위한 스크립트
    // 탄창 수
    public int currentBullet;
    public int maxBullet; // 최대 탄창

    // 연사속도
    public float fireRate;
    private float fireTime; // 연사 계산을 위한 발사시간

    //장전속도
    private float timer;
    public float reloadingTime;

    public Transform shootPoint; // 발사지점
    public float range; // 사정거리
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
        if(timer >= reloadingTime) // 장전시간을 넘기면 장전
        {
            currentBullet = maxBullet;
            timer = 0f;
        }
    }
}
