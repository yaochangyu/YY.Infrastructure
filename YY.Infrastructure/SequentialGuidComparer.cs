using System;
using System.Collections.Generic;

namespace YY.Infrastructure
{
    /// <summary>
    ///     Class SequentialGuidComparer.
    /// </summary>
    public class SequentialGuidComparer : IComparer<Guid>, IComparer<SequentialGuid>
    {
        /// <summary>
        ///     Compares the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>System.Int32.</returns>
        public int Compare(Guid x, Guid y)
        {
            return SequentialGuid.CompareImplementation(x, y);
        }

        /// <summary>
        ///     Compares the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>System.Int32.</returns>
        public int Compare(SequentialGuid x, SequentialGuid y)
        {
            return SequentialGuid.CompareImplementation(x, y);
        }
    }
}