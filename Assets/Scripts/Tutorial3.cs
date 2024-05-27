using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial3 : MonoBehaviour
{
    public void OnokHandler()
    {
        SceneManager.LoadScene("Homepage");
    }
}
