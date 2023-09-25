using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mimi.InspectorAnimation
{
    public class InspectorUpdateGroup : IEnumerable<InspectorAnimator>
    {
        public readonly struct GroupKey
        {
            public GroupKey(Editor editor, double frameRate)
            {
                Editor = editor;
                FrameRate = frameRate;
            }

            public Editor Editor { get; }
            public double FrameRate { get; }
        }

        public class List
        {
            private InspectorUpdateGroup[]? updateGroups;

            public void Refresh(InspectorAnimator animation, InspectorUpdateGroup[] newUpdateGroups)
            {
                //Debug.Log($"## Refresh Count = {newUpdateGroups.Length}");
                if (updateGroups == null)
                {
                    foreach (var group in newUpdateGroups)
                    {
                        //DebugLog(animation, group, $"# Try Add");
                        if (group.animations.Add(animation))
                        {
                            //DebugLog(animation, group, "Add");
                        }
                    }
                }
                else
                {
                    foreach (var group in updateGroups)
                    {
                        group.animations.Remove(animation);
                    }

                    foreach (var group in newUpdateGroups)
                    {
                        //DebugLog(animation, group, $"# Try Add");
                        if (group.animations.Add(animation))
                        {
                            //DebugLog(animation, group, "Add");
                        }
                    }
                }

                updateGroups = newUpdateGroups;
            }

            public void ForceRepaint()
            {
                foreach (var g in updateGroups!) g.forceRepaint = true;
            }
        }

        private readonly HashSet<InspectorAnimator> animations;
        private bool forceRepaint = false;
        public InspectorUpdateGroup(GroupKey key)
        {
            animations = new HashSet<InspectorAnimator>();
            Key = key;
            Editor = key.Editor;
            DeltaTime = TimeSpan.FromSeconds(1.0f / key.FrameRate);
            LastRepaintTime = DateTime.MinValue;
            UpdateTime = DateTime.MinValue;
        }

        public InspectorAnimator[] Animations => animations.ToArray();
        public GroupKey Key { get; }
        public Editor Editor { get; }
        public TimeSpan DeltaTime { get; }
        public DateTime LastRepaintTime { get; private set; }
        public DateTime UpdateTime { get; private set; }
        public int Count => animations.Count;
        public bool HasAnimations => forceRepaint || animations.Count > 0;
        public bool IsRepaintable => HasAnimations && NonRepaintTime >= DeltaTime;
        public bool IsContinuous => HasAnimations || NonRepaintTime < DeltaTime * 30;
        public TimeSpan NonRepaintTime => UpdateTime - LastRepaintTime;

        public void TryUpdate(DateTime updateTime, out bool isRepainted, out bool isContinuous)
        {
            TrimEndAnimation();
            UpdateTime = updateTime;
            isRepainted = IsRepaintable;
            isContinuous = IsContinuous;

            if (isRepainted)
            {
                LastRepaintTime = updateTime;
                Editor.Repaint();
                forceRepaint = false;
            }
            else if (!isContinuous)
            {
                Editor.Repaint();
            }
        }

        private void TrimEndAnimation()
        {
            if (Editor == null)
            {
                animations.Clear();
            }
            else
            {
                for (int i = animations.Count - 1; i >= 0; i--)
                {
                    var animation = animations.ElementAt(i);
                    if (!animation.IsRepeating && !animation.IsPlaying)
                    {
                        //DebugLog(animation, this, "Remove End Animation");
                        animations.Remove(animation);
                    }
                }
            }
        }

        public IEnumerator<InspectorAnimator> GetEnumerator()
        {
            return ((IEnumerable<InspectorAnimator>)animations).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)animations).GetEnumerator();
        }
    }
}