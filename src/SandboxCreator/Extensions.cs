using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxCreator
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string theString)
        {
            return string.IsNullOrEmpty(theString);
        }
    }
}
