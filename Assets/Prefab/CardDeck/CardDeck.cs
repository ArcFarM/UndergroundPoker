using System.Collections.Generic;
using UnityEngine;

namespace UnderGroundPoker.Prefab.Card
{
    public class CardDeck : MonoBehaviour
    {

        /*카드 덱이 구현해야 할 기능

        2. 카드 나눠주기
            1. 위에서부터 카드 5장 나눠주기
            2. 카드가 덱에서 뽑혀서 플레이어에게 전달되야 함
            3. 어떤 카드가 빠졌는 지 카드 덱이 알고 있어야 함
        3. 카드 교체
            1. 플레이어 또는 AI가 카드를 덱에 반납
            2. 받아서 카드를 섞고, 반납한 수 만큼 다시 뽑아서 주기
        4. 카드 현황에 따른 재배치
            1. 현재 덱 매수에 맞게 카드 덱 전체 쌓인 높이를 재배치
            2. 카드가 많을 수록 높이가 높아지고, 적을 수록 낮아짐
            3. 기본 설정 : 카드 덱 높이는 53(52 + 조커)장에 대해 0.265f = 장당 0.005f

         */
        #region Variables
        float baseHeight = 0.265f; //52장 + 조커 1장 (카드 1장당 0.005f)
        float cardHeight = 0.005f; //카드 1장당 높이

        List<GameObject> graveyard; //버린 카드 더미
        List<GameObject> removedCard; //제외된 카드 더미

        public GameObject defaultDeck; //기본 덱
        public GameObject currDeck; //현재 덱
        public GameObject beforeLastModify; //마지막 수정 이전 상태
        #endregion

        #region Properties
        public int CardCount => transform.childCount; //덱에 남아있는 카드 매수
        public bool IsEmtpy => transform.childCount == 0; //덱이 비었는지 여부


        #endregion

        #region Unity Methods

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            DeckShuffle();
        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion

        #region Custom Methods

        #region Default Deck Functions
        public void DeckShuffle()
        {
            /*
                        카드 섞기
            2. 무작위로 카드를 섞어 놓고
            3. 카드 섞기 애니메이션 실행
                */

            //자식 오브젝트들 중 활성화된 오브젝트들 가져오기
            int count = transform.childCount;
            GameObject[] cards = new GameObject[count];
            for (int i = 0; i < count; i++)
            {

                cards[i] = transform.GetChild(i).gameObject;

            }

            //무작위 인덱스로 재설정 및 높이도 재설정
            for (int i = 0; i < count * 5; i++)
            {
                int rnd = Random.Range(0, count);
                int idx = i % count;
                GameObject toChange = cards[idx];
                GameObject changeWith = cards[rnd];
                //인덱스 재설정
                toChange.transform.SetSiblingIndex(rnd);
                changeWith.transform.SetSiblingIndex(idx);
                //높이 재설정
                toChange.transform.localPosition = new Vector3(0, 0, rnd * cardHeight / baseHeight);
                changeWith.transform.localPosition = new Vector3(0, 0, idx * cardHeight / baseHeight);
            }

            //카드 섞기 애니메이션 실행
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Shuffle");
        }

        public void ArrangeHeight()
        {
            //덱 높이를 재조정
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform card = transform.GetChild(i);
                card.localPosition = new Vector3(0, 0, i * cardHeight / baseHeight);
            }
        }

        public List<GameObject> DrawCard(int n)
        {
            //덱 맨 위에서 카드 n장 뽑기 (안되면 가능한 만큼)
            List<GameObject> cards = new List<GameObject>();
            for (int i = 0; i < n; i++)
            {
                if (transform.childCount > 0)
                {
                    Transform topCard = transform.GetChild(transform.childCount - 1);
                    cards.Add(topCard.gameObject);
                    topCard.SetParent(null);
                    //TODO : 카드 뽑기 애니메이션 수행
                    /*
                        대상 플레이어 방향으로 이동 및 회전
                        */
                }
            }

            //카드 다 뽑고 높이 재조정
            ArrangeHeight();
            return cards;
        }

        public void ReturnCard(List<GameObject> cards)
        {
            //카드 덱에 카드 반납
            int count = transform.childCount;
            for(int i = 0; i < cards.Count; i++)
            {

                GameObject card = cards[i];
                //널 체크 및 중복 검사
                if (card == null || card.transform.parent == transform) continue;

                card.transform.SetParent(transform);
                card.transform.localPosition = new Vector3(0, 0, count * cardHeight / baseHeight);
                card.transform.localRotation = Quaternion.Euler(0, 0, 0);
                //TODO : 카드 반납 애니메이션 수행
                /*
                    대상 카드 덱 방향으로 이동 및 회전
                    */
            }
            //카드 다 반납하고 높이 재조정
            ArrangeHeight();
        }

        public void DiscardCard(List<GameObject> cards, List<GameObject> destination)
        {
            foreach (GameObject card in cards)
            {
                card.transform.SetParent(null);
                //TODO : 카드 버리기 애니메이션 수행
                /*
                    대상 카드 더미 방향으로 이동 및 회전
                    */
                destination.Add(card);
            }
        }

        public void DeckReset()
        {
            //덱 리셋
            //버린 카드들을 모두 덱에 반납하고 덱 섞기
            ReturnCard(graveyard);
            graveyard.Clear();
            DeckShuffle();
            ArrangeHeight();
        }

        public void TrueDeckReset()
        {
            //제외된 카드들까지 모두 한꺼번에 덱에 반납하고 덱 섞기
            ReturnCard(graveyard);
            graveyard.Clear();
            ReturnCard(removedCard);
            removedCard.Clear();
            DeckShuffle();
            ArrangeHeight();
        }

        public Card PeekCard() {
            return transform.GetChild(transform.childCount - 1).GetComponent<Card>();
        }
        #endregion

        #region special card interaction
        #endregion

        #endregion




    }

}
