using UnityEngine;
using UnityEngine.UI;
using UnderGroundPoker.Manager;
using System.Collections.Generic;
using UnderGroundPoker.Prefab.Card;

public class MulliganTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(DoMulligan);
    }

    void DoMulligan() {
        //카드들 클릭 상호작용 가능 상태로 변경
        //테스트용 : 플레이어 0(유저)를 직접 타겟으로 멀리건 실행
        PlayerManager player = GameManager.Instance.Players[0];

        //1단계. 무작위로 카드 1 ~ 5 장을 골라서 덱에 집어넣고, 다른 카드로 교체
        //1 ~ 5 장 랜덤 선택
        int num = Random.Range(1, 6);
        //0 ~ 4 인덱스 중에서 num 개수만큼 순차적으로 선택

        while(player.PlayerHand.Hand.Count > 5 - num) {
            player.PlayerHand.RemoveCard(player.PlayerHand.Hand[0]);
        }

        GameManager.Instance.Carddeck.DeckShuffle();
        List<GameObject> newCards = GameManager.Instance.Carddeck.DrawCard(num);
        foreach (var card in newCards) {
            player.PlayerHand.AddCard(card.GetComponent<Card>());
        }

        //2단계. 카드를 상호작용 가능 상태로 변경해서, 유저가 직접 교체할 카드 선택 가능하도록 하기
    }

}
