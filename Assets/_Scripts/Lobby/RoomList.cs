using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Lobby
{
    public class RoomList : MonoBehaviour
    {
        [SerializeField] private GameObject _roomItemPrefab;
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private VerticalLayoutGroup _verticalLayout;
        private bool _isAllRoomNotOpen;

        public void UpdateList(List<RoomInfo> roomList)
        {
            ClearList();
            _isAllRoomNotOpen = true;
            if (roomList.Count == 0)
            {
                OnNoRoomsFound();
            }
            foreach (RoomInfo room in roomList)
            {
                if (!room.IsVisible || !room.IsOpen || room.MaxPlayers==0)
                {
                    continue;
                }

                RoomItem roomItem = Instantiate(_roomItemPrefab, _verticalLayout.transform).GetComponent<RoomItem>();
                roomItem.SetItem(room);
                _isAllRoomNotOpen = false;
            }

            if (_isAllRoomNotOpen)
            {
                OnNoRoomsFound();
            }
            
        }
        public void OnNoRoomsFound()
        {
            _statusText.text = "No room found";
            _statusText.gameObject.SetActive(true);
        }
        private void ClearList()
        {
            foreach (Transform child in _verticalLayout.transform)
            {
                Destroy(child.gameObject);
            }

            _statusText.gameObject.SetActive(false);
        }
    }
}