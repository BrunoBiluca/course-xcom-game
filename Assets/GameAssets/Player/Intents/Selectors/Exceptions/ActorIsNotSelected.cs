using System;

namespace GameAssets
{
    public sealed class ActorIsNotSelected : Exception
    {
        const string message = "Actor needs to be selected to do this operation";
        public ActorIsNotSelected() : base(message)
        {
        }
    }
}