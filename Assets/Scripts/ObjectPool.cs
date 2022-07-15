using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public GameObject _BulletEffect;

    private Queue<BulletEffect> poolingObjectQueue = new Queue<BulletEffect>();

    private void Awake()
    {
        Instance = this;
        Initialize(50); // 50�� ��ŭ �Ҵ�
    }

    private void Initialize(int initCount) // ť �ʱ�ȭ
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject()); // ������Ʈ ���� �� ����
        }
    }

    private BulletEffect CreateNewObject() // ���������κ��� ������Ʈ ������
    {
        var _object = Instantiate(_BulletEffect).GetComponent<BulletEffect>();
        _object.gameObject.SetActive(false);
        _object.transform.SetParent(transform);
        return _object;
    }

    public static BulletEffect GetObject() // ť���� ������Ʈ ������
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null); // Ǯ�κ����� ���� ����
            obj.gameObject.SetActive(true); // ������Ʈ Ȱ��ȭ
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject(); // ť�� ���������
            Instance.poolingObjectQueue.Enqueue(newObj); // �����ؼ� �ְ�
            Instance.poolingObjectQueue.Dequeue(); // ������
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnObject(BulletEffect obj) // ť�� �ٽ� ��ȯ�ϱ�
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
