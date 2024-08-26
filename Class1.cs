using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Reflection;



namespace BBPlusFrench
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class BBPFrench : BaseUnityPlugin
    {
        public const string pluginGuid = "maxou555.bbplus.french";
        public const string pluginName = "BBP: Traduction Francaise";
        public const string pluginVersion = "1.0.1.0";

        public void Awake()
        {
            Logger.LogInfo("=========================================================================");
            Logger.LogInfo("========== BBP: Traduction Francaise a été chargé avec succès. ==========");
            Logger.LogInfo("==========                   Version 1.0.1.0                   ==========");
            Logger.LogInfo("==========               Compatible pour : 0.6.0               ==========");
            Logger.LogInfo("=========================================================================");

            Harmony harmony = new Harmony(pluginGuid);

            MethodInfo original = AccessTools.Method(typeof(DetentionUi), "Initialize");
            MethodInfo patch = AccessTools.Method(typeof(MyPatches), "InitializeDetentionUI_MyPatch");

            MethodInfo originalInitialize = AccessTools.Method(typeof(ElevatorScreen), "Initialize");
            MethodInfo patchInitialize = AccessTools.Method(typeof(MyPatches), "InitializeElevatorScreen_Patch");
            harmony.Patch(originalInitialize, postfix: new HarmonyMethod(patchInitialize));

            MethodInfo originalWarningScreen = AccessTools.Method(typeof(WarningScreen), "Start");
            MethodInfo patchWarningScreen = AccessTools.Method(typeof(MyPatches), "StartScreen_Patch");
            harmony.Patch(originalWarningScreen, postfix: new HarmonyMethod(patchWarningScreen));

            MethodInfo originalEndlessGameManager = AccessTools.Method(typeof(EndlessGameManager), "RestartLevel");
            MethodInfo patchEndlessGameManager = AccessTools.Method(typeof(MyPatches), "EndlessGameManagerRestartLevel_Patch");
            harmony.Patch(originalEndlessGameManager, postfix: new HarmonyMethod(patchEndlessGameManager));

            harmony.PatchAll();
        }
    }

    public class MyPatches
    {
        // ================ WARNING SCREEN ================
        public static void StartScreen_Patch(WarningScreen __instance)
        {
            Debug.Log("StartScreen_Patch applied");

            if (__instance.textBox != null)
            {
                Debug.Log("TextBox found, changing text");
                __instance.textBox.text = "<color=red><b>ATTENTION !</b></color>\n<size=22>Baldi's Basics Plus est un 'léger' <color=red>jeu d'horreur</color> et n'est pas un logiciel éducatif.\n\nMême si les éléments d'horreur sont assez légers à ce point du développement du jeu, il y a toujours quelques jumpsacres et autres éléments pouvant effrayer certains joueurs.</size>\n\nAPPUIE SUR N'IMPORTE QUELLE TOUCHE POUR CONTINUER";
            }
            else
            {
                Debug.Log("TextBox not found");
            }
        }
        // ================================================

        /* ================= DETENTION UI =================
        public static void InitializeDetentionUI_MyPatch(DetentionUi __instance, Camera cam, float time, EnvironmentController ec)
        {
            TMP_Text textComponent = __instance.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = "Détention pour vous !\n  secondes restantes !";
            }
        }
        // ================================================ */

        // ================== ENDLESS UI ==================

        public static void EndlessGameManagerRestartLevel_Patch(EndlessGameManager __instance)
        {
            Debug.Log("EndlessGameManagerRestartLevel_Patch applied");

            TMP_Text[] allTextComponents = GameObject.FindObjectsOfType<TMP_Text>(true);

            foreach (var textComponent in allTextComponents)
            {
                if (textComponent.text.Contains("Final Score:"))
                {
                    textComponent.text = "Score final : " + __instance.FoundNotebooks.ToString();
                    Debug.Log("Text modified: " + textComponent.text);
                }
            }

            var scoreTextField = typeof(EndlessGameManager).GetField("scoreText", BindingFlags.NonPublic | BindingFlags.Instance);
            if (scoreTextField != null)
            {
                var scoreText = scoreTextField.GetValue(__instance) as TMP_Text;
                if (scoreText != null)
                {
                    Debug.Log("scoreText found, clearing text");
                    scoreText.text = "";
                }
                else
                {
                    Debug.LogError("scoreText is null.");
                }
            }
            else
            {
                Debug.LogError("scoreText not found!");
            }

            var endlessLevelField = typeof(EndlessGameManager).GetField("endlessLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            if (endlessLevelField != null)
            {
                var endlessLevel = endlessLevelField.GetValue(__instance);
                Debug.Log("endlessLevel found");

                var congratsTextField = typeof(EndlessGameManager).GetField("congratsText", BindingFlags.NonPublic | BindingFlags.Instance);
                if (congratsTextField != null)
                {
                    var congratsText = congratsTextField.GetValue(__instance) as TMP_Text;
                    if (congratsText != null)
                    {
                        Debug.Log("congratsText found, changing text");

                        string rankTextValue = "";
                        var rankTextField = typeof(EndlessGameManager).GetField("rankText", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (rankTextField != null)
                        {
                            var rankText = rankTextField.GetValue(__instance) as TMP_Text;
                            if (rankText != null)
                            {
                                rankTextValue = rankText.text;
                                Debug.Log("rankText found, value: " + rankTextValue);
                            }
                            else
                            {
                                Debug.LogError("rankText is null.");
                            }
                        }
                        else
                        {
                            Debug.LogError("rankText field not found!");
                        }

                        congratsText.text = "Tu as établi un nouveau meilleur score !\n\nRang actuel : " + rankTextValue;
                    }
                    else
                    {
                        Debug.LogError("congratsText is null.");
                    }
                }
                else
                {
                    Debug.LogError("congratsText field not found!");
                }

                var rankTextFieldToHide = typeof(EndlessGameManager).GetField("rankText", BindingFlags.NonPublic | BindingFlags.Instance);
                if (rankTextFieldToHide != null)
                {
                    var rankTextToHide = rankTextFieldToHide.GetValue(__instance) as TMP_Text;
                    if (rankTextToHide != null)
                    {
                        Debug.Log("rankText found, hiding text");

                        //rankTextToHide.text = "";
                        rankTextToHide.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.LogError("rankText is null.");
                    }
                }
                else
                {
                    Debug.LogError("rankText not found!");
                }
            }
            else
            {
                Debug.LogError("endlessLevel not found!");
            }
        }

        // ================================================

        // ================ ELEVATOR SCREEN ===============
        public static void InitializeElevatorScreen_Patch(ElevatorScreen __instance)
        {
            var bigScreenField = typeof(ElevatorScreen).GetField("bigScreen", BindingFlags.NonPublic | BindingFlags.Instance);
            if (bigScreenField != null)
            {
                var bigScreen = bigScreenField.GetValue(__instance);
                if (bigScreen != null)
                {
                    var resultsTextField = bigScreen.GetType().GetField("resultsText", BindingFlags.Public | BindingFlags.Instance);
                    var resultsText = resultsTextField?.GetValue(bigScreen) as GameObject;
                    if (resultsText != null)
                    {
                        TMP_Text resultsTMPText = resultsText.GetComponent<TMP_Text>();
                        if (resultsTMPText != null)
                        {
                            resultsTMPText.text = "Résultats";
                            RectTransform resultsRectTransform = resultsText.GetComponent<RectTransform>();
                            if (resultsRectTransform != null)
                            {
                                resultsRectTransform.localPosition = new Vector3(-55f, 105f, 0f);
                                resultsRectTransform.sizeDelta = new Vector2(125f, -33f);
                            }
                        }
                    }

                    var timeTextField = bigScreen.GetType().GetField("timeText", BindingFlags.Public | BindingFlags.Instance);
                    var timeText = timeTextField?.GetValue(bigScreen) as GameObject;
                    if (timeText != null)
                    {
                        TMP_Text timeTMPText = timeText.GetComponent<TMP_Text>();
                        if (timeTMPText != null)
                        {
                            timeTMPText.text = "Temps :";
                        }
                    }

                    var gradeTextField = bigScreen.GetType().GetField("gradeText", BindingFlags.Public | BindingFlags.Instance);
                    var gradeText = gradeTextField?.GetValue(bigScreen) as GameObject;
                    if (gradeText != null)
                    {
                        TMP_Text gradeTMPText = gradeText.GetComponent<TMP_Text>();
                        if (gradeTMPText != null)
                        {
                            gradeTMPText.text = "Note Actuelle :";
                        }
                    }

                    var pointsTextField = bigScreen.GetType().GetField("pointsText", BindingFlags.Public | BindingFlags.Instance);
                    var pointsText = pointsTextField?.GetValue(bigScreen) as GameObject;
                    if (pointsText != null)
                    {
                        TMP_Text pointsTMPText = pointsText.GetComponent<TMP_Text>();
                        if (pointsTMPText != null)
                        {
                            pointsTMPText.text = "YTPs Gagnés :";
                        }
                    }

                    var totalTextField = bigScreen.GetType().GetField("totalText", BindingFlags.Public | BindingFlags.Instance);
                    var totalText = totalTextField?.GetValue(bigScreen) as GameObject;
                    if (totalText != null)
                    {
                        TMP_Text totalTMPText = totalText.GetComponent<TMP_Text>();
                        if (totalTMPText != null)
                        {
                            totalTMPText.text = "YTPs Totaux :";
                        }
                    }

                    var gradeBonusTextField = bigScreen.GetType().GetField("gradeBonusText", BindingFlags.Public | BindingFlags.Instance);
                    var gradeBonusText = gradeBonusTextField?.GetValue(bigScreen) as GameObject;
                    if (gradeBonusText != null)
                    {
                        TMP_Text gradeBonusTMPText = gradeBonusText.GetComponent<TMP_Text>();
                        if (gradeBonusTMPText != null)
                        {
                            gradeBonusTMPText.text = "Bonus de Note !";
                        }
                    }

                    var timeBonusTextField = bigScreen.GetType().GetField("timeBonusText", BindingFlags.Public | BindingFlags.Instance);
                    var timeBonusText = timeBonusTextField?.GetValue(bigScreen) as GameObject;
                    if (timeBonusText != null)
                    {
                        TMP_Text timeBonusTMPText = timeBonusText.GetComponent<TMP_Text>();
                        if (timeBonusTMPText != null)
                        {
                            timeBonusTMPText.text = "Temps Bonus !";
                        }
                    }

                    var multiplierTextField = bigScreen.GetType().GetField("multiplierText", BindingFlags.Public | BindingFlags.Instance);
                    var multiplierText = multiplierTextField?.GetValue(bigScreen) as GameObject;
                    if (multiplierText != null)
                    {
                        TMP_Text multiplierTMPText = multiplierText.GetComponent<TMP_Text>();
                        if (multiplierTMPText != null)
                        {
                            multiplierTMPText.text = "Bonus de vies :";
                        }
                    }
                }
            }
        }
        // ================================================
    }
}