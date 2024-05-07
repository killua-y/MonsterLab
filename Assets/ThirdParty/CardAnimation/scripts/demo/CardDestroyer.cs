using events;
using UnityEngine;

namespace demo {
    public class CardDestroyer : MonoBehaviour {
        public CardContainer container;
        public void OnCardDestroyed(CardPlayed evt) {
            Debug.Log("Destory Card: ");
            //string cardName = evt.card.GetComponent<MonsterCardDisplay>().GetCardName();
            //if (cardName != null)
            //{
            //    Debug.Log("Destory Card: " + cardName);
            //}
            //container.DestroyCard(evt.card);
        }
    }
}
