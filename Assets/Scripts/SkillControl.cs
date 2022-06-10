using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : MonoBehaviour
{
    //private Rigidbody m_rigidbody;
    public Transform Camera;
    public float power;
    public Vector3 desiredDir;
    public Vector3 destination;
    public ArrayList originalPosition;
    public CanvasGroup canvas;

    public float speed;
    public float range;
    public float num;
    public float cameraHeight;
    private float timer;

    private bool isBlink;
    private bool isSave;
    void Start()
    {
        //m_rigidbody = GetComponent<Rigidbody>();
        isBlink = false; isSave = true; timer = 0f;
        originalPosition = new ArrayList();
    }
    void Update()
    {
        SavePosition(isSave);

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(TimeTravel());
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Blink();
        }
        if (isBlink)
        {
            float distance = Vector3.Distance(transform.position, destination);
            if (distance > 0.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
                canvas.alpha = 0;
            }
            else isBlink = false;
        }
    }
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
    IEnumerator TimeTravel()
    {
        canvas.alpha = 1;
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
    void Blink()
    {
        canvas.alpha = 1;
        Vector3 start = Camera.position;
        Vector3 end = Camera.forward;
        // 점멸 방향
        if (Input.GetKey(KeyCode.W))
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
        if (Physics.Raycast(start, end, out hit, range))
        {
            //Debug.DrawLine(Camera.position, hit.point * num, Color.red,2);
            // 장애물이 있으면 
            if (hit.transform.gameObject.name != "Bot")
            {
                //Debug.Log("Hit");
                destination = hit.point * num; // 목적지보다 조금 덜 간다
            }
        }
        else // 장애물이 없으면
        {
            //Debug.Log("no Hit");
            destination = (start + end.normalized * range) * num;
        }
        destination.y += cameraHeight;
        isBlink = true;
    }
}
