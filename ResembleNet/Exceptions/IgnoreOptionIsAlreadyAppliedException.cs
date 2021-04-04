using System;

namespace ResembleNet.Exceptions
{
    public class IgnoreOptionIsAlreadyAppliedException : Exception
    {
        public IgnoreOptionIsAlreadyAppliedException(string message)
            : base(message)
        {
        }
    }
}
