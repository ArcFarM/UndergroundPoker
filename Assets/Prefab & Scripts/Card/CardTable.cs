using UnderGroundPoker.Prefab.Card;
using UnderGroundPoker.Prefab.Manager;
using UnityEngine;

public class CardTable : MonoBehaviour
{
    #region Variables
    //전체 폭
    public float width = 5f;
    //이 카드 배치란을 사용할 플레이어
    [SerializeField] PlayerManager player;
    PlayerHand hand;

    //현재 카드 매수
    [SerializeField] int cardCount = 0;

    #endregion
    private void Start() {
        if(player != null)
        {
            hand = player.PlayerHand;
        }
    }

    private void Update() {
        if(player == null || hand.Hand.Count == 0)
        {
            Start();
            return;
        }

        if (cardCount != hand.Hand.Count)
        {
            ArrangeWidth();
        }
    }

    //Horizontal Layout을 사용한 것 처럼 일정한 간격을 두고 자동으로 배치 되게 하기
    void ArrangeWidth() {
        cardCount = hand.Hand.Count;

        if (cardCount == 0) return;

        float leftMost = -width / 2f;
        float rightMost = width / 2f;

        //시작/끝 위치
        Vector3 startPos = new Vector3(leftMost, 0, 0);
        Vector3 endPos = new Vector3(rightMost, 0, 0);

        for (int i = 0; i < cardCount; i++) {
            float t = (cardCount == 1) ? 0.5f : (float)i / (cardCount - 1);
            Vector3 targetPos = Vector3.Lerp(startPos, endPos, t);
            targetPos.z = 0f;

            Transform cardTf = hand.Hand[i].transform;
            cardTf.SetParent(this.transform);
            cardTf.localPosition = targetPos;
            cardTf.localRotation = Quaternion.identity;
            cardTf.localScale = Vector3.one;
        }
    }
}


