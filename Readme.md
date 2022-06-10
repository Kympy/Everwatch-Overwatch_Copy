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
      
    ```C#
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
		
		
- 발사 방식은 히트스캔 방식으로 구현하였으며 레이를 쏘아 맞은 지점에 탄흔 오브젝트를 생성하고 부착하였다. 탄흔 오브젝트의 경우 자주 생성, 삭제를 반복하므로 오브젝트 풀링을 사용
      하여 한 탄창보다 넉넉한 크기만큼 Queue를 할당하였다.
      
      
	```C#
	public class ObjectPool : MonoBehaviour
	{
	    public static ObjectPool Instance;
	
	    public GameObject _BulletEffect;

	    Queue<BulletEffect> poolingObjectQueue = new Queue<BulletEffect>();

	    private void Awake()
	    {
	        Instance = this;
 	        Initialize(50);
	    }

	    private void Initialize(int initCount)
 	   {
  	      for (int i = 0; i < initCount; i++)
   	      {
          	  poolingObjectQueue.Enqueue(CreateNewObject());
        	}
    	}

    	private BulletEffect CreateNewObject()
    	{
        	var _object = Instantiate(_BulletEffect).GetComponent<BulletEffect>();
        	_object.gameObject.SetActive(false);
        	_object.transform.SetParent(transform);
       	  return _object;
   	  }
	}
- 점멸 / 시간역행
    
    ![Video Label](http://img.youtube.com/vi/KvbRFXK08i4/0.jpg)
    
    https://youtu.be/KvbRFXK08i4
		
 - 점멸은 시작 지점에서 Ray를 쏴 전방에 부딪힌 장애물이 있다면 장애물 위치까지만 점멸하고, 장애물이 없다면 기존 점멸 사거리만큼 정상적으로 이동한다. 단, 물체를 관통하거나 바닥에 		박히는 일이 없도록 num이라는 상수를 통해 이동거리를 보정하고, y축은 카메라 시점과 일치시켜 보정한다.
 
	```C#
    void Blink()
    {
        canvas.alpha = 1;
        Vector3 start = Camera.position;
        Vector3 end = Camera.forward;
        // 점멸 방향
        if (Input.GetKey(KeyCode.W)) // 각 방향에 따른 독립적인 점멸 구현
        {
            end += Camera.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            end += -Camera.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            end += -Camera.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            end += Camera.right;
        }
        RaycastHit hit;
        if (Physics.Raycast(start, end, out hit, range)) // 무언가 장애물이 있으면 맞은 위치에 떨어짐
        {
            //Debug.DrawLine(Camera.position, hit.point * num, Color.red,2);
            destination = hit.point * num; // 목적지보다 num이라는 상수(0.8~0.9)배 만큼 덜 간다. 관통 / 버그 방지
        }
        else // 전방에 장애물이 없으면 점멸 사거리만큼 앞으로 도약
        {
            //Debug.Log("no Hit");
            destination = (start + end.normalized * range) * num;
        }
        destination.y += cameraHeight; // 점멸과정 중 바닥에 파묻히는걸 막기 위해 시점 높이만큼 y축을 조절함
        isBlink = true;
    }
    
  - 시간 역행을 하기 위해서 3초동안 60개의 이동 지점을 0.05초 마다 List에 저장한다. 60개 정도의 저장이 위치의 급격한 도약없이 적절하게 저장된다고 판단했고, 60개 정도의 공간이면 		 List의 자료구조가 적절할 것이라 생각했다.
    ```C#  
    void SavePosition(bool start)
    {
        if (start)
        {
            timer += Time.deltaTime;
            if (timer >= 0.05f)
            {
                if (originalPosition.Count < 60) // 3초 전까지 저장
                {
                    originalPosition.Add(transform.position);
                    //Debug.Log(originalPosition.Count);

                }
                else
                {
                    originalPosition.RemoveAt(0); // 최대 이상으로 저장되면 첫번째 요소부터 지운다. 자동으로 인덱스 당겨짐
                    originalPosition.Add(transform.position);
                    //Debug.Log(originalPosition.Count);
                }
                timer = 0f;
            }
        }
        else return;
    }
- 저장된 위치는 3초마다 갱신되며, 저장된 위치를 다시 되돌아가는 방식으로 시간역행을 할 수 있었다. 처음에는 현 위치에서 배열의 역순으로 MoveTowards 해주는 방식으로 이동을 실시했으나 움직임이 다소 부자연스럽게 느껴졌다. 따라서 Lerp를 사용하여 위치와 위치를 보간하여 좀 더 부드럽게 이동할 수 있었다.

	```C#
	  IEnumerator TimeTravel()
    {
        canvas.alpha = 1; // 밋밋해서 화면이 파래지는 이펙트를 살짝 주었다.
        isSave = false;
        for (int i = originalPosition.Count - 1; i >= 0; i--)
        {
            //transform.position = Vector3.MoveTowards(transform.position, (Vector3)originalPosition[i], Time.deltaTime * speed * 2);
            transform.position = Vector3.Lerp(transform.position, (Vector3)originalPosition[i], 0.2f);
            //Debug.Log(originalPosition[i]);
            yield return new WaitForSeconds(0.01f); // 총 1.5 초가 걸려서 돌아온다.
        }
        isSave = true;
        canvas.alpha = 0;
    }
		

## 3. 한계점 & 셀프 디스

  + 일단, 애니메이션과 모델의 부재로 인해 좀 더 '그럴싸'한 것들이 나오지 못해 아쉽다. 어차피 목적은 공부에 있었으므로 크게 중요하지 않았다고 생각한다. 전체적으로 어찌저찌 기능 구현은 하겠지만, 캐릭터가 다양해지고 볼륨이 커지면 어떻게 효율적인 최적화를 할 수 있을지 고민해보아야 할 것이다. 짧게 나마 트레이서를 구현하면서 학교에서는 배우지 않았던 디자인 패턴에 대한 공부나, 오브젝트 풀링 기법등을 알게되었기에 얻은 점은 있다고 생각한다. 영웅갤러리, 전리품상자 오픈, 다른 몇몇 캐릭터(날아다니는 파라, 투사체가 많고 벽을 타는 루시우) 등 다른 기능도 만들어보고 싶었지만, 언리얼 엔진 공부를 시작해보고 싶어서 잠시 이 프로젝트는 멈춰야겠다. 
