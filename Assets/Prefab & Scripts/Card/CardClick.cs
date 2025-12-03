using UnityEngine;
using UnityEngine.EventSystems;
//각 카드에 붙어서 카드를 클릭을 통한 상호작용을 가능하게 만드는 컴포넌트

namespace UnderGroundPoker.Prefab.Card {
    //콜라이더가 있어야 상호작용 가능 - 없으면 강제로 부착
    [RequireComponent(typeof(Collider))]
    public class CardClick : MonoBehaviour, IPointerClickHandler {
        //이벤트로 카드 클릭 시 Card 컴포넌트가 '선택됨' 상태가 되도록 전환
        public Card Card;

        //최소 클릭 대기 시간 - 연속 클릭 혹은 의도하지 않은 중복 클릭 방지
        float minClickInterval = 0.1f;
        float lastClick = -1f;

        void Start() {
            if(Card == null) {
                Card = GetComponentInParent<Card>();
            }
            if(TryGetComponent<Card>(out Card card)) {
                Card = card;
            } else {
                Debug.LogWarning("카드 컴포넌트 없음");
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if(lastClick + minClickInterval > Time.time) {
                return;
            }

            lastClick = Time.time;

            if (Card != null) {
                Card.ToggleSelect();
            } else {
                Debug.LogWarning("카드 컴포넌트 없음");
            }
        }
    }

}
