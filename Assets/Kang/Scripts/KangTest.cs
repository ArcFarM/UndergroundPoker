using UnityEngine;
using UnderGroundPoker.Manager;

namespace Kang
{
    public class KangTest : MonoBehaviour
    {
        #region Varaibles
        [SerializeField] private EffectManager eManager;
        #endregion

        #region Unity Event Method
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                eManager.RotateTowardEnemy();
                Debug.Log("1번 누름");
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                eManager.RotateTowardPlayer();
                Debug.Log("2번 누름");
            }
        }
        #endregion

    }

}
