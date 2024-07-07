using HarmonyLib;
using MGSC;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using System.IO;
using System;

using TinyJson;
namespace QM_DisplayMovementSpeed
{
    public class Plugin
    {
        public static KeyCode toggleKey = KeyCode.Comma;
        public static bool show = true;
        public static List<Monster> monsters = new List<Monster>();
        [Hook(ModHookType.AfterBootstrap)]
        public static void Bootstrap(IModContext context)
        {

            string configPath = Path.Combine(Application.persistentDataPath, Assembly.GetExecutingAssembly().GetName().Name) + ".json";
            Debug.Log(configPath);

            // thanks NBK_redspy, i just looked at your code because i had no idea how to do this
            if (File.Exists(configPath))
            {
                try
                {
                    string fileJson = File.ReadAllText(configPath);
                    Dictionary<string, string> values = fileJson.FromJson<Dictionary<string, string>>();
                    toggleKey = (KeyCode)Enum.Parse(typeof(KeyCode), values["toggleKey"]);
                }
                catch (Exception ex)
                {
                    Debug.Log("DisplayMovementSpeed: Error reading config file");
                    Debug.LogException(ex);
                }
            }
            else

            {
                try
                {
                    var text = "{\"toggleKey\":\"Comma\"}";
                    File.WriteAllText(configPath, text);
                }
                catch (Exception ex)
                {
                    Debug.Log("DisplayMovementSpeed: Error writing to config");
                    Debug.LogException(ex);
                }
            }

            // Plugin startup logic
            Debug.Log("Display enemy movement speed is loaded! :)");
            var harmony = new Harmony("QM_DisplayMovementSpeed");
            harmony.PatchAll();
        }

        [Hook(ModHookType.DungeonUpdateBeforeGameLoop)]
        public static void DungeonUpdateBeforeGameLoop(IModContext context)
        {
            if (InputHelper.GetKeyDown(toggleKey)) 
            {
                Debug.Log("toggling speed display");
                show = !show;
            }
            
        }

        public static void createText(Monster __instance)
        {
            Plugin.monsters.Add(__instance);
            GameObject monsterGameObject = __instance.CreatureView.gameObject;
            GameObject textGameObject = new GameObject("movementSpeedText");

            textGameObject.transform.SetParent(monsterGameObject.transform);
            textGameObject.transform.localPosition = new Vector3(0.1f, 0.1f, 0);

            textGameObject.AddComponent(typeof(TextMeshPro));

            TextMeshPro text = textGameObject.GetComponent<TextMeshPro>();
            text.text = (__instance.ActionPointsLeft + __instance.ActionPointsProcessed) + "";
            text.fontSize = 1f;
            text.fontStyle = FontStyles.Bold;
            text.lineSpacing = 1;
            text.alignment = TMPro.TextAlignmentOptions.Center;
            text.color = Color.white;
            text.outlineColor = Color.black;
            text.outlineWidth = 0.3f;


            HideTextMesh hider = __instance.gameObject.AddComponent<HideTextMesh>();
            hider.monster = __instance;
        }

    }

    [HarmonyPatch(typeof(Monster), nameof(Monster.OnAfterLoad))]
    public static class Patch_OnMonsterLoad
    {
        public static void Postfix(Monster __instance)
        {
            Plugin.createText(__instance);
        }
    }

    [HarmonyPatch(typeof(Monster), nameof(Monster.Mutate))]
    public static class Patch_OnMutate
    {
        public static void Postfix(Monster __instance)
        {
            Plugin.createText(__instance);
        }
    }

    [HarmonyPatch(typeof(Monster), nameof(Monster.Initialize))]
    public static class Path_OnInit
    {
        public static void Postfix(Monster __instance)
        {
            Plugin.createText(__instance);
        }
    }
}
