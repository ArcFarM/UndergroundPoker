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
    int cardCount = 0;

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
        
        float leftMost = -width / 2f;
        float rightMost = width / 2f;

        Vector3 startPos = this.transform.position + new Vector3(leftMost, 0, 0);
        Vector3 endPos = this.transform.position + new Vector3(rightMost, 0, 0);

        for(int i = 0; i < cardCount; i++) {
            Vector3 targetPos = Vector3.Lerp(startPos, endPos, (float)i / cardCount);
            
            hand.Hand[i].transform.SetParent(this.transform);
            hand.Hand[i].transform.localPosition = targetPos;
            hand.Hand[i].transform.localRotation = Quaternion.identity;
            hand.Hand[i].transform.localScale = Vector3.one;
        }
    }
}


