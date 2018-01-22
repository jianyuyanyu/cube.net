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
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using Cube.Net.Rss;
using Cube.Xui;

namespace Cube.Net.App.Rss.Reader
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
        /// <param name="feed">RssFeed オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public PropertyViewModel(RssEntry entry, RssFeed feed) :
            base(new Messenger())
        {
            System.Diagnostics.Debug.Assert(entry != null && feed != null);
            Entry = new Bindable<RssEntry>(entry);
            Feed  = new Bindable<RssFeed>(feed);
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
        public Bindable<RssEntry> Entry { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Feed
        /// 
        /// <summary>
        /// 対象となる RssFeed オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<RssFeed> Feed { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Frequencies
        /// 
        /// <summary>
        /// 更新頻度を表すオブジェクト一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Frequency> Frequencies { get; } =
            Enum.GetValues(typeof(Frequency)).Cast<Frequency>();

        #endregion
    }
}
