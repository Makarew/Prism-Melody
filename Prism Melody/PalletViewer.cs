using IdolShowdown.Managers;
using IdolShowdown.Structs;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.IO;

namespace Prism_Melody
{
    internal class PalletViewer : MonoBehaviour
    {
        public int charID = 0;
        private int maxID = 0;

        public int palletID = 0;

        private int waitFrame = 30;

        private bool newChar;
        private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

        // Screenshot Stuff
        private List<Animator> anim = new List<Animator>();

        // Game Window Resolution
        public int resWidth = 1920;
        public int resHeight = 1080;

        // Resolution Mulitplier Used During Image Rendering
        public float multiplier = 2;

        // Primary Camera
        public Camera camera;

        private bool ssNextFrame;

        void Start()
        {
            foreach (Idol idol in GlobalManager.Instance.GameManager.Characters)
            {
                if (idol.characterFullHeightArt.name != "boss_portrait" && idol.name != "Boss")
                {
                    maxID++;
                }
            }

            camera = GameObject.Find("Preview Cam").GetComponent<Camera>();
        }

        void Update()
        {
            if (waitFrame > 0)
            {
                waitFrame--;
                return;
            } else if (waitFrame == 0)
            {
                waitFrame = -1;

                charID--;
                if (charID < 0)
                    charID = maxID - 1;

                GameObject.Find("Preview Cam").transform.position = new Vector3(0, 0, -1.7f);

                Melon<Plugin>.Instance.palletViewer = true;
                MethodInfo method = typeof(StageProperties).GetMethod("LogicOnStart", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(GameObject.FindObjectOfType<StageProperties>(), null);
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                charID--;
                if (charID < 0)
                    charID = maxID - 1;

                palletID = 0;

                newChar = true;

                Melon<Plugin>.Instance.palletViewer = true;
                MethodInfo method = typeof(StageProperties).GetMethod("LogicOnStart", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(GameObject.FindObjectOfType<StageProperties>(), null);
                return;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                charID++;
                if (charID >= maxID)
                    charID = 0;

                palletID = 0;

                newChar = true;

                Melon<Plugin>.Instance.palletViewer = true;
                MethodInfo method = typeof(StageProperties).GetMethod("LogicOnStart", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(GameObject.FindObjectOfType<StageProperties>(), null);
                return;
            }

            GameObject[] obj = SceneManager.GetSceneByName("Global").GetRootGameObjects();

            if (newChar || (renderers.Count == 1 && renderers[0] == null) || renderers.Count == 0)
            {
                renderers.Clear();
                anim.Clear();
                foreach (GameObject go in obj)
                {
                    if (go.name == "player1")
                    {
                        go.transform.position = Vector3.zero;
                        renderers.Add(go.GetComponent<SpriteRenderer>());
                    }
                    else if (go.name == "player2")
                    {
                        go.transform.position = new Vector3(-30, 0, 0);
                        renderers.Add(go.GetComponent<SpriteRenderer>());
                    }
                    else if (go.name != "global" && go.name != "New Game Object")
                    {
                        renderers.Add(go.GetComponent<SpriteRenderer>());
                    }

                    if (go.GetComponent<Animator>() != null)
                    {
                        anim.Add(go.GetComponent<Animator>());
                    }
                }
                newChar = false;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                palletID++;
                if (palletID >= GlobalManager.Instance.GameManager.Characters[charID].paletteSwapMaterials.Count)
                {
                    palletID = 0;
                }

                Material mat = new Material(GlobalManager.Instance.GameManager.Characters[charID].paletteSwapMaterials[palletID]);

                foreach (SpriteRenderer renderer in renderers)
                {
                    if (renderer == null) continue;
                    renderer.material = mat;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                palletID--;
                if (palletID <0)
                {
                    palletID = GlobalManager.Instance.GameManager.Characters[charID].paletteSwapMaterials.Count - 1;
                }

                Material mat = new Material(GlobalManager.Instance.GameManager.Characters[charID].paletteSwapMaterials[palletID]);

                foreach (SpriteRenderer renderer in renderers)
                {
                    if (renderer == null) continue;
                    renderer.material = mat;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GlobalManager.Instance.AudioManager.StopAllMusic();
                SceneManager.LoadScene(SceneManager.GetSceneByName("Global").buildIndex);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                foreach (Animator a in anim)
                {
                    a.ForceStateNormalizedTime(0);
                    a.speed = 0;
                }
                ssNextFrame = true;
                return;
            }
            if (ssNextFrame)
            {
                ssNextFrame = false;
                DoScreenshot();
            }
        }

        void OnGUI()
        {
            GUI.Label(new Rect(10, 20, 500, 70), "Use Left/Right Arrow Keys To Change Character");
            GUI.Label(new Rect(10, 40, 500, 70), "Use Up/Down Arrow Keys To Change Pallet");
            GUI.Label(new Rect(10, 60, 500, 70), "Use ESC To Return To Menu");
        }

        public void DoScreenshot()
        {
            if (!Directory.Exists(Application.dataPath + "/../screenshots")) Directory.CreateDirectory(Application.dataPath + "/../screenshots");
            if (!Directory.Exists(Application.dataPath + "/../screenshots/" + GlobalManager.Instance.GameManager.Characters[charID].charName)) Directory.CreateDirectory(Application.dataPath + "/../screenshots/" + GlobalManager.Instance.GameManager.Characters[charID].charName);

            camera.clearFlags = CameraClearFlags.Nothing;

            // Create A Texture2D To Apply The Render To Before Export
            Texture2D screenShot = new Texture2D(Mathf.RoundToInt(float.Parse(resWidth.ToString()) * multiplier), Mathf.RoundToInt(float.Parse(resHeight.ToString()) * multiplier), TextureFormat.RGBA32, false);

            // Create A Render Texture
            RenderTexture rt = new RenderTexture(Mathf.RoundToInt(float.Parse(resWidth.ToString()) * multiplier), Mathf.RoundToInt(float.Parse(resHeight.ToString()) * multiplier), 24);
            // Set The Main Camera's Render Texture
            camera.targetTexture = rt;

            // Render The Main Camera To The Render Texture
            camera.Render();

            // Set Our Render Texture As The Active Render Texture
            RenderTexture.active = rt;

            // Render The Render Texture To The Texture2D
            screenShot.ReadPixels(new Rect(0, 0, Mathf.RoundToInt(float.Parse(resWidth.ToString()) * multiplier), Mathf.RoundToInt(float.Parse(resHeight.ToString()) * multiplier)), 0, 0);

            // Reset Camera And Remove The Active Render Texture
            camera.targetTexture = null;
            RenderTexture.active = null;

            // Destroy Our Render Texture
            Destroy(rt);

            // Encode The Texture2D To A .png
            byte[] bytes = screenShot.EncodeToPNG();
            // Get The Filename
            string filename = ScreenshotName(Mathf.RoundToInt(float.Parse(resWidth.ToString()) * multiplier), Mathf.RoundToInt(float.Parse(resHeight.ToString()) * multiplier));
            // Create The File
            File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));

            camera.clearFlags = CameraClearFlags.Skybox;
            foreach (Animator a in anim)
            {
                a.speed = 1;
            }
        }

        private string ScreenshotName(int width, int height)
        {
            return string.Format("{0}/../screenshots/{4}/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
                             GlobalManager.Instance.GameManager.Characters[charID].charName);
        }
    }
}
