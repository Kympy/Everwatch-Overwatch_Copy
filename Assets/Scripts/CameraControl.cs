using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player; // 플레이어
    private Animator p_animator;
    private Transform p_upperChest;

    [SerializeField]
    private float mouseSpeed;
    private float mouseX;
    private float mouseY;

    private Vector3 chestDir;
    private Vector3 chestOffset;
    private void Start()
    {
        p_animator = player.GetComponent<Animator>();
        p_upperChest = p_animator.GetBoneTransform(HumanBodyBones.UpperChest);
    }

    void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed;

        mouseY = Mathf.Clamp(mouseY, -45f, 50f); // 상하 시점 제한
        this.transform.eulerAngles = new Vector3(mouseY, mouseX, 0);
        player.transform.eulerAngles = new Vector3(0, mouseX, 0); // 마우스 좌우 = 캐릭터 회전
    }
    private void LateUpdate()
    {
        chestDir = this.transform.position + this.transform.forward * 50f;
        p_upperChest.LookAt(chestDir);
        p_upperChest.rotation = p_upperChest.rotation * Quaternion.Euler(0, -90, -90); // 상체 회전 보정
    }
}
