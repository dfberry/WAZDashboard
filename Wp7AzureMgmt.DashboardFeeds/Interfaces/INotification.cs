// -----------------------------------------------------------------------
// <copyright file="INotification.cs" company="DFBerry">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.DashboardFeeds.Interfaces
{
    using System;

    /// <summary>
    /// Interface for notifications regardless
    /// if they are by mail, trace, or wherever.
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Send notification.
        /// </summary>
        /// <param name="title">title of notification as string</param>
        /// <param name="text">text of notification as string</param>
        void Notify(string title, string text);
    }
}