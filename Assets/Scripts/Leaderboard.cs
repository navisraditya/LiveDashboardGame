using System.Threading.Tasks;
using App;
using Supabase.Gotrue;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] TMP_InputField nameList;
    [SerializeField] TMP_InputField scoreList;
    [SerializeField] TMP_InputField playerPosition;
    async void Awake() {        
        if(SupabaseStuff.Instance != null) {
            
        }
    }
}