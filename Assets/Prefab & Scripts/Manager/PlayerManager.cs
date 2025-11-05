using UnderGroundPoker.Manager;
using UnderGroundPoker.Prefab.Card;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnderGroundPoker.Prefab.Manager {
    //각  플레이어에 부착될 플레이어 매니저
    public class PlayerManager : MonoBehaviour, IManagerReset {
        #region player variables
        //플레이어 목숨(칩)
        int playerLife;
        [SerializeField] int initialLife = 5;
        [SerializeField] int maxLife = 5;
        [SerializeField] int currentBet = 0;
        public int CurrentBet => currentBet;
        [SerializeField] int maxBet = 0;
        //플레이어 목숨 프로퍼티
        public int PlayerLife {
            get { return playerLife; }
            set {
                playerLife = Mathf.Clamp(value, 0, maxLife);
            }
        }
        //플레이어 유저 여부
        [SerializeField] bool isUser = true;
        public bool IsUser => isUser;
        //player hand
        PlayerHand playerHand;
        public PlayerHand PlayerHand => playerHand;
        SCardDeck specialHand;
        public SCardDeck SpecialHand => specialHand;    
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
        //플레이어 특수카드 사용
        public void UseSpecialCard(SpecialCard card)
        {
            //TODO : 특수카드 사용 로직
            if(card.Equals(null))
            {
                Debug.LogWarning("No Special Card to use.");
                return;
            }

            if(card.TargetPlayer)
            {
                card.ApplyEffect(this);
            }
            else
            {
                //TODO : 상대 플레이어에게 효과 적용
                //GameManager.Instance.
            }
        }

        #endregion

        //초기화 : 게임 매니저에 자동으로 등록
        private void Awake() {
            GameManager.Instance.managerReset.Add(this);
        }

        public void Initialize() {
            InitPlayerLife();
            playerHand = GetComponent<PlayerHand>();
            InitPlayerHand();
        }

        //라운드 매니저를 통해 최대 한도를 얻고 베팅하기
        public void Bet()
        {
            int maxBet = GameManager.Instance.RoundManager.BetLimit;
            //TODO : UI를 통해 베팅 설정하기
            currentBet = Mathf.Clamp(currentBet, 1, maxBet); //최소 1의 베팅을 해야 함

            //이제 Roundmanager에서 플레이어의 베팅 금액을 확인하여 저장
        }
    }

}

