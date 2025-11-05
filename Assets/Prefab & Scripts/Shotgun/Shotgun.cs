using System.Collections.Generic;
using UnderGroundPoker.Prefab.Manager;
using UnityEngine;

namespace UnderGroundPoker.Objects {
    public class Shotgun : MonoBehaviour {
        #region Variables
        //총알이 담길 배열
        [SerializeField] private int magsize = 6;
        private List<Bullet> mag;
        //진짜 총알 수와 가짜 총알 수 (UI 출력에 필요)
        int realBulletCount = 0;
        int falseBulletCount = 0;
        public int RealBulletCount { get { return realBulletCount; } private set { realBulletCount = value; } }
        public int FalseBulletCount { get { return falseBulletCount; } private set { falseBulletCount = value; } }
        public List<Bullet> Bulletinfo { get { return mag; } set { mag = value; } }
        #endregion

        private void Start() {
            //초기화
            Initialize();
        }

        public void Initialize() {
            if (mag == null)
                mag = new List<Bullet>();
            else mag.Clear();
        }

        #region Bullet Info Methods
        //탄창 정보 갱신
        public void RefreshMag()
        {
            RealBulletCount = 0;
            FalseBulletCount = 0;
            if (mag == null)
            {
                Initialize();
                return;
            }

            foreach (var bullet in mag)
            {
                if (bullet != null)
                {
                    if (bullet.isFalse)
                        FalseBulletCount++;
                    else
                        RealBulletCount++;
                }
            }
        }

        #endregion

        #region Shot Methods
        //정해진 수 만큼 총알 생성하여 장전
        public void Reload(int totalNum, int falseNum)
        {
            //진짜 총알 수와 가짜 총알 수 갱신
            RealBulletCount = totalNum - falseNum;
            FalseBulletCount = falseNum;

            int falseCount = 0;
            //정해진 수 만큼 가짜 총알/ 진짜 총알 생성해서 배열에 저장
            for (int i = 0; i < totalNum; i++)
            {
                //두 개의 if문을 하나로 통합하고, Add로 대체 - 탄창초기화 메서드에서 리스트 초기화만 함
                Bullet newBullet = new Bullet();
                newBullet.isFalse = falseCount < falseNum;
                if (newBullet.isFalse) falseCount++;
                mag.Add(newBullet);
            }

            //배열 무작위로 섞기 - Fisher–Yates 알고리즘 사용 (배열의 마지막 자리부터 1 ~ index 번째 위치한 파일 중 무작위로 선택된 것과 치환하는 방법)
            for (int i = mag.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                Bullet temp = mag[i];
                mag[i] = mag[j];
                mag[j] = temp;
            }
        }

        //매개변수로 받는 플레이어에게 총알 발사
        public void Fire(PlayerManager player)
        {
            if(player == null)
            {
                return;
            }

            if(mag == null)
            {
                return;
            }

            //총알을 꺼낼 수 있으면 리스트의 0번 총알 꺼내고 리스트에서 제거
            if (mag.Count == 0)
            {
                //발사 할 총알이 없으므로 종료, 라운드 종료 처리 필요
                return;
            }
            Bullet bullet = mag[0];
            mag.RemoveAt(0);

            //총알이 진짜라면 총알의 강화 여부를 확인하고 피해
            if (!bullet.isFalse)
            {
                if (bullet.isStrong)
                {
                    player.PlayerLife -= 2;
                }
                else player.PlayerLife--;
            }

            //발사 후 남은 총알 정보 갱신
            RefreshMag();
        }
        #endregion
    }

}
