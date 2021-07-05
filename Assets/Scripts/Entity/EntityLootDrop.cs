using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity
{
    public class EntityLootDrop : MonoBehaviour
    {
        [SerializeField] private LootTable lootTable;
        [SerializeField] private GameObject entityItemPreFab;

        private void Start()
        {
            if (TryGetComponent(out Entity entity)) entity.OnDeath.AddListener(Death);
        }

        private void Death()
        {
            int sum = lootTable.Entries.Sum(entry => entry.weight);
            float[] bucket = new float[lootTable.Entries.Length];

            for (var i = 0; i < bucket.Length; i++) bucket[i] = lootTable.Entries[i].weight / (float) sum;

            Dictionary<Item, int> results = new Dictionary<Item, int>();
            for (int _ = 0; _ < Random.Range(lootTable.Amount.Min, lootTable.Amount.Max); _++)
            {
                float randomValue = Random.value;
                for (int i = 0; i < bucket.Length; i++)
                {
                    if (randomValue < bucket[i])
                    {
                        Item item = lootTable.Entries[i].item;
                        if (results.ContainsKey(item))
                            results[item]++;
                        else
                            results.Add(item, 1);
                        break;
                    }
                    else
                        randomValue -= bucket[i];
                }
            }

            foreach (KeyValuePair<Item, int> result in results)
            {
                GameObject obj = Instantiate(entityItemPreFab);
                obj.transform.position = transform.position;
                if (obj.TryGetComponent(out ItemEntity entity))
                    entity.ItemStack = new ItemStack {Amount = result.Value, Item = result.Key};
            }
        }
    }
}