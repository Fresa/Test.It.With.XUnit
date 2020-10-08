# Test.It.With.XUnit
Provides test specifications for XUnit in BDD style.

![Continuous Delivery](https://github.com/Fresa/Test.It.With.XUnit/workflows/Continuous%20Delivery/badge.svg)

## Download
https://www.nuget.org/packages/Test.It.With.XUnit/

## Getting Started
Let the XUnit 2.\* test class inherit `XUnit2Specification`.

If you want anything written through `Console` and `Trace` to show up in the test output, you need to add XUnit's `ITestOutputHelper` in the constructor of the test class and forward it to `XUnit2Specification` which will setup output. More info about the changes regarding output in XUnit 2.\* can be read here: https://xunit.github.io/docs/capturing-output.html.
