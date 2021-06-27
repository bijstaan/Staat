using Markdig;
using Markdig.Prism;

namespace Staat.Helpers
{
    public class MarkdownHelper
    {
        public static string ToHtml(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAutoLinks()
                .UsePipeTables()
                .UseMediaLinks()
                .UseEmphasisExtras()
                .UseEmojiAndSmiley()
                .UseAbbreviations()
                .UseTaskLists()
                .UseDiagrams()
                .UseAdvancedExtensions()
                .UsePrism()
                .Build();
            return Markdown.ToHtml(markdown, pipeline);
        }
    }
}