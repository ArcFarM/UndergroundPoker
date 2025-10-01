using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnderGroundPoker.Prefab.Card
{
    // 포커 족보 열거형
    public enum HandRank
    {
        //조커가 있으므로 파이브카드 추가
        Top = 0,
        OnePair = 1, TwoPair = 2,
        Triple = 3, Straight = 4, Flush = 5,
        FullHouse = 6, FourCard = 7,
        StraightFlush = 8, RoyalStraightFlsuh = 9,
        FiveCard = 10
    }

    public class PlayerHand : MonoBehaviour
    {
        #region Variables
        #region hand Info
        [SerializeField] private List<Card> hand = new List<Card>(); // 보유 카드 5장
        private HandRank currentHandRank; // 현재 족보
        private string handScore; // 같은 족보 비교 점수
        #endregion

        #region Eval Options
        [SerializeField] private bool includeWheel = true; // 휠(A2345) 허용
        [SerializeField] private bool useSuitPriority = true; // 무늬 우선순위 사용
        #endregion

        #region Preprocessed
        private bool hasJoker; // 조커 개수
        private List<Card> realCards; // 비조커 카드 목록
        private Dictionary<CardRank, int> rankCounts; // 랭크별 개수
        private Dictionary<CardSuit, int> suitCount; // 무늬별 개수
        private Dictionary<CardSuit, HashSet<CardRank>> suitRanks; // 무늬별 랭크 집합
        #endregion
        #endregion

        #region Properties
        public List<Card> Cards => hand; // 보유 카드 조회
        public HandRank CurrentHandRank => currentHandRank; // 현재 족보 조회
        public string HandScore => handScore; // 현재 점수 조회
        public int CardCount => hand.Count; // 카드 장수 조회
        #endregion

        #region Unity Methods
        void Start()
        {
            // 핸드 초기화
        }
        #endregion

        #region Custom Methods
        #region 카드 관리
        public void Mulligan(List<Card> toChange, CardDeck deck)
        {
            // 선택 카드를 덱에 반납하고 교체
            /*Linq 사용 : gameobject list를 받는 deck.ReturnCard()에 맞추기 위해
             * card 리스트에서 card => card.gameObject로 변환한 리스트를 전달하고,
             * 기존 리스트에서 교체할 카드 제외 후 덱에서 새 카드를 받아서 추가.
             * 마찬가지로 gameobject 리스트에서 card 컴포넌트로 변환하여 hand에 추가.
             * */
            List<GameObject> gameObjects = toChange.Select((Card card) => card.gameObject).ToList();
            hand = hand.Except(toChange).ToList();

            deck.ReturnCard(gameObjects);

            List<GameObject> newCards = deck.DrawCard(toChange.Count);
            hand.AddRange(newCards.Select((GameObject go) => go.GetComponent<Card>()));
        }

        public void SortCards()
        {
            // 카드 정렬 규칙 적용
            
        }
        #endregion

        #region 족보 평가
        /*public HandRank EvaluateHand()
        {
            // 전처리 실행
            PrepareHand();

            // 상위 족보부터 판정

            // 하이카드 점수 산출
        }*/

        private void PrepareHand()
        {
            // 조커 개수 계산
            
            // 비조커 목록 생성

            // 랭크별 개수 집계
        }

       
        #endregion

        #region Hand Evaluation Helper
        #endregion

        #region Hand Comparison
        public bool CheckHand(PlayerHand otherHand)
        {
            // 두 핸드 점수 비교로 승패 판정
            return false;
        }
        #endregion
        #endregion
    }
}