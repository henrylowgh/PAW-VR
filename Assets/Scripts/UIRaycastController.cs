using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIRaycastController : MonoBehaviour
{
    public LayerMask uiLayerMask;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, uiLayerMask))
        {
            Button button = hit.collider.GetComponent<Button>();
            if (button != null && Input.GetButtonDown("Fire1")) // Replace "Fire1" with your interaction key
            {
                button.onClick.Invoke();
            }
        }

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("Raycast hit: " + hit.collider.gameObject.name); // check if raycast is hitting things
        }

    }
}
