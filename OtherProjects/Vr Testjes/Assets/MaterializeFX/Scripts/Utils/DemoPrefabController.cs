using UnityEngine;

namespace Assets.MaterializeFX.Scripts.Utils
{
    internal sealed class DemoPrefabController : MonoBehaviour
    {
        public int StartNum;
        public GameObject[] Prefabs;

        private GameObject _currentInstance;
        private int _currentPrefabNum;

        public void Next()
        {
            if (Prefabs.Length == 0)
                return;

            _currentPrefabNum++;
            if (_currentPrefabNum >= Prefabs.Length)
                _currentPrefabNum = 0;

            ChangePrefab(_currentPrefabNum);
        }

        private void Start()
        {
            _currentPrefabNum = StartNum;

            ChangePrefab(_currentPrefabNum);
        }

        private void ChangePrefab(int num)
        {
            if (_currentInstance != null)
                Destroy(_currentInstance);
            var newPrefab = Prefabs[num];
            _currentInstance = Instantiate(newPrefab, newPrefab.transform.position, newPrefab.transform.transform.rotation);
            _currentInstance.SetActive(true);
        }
    }
}