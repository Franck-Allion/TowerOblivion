using TMPro;
using UnityEngine;

namespace TowerOblivion.Presentation.RoomView
{
    public sealed class RoomView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _roomNameLabel;

        public void Initialize(TextMeshProUGUI roomNameLabel)
        {
            _roomNameLabel = roomNameLabel;
        }

        public void SetRoomName(string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return;
            }

            if (_roomNameLabel != null)
            {
                _roomNameLabel.text = roomName;
            }
        }
    }
}
