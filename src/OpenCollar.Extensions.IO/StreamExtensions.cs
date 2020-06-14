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

namespace OpenCollar.Extensions.IO
{
    /// <summary>
    ///     Extensions to the <see cref="Stream" /> class.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        ///     Reads all the data in a stream and returns it as a <see cref="byte" /> array.
        /// </summary>
        /// <param name="source">
        ///     The stream from which to read. If this is <see langword="null" /> an empty array is returned.
        /// </param>
        /// <returns>
        ///     An array containing the data read from the source stream. This will never be <see langword="null" />
        ///     even if the stream is.
        /// </returns>
        [JetBrains.Annotations.NotNull]
        public static byte[] ReadAll([JetBrains.Annotations.CanBeNull] this System.IO.Stream source)
        {
            if(ReferenceEquals(source, null))
            {
                return System.Array.Empty<byte>();
            }

            using(var destination = new System.IO.MemoryStream())
            {
                source.CopyTo(destination);
                return destination.ToArray();
            }
        }
    }
}