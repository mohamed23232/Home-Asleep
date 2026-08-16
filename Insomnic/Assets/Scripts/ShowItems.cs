using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShowItems : MonoBehaviour
{
    private int pressCount = 0;
    private List<GameObject> asleepObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        asleepObjects.AddRange(GameObject.FindGameObjectsWithTag("asleep"));
        asleepObjects.AddRange(GameObject.FindGameObjectsWithTag("horizontal"));
        asleepObjects.AddRange(GameObject.FindGameObjectsWithTag("vertical"));

        foreach (GameObject obj in asleepObjects)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            pressCount++;
            bool isShown = (pressCount%2==1);

            foreach (GameObject obj in asleepObjects)
            {
                obj.SetActive(isShown);
            }
            Debug.Log("Z pressed Count = " + pressCount + " show: " + isShown +"");
        }
    }
}
