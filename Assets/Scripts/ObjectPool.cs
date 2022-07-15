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
        Initialize(50); // 50개 만큼 할당
    }

    private void Initialize(int initCount) // 큐 초기화
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject()); // 오브젝트 생성 후 대입
        }
    }

    private BulletEffect CreateNewObject() // 프리팹으로부터 오브젝트 가져옴
    {
        var _object = Instantiate(_BulletEffect).GetComponent<BulletEffect>();
        _object.gameObject.SetActive(false);
        _object.transform.SetParent(transform);
        return _object;
    }

    public static BulletEffect GetObject() // 큐에서 오브젝트 꺼내기
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null); // 풀로부터의 관계 해제
            obj.gameObject.SetActive(true); // 오브젝트 활성화
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject(); // 큐가 비어있으면
            Instance.poolingObjectQueue.Enqueue(newObj); // 생성해서 넣고
            Instance.poolingObjectQueue.Dequeue(); // 꺼내기
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnObject(BulletEffect obj) // 큐에 다시 반환하기
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
