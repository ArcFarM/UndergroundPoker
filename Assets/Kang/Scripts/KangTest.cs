using UnityEngine;
using UnderGroundPoker.Manager;

namespace Kang
{
    /// <summary>
    /// 기능 시뮬레이션용 클래스
    /// </summary>
    public class KangTest : MonoBehaviour
    {
        #region Variables

        #endregion

        #region Unity Event Method
        void Update()
        {
            // 적방향 회전
            if (Input.GetKeyDown(KeyCode.O))
            {
                EffectManager.Instance.RotateTowardEnemy();
                Debug.Log("1번 누름");
            }

            // 플레이어방향 회전
            if (Input.GetKeyDown(KeyCode.P))
            {
                EffectManager.Instance.RotateTowardPlayer();
                Debug.Log("2번 누름");
            }

            // 기본 격발
            if (Input.GetKeyDown(KeyCode.K))
            {
                EffectManager.Instance.PullTrigger();
            }

            // 2배 격발
            if (Input.GetKeyDown(KeyCode.L))
            {
                EffectManager.Instance.PullTrigger(true);
            }

            // 벅샷머신 리셋
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EffectManager.Instance.ResetBuckshotMachine();
            }
        }
        #endregion

    }

}
