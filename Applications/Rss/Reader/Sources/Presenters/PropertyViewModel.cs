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
using Cube.Mixin.Commands;
using Cube.Xui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Cube.Net.Rss.Reader
{
    /* --------------------------------------------------------------------- */
    ///
    /// PropertyViewModel
    ///
    /// <summary>
    /// RSS フィードのプロパティ画面とモデルを関連付けるためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PropertyViewModel : CommonViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PropertyViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="entry">RssEntry オブジェクト</param>
        /// <param name="callback">コールバック関数</param>
        ///
        /* ----------------------------------------------------------------- */
        public PropertyViewModel(RssEntry entry, Action<RssEntry> callback) :
            base(new Aggregator())
        {
            System.Diagnostics.Debug.Assert(entry != null);
            Entry = new BindableValue<RssEntry>(entry, entry.Dispatcher);
            _callback = callback;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Entry
        ///
        /// <summary>
        /// 対象となる RssEntry オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableValue<RssEntry> Entry { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Frequencies
        ///
        /// <summary>
        /// 更新頻度を表すオブジェクト一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<RssCheckFrequency> Frequencies { get; } =
            Enum.GetValues(typeof(RssCheckFrequency)).Cast<RssCheckFrequency>();

        #endregion

        #region Commands

        /* ----------------------------------------------------------------- */
        ///
        /// Apply
        ///
        /// <summary>
        /// 内容を適用するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Apply => Get(() => new DelegateCommand(() =>
        {
            Send<UpdateSourcesMessage>();
            Close.Execute();
            _callback?.Invoke(Entry.Value);
        }));

        #endregion

        #region Fields
        private readonly Action<RssEntry> _callback;
        #endregion
    }
}
