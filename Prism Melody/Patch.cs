using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using IdolShowdown;
using System.Reflection;
using IdolShowdown.Managers;
using UnityEngine;
using MelonLoader;
using UnityEngine.SceneManagement;

namespace Prism_Melody
{
    internal class Patch
    {
        public static void Init()
        {
            MethodInfo method = typeof(LobbyHelper).GetMethod("PackPlayerInformation", BindingFlags.Instance | BindingFlags.NonPublic);
            HarmonyMethod harmonyPre = new HarmonyMethod(typeof(Patch).GetMethod("PackPlayerInformation"));
            HarmonyMethod harmonyPost = new HarmonyMethod(typeof(Patch).GetMethod("PackPlayerInformationPost"));
            Plugin.Harmony.Patch(method, harmonyPre, harmonyPost);

            MethodInfo m2 = typeof(CSSDataUpdater).GetMethod("SelectCharacter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            HarmonyMethod hPost2 = new HarmonyMethod(typeof(Patch).GetMethod("SetSelectedCharacterDataPost"));
            Plugin.Harmony.Patch(m2, null, hPost2);

            MethodInfo m3 = typeof(StageProperties).GetMethod("LogicOnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            HarmonyMethod hPre3 = new HarmonyMethod(typeof(Patch).GetMethod("LogicOnStart"));
            Plugin.Harmony.Patch(m3, hPre3);
        }

        public static void PackPlayerInformation(out int __state)
        {
            OnlineMatchInfo omi = GlobalManager.Instance.OnlineComponents.matchInfo;

            if (omi.IsPlayerOne)
            {
                __state = omi.InfoForVSScreen.p1Pallet;
                if (omi.InfoForVSScreen.p1Pallet > 10)
                    omi.InfoForVSScreen.p1Pallet = 0;
            } else
            {
                __state = omi.InfoForVSScreen.p2Pallet;
                if (omi.InfoForVSScreen.p2Pallet > 10)
                    omi.InfoForVSScreen.p2Pallet = 0;
            }

            GlobalManager.Instance.OnlineComponents.matchInfo = omi;
        }

        public static void PackPlayerInformationPost(int __state)
        {
            OnlineMatchInfo omi = GlobalManager.Instance.OnlineComponents.matchInfo;

            if (omi.IsPlayerOne)
            {
                omi.InfoForVSScreen.p1Pallet = __state;
            }
            else
            {
                omi.InfoForVSScreen.p2Pallet = __state;
            }

            GlobalManager.Instance.OnlineComponents.matchInfo = omi;
        }

        public static void SetSelectedCharacterDataPost()
        {
            Melon<Plugin>.Instance.cnu.StateUpdate();
        }

        public static bool LogicOnStart()
        {
            if (Melon<Plugin>.Instance.palletViewer)
            {
                GameManager gm = GlobalManager.Instance.GameManager;

                GameObject[] rootObj = SceneManager.GetSceneByName("HoloStage").GetRootGameObjects();
                bool exists = false;

                foreach (GameObject obj in rootObj)
                {
                    if (obj.name == "Pallet Viewer")
                        exists = true;
                }

                if (exists)
                {
                    GameObject.Destroy(GameObject.Find("player1"));
                    GameObject.Destroy(GameObject.Find("player2"));
                    GameObject[] familiars = SceneManager.GetSceneByName("Global").GetRootGameObjects();

                    foreach (GameObject go in familiars)
                    {
                        if (go.name.StartsWith("Familiar"))
                        {
                            GameObject.Destroy(go);
                        }
                    }

                    gm.SetPlayer(gm.Characters[GameObject.FindObjectOfType<PalletViewer>().charID], PlayerNumber.playerOne);
                    gm.SetPlayer(gm.Characters[GameObject.FindObjectOfType<PalletViewer>().charID], PlayerNumber.playerTwo);
                } else
                {
                    gm.SetPlayer(gm.Characters[0], PlayerNumber.playerOne);
                    gm.SetPlayer(gm.Characters[0], PlayerNumber.playerTwo);
                }
                

                GlobalManager.Instance.GameStateManager.CharacterManager.SpawnCharacters();

                Melon<Plugin>.Instance.palletViewer = false;



                if (!exists)
                {
                    GameObject previewCam = GameObject.Instantiate(new GameObject());
                    previewCam.name = "Preview Cam";
                    previewCam.AddComponent<Camera>().depth = 1;
                    previewCam.GetComponent<Camera>().backgroundColor = Color.black;
                    SceneManager.MoveGameObjectToScene(previewCam, SceneManager.GetSceneByName("HoloStage"));
                    previewCam.transform.position = new Vector3(0, 50, -1.7f);

                    GameObject.FindObjectOfType<StageIntroController>().EndIntro();

                    foreach (var item in rootObj)
                    {
                        if (item.name == "Scene" || item.name == "Music + sound bank" || item.name == "StageAmbience" || item.name == "StageIntroController (1)")
                            item.SetActive(false);
                    }

                    GameObject pv = GameObject.Instantiate(new GameObject());
                    pv.name = "Pallet Viewer";
                    SceneManager.MoveGameObjectToScene(pv, SceneManager.GetSceneByName("HoloStage"));
                    pv.AddComponent<PalletViewer>();
                }

                return false;
            }
            return true;
        }
    }
}
