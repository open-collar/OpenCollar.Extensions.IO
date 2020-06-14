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

using System.IO;
using System.IO.Compression;

using JetBrains.Annotations;

namespace OpenCollar.Extensions.IO
{
    /// <summary>
    ///     Extension methods that consume or return compressed data.
    /// </summary>
    public static class CompressionExtensions
    {
        /// <summary>
        ///     Decompresses the compressed data given. It is assumed that the data was compressed using the
        ///     <see cref="GetCompressed" /> method (which uses GZIP).
        /// </summary>
        /// <param name="compressedData">
        ///     The compressed data to decompress.
        /// </param>
        /// <returns>
        ///     The data given, decompressed using GZIP, or <see langword="null" /> if
        ///     <paramref name="compressedData" /> was <see langword="null" />.
        /// </returns>
        [ContractAnnotation("null=>null;notnull=>notnull")]
        public static byte[] GetDecompressed([JetBrains.Annotations.CanBeNull] this byte[] compressedData)
        {
            if(ReferenceEquals(compressedData, null))
            {
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
            }

            using(var compressed = new MemoryStream(compressedData))
            {
                using(var decompressing = new GZipStream(compressed, CompressionMode.Decompress, true))
                {
                    return decompressing.ReadAll();
                }
            }
        }

        /// <summary>
        ///     Given a byte array returns an array of bytes containing the same data compress with GZIP.
        /// </summary>
        /// <param name="data">
        ///     The bytes to compress.
        /// </param>
        /// <returns>
        ///     The data given, compressed using GZIP, or <see langword="null" /> if <paramref name="data" /> was <see langword="null" />.
        /// </returns>
        [ContractAnnotation("null=>null;notnull=>notnull")]
        public static byte[] GetCompressed([JetBrains.Annotations.CanBeNull] this byte[] data)
        {
            if(ReferenceEquals(data, null))
            {
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
            }

            using(var compressed = new MemoryStream())
            {
                using(var compressing = new GZipStream(compressed, CompressionLevel.Optimal, true))
                {
                    compressing.Write(data, 0, data.Length);
                    compressing.Flush();
                    return compressed.ToArray();
                }
            }
        }
    }
}