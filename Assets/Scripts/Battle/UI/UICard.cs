using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore {
  public class UICard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    private Card Card;

    public void Init(Card card) {
      Card = card;
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnPointerUp(PointerEventData eventData) {
    }
  }
}