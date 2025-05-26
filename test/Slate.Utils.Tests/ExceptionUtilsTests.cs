using System;
using FluentAssertions;
using Xunit;

namespace Slate.Utils.Tests
{
    public class ExceptionUtilsTests
    {
        [Fact]
        public void FlattenExceptions_IncludesInnerExceptions()
        {
            var inner = new InvalidOperationException("inner");
            var outer = new Exception("outer", inner);

            var result = ExceptionUtils.FlattenExceptions(outer);

            result.Should().ContainInOrder(outer, inner);
        }

        [Fact]
        public void Unwrap_ReturnsInnermostException()
        {
            var inner = new InvalidOperationException("inner");
            var outer = new Exception("outer", inner);

            var unwrapped = ExceptionUtils.Unwrap(outer);

            unwrapped.Should().Be(inner);
        }
    }
}
