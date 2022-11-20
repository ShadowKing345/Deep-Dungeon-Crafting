using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;
using Utils.Interfaces;

namespace Entity
{
    public class EntityLootDrop : MonoBehaviour
    {
        [SerializeField] private LootTable lootTable;
        [SerializeField] private GameObject entityItemPreFab;

        private void Start()
        {
            // if (TryGetComponent(out Entity entity)) entity.OnDeath += Death;
        }

        private void Death(IDamageable damageable)
        {
            var sum = lootTable.Entries.Sum(entry => entry.weight);
            var bucket = new float[lootTable.Entries.Length];

            for (var i = 0; i < bucket.Length; i++) bucket[i] = lootTable.Entries[i].weight / (float) sum;

            var results = new Dictionary<Item, int>();
            for (var _ = 0; _ < Random.Range(lootTable.Amount.Min, lootTable.Amount.Max); _++)
            {
                var randomValue = Random.value;
                for (var i = 0; i < bucket.Length; i++)
                    if (randomValue < bucket[i])
                    {
                        var item = lootTable.Entries[i].item;
                        if (results.ContainsKey(item))
                            results[item]++;
                        else
                            results.Add(item, 1);
                        break;
                    }
                    else
                    {
                        randomValue -= bucket[i];
                    }
            }

            foreach (var result in results)
            {
                var obj = Instantiate(entityItemPreFab, transform.parent);
                obj.transform.position = transform.position;
                if (obj.TryGetComponent(out ItemEntity entity))
                    entity.ItemStack = new ItemStack {Amount = result.Value, Item = result.Key};
            }
        }
    }
}