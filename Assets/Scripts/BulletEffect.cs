using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    private void Update()
    {
        Destroy(this.gameObject, 1.5f);
    }
}
