using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Homepagepanel : MonoBehaviour
{
    public GameObject SettingsPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPlayHandler()
    {
        SceneManager.LoadScene("PenguinEatSushi");
    }
    public void OnSettingsHandler()
    {
        SettingsPanel.SetActive(true);

    }
    public void OnTutorialHandler()
    {
        SceneManager.LoadScene("Tutorial1");
    }
}
