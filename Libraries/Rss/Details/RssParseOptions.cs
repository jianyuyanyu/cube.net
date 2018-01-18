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
namespace Cube.Net.Rss
{
    /* --------------------------------------------------------------------- */
    ///
    /// RssParseOptions
    ///
    /// <summary>
    /// RSS 解析時のオプションを定義するためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    internal class RssParseOptions
    {
        /* ----------------------------------------------------------------- */
        ///
        /// MaxSummaryLength
        /// 
        /// <summary>
        /// Summary の最大長を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static int MaxSummaryLength { get; set; } = 200;

        /* ----------------------------------------------------------------- */
        ///
        /// NsModContent
        /// 
        /// <summary>
        /// Content モジュールの名前空間を表す URL を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static string NsModContent { get; } = "http://purl.org/rss/1.0/modules/content/";

        /* ----------------------------------------------------------------- */
        ///
        /// NsDcElements
        /// 
        /// <summary>
        /// Document 要素用の名前空間を表す URL を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static string NsDcElements { get; } = "http://purl.org/dc/elements/1.1/";
    }
}
