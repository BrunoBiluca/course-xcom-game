using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;

namespace GameAssets
{
    public class DebuggerScreen : Singleton<DebuggerScreen>
    {
        [SerializeField] private GameObject actionButtonPrefab;

        public void Start()
        {
            var holder = transform.Find("holder");

            var actions = GetComponents<IDebuggerAction>();
            foreach(var action in actions)
            {
                var go = Instantiate(actionButtonPrefab, holder);

                go.GetComponent<Button>().onClick.AddListener(action.Execute);
                go.transform.FindComponent<TextMeshProUGUI>("text").text = action.Name;
            }
        }
    }
}
