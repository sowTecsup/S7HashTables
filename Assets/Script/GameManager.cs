using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    public MyHashTable<string, object> myHashTable;
    public string nombre;
    public int debugQuantity;
    void Start()
    {
        
    }


    [Button]
    public void ShowHashCode()
    {
        Debug.Log("Hash code : " + (nombre.GetHashCode() & 0x7FFFFFF));
    }
    [Button]
    public void ShowHashCodeClamped()
    {
        Debug.Log("Hash code : " + (nombre.GetHashCode() % debugQuantity));
    }

}
