using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace Attrition
{
    /// <summary>
    /// Provides a menu item to run all tests in Edit Mode followed by Play Mode.
    /// </summary>
    public static class RunAllTestsMenu
    {
        /// <summary>
        /// Executes all tests in the project, starting with Edit Mode tests and then Play Mode tests.
        /// </summary>
        [MenuItem("Tools/Run All Tests")]
        public static void RunAllTests()
        {
            var api = ScriptableObject.CreateInstance<TestRunnerApi>();

            Debug.Log("Starting Edit Mode tests...");

            var callbacks = new TestRunCallbacks(
                onComplete: editModeResult =>
                {
                    LogTestResults("Edit Mode", editModeResult);
                    Debug.Log("Starting Play Mode tests...");
                    RunTests(api, TestMode.PlayMode, "Play Mode");
                },
                onError: error => Debug.LogError($"Edit Mode tests encountered an error: {error}")
            );

            api.RegisterCallbacks(callbacks);
            RunTests(api, TestMode.EditMode, "Edit Mode");
        }

        /// <summary>
        /// Executes tests for the specified mode.
        /// </summary>
        /// <param name="api">The TestRunner API instance used to execute tests.</param>
        /// <param name="mode">The test mode to execute (Edit Mode or Play Mode).</param>
        /// <param name="modeName">A descriptive name for the test mode.</param>
        private static void RunTests(TestRunnerApi api, TestMode mode, string modeName)
        {
            var filter = new Filter { testMode = mode };
            Debug.Log($"Running {modeName} tests...");
            api.Execute(new(filter));
        }

        /// <summary>
        /// Logs the results of a test run to the console.
        /// </summary>
        /// <param name="modeName">The name of the test mode (e.g., Edit Mode or Play Mode).</param>
        /// <param name="result">The result of the test run.</param>
        private static void LogTestResults(string modeName, ITestResultAdaptor result)
        {
            if (result != null)
            {
                Debug.Log($"{modeName} tests completed: {result.AssertCount} tests run, {result.PassCount} passed, {result.FailCount} failed.");
            }
        }

        /// <summary>
        /// Handles callbacks during a test run.
        /// </summary>
        private class TestRunCallbacks : ICallbacks
        {
            private readonly System.Action<ITestResultAdaptor> onComplete;
            private readonly System.Action<string> onError;

            /// <summary>
            /// Initializes a new instance of the <see cref="TestRunCallbacks"/> class.
            /// </summary>
            /// <param name="onComplete">Callback invoked when a test run completes successfully.</param>
            /// <param name="onError">Callback invoked when a test run encounters an error.</param>
            public TestRunCallbacks(System.Action<ITestResultAdaptor> onComplete, System.Action<string> onError)
            {
                this.onComplete = onComplete;
                this.onError = onError;
            }

            /// <summary>
            /// Invoked when a test run starts.
            /// </summary>
            /// <param name="testsToRun">The tests being executed.</param>
            public void RunStarted(ITestAdaptor testsToRun)
            {
                Debug.Log($"Test run started: {testsToRun.Name}");
            }

            /// <summary>
            /// Invoked when a test run finishes.
            /// </summary>
            /// <param name="result">The result of the test run.</param>
            public void RunFinished(ITestResultAdaptor result)
            {
                if (result != null && result.TestStatus == TestStatus.Failed)
                {
                    onError?.Invoke($"Some tests failed: {result.Name}");
                }
                else
                {
                    onComplete?.Invoke(result);
                }
            }

            /// <summary>
            /// Invoked when an individual test starts.
            /// </summary>
            /// <param name="test">The test being executed.</param>
            public void TestStarted(ITestAdaptor test)
            {
                Debug.Log($"Running test: {test.Name}");
            }

            /// <summary>
            /// Invoked when an individual test finishes.
            /// </summary>
            /// <param name="result">The result of the test.</param>
            public void TestFinished(ITestResultAdaptor result)
            {
                Debug.Log($"Test finished: {result.Name} - {result.TestStatus}");
            }
        }
    }
}
