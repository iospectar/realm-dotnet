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

namespace Realms.Sync.Exceptions
{
    /// <summary>
    /// Callback triggered when an error occurs in a session.
    /// </summary>
    /// <param name="session">
    /// The <see cref="Session"/> where the error occurred.
    /// </param>
    /// <param name="error">
    /// The <see cref="SessionException"/> that was raised by the <see paramref="session"/>.
    /// </param>
    public delegate void SessionErrorCallback(Session session, SessionException error);

    /// <summary>
    /// An exception type that describes a session-level error condition.
    /// </summary>
    public class SessionException : Exception
    {
        internal const string OriginalFilePathKey = "ORIGINAL_FILE_PATH";
        internal const string BackupFilePathKey = "RECOVERY_FILE_PATH";

        /// <summary>
        /// Gets the error code that describes the session error this exception represents.
        /// </summary>
        /// <value>An enum value, providing more detailed information for the cause of the error.</value>
        public ErrorCode ErrorCode { get; }

        internal SessionException(string message, ErrorCode errorCode, Exception innerException = null) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
