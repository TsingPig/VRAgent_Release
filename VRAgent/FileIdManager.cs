using System.Collections.Generic;
using UnityEngine;

namespace HenryLab.VRAgent
{
    public class FileIdManager : MonoBehaviour
    {
        // �� GameObject
        public List<string> fileIds = new List<string>();

        public List<GameObject> objects = new List<GameObject>();

        // �� MonoBehaviour Component
        public List<string> scriptFileIds = new List<string>();

        public List<MonoBehaviour> scripts = new List<MonoBehaviour>();

        public void Clear()
        {
            fileIds.Clear();
            objects.Clear();
            scriptFileIds.Clear();
            scripts.Clear();
        }

        public void Add(string fileId, GameObject go)
        {
            if(string.IsNullOrEmpty(fileId) || go == null) return;
            if(!fileIds.Contains(fileId))
            {
                fileIds.Add(fileId);
                objects.Add(go);
            }
        }

        public GameObject GetObject(string fileId)
        {
            int index = fileIds.IndexOf(fileId);
            if(index >= 0 && index < objects.Count)
                return objects[index];
            Debug.LogWarning($"{Str.Tags.LogsTag} Missing object for FileID: {fileId}", this);
            return null;
        }

        public new MonoBehaviour GetComponent(string scriptFileId)
        {
            int index = scriptFileIds.IndexOf(scriptFileId);
            if(index >= 0 && index < scripts.Count)
                return scripts[index];
            Debug.LogWarning($"{Str.Tags.LogsTag} Missing script for FileID: {scriptFileId}", this);
            return null;
        }

        /// <summary>
        /// ��һ�� eventUnits ��ӵ� FileIdManager
        /// </summary>
        /// <param name="eventUnits">�¼��б�</param>
        /// <param name="methodCallCount">�����methodCall����</param>
        /// <param name="hitMethodCallCount">�������ЧmethodCall����</param>
        public void AddComponents(IEnumerable<eventUnit> eventUnits, ref int methodCallCount, ref int hitMethodCallCount)
        {
            if(eventUnits == null) return;

            foreach(var eventUnit in eventUnits)
            {
                if(eventUnit.methodCallUnits == null) continue;

                foreach(var methodCallUnit in eventUnit.methodCallUnits)
                {
                    methodCallCount++;
                    MonoBehaviour component = FileIdResolver.FindComponentByFileID(methodCallUnit.script);
                    if(component != null)
                    {
                        hitMethodCallCount++;
                        _AddComponent(methodCallUnit.script, component);
                    }
                    else
                    {
                        Debug.LogWarning($"{Str.Tags.LogsTag}{methodCallUnit}'s script is null");
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// ��һ�� eventUnit ��ӵ� FileIdManager
        /// </summary>
        /// <param name="scriptFileId"></param>
        /// <param name="component"></param>
        internal void _AddComponent(string scriptFileId, MonoBehaviour component)
        {
            if(string.IsNullOrEmpty(scriptFileId) || component == null) return;
            if(!scriptFileIds.Contains(scriptFileId))
            {
                scriptFileIds.Add(scriptFileId);
                scripts.Add(component);
            }
        }
    }
}