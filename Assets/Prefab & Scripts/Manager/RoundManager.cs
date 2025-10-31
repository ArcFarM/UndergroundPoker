using UnderGroundPoker.Objects;
using UnderGroundPoker.Prefab.Card;
using UnityEngine;


namespace UnderGroundPoker.Manager {
    public class RoundManager : MonoBehaviour, IManagerReset {

        #region RoundInfo
        //각 스테이지 별 한 라운드 당 생성할 총알과 지급할 아이템 = 특수 카드의 정보

        class StageInfo {
            public int[] TotalBulletCount;
            public int[] FalseBulletCount;
            public int[] SpecialCardCount;
            public int maxStage;

            public StageInfo(int[] totalBulletCount = null, int[] falseBulletCount = null, int[] specialCardCount = null) {
                TotalBulletCount = totalBulletCount;
                FalseBulletCount = falseBulletCount;
                SpecialCardCount = specialCardCount;
            }

            public void Initialize() {
                for(int i = 0; i < maxStage; i++) {
                    TotalBulletCount[i] = (int)System.Enum.Parse(typeof(TotalBulletCount), "Stage" + (i + 1));
                    FalseBulletCount[i] = (int)System.Enum.Parse(typeof(FalseBulletCount), "Stage" + (i + 1));
                    SpecialCardCount[i] = (int)System.Enum.Parse(typeof(SpecialCardCount), "Stage" + (i + 1));
                }
            }
        }
        public enum TotalBulletCount {
            Stage1 = 5,
            Stage2 = 7,
            Stage3 = 9
        }

        public enum FalseBulletCount {
            Stage1 = 2,
            Stage2 = 3,
            Stage3 = 4
        }
        public enum SpecialCardCount {
            Stage1 = 2,
            Stage2 = 3,
            Stage3 = 4
        }
        #endregion

        #region Initialization
        private void Awake() {
            //매니저 리스트에 자신 추가
            GameManager.Instance.managerReset.Add(this);
            //게임 매니저를 통해 할당된 샷건 오브젝트 가져오기
            shotgun = GameManager.Instance.Shotgun;
            //스테이지 정보 초기화
            Initialize();
        }

        //초기화 함수
        public void Initialize() {
            //TODO : 라운드 매니저 초기화
            stageinfo = new StageInfo();
            stageinfo.maxStage = maxStage;
            stageinfo.Initialize();
        }
        #endregion

        #region Round Variables
        int roundNum = 0;
        int maxStage = 3; //만약 라운드 수에 제한을 둔다면
        //라운드 시작 시 지급 될 특수 카드 풀
        //TODO : 특수 카드 풀과 특수 카드 지급량 << 얘는 게임 매니저에서 각 플레이어의 특수 카드 풀 오브젝트에 접근하기
        Shotgun shotgun;
        //Stage Info
        StageInfo stageinfo;

        #endregion
        #region Round Methods
        public void RoundStart(int num) {
            //라운드 시작

            //샷건에 무작위로 총알 생성 및 장전하기
            //int currStage = ****;
            //int 
            //shotgun.Reload(TotalBulletCount)
            //각 플레이어에게 특수 카드 지급하기

            //각 플레이어에게 카드 나눠주기
        }

        public void RoundEnd() {
            //라운드 종료 처리

            //양 측의 패를 비교해서 승패 결정

            //승자/패자 처리 - 승자가 베팅한 만큼 발사하기

            //라운드 수 증가

            //카드 패 회수하고 덱 섞기

            //총알을 모두 소비했다면 새 라운드 시작하기

            //어느 한 플레이어가 죽었다면 다음 스테이지 시작하기 (현재는 1스테이지 기획이므로 바로 게임 엔딩)
        }

        public void Prepare() {
            //다음 라운드 준비

            
        }


        #endregion

        #region Phase Setting
        //각 라운드의 단계 = 페이즈 설정
        public enum RoundPhase {

        }

        //각 단계 관리
        public void PhaseControl(RoundPhase phase) {
            switch (phase) {
                //TODO : 각 페이즈에 따른 처리
            }
        }

        //각 페이즈 진입/종료 시 실시할 함수
        public void OnPhaseEnter(RoundPhase phase) {
            switch (phase) {
                //TODO : 각 페이즈 진입 시 처리
            }
        }

        public void OnPhaseExit(RoundPhase phase) {
            switch (phase) {
                //TODO : 각 페이즈 종료 시 처리
            }
        }

        #region Phase Methods
        //TODO : OnPhaseEnter/Exit에서 사용할 각 페이즈별 함수들
        #endregion


        #endregion
    }
}