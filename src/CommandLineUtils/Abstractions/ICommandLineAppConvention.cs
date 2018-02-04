// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace McMaster.Extensions.CommandLineUtils.Abstractions
{
    /// <summary>
    /// Defines a convention that maps a CLR type to <see cref="CommandLineApplication"/>.
    /// </summary>
    public interface ICommandLineAppConvention
    {
        /// <summary>
        /// Apply the convention to the creation of the <see cref="CommandLineApplication"/>.
        /// </summary>
        /// <param name="context">The context in which the convention is created.</param>
        void Apply(ConventionCreationContext context);
    }
}
