// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace McMaster.Extensions.CommandLineUtils.Abstractions
{
    /// <summary>
    /// Represents the context in which a <see cref="ICommandLineAppConvention"/> is being applied.
    /// </summary>
    /// <remarks>
    /// The lifecycle of a reflection-based application are as follows:
    /// <list type="number">
    ///     <item>
    ///         <see cref="CommandLineApplication.Execute{TApp}(string[])"/> is invoked.
    ///     </item>
    ///     <item>
    ///         An instance of <see cref="TargetType"/> is created.
    ///         <see cref="RegisterTargetTypeInitializedAction(TargetTypeInitializedAction)"/> handlers are invoked.
    ///     </item>
    ///     <item>
    ///         Arguments are processed and applied to <see cref="Application"/>.
    ///         <see cref="RegisterParsingCompleteAction(ParsingCompleteAction)"/> handlers are invoked.
    ///     </item>
    ///     <item>
    ///         Validation is applied using <see cref="CommandLineApplication.GetValidationResult"/>.
    ///     </item>
    ///     <item>
    ///         If validation passes, the OnExecute method on <see cref="TargetType"/> is invoked.
    ///     </item>
    /// </list>
    /// </remarks>
    public class ConventionCreationContext
    {
        private readonly List<TargetTypeInitializedAction> _initializeHandlers = new List<TargetTypeInitializedAction>();
        private readonly List<ParsingCompleteAction> _beforeExecute = new List<ParsingCompleteAction>();

        /// <summary>
        /// The underlying parsing model.
        /// </summary>
        public CommandLineApplication Application { get; internal set; }

        /// <summary>
        /// The type to which the convention is being applied.
        /// </summary>
        public Type TargetType { get; internal set; }

        /// <summary>
        /// Handle an initialized instance of <see cref="TargetType"/> before string[] arguments have been processed.
        /// </summary>
        /// <param name="target">The instance.</param>
        public delegate void TargetTypeInitializedAction(object target);

        /// <summary>
        /// Add a callback when the <see cref="TargetType"/> is initialized.
        /// This type is always initialized before parsing arguments.
        /// </summary>
        /// <param name="action">A callback that recevies an instance of <see cref="TargetType"/>.</param>
        public void RegisterTargetTypeInitializedAction(TargetTypeInitializedAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _initializeHandlers.Add(action);
        }

        /// <summary>
        /// A callback received after the arguments have been processed and applied to <see cref="Application"/>,
        /// and before the "OnExecute" method is invoked on <paramref name="target" />.
        /// </summary>
        /// <param name="target">An instance of <see cref="TargetType"/>.</param>
        /// <param name="executionContext">The context in which the target type will be executed.</param>
        public delegate void ParsingCompleteAction(object target, CommandLineContext executionContext);

        /// <summary>
        /// Add a callback that is invoked after string arguments have been processed
        /// and applied to <see cref="Application"/>, but before the "OnExecute" method is invoked.
        /// </summary>
        /// <param name="action">A callback that receives the initalized target and the original string arguments.</param>
        public void RegisterParsingCompleteAction(ParsingCompleteAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _beforeExecute.Add(action);
        }

        internal void TargetTypeInitialized(object target)
        {
            foreach (var callback in _initializeHandlers)
            {
                callback(target);
            }
        }

        internal void ParsingComplete(object target, CommandLineContext context)
        {
            foreach (var callback in _beforeExecute)
            {
                callback(target, context);
            }
        }
    }
}
