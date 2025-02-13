using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeTracker.Tests.Helpers
{
    public static class TestHelper
    {
        public static bool IsMatchingId(object param, int expectedId)
        {
            var idProperty = param.GetType().GetProperty("Id");
            if (idProperty == null) return false;

            var value = idProperty.GetValue(param);
            return value is int intValue && intValue == expectedId;
        }
    }
}
