namespace AgileEngine.ImageGallerySearch.Model
{
    public class Image
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Camera { get; set; }
        public string Tags { get; set; }
        public string CroppedPicture { get; set; }
        public string FullPicture { get; set; }
    }
}
