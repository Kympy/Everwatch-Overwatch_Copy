using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player; // 플레이어

    [SerializeField]
    private float mouseSpeed;
    private float mouseX;
    private float mouseY;

    private float posY;
    public float applyCrouchPosY;
    public float applyWalkPosY;
    private void Start()
    {
        posY = this.transform.position.y;
    }
    void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed;

        mouseY = Mathf.Clamp(mouseY, -45f, 50f); // 상하 시점 제한
        this.transform.eulerAngles = new Vector3(mouseY, mouseX, 0);
        player.transform.eulerAngles = new Vector3(0, mouseX, 0); // 마우스 좌우 = 캐릭터 회전
    }
    public void Crouch(bool isCrouch) // 앉기 시에 카메라의 시점 조작
    {
        if (isCrouch)
        {
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.1f);
            this.transform.position = new Vector3(this.transform.position.x, posY, this.transform.position.z);
        }
        else
        {
            if(!Mathf.Approximately(posY, applyWalkPosY))
            {
                posY = Mathf.Lerp(posY, applyWalkPosY, 0.1f);
                this.transform.position = new Vector3(this.transform.position.x, posY, this.transform.position.z);
            }
        }
    }
}
