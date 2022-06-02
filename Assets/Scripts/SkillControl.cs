using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    public Transform Camera;
    public float power;
    public Vector3 desiredDir;
    public Vector3 destination;

    public float speed;
    public float range;
    public float num;
    public float cameraHeight;

    private bool isBlink;
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        isBlink = false;
    }
    void OnKeyBoard()
    {
        

    }
    void Update()
    {
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
            }
            else isBlink = false;
        }
    }
    void Blink()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.position, Camera.forward, out hit, range))
        {
            Debug.DrawLine(Camera.position, hit.point * num, Color.red,2);
            // 장애물이 있으면 
            Debug.Log("Hit");
            destination = hit.point * num; // 목적지보다 조금 덜 간다

        }
        else // 장애물이 없으면
        {
            Debug.Log("no Hit");
            destination = (Camera.position + Camera.forward.normalized * range) * num;
        }
        destination.y += cameraHeight;
        isBlink = true;
    }
}
