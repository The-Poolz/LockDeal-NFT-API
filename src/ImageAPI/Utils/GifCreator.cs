using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageAPI.Utils;

public static class GifCreator
{
    public const int FrameDelay = 10;

    public static Image ImagesToGif(IReadOnlyList<Image> images)
    {
        var gif = images[0].Clone(_ => { });

        var gifMetaData = gif.Metadata.GetGifMetadata();
        gifMetaData.RepeatCount = 0;

        SlideImages(gif, images);

        return gif;
    }

    private static void SlideImages(Image gif, IReadOnlyList<Image> images)
    {
        const int slideSteps = 20;
        var stepSize = images[0].Width / slideSteps;

        const int staticFrameDelay = 500;

        for (var i = 0; i < images.Count; i++)
        {
            var currentImage = images[i];
            var nextImage = images[(i + 1) % images.Count];

            using (var staticFrame = new Image<Rgba32>(currentImage.Width, currentImage.Height))
            {
                staticFrame.Mutate(ctx => ctx.DrawImage(currentImage, new Point(0, 0), 1f));
                var staticMetadata = staticFrame.Frames.RootFrame.Metadata.GetGifMetadata();
                staticMetadata.FrameDelay = staticFrameDelay;
                gif.Frames.AddFrame(staticFrame.Frames.RootFrame);
            }

            for (var step = 0; step < slideSteps; step++)
            {
                var offset = step * stepSize;
                using var frame = new Image<Rgba32>(currentImage.Width, currentImage.Height);

                if (-offset < frame.Width && currentImage.Width - offset > 0)
                {
                    frame.Mutate(ctx => ctx.DrawImage(currentImage, new Point(-offset, 0), 1f));
                }

                if (currentImage.Width - offset < frame.Width && offset > 0)
                {
                    frame.Mutate(ctx => ctx.DrawImage(nextImage, new Point(currentImage.Width - offset, 0), 1f));
                }

                var metadata = frame.Frames.RootFrame.Metadata.GetGifMetadata();
                metadata.FrameDelay = FrameDelay;
                gif.Frames.AddFrame(frame.Frames.RootFrame);
            }
        }
    }
}