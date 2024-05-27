using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial2 : MonoBehaviour
{
    
   public void OnOkHandler()
    {
        SceneManager.LoadScene("Tutorial3");
    }
}
