using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBehavior : MonoBehaviour
{
    private bool isActive;
    [SerializeField]
    private GameObject InteractedObject;
    private IInteractable interactor;

    // Start is called before the first frame update
    void Start()
    {
        interactor = InteractedObject.gameObject.GetComponent<IInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Switch()
    {
        if (isActive)
        {
            // turn off
            isActive = false;
            transform.rotation.eulerAngles.Set(0, 0, 0);
            interactor.InteractOff();
        }
        else
        {
            // turn on
            isActive = true;
            transform.rotation.eulerAngles.Set(0, 180, 0);
            interactor.InteractOn();
        }
    }
}
