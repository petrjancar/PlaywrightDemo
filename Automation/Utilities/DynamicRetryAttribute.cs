// ***********************************************************************
// Copyright (c) 2018 Charlie Poole, Rob Prouse
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using Automation.Configuration;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace Automation.Utilities;

/// <summary>
/// Specifies that a test method should be rerun on failure up to the specified 
/// maximum number of times.
/// </summary>
/// <remarks>For retry to work with any kind of Exception, the Execute method had to be modified. Hence this copied file.</remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class DynamicRetryAttribute : NUnitAttribute, IRepeatTest
{
    private readonly int tryCount;

    /// <summary>
    /// Construct a <see cref="DynamicRetryAttribute" />
    /// </summary>
    public DynamicRetryAttribute()
    {
        tryCount = Settings.RetryCount;
    }

    #region IRepeatTest Members

    /// <summary>
    /// Wrap a command and return the result.
    /// </summary>
    /// <param name="command">The command to be wrapped</param>
    /// <returns>The wrapped command</returns>
    public TestCommand Wrap(TestCommand command)
    {
        return new RetryCommand(command, tryCount);
    }

    #endregion

    #region Nested RetryCommand Class

    /// <summary>
    /// The test command for the <see cref="DynamicRetryAttribute"/>
    /// </summary>
    public class RetryCommand : DelegatingTestCommand
    {
        private readonly int tryCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryCommand"/> class.
        /// </summary>
        /// <param name="innerCommand">The inner command.</param>
        /// <param name="tryCount">The maximum number of repetitions</param>
        public RetryCommand(TestCommand innerCommand, int tryCount)
            : base(innerCommand)
        {
            this.tryCount = tryCount;
        }

        /// <summary>
        /// Runs the test, saving a TestResult in the supplied TestExecutionContext.
        /// </summary>
        /// <param name="context">The context in which the test should run.</param>
        /// <returns>A TestResult</returns>
        public override TestResult Execute(TestExecutionContext context)
        {
            int count = tryCount;

            while (count-- > 0)
            {
                try
                {
                    context.CurrentResult = innerCommand.Execute(context);
                }
                // Commands are supposed to catch exceptions, but some don't
                // and we want to look at restructuring the API in the future.
                catch (Exception ex)
                {
                    if (context.CurrentResult == null) context.CurrentResult = context.CurrentTest.MakeTestResult();
                    context.CurrentResult.RecordException(ex);
                }

                // This change was necessary for retry to happen on any kind of Exception
                var results = context.CurrentResult.ResultState;
                if (results != ResultState.Error
                    && results != ResultState.Failure
                    && results != ResultState.SetUpError
                    && results != ResultState.SetUpFailure
                    && results != ResultState.TearDownError
                    && results != ResultState.ChildFailure)
                    break;

                // Clear result for retry
                if (count > 0)
                {
                    context.CurrentResult = context.CurrentTest.MakeTestResult();
                    context.CurrentRepeatCount++; // increment Retry count for next iteration. will only happen if we are guaranteed another iteration
                }
            }

            return context.CurrentResult;
        }
    }

    #endregion
}
