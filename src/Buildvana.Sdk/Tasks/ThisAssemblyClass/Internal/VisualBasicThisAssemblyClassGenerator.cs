﻿// -----------------------------------------------------------------------------------
// Copyright (C) Riccardo De Agostini and Buildvana contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// -----------------------------------------------------------------------------------

using System.Globalization;
using Buildvana.Sdk.Tasks.Internal;

namespace Buildvana.Sdk.Tasks.ThisAssemblyClass.Internal
{
    internal sealed class VisualBasicThisAssemblyClassGenerator : ThisAssemblyClassGeneratorBase<VisualBasicCodeBuilder>
    {
        protected override void BeginNamespace(string name)
        {
            NewLine("Namespace Global");
            if (!StringUtility.IsNullOrEmpty(name))
            {
                Text('.');
                Text(name);
            }
        }

        protected override void EndNamespace(string name) => NewLine("End Namespace");

        protected override void BeginInternalStaticClass(string name)
        {
            NewLine("Friend NotInheritable Partial Class ");
            Text(name);
            NewLine("Public Sub New()");
            NewLine("End Sub");
        }

        protected override void EndInternalStaticClass(string name) => NewLine("End Class");

        protected override void PublicConstant(string name, object value)
        {
            var type = value.GetType();
            NewLine("Public Const ");
            Text(name);
            Text(" As ");
            Text(type.FullName!);
            Text(" = ");
            switch (value)
            {
                case string stringValue:
                    QuotedText(stringValue);
                    break;
                case bool boolValue:
                    Text(boolValue ? "True" : "False");
                    break;
                case long longValue:
                    Text(longValue.ToString(CultureInfo.InvariantCulture));
                    Text('L');
                    break;
                default:
                    Text(value.ToString());
                    break;
            }

            Text(';');
        }
    }
}