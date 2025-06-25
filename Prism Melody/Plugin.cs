using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdolShowdown;
using IdolShowdown.Managers;
using IdolShowdown.Structs;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using static Prism_Melody.CosplayNameUpdater;
using UnityEngine.SceneManagement;

namespace Prism_Melody
{
    public class Plugin : MelonMod
    {
        internal bool loadedPalettes = false;

        internal AnimationClip[] cssIdle = new AnimationClip[8];

        public static new HarmonyLib.Harmony Harmony { get; private set; }

        public List<CosplayNameUpdater.CosplayList> cosplayLists = new List<CosplayNameUpdater.CosplayList>();
        public CosplayNameUpdater cnu;

        public bool palletViewer = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (GlobalManager.Instance && GlobalManager.Instance.GameManager && sceneName == "MainMenu" && !loadedPalettes)
            {
                LoadPalettes();

                Harmony = new HarmonyLib.Harmony("Prism Melody");
                Patch.Init();

                //CharacterTest.LoadTestCharacter();
                //CharacterTest.SetCSSIdles();
            }
            if (sceneName == "CharacterSelect")
            {
                cnu = GameObject.Instantiate(new GameObject()).AddComponent<CosplayNameUpdater>();
                cnu.cosplayList = cosplayLists;
                SceneManager.MoveGameObjectToScene(cnu.gameObject, SceneManager.GetSceneByBuildIndex(buildIndex));
                GameObject[] go = SceneManager.GetSceneByBuildIndex(buildIndex).GetRootGameObjects();
                foreach (var goItem in go)
                {
                    if (goItem.GetComponent<CosplayNameUpdater>() != null)
                    {
                        cnu = goItem.GetComponent<CosplayNameUpdater>();
                        break;
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (SceneManager.GetSceneByName("MainMenu").isLoaded)
                {
                    palletViewer = true;

                    GlobalManager.Instance.SceneManager.SwitchSceneToAsyncWithFadeSpecific("HoloStage", "MainMenu", 1, false, false);

                }
            }
        }

        internal void LoadPalettes()
        {
            string[] allPaletteDirectories = Directory.GetFiles(Path.Combine(MelonHandler.PluginsDirectory, "Palettes"), "mod.json", SearchOption.AllDirectories);

            foreach (Idol idol in GlobalManager.Instance.GameManager.Characters)
            {
                List<string> cCharIn = new List<string>();
                List<string> cCharOut = new List<string>();

                List<string> cosplays = new List<string>();

                foreach (string cChar in allPaletteDirectories)
                {
                    MetadataBase mb = MetadataBase.Load(cChar);

                    if (mb.Character == idol.charName)
                    {
                        Material mat = new Material(idol.paletteSwapMaterials[mb.Outfit]);

                        string line;
                        int k = 0;
                        using (StreamReader sr = new StreamReader(Path.Combine(mb.Location, mb.Character + mb.Outfit + "in.txt")))
                        {
                            do
                            {
                                line = sr.ReadLine();
                                if (!string.IsNullOrEmpty(line))
                                {
                                    string[] lineData = line.Split(';');
                                    if (lineData.Length < 4) continue; // Check for bad lines

                                    mat.SetVector("inColour" + k, new Vector4(float.Parse(lineData[0]), float.Parse(lineData[1]), float.Parse(lineData[2]), float.Parse(lineData[3])));
                                    k++;
                                }
                            } while (!string.IsNullOrEmpty(line));

                            sr.Close();
                        }

                        k = 0;
                        using (StreamReader sr = new StreamReader(Path.Combine(mb.Location, mb.Character + mb.Outfit + "out.txt")))
                        {
                            do
                            {
                                line = sr.ReadLine();
                                if (!string.IsNullOrEmpty(line))
                                {
                                    string[] lineData = line.Split(';');
                                    if (lineData.Length < 4) continue; // Check for bad lines

                                    mat.SetVector("outColour" + k, new Vector4(float.Parse(lineData[0]), float.Parse(lineData[1]), float.Parse(lineData[2]), float.Parse(lineData[3])));
                                    k++;
                                }
                            } while (!string.IsNullOrEmpty(line));

                            sr.Close();
                        }

                        cosplays.Add(mb.Title + "\r\n<color=#00ddc0>" + mb.Author);

                        idol.paletteSwapMaterials.Add(mat);

                        CosplayData cd = new CosplayData(mat, true);
                        GlobalManager.Instance.SinglePlayerManager.SaveData.SetCosplayUnlocked(idol, true, idol.paletteSwapMaterials.Count - 1);
                    }
                }

                AddCharCosplay(idol.charName, cosplays);
            }

            loadedPalettes = true;
        }

        public void AddCharCosplay(string idol, List<string> cosplays)
        {
            CosplayList cosplay = new CosplayList();
            cosplay.idol = idol;
            cosplay.cosplayName = cosplays;
            cosplayLists.Add(cosplay);
        }
    }
}
