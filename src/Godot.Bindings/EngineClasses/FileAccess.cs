using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Godot.Bridge;

namespace Godot;

partial class FileAccess
{
    /// <summary>
    /// Reads up to <c>buffer.Length</c> bytes from the file into <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">The buffer to read data into.</param>
    /// <returns>
    /// The number of bytes actually read.
    /// </returns>
    public unsafe int GetBuffer(Span<byte> buffer)
    {
        if (buffer.IsEmpty)
        {
            return 0;
        }

        fixed (byte* ptr = buffer)
        {
            ulong bytesRead = GodotBridge.GDExtensionInterface.file_access_get_buffer(
                (void*)GetNativePtr(this),
                ptr,
                (uint)buffer.Length);
            return checked((int)bytesRead);
        }
    }

    /// <summary>
    /// Writes <paramref name="buffer"/> to the file.
    /// </summary>
    /// <param name="buffer">The data to write to the file.</param>
    public unsafe void StoreBuffer(ReadOnlySpan<byte> buffer)
    {
        if (buffer.IsEmpty)
        {
            return;
        }

        fixed (byte* ptr = buffer)
        {
            GodotBridge.GDExtensionInterface.file_access_store_buffer(
                (void*)GetNativePtr(this),
                ptr,
                (uint)buffer.Length);
        }
    }

    /// <summary>
    /// Opens a file at <paramref name="path"/>, reads all lines lazily using
    /// UTF-8 encoding, and closes the file when enumeration is complete.
    /// </summary>
    /// <param name="path">The file path to read from.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="path"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="IOException">
    /// An I/O error occurs while opening or reading the file.
    /// </exception>
    public static IEnumerable<string> ReadLines(string path)
    {
        return ReadLines(path, Encoding.UTF8);
    }

    /// <summary>
    /// Opens a file at <paramref name="path"/>, reads all lines lazily using
    /// the specified <paramref name="encoding"/>, and closes the file when
    /// enumeration is complete.
    /// </summary>
    /// <param name="path">The file path to read from.</param>
    /// <param name="encoding">The text encoding to use when reading lines.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="path"/> or <paramref name="encoding"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="IOException">
    /// An I/O error occurs while opening or reading the file.
    /// </exception>
    public static IEnumerable<string> ReadLines(string path, Encoding encoding)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(encoding);
        return ReadLinesCore(path, encoding);
    }

    private static IEnumerable<string> ReadLinesCore(string path, Encoding encoding)
    {
        using var reader = new StreamReader(GodotFileStream.Open(path, ModeFlags.Read), encoding);
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            yield return line;
        }
    }
}
