﻿// -----------------------------------------------------------------------------------
// Copyright (C) Riccardo De Agostini and Buildvana contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// -----------------------------------------------------------------------------------

using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Buildvana.Sdk.Tasks.Internal;
using Buildvana.Sdk.Tasks.Resources;
using JetBrains.Annotations;
using Microsoft.Build.Framework;

namespace Buildvana.Sdk.Tasks.AssemblySigning
{
    public sealed class ConvertPfxToSnk : BuildvanaSdkTask
    {
        [PublicAPI]
        [Required]
        public string? PfxPath { get; set; }

        [PublicAPI]
        [Required]
        public string? PfxPassword { get; set; }

        [PublicAPI]
        [Required]
        public string? OutputPath { get; set; }

        protected override void Run()
        {
            if (StringUtility.IsNullOrEmpty(PfxPath))
            {
                throw new BuildErrorException(Strings.MissingParameterFmt, nameof(PfxPath));
            }

            if (StringUtility.IsNullOrEmpty(PfxPassword))
            {
                throw new BuildErrorException(Strings.MissingParameterFmt, nameof(PfxPassword));
            }

            if (StringUtility.IsNullOrEmpty(OutputPath))
            {
                throw new BuildErrorException(Strings.MissingParameterFmt, nameof(OutputPath));
            }

            using var cert = LoadCertificate(PfxPath, PfxPassword);
            var privateKey = (RSACryptoServiceProvider)cert.PrivateKey;
            var keyBytes = privateKey.ExportCspBlob(true);
            File.WriteAllBytes(OutputPath, keyBytes);
        }

        private static X509Certificate2 LoadCertificate(string path, string password)
        {
            try
            {
                return new X509Certificate2(path, password, X509KeyStorageFlags.Exportable);
            }
            catch (CryptographicException)
            {
                throw new BuildErrorException(Strings.AssemblySigning.CannotExtractKeyFmt, path);
            }
        }
    }
}