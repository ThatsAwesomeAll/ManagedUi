using System;
using UnityEngine;

namespace UI.ManagedUi
{
    public class UiInputManager : MonoBehaviour
    {
        
        public Action OnLeft;
        public Action OnRight;
        public Action OnUp;
        public Action OnDown;

        private int currentSecond = 0;
        
        void Update()
        {
            if (currentSecond != (int)Time.time)
            {
                currentSecond++;
            }

            switch (currentSecond % 4)
            {
                case 0:
                    OnLeft?.Invoke();
                    break;
                case 1:
                    OnRight?.Invoke();
                    break;
                case 2:
                    OnUp?.Invoke();
                    break;
                case 3:
                    OnDown?.Invoke();
                    break;
            }
        }

        
        
    }
}