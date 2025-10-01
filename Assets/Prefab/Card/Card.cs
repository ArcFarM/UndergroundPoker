using UnityEngine;

namespace UnderGroundPoker.Prefab.Card
{
    public enum CardSuit
    {
        Spades,
        Diamonds,
        Hearts,
        Clubs,
        Joker
    }

    public enum CardRank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace,
        Joker
    }

    public class Card : MonoBehaviour
    {
        #region Variables
        //카드 정보들
        [SerializeField] private CardSuit suit;
        [SerializeField] private CardRank rank;
        //카드 선택 여부
        [SerializeField] private bool isSelected;
        //참고할 카드 프리팹
        [SerializeField] private GameObject cardPrefab;
        //카드 뒤집기 함수 용
        bool isFront;

        #endregion

        #region Properties
        public CardSuit Suit => suit;
        public CardRank Rank => rank;
        public bool IsSelected { get => isSelected; set => isSelected = value; }
        #endregion

        #region Unity Methods
        private void Start()
        {
            Initialize();
        }
        #endregion

        #region Custom Methods
        //카드 정보 설정
        private void Initialize()
        {
            //카드 프리팹에서 카드 정보 가져오기
            isSelected = false;
            isFront = false;
        }

        public void FlipCard()
        {
            if (isFront)
            {
                //카드 뒤집기 (앞면 -> 뒷면)
                transform.rotation = Quaternion.Euler(0, 180, 0);
                isFront = false;
            }
            else
            {
                //카드 뒤집기 (뒷면 -> 앞면)
                transform.rotation = Quaternion.Euler(0, 0, 0);
                isFront = true;
            }
        }

        public void ToggleSelect()
        {
            if(isSelected)
            {
                //선택 시각적 효과 제거
                //비선택 상태로 돌아가기
            }
            else
            {
                //선택 시각적 효과 추가
                //선택 상태로 변경
            }
        }
        #endregion
    }
}
