using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Slate.Utils.Tests
{
    public class TaskUtilsTests
    {
        [Theory]
        [InlineData(0, 0, 0, true)]
        [InlineData(0, 0, 0, false)]
        [InlineData(1, 1, 0, true)]
        [InlineData(1, 1, 1, true)]
        [InlineData(1, 1, 64, true)]
        [InlineData(100000, 2, 64, true)]
        [InlineData(100, 10, 8, true)]
        [InlineData(100000, 2, 64, false)]

        public async Task TaskUtils_VerifyTaskResults(int max, int delay, int maxThreads, bool useTaskRun)
        {
            var tasks = new List<Func<Task<int>>>(max);

            for (var i = 0; i < max; i++)
            {
                var cur = i;
                tasks.Add(new Func<Task<int>>(() =>
                {
                    var taskCurrent = cur;
                    return Run(taskCurrent, delay);
                }));
            }

            var results = await TaskUtils.RunAsync(tasks,
                useTaskRun: useTaskRun,
                maxThreads: maxThreads,
                process: Process,
                token: CancellationToken.None);

            var expected = new int[max];

            results.Length.Should().Be(max);

            for (var i = 0; i < max; i++)
            {
                results[i].Should().Be(i * 2);
            }
        }

        private static async Task<int> Run(int x, int delay)
        {
            await Task.Delay(delay);
            return x;
        }

        private static async Task<int> Process(Task<int> x)
        {
            var y = (await x);
            return y * 2;
        }
    }
}
