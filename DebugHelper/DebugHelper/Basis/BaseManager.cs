using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DebugHelper.Basis
{
    public abstract class BaseManager : MonoBehaviour
    {
        public float UpdateRepeatRate
        {
            get => m_updateRepeatRate;
            set
            {
                m_updateRepeatRate = value;
                if (isTicking)
                {
                    StopTicking();
                    StartTicking(m_updateRepeatRate);
                }
            }
        }
        private float m_updateRepeatRate = 1f;
        public bool isTicking { get; private set; }

        #region Ticking
        public virtual void StartTicking(float startDelay = 0f)
        {
            isTicking = true;
            InvokeRepeating(nameof(Tick), startDelay, UpdateRepeatRate);
        }
        public abstract void Tick();
        public virtual void StopTicking()
        {
            isTicking = false;
            CancelInvoke(nameof(Tick));
        }
        #endregion
    }
}
