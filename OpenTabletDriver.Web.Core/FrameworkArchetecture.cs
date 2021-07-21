namespace OpenTabletDriver.Web.Core
{
    public enum FrameworkArchetecture
    {
        Unknown = 0,
        x86 = 1 << 0,
        x64 = 1 << 1,
        ARM64 = 1 << 2
    }
}