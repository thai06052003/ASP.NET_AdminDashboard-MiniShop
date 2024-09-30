﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020687.BusinessLayers
{
    /// <summary>
    /// Cấu hình cho tầng Business
    /// </summary>
    public static class Configuration
    {
        public static string ConnectionString { get; private set; } = "";
        /// <summary>
        /// Khởi tạo cấu hình
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Intialize (string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
