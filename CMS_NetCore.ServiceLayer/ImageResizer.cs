using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CMS_NetCore.ServiceLayer;

public class ImageResizer
{
    /// <summary>
    /// http://www.blackbeltcoder.com/Articles/graph/programmatically-resizing-an-image
    /// Maximum width of resized image.
    /// </summary>
    private int MaxX { get; set; }

    /// <summary>
    /// Maximum height of resized image.
    /// </summary>
    private int MaxY { get; set; }

    /// <summary>
    /// If true, resized image is trimmed to exactly fit
    /// maximum width and height dimensions.
    /// </summary>
    private bool TrimImage { get; set; }

    /// <summary>
    /// Format used to save resized image.
    /// </summary>
    private ImageFormat SaveFormat { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public ImageResizer(int size)
    {
        MaxX = MaxY = size;
        TrimImage = false;
        SaveFormat = ImageFormat.Jpeg;
    }

    /// <summary>
    /// Resizes the image from the source file according to the
    /// current settings and saves the result to the targe file.
    /// </summary>
    /// <param name="source">Path containing image to resize</param>
    /// <param name="target">Path to save resized image</param>
    /// <returns>True if successful, false otherwise.</returns>
    public bool Resize(
        string source,
        string target
    )
    {
        try
        {
            using (var fromFile = Image.FromFile(
                       source,
                       true
                   ))
            {
                // Check that we have an image
                if (fromFile != null)
                {
                    int origX, origY, newX, newY;
                    int trimX = 0, trimY = 0;

                    // Default to size of source image
                    newX = origX = fromFile.Width;
                    newY = origY = fromFile.Height;

                    // Does image exceed maximum dimensions?
                    if (origX > MaxX || origY > MaxY)
                    {
                        // Need to resize image
                        if (TrimImage)
                        {
                            // Trim to exactly fit maximum dimensions
                            var factor = Math.Max(
                                MaxX / (double)origX,
                                MaxY / (double)origY
                            );
                            newX = (int)Math.Ceiling((double)origX * factor);
                            newY = (int)Math.Ceiling((double)origY * factor);
                            trimX = newX - MaxX;
                            trimY = newY - MaxY;
                        }
                        else
                        {
                            // Resize (no trim) to keep within maximum dimensions
                            var factor = Math.Min(
                                MaxX / (double)origX,
                                MaxY / (double)origY
                            );
                            newX = (int)Math.Ceiling((double)origX * factor);
                            newY = (int)Math.Ceiling((double)origY * factor);
                        }
                    }

                    // Create destination image
                    using (Image dest = new Bitmap(
                               newX - trimX,
                               newY - trimY
                           ))
                    {
                        Graphics graph = Graphics.FromImage(dest);
                        graph.InterpolationMode =
                            InterpolationMode.HighQualityBicubic;
                        graph.DrawImage(
                            fromFile,
                            -(trimX / 2),
                            -(trimY / 2),
                            newX,
                            newY
                        );
                        dest.Save(
                            target,
                            SaveFormat
                        );
                        // Indicate success
                        return true;
                    }
                }
            }
        }
        catch
        {
            return false;
        }

        // Indicate failure
        return false;
    }
}