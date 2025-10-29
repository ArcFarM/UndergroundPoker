using UnityEngine;
using UnderGroundPoker.Animation;
using System.Collections;
using UnderGroundPoker.Utility;

namespace UnderGroundPoker.Manager
{
    public class EffectManager : Singleton<EffectManager>
    {
        #region Variables
        

        [Header("Buckshot Machine")]
        //참조
        [SerializeField] private Animator aBuckShot;
        [SerializeField] private Transform rotatePart;
        

        //수치값
        [SerializeField] private float rotateDuration = 3f;

        //플래그
        private bool targetFixed = true;
        #endregion

        #region Property
        public bool IsDoubled { get; private set; }
        #endregion

        #region Unity Event Method

        #endregion

        #region Custom Method
        public void RotateTowardPlayer()
        {
            if (!targetFixed) return;
            targetFixed = false;
            //애니메이터를 꼭 꺼줘야됨!!
            aBuckShot.enabled = false;
            StartCoroutine(TurnAround(false));
        }

        public void RotateTowardEnemy()
        {
            if (!targetFixed) return;
            targetFixed = false;
            aBuckShot.enabled = false;
            StartCoroutine(TurnAround(true));
        }

        private IEnumerator TurnAround(bool towardEnemy)
        {
            Debug.Log("코루틴 실행됨");

            Vector3 startRot = rotatePart.localEulerAngles;
            // 목표 회전값 (Y만 변경)
            Vector3 targetRot = new Vector3(startRot.x, towardEnemy ? 90f : -90f, startRot.z);

            float tValue = 0f;

            while (tValue < 1f)
            {
                tValue += Time.deltaTime / rotateDuration;
                float newY = Mathf.LerpAngle(startRot.y, targetRot.y, tValue);
                Vector3 newRot = new Vector3(startRot.x, newY, startRot.z);
                rotatePart.localEulerAngles = newRot;

                yield return null;
            }

            rotatePart.localEulerAngles = targetRot;
            targetFixed = true;
            aBuckShot.enabled = true;
        }

        public void ResetBuckshotMachine()
        {
            rotatePart.localEulerAngles = Vector3.zero;
        }


        // 격발 이벤트 발생시 호출
        public void PullTrigger(bool doubled = false)
        {
            IsDoubled = doubled;
            aBuckShot.SetTrigger(doubled ? AnimHash.doubleFire : AnimHash.singleFire);
        }
        #endregion
    }

}
