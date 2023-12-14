﻿using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public abstract class ToDrawing
{
    public const float AmountFontSize = 24f;
    public const float DateTimeFontSize = 14f;
    public const float TextFontSize = 14f;
    public const float ProviderNameFontSize = 32f;

    public abstract Image Draw(Image drawOn);

    protected Image Draw(Image drawOn, string text, PointF coordinates, float fontSize, FontStyle fontStyle = FontStyle.Regular)
    {
        var font = LoadFont(fontSize, fontStyle);
        var imageProcessor = new ImageProcessor(drawOn, font);
        return imageProcessor.DrawText(text, coordinates);
    }

    protected static Font LoadFont(float fontSize, FontStyle fontStyle)
    {
        // TODO: Create class which provide caching fonts by fontSize. This class get exist font or create new and save it.
        return new ResourcesLoader().LoadFontFromEmbeddedResources(fontSize, fontStyle);
    }
}