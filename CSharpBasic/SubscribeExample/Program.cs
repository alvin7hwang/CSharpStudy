namespace SubscribeExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Youtuber youtuber = new Youtuber();

            // 구독자 0 명
            Content content1 = new Content("사자의 하루");
            youtuber.UploadContent(content1);

            Subscriber subscriber1 = new Subscriber("Luke");
            youtuber.OnContentUploaded += subscriber1.ContentUploadedCallback;

            // 구독자 1명
            Content content2 = new Content("얼룩말의 일상");
            youtuber.UploadContent(content2);

            Subscriber subscriber2 = new Subscriber("Rin");
            youtuber.OnContentUploaded += subscriber2.ContentUploadedCallback;

            // 구독자 2명
            Content content3 = new Content("킹크랩 어선 탐험");
            youtuber.UploadContent(content3);
        }
    }
}
