// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using Xunit;

namespace McMaster.Extensions.CommandLineUtils.Tests
{
    public class ParsingConventionTests
    {
        [ParsingConvention(typeof(OptionsForStringFields))]
        public class Program
        {
            public string _name;

            private void OnExecute() { }
        }

        private class OptionsForStringFields : ICommandLineAppConvention
        {
            public void Apply(ConventionCreationContext context)
            {
                var fields = context.TargetType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                foreach (var field in fields.Where(f => f.FieldType == typeof(string)))
                {
                    var option = context.Application.Option("--" + field.Name.TrimStart('_'), "", CommandOptionType.SingleValue);
                    context.RegisterParsingCompleteAction((instance, _) =>
                    {
                        if (option.HasValue())
                        {
                            field.SetValue(instance, option.Value());
                        }
                    });
                }
            }
        }

        [Fact]
        public void ItAppliesParsingConventions()
        {
            var app = CommandLineParser.ParseArgs<Program>("--name", "my value");
            Assert.Equal("my value", app._name);
        }
    }
}
