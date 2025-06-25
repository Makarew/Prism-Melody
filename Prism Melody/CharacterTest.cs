//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using MelonLoader;
//using IdolShowdown.Managers;
//using IdolShowdown.Structs;
//using UnityEngine.UI;
//using IdolShowdown;

//namespace Prism_Melody
//{
//    internal class CharacterTest
//    {
//        internal static AssetBundle bundle;

//        internal static void LoadTestCharacter()
//        {
//            if (bundle == null)
//                bundle = AssetBundle.LoadFromFile(Path.Combine(MelonHandler.PluginsDirectory, "Palettes", "Makoto", "makoto.ischar"));

//            if (bundle != null )
//                Melon<Plugin>.Logger.Msg("Found Makoto");

//            //Melon<Plugin>.Logger.Msg(GlobalManager.Instance.GameManager.Characters[3].originalPrefab.GetComponent<Animator>().runtimeAnimatorController.animationClips[56].name);

//            AnimatorOverrideController aoc = new AnimatorOverrideController(GlobalManager.Instance.GameManager.Characters[3].originalPrefab.GetComponent<Animator>().runtimeAnimatorController);

//            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

//            foreach(var a in aoc.animationClips)
//            {
//                if (bundle.Contains(a.name))
//                {
//                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, (AnimationClip)bundle.LoadAsset(a.name)));

//                    if (a.name.ToLower().Contains("idle"))
//                    {
//                        Melon<Plugin>.Instance.cssIdle[4] = (AnimationClip)bundle.LoadAsset(a.name);
//                    }
//                }
//                else
//                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, a));
//            }

//            aoc.ApplyOverrides(anims);
//            GlobalManager.Instance.GameManager.Characters[3].originalPrefab.GetComponent<Animator>().runtimeAnimatorController = aoc;

//            foreach (Idol idol in GlobalManager.Instance.GameManager.Characters)
//            {
//                if (bundle.Contains(idol.charName.ToLower() + "_hud"))
//                {
//                    Texture2D tex = (Texture2D)bundle.LoadAsset(idol.charName.ToLower() + "_hud");
//                    idol.charIcon = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
//                }
//                if (bundle.Contains(idol.charName.ToLower() + "_portrait"))
//                {
//                    Texture2D tex = (Texture2D)bundle.LoadAsset(idol.charName.ToLower() + "_portrait");
//                    idol.superFlashSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
//                }
//                if (bundle.Contains(idol.charName.ToLower() + "_health"))
//                {
//                    Texture2D tex = (Texture2D)bundle.LoadAsset(idol.charName.ToLower() + "_health");
//                    idol.charHUDIcon = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
//                }
//                if (bundle.Contains(idol.charName.ToLower() + "_cssicon"))
//                {
//                    Texture2D tex = (Texture2D)bundle.LoadAsset(idol.charName.ToLower() + "_cssicon");
//                    GameObject.Find("CSSMain/CharacterDiamond/CharacterButtons/" + idol.charName + "Button/IdolIconMask/IdolImage").GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 200.0f);

//                    GlobalManager.Instance.GameManager.CharSelect.GetComponent<CSSDataController>().loadedCommand
//                }
//            }

//                //AnimationClip anim = (AnimationClip)bundle.LoadAsset("Idle");

//                //GlobalManager.Instance.GameManager.Characters[3].originalPrefab.GetComponent<Animator>().runtimeAnimatorController.animationClips[56] = anim;
//                //Melon<Plugin>.Logger.Msg(anim.name);
//                //Melon<Plugin>.Logger.Msg(GlobalManager.Instance.GameManager.Characters[3].originalPrefab.GetComponent<Animator>().runtimeAnimatorController.animationClips[56].name);
//            }

//        internal static void SetCSSIdles()
//        {
//            AnimatorOverrideController aoc = new AnimatorOverrideController(GameObject.Find("CSSMain/CharacterData/P1/BottomTriangle/TriangleMask/DummySprite").GetComponent<Animator>().runtimeAnimatorController);

//            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

//            foreach(var a in aoc.animationClips)
//            {
//                if (a.name == "CSSFubukiIdle" && bundle.Contains("fubuki_css"))
//                {
//                    AnimationClip anim = (AnimationClip)bundle.LoadAsset("fubuki_css");
//                    anim.name = "CSSFubukiIdle";
//                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, anim));
//                } else
//                {
//                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, a));
//                }
//            }

//            aoc.ApplyOverrides(anims);
//            GameObject.Find("CSSMain/CharacterData/P1/BottomTriangle/TriangleMask/DummySprite").GetComponent<Animator>().runtimeAnimatorController = aoc;
//        }
//    }
//}
