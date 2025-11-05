using System;
using System.Collections.Generic;
using UnderGroundPoker.Prefab.Manager;
using UnderGroundPoker.Objects;
using UnityEngine;
using UnderGroundPoker.Prefab.Card;

namespace UnderGroundPoker.Manager {

    //싱글톤 초기화에 사용될 인터페이스
    public interface IManagerReset {
        void Initialize();
    }

    public class GameManager : MonoBehaviour {
        //싱글톤 인스턴스
        #region Singleton Instance
        public static GameManager Instance { get; private set; }
        static GameManager instance;
        private void Awake() {
            if (Instance == null) {
                instance = this;
                Instance = instance;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
                return;
            }
        }

        //싱글톤 초기화


        #region Manager Variables
        //원활한 매니저 초기화를 위한 리스트와 인터페이스
        public List<IManagerReset> managerReset = new List<IManagerReset>();
        //각 매니저 객체들 - 각 매니저들은 Awake나 Start에서 자동으로 리스트에 추가됨
        [SerializeField] RoundManager roundManager;
        public RoundManager RoundManager { get { return roundManager; } }
        [SerializeField] List<PlayerManager> players;
        public List<PlayerManager> Players { get { return players; } }
        #endregion
        public void InitializeSingleton() {
            //TODO : 싱글톤 초기화

            //TODO : 모든 매니저 객체들 초기화
            foreach (IManagerReset manager in managerReset) {
                manager.Initialize();
            }
        }
        #endregion

        #region GameObjects
        //게임 내 주요 오브젝트들
        [SerializeField] Shotgun shotgun;
        public Shotgun Shotgun { get { return shotgun; } }
        [SerializeField] CardDeck carddeck;
        public CardDeck Carddeck { get { return carddeck; } }

        #endregion

        #region Game Control
        //게임 종료
        public void QuitGame() {
            Application.Quit();
        }
        //게임 일시중지 및 재개
        public void PauseToggle(bool pause) {
            Time.timeScale = pause ? 0 : 1;
        }
        //게임 초기화
        public void InitiateGame() {
            //TODO : 게임 초기화
        }
        //게임 재시작
        public void RestartGame() {
            //TODO : 게임 재시작
        }
        //게임 종료
        public void EndGame() {
            //TODO : 게임 종료
        }
        //승패 구분
        public void SetWinner(bool isPlayer) {
            if(isPlayer) {
                //TODO : 플레이어 승리
            } else {
                //TODO : 적 승리
            }
        }
        #endregion

        #region Events
        #endregion
    }

}
