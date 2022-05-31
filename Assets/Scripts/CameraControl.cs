using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player; // �÷��̾�
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

        mouseY = Mathf.Clamp(mouseY, -45f, 50f); // ���� ���� ����
        this.transform.eulerAngles = new Vector3(mouseY, mouseX, 0);
        player.transform.eulerAngles = new Vector3(0, mouseX, 0); // ���콺 �¿� = ĳ���� ȸ��
    }
    private void LateUpdate()
    {
        chestDir = this.transform.position + this.transform.forward * 50f;
        p_upperChest.LookAt(chestDir);
        p_upperChest.rotation = p_upperChest.rotation * Quaternion.Euler(0, -90, -90); // ��ü ȸ�� ����
    }
}
