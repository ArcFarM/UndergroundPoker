using System;
using System.Collections.Generic;
using UnderGroundPoker.Manager;
using Unity.VisualScripting;
using UnityEngine;

namespace UnderGroundPoker.Prefab.Card {
    public class CardDeck : MonoBehaviour {

        #region Variables

        //전체 카드 목록과 현재 덱에 있는 카드 목록와 현재 덱에 없는 카드 목록
        Dictionary<(CardSuit, CardRank), GameObject> cardAll;
        HashSet<(CardSuit, CardRank)> cardInDeck;
        HashSet<(CardSuit, CardRank)> cardNotInDeck;

        public GameObject defaultDeck; //기본 덱
        public GameObject currDeck; //현재 덱

        #endregion

        #region Properties
        #endregion

        #region Unity Methods

        void Start() {
            DeckShuffle();
            FillCardInfo();
        }

        void FillCardInfo() {
            //null check
            if (defaultDeck == null) {
                Debug.LogError("Default Deck is not assigned.");
                return;
            }

            //덱 정보 채우기
            cardAll = new Dictionary<(CardSuit, CardRank), GameObject>();
            cardInDeck = new HashSet<(CardSuit, CardRank)>();
            cardNotInDeck = new HashSet<(CardSuit, CardRank)>();

            for (int i = 0; i < defaultDeck.transform.childCount; i++) {
                Card card = defaultDeck.transform.GetChild(i).GetComponent<Card>();
                var key = (card.Suit, card.Rank);
                cardAll.TryAdd(key, defaultDeck.transform.GetChild(i).gameObject);
                cardInDeck.Add(key);
            }
        }

        // Update is called once per frame
        void Update() {

        }

        #endregion

        #region Custom Methods

        #region Default Deck Functions
        public void DeckShuffle() {
            //카드 섞기 애니메이션 실행
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Shuffle");
        }

        public List<GameObject> DrawCard(int n) {
            //카드 n장 뽑기 - 평범한 5장 카드 포커이므로 덱이 모자랄 일은 없음
            List<GameObject> cards = new List<GameObject>();

            var suits = Enum.GetValues(typeof(CardSuit));
            var ranks = Enum.GetValues(typeof(CardRank));

            while (cards.Count < n) {
                //무작위 카드 선택
                CardSuit randomSuit = (CardSuit)suits.GetValue(UnityEngine.Random.Range(0, suits.Length));
                CardRank randomRank = (CardRank)ranks.GetValue(UnityEngine.Random.Range(0, ranks.Length));
                var key = (randomSuit, randomRank);
                //덱에 있는 카드인지 확인
                if (cardInDeck.Remove(key)) {
                    //덱에서 카드 제거 및 반환 리스트에 추가
                    cardNotInDeck.Add(key);
                    cards.Add(Instantiate(cardAll[key]));
                }
            }
            return cards;
        }

        public void ReturnCard(List<GameObject> cards) {
            //카드 덱에 카드 반납

            foreach (var cardObj in cards) {
                Card card = cardObj.GetComponent<Card>();
                var key = (card.Suit, card.Rank);
                if (cardNotInDeck.Contains(key)) {
                    cardNotInDeck.Remove(key);
                    cardInDeck.Add(key);
                }
                Destroy(cardObj);
            }
            cards.Clear();
        }

        public void DeckReset() {
            //덱 초기화
            cardInDeck.Clear();
            cardNotInDeck.Clear();
            foreach (var key in cardAll.Keys) {
                cardInDeck.Add(key);
            }
        }

        #endregion

        #region special card interaction
        #endregion

        #endregion




    }

}
