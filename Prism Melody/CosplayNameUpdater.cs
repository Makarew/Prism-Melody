using IdolShowdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;

namespace Prism_Melody
{
    public class CosplayNameUpdater : MonoBehaviour
    {
        public struct CosplayList
        {
            public string idol;
            public List<string> cosplayName;
        }

        public List<CosplayList> cosplayList;

        public CSSDataUpdater p1;
        public CSSDataUpdater p2;

        public SelectState p1State;
        public SelectState p2State;

        private void Start()
        {
            p1 = GameObject.Find("CSSMain/CharacterData/P1").GetComponent<CSSDataUpdater>();
            p2 = GameObject.Find("CSSMain/CharacterData/P2").GetComponent<CSSDataUpdater>();
        }

        public void StateUpdate()
        {
            if (p1State == SelectState.CHARACTER && p1.CurrentState == SelectState.COSPLAY)
            {
                CosplayList myList = new CosplayList();
                foreach(CosplayList cl in cosplayList)
                {
                    if (cl.idol == p1.SelectedCharacter.charName)
                    {
                        myList = cl;
                        break;
                    }
                }

                System.Type t = typeof(SwitchButtonScript);
                FieldInfo field = t.GetField("options", BindingFlags.Instance | BindingFlags.NonPublic);
                List<string> list = (List<string>)field.GetValue(GameObject.Find("CSSMain/CharacterData/P1/BottomTriangle/CosplaySwitcher").GetComponent<SwitchButtonScript>());

                for (int i = 0; i < myList.cosplayName.Count; i++)
                {
                    list[11 + i] = myList.cosplayName[i];
                }

                field.SetValue(GameObject.Find("CSSMain/CharacterData/P1/BottomTriangle/CosplaySwitcher").GetComponent<SwitchButtonScript>(), list);
            }
            if (p2State == SelectState.CHARACTER && p2.CurrentState == SelectState.COSPLAY)
            {
                CosplayList myList = new CosplayList();
                foreach (CosplayList cl in cosplayList)
                {
                    if (cl.idol == p2.SelectedCharacter.charName)
                    {
                        myList = cl;
                        break;
                    }
                }

                System.Type t = typeof(SwitchButtonScript);
                FieldInfo field = t.GetField("options", BindingFlags.Instance | BindingFlags.NonPublic);
                List<string> list = (List<string>)field.GetValue(GameObject.Find("CSSMain/CharacterData/P2/BottomTriangle/CosplaySwitcher").GetComponent<SwitchButtonScript>());

                for (int i = 0; i < myList.cosplayName.Count; i++)
                {
                    list[11 + i] = myList.cosplayName[i];
                }

                field.SetValue(GameObject.Find("CSSMain/CharacterData/P1/BottomTriangle/CosplaySwitcher").GetComponent<SwitchButtonScript>(), list);
            }
        }
    }
}
