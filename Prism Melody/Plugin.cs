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
        internal bool loadedPalettes = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            if (GameObject.FindObjectOfType<GameManager>() && sceneName == "CharacterSelect")
            {
                LoadPalettes();
            }
        }

        internal void LoadPalettes()
        {
            Idol[] idols = GameObject.FindObjectOfType<GameManager>().Characters;

            for (int i = 0; i < idols.Length; i++)
            {
                string charName = "";
                switch (i)
                {
                    case 0:
                        charName = "Aki";
                        break;
                    case 1:
                        charName = "Ayame";
                        break;
                    case 2:
                        charName = "Coco";
                        break;
                    case 3:
                        charName = "Fubuki";
                        break;
                    case 4:
                        charName = "Korone";
                        break;
                    case 5:
                        charName = "Sora";
                        break;
                    case 6:
                        charName = "Suisei";
                        break;
                    case 7:
                        charName = "Botan";
                        break;
                }

                Idol idol = idols[i];

                for (int j = 0; j < idol.paletteSwapMaterials.Count; j++)
                {
                    string filePath = Path.Combine(MelonHandler.PluginsDirectory, "Palettes", charName + j.ToString());

                    List<Vector4> inColors = new List<Vector4>();
                    List<Vector4> outColors = new List<Vector4>();

                    Material mat = idol.paletteSwapMaterials[j];

                    if (File.Exists(filePath + "in.txt"))
                    {
                        MelonLogger.Msg("Found an In Palette For Outfit " + j + " For " + charName);

                        StreamReader sr = new StreamReader(filePath + "in.txt");

                        string line;
                        using (sr)
                        {
                            do
                            {
                                line = sr.ReadLine();
                                if (line != null)
                                {
                                    string[] lineData = line.Split(';');

                                    inColors.Add(new Vector4(float.Parse(lineData[0]), float.Parse(lineData[1]), float.Parse(lineData[2]), float.Parse(lineData[3])));
                                }
                            } while (line != null);

                            sr.Close();
                        }

                        for (int k = 0; k < inColors.Count; k++)
                        {
                            mat.SetVector("inColour" + k, inColors[k]);
                        }
                    }

                    if (File.Exists(filePath + "out.txt"))
                    {
                        MelonLogger.Msg("Found an Out Palette For Outfit " + j + " For " + charName);

                        StreamReader sr = new StreamReader(filePath + "out.txt");

                        string line;
                        using (sr)
                        {
                            do
                            {
                                line = sr.ReadLine();
                                if (line != null)
                                {
                                    string[] lineData = line.Split(';');

                                    outColors.Add(new Vector4(float.Parse(lineData[0]), float.Parse(lineData[1]), float.Parse(lineData[2]), float.Parse(lineData[3])));
                                }
                            } while (line != null);

                            sr.Close();
                        }

                        for (int k = 0; k < outColors.Count; k++)
                        {
                            mat.SetVector("outColour" + k, outColors[k]);
                        }
                    }
                }
            }
        }
    }
}
