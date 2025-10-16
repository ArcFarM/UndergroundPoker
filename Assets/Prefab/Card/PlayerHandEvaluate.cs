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
                    //타이브레이커 설정
                    foreach (Card card in hand)
                    {
                        result.TieBreaker.Add(card.Rank);
                    }
                    result.TieBreaker = result.TieBreaker.OrderByDescending(rank => rank).ToList();
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
                    //타이브레이커 설정
                    result.TieBreaker.Add(CardRank.Five);
                    CheckOther();
                    return;
                }
            }
            //예외 : 마운틴 (10 J Q K A)
            if (high == (int)CardRank.Ace && low == (int)CardRank.Ten)
            {
                ranks[(int)HandRank.Straight] = true;
                ranks[(int)HandRank.Mountain] = true;
                //타이브레이커 설정
                result.TieBreaker.Add(CardRank.Ace);
                CheckOther();
                return;
            }

            //3. 하이 - 로우 랭크 차이가 정확하게 4면 스트레이트
            if (high - low == 4)
            {
                ranks[(int)HandRank.Straight] = true;
                //타이브레이커 설정
                result.TieBreaker.Add((CardRank)high);
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
                // 타이브레이커 초기화 후 재설정
                result.TieBreaker.Clear();
                result.TieBreaker.Add((CardRank)high);
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
                    //타이브레이커 초기화 후 설정
                    result.TieBreaker.Clear();
                    result.TieBreaker.Add((CardRank)high);
                    // 키커 추가 (포카드가 아닌 나머지 1장)
                    foreach (var rank in rankCount)
                    {
                        if ((int)rank.Key != high)
                        {
                            result.TieBreaker.Add(rank.Key);
                            break;
                        }
                    }
                    break;
                case 3:
                    ranks[(int)HandRank.Triple] = true;
                    //풀하우스 확인
                    if (second == 2)
                    {
                        ranks[(int)HandRank.FullHouse] = true;
                        //타이브레이커 초기화 후 설정
                        result.TieBreaker.Clear();
                        result.TieBreaker.Add((CardRank)high);
                        // 페어 숫자 추가
                        foreach (var rank in rankCount)
                        {
                            if (rank.Value == 2)
                            {
                                result.TieBreaker.Add(rank.Key);
                                break;
                            }
                        }
                    }
                    else
                    {
                        //타이브레이커 초기화 후 설정
                        result.TieBreaker.Clear();
                        result.TieBreaker.Add((CardRank)high);
                        // 키커 2장 추가
                        foreach (var rank in rankCount.OrderByDescending(r => r.Key))
                        {
                            if ((int)rank.Key != high)
                            {
                                result.TieBreaker.Add(rank.Key);
                            }
                        }
                    }
                    break;
                case 2:
                    ranks[(int)HandRank.OnePair] = true;
                    //투페어 확인
                    if (second == 2)
                    {
                        ranks[(int)HandRank.TwoPair] = true;
                        //타이브레이커 초기화 후 설정
                        result.TieBreaker.Clear();
                        // 두 페어를 내림차순으로 추가
                        var pairs = rankCount.Where(r => r.Value == 2).OrderByDescending(r => r.Key).ToList();
                        result.TieBreaker.Add(pairs[0].Key);
                        result.TieBreaker.Add(pairs[1].Key);
                        // 키커 1장 추가
                        foreach (var rank in rankCount)
                        {
                            if (rank.Value == 1)
                            {
                                result.TieBreaker.Add(rank.Key);
                                break;
                            }
                        }
                    }
                    else
                    {
                        //타이브레이커 초기화 후 설정
                        result.TieBreaker.Clear();
                        result.TieBreaker.Add((CardRank)high);
                        // 키커 3장 추가
                        foreach (var rank in rankCount.OrderByDescending(r => r.Key))
                        {
                            if ((int)rank.Key != high)
                            {
                                result.TieBreaker.Add(rank.Key);
                            }
                        }
                    }
                    break;
                //기본 탑/하이카드라서 default = 1은 필요없음
                default:
                    //하이카드인 경우 - 타이브레이커 초기화 후 전체 카드 내림차순 추가
                    result.TieBreaker.Clear();
                    foreach (var rank in rankCount.OrderByDescending(r => r.Key))
                    {
                        result.TieBreaker.Add(rank.Key);
                    }
                    break;
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
                    //타이브레이커 설정
                    foreach (Card card in hand)
                    {
                        result.TieBreaker.Add(card.Rank);
                    }
                    //조커는 같은 족보면 무조건 이김
                    result.TieBreaker = result.TieBreaker.OrderByDescending(rank => rank).ToList();
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
                //타이브레이커 설정 - 조커가 포함된 패는 무조건 타이브레이커에서 승리한다.
                result.TieBreaker.Add(CardRank.Joker);
                CheckJokerOther();
                return;
            }
            //3. 예외 : 하이 카드가 K 이상, 로우 카드가 10이라면 마운틴
            if (high >= (int)CardRank.King && low == (int)CardRank.Ten)
            {
                ranks[(int)HandRank.Straight] = true;
                ranks[(int)HandRank.Mountain] = true;
                //타이브레이커 설정 - 조커가 포함된 패는 무조건 타이브레이커에서 승리한다.
                result.TieBreaker.Add(CardRank.Joker);
                CheckJokerOther();
                return;
            }
            //4. 나머지 경우 계산
            if (high - low == 3 || high - low == 4)
            {
                ranks[(int)HandRank.Straight] = true;
                //타이브레이커 설정 - 조커가 포함된 패는 무조건 타이브레이커에서 승리한다.
                result.TieBreaker.Add(CardRank.Joker);
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
                // 타이브레이커 초기화 후 재설정
                result.TieBreaker.Clear();
                result.TieBreaker.Add(CardRank.Joker);
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
                    //타이브레이커 초기화 후 설정
                    result.TieBreaker.Clear();
                    result.TieBreaker.Add(CardRank.Joker);
                    break;
                case 3:
                    ranks[(int)HandRank.FourCard] = true;
                    ranks[(int)HandRank.FullHouse] = true;
                    ranks[(int)HandRank.Triple] = true;
                    //타이브레이커 초기화 후 설정
                    result.TieBreaker.Clear();
                    result.TieBreaker.Add(CardRank.Joker);
                    result.TieBreaker.Add((CardRank)high);
                    break;
                case 2:
                    ranks[(int)HandRank.Triple] = true;
                    ranks[(int)HandRank.TwoPair] = true;
                    ranks[(int)HandRank.OnePair] = true;
                    //타이브레이커 초기화 후 설정
                    result.TieBreaker.Clear();
                    result.TieBreaker.Add(CardRank.Joker);
                    result.TieBreaker.Add((CardRank)high);
                    break;
                case 1:
                    ranks[(int)HandRank.OnePair] = true;
                    //타이브레이커 초기화 후 설정
                    result.TieBreaker.Clear();
                    result.TieBreaker.Add(CardRank.Joker);
                    result.TieBreaker.Add((CardRank)high);
                    break;
            }
        }
        #endregion
    }
}