using SixLabors.Fonts;
using MetaDataAPI.Utils;
using System.Globalization;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Utils;

public class ImageProcessor
{
    private readonly Font font;
    public readonly Image Image;

    public ImageProcessor(Image image, Font font)
    {
        Image = image;
        this.font = font;
    }

    public virtual void DrawText(Erc721Attribute attribute, TextOptions textOptions, IBrush? brush = null, IPen? pen = null)
    {
        string text;
        if (attribute.DisplayType == DisplayType.String)
        {
            text = attribute.Value.ToString()!;
        }
        else
        {
            text = attribute.DisplayType == DisplayType.Date ? TimeUtils.FromUnixTimestamp((long)attribute.Value).ToString(CultureInfo.InvariantCulture) : attribute.Value.ToString()!;
        }

        DrawText(text, textOptions, brush, pen);
    }

    public virtual void DrawText(string text, TextOptions textOptions, IBrush? brush = null, IPen? pen = null)
    {
        brush ??= Brushes.Solid(Color.Black);
        pen ??= Pens.Solid(Color.DarkRed, 2);

        Image.Mutate(x => x.DrawText(textOptions, text, brush, pen));

        Image.Save("result.jpg");
    }

    public virtual TextOptions CreateTextOptions(PointF coordinates)
    {
        return new TextOptions(font)
        {
            Origin = coordinates,
            TabWidth = 4,
            HorizontalAlignment = HorizontalAlignment.Left
        };
    }
}