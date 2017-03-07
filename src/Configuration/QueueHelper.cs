﻿using System;

namespace Busey.Configuration
{
    public static class QueueHelper
    {
        public static string ToQueue(this Type input)
        {
            return $"q.{input.FullName}";
        }

        public static string ToTopic(this Type input)
        {
            return $"t.{input.FullName}";
        }
    }
}