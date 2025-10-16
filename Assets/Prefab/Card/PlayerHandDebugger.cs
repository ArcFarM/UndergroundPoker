using UnityEngine;
using UnityEngine.UI;

namespace UnderGroundPoker.Prefab.Card
{
    public class PlayerHandDebugger : MonoBehaviour
    {
        [SerializeField] private PlayerHand testHand;

        private void Start()
        {
            this.GetComponent<Button>().onClick.AddListener(() =>
            {
                Test();
            });
        }

        void Test()
        {
            Debug.Log("손패 정렬 테스트");
            testHand.PrintHand();
            testHand.EvaluateHand();
            testHand.SortCard();
            testHand.PrintHand();
            Debug.Log("족보 평가 테스트");
            testHand.PrintHandInfo();
            testHand.PrintPossibleRanks();
            testHand.PrintHandResult();
        }
    }
}