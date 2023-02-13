using System;

namespace GameAssets
{
    public class NoAPAvaiable : InvalidOperationException
    {
        const string message = "Actor has no action points avaialable";
        public NoAPAvaiable() : base(message)
        {
        }
    }
}
