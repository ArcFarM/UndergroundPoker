using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnderGroundPoker.Prefab.Card;

namespace UnderGroundPoker.GameDebug
{
    public class CardDeckDebugger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardDeck targetDeck;

        [Header("Debug Settings")]
        [SerializeField] private bool enableKeyboardInput = true;
        [SerializeField] private bool showDebugUI = true;
        [SerializeField] private int defaultDrawCount = 5;

        [Header("Auto Test")]
        [SerializeField] private bool runAutoTestOnStart = false;
        [SerializeField] private float testDelaySeconds = 1f;

        // 테스트용 핸드
        private List<GameObject> testHand = new List<GameObject>();

        // UI 위치
        private Rect windowRect = new Rect(10, 10, 320, 600);

        void Start()
        {
            if (targetDeck == null)
            {
                targetDeck = GetComponent<CardDeck>();
                if (targetDeck == null)
                {
                    UnityEngine.Debug.LogError("CardDeck not found! Please assign it in Inspector.");
                    enabled = false;
                    return;
                }
            }

            if (runAutoTestOnStart)
            {
                StartCoroutine(RunAutoTests());
            }
        }

        void Update()
        {
            if (!enableKeyboardInput) return;

            // 숫자 키로 빠른 테스트
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TestDrawCards(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                TestDrawCards(defaultDrawCount);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                TestReturnCards();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                TestShuffle();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                TestArrangeHeight();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                TestDeckReset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                PrintDeckInfo();
            }
        }

        void OnGUI()
        {
            if (!showDebugUI) return;

            windowRect = GUILayout.Window(0, windowRect, DrawDebugWindow, "Card Deck Debugger");
        }

        void DrawDebugWindow(int windowID)
        {
            GUILayout.BeginVertical();

            // 현재 상태 표시
            GUILayout.Box("=== Current State ===");
            GUILayout.Label($"Cards in Deck: {targetDeck.CardCount}");
            GUILayout.Label($"Cards in Test Hand: {testHand.Count}");
            GUILayout.Label($"Is Empty: {targetDeck.IsEmtpy}");

            GUILayout.Space(10);

            // 카드 뽑기
            GUILayout.Box("=== Draw Cards ===");
            if (GUILayout.Button("Draw 1 Card (Key: 1)"))
            {
                TestDrawCards(1);
            }
            if (GUILayout.Button($"Draw {defaultDrawCount} Cards (Key: 2)"))
            {
                TestDrawCards(defaultDrawCount);
            }

            GUILayout.Space(5);

            // 카드 반납
            GUILayout.Box("=== Return Cards ===");
            GUI.enabled = testHand.Count > 0;
            if (GUILayout.Button($"Return All Cards (Key: 3)"))
            {
                TestReturnCards();
            }
            if (GUILayout.Button("Return Random Half"))
            {
                TestReturnRandomCards();
            }
            GUI.enabled = true;

            GUILayout.Space(5);

            // 덱 조작
            GUILayout.Box("=== Deck Operations ===");
            if (GUILayout.Button("Shuffle Deck (Key: 4)"))
            {
                TestShuffle();
            }
            if (GUILayout.Button("Arrange Height (Key: 5)"))
            {
                TestArrangeHeight();
            }
            if (GUILayout.Button("Deck Reset (Key: 6)"))
            {
                TestDeckReset();
            }

            GUILayout.Space(5);

            // 정보 출력
            GUILayout.Box("=== Information ===");
            if (GUILayout.Button("Print Deck Info (Key: 0)"))
            {
                PrintDeckInfo();
            }
            if (GUILayout.Button("Print Test Hand"))
            {
                PrintTestHand();
            }

            GUILayout.Space(5);

            // 자동 테스트
            GUILayout.Box("=== Auto Test ===");
            if (GUILayout.Button("Run All Tests"))
            {
                StartCoroutine(RunAutoTests());
            }

            GUILayout.Space(5);

            // 초기화
            GUILayout.Box("=== Reset ===");
            if (GUILayout.Button("Clear Test Hand"))
            {
                testHand.Clear();
                UnityEngine.Debug.Log("[Debug] Test hand cleared");
            }

            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        #region Test Methods

        private void TestDrawCards(int count)
        {
            UnityEngine.Debug.Log($"[Test] Drawing {count} cards...");
            List<GameObject> drawnCards = targetDeck.DrawCard(count);
            testHand.AddRange(drawnCards);
            UnityEngine.Debug.Log($"[Test] Drew {drawnCards.Count} cards. Hand size: {testHand.Count}, Deck: {targetDeck.CardCount}");
        }

        private void TestReturnCards()
        {
            if (testHand.Count == 0)
            {
                UnityEngine.Debug.LogWarning("[Test] No cards in test hand to return");
                return;
            }

            UnityEngine.Debug.Log($"[Test] Returning {testHand.Count} cards...");
            targetDeck.ReturnCard(testHand);
            UnityEngine.Debug.Log($"[Test] Returned all cards. Deck: {targetDeck.CardCount}");
            testHand.Clear();
        }

        private void TestReturnRandomCards()
        {
            if (testHand.Count == 0)
            {
                UnityEngine.Debug.LogWarning("[Test] No cards in test hand");
                return;
            }

            int returnCount = Mathf.Max(1, testHand.Count / 2);
            List<GameObject> toReturn = new List<GameObject>();

            for (int i = 0; i < returnCount; i++)
            {
                int randomIndex = Random.Range(0, testHand.Count);
                toReturn.Add(testHand[randomIndex]);
                testHand.RemoveAt(randomIndex);
            }

            UnityEngine.Debug.Log($"[Test] Returning {toReturn.Count} random cards...");
            targetDeck.ReturnCard(toReturn);
            UnityEngine.Debug.Log($"[Test] Returned. Hand: {testHand.Count}, Deck: {targetDeck.CardCount}");
        }

        private void TestShuffle()
        {
            UnityEngine.Debug.Log("[Test] Shuffling deck...");
            targetDeck.DeckShuffle();
            UnityEngine.Debug.Log("[Test] Deck shuffled");
        }

        private void TestArrangeHeight()
        {
            UnityEngine.Debug.Log("[Test] Arranging height...");
            targetDeck.ArrangeHeight();
            UnityEngine.Debug.Log("[Test] Height arranged");
        }

        private void TestDeckReset()
        {
            UnityEngine.Debug.Log("[Test] Resetting deck...");
            testHand.Clear();
            targetDeck.DeckReset();
            UnityEngine.Debug.Log($"[Test] Deck reset. Cards: {targetDeck.CardCount}");
        }

        #endregion

        #region Information Methods

        private void PrintDeckInfo()
        {
            UnityEngine.Debug.Log("========== Deck Information ==========");
            UnityEngine.Debug.Log($"Total Cards: {targetDeck.CardCount}");
            UnityEngine.Debug.Log($"Is Empty: {targetDeck.IsEmtpy}");
            UnityEngine.Debug.Log($"Cards in Test Hand: {testHand.Count}");
            UnityEngine.Debug.Log("--- Card Stack (Bottom to Top) ---");

            for (int i = 0; i < targetDeck.transform.childCount; i++)
            {
                Transform card = targetDeck.transform.GetChild(i);
                string cardName = card.GetComponent<Card>() != null
                    ? card.GetComponent<Card>().ToString()
                    : card.name;
                UnityEngine.Debug.Log($"[{i}] {cardName} | Position: {card.localPosition} | Parent: {card.parent.name}");
            }
            UnityEngine.Debug.Log("=====================================");
        }

        private void PrintTestHand()
        {
            UnityEngine.Debug.Log("========== Test Hand ==========");
            UnityEngine.Debug.Log($"Total Cards: {testHand.Count}");

            for (int i = 0; i < testHand.Count; i++)
            {
                GameObject card = testHand[i];
                if (card == null)
                {
                    UnityEngine.Debug.Log($"[{i}] NULL CARD");
                    continue;
                }

                string cardName = card.GetComponent<Card>() != null
                    ? card.GetComponent<Card>().ToString()
                    : card.name;
                UnityEngine.Debug.Log($"[{i}] {cardName} | Position: {card.transform.position}");
            }
            UnityEngine.Debug.Log("==============================");
        }

        #endregion

        #region Auto Test

        private IEnumerator RunAutoTests()
        {
            UnityEngine.Debug.Log("========== Starting Auto Tests ==========");
            testHand.Clear();

            yield return new WaitForSeconds(testDelaySeconds);

            // Test 1: Draw cards
            UnityEngine.Debug.Log(">>> Test 1: Drawing 5 cards");
            var hand = targetDeck.DrawCard(5);
            testHand.AddRange(hand);
            Debug.Assert(hand.Count == 5, "Test 1 Failed: Should draw 5 cards");
            Debug.Assert(targetDeck.CardCount == 48, "Test 1 Failed: Should have 48 cards left");
            UnityEngine.Debug.Log("✓ Test 1 Passed: Drew 5 cards successfully");

            yield return new WaitForSeconds(testDelaySeconds);

            // Test 2: Return cards
            UnityEngine.Debug.Log(">>> Test 2: Returning 5 cards");
            targetDeck.ReturnCard(testHand);
            Debug.Assert(targetDeck.CardCount == 53, "Test 2 Failed: Should have 53 cards");
            UnityEngine.Debug.Log("✓ Test 2 Passed: Returned cards successfully");
            testHand.Clear();

            yield return new WaitForSeconds(testDelaySeconds);

            // Test 3: Shuffle
            UnityEngine.Debug.Log(">>> Test 3: Shuffling deck");
            targetDeck.DeckShuffle();
            Debug.Assert(targetDeck.CardCount == 53, "Test 3 Failed: Card count changed after shuffle");
            UnityEngine.Debug.Log("✓ Test 3 Passed: Shuffled successfully");

            yield return new WaitForSeconds(testDelaySeconds);

            // Test 4: Draw all cards
            UnityEngine.Debug.Log(">>> Test 4: Drawing all cards (requesting 100)");
            var allCards = targetDeck.DrawCard(100);
            testHand.AddRange(allCards);
            Debug.Assert(allCards.Count == 53, "Test 4 Failed: Should draw all 53 cards");
            Debug.Assert(targetDeck.CardCount == 0, "Test 4 Failed: Deck should be empty");
            Debug.Assert(targetDeck.IsEmtpy == true, "Test 4 Failed: IsEmpty should be true");
            UnityEngine.Debug.Log("✓ Test 4 Passed: Drew all cards, deck is empty");

            yield return new WaitForSeconds(testDelaySeconds);

            // Test 5: Try to draw from empty deck
            UnityEngine.Debug.Log(">>> Test 5: Drawing from empty deck");
            var emptyDraw = targetDeck.DrawCard(5);
            Debug.Assert(emptyDraw.Count == 0, "Test 5 Failed: Should not draw from empty deck");
            UnityEngine.Debug.Log("✓ Test 5 Passed: Cannot draw from empty deck");

            yield return new WaitForSeconds(testDelaySeconds);

            // Test 6: Return all and reset
            UnityEngine.Debug.Log(">>> Test 6: Returning all cards and resetting");
            targetDeck.ReturnCard(testHand);
            testHand.Clear();
            Debug.Assert(targetDeck.CardCount == 53, "Test 6 Failed: Should have all cards back");
            UnityEngine.Debug.Log("✓ Test 6 Passed: All cards returned");

            yield return new WaitForSeconds(testDelaySeconds);

            // Test 7: Partial return
            UnityEngine.Debug.Log(">>> Test 7: Partial draw and return");
            var partial = targetDeck.DrawCard(10);
            testHand.AddRange(partial);
            List<GameObject> toReturn = new List<GameObject>();
            for (int i = 0; i < 5; i++)
            {
                toReturn.Add(testHand[0]);
                testHand.RemoveAt(0);
            }
            targetDeck.ReturnCard(toReturn);
            Debug.Assert(testHand.Count == 5, "Test 7 Failed: Should have 5 cards in hand");
            Debug.Assert(targetDeck.CardCount == 48, "Test 7 Failed: Should have 48 cards in deck");
            UnityEngine.Debug.Log("✓ Test 7 Passed: Partial return works");

            // 정리
            targetDeck.ReturnCard(testHand);
            testHand.Clear();
            targetDeck.DeckReset();

            UnityEngine.Debug.Log("========== All Tests Passed! ==========");
        }

        #endregion

        #region Context Menu (Inspector에서 직접 실행)

        [ContextMenu("Quick Test: Draw 5")]
        private void ContextMenuDraw5()
        {
            TestDrawCards(5);
        }

        [ContextMenu("Quick Test: Return All")]
        private void ContextMenuReturnAll()
        {
            TestReturnCards();
        }

        [ContextMenu("Quick Test: Print Info")]
        private void ContextMenuPrintInfo()
        {
            PrintDeckInfo();
        }

        [ContextMenu("Run Auto Tests")]
        private void ContextMenuRunTests()
        {
            StartCoroutine(RunAutoTests());
        }

        #endregion
    }
}