using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // 에임 변화 조절 위한 스크립트
    public AimControl aimControl;
    //public GameObject aim;
    // 탄창 수
    public int currentBullet;
    public int maxBullet; // 최대 탄창
    public GameObject bulletEffect;

    // 연사속도
    public float fireRate;
    private float fireTime; // 연사 계산을 위한 발사시간

    //장전속도
    public float reloadingTime;

    //public Transform shootPoint; // 발사지점
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
            if(Physics.Raycast(Camera.main.transform.position,  // 에임 정확도에 따라 레이를 쏨
                Camera.main.transform.forward +
                new Vector3(Random.Range(-aimControl.accuracy, aimControl.accuracy), Random.Range(-aimControl.accuracy, aimControl.accuracy),0f),
                out hit, range))
            {
                //Debug.Log(Random.Range(-aimControl.accuracy, aimControl.accuracy));
                /*Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward +
                new Vector3(Random.Range(-aimControl.accuracy, aimControl.accuracy), Random.Range(-aimControl.accuracy, aimControl.accuracy), 0f),Color.red, 2f);*/
            }
            GameObject effect = Instantiate(bulletEffect, hit.point, Quaternion.LookRotation(hit.normal)); // 총 맞은 위치 표현
            Destroy(effect, 1f);
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
