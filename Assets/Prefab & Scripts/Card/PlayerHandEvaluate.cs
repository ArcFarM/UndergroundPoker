using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnderGroundPoker.Prefab.Card
{
    public partial class PlayerHand : MonoBehaviour
    {
        #region Hand Evaluator - No Joker
        void CheckFlush()
        {
            //5개 가진 문양이 있으면 true
            foreach (var suit in suitCount)
            {
                if (suit.Value >= 5)
                {
                    ranks[(int)HandRank.Flush] = true;
                    break;
                }
            }
            CheckStraight();
        }

        void CheckStraight()
        {
            //1. 2장 이상 가진 숫자가 있으면 false를 반환한다.
            bool notStraight = false;
            int high = 0, low = (int)CardRank.Joker + 1;
            foreach (var rank in rankCount)
            {
                if (rank.Value > 1)
                {
                    notStraight = true;
                    break;
                }
                if (rank.Value == 1)
                {
                    high = ((int)rank.Key > high) ? (int)rank.Key : high;
                    low = ((int)rank.Key < low) ? (int)rank.Key : low;
                }
            }

            if (notStraight)
            {
                CheckOther();
                return;
            }

            //2. 예외 : A 2 3 4 5 인 경우 백스트레이트
            if (rankCount.ContainsKey(CardRank.Ace))
            {
                bool isBackStraight = true;
                for (int i = (int)CardRank.Two; i <= (int)CardRank.Five; i++)
                {
                    if (!rankCount.ContainsKey((CardRank)i))
                    {
                        isBackStraight = false;
                        break;
                    }
                }
                if (isBackStraight)
                {
                    ranks[(int)HandRank.Straight] = true;
                    ranks[(int)HandRank.BackStraight] = true;
                    CheckOther();
                    return;
                }
            }
            //예외 : 마운틴 (10 J Q K A)
            if (high == (int)CardRank.Ace && low == (int)CardRank.Ten)
            {
                ranks[(int)HandRank.Straight] = true;
                ranks[(int)HandRank.Mountain] = true;
                CheckOther();
                return;
            }

            //3. 하이 - 로우 랭크 차이가 정확하게 4면 스트레이트
            if (high - low == 4)
            {
                ranks[(int)HandRank.Straight] = true;
                CheckOther();
                return;
            }

            //스트레이트 플러쉬 확인
            if (ranks[(int)HandRank.Flush] && ranks[(int)HandRank.Straight])
            {
                ranks[(int)HandRank.StraightFlush] = true;
                //로얄 스트레이트 플러쉬 확인
                if (ranks[(int)HandRank.Mountain])
                {
                    ranks[(int)HandRank.RoyalStraightFlush] = true;
                }
            }
            CheckOther();
        }

        void CheckOther()
        {
            //제일 많이 가지고 있는 숫자 찾기
            int high = 0;
            int second = 0;
            int max = 0;
            foreach (var rank in rankCount)
            {
                if (rank.Value >= max)
                {
                    second = max;
                    max = rank.Value;
                    high = (int)rank.Key;
                }
            }

            switch (max)
            {
                case 4:
                    ranks[(int)HandRank.FourCard] = true;
                    break;
                case 3:
                    ranks[(int)HandRank.Triple] = true;
                    //풀하우스 확인
                    if (second == 2)
                    {
                        ranks[(int)HandRank.FullHouse] = true;
                    }
                    break;
                case 2:
                    ranks[(int)HandRank.OnePair] = true;
                    //투페어 확인
                    if (second == 2)
                    {
                        ranks[(int)HandRank.TwoPair] = true;
                    }
                    break;
                //기본 탑/하이카드라서 default = 1은 필요없음
            }
   
        }
        #endregion
        #region Hand Evaluator - With Joker
        void CheckJokerFlush()
        {
            //조커가 있다면 나머지 문양이 4장이면 플러쉬
            foreach (var suit in suitCount)
            {
                if (suit.Value >= 4)
                {
                    ranks[(int)HandRank.Flush] = true;
                    break;
                }
            }
            CheckJokerStraight();
        }

        void CheckJokerStraight()
        {
            //1. 2장 이상 가진 숫자가 있으면 false를 반환한다.
            int sum = 0;
            int high = 0, low = (int)CardRank.Joker + 1;
            foreach (var rank in rankCount)
            {
                if (rank.Value > 1)
                {
                    CheckJokerOther();
                    return;
                }
                else
                {
                    //조커를 제외하고 하이카드 계산
                    if (rank.Key != CardRank.Joker)
                    {
                        sum += (int)rank.Key;
                        high = ((int)rank.Key > high) ? (int)rank.Key : high;
                        low = ((int)rank.Key < low) ? (int)rank.Key : low;
                    }
                }
            }

            //2. 예외 : 조커를 제외한 카드 수의 합 = sum에 대해 15 - sum이 1 ~ 5 라면 백스트레이트
            if (15 - sum >= 1 && 15 - sum <= 5)
            {
                ranks[(int)HandRank.Straight] = true;
                ranks[(int)HandRank.BackStraight] = true;
                CheckJokerOther();
                return;
            }
            //3. 예외 : 하이 카드가 K 이상, 로우 카드가 10이라면 마운틴
            if (high >= (int)CardRank.King && low == (int)CardRank.Ten)
            {
                ranks[(int)HandRank.Straight] = true;
                ranks[(int)HandRank.Mountain] = true;
                CheckJokerOther();
                return;
            }
            //4. 나머지 경우 계산
            if (high - low == 3 || high - low == 4)
            {
                ranks[(int)HandRank.Straight] = true;
                CheckJokerOther();
                return;
            }
            //5. 스티플/로티플 확인
            if (ranks[(int)HandRank.Flush] && ranks[(int)HandRank.Straight])
            {
                ranks[(int)HandRank.StraightFlush] = true;
                //로얄 스트레이트 플러쉬 확인
                if (ranks[(int)HandRank.Mountain])
                {
                    ranks[(int)HandRank.RoyalStraightFlush] = true;
                }
            }
            CheckJokerOther();
        }

        void CheckJokerOther()
        {
            //하이 카드, 로우 카드, 제일 많이 가진 것 확인
            int high = 0;
            int second = 0;
            int max = 0;

            foreach (var rank in rankCount)
            {
                if (rank.Value >= max && rank.Key != CardRank.Joker)
                {
                    second = max;
                    max = rank.Value;
                    high = (int)rank.Key;
                }
            }

            switch (max)
            {
                case 4:
                    ranks[(int)HandRank.FiveCard] = true;
                    ranks[(int)HandRank.FourCard] = true;
                    break;
                case 3:
                    ranks[(int)HandRank.FourCard] = true;
                    ranks[(int)HandRank.FullHouse] = true;
                    ranks[(int)HandRank.Triple] = true;
                    break;
                case 2:
                    ranks[(int)HandRank.Triple] = true;
                    ranks[(int)HandRank.TwoPair] = true;
                    ranks[(int)HandRank.OnePair] = true;
                    break;
                case 1:
                    ranks[(int)HandRank.OnePair] = true;
                    break;
            }
        }

        void SetTieBreaker() {
            //가진 족보 중 제일 강한 것을 기준으로 타이브레이커 설정
            //1. 손패를 정렬하고
            SortCard();
            //2. 손패를 중복되지 않게 리스트로 저장
            HashSet<CardRank> tiebreakers = new HashSet<CardRank>();
            foreach (var card in hand)
            {
                tiebreakers.Add(card.Rank);
            }
            result.TieBreaker = tiebreakers.ToList();
        }
        #endregion
    }
}