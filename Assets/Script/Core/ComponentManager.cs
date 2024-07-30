using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    private MonoBehaviour[] components;

    private void Awake()
    {
        components = GetComponents<MonoBehaviour>();
    }

    public void DeactivateComponents()
    {
        foreach (var component in components)
        {
            if (component != null && component != this)
            {
                component.enabled = false;
            }
        }
        Debug.Log("Components deactivated.");
    }

    public void ActivateComponents()
    {
        foreach (var component in components)
        {
            if (component != null && component != this)
            {
                component.enabled = true;
            }
        }
        Debug.Log("Components activated.");
    }
}
