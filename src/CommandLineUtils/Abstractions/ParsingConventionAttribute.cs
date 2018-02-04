// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace McMaster.Extensions.CommandLineUtils.Abstractions
{
    /// <summary>
    /// Allows extending <see cref="CommandLineApplication.Execute{TApp}(string[])"/> with custom conventions for
    /// how the type is parsed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ParsingConventionAttribute : Attribute, ICommandLineAppConvention
    {
        /// <summary>
        /// Initializes an instance of <see cref="ParsingConventionAttribute"/> with the type of a convention.
        /// </summary>
        /// <param name="conventionType">This type should have a parameterless constructor and implement <see cref="ICommandLineAppConvention"/>.</param>
        public ParsingConventionAttribute(Type conventionType)
        {
            ConventionType = conventionType ?? throw new ArgumentNullException(nameof(conventionType));
        }

        internal Type ConventionType { get; }

        void ICommandLineAppConvention.Apply(ConventionCreationContext context)
        {
            var convention = (ICommandLineAppConvention)Activator.CreateInstance(ConventionType);
            convention.Apply(context);
        }
    }
}
