using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnderGroundPoker.Prefab.Card
{
    /// <summary>
    /// PlayerHand 평가 시스템 디버거
    /// 핸드 평가 과정을 단계별로 출력하고 검증합니다.
    /// </summary>
    public class PlayerHandDebugger : MonoBehaviour
    {
        #region Variables
        [Header("디버그 설정")]
        [SerializeField] private bool enableDebugLogs = true;          // 디버그 로그 활성화
        [SerializeField] private bool showDetailedAnalysis = true;     // 상세 분석 표시
        
        [Header("테스트 대상")]
        [SerializeField] private PlayerHand targetHand;                // 테스트할 핸드
        
        [Header("수동 테스트 입력")]
        [SerializeField] private List<CardTestData> manualTestCards = new List<CardTestData>();  // 수동 입력 카드
        
        private StringBuilder debugOutput;                             // 디버그 출력 버퍼
        #endregion

        #region Test Data Structure
        [System.Serializable]
        public struct CardTestData
        {
            public CardSuit suit;
            public CardRank rank;
            
            public override string ToString()
            {
                if (rank == CardRank.Joker) return "🃏 Joker";
                return $"{GetSuitSymbol(suit)}{GetRankSymbol(rank)}";
            }
            
            private string GetSuitSymbol(CardSuit s)
            {
                switch (s)
                {
                    case CardSuit.Spade: return "♠";
                    case CardSuit.Diamond: return "♦";
                    case CardSuit.Heart: return "♥";
                    case CardSuit.Club: return "♣";
                    default: return "";
                }
            }
            
            private string GetRankSymbol(CardRank r)
            {
                if ((int)r >= 2 && (int)r <= 10) return ((int)r).ToString();
                switch (r)
                {
                    case CardRank.Jack: return "J";
                    case CardRank.Queen: return "Q";
                    case CardRank.King: return "K";
                    case CardRank.Ace: return "A";
                    default: return "?";
                }
            }
        }
        #endregion

        #region Unity Methods
        void Start()
        {
            if (targetHand == null)
            {
                Debug.LogWarning("[PlayerHandDebugger] targetHand가 설정되지 않았습니다.");
            }
        }
        #endregion

        #region Public Methods - Manual Testing
        
        [ContextMenu("▶ 수동 입력 카드로 테스트 실행")]
        public void RunManualTest()
        {
            if (manualTestCards == null || manualTestCards.Count != 5)
            {
                Debug.LogError("[PlayerHandDebugger] 정확히 5장의 카드를 입력해주세요.");
                return;
            }
            
            debugOutput = new StringBuilder();
            debugOutput.AppendLine("═══════════════════════════════════════");
            debugOutput.AppendLine("     PlayerHand 수동 테스트 시작");
            debugOutput.AppendLine("═══════════════════════════════════════");
            debugOutput.AppendLine();
            
            // 입력 카드 표시
            debugOutput.AppendLine("📋 입력 카드:");
            for (int i = 0; i < manualTestCards.Count; i++)
            {
                debugOutput.AppendLine($"  [{i+1}] {manualTestCards[i]}");
            }
            debugOutput.AppendLine();
            
            // 테스트 핸드 생성
            CreateTestHand();
            
            // 평가 실행
            DebugEvaluateHand(targetHand);
            
            // 결과 출력
            Debug.Log(debugOutput.ToString());
        }
        
        [ContextMenu("▶ 프리셋 테스트 - 로얄 플러쉬")]
        public void TestRoyalFlush()
        {
            manualTestCards = new List<CardTestData>
            {
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.Ace },
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.King },
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.Queen },
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.Jack },
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.Ten }
            };
            RunManualTest();
        }
        
        [ContextMenu("▶ 프리셋 테스트 - 백스트레이트 (조커)")]
        public void TestBackStraightWithJoker()
        {
            manualTestCards = new List<CardTestData>
            {
                new CardTestData { suit = CardSuit.Heart, rank = CardRank.Ace },
                new CardTestData { suit = CardSuit.Diamond, rank = CardRank.Two },
                new CardTestData { suit = CardSuit.Club, rank = CardRank.Three },
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.Four },
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.Joker }
            };
            RunManualTest();
        }
        
        [ContextMenu("▶ 프리셋 테스트 - 풀 하우스")]
        public void TestFullHouse()
        {
            manualTestCards = new List<CardTestData>
            {
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.King },
                new CardTestData { suit = CardSuit.Heart, rank = CardRank.King },
                new CardTestData { suit = CardSuit.Diamond, rank = CardRank.King },
                new CardTestData { suit = CardSuit.Club, rank = CardRank.Eight },
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.Eight }
            };
            RunManualTest();
        }

        [ContextMenu("▶ 프리셋 테스트 - 투페어")]
        public void TestTwoPair()
        {
            manualTestCards = new List<CardTestData>
            {
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.King },
                new CardTestData { suit = CardSuit.Heart, rank = CardRank.King },
                new CardTestData { suit = CardSuit.Diamond, rank = CardRank.Eight },
                new CardTestData { suit = CardSuit.Club, rank = CardRank.Eight },
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.Ace }
            };
            RunManualTest();
        }
        
        #endregion

        #region Private Methods - Test Hand Creation
        
        private void CreateTestHand()
        {
            if (targetHand == null)
            {
                Debug.LogError("[PlayerHandDebugger] targetHand가 null입니다. Inspector에서 설정해주세요.");
                return;
            }

            // 기존 핸드 초기화
            targetHand.Hand.Clear();

            // 각 카드 데이터로 Card GameObject 생성
            foreach (var testCard in manualTestCards)
            {
                GameObject cardObj = new GameObject($"TestCard_{testCard.rank}_{testCard.suit}");
                Card card = cardObj.AddComponent<Card>();
                
                // 리플렉션으로 private 필드 설정 (임시 - 실제로는 Card에 생성자나 Initialize 메서드 필요)
                var suitField = typeof(Card).GetField("suit", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var rankField = typeof(Card).GetField("rank", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                suitField?.SetValue(card, testCard.suit);
                rankField?.SetValue(card, testCard.rank);
                
                targetHand.AddCard(card);
            }

            debugOutput.AppendLine("✅ 테스트 핸드 생성 완료");
            debugOutput.AppendLine();
        }
        
        #endregion

        #region Public Methods - Debug Evaluation
        
        public void DebugEvaluateHand(PlayerHand hand)
        {
            if (!enableDebugLogs) return;
            if (hand == null)
            {
                Debug.LogError("[PlayerHandDebugger] 평가할 핸드가 null입니다.");
                return;
            }
            
            debugOutput.AppendLine("───────────────────────────────────────");
            debugOutput.AppendLine("1️⃣  현재 핸드 상태");
            debugOutput.AppendLine("───────────────────────────────────────");
            
            PrintCurrentHand(hand);
            
            debugOutput.AppendLine();
            debugOutput.AppendLine("───────────────────────────────────────");
            debugOutput.AppendLine("2️⃣  핸드 평가 실행");
            debugOutput.AppendLine("───────────────────────────────────────");
            
            // 실제 평가 실행
            hand.EvaluateHand();
            
            debugOutput.AppendLine("  ✅ EvaluateHand() 호출 완료");
            debugOutput.AppendLine();
            
            debugOutput.AppendLine("───────────────────────────────────────");
            debugOutput.AppendLine("3️⃣  평가 결과");
            debugOutput.AppendLine("───────────────────────────────────────");
            
            PrintEvaluationResult(hand);
        }
        
        #endregion

        #region Private Methods - Debug Output
        
        private void PrintCurrentHand(PlayerHand hand)
        {
            debugOutput.AppendLine("  🎴 현재 카드:");
            
            if (hand.Hand == null || hand.Hand.Count == 0)
            {
                debugOutput.AppendLine("    ❌ 핸드가 비어있습니다.");
                return;
            }

            for (int i = 0; i < hand.Hand.Count; i++)
            {
                Card card = hand.Hand[i];
                string suitSymbol = GetSuitSymbol(card.Suit);
                string rankSymbol = GetRankSymbol(card.Rank);
                debugOutput.AppendLine($"    [{i+1}] {suitSymbol}{rankSymbol}");
            }

            bool hasJoker = hand.HasJoker;
            debugOutput.AppendLine($"\n  🃏 조커 보유: {(hasJoker ? "✅ 있음" : "❌ 없음")}");
        }
        
        private void PrintEvaluationResult(PlayerHand hand)
        {
            // PlayerHand에는 result 필드가 private이므로 리플렉션으로 접근
            var resultField = typeof(PlayerHand).GetField("result", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (resultField == null)
            {
                debugOutput.AppendLine("  ❌ result 필드를 찾을 수 없습니다.");
                return;
            }

            HandResult result = (HandResult)resultField.GetValue(hand);

            debugOutput.AppendLine($"  🏆 최종 족보: {GetHandRankName(result.Rank)}");
            
            if (result.TieBreaker != null && result.TieBreaker.Count > 0)
            {
                debugOutput.Append("  🎯 타이브레이커: [");
                for (int i = 0; i < result.TieBreaker.Count; i++)
                {
                    debugOutput.Append(GetRankSymbol(result.TieBreaker[i]));
                    if (i < result.TieBreaker.Count - 1)
                        debugOutput.Append(", ");
                }
                debugOutput.AppendLine("]");
            }
            else
            {
                debugOutput.AppendLine("  🎯 타이브레이커: 없음");
            }
        }
        
        #endregion

        #region Helper Methods
        
        private string GetSuitSymbol(CardSuit s)
        {
            switch (s)
            {
                case CardSuit.Spade: return "♠";
                case CardSuit.Diamond: return "♦";
                case CardSuit.Heart: return "♥";
                case CardSuit.Club: return "♣";
                default: return "?";
            }
        }
        
        private string GetRankSymbol(CardRank r)
        {
            if (r == CardRank.Joker) return "Joker";
            if ((int)r >= 2 && (int)r <= 10) return ((int)r).ToString();
            switch (r)
            {
                case CardRank.Jack: return "J";
                case CardRank.Queen: return "Q";
                case CardRank.King: return "K";
                case CardRank.Ace: return "A";
                default: return "?";
            }
        }

        private string GetHandRankName(HandRank rank)
        {
            switch (rank)
            {
                case HandRank.Top: return "하이 카드";
                case HandRank.OnePair: return "원 페어";
                case HandRank.TwoPair: return "투 페어";
                case HandRank.Triple: return "트리플";
                case HandRank.Straight: return "스트레이트";
                case HandRank.BackStraight: return "백 스트레이트";
                case HandRank.Mountain: return "마운틴";
                case HandRank.Flush: return "플러쉬";
                case HandRank.FullHouse: return "풀 하우스";
                case HandRank.FourCard: return "포 카드";
                case HandRank.StraightFlush: return "스트레이트 플러쉬";
                case HandRank.RoyalStraightFlush: return "로얄 스트레이트 플러쉬";
                case HandRank.FiveCard: return "파이브 카드";
                default: return "알 수 없음";
            }
        }
        
        #endregion

        #region Comparison Test
        
        [ContextMenu("▶ 두 핸드 비교 테스트 (현재 핸드 vs 새 핸드)")]
        public void CompareWithNewHand()
        {
            if (targetHand == null)
            {
                Debug.LogError("[PlayerHandDebugger] targetHand가 null입니다.");
                return;
            }

            // 새로운 테스트 핸드 생성
            GameObject newHandObj = new GameObject("TestHand2");
            PlayerHand hand2 = newHandObj.AddComponent<PlayerHand>();

            // 두 번째 핸드용 카드 (예시: 에이스 페어)
            List<CardTestData> hand2Cards = new List<CardTestData>
            {
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.Ace },
                new CardTestData { suit = CardSuit.Heart, rank = CardRank.Ace },
                new CardTestData { suit = CardSuit.Diamond, rank = CardRank.King },
                new CardTestData { suit = CardSuit.Club, rank = CardRank.Queen },
                new CardTestData { suit = CardSuit.Spade, rank = CardRank.Jack }
            };

            foreach (var testCard in hand2Cards)
            {
                GameObject cardObj = new GameObject($"Card_{testCard.rank}_{testCard.suit}");
                Card card = cardObj.AddComponent<Card>();
                
                var suitField = typeof(Card).GetField("suit", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var rankField = typeof(Card).GetField("rank", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                suitField?.SetValue(card, testCard.suit);
                rankField?.SetValue(card, testCard.rank);
                
                hand2.AddCard(card);
            }

            CompareHands(targetHand, hand2);

            // 정리
            Destroy(newHandObj);
        }

        public void CompareHands(PlayerHand hand1, PlayerHand hand2)
        {
            debugOutput = new StringBuilder();
            debugOutput.AppendLine("═══════════════════════════════════════");
            debugOutput.AppendLine("     두 핸드 비교 테스트");
            debugOutput.AppendLine("═══════════════════════════════════════");
            
            debugOutput.AppendLine("\n[핸드 1 평가]");
            hand1.EvaluateHand();
            PrintCurrentHand(hand1);
            PrintEvaluationResult(hand1);
            
            debugOutput.AppendLine("\n[핸드 2 평가]");
            hand2.EvaluateHand();
            PrintCurrentHand(hand2);
            PrintEvaluationResult(hand2);
            
            debugOutput.AppendLine("\n───────────────────────────────────────");
            debugOutput.AppendLine("⚔️  승부 판정");
            debugOutput.AppendLine("───────────────────────────────────────");
            
            // result 필드 가져오기
            var resultField = typeof(PlayerHand).GetField("result", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            HandResult result1 = (HandResult)resultField.GetValue(hand1);
            HandResult result2 = (HandResult)resultField.GetValue(hand2);
            
            int comparison = hand1.CompareTo(result1, result2);
            
            if (comparison > 0)
            {
                debugOutput.AppendLine("  🏆 핸드 1 승리!");
            }
            else if (comparison < 0)
            {
                debugOutput.AppendLine("  🏆 핸드 2 승리!");
            }
            else
            {
                debugOutput.AppendLine("  🤝 무승부!");
            }
            
            Debug.Log(debugOutput.ToString());
        }
        
        #endregion
    }
}