using UnderGroundPoker.Prefab.Card;
using UnityEngine;


namespace UnderGroundPoker.Manager {
    public class RoundManager : MonoBehaviour, IManagerReset {
        #region Initialization
        private void Awake() {
            //매니저 리스트에 자신 추가
            GameManager.Instance.managerReset.Add(this);
        }

        //초기화 함수
        public void Initialize() {
            //TODO : 라운드 매니저 초기화
        }
        #endregion

        #region Round Variables
        int roundNum = 0;
        int maxRound = 10; //만약 라운드 수에 제한을 둔다면
        //라운드 시작 시 지급 될 특수 카드 풀
        //TODO : 특수 카드 풀과 특수 카드 지급량

        #endregion
        #region Round Methods
        public void RoundStart(int num) {
            //라운드 시작
        }

        public void RoundEnd() {
            //라운드 종료 처리
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