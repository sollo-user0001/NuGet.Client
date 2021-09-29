// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using NuGet.Common;

namespace NuGet.Packaging.Core
{
    [Serializable]
    public class PackagingException : Exception, ILogMessageException
    {
        private readonly IPackLogMessage _logMessage;

        public PackagingException(string message)
            : base(message)
        {
            _logMessage = PackagingLogMessage.CreateError(message, NuGetLogCode.NU5000);
        }

        public PackagingException(NuGetLogCode logCode, string message)
            : base(message)
        {
            _logMessage = PackagingLogMessage.CreateError(message, logCode);
        }

        public PackagingException(NuGetLogCode logCode, string message, Exception innerException)
            : base(message, innerException)
        {
            _logMessage = PackagingLogMessage.CreateError(message, logCode);
        }

        public PackagingException(string message, Exception innerException)
            : base(message, innerException)
        {
            _logMessage = PackagingLogMessage.CreateError(message, NuGetLogCode.NU5000);
        }

        public virtual ILogMessage AsLogMessage()
        {
            return _logMessage;
        }

        protected PackagingException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {
            string logCode = serializationInfo.GetString("logCode");
            string message = serializationInfo.GetString("Message");

            _logMessage = PackagingLogMessage.CreateError(message, (NuGetLogCode)Enum.Parse(typeof(NuGetLogCode), logCode));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("logCode", _logMessage.Code.ToString());
        }
    }
}
