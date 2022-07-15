using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : MonoBehaviour
{
    public Transform Camera; // 카메라 위치
    public float power; // 점멸 파워
    public Vector3 desiredDir; // 목적지 방향
    public Vector3 destination; // 목적지
    public ArrayList originalPosition; // 기존 위치
    public CanvasGroup canvas; // 간단하게 준 이펙트

    private float speed; // 속도
    private float range; // 점멸 사거리
    private float num = 0.8f; // 상수
    public float cameraHeight; // 카메라 고도
    private float timer; // 시간역행 타이머

    private bool isBlink; // 점멸 여부
    private bool isSave; // 위치 저장 활성화 여부
    void Start()
    {
        isBlink = false; isSave = true; timer = 0f; // 초기화
        originalPosition = new ArrayList(); // 초기위치배열
    }
    void Update()
    {
        SavePosition(isSave); // 위치를 저장

        if (Input.GetKeyDown(KeyCode.E)) // 시간역행 키
        {
            StartCoroutine(TimeTravel()); // 시간역행 동작
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) // 점멸 키
        {
            Blink(); // 점멸 동작
        }
        if (isBlink)
        {
            float distance = Vector3.Distance(transform.position, destination); // 목적지와의 거리 설정
            if (distance > 0.2f) // 거리가 최소 이상이라면
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed); // 캐릭터를 빠르게 이동
                canvas.alpha = 0;
            }
            else isBlink = false;
        }
    }
    void SavePosition(bool start) // 시간역행을 위한 위치저장
    {
        if (start)
        {
            timer += Time.deltaTime; // 타이머 가동
            if (timer >= 0.05f) // 0.05초 마다 저장
            {
                if (originalPosition.Count < 60) // 3초 전까지 저장 0.05 * 60 = 3
                {
                    originalPosition.Add(transform.position); // 이동 경로 위치를 배열에 추가
                    //Debug.Log(originalPosition.Count);
                }
                else
                {
                    originalPosition.RemoveAt(0); // 최대 이상으로 저장되면 첫번째 요소부터 지운다. 자동으로 인덱스 당겨짐
                    originalPosition.Add(transform.position); // 지우고 나서 추가
                    //Debug.Log(originalPosition.Count);
                }
                timer = 0f;
            }
        }
        else return;
    }
    IEnumerator TimeTravel() // 시간역행
    {
        canvas.alpha = 1; // 밋밋해서 화면이 파래지는 이펙트를 살짝 주었다.
        isSave = false;
        for (int i = originalPosition.Count - 1; i >= 0; i--)
        {
            //transform.position = Vector3.MoveTowards(transform.position, (Vector3)originalPosition[i], Time.deltaTime * speed * 2);
            transform.position = Vector3.Lerp(transform.position, (Vector3)originalPosition[i], 0.2f);
            //Debug.Log(originalPosition[i]);
            yield return new WaitForSeconds(0.01f); // 총 1.5 초가 걸려서 돌아온다. >> 너무 빠르게 돌아오는 것을 방지
        }
        isSave = true;
        canvas.alpha = 0;
    }
    void Blink() // 점멸
    {
        canvas.alpha = 1;
        Vector3 start = Camera.position; // 시작 위치
        Vector3 end = Camera.forward; // 목적 방향
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
}
