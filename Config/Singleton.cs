
using System.Runtime.InteropServices;
namespace WorkerServiceTest2.Config
{
    // from https://csharpindepth.com/articles/singleton

    // Fully lazy instantiation. It is triggered first time there is a reference to the static member of the containing
    // Class in Instance, and it will only be executed once per app domain, which does so that only 1 thread at a time 
    // can run it. These two things makes it lazy and threadsafe

    // It is made a singleton, because it has to use the list of tokens all over the application.

    internal sealed class Singleton
    {
        internal string osPlatform = null;

        internal Singleton()
        {
        }
        // Here the class returns itself
        internal static Singleton Instance { get { return check.instance; } }
        private class check
        {
            static check()
            {

            }
            internal static readonly Singleton instance = new Singleton();
        }
    }
}
