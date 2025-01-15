using UnityEngine.Events;
using UnityEngine;

public class VrButton : MonoBehaviour, IOnHoverImpulsable
{
    [SerializeField] private UnityEvent onClickEvent;
    public void OnClicked()
    {
        onClickEvent.Invoke();
    }
}