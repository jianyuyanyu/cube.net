﻿/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cube.Mixin.Assembly;
using Cube.Net.Http;
using Cube.Tests;
using Microsoft.Win32;
using NUnit.Framework;

namespace Cube.Net.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// HttpMonitorTest
    ///
    /// <summary>
    /// Tests the HttpMonitor(T) class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class HttpMonitorTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Monitor
        ///
        /// <summary>
        /// Tests to communicate with the HTTP server periodically.
        /// </summary>
        ///
        /// <remarks>
        /// 過去、TCP コネクションが CLOSE_WAIT のまま残存し、3 回目以降の
        /// 通信に失敗すると言う不都合（.NET の初期設定では一つのエンド
        /// ポイントに対して同時に確立できるコネクションは 2 つ）が
        /// 確認されたので、その確認テストも兼ねます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Monitor()
        {
            using var mon = Create();
            var count = 0;
            var start = DateTime.Now;

            Assert.That(mon.UserAgent, Does.StartWith("Cube.Net.Tests"));

            mon.Interval = TimeSpan.FromMilliseconds(50);
            mon.Uri = new Uri("http://www.example.com/");

            var cts = new CancellationTokenSource();
            _ = mon.Subscribe((u, x) => throw new ArgumentException("Test"));
            _ = mon.Subscribe((u, x) =>
            {
                count++;
                if (count >= 3) cts.Cancel();
                return Task.FromResult(0);
            });

            mon.Start();
            mon.Start(); // ignore
            Assert.That(Wait.For(cts.Token), "Timeout");
            mon.Stop();
            mon.Stop(); // ignore

            Assert.That(mon.Last.Value, Is.GreaterThan(start));
            Assert.That(count, Is.AtLeast(3));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Monitor_NoSubscriptions
        ///
        /// <summary>
        /// Tests to monitor the HTTP server with no subscriptions.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Monitor_NoSubscriptions()
        {
            using var mon = Create();
            mon.Interval = TimeSpan.FromMilliseconds(50);
            mon.Uri = new Uri("http://www.example.com/");

            mon.Start();
            Task.Delay(100).Wait();
            mon.Stop();
            Assert.That(mon.Last.HasValue, Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Monitor_NotFound
        ///
        /// <summary>
        /// Tests to monitor the inexistent HTTP server.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Monitor_NotFound()
        {
            using var mon = Create();
            var count = 0;

            mon.Timeout = TimeSpan.FromMilliseconds(200);
            mon.Interval = TimeSpan.FromMilliseconds(50);
            mon.Uri = new Uri("http://www.cube-soft.jp/404.html");
            mon.RetryCount = 0;
            _ = mon.Subscribe((e, n) => { count++; return Task.FromResult(0); });

            mon.Start();
            Task.Delay(mon.Timeout).Wait();
            mon.Stop();
            Assert.That(count, Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Monitor_ConverterThrows
        ///
        /// <summary>
        /// Tests the behavior of the converter object when it throws an
        /// exception.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Monitor_ConverterThrows()
        {
            using var mon = new HttpMonitor<string>(e => throw new ArgumentException("Test"));
            var count = 0;

            mon.Timeout = TimeSpan.FromMilliseconds(200);
            mon.Interval = TimeSpan.FromMilliseconds(50);
            mon.Uri = new Uri("http://www.example.com/");
            _ = mon.Subscribe((e, n) => { count++; return Task.FromResult(0); });

            mon.Start();
            Task.Delay(mon.Timeout).Wait();
            mon.Stop();
            Assert.That(count, Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Monitor_PowerModeChanged
        ///
        /// <summary>
        /// Tests the PowerModeChanged event.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Monitor_PowerModeChanged()
        {
            var power = new PowerModeContext(PowerModes.Resume);
            Power.Configure(power);

            using var mon = Create();
            var count = 0;
            var cts = new CancellationTokenSource();

            mon.Interval = TimeSpan.FromMilliseconds(100);
            mon.Uri = new Uri("http://www.example.com/");
            _ = mon.Subscribe((e, n) => { count++; cts.Cancel(); return Task.FromResult(0); });

            mon.Start(mon.Interval);
            power.Mode = PowerModes.Suspend;
            Task.Delay(200).Wait();
            power.Mode = PowerModes.Resume;
            Assert.That(Wait.For(cts.Token), "Timeout");
            mon.Stop();
            Assert.That(count, Is.EqualTo(1));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Tests the Reset method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Reset()
        {
            using var mon = Create();
            var count = 0;
            var cts = new CancellationTokenSource();

            mon.Interval = TimeSpan.FromSeconds(1);
            mon.Uri = new Uri("http://www.example.com/");
            _ = mon.Subscribe((e, n) => { ++count; cts.Cancel(); return Task.FromResult(0); });

            mon.Reset();
            mon.Start(mon.Interval);
            mon.Reset();
            Assert.That(Wait.For(cts.Token), "Timeout");
            mon.Stop();
            Assert.That(count, Is.EqualTo(1));
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the HttpMonitor class.
        /// </summary>
        ///
        /// <remarks>
        /// テスト中で 3XX が返されると不都合な項目があるため
        /// HttpMonitor のテストでは EntityTag は無効に設定しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private HttpMonitor<int> Create() =>
            new(new ContentHandler<int>(s => s.ReadByte()) { UseEntityTag = false })
        {
            Interval  = TimeSpan.FromMinutes(1),
            Timeout   = TimeSpan.FromSeconds(2),
            UserAgent = $"Cube.Net.Tests/{Assembly.GetExecutingAssembly().GetVersion()}",
        };

        #endregion
    }
}
