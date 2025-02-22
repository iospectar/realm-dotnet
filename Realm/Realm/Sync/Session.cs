////////////////////////////////////////////////////////////////////////////
//
// Copyright 2016 Realm Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Realms.Sync
{
    /// <summary>
    /// An object encapsulating a synchronization session. Sessions represent the communication between the client (and a local Realm file on disk),
    /// and the server (and a remote Realm served by a MongoDB Realm Server). Sessions are always created by the SDK and vended
    /// out through various APIs. The lifespans of sessions associated with Realms are managed automatically.
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Triggered when an error occurs on a session. The <c>sender</c> argument will be the session which has errored.
        /// </summary>
        public static event EventHandler<ErrorEventArgs> Error;

        internal bool IsClosed => _handle.IsClosed;

        /// <summary>
        /// Gets the session’s current state.
        /// </summary>
        /// <value>An enum value indicating the state of the session.</value>
        public SessionState State => Handle.GetState();

        /// <summary>
        /// Gets the <see cref="User"/> defined by the <see cref="SyncConfigurationBase"/> that is used to connect to MongoDB Realm.
        /// </summary>
        /// <value>The <see cref="User"/> that was used to create the <see cref="Realm"/>'s <see cref="SyncConfigurationBase"/>.</value>
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "The User instance will own its handle.")]
        public User User => Handle.TryGetUser(out var userHandle) ? new User(userHandle) : null;

        /// <summary>
        /// Gets the on-disk path of the Realm file backing the <see cref="Realm"/> this Session represents.
        /// </summary>
        /// <value>The file path.</value>
        public string Path => Handle.GetPath();

        /// <summary>
        /// Gets an <see cref="IObservable{T}"/> that can be used to track upload or download progress.
        /// </summary>
        /// <remarks>
        /// To start receiving notifications, you should call <see cref="IObservable{T}.Subscribe"/> on the returned object.
        /// The token returned from <see cref="IObservable{T}.Subscribe"/> should be retained as long as progress
        /// notifications are desired. To stop receiving notifications, call <see cref="IDisposable.Dispose"/>
        /// on the token.
        /// You don't need to keep a reference to the observable itself.
        /// The progress callback will always be called once immediately upon subscribing in order to provide
        /// the latest available status information.
        /// </remarks>
        /// <returns>An observable that you can subscribe to and receive progress updates.</returns>
        /// <param name="direction">The transfer direction (upload or download) to track in the subscription callback.</param>
        /// <param name="mode">The desired behavior of this progress notification block.</param>
        /// <example>
        /// <code>
        /// class ProgressNotifyingViewModel
        /// {
        ///     private IDisposable notificationToken;
        ///
        ///     public void ShowProgress()
        ///     {
        ///         var observable = session.GetProgressObservable(ProgressDirection.Upload, ProgressMode.ReportIndefinitely);
        ///         notificationToken = observable.Subscribe(progress =>
        ///         {
        ///             // Update relevant properties by accessing
        ///             // progress.TransferredBytes and progress.TransferableBytes
        ///         });
        ///     }
        ///
        ///     public void HideProgress()
        ///     {
        ///         notificationToken?.Dispose();
        ///         notificationToken = null;
        ///     }
        /// }
        /// </code>
        /// In this example we're using <see href="https://msdn.microsoft.com/en-us/library/ff402849(v=vs.103).aspx">ObservableExtensions.Subscribe</see>
        /// found in the <see href="https://github.com/Reactive-Extensions/Rx.NET">Reactive Extensions</see> class library.
        /// If you prefer not to take a dependency on it, you can create a class that implements <see cref="IObserver{T}"/>
        /// and use it to subscribe instead.
        /// </example>
        public IObservable<SyncProgress> GetProgressObservable(ProgressDirection direction, ProgressMode mode) => new SyncProgressObservable(Handle, direction, mode);

        /// <summary>
        /// Waits for the <see cref="Session"/> to finish all pending uploads.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/> that will be completed when all pending uploads for this <see cref="Session"/> are completed.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a faulted session is waited on.</exception>
        public Task WaitForUploadAsync() => Handle.WaitAsync(ProgressDirection.Upload);

        /// <summary>
        /// Waits for the <see cref="Session"/> to finish all pending downloads.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/> that will be completed when all pending downloads for this <see cref="Session"/> are completed.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a faulted session is waited on.</exception>
        public Task WaitForDownloadAsync() => Handle.WaitAsync(ProgressDirection.Download);

        /// <summary>
        /// Stops any synchronization with the server until the Realm is re-opened again
        /// after fully closing it.
        /// <br/>
        /// Synchronization can be re-enabled by calling <see cref="Start"/> again.
        /// </summary>
        /// <remarks>
        /// If the session is already stopped, calling this method will do nothing.
        /// </remarks>
        public void Stop() => Handle.Stop();

        /// <summary>
        /// Attempts to resume the session and enable synchronization with the server.
        /// </summary>
        /// <remarks>
        /// All sessions will be active by default and calling this method only makes sense if
        /// <see cref="Stop"/> was called before that.
        /// </remarks>
        public void Start() => Handle.Start();

        private readonly SessionHandle _handle;

        private SessionHandle Handle
        {
            get
            {
                if (_handle.IsClosed)
                {
                    throw new ObjectDisposedException(
                        nameof(Session),
                        "This Session instance is invalid. This typically means that Sync has closed or otherwise invalidated the native session. You can get a new valid instance by calling realm.GetSession().");
                }

                return _handle;
            }
        }

        internal Session(SessionHandle handle)
        {
            _handle = handle;
        }

        internal static void RaiseError(Session session, Exception error)
        {
            var args = new ErrorEventArgs(error);
            Error?.Invoke(session, args);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => obj is Session other &&
               Handle.GetRawPointer() == other.Handle.GetRawPointer();

        /// <inheritdoc/>
        public override int GetHashCode() => Handle.GetRawPointer().GetHashCode();

        internal void CloseHandle(bool waitForShutdown = false)
        {
            GC.SuppressFinalize(this);
            if (!IsClosed)
            {
                if (waitForShutdown)
                {
                    _handle.ShutdownAndWait();
                }

                _handle.Close();
            }
        }

        internal void ReportErrorForTesting(int errorCode, string errorMessage, bool isFatal) => Handle.ReportErrorForTesting(errorCode, errorMessage, isFatal);
    }
}
