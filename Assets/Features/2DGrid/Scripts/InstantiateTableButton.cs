using UnityEngine;
using UnityEngine.UI;

namespace Features._2DGrid.Scripts
{
    public class InstantiateTableButton : MonoBehaviour
    {
        [SerializeField] private GameObject[] tablePrefab;
        [SerializeField] private InputManager inputManager;


        public void InstantiateTable(int index)
        {
            Debug.Log("Herer");
            // Call the InstantiateAndPlaceTable method in the InputManager
            inputManager.InstantiateAndPlaceTable(tablePrefab[index], index);
        }
    }
}