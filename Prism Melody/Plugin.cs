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
            if (GlobalManager.Instance && GlobalManager.Instance.GameManager && sceneName == "CharacterSelect")
            {
                LoadPalettes();
            }
        }

        internal void LoadPalettes()
        {
            string[] allPaletteDirectories = Directory.GetFiles(Path.Combine(MelonHandler.PluginsDirectory, "Palettes"), "mod.json", SearchOption.AllDirectories);

            foreach (Idol idol in GlobalManager.Instance.GameManager.Characters)
            {
                List<string> cCharIn = new List<string>();
                List<string> cCharOut = new List<string>();

                foreach (string cChar in allPaletteDirectories)
                {
                    MetadataBase mb = MetadataBase.Load(cChar);

                    if (mb.Character == idol.charName)
                    {
                        Material mat = idol.paletteSwapMaterials[mb.Outfit];

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

                                    mat.SetVector("inColour" + k++, new Vector4(float.Parse(lineData[0]), float.Parse(lineData[1]), float.Parse(lineData[2]), float.Parse(lineData[3])));
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
