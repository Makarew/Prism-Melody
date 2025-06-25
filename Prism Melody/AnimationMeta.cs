using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Prism_Melody
{
    internal class AnimationMeta
    {
        public string character = "Fubuki";
        public string animationExtension = "_5L";
        public int framerate = 12;

        public FrameData[] frames = new FrameData[0];

        public struct FrameData
        {
            public string imageName;
            public int spriteIndex;
        }

        public static AnimationMeta Load(string path)
            => JsonConvert.DeserializeObject<AnimationMeta>(File.ReadAllText(path)); //JsonUtility.FromJson<AnimationMeta>(File.ReadAllText(path));
    }
}
