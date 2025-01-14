using System.Runtime.InteropServices;

namespace OverlaySharp.Interop.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Converts a managed <see cref="string"/> to an unmanaged <c>char*</c>.
        /// </summary>
        /// <param name="str">The string to convert. If the string is <c>null</c> or whitespace, the method returns <c>null</c>.</param>
        /// <returns>
        /// A char* to the unmanaged string or <c>null</c> if the string is <c>null</c> or whitespace.
        /// </returns>
        /// <remarks>
        /// The memory allocated for the unmanaged string must be freed manually using
        /// <see cref="Marshal.FreeHGlobal(nint)"/> to avoid memory leaks.
        /// </remarks>
        public static unsafe char* ToCharPointer(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            return (char*)Marshal.StringToHGlobalUni(str);
        }
    }
}
