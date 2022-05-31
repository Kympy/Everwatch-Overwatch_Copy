using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    public void GoTrainingRoom()
    {
        SceneManager.LoadScene(1);
    }
}
