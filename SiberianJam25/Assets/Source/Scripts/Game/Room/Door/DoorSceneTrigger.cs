using UnityEngine;

public class DoorSceneTrigger : MonoBehaviour
{
    private RoomRoot _root;

    public void Initialize(RoomRoot root)
    {
        _root = root;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Player>(out Player player))
        {
           _root.TryLoadNextLevel();
        }
    }

}
