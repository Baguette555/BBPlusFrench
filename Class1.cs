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

        // ================= DETENTION UI =================
        public static void InitializeDetentionUI_MyPatch(DetentionUi __instance, Camera cam, float time, EnvironmentController ec)
        {
            TMP_Text textComponent = __instance.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = "Détention pour vous !\n  secondes restantes !";
            }
        }
        // ================================================

        // ================ ELEVATOR SCREEN ===============
        public static void InitializeElevatorScreen_Patch(ElevatorScreen __instance)
        {
            var saveButtonField = typeof(ElevatorScreen).GetField("saveButton", BindingFlags.NonPublic | BindingFlags.Instance);
            if (saveButtonField != null)
            {
                var saveButton = saveButtonField.GetValue(__instance) as GameObject;
                if (saveButton != null)
                {
                    TMP_Text saveButtonText = saveButton.transform.GetChild(0).GetComponent<TMP_Text>();
                    if (saveButtonText != null)
                    {
                        saveButtonText.text = "Sauv. et quitter";
                    }
                }
            }

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