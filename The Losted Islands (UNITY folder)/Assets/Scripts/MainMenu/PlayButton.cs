using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void ChangeScene(int _number)
    {
        SceneManager.LoadScene(_number);
    }
}
