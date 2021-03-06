// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.ReverseProxy.Common.Abstractions.Telemetry;
using Microsoft.ReverseProxy.Common.Util;

namespace Microsoft.ReverseProxy.Common.Telemetry
{
    /// <summary>
    /// Default implementation of <see cref="IOperationLogger"/>
    /// which logs activity start / end events as Information messages.
    /// </summary>
    public class TextOperationLogger : IOperationLogger
    {
        private readonly ILogger<TextOperationLogger> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextOperationLogger"/> class.
        /// </summary>
        /// <param name="logger">Logger where text messages will be logger.</param>
        public TextOperationLogger(ILogger<TextOperationLogger> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        // TODO: Implement this.
        public IOperationContext Context => null;

        /// <inheritdoc/>
        public void Execute(string operationName, Action action)
        {
            var stopwatch = ValueStopwatch.StartNew();
            try
            {
                _logger.LogInformation($"Operation started: {operationName}");
                action();
                _logger.LogInformation($"Operation ended: {operationName}, {stopwatch.Elapsed.TotalMilliseconds:F1}ms, success");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Operation ended: {operationName}, {stopwatch.Elapsed.TotalMilliseconds:F1}ms, error: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public TResult Execute<TResult>(string operationName, Func<TResult> func)
        {
            var stopwatch = ValueStopwatch.StartNew();
            try
            {
                _logger.LogInformation($"Operation started: {operationName}");
                var res = func();
                _logger.LogInformation($"Operation ended: {operationName}, {stopwatch.Elapsed.TotalMilliseconds:F1}ms, success");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Operation ended: {operationName}, {stopwatch.Elapsed.TotalMilliseconds:F1}ms, error: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(string operationName, Func<Task> action)
        {
            var stopwatch = ValueStopwatch.StartNew();
            try
            {
                _logger.LogInformation($"Operation started: {operationName}");
                await action();
                _logger.LogInformation($"Operation ended: {operationName}, {stopwatch.Elapsed.TotalMilliseconds:F1}ms, success");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Operation ended: {operationName}, {stopwatch.Elapsed.TotalMilliseconds:F1}ms, error: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TResult> ExecuteAsync<TResult>(string operationName, Func<Task<TResult>> func)
        {
            var stopwatch = ValueStopwatch.StartNew();
            try
            {
                _logger.LogInformation($"Operation started: {operationName}");
                var res = await func();
                _logger.LogInformation($"Operation ended: {operationName}, {stopwatch.Elapsed.TotalMilliseconds:F1}ms, success");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Operation ended: {operationName}, {stopwatch.Elapsed.TotalMilliseconds:F1}ms, error: {ex.Message}");
                throw;
            }
        }
    }
}
