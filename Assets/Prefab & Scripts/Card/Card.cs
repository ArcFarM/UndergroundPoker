using UnityEngine;

namespace UnderGroundPoker.Prefab.Card
{
    public enum CardSuit
    {
        Spade,
        Diamond,
        Heart,
        Club,
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
        //카드가 덱에 있는 지 여부
        bool isFront;
        public bool IsInDeck { get; set; }

        #endregion

        #region Properties
        public CardSuit Suit => suit;
        public CardRank Rank => rank;
        public bool IsSelected { get => isSelected; set => isSelected = value; }
        public bool IsJoker => rank == CardRank.Joker;
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

        public bool Equals(Card other)
        {
            return this.suit == other.suit && this.rank == other.rank;
        }

        public int CompareTo(Card other)
        {
            if (this.rank != other.rank)
            {
                return this.rank.CompareTo(other.rank);
            }
            else
            {
                return this.suit.CompareTo(other.suit);
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

        #region Display Methods
        public void ShowCardInfo()
        {
            Debug.Log($"Card Info - Suit: {suit}, Rank: {rank}, IsSelected: {isSelected}");
        }

        public override string ToString()
        {
            return $"{rank} of {suit}";
        }
        #endregion
        #endregion
    }
}
