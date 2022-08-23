using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
// ref - http://answers.unity.com/answers/817043/view.html
    public class RandoAudio : MonoBehaviour
    {
        public List<AudioClip> clipList;
        public AudioClip[] clipListArray;

        public AudioClip GetRandomClip(string path)
        {
            // store audio & get random clip
            clipListArray = Resources.LoadAll<AudioClip>(path);
            clipList = clipListArray.ToList();
            var clipToPlay = clipList[Random.Range(0, clipList.Count)];

            return clipToPlay;
        }
    }
}