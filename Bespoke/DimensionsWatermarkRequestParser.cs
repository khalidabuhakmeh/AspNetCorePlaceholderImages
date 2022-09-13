using System.Globalization;
using Microsoft.Extensions.Options;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Processors;

namespace Bespoke;

public class DimensionsWatermarkProcessor : IImageWebProcessor
{
    public const string Dimensions = "dim";

    private static readonly IEnumerable<string> DimensionsCommands
        = new[] { Dimensions };

    private readonly ImageSharpMiddlewareOptions options;

    public FormattedImage Process(FormattedImage image, ILogger logger, CommandCollection commands, CommandParser parser,
        CultureInfo culture)
    {
        var extension = commands.GetValueOrDefault(Dimensions);

        if (!string.IsNullOrEmpty(extension))
        {
            // https://www.adamrussell.com/adding-image-watermark-text-in-c-with-imagesh
            var text = $"{image.Image.Width}x{image.Image.Height}";

            // substitute your own font here if you like
            var font = SystemFonts.Families.First().CreateFont(18f, FontStyle.Regular);
            var textOptions = new TextOptions(font) { Dpi = 72, KerningMode = KerningMode.Normal };
            var rect = TextMeasurer.Measure(text, textOptions);

            // add watermark
            image.Image.Mutate(x => x.DrawText(
                $"{image.Image.Width} x {image.Image.Height}",
                font,
                new Color(Rgba32.ParseHex("#FFFFFF")),
                new PointF(image.Image.Width - rect.Width - 18f,
                    image.Image.Height - rect.Height - 5f)
            ));
        }

        return image;
    }

    public bool RequiresTrueColorPixelFormat(CommandCollection commands, CommandParser parser, CultureInfo culture)
        => false;

    public IEnumerable<string> Commands { get; } = DimensionsCommands;

    public DimensionsWatermarkProcessor(IOptions<ImageSharpMiddlewareOptions> options)
        => this.options = options.Value;
}