﻿// -----------------------------------------------------------------------------------
// Copyright (C) Riccardo De Agostini and Buildvana contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// -----------------------------------------------------------------------------------

using System.Text;
using Buildvana.Sdk.Internal;

namespace Buildvana.Sdk.Tasks.Internal
{
    internal sealed class VisualBasicThisAssemblyClassCompiler : ThisAssemblyClassCompilerBase
    {
        public VisualBasicThisAssemblyClassCompiler(WriteThisAssemblyClass task)
            : base(task)
        {
        }

        protected override void AppendBeforeConstants(StringBuilder sb)
        {
            _ = sb.AppendLine("' <auto-generated/>");

            _ = sb.Append("Namespace Global");
            if (!StringUtility.IsNullOrEmpty(Namespace))
            {
                _ = sb.Append('.').Append(Namespace);
            }

            _ = sb.AppendLine()
                .Append("Friend NotInheritable Partial Class ").AppendLine(ClassName)
                .AppendLine("Public Sub New()").AppendLine("End Sub");
        }

        protected override void AppendAfterConstants(StringBuilder sb)
            => _ = sb.AppendLine("End Class").AppendLine("End Namespace");

        protected override void AppendConstant(StringBuilder sb, (string Name, object Value) constant)
        {
            var type = constant.Value.GetType();
            _ = sb.Append("Public Const ").Append(constant.Name).Append(" As ").Append(type.FullName).Append(" = ");
            _ = type == typeof(string) ? AppendQuoted(sb, (string)constant.Value)
                : type == typeof(long) ? sb.Append(constant.Value).Append('L')
                : sb.Append(constant.Value);

            _ = sb.AppendLine();
        }

        private static StringBuilder AppendQuoted(StringBuilder sb, string str)
        {
            _ = sb.Append('"');
            var length = str.Length;
            if (length > 0)
            {
                var position = 0;
                while (position < length)
                {
                    var nextPosition = str.IndexOf('"', position);
                    if (nextPosition < 0)
                    {
                        _ = sb.Append(str, position, length - position);
                        position = length;
                    }
                    else if (nextPosition == position)
                    {
                        _ = sb.Append("\"\"");
                        position++;
                    }
                    else
                    {
                        _ = sb.Append(str, position, nextPosition - position).Append("\"\"");
                        position = nextPosition + 1;
                    }
                }
            }

            return sb.Append('"');
        }
    }
}