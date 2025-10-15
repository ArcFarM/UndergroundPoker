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
            foreach(var suit in suitCount)
            {
                if(suit.Value >= 5)
                {
                    ranks[(int)HandRank.Flush] = true;
                    //타이브레이커 설정
                    foreach(Card card in hand)
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
            int high = 0, low = (int)CardRank.Joker+1;
            foreach(var rank in rankCount)
            {
                if(rank.Value > 1)
                {
                    notStraight = true;
                    break;
                }
                if(rank.Value == 1)
                {
                    high = ((int)rank.Key > high) ? (int)rank.Key : high;
                    low = ((int)rank.Key < low) ? (int)rank.Key : low;
                }
            }

            if(notStraight)
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
                    if(!rankCount.ContainsKey((CardRank)i))
                    {
                        isBackStraight = false;
                        break;
                    }
                }
                if(isBackStraight)
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
            if(high == (int)CardRank.Ace && low == (int)CardRank.Ten)
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
            if(ranks[(int)HandRank.Flush] && ranks[(int)HandRank.Straight])
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
            foreach(var rank in rankCount)
            {
                if(rank.Value >= max)
                {
                    second = max;
                    max = rank.Value;
                    high = (int)rank.Key;
                }
            }

            switch(max)
            {
                case 4:
                    ranks[(int)HandRank.FourCard] = true;
                    //타이브레이커 설정
                    result.TieBreaker.Add((CardRank)high);
                    break;
                case 3:
                    ranks[(int)HandRank.Triple] = true;
                    //풀하우스 확인
                    if(second == 2)
                    {
                        ranks[(int)HandRank.FullHouse] = true;
                        //타이브레이커 설정
                        result.TieBreaker.Add((CardRank)high);
                    } else
                    {
                        //타이브레이커 설정
                        result.TieBreaker.Add((CardRank)high);
                    }
                    break;
                case 2:
                    ranks[(int)HandRank.OnePair] = true;
                    //투페어 확인
                    if(second == 2)
                    {
                        ranks[(int)HandRank.TwoPair] = true;
                        //타이브레이커 설정
                        result.TieBreaker.Add((CardRank)high);
                    } else
                    {
                        //타이브레이커 설정
                        result.TieBreaker.Add((CardRank)high);
                    }
                 break;
                //기본 탑/하이카드라서 default = 1은 필요없음
                default:
                break;
            }
            //가장 높은 족보 반영
            for (int i = (int)HandRank.FiveCard - 1; i >= 0; i--)
            {
                if(ranks[i])
                {
                    result.Rank = (HandRank)i;
                    break;
                }
            }
        }
        #endregion
        #region Hand Evaluator - With Joker
        void CheckJokerFlush()
        {
            CheckJokerStraight();
        }

        void CheckJokerStraight()
        {
            CheckJokerOther();
        }

        void CheckJokerOther()
        {
            Debug.Log("Joker Hand Evaluation Not Implemented yet");
            return;
        }
        #endregion
    }
}

