using Managers;
using UnityEngine;

namespace Ui.Saves
{
    public class SaveController : MonoBehaviour
    {
        [SerializeField] private GameObject entryPreFab;
        [SerializeField] private Transform content;
        [SerializeField] private State state;
        private GameManager _gameManager;
        private SaveManager _saveManager;

        private void OnEnable()
        {
            _saveManager = SaveManager.Instance;
            _gameManager = GameManager.Instance;
        }

        public void OnButtonClick(int index)
        {
            if (state == State.NewSave)
            {
                _saveManager.NewSave(index - 1);
                _gameManager.NewGame();
                return;
            }

            _saveManager.LoadSave(index - 1);
            _gameManager.LoadHub();
        }

        public void Load()
        {
            state = State.LoadExisting;
        }

        public void New()
        {
            state = State.NewSave;
        }

        private enum State
        {
            LoadExisting,
            NewSave
        }
    }
}