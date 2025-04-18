﻿// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using Automation.Configuration;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace Automation.Utilities.Attributes
{
    /// <summary>
    /// Specifies that a test method should be rerun on failure/error up to the
    /// maximum number of times specified in <see cref="Settings.RetryCount"/>
    /// </summary>
    /// <remarks>
    /// For UI tests, it was needed to Retry on any kind of exception => Execute method had to be modified.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class UIRetryAttribute : NUnitAttribute, IRepeatTest
    {
        private readonly int _tryCount;

        /// <summary>
        /// Construct a <see cref="UIRetryAttribute" />
        /// </summary>
        public UIRetryAttribute()
        {
            _tryCount = Settings.RetryCount;
        }

        #region IRepeatTest Members

        /// <summary>
        /// Wrap a command and return the result.
        /// </summary>
        /// <param name="command">The command to be wrapped</param>
        /// <returns>The wrapped command</returns>
        public TestCommand Wrap(TestCommand command)
        {
            return new RetryCommand(command, _tryCount);
        }

        #endregion

        #region Nested RetryCommand Class

        /// <summary>
        /// The test command for the <see cref="UIRetryAttribute"/>
        /// </summary>
        public class RetryCommand : DelegatingTestCommand
        {
            private readonly int _tryCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="RetryCommand"/> class.
            /// </summary>
            /// <param name="innerCommand">The inner command.</param>
            /// <param name="tryCount">The maximum number of repetitions</param>
            public RetryCommand(TestCommand innerCommand, int tryCount)
                : base(innerCommand)
            {
                _tryCount = tryCount;
            }

            /// <summary>
            /// Runs the test, saving a TestResult in the supplied TestExecutionContext.
            /// </summary>
            /// <param name="context">The context in which the test should run.</param>
            /// <returns>A TestResult</returns>
            public override TestResult Execute(TestExecutionContext context)
            {
                int count = _tryCount;

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
                        if (context.CurrentResult is null)
                            context.CurrentResult = context.CurrentTest.MakeTestResult();
                        context.CurrentResult.RecordException(ex);
                    }

                    // Original code
                    /*
                    if (context.CurrentResult.ResultState != ResultState.Failure)
                        break;
                    */

                    // Modified code
                    var results = context.CurrentResult.ResultState;
                    if (results != ResultState.Failure
                        && results != ResultState.Error
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
}
