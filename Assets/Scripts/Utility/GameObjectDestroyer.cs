using UnityEngine;

public class GameObjectDestroyer : MonoBehaviour
{
    [SerializeField] private GameObject target;

    public void DestroyObject()
    {
        if (target) Destroy(target);
    }
}
