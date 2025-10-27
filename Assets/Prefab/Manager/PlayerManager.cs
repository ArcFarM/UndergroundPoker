using UnderGroundPoker.Prefab.Card;
using UnityEngine;

namespace UnderGroundPoker.Prefab.Manager {
    public class PlayerManager : MonoBehaviour {
        #region player variables
        //플레이어 목숨(칩)
        int playerLife;
        [SerializeField] int initialLife = 10;
        [SerializeField] int maxLife = 10;
        [SerializeField] int currentBet = 0;
        //플레이어 목숨 프로퍼티
        public int PlayerLife {
            get { return playerLife; }
            set {
                playerLife = Mathf.Clamp(value, 0, maxLife);
            }
        }
        //player hand
        PlayerHand playerHand;
        #endregion

        #region player life
        //플레이어 목숨 초기화
        public void InitPlayerLife() {
            PlayerLife = initialLife;
        }
        //플레이어 손패 초기화
        public void InitPlayerHand() {
            playerHand.AllClear();
        }
        //플레이어 특수카드 초기화
        
        //플레이어 목숨 증가/차감 << 프로퍼티 사용
        //플레이어 특수카드 사용 <<< 특수 카드 클래스에서 사용

        #endregion
    }

}

