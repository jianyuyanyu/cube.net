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
using Cube.Net.Rss;
using Cube.Xui;

namespace Cube.Net.App.Rss.Reader
{
    /* --------------------------------------------------------------------- */
    ///
    /// RssBindableData
    ///
    /// <summary>
    /// メイン画面にバインドされるデータ群を定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class RssBindableData
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// RssBindableData
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="root">ルートオブジェクト</param>
        /// <param name="settings">設定用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public RssBindableData(IEnumerable<IRssEntry> root, Settings settings)
        {
            Root = root;
            User = new Bindable<Settings>(settings);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Root
        ///
        /// <summary>
        /// 登録されている RssEntry のルートにあたるオブジェクトを
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<IRssEntry> Root { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// ユーザ設定を保持するオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<Settings> User { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Current
        ///
        /// <summary>
        /// 選択中のカテゴリまたは RSS エントリを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<IRssEntry> Current { get; } = new Bindable<IRssEntry>();

        /* ----------------------------------------------------------------- */
        ///
        /// LastEntry
        ///
        /// <summary>
        /// 最後に選択した RSS エントリを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// RSS エントリを選択中の場合、Current と LastEntry は同じ値に
        /// なります。カテゴリを選択中の場合、Current は該当カテゴリの
        /// 値を LastEntry は直前まで選択されていた RssEntry の値を保持
        /// します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<RssEntry> LastEntry { get; } = new Bindable<RssEntry>();

        /* ----------------------------------------------------------------- */
        ///
        /// Content
        ///
        /// <summary>
        /// Web ブラウザに表示する内容を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<object> Content { get; } = new Bindable<object>();

        /* ----------------------------------------------------------------- */
        ///
        /// Message
        ///
        /// <summary>
        /// メッセージを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<string> Message { get; } = new Bindable<string>();

        #endregion
    }
}
