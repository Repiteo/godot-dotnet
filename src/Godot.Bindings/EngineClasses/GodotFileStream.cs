using System;
using System.IO;
using Godot.Collections;

namespace Godot;

/// <summary>
/// Provides a <see cref="Stream"/> implementation backed by Godot's <see cref="FileAccess"/> API.
/// </summary>
/// <example>
/// Using a Godot path with <c>System.Text.Json</c>:
/// <code>
/// using var stream = GodotFileStream.OpenRead("res://enemy_stats.json");
/// var data = JsonSerializer.Deserialize&lt;EnemyStats&gt;(stream);
/// </code>
/// </example>
public sealed class GodotFileStream : Stream
{
    private readonly FileAccess _fileAccess;
    private readonly FileAccess.ModeFlags _modeFlags;
    private bool _disposed;

    private GodotFileStream(FileAccess fileAccess, FileAccess.ModeFlags modeFlags)
    {
        _fileAccess = fileAccess;
        _modeFlags = modeFlags;
    }

    /// <summary>
    /// Opens a file at the given Godot path with the specified mode flags and returns
    /// a <see cref="GodotFileStream"/>.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <param name="modeFlags">The mode to open the file in.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <exception cref="IOException">
    /// The file could not be opened.
    /// </exception>
    /// <returns>A <see cref="GodotFileStream"/> for the specified path.</returns>
    public static GodotFileStream Open(string path, FileAccess.ModeFlags modeFlags)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);

        var fileAccess = FileAccess.Open(path, modeFlags);
        if (fileAccess is null)
        {
            var error = FileAccess.GetOpenError();
            throw new IOException(SR.FormatIO_FileOpenFailed(path, error));
        }

        return new GodotFileStream(fileAccess, modeFlags);
    }

    /// <summary>
    /// Opens a file at the given Godot path for reading and returns a read-only
    /// <see cref="GodotFileStream"/>.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <exception cref="IOException">
    /// The file could not be opened.
    /// </exception>
    /// <returns>A read-only <see cref="GodotFileStream"/> for the specified path.</returns>
    public static GodotFileStream OpenRead(string path)
    {
        return Open(path, FileAccess.ModeFlags.Read);
    }

    /// <summary>
    /// Opens or creates a file at the given Godot path for writing and returns
    /// a write-only <see cref="GodotFileStream"/>. The file is created if it does
    /// not exist, and truncated if it does.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <exception cref="IOException">
    /// The file could not be opened.
    /// </exception>
    /// <returns>A write-only <see cref="GodotFileStream"/> for the specified path.</returns>
    public static GodotFileStream OpenWrite(string path)
    {
        return Open(path, FileAccess.ModeFlags.Write);
    }

    /// <summary>
    /// Opens an existing file for reading and returns a <see cref="StreamReader"/> using UTF-8 encoding.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <exception cref="IOException">
    /// The file could not be opened.
    /// </exception>
    /// <returns>A <see cref="StreamReader"/> for the specified path.</returns>
    public static StreamReader OpenText(string path)
    {
        return new StreamReader(Open(path, FileAccess.ModeFlags.Read));
    }

    /// <summary>
    /// Creates or overwrites a file for writing and returns a <see cref="StreamWriter"/> using UTF-8 encoding.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <exception cref="IOException">
    /// The file could not be opened.
    /// </exception>
    /// <returns>A <see cref="StreamWriter"/> for the specified path.</returns>
    public static StreamWriter CreateText(string path)
    {
        return new StreamWriter(Open(path, FileAccess.ModeFlags.Write));
    }

    /// <summary>
    /// Opens a compressed file with the specified mode flags.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <param name="modeFlags">The mode to open the file in.</param>
    /// <param name="compressionMode">The compression mode to use.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <exception cref="IOException">
    /// The file could not be opened.
    /// </exception>
    /// <returns>A <see cref="GodotFileStream"/> for the specified path.</returns>
    public static GodotFileStream OpenCompressed(string path, FileAccess.ModeFlags modeFlags, FileAccess.CompressionMode compressionMode = FileAccess.CompressionMode.Fastlz)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);

        var fileAccess = FileAccess.OpenCompressed(path, modeFlags, compressionMode);
        if (fileAccess is null)
        {
            var error = FileAccess.GetOpenError();
            throw new IOException(SR.FormatIO_FileOpenFailed(path, error));
        }

        return new GodotFileStream(fileAccess, modeFlags);
    }

    /// <summary>
    /// Opens an AES-256 encrypted file with the specified mode flags.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <param name="modeFlags">The mode to open the file in.</param>
    /// <param name="key">The encryption key.</param>
    /// <param name="iv">The initialization vector.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <exception cref="IOException">
    /// The file could not be opened.
    /// </exception>
    /// <returns>A <see cref="GodotFileStream"/> for the specified path.</returns>
    public static GodotFileStream OpenEncrypted(string path, FileAccess.ModeFlags modeFlags, PackedByteArray key, PackedByteArray? iv = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);

        var fileAccess = FileAccess.OpenEncrypted(path, modeFlags, key, iv);
        if (fileAccess is null)
        {
            var error = FileAccess.GetOpenError();
            throw new IOException(SR.FormatIO_FileOpenFailed(path, error));
        }

        return new GodotFileStream(fileAccess, modeFlags);
    }

    /// <summary>
    /// Opens a password-encrypted file with the specified mode flags.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <param name="modeFlags">The mode to open the file in.</param>
    /// <param name="pass">The password to use for encryption.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is <see langword="null"/> or empty.
    /// -or-
    /// <paramref name="pass"/> is <see langword="null"/> or empty.
    /// </exception>
    /// <exception cref="IOException">
    /// The file could not be opened.
    /// </exception>
    /// <returns>A <see cref="GodotFileStream"/> for the specified path.</returns>
    public static GodotFileStream OpenEncryptedWithPass(string path, FileAccess.ModeFlags modeFlags, string pass)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        ArgumentException.ThrowIfNullOrEmpty(pass);

        var fileAccess = FileAccess.OpenEncryptedWithPass(path, modeFlags, pass);
        if (fileAccess is null)
        {
            var error = FileAccess.GetOpenError();
            throw new IOException(SR.FormatIO_FileOpenFailed(path, error));
        }

        return new GodotFileStream(fileAccess, modeFlags);
    }

    /// <inheritdoc/>
    public override bool CanSeek => _fileAccess.IsOpen();

    /// <inheritdoc/>
    public override bool CanRead => _modeFlags.HasFlag(FileAccess.ModeFlags.Read) && _fileAccess.IsOpen();

    /// <inheritdoc/>
    public override bool CanWrite => _modeFlags.HasFlag(FileAccess.ModeFlags.Write) && _fileAccess.IsOpen();

    /// <inheritdoc/>
    public override long Length
    {
        get
        {
            ThrowIfClosed();
            return checked((long)_fileAccess.GetLength());
        }
    }

    /// <inheritdoc/>
    public override long Position
    {
        get
        {
            ThrowIfClosed();
            return checked((long)_fileAccess.GetPosition());
        }
        set
        {
            ThrowIfClosed();
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _fileAccess.Seek((ulong)value);
        }
    }

    /// <summary>
    /// Clears buffers for this stream and causes any buffered data to be
    /// written to the file.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// The stream is closed.
    /// </exception>
    public override void Flush()
    {
        ThrowIfClosed();

        if (CanWrite)
        {
            _fileAccess.Flush();
        }
    }

    /// <summary>
    /// Sets the length of this stream to the given value.
    /// </summary>
    /// <param name="value">The new length of the stream.</param>
    /// <exception cref="ObjectDisposedException">
    /// The stream is closed.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> is negative.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The stream does not support writing.
    /// </exception>
    /// <exception cref="IOException">
    /// An I/O error has occurred.
    /// </exception>
    public override void SetLength(long value)
    {
        ThrowIfClosed();
        ThrowIfCantWrite();
        ArgumentOutOfRangeException.ThrowIfNegative(value);

        var error = _fileAccess.Resize(value);
        if (error != Error.Ok)
        {
            throw new IOException(SR.FormatIO_SetLengthFailed(error));
        }
    }

    /// <summary>
    /// Sets the current position of this stream to the given value.
    /// </summary>
    /// <param name="offset">
    /// The point relative to <paramref name="origin"/> from which to
    /// begin seeking.
    /// </param>
    /// <param name="origin">
    /// Specifies the beginning, the end, or the current position as a
    /// reference point for <paramref name="offset"/>, using a value of
    /// type <see cref="SeekOrigin"/>.
    /// </param>
    /// <returns>The new position in the stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// The stream is closed.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="origin"/> value is invalid.
    /// </exception>
    /// <exception cref="IOException">
    /// An I/O error has occurred.
    /// </exception>
    public override long Seek(long offset, SeekOrigin origin)
    {
        ThrowIfClosed();

        switch (origin)
        {
            case SeekOrigin.Begin:
            {
                if (offset < 0)
                {
                    throw new IOException(SR.IO_SeekBeforeBegin);
                }

                _fileAccess.Seek(checked((ulong)offset));
                break;
            }

            case SeekOrigin.Current:
            {
                long newPosition = Position + offset;
                if (newPosition < 0)
                {
                    throw new IOException(SR.IO_SeekBeforeBegin);
                }

                _fileAccess.Seek(checked((ulong)newPosition));
                break;
            }

            case SeekOrigin.End:
            {
                _fileAccess.SeekEnd(offset);
                break;
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
        }

        return Position;
    }

    /// <summary>
    /// Reads a single byte from the stream and returns it as an integer.
    /// </summary>
    /// <returns>
    /// The byte cast to an <see cref="int"/>, or -1 if at the end of the stream.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// The stream is closed.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The stream does not support reading.
    /// </exception>
    public override int ReadByte()
    {
        ThrowIfClosed();
        ThrowIfCantRead();

        if (_fileAccess.EofReached())
        {
            return -1;
        }

        return _fileAccess.Get8();
    }

    /// <summary>
    /// Reads a block of bytes from the stream and writes the data in a given buffer.
    /// </summary>
    /// <param name="buffer">
    /// When this method returns, contains the specified byte array with the
    /// values between <paramref name="offset"/> and
    /// (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced
    /// by the bytes read from the current source.
    /// </param>
    /// <param name="offset">
    /// The byte offset in <paramref name="buffer"/> at which the read bytes
    /// will be placed.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <returns>
    /// The total number of bytes read into the buffer. This might be less
    /// than the number of bytes requested if that number of bytes are not
    /// currently available, or zero if the end of the stream is reached.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="buffer"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="offset"/> or <paramref name="count"/> is negative.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="offset"/> and <paramref name="count"/> describe an
    /// invalid range in <paramref name="buffer"/>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The stream is closed.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The stream does not support reading.
    /// </exception>
    public override int Read(byte[] buffer, int offset, int count)
    {
        ValidateBufferArguments(buffer, offset, count);

        return Read(buffer.AsSpan(offset, count));
    }

    /// <summary>
    /// Reads a sequence of bytes from the current file stream and advances
    /// the position within the file stream by the number of bytes read.
    /// </summary>
    /// <param name="buffer">
    /// A region of memory. When this method returns, the contents of this
    /// region are replaced by the bytes read from the current file stream.
    /// </param>
    /// <returns>
    /// The total number of bytes read into the buffer. This can be less
    /// than the number of bytes allocated in the buffer if that many bytes
    /// are not currently available, or zero if the end of the stream is reached.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// The stream is closed.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The stream does not support reading.
    /// </exception>
    public override int Read(Span<byte> buffer)
    {
        ThrowIfClosed();
        ThrowIfCantRead();

        return _fileAccess.GetBuffer(buffer);
    }

    /// <summary>
    /// Writes a single byte to the file stream.
    /// </summary>
    /// <param name="value">
    /// The byte to write to the stream.
    /// </param>
    /// <exception cref="ObjectDisposedException">
    /// The stream is closed.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The stream does not support writing.
    /// </exception>
    /// <exception cref="IOException">
    /// An I/O error has occurred.
    /// </exception>
    public override void WriteByte(byte value)
    {
        ThrowIfClosed();
        ThrowIfCantWrite();

        if (!_fileAccess.Store8(value))
        {
            throw new IOException(SR.IO_FileWriteError);
        }
    }

    /// <summary>
    /// Writes a block of bytes to the file stream.
    /// </summary>
    /// <param name="buffer">
    /// The buffer containing data to write to the stream.
    /// </param>
    /// <param name="offset">
    /// The zero-based byte offset in array from which to begin copying
    /// bytes to the stream.
    /// </param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="buffer"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="offset"/> or <paramref name="count"/> is negative.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="offset"/> and <paramref name="count"/> describe an
    /// invalid range in <paramref name="buffer"/>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The stream is closed.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The stream does not support writing.
    /// </exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
        ValidateBufferArguments(buffer, offset, count);

        Write(buffer.AsSpan(offset, count));
    }

    /// <summary>
    /// Writes a sequence of bytes from a read-only span to the current file
    /// stream and advances the current position within this file stream by
    /// the number of bytes written.
    /// </summary>
    /// <param name="buffer">
    /// A region of memory. This method copies the contents of this region
    /// to the current file stream.
    /// </param>
    /// <exception cref="ObjectDisposedException">
    /// The stream is closed.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The stream does not support writing.
    /// </exception>
    public override void Write(ReadOnlySpan<byte> buffer)
    {
        ThrowIfClosed();
        ThrowIfCantWrite();

        _fileAccess.StoreBuffer(buffer);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        _disposed = true;

        if (disposing)
        {
            _fileAccess.Dispose();
        }

        base.Dispose(disposing);
    }

    private void ThrowIfClosed()
    {
        ObjectDisposedException.ThrowIf(_disposed || !_fileAccess.IsOpen(), this);
    }

    private void ThrowIfCantRead()
    {
        if (!_modeFlags.HasFlag(FileAccess.ModeFlags.Read))
        {
            throw new NotSupportedException(SR.NotSupported_UnreadableStream);
        }
    }

    private void ThrowIfCantWrite()
    {
        if (!_modeFlags.HasFlag(FileAccess.ModeFlags.Write))
        {
            throw new NotSupportedException(SR.NotSupported_UnwritableStream);
        }
    }
}
