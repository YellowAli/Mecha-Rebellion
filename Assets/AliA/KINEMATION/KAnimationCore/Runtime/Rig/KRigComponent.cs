using System.Collections.Generic;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Rig
{
    public class KRigComponent : MonoBehaviour
    {
        [SerializeField] private List<Transform> hierarchy = new List<Transform>();
        private List<KVirtualElement> _virtualElements;
        private Dictionary<string, int> _hierarchyMap;

#if UNITY_EDITOR
        [SerializeField] private List<int> hierarchyDepths = new List<int>();

        public int[] GetHierarchyDepths()
        {
            return hierarchyDepths.ToArray();
        }
        
        public void RefreshHierarchy()
        {
            hierarchy.Clear();
            hierarchyDepths.Clear();
            TraverseHierarchyByLayer(transform, 0);
        }

        public Transform[] GetHierarchy()
        {
            if (hierarchy == null)
            {
                return null;
            }
            
            return hierarchy.ToArray();
        }

        public bool Contains(string entry)
        {
            if (hierarchy == null) return false;

            HashSet<string> set = new HashSet<string>();
            foreach (var element in hierarchy)
            {
                set.Add(element.name);
            }

            return set.Contains(entry);
        }
        
        private void TraverseHierarchyByLayer(Transform currentTransform, int depth)
        {
            hierarchy.Add(currentTransform);
            hierarchyDepths.Add(depth);
            
            foreach (Transform child in currentTransform)
            {
                TraverseHierarchyByLayer(child, depth + 1);
            }
        }
#endif
        
        public void Initialize()
        {
            // Register Virtual Elements.
            _virtualElements = new List<KVirtualElement>();
            KVirtualElement[] virtualElements = GetComponentsInChildren<KVirtualElement>();

            foreach (var virtualElement in virtualElements)
            {
                _virtualElements.Add(virtualElement);
            }

            // Map the hierarchy indexes to the element names.
            _hierarchyMap = new Dictionary<string, int>();

            int count = hierarchy.Count;
            for (int i = 0; i < count; i++)
            {
                _hierarchyMap.TryAdd(hierarchy[i].name, i);
            }
        }

        public void AnimateVirtualElements()
        {
            foreach (var virtualElement in _virtualElements)
            {
                virtualElement.Animate();
            }
        }
        
        public Transform[] GetRigTransforms()
        {
            return hierarchy.ToArray();
        }

        public Transform GetRigTransform(KRigElement rigElement)
        {
            int index = rigElement.index;
            
            // Invalid index, try to use the element name instead.
            if (index < 0 || index > hierarchy.Count - 1)
            {
                index = _hierarchyMap[rigElement.name];
            }

            // Total failure, return null.
            if (index < 0 || index > hierarchy.Count - 1)
            {
                return null;
            }
            
            return hierarchy[index].transform;
        }
        
        public Transform GetRigTransform(string elementName)
        {
            if (_hierarchyMap.TryGetValue(elementName, out var element))
            {
                return hierarchy[element];
            }
            
            return null;
        }
        
        public Transform GetRigTransform(int elementIndex)
        {
            if (elementIndex < 0 || elementIndex > hierarchy.Count - 1)
            {
                return null;
            }

            return hierarchy[elementIndex].transform;
        }
    }
}