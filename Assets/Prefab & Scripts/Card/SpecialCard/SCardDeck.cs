using UnityEngine;
using System.Collections.Generic;

namespace UnderGroundPoker.Prefab.Card
{
    //게임 진행에서 사용할 특수 카드 덱 (아이템 인벤토리)
    public class SCardDeck : MonoBehaviour
    {
        #region Variables
        //카드 프리팹
        public GameObject SpecialDeck;
        List<SpecialCard> specialCards = new List<SpecialCard>();
        public List<SpecialCard> SpecialCards => specialCards;
        #endregion

        #region Card Methods
        //특수 카드 덱 초기화
        public void DeckReset()
        {
            specialCards.Clear();
            //TODO : 특수 카드 덱 초기화 로직 구현
        }
        //특수 카드 덱에 카드 추가 (n장 무작위 추가하기, 특정 특수 카드 추가하기)
        public void AddSpecialCard(int n)
        {
            for (int i = 0; i < n; i++)
            {
                //TODO : 특수 카드 덱에 카드 추가 로직 구현
                //무작위로 카드풀에서 카드 생성하여 추가
            }
        }

        public void AddSpecialCard(SpecialCard card)
        {
            specialCards.Add(card);
        }
        //특수 카드 덱에서 카드 꺼내기
        public void PopSpecialCard()
        {
            //제일 마지막에 위치한 카드 꺼내기
            int last = specialCards.Count - 1;
            if (last < 0) return;
            SpecialCard card = specialCards[last];
            specialCards.RemoveAt(last);

            //TODO : 꺼낸 카드 사용 로직 구현
        }
        #endregion
    }
}