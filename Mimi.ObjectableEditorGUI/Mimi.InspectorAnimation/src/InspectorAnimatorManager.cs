using System;
using System.Collections.Generic;
using UnityEditor;

namespace Mimi.InspectorAnimation
{

    public class InspectorAnimatorManager
    {
        private readonly Dictionary<InspectorUpdateGroup.GroupKey, InspectorUpdateGroup> updateGroups = 
            new Dictionary<InspectorUpdateGroup.GroupKey, InspectorUpdateGroup>();

        public InspectorUpdateGroup[]? GetUpdateGroups(Editor[] editors, double frameRate)
        {
            if (editors == null) return null;

            var result = new List<InspectorUpdateGroup>(editors.Length);

            foreach (var editor in editors)
            {
                var key = new InspectorUpdateGroup.GroupKey(editor, frameRate);

                if (!updateGroups.TryGetValue(key, out var group))
                {
                    updateGroups[key] = group = new InspectorUpdateGroup(key);
                }

                result.Add(group);
            }

            return result.ToArray();
        }

        public bool IsUpdating { get; private set; } = false;

        public void TryStartUpdate()
        {
            if (!IsUpdating)
            {
                //Debug.Log("Start");
                IsUpdating = true;
                EditorApplication.update += Update;
            }
        }

        public void EndUpdate()
        {
            IsUpdating = false;
            //Debug.Log("End");
            EditorApplication.update -= Update;

            updateGroups.Clear();
            updateGroups.TrimExcess();
        }

        private void Update()
        {
            var updateTime = DateTime.Now;

            //Debug.Log($"UpdateGroups Count : {updateGroups.Count}");

            HashSet<Editor> checkedEditors = new HashSet<Editor>();
            List<InspectorUpdateGroup.GroupKey> removeList = new List<InspectorUpdateGroup.GroupKey>();
            foreach (var (key, updateGroup) in updateGroups)
            {
                if (checkedEditors.Contains(updateGroup.Editor))
                {
                    continue;
                }

                updateGroup.TryUpdate(updateTime, out bool isRepainted, out bool isContinuous);

                if (!isContinuous)
                {
                    removeList.Add(key);
                    checkedEditors.Add(updateGroup.Editor);
                }
                else if (isRepainted)
                {
                    checkedEditors.Add(updateGroup.Editor);
                }
            }

            foreach (var remove in removeList)
            {
                updateGroups.Remove(remove);
            }

            if (updateGroups.Count == 0)
            {
                EndUpdate();
            }
        }
    }
}