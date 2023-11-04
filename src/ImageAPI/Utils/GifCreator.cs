using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageAPI.Utils;

public static class GifCreator
{
    public const int FrameDelay = 10;
    public const int StaticFrameDelay = 500;
    public const int SlideSteps = 20;

    public static Image ImagesToGif(Image[] images)
    {
        var gif = images[0].Clone(_ => { });

        var gifMetaData = gif.Metadata.GetGifMetadata();
        gifMetaData.RepeatCount = 0;

        SlideImages(gif, images);

        return gif;
    }

    private static void SlideImages(Image gif, IReadOnlyList<Image> images)
    {
        var stepSize = images[0].Width / SlideSteps;

        var rootFrame = images[0].Frames.RootFrame;
        rootFrame.Metadata.GetGifMetadata().FrameDelay = StaticFrameDelay;
        gif.Frames.AddFrame(rootFrame);

        for (var i = 0; i < images.Count; i++)
        {
            var currentImage = images[i];
            var nextImage = images[(i + 1) % images.Count];

            for (var step = 0; step < SlideSteps; step++)
            {
                var offset = step * stepSize;
                using var frameImage = new Image<Rgba32>(currentImage.Width, currentImage.Height);
                frameImage.Mutate(ctx =>
                {
                    if (-offset < frameImage.Width)
                    {
                        ctx.DrawImage(currentImage, new Point(-offset, 0), 1f);
                    }

                    if (currentImage.Width - offset < frameImage.Width)
                    {
                        ctx.DrawImage(nextImage, new Point(currentImage.Width - offset, 0), 1f);
                    }
                });

                frameImage.Frames[0].Metadata.GetGifMetadata().FrameDelay = FrameDelay;
                gif.Frames.AddFrame(frameImage.Frames[0]);
            }
        }
    }
}