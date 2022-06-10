using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotHead : MonoBehaviour
{
    private BotControl bot;
    private void Start()
    {
        bot = GetComponentInParent<BotControl>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            bot.Damaged(10);
        }
    }
}
