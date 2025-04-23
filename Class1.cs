using BepInEx;
using HarmonyLib;
using UnityEngine;
using MTM101BaldAPI;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace BBPlusFrench
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class BBPFrench : BaseUnityPlugin
    {
        public const string pluginGuid = "maxou555.bbplus.french";
        public const string pluginName = "BBP: Traduction Francaise";
        public const string pluginVersion = "1.0.5.0";

        public void Awake()
        {
            Logger.LogInfo("=========================================================================");
            Logger.LogInfo("========== BBP: Traduction Francaise a été chargé avec succès. ==========");
            Logger.LogInfo("==========                   Version 1.0.5.0                   ==========");
            Logger.LogInfo("==========               Compatible pour : 0.10.0              ==========");
            Logger.LogInfo("=========================================================================");

            Harmony harmony = new Harmony(pluginGuid);


            MethodInfo originalReadMe = AccessTools.Method(typeof(Readme), "Readme");
            MethodInfo patchReadMe = AccessTools.Method(typeof(MyPatches), "ReadMe_Patch");
            harmony.Patch(originalReadMe, postfix: new HarmonyMethod(patchReadMe));

            
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
            Debug.Log("[FR] StartScreen_Patch applied");

            if (__instance.textBox != null)
            {
                __instance.textBox.text = "<color=red><b>ATTENTION !</b></color>\n<size=22>Baldi's Basics Plus est un 'léger' <color=red>jeu d'horreur</color> et n'est pas un logiciel éducatif.\n\nMême si les éléments d'horreur sont assez légers à ce point du développement du jeu, il y a toujours quelques jumpsacres et autres éléments pouvant effrayer certains joueurs.</size>\n\nAPPUIE SUR N'IMPORTE QUELLE TOUCHE POUR CONTINUER";
            }
            else
            {
                Debug.Log("[FR] StartScreen_Patch TextBox not found");
            }
        }
        // ================================================

        // ================ README TESTING ================
        public static void ReadMe_Patch(Readme __instance)
        {
            Debug.Log("ReadMe_Patch applied");

            var textField = typeof(Readme).GetField("text", BindingFlags.NonPublic | BindingFlags.Instance);
            if (textField != null)
            {
                Debug.Log("ReadMe text found, changing text");
                textField.SetValue(__instance, "Test, le README a été modifié !");
            }
            else
            {
                Debug.LogWarning("ReadMe text not found");
            }

            var headingField = typeof(Readme).GetField("heading", BindingFlags.NonPublic | BindingFlags.Instance);
            if (headingField != null)
            {
                Debug.Log("heading text found, changing text");
                headingField.SetValue(__instance, "Test numéro 2, heading modifié !");
            }
            else
            {
                Debug.LogWarning("heading text not found");
            }
        }
        // ================================================

        // ================== ENDLESS UI ==================
        public static void EndlessGameManagerRestartLevel_Patch(EndlessGameManager __instance)
        {
            Debug.Log("[FR] EndlessGameManagerRestartLevel_Patch patched");

            TMP_Text[] allTextComponents = GameObject.FindObjectsOfType<TMP_Text>(true);

            foreach (var textComponent in allTextComponents)
            {
                if (textComponent.text.Contains("Final Score:"))
                {
                    textComponent.text = "Score final : " + __instance.FoundNotebooks.ToString();
                }
            }

            var scoreTextField = typeof(EndlessGameManager).GetField("scoreText", BindingFlags.NonPublic | BindingFlags.Instance);
            if (scoreTextField != null)
            {
                var scoreText = scoreTextField.GetValue(__instance) as TMP_Text;
                if (scoreText != null)
                {
                    scoreText.text = "";
                }
                else
                {
                    Debug.LogError("[FR] scoreText is null.");
                }
            }
            else
            {
                Debug.LogError("[FR] scoreText not found!");
            }

            var endlessLevelField = typeof(EndlessGameManager).GetField("endlessLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            if (endlessLevelField != null)
            {
                var endlessLevel = endlessLevelField.GetValue(__instance);

                var congratsTextField = typeof(EndlessGameManager).GetField("congratsText", BindingFlags.NonPublic | BindingFlags.Instance);
                if (congratsTextField != null)
                {
                    var congratsText = congratsTextField.GetValue(__instance) as TMP_Text;
                    if (congratsText != null)
                    {
                        string rankTextValue = "";
                        var rankTextField = typeof(EndlessGameManager).GetField("rankText", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (rankTextField != null)
                        {
                            var rankText = rankTextField.GetValue(__instance) as TMP_Text;
                            if (rankText != null)
                            {
                                rankTextValue = rankText.text;
                            }
                        }
                        else
                        {
                            Debug.LogError("[FR] rankText field not found!");
                        }
                        congratsText.text = "Tu as établi un nouveau meilleur score !\n\nRang actuel : " + rankTextValue;
                    }
                }
                else
                {
                    Debug.LogError("[FR] congratsText field not found!");
                }

                var rankTextFieldToHide = typeof(EndlessGameManager).GetField("rankText", BindingFlags.NonPublic | BindingFlags.Instance);
                if (rankTextFieldToHide != null)
                {
                    var rankTextToHide = rankTextFieldToHide.GetValue(__instance) as TMP_Text;
                    if (rankTextToHide != null)
                    {
                        //rankTextToHide.text = "";
                        rankTextToHide.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.LogError("[FR] rankText is null.");
                    }
                }
                else
                {
                    Debug.LogError("[FR] rankText not found!");
                }
            }
            else
            {
                Debug.LogError("[FR] endlessLevel not found!");
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