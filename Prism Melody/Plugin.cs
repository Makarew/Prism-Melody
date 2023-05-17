using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdolShowdown.Managers;
using IdolShowdown.Structs;
using MelonLoader;
using UnityEngine;

namespace Prism_Melody
{
    public class Plugin : MelonMod
    {
        public override void OnApplicationStart()
        {
            HarmonyInstance.Patch(typeof(GameManager).GetMethod("Start",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic),null,
                new HarmonyLib.HarmonyMethod(GetType().GetMethod(nameof(OnGameManagerLoaded),System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic)));
        }
        private static void OnGameManagerLoaded(){
            LoadPalettes();
        }

        internal static void LoadPalettes()
        {
            foreach(Idol idol in GlobalManager.Instance.GameManager.Characters)
            {
                for (int j = 0; j < idol.paletteSwapMaterials.Count; j++)
                {
                    string filePath = Path.Combine(MelonHandler.PluginsDirectory, "Palettes", idol.charName + j);

                    List<Vector4> inColors = new List<Vector4>();
                    List<Vector4> outColors = new List<Vector4>();

                    Material mat = idol.paletteSwapMaterials[j];

                    if (File.Exists(filePath + "in.txt"))
                    {
                        MelonLogger.Msg("Found an In Palette For Outfit " + j + " For " + idol.charName);

                        string line;
                        int k = 0;
                        using (StreamReader sr = new StreamReader(filePath + "in.txt"))
                        {
                            do
                            {
                                line = sr.ReadLine();
                                if (!string.IsNullOrEmpty(line))
                                {
                                    string[] lineData = line.Split(';');
                                    if(lineData.Length<4) continue; // Check for bad lines

                                    mat.SetVector("inColour" + k++, new Vector4(float.Parse(lineData[0]), float.Parse(lineData[1]), float.Parse(lineData[2]), float.Parse(lineData[3])));
                                }
                            } while (!string.IsNullOrEmpty(line));

                            sr.Close();
                        }
                    }

                    if (File.Exists(filePath + "out.txt"))
                    {
                        MelonLogger.Msg("Found an Out Palette For Outfit " + j + " For " + idol.charName);

                        string line;
                        int k = 0;
                        using (StreamReader sr = new StreamReader(filePath + "out.txt"))
                        {
                            do
                            {
                                line = sr.ReadLine();
                                if (!string.IsNullOrEmpty(line))
                                {
                                    string[] lineData = line.Split(';');
                                    if(lineData.Length<4) continue; // Check for bad lines

                                    mat.SetVector("outColour" + k++, new Vector4(float.Parse(lineData[0]), float.Parse(lineData[1]), float.Parse(lineData[2]), float.Parse(lineData[3])));
                                }
                            } while (!string.IsNullOrEmpty(line));

                            sr.Close();
                        }
                    }
                }
            }
        }
    }
}
