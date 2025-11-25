using UnityEngine;
using UnderGroundPoker.Manager;
using UnderGroundPoker.Manager;

namespace UnderGroundPoker.Prefab.Card
{
    public class SCard_ChangeCard : MonoBehaviour
    {
        //카드 한 장을 골라서 덱에 반환하고, 덱을 섞은 다음 새 카드 한 장을 뽑아 손패에 추가
        public void ApplyEffect(PlayerManager player)
        {
            var hand = player.PlayerHand;
            var cards = hand.Hand;
            int index = -1;

            CardDeck deck = GameManager.Instance.Carddeck;
            //TODO : 플레이어에게 카드 선택 UI 표시 후 index 값 받아오기
            if (hand != null && cards.Count > 0)
            {
                if(index >= 0)
                {
                    hand.RemoveCard(cards[index]);
                    deck.DeckShuffle();
                    GameObject newCard = deck.DrawCard(1)[0];
                    hand.AddCard(newCard.GetComponent<Card>());
                    hand.SortCard();
                }
            }
        }
    }
}
