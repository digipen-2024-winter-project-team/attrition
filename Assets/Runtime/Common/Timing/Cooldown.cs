using System;
using Attrition.Common.SerializedEvents;
using UnityEngine;

namespace Attrition.Common.Timing
{
    [Serializable]
    public class Cooldown
    {
        private float time;
        [SerializeField]
        private float duration = 5f;
        [SerializeField]
        private bool isRunning;
        [SerializeField]
        private SerializedEvent elapsed;
        
        public float Time
        {
            get => this.time;
            private set => this.time = value;
        }
        public float Duration
        {
            get => this.duration;
            set => this.duration = value;
        }
        public float Percentage => this.Duration > 0f ? this.Time / this.Duration : 0f;
        public bool IsRunning
        {
            get => this.isRunning;
            private set => this.isRunning = value;
        }
        public SerializedEvent Elapsed => this.elapsed;
        
        public void Start()
        {
            if (!this.IsRunning)
            {
                this.Time = this.Duration;
            }
            
            this.IsRunning = true;
        }

        public void Stop()
        {
            this.IsRunning = false;
        }

        public void Reset()
        {
            this.Time = this.Duration;
            this.IsRunning = false;
        }

        public void Restart()
        {
            this.Reset();
            this.Start();
        }

        public void Tick(float deltaTime)
        {
            if (!this.IsRunning)
            {
                return;
            }
            
            this.Time = Math.Max(0f, this.Time - deltaTime);
            
            if (this.Time <= 0f)
            {
                this.Elapsed.Invoke();
                this.Reset();
            }
        }
    }
}
