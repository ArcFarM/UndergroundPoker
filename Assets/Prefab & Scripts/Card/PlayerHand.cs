using System.Collections.Generic;
using System.Linq;
using UnderGroundPoker.Manager;
using UnityEngine;

namespace UnderGroundPoker.Prefab.Card
{
    // 포커 족보 열거형
    public enum HandRank
    {
        //조커가 있으므로 파이브카드 추가
        Top = 0,
        OnePair, TwoPair, Triple,
        Straight, BackStraight, Mountain,
        Flush, FullHouse, FourCard,
        StraightFlush, RoyalStraightFlush,
        FiveCard
    }

    public struct HandResult
    {
        //결과 족보
        public HandRank Rank;
        //족보가 같을 경우 승패를 가르기 위한 타이브레이커
        public List<CardRank> TieBreaker;
        public HandResult(List<CardRank> cardranks, HandRank rank = HandRank.Top)
        {
            Rank = rank;
            TieBreaker = cardranks;
        }
    }

    //partial 클래스 사용 이유 : 족보를 판별하는 메서드들을 따로 분리하기 위해서 - 가시성 목적
    public partial class PlayerHand : MonoBehaviour
    {
        #region Variables
        #region HandData
        //플레이어의 손패
        [SerializeField] private List<Card> hand = new List<Card>();
        //손패 크기
        private const int handSize = 5;
        //문양별 손패 정보
        Dictionary<CardSuit, int> suitCount = new();
        //숫자별 손패 정보
        Dictionary<CardRank, int> rankCount = new();
        //가능한 족보 정보
        [SerializeField] bool[] ranks = new bool[(int)HandRank.FiveCard];
        //빠른 평가를 위한 조커 보유 여부
        private bool hasJoker = false;
        //족보를 가진 카드들 - 정렬을 위해 따로 관리
        private List<Card> rankedCards = new List<Card>();
        #endregion
        //플레이어의 족보
        private HandResult result = new HandResult(new List<CardRank>());
        //베팅
        private int betting;
        
        #region Properties
        public List<Card> Hand => hand;
        public HandResult Result { get => result; }

        public int Betting { get => betting; set => betting = value; }
        public bool HasJoker { get => hasJoker; set => hasJoker = value; }
        #endregion

        #region Unity Event Methods
        private void Start()
        {
            //초기화 및 족보 받고, 평가하기 위한 준비
            AllClear();
            ranks[(int)HandRank.Top] = true; //무조건 탑은 가능
            //손패 정렬 실시
        }
        #endregion

        #region Card Methods
        public void AddCard(Card card)
        {
            //
            if(hand.Count < handSize)
                hand.Add(card);
            else
            {
                //TODO : 먼저 손패를 4장까지 줄이고 카드를 추가
            }
        }

        public void RemoveCard(Card card)
        {
            //해당 카드를 덱/버린 카드 더미에 반납하기
            List<GameObject> cards = new List<GameObject> { card.gameObject };
            GameManager.Instance.Carddeck.ReturnCard(cards);
            //그 후 손패에서 삭제
            hand.Remove(card);
            cards.Clear();
        }

        public void ReplaceCard(int index, Card card)
        {
            //내 손패의 index 위치에 있는 카드를 card로 교체
            RemoveCard(hand[index]);
            hand.Insert(index, card);
        }

        public void SortCard()
        {
            //족보를 이루기 전 : 숫자 >> 문양 순으로 정렬
            if(result.Rank == HandRank.Top)
            {
                //조커가 제일 앞에 오도록 내림차순 정렬, 문양은 Spade가 0이므로 오름차순 정렬
                List<Card> sortedByRank = hand.OrderByDescending(card => card.Rank).ToList();
                hand = sortedByRank.OrderBy(card => card.Suit).ToList();
            }
            //족보를 이룬 후 : 족보를 이루는 카드들 >> 나머지 카드들 순으로 같은 기준으로 정렬
            else
            {
                //족보를 가진 카드들 먼저 추가해서 서로 따로 정렬
                List<Card> sortedRankHand = rankedCards.OrderByDescending(card => card.Rank).ToList();
                sortedRankHand = sortedRankHand.OrderBy(card => card.Suit).ToList();
                hand = hand.OrderByDescending(card => card.Rank).ToList();
                hand = hand.OrderBy(card => card.Suit).ToList();

                List<Card> newHand = new List<Card>();
                foreach(Card card in sortedRankHand)
                {
                    newHand.Add(card);
                }
                foreach(Card card in hand)
                {
                    if(!rankedCards.Contains(card))
                        newHand.Add(card);
                }
                hand = newHand;
            }
        }
        #endregion

        void PrepareHand()
        {
            //초기화
            suitCount.Clear();
            rankCount.Clear();
            hasJoker = false;
            //손패를 숫자별, 문양별로 파악해놓기
            foreach (Card card in hand)
            {
                CardRank rank = card.Rank;
                CardSuit suit = card.Suit;

                if (rankCount.ContainsKey(rank))
                    rankCount[rank]++;
                else
                    rankCount[rank] = 1;

                if (suitCount.ContainsKey(suit))
                    suitCount[suit]++;
                else
                    suitCount[suit] = 1;
            }

            if (rankCount.ContainsKey(CardRank.Joker))
                hasJoker = true;
        }
        public void EvaluateHand()
        {
            //TODO : 손패 평가 로직 구현
            //1. 손패 정보 정리해놓기
            PrepareHand();
            //2. 손패 정보 기반으로 족보 판별하기
            //2-1. 조커가 없다면?
            if(!hasJoker)
            {
                CheckFlush();
            }
            else
            {
                //2-2. 조커가 있다면?
                CheckJokerFlush();
            }
            //2-3. 제일 높은 족보 반영 및 타이브레이커 설정
            for (int i = ranks.Length - 1; i >= 0; i--)
            {
                if (ranks[i])
                {
                    result.Rank = (HandRank)i;
                    Debug.Log((HandRank)i);
                    break;
                }
            }
            SetTieBreaker();
            //TODO : 특수 카드 처리하기
            //3. 베팅 단계로 넘어가기
        }

        public int CompareTo(HandResult mine, HandResult other)
        {
            //족보 비교
            if(mine.Rank == other.Rank)
            {
                //족보가 같다면 타이브레이커 비교
                for(int i = 0; i < mine.TieBreaker.Count; i++)
                {
                    CardRank myRank = mine.TieBreaker[i];
                    CardRank otherRank = other.TieBreaker[i];
                    if(myRank > otherRank)
                        return 1;
                    else if(myRank < otherRank)
                        return -1;
                }
                //타이브레이커가 존재하지 않거나, 타이브레이커까지 동일하면 무승부
                return 0;
            }
            if(mine.Rank > other.Rank)
                return 1;
            else
                return -1;
        }

        public void AllClear()
        {
            //다음 라운드를 위한 전체 초기화
            hand.Clear();
            suitCount.Clear();
            rankCount.Clear();
            ranks = new bool[(int)HandRank.FiveCard + 1];
            hasJoker = false;
            result = new HandResult(new List<CardRank>());
            rankedCards.Clear();
        }

        
        #endregion

        #region Debug Methods
        //디버그용 : 현재 손패 출력
        public void PrintHand()
        {
            string handString = "현재 손패: ";
            foreach (Card card in hand)
            {
                handString += $"{card.Suit}-{card.Rank} ";
            }
            Debug.Log(handString);
        }

        //디버그용 : 현재 족보 출력
        public void PrintHandResult()
        {
            string resultString = $"현재 족보: {result.Rank}, 타이브레이커: ";
            foreach (CardRank rank in result.TieBreaker)
            {
                resultString += $"{rank} ";
            }
            Debug.Log(resultString);
        }

        //디버그용 : 현재 문양별, 숫자별 손패 정보 출력
        public void PrintHandInfo()
        {
            string suitInfo = "문양별 손패 정보: ";
            foreach (var kvp in suitCount)
            {
                suitInfo += $"{kvp.Key}: {kvp.Value} ";
            }
            Debug.Log(suitInfo);
            string rankInfo = "숫자별 손패 정보: ";
            foreach (var kvp in rankCount)
            {
                rankInfo += $"{kvp.Key}: {kvp.Value} ";
            }
            Debug.Log(rankInfo);
        }

        //디버그용 : 현재 가능한 족보 정보 출력
        public void PrintPossibleRanks()
        {
            string ranksInfo = "가능한 족보 정보: ";
            for (int i = ranks.Length - 1; i >= 0; i--)
            {
                if (ranks[i])
                {
                    ranksInfo += $"{(HandRank)i} ";
                }
            }
            Debug.Log(ranksInfo);
        }
        #endregion
    }
}