using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Lobby
{
    public class SelectButton : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] private GameObject _selected;
        void OnEnable()
        {
            _selected.SetActive(false);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            _selected.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _selected.SetActive(false);
        }
    }
}
