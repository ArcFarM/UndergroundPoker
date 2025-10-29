using UnderGroundPoker.Manager;
using UnityEngine;

namespace UnderGroundPoker.Animation
{
    /// <summary>
    /// 벅샷머신 애니메이션 이벤트 트리거용 클래스
    /// </summary>
    public class BuckShotEvent : MonoBehaviour
    {
        #region Variables
        [Header("참조")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private Rigidbody hangingLamp;
        [SerializeField] private ParticleSystem spark;

        private float impactForce = 10;
        #endregion

        #region Custom Method
        // Fire
        public void FireEffect()
        {
            //방향 무작위
            impactForce *= (Random.value > 0.5) ? 1 : -1;
            hangingLamp.AddForce(new Vector3(EffectManager.Instance.IsDoubled ? impactForce * 3 : impactForce, 0, 0), ForceMode.Impulse);

            // 여기에 vfx, sfx
            // (가짜 탄인지 진짜 탄인지 구분할 수 있는 프로퍼티 있으면 조건분기해서 차이 주기)
            //...
        }

        // TODO: 탄피 배출
        public void EjectShell()
        {

        }

        public void OverDrive()
        {
            spark.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            spark.Play();
        }
        #endregion
    }

}
