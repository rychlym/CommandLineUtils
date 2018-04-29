// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using McMaster.Extensions.CommandLineUtils;

public class Attributes
{
    public static int Main(string[] args)
        => CommandLineApplication.Execute<Attributes>(args);

    [Option(Description = "The subject")]
    public string Subject { get; }

    [Option(ShortName = "n")]
    public int Count { get; }

    private void OnExecute()
    {
        var subject = Subject ?? "world";
        for (var i = 0; i < Count; i++)
        {
            Console.WriteLine($"Hello {subject}!");
        }
    }
}
