using UnityEngine;
using UnityEngine.UI;

namespace UnderGroundPoker.Prefab.Card {
    //특수 카드 클래스
    public abstract class SpecialCard {
        //다른 특수카드들의 모태가 되는 원판 : 상속 전용 클래스

        //특수 카드 공통 속성 : 이름, 설명, 아이콘 - 카드 프리팹, 효과 등
        //카드 마우스 오버 시 효과 설명을 팝업으로 보여줘야 함
        public string CardName { get; protected set; }
        //카드 설명은 Inspector를 통해 수정하도록 설정
        public string Description { get; protected set; }
        [SerializeField] protected GameObject card;

        [SerializeField] protected bool targetPlayer; //참이면 플레이어 = 나를 대상
        public bool TargetPlayer => targetPlayer;

        public void ShowCardInfo()
        {
            //TODO : 카드 정보 팝업 구현
            Debug.Log($"Card Name: {CardName}\nDescription: {Description}");
        }

        public abstract void ApplyEffect(GameObject player);
        //매개변수 player : 카드 효과를 받을 대상

    }
}