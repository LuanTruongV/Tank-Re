using _Scripts.InGame.Message;
using Photon.Pun;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.InGame
{
    public class Map : MonoBehaviourPunCallbacks
    {
        [FormerlySerializedAs("_base")] [SerializeField] private GameObject _basePrefab;
        private Base _base;

        private void Awake()
        {
            MessageBroker.Default.Receive<Destroy>().Subscribe(x => DestroyMap(x.go)).AddTo(this);
            EnemyType enemyType = EnemyType.Armor;

        }

        private void Start()
        {
            CreateMap();
        }

        private void CreateMap()
        {
            int level = PlayerPrefs.GetInt("level", 1);
            string path = $"Level/Lv{level}";
            GameObject map=Resources.Load<GameObject>(path);
            Instantiate(map,transform);
            Instantiate(_basePrefab, transform);
            
        }

        public void DestroyMap(GameObject go)
        {
            int id1 = go.transform.GetSiblingIndex();
            int id2 = go.transform.parent.GetSiblingIndex();
            int id3 = go.transform.parent.parent.parent.GetSiblingIndex();
            photonView.RPC(nameof(DestroyMapRPC),RpcTarget.AllBuffered,id1,id2,id3);
        }
        [PunRPC]
        private void DestroyMapRPC(int id1,int id2,int id3)
        {
            transform.GetChild(id3).GetChild(0).GetChild(id2).GetChild(id1).gameObject.SetActive(false);
        }
    }
}