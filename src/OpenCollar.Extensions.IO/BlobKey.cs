/*
 * This file is part of OpenCollar.Extensions.IO.
 *
 * OpenCollar.Extensions.IO is free software: you can redistribute it
 * and/or modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * OpenCollar.Extensions.IO is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public
 * License for more details.
 *
 * You should have received a copy of the GNU General Public License along with
 * OpenCollar.Extensions.IO.  If not, see <https://www.gnu.org/licenses/>.
 *
 * Copyright © 2019-2020 Jonathan Evans (jevans@open-collar.org.uk).
 */

using System;
using System.IO;
using System.Security.Cryptography;

namespace OpenCollar.Extensions.IO
{
    /// <summary>
    ///     A class that represents a key for large binary objects in a compact and compute efficient way. This class
    ///     cannot be inherited.
    /// </summary>
    public sealed class BlobKey : IEquatable<BlobKey>, IComparable<BlobKey>, IComparable
    {
        /// <summary>
        ///     The hash code for the blob (as a base-64 string)
        /// </summary>
        [JetBrains.Annotations.CanBeNull]
        private readonly string _hash;

        /// <summary>
        ///     The exact length of the blob (in bytes).
        /// </summary>
        private readonly long _length;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BlobKey" /> class.
        /// </summary>
        /// <param name="blob">
        ///     A byte array containing the binary data to be represented.
        /// </param>
        public BlobKey([JetBrains.Annotations.CanBeNull] byte[] blob)
        {
            if(ReferenceEquals(blob, null))
            {
                _length = -1;
                _hash = null;
                return;
            }

            _length = blob.LongLength;
            using(var hashProvider = new SHA1CryptoServiceProvider())
            {
                _hash = Convert.ToBase64String(hashProvider.ComputeHash(blob));
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BlobKey" /> class.
        /// </summary>
        /// <param name="blob">
        ///     A stream containing the binary data to be represented. If the stream supports seek then this will be
        ///     unchanged following construction.
        /// </param>
        public BlobKey([JetBrains.Annotations.CanBeNull] Stream blob)
        {
            if(ReferenceEquals(blob, null))
            {
                _length = -1;
                _hash = null;
                return;
            }

            long position = 0;
            var restorePosition = blob.CanSeek;
            if(restorePosition)
                position = blob.Position;
            try
            {
                using(var hashProvider = new SHA1CryptoServiceProvider())
                {
                    _hash = Convert.ToBase64String(hashProvider.ComputeHash(blob));
                }
                _length = blob.Length;
            }
            finally
            {
                if(restorePosition)
                    blob.Seek(position, SeekOrigin.Begin);
            }
        }

        /// <summary>
        ///     Implements the inequality operator.
        /// </summary>
        /// <param name="left">
        ///     The left operand.
        /// </param>
        /// <param name="right">
        ///     The right operand.
        /// </param>
        /// <returns>
        ///     The result of the operation.
        /// </returns>
        public static bool operator !=(BlobKey left, BlobKey right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Implements the less-than operator.
        /// </summary>
        /// <param name="left">
        ///     The left operand.
        /// </param>
        /// <param name="right">
        ///     The right operand.
        /// </param>
        /// <returns>
        ///     The result of the operation.
        /// </returns>
        public static bool operator <(BlobKey left, BlobKey right)
        {
            if(ReferenceEquals(left, right))
                return false;

            if(ReferenceEquals(left, null))
                return false;

            return left.CompareTo(right) < 0;
        }

        /// <summary>
        ///     Implements the less-than-or-equals operator.
        /// </summary>
        /// <param name="left">
        ///     The left operand.
        /// </param>
        /// <param name="right">
        ///     The right operand.
        /// </param>
        /// <returns>
        ///     The result of the operation.
        /// </returns>
        public static bool operator <=(BlobKey left, BlobKey right)
        {
            if(ReferenceEquals(left, right))
                return true;

            if(ReferenceEquals(left, null))
                return false;

            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        ///     Implements the equality operator.
        /// </summary>
        /// <param name="left">
        ///     The left operand.
        /// </param>
        /// <param name="right">
        ///     The right operand.
        /// </param>
        /// <returns>
        ///     The result of the operation.
        /// </returns>
        public static bool operator ==(BlobKey left, BlobKey right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Implements the greater-than operator.
        /// </summary>
        /// <param name="left">
        ///     The left operand.
        /// </param>
        /// <param name="right">
        ///     The right operand.
        /// </param>
        /// <returns>
        ///     The result of the operation.
        /// </returns>
        public static bool operator >(BlobKey left, BlobKey right)
        {
            if(ReferenceEquals(left, right))
                return false;

            if(ReferenceEquals(left, null))
                return false;

            return left.CompareTo(right) > 0;
        }

        /// <summary>
        ///     Implements the greater-than-or-equals operator.
        /// </summary>
        /// <param name="left">
        ///     The left operand.
        /// </param>
        /// <param name="right">
        ///     The right operand.
        /// </param>
        /// <returns>
        ///     The result of the operation.
        /// </returns>
        public static bool operator >=(BlobKey left, BlobKey right)
        {
            if(ReferenceEquals(left, right))
                return false;

            if(ReferenceEquals(left, null))
                return true;

            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        ///     Compares the current instance with another object of the same type and returns an integer that indicates
        ///     whether the current instance precedes, follows, or occurs in the same position in the sort order as the
        ///     other object.
        /// </summary>
        /// <returns>
        ///     A value that indicates the relative order of the objects being compared. The return value has these
        ///     meanings: Value Meaning Less than zero This instance is less than <paramref name="obj" />. Zero This
        ///     instance is equal to <paramref name="obj" />. Greater than zero This instance is greater than <paramref name="obj" />.
        /// </returns>
        /// <param name="obj">
        ///     An object to compare with this instance.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="obj" /> is not the same type as this instance.
        /// </exception>
        public int CompareTo(object obj)
        {
            if(ReferenceEquals(obj, null))
                return 1;

            var other = obj as BlobKey;

            if(ReferenceEquals(other, null))
                throw new ArgumentException(@"'obj'  is not the same type as this instance.", nameof(obj));

            return CompareTo(other);
        }

        /// <summary>
        ///     Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        ///     A value that indicates the relative order of the objects being compared. The return value has the following
        ///     meanings: Value Meaning Less than zero This object is less than the <paramref name="other" />
        ///     parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is
        ///     greater than <paramref name="other" />.
        /// </returns>
        /// <param name="other">
        ///     An object to compare with this object.
        /// </param>
        public int CompareTo(BlobKey other)
        {
            if(ReferenceEquals(other, null))
                return 1;

            if(ReferenceEquals(other, this))
                return 0;

            var c = _length.CompareTo(other._length);
            if(c != 0)
                return c;

            return string.CompareOrdinal(_hash, other._hash);
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter;
        ///     otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="other">
        ///     An object to compare with this object.
        /// </param>
        public bool Equals(BlobKey other)
        {
            if(ReferenceEquals(null, other))
                return false;
            if(ReferenceEquals(this, other))
                return true;
            return (_length == other._length) && string.Equals(_hash, other._hash, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="T:System.Object" />; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="obj">
        ///     The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.
        /// </param>
        public override bool Equals(object obj)
        {
            return Equals(obj as BlobKey);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_length.GetHashCode() * 397) ^ (_hash != null ? _hash.GetHashCode() : 0);
            }
        }
    }
}