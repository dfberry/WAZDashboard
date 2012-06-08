// -----------------------------------------------------------------------
// <copyright file="TestObject.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.Core.Test
{
    using System;

    /// <summary>
    /// Test object of two properties for reflection test.
    /// </summary>
    public class TestObject
    {
        /// <summary>
        /// Gets or sets Property1 just so relection can find it.
        /// </summary>
        public string Property1 { get; set; }

        /// <summary>
        /// Gets or sets Property1 just so relection can find it.
        /// </summary>
        public int Property2 { get; set; }
    }
}
