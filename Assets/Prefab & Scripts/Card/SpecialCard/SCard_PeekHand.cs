using UnityEngine;
using UnderGroundPoker.Prefab.Manager;
using NUnit.Framework;


namespace UnderGroundPoker.Prefab.Card
{
    public class SCard_PeakHand : SpecialCard
    {
        public override void ApplyEffect(PlayerManager player)
        {
            //대상의 손패 중 무작위로 2장을 골라서 그 카드 프리팹의 복사본을 UI에 잠깐 표시
            var hand = player.PlayerHand.Hand;

            if (hand != null)
            {
                GameObject randCardCopy1 = new GameObject();
                GameObject randCardCopy2 = new GameObject();

                int randIndex1 = Random.Range(0, hand.Count);
                int randIndex2;
                while(true)
                {
                    randIndex2 = Random.Range(0, hand.Count);
                    if(randIndex2 != randIndex1)
                    {
                        break;
                    }
                }

                randCardCopy1 = Instantiate(hand[randIndex1].gameObject);
                randCardCopy2 = Instantiate(hand[randIndex2].gameObject);

                //TODO : 씬 내/UI 내에 카드 복사본 표시
            }

        }
    }

}
