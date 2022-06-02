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
            else StartCoroutine(Reload()); // 총알이 0 이하면 재장전 코루틴 실행

            if (fireTime < fireRate) // 발사시간이 연사속도보다 작으면
            {
                fireTime += Time.deltaTime; // 발사시간 누적
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
    IEnumerator Reload() // 코루틴으로 재장전 구현
    {
        Debug.Log("Reloading..");
        yield return new WaitForSeconds(reloadingTime); // 재장전 시간만큼 후에 장전
        currentBullet = maxBullet;
        Debug.Log("Reload finish");
    }
}
