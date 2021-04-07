using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    [SerializeField] private string SceneToSwitch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void switchScene()
    {
        SceneManager.LoadScene(SceneToSwitch);
    }
}
