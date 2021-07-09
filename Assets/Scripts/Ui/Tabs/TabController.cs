using System;
using UnityEngine;

namespace Ui.Tabs
{
    public class TabController : MonoBehaviour
    {
        [SerializeField] private int currentPage;
        [SerializeField] private GameObject[] pages;

        private void OnEnable() => ChangePage(currentPage);

        public void ChangePage(int index)
        {
            if (index >= pages.Length) return;
            if (index == currentPage) return;
            
            pages[currentPage].SetActive(false);
            pages[index].SetActive(true);

            currentPage = index;
        }
    }
}