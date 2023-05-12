using UnityEngine;

namespace GameMechanicsControllers
{
    public class CursorController : MonoBehaviour
    {
        public Texture2D aimCursor;
        public GameObject gameLogic;

        private bool _endGame;

        private void Start()
        {
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.x += 10;
            cursorPosition.y += 10;
            Cursor.SetCursor(aimCursor, cursorPosition, CursorMode.Auto);
            _endGame = false;
        }

        private void Update()
        {
            var cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = cursorPos;
            _endGame = gameLogic.GetComponent<GameGuideController>().EndGame;
            Cursor.SetCursor(_endGame ? null :  aimCursor, cursorPos, CursorMode.Auto);
        }
    }
}
