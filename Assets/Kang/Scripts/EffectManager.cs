using UnityEngine;
using UnderGroundPoker.Animation;
using System.Collections;

namespace UnderGroundPoker.Manager
{
    public class EffectManager : MonoBehaviour
    {
        #region Variables
        [Header("참조")]
        [SerializeField] private Animator aBuckShot;
        [SerializeField] private Transform rotatePart;

        [Header("Buckshot Machine")]
        [SerializeField] private float rotateDuration = 3f;

        private bool targetFixed = true;
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
        #endregion
    }

}
