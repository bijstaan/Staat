/*
 * Staat - Staat
 * Copyright (C) 2021 Bijstaan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Markdig;
using Markdig.Prism;

namespace Staat.Helpers
{
    public class MarkdownHelper
    {
        public static string ToHtml(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                    .UseAbbreviations()
                    .UseAutoLinks()
                    .UseCustomContainers()
                    .UseDefinitionLists()
                    .UseDiagrams()
                    .UseEmphasisExtras()
                    .UseEmojiAndSmiley()
                    .UseFootnotes()
                    .UseGridTables()
                    .UseListExtras()
                    .UseMathematics()
                    .UseMediaLinks()
                    .UsePipeTables()
                    .UsePrism()
                    .UseReferralLinks("nofollow", "noreferrer")
                    .UseTaskLists()
                    .UseGenericAttributes() // Must be last
                    .Build();
            return Markdown.ToHtml(markdown, pipeline);
        }
    }
}