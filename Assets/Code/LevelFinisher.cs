using UnityEditor.SceneManagement;
using UnityEngine;

public class LevelFinisher : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        OpenScene();
    }
}
