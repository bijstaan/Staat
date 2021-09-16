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