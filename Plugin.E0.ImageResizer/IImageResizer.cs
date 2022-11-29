namespace Plugin.E0.ImageResizer
{
    // All the code in this file is included in all platforms.
    public interface IImageResizer
    {
        byte[] ResizeHeight(float toHeight, byte[] Image);
        byte[] ResizeWidth(float toWidth, byte[] Image);
        byte[] Resize(float toHeight, float toWidth, byte[] Image);
    }
}