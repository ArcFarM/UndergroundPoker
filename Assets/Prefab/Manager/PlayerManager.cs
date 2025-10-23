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
    }

}

