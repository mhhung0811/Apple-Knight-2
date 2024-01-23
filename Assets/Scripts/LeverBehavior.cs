using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBehavior : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float coolDown;
    private float timer;
    private bool isActive;
    [SerializeField]
    private GameObject InteractedObject;
    private IInteractable interactor;

    // Start is called before the first frame update
    void Start()
    {
        coolDown = 0.5f;
        timer = Time.time;
        interactor = InteractedObject.gameObject.GetComponent<IInteractable>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Switch()
    {
        if (Time.time - timer < coolDown) return;

        timer = Time.time;
        //Debug.Log("Active");

        if (isActive)
        {
            // turn off
            isActive = false;
            transform.RotateAround(transform.position, transform.up, 180);
            interactor.InteractOff();
        }
        else
        {
            // turn on
            isActive = true;
            transform.RotateAround(transform.position, transform.up, 180);
            interactor.InteractOn();
        }
    }
    public void InteractOn()
    {
        Switch();
    }
    public void InteractOff()
    {
        // Do nothing
    }
}
