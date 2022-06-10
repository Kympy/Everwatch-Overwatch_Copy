# Everwatch-오버워치 모작

## 1. 개요
  오버워치의 메인화면을 비슷하게 구현해보고자 하며, 훈련장에서 캐릭터의 스킬들을 모방하며 공부하는 것에 의미를 두었다. 오버워치의 모델과 이미지들은 공개되어 있지 않기에 에셋 스토의 무료 에셋 중에서 분위기가 비슷한 것들을 골라 사용했다. 별도의 이펙트, 사운드 등은 활용하지 않았으며 기능을 구현하는 것만 시도하였다. 
## 2. 주요 기능
  + 메인화면

    ![Video Label](http://img.youtube.com/vi/Sht9aB_tLRw/0.jpg)
    
    https://youtu.be/Sht9aB_tLRw
    - 메인화면의 모양을 최대한 비슷하게 따라해보았다. 버튼은 마우스 오버 시 확대되는 애니메이션을 간단하게 만들어 사용했고, 뒷 배경 흐림은 유니티 프로가 아니어서 그런지 피사계심도가         적용되지 않아 가로 세로 방향으로 블러를 주는 쉐이더를 사용해서 구현했다.
  + 트레이서
    - 이동, 앉기, 점프, 조준, 사격
    
      ![Video Label](http://img.youtube.com/vi/es291dzJh8Y/0.jpg)
      
      https://youtu.be/es291dzJh8Y
      - 이동은 일반적인 방법으로 키보드로 입력받았으며, 점프는 AddForce를 사용했다. 앉기 같은 경우는 1인칭 플레이어 카메라의 position을 낮추는 방식으로 앉는 느낌을 주었다.
        (캐릭터 모델과 애니메이션의 부재로 머리에 카메라를 다는 방식을 대체하였다)
      - 조준점은 기본적으로 1의 Scale값을 가지고 있다. 점프, 사격, 앉기 시 각각의 상황에 따른 Scale값을 0.8~1.5 사이로 조절하여 조준점의 벌어지는 정도를 표현하였다.
      
    ``` C#
    void Fire()
    {
        aimControl.Fire();
        if (fireTime >= fireRate) // 연사속도 시간을 넘으면
        {
            RaycastHit hit; 
            if(Physics.Raycast(Camera.main.transform.position,  // 에임 정확도에 따라 레이를 쏨
                Camera.main.transform.forward + // 현재 카메라가 보는 방향을 시작점으로 에임의 정확도 범위 중 랜덤한 위치로 발사
                new Vector3(Random.Range(-aimControl.accuracy, aimControl.accuracy), Random.Range(-aimControl.accuracy, aimControl.accuracy),0f),
                out hit, range))
            {
                if(hit.transform.tag == "Head") // 머리에 맞으면 2배의 데미지
                {
                    BotControl bot = hit.transform.gameObject.GetComponent<BotControl>();
                    bot.Damaged(10);
                }
                else if(hit.transform.gameObject.tag == "Bot") // 몸통일 경우 일반 데미지
                {
                    BotControl bot = hit.transform.gameObject.GetComponent<BotControl>();
                    bot.Damaged(5);
                }
            }
            Instantiate(bulletEffect, hit.point * 1.001f, Quaternion.LookRotation(hit.normal)); // 총 맞은 위치 표현
            GameObject effect_L = Instantiate(shootEffect, shootPoint_L); // 좌우 총 이펙트
            GameObject effect_R = Instantiate(shootEffect, shootPoint_R);

            Destroy(effect_L, 0.1f); Destroy(effect_R, 0.1f); // 이펙트 삭제
            currentBullet--; // 총알 감소
            fireTime = 0f;
        }
    }
  ```
  
      - 발사 방식은 히트스캔 방식으로 구현하였으며 레이를 쏘아 맞은 지점에 탄흔 오브젝트를 생성하고 부착하였다. 탄흔 오브젝트의 경우 자주 생성, 삭제를 반복하므로 오브젝트 풀링을 사용
        하여 한 탄창보다 넉넉한 크기만큼 Queue를 할당하였다.
    - 점멸
      
    - 시간역행
## 3. 한계점
