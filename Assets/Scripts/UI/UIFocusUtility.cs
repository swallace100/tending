using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UIFocusUtility
{
    // Selects the first interactable Selectable under root so keyboard Submit (Enter/Space) has a target.
    public static void SelectFirst(GameObject root)
    {
        if (!root || !EventSystem.current) return;

        foreach (Selectable selectable in root.GetComponentsInChildren<Selectable>(false))
        {
            if (selectable.interactable)
            {
                EventSystem.current.SetSelectedGameObject(selectable.gameObject);
                return;
            }
        }
    }
}
